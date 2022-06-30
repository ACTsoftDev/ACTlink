using MvvmCross.Core.ViewModels;
using static actchargers.ACUtility;
using System.Collections.Generic;
using System;

namespace actchargers
{
    public abstract class BatterySettingsBaseSubController : WithBattViewMobileBaseSubController
    {
        #region BattView Items

        internal ListViewItem Batt_installationDatePickerTOSAVE;
        internal ListViewItem Batt_timeZoneComboBox;
        internal ListViewItem Batt_defaultBatteryCapacityTextBox;
        internal ListViewItem Batt_defaultBattreyVoltageComboBox;
        internal ListViewItem Batt_temperatureCompensationTextBox;
        internal ListViewItem Batt_HighTemperatureThresholdTextBox;
        internal ListViewItem Batt_foldTemperatureTextBox;
        internal ListViewItem Batt_coolDownTemperatureTextLabel;
        internal ListViewItem Batt_trickleTemperatureTextBox;
        internal ListViewItem Batt_batteryTypeComboBox;
        internal ListViewItem Batt_chargerTypeTOSAVE;
        internal ListViewItem Batt_WarrantedAHRTextBoxTOSAVE;

        #endregion

        #region MCB Items

        internal ListViewItem MCB_defaultBatteryCapacity24TextBox;
        internal ListViewItem MCB_defaultBatteryCapacity36TextBox;
        internal ListViewItem MCB_defaultBatteryCapacity48TextBox;
        internal ListViewItem MCB_defaultBatteryCapacity80TextBox;
        internal ListViewItem MCB_defaultBatteryCapacityTextBox;
        internal ListViewItem MCB_defaultBattreyVoltageComboBox;
        internal ListViewItem MCB_nominalTemperatureRangeComboBox;
        internal ListViewItem MCB_temperatureCompensationTextBox;
        internal ListViewItem MCB_HighTemperatureThresholdTextBox;
        internal ListViewItem MCB_trickleTemperatureTextBox;
        internal ListViewItem MCB_foldTemperatureTextBox;
        internal ListViewItem MCB_coolDownTemperatureTextLabel;
        internal ListViewItem MCB_batteryTypeComboBox;

        #endregion

        internal static DateTime MIN_INSTALLATION_DATE = new DateTime(2015, 9, 1, 0, 0, 0, DateTimeKind.Utc);
        internal static DateTime MAX_INSTALLATION_DATE = DateTime.UtcNow.AddDays(180);

        internal static DateTime MIN_MANUFACTRING_DATE = DateTime.Now.AddYears(-10);
        internal static DateTime MAX_MANUFACTRING_DATE = DateTime.UtcNow.AddDays(180);

        protected BatterySettingsBaseSubController
        (bool isBattView, bool isBattViewMobile, bool isSiteView)
            : base(isBattView, isBattViewMobile, isSiteView)
        {
        }

        internal override void InitSharedBattViewMobileItems()
        {
            InitSharedCrossBattViewItems();
        }

        internal override void InitSharedRegularBattViewItems()
        {
            InitSharedCrossBattViewItems();
        }

        #region Init BattView Items

