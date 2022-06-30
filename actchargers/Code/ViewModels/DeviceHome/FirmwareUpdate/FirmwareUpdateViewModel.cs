using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;
using Plugin.DeviceInfo;
using Plugin.FilePicker;
using Plugin.FilePicker.Abstractions;

namespace actchargers
{
    public class FirmwareUpdateViewModel : BaseViewModel
    {
        ListViewItem LoadFirmwareFileButton;
        ListViewItem UpdateFirmwareButton;
        ListViewItem DevRequestButton;
        ListViewItem UpdateWiFiFirmwareButton;

        public FirmwareUpdateViewModel()
        {
            DevicePlatform = CrossDeviceInfo.Current.Platform;
            ItemSource = new ObservableCollection<ListViewItem>();
            ViewTitle = AppResources.firmware_update;
            CreateList();
        }

        ObservableCollection<ListViewItem> itemSource;
        public ObservableCollection<ListViewItem> ItemSource
        {
            get { return itemSource; }
            set
            {
                SetProperty(ref itemSource, value);
            }
        }

        public void CreateList()
        {
            ItemSource.Clear();

            DevRequestButton = new ListViewItem
            {
                Title = AppResources.request_now,
                DefaultCellType = ACUtility.CellTypes.Button,
                ListSelectionCommand = ButtonSelectorCommand

            };

            if (DevicePlatform == Plugin.DeviceInfo.Abstractions.Platform.Android)
            {
                LoadFirmwareFileButton = new ListViewItem
                {
                    Title = AppResources.load_hex_file,
                    DefaultCellType = ACUtility.CellTypes.Button,
                    ListSelectionCommand = ButtonSelectorCommand
                };
            }

            UpdateFirmwareButton = new ListViewItem
            {
                Title = AppResources.update_firmware,
                DefaultCellType = ACUtility.CellTypes.Button,
                ListSelectionCommand = UpdateFirmwareCommand
            };

            if (IsEsp32Enabled())
            {
                UpdateWiFiFirmwareButton = new ListViewItem
                {
                    Title = AppResources.update_wifi_firmware,
                    DefaultCellType = ACUtility.CellTypes.Button,
                    ListSelectionCommand = UpdateWiFiFirmwareCommand
                };
            }

            if (IsBattView)
            {
                if (BattViewFirmwareAccessApply() == 0)
                {
                    ItemSource.Clear();
                    ACUserDialogs.ShowAlertWithOkButtonsAction(AppResources.no_data_found, () => { ShowViewModel<FirmwareUpdateViewModel>(new { pop = "pop" }); });
                    return;
                }
            }
            else
            {
                if (ChargerFirmwareAccessApply() == 0)
                {
                    ItemSource.Clear();
                    ACUserDialogs.ShowAlertWithOkButtonsAction(AppResources.no_data_found, () => { ShowViewModel<FirmwareUpdateViewModel>(new { pop = "pop" }); });
                    return;
                }
            }
        }

        int ChargerFirmwareAccessApply()
        {
            UIAccessControlUtility accessControlUtility = new UIAccessControlUtility();

            if (ControlObject.UserAccess.MCB_FirmwareUpdate == AccessLevelConsts.noAccess)
                return 0;

            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_FirmwareRequestUpdateDebug, DevRequestButton, ItemSource);
            if (DevicePlatform == Plugin.DeviceInfo.Abstractions.Platform.Android)
            {
                if (ManagementUsers.IsManagementUser())
                {
                    accessControlUtility
                        .DoApplyAccessControl
                        (ControlObject.UserAccess.MCB_manualFirmwareUpdate,
                         LoadFirmwareFileButton, ItemSource);
                }
            }

            ItemSource.Add(UpdateFirmwareButton);

            if (IsEsp32Enabled())
                ItemSource.Add(UpdateWiFiFirmwareButton);

