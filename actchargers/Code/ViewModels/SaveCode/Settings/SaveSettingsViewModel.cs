using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;
using Newtonsoft.Json;
using static actchargers.ACUtility;

namespace actchargers
{
    public class SaveSettingsViewModel : BaseViewModel
    {
        List<string> device_LoadWarnings;

        #region CableSettings Items

        ListViewItem CableSettings_ChargerCableLength;
        ListViewItem CableSettings_CableGauge;
        ListViewItem CableSettings_NumberOfCables;
        ListViewItem CableSettings_BatteryCableLength;
        ListViewItem CableSettings_Calculate;
        ListViewItem CableSettings_Cancel;

        #endregion

        ListViewItem Batt_autoLogTime; //Textbox
        ListViewItem Batt_isPA; //Switch
        ListViewItem Batt_enableExtTempSensingEnable; //Switch
        ListViewItem Batt_disableIntercell; //Switch
        ListViewItem Batt_enableElectrolyeSensingEnable; // Switch
        ListViewItem Batt_enableHallEffectSensingYes; //Switch
        ListViewItem Batt_NumberOfCables;
        ListViewItem Batt_enablePLCEnable; //Switch
        ListViewItem Batt_DayLightSaving_Enable; //Switch
        ListViewItem Batt_temperatureFormat; //Switch
                                             //ListViewItem Batt_temperatureFormat_C; //Switch
        ListViewItem BATT_temperatureControl;//ListItems

        #region charger listview item declaration

        ListViewItem MCB_IR_val_TextBox;
        ListViewItem MCB_AutoStartCustomRadioButtonTOSAVE;
        ListViewItem MCB_autoStartTime_TextBoxTOSAVE;
        ListViewItem MCB_Refresh_AfterEQ_Radio;
        ListViewItem MCB_Refresh_AfterFI_Radio;
        ListViewItem MCB_refreshComboBoxTOSAVE;
        ListViewItem MCB_TempSensorEnable_Radio;
        ListViewItem MCB_MultiVoltageEnableRadio;
        ListViewItem MCB_TemperatureFormatcelsiusRadio;
        ListViewItem MCB_DayLightSavingEnableRadio;
        ListViewItem MCB_enablePLC_enableRadio;
        ListViewItem MCB_enableManualEQ_enableRadio;
        ListViewItem MCB_enableManualDesulfate_enableRadio;
        ListViewItem MCB_enablepushbutton_enableRadio;
        ListViewItem MCB_enableLED_enableRadio;
        ListViewItem MCB_ignoreBAttviewSOC_YesRadio;
        ListViewItem MCB_OverrideFIEQSched_YesRadio;
        ListViewItem MCB_minSteps_ComboBox;
        ListViewItem MCB_BattviewAutoCalibration_EnableRadio;
        ListViewItem MCB_RemoteStopButton;
        ListViewItem MCB_UpdateCableSettingsButton;
        ListViewItem MCB_LoadPlcFirmwareAndRestartButton;

        #endregion

        VerifyControl verifyControl;

        ObservableCollection<ListViewItem> mainItemSource =
            new ObservableCollection<ListViewItem>();
        ObservableCollection<ListViewItem> cableSettingsItemSource =
            new ObservableCollection<ListViewItem>();

