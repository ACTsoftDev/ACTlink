using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using static actchargers.ACUtility;

namespace actchargers
{
    public class BatterySettingsDeviceSubController : BatterySettingsBaseSubController
    {
        #region BattView Items

        ListViewItem Batt_batteryIDTextBoxTOSAVE;
        ListViewItem Batt_batteryModelTextBoxTOSAVE;
        ListViewItem Batt_batterySNTextBoxTOSAVE;
        ListViewItem Batt_BatteryManufacturingSameAsInstallation;
        ListViewItem Batt_ManufactringDatePicker;
        ListViewItem Batt_TruckIDTextBoxTOSAVE;
        ListViewItem Batt_StudyName;
        ListViewItem Batt_NumberOfCables;

        #endregion

        #region MCB Items

        ListViewItem MCB_nominalTemperatureValueTextBox;
        ListViewItem MCB_LiIon_CellVoltageTextBox;
        ListViewItem MCB_LiIon_numberOfCellsTextBox;

        #endregion

        public BatterySettingsDeviceSubController
        (bool isBattView, bool isBattViewMobile) : base(isBattView, isBattViewMobile, false)
        {
        }

        #region Init BattView Items

        internal override void InitExclusiveBattViewMobileItems()
        {
            InitExclusiveCrossBattViewItems();
        }

        internal override void InitExclusiveRegularBattViewItems()
        {
            InitExclusiveCrossBattViewItems();
        }

