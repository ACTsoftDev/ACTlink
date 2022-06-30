using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;
using actchargers.Code.Utility;
using System.Threading.Tasks;

namespace actchargers
{
    public class ReplacementViewModel : BaseViewModel
    {
        DeviceInfoItem replacementItem;
        ObservableCollection<SynchObjectsBufferedData> _MainList;
        public ObservableCollection<SynchObjectsBufferedData> MainList
        {
            get { return _MainList; }
            set
            {
                _MainList = value;
                RaisePropertyChanged(() => MainList);
            }
        }

        ObservableCollection<SynchObjectsBufferedData> _ListItemSource;
        public ObservableCollection<SynchObjectsBufferedData> ListItemSource
        {
            get { return _ListItemSource; }
            set
            {
                _ListItemSource = value;
                RaisePropertyChanged(() => ListItemSource);
            }
        }

        string _PlaceHolderText;
        public string PlaceHolderText
        {
            get { return _PlaceHolderText; }
            set
            {
                _PlaceHolderText = value;
                RaisePropertyChanged(() => PlaceHolderText);
            }
        }

        string _SearchText;
        public string SearchText
        {
            get { return _SearchText; }
            set
            {
                _SearchText = value;
                if (value != null)
                {
                    SearchClick(value);
                }
                RaisePropertyChanged(() => SearchText);
            }
        }

        public void Init(string itemStr)
        {
            if (!string.IsNullOrEmpty(itemStr))
            {
                replacementItem = JsonParser.DeserializeObject<DeviceInfoItem>(itemStr);

                List<SynchObjectsBufferedData> list;
                var synchObjectsBufferedDataLoader = DbSingleton
                    .DBManagerServiceInstance.GetSynchObjectsBufferedDataLoader();

                var sites = DbSingleton
                    .DBManagerServiceInstance
                    .GetSynchSiteObjectsLoader()
                    .GetAll();

                if (replacementItem != null)
                {
                    if (replacementItem.IsBATTReplacement)
                    {
                        list = synchObjectsBufferedDataLoader.GetBattviews(0);
                    }
                    else
                    {
                        list = synchObjectsBufferedDataLoader.GetChargers(0);
                    }
                }
                else
                {
                    list = new List<SynchObjectsBufferedData>();
                }

                MainList = new ObservableCollection<SynchObjectsBufferedData>
                    (list);

                foreach (var item in MainList)
                {
                    item.DeviceFullName = item.DeviceName + " (" + item.SerialNumber + ")";
                }

                ListItemSource = new ObservableCollection<SynchObjectsBufferedData>(MainList);

                RaisePropertyChanged(() => MainList);
                RaisePropertyChanged(() => ListItemSource);
            }
        }

        public ReplacementViewModel()
        {
            ViewTitle = AppResources.select_replacement;
            PlaceHolderText = AppResources.search_bar_placeholder;

            ListItemSource = new ObservableCollection<SynchObjectsBufferedData>();
            MainList = new ObservableCollection<SynchObjectsBufferedData>();

        }

        private MvxCommand<SynchObjectsBufferedData> m_SelectItemCommand;

        public ICommand SelectItemCommand
        {
            get
            {
                return m_SelectItemCommand ?? (m_SelectItemCommand = new MvxCommand<SynchObjectsBufferedData>((obj) => this.ExecuteSelectItemCommand(obj)));
            }
        }

        private void ExecuteSelectItemCommand(SynchObjectsBufferedData item)
        {
            string message = AppResources.you_have_selected + (replacementItem.IsBATTReplacement ? "Battery" : "Charger") + "\n" + item.DeviceFullName + "\n" + AppResources.do_you_want_to_process;

            ACUserDialogs.ShowAlertWithTwoButtons(message, AppResources.confirm_replacement, AppResources.yes, AppResources.no, () => OnConfirmClick(item), null);

        }

