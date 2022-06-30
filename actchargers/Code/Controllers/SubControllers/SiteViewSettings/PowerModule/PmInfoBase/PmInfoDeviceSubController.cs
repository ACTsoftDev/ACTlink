namespace actchargers
{
    public class PmInfoDeviceSubController : PmInfoBaseSubController
    {
        ListViewItem MCB_NumberOfInstalledPMsTextBox;
        ListViewItem MCB_PMMaxCurrent;

        public PmInfoDeviceSubController() : base(false)
        {
        }

        #region Init Items

        internal override void InitExclusiveMcbItems()
        {
            MCB_NumberOfInstalledPMsTextBox = new ListViewItem()
            {
                Index = 0,
                Title = AppResources.number_of_installed_power_modules,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.LabelTextEdit,
                TextInputType = ACUtility.InputType.Number
            };
            MCB_PMMaxCurrent = new ListViewItem()
            {
                Index = 4,
                Title = AppResources.max_pm_current,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.LabelTextEdit,
                TextInputType = ACUtility.InputType.Number
            };
        }

        #endregion

        #region Load MCB

        internal override void LoadMcbValues()
        {
            MCBobject currentMcb = MCBQuantum.Instance.GetMCB();
            var config = currentMcb.Config;

            MCB_NumberOfInstalledPMsTextBox.SubTitle = MCB_NumberOfInstalledPMsTextBox.Text = config.numberOfInstalledPMs;

            MCB_PMVoltageComboBox.SelectedItem = config.PMvoltage;
            MCB_PMVoltageComboBox.SelectedIndex = MCB_PMVoltageComboBox.Items.FindIndex(o => ((string)o).Equals(MCB_PMVoltageComboBox.SelectedItem));

            if (config.PMvoltageInputValue < MCB_PMInputVoltageComboBox.Items.Count)
            {
                MCB_PMInputVoltageComboBox.SelectedIndex = config.PMvoltageInputValue;
                MCB_PMInputVoltageComboBox.SelectedItem = MCB_PMInputVoltageComboBox.Items[MCB_PMInputVoltageComboBox.SelectedIndex].ToString();
            }
            else
            {
                MCB_PMInputVoltageComboBox.SelectedIndex = -1;
            }

            MCB_PMeffiencieyTextBox.SubTitle = MCB_PMeffiencieyTextBox.Text = config.PMefficiency;

            if (currentMcb.FirmwareRevision >= 2.82f)
            {
                if (config.PMMaxCurrent == 0)
                    config.PMMaxCurrent = 99;

                MCB_PMMaxCurrent.SubTitle = MCB_PMMaxCurrent.Text = config.PMMaxCurrent.ToString();
            }
        }

        #endregion

        #region Add MCB

        internal override int McbAccessApply()
        {
            MCBobject currentMcb = MCBQuantum.Instance.GetMCB();

            accessControlUtility = SharedMcbAccessApply();

            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_numberOfInstalledPMs, MCB_NumberOfInstalledPMsTextBox, ItemSource);

            if (currentMcb.FirmwareRevision >= 2.82f)
            {
                accessControlUtility
                 .DoApplyAccessControl
                 (ControlObject.UserAccess.PMMaxCurrent,
                  MCB_PMMaxCurrent, ItemSource);
            }

            ShowEdit &= accessControlUtility.GetSavedCount() != 0;

            return accessControlUtility.GetVisibleCount();
        }

        #endregion

        #region Save MCB

        internal override VerifyControl VerfiyMcbSettings()
        {
            VerifyControl verifyControl = VerfiySharedMcbSettings();

            verifyControl.VerifyInteger
            (MCB_NumberOfInstalledPMsTextBox, MCB_NumberOfInstalledPMsTextBox,
               ControlObject.FormLimits.numberOfInstalledPMs_Min,
            ControlObject.FormLimits.numberOfInstalledPMs_Max);

            verifyControl.VerifyInteger(MCB_PMMaxCurrent, MCB_PMMaxCurrent, 10, 99);

            return verifyControl;
        }

        internal override void SaveMcbToConfigObject(MCBobject device)
        {
            SaveSharedMcbToConfigObject(device);

            device.Config.numberOfInstalledPMs = MCB_NumberOfInstalledPMsTextBox.Text;
            device.Config.PMMaxCurrent = byte.Parse(MCB_PMMaxCurrent.Text);
        }

        #endregion
    }
}