        void InitExclusiveCrossBattViewItems()
        {
            Batt_batteryIDTextBoxTOSAVE = new ListViewItem()
            {
                Index = 0,
                Title = AppResources.battery_id,
                DefaultCellType = CellTypes.LabelLabel,
                EditableCellType = CellTypes.LabelTextEdit,
                TextMaxLength = 15
            };
            Batt_batteryModelTextBoxTOSAVE = new ListViewItem()
            {
                Index = 1,
                Title = AppResources.battery_model,
                DefaultCellType = CellTypes.LabelLabel,
                EditableCellType = CellTypes.LabelTextEdit
            };
            Batt_batterySNTextBoxTOSAVE = new ListViewItem()
            {
                Index = 2,
                Title = AppResources.battery_serial_number,
                DefaultCellType = CellTypes.LabelLabel,
                EditableCellType = CellTypes.LabelTextEdit,
                TextMaxLength = 18
            };
            Batt_BatteryManufacturingSameAsInstallation = new ListViewItem()
            {
                Index = 4,
                Title = AppResources.battery_manufacturing_date_same_as_installation,
                DefaultCellType = CellTypes.LabelLabel,
                EditableCellType = CellTypes.LabelSwitch,
                SwitchValueChanged = SwitchValueChanged
            };
            Batt_ManufactringDatePicker = new ListViewItem()
            {
                Index = 5,
                Title = AppResources.battery_manufacturing_date,
                DefaultCellType = CellTypes.LabelLabel,
                EditableCellType = CellTypes.DatePicker,
                SwitchValueChanged = SwitchValueChanged
            };
            Batt_TruckIDTextBoxTOSAVE = new ListViewItem()
            {
                Index = 6,
                Title = AppResources.truck_id,
                DefaultCellType = CellTypes.LabelLabel,
                EditableCellType = CellTypes.LabelTextEdit,
                TextMaxLength = 17
            };
            Batt_StudyName = new ListViewItem()
            {
                Index = 7,
                Title = AppResources.study_name,
                DefaultCellType = CellTypes.LabelLabel,
                EditableCellType = CellTypes.LabelLabel,
                IsEditable = false
            };
            Batt_NumberOfCables = new ListViewItem
            {
                Index = 19,
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
        }

        #endregion

        #region Override Execute

        internal override void ExecuteSwitchValueChanged(ListViewItem item)
        {
            base.ExecuteSwitchValueChanged(item);

            if (item.Title == AppResources.battery_manufacturing_date_same_as_installation)
                ExecuteSwitchValueChangedForBatteryManufacturingSameAsInstallation(item.IsSwitchEnabled);
        }

        void ExecuteSwitchValueChangedForBatteryManufacturingSameAsInstallation(bool isSwitchEnabled)
        {
            if (isSwitchEnabled)
                Batt_ManufactringDatePicker.Date = Batt_installationDatePickerTOSAVE.Date;

            Batt_ManufactringDatePicker.ChangeEditMode(EditingMode && !isSwitchEnabled);
        }

        internal override void OnListItemSelected(ListSelectorMessage selectedListItem)
        {
            base.OnListItemSelected(selectedListItem);

            if (selectedListItem.ParentItemIndex == MCB_batteryTypeComboBox.Index)
                OnListItemSelectedForMcbBatteryType(selectedListItem);
        }

        void OnListItemSelectedForMcbBatteryType(ListSelectorMessage selectedListItem)
        {
            Task.Run(async () => await OnListItemSelectedForMcbBatteryTypeTask(selectedListItem));
        }

        async Task OnListItemSelectedForMcbBatteryTypeTask(ListSelectorMessage selectedListItem)
        {
            var currentMcb = MCBQuantum.Instance.GetMCB();
            var config = currentMcb.Config;

            AddMcbItemsWithLogic(selectedListItem.SelectedItem);

            await ChangeEditMode(EditingMode);
        }

        #endregion

        #region Init Exclusive MCB Items

        internal override void InitExclusiveMcbItems()
        {
            MCB_nominalTemperatureValueTextBox = new ListViewItem()
            {
                Index = 13,
                Title = AppResources.battery_electrolyte_temp,
                DefaultCellType = CellTypes.LabelLabel,
                EditableCellType = CellTypes.LabelTextEdit,
                TextMaxLength = 4
            };
            MCB_LiIon_CellVoltageTextBox = new ListViewItem()
            {
                Index = 14,
                Title = AppResources.li_ion_cell_voltage,
                DefaultCellType = CellTypes.LabelLabel,
                EditableCellType = CellTypes.LabelTextEdit,
                TextMinLength = 1
            };
            MCB_LiIon_numberOfCellsTextBox = new ListViewItem()
            {
                Index = 15,
                Title = AppResources.li_ion_number_of_cells,
                DefaultCellType = CellTypes.LabelLabel,
                EditableCellType = CellTypes.LabelTextEdit,
                TextMinLength = 1
            };
        }

        #endregion

        #region Load BattView Values

        internal override void LoadBattViewMobileValues()
        {
            LoadExclusiveCrossBattViewValues();
        }

        internal override void LoadRegularBattViewValues()
        {
            LoadExclusiveCrossBattViewValues();
        }

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
            BattViewObject currentBattView = BattViewQuantum.Instance.GetBATTView();
            var config = currentBattView.Config;

            Batt_batteryIDTextBoxTOSAVE.SubTitle = Batt_batteryIDTextBoxTOSAVE.Text = config.batteryID;
            Batt_batteryModelTextBoxTOSAVE.SubTitle = Batt_batteryModelTextBoxTOSAVE.Text = config.batterymodel;
            Batt_batterySNTextBoxTOSAVE.SubTitle = Batt_batterySNTextBoxTOSAVE.Text = config.batterysn;

            Batt_installationDatePickerTOSAVE.MinDate = MIN_INSTALLATION_DATE;
            Batt_installationDatePickerTOSAVE.MaxDate = MAX_INSTALLATION_DATE;

            Batt_ManufactringDatePicker.MinDate = MIN_MANUFACTRING_DATE;
            Batt_ManufactringDatePicker.MaxDate = MAX_MANUFACTRING_DATE;

            DateTime toSelect = DateTime.Now;

            Batt_ManufactringDatePicker.Date = toSelect;
            Batt_ManufactringDatePicker.SubTitle = Batt_ManufactringDatePicker.Date.ToString(ACConstants.DATE_TIME_FORMAT_IOS_UI);

            Batt_TruckIDTextBoxTOSAVE.SubTitle = Batt_TruckIDTextBoxTOSAVE.Text = config.TruckId;

            Batt_StudyName.SubTitle = config.studyName;

            if (config.installationDate < Batt_installationDatePickerTOSAVE.MinDate ||
                config.installationDate > Batt_installationDatePickerTOSAVE.MaxDate)
            {
                Batt_installationDatePickerTOSAVE.Date = DateTime.UtcNow;
                Batt_installationDatePickerTOSAVE.SubTitle = DateTime.UtcNow.Date.ToString(ACConstants.DATE_TIME_FORMAT_IOS_UI);
            }
            else
            {
                Batt_installationDatePickerTOSAVE.Date = config.installationDate;
                Batt_installationDatePickerTOSAVE.SubTitle = Batt_installationDatePickerTOSAVE.Date.ToString(ACConstants.DATE_TIME_FORMAT_IOS_UI);
            }

            if (config.batteryManfacturingDate < Batt_ManufactringDatePicker.MinDate ||
                config.batteryManfacturingDate > Batt_ManufactringDatePicker.MaxDate)
            {
                Batt_ManufactringDatePicker.Date = DateTime.UtcNow;
                Batt_ManufactringDatePicker.SubTitle = DateTime.UtcNow.Date.ToString(ACConstants.DATE_TIME_FORMAT_IOS_UI);
            }
            else
            {
                Batt_ManufactringDatePicker.Date = config.batteryManfacturingDate;
                Batt_ManufactringDatePicker.SubTitle = Batt_ManufactringDatePicker.Date.ToString(ACConstants.DATE_TIME_FORMAT_IOS_UI);
            }

            Batt_BatteryManufacturingSameAsInstallation.IsSwitchEnabled = Batt_installationDatePickerTOSAVE.Date == Batt_ManufactringDatePicker.Date;

            Batt_timeZoneComboBox.Items = StaticDataAndHelperFunctions.GetZonesListAsObjects();
            if (currentBattView.myZone == 0)
            {
                Batt_timeZoneComboBox.SelectedIndex = -1;
            }
            else
            {
                Batt_timeZoneComboBox.SelectedItem = StaticDataAndHelperFunctions.getZoneByID(currentBattView.myZone).display_name;
                Batt_timeZoneComboBox.SelectedIndex = StaticDataAndHelperFunctions.GetZonesList().FindIndex(o => o.display_name == Batt_timeZoneComboBox.SelectedItem);
            }

            Batt_defaultBatteryCapacityTextBox.SubTitle = Batt_defaultBatteryCapacityTextBox.Text = config.ahrcapacity.ToString();

            Batt_defaultBattreyVoltageComboBox.Items = GetBattreyVoltageList();
            if (IsRegularUser())
            {
                if (config.nominalvoltage != 24 && config.nominalvoltage != 36
                    && config.nominalvoltage != 48 && config.nominalvoltage != 72
                    && config.nominalvoltage != 80 && config.nominalvoltage % 2 == 0)
                {
                    Batt_defaultBattreyVoltageComboBox.Items.Add(config.nominalvoltage.ToString());
                }
            }

            Batt_defaultBattreyVoltageComboBox.SelectedItem = config.nominalvoltage.ToString();
            Batt_defaultBattreyVoltageComboBox.SelectedIndex = Batt_defaultBattreyVoltageComboBox.Items.FindIndex(o => ((string)o).Equals(Batt_defaultBattreyVoltageComboBox.SelectedItem));

            Batt_temperatureCompensationTextBox.SubTitle = Batt_temperatureCompensationTextBox.Text = (config.batteryTemperatureCompesnation / 10.0f).ToString("N1").TrimEnd('0').TrimEnd(System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator[0]);

            Batt_HighTemperatureThresholdTextBox.SubTitle = Batt_HighTemperatureThresholdTextBox.Text = ((config.batteryHighTemperature / 10.0f) * 1.8 + 32).ToString("N0");
            Batt_foldTemperatureTextBox.SubTitle = Batt_foldTemperatureTextBox.Text = ((config.foldTemperature / 10.0f) * 1.8 + 32).ToString("N0");
            Batt_coolDownTemperatureTextLabel.SubTitle = Batt_coolDownTemperatureTextLabel.Text = ((config.coolDownTemperature / 10.0f) * 1.8 + 32).ToString("N0");
            Batt_trickleTemperatureTextBox.SubTitle = Batt_trickleTemperatureTextBox.Text = ((config.TRTemperature / 10.0f) * 1.8 + 32).ToString("N0");

            Batt_batteryTypeComboBox.Items = GetBattreyTypesList();

            if (Batt_batteryTypeComboBox.Items.Count > config.batteryType)
            {
                Batt_batteryTypeComboBox.SelectedIndex = config.batteryType;
                Batt_batteryTypeComboBox.SelectedItem = Batt_batteryTypeComboBox.Items[Batt_batteryTypeComboBox.SelectedIndex].ToString();
            }

            Batt_chargerTypeTOSAVE.Items = GetChargerTypesList();
            if (Batt_chargerTypeTOSAVE.Items.Count > config.chargerType)
            {
                Batt_chargerTypeTOSAVE.SelectedIndex = config.chargerType;
                Batt_chargerTypeTOSAVE.SelectedItem = Batt_chargerTypeTOSAVE.Items[Batt_chargerTypeTOSAVE.SelectedIndex].ToString();
            }

            Batt_WarrantedAHRTextBoxTOSAVE.SubTitle = Batt_WarrantedAHRTextBoxTOSAVE.Text = config.warrantedAHR.ToString();

            if (config.HallEffectScale > 1.5f)
            {
                Batt_NumberOfCables.SelectedIndex = 1;
                Batt_NumberOfCables.SelectedItem = Batt_NumberOfCables.Items[1].ToString();
            }
            else
            {
                Batt_NumberOfCables.SelectedIndex = 0;
                Batt_NumberOfCables.SelectedItem = Batt_NumberOfCables.Items[0].ToString();
            }
        }