        ObservableCollection<ListViewItem> _settingsItemSource;
        public ObservableCollection<ListViewItem> SettingsViewItemSource
        {
            get { return _settingsItemSource; }
            set
            {
                _settingsItemSource = value;
                RaisePropertyChanged(() => SettingsViewItemSource);
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
        /// The is visible mcb reset LCDC alibration.
        /// </summary>
        private bool _isVisible_MCB_ResetLCDCalibration;
        public bool IsVisible_MCB_ResetLCDCalibration
        {
            get
            {
                return _isVisible_MCB_ResetLCDCalibration;
            }
            set
            {
                _isVisible_MCB_ResetLCDCalibration = value;
                RaisePropertyChanged(() => IsVisible_MCB_ResetLCDCalibration);
            }
        }

        /// <summary>
        /// The is edit enabled mcb reset LCDC alibration.
        /// </summary>
        private bool _isEditEnabled_MCB_ResetLCDCalibration;
        public bool IsEditEnabled_MCB_ResetLCDCalibration
        {
            get
            {
                return _isEditEnabled_MCB_ResetLCDCalibration;
            }
            set
            {
                _isEditEnabled_MCB_ResetLCDCalibration = value;
                RaisePropertyChanged(() => IsEditEnabled_MCB_ResetLCDCalibration);
            }
        }

        public string ResetLCDCalibrationTitle { get; set; }

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

        bool _IsCableSettings;
        public bool IsCableSettings
        {
            get
            {
                return _IsCableSettings;
            }
            set
            {
                SetProperty(ref _IsCableSettings, value);

                ChangeList();
            }
        }

        MvxSubscriptionToken _mListSelector;

        public SaveSettingsViewModel()
        {
            ShowEdit = true;
            device_LoadWarnings = new List<string>();
            SettingsViewItemSource = new ObservableCollection<ListViewItem>();
            CreateList();
        }

        void CreateList()
        {
            if (IsBattView)
            {
                ViewTitle = AppResources.battview_settings;
                CreateListForBattView();
            }
            else
            {
                ViewTitle = AppResources.charger_settings;
                CretaeListForChargers();
            }

            CreateListCableSettings();

            ChangeList();
        }

        void CreateListCableSettings()
        {
            CableSettings_ChargerCableLength = new ListViewItem()
            {
                Index = 0,
                Title = AppResources.charger_cable_length,
                DefaultCellType = CellTypes.LabelTextEdit,
                EditableCellType = CellTypes.LabelTextEdit,
                IsEditable = true,
                TextInputType = InputType.Number
            };
            cableSettingsItemSource.Add(CableSettings_ChargerCableLength);

            CableSettings_CableGauge = new ListViewItem()
            {
                Index = 1,
                Title = AppResources.cable_gauge,
                DefaultCellType = CellTypes.ListSelector,
                EditableCellType = CellTypes.ListSelector,
                ListSelectionCommand = ListSelectorCommand
            };
            CableSettings_CableGauge.Items = new List<object>();
            CableSettings_CableGauge.Items.AddRange(
                new object[]
            {
                "1/0",
                "2/0",
                "3/0",
                "4/0"
            });
            CableSettings_CableGauge.SelectedIndex = -1;
            cableSettingsItemSource.Add(CableSettings_CableGauge);

            CableSettings_NumberOfCables = new ListViewItem
            {
                Index = 2,
                Title = AppResources.number_of_cables,
                DefaultCellType = CellTypes.ListSelector,
                EditableCellType = CellTypes.ListSelector,
                ListSelectionCommand = ListSelectorCommand
            };
            CableSettings_NumberOfCables.Items = new List<object>
            {
                AppResources.single,
                AppResources.dual
            };
            CableSettings_NumberOfCables.SelectedIndex = -1;
            cableSettingsItemSource.Add(CableSettings_NumberOfCables);

            CableSettings_BatteryCableLength = new ListViewItem
            {
                Index = 3,
                Title = AppResources.battery_cable_length,
                DefaultCellType = CellTypes.LabelTextEdit,
                EditableCellType = CellTypes.LabelTextEdit,
                IsEditable = true,
                TextInputType = InputType.Number
            };
            cableSettingsItemSource.Add(CableSettings_BatteryCableLength);

            CableSettings_Calculate = new ListViewItem
            {
                Index = 4,
                Title = AppResources.calculate,
                DefaultCellType = CellTypes.Button,
                ListSelectionCommand = CalculateSelectionCommand
            };
            cableSettingsItemSource.Add(CableSettings_Calculate);

            CableSettings_Cancel = new ListViewItem
            {
                Index = 5,
                Title = AppResources.cancel,
                DefaultCellType = CellTypes.Button,
                ListSelectionCommand = CancelSelectionCommand
            };
            cableSettingsItemSource.Add(CableSettings_Cancel);

            if (cableSettingsItemSource.Count > 0)
            {
                cableSettingsItemSource =
                    new ObservableCollection<ListViewItem>
                    (cableSettingsItemSource.OrderBy(o => o.Index));
            }
        }

        void CreateListForBattView()
        {

            device_LoadWarnings = new List<string>();
            #region BATTView Settings Data
            Batt_autoLogTime = new ListViewItem()
            {
                Index = 0,
                Title = AppResources.samples_time_span,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.LabelTextEdit,
                TextInputType = ACUtility.InputType.Number
            };
            Batt_isPA = new ListViewItem()
            {
                Index = 1,
                Title = AppResources.battview_mobile,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.LabelSwitch
            };
            Batt_enableExtTempSensingEnable = new ListViewItem()
            {
                Index = 2,
                Title = AppResources.post_electrolyte_ntc,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.LabelSwitch
            };
            Batt_disableIntercell = new ListViewItem()
            {
                Index = 3,
                Title = AppResources.disable_intercell,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.LabelSwitch
            };
            Batt_enableElectrolyeSensingEnable = new ListViewItem()
            {
                Index = 4,
                Title = AppResources.electrolyte_sensing,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.LabelSwitch
            };
            Batt_enableHallEffectSensingYes = new ListViewItem()
            {
                Index = 5,
                Title = AppResources.use_hall_effect_sensor,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.LabelSwitch
            };

            Batt_NumberOfCables = new ListViewItem
            {
                Index = 6,
                Title = AppResources.number_of_cables,
                DefaultCellType = CellTypes.LabelLabel,
                EditableCellType = CellTypes.ListSelector,
                ListSelectionCommand = ListSelectorCommand
            };

            Batt_NumberOfCables.Items = new List<object>
            {
                AppResources.single,
                AppResources.dual
            };

            Batt_enablePLCEnable = new ListViewItem()
            {
                Index = 7,
                Title = AppResources.plc,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.LabelSwitch
            };
            Batt_DayLightSaving_Enable = new ListViewItem()
            {
                Index = 8,
                Title = AppResources.daylight_saving,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.LabelSwitch
            };
            Batt_temperatureFormat = new ListViewItem()
            {
                Index = 9,
                Title = AppResources.temperature_format,
                DefaultCellType = CellTypes.LabelLabel,
                EditableCellType = CellTypes.ListSelector,
                ListSelectorType = ACUtility.ListSelectorType.TemperatureFormate,
                ListSelectionCommand = ListSelectorCommand,
            };

            #endregion

            try
            {
                Batt_loadBattViewSettings();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Logger.AddLog(true, "X24" + ex);
            }

            if (BattViewSettingsAccessApply() == 0)
            {
                mainItemSource.Clear();

                ACUserDialogs.ShowAlertWithOkButtonsAction
                             (AppResources.no_data_found, () =>
                             {
                                 ShowViewModel<SettingsViewModel>(new { pop = "pop" });
                             }
                             );
                return;
            }

            if (mainItemSource.Count > 0)
            {
                mainItemSource =
                    new ObservableCollection<ListViewItem>
                    (mainItemSource.OrderBy(o => o.Index));
            }
        }

        void CretaeListForChargers()
        {
            ResetLCDCalibrationTitle = AppResources.reset_lcd;

            MCB_IR_val_TextBox = new ListViewItem()
            {
                Index = 0,
                Title = AppResources.cable_resistance,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.LabelTextEdit,
                TextInputType = ACUtility.InputType.Decimal,
                IsVisible = true,
                TextMaxLength = 8
            };
            MCB_AutoStartCustomRadioButtonTOSAVE = new ListViewItem()
            {
                Index = 1,
                Title = AppResources.auto_start,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.LabelSwitch,
                IsVisible = true,
                SwitchValueChanged = SwitchValueChanged,
                EnableItemsList = new List<int>() { 2 }
            };
            MCB_autoStartTime_TextBoxTOSAVE = new ListViewItem()
            {
                Index = 2,
                Title = AppResources.auto_start_count_down,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.LabelTextEdit,
                TextInputType = ACUtility.InputType.Number,
                IsVisible = true,
                TextMaxLength = 2
            };
            MCB_Refresh_AfterEQ_Radio = new ListViewItem()
            {
                Index = 3,
                Title = AppResources.refresh_cycle_after_eq,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.LabelSwitch,
                SwitchValueChanged = SwitchValueChanged,
                IsVisible = true
            };
            MCB_Refresh_AfterFI_Radio = new ListViewItem()
            {
                Index = 4,
                Title = AppResources.refresh_cycle_after_fi,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.LabelSwitch,
                SwitchValueChanged = SwitchValueChanged,
                IsVisible = true
            };
            MCB_refreshComboBoxTOSAVE = new ListViewItem()
            {
                Index = 5,
                Title = AppResources.refresh_frequency,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.ListSelector,
                ListSelectionCommand = ListSelectorCommand,
                IsVisible = true
            };
            MCB_TempSensorEnable_Radio = new ListViewItem()
            {
                Index = 6,
                Title = AppResources.temperature_sensor,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.LabelSwitch,
                IsVisible = true
            };
            MCB_MultiVoltageEnableRadio = new ListViewItem()
            {
                Index = 7,
                Title = AppResources.multi_voltage,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.LabelSwitch,
                IsVisible = true
            };
            MCB_TemperatureFormatcelsiusRadio = new ListViewItem()
            {
                Index = 8,
                Title = AppResources.temperature_format,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.ListSelector,
                ListSelectorType = ACUtility.ListSelectorType.TemperatureFormate,
                ListSelectionCommand = ListSelectorCommand,
                IsVisible = true
            };
            MCB_DayLightSavingEnableRadio = new ListViewItem()
            {
                Index = 9,
                Title = AppResources.daylight_saving,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.LabelSwitch,
                IsVisible = true
            };
            MCB_enablePLC_enableRadio = new ListViewItem()
            {
                Index = 10,
                Title = AppResources.enable_plc,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.LabelSwitch,
                IsVisible = true
            };
            MCB_enableManualEQ_enableRadio = new ListViewItem()
            {
                Index = 11,
                Title = AppResources.allow_manual_eq,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.LabelSwitch,
                IsVisible = true
            };
            MCB_enableManualDesulfate_enableRadio = new ListViewItem()
            {
                Index = 12,
                Title = AppResources.allow_manual_desulfate,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.LabelSwitch,
                IsVisible = true
            };
            MCB_enablepushbutton_enableRadio = new ListViewItem()
            {
                Index = 13,
                Title = AppResources.enable_stop_push_button,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.LabelSwitch,
                IsVisible = true
            };
            MCB_enableLED_enableRadio = new ListViewItem()
            {
                Index = 14,
                Title = AppResources.led_strip,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.LabelSwitch,
                IsVisible = true
            };
            MCB_ignoreBAttviewSOC_YesRadio = new ListViewItem()
            {
                Index = 15,
                Title = AppResources.charger_control_soc,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.LabelSwitch,
                IsVisible = true
            };
            MCB_OverrideFIEQSched_YesRadio = new ListViewItem()
            {
                Index = 16,
                Title = AppResources.charger_controls_fi_scheduling,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.LabelSwitch,
                IsVisible = true
            };
            MCB_minSteps_ComboBox = new ListViewItem()
            {
                Index = 17,
                Title = AppResources.current_ramp_minimum_steps,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.ListSelector,
                ListSelectionCommand = ListSelectorCommand,
                IsVisible = true
            };
            MCB_BattviewAutoCalibration_EnableRadio = new ListViewItem()
            {
                Index = 18,
                Title = AppResources.battview_auto_calibration,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.LabelSwitch,
                IsVisible = true
            };
            MCB_RemoteStopButton = new ListViewItem()
            {
                Index = 19,
                Title = AppResources.charger_remote_control_settings,
                DefaultCellType = CellTypes.LabelLabel,
                EditableCellType = CellTypes.ListSelector,
                ListSelectionCommand = ListSelectorCommand
            };
            MCB_RemoteStopButton.Items = new List<object>();
            MCB_RemoteStopButton.Items.AddRange(
                new object[]
            {
                AppResources.charger_remote_control_settings_disable,
                AppResources.charger_remote_control_settings_remote_stop,
                AppResources.charger_remote_control_settings_dual_cables
            });
            MCB_RemoteStopButton.SelectedIndex = 0;

            MCB_UpdateCableSettingsButton = new ListViewItem
            {
                Index = 20,
                Title = AppResources.update_cable_settings,
                DefaultCellType = CellTypes.Button,
                ListSelectionCommand = MCB_UpdateCableSettingsClickCommand
            };
            MCB_LoadPlcFirmwareAndRestartButton = new ListViewItem
            {
                Index = 21,
                Title = AppResources.load_plc_firmware_and_restart,
                DefaultCellType = CellTypes.Button,
                ListSelectionCommand = MCB_LoadPlcFirmwareAndRestartClickCommand
            };

            try
            {
                MCB_loadChargerSettings();
            }

            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Logger.AddLog(true, "X24" + ex.ToString());
            }

            if (ChargerSettingsAccessApply() == 0)
            {
                mainItemSource.Clear();
                ACUserDialogs.ShowAlertWithOkButtonsAction(AppResources.no_data_found, () => { ShowViewModel<SettingsViewModel>(new { pop = "pop" }); });
                return;
            }

            if (mainItemSource.Count > 0)
            {
                mainItemSource = new ObservableCollection<ListViewItem>(mainItemSource.Where(o => o.IsVisible).OrderBy(o => o.Index));
            }
        }


        /// <summary>
        /// Gets the switch value changed.
        /// </summary>
        /// <value>The switch value changed.</value>
        public IMvxCommand SwitchValueChanged
        {
            get
            {
                return new MvxCommand<ListViewItem>(ExecuteSwitchValueChanged);
            }
        }

        /// <summary>
        /// Executes the switch value changed.
        /// </summary>
        /// <param name="item">Item.</param>
        void ExecuteSwitchValueChanged(ListViewItem item)
        {
            if (item.EnableItemsList != null && item.EnableItemsList.Count > 0)
            {
                foreach (int editItemIndex in item.EnableItemsList)
                {
                    try
                    {
                        if (SettingsViewItemSource.Contains(item))
                        {
                            ListViewItem editItem = SettingsViewItemSource[editItemIndex];
                            if (editItem.IsEditEnabled)
                            {
                                editItem.IsEditable = item.IsSwitchEnabled && EditingMode;
                                editItem.Text = editItem.SubTitle;
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }
                }

                RaisePropertyChanged(() => SettingsViewItemSource);
            }

            if (!IsBattView && (item.Index == 3 || item.Index == 4))
            {
                if ((MCB_Refresh_AfterEQ_Radio.IsSwitchEnabled || MCB_Refresh_AfterFI_Radio.IsSwitchEnabled))
                {
                    MCB_refreshComboBoxTOSAVE.IsEditEnabled = true;
                }
                else
                {
                    MCB_refreshComboBoxTOSAVE.IsEditEnabled = false;
                }
                MCB_refreshComboBoxTOSAVE.IsEditable = EditingMode && MCB_refreshComboBoxTOSAVE.IsEditEnabled;
                RaisePropertyChanged(() => SettingsViewItemSource);
            }


        }
        private int BattViewSettingsAccessApply()
        {
            UIAccessControlUtility accessControlUtility = new UIAccessControlUtility();
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_setPA, Batt_isPA, mainItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_RTSampleTime, Batt_autoLogTime, mainItemSource);

            accessControlUtility
                .DoApplyAccessControl
                (ControlObject.UserAccess.Batt_enablePostSensor,
                 Batt_enableExtTempSensingEnable, mainItemSource);
            accessControlUtility
                .DoApplyAccessControl
                (ControlObject.UserAccess.Batt_enablePostSensor,
                 Batt_disableIntercell, mainItemSource);

            if (BATT_temperatureControl != null)
            {
                accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_onlyForEnginneringTeam, BATT_temperatureControl, mainItemSource);
            }
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_EnableEL, Batt_enableElectrolyeSensingEnable, mainItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_setHallEffect, Batt_enableHallEffectSensingYes, mainItemSource);

            if (CanShowNumberOfCables())
            {
                accessControlUtility
                    .DoApplyAccessControl
                    (AccessLevelConsts.write, Batt_NumberOfCables, mainItemSource);
            }

            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_enablePLC, Batt_enablePLCEnable, mainItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_TemperatureFormat, Batt_temperatureFormat, mainItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_dayLightSaving_Enable, Batt_DayLightSaving_Enable, mainItemSource);

