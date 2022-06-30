using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;
using static actchargers.ACUtility;

namespace actchargers
{
    public class SettingsDeviceSubController : SettingsBaseSubController
    {
        MvxSubscriptionToken cableSettingsMessage;

        bool shouldRestart;

        #region BattView Items

        ListViewItem Batt_isPA;
        ListViewItem Batt_disableIntercell;
        ListViewItem HallEffectReverse;

        #endregion

        #region MCB Items

        ListViewItem MCB_IR_val_TextBox;
        ListViewItem MCB_TempSensorEnable_Radio;
        ListViewItem MCB_DoPlcStackCheck;
        ListViewItem MCB_Enable72V;
        ListViewItem MCB_ReconnectTimer;
        ListViewItem MCB_DaughterCardEnabled;
        ListViewItem MCB_UpdateCableSettingsButton;
        ListViewItem MCB_LoadPlcFirmwareAndRestartButton;
        ListViewItem MCB_BMSBitRate;
        ListViewItem MCB_DefaultBrightness_TextBox;
        ListViewItem MCB_PLCgain;

        #endregion

        public SettingsDeviceSubController(bool isBattView) : base(isBattView, false)
        {
        }

        #region Init BattView Items

        internal override void InitExclusiveBattViewItems()
        {
            Batt_isPA = new ListViewItem()
            {
                Index = 1,
                Title = AppResources.battview_mobile,
                DefaultCellType = CellTypes.LabelLabel,
                EditableCellType = CellTypes.LabelSwitch
            };
            Batt_disableIntercell = new ListViewItem()
            {
                Index = 3,
                Title = AppResources.disable_intercell,
                DefaultCellType = CellTypes.LabelLabel,
                EditableCellType = CellTypes.LabelSwitch
            };
            HallEffectReverse = new ListViewItem()
            {
                Index = 11,
                Title = AppResources.reverse_hall_effect,
                DefaultCellType = CellTypes.Button,
                EditableCellType = CellTypes.Button,
                ListSelectionCommand = HallEffectReverseCommand
            };
        }

        #endregion

        #region Init Mcb Items

        internal override void InitExclusiveMcbItems()
        {
            MCB_IR_val_TextBox = new ListViewItem()
            {
                Index = 0,
                Title = AppResources.cable_resistance,
                DefaultCellType = CellTypes.LabelLabel,
                EditableCellType = CellTypes.LabelTextEdit,
                TextInputType = InputType.Decimal,
                IsVisible = true,
                TextMaxLength = 8
            };
            MCB_TempSensorEnable_Radio = new ListViewItem()
            {
                Index = 6,
                Title = AppResources.temperature_sensor,
                DefaultCellType = CellTypes.LabelLabel,
                EditableCellType = CellTypes.LabelSwitch,
                IsVisible = true
            };
            MCB_DoPlcStackCheck = new ListViewItem()
            {
                Index = 11,
                Title = AppResources.do_plc_stack_check,
                DefaultCellType = CellTypes.LabelLabel,
                EditableCellType = CellTypes.LabelSwitch,
                IsVisible = true
            };
            MCB_Enable72V = new ListViewItem()
            {
                Index = 21,
                Title = AppResources.enable_72v_operation,
                DefaultCellType = CellTypes.LabelLabel,
                EditableCellType = CellTypes.LabelSwitch,
                IsVisible = true
            };
            MCB_ReconnectTimer = new ListViewItem()
            {
                Index = 22,
                Title = AppResources.reconnect_timer,
                DefaultCellType = CellTypes.LabelLabel,
                EditableCellType = CellTypes.LabelTextEdit,
                TextInputType = InputType.Number,
                IsVisible = true
            };
            MCB_DaughterCardEnabled = new ListViewItem()
            {
                Index = 23,
                Title = AppResources.bms_aughter_card,
                DefaultCellType = CellTypes.LabelLabel,
                EditableCellType = CellTypes.ListSelector,
                Items = BuildDaughterCardEnabledItems(),
                ListSelectionCommand = ListSelectorCommand
            };
            MCB_BMSBitRate = new ListViewItem
            {
                Index = 24,
                Title = AppResources.bms_bit_rate,
                DefaultCellType = CellTypes.LabelLabel,
                EditableCellType = CellTypes.ListSelector,
                Items = BuildBMSBitRateItems(),
                ListSelectionCommand = ListSelectorCommand
            };
            MCB_DefaultBrightness_TextBox = new ListViewItem()
            {
                Index = 25,
                Title = AppResources.default_brightness,
                DefaultCellType = CellTypes.LabelLabel,
                EditableCellType = CellTypes.LabelTextEdit,
                TextInputType = InputType.Decimal,
                TextMaxLength = 3
            };
            MCB_PLCgain = new ListViewItem()
            {
                Index = 26,
                Title = AppResources.plc_gain,
                DefaultCellType = CellTypes.LabelLabel,
                EditableCellType = CellTypes.ListSelector,
                Items = BuildPLCGainItems(),
                ListSelectionCommand = ListSelectorCommand
            };
            MCB_UpdateCableSettingsButton = new ListViewItem
            {
                Index = 27,
                Title = AppResources.update_cable_settings,
                DefaultCellType = CellTypes.Button,
                EditableCellType = CellTypes.Button,
                ListSelectionCommand = McbUpdateCableSettingsClickCommand
            };
            MCB_LoadPlcFirmwareAndRestartButton = new ListViewItem
            {
                Index = 28,
                Title = AppResources.load_plc_firmware_and_restart,
                DefaultCellType = CellTypes.Button,
                EditableCellType = CellTypes.Button,
                ListSelectionCommand = MCB_LoadPlcFirmwareAndRestartClickCommand
            };
        }