        #endregion

        #region Load MCB Values

        internal override void LoadMcbValues()
        {
            var currentMcb = MCBQuantum.Instance.GetMCB();
            var config = currentMcb.Config;

            if (currentMcb.FirmwareRevision < 2.04f)
            {
                MCB_defaultBatteryCapacity24TextBox.IsVisible = false;
                MCB_defaultBatteryCapacity36TextBox.IsVisible = false;
                MCB_defaultBatteryCapacity48TextBox.IsVisible = false;
                MCB_defaultBatteryCapacity80TextBox.IsVisible = false;

                MCB_defaultBatteryCapacityTextBox.IsVisible |= ControlObject.UserAccess.MCB_BatteryCapacity != AccessLevelConsts.noAccess;
                MCB_defaultBattreyVoltageComboBox.IsVisible |= ControlObject.UserAccess.MCB_BatteryVoltage != AccessLevelConsts.noAccess;
            }

            MCB_defaultBatteryCapacity24TextBox.SubTitle = MCB_defaultBatteryCapacity24TextBox.Text = config.batteryCapacity24.ToString();
            MCB_defaultBatteryCapacity36TextBox.SubTitle = MCB_defaultBatteryCapacity36TextBox.Text = config.batteryCapacity36.ToString();
            MCB_defaultBatteryCapacity48TextBox.SubTitle = MCB_defaultBatteryCapacity48TextBox.Text = config.batteryCapacity48.ToString();
            MCB_defaultBatteryCapacity80TextBox.SubTitle = MCB_defaultBatteryCapacity80TextBox.Text = config.batteryCapacity80.ToString();

            MCB_defaultBatteryCapacityTextBox.SubTitle = MCB_defaultBatteryCapacityTextBox.Text = config.batteryCapacity;

            MCB_defaultBattreyVoltageComboBox.Items = GetBattreyVoltageList();

            if (IsRegularUser() || currentMcb.FirmwareRevision < 2.11f)
            {
                MCB_defaultBattreyVoltageComboBox.Items.Add("24");
                MCB_defaultBattreyVoltageComboBox.Items.Add("36");

                if (config.PMvoltage == "48")
                    MCB_defaultBattreyVoltageComboBox.Items.Add("48");

                int volt = int.Parse(config.batteryVoltage);

                if (volt != 24 && volt != 36 && volt != 48 && currentMcb.FirmwareRevision > 2.1f)
                {
                    if (volt % 2 == 0)
                        MCB_defaultBattreyVoltageComboBox.Items.Add(config.batteryVoltage);
                }
            }
            else
            {
                for (int i = 24; i <= int.Parse(config.PMvoltage); i += 2)
                    MCB_defaultBattreyVoltageComboBox.Items.Add(i.ToString());
            }

            MCB_defaultBattreyVoltageComboBox.SubTitle = MCB_defaultBattreyVoltageComboBox.SelectedItem = config.batteryVoltage;

            double c = config.nominal_temperature_shift / 2.0;

            MCB_nominalTemperatureRangeComboBox.Items = GetTemperatureRangeList();

            MCB_nominalTemperatureRangeComboBox.SelectedIndex = c > 0.1 ? 2 : c < -0.1 ? 0 : 1;

            switch (MCB_nominalTemperatureRangeComboBox.SelectedIndex)
            {
                case 0:
                    MCB_nominalTemperatureValueTextBox.SubTitle = MCB_nominalTemperatureValueTextBox.Text = "18.0";
                    break;
                case 1:
                    MCB_nominalTemperatureValueTextBox.SubTitle = MCB_nominalTemperatureValueTextBox.Text = "25.0";
                    break;
                case 2:
                    MCB_nominalTemperatureValueTextBox.SubTitle = MCB_nominalTemperatureValueTextBox.Text = "32.0";
                    break;
            }

            MCB_nominalTemperatureValueTextBox.IsVisible = currentMcb.FirmwareRevision >= 2.14f && ControlObject.UserAccess.MCB_onlyForEnginneringTeam != AccessLevelConsts.noAccess;

            if (currentMcb.FirmwareRevision < 2.14f)
            {
                MCB_nominalTemperatureRangeComboBox.IsVisible = false;
            }
            else
            {
                MCB_nominalTemperatureRangeComboBox.IsVisible = true;

                if (MCB_nominalTemperatureRangeComboBox.SelectedIndex != -1)
                    MCB_nominalTemperatureRangeComboBox.SelectedItem = MCB_nominalTemperatureRangeComboBox.Items[MCB_nominalTemperatureRangeComboBox.SelectedIndex].ToString();
            }

            MCB_temperatureCompensationTextBox.SubTitle = MCB_temperatureCompensationTextBox.Text = config.temperatureVoltageCompensation;

            MCB_HighTemperatureThresholdTextBox.SubTitle = config.temperatureFormat ? (MCB_HighTemperatureThresholdTextBox.Text = config.maxTemperatureFault) : (MCB_HighTemperatureThresholdTextBox.Text = (float.Parse(config.maxTemperatureFault) * 1.8 + 32).ToString("N0"));

            MCB_trickleTemperatureTextBox.SubTitle = config.temperatureFormat ? (MCB_trickleTemperatureTextBox.Text = config.TRtemperature) : (MCB_trickleTemperatureTextBox.Text = (float.Parse(config.TRtemperature) * 1.8 + 32).ToString("N0"));

            MCB_foldTemperatureTextBox.SubTitle = config.temperatureFormat ? (MCB_foldTemperatureTextBox.Text = config.foldTemperature) : (MCB_foldTemperatureTextBox.Text = (float.Parse(config.foldTemperature) * 1.8 + 32).ToString("N0"));

            MCB_coolDownTemperatureTextLabel.SubTitle = config.temperatureFormat ? (MCB_coolDownTemperatureTextLabel.Text = config.coolDownTemperature) : (MCB_coolDownTemperatureTextLabel.Text = (float.Parse(config.coolDownTemperature) * 1.8 + 32).ToString("N0"));

            MCB_nominalTemperatureValueTextBox.IsVisible = currentMcb.FirmwareRevision >= 2.14f && ControlObject.UserAccess.MCB_onlyForEnginneringTeam != AccessLevelConsts.noAccess;

            MCB_batteryTypeComboBox.Items = GetBattreyTypesList();

            MCB_batteryTypeComboBox.SubTitle = MCB_batteryTypeComboBox.SelectedItem = config.batteryType;

            MCB_LiIon_CellVoltageTextBox.SubTitle = MCB_LiIon_CellVoltageTextBox.Text = (config.LiIon_CellVoltage / 100.0).ToString("N2");
            MCB_LiIon_numberOfCellsTextBox.SubTitle = MCB_LiIon_numberOfCellsTextBox.Text = Convert.ToInt16(config.LiIon_numberOfCells).ToString("N0");
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
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_InstallationDate, Batt_BatteryManufacturingSameAsInstallation, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_InstallationDate, Batt_ManufactringDatePicker, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_BatteryCapacity, Batt_defaultBatteryCapacityTextBox, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_BatteryVoltage, Batt_defaultBattreyVoltageComboBox, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_temperatureCompensation, Batt_temperatureCompensationTextBox, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_TempertureHigh, Batt_HighTemperatureThresholdTextBox, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_BatteryType, Batt_batteryTypeComboBox, ItemSource);


            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_batteryID, Batt_batteryIDTextBoxTOSAVE, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_batteryModelandSN, Batt_batteryModelTextBoxTOSAVE, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_batteryModelandSN, Batt_TruckIDTextBoxTOSAVE, ItemSource);

