using System;
using System.Threading.Tasks;

namespace actchargers
{
    public class SettingsSiteViewSubController : SettingsBaseSubController
    {
        public SettingsSiteViewSubController(bool isBattView) : base(isBattView, true)
        {
        }

        public async override Task Start()
        {
            await base.Start();

            IsResetLcdCalibrationVisible = false;
            IsResetLcdCalibrationEditEnabled = false;
        }

        internal override void InitExclusiveBattViewItems()
        {
        }

        internal override void InitExclusiveMcbItems()
        {
        }

        internal override void LoadBattViewValues()
        {
        }

        internal override void LoadMcbValues()
        {

        }

        internal override void LoadExclusiveValues()
        {
        }

        #region Add BattView

        internal override int BattViewAccessApply()
        {
            accessControlUtility = new UIAccessControlUtility();

            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_RTSampleTime, Batt_autoLogTime, ItemSource);

            accessControlUtility
                .DoApplyAccessControl
                (ControlObject.UserAccess.Batt_enablePostSensor,
                 Batt_enableExtTempSensingEnable, ItemSource);

            if (BATT_temperatureControl != null)
                accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_onlyForEnginneringTeam, BATT_temperatureControl, ItemSource);

            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_EnableEL, Batt_enableElectrolyeSensingEnable, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_setHallEffect, Batt_enableHallEffectSensingYes, ItemSource);

            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_enablePLC, Batt_enablePLCEnable, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_TemperatureFormat, Batt_temperatureFormat, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_dayLightSaving_Enable, Batt_DayLightSaving_Enable, ItemSource);

            ShowEdit &= accessControlUtility.GetSavedCount() != 0;

            return accessControlUtility.GetVisibleCount();
        }

        #endregion

        #region Add MCB

        internal override int McbAccessApply()
        {
            accessControlUtility = new UIAccessControlUtility();

            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_autoStart_Enable, MCB_AutoStartCustomRadioButtonTOSAVE, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_autoStart_count, MCB_autoStartTime_TextBoxTOSAVE, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_refreshEnable, MCB_Refresh_AfterEQ_Radio, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_refreshEnable, MCB_Refresh_AfterFI_Radio, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_refreshTimer, MCB_refreshComboBoxTOSAVE, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_TemperatureFormat, MCB_TemperatureFormatcelsiusRadio, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_multiVoltage, MCB_MultiVoltageEnableRadio, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_dayLightSaving_Enable, MCB_DayLightSavingEnableRadio, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_enablePLC, MCB_enablePLC_enableRadio, ItemSource);

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
            {
                accessControlUtility.DoApplyAccessControl(AccessLevelConsts.write, MCB_ignoreBAttviewSOC_YesRadio, ItemSource);
            }
            if (MCB_BattviewAutoCalibration_EnableRadio.IsVisible)
            {
                accessControlUtility.DoApplyAccessControl(AccessLevelConsts.write, MCB_BattviewAutoCalibration_EnableRadio, ItemSource);
            }

            accessControlUtility
                .DoApplyAccessControl
                (AccessLevelConsts.write, MCB_RemoteStopButton, ItemSource);

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

            config.enableExtTempSensing = (byte)(Batt_enableExtTempSensingEnable.IsSwitchEnabled ? 0x01 : 0x00);
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

        #endregion

        #region Save MCB

        internal override VerifyControl VerfiyMcbSettings()
        {
            var verifyControl = new VerifyControl();

            verifyControl.VerifyInteger(MCB_autoStartTime_TextBoxTOSAVE, MCB_autoStartTime_TextBoxTOSAVE, 9, 99);
            verifyControl.VerifyComboBox(MCB_refreshComboBoxTOSAVE);
            verifyControl.VerifyComboBox(MCB_minSteps_ComboBox);
            verifyControl.VerifyComboBox(MCB_RemoteStopButton);

            return verifyControl;
        }

        internal override void SaveMcbToConfigObject(MCBobject device)
        {
            var config = device.Config;

            config.autoStartEnable = MCB_AutoStartCustomRadioButtonTOSAVE.IsSwitchEnabled;
            config.autoStartCountDownTimer = MCB_autoStartTime_TextBoxTOSAVE.Text;
            config.enableRefreshCycleAfterEQ = MCB_Refresh_AfterEQ_Radio.IsSwitchEnabled;
            config.enableRefreshCycleAfterFI = MCB_Refresh_AfterFI_Radio.IsSwitchEnabled;
            config.refreshTimer = MCB_refreshComboBoxTOSAVE.Text;
            config.enableAutoDetectMultiVoltage = MCB_MultiVoltageEnableRadio.IsSwitchEnabled;
            bool flag = MCB_TemperatureFormatcelsiusRadio.SelectedIndex == 0;
            config.temperatureFormat = flag;
            config.daylightSaving = MCB_DayLightSavingEnableRadio.IsSwitchEnabled;
            config.enablePLC = MCB_enablePLC_enableRadio.IsSwitchEnabled;
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
        }

        #endregion

        internal override Task McbResetLcdCalibration()
        {
            return null;
        }
    }
}