            if (accessControlUtility.GetSavedCount() == 0)
            {
                ShowEdit = false;
            }

            return accessControlUtility.GetVisibleCount();
        }

        bool CanShowNumberOfCables()
        {
            var config = BattViewQuantum.Instance.GetBATTView().Config;

            bool canShowNumberOfCables = (Convert.ToBoolean(config.isPA)) &&
                (Convert.ToBoolean(config.enableHallEffectSensing));

            return canShowNumberOfCables;
        }

        void RefreshList()
        {
            if (IsBattView)
            {
                foreach (var item in SettingsViewItemSource)
                {
                    item.IsEditable = item.IsEditEnabled && EditingMode;
                    item.Text = item.SubTitle;
                    if (item.CellType == ACUtility.CellTypes.LabelSwitch)
                    {
                        item.IsSwitchEnabled = (item.Text == AppResources.yes) ? true : false;
                    }
                }
            }
            else
            {
                DoApplyAccessControlForMCB_ResetLCDCalibration();
                foreach (var item in SettingsViewItemSource)
                {
                    if (item.EditableCellType == ACUtility.CellTypes.LabelSwitch)
                    {
                        item.IsSwitchEnabled = (item.Text == AppResources.yes) ? true : false;
                        item.IsEditable = item.IsEditEnabled && EditingMode;
                        item.Text = item.SubTitle;
                        if (item.EnableItemsList != null && item.EnableItemsList.Count > 0)
                        {
                            ExecuteSwitchValueChanged(item);
                        }
                    }
                    else if (item.Index == 2)
                    {

                    }
                    else
                    {
                        item.IsEditable = item.IsEditEnabled && EditingMode;
                        item.Text = item.SubTitle;
                    }
                }
            }
        }