        void InitSharedCrossBattViewItems()
        {
            Batt_installationDatePickerTOSAVE = new ListViewItem()
            {
                Index = 3,
                Title = AppResources.installation_date,
                DefaultCellType = CellTypes.LabelLabel,
                EditableCellType = CellTypes.DatePicker,
                IsVisible = true
            };
            Batt_timeZoneComboBox = new ListViewItem()
            {
                Index = 8,
                Title = AppResources.time_zone,
                DefaultCellType = CellTypes.LabelLabel,
                EditableCellType = CellTypes.ListSelector,
                ListSelectionCommand = ListSelectorCommand,
                ListSelectorType = ListSelectorType.Timezone
            };
            Batt_defaultBatteryCapacityTextBox = new ListViewItem()
            {
                Index = 9,
                Title = AppResources.battery_capacity,
                DefaultCellType = CellTypes.LabelLabel,
                EditableCellType = CellTypes.LabelTextEdit,
                TextInputType = InputType.Number,
                TextValueChanged = CapacityTextChanged,
                TextMaxLength = 4
            };
            Batt_defaultBattreyVoltageComboBox = new ListViewItem()
            {
                Index = 10,
                Title = AppResources.battery_voltage,
                DefaultCellType = CellTypes.LabelLabel,
                EditableCellType = CellTypes.ListSelector,
                ListSelectionCommand = ListSelectorCommand,
                ListSelectorType = ListSelectorType.BatteryVoltageUnits
            };
            Batt_temperatureCompensationTextBox = new ListViewItem()
            {
                Index = 11,
                Title = AppResources.temperature_compesation,
                DefaultCellType = CellTypes.LabelLabel,
                EditableCellType = CellTypes.LabelTextEdit,
                TextInputType = InputType.Decimal,
                TextMaxLength = 3
            };
            Batt_HighTemperatureThresholdTextBox = new ListViewItem()
            {
                Index = 12,
                Title = AppResources.max_battery_temperature,
                DefaultCellType = CellTypes.LabelLabel,
                EditableCellType = CellTypes.LabelTextEdit,
                TextMaxLength = 5
            };
            Batt_foldTemperatureTextBox = new ListViewItem()
            {
                Index = 13,
                Title = AppResources.foldback_temperature,
                DefaultCellType = CellTypes.LabelLabel,
                EditableCellType = CellTypes.LabelTextEdit,
                TextInputType = InputType.Decimal,
                TextMaxLength = 5
            };
            Batt_coolDownTemperatureTextLabel = new ListViewItem()
            {
                Index = 14,
                Title = AppResources.cool_down_temperature,
                DefaultCellType = CellTypes.LabelLabel,
                EditableCellType = CellTypes.LabelTextEdit,
                TextInputType = InputType.Decimal,
                TextMaxLength = 5
            };
            Batt_trickleTemperatureTextBox = new ListViewItem()
            {
                Index = 15,
                Title = AppResources.min_cold_charge_temperature,
                DefaultCellType = CellTypes.LabelLabel,
                EditableCellType = CellTypes.LabelTextEdit,
                TextInputType = InputType.Decimal,
                TextMaxLength = 5
            };
            Batt_batteryTypeComboBox = new ListViewItem()
            {
                Index = 16,
                Title = AppResources.battery_type,
                DefaultCellType = CellTypes.LabelLabel,
                EditableCellType = CellTypes.ListSelector,
                ListSelectionCommand = ListSelectorCommand,
                ListSelectorType = ListSelectorType.BatteryType
            };
            Batt_chargerTypeTOSAVE = new ListViewItem()
            {
                Index = 17,
                Title = AppResources.application_type,
                DefaultCellType = CellTypes.LabelLabel,
                EditableCellType = CellTypes.ListSelector,
                ListSelectionCommand = ListSelectorCommand,
                ListSelectorType = ListSelectorType.ApplicationType
            };
            Batt_WarrantedAHRTextBoxTOSAVE = new ListViewItem()
            {
                Index = 18,
                Title = AppResources.warranted,
                DefaultCellType = CellTypes.LabelLabel,
                EditableCellType = CellTypes.LabelTextEdit,
                TextInputType = InputType.Number,
                TextMaxLength = 7
            };
        }

        IMvxCommand CapacityTextChanged
        {
            get
            {
                return new MvxCommand(CapacityTextValueChanged);
            }

        }

        void CapacityTextValueChanged()
        {
            ushort ahr = 0;
            if (ushort.TryParse(Batt_defaultBatteryCapacityTextBox.Text, out ahr))
            {
                Batt_WarrantedAHRTextBoxTOSAVE.Text = (1250 * ahr).ToString();
            }
        }

        #endregion

        #region Init MCB Items

