using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static actchargers.ACUtility;

namespace actchargers
{
    public abstract class SettingsBaseSubController : SiteViewSettingsBaseSubController
    {
        public event EventHandler<bool> OnIsResetLcdCalibrationVisibleChanged;
        public event EventHandler<bool> OnIsResetLcdCalibrationEditEnabledChanged;
        public event EventHandler OnNavigatingToCableSettings;

        #region BattView Items

        internal ListViewItem Batt_autoLogTime;
        internal ListViewItem Batt_enableExtTempSensingEnable;
        internal ListViewItem Batt_enableElectrolyeSensingEnable;
        internal ListViewItem Batt_enableHallEffectSensingYes;
        internal ListViewItem Batt_enablePLCEnable;
        internal ListViewItem Batt_DayLightSaving_Enable;
        internal ListViewItem Batt_temperatureFormat;
        internal ListViewItem BATT_temperatureControl;

        #endregion

        #region MCB Items

        internal ListViewItem MCB_AutoStartCustomRadioButtonTOSAVE;
        internal ListViewItem MCB_autoStartTime_TextBoxTOSAVE;
        internal ListViewItem MCB_Refresh_AfterEQ_Radio;
        internal ListViewItem MCB_Refresh_AfterFI_Radio;
        internal ListViewItem MCB_refreshComboBoxTOSAVE;
        internal ListViewItem MCB_MultiVoltageEnableRadio;
        internal ListViewItem MCB_TemperatureFormatcelsiusRadio;
        internal ListViewItem MCB_DayLightSavingEnableRadio;
        internal ListViewItem MCB_enablePLC_enableRadio;
        internal ListViewItem MCB_enableManualEQ_enableRadio;
        internal ListViewItem MCB_enableManualDesulfate_enableRadio;
        internal ListViewItem MCB_enablepushbutton_enableRadio;
        internal ListViewItem MCB_LedOptions;
        internal ListViewItem MCB_ignoreBAttviewSOC_YesRadio;
        internal ListViewItem MCB_OverrideFIEQSched_YesRadio;
        internal ListViewItem MCB_minSteps_ComboBox;
        internal ListViewItem MCB_BattviewAutoCalibration_EnableRadio;
        internal ListViewItem MCB_RemoteStopButton;

        #endregion

        bool isResetLcdCalibrationVisible;
        public bool IsResetLcdCalibrationVisible
        {
            get
            {
                return isResetLcdCalibrationVisible;
            }
            set
            {
                isResetLcdCalibrationVisible = value;

                FireOnIsResetLcdCalibrationVisibleChanged();
            }
        }

        bool isResetLcdCalibrationEditEnabled;
        public bool IsResetLcdCalibrationEditEnabled
        {
            get
            {
                return isResetLcdCalibrationEditEnabled;
            }
            set
            {
                isResetLcdCalibrationEditEnabled = value;

                FireOnIsResetLcdCalibrationEditEnabledChanged();
            }
        }

        protected SettingsBaseSubController(bool isBattView, bool isSiteView) : base(isBattView, isSiteView)
        {
        }

        #region Init BattView Items

        internal override void InitSharedBattViewItems()
        {
            Batt_autoLogTime = new ListViewItem()
            {
                Index = 0,
                Title = AppResources.samples_time_span,
                DefaultCellType = CellTypes.LabelLabel,
                EditableCellType = CellTypes.LabelTextEdit,
                TextInputType = InputType.Number
            };
            Batt_enableExtTempSensingEnable = new ListViewItem()
            {
                Index = 2,
                Title = AppResources.post_electrolyte_ntc,
                DefaultCellType = CellTypes.LabelLabel,
                EditableCellType = CellTypes.LabelSwitch
            };
            Batt_enableElectrolyeSensingEnable = new ListViewItem()
            {
                Index = 4,
                Title = AppResources.electrolyte_sensing,
                DefaultCellType = CellTypes.LabelLabel,
                EditableCellType = CellTypes.LabelSwitch
            };
            Batt_enableHallEffectSensingYes = new ListViewItem()
            {
                Index = 5,
                Title = AppResources.use_hall_effect_sensor,
                DefaultCellType = CellTypes.LabelLabel,
                EditableCellType = CellTypes.LabelSwitch,
                EnableItemsList = new List<int> { 11 },
                SwitchValueChanged = SwitchValueChanged
            };
            Batt_enablePLCEnable = new ListViewItem()
            {
                Index = 7,
                Title = AppResources.plc,
                DefaultCellType = CellTypes.LabelLabel,
                EditableCellType = CellTypes.LabelSwitch
            };
            Batt_DayLightSaving_Enable = new ListViewItem()
            {
                Index = 8,
                Title = AppResources.daylight_saving,
                DefaultCellType = CellTypes.LabelLabel,
                EditableCellType = CellTypes.LabelSwitch
            };
            Batt_temperatureFormat = new ListViewItem()
            {
                Index = 9,
                Title = AppResources.temperature_format,
                DefaultCellType = CellTypes.LabelLabel,
                EditableCellType = CellTypes.ListSelector,
                ListSelectorType = ListSelectorType.TemperatureFormate,
                ListSelectionCommand = ListSelectorCommand
            };
            BATT_temperatureControl = new ListViewItem()
            {
                Title = AppResources.temperature_fallback_control,
                Index = 10,
                DefaultCellType = CellTypes.LabelLabel,
                EditableCellType = CellTypes.ListSelector,
                ListSelectorType = ListSelectorType.TemperatureFallbackControl,
                ListSelectionCommand = ListSelectorCommand
            };
        }