            if (isBattViewMobile)
                accessControlUtility
                    .DoApplyAccessControl
                        (AccessLevelConsts.write, Batt_StudyName, ItemSource);

            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_batteryModelandSN, Batt_batterySNTextBoxTOSAVE, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_TempertureHigh, Batt_trickleTemperatureTextBox, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_TempertureHigh, Batt_foldTemperatureTextBox, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_TempertureHigh, Batt_coolDownTemperatureTextLabel, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_chargerType, Batt_chargerTypeTOSAVE, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_TimeZone, Batt_timeZoneComboBox, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_OverrideWarrantedAHR, Batt_WarrantedAHRTextBoxTOSAVE, ItemSource);

            if (BattViewQuantum.Instance.GetBATTView().Config.enableHallEffectSensing != 0x00)
            {
                accessControlUtility
                    .DoApplyAccessControl
                    (AccessLevelConsts.write, Batt_NumberOfCables, ItemSource);
            }

            ShowEdit &= accessControlUtility.GetSavedCount() != 0;

            return accessControlUtility.GetVisibleCount();
        }

        #endregion

        #region Add MCB

        internal override int McbAccessApply()
        {
            return AddMcbItemsWithLogic(MCB_batteryTypeComboBox.SelectedItem);
        }

        int AddMcbItemsWithLogic(string selectedBatteryType)
        {
            ItemSource.Clear();

            accessControlUtility = new UIAccessControlUtility();

            var currentMcb = MCBQuantum.Instance.GetMCB();
            var config = currentMcb.Config;

            bool is2_5OrAbove = McbHelper.IsFirmwarEequalOrAbove(currentMcb, 2.5f);
            bool isLithium = selectedBatteryType.Equals("Lithium-ion", StringComparison.OrdinalIgnoreCase);
            bool isLithiumAnd2_5OrAbove = is2_5OrAbove && isLithium;
            bool canAddLithium = isLithiumAnd2_5OrAbove && !config.enableAutoDetectMultiVoltage;

            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_BatteryCapacity, MCB_defaultBatteryCapacity24TextBox, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_BatteryCapacity, MCB_defaultBatteryCapacity36TextBox, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_BatteryCapacity, MCB_defaultBatteryCapacity48TextBox, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_BatteryCapacity, MCB_defaultBatteryCapacity80TextBox, ItemSource);

            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_BatteryCapacity, MCB_defaultBatteryCapacityTextBox, ItemSource);

            if (!isLithiumAnd2_5OrAbove)
            {
                accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_BatteryVoltage, MCB_defaultBattreyVoltageComboBox, ItemSource);
                accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_temperatureCompensation, MCB_temperatureCompensationTextBox, ItemSource);
            }

            if (MCB_nominalTemperatureRangeComboBox.IsVisible)
                accessControlUtility.DoApplyAccessControl(AccessLevelConsts.write, MCB_nominalTemperatureRangeComboBox, ItemSource);

            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_TempertureHigh, MCB_HighTemperatureThresholdTextBox, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_TempertureHigh, MCB_trickleTemperatureTextBox, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_TempertureHigh, MCB_foldTemperatureTextBox, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_TempertureHigh, MCB_coolDownTemperatureTextLabel, ItemSource);

            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_BatteryType, MCB_batteryTypeComboBox, ItemSource);

            if (MCB_nominalTemperatureValueTextBox.IsVisible)
                accessControlUtility.DoApplyAccessControl(AccessLevelConsts.write, MCB_nominalTemperatureValueTextBox, ItemSource);

            if (canAddLithium)
            {
                accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_BatteryVoltage, MCB_LiIon_CellVoltageTextBox, ItemSource);
                accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_BatteryVoltage, MCB_LiIon_numberOfCellsTextBox, ItemSource);
            }

            ShowEdit &= accessControlUtility.GetSavedCount() != 0;

            int visibleCount = accessControlUtility.GetVisibleCount();

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

            verifyControl.VerifyTextBox(Batt_batteryIDTextBoxTOSAVE, Batt_batteryIDTextBoxTOSAVE, 1, 17);
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

            var currentMcb = MCBQuantum.Instance.GetMCB();
            var config = currentMcb.Config;
            bool isLithiumAnd2_5OrAbove = McbHelper.IsLithiumAnd2_5OrAbove(currentMcb);
            bool canAddLithium = isLithiumAnd2_5OrAbove && !config.enableAutoDetectMultiVoltage;

            verifyControl.VerifyInteger(MCB_defaultBatteryCapacityTextBox, MCB_defaultBatteryCapacityTextBox, 40, 5000);
            verifyControl.VerifyInteger(MCB_defaultBatteryCapacity24TextBox, MCB_defaultBatteryCapacity24TextBox, 40, 5000);
            verifyControl.VerifyInteger(MCB_defaultBatteryCapacity36TextBox, MCB_defaultBatteryCapacity36TextBox, 40, 5000);
            verifyControl.VerifyInteger(MCB_defaultBatteryCapacity48TextBox, MCB_defaultBatteryCapacity48TextBox, 40, 5000);
            verifyControl.VerifyInteger(MCB_defaultBatteryCapacity80TextBox, MCB_defaultBatteryCapacity80TextBox, 40, 5000);

            if (!isLithiumAnd2_5OrAbove)
            {
                verifyControl.VerifyComboBox(MCB_defaultBattreyVoltageComboBox);
                verifyControl.VerifyFloatNumber(MCB_temperatureCompensationTextBox, MCB_temperatureCompensationTextBox, 3.0f, 8.0f);
            }

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

            verifyControl.VerifyFloatNumber(MCB_nominalTemperatureValueTextBox, MCB_nominalTemperatureValueTextBox, -25.0f, 50.0f);

            if (canAddLithium)
            {
                verifyControl.VerifyFloatNumber(MCB_LiIon_CellVoltageTextBox, MCB_LiIon_CellVoltageTextBox, 2.5f, 4.0f);
                verifyControl.VerifyUInteger(MCB_LiIon_numberOfCellsTextBox, MCB_LiIon_numberOfCellsTextBox, 8, 27);
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

            bool isDirty = false;

            device.RequireRefresh = config.batteryID != Batt_batteryIDTextBoxTOSAVE.Text;

            if (config.batteryID != Batt_batteryIDTextBoxTOSAVE.Text)
            {
                isDirty = true;
                config.batteryID = Batt_batteryIDTextBoxTOSAVE.Text;
            }

            if (config.batterymodel != Batt_batteryModelTextBoxTOSAVE.Text)
            {
                isDirty = true;
                config.batterymodel = Batt_batteryModelTextBoxTOSAVE.Text;
            }

            if (config.batterysn != Batt_batterySNTextBoxTOSAVE.Text)
            {
                isDirty = true;
                config.batterysn = Batt_batterySNTextBoxTOSAVE.Text;
            }

            if (config.installationDate.Date != Batt_installationDatePickerTOSAVE.Date.Date)
            {
                isDirty = true;
                config.installationDate = Batt_installationDatePickerTOSAVE.Date.Date;
            }

            JsonZone info = StaticDataAndHelperFunctions.getZoneByText(Batt_timeZoneComboBox.Text);
            if (device.myZone != info.id)
            {
                isDirty = true;
                device.myZone = info.id;
            }

            if (config.warrantedAHR != UInt32.Parse(Batt_WarrantedAHRTextBoxTOSAVE.Text))
            {
                isDirty = true;
                config.warrantedAHR = UInt32.Parse(Batt_WarrantedAHRTextBoxTOSAVE.Text);
            }

            if (config.ahrcapacity != ushort.Parse(Batt_defaultBatteryCapacityTextBox.Text))
            {
                isDirty = true;
                config.ahrcapacity = ushort.Parse(Batt_defaultBatteryCapacityTextBox.Text);
            }

            if (config.nominalvoltage != byte.Parse(Batt_defaultBattreyVoltageComboBox.Text))
            {
                isDirty = true;
                config.nominalvoltage = byte.Parse(Batt_defaultBattreyVoltageComboBox.Text);
            }

            if (config.batteryTemperatureCompesnation != (byte)Math.Round(float.Parse(Batt_temperatureCompensationTextBox.Text) * 10.0f))
            {
                isDirty = true;
                config.batteryTemperatureCompesnation = (byte)Math.Round(float.Parse(Batt_temperatureCompensationTextBox.Text) * 10.0f);
            }

            if (config.batteryHighTemperature != (ushort)Math.Round(((float.Parse(Batt_HighTemperatureThresholdTextBox.Text) - 32) / 1.8) * 10.0f))
            {
                isDirty = true;
                config.batteryHighTemperature = (ushort)Math.Round(((float.Parse(Batt_HighTemperatureThresholdTextBox.Text) - 32) / 1.8) * 10.0f);
            }

            if (config.foldTemperature != (short)Math.Round(((float.Parse(Batt_foldTemperatureTextBox.Text) - 32) / 1.8) * 10.0f))
            {
                isDirty = true;
                config.foldTemperature = (short)Math.Round(((float.Parse(Batt_foldTemperatureTextBox.Text) - 32) / 1.8) * 10.0f);
            }

            if (config.coolDownTemperature != (short)Math.Round(((float.Parse(Batt_coolDownTemperatureTextLabel.Text) - 32) / 1.8) * 10.0f))
            {
                isDirty = true;
                config.coolDownTemperature = (short)Math.Round(((float.Parse(Batt_coolDownTemperatureTextLabel.Text) - 32) / 1.8) * 10.0f);
            }

            if (config.TRTemperature != (short)Math.Round(((float.Parse(Batt_trickleTemperatureTextBox.Text) - 32) / 1.8) * 10.0f))
            {
                isDirty = true;
                config.TRTemperature = (short)Math.Round(((float.Parse(Batt_trickleTemperatureTextBox.Text) - 32) / 1.8) * 10.0f);
            }

            if (config.batteryType != (byte)Batt_batteryTypeComboBox.SelectedIndex)
            {
                isDirty = true;
                config.batteryType = (byte)Batt_batteryTypeComboBox.SelectedIndex;
                BattViewQuantum.Instance.Batt_saveDefaultChargeProfile();
            }

            if (Batt_installationDatePickerTOSAVE.Date != config.installationDate)
            {
                isDirty = true;
                config.installationDate = Batt_installationDatePickerTOSAVE.Date;
            }

            if (Batt_ManufactringDatePicker.Date != config.batteryManfacturingDate)
            {
                isDirty = true;
                config.batteryManfacturingDate = Batt_ManufactringDatePicker.Date;
            }

            if (config.chargerType != (byte)Batt_chargerTypeTOSAVE.SelectedIndex)
            {
                isDirty = true;
                config.chargerType = (byte)Batt_chargerTypeTOSAVE.SelectedIndex;

                if (!CompareBattViewWithDefaultChargeProfile())
                    BattViewQuantum.Instance.Batt_saveDefaultChargeProfile();
            }

            if (config.TruckId != Batt_TruckIDTextBoxTOSAVE.Text)
            {
                if (config.firmwareversion < 2.09f && !isDirty)
                {
                    //before 2.09 truckid was not introduced , so we have to same something to let the battview take it
                    if (config.warrantedAHR % 2 == 0)
                        config.warrantedAHR++;
                    else
                        config.warrantedAHR--;
                }

                config.TruckId = Batt_TruckIDTextBoxTOSAVE.Text;
            }

            if (config.enableHallEffectSensing != 0x00)
            {
                int numberOfCablesIndex = Batt_NumberOfCables.SelectedIndex;
                switch (numberOfCablesIndex)
                {
                    case 0:
                        config.HallEffectScale = 1.0f;

                        break;

                    case 1:
                        config.HallEffectScale = 2.0f;

                        break;
                }
            }
        }

        bool CompareBattViewWithDefaultChargeProfile()
        {
            if (!BattViewQuantum.Instance.BattView_quickSerialNumberCheckStrict())
                return true;

            if (BattViewQuantum.Instance.GetBATTView().Config.trickleCurrentRate != 500 ||
            BattViewQuantum.Instance.GetBATTView().Config.FIcurrentRate != 500 ||
            BattViewQuantum.Instance.GetBATTView().Config.EQcurrentRate != 400 ||
            BattViewQuantum.Instance.GetBATTView().Config.trickleVoltage != 200 ||
            BattViewQuantum.Instance.GetBATTView().Config.FItargetVoltage != 260 ||
            BattViewQuantum.Instance.GetBATTView().Config.EQvoltage != 265 ||
            BattViewQuantum.Instance.GetBATTView().Config.CVendCurrentRate != 24 ||
            BattViewQuantum.Instance.GetBATTView().Config.CVcurrentStep != 0 ||
            BattViewQuantum.Instance.GetBATTView().Config.cvMaxDuration != 14400 ||
            BattViewQuantum.Instance.GetBATTView().Config.FIduration != 10800 ||
            BattViewQuantum.Instance.GetBATTView().Config.EQduration != 14400 ||
            BattViewQuantum.Instance.GetBATTView().Config.desulfation != 43200 ||
            BattViewQuantum.Instance.GetBATTView().Config.FIdv != 5 ||
            BattViewQuantum.Instance.GetBATTView().Config.FIdt != 59 ||
            BattViewQuantum.Instance.GetBATTView().Config.EQcloseWindow != 86400 ||
            BattViewQuantum.Instance.GetBATTView().Config.EQstartWindow != 0 ||
            BattViewQuantum.Instance.GetBATTView().Config.FIstartWindow != 0)
                return false;
            switch (BattViewQuantum.Instance.GetBATTView().Config.chargerType)
            {
                case 0:
                    //FAST
                    if (BattViewQuantum.Instance.GetBATTView().Config.CCrate != 4000 ||
                    BattViewQuantum.Instance.GetBATTView().Config.CVTargetVoltage != 242 ||
                        BattViewQuantum.Instance.GetBATTView().Config.FIcloseWindow != 86400 ||
                        BattViewQuantum.Instance.GetBATTView().Config.FIdaysMask != (0x01 | 0x40) ||
                        BattViewQuantum.Instance.GetBATTView().Config.EQdaysMask != (0x01))
                        return false;
                    break;
                case 1:
                    if (BattViewQuantum.Instance.GetBATTView().Config.CCrate != 1700 ||
        BattViewQuantum.Instance.GetBATTView().Config.CVTargetVoltage != 237 ||
            BattViewQuantum.Instance.GetBATTView().Config.FIcloseWindow != 86400 ||
            BattViewQuantum.Instance.GetBATTView().Config.FIdaysMask != 0x7f ||
            BattViewQuantum.Instance.GetBATTView().Config.EQdaysMask != (0x01))
                        return false;
                    break;
                case 2:
                    //Opp
                    if (BattViewQuantum.Instance.GetBATTView().Config.CCrate != 2500 ||
                    BattViewQuantum.Instance.GetBATTView().Config.CVTargetVoltage != 240 ||
                        BattViewQuantum.Instance.GetBATTView().Config.FIcloseWindow != 8 * 3600 ||
                        BattViewQuantum.Instance.GetBATTView().Config.FIdaysMask != 0x7f ||
                        BattViewQuantum.Instance.GetBATTView().Config.EQdaysMask != (0x01))
                        return false;
                    break;
            }

            return true;
        }

        internal override async Task<bool> SaveBattViewAndReturnResult(BattViewObject device)
        {
            var result = await device.SaveConfigAndTime();

            if (NeedsLoad(result, device.RequireRefresh))
                result = await DoLoad(device);

            return result;
        }

        bool NeedsLoad(bool result, bool requireRefresh)
        {
            return result && requireRefresh;
        }

        async Task<bool> DoLoad(BattViewObject device)
        {
            bool result = await device.DoLoad();

            if (result)
                device.RequireRefresh = false;

            return result;
        }

        #endregion

        #region Save MCB

        internal override void SaveMcbToConfigObject(MCBobject device)
        {
            var config = device.Config;
            bool isLithiumAnd2_5OrAbove = McbHelper.IsLithiumAnd2_5OrAbove(device);
            bool canAddLithium = isLithiumAnd2_5OrAbove && !config.enableAutoDetectMultiVoltage;

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

            if (!isLithiumAnd2_5OrAbove)
            {
                config.batteryVoltage = MCB_defaultBattreyVoltageComboBox.Text;
                config.temperatureVoltageCompensation = MCB_temperatureCompensationTextBox.Text;
            }

            config.maxTemperatureFault = config.temperatureFormat ? MCB_HighTemperatureThresholdTextBox.Text : ((float.Parse(MCB_HighTemperatureThresholdTextBox.Text) - 32) / 1.8f).ToString("N1");

            config.batteryType = MCB_batteryTypeComboBox.Text;

            config.TRtemperature = config.temperatureFormat ? MCB_trickleTemperatureTextBox.Text : ((float.Parse(MCB_trickleTemperatureTextBox.Text) - 32) / 1.8f).ToString("N1");

            config.foldTemperature = config.temperatureFormat ? MCB_foldTemperatureTextBox.Text : ((float.Parse(MCB_foldTemperatureTextBox.Text) - 32) / 1.8f).ToString("N1");

            config.coolDownTemperature = config.temperatureFormat ? MCB_coolDownTemperatureTextLabel.Text : ((float.Parse(MCB_coolDownTemperatureTextLabel.Text) - 32) / 1.8f).ToString("N1");

            if (device.FirmwareRevision > 2.13f)
            {
                double c = double.Parse(MCB_nominalTemperatureValueTextBox.Text);
                sbyte v = (sbyte)Math.Round(2.0 * (c - 25.0));
                config.nominal_temperature_shift = v;
            }

            if (canAddLithium)
            {
                float cellVoltage = float.Parse(MCB_LiIon_CellVoltageTextBox.Text);
                double cellVoltage100 = cellVoltage * 100;
                config.LiIon_CellVoltage = Convert.ToUInt16(cellVoltage100);

                config.LiIon_numberOfCells = byte.Parse(MCB_LiIon_numberOfCellsTextBox.Text);
            }
        }

        #endregion
    }
}