        List<object> BuildDaughterCardEnabledItems()
        {
            var items = new List<object>
            {
                AppResources.disabled,
                AppResources.bms_only,
                AppResources.bms_with_fail_over,
                AppResources.bms_with_plc_fail_over
            };

            return items;
        }

        List<object> BuildBMSBitRateItems()
        {
            var items = new List<object>
            {
                AppResources.can_baud_125,
                AppResources.can_baud_250,
                AppResources.can_baud_500
            };

            return items;
        }

        List<object> BuildPLCGainItems()
        {
            var items = new List<object>
            {
                AppResources.gain_value_1,
                AppResources.gain_value_2,
                AppResources.gain_value_3,
                AppResources.gain_value_4,
                AppResources.gain_value_5
            };

            return items;
        }

        #endregion

        #region Cable Settings

        public IMvxCommand McbUpdateCableSettingsClickCommand
        {
            get
            {
                return new MvxCommand(McbUpdateCableSettings);
            }
        }

        void McbUpdateCableSettings()
        {
            ListenToCableSettings();

            FireOnNavigatingToCableSettings();
        }

        void ListenToCableSettings()
        {
            cableSettingsMessage =
                Mvx.Resolve<IMvxMessenger>()
                   .Subscribe<CableSettingsMessage>(OnCableSettingsMessage);
        }

        void OnCableSettingsMessage(CableSettingsMessage obj)
        {
            Task.Run(() => OnCableSettingsMessageTask(obj));
        }

        async Task OnCableSettingsMessageTask(CableSettingsMessage obj)
        {
            await UpdateIr(obj.Ir);

            ResetMesage();
        }

        async Task UpdateIr(float ir)
        {
            await GoToEditModeIfNot();

            MCB_IR_val_TextBox.SubTitle = ir.ToString();
            MCB_IR_val_TextBox.Text = ir.ToString();
        }

        async Task GoToEditModeIfNot()
        {
            if (!EditingMode)
                await ChangeEditMode(true);
        }

        void ResetMesage()
        {
            Mvx.Resolve<IMvxMessenger>().Unsubscribe<CableSettingsMessage>(cableSettingsMessage);
        }

        #endregion

        public IMvxCommand HallEffectReverseCommand
        {
            get
            {
                return new MvxAsyncCommand(HallEffectReverseClick);
            }
        }

        async Task HallEffectReverseClick()
        {
            var config = BattViewQuantum.Instance.GetBATTView().Config;

            config.currentClampCalA *= -1;
            config.currentClampCalB *= -1;
            config.currentClamp2CalA *= -1;
            config.currentClamp2CalB *= -1;

            ChooseCorrectHallEffectReverseTitle();

            await ChangeEditMode(true);

            ACUserDialogs.ShowAlert(AppResources.done_save);
        }