        #endregion

        #region Init Mcb Items

        internal override void InitSharedMcbItems()
        {
            MCB_AutoStartCustomRadioButtonTOSAVE = new ListViewItem()
            {
                Index = 1,
                Title = AppResources.auto_start,
                DefaultCellType = CellTypes.LabelLabel,
                EditableCellType = CellTypes.LabelSwitch,
                IsVisible = true,
                SwitchValueChanged = SwitchValueChanged,
                EnableItemsList = new List<int>() { 2 }
            };
            MCB_autoStartTime_TextBoxTOSAVE = new ListViewItem()
            {
                Index = 2,
                Title = AppResources.auto_start_count_down,
                DefaultCellType = CellTypes.LabelLabel,
                EditableCellType = CellTypes.LabelTextEdit,
                TextInputType = InputType.Number,
                IsVisible = true,
                TextMaxLength = 2
            };
            MCB_Refresh_AfterEQ_Radio = new ListViewItem()
            {
                Index = 3,
                Title = AppResources.refresh_cycle_after_eq,
                DefaultCellType = CellTypes.LabelLabel,
                EditableCellType = CellTypes.LabelSwitch,
                SwitchValueChanged = SwitchValueChanged,
                IsVisible = true
            };
            MCB_Refresh_AfterFI_Radio = new ListViewItem()
            {
                Index = 4,
                Title = AppResources.refresh_cycle_after_fi,
                DefaultCellType = CellTypes.LabelLabel,
                EditableCellType = CellTypes.LabelSwitch,
                SwitchValueChanged = SwitchValueChanged,
                IsVisible = true
            };
            MCB_refreshComboBoxTOSAVE = new ListViewItem()
            {
                Index = 5,
                Title = AppResources.refresh_frequency,
                DefaultCellType = CellTypes.LabelLabel,
                EditableCellType = CellTypes.ListSelector,
                ListSelectionCommand = ListSelectorCommand,
                IsVisible = true
            };
            MCB_refreshComboBoxTOSAVE.Items = GetMcbRefreshFrequencyList();
            MCB_refreshComboBoxTOSAVE.SelectedIndex = 0;

            MCB_MultiVoltageEnableRadio = new ListViewItem()
            {
                Index = 7,
                Title = AppResources.multi_voltage,
                DefaultCellType = CellTypes.LabelLabel,
                EditableCellType = CellTypes.LabelSwitch,
                IsVisible = true
            };
            MCB_TemperatureFormatcelsiusRadio = new ListViewItem()
            {
                Index = 8,
                Title = AppResources.temperature_format,
                DefaultCellType = CellTypes.LabelLabel,
                EditableCellType = CellTypes.ListSelector,
                ListSelectorType = ListSelectorType.TemperatureFormate,
                ListSelectionCommand = ListSelectorCommand,
                IsVisible = true
            };
            MCB_DayLightSavingEnableRadio = new ListViewItem()
            {
                Index = 9,
                Title = AppResources.daylight_saving,
                DefaultCellType = CellTypes.LabelLabel,
                EditableCellType = CellTypes.LabelSwitch,
                IsVisible = true
            };
            MCB_enablePLC_enableRadio = new ListViewItem()
            {
                Index = 10,
                Title = AppResources.enable_plc,
                DefaultCellType = CellTypes.LabelLabel,
                EditableCellType = CellTypes.LabelSwitch,
                IsVisible = true
            };
            MCB_enableManualEQ_enableRadio = new ListViewItem()
            {
                Index = 12,
                Title = AppResources.allow_manual_eq,
                DefaultCellType = CellTypes.LabelLabel,
                EditableCellType = CellTypes.LabelSwitch,
                IsVisible = true
            };
            MCB_enableManualDesulfate_enableRadio = new ListViewItem()
            {
                Index = 13,
                Title = AppResources.allow_manual_desulfate,
                DefaultCellType = CellTypes.LabelLabel,
                EditableCellType = CellTypes.LabelSwitch,
                IsVisible = true
            };
            MCB_enablepushbutton_enableRadio = new ListViewItem()
            {
                Index = 14,
                Title = AppResources.enable_stop_push_button,
                DefaultCellType = CellTypes.LabelLabel,
                EditableCellType = CellTypes.LabelSwitch,
                IsVisible = true
            };

            MCB_LedOptions = new ListViewItem()
            {
                Index = 15,
                Title = AppResources.led_options,
                DefaultCellType = CellTypes.LabelLabel,
                EditableCellType = CellTypes.ListSelector,
                ListSelectionCommand = ListSelectorCommand,
                Items = BuilsLedOptiobsItems(),
                IsVisible = true
            };

            MCB_ignoreBAttviewSOC_YesRadio = new ListViewItem()
            {
                Index = 15,
                Title = AppResources.charger_control_soc,
                DefaultCellType = CellTypes.LabelLabel,
                EditableCellType = CellTypes.LabelSwitch,
                IsVisible = true
            };
            MCB_OverrideFIEQSched_YesRadio = new ListViewItem()
            {
                Index = 17,
                Title = AppResources.charger_controls_fi_scheduling,
                DefaultCellType = CellTypes.LabelLabel,
                EditableCellType = CellTypes.LabelSwitch,
                IsVisible = true
            };
            MCB_minSteps_ComboBox = new ListViewItem()
            {
                Index = 18,
                Title = AppResources.current_ramp_minimum_steps,
                DefaultCellType = CellTypes.LabelLabel,
                EditableCellType = CellTypes.ListSelector,
                ListSelectionCommand = ListSelectorCommand,
                IsVisible = true
            };
            MCB_minSteps_ComboBox.Items = new List<object>();
            MCB_minSteps_ComboBox.Items.AddRange(
                new object[]
            {
                "Default",
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
                "60"
            }
            );

            MCB_BattviewAutoCalibration_EnableRadio = new ListViewItem()
            {
                Index = 19,
                Title = AppResources.battview_auto_calibration,
                DefaultCellType = CellTypes.LabelLabel,
                EditableCellType = CellTypes.LabelSwitch,
                IsVisible = true
            };
            MCB_RemoteStopButton = new ListViewItem()
            {
                Index = 20,
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
        }

        List<object> GetMcbRefreshFrequencyList()
        {
            var list = new List<object>();

            for (int i = ControlObject.FormLimits.rfTimerStart; i <= ControlObject.FormLimits.rfTimerEnd; i += ControlObject.FormLimits.rfTimerStep)
            {
                if (i != 0)
                    list.Add(string.Format("{0:00}:{1:00}", i / 4, (i % 4) * 15));
                else
                    list.Add(string.Format("00:01"));
            }

            return list;
        }

        List<object> BuilsLedOptiobsItems()
        {
            var list = new List<object>
            {
                AppResources.led_strip,
                AppResources.disabled
            };

            float fw = 0.0f;

            if (!isSiteView)
                fw = MCBQuantum.Instance.GetMCB().FirmwareRevision;

            if (fw >= 2.82f || isSiteView)
                list.Add(AppResources.stack_light);

            return list;
        }

        internal override void ExecuteSwitchValueChanged(ListViewItem item)
        {
            base.ExecuteSwitchValueChanged(item);

            if (!isBattView)
                ChangeAbilityToMcbRefreshFrequency();

            FireOnListChanged();
        }

        void ChangeAbilityToMcbRefreshFrequency()
        {
            bool isEditable = (MCB_Refresh_AfterEQ_Radio.IsSwitchEnabled) ||
                (MCB_Refresh_AfterFI_Radio.IsSwitchEnabled);

            MCB_refreshComboBoxTOSAVE.IsEditEnabled = isEditable;
            MCB_refreshComboBoxTOSAVE.ChangeEditMode(isEditable);
        }

        #endregion

        internal void DoApplyAccessControlForResetLCDCalibration()
        {
            IsResetLcdCalibrationVisible =
                ControlObject.UserAccess.MCB_ResetLCDCalibration != AccessLevelConsts.noAccess;

            IsResetLcdCalibrationEditEnabled =
                ControlObject.UserAccess.MCB_ResetLCDCalibration !=
                             AccessLevelConsts.readOnly && EditingMode;
        }

        public override void LoadDefaults()
        {
        }

        internal abstract Task McbResetLcdCalibration();

        void FireOnIsResetLcdCalibrationVisibleChanged()
        {
            OnIsResetLcdCalibrationVisibleChanged?.Invoke(this, IsResetLcdCalibrationVisible);
        }

        void FireOnIsResetLcdCalibrationEditEnabledChanged()
        {
            OnIsResetLcdCalibrationEditEnabledChanged?.Invoke(this, IsResetLcdCalibrationEditEnabled);
        }

        internal void FireOnNavigatingToCableSettings()
        {
            OnNavigatingToCableSettings?.Invoke(this, EventArgs.Empty);
        }
    }
}
