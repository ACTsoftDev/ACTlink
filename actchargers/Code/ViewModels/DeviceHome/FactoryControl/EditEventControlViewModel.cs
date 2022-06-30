using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;

namespace actchargers
{
    public class EditEventControlViewModel : BaseViewModel
    {
        MvxSubscriptionToken _mListSelector;

        VerifyControl verifyControl;

        ListViewItem Batt_eventDetectVoltagePercentageComboBoxTOSAVE;
        ListViewItem Batt_eventDetectCurrentPercentageComboBoxTOSAVE;
        ListViewItem Batt_eventDetectTimerPercentageComboBoxTOSAVE;
        ListViewItem Batt_inUsetoIdleTimerTextBoxTOSAVE;
        ListViewItem Batt_chargeToIdleTimerTextBoxTOSAVE;
        ListViewItem Batt_chargeToInUseTimerTextBoxTOSAVE;
        ListViewItem Batt_inUseToChargeTimerTextBoxTOSAVE;
        ListViewItem Batt_currentInUseToIdleTextBoxTOSAVE;
        ListViewItem Batt_idleTochargeTimerTextBoxTOSAVE;
        ListViewItem Batt_currentInUseToChargeTextBoxTOSAVE;
        ListViewItem Batt_idleToInUseTimerTextBoxTOSAVE;
        ListViewItem Batt_currentChargeToInUseTextBoxTOSAVE;
        ListViewItem Batt_electrolyte_HLTTextBoxTOSAVE;
        ListViewItem Batt_currentChargeToIdleTextBoxTOSAVE;
        ListViewItem Batt_electrolyte_LHTTextBoxTOSAVE;
        ListViewItem Batt_currentIdleToInUseTextBoxTOSAVE;
        ListViewItem Batt_currentIdleToChargeTextBoxTOSAVE;

