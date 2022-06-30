using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;
using Newtonsoft.Json;

namespace actchargers
{
    public class SaveEnergyViewModel : BaseViewModel
    {

        MvxSubscriptionToken _mListSelector;
        VerifyControl verifyControl;
        ListViewItem MCB_Energy_lockoutStartTime;
        ListViewItem MCB_Energy_lockoutWindow;
        ListViewItem MCB_Energy_lockoutDays;

        ListViewItem MCB_Energy_powerfactor;
        ListViewItem MCB_Energy_powerStartTime;
        ListViewItem MCB_Energy_powerWindow;
        ListViewItem MCB_Energy_powerDays;

        ListViewItem MCB_Energy_lockoutInfoTextBox;
        ListViewItem MCB_Energy_powerInfoTextBox;

        List<string> device_LoadWarnings;

        private TableHeaderItem Lockout_Section;
        private TableHeaderItem Power_Limiting_Section;

        /// <summary>
        /// The info item source.
        /// </summary>
        private ObservableCollection<TableHeaderItem> _ListItemSource;
        public ObservableCollection<TableHeaderItem> ListItemSource
        {
            get { return _ListItemSource; }
            set
            {
                _ListItemSource = value;
                RaisePropertyChanged(() => ListItemSource);
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
                RaisePropertyChanged(() => ListItemSource);
                RaisePropertyChanged(() => EditingMode);
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


        public IMvxCommand EditBtnClickCommand
        {
            get { return new MvxCommand(OnEditClick); }
        }

        void OnEditClick()
        {
            EditingMode = true;
            CreateList();
        }

        void CreateList()
        {
            foreach (var item in ListItemSource)
            {
                foreach (var listItem in item)
                {
                    listItem.IsEditable = EditingMode && listItem.IsEditEnabled;
                    //if (listItem.EditableCellType == ACUtility.CellTypes.Days)
                    //{
                    //    if (listItem.Items != null && listItem.Items.Count > 0)
                    //    {
                    //        foreach (var dayItem in listItem.Items)
                    //        {
                    //            (dayItem as DayViewItem).IsEditable = EditingMode;
                    //        }
                    //    }

                    //}
                }
            }
        }


        void ResetOldData()
        {
            foreach (var item in ListItemSource)
            {
                foreach (var listItem in item)
                {
                    listItem.SubTitle = string.Empty;
                }
            }
        }

        public IMvxCommand CancelBtnClickCommand
        {
            get { return new MvxCommand(OnBackClick); }
        }


        /// <summary>
        /// Ons the back button click.
        /// </summary>
        public void OnBackButtonClick()
        {
            ShowViewModel<SaveEnergyViewModel>(new { pop = "pop" });
        }

        private void OnYesClick()
        {
            EditingMode = false;
            foreach (var item in ListItemSource)
            {
                foreach (var listItem in item)
                {
                    listItem.IsEditable = EditingMode;
                    listItem.Text = listItem.SubTitle;
                    if (listItem.EditableCellType == ACUtility.CellTypes.Days)
                    {
                        if (listItem.Items != null && listItem.Items.Count > 0)
                        {
                            foreach (var dayItem in listItem.Items)
                            {
                                //(dayItem as DayViewItem).IsEditable = EditingMode;
                                (dayItem as DayViewItem).IsSelected = (dayItem as DayViewItem).OriginalValue;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets the back button click command.
        /// </summary>
        /// <value>The back button click command.</value>
        public IMvxCommand BackBtnClickCommand
        {
            get { return new MvxCommand(OnBackClick); }
        }


        /// <summary>
        /// Ons the cancel click.
        /// </summary>
        void OnBackClick()
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

        private bool CheckForEditedChanges()
        {
            bool textChanged = false;

            foreach (var item in ListItemSource)
            {
                foreach (var listItem in item)
                {
                    if (listItem.EditableCellType == ACUtility.CellTypes.Days)
                    {
                        if (listItem.Items != null && listItem.Items.Count > 0)
                        {
                            foreach (var dayItem in listItem.Items)
                            {
                                if ((dayItem as DayViewItem).IsSelected != (dayItem as DayViewItem).OriginalValue)
                                {
                                    textChanged = true;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (listItem.Text != listItem.SubTitle)
                        {
                            textChanged = true;
                        }
                    }
                }
            }
            return textChanged;
        }


        /// <summary>
        /// Gets the list selector command.
        /// </summary>
        /// <value>The list selector command.</value>
        public IMvxCommand ListSelectorCommand
        {
            get { return new MvxCommand<ListViewItem>(ExecuteListSelectorCommand); }
        }

        public void ExecuteListSelectorCommand(ListViewItem item)
        {
            if (item.CellType == ACUtility.CellTypes.ListSelector)
            {
                _mListSelector = Mvx.Resolve<IMvxMessenger>().Subscribe<ListSelectorMessage>(OnListSelectorMessage);
                ShowViewModel<ListSelectorViewModel>(new { title = item.Title, type = item.ListSelectorType, selectedItemIndex = item.ParentIndex, selectedChildPosition = ListItemSource[item.ParentIndex].IndexOf(item), ItemSourceStr = JsonConvert.SerializeObject(item.Items) });
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
                ListItemSource[obj.SelectedItemindex][obj.SelectedChildItemindex].Text = obj.SelectedItem;
                ListItemSource[obj.SelectedItemindex][obj.SelectedChildItemindex].SelectedIndex = obj.SelectedIndex;
                RaisePropertyChanged(() => ListItemSource);

                SetInfoText(true);
            }
        }

        /// <summary>
        /// Gets the save button click command.
        /// </summary>
        /// <value>The save button click command.</value>
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
                if (MCB_VerfiyEnergyManagementSched())
                {
                    ACUserDialogs.ShowProgress();
                    McbCommunicationTypes caller = McbCommunicationTypes.NOCall;
                    bool arg1 = false;
                    try
                    {
                        MCB_SaveIntoEnergyManagementSched();
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
                                CreateList();

                                try
                                {
                                    ResetOldData();
                                    MCB_loadEnergyManagementSched();
                                }

                                catch (Exception ex)
                                {
                                    Debug.WriteLine(ex.Message);
                                    Logger.AddLog(true, "X24" + ex.ToString());
                                }
                            }
                            else
                            {
                                //Saving to BATTView failed.
                                //Be always in the Edit Screen
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
                    //TODO Show Correct Errors Alert
                    ACUserDialogs.ShowAlert(AppResources.alert_enter_valid + " " + verifyControl.GetErrorString());
                }

            }
        }

        public SaveEnergyViewModel()
        {
            ViewTitle = AppResources.energy_management;
            EditingMode = false;
            ShowEdit = true;
            ListItemSource = new ObservableCollection<TableHeaderItem>();
            device_LoadWarnings = new List<string>();

            Lockout_Section = new TableHeaderItem
            {
                SectionHeader = AppResources.lock_out,
            };
            Power_Limiting_Section = new TableHeaderItem
            {
                SectionHeader = AppResources.power_limiting,
            };
            ListItemSource.Add(Lockout_Section);
            ListItemSource.Add(Power_Limiting_Section);

            MCB_Energy_lockoutStartTime = new ListViewItem
            {
                Title = AppResources.lockout_start_time,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,//list type and no edit for all roles
                EditableCellType = ACUtility.CellTypes.TimePicker,
                TextValueChanged = TextValueChanged,
                ParentIndex = 0
            };

            MCB_Energy_lockoutWindow = new ListViewItem
            {
                Title = AppResources.lockout_window,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,//list type and no edit for all roles
                EditableCellType = ACUtility.CellTypes.ListSelector,
                ListSelectionCommand = ListSelectorCommand,
                ParentIndex = 0
            };



            MCB_Energy_lockoutDays = new ListViewItem
            {
                Title = AppResources.days,
                SubTitle = "",
                DefaultCellType = ACUtility.CellTypes.Days,
                EditableCellType = ACUtility.CellTypes.Days,
                IsEditable = EditingMode,
                ParentIndex = 0

            };

            AddDays(MCB_Energy_lockoutDays);


            MCB_Energy_lockoutInfoTextBox = new ListViewItem
            {
                Title = "",
                DefaultCellType = ACUtility.CellTypes.LabelText,
                EditableCellType = ACUtility.CellTypes.LabelText,
                IsEditable = false,
                IsVisible = true,
                IsEditEnabled = false,
                ParentIndex = 1
            };

            MCB_Energy_powerfactor = new ListViewItem
            {
                Title = AppResources.power_limit_factor,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,//list type and no edit for all roles
                EditableCellType = ACUtility.CellTypes.ListSelector,
                TextValueChanged = TextValueChanged,
                ListSelectionCommand = ListSelectorCommand,
                ParentIndex = 1
            };

            MCB_Energy_powerStartTime = new ListViewItem
            {
                Title = AppResources.start_time,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,//list type and no edit for all roles
                EditableCellType = ACUtility.CellTypes.TimePicker,
                TextValueChanged = TextValueChanged,
                ParentIndex = 1
            };

            MCB_Energy_powerWindow = new ListViewItem
            {
                Title = AppResources.power_limit_window,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,//list type and no edit for all roles
                EditableCellType = ACUtility.CellTypes.ListSelector,
                TextValueChanged = TextValueChanged,
                ListSelectionCommand = ListSelectorCommand,
                ParentIndex = 1
            };

            MCB_Energy_powerDays = new ListViewItem
            {
                Title = AppResources.days,
                SubTitle = "",
                DefaultCellType = ACUtility.CellTypes.Days,
                EditableCellType = ACUtility.CellTypes.Days,
                ParentIndex = 1
            };

            AddDays(MCB_Energy_powerDays);

            MCB_Energy_powerInfoTextBox = new ListViewItem
            {
                Title = "",
                DefaultCellType = ACUtility.CellTypes.LabelText,
                EditableCellType = ACUtility.CellTypes.LabelText,
                IsEditable = false,
                IsVisible = true,
                IsEditEnabled = false,
                ParentIndex = 1
            };

            try
            {
                MCB_loadEnergyManagementSched();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Logger.AddLog(true, "X26" + ex.ToString());
            }

            if (chargerEnergyManagmentAccessApply() == 0)
            {
                ListItemSource.Clear();
                ACUserDialogs.ShowAlertWithOkButtonsAction(AppResources.no_data_found, () => { ShowViewModel<SaveEnergyViewModel>(new { pop = "pop" }); });
                return;
            }

        }

        void AddDays(ListViewItem mCB_Energy_lockoutDays)
        {
            mCB_Energy_lockoutDays.Items = new List<object>();
            mCB_Energy_lockoutDays.Items.Add(new DayViewItem() { Title = "Monday", id = 0, DayButtonClicked = DaysButtonClickCommand });
            mCB_Energy_lockoutDays.Items.Add(new DayViewItem() { Title = "Tuesday", id = 1, DayButtonClicked = DaysButtonClickCommand });
            mCB_Energy_lockoutDays.Items.Add(new DayViewItem() { Title = "Wednesday", id = 2, DayButtonClicked = DaysButtonClickCommand });
            mCB_Energy_lockoutDays.Items.Add(new DayViewItem() { Title = "Thursday", id = 3, DayButtonClicked = DaysButtonClickCommand });
            mCB_Energy_lockoutDays.Items.Add(new DayViewItem() { Title = "Friday", id = 4, DayButtonClicked = DaysButtonClickCommand });
            mCB_Energy_lockoutDays.Items.Add(new DayViewItem() { Title = "Saturday", id = 5, DayButtonClicked = DaysButtonClickCommand });
            mCB_Energy_lockoutDays.Items.Add(new DayViewItem() { Title = "Sunday", id = 6, DayButtonClicked = DaysButtonClickCommand });

        }

        private int chargerEnergyManagmentAccessApply()
        {
            UIAccessControlUtility accessControlUtility = new UIAccessControlUtility();
            if (ControlObject.UserAccess.MCB_EnergyManagment == AccessLevelConsts.noAccess)
            {
                //MCB_EnergyButton.Hide();
                //return 0;
            }
            accessControlUtility.DoApplyAccessControlDayList(ControlObject.UserAccess.MCB_EnergyManagment, MCB_Energy_lockoutStartTime, ListItemSource[0]);
            accessControlUtility.DoApplyAccessControlDayList(ControlObject.UserAccess.MCB_EnergyManagment, MCB_Energy_lockoutWindow, ListItemSource[0]);
            accessControlUtility.DoApplyAccessControlDayList(ControlObject.UserAccess.MCB_EnergyManagment, MCB_Energy_lockoutDays, ListItemSource[0]);
            foreach (DayViewItem dayItems in MCB_Energy_lockoutDays.Items)
            {
                dayItems.IsEditable = MCB_Energy_lockoutDays.IsEditEnabled;
            }
            accessControlUtility.DoApplyAccessControlDayList(ControlObject.UserAccess.MCB_EnergyManagment, MCB_Energy_lockoutInfoTextBox, ListItemSource[0]);

            accessControlUtility.DoApplyAccessControlDayList(ControlObject.UserAccess.MCB_EnergyManagment, MCB_Energy_powerfactor, ListItemSource[1]);
            accessControlUtility.DoApplyAccessControlDayList(ControlObject.UserAccess.MCB_EnergyManagment, MCB_Energy_powerStartTime, ListItemSource[1]);
            accessControlUtility.DoApplyAccessControlDayList(ControlObject.UserAccess.MCB_EnergyManagment, MCB_Energy_powerWindow, ListItemSource[1]);
            accessControlUtility.DoApplyAccessControlDayList(ControlObject.UserAccess.MCB_EnergyManagment, MCB_Energy_powerDays, ListItemSource[1]);
            foreach (DayViewItem dayItems in MCB_Energy_powerDays.Items)
            {
                dayItems.IsEditable = MCB_Energy_powerDays.IsEditEnabled;
            }
            accessControlUtility.DoApplyAccessControlDayList(ControlObject.UserAccess.MCB_EnergyManagment, MCB_Energy_powerInfoTextBox, ListItemSource[1]);

            if (accessControlUtility.GetSavedCount() == 0)
            {
                ShowEdit = false;
            }
            return accessControlUtility.GetVisibleCount();
        }

        void MCB_loadEnergyManagementSched()
        {
            if (!MCBQuantum.Instance.MCB_quickSerialNumberCheckStrict())
                return;
            UInt32 diff;
            //Load lock

            MCB_Energy_lockoutWindow.Items = new List<object>();
            MCB_Energy_powerWindow.Items = new List<object>();
            for (int i = ControlObject.FormLimits.energyTimerStart; i <= ControlObject.FormLimits.energyTimerEnd; i += ControlObject.FormLimits.energyTimerStep)
            {
                MCB_Energy_lockoutWindow.Items.Add(String.Format("{0:00}:{1:00}", i / 4, (i % 4) * 15));
                MCB_Energy_powerWindow.Items.Add(String.Format("{0:00}:{1:00}", i / 4, (i % 4) * 15));
            }

            (MCB_Energy_lockoutDays.Items[6] as DayViewItem).IsSelected = (MCB_Energy_lockoutDays.Items[6] as DayViewItem).OriginalValue = MCBQuantum.Instance.GetMCB().Config.lockoutDaysMask.Sunday;
            (MCB_Energy_lockoutDays.Items[0] as DayViewItem).IsSelected = (MCB_Energy_lockoutDays.Items[0] as DayViewItem).OriginalValue = MCBQuantum.Instance.GetMCB().Config.lockoutDaysMask.Monday;
            (MCB_Energy_lockoutDays.Items[1] as DayViewItem).IsSelected = (MCB_Energy_lockoutDays.Items[1] as DayViewItem).OriginalValue = MCBQuantum.Instance.GetMCB().Config.lockoutDaysMask.Tuesday;
            (MCB_Energy_lockoutDays.Items[2] as DayViewItem).IsSelected = (MCB_Energy_lockoutDays.Items[2] as DayViewItem).OriginalValue = MCBQuantum.Instance.GetMCB().Config.lockoutDaysMask.Wednesday;
            (MCB_Energy_lockoutDays.Items[3] as DayViewItem).IsSelected = (MCB_Energy_lockoutDays.Items[3] as DayViewItem).OriginalValue = MCBQuantum.Instance.GetMCB().Config.lockoutDaysMask.Thursday;
            (MCB_Energy_lockoutDays.Items[4] as DayViewItem).IsSelected = (MCB_Energy_lockoutDays.Items[4] as DayViewItem).OriginalValue = MCBQuantum.Instance.GetMCB().Config.lockoutDaysMask.Friday;
            (MCB_Energy_lockoutDays.Items[5] as DayViewItem).IsSelected = (MCB_Energy_lockoutDays.Items[5] as DayViewItem).OriginalValue = MCBQuantum.Instance.GetMCB().Config.lockoutDaysMask.Saturday;
            //MCB_Energy_lockoutDays.IsEditEnabled = true;

            //MCB_Energy_lockoutWindow.SelectedItem = String.Format("{0:00}:{1:00}", connManager.activeMCB.MCBConfig.lo / 60, (st % 60));
            MCB_Energy_lockoutStartTime.SubTitle = MCB_Energy_lockoutStartTime.Text = String.Format("{0:00}:{1:00}", MCBQuantum.Instance.GetMCB().Config.lockoutStartTime / 3600, ((MCBQuantum.Instance.GetMCB().Config.lockoutStartTime % 3600) / 60));
            if (MCBQuantum.Instance.GetMCB().Config.lockoutStartTime < MCBQuantum.Instance.GetMCB().Config.lockoutCloseTime)
            {
                diff = MCBQuantum.Instance.GetMCB().Config.lockoutCloseTime - MCBQuantum.Instance.GetMCB().Config.lockoutStartTime;
            }
            else
            {
                diff = 86400 + MCBQuantum.Instance.GetMCB().Config.lockoutCloseTime - MCBQuantum.Instance.GetMCB().Config.lockoutStartTime;
            }
            MCB_Energy_lockoutWindow.SelectedItem = String.Format("{0:00}:{1:00}", (diff / 3600), (diff % 3600) / 60);
            if (MCB_Energy_lockoutWindow.SelectedIndex == -1)
            {
                device_LoadWarnings.Add("Energy Management Locout Window is invalid");

            }

            //Load energy 
            (MCB_Energy_powerDays.Items[6] as DayViewItem).IsSelected = (MCB_Energy_powerDays.Items[6] as DayViewItem).OriginalValue = MCBQuantum.Instance.GetMCB().Config.energyDaysMask.Sunday;
            (MCB_Energy_powerDays.Items[0] as DayViewItem).IsSelected = (MCB_Energy_powerDays.Items[0] as DayViewItem).OriginalValue = MCBQuantum.Instance.GetMCB().Config.energyDaysMask.Monday;
            (MCB_Energy_powerDays.Items[1] as DayViewItem).IsSelected = (MCB_Energy_powerDays.Items[1] as DayViewItem).OriginalValue = MCBQuantum.Instance.GetMCB().Config.energyDaysMask.Tuesday;
            (MCB_Energy_powerDays.Items[2] as DayViewItem).IsSelected = (MCB_Energy_powerDays.Items[2] as DayViewItem).OriginalValue = MCBQuantum.Instance.GetMCB().Config.energyDaysMask.Wednesday;
            (MCB_Energy_powerDays.Items[3] as DayViewItem).IsSelected = (MCB_Energy_powerDays.Items[3] as DayViewItem).OriginalValue = MCBQuantum.Instance.GetMCB().Config.energyDaysMask.Thursday;
            (MCB_Energy_powerDays.Items[4] as DayViewItem).IsSelected = (MCB_Energy_powerDays.Items[4] as DayViewItem).OriginalValue = MCBQuantum.Instance.GetMCB().Config.energyDaysMask.Friday;
            (MCB_Energy_powerDays.Items[5] as DayViewItem).IsSelected = (MCB_Energy_powerDays.Items[5] as DayViewItem).OriginalValue = MCBQuantum.Instance.GetMCB().Config.energyDaysMask.Saturday;
            //MCB_Energy_powerDays.IsEditEnabled = true;


            MCB_Energy_powerfactor.SelectedItem = MCBQuantum.Instance.GetMCB().Config.energyDecreaseValue.ToString();
            if (MCB_Energy_powerfactor.SelectedIndex == -1)
            {
                device_LoadWarnings.Add("Energy Management Power factor is invalid");

            }

            MCB_Energy_powerfactor.Items = new List<object>();
            for (int i = ControlObject.FormLimits.energyFactorMin; i <= ControlObject.FormLimits.energyFactorMax; i += ControlObject.FormLimits.energyFactorStep)
            {
                MCB_Energy_powerfactor.Items.Add(String.Format(i.ToString()));
            }

            MCB_Energy_powerStartTime.SubTitle = MCB_Energy_powerStartTime.Text = String.Format("{0:00}:{1:00}", MCBQuantum.Instance.GetMCB().Config.energyStartTime / 3600, ((MCBQuantum.Instance.GetMCB().Config.energyStartTime % 3600) / 60));
            MCB_Energy_lockoutStartTime.SubTitle = MCB_Energy_lockoutStartTime.Text = String.Format("{0:00}:{1:00}", MCBQuantum.Instance.GetMCB().Config.lockoutStartTime / 3600, ((MCBQuantum.Instance.GetMCB().Config.lockoutStartTime % 3600) / 60));
            if (MCBQuantum.Instance.GetMCB().Config.energyStartTime < MCBQuantum.Instance.GetMCB().Config.energyCloseTime)
            {
                diff = MCBQuantum.Instance.GetMCB().Config.energyCloseTime - MCBQuantum.Instance.GetMCB().Config.energyStartTime;
            }
            else
            {
                diff = 86400 + MCBQuantum.Instance.GetMCB().Config.energyCloseTime - MCBQuantum.Instance.GetMCB().Config.energyStartTime;
            }
            MCB_Energy_powerWindow.SelectedItem = String.Format("{0:00}:{1:00}", (diff / 3600), (diff % 3600) / 60);
            if (MCB_Energy_powerWindow.SelectedIndex == -1)
            {
                device_LoadWarnings.Add("Energy Management Energy Managment Window is invalid");

            }

            SetInfoText(false);
            MCB_VerfiyEnergyManagementSched();

        }

        bool MCB_VerfiyEnergyManagementSched()
        {
            if (!MCBQuantum.Instance.MCB_quickSerialNumberCheckStrict())
                return false;

            verifyControl = new VerifyControl();
            Match match;
            match = Regex.Match(MCB_Energy_lockoutStartTime.Text, @"^(\d{1,2}){1}:(\d{1,2}){1}$", RegexOptions.IgnoreCase);
            if (!match.Success || byte.Parse(match.Groups[1].Value) >= 24 || byte.Parse(match.Groups[2].Value) >= 60)
                verifyControl.InsertRemoveFault(true, MCB_Energy_lockoutStartTime);
            else
            {
                UInt32 tempLong = UInt32.Parse(match.Groups[1].Value) * 3600 + UInt32.Parse(match.Groups[2].Value) * 60;

                if (tempLong > 86400)
                    verifyControl.InsertRemoveFault(true, MCB_Energy_lockoutStartTime);
                else
                    verifyControl.InsertRemoveFault(false, MCB_Energy_lockoutStartTime);
            }
            verifyControl.VerifyComboBox(MCB_Energy_lockoutWindow, MCB_Energy_lockoutWindow);


            match = Regex.Match(MCB_Energy_powerStartTime.Text, @"^(\d{1,2}){1}:(\d{1,2}){1}$", RegexOptions.IgnoreCase);
            if (!match.Success || byte.Parse(match.Groups[1].Value) >= 24 || byte.Parse(match.Groups[2].Value) >= 60)
                verifyControl.InsertRemoveFault(true, MCB_Energy_powerStartTime, Power_Limiting_Section);
            else
            {
                UInt32 tempLong = UInt32.Parse(match.Groups[1].Value) * 3600 + UInt32.Parse(match.Groups[2].Value) * 60;

                if (tempLong > 86400)
                    verifyControl.InsertRemoveFault(true, MCB_Energy_powerStartTime, Power_Limiting_Section);
                else
                    verifyControl.InsertRemoveFault(false, MCB_Energy_powerStartTime, Power_Limiting_Section);
            }
            verifyControl.VerifyComboBox(MCB_Energy_powerWindow, MCB_Energy_powerWindow);
            verifyControl.VerifyComboBox(MCB_Energy_powerfactor, MCB_Energy_powerfactor);

            return !verifyControl.HasErrors();
        }

        void MCB_SaveIntoEnergyManagementSched()
        {
            if (!MCBQuantum.Instance.MCB_quickSerialNumberCheckStrict())
                return;
            Match match;
            match = Regex.Match(MCB_Energy_lockoutStartTime.Text, @"^(\d{1,2}){1}:(\d{1,2}){1}$", RegexOptions.IgnoreCase);
            MCBQuantum.Instance.GetMCB().Config.lockoutStartTime = UInt32.Parse(match.Groups[1].Value) * 3600 + UInt32.Parse(match.Groups[2].Value) * 60;

            match = Regex.Match(MCB_Energy_lockoutWindow.Text, @"^(\d{1,2}){1}:(\d{1,2}){1}$", RegexOptions.IgnoreCase);
            MCBQuantum.Instance.GetMCB().Config.lockoutCloseTime = MCBQuantum.Instance.GetMCB().Config.lockoutStartTime + UInt32.Parse(match.Groups[1].Value) * 3600 + UInt32.Parse(match.Groups[2].Value) * 60;
            if (MCBQuantum.Instance.GetMCB().Config.lockoutCloseTime != 86400)
                MCBQuantum.Instance.GetMCB().Config.lockoutCloseTime %= (86400);


            MCBQuantum.Instance.GetMCB().Config.lockoutDaysMask.Sunday = (MCB_Energy_lockoutDays.Items[6] as DayViewItem).IsSelected;
            MCBQuantum.Instance.GetMCB().Config.lockoutDaysMask.Monday = (MCB_Energy_lockoutDays.Items[0] as DayViewItem).IsSelected;
            MCBQuantum.Instance.GetMCB().Config.lockoutDaysMask.Tuesday = (MCB_Energy_lockoutDays.Items[1] as DayViewItem).IsSelected;
            MCBQuantum.Instance.GetMCB().Config.lockoutDaysMask.Wednesday = (MCB_Energy_lockoutDays.Items[2] as DayViewItem).IsSelected;
            MCBQuantum.Instance.GetMCB().Config.lockoutDaysMask.Thursday = (MCB_Energy_lockoutDays.Items[3] as DayViewItem).IsSelected;
            MCBQuantum.Instance.GetMCB().Config.lockoutDaysMask.Friday = (MCB_Energy_lockoutDays.Items[4] as DayViewItem).IsSelected;
            MCBQuantum.Instance.GetMCB().Config.lockoutDaysMask.Saturday = (MCB_Energy_lockoutDays.Items[5] as DayViewItem).IsSelected;

            match = Regex.Match(MCB_Energy_powerStartTime.Text, @"^(\d{1,2}){1}:(\d{1,2}){1}$", RegexOptions.IgnoreCase);
            MCBQuantum.Instance.GetMCB().Config.energyStartTime = UInt32.Parse(match.Groups[1].Value) * 3600 + UInt32.Parse(match.Groups[2].Value) * 60;

            match = Regex.Match(MCB_Energy_powerWindow.Text, @"^(\d{1,2}){1}:(\d{1,2}){1}$", RegexOptions.IgnoreCase);
            MCBQuantum.Instance.GetMCB().Config.energyCloseTime = MCBQuantum.Instance.GetMCB().Config.energyStartTime + UInt32.Parse(match.Groups[1].Value) * 3600 + UInt32.Parse(match.Groups[2].Value) * 60;
            if (MCBQuantum.Instance.GetMCB().Config.energyCloseTime != 86400)
                MCBQuantum.Instance.GetMCB().Config.energyCloseTime %= (86400);

            MCBQuantum.Instance.GetMCB().Config.energyDecreaseValue = byte.Parse((string)MCB_Energy_powerfactor.Text);
            MCBQuantum.Instance.GetMCB().Config.energyDaysMask.Sunday = (MCB_Energy_powerDays.Items[6] as DayViewItem).IsSelected;
            MCBQuantum.Instance.GetMCB().Config.energyDaysMask.Monday = (MCB_Energy_powerDays.Items[0] as DayViewItem).IsSelected;
            MCBQuantum.Instance.GetMCB().Config.energyDaysMask.Tuesday = (MCB_Energy_powerDays.Items[1] as DayViewItem).IsSelected;
            MCBQuantum.Instance.GetMCB().Config.energyDaysMask.Wednesday = (MCB_Energy_powerDays.Items[2] as DayViewItem).IsSelected;
            MCBQuantum.Instance.GetMCB().Config.energyDaysMask.Thursday = (MCB_Energy_powerDays.Items[3] as DayViewItem).IsSelected;
            MCBQuantum.Instance.GetMCB().Config.energyDaysMask.Friday = (MCB_Energy_powerDays.Items[4] as DayViewItem).IsSelected;
            MCBQuantum.Instance.GetMCB().Config.energyDaysMask.Saturday = (MCB_Energy_powerDays.Items[5] as DayViewItem).IsSelected;

        }

        public IMvxCommand TextValueChanged
        {
            get
            {
                return new MvxCommand(ExecuteTextValueChanged);
            }
        }

        void ExecuteTextValueChanged()
        {
            SetInfoText(true);
        }

        void SetInfoText(bool isReloading)
        {
            //Lockout
            string MCB_lockoutInfoString = "Lockout cycle starts at " + MCB_Energy_lockoutStartTime.Text + " and ends in " + MCB_Energy_lockoutWindow.Text + " on every";
            bool isOneDayAdded = false;

            foreach (DayViewItem item in MCB_Energy_lockoutDays.Items)
            {
                if (item.IsSelected)
                {
                    MCB_lockoutInfoString = MCB_lockoutInfoString + (isOneDayAdded ? ", " : " ") + item.Title;
                    isOneDayAdded = true;
                }
            }

            MCB_Energy_lockoutInfoTextBox.Text = MCB_lockoutInfoString;
            if (!isReloading)
            {
                MCB_Energy_lockoutInfoTextBox.SubTitle = MCB_Energy_lockoutInfoTextBox.Text;
            }

            //Power limiting text

            string MCB_PowerLimitingInfoString = "Power limit factor is " + MCB_Energy_powerfactor.Text + ". Cycle starts at " + MCB_Energy_powerStartTime.Text + " and ends in " + MCB_Energy_powerWindow.Text + " on every";
            bool isFirstSelected = false;

            foreach (DayViewItem item in MCB_Energy_powerDays.Items)
            {
                if (item.IsSelected)
                {
                    MCB_PowerLimitingInfoString = MCB_PowerLimitingInfoString + (isFirstSelected ? ", " : " ") + item.Title;
                    isFirstSelected = true;
                }
            }

            MCB_Energy_powerInfoTextBox.Text = MCB_PowerLimitingInfoString;
            if (!isReloading)
            {
                MCB_Energy_powerInfoTextBox.SubTitle = MCB_Energy_powerInfoTextBox.Text;
            }
        }

        public IMvxCommand DaysButtonClickCommand
        {
            get { return new MvxCommand<DayViewItem>(OnDaysButton); }
        }

        void OnDaysButton(DayViewItem item)
        {
            if (EditingMode && item.IsEditable)
            {
                item.IsSelected = !item.IsSelected;
                SetInfoText(true);
            }
        }
    }
}