            return 1;
        }
        int BattViewFirmwareAccessApply()
        {
            UIAccessControlUtility accessControlUtility = new UIAccessControlUtility();

            if (ControlObject.UserAccess.Batt_FirmwareUpdate == AccessLevelConsts.noAccess)
                return 0;

            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_FirmwareRequestUpdateDebug, DevRequestButton, ItemSource);
            if (DevicePlatform == Plugin.DeviceInfo.Abstractions.Platform.Android)
            {
                if (ManagementUsers.IsManagementUser())
                {
                    accessControlUtility
                            .DoApplyAccessControl
                            (ControlObject.UserAccess.Batt_manualFirmwareUpdate,
                             LoadFirmwareFileButton, ItemSource);
                }
            }

            ItemSource.Add(UpdateFirmwareButton);

            if (IsEsp32Enabled())
                ItemSource.Add(UpdateWiFiFirmwareButton);

            return 1;
        }

        bool IsEsp32Enabled()
        {
            bool isEsp32WiFi;
            bool isEnginneringTeam;

            if (IsBattView)
            {
                isEsp32WiFi = BattViewQuantum.Instance.GetBATTView().IsEsp32WiFi;
                isEnginneringTeam = ControlObject.UserAccess.Batt_onlyForEnginneringTeam != AccessLevelConsts.noAccess;
            }
            else
            {
                isEsp32WiFi = MCBQuantum.Instance.GetMCB().IsEsp32WiFi;
                isEnginneringTeam = ControlObject.UserAccess.MCB_onlyForEnginneringTeam != AccessLevelConsts.noAccess;
            }

            return isEsp32WiFi && isEnginneringTeam;
        }

        IMvxCommand UpdateFirmwareCommand
        {
            get
            {
                return new MvxAsyncCommand(ExecuteUpdateFirmwareCommand);
            }
        }

        async Task ExecuteUpdateFirmwareCommand()
        {
            ACUserDialogs.ShowProgressWithCancel(this);

            try
            {
                await TryExecuteUpdateFirmwareCommand();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }

            ACUserDialogs.HideProgressWithCancel();
        }

        async Task TryExecuteUpdateFirmwareCommand()
        {
            Tuple<bool, string> result;

            if (IsBattView)
                result = await BattViewQuantum.Instance.GetBATTView().SiteViewUpdate();
            else
                result = await MCBQuantum.Instance.GetMCB().SiteViewUpdate();

            string message = result.Item2;

            if (!string.IsNullOrEmpty(message))
            {
                ACUserDialogs.HideProgressWithCancel();

                ACUserDialogs.ShowAlert(message);
            }
        }

        IMvxCommand ButtonSelectorCommand
        {
            get
            {
                return new MvxCommand<ListViewItem>(async (ListViewItem obj) =>
                {
                    await ExecuteButtonSelectorCommand(obj);
                });
            }
        }

        async Task ExecuteButtonSelectorCommand(ListViewItem item)
        {
            if (IsBattView)
            {
                await BATT_simpleCommunicationButtonActionInternal(item);
            }
            else
            {
                await MCB_simpleCommunicationButtonActionInternal(item);
            }

        }

        async Task BATT_simpleCommunicationButtonActionInternal(ListViewItem item)
        {
            ACUserDialogs.ShowProgressWithCancel(this);

            await BATTCommunicationButtonAction(item);

            ACUserDialogs.HideProgressWithCancel();
        }

        async Task BATTCommunicationButtonAction(ListViewItem item)
        {
            BattViewCommunicationTypes caller = BattViewCommunicationTypes.NOCall;
            object arg1 = null;
            string msg = "";
            if (!BattViewQuantum.Instance.BattView_quickSerialNumberCheckStrict())
                return;
            try
            {
                if (item.Title == AppResources.load_hex_file)
                {
                    string hexFile = null;
                    FileData filedata = await CrossFilePicker.Current.PickFile();

                    if (filedata != null)
                    {
                        hexFile = filedata.FilePath;
                    }
                    if (hexFile.Length == 0)
                    {
                        return;//no file selected
                    }
                    if (!hexFile.Contains(".hex"))
                    {
                        ACUserDialogs.ShowAlertWithTitleAndOkButton("Please select hex file");
                        return;
                    }
                    //IsBusy = true;
                    byte[] serials = null;
                    Firmware firmwareManager = new Firmware();
                    FirmwareResult res = firmwareManager.GetHexFileBinary(DeviceBaseType.BATTVIEW, hexFile, ref serials, filedata.DataArray);
                    //IsBusy = false;
                    if (res != FirmwareResult.fileOK)
                    {
                        switch (res)
                        {
                            case FirmwareResult.badFileEncode:
                                msg = "Bad file encoding"; break;
                            case FirmwareResult.badFileFormat:
                                msg = "Bad file format"; break;
                            case FirmwareResult.fileNotFound:
                                msg = "File not found"; break;
                            case FirmwareResult.noAcessFile:
                                msg = "Can't read file"; break;
                        }
                    }
                    else
                    {
                        caller = BattViewCommunicationTypes.firmwareWrite;
                        arg1 = serials;
                    }
                }

                else if (item.Title == DevRequestButton.Title)
                {
                    caller = BattViewCommunicationTypes.firmwareUpdateRequest;
                }

                if (caller != BattViewCommunicationTypes.NOCall)
                {
                    List<object> arguments = new List<object>
                    {
                        caller,
                        arg1
                    };
                    List<object> results = new List<object>();
                    try
                    {
                        results = await BattViewQuantum.Instance.CommunicateBATTView(arguments);
                        if (results.Count > 0)
                        {
                            var callerStatus = (BattViewCommunicationTypes)results[3];
                            var status = (CommunicationResult)results[2];
                            if (callerStatus == BattViewCommunicationTypes.firmwareUpdateRequest)
                            {
                                if (status == CommunicationResult.COMMAND_DELAYED)
                                {

                                    msg = "BATTView is busy, the firmware update will take place automatically.";
                                }
                                else if (status == CommunicationResult.OK)
                                {
                                    //scanRelated_prepare(scanRelatedTypes.doScan);
                                    Mvx.Resolve<IMvxMessenger>().Publish(new DeviceResetMessage(this, BattViewQuantum.Instance.GetBATTView().IPAddress));
                                    ShowViewModel<ConnectToDeviceViewModel>(new { popto = "popto" });
                                    msg = "BATTView is reflushing Firmware..it may take few minutes to update the firmware";
                                }
                                else
                                {

                                    msg = "Something went wrong, please retry";
                                }

                            }
                            else if (callerStatus == BattViewCommunicationTypes.firmwareWrite)
                            {
                                msg = AppResources.cant_update_firmware;
                            }

                        }
                        else
                        {
                            ShowAlert(AppResources.opration_failed);
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.AddLog(true, "X86" + ex);

                        return;
                    }
                }

                if (msg != string.Empty)
                    ShowAlert(msg);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        async Task MCB_simpleCommunicationButtonActionInternal(ListViewItem item)
        {
            ACUserDialogs.ShowProgressWithCancel(this);

            await MCBCommunicationButtonAction(item);

            ACUserDialogs.HideProgressWithCancel();
        }

        async Task MCBCommunicationButtonAction(ListViewItem item)
        {
            McbCommunicationTypes caller = McbCommunicationTypes.NOCall;
            object arg1 = null;
            string msg = "";
            if (!MCBQuantum.Instance.MCB_quickSerialNumberCheckStrict())
                return;
            try
            {
                if (item.Title == AppResources.load_hex_file)
                {
                    string hexFile = null;

                    FileData filedata = await CrossFilePicker.Current.PickFile();

                    if (filedata != null)
                    {
                        hexFile = filedata.FilePath;

                    }
                    if (hexFile.Length == 0)
                    {
                        return;//no file selected
                    }
                    if (!hexFile.Contains(".hex"))
                    {
                        ACUserDialogs.ShowAlertWithTitleAndOkButton("Please select hex file");
                        return;
                    }
                    //IsBusy = true;
                    byte[] serials = null;
                    Firmware firmwareManager = new Firmware();

                    FirmwareResult res = firmwareManager.GetHexFileBinary(DeviceBaseType.MCB, hexFile, ref serials, filedata.DataArray);
                    //IsBusy = false;
                    if (res != FirmwareResult.fileOK)
                    {
                        switch (res)
                        {
                            case FirmwareResult.badFileEncode:
                                msg = "Bad file encoding"; break;
                            case FirmwareResult.badFileFormat:
                                msg = "Bad file format"; break;
                            case FirmwareResult.fileNotFound:
                                msg = "File not found"; break;
                            case FirmwareResult.noAcessFile:
                                msg = "Can't read file"; break;
                        }
                    }
                    else
                    {
                        caller = McbCommunicationTypes.firmwareWrite;
                        arg1 = serials;
                    }
                }

                else if (item.Title == DevRequestButton.Title)
                {
                    caller = McbCommunicationTypes.firmwareUpdateRequest;
                }

                if (caller != McbCommunicationTypes.NOCall)
                {

                    List<object> arguments = new List<object>
                    {
                        caller,
                        arg1
                    };
                    List<object> results = new List<object>();
                    try
                    {
                        results = await MCBQuantum.Instance.CommunicateMCB(arguments);
                        if (results.Count > 0)
                        {
                            var callerStatus = (McbCommunicationTypes)results[3];
                            var status = (CommunicationResult)results[2];
                            if (callerStatus == McbCommunicationTypes.firmwareUpdateRequest)
                            {
                                if (status == CommunicationResult.COMMAND_DELAYED)
                                {
                                    msg = "Charger is busy, the firmware update will take place automatically once  the charge cycle is done.";
                                }
                                else if (status == CommunicationResult.OK || MCBQuantum.Instance.GetMCB().FirmwareRevision < 2.05f)
                                {
                                    Mvx.Resolve<IMvxMessenger>().Publish(new DeviceResetMessage(this, MCBQuantum.Instance.GetMCB().IPAddress));
                                    ShowViewModel<ConnectToDeviceViewModel>(new { popto = "popto" });
                                    msg = "charger is reflushing Firmware..it may take few minutes to update the firmware";
                                    //scanRelated_prepare(scanRelatedTypes.doScan);
                                }

                            }
                            else if (callerStatus == McbCommunicationTypes.firmwareWrite)
                            {
                                msg = AppResources.cant_update_firmware;
                            }
                        }
                        else
                        {
                            ShowAlert(AppResources.opration_failed);
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.AddLog(true, "X86" + ex);
                        return;
                    }
                }

                if (msg != string.Empty)
                    ShowAlert(msg);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        IMvxCommand UpdateWiFiFirmwareCommand
        {
            get
            {
                return new MvxAsyncCommand(ExecuteUpdateWiFiFirmwareCommand);
            }
        }

        async Task ExecuteUpdateWiFiFirmwareCommand()
        {
            DeviceObjectParent device = null;

            if (IsBattView)
                device = BattViewQuantum.Instance.GetBATTView();
            else
                device = MCBQuantum.Instance.GetMCB();

            await UpdateDeviceWiFiFirmware(device);
        }

        async Task UpdateDeviceWiFiFirmware(DeviceObjectParent device)
        {
            ACUserDialogs.ShowProgressWithCancel(this);

            try
            {
                var result = await TryUpdateDeviceWiFiFirmware(device);

                if (!string.IsNullOrEmpty(result.Item2))
                    ShowAlert(result.Item2);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }

            ACUserDialogs.HideProgressWithCancel();
        }

        async Task<Tuple<bool, string>> TryUpdateDeviceWiFiFirmware(DeviceObjectParent device)
        {
            return await device.UpdateWiFiFirmware(false, false);
        }

        void ShowAlert(string message)
        {
            InvokeOnMainThread(() =>
            {
                ACUserDialogs.ShowAlertWithTitleAndOkButton(message);
            });
        }

        public override void Cancel()
        {
            base.Cancel();

            if (MCBQuantum.Instance != null)
                MCBQuantum.Instance.Cancel();

            if (BattViewQuantum.Instance != null)
                BattViewQuantum.Instance.Cancel();
        }

        public override void OnBackButtonClick()
        {
            base.OnBackButtonClick();

            ShowViewModel<FirmwareUpdateViewModel>(new { pop = "pop" });
        }
    }
}
