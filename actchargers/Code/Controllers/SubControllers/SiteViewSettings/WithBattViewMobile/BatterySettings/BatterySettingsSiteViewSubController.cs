using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace actchargers
{
    public class BatterySettingsSiteViewSubController : BatterySettingsBaseSubController
    {
        public BatterySettingsSiteViewSubController
        (bool isBattView, bool isBattViewMobile) : base(isBattView, isBattViewMobile, true)
        {
        }

        internal override void InitExclusiveBattViewMobileItems()
        {
        }

        internal override void InitExclusiveRegularBattViewItems()
        {
        }

        internal override void InitExclusiveMcbItems()
        {
        }

        internal override void LoadBattViewMobileValues()
        {
            LoadExclusiveCrossBattViewValues();
        }

        internal override void LoadRegularBattViewValues()
        {
            LoadExclusiveCrossBattViewValues();
        }

        #region Load BattView Values

        void LoadExclusiveCrossBattViewValues()
        {
            try
            {
                TryLoadExclusiveCrossBattViewValues();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Logger.AddLog(true, "X25" + ex);
            }
        }

        void TryLoadExclusiveCrossBattViewValues()
        {
            Task.Run((Action)TryLoadExclusiveCrossBattViewValuesTask);
        }

        void TryLoadExclusiveCrossBattViewValuesTask()
        {
            Batt_installationDatePickerTOSAVE.MinDate = MIN_INSTALLATION_DATE;
            Batt_installationDatePickerTOSAVE.MaxDate = MAX_INSTALLATION_DATE;

            DateTime toSelect = DateTime.Now;

            Batt_timeZoneComboBox.Items = StaticDataAndHelperFunctions.GetZonesListAsObjects();

            Batt_defaultBattreyVoltageComboBox.Items = GetBattreyVoltageList();

            Batt_batteryTypeComboBox.Items = GetBattreyTypesList();
        }

        #endregion

        #region Load MCB Values

        internal override void LoadMcbValues()
        {
            MCB_defaultBattreyVoltageComboBox.Items = GetBattreyVoltageList();
            if (IsRegularUser())
            {
                MCB_defaultBattreyVoltageComboBox.Items.Add("24");
                MCB_defaultBattreyVoltageComboBox.Items.Add("36");
            }

            MCB_nominalTemperatureRangeComboBox.Items = GetTemperatureRangeList();

            MCB_batteryTypeComboBox.Items = GetBattreyTypesList();
        }

        #endregion

        #region Add BattView

        internal override int BattViewMobileAccessApply()
        {
            return ExclusiveCrossBattViewAccessApply();
        }

        internal override int RegularBattViewAccessApply()
        {
            return ExclusiveCrossBattViewAccessApply();
        }

        int ExclusiveCrossBattViewAccessApply()
        {
            accessControlUtility = new UIAccessControlUtility();

            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_InstallationDate, Batt_installationDatePickerTOSAVE, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_BatteryCapacity, Batt_defaultBatteryCapacityTextBox, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_BatteryVoltage, Batt_defaultBattreyVoltageComboBox, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_temperatureCompensation, Batt_temperatureCompensationTextBox, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_TempertureHigh, Batt_HighTemperatureThresholdTextBox, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_BatteryType, Batt_batteryTypeComboBox, ItemSource);

            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_TempertureHigh, Batt_trickleTemperatureTextBox, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_TempertureHigh, Batt_foldTemperatureTextBox, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_TempertureHigh, Batt_coolDownTemperatureTextLabel, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_chargerType, Batt_chargerTypeTOSAVE, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_TimeZone, Batt_timeZoneComboBox, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_OverrideWarrantedAHR, Batt_WarrantedAHRTextBoxTOSAVE, ItemSource);

            ShowEdit &= accessControlUtility.GetSavedCount() != 0;

            return accessControlUtility.GetVisibleCount();
        }

        #endregion

        #region Add MCB

        internal override int McbAccessApply()
        {
            accessControlUtility = new UIAccessControlUtility();

            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_BatteryCapacity, MCB_defaultBatteryCapacityTextBox, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_BatteryCapacity, MCB_defaultBatteryCapacity24TextBox, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_BatteryCapacity, MCB_defaultBatteryCapacity36TextBox, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_BatteryCapacity, MCB_defaultBatteryCapacity48TextBox, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_BatteryCapacity, MCB_defaultBatteryCapacity80TextBox, ItemSource);

            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_BatteryVoltage, MCB_defaultBattreyVoltageComboBox, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_temperatureCompensation, MCB_temperatureCompensationTextBox, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_TempertureHigh, MCB_HighTemperatureThresholdTextBox, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_BatteryType, MCB_batteryTypeComboBox, ItemSource);

            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_TempertureHigh, MCB_trickleTemperatureTextBox, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_TempertureHigh, MCB_foldTemperatureTextBox, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_TempertureHigh, MCB_coolDownTemperatureTextLabel, ItemSource);

            ShowEdit &= accessControlUtility.GetSavedCount() != 0;

            int visibleCount = accessControlUtility.GetVisibleCount();

            if (MCB_nominalTemperatureRangeComboBox.IsVisible)
                accessControlUtility.DoApplyAccessControl(AccessLevelConsts.write, MCB_nominalTemperatureRangeComboBox, ItemSource);

            return visibleCount;
        }

        #endregion

        #region Verfiy BattView

        internal override VerifyControl VerfiyBattViewMobileSettings()
        {
            return VerfiyExclusiveCrossBattViewSettings();
        }

        internal override VerifyControl VerfiyRegularBattViewSettings()
        {
            return VerfiyExclusiveCrossBattViewSettings();
        }

        VerifyControl VerfiyExclusiveCrossBattViewSettings()
        {
            var verifyControl = new VerifyControl();

            verifyControl.VerifyComboBox(Batt_timeZoneComboBox);
            verifyControl.VerifyInteger(Batt_defaultBatteryCapacityTextBox, Batt_defaultBatteryCapacityTextBox, 100, 2000);
            verifyControl.VerifyComboBox(Batt_defaultBattreyVoltageComboBox);
            verifyControl.VerifyFloatNumber(Batt_temperatureCompensationTextBox, Batt_temperatureCompensationTextBox, 3.0f, 8.0f);

            verifyControl.VerifyFloatNumber(Batt_HighTemperatureThresholdTextBox, Batt_HighTemperatureThresholdTextBox, 77, 255);

            verifyControl.VerifyFloatNumber(Batt_foldTemperatureTextBox, Batt_foldTemperatureTextBox, 77, 255);
            verifyControl.VerifyFloatNumber(Batt_coolDownTemperatureTextLabel, Batt_coolDownTemperatureTextLabel, 77, 255);
            verifyControl.VerifyFloatNumber(Batt_trickleTemperatureTextBox, Batt_trickleTemperatureTextBox, 23, 77);

            verifyControl.VerifyComboBox(Batt_batteryTypeComboBox);
            verifyControl.VerifyComboBox(Batt_chargerTypeTOSAVE);

            verifyControl.VerifyUInteger(Batt_WarrantedAHRTextBoxTOSAVE, Batt_WarrantedAHRTextBoxTOSAVE, 125000, 6250000);

            if (!verifyControl.HasErrors())
            {
                if (float.Parse(Batt_HighTemperatureThresholdTextBox.Text) <= float.Parse(Batt_foldTemperatureTextBox.Text))
                    verifyControl.InsertRemoveFault(true, Batt_HighTemperatureThresholdTextBox);

                if (float.Parse(Batt_foldTemperatureTextBox.Text) <= float.Parse(Batt_coolDownTemperatureTextLabel.Text))
                    verifyControl.InsertRemoveFault(true, Batt_foldTemperatureTextBox);
            }

            return verifyControl;
        }

        #endregion

        #region Verfiy MCB

        internal override VerifyControl VerfiyMcbSettings()
        {
            var verifyControl = new VerifyControl();

            var config = MCBQuantum.Instance.GetMCB().Config;

            verifyControl.VerifyInteger(MCB_defaultBatteryCapacityTextBox, MCB_defaultBatteryCapacityTextBox, 40, 5000);
            verifyControl.VerifyInteger(MCB_defaultBatteryCapacity24TextBox, MCB_defaultBatteryCapacity24TextBox, 40, 5000);
            verifyControl.VerifyInteger(MCB_defaultBatteryCapacity36TextBox, MCB_defaultBatteryCapacity36TextBox, 40, 5000);
            verifyControl.VerifyInteger(MCB_defaultBatteryCapacity48TextBox, MCB_defaultBatteryCapacity48TextBox, 40, 5000);
            verifyControl.VerifyInteger(MCB_defaultBatteryCapacity80TextBox, MCB_defaultBatteryCapacity80TextBox, 40, 5000);

            verifyControl.VerifyComboBox(MCB_defaultBattreyVoltageComboBox);
            verifyControl.VerifyFloatNumber(MCB_temperatureCompensationTextBox, MCB_temperatureCompensationTextBox, 3.0f, 8.0f);

            if (config.temperatureFormat)
                verifyControl.VerifyFloatNumber(MCB_HighTemperatureThresholdTextBox, MCB_HighTemperatureThresholdTextBox, 25, 125);
            else
                verifyControl.VerifyFloatNumber(MCB_HighTemperatureThresholdTextBox, MCB_HighTemperatureThresholdTextBox, 77, 255);

            if (config.temperatureFormat)
                verifyControl.VerifyFloatNumber(MCB_trickleTemperatureTextBox, MCB_trickleTemperatureTextBox, -5, 25);
            else
                verifyControl.VerifyFloatNumber(MCB_trickleTemperatureTextBox, MCB_trickleTemperatureTextBox, 23, 77);

            if (config.temperatureFormat)
                verifyControl.VerifyFloatNumber(MCB_foldTemperatureTextBox, MCB_foldTemperatureTextBox, 25, 125);
            else
                verifyControl.VerifyFloatNumber(MCB_foldTemperatureTextBox, MCB_foldTemperatureTextBox, 77, 255);

            if (config.temperatureFormat)
                verifyControl.VerifyFloatNumber(MCB_coolDownTemperatureTextLabel, MCB_coolDownTemperatureTextLabel, 25, 125);
            else
                verifyControl.VerifyFloatNumber(MCB_coolDownTemperatureTextLabel, MCB_coolDownTemperatureTextLabel, 77, 255);

            verifyControl.VerifyComboBox(MCB_batteryTypeComboBox);

            if (!verifyControl.HasErrors())
            {
                if (float.Parse(MCB_HighTemperatureThresholdTextBox.Text) <= float.Parse(MCB_foldTemperatureTextBox.Text))
                    verifyControl.InsertRemoveFault(true, MCB_foldTemperatureTextBox);

                if (float.Parse(MCB_foldTemperatureTextBox.Text) <= float.Parse(MCB_coolDownTemperatureTextLabel.Text))
                    verifyControl.InsertRemoveFault(true, MCB_foldTemperatureTextBox);
            }

            return verifyControl;
        }

        #endregion

        #region Save BattView

        internal override void SaveBattViewMobileToConfigObject(BattViewObject device)
        {
            SaveExclusiveCrossBattViewToConfigObject(device);
        }

        internal override void SaveBattViewRegularToConfigObject(BattViewObject device)
        {
            SaveExclusiveCrossBattViewToConfigObject(device);
        }

        void SaveExclusiveCrossBattViewToConfigObject(BattViewObject device)
        {
            var config = device.Config;

            if (config.installationDate.Date != Batt_installationDatePickerTOSAVE.Date.Date)
            {
                config.installationDate = Batt_installationDatePickerTOSAVE.Date.Date;
            }

            JsonZone info = StaticDataAndHelperFunctions.getZoneByText(Batt_timeZoneComboBox.Text);

            device.myZone = info.id;

            config.warrantedAHR = UInt32.Parse(Batt_WarrantedAHRTextBoxTOSAVE.Text);

            config.ahrcapacity = ushort.Parse(Batt_defaultBatteryCapacityTextBox.Text);

            config.nominalvoltage = byte.Parse(Batt_defaultBattreyVoltageComboBox.Text);

            if (config.batteryTemperatureCompesnation != (byte)Math.Round(float.Parse(Batt_temperatureCompensationTextBox.Text) * 10.0f))
                config.batteryTemperatureCompesnation = (byte)Math.Round(float.Parse(Batt_temperatureCompensationTextBox.Text) * 10.0f);

            if (config.batteryHighTemperature != (ushort)Math.Round(((float.Parse(Batt_HighTemperatureThresholdTextBox.Text) - 32) / 1.8) * 10.0f))
                config.batteryHighTemperature = (ushort)Math.Round(((float.Parse(Batt_HighTemperatureThresholdTextBox.Text) - 32) / 1.8) * 10.0f);

            if (config.foldTemperature != (short)Math.Round(((float.Parse(Batt_foldTemperatureTextBox.Text) - 32) / 1.8) * 10.0f))
                config.foldTemperature = (short)Math.Round(((float.Parse(Batt_foldTemperatureTextBox.Text) - 32) / 1.8) * 10.0f);

            if (config.coolDownTemperature != (short)Math.Round(((float.Parse(Batt_coolDownTemperatureTextLabel.Text) - 32) / 1.8) * 10.0f))
                config.coolDownTemperature = (short)Math.Round(((float.Parse(Batt_coolDownTemperatureTextLabel.Text) - 32) / 1.8) * 10.0f);

            if (config.TRTemperature != (short)Math.Round(((float.Parse(Batt_trickleTemperatureTextBox.Text) - 32) / 1.8) * 10.0f))
                config.TRTemperature = (short)Math.Round(((float.Parse(Batt_trickleTemperatureTextBox.Text) - 32) / 1.8) * 10.0f);

            if (config.batteryType != (byte)Batt_batteryTypeComboBox.SelectedIndex)
                config.batteryType = (byte)Batt_batteryTypeComboBox.SelectedIndex;

            if (config.chargerType != (byte)Batt_chargerTypeTOSAVE.SelectedIndex)
                config.chargerType = (byte)Batt_chargerTypeTOSAVE.SelectedIndex;
        }

        #endregion

        #region Save MCB

        internal override void SaveMcbToConfigObject(MCBobject device)
        {
            var config = device.Config;

            if (config.enableAutoDetectMultiVoltage && device.FirmwareRevision > 2.03f)
            {
                config.batteryCapacity24 = UInt16.Parse(MCB_defaultBatteryCapacity24TextBox.Text);
                config.batteryCapacity36 = UInt16.Parse(MCB_defaultBatteryCapacity36TextBox.Text);
                config.batteryCapacity48 = UInt16.Parse(MCB_defaultBatteryCapacity48TextBox.Text);
                config.batteryCapacity80 = UInt16.Parse(MCB_defaultBatteryCapacity80TextBox.Text);
            }
            else
            {
                config.batteryCapacity = MCB_defaultBatteryCapacityTextBox.Text;
            }

            config.batteryVoltage = MCB_defaultBattreyVoltageComboBox.Text;
            config.temperatureVoltageCompensation = MCB_temperatureCompensationTextBox.Text;

            config.maxTemperatureFault = config.temperatureFormat ? MCB_HighTemperatureThresholdTextBox.Text : ((float.Parse(MCB_HighTemperatureThresholdTextBox.Text) - 32) / 1.8f).ToString("N1");

            config.batteryType = MCB_batteryTypeComboBox.Text;

            config.TRtemperature = config.temperatureFormat ? MCB_trickleTemperatureTextBox.Text : ((float.Parse(MCB_trickleTemperatureTextBox.Text) - 32) / 1.8f).ToString("N1");

            config.foldTemperature = config.temperatureFormat ? MCB_foldTemperatureTextBox.Text : ((float.Parse(MCB_foldTemperatureTextBox.Text) - 32) / 1.8f).ToString("N1");

            config.coolDownTemperature = config.temperatureFormat ? MCB_coolDownTemperatureTextLabel.Text : ((float.Parse(MCB_coolDownTemperatureTextLabel.Text) - 32) / 1.8f).ToString("N1");
        }

        #endregion
    }
}