        private ObservableCollection<TableHeaderItem> _eventControlItemSource;
        public ObservableCollection<TableHeaderItem> EventControlItemSource
        {
            get { return _eventControlItemSource; }

            set
            {
                _eventControlItemSource = value;
                RaisePropertyChanged(() => EventControlItemSource);
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

        public EditEventControlViewModel()
        {
            ViewTitle = AppResources.edit_event_control;
            EditingMode = false;
            EventControlItemSource = new ObservableCollection<TableHeaderItem>();

            //Section 0
            EventControlItemSource.Add(new TableHeaderItem
            {
                //SectionHeader = AppResources.cummulative_usage_summary,
            });
            Batt_eventDetectVoltagePercentageComboBoxTOSAVE = new ListViewItem
            {
                Title = AppResources.voltage_detect_threshold,
                EditableCellType = ACUtility.CellTypes.ListSelector,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                ListSelectorType = ACUtility.ListSelectorType.VoltageDetectThreshold,
                ListSelectionCommand = ListSelectorCommand,
                ParentIndex = 0
            };
            EventControlItemSource[0].Add(Batt_eventDetectVoltagePercentageComboBoxTOSAVE);

            Batt_eventDetectCurrentPercentageComboBoxTOSAVE = new ListViewItem
            {
                Title = AppResources.current_detect_threshold,
                EditableCellType = ACUtility.CellTypes.ListSelector,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                ListSelectorType = ACUtility.ListSelectorType.CurrentDetectThreshold,
                ListSelectionCommand = ListSelectorCommand,
                ParentIndex = 0
            };
            Batt_eventDetectTimerPercentageComboBoxTOSAVE = new ListViewItem
            {
                Title = AppResources.timer_detect_threshold,
                EditableCellType = ACUtility.CellTypes.ListSelector,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                ListSelectorType = ACUtility.ListSelectorType.TimerDetectThreshold,
                ListSelectionCommand = ListSelectorCommand,
                ParentIndex = 0
            };
            EventControlItemSource[0].Add(Batt_eventDetectCurrentPercentageComboBoxTOSAVE);
            EventControlItemSource[0].Add(Batt_eventDetectTimerPercentageComboBoxTOSAVE);

            //Section 1
            EventControlItemSource.Add(new TableHeaderItem
            {
                SectionHeader = AppResources.charge_to_idle
            });
            Batt_chargeToIdleTimerTextBoxTOSAVE = new ListViewItem
            {
                Title = AppResources.timer,
                EditableCellType = ACUtility.CellTypes.LabelTextEdit,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                TextMaxLength = 3
            };
            Batt_currentChargeToIdleTextBoxTOSAVE = new ListViewItem
            {
                Title = AppResources.current_limit,
                EditableCellType = ACUtility.CellTypes.LabelTextEdit,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                TextMaxLength = 4
            };
            EventControlItemSource[1].Add(Batt_chargeToIdleTimerTextBoxTOSAVE);
            EventControlItemSource[1].Add(Batt_currentChargeToIdleTextBoxTOSAVE);

            //Section 2
            EventControlItemSource.Add(new TableHeaderItem
            {
                SectionHeader = AppResources.charge_to_inUse
            });
            Batt_chargeToInUseTimerTextBoxTOSAVE = new ListViewItem
            {
                Title = AppResources.timer,
                EditableCellType = ACUtility.CellTypes.LabelTextEdit,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                TextMaxLength = 3
            };
            Batt_currentChargeToInUseTextBoxTOSAVE = new ListViewItem
            {
                Title = AppResources.current_limit,
                EditableCellType = ACUtility.CellTypes.LabelTextEdit,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                TextMaxLength = 5
            };
            EventControlItemSource[2].Add(Batt_chargeToInUseTimerTextBoxTOSAVE);
            EventControlItemSource[2].Add(Batt_currentChargeToInUseTextBoxTOSAVE);

            //Section 3
            EventControlItemSource.Add(new TableHeaderItem
            {
                SectionHeader = AppResources.inuse_to_Charge,
            });
            Batt_inUseToChargeTimerTextBoxTOSAVE = new ListViewItem
            {
                Title = AppResources.timer,
                EditableCellType = ACUtility.CellTypes.LabelTextEdit,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                TextMaxLength = 3
            };
            Batt_currentInUseToChargeTextBoxTOSAVE = new ListViewItem
            {
                Title = AppResources.current_limit,
                EditableCellType = ACUtility.CellTypes.LabelTextEdit,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                TextMaxLength = 4
            };
            EventControlItemSource[3].Add(Batt_inUseToChargeTimerTextBoxTOSAVE);
            EventControlItemSource[3].Add(Batt_currentInUseToChargeTextBoxTOSAVE);

            //Section 4
            EventControlItemSource.Add(new TableHeaderItem
            {
                SectionHeader = AppResources.inuse_to_idle
            });
            Batt_inUsetoIdleTimerTextBoxTOSAVE = new ListViewItem
            {
                Title = AppResources.timer,
                EditableCellType = ACUtility.CellTypes.LabelTextEdit,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                TextMaxLength = 3
            };
            Batt_currentInUseToIdleTextBoxTOSAVE = new ListViewItem
            {
                Title = AppResources.current_limit,
                EditableCellType = ACUtility.CellTypes.LabelTextEdit,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                TextMaxLength = 5
            };
            EventControlItemSource[4].Add(Batt_inUsetoIdleTimerTextBoxTOSAVE);
            EventControlItemSource[4].Add(Batt_currentInUseToIdleTextBoxTOSAVE);

            //Section 5
            EventControlItemSource.Add(new TableHeaderItem
            {
                SectionHeader = AppResources.idle_to_charge,
            });
            Batt_idleTochargeTimerTextBoxTOSAVE = new ListViewItem
            {
                Title = AppResources.timer,
                EditableCellType = ACUtility.CellTypes.LabelTextEdit,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                TextMaxLength = 3
            };
            Batt_currentIdleToChargeTextBoxTOSAVE = new ListViewItem
            {
                Title = AppResources.current_limit,
                EditableCellType = ACUtility.CellTypes.LabelTextEdit,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                TextMaxLength = 4
            };
            EventControlItemSource[5].Add(Batt_idleTochargeTimerTextBoxTOSAVE);
            EventControlItemSource[5].Add(Batt_currentIdleToChargeTextBoxTOSAVE);

            //Section 6
            EventControlItemSource.Add(new TableHeaderItem
            {
                SectionHeader = AppResources.idle_to_inUse,
            });
            Batt_idleToInUseTimerTextBoxTOSAVE = new ListViewItem
            {
                Title = AppResources.timer,
                EditableCellType = ACUtility.CellTypes.LabelTextEdit,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                TextMaxLength = 3
            };
            Batt_currentIdleToInUseTextBoxTOSAVE = new ListViewItem
            {
                Title = AppResources.current_limit,
                EditableCellType = ACUtility.CellTypes.LabelTextEdit,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                TextMaxLength = 5
            };
            EventControlItemSource[6].Add(Batt_idleToInUseTimerTextBoxTOSAVE);
            EventControlItemSource[6].Add(Batt_currentIdleToInUseTextBoxTOSAVE);

            //Section 7
            EventControlItemSource.Add(new TableHeaderItem
            {
                SectionHeader = AppResources.electrolyte,
            });
            Batt_electrolyte_HLTTextBoxTOSAVE = new ListViewItem
            {
                Title = AppResources.high_to_low,
                EditableCellType = ACUtility.CellTypes.LabelTextEdit,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                TextMaxLength = 3
            };
            Batt_electrolyte_LHTTextBoxTOSAVE = new ListViewItem
            {
                Title = AppResources.low_to_high,
                EditableCellType = ACUtility.CellTypes.LabelTextEdit,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                TextMaxLength = 3
            };
            EventControlItemSource[7].Add(Batt_electrolyte_HLTTextBoxTOSAVE);
            EventControlItemSource[7].Add(Batt_electrolyte_LHTTextBoxTOSAVE);

            try
            {
                Batt_loadEventsControl();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Logger.AddLog(true, "X26" + ex.ToString());
            }

            //if (!Batt_verifyEventsControl())
            //{
            //    ACUserDialogs.ShowAlert(AppResources.no_data_found);
            //    if (DevicePlatform == Plugin.DeviceInfo.Abstractions.Platform.iOS)
            //    {
            //        ShowViewModel<BatterySettingsViewModel>(new { pop = "pop" });
            //    }
            //    return;
            //}
        }

        void Batt_loadEventsControl()
        {
            if (!BattViewQuantum.Instance.BattView_quickSerialNumberCheckStrict())
                return;
            BattViewObject activeBattView = BattViewQuantum.Instance.GetBATTView();
            Batt_chargeToIdleTimerTextBoxTOSAVE.SubTitle = Batt_chargeToIdleTimerTextBoxTOSAVE.Text = activeBattView.Config.chargeToIdleTimer.ToString();
            Batt_chargeToInUseTimerTextBoxTOSAVE.SubTitle = Batt_chargeToInUseTimerTextBoxTOSAVE.Text = activeBattView.Config.chargeToInUseTimer.ToString();
            Batt_inUseToChargeTimerTextBoxTOSAVE.SubTitle = Batt_inUseToChargeTimerTextBoxTOSAVE.Text = activeBattView.Config.inUseToChargeTimer.ToString();
            Batt_inUsetoIdleTimerTextBoxTOSAVE.SubTitle = Batt_inUsetoIdleTimerTextBoxTOSAVE.Text = activeBattView.Config.inUsetoIdleTimer.ToString();
            Batt_idleTochargeTimerTextBoxTOSAVE.SubTitle = Batt_idleTochargeTimerTextBoxTOSAVE.Text = activeBattView.Config.idleToChargeTimer.ToString();
            Batt_idleToInUseTimerTextBoxTOSAVE.SubTitle = Batt_idleToInUseTimerTextBoxTOSAVE.Text = activeBattView.Config.idleToInUseTimer.ToString();
            Batt_electrolyte_HLTTextBoxTOSAVE.SubTitle = Batt_electrolyte_HLTTextBoxTOSAVE.Text = activeBattView.Config.electrolyteHLT.ToString();
            Batt_electrolyte_LHTTextBoxTOSAVE.SubTitle = Batt_electrolyte_LHTTextBoxTOSAVE.Text = activeBattView.Config.electrolyteLHT.ToString();

            Batt_currentIdleToChargeTextBoxTOSAVE.SubTitle = Batt_currentIdleToChargeTextBoxTOSAVE.Text = (activeBattView.Config.currentIdleToCharge / 10.0).ToString("00.0");
            Batt_currentIdleToInUseTextBoxTOSAVE.SubTitle = Batt_currentIdleToInUseTextBoxTOSAVE.Text = (activeBattView.Config.currentIdleToInUse / 10.0).ToString("00.0");
            Batt_currentChargeToIdleTextBoxTOSAVE.SubTitle = Batt_currentChargeToIdleTextBoxTOSAVE.Text = (activeBattView.Config.currentChargeToIdle / 10.0).ToString("00.0");
            Batt_currentChargeToInUseTextBoxTOSAVE.SubTitle = Batt_currentChargeToInUseTextBoxTOSAVE.Text = (activeBattView.Config.currentChargeToInUse / 10.0).ToString("00.0");
            Batt_currentInUseToChargeTextBoxTOSAVE.SubTitle = Batt_currentInUseToChargeTextBoxTOSAVE.Text = (activeBattView.Config.currentInUseToCharge / 10.0).ToString("00.0");
            Batt_currentInUseToIdleTextBoxTOSAVE.SubTitle = Batt_currentInUseToIdleTextBoxTOSAVE.Text = (activeBattView.Config.currentInUseToIdle / 10.0).ToString("00.0");

            Batt_eventDetectVoltagePercentageComboBoxTOSAVE.SelectedItem = (activeBattView.Config.eventDetectVoltagePercentage / 2.0).ToString("0.0") + "%";
            Batt_eventDetectCurrentPercentageComboBoxTOSAVE.SelectedItem = (activeBattView.Config.eventDetectCurrentRangePercentage / 2.0).ToString("0.0") + "%";
            Batt_eventDetectTimerPercentageComboBoxTOSAVE.SelectedItem = activeBattView.Config.eventDetectTimeRangePercentage.ToString();
        }

        bool Batt_verifyEventsControl()
        {
            if (!BattViewQuantum.Instance.BattView_quickSerialNumberCheckStrict())
                return false;
            verifyControl = new VerifyControl();

            verifyControl.VerifyUInteger(Batt_chargeToIdleTimerTextBoxTOSAVE, Batt_chargeToIdleTimerTextBoxTOSAVE, 1, 1200);
            verifyControl.VerifyUInteger(Batt_chargeToInUseTimerTextBoxTOSAVE, Batt_chargeToInUseTimerTextBoxTOSAVE, 1, 1200);
            verifyControl.VerifyUInteger(Batt_inUseToChargeTimerTextBoxTOSAVE, Batt_inUseToChargeTimerTextBoxTOSAVE, 1, 1200);
            verifyControl.VerifyUInteger(Batt_inUsetoIdleTimerTextBoxTOSAVE, Batt_inUsetoIdleTimerTextBoxTOSAVE, 1, 1200);
            verifyControl.VerifyUInteger(Batt_idleTochargeTimerTextBoxTOSAVE, Batt_idleTochargeTimerTextBoxTOSAVE, 1, 1200);
            verifyControl.VerifyUInteger(Batt_idleToInUseTimerTextBoxTOSAVE, Batt_idleToInUseTimerTextBoxTOSAVE, 1, 1200);

            verifyControl.VerifyUInteger(Batt_electrolyte_HLTTextBoxTOSAVE, Batt_electrolyte_HLTTextBoxTOSAVE, 1, 1200);
            verifyControl.VerifyUInteger(Batt_electrolyte_LHTTextBoxTOSAVE, Batt_electrolyte_LHTTextBoxTOSAVE, 1, 1200);

            verifyControl.VerifyComboBox(Batt_eventDetectVoltagePercentageComboBoxTOSAVE, Batt_eventDetectVoltagePercentageComboBoxTOSAVE);
            verifyControl.VerifyComboBox(Batt_eventDetectCurrentPercentageComboBoxTOSAVE, Batt_eventDetectCurrentPercentageComboBoxTOSAVE);
            verifyControl.VerifyComboBox(Batt_eventDetectTimerPercentageComboBoxTOSAVE, Batt_eventDetectTimerPercentageComboBoxTOSAVE);
            return !verifyControl.HasErrors();
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
                ShowViewModel<ListSelectorViewModel>(new { title = item.Title, type = item.ListSelectorType, selectedItemIndex = item.ParentIndex, selectedChildPosition = EventControlItemSource[item.ParentIndex].IndexOf(item) });
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
                EventControlItemSource[obj.SelectedItemindex][obj.SelectedChildItemindex].Text = obj.SelectedItem;
                EventControlItemSource[obj.SelectedItemindex][obj.SelectedChildItemindex].SelectedIndex = obj.SelectedIndex;
            }
        }

        void Batt_saveIntoEventsControl()
        {
            if (!BattViewQuantum.Instance.BattView_quickSerialNumberCheckStrict())
                return;
            BattViewObject activeBattView = BattViewQuantum.Instance.GetBATTView();
            activeBattView.Config.chargeToIdleTimer = UInt16.Parse(Batt_chargeToIdleTimerTextBoxTOSAVE.Text);
            activeBattView.Config.chargeToInUseTimer = UInt16.Parse(Batt_chargeToInUseTimerTextBoxTOSAVE.Text);
            activeBattView.Config.inUseToChargeTimer = UInt16.Parse(Batt_inUseToChargeTimerTextBoxTOSAVE.Text);
            activeBattView.Config.inUsetoIdleTimer = UInt16.Parse(Batt_inUsetoIdleTimerTextBoxTOSAVE.Text);
            activeBattView.Config.idleToChargeTimer = UInt16.Parse(Batt_idleTochargeTimerTextBoxTOSAVE.Text);
            activeBattView.Config.idleToInUseTimer = UInt16.Parse(Batt_idleToInUseTimerTextBoxTOSAVE.Text);

            activeBattView.Config.electrolyteHLT = UInt16.Parse(Batt_electrolyte_HLTTextBoxTOSAVE.Text);
            activeBattView.Config.electrolyteLHT = UInt16.Parse(Batt_electrolyte_LHTTextBoxTOSAVE.Text);


            activeBattView.Config.currentIdleToCharge = (Int16)(float.Parse(Batt_currentIdleToChargeTextBoxTOSAVE.Text) * 10.0f);

            activeBattView.Config.currentIdleToInUse = (Int16)(float.Parse(Batt_currentIdleToInUseTextBoxTOSAVE.Text) * 10.0f);
            activeBattView.Config.currentChargeToIdle = (Int16)(float.Parse(Batt_currentChargeToIdleTextBoxTOSAVE.Text) * 10.0f);
            activeBattView.Config.currentChargeToInUse = (Int16)(float.Parse(Batt_currentChargeToInUseTextBoxTOSAVE.Text) * 10.0f);
            activeBattView.Config.currentInUseToCharge = (Int16)(float.Parse(Batt_currentInUseToChargeTextBoxTOSAVE.Text) * 10.0f);
            activeBattView.Config.currentInUseToIdle = (Int16)(float.Parse(Batt_currentInUseToIdleTextBoxTOSAVE.Text) * 10.0f);

            Match match = Regex.Match(Batt_eventDetectVoltagePercentageComboBoxTOSAVE.Text, @"^([-+]?[0-9]*\.?[0-9]+)%$", RegexOptions.IgnoreCase);
            activeBattView.Config.eventDetectVoltagePercentage = (byte)Math.Round(float.Parse(match.Groups[1].Value) * 2.0f);

            match = Regex.Match(Batt_eventDetectCurrentPercentageComboBoxTOSAVE.Text, @"^([-+]?[0-9]*\.?[0-9]+)%$", RegexOptions.IgnoreCase);
            activeBattView.Config.eventDetectCurrentRangePercentage = (byte)Math.Round(float.Parse(match.Groups[1].Value) * 2.0f);

            activeBattView.Config.eventDetectTimeRangePercentage = byte.Parse(Batt_eventDetectTimerPercentageComboBoxTOSAVE.Text);
        }

        /// <summary>
        /// Gets the edit button click command.
        /// </summary>
        /// <value>The edit button click command.</value>
        public IMvxCommand EditBtnClickCommand
        {
            get { return new MvxCommand(OnEditClick); }
        }


        /// <summary>
        /// Ons the edit click.
        /// </summary>
        void OnEditClick()
        {
            EditingMode = true;
            CreateData();
            RaisePropertyChanged(() => EventControlItemSource);
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


        bool batt_validation()
        {
            foreach (TableHeaderItem item in EventControlItemSource)
            {
                foreach (ListViewItem childItem in item)
                {
                    if (childItem.TextMaxLength == 4 || childItem.TextMaxLength == 5)
                    {
                        Regex objAlphaPattern;
                        if (childItem.TextMaxLength == 4 )
                        {
                            objAlphaPattern = new Regex(@"[0-9][0-9].[0-9]");
                        }
                        else
                        {
                            objAlphaPattern = new Regex(@"-[0-9][0-9].[0-9]");
                        }
                        if (!objAlphaPattern.IsMatch(childItem.Text))
                            return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Ons the save click.
        /// </summary>
        /// <returns>The save click.</returns>
        async Task OnSaveClick()
        {
            if (NetworkCheck())
            {
                if (batt_validation())
                {
                    //Save logic
                    if (Batt_verifyEventsControl())
                    {
                        ACUserDialogs.ShowProgress();
                        BattViewCommunicationTypes caller = BattViewCommunicationTypes.NOCall;
                        bool arg1 = false;
                        try
                        {
                            //arg1 = 
                            Batt_saveIntoEventsControl();
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
                                    CreateData();
                                    try
                                    {
                                        ResetOldData();
                                        Batt_loadEventsControl();
                                        RaisePropertyChanged(() => EventControlItemSource);
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
                    //End of save logic
                }
                else
                {
                    ACUserDialogs.ShowAlertWithTitleAndOkButton("Invalid input");
                }
            }
        }

        void ResetOldData()
        {
            foreach (var item in EventControlItemSource)
            {
                foreach (var childItem in item)
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
            CreateData();
            RaisePropertyChanged(() => EventControlItemSource);
        }

        private bool CheckForEditedChanges()
        {
            bool textChanged = false;
            foreach (var item in EventControlItemSource)
            {
                foreach (ListViewItem childItem in item)
                {
                    if (childItem.Text != childItem.SubTitle)
                    {
                        textChanged = true;
                    }
                }
            }
            return textChanged;
        }

        public void CreateData()
        {
            foreach (var item in EventControlItemSource)
            {
                foreach (var childItem in item)
                {
                    childItem.IsEditable = EditingMode;
                    childItem.Text = childItem.SubTitle;
                }
            }

        }
    }
}