        public IMvxCommand MCB_LoadPlcFirmwareAndRestartClickCommand
        {
            get
            {
                return new MvxAsyncCommand(MCB_LoadPlcFirmwareAndRestart);
            }
        }

        #region Plc Firmware And Restart

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
                FireOnDisconnectingDevice();

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

        #endregion

        #region Load BattView

        internal override void LoadBattViewValues()
        {
            Task.Run((Action)LoadBattViewValuesTask);
        }

        void LoadBattViewValuesTask()
        {
            BattViewObject currentBattView = BattViewQuantum.Instance.GetBATTView();
            var config = currentBattView.Config;

            Batt_autoLogTime.SubTitle = Batt_autoLogTime.Text = config.autoLogTime.ToString();
            bool flag = config.isPA != 0x00;
            Batt_isPA.IsSwitchEnabled = flag;

            flag = config.enableExtTempSensing != 0x00;
            Batt_enableExtTempSensingEnable.IsSwitchEnabled = flag;

            Batt_disableIntercell.IsSwitchEnabled = !config.disableIntercell;

            if (currentBattView.FirmwareRevision >= 2.09f)
            {
                try
                {
                    int tempcontrolIndex = Convert.ToInt32(config.temperatureControl);
                    BATT_temperatureControl.SelectedIndex = tempcontrolIndex;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    BATT_temperatureControl.SelectedIndex = 9;
                }

                BATT_temperatureControl.SelectedItem = Instance.TemperatureFallbackControllValues[BATT_temperatureControl.SelectedIndex];
            }

            flag = config.enableElectrolyeSensing != 0x00;
            Batt_enableElectrolyeSensingEnable.IsSwitchEnabled = flag;

            flag = config.enableHallEffectSensing != 0x00;
            Batt_enableHallEffectSensingYes.IsSwitchEnabled = flag;

            HallEffectReverse.Enable = flag;

            flag = config.enablePLC != 0x00;
            Batt_enablePLCEnable.IsSwitchEnabled = flag;

            flag = config.enableDayLightSaving != 0x00;
            Batt_DayLightSaving_Enable.IsSwitchEnabled = flag;

            flag = config.temperatureFormat != 0x00;
            Batt_temperatureFormat.SelectedIndex = flag ? 1 : 0;

            Batt_temperatureFormat.SelectedItem = Instance.TemperatureUnits[Batt_temperatureFormat.SelectedIndex];

            ChooseCorrectHallEffectReverseTitle();
        }

        void ChooseCorrectHallEffectReverseTitle()
        {
            HallEffectReverse
                .Title =
                    BattViewQuantum.Instance.GetBATTView().Config.currentClamp2CalA > 0.0f
                ? AppResources.reverse_hall_effect
                : AppResources.restore_hall_effect;
        }

        #endregion

        #region Load MCB