        void OnListSelectorMessage(ListSelectorMessage obj)
        {
            if (_mListSelector != null)
            {
                Mvx.Resolve<IMvxMessenger>().Unsubscribe<ListSelectorMessage>(_mListSelector);
                _mListSelector = null;
                SettingsViewItemSource[obj.SelectedItemindex].Text = obj.SelectedItem;
                if (SettingsViewItemSource[obj.SelectedItemindex].CellType == CellTypes.ListSelector)
                {
                    SettingsViewItemSource[obj.SelectedItemindex].Text = obj.SelectedItem;
                    SettingsViewItemSource[obj.SelectedItemindex].SelectedIndex = obj.SelectedIndex;
                }
                RaisePropertyChanged(() => SettingsViewItemSource);

            }
        }

        //
        public IMvxCommand EditBtnClickCommand
        {
            get { return new MvxCommand(OnEditClick); }
        }

        void OnEditClick()
        {
            EditingMode = true;
            RefreshList();
            RaisePropertyChanged(() => SettingsViewItemSource);
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
                    if (Batt_VerfiyBattViewSettings())
                    {
                        ACUserDialogs.ShowProgress();
                        BattViewCommunicationTypes caller = BattViewCommunicationTypes.NOCall;
                        try
                        {
                            Batt_SaveIntoBattViewSettings();
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine(ex.Message);
                        }

                        bool arg1 = false;
                        caller = BattViewCommunicationTypes.saveConfig;

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
                                        Batt_loadBattViewSettings();
                                        RaisePropertyChanged(() => SettingsViewItemSource);
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
                    if (MCB_VerfiyChargerSettings())
                    {
                        ACUserDialogs.ShowProgress();
                        McbCommunicationTypes caller = McbCommunicationTypes.NOCall;
                        bool arg1 = false;
                        try
                        {
                            MCB_SaveIntoChargerSettings();
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
                                        MCB_loadChargerSettings();
                                        RaisePropertyChanged(() => SettingsViewItemSource);
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
            foreach (var item in SettingsViewItemSource)
            {
                item.SubTitle = string.Empty;
            }
        }
        public IMvxCommand BackBtnClickCommand
        {
            get { return new MvxCommand(OnCancelClick); }
        }


        public IMvxCommand CancelBtnClickCommand
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
            RaisePropertyChanged(() => SettingsViewItemSource);
        }

        private bool CheckForEditedChanges()
        {
            bool textChanged = false;
            foreach (var item in SettingsViewItemSource)
            {
                if (item.Text != item.SubTitle)
                {
                    textChanged = true;
                }
            }

            return textChanged;
        }

        /// <summary>
        /// Ons the back button click.
        /// </summary>
        public void OnBackButtonClick()
        {
            ShowViewModel<SettingsViewModel>(new { pop = "pop" });
        }

        /// <summary>
        /// Loads the BATTView Settings data
        /// </summary>
        void Batt_loadBattViewSettings()
        {
            BattViewObject currentBattView = BattViewQuantum.Instance.GetBATTView();
            Batt_autoLogTime.SubTitle = Batt_autoLogTime.Text = currentBattView.Config.autoLogTime.ToString();
            bool flag = currentBattView.Config.isPA != 0x00;
            Batt_isPA.IsSwitchEnabled = flag;


            flag = currentBattView.Config.enableExtTempSensing != 0x00;
            Batt_enableExtTempSensingEnable.IsSwitchEnabled = flag;

            Batt_disableIntercell.IsSwitchEnabled = !currentBattView.Config.disableIntercell;

            if (BattViewQuantum.Instance.GetBATTView().FirmwareRevision >= 2.09f)
            {
                if (BATT_temperatureControl == null)
                {
                    BATT_temperatureControl = new ListViewItem()
                    {
                        Title = AppResources.temperature_fallback_control,
                        Index = 8,
                        DefaultCellType = CellTypes.LabelLabel,
                        EditableCellType = CellTypes.ListSelector,
                        ListSelectorType = ACUtility.ListSelectorType.TemperatureFallbackControl,
                        ListSelectionCommand = ListSelectorCommand
                    };
                }

                try
                {
                    int tempcontrolIndex = Convert.ToInt32(BattViewQuantum.Instance.GetBATTView().Config.temperatureControl);
                    BATT_temperatureControl.SelectedIndex = tempcontrolIndex;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    BATT_temperatureControl.SelectedIndex = 9;
                }
                BATT_temperatureControl.SelectedItem = ACUtility.Instance.TemperatureFallbackControllValues[BATT_temperatureControl.SelectedIndex];

            }

            flag = currentBattView.Config.enableElectrolyeSensing != 0x00;
            Batt_enableElectrolyeSensingEnable.IsSwitchEnabled = flag;

            flag = currentBattView.Config.enableHallEffectSensing != 0x00;
            Batt_enableHallEffectSensingYes.IsSwitchEnabled = flag;

            if (currentBattView.Config.HallEffectScale > 1.5f)
            {
                Batt_NumberOfCables.SelectedIndex = 1;
                Batt_NumberOfCables.SubTitle = Batt_NumberOfCables.Items[1].ToString();
            }
            else
            {
                Batt_NumberOfCables.SelectedIndex = 0;
                Batt_NumberOfCables.SubTitle = Batt_NumberOfCables.Items[0].ToString();
            }


            flag = currentBattView.Config.enablePLC != 0x00;
            Batt_enablePLCEnable.IsSwitchEnabled = flag;

            flag = currentBattView.Config.enableDayLightSaving != 0x00;
            Batt_DayLightSaving_Enable.IsSwitchEnabled = flag;


            flag = currentBattView.Config.temperatureFormat != 0x00;
            if (flag)
            {
                Batt_temperatureFormat.SelectedIndex = 0;
            }
            else
            {
                Batt_temperatureFormat.SelectedIndex = 1;
            }
            Batt_temperatureFormat.SelectedItem = ACUtility.Instance.TemperatureUnits[Batt_temperatureFormat.SelectedIndex];

            Batt_VerfiyBattViewSettings();
        }

        bool Batt_VerfiyBattViewSettings()
        {
            verifyControl = new VerifyControl();

            //TODO Valication - Check Batt_autoLogTime whether is an Integer or Not
            verifyControl.VerifyInteger(Batt_autoLogTime, Batt_autoLogTime, 1, 65535);
            return !verifyControl.HasErrors();
        }

        void Batt_SaveIntoBattViewSettings()
        {
            if (!BattViewQuantum.Instance.BattView_quickSerialNumberCheckStrict())
                return;
            BattViewQuantum.Instance.GetBATTView().Config.autoLogTime = ushort.Parse(Batt_autoLogTime.Text);

            BattViewQuantum.Instance.GetBATTView().Config.isPA = (byte)(Batt_isPA.IsSwitchEnabled ? 0x01 : 0x00);
            BattViewQuantum.Instance.GetBATTView().Config.enableExtTempSensing = (byte)(Batt_enableExtTempSensingEnable.IsSwitchEnabled ? 0x01 : 0x00);
            BattViewQuantum.Instance.GetBATTView().Config.disableIntercell = !Batt_disableIntercell.IsSwitchEnabled;
            BattViewQuantum.Instance.GetBATTView().Config.enableElectrolyeSensing = (byte)(Batt_enableElectrolyeSensingEnable.IsSwitchEnabled ? 0x01 : 0x00);
            BattViewQuantum.Instance.GetBATTView().Config.enableHallEffectSensing =
                               (byte)(Batt_enableHallEffectSensingYes.IsSwitchEnabled ? 0x01 : 0x00);

            if (BattViewQuantum.Instance.GetBATTView().Config.enableHallEffectSensing != 0x00)
            {
                int numberOfCablesIndex = Batt_NumberOfCables.SelectedIndex;
                switch (numberOfCablesIndex)
                {
                    case 0:
                        BattViewQuantum.Instance.GetBATTView().Config.HallEffectScale = 1.0f;

                        break;

                    case 1:
                        BattViewQuantum.Instance.GetBATTView().Config.HallEffectScale = 2.0f;

                        break;
                }
            }

            BattViewQuantum.Instance.GetBATTView().Config.enablePLC = (byte)(Batt_enablePLCEnable.IsSwitchEnabled ? 0x01 : 0x00);
            BattViewQuantum.Instance.GetBATTView().Config.enableDayLightSaving = (byte)(Batt_DayLightSaving_Enable.IsSwitchEnabled ? 0x01 : 0x00);
            BattViewQuantum.Instance.GetBATTView().Config.temperatureFormat = (byte)(Batt_temperatureFormat.Text.Equals("Fahrenheit") ? 0x01 : 0x00);

            if (BattViewQuantum.Instance.GetBATTView().FirmwareRevision >= 2.09f)
            {
                if (BATT_temperatureControl.SelectedIndex != -1)
                {
                    BattViewQuantum.Instance.GetBATTView().Config.temperatureControl = Convert.ToByte(BATT_temperatureControl.SelectedIndex);
                }
            }
        }

        #region start charger load,save here

        void MCB_loadChargerSettings()
        {
            if (!MCBQuantum.Instance.MCB_quickSerialNumberCheckStrict())
                return;
            MCBobject activeMCB = MCBQuantum.Instance.GetMCB();

            MCB_IR_val_TextBox.SubTitle = MCB_IR_val_TextBox.Text = activeMCB.Config.IR.ToString();
            if (activeMCB.Config.autoStartEnable)
            {
                MCB_AutoStartCustomRadioButtonTOSAVE.IsSwitchEnabled = true;
            }
            else
            {
                MCB_AutoStartCustomRadioButtonTOSAVE.IsSwitchEnabled = false;
            }
            MCB_autoStartTime_TextBoxTOSAVE.SubTitle = MCB_autoStartTime_TextBoxTOSAVE.Text = activeMCB.Config.autoStartCountDownTimer;

            if (activeMCB.Config.enableRefreshCycleAfterEQ)
            {
                MCB_Refresh_AfterEQ_Radio.IsSwitchEnabled = true;
            }
            else
            {
                MCB_Refresh_AfterEQ_Radio.IsSwitchEnabled = false;
            }

            if (activeMCB.Config.enableRefreshCycleAfterFI)
            {
                MCB_Refresh_AfterFI_Radio.IsSwitchEnabled = true;
            }
            else
            {
                MCB_Refresh_AfterFI_Radio.IsSwitchEnabled = false;
            }

            //add items refresh list
            MCB_refreshComboBoxTOSAVE.Items = new List<object>();
            MCB_refreshComboBoxTOSAVE.Items.Clear();
            for (int i = ControlObject.FormLimits.rfTimerStart; i <= ControlObject.FormLimits.rfTimerEnd; i += ControlObject.FormLimits.rfTimerStep)
            {
                if (i != 0)
                    MCB_refreshComboBoxTOSAVE.Items.Add(String.Format("{0:00}:{1:00}", i / 4, (i % 4) * 15));
                else
                    MCB_refreshComboBoxTOSAVE.Items.Add(String.Format("00:01"));
            }
            MCB_refreshComboBoxTOSAVE.SelectedItem = activeMCB.Config.refreshTimer.ToString();
            MCB_refreshComboBoxTOSAVE.SelectedIndex = MCB_refreshComboBoxTOSAVE.Items.FindIndex(o => ((string)o).Equals(MCB_refreshComboBoxTOSAVE.SelectedItem));
            if (MCB_refreshComboBoxTOSAVE.SelectedIndex == -1)
            {

                device_LoadWarnings.Add("Refresh Timer is out of Range");
            }


            if (activeMCB.Config.temperatureSensorInstalled)
            {
                MCB_TempSensorEnable_Radio.IsSwitchEnabled = true;
            }
            else
            {
                MCB_TempSensorEnable_Radio.IsSwitchEnabled = false;
            }

            if (activeMCB.Config.enableAutoDetectMultiVoltage)
            {
                MCB_MultiVoltageEnableRadio.IsSwitchEnabled = true;

            }
            else
            {
                MCB_MultiVoltageEnableRadio.IsSwitchEnabled = false;
            }

            if (activeMCB.Config.temperatureFormat)
            {
                MCB_TemperatureFormatcelsiusRadio.SelectedIndex = 1;
            }
            else
            {
                MCB_TemperatureFormatcelsiusRadio.SelectedIndex = 0;
            }
            MCB_TemperatureFormatcelsiusRadio.SelectedItem = ACUtility.Instance.TemperatureUnits[MCB_TemperatureFormatcelsiusRadio.SelectedIndex];
            if (activeMCB.Config.daylightSaving)
            {
                MCB_DayLightSavingEnableRadio.IsSwitchEnabled = true;
            }
            else
            {
                MCB_DayLightSavingEnableRadio.IsSwitchEnabled = false;
            }

            MCB_enablePLC_enableRadio.IsEditEnabled = activeMCB.Config.enablePLC;
            MCB_enablePLC_enableRadio.IsSwitchEnabled = activeMCB.Config.enablePLC;

            if (ControlObject.UserAccess.MCB_RunBattViewCal == AccessLevelConsts.write)
            {
                //using charger battery info screen, need validate there
                //MCB_AdminActionsCalibrateBattViewButton.Enabled = activeMCB.MCBConfig.enablePLC;
            }

            MCB_enableManualEQ_enableRadio.IsSwitchEnabled = activeMCB.Config.enableManualEQ;
            MCB_enableManualDesulfate_enableRadio.IsSwitchEnabled = activeMCB.Config.enableManualDesulfate;
            MCB_enablepushbutton_enableRadio.IsSwitchEnabled = !activeMCB.Config.disablePushButton;
            //MCB_enableLED_enableRadio.IsSwitchEnabled = !activeMCB.Config.disableLED;

            MCB_OverrideFIEQSched_YesRadio.IsSwitchEnabled = activeMCB.Config.chargerOverrideBattviewFIEQsched;

            MCB_ignoreBAttviewSOC_YesRadio.IsSwitchEnabled = activeMCB.Config.ignoreBATTViewSOC;

            MCB_BattviewAutoCalibration_EnableRadio.IsSwitchEnabled = activeMCB.Config.battviewAutoCalibrationEnable;

            if (activeMCB.FirmwareRevision >= 2.21f)
            {
                int indexMCB_RemoteStopButton = Convert.ToInt32(activeMCB.Config.OCD_Remote);
                MCB_RemoteStopButton.SelectedIndex = indexMCB_RemoteStopButton;
                MCB_RemoteStopButton.SubTitle =
                                        MCB_RemoteStopButton.Items[indexMCB_RemoteStopButton].ToString();
            }
            MCB_minSteps_ComboBox.Items = new List<object>();
            this.MCB_minSteps_ComboBox.Items.AddRange(new object[] {
            "default",
            "5",
            "10",
            "15",
            "20",
            "25",
            "30",
            "35",
            "40",
            "45",
            "50",
            "55",
            "60"});

            if (activeMCB.Config.cc_ramping_min_steps == 0)
            {
                MCB_minSteps_ComboBox.SelectedIndex = 0;//default
                MCB_minSteps_ComboBox.SelectedItem = MCB_minSteps_ComboBox.Items[MCB_minSteps_ComboBox.SelectedIndex].ToString();
            }
            else
            {
                MCB_minSteps_ComboBox.SelectedItem = activeMCB.Config.cc_ramping_min_steps.ToString();
                MCB_minSteps_ComboBox.SelectedIndex = MCB_minSteps_ComboBox.Items.FindIndex(o => ((string)o).Equals(MCB_minSteps_ComboBox.SelectedItem));
                if (MCB_minSteps_ComboBox.SelectedIndex == -1)
                {
                    device_LoadWarnings.Add("CC Ramping Min. Steps is out of Range");

                }
            }

            if (activeMCB.FirmwareRevision < 2.14f || ControlObject.UserAccess.MCB_CC_CurrentRate == AccessLevelConsts.noAccess)
            {
                MCB_minSteps_ComboBox.IsVisible = false;

            }
            else
            {
                MCB_minSteps_ComboBox.IsVisible = true;
            }

            if (activeMCB.FirmwareRevision < 2.12f || ControlObject.UserAccess.MCB_FI_EQ_sched_CustomSettings == AccessLevelConsts.noAccess)
            {
                MCB_OverrideFIEQSched_YesRadio.IsVisible = false;
            }
            else
            {
                MCB_OverrideFIEQSched_YesRadio.IsVisible = true;
            }
            if (activeMCB.FirmwareRevision < 2.13f)
            {
                MCB_ignoreBAttviewSOC_YesRadio.IsVisible = false;
                MCB_BattviewAutoCalibration_EnableRadio.IsVisible = false;
            }
            else
            {
                MCB_ignoreBAttviewSOC_YesRadio.IsVisible = true;
                MCB_BattviewAutoCalibration_EnableRadio.IsVisible = true;
            }

            MCB_VerfiyChargerSettings();

        }

        bool MCB_VerfiyChargerSettings()
        {
            if (!MCBQuantum.Instance.MCB_quickSerialNumberCheckStrict())
                return false;
            verifyControl = new VerifyControl();

            verifyControl.VerifyFloatNumber(MCB_IR_val_TextBox, MCB_IR_val_TextBox, 0.0f, 9999.99f);
            //MCB_AutoStartCustomRadioButtonTOSAVE check box
            verifyControl.VerifyInteger(MCB_autoStartTime_TextBoxTOSAVE, MCB_autoStartTime_TextBoxTOSAVE, 9, 99);
            //MCB_Refresh_AfterEQ_Radio check
            //MCB_Refresh_AfterFI_Radio check
            verifyControl.VerifyComboBox(MCB_refreshComboBoxTOSAVE);


            //MCB_TempSensorEnable_Radio check
            //MCB_MultiVoltageEnableRadio check
            //MCB_TemperatureFormatcelsiusRadio check
            //MCB_DayLightSavingEnableRadio check

            return !verifyControl.HasErrors();
        }

        int ChargerSettingsAccessApply()
        {
            UIAccessControlUtility accessControlUtility = new UIAccessControlUtility();

            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_IR, MCB_IR_val_TextBox, mainItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_autoStart_count, MCB_autoStartTime_TextBoxTOSAVE, mainItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_autoStart_Enable, MCB_AutoStartCustomRadioButtonTOSAVE, mainItemSource);
            //accessControlUtility.doApplyAccessControl(ControlObject.MCB_access.MCB_autoSatrt_DaysMask, , MCB_TableLayoutChargerSettings) ;
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_refreshEnable, MCB_Refresh_AfterEQ_Radio, mainItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_refreshEnable, MCB_Refresh_AfterFI_Radio, mainItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_refreshTimer, MCB_refreshComboBoxTOSAVE, mainItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_TemperatureFormat, MCB_TemperatureFormatcelsiusRadio, mainItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_TemperatureSensorEnable, MCB_TempSensorEnable_Radio, mainItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_multiVoltage, MCB_MultiVoltageEnableRadio, mainItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_dayLightSaving_Enable, MCB_DayLightSavingEnableRadio, mainItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_enablePLC, MCB_enablePLC_enableRadio, mainItemSource);

            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_enableLauncher, MCB_enableManualEQ_enableRadio, mainItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_enableLauncher, MCB_enableManualDesulfate_enableRadio, mainItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_DisablePushButton, MCB_enablepushbutton_enableRadio, mainItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_EnableLED, MCB_enableLED_enableRadio, mainItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_FI_EQ_sched_CustomSettings, MCB_OverrideFIEQSched_YesRadio, mainItemSource);

            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_CC_CurrentRate, MCB_minSteps_ComboBox, mainItemSource);

            if ((MCB_Refresh_AfterEQ_Radio.IsSwitchEnabled || MCB_Refresh_AfterFI_Radio.IsSwitchEnabled))
            {
                MCB_refreshComboBoxTOSAVE.IsEditEnabled = true;
            }
            else
            {
                MCB_refreshComboBoxTOSAVE.IsEditEnabled = false;
            }

            if (accessControlUtility.GetSavedCount() == 0)
            {
                ShowEdit = false;
            }

            DoApplyAccessControlForMCB_ResetLCDCalibration();
            int returncount = accessControlUtility.GetVisibleCount();

            if (MCB_ignoreBAttviewSOC_YesRadio.IsVisible)
            {
                accessControlUtility.DoApplyAccessControl(AccessLevelConsts.write, MCB_ignoreBAttviewSOC_YesRadio, mainItemSource);
            }
            if (MCB_BattviewAutoCalibration_EnableRadio.IsVisible)
            {
                accessControlUtility.DoApplyAccessControl(AccessLevelConsts.write, MCB_BattviewAutoCalibration_EnableRadio, mainItemSource);
            }

            accessControlUtility
                .DoApplyAccessControl
                (AccessLevelConsts.write, MCB_RemoteStopButton, mainItemSource);

            accessControlUtility
                .DoApplyAccessControl
                (AccessLevelConsts.write, MCB_UpdateCableSettingsButton, mainItemSource);
            accessControlUtility
                .DoApplyAccessControl
                (AccessLevelConsts.write, MCB_LoadPlcFirmwareAndRestartButton, mainItemSource);

            return returncount;
        }

        void DoApplyAccessControlForMCB_ResetLCDCalibration()
        {
            if (ControlObject.UserAccess.MCB_ResetLCDCalibration == AccessLevelConsts.noAccess)
            {
                IsVisible_MCB_ResetLCDCalibration = false;
            }
            else
            {
                IsVisible_MCB_ResetLCDCalibration = true;
            }
            if (ControlObject.UserAccess.MCB_ResetLCDCalibration == AccessLevelConsts.readOnly)
            {
                IsEditEnabled_MCB_ResetLCDCalibration = false;
            }
            else
            {
                IsEditEnabled_MCB_ResetLCDCalibration = EditingMode;
            }
        }

        void MCB_SaveIntoChargerSettings()
        {
            if (!MCBQuantum.Instance.MCB_quickSerialNumberCheckStrict())
                return;
            MCBobject activeMCB = MCBQuantum.Instance.GetMCB();
            activeMCB.Config.IR = float.Parse(MCB_IR_val_TextBox.Text);
            activeMCB.Config.autoStartEnable = MCB_AutoStartCustomRadioButtonTOSAVE.IsSwitchEnabled;
            activeMCB.Config.autoStartCountDownTimer = MCB_autoStartTime_TextBoxTOSAVE.Text;
            activeMCB.Config.enableRefreshCycleAfterEQ = MCB_Refresh_AfterEQ_Radio.IsSwitchEnabled;
            activeMCB.Config.enableRefreshCycleAfterFI = MCB_Refresh_AfterFI_Radio.IsSwitchEnabled;
            activeMCB.Config.refreshTimer = (string)MCB_refreshComboBoxTOSAVE.Text;
            activeMCB.Config.temperatureSensorInstalled = MCB_TempSensorEnable_Radio.IsSwitchEnabled;
            activeMCB.Config.enableAutoDetectMultiVoltage = MCB_MultiVoltageEnableRadio.IsSwitchEnabled;
            bool flag = MCB_TemperatureFormatcelsiusRadio.SelectedIndex == 0 ? false : true;
            activeMCB.Config.temperatureFormat = flag;
            activeMCB.Config.daylightSaving = MCB_DayLightSavingEnableRadio.IsSwitchEnabled;
            activeMCB.Config.enablePLC = MCB_enablePLC_enableRadio.IsSwitchEnabled;
            if (ControlObject.UserAccess.MCB_RunBattViewCal == AccessLevelConsts.write)
            {
                //using charger battery info screen need validate there
                //MCB_AdminActionsCalibrateBattViewButton.Enabled = activeMCB.MCBConfig.enablePLC;
            }
            activeMCB.Config.enableManualDesulfate = MCB_enableManualDesulfate_enableRadio.IsSwitchEnabled;
            activeMCB.Config.enableManualEQ = MCB_enableManualEQ_enableRadio.IsSwitchEnabled;
            activeMCB.Config.disablePushButton = !MCB_enablepushbutton_enableRadio.IsSwitchEnabled;
            //activeMCB.Config.disableLED = !MCB_enableLED_enableRadio.IsSwitchEnabled;
            if (activeMCB.FirmwareRevision > 2.11f)
            {
                activeMCB.Config.chargerOverrideBattviewFIEQsched = MCB_OverrideFIEQSched_YesRadio.IsSwitchEnabled;
            }
            if (activeMCB.FirmwareRevision > 2.12f)
            {
                activeMCB.Config.ignoreBATTViewSOC = MCB_ignoreBAttviewSOC_YesRadio.IsSwitchEnabled;
                activeMCB.Config.battviewAutoCalibrationEnable = MCB_BattviewAutoCalibration_EnableRadio.IsSwitchEnabled;


            }
            if (activeMCB.FirmwareRevision > 2.13f)
            {
                if (MCB_minSteps_ComboBox.SelectedIndex == 0)
                    activeMCB.Config.cc_ramping_min_steps = 0;
                else
                {
                    activeMCB.Config.cc_ramping_min_steps = byte.Parse((string)MCB_minSteps_ComboBox.Text);

                }
            }

            if (activeMCB.FirmwareRevision > 2.21f)
            {
                activeMCB.Config.OCD_Remote =
                             Convert.ToByte(MCB_RemoteStopButton.SelectedIndex);
            }
        }

        public IMvxCommand MCB_ResetLCDCalibrationClickCommand
        {
            get
            {
                return new MvxCommand(async () =>
                {
                    await MCB_ResetLCDCalibration();
                });
            }
        }

        async Task MCB_ResetLCDCalibration()
        {
            if (NetworkCheck())
            {
                if (!MCBQuantum.Instance.MCB_quickSerialNumberCheckStrict())
                    return;

                ACUserDialogs.ShowProgress();
                McbCommunicationTypes caller = McbCommunicationTypes.ResetLCDCalibration;
                object arg1 = null;

                List<object> arguments = new List<object>();
                arguments.Add(caller);
                arguments.Add(arg1);
                arguments.Add(false);
                List<object> results = new List<object>();
                try
                {
                    results = await MCBQuantum.Instance.CommunicateMCB(arguments);
                    if (results.Count > 0)
                    {
                        var status = (CommunicationResult)results[2];
                        if (status == CommunicationResult.CHARGER_BUSY)
                        {
                            ACUserDialogs.ShowAlertWithTitleAndOkButton(AppResources.charger_is_busy);
                        }
                        else if (status == CommunicationResult.OK)
                        {
                            //TODO: Navigate to ConnectToDevice
                            //success reset
                            ACUserDialogs.HideProgress();
                            Mvx.Resolve<IMvxMessenger>().Publish(new DeviceResetMessage(this, MCBQuantum.Instance.GetMCB().IPAddress));
                            ShowViewModel<ConnectToDeviceViewModel>(new { popto = "popto" });
                            ACUserDialogs.ShowAlertWithTitleAndOkButton(AppResources.charger_restarting);
                            return;
                        }
                        else
                        {
                            ACUserDialogs.ShowAlertWithTitleAndOkButton(AppResources.reset_lcd_failed);
                        }
                    }


                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
                ACUserDialogs.HideProgress();
            }
        }

        #endregion

        public IMvxCommand ListSelectorCommand
        {
            get { return new MvxCommand<ListViewItem>(ExecuteListSelectorCommand); }
        }

        void ExecuteListSelectorCommand(ListViewItem item)
        {
            _mListSelector = Mvx.Resolve<IMvxMessenger>().Subscribe<ListSelectorMessage>(OnListSelectorMessage);

            if (EditingMode && CellTypes.ListSelector == item.CellType)
            {
                if (item.ListSelectorType == ACUtility.ListSelectorType.TemperatureFormate)
                {
                    ShowViewModel<ListSelectorViewModel>(new { type = ACUtility.ListSelectorType.TemperatureFormate, selectedItemIndex = SettingsViewItemSource.IndexOf(item), ItemSourceStr = JsonConvert.SerializeObject(item.Items) });
                }
                else if (item.ListSelectorType == ACUtility.ListSelectorType.TemperatureFallbackControl)
                {
                    ShowViewModel<ListSelectorViewModel>(new { type = ACUtility.ListSelectorType.TemperatureFallbackControl, selectedItemIndex = SettingsViewItemSource.IndexOf(item), ItemSourceStr = JsonConvert.SerializeObject(item.Items) });
                }
                else
                {
                    ShowViewModel<ListSelectorViewModel>(new { selectedItemIndex = SettingsViewItemSource.IndexOf(item), ItemSourceStr = JsonConvert.SerializeObject(item.Items), title = item.Title });
                }
            }
        }

        public IMvxCommand MCB_UpdateCableSettingsClickCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    MCB_UpdateCableSettings();
                });
            }
        }

        void MCB_UpdateCableSettings()
        {
            ShowHideCableSettings(true);
        }

        public ICommand CalculateSelectionCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    ExecuteCalculateSelectionCommand();
                });
            }
        }

        void ExecuteCalculateSelectionCommand()
        {
            try
            {
                CalculateAndUpdateIR();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }

            ShowHideCableSettings(false);
        }

        void CalculateAndUpdateIR()
        {
            float ir = CalculateIR();

            MCB_IR_val_TextBox.SubTitle = ir.ToString();
            MCB_IR_val_TextBox.Text = ir.ToString();
        }

        float CalculateIR()
        {
            float ir = 0.0f;

            float chargerCableLength = float.Parse(CableSettings_ChargerCableLength.Text);
            bool isDualCable = CableSettings_CableGauge.IsSwitchEnabled;
            int nmberOfCablesIndex = CableSettings_NumberOfCables.SelectedIndex;
            float batteryCableLength = float.Parse(CableSettings_BatteryCableLength.Text);

            ir = chargerCableLength + batteryCableLength;

            if (isDualCable)
                ir *= 4;
            else
                ir *= 2;

            switch (nmberOfCablesIndex)
            {
                case 0:
                    ir *= 0.105f;

                    break;

                case 1:
                    ir *= 0.084f;

                    break;

                case 2:
                    ir *= 0.067f;

                    break;

                case 3:
                    ir *= 0.053f;

                    break;
            }

            return ir;
        }

        public ICommand CancelSelectionCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    ExecuteCancelSelectionCommand();
                });
            }
        }

        void ExecuteCancelSelectionCommand()
        {
            ShowHideCableSettings(false);
        }

        void ShowHideCableSettings(bool show)
        {
            if (show)
            {
                IsCableSettings = true;
                EditingMode = true;
            }
            else
            {
                IsCableSettings = false;
            }
        }

        void ChangeList()
        {
            if (IsCableSettings)
            {
                SettingsViewItemSource = cableSettingsItemSource;
            }
            else
            {
                SettingsViewItemSource = mainItemSource;
            }

            RaisePropertyChanged(() => SettingsViewItemSource);
        }

        public IMvxCommand MCB_LoadPlcFirmwareAndRestartClickCommand
        {
            get
            {
                return new MvxCommand(async () =>
                {
                    await MCB_LoadPlcFirmwareAndRestart();
                });
            }
        }

        async Task MCB_LoadPlcFirmwareAndRestart()
        {
            IsBusy = true;

            byte[] serials = null;
            Firmware firmwareManager = new Firmware();
            if (firmwareManager.GetPLCBinaries(ref serials) != FirmwareResult.fileOK)
            {
                IsBusy = false;

                string msg = AppResources.bad_firmware_encoding;

                ACUserDialogs.ShowAlertWithTitleAndOkButton(msg);
            }
            else
            {
                McbCommunicationTypes caller = McbCommunicationTypes.loadPLC;
                byte[] arg1 = serials;

                await ComunicateWithMcb(caller, arg1);
            }
        }

        async Task ComunicateWithMcb(McbCommunicationTypes caller, byte[] arg1)
        {
            List<object> arguments = new List<object>
            {
                caller,
                arg1
            };

            try
            {
                await TryComunicateWithMcb(arguments);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        async Task TryComunicateWithMcb(List<object> arguments)
        {
            var results = await MCBQuantum.Instance.CommunicateMCB(arguments);

            IsBusy = false;

            if (results.Count > 0)
            {
                OnNonZeroResults(results);
            }
        }

        void OnNonZeroResults(List<object> results)
        {
            var callerStatus = results[3];
            var status = (CommunicationResult)results[2];

            if (McbCommunicationTypes.restartDevice.Equals(callerStatus))
            {
                OnRestart(status);
            }
            else if (McbCommunicationTypes.loadPLC.Equals(callerStatus))
            {
                OnLoadPlc(status);
            }
        }

        void OnRestart(CommunicationResult status)
        {
            if (status == CommunicationResult.CHARGER_BUSY)
            {
                ACUserDialogs.ShowAlertWithTitleAndOkButton(AppResources.charger_is_busy);
            }
            else if (IsReadyToRestart(status))
            {
                ShowViewModel<ConnectToDeviceViewModel>(new { popto = "popto" });

                ACUserDialogs.ShowAlertWithTitleAndOkButton(AppResources.charger_restarting);
            }
        }

        bool IsReadyToRestart(CommunicationResult status)
        {
            return (status == CommunicationResult.OK) ||
                (MCBQuantum.Instance.GetMCB().FirmwareRevision < 2.05f);
        }

        void OnLoadPlc(CommunicationResult status)
        {
            if (status == CommunicationResult.OK)
            {
                ACUserDialogs.ShowAlertWithTitleAndOkButton(AppResources.charger_is_reflushing_plc_firmware);
            }
            else
            {
                ACUserDialogs.ShowAlertWithTitleAndOkButton(AppResources.cant_update_firmware);
            }
        }
    }
}
