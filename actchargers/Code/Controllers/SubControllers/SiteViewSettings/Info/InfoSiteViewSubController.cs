using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace actchargers
{
    public class InfoSiteViewSubController : InfoBaseSubController
    {
        public InfoSiteViewSubController(bool isBattView) : base(isBattView, true)
        {
        }

        internal override void InitExclusiveBattViewItems()
        {
        }

        internal override void InitExclusiveMcbItems()
        {
        }

        internal override int BattViewAccessApply()
        {
            return 0;
        }

        internal override void AddExclusiveItems()
        {
        }

        #region Load BattView

        internal override void LoadBattViewValues()
        {
        }

        #endregion

        #region Load MCB

        internal override void LoadMcbValues()
        {
            try
            {
                TryLoadMcbValues();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Logger.AddLog(true, "X25" + ex);
            }
        }

        void TryLoadMcbValues()
        {
            Task.Run((Action)TryLoadMcbValuesTask);
        }

        void TryLoadMcbValuesTask()
        {
            MCB_chargerModelTextBoxTOSAVE.SubTitle = MCB_chargerModelTextBoxTOSAVE.Text = "";

            MCB_installationDatePickerTOSAVE.MinDate = minInstallationDate;
            MCB_installationDatePickerTOSAVE.MaxDate = maxInstallationDate;
            MCB_installationDatePickerTOSAVE.Date = DateTime.Now;

            MCB_chargerTypeTOSAVE.Items = new List<object>(chargerTypes);

            MCB_timeZoneComboBoxTOSAVE.Items = StaticDataAndHelperFunctions.GetZonesListAsObjects();

            MCB_lcdVersion_textBox.SubTitle = MCB_lcdVersion_textBox.Text = "";
            MCB_WIFIVersion_textBox.SubTitle = MCB_WIFIVersion_textBox.Text = "";
        }

        #endregion

        #region Add MCB

        internal override int McbAccessApply()
        {
            accessControlUtility = new UIAccessControlUtility();

            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_Model, MCB_chargerModelTextBoxTOSAVE, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_chargerType, MCB_chargerTypeTOSAVE, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_InstallationDate, MCB_installationDatePickerTOSAVE, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_TimeZone, MCB_timeZoneComboBoxTOSAVE, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_HWversionControl, MCB_lcdVersion_textBox, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_HWversionControl, MCB_WIFIVersion_textBox, ItemSource);

            ShowEdit &= accessControlUtility.GetSavedCount() != 0;

            return accessControlUtility.GetSavedCount();
        }

        #endregion

        #region Save BattView

        internal override VerifyControl VerfiyBattViewSettings()
        {
            return new VerifyControl();
        }

        internal override void SaveBattViewToConfigObject(BattViewObject device)
        {
        }

        #endregion

        #region Save MCB

        internal override VerifyControl VerfiyMcbSettings()
        {
            var verifyControl = new VerifyControl();

            if (ControlObject.isHWMnafacturer)
                return verifyControl;

            verifyControl.VerifyTextBox(MCB_chargerModelTextBoxTOSAVE, MCB_chargerModelTextBoxTOSAVE, 1, 15);

            verifyControl.VerifyComboBox(MCB_chargerTypeTOSAVE, MCB_chargerTypeTOSAVE);
            verifyControl.VerifyComboBox(MCB_timeZoneComboBoxTOSAVE, MCB_timeZoneComboBoxTOSAVE);
            verifyControl.VerifyInteger(MCB_lcdVersion_textBox, MCB_lcdVersion_textBox, 1, 255);
            verifyControl.VerifyInteger(MCB_WIFIVersion_textBox, MCB_WIFIVersion_textBox, 1, 255);

            return verifyControl;
        }

        internal override void SaveMcbToConfigObject(MCBobject device)
        {
            if (ControlObject.isHWMnafacturer)
                return;

            var config = device.Config;

            config.model = MCB_chargerModelTextBoxTOSAVE.Text;
            config.InstallationDate = MCB_installationDatePickerTOSAVE.Date;

            if (config.chargerType != (byte)MCB_chargerTypeTOSAVE.SelectedIndex)
            {
                config.chargerType = (byte)MCB_chargerTypeTOSAVE.SelectedIndex;

                if (!CompareMcbProfileWithDefault(config))
                    MCBQuantum.Instance.MCB_saveDefaultChargeProfile();
            }

            config.FIschedulingMode |= MCB_chargerTypeTOSAVE.Text != "Conventional";

            var zone = StaticDataAndHelperFunctions.getZoneByText(MCB_timeZoneComboBoxTOSAVE.Text);
            device.myZone = zone.id;

            config.lcdMemoryVersion = MCB_lcdVersion_textBox.Text;
            config.wifiFirmwareVersion = MCB_WIFIVersion_textBox.Text;
        }

        #endregion
    }
}