        internal override void LoadMcbValues()
        {
            try
            {
                Task.Run(async () => await TryLoadMcbValuesTask());
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        async Task TryLoadMcbValuesTask()
        {
            MCBobject currentMcb = MCBQuantum.Instance.GetMCB();
            var config = currentMcb.Config;

            MCB_IR_val_TextBox.SubTitle = MCB_IR_val_TextBox.Text = config.IR.ToString();
            MCB_AutoStartCustomRadioButtonTOSAVE.IsSwitchEnabled = config.autoStartEnable;

            MCB_autoStartTime_TextBoxTOSAVE.SubTitle = MCB_autoStartTime_TextBoxTOSAVE.Text = config.autoStartCountDownTimer ?? "";

            MCB_Refresh_AfterEQ_Radio.IsSwitchEnabled = config.enableRefreshCycleAfterEQ;

            MCB_Refresh_AfterFI_Radio.IsSwitchEnabled = config.enableRefreshCycleAfterFI;

            MCB_refreshComboBoxTOSAVE.SelectedItem = config.refreshTimer;
            MCB_refreshComboBoxTOSAVE.SelectedIndex = MCB_refreshComboBoxTOSAVE.Items.FindIndex(o => ((string)o).Equals(MCB_refreshComboBoxTOSAVE.SelectedItem));

            MCB_TempSensorEnable_Radio.IsSwitchEnabled = config.temperatureSensorInstalled;

            MCB_MultiVoltageEnableRadio.IsSwitchEnabled = config.enableAutoDetectMultiVoltage;

            MCB_TemperatureFormatcelsiusRadio.SelectedIndex = config.temperatureFormat ? 0 : 1;

            MCB_TemperatureFormatcelsiusRadio.SelectedItem = Instance.TemperatureUnits[MCB_TemperatureFormatcelsiusRadio.SelectedIndex];

            MCB_DayLightSavingEnableRadio.IsSwitchEnabled = config.daylightSaving;

            MCB_enablePLC_enableRadio.IsEditEnabled = config.enablePLC;
            MCB_enablePLC_enableRadio.IsSwitchEnabled = config.enablePLC;

            MCB_DoPlcStackCheck.IsSwitchEnabled = config.doPLCStackCheck;

            MCB_enableManualEQ_enableRadio.IsSwitchEnabled = config.enableManualEQ;
            MCB_enableManualDesulfate_enableRadio.IsSwitchEnabled = config.enableManualDesulfate;
            MCB_enablepushbutton_enableRadio.IsSwitchEnabled = !config.disablePushButton;

            int ledControl = config.ledcontrol;
            if (ledControl < MCB_LedOptions.Items.Count)
            {
                MCB_LedOptions.SelectedIndex = ledControl;
                MCB_LedOptions.SelectedItem = MCB_LedOptions.Items[ledControl].ToString();
            }

            MCB_OverrideFIEQSched_YesRadio.IsSwitchEnabled = config.chargerOverrideBattviewFIEQsched;

            MCB_ignoreBAttviewSOC_YesRadio.IsSwitchEnabled = config.ignoreBATTViewSOC;

            MCB_BattviewAutoCalibration_EnableRadio.IsSwitchEnabled = config.battviewAutoCalibrationEnable;

            if (currentMcb.FirmwareRevision >= 2.21f)
            {
                int indexMCB_RemoteStopButton = Convert.ToInt32(config.OCD_Remote);
                MCB_RemoteStopButton.SelectedIndex = indexMCB_RemoteStopButton;
                string selectedItem = MCB_RemoteStopButton.Items[indexMCB_RemoteStopButton].ToString();
                MCB_RemoteStopButton.SelectedItem = selectedItem;
                MCB_RemoteStopButton.SubTitle = MCB_RemoteStopButton.Items[indexMCB_RemoteStopButton].ToString();
            }

            if (config.cc_ramping_min_steps == 0)
            {
                MCB_minSteps_ComboBox.SelectedIndex = 0;
                MCB_minSteps_ComboBox.SelectedItem = MCB_minSteps_ComboBox.Items[MCB_minSteps_ComboBox.SelectedIndex].ToString();
            }
            else
            {
                MCB_minSteps_ComboBox.SelectedItem = config.cc_ramping_min_steps.ToString();
                MCB_minSteps_ComboBox.SelectedIndex = MCB_minSteps_ComboBox.Items.FindIndex(o => ((string)o).Equals(MCB_minSteps_ComboBox.SelectedItem));
            }

            MCB_minSteps_ComboBox.IsVisible = currentMcb.FirmwareRevision >= 2.14f && ControlObject.UserAccess.MCB_CC_CurrentRate != AccessLevelConsts.noAccess;

            MCB_OverrideFIEQSched_YesRadio.IsVisible = currentMcb.FirmwareRevision >= 2.12f && ControlObject.UserAccess.MCB_FI_EQ_sched_CustomSettings != AccessLevelConsts.noAccess;

            if (currentMcb.FirmwareRevision < 2.13f)
            {
                MCB_ignoreBAttviewSOC_YesRadio.IsVisible = false;
                MCB_BattviewAutoCalibration_EnableRadio.IsVisible = false;
            }
            else
            {
                MCB_ignoreBAttviewSOC_YesRadio.IsVisible = true;
                MCB_BattviewAutoCalibration_EnableRadio.IsVisible = true;
            }

            if (CanAddEnable72V())
                MCB_Enable72V.IsSwitchEnabled = Convert.ToBoolean(config.Enable_72V);

            MCB_ReconnectTimer.SubTitle = MCB_ReconnectTimer.Text = config.ReconnectTimer.ToString();

            if (currentMcb.FirmwareRevision >= 2.82f)
            {
                byte daughterCardEnabled = config.DaughterCardEnabled;

                if (daughterCardEnabled > MCB_DaughterCardEnabled.Items.Count)
                    MCB_DaughterCardEnabled.SelectedIndex = 0;
                else
                    MCB_DaughterCardEnabled.SelectedIndex = daughterCardEnabled;

                MCB_DaughterCardEnabled.SelectedItem = MCB_DaughterCardEnabled.Items[MCB_DaughterCardEnabled.SelectedIndex].ToString();
            }

            if (currentMcb.FirmwareRevision >= 2.93f)
            {
                MCB_BMSBitRate.SelectedItem = config.bmsBitRate.ToString();
                if (config.defaultBrightness / 2 > 100)
                    MCB_DefaultBrightness_TextBox.Text = "100";
                else
                    MCB_DefaultBrightness_TextBox.Text = (config.defaultBrightness / 2).ToString();
            }

            if (currentMcb.FirmwareRevision >= 2.96f)
            {
                MCB_PLCgain.SelectedItem = config.plc_gain.ToString();
            }

            await Task.Delay(500);
        }

        #endregion

        internal override void LoadExclusiveValues()
        {
        }

        #region Add BattView

        internal override int BattViewAccessApply()
        {
            accessControlUtility = new UIAccessControlUtility();

            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_setPA, Batt_isPA, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_RTSampleTime, Batt_autoLogTime, ItemSource);

            accessControlUtility
                .DoApplyAccessControl
                (ControlObject.UserAccess.Batt_enablePostSensor,
                 Batt_enableExtTempSensingEnable, ItemSource);
            accessControlUtility
                .DoApplyAccessControl
                (ControlObject.UserAccess.Batt_enablePostSensor,
                 Batt_disableIntercell, ItemSource);

            if (BATT_temperatureControl != null)
                accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_onlyForEnginneringTeam, BATT_temperatureControl, ItemSource);

            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_EnableEL, Batt_enableElectrolyeSensingEnable, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_setHallEffect, Batt_enableHallEffectSensingYes, ItemSource);

            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_enablePLC, Batt_enablePLCEnable, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_TemperatureFormat, Batt_temperatureFormat, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_dayLightSaving_Enable, Batt_DayLightSaving_Enable, ItemSource);

