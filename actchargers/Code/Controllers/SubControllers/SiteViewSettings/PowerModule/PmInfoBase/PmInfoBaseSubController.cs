using System.Collections.Generic;
using System;

namespace actchargers
{
    public abstract class PmInfoBaseSubController : SiteViewSettingsBaseSubController
    {
        internal ListViewItem MCB_PMVoltageComboBox;
        internal ListViewItem MCB_PMeffiencieyTextBox;
        internal ListViewItem MCB_PMInputVoltageComboBox;

        protected PmInfoBaseSubController(bool isSiteView) : base(false, isSiteView)
        {
        }

        internal override void InitSharedBattViewItems()
        {
        }

        #region Init Items

        internal override void InitSharedMcbItems()
        {
            MCB_PMVoltageComboBox = new ListViewItem()
            {
                Index = 1,
                Title = AppResources.rated_voltage,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.ListSelector,
                Items = new List<object> { "36", "48", "80" },
                ListSelectionCommand = ListSelectorCommand,
            };

            MCB_PMeffiencieyTextBox = new ListViewItem()
            {
                Index = 2,
                Title = AppResources.efficiency,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.LabelTextEdit,
                TextInputType = ACUtility.InputType.Decimal
            };

            MCB_PMInputVoltageComboBox = new ListViewItem()
            {
                Index = 3,
                Title = AppResources.pm_input_voltage,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.ListSelector,
                Items = new List<object> { "480", "208", "600", "380" },
                ListSelectionCommand = ListSelectorCommand
            };
        }

        #endregion

        internal override void InitExclusiveBattViewItems()
        {
        }

        internal override void LoadBattViewValues()
        {
        }

        internal override void LoadExclusiveValues()
        {
        }

        public override void LoadDefaults()
        {
        }

        internal override int BattViewAccessApply()
        {
            return 0;
        }

        internal override void AddExclusiveItems()
        {
        }

        #region Add MCB

        internal UIAccessControlUtility SharedMcbAccessApply()
        {
            accessControlUtility = new UIAccessControlUtility();

            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_PM_effieciency, MCB_PMeffiencieyTextBox, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_PM_Voltage, MCB_PMVoltageComboBox, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_PM_Voltage, MCB_PMInputVoltageComboBox, ItemSource);

            return accessControlUtility;
        }

        #endregion

        internal override VerifyControl VerfiyBattViewSettings()
        {
            return new VerifyControl();
        }

        internal override void SaveBattViewToConfigObject(BattViewObject device)
        {
        }

        #region Save MCB

        internal VerifyControl VerfiySharedMcbSettings()
        {
            var verifyControl = new VerifyControl();

            verifyControl.VerifyComboBox(MCB_PMVoltageComboBox, MCB_PMVoltageComboBox);
            verifyControl.VerifyComboBox(MCB_PMInputVoltageComboBox, MCB_PMInputVoltageComboBox);
            verifyControl.VerifyFloatNumber(MCB_PMeffiencieyTextBox, MCB_PMeffiencieyTextBox, 85.0f, 99.999f);

            return verifyControl;
        }

        internal void SaveSharedMcbToConfigObject(MCBobject device)
        {
            var config = device.Config;

            bool reload_Batt_BatterySettings = false || config.PMvoltage != MCB_PMVoltageComboBox.Text;

            config.PMvoltage = MCB_PMVoltageComboBox.Text;
            config.PMvoltageInputValue = (byte)MCB_PMInputVoltageComboBox.SelectedIndex;

            config.PMefficiency = MCB_PMeffiencieyTextBox.Text;

            if (reload_Batt_BatterySettings && string.IsNullOrEmpty(config.batteryVoltage))
                config.batteryVoltage = "36";

            int maxNumberOfPMs = 12;

            string model = config.serialNumber.Substring(1, 2);

            if (model == "10")
                maxNumberOfPMs = 4;
            else if (model == "20")
                maxNumberOfPMs = 6;

            config.model = "Q" + maxNumberOfPMs.ToString() + "-";
            config.model += config.PMvoltage + "-";

            int currentRating = int.Parse(config.numberOfInstalledPMs) * (config.PMvoltage == "36" ? 50 : 40);

            config.model += currentRating.ToString() + "-";
            config.model += (config.PMvoltageInputValue != 0 ? "208" : "480");

            if (!config.enablePLC && Convert.ToBoolean(config.ledcontrol))
                config.model += "-B";
        }

        #endregion
    }
}