        internal override void InitSharedMcbItems()
        {
            MCB_defaultBatteryCapacity24TextBox = new ListViewItem()
            {
                Index = 0,
                Title = AppResources.batt_capacity_24v,
                DefaultCellType = CellTypes.LabelLabel,
                EditableCellType = CellTypes.LabelTextEdit,
                TextMaxLength = 4
            };
            MCB_defaultBatteryCapacity36TextBox = new ListViewItem()
            {
                Index = 1,
                Title = AppResources.batt_capacity_36v,
                DefaultCellType = CellTypes.LabelLabel,
                EditableCellType = CellTypes.LabelTextEdit,
                TextMaxLength = 4
            };
            MCB_defaultBatteryCapacity48TextBox = new ListViewItem()
            {
                Index = 2,
                Title = AppResources.batt_capacity_48v,
                DefaultCellType = CellTypes.LabelLabel,
                EditableCellType = CellTypes.LabelTextEdit,
            };
            MCB_defaultBatteryCapacity80TextBox = new ListViewItem()
            {
                Index = 3,
                Title = AppResources.batt_capacity_80v,
                DefaultCellType = CellTypes.LabelLabel,
                EditableCellType = CellTypes.LabelTextEdit,
            };
            MCB_defaultBatteryCapacityTextBox = new ListViewItem()
            {
                Index = 4,
                Title = AppResources.battery_capacity,
                DefaultCellType = CellTypes.LabelLabel,
                EditableCellType = CellTypes.LabelTextEdit,
                TextMaxLength = 4
            };
            MCB_defaultBattreyVoltageComboBox = new ListViewItem()
            {
                Index = 5,
                Title = AppResources.battery_voltage,
                DefaultCellType = CellTypes.LabelLabel,
                EditableCellType = CellTypes.ListSelector,
                ListSelectionCommand = ListSelectorCommand
            };
            MCB_temperatureCompensationTextBox = new ListViewItem()
            {
                Index = 6,
                Title = AppResources.temp_compensation,
                DefaultCellType = CellTypes.LabelLabel,
                EditableCellType = CellTypes.LabelTextEdit,
                TextMaxLength = 3
            };
            MCB_nominalTemperatureRangeComboBox = new ListViewItem()
            {
                Index = 7,
                Title = AppResources.plant_temparature,
                DefaultCellType = CellTypes.LabelLabel,
                EditableCellType = CellTypes.ListSelector,
                ListSelectionCommand = ListSelectorCommand
            };
            MCB_HighTemperatureThresholdTextBox = new ListViewItem()
            {
                Index = 8,
                Title = AppResources.max_batt_temp,
                DefaultCellType = CellTypes.LabelLabel,
                EditableCellType = CellTypes.LabelTextEdit,
                TextMaxLength = 5
            };
            MCB_trickleTemperatureTextBox = new ListViewItem()
            {
                Index = 9,
                Title = AppResources.min_charge_temp,
                DefaultCellType = CellTypes.LabelLabel,
                EditableCellType = CellTypes.LabelTextEdit,
                TextMaxLength = 5
            };
            MCB_foldTemperatureTextBox = new ListViewItem()
            {
                Index = 10,
                Title = AppResources.foldback_temp,
                DefaultCellType = CellTypes.LabelLabel,
                EditableCellType = CellTypes.LabelTextEdit,
                TextMaxLength = 5
            };
            MCB_coolDownTemperatureTextLabel = new ListViewItem()
            {
                Index = 11,
                Title = AppResources.cool_down_temp,
                DefaultCellType = CellTypes.LabelLabel,
                EditableCellType = CellTypes.LabelTextEdit,
                TextMaxLength = 5
            };
            MCB_batteryTypeComboBox = new ListViewItem()
            {
                Index = 12,
                Title = AppResources.battery_type,
                DefaultCellType = CellTypes.LabelLabel,
                EditableCellType = CellTypes.ListSelector,
                ListSelectionCommand = ListSelectorCommand
            };
        }

        #endregion

        internal override void LoadExclusiveValues()
        {
        }

        public override void LoadDefaults()
        {
        }

        internal override void AddExclusiveItems()
        {
        }

        internal List<object> GetBattreyVoltageList()
        {
            object[] array;
            if (IsRegularUser())
                array = new object[]
            {
                "24",
                "36",
                "48",
                "72",
                "80"
            };
            else
                array = new object[]
            {
                "24",
                "26",
                "28",
                "30",
                "32",
                "34",
                "36",
                "38",
                "40",
                "42",
                "46",
                "48",
                "72",
                "80"
            };

            return new List<object>(array);
        }

        internal bool IsRegularUser()
        {
            return (!ControlObject.isDebugMaster)
                && (!ControlObject.isACTOem)
                && (ControlObject.UserAccess.Batt_onlyForEnginneringTeam != AccessLevelConsts.write);
        }

        internal List<object> GetBattreyTypesList()
        {
            return new List<object>
            {
                AppResources.battrey_type_lead_acid,
                AppResources.battrey_type_lithium_ion,
                AppResources.battrey_type_lead_gel
            };
        }

        internal List<object> GetChargerTypesList()
        {
            return new List<object>
            {
                AppResources.charger_type_fast,
                AppResources.charger_type_conventional,
                AppResources.charger_type_opportunity
            };
        }

        internal List<object> GetTemperatureRangeList()
        {
            return new List<object> { "<65", "65-90", ">90" };
        }
    }
}
