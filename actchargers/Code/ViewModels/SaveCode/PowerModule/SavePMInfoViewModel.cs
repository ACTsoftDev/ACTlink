using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;
using Newtonsoft.Json;

namespace actchargers
{
    public class SavePMInfoViewModel : BaseViewModel
    {
        private List<string> device_LoadWarnings;

        VerifyControl verifyControl;

        MvxSubscriptionToken _mListSelector;
        /// <summary>
        /// The settings item source.
        /// </summary>
        private ObservableCollection<ListViewItem> _infoItemSource;

        public ObservableCollection<ListViewItem> PMInfoItemSource
        {
            get { return _infoItemSource; }
            set
            {
                _infoItemSource = value;
                RaisePropertyChanged(() => PMInfoItemSource);
            }
        }

        /// <summary>
        /// The edit mode.
        /// </summary>
        private bool _showEdit;
        public bool ShowEdit
        {
            get
            {
                return _showEdit;
            }
            set
            {
                _showEdit = value;
                RaisePropertyChanged(() => ShowEdit);
            }
        }

        /// <summary>
        /// The edit mode.
        /// </summary>
        private bool _editMode;
        public bool EditingMode
        {
            get
            {
                return _editMode = _editMode ? _editMode : false;
            }
            set
            {
                _editMode = value;
                RaisePropertyChanged(() => EditingMode);
            }
        }

        ListViewItem MCB_NumberOfInstalledPMsTextBox;
        ListViewItem MCB_PMVoltageComboBox;
        ListViewItem MCB_PMeffiencieyTextBox;
        ListViewItem MCB_PMInputVoltageComboBox;

        public SavePMInfoViewModel()
        {
            device_LoadWarnings = new List<string>();
            ViewTitle = AppResources.pm_info;
            ShowEdit = true;
            verifyControl = new VerifyControl();

            PMInfoItemSource = new ObservableCollection<ListViewItem>();
            MCB_NumberOfInstalledPMsTextBox = new ListViewItem()
            {
                Index = 0,
                Title = AppResources.number_of_installed_power_modules,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.LabelTextEdit,
                TextInputType = ACUtility.InputType.Number
            };
            MCB_PMVoltageComboBox = new ListViewItem()
            {
                Index = 1,
                Title = AppResources.rated_voltage,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.ListSelector,
                ListSelectionCommand = ListSelectorCommand
            };
            MCB_PMeffiencieyTextBox = new ListViewItem()
            {
                Index = 2,
                Title = AppResources.efficiency,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.LabelTextEdit,
                TextInputType = ACUtility.InputType.Decimal
            };
            MCB_PMInputVoltageComboBox = new ListViewItem()
            {
                Index = 3,
                Title = AppResources.pm_input_voltage,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.ListSelector,
                ListSelectionCommand = ListSelectorCommand
            };

            try
            {
                MCB_loadPMInformation();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Logger.AddLog(true, "X26" + ex.ToString());
            }

            if (PMInformationAccessApply() == 0)
            {
                PMInfoItemSource.Clear();
                ACUserDialogs.ShowAlertWithOkButtonsAction(AppResources.no_data_found, () => { ShowViewModel<SavePMInfoViewModel>(new { pop = "pop" }); });
                return;
            }

            if (PMInfoItemSource.Count > 0)
            {
                PMInfoItemSource = new ObservableCollection<ListViewItem>(PMInfoItemSource.OrderBy(o => o.Index));
            }
            RaisePropertyChanged(() => PMInfoItemSource);

        }

        private int PMInformationAccessApply()
        {

            UIAccessControlUtility accessControlUtility = new UIAccessControlUtility();
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_PM_effieciency, MCB_PMeffiencieyTextBox, PMInfoItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_numberOfInstalledPMs, MCB_NumberOfInstalledPMsTextBox, PMInfoItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_PM_Voltage, MCB_PMVoltageComboBox, PMInfoItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_PM_Voltage, MCB_PMInputVoltageComboBox, PMInfoItemSource);
            //

            if (accessControlUtility.GetSavedCount() == 0)
            {
                ShowEdit = false;
            }
            return accessControlUtility.GetVisibleCount();
        }

        public IMvxCommand EditBtnClickCommand
        {
            get { return new MvxCommand(OnEditClick); }
        }

        void OnEditClick()
        {
            EditingMode = true;
            RefreshList();
            RaisePropertyChanged(() => PMInfoItemSource);

        }

        public void RefreshList()
        {
            foreach (var item in PMInfoItemSource)
            {
                item.IsEditable = EditingMode && item.IsEditEnabled;
                item.Text = item.SubTitle;
            }
        }

        public IMvxCommand BackBtnClickCommand
        {
            get { return new MvxCommand(OnCancelClick); }
        }

