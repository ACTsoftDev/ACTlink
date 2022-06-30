using System;
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
    public class SaveDefaultChargeProfileViewModel : BaseViewModel
    {
        MvxSubscriptionToken _mListSelector;
        List<string> device_LoadWarnings;

        ListViewItem Batt_trCurrentRateComboBox; //ComboBox
        ListViewItem Batt_ccCurrentRateComboBox; //ComboBox
        ListViewItem Batt_fiCurrentRateComboBox; //ComboBox
        ListViewItem Batt_eqCurrentRateComboBox; //ComboBox
        ListViewItem Batt_trickleVoltageTextBox; //Textbox
        ListViewItem Batt_cvVoltageTextBox; //Textbox
        ListViewItem Batt_finishVoltageTextBox; //Textbox
        ListViewItem Batt_equalizeVoltageTextBox; //Textbox
        ListViewItem Batt_cvFinishCurrentRateComboBox; //ComboBox
        ListViewItem Batt_cvCurrentStepComboBox; //ComboBox
        ListViewItem Batt_cvTimerComboBox; //ComboBox
        ListViewItem Batt_finishTimerComboBox; //ComboBox
        ListViewItem Batt_equalizeTimerComboBox; //ComboBox
        ListViewItem Batt_DesulfationTimerComboBox;//ComboBox
        ListViewItem Batt_finishDVVoltageComboBox;//ComboBox
        ListViewItem Batt_finishDTVoltageComboBox;//ComboBox
        ListViewItem MCB_ForceFinishDurationDisableRadio;// Switch


        VerifyControl verifyControl;

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

        private ObservableCollection<ListViewItem> _defaultChargeProfileItemSource;
        string keepSubValue;
        public ObservableCollection<ListViewItem> DefaultChargeProfileItemSource
        {
            get { return _defaultChargeProfileItemSource; }
            set
            {
                _defaultChargeProfileItemSource = value;
                RaisePropertyChanged(() => DefaultChargeProfileItemSource);
            }
        }
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

        /// <summary>
        /// The load default title.
        /// </summary>
        private string _loadDefaultTitle;
        public string LoadDefaultTitle
        {
            get { return _loadDefaultTitle; }
            set
            {
                _loadDefaultTitle = value;
                RaisePropertyChanged(() => LoadDefaultTitle);
            }
        }

        /// <summary>
        /// is load default enable.
        /// </summary>
        private bool _isLoadDefaultEnable;
        public bool IsLoadDefaultEnable
        {
            get
            {
                return _isLoadDefaultEnable;

            }
            set
            {
                _isLoadDefaultEnable = value;
                RaisePropertyChanged(() => IsLoadDefaultEnable);
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

            if (IsBattView)
            {
                if (Batt_compareWithDefaultChargeProfile())
                {
                    IsLoadDefaultEnable = false;
                }
                else
                {
                    IsLoadDefaultEnable = true;
                }
            }
            else
            {
                IsLoadDefaultEnable = true;
            }

            RaisePropertyChanged(() => DefaultChargeProfileItemSource);
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
                if (IsBattView)
                {
                    if (Batt_VerfiyChargeProfile())
                    {
                        ACUserDialogs.ShowProgress();
                        BattViewCommunicationTypes caller = BattViewCommunicationTypes.NOCall;
                        bool arg1 = false;
                        try
                        {
                            Batt_SaveIntoChargeProfile();
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
                                    IsLoadDefaultEnable = false;
                                    CreateList();

                                    try
                                    {
                                        ResetOldData();
                                        Batt_loadChargeProfile();
                                        RaisePropertyChanged(() => DefaultChargeProfileItemSource);
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
                        //TODO Show Correct Errors Alert
                        ACUserDialogs.ShowAlert(AppResources.alert_enter_valid + " " + verifyControl.GetErrorString());
                    }
                }
                else
                {
                    if (MCB_VerfiyChargeProfile())
                    {
                        ACUserDialogs.ShowProgress();
                        McbCommunicationTypes caller = McbCommunicationTypes.NOCall;
                        bool arg1 = false;
                        try
                        {
                            MCB_SaveIntoChargeProfile();
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
                                    IsLoadDefaultEnable = false;
                                    CreateList();

                                    try
                                    {
                                        ResetOldData();
                                        MCB_loadChargeProfile();
                                        RaisePropertyChanged(() => DefaultChargeProfileItemSource);
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
        }

        void ResetOldData()
        {
            foreach (var item in DefaultChargeProfileItemSource)
            {
                item.SubTitle = string.Empty;
            }
        }

        public IMvxCommand BackBtnClickCommand
        {
            get { return new MvxCommand(OnBackClick); }
        }

        /// <summary>
        /// Gets the cancel button click command.
        /// </summary>
        /// <value>The cancel button click command.</value>
        public IMvxCommand CancelBtnClickCommand
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
            CreateList(true);
            IsLoadDefaultEnable = false;
            RaisePropertyChanged(() => DefaultChargeProfileItemSource);
        }

        private bool CheckForEditedChanges()
        {
            bool textChanged = false;
            foreach (var item in DefaultChargeProfileItemSource)
            {
                if (item.Text != item.SubTitle)
                {
                    textChanged = true;
                }
            }

            return textChanged;
        }

        public SaveDefaultChargeProfileViewModel()
        {
            ViewTitle = AppResources.default_charge_profile;
            device_LoadWarnings = new List<string>();
            DefaultChargeProfileItemSource = new ObservableCollection<ListViewItem>();
            ShowEdit = true;
            IsLoadDefaultEnable = false;
            LoadDefaultTitle = "Load Default";

            if (IsBattView)
                createDataForBATTView();
            else
                createDataForCharger();
        }

        void CreateList(bool resetTextValue = false)
        {
            foreach (var item in DefaultChargeProfileItemSource)
            {
                if (item.IsEditEnabled)
                {
                    item.IsEditable = EditingMode;
                    if (resetTextValue)
                    {
                        item.Text = item.SubTitle;
                        if (item.EditableCellType == ACUtility.CellTypes.LabelSwitch)
                        {
                            item.IsSwitchEnabled = (item.Text == "Yes");
                        }
                    }
                }
            }
        }

        void createDataForBATTView()
        {
            Batt_trCurrentRateComboBox = new ListViewItem()
            {
                Index = 0,
                Title = AppResources.tr_current_rate,
                IsEditable = EditingMode,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.ListSelector,
                ListSelectionCommand = ListSelectorCommand,
                ListSelectorType = ACUtility.ListSelectorType.DefaultCharge_Tr_Amps
            };
            Batt_ccCurrentRateComboBox = new ListViewItem()
            {
                Index = 1,
                Title = AppResources.cc_current_rate,
                IsEditable = EditingMode,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.ListSelector,
                ListSelectionCommand = ListSelectorCommand,
                ListSelectorType = ACUtility.ListSelectorType.DefaultCharge_Cc_Amps
            };
            Batt_fiCurrentRateComboBox = new ListViewItem()
            {
                Index = 2,
                Title = AppResources.fi_current_rate,
                IsEditable = EditingMode,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.ListSelector,
                ListSelectionCommand = ListSelectorCommand,
                ListSelectorType = ACUtility.ListSelectorType.DefaultCharge_Fi_Amps
            };
            Batt_eqCurrentRateComboBox = new ListViewItem()
            {
                Index = 3,
                Title = AppResources.eq_current_rate,
                IsEditable = EditingMode,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.ListSelector,
                ListSelectionCommand = ListSelectorCommand,
                ListSelectorType = ACUtility.ListSelectorType.DefaultCharge_Eq_Amps
            };
            Batt_trickleVoltageTextBox = new ListViewItem()
            {
                Index = 4,
                Title = AppResources.tr_target_voltage,
                IsEditable = EditingMode,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.LabelTextEdit,
                TextInputType = ACUtility.InputType.Decimal
            };
            Batt_cvVoltageTextBox = new ListViewItem()
            {
                Index = 5,
                Title = AppResources.cv_target_voltage,
                IsEditable = EditingMode,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.LabelTextEdit,
                TextInputType = ACUtility.InputType.Decimal
            };
            Batt_finishVoltageTextBox = new ListViewItem()
            {
                Index = 6,
                Title = AppResources.fi_target_voltage,
                IsEditable = EditingMode,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.LabelTextEdit,
                TextInputType = ACUtility.InputType.Decimal
            };
            Batt_equalizeVoltageTextBox = new ListViewItem()
            {
                Index = 7,
                Title = AppResources.eq_target_voltage,
                IsEditable = EditingMode,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.LabelTextEdit,
                TextInputType = ACUtility.InputType.Decimal
            };
            //Todo: Partial charge stop is loaded with dummy values, It needs to be changed from windows code, Delayed as it is not identifiable in windows code.
            Batt_cvFinishCurrentRateComboBox = new ListViewItem()
            {
                Index = 8,
                Title = AppResources.partial_charge_stop,
                IsEditable = EditingMode,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.ListSelector,
                ListSelectionCommand = ListSelectorCommand,
                ListSelectorType = ACUtility.ListSelectorType.DefaultCharge_PartialChargeStop
            };
            Batt_cvCurrentStepComboBox = new ListViewItem()
            {
                Index = 9,
                Title = AppResources.cv_current_step,
                IsEditable = EditingMode,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.ListSelector,
                ListSelectionCommand = ListSelectorCommand,
                ListSelectorType = ACUtility.ListSelectorType.DefaultCharge_Cv_Current,
                KeepIndex = 1
            };
            Batt_cvTimerComboBox = new ListViewItem()
            {
                Index = 10,
                Title = AppResources.cv_timeout,
                IsEditable = EditingMode,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.ListSelector,
                ListSelectionCommand = ListSelectorCommand,
                ListSelectorType = ACUtility.ListSelectorType.DefaultCharge_Cv_Timer

            };
            Batt_finishTimerComboBox = new ListViewItem()
            {
                Index = 11,
                Title = AppResources.finish_timeout,
                IsEditable = EditingMode,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.ListSelector,
                ListSelectionCommand = ListSelectorCommand,
                ListSelectorType = ACUtility.ListSelectorType.DefaultCharge_Finish_Timer
            };
            Batt_equalizeTimerComboBox = new ListViewItem()
            {
                Index = 12,
                Title = AppResources.equilize_timeout,
                IsEditable = EditingMode,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.ListSelector,
                ListSelectionCommand = ListSelectorCommand,
                ListSelectorType = ACUtility.ListSelectorType.DefaultCharge_Eq_Timer
            };

            Batt_DesulfationTimerComboBox = new ListViewItem()
            {
                Index = 13,
                Title = AppResources.desulfation_duration,
                IsEditable = EditingMode,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.ListSelector,
                ListSelectionCommand = ListSelectorCommand,
                ListSelectorType = ACUtility.ListSelectorType.DefaultCharge_Desulfate_Timer
            };
            Batt_finishDVVoltageComboBox = new ListViewItem()
            {
                Index = 14,
                Title = AppResources.finish_dv_voltage,
                IsEditable = EditingMode,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.ListSelector,
                ListSelectionCommand = ListSelectorCommand,
                ListSelectorType = ACUtility.ListSelectorType.DefaultCharge_Finish_Dv_Voltage
            };
            Batt_finishDTVoltageComboBox = new ListViewItem()
            {
                Index = 15,
                Title = AppResources.finish_dt_time,
                IsEditable = EditingMode,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.ListSelector,
                ListSelectionCommand = ListSelectorCommand,
                ListSelectorType = ACUtility.ListSelectorType.DefaultCharge_Dt_Time
            };


            try
            {
                Batt_loadChargeProfile();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Logger.AddLog(true, "X26" + ex.ToString());
            }


            if (BattViewBatteryChargeprofileAccessApply() == 0)
            {
                DefaultChargeProfileItemSource.Clear();
                ACUserDialogs.ShowAlertWithOkButtonsAction(AppResources.no_data_found, () => { ShowViewModel<DefaultChargeProfileViewModel>(new { pop = "pop" }); });
                return;
            }

            if (DefaultChargeProfileItemSource.Count > 0)
            {
                DefaultChargeProfileItemSource = new ObservableCollection<ListViewItem>(DefaultChargeProfileItemSource.OrderBy(o => o.Index));
            }
            RaisePropertyChanged(() => DefaultChargeProfileItemSource);

        }

        void createDataForCharger()
        {
            Batt_trCurrentRateComboBox = new ListViewItem()
            {
                Index = 0,
                Title = AppResources.tr_current_rate,
                IsEditable = EditingMode,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.ListSelector,
                ListSelectionCommand = ListSelectorCommand,
            };
            Batt_ccCurrentRateComboBox = new ListViewItem()
            {
                Index = 1,
                Title = AppResources.cc_current_rate,
                IsEditable = EditingMode,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.ListSelector,
                ListSelectionCommand = ListSelectorCommand,
            };
            Batt_fiCurrentRateComboBox = new ListViewItem()
            {
                Index = 2,
                Title = AppResources.fi_current_rate,
                IsEditable = EditingMode,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.ListSelector,
                ListSelectionCommand = ListSelectorCommand,
            };
            Batt_eqCurrentRateComboBox = new ListViewItem()
            {
                Index = 3,
                Title = AppResources.eq_current_rate,
                IsEditable = EditingMode,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.ListSelector,
                ListSelectionCommand = ListSelectorCommand,
            };
            Batt_trickleVoltageTextBox = new ListViewItem()
            {
                Index = 4,
                Title = AppResources.tr_target_voltage,
                IsEditable = EditingMode,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.LabelTextEdit,
                TextInputType = ACUtility.InputType.Decimal
            };
            Batt_cvVoltageTextBox = new ListViewItem()
            {
                Index = 5,
                Title = AppResources.cv_target_voltage,
                IsEditable = EditingMode,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.LabelTextEdit,
                TextInputType = ACUtility.InputType.Decimal
            };
            Batt_finishVoltageTextBox = new ListViewItem()
            {
                Index = 6,
                Title = AppResources.fi_target_voltage,
                IsEditable = EditingMode,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.LabelTextEdit,
                TextInputType = ACUtility.InputType.Decimal
            };
            Batt_equalizeVoltageTextBox = new ListViewItem()
            {
                Index = 7,
                Title = AppResources.eq_target_voltage,
                IsEditable = EditingMode,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.LabelTextEdit,
                TextInputType = ACUtility.InputType.Decimal
            };
            //Todo: Partial charge stop is loaded with dummy values, It needs to be changed from windows code, Delayed as it is not identifiable in windows code.
            Batt_cvFinishCurrentRateComboBox = new ListViewItem()
            {
                Index = 8,
                Title = AppResources.partial_charge_stop,
                IsEditable = EditingMode,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.ListSelector,
                ListSelectionCommand = ListSelectorCommand,
            };
            Batt_cvCurrentStepComboBox = new ListViewItem()
            {
                Index = 9,
                Title = AppResources.cv_current_step,
                IsEditable = EditingMode,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.ListSelector,
                ListSelectionCommand = ListSelectorCommand,
                KeepIndex = 1
            };
            Batt_cvTimerComboBox = new ListViewItem()
            {
                Index = 10,
                Title = AppResources.cv_timeout,
                IsEditable = EditingMode,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.ListSelector,
                ListSelectionCommand = ListSelectorCommand,

            };
            Batt_finishTimerComboBox = new ListViewItem()
            {
                Index = 11,
                Title = AppResources.finish_timeout,
                IsEditable = EditingMode,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.ListSelector,
                ListSelectionCommand = ListSelectorCommand,
            };


            MCB_ForceFinishDurationDisableRadio = new ListViewItem()
            {
                Index = 12,
                Title = AppResources.force_finish_timout,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.LabelSwitch,
            };

            Batt_equalizeTimerComboBox = new ListViewItem()
            {
                Index = 13,
                Title = AppResources.equilize_timeout,
                IsEditable = EditingMode,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.ListSelector,
                ListSelectionCommand = ListSelectorCommand,
            };

            Batt_DesulfationTimerComboBox = new ListViewItem()
            {
                Index = 14,
                Title = AppResources.desulfation_duration,
                IsEditable = EditingMode,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.ListSelector,
                ListSelectionCommand = ListSelectorCommand,
            };
            Batt_finishDVVoltageComboBox = new ListViewItem()
            {
                Index = 15,
                Title = AppResources.finish_dv_voltage,
                IsEditable = EditingMode,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.ListSelector,
                ListSelectionCommand = ListSelectorCommand,
            };
            Batt_finishDTVoltageComboBox = new ListViewItem()
            {
                Index = 16,
                Title = AppResources.finish_dt_time,
                IsEditable = EditingMode,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.ListSelector,
                ListSelectionCommand = ListSelectorCommand,
            };


            try
            {
                MCB_loadChargeProfile();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Logger.AddLog(true, "X26" + ex.ToString());
            }


            if (chargerBatteryChargeprofileAccessApply() == 0)
            {
                DefaultChargeProfileItemSource.Clear();
                ACUserDialogs.ShowAlertWithOkButtonsAction(AppResources.no_data_found, () => { ShowViewModel<DefaultChargeProfileViewModel>(new { pop = "pop" }); });
                return;
            }

            if (DefaultChargeProfileItemSource.Count > 0)
            {
                DefaultChargeProfileItemSource = new ObservableCollection<ListViewItem>(DefaultChargeProfileItemSource.OrderBy(o => o.Index));
            }
            RaisePropertyChanged(() => DefaultChargeProfileItemSource);

        }


        /// <summary>
        /// Ons the back button click.
        /// </summary>
        public void OnBackButtonClick()
        {
            ShowViewModel<DefaultChargeProfileViewModel>(new { pop = "pop" });
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
            try
            {
                if (item.CellType == ACUtility.CellTypes.ListSelector)
                {
                    _mListSelector = Mvx.Resolve<IMvxMessenger>().Subscribe<ListSelectorMessage>(OnListSelectorMessage);

                    if (item.KeepIndex != -1)
                    {
                        int enabelItem = item.KeepIndex;
                        foreach (ListViewItem _item in DefaultChargeProfileItemSource)
                        {
                            if (_item.Index == enabelItem)
                            {
                                keepSubValue = _item.SubTitle;
                            }
                        }
                        ShowViewModel<ListSelectorViewModel>(new { type = item.ListSelectorType, selectedItemIndex = DefaultChargeProfileItemSource.IndexOf(item), ItemSourceStr = JsonConvert.SerializeObject(item.Items), keepSubvalue = keepSubValue });
                    }
                    else
                    {

                        ShowViewModel<ListSelectorViewModel>(new { type = item.ListSelectorType, selectedItemIndex = DefaultChargeProfileItemSource.IndexOf(item), ItemSourceStr = JsonConvert.SerializeObject(item.Items) });
                    }
                }

            }
            catch (Exception ex)
            {

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
                DefaultChargeProfileItemSource[obj.SelectedItemindex].Text = obj.SelectedItem;
                DefaultChargeProfileItemSource[obj.SelectedItemindex].SelectedIndex = obj.SelectedIndex;
                RaisePropertyChanged(() => DefaultChargeProfileItemSource);

            }
        }

        private int BattViewBatteryChargeprofileAccessApply()
        {
            UIAccessControlUtility accessControlUtility = new UIAccessControlUtility();
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_TR_CurrentRate, Batt_trCurrentRateComboBox, DefaultChargeProfileItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_CC_CurrentRate, Batt_ccCurrentRateComboBox, DefaultChargeProfileItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_FI_CurrentRate, Batt_fiCurrentRateComboBox, DefaultChargeProfileItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_EQ_CurrentRate, Batt_eqCurrentRateComboBox, DefaultChargeProfileItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_TrickleVoltage, Batt_trickleVoltageTextBox, DefaultChargeProfileItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_CVVoltage, Batt_cvVoltageTextBox, DefaultChargeProfileItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_finishVoltage, Batt_finishVoltageTextBox, DefaultChargeProfileItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_EqualaizeVoltage, Batt_equalizeVoltageTextBox, DefaultChargeProfileItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_cvCurrentStep, Batt_cvCurrentStepComboBox, DefaultChargeProfileItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_cvFinishCurrent, Batt_cvFinishCurrentRateComboBox, DefaultChargeProfileItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_CVMaxTimer, Batt_cvTimerComboBox, DefaultChargeProfileItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_finishTimer, Batt_finishTimerComboBox, DefaultChargeProfileItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_EqualizeTimer, Batt_equalizeTimerComboBox, DefaultChargeProfileItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_desulfationTimer, Batt_DesulfationTimerComboBox, DefaultChargeProfileItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_finishdVdT, Batt_finishDTVoltageComboBox, DefaultChargeProfileItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_finishdVdT, Batt_finishDVVoltageComboBox, DefaultChargeProfileItemSource);

            if (accessControlUtility.GetSavedCount() == 0)
            {
                ShowEdit = false;
            }
            return accessControlUtility.GetVisibleCount(); ;
        }

        void Batt_loadChargeProfile()
        {
            if (!BattViewQuantum.Instance.BattView_quickSerialNumberCheckStrict())
                return;


            //To fill the ComboBox Items from the loop
            //Batt_trCurrentRateComboBox.Items = new List<object>();
            //for (float i = ControlObject.FormLimits.trCurrentRate_min; i < ControlObject.FormLimits.trCurrentRate_max; i++)
            //{
            //    Batt_trCurrentRateComboBox.Items.Add(Batt_FormatRate(i, 1));
            //}


            Batt_trCurrentRateComboBox.SelectedItem = Batt_FormatRate((BattViewQuantum.Instance.GetBATTView().Config.trickleCurrentRate / 100.0f), 1);
            //Batt_trCurrentRateComboBox.SelectedIndex = Batt_trCurrentRateComboBox.Items.FindIndex(o => ((string)o).Equals(Batt_trCurrentRateComboBox.SelectedItem));
            if (Batt_trCurrentRateComboBox.SelectedIndex == -1)
            {
                device_LoadWarnings.Add(AppResources.invalid_tr_rate_value);
            }


            //To fill the ComboBox Items from the loop
            //Batt_ccCurrentRateComboBox.Items = new List<object>();
            //for (float i = ControlObject.FormLimits.ccCurrentRate_min; i <= ControlObject.FormLimits.ccCurrentRate_max; i++)
            //{
            //    Batt_ccCurrentRateComboBox.Items.Add(Batt_FormatRate(i, 1));
            //}


            Batt_ccCurrentRateComboBox.SelectedItem = Batt_FormatRate((BattViewQuantum.Instance.GetBATTView().Config.CCrate / 100.0f), 1);
            //Batt_ccCurrentRateComboBox.SelectedIndex = Batt_ccCurrentRateComboBox.Items.FindIndex(o => ((string)o).Equals(Batt_ccCurrentRateComboBox.SelectedItem));
            if (Batt_ccCurrentRateComboBox.SelectedIndex == -1)
            {
                device_LoadWarnings.Add(AppResources.invalid_cc_rate_value);
            }


            //To fill the ComboBox Items from the loop
            //Batt_fiCurrentRateComboBox.Items = new List<object>();
            //for (float i = ControlObject.FormLimits.fiCurrentRate_min; i < ControlObject.FormLimits.fiCurrentRate_max; i++)
            //{
            //    Batt_fiCurrentRateComboBox.Items.Add(Batt_FormatRate(i, 1));
            //}

            Batt_fiCurrentRateComboBox.SelectedItem = Batt_FormatRate((BattViewQuantum.Instance.GetBATTView().Config.FIcurrentRate / 100.0f), 1);
            //Batt_fiCurrentRateComboBox.SelectedIndex = Batt_fiCurrentRateComboBox.Items.FindIndex(o => ((string)o).Equals(Batt_fiCurrentRateComboBox.SelectedItem));
            if (Batt_fiCurrentRateComboBox.SelectedIndex == -1)
            {
                device_LoadWarnings.Add(AppResources.invalid_finish_rate_value);
            }


            //To fill the ComboBox Items from the loop
            //Batt_eqCurrentRateComboBox.Items = new List<object>();
            //for (float i = ControlObject.FormLimits.eqCurrentRate_min; i < ControlObject.FormLimits.eqCurrentRate_max; i++)
            //{
            //    Batt_eqCurrentRateComboBox.Items.Add(Batt_FormatRate(i, 1));
            //}

            Batt_eqCurrentRateComboBox.SelectedItem = Batt_FormatRate((BattViewQuantum.Instance.GetBATTView().Config.EQcurrentRate / 100.0f), 1);
            //Batt_eqCurrentRateComboBox.SelectedIndex = Batt_eqCurrentRateComboBox.Items.FindIndex(o => ((string)o).Equals(Batt_eqCurrentRateComboBox.SelectedItem));
            if (Batt_eqCurrentRateComboBox.SelectedIndex == -1)
            {
                device_LoadWarnings.Add(AppResources.invalid_eq_rate_value);
            }


            Batt_trickleVoltageTextBox.SubTitle = Batt_trickleVoltageTextBox.Text = (BattViewQuantum.Instance.GetBATTView().Config.trickleVoltage / 100.0f).ToString();
            Batt_cvVoltageTextBox.SubTitle = Batt_cvVoltageTextBox.Text = (BattViewQuantum.Instance.GetBATTView().Config.CVTargetVoltage / 100.0f).ToString();
            Batt_finishVoltageTextBox.SubTitle = Batt_finishVoltageTextBox.Text = (BattViewQuantum.Instance.GetBATTView().Config.FItargetVoltage / 100.0f).ToString();
            Batt_equalizeVoltageTextBox.SubTitle = Batt_equalizeVoltageTextBox.Text = (BattViewQuantum.Instance.GetBATTView().Config.EQvoltage / 100.0f).ToString();


            //To fill the ComboBox Items from the loop
            //Batt_cvFinishCurrentRateComboBox.Items = new List<object>();
            //for (float i = ControlObject.FormLimits.cvFinishCurrentRate_min; i <= ControlObject.FormLimits.cvFinishCurrentRate_max; i += 0.5f)
            //{
            //    Batt_cvFinishCurrentRateComboBox.Items.Add(Batt_FormatRate(i, 1));
            //}

            //Inital battview default charge profile CVCurrentstep
            //todo: remove this if loop if its not forming the data in list 
            if (BattViewQuantum.Instance.GetBATTView().Config.CVcurrentStep == 0)
            {
                Batt_cvCurrentStepComboBox.SelectedItem = "Default";
            }
            //else
            //{
            //  Batt_cvCurrentStepComboBox.SelectedItem = Batt_FormatRate(BATTQuantum.Instance.GetBATTView().config.CVcurrentStep / 2.0f, ccRate);
            //}

            Batt_cvFinishCurrentRateComboBox.SelectedItem = Batt_FormatRate((BattViewQuantum.Instance.GetBATTView().Config.CVendCurrentRate / 2.0f), 1);
            //Batt_cvFinishCurrentRateComboBox.SelectedIndex = Batt_cvFinishCurrentRateComboBox.Items.FindIndex(o => ((string)o).Equals(Batt_cvFinishCurrentRateComboBox.SelectedItem));
            if (Batt_cvFinishCurrentRateComboBox.SelectedIndex == -1)
            {
                device_LoadWarnings.Add(AppResources.invalid_cv_finish_rate_value);
            }

            Batt_CV_CurrentStepLoad(false);

            //cvTimerStart

            Batt_cvTimerComboBox.SelectedItem = String.Format("{0:00}:{1:00}", (BattViewQuantum.Instance.GetBATTView().Config.cvMaxDuration / 3600), (BattViewQuantum.Instance.GetBATTView().Config.cvMaxDuration % 3600) / 60);

            if (Batt_cvTimerComboBox.SelectedIndex == -1)
                device_LoadWarnings.Add(AppResources.invalid_cv_max_duration_value);


            Batt_finishTimerComboBox.SelectedItem = String.Format("{0:00}:{1:00}", (BattViewQuantum.Instance.GetBATTView().Config.FIduration / 3600), (BattViewQuantum.Instance.GetBATTView().Config.FIduration % 3600) / 60);
            if (Batt_finishTimerComboBox.SelectedIndex == -1)
                device_LoadWarnings.Add(AppResources.invalid_finish_duration_value);

            Batt_equalizeTimerComboBox.SelectedItem = String.Format("{0:00}:{1:00}", (BattViewQuantum.Instance.GetBATTView().Config.EQduration / 3600), (BattViewQuantum.Instance.GetBATTView().Config.EQduration % 3600) / 60);
            if (Batt_equalizeTimerComboBox.SelectedIndex == -1)
                device_LoadWarnings.Add(AppResources.invalid_equalize_duration_value);

            Batt_DesulfationTimerComboBox.SelectedItem = String.Format("{0:00}:{1:00}", (BattViewQuantum.Instance.GetBATTView().Config.desulfation / 3600), (BattViewQuantum.Instance.GetBATTView().Config.desulfation % 3600) / 60);
            if (Batt_DesulfationTimerComboBox.SelectedIndex == -1)
                device_LoadWarnings.Add(AppResources.invalid_desulfate_duration_value);

            Batt_finishDVVoltageComboBox.SelectedItem = BattViewQuantum.Instance.GetBATTView().Config.FIdv.ToString();
            if (Batt_finishDVVoltageComboBox.SelectedIndex == -1)
                device_LoadWarnings.Add(AppResources.invalid_finish_dv_value);

            Batt_finishDTVoltageComboBox.SelectedItem = BattViewQuantum.Instance.GetBATTView().Config.FIdt.ToString();
            if (Batt_finishDTVoltageComboBox.SelectedIndex == -1)
                device_LoadWarnings.Add(AppResources.invalid_finish_dt_value);

            Batt_VerfiyChargeProfile();
        }

        string Batt_FormatRate(float percent, float select)
        {
            return percent.ToString() + "% - " + Math.Round(0.01f * percent * BattViewQuantum.Instance.GetBATTView().Config.ahrcapacity * select).ToString() + "A";
        }


        void Batt_CV_CurrentStepLoad(bool keepIndex)
        {
            if (!BattViewQuantum.Instance.BattView_quickSerialNumberCheckStrict())
                return;
            //get CC rate
            if (Batt_ccCurrentRateComboBox.SelectedIndex == -1)
                return;
            int selectedIndex = 0;
            if (keepIndex)
                selectedIndex = Batt_cvCurrentStepComboBox.SelectedIndex;
            float ccRate = Batt_getValueFromRates((string)Batt_ccCurrentRateComboBox.Text);
            ccRate /= 100;

            //To fill the ComboBox Items from the loop
            Batt_cvCurrentStepComboBox.Items = new List<object>();
            Batt_cvCurrentStepComboBox.Items.Add("Default");
            for (float i = ControlObject.FormLimits.cvCurrentStep_min; i <= ControlObject.FormLimits.cvCurrentStep_max; i += 0.5f)
            {
                Batt_cvCurrentStepComboBox.Items.Add(Batt_FormatRate(i, ccRate));
            }

            if (keepIndex)
            {
                Batt_cvCurrentStepComboBox.SelectedIndex = selectedIndex;
                return;
            }
            if (BattViewQuantum.Instance.GetBATTView().Config.CVcurrentStep == 0)
            {
                Batt_cvCurrentStepComboBox.SelectedItem = "Default";
            }
            else
            {
                Batt_cvCurrentStepComboBox.SelectedItem = Batt_FormatRate(BattViewQuantum.Instance.GetBATTView().Config.CVcurrentStep / 2.0f, ccRate);
            }
            if (Batt_cvCurrentStepComboBox.SelectedIndex == -1)
            {
                device_LoadWarnings.Add(AppResources.invalid_cv_current_step_rate_value);
            }
        }

        float Batt_getValueFromRates(string value)
        {
            if (!BattViewQuantum.Instance.BattView_quickSerialNumberCheckStrict())
                return 1;
            return float.Parse(value.Split(new char[] { '%' })[0]);

        }

        bool Batt_VerfiyChargeProfile()
        {
            if (!BattViewQuantum.Instance.BattView_quickSerialNumberCheckStrict())
                return false;

            //if (Batt_compareWithDefaultChargeProfile())
            //{
            //    //Batt_ChargeProfileDefaultButton.Hide();
            //    IsLoadDefaultEnable = false;
            //}
            //else
            //{
            //    //Batt_ChargeProfileDefaultButton.Show();
            //    IsLoadDefaultEnable = true;
            //}

            verifyControl = new VerifyControl();
            verifyControl.VerifyComboBox(Batt_trCurrentRateComboBox);
            verifyControl.VerifyComboBox(Batt_ccCurrentRateComboBox);
            verifyControl.VerifyComboBox(Batt_fiCurrentRateComboBox);
            verifyControl.VerifyComboBox(Batt_eqCurrentRateComboBox);
            verifyControl.VerifyFloatNumber(Batt_trickleVoltageTextBox, Batt_trickleVoltageTextBox, ControlObject.FormLimits.trVoltage_min, ControlObject.FormLimits.trVoltage_max);
            verifyControl.VerifyFloatNumber(Batt_cvVoltageTextBox, Batt_cvVoltageTextBox, ControlObject.FormLimits.cvVoltage_min, ControlObject.FormLimits.cvVoltage_max);
            verifyControl.VerifyFloatNumber(Batt_finishVoltageTextBox, Batt_finishVoltageTextBox, ControlObject.FormLimits.fiVoltage_min, ControlObject.FormLimits.fiVoltage_max);
            verifyControl.VerifyFloatNumber(Batt_equalizeVoltageTextBox, Batt_equalizeVoltageTextBox, ControlObject.FormLimits.eqVoltage_min, ControlObject.FormLimits.eqVoltage_max);
            verifyControl.VerifyComboBox(Batt_cvFinishCurrentRateComboBox);
            verifyControl.VerifyComboBox(Batt_cvCurrentStepComboBox);
            verifyControl.VerifyComboBox(Batt_cvTimerComboBox);
            verifyControl.VerifyComboBox(Batt_finishTimerComboBox);
            verifyControl.VerifyComboBox(Batt_equalizeTimerComboBox);
            verifyControl.VerifyComboBox(Batt_DesulfationTimerComboBox);
            verifyControl.VerifyComboBox(Batt_finishDVVoltageComboBox);
            verifyControl.VerifyComboBox(Batt_finishDTVoltageComboBox);

            if (!verifyControl.HasErrors())
            {
                if (Batt_getValueFromRates((string)Batt_trCurrentRateComboBox.Text) > Batt_getValueFromRates((string)Batt_ccCurrentRateComboBox.Text))
                    verifyControl.InsertRemoveFault(true, Batt_trCurrentRateComboBox);
                else
                    verifyControl.InsertRemoveFault(false, Batt_trCurrentRateComboBox);

                if (Batt_getValueFromRates((string)Batt_fiCurrentRateComboBox.Text) > Batt_getValueFromRates((string)Batt_ccCurrentRateComboBox.Text))
                    verifyControl.InsertRemoveFault(true, Batt_fiCurrentRateComboBox);
                else
                    verifyControl.InsertRemoveFault(false, Batt_fiCurrentRateComboBox);

                if (Batt_getValueFromRates((string)Batt_eqCurrentRateComboBox.Text) > Batt_getValueFromRates((string)Batt_fiCurrentRateComboBox.Text))
                    verifyControl.InsertRemoveFault(true, Batt_eqCurrentRateComboBox);
                else
                    verifyControl.InsertRemoveFault(false, Batt_eqCurrentRateComboBox);

                if (Batt_getValueFromRates((string)Batt_cvFinishCurrentRateComboBox.SelectedItem) > Batt_getValueFromRates((string)Batt_ccCurrentRateComboBox.SelectedItem))
                    verifyControl.InsertRemoveFault(true, Batt_cvFinishCurrentRateComboBox);
                else
                    verifyControl.InsertRemoveFault(false, Batt_cvFinishCurrentRateComboBox);

                if (float.Parse(Batt_cvVoltageTextBox.Text) < float.Parse(Batt_trickleVoltageTextBox.Text))
                    verifyControl.InsertRemoveFault(true, Batt_cvVoltageTextBox);
                else
                    verifyControl.InsertRemoveFault(false, Batt_cvVoltageTextBox);

                if (float.Parse(Batt_finishVoltageTextBox.Text) < float.Parse(Batt_cvVoltageTextBox.Text))
                    verifyControl.InsertRemoveFault(true, Batt_finishVoltageTextBox);
                else
                    verifyControl.InsertRemoveFault(false, Batt_finishVoltageTextBox);

                if (float.Parse(Batt_equalizeVoltageTextBox.Text) < float.Parse(Batt_finishVoltageTextBox.Text))
                    verifyControl.InsertRemoveFault(true, Batt_equalizeVoltageTextBox);
                else
                    verifyControl.InsertRemoveFault(false, Batt_equalizeVoltageTextBox);
            }


            return !verifyControl.HasErrors();
        }

        bool Batt_compareWithDefaultChargeProfile()
        {
            if (!BattViewQuantum.Instance.BattView_quickSerialNumberCheckStrict())
                return true;

            if (BattViewQuantum.Instance.GetBATTView().Config.trickleCurrentRate != 500 ||
            BattViewQuantum.Instance.GetBATTView().Config.FIcurrentRate != 500 ||
            BattViewQuantum.Instance.GetBATTView().Config.EQcurrentRate != 400 ||
            BattViewQuantum.Instance.GetBATTView().Config.trickleVoltage != 200 ||
            BattViewQuantum.Instance.GetBATTView().Config.FItargetVoltage != 260 ||
            BattViewQuantum.Instance.GetBATTView().Config.EQvoltage != 265 ||
            BattViewQuantum.Instance.GetBATTView().Config.CVendCurrentRate != 24 ||
            BattViewQuantum.Instance.GetBATTView().Config.CVcurrentStep != 0 ||
            BattViewQuantum.Instance.GetBATTView().Config.cvMaxDuration != 14400 ||
            BattViewQuantum.Instance.GetBATTView().Config.FIduration != 10800 ||
            BattViewQuantum.Instance.GetBATTView().Config.EQduration != 14400 ||
            BattViewQuantum.Instance.GetBATTView().Config.desulfation != 43200 ||
            BattViewQuantum.Instance.GetBATTView().Config.FIdv != 5 ||
            BattViewQuantum.Instance.GetBATTView().Config.FIdt != 59 ||
            BattViewQuantum.Instance.GetBATTView().Config.EQcloseWindow != 86400 ||
            BattViewQuantum.Instance.GetBATTView().Config.EQstartWindow != 0 ||
            BattViewQuantum.Instance.GetBATTView().Config.FIstartWindow != 0)
                return false;
            if (BattViewQuantum.Instance.GetBATTView().Config.chargerType == 0)
            {
                //FAST
                if (BattViewQuantum.Instance.GetBATTView().Config.CCrate != 4000 ||
                BattViewQuantum.Instance.GetBATTView().Config.CVTargetVoltage != 242 ||
                    BattViewQuantum.Instance.GetBATTView().Config.FIcloseWindow != 86400 ||
                    BattViewQuantum.Instance.GetBATTView().Config.FIdaysMask != (0x01 | 0x40) ||
                    BattViewQuantum.Instance.GetBATTView().Config.EQdaysMask != (0x01))
                    return false;
            }
            else if (BattViewQuantum.Instance.GetBATTView().Config.chargerType == 1)
            {
                if (BattViewQuantum.Instance.GetBATTView().Config.CCrate != 1700 ||
                BattViewQuantum.Instance.GetBATTView().Config.CVTargetVoltage != 237 ||
                    BattViewQuantum.Instance.GetBATTView().Config.FIcloseWindow != 86400 ||
                    BattViewQuantum.Instance.GetBATTView().Config.FIdaysMask != 0x7f ||
                    BattViewQuantum.Instance.GetBATTView().Config.EQdaysMask != (0x01))
                    return false;
                //Conventional
            }
            else if (BattViewQuantum.Instance.GetBATTView().Config.chargerType == 2)
            {
                //Opp
                if (BattViewQuantum.Instance.GetBATTView().Config.CCrate != 2500 ||
                BattViewQuantum.Instance.GetBATTView().Config.CVTargetVoltage != 240 ||
                    BattViewQuantum.Instance.GetBATTView().Config.FIcloseWindow != 8 * 3600 ||
                    BattViewQuantum.Instance.GetBATTView().Config.FIdaysMask != 0x7f ||
                    BattViewQuantum.Instance.GetBATTView().Config.EQdaysMask != (0x01))
                    return false;
            }

            return true;
        }


        void Batt_SaveIntoChargeProfile()
        {
            if (!BattViewQuantum.Instance.BattView_quickSerialNumberCheckStrict())
                return;
            BattViewQuantum.Instance.SaveBATTViewData();
            BattViewQuantum.Instance.GetBATTView().Config.trickleCurrentRate = (UInt16)Math.Round(100 * Batt_getValueFromRates((string)Batt_trCurrentRateComboBox.Text));
            BattViewQuantum.Instance.GetBATTView().Config.CCrate = (UInt16)Math.Round(100 * Batt_getValueFromRates((string)Batt_ccCurrentRateComboBox.Text));
            BattViewQuantum.Instance.GetBATTView().Config.FIcurrentRate = (UInt16)Math.Round(100 * Batt_getValueFromRates((string)Batt_fiCurrentRateComboBox.Text));
            BattViewQuantum.Instance.GetBATTView().Config.EQcurrentRate = (UInt16)Math.Round(100 * Batt_getValueFromRates((string)Batt_eqCurrentRateComboBox.Text));
            BattViewQuantum.Instance.GetBATTView().Config.trickleVoltage = (UInt16)Math.Round(100.0f * float.Parse(Batt_trickleVoltageTextBox.Text));
            BattViewQuantum.Instance.GetBATTView().Config.CVTargetVoltage = (UInt16)Math.Round(100.0f * float.Parse(Batt_cvVoltageTextBox.Text));
            BattViewQuantum.Instance.GetBATTView().Config.FItargetVoltage = (UInt16)Math.Round(100.0f * float.Parse(Batt_finishVoltageTextBox.Text));
            BattViewQuantum.Instance.GetBATTView().Config.EQvoltage = (UInt16)Math.Round(100.0f * float.Parse(Batt_equalizeVoltageTextBox.Text));
            BattViewQuantum.Instance.GetBATTView().Config.CVendCurrentRate = (byte)Math.Round(2 * Batt_getValueFromRates((string)Batt_cvFinishCurrentRateComboBox.Text));
            if ((string)Batt_cvCurrentStepComboBox.Text == "Default")
                BattViewQuantum.Instance.GetBATTView().Config.CVcurrentStep = 0;
            else
                BattViewQuantum.Instance.GetBATTView().Config.CVcurrentStep = (byte)Math.Round(2 * Batt_getValueFromRates((string)Batt_cvCurrentStepComboBox.Text));

            Match match;
            match = Regex.Match(Batt_cvTimerComboBox.Text, @"^(\d{1,2}){1}:(\d{1,2}){1}$", RegexOptions.IgnoreCase);
            BattViewQuantum.Instance.GetBATTView().Config.cvMaxDuration = UInt32.Parse(match.Groups[1].Value) * 3600 + UInt32.Parse(match.Groups[2].Value) * 60;

            match = Regex.Match(Batt_finishTimerComboBox.Text, @"^(\d{1,2}){1}:(\d{1,2}){1}$", RegexOptions.IgnoreCase);
            BattViewQuantum.Instance.GetBATTView().Config.FIduration = UInt32.Parse(match.Groups[1].Value) * 3600 + UInt32.Parse(match.Groups[2].Value) * 60;

            match = Regex.Match(Batt_equalizeTimerComboBox.Text, @"^(\d{1,2}){1}:(\d{1,2}){1}$", RegexOptions.IgnoreCase);
            BattViewQuantum.Instance.GetBATTView().Config.EQduration = UInt32.Parse(match.Groups[1].Value) * 3600 + UInt32.Parse(match.Groups[2].Value) * 60;

            match = Regex.Match(Batt_DesulfationTimerComboBox.Text, @"^(\d{1,2}){1}:(\d{1,2}){1}$", RegexOptions.IgnoreCase);
            BattViewQuantum.Instance.GetBATTView().Config.desulfation = UInt32.Parse(match.Groups[1].Value) * 3600 + UInt32.Parse(match.Groups[2].Value) * 60;

            BattViewQuantum.Instance.GetBATTView().Config.FIdv = byte.Parse((string)Batt_finishDVVoltageComboBox.Text);
            BattViewQuantum.Instance.GetBATTView().Config.FIdt = byte.Parse((string)Batt_finishDTVoltageComboBox.Text);
        }


        void MCB_loadChargeProfile()
        {
            if (!MCBQuantum.Instance.MCB_quickSerialNumberCheckStrict())
                return;
            Batt_trCurrentRateComboBox.Items = new List<object>();
            for (float i = ControlObject.FormLimits.trCurrentRate_min; i <= ControlObject.FormLimits.trCurrentRate_max; i++)
            {
                Batt_trCurrentRateComboBox.Items.Add(MCB_FormatRate(i, 1));
            }
            Batt_trCurrentRateComboBox.SelectedItem = MCB_FormatRate((MCBQuantum.Instance.GetMCB().Config.TRrate / 100.0f), 1);
            Batt_trCurrentRateComboBox.SelectedIndex = Batt_trCurrentRateComboBox.Items.IndexOf(Batt_trCurrentRateComboBox.Items.FirstOrDefault(o => o.ToString() == Batt_trCurrentRateComboBox.SelectedItem));

            if (Batt_trCurrentRateComboBox.SelectedIndex == -1)
            {
                device_LoadWarnings.Add("Invalid TR Rate Value");
            }
            Batt_ccCurrentRateComboBox.Items = new List<object>();
            for (float i = ControlObject.FormLimits.ccCurrentRate_min; i <= ControlObject.FormLimits.ccCurrentRate_max; i++)
            {
                Batt_ccCurrentRateComboBox.Items.Add(MCB_FormatRate(i, 1));
            }
            Batt_ccCurrentRateComboBox.SelectedItem = MCB_FormatRate((MCBQuantum.Instance.GetMCB().Config.CCrate / 100.0f), 1);
            Batt_ccCurrentRateComboBox.SelectedIndex = Batt_ccCurrentRateComboBox.Items.IndexOf(Batt_ccCurrentRateComboBox.Items.FirstOrDefault(o => o.ToString() == Batt_ccCurrentRateComboBox.SelectedItem));
            //MCB_CV_CurrentStepLoad(true);

            if (Batt_ccCurrentRateComboBox.SelectedIndex == -1)
            {
                device_LoadWarnings.Add("Invalid CC Rate Value");
            }


            //
            Batt_fiCurrentRateComboBox.Items = new List<object>();
            int maxFI = ControlObject.FormLimits.fiCurrentRate_max;
            if (MCBQuantum.Instance.GetMCB().Config.useNewEastPennProfile)
                maxFI *= 2;
            if (maxFI > 99)
                maxFI = 99;
            for (float i = ControlObject.FormLimits.fiCurrentRate_min; i < maxFI; i += 0.5f)
            {
                Batt_fiCurrentRateComboBox.Items.Add(MCB_FormatRate(i, 1));
            }
            Batt_fiCurrentRateComboBox.SelectedItem = MCB_FormatRate((MCBQuantum.Instance.GetMCB().Config.FIrate / 100.0f), 1);
            Batt_fiCurrentRateComboBox.SelectedIndex = Batt_fiCurrentRateComboBox.Items.IndexOf(Batt_fiCurrentRateComboBox.Items.FirstOrDefault(o => o.ToString() == Batt_fiCurrentRateComboBox.SelectedItem));

            if (Batt_fiCurrentRateComboBox.SelectedIndex == -1)
            {
                device_LoadWarnings.Add("Invalid Finish Rate Value");
            }
            //
            Batt_eqCurrentRateComboBox.Items = new List<object>();
            for (float i = ControlObject.FormLimits.eqCurrentRate_min; i < ControlObject.FormLimits.eqCurrentRate_max; i++)
            {
                Batt_eqCurrentRateComboBox.Items.Add(MCB_FormatRate(i, 1));
            }
            Batt_eqCurrentRateComboBox.SelectedItem = MCB_FormatRate((MCBQuantum.Instance.GetMCB().Config.EQrate / 100.0f), 1);
            Batt_eqCurrentRateComboBox.SelectedIndex = Batt_eqCurrentRateComboBox.Items.IndexOf(Batt_eqCurrentRateComboBox.Items.FirstOrDefault(o => o.ToString() == Batt_eqCurrentRateComboBox.SelectedItem));

            if (Batt_eqCurrentRateComboBox.SelectedIndex == -1)
            {
                device_LoadWarnings.Add("Invalid EQ Rate Value");
            }
            Batt_trickleVoltageTextBox.SubTitle = Batt_trickleVoltageTextBox.Text = MCBQuantum.Instance.GetMCB().Config.trickleVoltage;
            Batt_cvVoltageTextBox.SubTitle = Batt_cvVoltageTextBox.Text = MCBQuantum.Instance.GetMCB().Config.CVvoltage;
            Batt_finishVoltageTextBox.SubTitle = Batt_finishVoltageTextBox.Text = MCBQuantum.Instance.GetMCB().Config.FIvoltage;
            Batt_equalizeVoltageTextBox.SubTitle = Batt_equalizeVoltageTextBox.Text = MCBQuantum.Instance.GetMCB().Config.EQvoltage;

            Batt_cvFinishCurrentRateComboBox.Items = new List<object>();
            for (float i = ControlObject.FormLimits.cvFinishCurrentRate_min; i <= ControlObject.FormLimits.cvFinishCurrentRate_max; i += 0.5f)
            {
                Batt_cvFinishCurrentRateComboBox.Items.Add(MCB_FormatRate(i, 1));
            }
            Batt_cvFinishCurrentRateComboBox.SelectedItem = MCB_FormatRate((MCBQuantum.Instance.GetMCB().Config.CVfinishCurrent / 2.0f), 1);
            Batt_cvFinishCurrentRateComboBox.SelectedIndex = Batt_cvFinishCurrentRateComboBox.Items.IndexOf(Batt_cvFinishCurrentRateComboBox.Items.FirstOrDefault(o => o.ToString() == Batt_cvFinishCurrentRateComboBox.SelectedItem));

            if (Batt_cvFinishCurrentRateComboBox.SelectedIndex == -1)
            {
                device_LoadWarnings.Add("Invalid CV Finish Rate Value");
            }
            MCB_CV_CurrentStepLoad(false);
            Batt_cvTimerComboBox.SelectedItem = MCBQuantum.Instance.GetMCB().Config.CVtimer;

            if (Batt_cvTimerComboBox.SelectedIndex == -1)
                device_LoadWarnings.Add("Invalid CV Max Duration Value");

            Batt_finishTimerComboBox.SelectedItem = MCBQuantum.Instance.GetMCB().Config.finishTimer;
            if (Batt_finishTimerComboBox.SelectedIndex == -1)
                device_LoadWarnings.Add("Invalid Finish Duration Value");
            MCB_ForceFinishDurationDisableRadio.IsSwitchEnabled = MCBQuantum.Instance.GetMCB().Config.forceFinishTimeout;
            Batt_equalizeTimerComboBox.SelectedItem = MCBQuantum.Instance.GetMCB().Config.EQtimer;
            if (Batt_equalizeTimerComboBox.SelectedIndex == -1)
                device_LoadWarnings.Add("Invalid Equalize Duration Value");

            Batt_DesulfationTimerComboBox.SelectedItem = MCBQuantum.Instance.GetMCB().Config.desulfationTimer;
            if (Batt_DesulfationTimerComboBox.SelectedIndex == -1)
                device_LoadWarnings.Add("Invalid Desulfate Duration Value");

            Batt_finishDVVoltageComboBox.SelectedItem = (MCBQuantum.Instance.GetMCB().Config.finishDV).ToString();
            if (Batt_finishDVVoltageComboBox.SelectedIndex == -1)
                device_LoadWarnings.Add("Invalid Finish dV Value");

            Batt_finishDTVoltageComboBox.SelectedItem = MCBQuantum.Instance.GetMCB().Config.finishDT;
            if (Batt_finishDTVoltageComboBox.SelectedIndex == -1)
                device_LoadWarnings.Add("Invalid Finish dt Value");
            MCB_LoadUIControlsFromLimits();

            MCB_VerfiyChargeProfile();
        }

        void MCB_CV_CurrentStepLoad(bool keepIndex)
        {
            if (!MCBQuantum.Instance.MCB_quickSerialNumberCheckStrict())
                return;
            //get CC rate
            if (Batt_ccCurrentRateComboBox.SelectedIndex == -1)
                return;
            int selectedIndex = 0;
            if (keepIndex)
                selectedIndex = Batt_cvCurrentStepComboBox.SelectedIndex;
            float ccRate = MCB_getValueFromRates((string)Batt_ccCurrentRateComboBox.SelectedItem);
            ccRate /= 100;
            Batt_cvCurrentStepComboBox.Items = new List<object>();
            Batt_cvCurrentStepComboBox.Items.Add("Default");
            for (float i = ControlObject.FormLimits.cvCurrentStep_min; i <= ControlObject.FormLimits.cvCurrentStep_max; i += 0.5f)
            {

                Batt_cvCurrentStepComboBox.Items.Add(MCB_FormatRate(i, ccRate));
            }
            if (keepIndex)
            {
                Batt_cvCurrentStepComboBox.SelectedIndex = selectedIndex;
                return;
            }
            if (MCBQuantum.Instance.GetMCB().Config.CVcurrentStep == 0)
            {
                Batt_cvCurrentStepComboBox.SelectedItem = "Default";
            }
            else
            {
                Batt_cvCurrentStepComboBox.SelectedItem = MCB_FormatRate(MCBQuantum.Instance.GetMCB().Config.CVcurrentStep / 2.0f, ccRate);
            }
            if (Batt_cvCurrentStepComboBox.SelectedIndex == -1)
            {
                device_LoadWarnings.Add("Invalid CV Current Step Rate Value");
            }
        }

        float MCB_getValueFromRates(string value)
        {
            return float.Parse(value.Split(new char[] { '%' })[0]);

        }

        string MCB_FormatRate(float percent, float select)
        {
            ushort ahr = 0;

            if (MCBQuantum.Instance.GetMCB().Config.enableAutoDetectMultiVoltage && MCBQuantum.Instance.GetMCB().FirmwareRevision > 2.03f)
            {


                //get the highest AHR
                //if (is24 && connManager.activeMCB.MCBConfig.batteryCapacity24 > ahr)
                //    ahr = connManager.activeMCB.MCBConfig.batteryCapacity24;
                //if (is36 && connManager.activeMCB.MCBConfig.batteryCapacity36 > ahr)
                //    ahr = connManager.activeMCB.MCBConfig.batteryCapacity36;
                //if (is48 && connManager.activeMCB.MCBConfig.batteryCapacity48 > ahr)
                //    ahr = connManager.activeMCB.MCBConfig.batteryCapacity48;
                string rx = percent.ToString() + "% - ";

                rx += Math.Round(0.01f * percent * MCBQuantum.Instance.GetMCB().Config.batteryCapacity24 * select).ToString("N0") + "A";

                rx += ", ";
                rx += Math.Round(0.01f * percent * MCBQuantum.Instance.GetMCB().Config.batteryCapacity36 * select).ToString("N0") + "A";


                if (byte.Parse(MCBQuantum.Instance.GetMCB().Config.PMvoltage) >= 48)
                {
                    rx += ", ";
                    rx += Math.Round(0.01f * percent * MCBQuantum.Instance.GetMCB().Config.batteryCapacity48 * select).ToString("N0") + "A";

                }
                if (byte.Parse(MCBQuantum.Instance.GetMCB().Config.PMvoltage) >= 80)
                {
                    rx += ", ";
                    rx += Math.Round(0.01f * percent * MCBQuantum.Instance.GetMCB().Config.batteryCapacity80 * select).ToString("N0") + "A";

                }
                return rx;

            }
            else
            {
                ahr = UInt16.Parse(MCBQuantum.Instance.GetMCB().Config.batteryCapacity);
                return percent.ToString() + "% - " + Math.Round(0.01f * percent * ahr * select).ToString("N0") + "A";
            }
        }


        bool MCB_VerfiyChargeProfile()
        {
            if (!MCBQuantum.Instance.MCB_quickSerialNumberCheckStrict())
                return false;

            if (compareChargeProfileWithDefault())
            {
                //MCB_ChargeProfileLoadDefaultsButton.Hide();
                // Hide the Load Defaults Button
            }

            else
            {
                // Show the Load Defaults Button
                //MCB_ChargeProfileLoadDefaultsButton.Show();
            }


            verifyControl = new VerifyControl();
            verifyControl.VerifyComboBox(Batt_trCurrentRateComboBox);
            verifyControl.VerifyComboBox(Batt_ccCurrentRateComboBox);
            verifyControl.VerifyComboBox(Batt_fiCurrentRateComboBox);
            verifyControl.VerifyComboBox(Batt_eqCurrentRateComboBox);
            verifyControl.VerifyFloatNumber(Batt_trickleVoltageTextBox, Batt_trickleVoltageTextBox, ControlObject.FormLimits.trVoltage_min, ControlObject.FormLimits.trVoltage_max);
            verifyControl.VerifyFloatNumber(Batt_cvVoltageTextBox, Batt_cvVoltageTextBox, ControlObject.FormLimits.cvVoltage_min, ControlObject.FormLimits.cvVoltage_max);
            verifyControl.VerifyFloatNumber(Batt_finishVoltageTextBox, Batt_finishVoltageTextBox, ControlObject.FormLimits.fiVoltage_min, ControlObject.FormLimits.fiVoltage_max);
            verifyControl.VerifyFloatNumber(Batt_equalizeVoltageTextBox, Batt_equalizeVoltageTextBox, ControlObject.FormLimits.eqVoltage_min, ControlObject.FormLimits.eqVoltage_max);
            verifyControl.VerifyComboBox(Batt_cvFinishCurrentRateComboBox, Batt_cvFinishCurrentRateComboBox);
            verifyControl.VerifyComboBox(Batt_cvCurrentStepComboBox);
            verifyControl.VerifyComboBox(Batt_cvTimerComboBox);
            verifyControl.VerifyComboBox(Batt_finishTimerComboBox);
            verifyControl.VerifyComboBox(Batt_equalizeTimerComboBox);
            verifyControl.VerifyComboBox(Batt_DesulfationTimerComboBox);
            verifyControl.VerifyComboBox(Batt_finishDVVoltageComboBox);
            verifyControl.VerifyComboBox(Batt_finishDTVoltageComboBox);

            if (!verifyControl.HasErrors())
            {
                if (MCB_getValueFromRates((string)Batt_trCurrentRateComboBox.Text) > MCB_getValueFromRates((string)Batt_ccCurrentRateComboBox.Text))
                    verifyControl.InsertRemoveFault(true, Batt_trCurrentRateComboBox);
                else
                    verifyControl.InsertRemoveFault(false, Batt_trCurrentRateComboBox);
                if (MCB_getValueFromRates((string)Batt_fiCurrentRateComboBox.Text) > MCB_getValueFromRates((string)Batt_ccCurrentRateComboBox.Text))
                    verifyControl.InsertRemoveFault(true, Batt_trCurrentRateComboBox);
                else
                    verifyControl.InsertRemoveFault(false, Batt_trCurrentRateComboBox);
                if (MCB_getValueFromRates((string)Batt_eqCurrentRateComboBox.Text) > MCB_getValueFromRates((string)Batt_fiCurrentRateComboBox.Text))
                    verifyControl.InsertRemoveFault(true, Batt_eqCurrentRateComboBox);
                else
                    verifyControl.InsertRemoveFault(false, Batt_eqCurrentRateComboBox);

                if (MCB_getValueFromRates((string)Batt_cvFinishCurrentRateComboBox.Text) > MCB_getValueFromRates((string)Batt_ccCurrentRateComboBox.Text))
                    verifyControl.InsertRemoveFault(true, Batt_cvFinishCurrentRateComboBox);
                else
                    verifyControl.InsertRemoveFault(false, Batt_cvFinishCurrentRateComboBox);

                if (float.Parse(Batt_finishVoltageTextBox.Text) < float.Parse(Batt_cvVoltageTextBox.Text))
                    verifyControl.InsertRemoveFault(true, Batt_finishVoltageTextBox);
                else
                    verifyControl.InsertRemoveFault(false, Batt_finishVoltageTextBox);


                if (float.Parse(Batt_equalizeVoltageTextBox.Text) < float.Parse(Batt_finishVoltageTextBox.Text))
                    verifyControl.InsertRemoveFault(true, Batt_equalizeVoltageTextBox);
                else
                    verifyControl.InsertRemoveFault(false, Batt_equalizeVoltageTextBox);


            }


            return !verifyControl.HasErrors();
        }

        bool compareChargeProfileWithDefault()
        {
            if (!MCBQuantum.Instance.MCB_quickSerialNumberCheckStrict())
                return true;

            if (MCBQuantum.Instance.GetMCB().Config.TRrate != 500 ||
            MCBQuantum.Instance.GetMCB().Config.FIrate != 500 ||
            MCBQuantum.Instance.GetMCB().Config.EQrate != 400 ||
            MCBQuantum.Instance.GetMCB().Config.trickleVoltage != "2.0" ||
            MCBQuantum.Instance.GetMCB().Config.FIvoltage != "2.6" ||
            MCBQuantum.Instance.GetMCB().Config.EQvoltage != "2.65" ||
            MCBQuantum.Instance.GetMCB().Config.CVfinishCurrent != 24 ||
            MCBQuantum.Instance.GetMCB().Config.CVtimer != "04:00" ||
            MCBQuantum.Instance.GetMCB().Config.finishTimer != "03:00" ||
            MCBQuantum.Instance.GetMCB().Config.EQtimer != "04:00" ||
            MCBQuantum.Instance.GetMCB().Config.desulfationTimer != "12:00" ||
            MCBQuantum.Instance.GetMCB().Config.finishDV != 5 ||
            MCBQuantum.Instance.GetMCB().Config.finishDT != "59" ||
            MCBQuantum.Instance.GetMCB().Config.CVcurrentStep != 0 ||
                MCBQuantum.Instance.GetMCB().Config.FIstartWindow != "00:00" ||
                MCBQuantum.Instance.GetMCB().Config.EQstartWindow != "00:00" ||
                MCBQuantum.Instance.GetMCB().Config.EQwindow != "24:00"
                )
                return false;
            if (MCBQuantum.Instance.GetMCB().Config.chargerType == 0)
            {
                //FAST
                if (MCBQuantum.Instance.GetMCB().Config.CVvoltage != "2.42" ||
                MCBQuantum.Instance.GetMCB().Config.CCrate != 4000 ||
                    MCBQuantum.Instance.GetMCB().Config.finishWindow != "24:00")
                    return false;

            }
            else if (MCBQuantum.Instance.GetMCB().Config.chargerType == 1)
            {
                //Conventional
                if (MCBQuantum.Instance.GetMCB().Config.CVvoltage != "2.37" ||
                MCBQuantum.Instance.GetMCB().Config.CCrate != 1700 ||
                    MCBQuantum.Instance.GetMCB().Config.finishWindow != "24:00")
                    return false;
            }
            else if (MCBQuantum.Instance.GetMCB().Config.chargerType == 2)
            {
                //Opp
                if (MCBQuantum.Instance.GetMCB().Config.CVvoltage != "2.4" ||
                MCBQuantum.Instance.GetMCB().Config.CCrate != 2500 ||
                    MCBQuantum.Instance.GetMCB().Config.finishWindow != "08:00")
                    return false;
            }
            return true;
        }


        private int chargerBatteryChargeprofileAccessApply()
        {
            UIAccessControlUtility accessControlUtility = new UIAccessControlUtility();
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_TR_CurrentRate, Batt_trCurrentRateComboBox, DefaultChargeProfileItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_CC_CurrentRate, Batt_ccCurrentRateComboBox, DefaultChargeProfileItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_FI_CurrentRate, Batt_fiCurrentRateComboBox, DefaultChargeProfileItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_EQ_CurrentRate, Batt_eqCurrentRateComboBox, DefaultChargeProfileItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_TrickleVoltage, Batt_trickleVoltageTextBox, DefaultChargeProfileItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_CVVoltage, Batt_cvVoltageTextBox, DefaultChargeProfileItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_finishVoltage, Batt_finishVoltageTextBox, DefaultChargeProfileItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_EqualaizeVoltage, Batt_equalizeVoltageTextBox, DefaultChargeProfileItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_cvCurrentStep, Batt_cvCurrentStepComboBox, DefaultChargeProfileItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_cvFinishCurrent, Batt_cvFinishCurrentRateComboBox, DefaultChargeProfileItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_CVMaxTimer, Batt_cvTimerComboBox, DefaultChargeProfileItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_finishTimer, Batt_finishTimerComboBox, DefaultChargeProfileItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_finishTimer, MCB_ForceFinishDurationDisableRadio, DefaultChargeProfileItemSource);

            //
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_EqualizeTimer, Batt_equalizeTimerComboBox, DefaultChargeProfileItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_desulfationTimer, Batt_DesulfationTimerComboBox, DefaultChargeProfileItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_finishdVdT, Batt_finishDVVoltageComboBox, DefaultChargeProfileItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_finishdVdT, Batt_finishDTVoltageComboBox, DefaultChargeProfileItemSource);

            if (accessControlUtility.GetSavedCount() == 0)
            {
                ShowEdit = false;
            }

            return accessControlUtility.GetVisibleCount(); ;
        }


        void MCB_SaveIntoChargeProfile()
        {
            if (!MCBQuantum.Instance.MCB_quickSerialNumberCheckStrict())
                return;
            MCBQuantum.Instance.GetMCB().Config.TRrate = (UInt16)(100 * MCB_getValueFromRates((string)Batt_trCurrentRateComboBox.Text));
            MCBQuantum.Instance.GetMCB().Config.CCrate = (UInt16)(100 * MCB_getValueFromRates((string)Batt_ccCurrentRateComboBox.Text));
            MCBQuantum.Instance.GetMCB().Config.FIrate = (UInt16)(100 * MCB_getValueFromRates((string)Batt_fiCurrentRateComboBox.Text));
            MCBQuantum.Instance.GetMCB().Config.EQrate = (UInt16)(100 * MCB_getValueFromRates((string)Batt_eqCurrentRateComboBox.Text));
            MCBQuantum.Instance.GetMCB().Config.trickleVoltage = Batt_trickleVoltageTextBox.Text;
            MCBQuantum.Instance.GetMCB().Config.CVvoltage = Batt_cvVoltageTextBox.Text;
            MCBQuantum.Instance.GetMCB().Config.FIvoltage = Batt_finishVoltageTextBox.Text;
            MCBQuantum.Instance.GetMCB().Config.EQvoltage = Batt_equalizeVoltageTextBox.Text;
            MCBQuantum.Instance.GetMCB().Config.CVfinishCurrent = (byte)(2 * MCB_getValueFromRates((string)Batt_cvFinishCurrentRateComboBox.Text));
            if ((string)Batt_cvCurrentStepComboBox.Text == "Default")
                MCBQuantum.Instance.GetMCB().Config.CVcurrentStep = 0;
            else
                MCBQuantum.Instance.GetMCB().Config.CVcurrentStep = (byte)(2 * MCB_getValueFromRates((string)Batt_cvCurrentStepComboBox.Text));
            MCBQuantum.Instance.GetMCB().Config.CVtimer = (string)Batt_cvTimerComboBox.Text;
            MCBQuantum.Instance.GetMCB().Config.finishTimer = (string)Batt_finishTimerComboBox.Text;
            MCBQuantum.Instance.GetMCB().Config.forceFinishTimeout = MCB_ForceFinishDurationDisableRadio.IsSwitchEnabled;
            MCBQuantum.Instance.GetMCB().Config.EQtimer = (string)Batt_equalizeTimerComboBox.Text;
            MCBQuantum.Instance.GetMCB().Config.desulfationTimer = (string)Batt_DesulfationTimerComboBox.Text;
            MCBQuantum.Instance.GetMCB().Config.finishDV = (byte)(int.Parse((string)Batt_finishDVVoltageComboBox.Text));
            MCBQuantum.Instance.GetMCB().Config.finishDT = (string)Batt_finishDTVoltageComboBox.Text;
        }


        void MCB_LoadUIControlsFromLimits()
        {
            //String.Format("{0:00}:{1:00}",
            //Batt_cvTimerComboBox
            Batt_cvTimerComboBox.Items = new List<object>();
            Batt_finishTimerComboBox.Items = new List<object>();
            Batt_equalizeTimerComboBox.Items = new List<object>();
            Batt_DesulfationTimerComboBox.Items = new List<object>();
            Batt_finishDTVoltageComboBox.Items = new List<object>();
            Batt_finishDVVoltageComboBox.Items = new List<object>();
            for (int i = ControlObject.FormLimits.cvTimerStart; i <= ControlObject.FormLimits.cvTimerEnd; i += ControlObject.FormLimits.cvTimerStep)
            {
                if (i != 0)
                {
                    Batt_cvTimerComboBox.Items.Add(String.Format("{0:00}:{1:00}", i / 4, (i % 4) * 15));

                }
                else
                {
                    Batt_cvTimerComboBox.Items.Add(String.Format("00:01"));
                }

            }
            for (int i = ControlObject.FormLimits.fiTimerStart; i <= ControlObject.FormLimits.fiTimerEnd; i += ControlObject.FormLimits.fiTimerStep)
            {
                if (i != 0)
                {
                    Batt_finishTimerComboBox.Items.Add(String.Format("{0:00}:{1:00}", i / 4, (i % 4) * 15));
                }
                else
                {
                    Batt_finishTimerComboBox.Items.Add(String.Format("00:01"));
                }
            }
            for (int i = ControlObject.FormLimits.eqTimerStart; i <= ControlObject.FormLimits.eqTimerEnd; i += ControlObject.FormLimits.eqTimerStep)
            {
                if (i != 0)
                {
                    Batt_equalizeTimerComboBox.Items.Add(String.Format("{0:00}:{1:00}", i / 4, (i % 4) * 15));

                }
                else
                {
                    Batt_equalizeTimerComboBox.Items.Add(String.Format("00:01"));
                }
            }
            for (int i = ControlObject.FormLimits.desTimerStart; i <= ControlObject.FormLimits.desTimerEnd; i += ControlObject.FormLimits.desTimerStep)
            {
                if (i != 0)
                {
                    Batt_DesulfationTimerComboBox.Items.Add(String.Format("{0:00}:{1:00}", i / 4, (i % 4) * 15));
                }
                else
                {
                    Batt_DesulfationTimerComboBox.Items.Add(String.Format("00:01"));

                }

            }
            for (int i = ControlObject.FormLimits.fidtStart; i <= ControlObject.FormLimits.fidtEnd; i++)
            {
                Batt_finishDTVoltageComboBox.Items.Add(i.ToString());
            }
            for (int i = ControlObject.FormLimits.fidVStart; i <= ControlObject.FormLimits.fidVEnd; i++)
            {
                Batt_finishDVVoltageComboBox.Items.Add((i * 5).ToString());

            }
        }

        public IMvxCommand LoadDefaultBtnClickCommand
        {
            get
            {
                return new MvxCommand(async () =>
                {
                    await OnLoadDefaultClick();
                });
            }
        }

        async Task OnLoadDefaultClick()
        {
            if (NetworkCheck())
            {
                if (IsBattView)
                {
                    ACUserDialogs.ShowProgress();
                    BattViewCommunicationTypes caller = BattViewCommunicationTypes.NOCall;
                    bool arg1 = false;
                    try
                    {
                        BattViewQuantum.Instance.Batt_saveDefaultChargeProfile();
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
                                IsLoadDefaultEnable = false;
                                CreateList();

                                try
                                {
                                    ResetOldData();
                                    Batt_loadChargeProfile();
                                    RaisePropertyChanged(() => DefaultChargeProfileItemSource);
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
                    ACUserDialogs.ShowProgress();
                    McbCommunicationTypes caller = McbCommunicationTypes.NOCall;
                    bool arg1 = false;
                    try
                    {
                        MCBQuantum.Instance.MCB_saveDefaultChargeProfile();

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
                                IsLoadDefaultEnable = false;
                                CreateList();

                                try
                                {
                                    ResetOldData();
                                    MCB_loadChargeProfile();
                                    RaisePropertyChanged(() => DefaultChargeProfileItemSource);
                                }
                                catch (Exception ex)
                                {
                                    Debug.WriteLine(ex.Message);
                                    Logger.AddLog(true, "X24" + ex.ToString());
                                }
                            }
                            else
                            {
                                //Saving to MCB failed.
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
            }
        }
    }
}