            accessControlUtility
                .DoApplyAccessControl
                (AccessLevelConsts.write,
                 HallEffectReverse, ItemSource);

            ShowEdit &= accessControlUtility.GetSavedCount() != 0;

            return accessControlUtility.GetVisibleCount();
        }

        bool CanShowNumberOfCables()
        {
            var config = BattViewQuantum.Instance.GetBATTView().Config;

            bool canShowNumberOfCables = Convert.ToBoolean(config.enableHallEffectSensing);

            return canShowNumberOfCables;
        }

        #endregion

        #region Add MCB

        internal override int McbAccessApply()
        {
            accessControlUtility = new UIAccessControlUtility();

            MCBobject currentMcb = MCBQuantum.Instance.GetMCB();

            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_IR, MCB_IR_val_TextBox, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_autoStart_Enable, MCB_AutoStartCustomRadioButtonTOSAVE, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_autoStart_count, MCB_autoStartTime_TextBoxTOSAVE, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_refreshEnable, MCB_Refresh_AfterEQ_Radio, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_refreshEnable, MCB_Refresh_AfterFI_Radio, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_refreshTimer, MCB_refreshComboBoxTOSAVE, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_TemperatureFormat, MCB_TemperatureFormatcelsiusRadio, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_TemperatureSensorEnable, MCB_TempSensorEnable_Radio, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_multiVoltage, MCB_MultiVoltageEnableRadio, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_dayLightSaving_Enable, MCB_DayLightSavingEnableRadio, ItemSource);

            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_enablePLC, MCB_enablePLC_enableRadio, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_enablePLC, MCB_DoPlcStackCheck, ItemSource);

            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_enableLauncher, MCB_enableManualEQ_enableRadio, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_enableLauncher, MCB_enableManualDesulfate_enableRadio, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_DisablePushButton, MCB_enablepushbutton_enableRadio, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_EnableLED, MCB_LedOptions, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_FI_EQ_sched_CustomSettings, MCB_OverrideFIEQSched_YesRadio, ItemSource);

            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_CC_CurrentRate, MCB_minSteps_ComboBox, ItemSource);

            ShowEdit &= accessControlUtility.GetSavedCount() != 0;

            DoApplyAccessControlForResetLCDCalibration();

            int returncount = accessControlUtility.GetVisibleCount();

            if (MCB_ignoreBAttviewSOC_YesRadio.IsVisible)
                accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_ignorebattviewsoc, MCB_ignoreBAttviewSOC_YesRadio, ItemSource);
            if (MCB_BattviewAutoCalibration_EnableRadio.IsVisible)
                accessControlUtility.DoApplyAccessControl(AccessLevelConsts.write, MCB_BattviewAutoCalibration_EnableRadio, ItemSource);

            if (CanAddEnable72V())
                accessControlUtility
                .DoApplyAccessControl
                    (ControlObject.UserAccess.MCB_PM_Voltage, MCB_Enable72V, ItemSource);

            accessControlUtility
            .DoApplyAccessControl
                (ControlObject.UserAccess.MCB_onlyForEnginneringTeam,
            MCB_ReconnectTimer, ItemSource);

            accessControlUtility
                .DoApplyAccessControl
                (AccessLevelConsts.write, MCB_RemoteStopButton, ItemSource);

            if (currentMcb.FirmwareRevision >= 2.82f)
            {
                accessControlUtility
                .DoApplyAccessControl
                (ControlObject.UserAccess.MCB_DaughterCardEnabled,
                 MCB_DaughterCardEnabled, ItemSource);
            }

            if (currentMcb.FirmwareRevision >= 2.93f)
            {
                accessControlUtility
                .DoApplyAccessControl
                (ControlObject.UserAccess.MCB_DaughterCardEnabled,
                 MCB_BMSBitRate, ItemSource);
                accessControlUtility
                .DoApplyAccessControl
                (ControlObject.UserAccess.MCB_defualtBrughtness,
                MCB_DefaultBrightness_TextBox, ItemSource);
            }

            if (currentMcb.FirmwareRevision >= 2.96f)
            {
                accessControlUtility
                .DoApplyAccessControl
                (ControlObject.UserAccess.MCB_onlyForEnginneringTeam,
                 MCB_PLCgain, ItemSource);
            }

                accessControlUtility
                .DoApplyAccessControl
                (AccessLevelConsts.write, MCB_UpdateCableSettingsButton, ItemSource);
            accessControlUtility
                .DoApplyAccessControl
                (AccessLevelConsts.write, MCB_LoadPlcFirmwareAndRestartButton, ItemSource);

            return returncount;
        }

        #endregion

        internal override void AddExclusiveItems()
        {
        }

        #region Save BattView

        internal override VerifyControl VerfiyBattViewSettings()
        {
            var verifyControl = new VerifyControl();

            verifyControl.VerifyInteger(Batt_autoLogTime, Batt_autoLogTime, 1, 65535);

            return verifyControl;
        }

        internal override void SaveBattViewToConfigObject(BattViewObject device)
        {
            var config = device.Config;

            config.autoLogTime = ushort.Parse(Batt_autoLogTime.Text);

            config.isPA = (byte)(Batt_isPA.IsSwitchEnabled ? 0x01 : 0x00);
            config.enableExtTempSensing = (byte)(Batt_enableExtTempSensingEnable.IsSwitchEnabled ? 0x01 : 0x00);
            config.disableIntercell = !Batt_disableIntercell.IsSwitchEnabled;
            config.enableElectrolyeSensing = (byte)(Batt_enableElectrolyeSensingEnable.IsSwitchEnabled ? 0x01 : 0x00);
            config.enableHallEffectSensing = (byte)(Batt_enableHallEffectSensingYes.IsSwitchEnabled ? 0x01 : 0x00);

            config.enablePLC = (byte)(Batt_enablePLCEnable.IsSwitchEnabled ? 0x01 : 0x00);
            config.enableDayLightSaving = (byte)(Batt_DayLightSaving_Enable.IsSwitchEnabled ? 0x01 : 0x00);
            config.temperatureFormat = Convert.ToByte(Batt_temperatureFormat.SelectedIndex == 0);

            if (device.FirmwareRevision >= 2.09f)
            {
                if (BATT_temperatureControl.SelectedIndex != -1)
                {
                    config.temperatureControl = Convert.ToByte(BATT_temperatureControl.SelectedIndex);
                }
            }
        }

        internal override async Task<bool> SaveBattViewAndReturnResult(BattViewObject device)
        {
            var result = await device.SaveConfigForPlc();

            if (result.Item2)
                ShowRebootConfirmation();

            return result.Item1;
        }

        void ShowRebootConfirmation()
        {
            FireOnDisconnectingDevice();

            ACUserDialogs.ShowAlertWithTitleAndOkButton(AppResources.batt_view_restarting);
        }

        #endregion

        #region Save MCB

        internal override VerifyControl VerfiyMcbSettings()
        {
            var verifyControl = new VerifyControl();

            MCBobject currentMcb = MCBQuantum.Instance.GetMCB();

            verifyControl.VerifyFloatNumber(MCB_IR_val_TextBox, MCB_IR_val_TextBox, 0.0f, 9999.99f);
            verifyControl.VerifyInteger(MCB_autoStartTime_TextBoxTOSAVE, MCB_autoStartTime_TextBoxTOSAVE, 9, 99);
            verifyControl.VerifyComboBox(MCB_refreshComboBoxTOSAVE);
            verifyControl.VerifyComboBox(MCB_minSteps_ComboBox);

            if (currentMcb.FirmwareRevision > 2.21f)
                verifyControl.VerifyComboBox(MCB_RemoteStopButton);

            verifyControl.VerifyUInteger(MCB_ReconnectTimer, MCB_ReconnectTimer, 0, 65534);

            if (currentMcb.FirmwareRevision >= 2.82f)
                verifyControl.VerifyComboBox(MCB_DaughterCardEnabled);

            if (currentMcb.FirmwareRevision >= 2.93f)
            {
                verifyControl.VerifyComboBox(MCB_BMSBitRate);
                verifyControl.VerifyInteger(MCB_DefaultBrightness_TextBox, MCB_DefaultBrightness_TextBox, 1, 100);
            }

            if (currentMcb.FirmwareRevision >= 2.96f)
            {
                verifyControl.VerifyComboBox(MCB_PLCgain);
            }

                return verifyControl;
        }

        internal override void SaveMcbToConfigObject(MCBobject device)
        {
            var config = device.Config;

            config.IR = float.Parse(MCB_IR_val_TextBox.Text);
            config.autoStartEnable = MCB_AutoStartCustomRadioButtonTOSAVE.IsSwitchEnabled;
            config.autoStartCountDownTimer = MCB_autoStartTime_TextBoxTOSAVE.Text;
            config.enableRefreshCycleAfterEQ = MCB_Refresh_AfterEQ_Radio.IsSwitchEnabled;
            config.enableRefreshCycleAfterFI = MCB_Refresh_AfterFI_Radio.IsSwitchEnabled;
            config.refreshTimer = MCB_refreshComboBoxTOSAVE.Text;
            config.temperatureSensorInstalled = MCB_TempSensorEnable_Radio.IsSwitchEnabled;
            config.enableAutoDetectMultiVoltage = MCB_MultiVoltageEnableRadio.IsSwitchEnabled;
            bool flag = MCB_TemperatureFormatcelsiusRadio.SelectedIndex == 0;
            config.temperatureFormat = flag;
            config.daylightSaving = MCB_DayLightSavingEnableRadio.IsSwitchEnabled;
            config.enablePLC = MCB_enablePLC_enableRadio.IsSwitchEnabled;
            config.doPLCStackCheck = MCB_DoPlcStackCheck.IsSwitchEnabled;
            config.enableManualDesulfate = MCB_enableManualDesulfate_enableRadio.IsSwitchEnabled;
            config.enableManualEQ = MCB_enableManualEQ_enableRadio.IsSwitchEnabled;
            config.disablePushButton = !MCB_enablepushbutton_enableRadio.IsSwitchEnabled;
            config.ledcontrol = (byte)MCB_LedOptions.SelectedIndex;

            if (device.FirmwareRevision > 2.11f)
                config.chargerOverrideBattviewFIEQsched = MCB_OverrideFIEQSched_YesRadio.IsSwitchEnabled;

            if (device.FirmwareRevision > 2.12f)
            {
                config.ignoreBATTViewSOC = MCB_ignoreBAttviewSOC_YesRadio.IsSwitchEnabled;
                config.battviewAutoCalibrationEnable = MCB_BattviewAutoCalibration_EnableRadio.IsSwitchEnabled;
            }

            if (device.FirmwareRevision > 2.13f)
                config.cc_ramping_min_steps = MCB_minSteps_ComboBox.SelectedIndex == 0 ? (byte)0 : byte.Parse(MCB_minSteps_ComboBox.Text);

            if (device.FirmwareRevision > 2.21f)
                config.OCD_Remote = Convert.ToByte(MCB_RemoteStopButton.SelectedIndex);

            if (CanAddEnable72V())
                config.Enable_72V = Convert.ToByte(MCB_Enable72V.IsSwitchEnabled);

            int reconnectTimerValue = int.Parse(MCB_ReconnectTimer.Text);
            reconnectTimerValue = reconnectTimerValue == 0 ? 60 : reconnectTimerValue;
            config.ReconnectTimer = (ushort)reconnectTimerValue;

            byte oldDaughterCardEnabled = config.DaughterCardEnabled;

            if (device.FirmwareRevision >= 2.82f)
            {
                byte daughterCardEnabled = (byte)MCB_DaughterCardEnabled.SelectedIndex;

                if (daughterCardEnabled > MCB_DaughterCardEnabled.Items.Count)
                    daughterCardEnabled = 0;

                config.DaughterCardEnabled = daughterCardEnabled;
            }

            if (device.FirmwareRevision >= 2.93f)
            {
                config.bmsBitRate = UInt16.Parse(MCB_BMSBitRate.Text);
                byte brightnessValue = byte.Parse(MCB_DefaultBrightness_TextBox.Text);
                brightnessValue *= 2;
                config.defaultBrightness = brightnessValue;
            }

            if (device.FirmwareRevision >= 2.96f)
            {
                config.plc_gain = byte.Parse(MCB_PLCgain.Text);
            }

            shouldRestart = oldDaughterCardEnabled != config.DaughterCardEnabled;
        }

        internal override async Task OnSaved(MCBobject device)
        {
            if (shouldRestart)
            {
                await device.ForceRecyclePower();

                FireOnDisconnectingDevice();

                ACUserDialogs.ShowAlertWithTitleAndOkButton(AppResources.charger_restarting);
            }
        }

        #endregion

        bool CanAddEnable72V()
        {
            var currentDevice = MCBQuantum.Instance.GetMCB();
            var config = currentDevice.Config;

            return currentDevice.FirmwareRevision > 2.42f && config.PMvoltage == "80";
        }

        #region MCB Reset LCD Calibration

        internal async override Task McbResetLcdCalibration()
        {
            if (!InternetConnectivityManager.NetworkCheck())
                return;

            if (!MCBQuantum.Instance.MCB_quickSerialNumberCheckStrict())
                return;

            ACUserDialogs.ShowProgress();

            try
            {
                await TryMcbResetLcdCalibration();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            ACUserDialogs.HideProgress();
        }

        async Task TryMcbResetLcdCalibration()
        {
            McbCommunicationTypes caller = McbCommunicationTypes.ResetLCDCalibration;
            object arg1 = null;

            List<object> arguments = new List<object> { caller, arg1, false };

            var results = await MCBQuantum.Instance.CommunicateMCB(arguments);

            if (results.Count > 0)
            {
                var status = (CommunicationResult)results[2];

                if (status == CommunicationResult.CHARGER_BUSY)
                {
                    ACUserDialogs.ShowAlertWithTitleAndOkButton(AppResources.charger_is_busy);
                }
                else if (status == CommunicationResult.OK)
                {
                    ACUserDialogs.HideProgress();

                    FireOnDisconnectingDevice();

                    ACUserDialogs.ShowAlertWithTitleAndOkButton(AppResources.charger_restarting);

                    return;
                }
                else
                {
                    ACUserDialogs.ShowAlertWithTitleAndOkButton(AppResources.reset_lcd_failed);
                }
            }
        }

        #endregion
    }
}