        void OnCancelClick()
        {
            if (CheckForEditedChanges())
            {
                ACUserDialogs.ShowAlertWithTwoButtons(AppResources.cancel_confirmation, "", AppResources.yes, AppResources.no, () => OnYesClick(), null);
            }
            else
            {
                OnYesClick();
            }
        }

        private void OnYesClick()
        {
            EditingMode = false;
            RefreshList();
            RaisePropertyChanged(() => PMInfoItemSource);

        }

        private bool CheckForEditedChanges()
        {
            bool textChanged = false;
            foreach (var item in PMInfoItemSource)
            {
                if (item.Text != item.SubTitle)
                {
                    textChanged = true;
                }
            }

            return textChanged;
        }

        public IMvxCommand SaveBtnClickCommand
        {
            get
            {
                return new MvxCommand(async () =>
                {
                    await OnSaveClick();
                });
            }
        }

        async Task OnSaveClick()
        {
            if (NetworkCheck())
            {

                if (MCB_VerfiyPMInformation())
                {
                    ACUserDialogs.ShowProgress();
                    McbCommunicationTypes caller = McbCommunicationTypes.NOCall;
                    bool arg1 = false;
                    try
                    {
                        MCB_SaveIntoPMInformation();
                        caller = McbCommunicationTypes.saveConfig;
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);

                    }

                    List<object> arguments = new List<object>();
                    arguments.Add(caller);
                    arguments.Add(arg1);
                    List<object> results = new List<object>();
                    try
                    {
                        results = await MCBQuantum.Instance.CommunicateMCB(arguments);
                        if (results.Count > 0)
                        {
                            var status = (CommunicationResult)results[2];
                            if (status == CommunicationResult.OK)
                            {
                                EditingMode = false;
                                RefreshList();
                                try
                                {
                                    ResetOldData();
                                    MCB_loadPMInformation();
                                    RaisePropertyChanged(() => PMInfoItemSource);

                                }

                                catch (Exception ex)
                                {
                                    Debug.WriteLine(ex.Message);
                                    Logger.AddLog(true, "X24" + ex.ToString());
                                }
                            }
                            else
                            {
                                ACUserDialogs.ShowAlertWithTitleAndOkButton(AppResources.save_failed);
                            }
                        }
                        else
                        {
                            ACUserDialogs.ShowAlertWithTitleAndOkButton(AppResources.save_failed);
                        }

                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }
                    ACUserDialogs.HideProgress();

                }
                else
                {
                    ACUserDialogs.ShowAlert(AppResources.alert_enter_valid + " " + verifyControl.GetErrorString());
                }
            }
        }


        void MCB_SaveIntoPMInformation()
        {
            if (!MCBQuantum.Instance.MCB_quickSerialNumberCheckStrict())
                return;

            MCBobject activeMCB = MCBQuantum.Instance.GetMCB();
            bool reload_Batt_BatterySettings = false;
            activeMCB.Config.numberOfInstalledPMs = MCB_NumberOfInstalledPMsTextBox.Text;
            if (activeMCB.Config.PMvoltage != (string)MCB_PMVoltageComboBox.Text)
                reload_Batt_BatterySettings = true;
            activeMCB.Config.PMvoltage = (string)MCB_PMVoltageComboBox.Text;
            activeMCB.Config.PMvoltageInputValue = (byte)MCB_PMInputVoltageComboBox.SelectedIndex;

            activeMCB.Config.PMefficiency = MCB_PMeffiencieyTextBox.Text;
            if (reload_Batt_BatterySettings)
            {
                if (string.IsNullOrEmpty(activeMCB.Config.batteryVoltage))
                    activeMCB.Config.batteryVoltage = "36";
            }

            //Auto-genrate the new Model
            int maxNumberOfPMs = 12;
            string model = "";
            model = activeMCB.Config.serialNumber.Substring(1, 2);
            if (model == "10")
                maxNumberOfPMs = 4;
            else if (model == "20")
                maxNumberOfPMs = 6;

            activeMCB.Config.model = "Q" + maxNumberOfPMs.ToString() + "-";
            activeMCB.Config.model += activeMCB.Config.PMvoltage + "-";
            int currentRating = int.Parse(activeMCB.Config.numberOfInstalledPMs) * (activeMCB.Config.PMvoltage == "36" ? 50 : 40);
            activeMCB.Config.model += currentRating.ToString() + "-";
            activeMCB.Config.model += (activeMCB.Config.PMvoltageInputValue != 0 ? "208" : "480");
            if (!activeMCB.Config.enablePLC && Convert.ToBoolean(activeMCB.Config.ledcontrol))
                activeMCB.Config.model += "-B";
        }
        bool MCB_VerfiyPMInformation()
        {
            if (!MCBQuantum.Instance.MCB_quickSerialNumberCheckStrict())
                return false;
            verifyControl = new VerifyControl();
            verifyControl.VerifyInteger(MCB_NumberOfInstalledPMsTextBox, MCB_NumberOfInstalledPMsTextBox, ControlObject.FormLimits.numberOfInstalledPMs_Min, ControlObject.FormLimits.numberOfInstalledPMs_Max);
            verifyControl.VerifyComboBox(MCB_PMVoltageComboBox, MCB_PMVoltageComboBox);
            verifyControl.VerifyComboBox(MCB_PMInputVoltageComboBox, MCB_PMInputVoltageComboBox);

            verifyControl.VerifyFloatNumber(MCB_PMeffiencieyTextBox, MCB_PMeffiencieyTextBox, 85.0f, 99.999f); ;
            return !verifyControl.HasErrors();
        }

        void ResetOldData()
        {
            foreach (var item in PMInfoItemSource)
            {
                item.SubTitle = string.Empty;
            }
        }

        void MCB_loadPMInformation()
        {
            if (!MCBQuantum.Instance.MCB_quickSerialNumberCheckStrict())
                return;
            MCBobject activeMCB = MCBQuantum.Instance.GetMCB();
            MCB_NumberOfInstalledPMsTextBox.SubTitle = MCB_NumberOfInstalledPMsTextBox.Text = activeMCB.Config.numberOfInstalledPMs;

            MCB_PMVoltageComboBox.Items = new List<object> {
            "36",
            "48",
            "80"};

            MCB_PMVoltageComboBox.SelectedItem = activeMCB.Config.PMvoltage;
            MCB_PMVoltageComboBox.SelectedIndex = MCB_PMVoltageComboBox.Items.FindIndex(o => ((string)o).Equals(MCB_PMVoltageComboBox.SelectedItem));

            MCB_PMInputVoltageComboBox.Items = new List<object> {
            "480",
            "208",
            "600",
            "380",
            "400"};

            if (activeMCB.Config.PMvoltageInputValue < MCB_PMInputVoltageComboBox.Items.Count)
            {
                MCB_PMInputVoltageComboBox.SelectedIndex = activeMCB.Config.PMvoltageInputValue;
                MCB_PMInputVoltageComboBox.SelectedItem = MCB_PMInputVoltageComboBox.Items[MCB_PMInputVoltageComboBox.SelectedIndex].ToString();
            }
            else
            {
                MCB_PMInputVoltageComboBox.SelectedIndex = -1;
                device_LoadWarnings.Add("PM Input voltage voltage is invalid");
            }

            if (MCB_PMVoltageComboBox.SelectedIndex == -1)
            {
                device_LoadWarnings.Add("PM voltage is invalid");
            }

            MCB_PMeffiencieyTextBox.SubTitle = MCB_PMeffiencieyTextBox.Text = activeMCB.Config.PMefficiency;
            //if (ControlObject.isDebugMaster)
            //    MCB_PowerModulesFaultsHistoryStartFaultIDTextBox.Text = connManager.activeMCB.minPMFaultRecordID.ToString();
            MCB_VerfiyPMInformation();
        }

        /// <summary>
        /// Gets the list selector command.
        /// </summary>
        /// <value>The list selector command.</value>
        public IMvxCommand ListSelectorCommand
        {
            get { return new MvxCommand<ListViewItem>(ExecuteListSelectorCommand); }
        }


        private void ExecuteListSelectorCommand(ListViewItem item)
        {
            if (item.CellType == ACUtility.CellTypes.ListSelector)
            {
                _mListSelector = Mvx.Resolve<IMvxMessenger>().Subscribe<ListSelectorMessage>(OnListSelectorMessage);
                ShowViewModel<ListSelectorViewModel>(new { title = item.Title, type = item.ListSelectorType, selectedItemIndex = PMInfoItemSource.IndexOf(item), ItemSourceStr = JsonConvert.SerializeObject(item.Items) });
            }
        }

        /// <summary>
        /// Ons the list selector message.
        /// </summary>
        /// <param name="obj">Object.</param>
        private void OnListSelectorMessage(ListSelectorMessage obj)
        {
            if (_mListSelector != null)
            {
                Mvx.Resolve<IMvxMessenger>().Unsubscribe<ListSelectorMessage>(_mListSelector);
                _mListSelector = null;
                PMInfoItemSource[obj.SelectedItemindex].Text = obj.SelectedItem;
                //PMInfoItemSource[obj.SelectedItemindex].SelectedItem = obj.SelectedItem;
                PMInfoItemSource[obj.SelectedItemindex].SelectedIndex = obj.SelectedIndex;
                RaisePropertyChanged(() => PMInfoItemSource);
            }
        }
    }
}
