using System;

namespace actchargers
{
    public abstract class InfoBaseSubController : SiteViewSettingsBaseSubController
    {
        internal ListViewItem MCB_chargerModelTextBoxTOSAVE;
        internal ListViewItem MCB_chargerTypeTOSAVE;
        internal ListViewItem MCB_installationDatePickerTOSAVE;
        internal ListViewItem MCB_timeZoneComboBoxTOSAVE;
        internal ListViewItem MCB_lcdVersion_textBox;
        internal ListViewItem MCB_WIFIVersion_textBox;

        internal DateTime minInstallationDate = new DateTime(2015, 9, 1);
        internal DateTime maxInstallationDate = DateTime.Now.AddDays(180);
        internal object[] chargerTypes =
        {
            AppResources.charger_type_fast,
            AppResources.charger_type_conventional,
            AppResources.charger_type_opportunity
        };

        protected InfoBaseSubController(bool isBattView, bool isSiteView) : base(isBattView, isSiteView)
        {
        }

        #region Shared Items

        internal override void InitSharedBattViewItems()
        {
        }

        internal override void InitSharedMcbItems()
        {
            MCB_chargerModelTextBoxTOSAVE = new ListViewItem()
            {
                Index = 3,
                Title = AppResources.charger_model,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.LabelTextEdit,
                TextMaxLength = 15
            };
            MCB_chargerTypeTOSAVE = new ListViewItem()
            {
                Index = 7,
                Title = AppResources.charger_type,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.ListSelector,
                ListSelectionCommand = ListSelectorCommand
            };
            MCB_installationDatePickerTOSAVE = new ListViewItem()
            {
                Index = 6,
                Title = AppResources.installation_date,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.DatePicker
            };
            MCB_timeZoneComboBoxTOSAVE = new ListViewItem()
            {
                Index = 8,
                Title = AppResources.time_zone,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.ListSelector,
                ListSelectorType = ACUtility.ListSelectorType.Timezone,
                ListSelectionCommand = ListSelectorCommand
            };
            MCB_lcdVersion_textBox = new ListViewItem()
            {
                Index = 9,
                Title = AppResources.lcd_images_version,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.LabelTextEdit,
                TextMaxLength = 3
            };
            MCB_WIFIVersion_textBox = new ListViewItem()
            {
                Index = 10,
                Title = AppResources.wifi_firmware_version,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.LabelTextEdit,
                TextMaxLength = 3
            };
        }

        #endregion

        internal override void LoadExclusiveValues()
        {
        }

        internal bool CompareMcbProfileWithDefault(MCBConfig config)
        {
            MCBobject currentMcb = MCBQuantum.Instance.GetMCB();
            if (!MCBQuantum.Instance.MCB_quickSerialNumberCheckStrict())
                return true;

            if (config.TRrate != 500 ||
            config.FIrate != 500 ||
            config.EQrate != 400 ||
            config.trickleVoltage != "2.0" ||
            config.FIvoltage != "2.6" ||
            config.EQvoltage != "2.65" ||
            config.CVfinishCurrent != 24 ||
            config.CVtimer != "04:00" ||
            config.finishTimer != "03:00" ||
            config.EQtimer != "04:00" ||
            config.desulfationTimer != "12:00" ||
            config.finishDV != 5 ||
            config.finishDT != "59" ||
            config.CVcurrentStep != 0 ||
                config.FIstartWindow != "00:00" ||
                config.EQstartWindow != "00:00" ||
                config.EQwindow != "24:00"
                )
                return false;

            switch (config.chargerType)
            {
                case 0:
                    //FAST
                    if (config.CVvoltage != "2.42" ||
                    config.CCrate != 4000 ||
                        config.finishWindow != "24:00")
                        return false;
                    break;
                case 1:
                    //Conventional
                    if (config.CVvoltage != "2.37" ||
                    config.CCrate != 1700 ||
                        config.finishWindow != "24:00")
                        return false;
                    break;
                case 2:
                    //Opp
                    if (config.CVvoltage != "2.4" ||
                    config.CCrate != 2500 ||
                        config.finishWindow != "08:00")
                        return false;
                    break;
            }

            return true;
        }

        public override void LoadDefaults()
        {
        }
    }
}
