using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;
using Newtonsoft.Json;

namespace actchargers
{
    public class SaveFinishAndEQSettingsViewModel : BaseViewModel
    {
        private MvxSubscriptionToken _mListSelector;
        //settings view
        private ListViewItem MCB_FinishSettingsListTOSAVE;
        //finish cycle views
        private ListViewItem MCB_FinishCycleStartTimeTextBox;
        private ListViewItem MCB_FinishCycleDurationList;
        private ListViewItem MCB_FinishCycleDaysMask;//days
        private ListViewItem MCB_FinishCycleInfoTextBox;
        //equalize cycle views
        private ListViewItem MCB_EQCycleStartTimeTextBox;
        private ListViewItem MCB_EQCycleDurationList;
        private ListViewItem MCB_EQCycleDaysMask;//days
        private ListViewItem MCB_EQCycleInfoTextBox;

        //for battview
        private ListViewItem Batt_EQBlockedDaysList;
        private ListViewItem Batt_finishBlockedDaysList;

        //Sections items
        private TableHeaderItem Finish_Cycle_Section;
        private TableHeaderItem EQ_Cycle_Section;
        private TableHeaderItem Settings_Section;

        private List<string> device_LoadWarnings;
        private VerifyControl verifyControl;
        /// <summary>
        /// The finish and EQS ettings item source.
        /// </summary>
        private ObservableCollection<TableHeaderItem> _finishAndEQSettingsItemSource = new ObservableCollection<TableHeaderItem>();
        public ObservableCollection<TableHeaderItem> FinishAndEQSettingsItemSource
        {
            get { return _finishAndEQSettingsItemSource; }

            set
            {
                _finishAndEQSettingsItemSource = value;
                SetProperty(ref _finishAndEQSettingsItemSource, value);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:actchargers.FinishAndEQSettingsViewModel"/> class.
        /// </summary>
        public SaveFinishAndEQSettingsViewModel()
        {
            ViewTitle = AppResources.finish_eq_settings;
            ShowEdit = true;
            CreateList();
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

        private bool _isListChanged;
        public bool IsListChanged
        {
            get
            {
                return _isListChanged;
            }
            set
            {
                _isListChanged = value;
                RaisePropertyChanged(() => IsListChanged);
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
                RaisePropertyChanged(() => FinishAndEQSettingsItemSource);
                RaisePropertyChanged(() => EditingMode);
            }
        }

        /// <summary>
        /// Gets the edit button click command.
        /// </summary>
        /// <value>The edit button click command.</value>
        public IMvxCommand EditBtnClickCommand
        {
            get { return new MvxCommand(OnEditClick); }
        }

        void OnEditClick()
        {
            EditingMode = true;
            RefreshList();
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

        /// <summary>
        /// Triggered when the Save Btn is Clicked
        /// </summary>
        /// <returns>The save click.</returns>
        async Task OnSaveClick()
        {
            if (NetworkCheck())
            {
                if (IsBattView)
                {
                    if (Batt_VerfiyFI_EQ_Sched())
                    {
                        ACUserDialogs.ShowProgress();
                        BattViewCommunicationTypes caller = BattViewCommunicationTypes.NOCall;
                        bool arg1 = false;
                        try
                        {
                            Batt_SaveIntoFI_EQ_Sched();
                            caller = BattViewCommunicationTypes.saveConfig;
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
                            results = await BattViewQuantum.Instance.CommunicateBATTView(arguments);
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
                                        Batt_loadFI_EQ_Sched();
                                        RaisePropertyChanged(() => FinishAndEQSettingsItemSource);
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
                else
                {
                    if (MCB_VerfiyFI_EQ_Sched())
                    {
                        ACUserDialogs.ShowProgress();
                        McbCommunicationTypes caller = McbCommunicationTypes.NOCall;
                        bool arg1 = false;
                        try
                        {
                            MCB_SaveIntoFI_EQ_Sched();
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
                                        MCB_loadFI_EQ_Sched();
                                        RaisePropertyChanged(() => FinishAndEQSettingsItemSource);
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
        }

        void ResetOldData()
        {
            foreach (var item in FinishAndEQSettingsItemSource)
            {
                List<ListViewItem> sublist = (item as IEnumerable).Cast<ListViewItem>().ToList();
                foreach (var childItem in sublist)
                {
                    childItem.SubTitle = string.Empty;
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
        private void OnYesClick()
        {
            EditingMode = false;
            RefreshList();
            //Adding and Removing Finish Cycle Section based on original values 
            if (MCB_FinishSettingsListTOSAVE.SelectedIndex == 0)
            {
                if (FinishAndEQSettingsItemSource.Contains(Finish_Cycle_Section))
                {
                    FinishAndEQSettingsItemSource.Remove(Finish_Cycle_Section);
                }
            }
            else
            {
                if (!FinishAndEQSettingsItemSource.Contains(Finish_Cycle_Section))
                {
                    foreach (ListViewItem item in Finish_Cycle_Section)
                    {
                        item.IsEditable = EditingMode && item.IsEditEnabled;
                    }
                    FinishAndEQSettingsItemSource.Add(Finish_Cycle_Section);
                }
            }
            RaisePropertyChanged(() => FinishAndEQSettingsItemSource);
        }

        private bool CheckForEditedChanges()
        {
            bool textChanged = false;
            foreach (var item in FinishAndEQSettingsItemSource)
            {
                List<ListViewItem> sublist = (item as IEnumerable).Cast<ListViewItem>().ToList();
                foreach (var childItem in sublist)
                {
                    if (childItem.EditableCellType == ACUtility.CellTypes.Days)
                    {
                        if (childItem.Items != null && childItem.Items.Count > 0)
                        {
                            foreach (var dayItem in childItem.Items)
                            {
                                if ((dayItem as DayViewItem).IsSelected != (dayItem as DayViewItem).OriginalValue)
                                {
                                    textChanged = true;
                                }
                            }
                        }
                    }
                    else if (childItem.Text != childItem.SubTitle)
                    {
                        textChanged = true;
                    }
                }
            }

            return textChanged;
        }

        private void RefreshList()
        {
            foreach (var item in FinishAndEQSettingsItemSource)
            {
                foreach (var listItem in item)
                {
                    if (listItem.ParentIndex == 0)
                    {
                        if (listItem.SubTitle == "Always")
                        {
                            listItem.SelectedIndex = 0;
                        }
                        else
                        {
                            listItem.SelectedIndex = 1;
                        }
                    }
                    listItem.IsEditable = EditingMode && listItem.IsEditEnabled;
                    listItem.Text = listItem.SubTitle;
                    if (listItem.EditableCellType == ACUtility.CellTypes.Days)
                    {
                        if (listItem.Items != null && listItem.Items.Count > 0)
                        {
                            foreach (var dayItem in listItem.Items)
                            {
                                (dayItem as DayViewItem).IsSelected = (dayItem as DayViewItem).OriginalValue;
                            }
                        }
                    }
                }
            }
            RaisePropertyChanged(() => FinishAndEQSettingsItemSource);
        }

        /// <summary>
        /// Gets the list selector command.
        /// </summary>
        /// <value>The list selector command.</value>
        public IMvxCommand ListSelectorCommand
        {
            get { return new MvxCommand<ListViewItem>(ExecuteListSelectorCommand); }
        }

        /// <summary>
        /// Executes the list selector command.
        /// </summary>
        /// <param name="item">Item.</param>
        /// <param name="groupPosition">Group position.</param>
        /// <param name="childPosition">Child position.</param>
        public void ExecuteListSelectorCommand(ListViewItem item)
        {
            if (item.CellType == ACUtility.CellTypes.ListSelector)
            {
                _mListSelector = Mvx.Resolve<IMvxMessenger>().Subscribe<ListSelectorMessage>(OnListSelectorMessage);
                ShowViewModel<ListSelectorViewModel>(new { selectedItemIndex = item.ParentIndex, selectedChildPosition = FinishAndEQSettingsItemSource[item.ParentIndex].IndexOf(item), ItemSourceStr = JsonConvert.SerializeObject(item.Items), title = item.Title });
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
                FinishAndEQSettingsItemSource[obj.SelectedItemindex][obj.SelectedChildItemindex].Text = obj.SelectedItem;
                //FinishAndEQSettingsItemSource[obj.SelectedItemindex][obj.SelectedChildItemindex].SelectedItem = obj.SelectedItem;
                FinishAndEQSettingsItemSource[obj.SelectedItemindex][obj.SelectedChildItemindex].SelectedIndex = obj.SelectedIndex;

                if (obj.SelectedItemindex == 0)
                {
                    if (ControlObject.UserAccess.MCB_FI_sched_Settings != AccessLevelConsts.noAccess)
                    {
                        if (MCB_FinishSettingsListTOSAVE.SelectedIndex == 1 && ControlObject.UserAccess.MCB_FI_sched_CustomSettings != AccessLevelConsts.noAccess)
                        {
                            if (!FinishAndEQSettingsItemSource.Contains(Finish_Cycle_Section))
                            {
                                foreach (ListViewItem item in Finish_Cycle_Section)
                                {
                                    item.IsEditable = EditingMode && item.IsEditEnabled;
                                }
                                FinishAndEQSettingsItemSource.Add(Finish_Cycle_Section);
                            }
                        }
                        else
                        {
                            if (FinishAndEQSettingsItemSource.Contains(Finish_Cycle_Section))
                            {
                                FinishAndEQSettingsItemSource.Remove(Finish_Cycle_Section);
                            }
                        }
                    }
                }
                RaisePropertyChanged(() => FinishAndEQSettingsItemSource);
            }
        }
        /// <summary>
        /// Mcbloads the fi eq sched.
        /// </summary>
        void MCB_loadFI_EQ_Sched()
        {
            if (!MCBQuantum.Instance.MCB_quickSerialNumberCheckStrict())
                return;

            MCBobject activeMCB = MCBQuantum.Instance.GetMCB();

            MCB_FinishSettingsListTOSAVE.Items = new List<object>();
            MCB_FinishSettingsListTOSAVE.Items.Clear();
            MCB_FinishSettingsListTOSAVE.Items.Add("Always");
            MCB_FinishSettingsListTOSAVE.Items.Add("Custom");
            if (activeMCB.Config.chargerType == 1)//Conventional 
            {
                MCB_FinishSettingsListTOSAVE.IsEditEnabled = true;
                if (!activeMCB.Config.FIschedulingMode)
                {
                    MCB_FinishSettingsListTOSAVE.SelectedIndex = 0;
                    MCB_FinishSettingsListTOSAVE.SelectedItem = MCB_FinishSettingsListTOSAVE.Items[MCB_FinishSettingsListTOSAVE.SelectedIndex].ToString();
                }
                else
                {
                    MCB_FinishSettingsListTOSAVE.SelectedIndex = 1;
                    MCB_FinishSettingsListTOSAVE.SelectedItem = MCB_FinishSettingsListTOSAVE.Items[MCB_FinishSettingsListTOSAVE.SelectedIndex].ToString();

                }
            }
            else
            {
                MCB_FinishSettingsListTOSAVE.IsEditEnabled = false;
                MCB_FinishSettingsListTOSAVE.SelectedIndex = 1;
                MCB_FinishSettingsListTOSAVE.SelectedItem = MCB_FinishSettingsListTOSAVE.Items[MCB_FinishSettingsListTOSAVE.SelectedIndex].ToString();
            }
            //as we dont have radio group checked change listener those validations doing here
            if (ControlObject.UserAccess.MCB_FI_sched_Settings != AccessLevelConsts.noAccess)
            {
                if (MCB_FinishSettingsListTOSAVE.SelectedIndex == 1 && ControlObject.UserAccess.MCB_FI_sched_CustomSettings != AccessLevelConsts.noAccess)
                {
                    if (!FinishAndEQSettingsItemSource.Contains(Finish_Cycle_Section))
                    {
                        FinishAndEQSettingsItemSource.Add(Finish_Cycle_Section);
                    }
                }
                else
                {
                    if (FinishAndEQSettingsItemSource.Contains(Finish_Cycle_Section))
                    {
                        FinishAndEQSettingsItemSource.Remove(Finish_Cycle_Section);
                    }
                }
            }
            else
            {
                MCB_FinishSettingsListTOSAVE.IsEditable = false;
            }

            //Loaf FI
            MCB_FinishCycleStartTimeTextBox.SubTitle = MCB_FinishCycleStartTimeTextBox.Text = activeMCB.Config.FIstartWindow;
            MCB_FinishCycleStartTimeTextBox.IsEditEnabled = true;

            MCB_FinishCycleDurationList.Items = new List<object>();
            for (int i = 1; i <= 96; i++)
            {
                int m = i * 15;
                string m2 = String.Format("{0:00}:{1:00}", (m / 60), (m % 60));
                MCB_FinishCycleDurationList.Items.Add(m2);
            }
            MCB_FinishCycleDurationList.SelectedItem = activeMCB.Config.finishWindow;
            MCB_FinishCycleDurationList.SelectedIndex = MCB_FinishCycleDurationList.Items.FindIndex(o => ((string)o).Equals(MCB_FinishCycleDurationList.SelectedItem));

            if (activeMCB.Config.FIschedulingMode && MCB_FinishCycleDurationList.SelectedIndex == -1)
            {
                device_LoadWarnings.Add("Finish Cycle Window span is invalid");
            }
            MCB_FinishCycleDurationList.IsEditEnabled = true;

            (MCB_FinishCycleDaysMask.Items[0] as DayViewItem).IsSelected = (MCB_FinishCycleDaysMask.Items[0] as DayViewItem).OriginalValue = activeMCB.Config.finishDaysMask.Monday;
            (MCB_FinishCycleDaysMask.Items[1] as DayViewItem).IsSelected = (MCB_FinishCycleDaysMask.Items[1] as DayViewItem).OriginalValue = activeMCB.Config.finishDaysMask.Tuesday;
            (MCB_FinishCycleDaysMask.Items[2] as DayViewItem).IsSelected = (MCB_FinishCycleDaysMask.Items[2] as DayViewItem).OriginalValue = activeMCB.Config.finishDaysMask.Wednesday;
            (MCB_FinishCycleDaysMask.Items[3] as DayViewItem).IsSelected = (MCB_FinishCycleDaysMask.Items[3] as DayViewItem).OriginalValue = activeMCB.Config.finishDaysMask.Thursday;
            (MCB_FinishCycleDaysMask.Items[4] as DayViewItem).IsSelected = (MCB_FinishCycleDaysMask.Items[4] as DayViewItem).OriginalValue = activeMCB.Config.finishDaysMask.Friday;
            (MCB_FinishCycleDaysMask.Items[5] as DayViewItem).IsSelected = (MCB_FinishCycleDaysMask.Items[5] as DayViewItem).OriginalValue = activeMCB.Config.finishDaysMask.Saturday;
            (MCB_FinishCycleDaysMask.Items[6] as DayViewItem).IsSelected = (MCB_FinishCycleDaysMask.Items[6] as DayViewItem).OriginalValue = activeMCB.Config.finishDaysMask.Sunday;

            //MCB_FinishCycleDaysMask.IsEditEnabled = true;
            foreach (DayViewItem dayItems in MCB_FinishCycleDaysMask.Items)
            {
                dayItems.IsEditable = MCB_FinishCycleDaysMask.IsEditEnabled;
            }


            MCB_FinishCycleInfoTextBox.IsEditEnabled = false;
            //Load EQ
            MCB_EQCycleStartTimeTextBox.SubTitle = MCB_EQCycleStartTimeTextBox.Text = activeMCB.Config.EQstartWindow;
            MCB_EQCycleStartTimeTextBox.IsEditEnabled = true;
            MCB_EQCycleDurationList.Items = new List<object>();
            for (int i = 1; i <= 96; i++)
            {
                int m = i * 15;
                string m2 = String.Format("{0:00}:{1:00}", (m / 60), (m % 60));
                MCB_EQCycleDurationList.Items.Add(m2);
            }
            MCB_EQCycleDurationList.SelectedItem = activeMCB.Config.EQwindow;
            MCB_EQCycleDurationList.SelectedIndex = MCB_EQCycleDurationList.Items.FindIndex(o => ((string)o).Equals(MCB_EQCycleDurationList.SelectedItem));
            MCB_EQCycleDurationList.IsEditEnabled = true;
            if (MCB_EQCycleDurationList.SelectedIndex == -1)
            {
                device_LoadWarnings.Add("Equalize Cycle Window span is invalid");
            }
            MCB_EQCycleDurationList.IsEditEnabled = true;

            (MCB_EQCycleDaysMask.Items[0] as DayViewItem).IsSelected = (MCB_EQCycleDaysMask.Items[0] as DayViewItem).OriginalValue = activeMCB.Config.EQdaysMask.Monday;
            (MCB_EQCycleDaysMask.Items[1] as DayViewItem).IsSelected = (MCB_EQCycleDaysMask.Items[1] as DayViewItem).OriginalValue = activeMCB.Config.EQdaysMask.Tuesday;
            (MCB_EQCycleDaysMask.Items[2] as DayViewItem).IsSelected = (MCB_EQCycleDaysMask.Items[2] as DayViewItem).OriginalValue = activeMCB.Config.EQdaysMask.Wednesday;
            (MCB_EQCycleDaysMask.Items[3] as DayViewItem).IsSelected = (MCB_EQCycleDaysMask.Items[3] as DayViewItem).OriginalValue = activeMCB.Config.EQdaysMask.Thursday;
            (MCB_EQCycleDaysMask.Items[4] as DayViewItem).IsSelected = (MCB_EQCycleDaysMask.Items[4] as DayViewItem).OriginalValue = activeMCB.Config.EQdaysMask.Friday;
            (MCB_EQCycleDaysMask.Items[5] as DayViewItem).IsSelected = (MCB_EQCycleDaysMask.Items[5] as DayViewItem).OriginalValue = activeMCB.Config.EQdaysMask.Saturday;
            (MCB_EQCycleDaysMask.Items[6] as DayViewItem).IsSelected = (MCB_EQCycleDaysMask.Items[6] as DayViewItem).OriginalValue = activeMCB.Config.EQdaysMask.Sunday;

            //MCB_EQCycleDaysMask.IsEditEnabled = true;
            //Load EQ FI values

            foreach (DayViewItem dayItems in MCB_EQCycleDaysMask.Items)
            {
                dayItems.IsEditable = MCB_EQCycleDaysMask.IsEditEnabled;
            }

            SetInfoText(false);
            MCB_EQCycleInfoTextBox.IsEditEnabled = false;
            MCB_VerfiyFI_EQ_Sched();

        }
        bool MCB_VerfiyFI_EQ_Sched()
        {
            if (!MCBQuantum.Instance.MCB_quickSerialNumberCheckStrict())
                return false;

            verifyControl = new VerifyControl();
            Match match;
            if (MCB_FinishSettingsListTOSAVE.SelectedIndex != 0)
            {
                match = Regex.Match(MCB_FinishCycleStartTimeTextBox.Text, @"^(\d{1,2}){1}:(\d{1,2}){1}$", RegexOptions.IgnoreCase);

                if (!match.Success || byte.Parse(match.Groups[1].Value) >= 24 || byte.Parse(match.Groups[2].Value) >= 60)
                    verifyControl.InsertRemoveFault(true, MCB_FinishCycleStartTimeTextBox, Finish_Cycle_Section);
                else
                {
                    UInt32 tempLong = UInt32.Parse(match.Groups[1].Value) * 3600 + UInt32.Parse(match.Groups[2].Value) * 60;

                    if (tempLong > 86400)
                        verifyControl.InsertRemoveFault(true, MCB_FinishCycleStartTimeTextBox, Finish_Cycle_Section);
                    else
                        verifyControl.InsertRemoveFault(false, MCB_FinishCycleStartTimeTextBox, Finish_Cycle_Section);
                }
                verifyControl.VerifyComboBox(MCB_FinishCycleDurationList, Finish_Cycle_Section);//, MCB_finishCycleDurationListLabel
            }
            match = Regex.Match(MCB_EQCycleStartTimeTextBox.Text, @"^(\d{1,2}){1}:(\d{1,2}){1}$", RegexOptions.IgnoreCase);

            if (!match.Success || byte.Parse(match.Groups[1].Value) >= 24 || byte.Parse(match.Groups[2].Value) >= 60)
                verifyControl.InsertRemoveFault(true, MCB_EQCycleStartTimeTextBox, EQ_Cycle_Section);
            else
            {
                UInt32 tempLong = UInt32.Parse(match.Groups[1].Value) * 3600 + UInt32.Parse(match.Groups[2].Value) * 60;

                if (tempLong > 86400)
                    verifyControl.InsertRemoveFault(true, MCB_EQCycleStartTimeTextBox, EQ_Cycle_Section);
                else
                    verifyControl.InsertRemoveFault(false, MCB_EQCycleStartTimeTextBox, EQ_Cycle_Section);
            }
            verifyControl.VerifyComboBox(MCB_EQCycleDurationList, MCB_EQCycleDurationList);


            return !verifyControl.HasErrors();
        }
        private int chargerFinishEQSchedulingAccessApply()
        {
            UIAccessControlUtility accessControlUtility = new UIAccessControlUtility();

            if (!accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_FI_sched_Settings, MCB_FinishSettingsListTOSAVE, Settings_Section))

            {
                //Settings_Section.IsSectionVisible = false;
            }

            //if (ControlObject.user_access.MCB_FI_EQ_sched_CustomSettings == access_level.noAccess)
            //{
            //    EQ_Cycle_Section.IsSectionVisible = false;
            //}
            //if (ControlObject.user_access.MCB_FI_sched_CustomSettings == access_level.noAccess)
            //{
            //    Finish_Cycle_Section.IsSectionVisible = false;
            //}

            //equalize cycle
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_FI_EQ_sched_CustomSettings, MCB_EQCycleStartTimeTextBox, EQ_Cycle_Section);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_FI_EQ_sched_CustomSettings, MCB_EQCycleDurationList, EQ_Cycle_Section);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_FI_EQ_sched_CustomSettings, MCB_EQCycleDaysMask, EQ_Cycle_Section);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_FI_EQ_sched_CustomSettings, MCB_EQCycleInfoTextBox, EQ_Cycle_Section);
            MCB_EQCycleInfoTextBox.IsEditEnabled = false;
            foreach (DayViewItem dayItems in MCB_EQCycleDaysMask.Items)
            {
                dayItems.IsEditable = MCB_EQCycleDaysMask.IsEditEnabled;
            }
            //finish cycle
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_FI_sched_CustomSettings, MCB_FinishCycleStartTimeTextBox, Finish_Cycle_Section);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_FI_sched_CustomSettings, MCB_FinishCycleDurationList, Finish_Cycle_Section);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_FI_sched_CustomSettings, MCB_FinishCycleDaysMask, Finish_Cycle_Section);
            foreach (DayViewItem dayItems in MCB_FinishCycleDaysMask.Items)
            {
                dayItems.IsEditable = MCB_FinishCycleDaysMask.IsEditEnabled;
            }
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_FI_sched_CustomSettings, MCB_FinishCycleInfoTextBox, Finish_Cycle_Section);
            MCB_FinishCycleInfoTextBox.IsEditEnabled = false;
            if (accessControlUtility.GetSavedCount() == 0)
            {
                ShowEdit = false;
            }
            if (MCBQuantum.Instance.GetMCB().Config.chargerType == 1)//Conventional 
            {
                MCB_FinishSettingsListTOSAVE.IsEditEnabled = true;
            }
            else
            {
                MCB_FinishSettingsListTOSAVE.IsEditEnabled = false;
            }
            return accessControlUtility.GetVisibleCount();
        }

        void MCB_SaveIntoFI_EQ_Sched()
        {
            if (!MCBQuantum.Instance.MCB_quickSerialNumberCheckStrict())
                return;
            MCBobject activeMCB = MCBQuantum.Instance.GetMCB();

            activeMCB.Config.FIschedulingMode = MCB_FinishSettingsListTOSAVE.SelectedIndex == 1 ? true : false; ;
            //Loaf FI
            if (activeMCB.Config.FIschedulingMode)
            {
                activeMCB.Config.FIstartWindow = MCB_FinishCycleStartTimeTextBox.Text;
                activeMCB.Config.finishWindow = (string)MCB_FinishCycleDurationList.Text;
                activeMCB.Config.finishDaysMask.Sunday = (MCB_FinishCycleDaysMask.Items[6] as DayViewItem).IsSelected;
                activeMCB.Config.finishDaysMask.Monday = (MCB_FinishCycleDaysMask.Items[0] as DayViewItem).IsSelected;
                activeMCB.Config.finishDaysMask.Tuesday = (MCB_FinishCycleDaysMask.Items[1] as DayViewItem).IsSelected;
                activeMCB.Config.finishDaysMask.Wednesday = (MCB_FinishCycleDaysMask.Items[2] as DayViewItem).IsSelected;
                activeMCB.Config.finishDaysMask.Thursday = (MCB_FinishCycleDaysMask.Items[3] as DayViewItem).IsSelected;
                activeMCB.Config.finishDaysMask.Friday = (MCB_FinishCycleDaysMask.Items[4] as DayViewItem).IsSelected;
                activeMCB.Config.finishDaysMask.Saturday = (MCB_FinishCycleDaysMask.Items[5] as DayViewItem).IsSelected;

            }
            activeMCB.Config.EQstartWindow = MCB_EQCycleStartTimeTextBox.Text;
            activeMCB.Config.EQwindow = (string)MCB_EQCycleDurationList.Text;

            activeMCB.Config.EQdaysMask.Sunday = (MCB_EQCycleDaysMask.Items[6] as DayViewItem).IsSelected;
            activeMCB.Config.EQdaysMask.Monday = (MCB_EQCycleDaysMask.Items[0] as DayViewItem).IsSelected;
            activeMCB.Config.EQdaysMask.Tuesday = (MCB_EQCycleDaysMask.Items[1] as DayViewItem).IsSelected;
            activeMCB.Config.EQdaysMask.Wednesday = (MCB_EQCycleDaysMask.Items[2] as DayViewItem).IsSelected;
            activeMCB.Config.EQdaysMask.Thursday = (MCB_EQCycleDaysMask.Items[3] as DayViewItem).IsSelected;
            activeMCB.Config.EQdaysMask.Friday = (MCB_EQCycleDaysMask.Items[4] as DayViewItem).IsSelected;
            activeMCB.Config.EQdaysMask.Saturday = (MCB_EQCycleDaysMask.Items[5] as DayViewItem).IsSelected;

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
            string MCB_FinishCycleInfoString = "Equalize cycle starts at " + MCB_FinishCycleStartTimeTextBox.Text + " and ends in " + MCB_FinishCycleDurationList.Text + " on every";
            bool isOneDayAdded = false;

            foreach (DayViewItem item in MCB_FinishCycleDaysMask.Items)
            {
                if (item.IsSelected)
                {
                    MCB_FinishCycleInfoString = MCB_FinishCycleInfoString + (isOneDayAdded ? ", " : " ") + item.Title;
                    isOneDayAdded = true;
                }
            }

            MCB_FinishCycleInfoTextBox.Text = MCB_FinishCycleInfoString;
            if (!isReloading)
            {
                MCB_FinishCycleInfoTextBox.SubTitle = MCB_FinishCycleInfoTextBox.Text;
            }

            //Equalize cycle text

            string MCB_EQCycleInfoString = "Equalize cycle starts at " + MCB_EQCycleStartTimeTextBox.Text + " and ends in " + MCB_EQCycleDurationList.Text + " on every";
            bool isFirstSelected = false;

            foreach (DayViewItem item in MCB_EQCycleDaysMask.Items)
            {
                if (item.IsSelected)
                {
                    MCB_EQCycleInfoString = MCB_EQCycleInfoString + (isFirstSelected ? ", " : " ") + item.Title;
                    isFirstSelected = true;
                }
            }

            MCB_EQCycleInfoTextBox.Text = MCB_EQCycleInfoString;
            if (!isReloading)
            {
                MCB_EQCycleInfoTextBox.SubTitle = MCB_EQCycleInfoTextBox.Text;
            }
        }


        #region Battview 
        void Batt_loadFI_EQ_Sched()
        {
            if (!BattViewQuantum.Instance.BattView_quickSerialNumberCheckStrict())
                return;

            BattViewObject activeBattView = BattViewQuantum.Instance.GetBATTView();

            bool alwaysFinish = false;
            UInt32 diff;
            if (activeBattView.Config.FIstartWindow < activeBattView.Config.FIcloseWindow)
            {
                diff = activeBattView.Config.FIcloseWindow - activeBattView.Config.FIstartWindow;
            }
            else
            {
                diff = 86400 + activeBattView.Config.FIcloseWindow - activeBattView.Config.FIstartWindow;
            }
            if (activeBattView.Config.FIstartWindow == 0 && diff >= 24 * 60 * 60 - 1 && (activeBattView.Config.FIdaysMask & 0x7f) == 0x7f)
            {
                alwaysFinish = true;
            }

            MCB_FinishSettingsListTOSAVE.Items = new List<object>();
            MCB_FinishSettingsListTOSAVE.Items.Clear();
            MCB_FinishSettingsListTOSAVE.Items.Add("Always");
            MCB_FinishSettingsListTOSAVE.Items.Add("Custom");
            if (activeBattView.Config.chargerType == 1)//Conventional 
            {
                MCB_FinishSettingsListTOSAVE.IsEditEnabled = true;
                if (alwaysFinish)
                {
                    MCB_FinishSettingsListTOSAVE.SelectedIndex = 0;
                    MCB_FinishSettingsListTOSAVE.SelectedItem = MCB_FinishSettingsListTOSAVE.Items[MCB_FinishSettingsListTOSAVE.SelectedIndex].ToString();
                }
                else
                {
                    MCB_FinishSettingsListTOSAVE.SelectedIndex = 1;
                    MCB_FinishSettingsListTOSAVE.SelectedItem = MCB_FinishSettingsListTOSAVE.Items[MCB_FinishSettingsListTOSAVE.SelectedIndex].ToString();
                }
            }
            else
            {
                MCB_FinishSettingsListTOSAVE.IsEditEnabled = false;
                MCB_FinishSettingsListTOSAVE.SelectedIndex = 1;
                MCB_FinishSettingsListTOSAVE.SelectedItem = MCB_FinishSettingsListTOSAVE.Items[MCB_FinishSettingsListTOSAVE.SelectedIndex].ToString();

            }
            //Load FI
            MCB_FinishCycleStartTimeTextBox.SubTitle = MCB_FinishCycleStartTimeTextBox.Text = String.Format("{0:00}:{1:00}", (activeBattView.Config.FIstartWindow / 3600), (activeBattView.Config.FIstartWindow % 3600) / 60);
            if (diff == 86400 - 1)
                diff = 86400;
            MCB_FinishCycleDurationList.Items = new List<object>();
            for (int i = 1; i <= 96; i++)
            {
                int m = i * 15;
                string m2 = String.Format("{0:00}:{1:00}", (m / 60), (m % 60));
                MCB_FinishCycleDurationList.Items.Add(m2);
            }
            MCB_FinishCycleDurationList.SelectedItem = String.Format("{0:00}:{1:00}", (diff / 3600), (diff % 3600) / 60);
            MCB_FinishCycleDurationList.SelectedIndex = MCB_FinishCycleDurationList.Items.FindIndex(o => ((string)o).Equals(MCB_FinishCycleDurationList.SelectedItem));

            if (!alwaysFinish && MCB_FinishCycleDurationList.SelectedIndex == -1)
                device_LoadWarnings.Add("Finish Cycle Window span is invalid");

            MCB_FinishCycleDurationList.IsEditEnabled = true;


            (MCB_FinishCycleDaysMask.Items[6] as DayViewItem).OriginalValue = (MCB_FinishCycleDaysMask.Items[6] as DayViewItem).IsSelected = (activeBattView.Config.FIdaysMask & 0x01) != 0;
            (MCB_FinishCycleDaysMask.Items[0] as DayViewItem).OriginalValue = (MCB_FinishCycleDaysMask.Items[0] as DayViewItem).IsSelected = (activeBattView.Config.FIdaysMask & 0x02) != 0;
            (MCB_FinishCycleDaysMask.Items[1] as DayViewItem).OriginalValue = (MCB_FinishCycleDaysMask.Items[1] as DayViewItem).IsSelected = (activeBattView.Config.FIdaysMask & 0x04) != 0;
            (MCB_FinishCycleDaysMask.Items[2] as DayViewItem).OriginalValue = (MCB_FinishCycleDaysMask.Items[2] as DayViewItem).IsSelected = (activeBattView.Config.FIdaysMask & 0x08) != 0;
            (MCB_FinishCycleDaysMask.Items[3] as DayViewItem).OriginalValue = (MCB_FinishCycleDaysMask.Items[3] as DayViewItem).IsSelected = (activeBattView.Config.FIdaysMask & 0x10) != 0;
            (MCB_FinishCycleDaysMask.Items[4] as DayViewItem).OriginalValue = (MCB_FinishCycleDaysMask.Items[4] as DayViewItem).IsSelected = (activeBattView.Config.FIdaysMask & 0x20) != 0;
            (MCB_FinishCycleDaysMask.Items[5] as DayViewItem).OriginalValue = (MCB_FinishCycleDaysMask.Items[5] as DayViewItem).IsSelected = (activeBattView.Config.FIdaysMask & 0x40) != 0;


            //MCB_FinishCycleDaysMask.IsEditEnabled = true;
            MCB_FinishCycleInfoTextBox.IsEditEnabled = false;

            //Load EQ
            if (activeBattView.Config.EQstartWindow < activeBattView.Config.EQcloseWindow)
            {
                diff = activeBattView.Config.EQcloseWindow - activeBattView.Config.EQstartWindow;
            }
            else
            {
                diff = 86400 + activeBattView.Config.EQcloseWindow - activeBattView.Config.EQstartWindow;
            }
            if (diff == 86400 - 1)
                diff = 86400;

            MCB_EQCycleStartTimeTextBox.SubTitle = MCB_EQCycleStartTimeTextBox.Text = String.Format("{0:00}:{1:00}", (activeBattView.Config.EQstartWindow / 3600), (activeBattView.Config.EQstartWindow % 3600) / 60);



            MCB_EQCycleDurationList.Items = new List<object>();
            for (int i = 1; i <= 96; i++)
            {
                int m = i * 15;
                string m2 = String.Format("{0:00}:{1:00}", (m / 60), (m % 60));
                MCB_EQCycleDurationList.Items.Add(m2);
            }
            MCB_EQCycleDurationList.SelectedItem = String.Format("{0:00}:{1:00}", (diff / 3600), (diff % 3600) / 60);
            MCB_EQCycleDurationList.SelectedIndex = MCB_EQCycleDurationList.Items.FindIndex(o => ((string)o).Equals(MCB_EQCycleDurationList.SelectedItem));

            if (MCB_EQCycleDurationList.SelectedIndex == -1)
                device_LoadWarnings.Add("Equalize Cycle Window span is invalid");

            (MCB_EQCycleDaysMask.Items[6] as DayViewItem).OriginalValue = (MCB_EQCycleDaysMask.Items[6] as DayViewItem).IsSelected = (activeBattView.Config.EQdaysMask & 0x01) != 0;
            (MCB_EQCycleDaysMask.Items[0] as DayViewItem).OriginalValue = (MCB_EQCycleDaysMask.Items[0] as DayViewItem).IsSelected = (activeBattView.Config.EQdaysMask & 0x02) != 0;
            (MCB_EQCycleDaysMask.Items[1] as DayViewItem).OriginalValue = (MCB_EQCycleDaysMask.Items[1] as DayViewItem).IsSelected = (activeBattView.Config.EQdaysMask & 0x04) != 0;
            (MCB_EQCycleDaysMask.Items[2] as DayViewItem).OriginalValue = (MCB_EQCycleDaysMask.Items[2] as DayViewItem).IsSelected = (activeBattView.Config.EQdaysMask & 0x08) != 0;
            (MCB_EQCycleDaysMask.Items[3] as DayViewItem).OriginalValue = (MCB_EQCycleDaysMask.Items[3] as DayViewItem).IsSelected = (activeBattView.Config.EQdaysMask & 0x10) != 0;
            (MCB_EQCycleDaysMask.Items[4] as DayViewItem).OriginalValue = (MCB_EQCycleDaysMask.Items[4] as DayViewItem).IsSelected = (activeBattView.Config.EQdaysMask & 0x20) != 0;
            (MCB_EQCycleDaysMask.Items[5] as DayViewItem).OriginalValue = (MCB_EQCycleDaysMask.Items[5] as DayViewItem).IsSelected = (activeBattView.Config.EQdaysMask & 0x40) != 0;

            if (activeBattView.FirmwareRevision > 2.09f)
            {
                Batt_EQBlockedDaysList.Items = new List<object>();
                Batt_EQBlockedDaysList.Items.AddRange(new object[] {
            "0",
            "5",
            "6",
            "7",
            "8",
            "9"});


                Batt_EQBlockedDaysList.IsVisible = true;
                Batt_finishBlockedDaysList.IsVisible = true;

                if (activeBattView.Config.blockedEQDays % 24 == 0)
                {
                    Batt_EQBlockedDaysList.SelectedItem = (activeBattView.Config.blockedEQDays / 24).ToString();
                }
                else if (activeBattView.Config.blockedEQDays % 24 > 9)
                {
                    Batt_EQBlockedDaysList.SelectedItem = (activeBattView.Config.blockedEQDays / 24).ToString() + ":" + (activeBattView.Config.blockedEQDays % 24).ToString();
                }
                else
                {
                    Batt_EQBlockedDaysList.SelectedItem = (activeBattView.Config.blockedEQDays / 24).ToString() + ":0" + (activeBattView.Config.blockedEQDays % 24).ToString();
                }
                Batt_EQBlockedDaysList.SelectedIndex = Batt_EQBlockedDaysList.Items.FindIndex(o => ((string)o).Equals(Batt_EQBlockedDaysList.SelectedItem));

                Batt_finishBlockedDaysList.Items = new List<object>();
                Batt_finishBlockedDaysList.Items.AddRange(new object[] {
            "0",
            "5",
            "6",
            "7",
            "8",
            "9"});
                if (activeBattView.Config.blockedFIDays % 24 == 0)
                {
                    Batt_finishBlockedDaysList.SelectedItem = (activeBattView.Config.blockedFIDays / 24).ToString();

                }
                else if (activeBattView.Config.blockedFIDays % 24 > 9)
                {
                    Batt_finishBlockedDaysList.SelectedItem = (activeBattView.Config.blockedFIDays / 24).ToString() + ":" + (activeBattView.Config.blockedEQDays % 24).ToString();
                }
                else
                {
                    Batt_finishBlockedDaysList.SelectedItem = (activeBattView.Config.blockedFIDays / 24).ToString() + ":0" + (activeBattView.Config.blockedEQDays % 24).ToString();
                }
                Batt_finishBlockedDaysList.SelectedIndex = Batt_finishBlockedDaysList.Items.FindIndex(o => ((string)o).Equals(Batt_finishBlockedDaysList.SelectedItem));
            }
            else
            {
                Batt_EQBlockedDaysList.IsVisible = false;
                Batt_finishBlockedDaysList.IsVisible = false;
            }
            SetInfoText(false);

        }

        private int BattViewFinishEQSchedulingAccessApply()
        {
            UIAccessControlUtility accessControlUtility = new UIAccessControlUtility();

            if (!accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_FI_sched_Settings, MCB_FinishSettingsListTOSAVE, Settings_Section))
            {
                //Settings_Section.IsSectionVisible = false;
            }

            //if (ControlObject.user_access.Batt_FI_EQ_sched_CustomSettings == access_level.noAccess)
            //{
            //    EQ_Cycle_Section.IsSectionVisible = false;
            //}
            //if (ControlObject.user_access.Batt_FI_sched_CustomSettings == access_level.noAccess)
            //{
            //    Finish_Cycle_Section.IsSectionVisible = false;
            //}

            //equalize cycle
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_FI_EQ_sched_CustomSettings, MCB_EQCycleStartTimeTextBox, EQ_Cycle_Section);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_FI_EQ_sched_CustomSettings, MCB_EQCycleDurationList, EQ_Cycle_Section);
            EQ_Cycle_Section.Add(Batt_EQBlockedDaysList);
            Batt_EQBlockedDaysList.IsEditEnabled = true;
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_FI_EQ_sched_CustomSettings, MCB_EQCycleDaysMask, EQ_Cycle_Section);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_FI_EQ_sched_CustomSettings, MCB_EQCycleInfoTextBox, EQ_Cycle_Section);
            MCB_EQCycleInfoTextBox.IsEditEnabled = false;

            //finish cycle
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_FI_sched_CustomSettings, MCB_FinishCycleStartTimeTextBox, Finish_Cycle_Section);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_FI_sched_CustomSettings, MCB_FinishCycleDurationList, Finish_Cycle_Section);
            Finish_Cycle_Section.Add(Batt_finishBlockedDaysList);
            Batt_finishBlockedDaysList.IsEditEnabled = true;
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_FI_sched_CustomSettings, MCB_FinishCycleDaysMask, Finish_Cycle_Section);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_FI_sched_CustomSettings, MCB_FinishCycleInfoTextBox, Finish_Cycle_Section);
            MCB_FinishCycleInfoTextBox.IsEditEnabled = false;

            if (BattViewQuantum.Instance.GetBATTView().Config.chargerType == 1)//Conventional 
            {
                MCB_FinishSettingsListTOSAVE.IsEditEnabled = true;
            }
            else
            {
                MCB_FinishSettingsListTOSAVE.IsEditEnabled = false;
            }

            if (accessControlUtility.GetSavedCount() == 0)
            {
                ShowEdit = false;
            }
            return accessControlUtility.GetVisibleCount();
        }

        void Batt_SaveIntoFI_EQ_Sched()
        {
            UInt32 window;
            if (!BattViewQuantum.Instance.BattView_quickSerialNumberCheckStrict())
                return;
            BattViewObject activeBattView = BattViewQuantum.Instance.GetBATTView();
            Match match;

            if (MCB_FinishSettingsListTOSAVE.SelectedIndex == 0)
            {
                activeBattView.Config.FIstartWindow = 0;
                activeBattView.Config.FIcloseWindow = 86400;
                activeBattView.Config.FIdaysMask = 0x7F;
            }
            else
            {

                match = Regex.Match(MCB_FinishCycleStartTimeTextBox.Text, @"^(\d{1,2}){1}:(\d{1,2}){1}$", RegexOptions.IgnoreCase);
                activeBattView.Config.FIstartWindow = UInt32.Parse(match.Groups[1].Value) * 3600 + UInt32.Parse(match.Groups[2].Value) * 60;

                match = Regex.Match(MCB_FinishCycleDurationList.Text, @"^(\d{1,2}){1}:(\d{1,2}){1}$", RegexOptions.IgnoreCase);

                window = UInt32.Parse(match.Groups[1].Value) * 3600 + UInt32.Parse(match.Groups[2].Value) * 60;
                if (window == 86400 && activeBattView.Config.FIstartWindow != 0)
                    window--;
                activeBattView.Config.FIcloseWindow = activeBattView.Config.FIstartWindow + window;

                if (activeBattView.Config.FIcloseWindow != 86400)
                    activeBattView.Config.FIcloseWindow %= 86400;

                activeBattView.Config.FIdaysMask = 0;
                activeBattView.Config.FIdaysMask |= (byte)((MCB_FinishCycleDaysMask.Items[6] as DayViewItem).IsSelected ? 0x01 : 0x00);
                activeBattView.Config.FIdaysMask |= (byte)((MCB_FinishCycleDaysMask.Items[0] as DayViewItem).IsSelected ? 0x02 : 0x00);
                activeBattView.Config.FIdaysMask |= (byte)((MCB_FinishCycleDaysMask.Items[1] as DayViewItem).IsSelected ? 0x04 : 0x00);
                activeBattView.Config.FIdaysMask |= (byte)((MCB_FinishCycleDaysMask.Items[2] as DayViewItem).IsSelected ? 0x08 : 0x00);
                activeBattView.Config.FIdaysMask |= (byte)((MCB_FinishCycleDaysMask.Items[3] as DayViewItem).IsSelected ? 0x10 : 0x00);
                activeBattView.Config.FIdaysMask |= (byte)((MCB_FinishCycleDaysMask.Items[4] as DayViewItem).IsSelected ? 0x20 : 0x00);
                activeBattView.Config.FIdaysMask |= (byte)((MCB_FinishCycleDaysMask.Items[5] as DayViewItem).IsSelected ? 0x40 : 0x00);
            }


            match = Regex.Match(MCB_EQCycleStartTimeTextBox.Text, @"^(\d{1,2}){1}:(\d{1,2}){1}$", RegexOptions.IgnoreCase);
            activeBattView.Config.EQstartWindow = UInt32.Parse(match.Groups[1].Value) * 3600 + UInt32.Parse(match.Groups[2].Value) * 60;

            match = Regex.Match(MCB_EQCycleDurationList.Text, @"^(\d{1,2}){1}:(\d{1,2}){1}$", RegexOptions.IgnoreCase);

            window = UInt32.Parse(match.Groups[1].Value) * 3600 + UInt32.Parse(match.Groups[2].Value) * 60;
            if (window == 86400 && activeBattView.Config.EQstartWindow != 0)
                window--;
            activeBattView.Config.EQcloseWindow = activeBattView.Config.EQstartWindow + window;
            if (activeBattView.Config.EQcloseWindow != 86400)
                activeBattView.Config.EQcloseWindow %= 86400;

            activeBattView.Config.EQdaysMask = 0;
            activeBattView.Config.EQdaysMask |= (byte)((MCB_EQCycleDaysMask.Items[6] as DayViewItem).IsSelected ? 0x01 : 0x00);
            activeBattView.Config.EQdaysMask |= (byte)((MCB_EQCycleDaysMask.Items[0] as DayViewItem).IsSelected ? 0x02 : 0x00);
            activeBattView.Config.EQdaysMask |= (byte)((MCB_EQCycleDaysMask.Items[1] as DayViewItem).IsSelected ? 0x04 : 0x00);
            activeBattView.Config.EQdaysMask |= (byte)((MCB_EQCycleDaysMask.Items[2] as DayViewItem).IsSelected ? 0x08 : 0x00);
            activeBattView.Config.EQdaysMask |= (byte)((MCB_EQCycleDaysMask.Items[3] as DayViewItem).IsSelected ? 0x10 : 0x00);
            activeBattView.Config.EQdaysMask |= (byte)((MCB_EQCycleDaysMask.Items[4] as DayViewItem).IsSelected ? 0x20 : 0x00);
            activeBattView.Config.EQdaysMask |= (byte)((MCB_EQCycleDaysMask.Items[5] as DayViewItem).IsSelected ? 0x40 : 0x00);

            activeBattView.Config.blockedFIDays = 0;
            activeBattView.Config.blockedEQDays = 0;
            if (activeBattView.FirmwareRevision > 2.09f)
            {
                string tempS = (string)Batt_finishBlockedDaysList.Text;
                string[] tempss = tempS.Split(new char[] { ':' });
                byte tempB = 0;
                if (tempss.Length > 0)
                {
                    if (byte.TryParse(tempss[0], out tempB))
                    {
                        activeBattView.Config.blockedFIDays = (byte)(tempB * 24);

                    }
                    if (tempss.Length > 1 && byte.TryParse(tempss[1], out tempB))
                    {
                        activeBattView.Config.blockedFIDays += tempB;
                    }

                }


                tempS = (string)Batt_EQBlockedDaysList.Text;
                tempss = tempS.Split(new char[] { ':' });
                tempB = 0;
                if (tempss.Length > 0)
                {
                    if (byte.TryParse(tempss[0], out tempB))
                    {
                        activeBattView.Config.blockedEQDays = (byte)(tempB * 24);

                    }
                    if (tempss.Length > 1 && byte.TryParse(tempss[1], out tempB))
                    {
                        activeBattView.Config.blockedEQDays += tempB;
                    }

                }
            }

        }
        bool Batt_VerfiyFI_EQ_Sched()
        {
            if (!BattViewQuantum.Instance.BattView_quickSerialNumberCheckStrict())
                return false;
            verifyControl = new VerifyControl();
            Match match;

            if (MCB_FinishSettingsListTOSAVE.SelectedIndex != 0)
            {
                match = Regex.Match(MCB_FinishCycleStartTimeTextBox.Text, @"^(\d{1,2}){1}:(\d{1,2}){1}$", RegexOptions.IgnoreCase);
                if (!match.Success || byte.Parse(match.Groups[1].Value) >= 24 || byte.Parse(match.Groups[2].Value) >= 60)
                    verifyControl.InsertRemoveFault(true, MCB_FinishCycleStartTimeTextBox, Finish_Cycle_Section);
                else
                {
                    UInt32 tempLong = UInt32.Parse(match.Groups[1].Value) * 3600 + UInt32.Parse(match.Groups[2].Value) * 60;

                    if (tempLong > 86400)
                        verifyControl.InsertRemoveFault(true, MCB_FinishCycleStartTimeTextBox, Finish_Cycle_Section);
                    else
                        verifyControl.InsertRemoveFault(false, MCB_FinishCycleStartTimeTextBox, Finish_Cycle_Section);
                }
                verifyControl.VerifyComboBox(MCB_FinishCycleDurationList, Finish_Cycle_Section);
            }


            match = Regex.Match(MCB_EQCycleStartTimeTextBox.Text, @"^(\d{1,2}){1}:(\d{1,2}){1}$", RegexOptions.IgnoreCase);
            if (!match.Success || byte.Parse(match.Groups[1].Value) >= 24 || byte.Parse(match.Groups[2].Value) >= 60)
                verifyControl.InsertRemoveFault(true, MCB_EQCycleStartTimeTextBox, EQ_Cycle_Section);
            else
            {
                UInt32 tempLong = UInt32.Parse(match.Groups[1].Value) * 3600 + UInt32.Parse(match.Groups[2].Value) * 60;

                if (tempLong > 24 * 3600)
                    verifyControl.InsertRemoveFault(true, MCB_EQCycleStartTimeTextBox, EQ_Cycle_Section);
                else
                    verifyControl.InsertRemoveFault(false, MCB_EQCycleStartTimeTextBox, EQ_Cycle_Section);
            }
            verifyControl.VerifyComboBox(MCB_EQCycleDurationList, MCB_EQCycleDurationList);
            return !verifyControl.HasErrors();
        }

        #endregion

        private void CreateListForCharger()
        {
            this.FinishAndEQSettingsItemSource.Clear();
            //Adding sections here
            Settings_Section = new TableHeaderItem
            {
                SectionHeader = "",
            };
            EQ_Cycle_Section = new TableHeaderItem
            {
                SectionHeader = AppResources.equalize_cycle,
            };

            Finish_Cycle_Section = new TableHeaderItem
            {
                SectionHeader = AppResources.finish_cycle,
            };

            FinishAndEQSettingsItemSource.Add(Settings_Section);
            FinishAndEQSettingsItemSource.Add(EQ_Cycle_Section);

            MCB_FinishSettingsListTOSAVE = new ListViewItem
            {
                Title = AppResources.finish_cycle_settings,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.ListSelector,
                ListSelectorType = ACUtility.ListSelectorType.FinishCycleSettingsType,
                ListSelectionCommand = ListSelectorCommand,
                IsEditable = EditingMode ? true : false,
                IsVisible = true,
                ParentIndex = 0
            };

            MCB_EQCycleStartTimeTextBox = new ListViewItem
            {
                Title = AppResources.starts,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.TimePicker,
                IsEditable = EditingMode ? true : false,
                TextValueChanged = TextValueChanged,
                IsVisible = true,
                ParentIndex = 1
            };
            MCB_EQCycleDurationList = new ListViewItem
            {
                Title = AppResources.duration,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.ListSelector,
                ListSelectorType = ACUtility.ListSelectorType.DurationIntervalTypes,
                ListSelectionCommand = ListSelectorCommand,
                IsEditable = EditingMode ? true : false,
                IsVisible = true,
                TextValueChanged = TextValueChanged,
                ParentIndex = 1
            };

            MCB_EQCycleDaysMask = new ListViewItem
            {
                Title = AppResources.days,
                SubTitle = "",
                DefaultCellType = ACUtility.CellTypes.Days,
                EditableCellType = ACUtility.CellTypes.Days,
                ParentIndex = 1
            };

            AddDays(MCB_EQCycleDaysMask);

            MCB_EQCycleInfoTextBox = new ListViewItem
            {
                Title = "",
                DefaultCellType = ACUtility.CellTypes.LabelText,
                EditableCellType = ACUtility.CellTypes.LabelText,
                IsEditable = false,
                IsVisible = true,
                ParentIndex = 1
            };

            MCB_FinishCycleStartTimeTextBox = new ListViewItem
            {
                Title = AppResources.starts,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.TimePicker,
                IsEditable = EditingMode ? true : false,
                TextValueChanged = TextValueChanged,
                IsVisible = true,
                ParentIndex = 2

            };
            MCB_FinishCycleDurationList = new ListViewItem
            {
                Title = AppResources.duration,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.ListSelector,
                ListSelectorType = ACUtility.ListSelectorType.DurationIntervalTypes,
                IsEditable = EditingMode ? true : false,
                ListSelectionCommand = ListSelectorCommand,
                IsVisible = true,
                TextValueChanged = TextValueChanged,
                ParentIndex = 2
            };

            MCB_FinishCycleDaysMask = new ListViewItem
            {
                Title = AppResources.days,
                SubTitle = "",
                DefaultCellType = ACUtility.CellTypes.Days,
                EditableCellType = ACUtility.CellTypes.Days,
                ParentIndex = 2
            };
            AddDays(MCB_FinishCycleDaysMask);

            MCB_FinishCycleInfoTextBox = new ListViewItem
            {
                Title = "",
                DefaultCellType = ACUtility.CellTypes.LabelText,
                EditableCellType = ACUtility.CellTypes.LabelText,
                IsEditable = false,
                IsVisible = true,
                ParentIndex = 2
            };

        }
        private void CreateListForBattView()
        {
            this.FinishAndEQSettingsItemSource.Clear();
            //Adding sections here
            Settings_Section = new TableHeaderItem
            {
                SectionHeader = "",
            };
            EQ_Cycle_Section = new TableHeaderItem
            {
                SectionHeader = AppResources.equalize_cycle,
            };

            Finish_Cycle_Section = new TableHeaderItem
            {
                SectionHeader = AppResources.finish_cycle,
            };

            FinishAndEQSettingsItemSource.Add(Settings_Section);
            FinishAndEQSettingsItemSource.Add(EQ_Cycle_Section);
            FinishAndEQSettingsItemSource.Add(Finish_Cycle_Section);

            MCB_FinishSettingsListTOSAVE = new ListViewItem
            {
                Title = AppResources.finish_cycle_settings,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.ListSelector,
                ListSelectorType = ACUtility.ListSelectorType.FinishCycleSettingsType,
                ListSelectionCommand = ListSelectorCommand,
                IsEditable = EditingMode ? true : false,
                IsVisible = true,
                ParentIndex = 0
            };

            MCB_EQCycleStartTimeTextBox = new ListViewItem
            {
                Title = AppResources.starts,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.TimePicker,
                IsEditable = EditingMode ? true : false,
                TextValueChanged = TextValueChanged,
                IsVisible = true,
                ParentIndex = 1
            };
            MCB_EQCycleDurationList = new ListViewItem
            {
                Title = AppResources.duration,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.ListSelector,
                ListSelectorType = ACUtility.ListSelectorType.DurationIntervalTypes,
                ListSelectionCommand = ListSelectorCommand,
                IsEditable = EditingMode ? true : false,
                IsVisible = true,
                TextValueChanged = TextValueChanged,
                ParentIndex = 1
            };

            Batt_EQBlockedDaysList = new ListViewItem
            {
                Title = AppResources.eq_frequency_days,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.ListSelector,
                ListSelectionCommand = ListSelectorCommand,
                IsEditable = EditingMode ? true : false,
                IsVisible = true,
                ParentIndex = 1
            };

            MCB_EQCycleDaysMask = new ListViewItem
            {
                Title = AppResources.days,
                SubTitle = "",
                DefaultCellType = ACUtility.CellTypes.Days,
                EditableCellType = ACUtility.CellTypes.Days,
                ParentIndex = 1
            };

            AddDays(MCB_EQCycleDaysMask);

            MCB_EQCycleInfoTextBox = new ListViewItem
            {
                Title = "",
                DefaultCellType = ACUtility.CellTypes.LabelText,
                EditableCellType = ACUtility.CellTypes.LabelText,
                IsEditable = false,
                IsVisible = true,
                ParentIndex = 1
            };

            MCB_FinishCycleStartTimeTextBox = new ListViewItem
            {
                Title = AppResources.starts,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.TimePicker,
                IsEditable = EditingMode ? true : false,
                TextValueChanged = TextValueChanged,
                IsVisible = true,
                ParentIndex = 2

            };
            MCB_FinishCycleDurationList = new ListViewItem
            {
                Title = AppResources.duration,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.ListSelector,
                ListSelectorType = ACUtility.ListSelectorType.DurationIntervalTypes,
                IsEditable = EditingMode ? true : false,
                ListSelectionCommand = ListSelectorCommand,
                IsVisible = true,
                TextValueChanged = TextValueChanged,
                ParentIndex = 2
            };

            Batt_finishBlockedDaysList = new ListViewItem
            {
                Title = AppResources.finish_frequency_days,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.ListSelector,
                ListSelectionCommand = ListSelectorCommand,
                IsEditable = EditingMode ? true : false,
                IsVisible = true,
                ParentIndex = 2
            };
            MCB_FinishCycleDaysMask = new ListViewItem
            {
                Title = AppResources.days,
                SubTitle = "",
                DefaultCellType = ACUtility.CellTypes.Days,
                EditableCellType = ACUtility.CellTypes.Days,
                ParentIndex = 2
            };
            AddDays(MCB_FinishCycleDaysMask);

            MCB_FinishCycleInfoTextBox = new ListViewItem
            {
                Title = "",
                DefaultCellType = ACUtility.CellTypes.LabelText,
                EditableCellType = ACUtility.CellTypes.LabelText,
                IsEditable = false,
                IsVisible = true,
                ParentIndex = 2
            };

        }


        /// <summary>
        /// Creates the list.
        /// </summary>
        private void CreateList()
        {
            device_LoadWarnings = new List<string>();
            //this list will be same for battview and charger


            try
            {
                if (IsBattView)
                {
                    CreateListForBattView();

                    if (BattViewFinishEQSchedulingAccessApply() == 0)
                    {
                        FinishAndEQSettingsItemSource.Clear();
                        ACUserDialogs.ShowAlertWithOkButtonsAction(AppResources.no_data_found, () => { ShowViewModel<FinishAndEQSettingsViewModel>(new { pop = "pop" }); });
                        return;
                    }


                    Batt_loadFI_EQ_Sched();

                    if (FinishAndEQSettingsItemSource.Count > 0)
                    {
                        FinishAndEQSettingsItemSource = new ObservableCollection<TableHeaderItem>(FinishAndEQSettingsItemSource);
                    }
                }
                else
                {
                    CreateListForCharger();

                    if (chargerFinishEQSchedulingAccessApply() == 0)
                    {
                        FinishAndEQSettingsItemSource.Clear();
                        ACUserDialogs.ShowAlertWithOkButtonsAction(AppResources.no_data_found, () => { ShowViewModel<FinishAndEQSettingsViewModel>(new { pop = "pop" }); });
                        return;
                    }

                    MCB_loadFI_EQ_Sched();

                    if (FinishAndEQSettingsItemSource.Count > 0)
                    {
                        FinishAndEQSettingsItemSource = new ObservableCollection<TableHeaderItem>(FinishAndEQSettingsItemSource);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Logger.AddLog(true, "X24" + ex.ToString());
            }

            RaisePropertyChanged(() => FinishAndEQSettingsItemSource);

        }

        void AddDays(ListViewItem days_item)
        {
            days_item.Items = new List<object>();
            days_item.Items.Add(new DayViewItem { Title = AppResources.monday, id = 0, DayButtonClicked = DaysButtonClickCommand });
            days_item.Items.Add(new DayViewItem { Title = AppResources.tuesday, id = 1, DayButtonClicked = DaysButtonClickCommand });
            days_item.Items.Add(new DayViewItem { Title = AppResources.wednesday, id = 2, DayButtonClicked = DaysButtonClickCommand });
            days_item.Items.Add(new DayViewItem { Title = AppResources.thursday, id = 3, DayButtonClicked = DaysButtonClickCommand });
            days_item.Items.Add(new DayViewItem { Title = AppResources.friday, id = 4, DayButtonClicked = DaysButtonClickCommand });
            days_item.Items.Add(new DayViewItem { Title = AppResources.saturday, id = 5, DayButtonClicked = DaysButtonClickCommand });
            days_item.Items.Add(new DayViewItem { Title = AppResources.sunday, id = 6, DayButtonClicked = DaysButtonClickCommand });

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