        void OnConfirmClick(SynchObjectsBufferedData item)
        {
            ACUserDialogs.ShowAlertWithTwoButtons(AppResources.operation_cant_undone, "", AppResources._continue, AppResources.cancel, () => OnContinueClick(item), null);
        }

        public void SearchClick(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                ListItemSource = new ObservableCollection<SynchObjectsBufferedData>(MainList.Where(o => o.SerialNumber.Contains(text)));
            }
            else
            {
                ListItemSource = new ObservableCollection<SynchObjectsBufferedData>(MainList);
            }

            RaisePropertyChanged(() => ListItemSource);
        }

        async void OnContinueClick(SynchObjectsBufferedData item)
        {
            UInt32 id = item.Id;
            List<object> arguments = new List<object>();
            List<object> results = new List<object>();
            ACUserDialogs.ShowProgress();
            try
            {
                if (replacementItem.IsBATTReplacement)
                {
                    arguments.Add(BattViewCommunicationTypes.replaceDevice);
                    arguments.Add(id);
                    results = await BattViewQuantum.Instance.CommunicateBATTView(arguments);

                    if (results.Count > 0)
                    {
                        var status = (BattViewCommunicationTypes)results[3];
                        if (status == BattViewCommunicationTypes.restartDevice)
                        {
                            Mvx.Resolve<IMvxMessenger>().Publish(new DeviceResetMessage(this, BattViewQuantum.Instance.GetBATTView().IPAddress));
                            ACUserDialogs.HideProgress();
                            ShowViewModel<ReplacementViewModel>(new { pop = "pop" });

                        }
                    }
                    else
                    {
                        ACUserDialogs.ShowAlertWithTitleAndOkButton(AppResources.save_failed);
                    }

                }
                else
                {
                    arguments.Add(McbCommunicationTypes.replaceDevice);
                    arguments.Add(id);
                    results = await MCBQuantum.Instance.CommunicateMCB(arguments);
                    //replacePart_mcbDevices = new List<ACTViewDeviceObject>();
                    if (results.Count > 0)
                    {
                        var status = (McbCommunicationTypes)results[3];
                        if (status == McbCommunicationTypes.restartDevice)
                        {
                            Mvx.Resolve<IMvxMessenger>().Publish(new DeviceResetMessage(this, MCBQuantum.Instance.GetMCB().IPAddress));
                            ACUserDialogs.HideProgress();
                            ShowViewModel<ReplacementViewModel>(new { pop = "pop" });

                        }
                    }
                    else
                    {
                        ACUserDialogs.ShowAlertWithTitleAndOkButton(AppResources.save_failed);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            ACUserDialogs.HideProgress();
        }

        #region Update Firmware

        public string UpdateFirmwareTitle
        {
            get { return AppResources.update_firmware; }
        }

        public IMvxCommand UpdateFirmwareCommand
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

            if (replacementItem.IsBATTReplacement)
                result = await UpdateBattView();
            else
                result = await UpdateMcb();

            string message = result.Item2;

            if (!string.IsNullOrEmpty(message))
            {
                ACUserDialogs.HideProgressWithCancel();

                ACUserDialogs.ShowAlert(message);
            }
        }

        async Task<Tuple<bool, string>> UpdateBattView()
        {
            var device = BattViewQuantum.Instance.GetBATTView();

            Firmware.DoesMcbRequireUpdate(device);

            var result = await device.SiteViewUpdate();

            return result;
        }

        async Task<Tuple<bool, string>> UpdateMcb()
        {
            var device = MCBQuantum.Instance.GetMCB();

            Firmware.DoesMcbRequireUpdate(device);

            var result = await device.SiteViewUpdate();

            return result;
        }

        #endregion

        public override void OnBackButtonClick()
        {
            base.OnBackButtonClick();

            Mvx.Resolve<IMvxMessenger>().Publish(new BackScanMessage(this));

            ShowViewModel<ReplacementViewModel>(new { pop = "pop" });
        }
    }
}
