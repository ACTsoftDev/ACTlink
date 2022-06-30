using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;
using Newtonsoft.Json;
using static actchargers.ACUtility;
namespace actchargers
{
    public class SaveBatterySettingsViewModel : BaseViewModel
    {
        MvxSubscriptionToken _mListSelector;
        List<string> device_LoadWarnings;

        ListViewItem Batt_batteryIDTextBoxTOSAVE;//Textbox
        ListViewItem Batt_batteryModelTextBoxTOSAVE;//Textbox
        ListViewItem Batt_batterySNTextBoxTOSAVE;//Textbox
        ListViewItem Batt_installationDatePickerTOSAVE;//DatePicker
        ListViewItem Batt_ManufactringDatePicker;//DatePicker
        ListViewItem Batt_ManufactringDateCheckBox;//Switch
        ListViewItem Batt_TruckIDTextBoxTOSAVE;//Textbox
        ListViewItem Batt_StudyName;
        ListViewItem Batt_timeZoneComboBox;// Combobox
        ListViewItem Batt_defaultBatteryCapacityTextBox;
        ListViewItem Batt_defaultBattreyVoltageComboBox;// Combobox
        ListViewItem Batt_temperatureCompensationTextBox; //Textbox
        ListViewItem Batt_HighTemperatureThresholdTextBox; //Textbox
        ListViewItem Batt_foldTemperatureTextBox; //Textbox
        ListViewItem Batt_coolDownTemperatureTextLabel; //Textbox
        ListViewItem Batt_trickleTemperatureTextBox; //Textbox
        ListViewItem Batt_batteryTypeComboBox; //Combobox
        ListViewItem Batt_chargerTypeTOSAVE; //Combobox
        ListViewItem Batt_WarrantedAHRTextBoxTOSAVE;//Textbox

        ListViewItem MCB_defaultBatteryCapacity24TextBox;//Textbox
        ListViewItem MCB_defaultBatteryCapacity36TextBox;//Textbox
        ListViewItem MCB_defaultBatteryCapacity48TextBox;//Textbox
        ListViewItem MCB_defaultBatteryCapacityTextBox;//Textbox
        ListViewItem MCB_defaultBattreyVoltageComboBox;//Combobox
        ListViewItem MCB_nominalTemperatureRangeComboBox;//Combobox
        ListViewItem MCB_temperatureCompensationTextBox;//Textbox
        ListViewItem MCB_HighTemperatureThresholdTextBox;//Textbox
        ListViewItem MCB_trickleTemperatureTextBox;//Textbox
        ListViewItem MCB_foldTemperatureTextBox;//Textbox
        ListViewItem MCB_coolDownTemperatureTextLabel;//Textblock
        ListViewItem MCB_batteryTypeComboBox;//Combobox
        ListViewItem MCB_nominalTemperatureValueTextBox;

        VerifyControl verifyControl;
        /// <summary>
        /// The edit mode.
        /// </summary>
        private bool _showEdit;
        public bool ShowEdit
        {
            get
            {
                return _showEdit;
            }
            set
            {
                _showEdit = value;
                RaisePropertyChanged(() => ShowEdit);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:actchargers.BatterySettingsViewModel"/> class.
        /// </summary>
        public SaveBatterySettingsViewModel()
        {
            BatterySettingsItemSource = new ObservableCollection<ListViewItem>();
            ViewTitle = AppResources.battery_settings;
            ShowEdit = true;
            device_LoadWarnings = new List<string>();
            CreateList();


        }

        void CreateList()
        {
            if (IsBattView)
            {
                CreateListForBattView();
            }
            else
            {
                CreateListForChargers();
            }
        }

        void CreateListForChargers()
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
            MCB_defaultBatteryCapacityTextBox = new ListViewItem()
            {
                Index = 3,
                Title = AppResources.battery_capacity,
                DefaultCellType = CellTypes.LabelLabel,
                EditableCellType = CellTypes.LabelTextEdit,
                TextMaxLength = 4
            };
            MCB_defaultBattreyVoltageComboBox = new ListViewItem()
            {
                Index = 4,
                Title = AppResources.battery_voltage,
                DefaultCellType = CellTypes.LabelLabel,
                EditableCellType = CellTypes.ListSelector,
                ListSelectionCommand = ListSelectorCommand
            };
            MCB_nominalTemperatureRangeComboBox = new ListViewItem()
            {
                Index = 9,
                Title = AppResources.plant_temparature,
                DefaultCellType = CellTypes.LabelLabel,
                EditableCellType = CellTypes.ListSelector,
                ListSelectionCommand = ListSelectorCommand
            };
            MCB_temperatureCompensationTextBox = new ListViewItem()
            {

                Index = 3,
                Title = AppResources.temp_compensation,
                DefaultCellType = CellTypes.LabelLabel,
                EditableCellType = CellTypes.LabelTextEdit,
                TextMaxLength = 3
            };
            MCB_HighTemperatureThresholdTextBox = new ListViewItem()
            {

                Index = 4,
                Title = AppResources.max_batt_temp,
                DefaultCellType = CellTypes.LabelLabel,
                EditableCellType = CellTypes.LabelTextEdit,
                TextMaxLength = 5
            };
            MCB_trickleTemperatureTextBox = new ListViewItem()
            {

                Index = 6,
                Title = AppResources.min_charge_temp,
                DefaultCellType = CellTypes.LabelLabel,
                EditableCellType = CellTypes.LabelTextEdit,
                TextMaxLength = 5
            };
            MCB_foldTemperatureTextBox = new ListViewItem()
            {

                Index = 7,
                Title = AppResources.foldback_temp,
                DefaultCellType = CellTypes.LabelLabel,
                EditableCellType = CellTypes.LabelTextEdit,
                TextMaxLength = 5
            };
            MCB_coolDownTemperatureTextLabel = new ListViewItem()
            {
                Index = 8,
                Title = AppResources.cool_down_temp,
                DefaultCellType = CellTypes.LabelLabel,
                EditableCellType = CellTypes.LabelTextEdit,
                TextMaxLength = 5
            };
            MCB_batteryTypeComboBox = new ListViewItem()
            {
                Index = 5,
                Title = AppResources.battery_type,
                DefaultCellType = CellTypes.LabelLabel,
                EditableCellType = CellTypes.ListSelector,
                ListSelectionCommand = ListSelectorCommand
            };
            MCB_nominalTemperatureValueTextBox = new ListViewItem()
            {
                Index = 10,
                Title = AppResources.battery_electrolyte_temp,
                DefaultCellType = CellTypes.LabelLabel,
                EditableCellType = CellTypes.LabelTextEdit,
                TextMaxLength = 4
            };

            try
            {
                MCB_loadBatterySettings();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Logger.AddLog(true, "X26" + ex.ToString());
            }

            MCB_MultiVoltageEnableRadio_CheckedChangedInternal();

            if (chargerBatterySettingsAccessApply() == 0)
            {
                BatterySettingsItemSource.Clear();
                ACUserDialogs.ShowAlertWithOkButtonsAction(AppResources.no_data_found, () => { ShowViewModel<BatterySettingsViewModel>(new { pop = "pop" }); });
                return;
            }

            if (BatterySettingsItemSource.Count > 0)
            {
                BatterySettingsItemSource = new ObservableCollection<ListViewItem>(BatterySettingsItemSource.Where(o => o.IsVisible).OrderBy(o => o.Index));
            }
            RaisePropertyChanged(() => BatterySettingsItemSource);
        }

        private int chargerBatterySettingsAccessApply()
        {
            UIAccessControlUtility accessControlUtility = new UIAccessControlUtility();

            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_BatteryCapacity, MCB_defaultBatteryCapacityTextBox, BatterySettingsItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_BatteryCapacity, MCB_defaultBatteryCapacity24TextBox, BatterySettingsItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_BatteryCapacity, MCB_defaultBatteryCapacity36TextBox, BatterySettingsItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_BatteryCapacity, MCB_defaultBatteryCapacity48TextBox, BatterySettingsItemSource);

            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_BatteryVoltage, MCB_defaultBattreyVoltageComboBox, BatterySettingsItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_temperatureCompensation, MCB_temperatureCompensationTextBox, BatterySettingsItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_TempertureHigh, MCB_HighTemperatureThresholdTextBox, BatterySettingsItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_BatteryType, MCB_batteryTypeComboBox, BatterySettingsItemSource);

            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_TempertureHigh, MCB_trickleTemperatureTextBox, BatterySettingsItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_TempertureHigh, MCB_foldTemperatureTextBox, BatterySettingsItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_TempertureHigh, MCB_coolDownTemperatureTextLabel, BatterySettingsItemSource);

            if (accessControlUtility.GetSavedCount() == 0)
            {
                ShowEdit = false;
            }
            int visibleCount = accessControlUtility.GetVisibleCount();

            if (MCB_nominalTemperatureRangeComboBox.IsVisible)
            {
                accessControlUtility.DoApplyAccessControl(AccessLevelConsts.write, MCB_nominalTemperatureRangeComboBox, BatterySettingsItemSource);
            }
            if (MCB_nominalTemperatureValueTextBox.IsVisible)
            {
                accessControlUtility.DoApplyAccessControl(AccessLevelConsts.write, MCB_nominalTemperatureValueTextBox, BatterySettingsItemSource);
            }

            return visibleCount;
        }

        public IMvxCommand CapacityTextChanged
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

        void CreateListForBattView()
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
            Batt_installationDatePickerTOSAVE = new ListViewItem()
            {
                Index = 3,
                Title = AppResources.installation_date,
                DefaultCellType = CellTypes.LabelLabel,
                EditableCellType = CellTypes.DatePicker,
                IsVisible = true
            };
            Batt_ManufactringDatePicker = new ListViewItem()
            {
                Index = 4,
                Title = AppResources.battery_manufacturing_date_same_as_installation,
                DefaultCellType = CellTypes.LabelLabel,
                EditableCellType = CellTypes.DatePickerWithSwitch,
                SwitchValueChanged = SwitchValueChanged
            };
            Batt_ManufactringDateCheckBox = new ListViewItem();
            Batt_TruckIDTextBoxTOSAVE = new ListViewItem()
            {

                Index = 5,
                Title = AppResources.truck_id,
                DefaultCellType = CellTypes.LabelLabel,
                EditableCellType = CellTypes.LabelTextEdit,
                TextMaxLength = 17
            };
            Batt_StudyName = new ListViewItem()
            {
                Index = 6,
                Title = AppResources.study_name,
                DefaultCellType = CellTypes.LabelLabel,
                EditableCellType = CellTypes.LabelLabel,
                IsEditable = false
            };
            Batt_timeZoneComboBox = new ListViewItem()
            {

                Index = 7,
                Title = AppResources.time_zone,
                DefaultCellType = CellTypes.LabelLabel,
                EditableCellType = CellTypes.ListSelector,
                ListSelectionCommand = ListSelectorCommand,
                ListSelectorType = ACUtility.ListSelectorType.Timezone
            };
            Batt_defaultBatteryCapacityTextBox = new ListViewItem()
            {

                Index = 8,
                Title = AppResources.battery_capacity,
                DefaultCellType = CellTypes.LabelLabel,
                EditableCellType = CellTypes.LabelTextEdit,
                TextInputType = ACUtility.InputType.Number,
                TextValueChanged = CapacityTextChanged,
                TextMaxLength = 4
            };
            Batt_defaultBattreyVoltageComboBox = new ListViewItem()
            {

                Index = 9,
                Title = AppResources.battery_voltage,
                DefaultCellType = CellTypes.LabelLabel,
                EditableCellType = CellTypes.ListSelector,
                ListSelectionCommand = ListSelectorCommand,
                ListSelectorType = ACUtility.ListSelectorType.BatteryVoltageUnits
            };
            Batt_temperatureCompensationTextBox = new ListViewItem()
            {

                Index = 10,
                Title = AppResources.temperature_compesation,
                DefaultCellType = CellTypes.LabelLabel,
                EditableCellType = CellTypes.LabelTextEdit,
                TextInputType = ACUtility.InputType.Decimal,
                TextMaxLength = 3
            };
            Batt_HighTemperatureThresholdTextBox = new ListViewItem()
            {

                Index = 11,
                Title = AppResources.max_battery_temperature,
                DefaultCellType = CellTypes.LabelLabel,
                EditableCellType = CellTypes.LabelTextEdit,
                TextMaxLength = 5
            };
            Batt_foldTemperatureTextBox = new ListViewItem()
            {

                Index = 12,
                Title = AppResources.foldback_temperature,
                DefaultCellType = CellTypes.LabelLabel,
                EditableCellType = CellTypes.LabelTextEdit,
                TextInputType = ACUtility.InputType.Decimal,
                TextMaxLength = 5
            };
            Batt_coolDownTemperatureTextLabel = new ListViewItem()
            {

                Index = 13,
                Title = AppResources.cool_down_temperature,
                DefaultCellType = CellTypes.LabelLabel,
                EditableCellType = CellTypes.LabelTextEdit,
                TextInputType = ACUtility.InputType.Decimal,
                TextMaxLength = 5
            };
            Batt_trickleTemperatureTextBox = new ListViewItem()
            {

                Index = 14,
                Title = AppResources.min_cold_charge_temperature,
                DefaultCellType = CellTypes.LabelLabel,
                EditableCellType = CellTypes.LabelTextEdit,
                TextInputType = ACUtility.InputType.Decimal,
                TextMaxLength = 5
            };
            Batt_batteryTypeComboBox = new ListViewItem()
            {

                Index = 15,
                Title = AppResources.battery_type,
                DefaultCellType = CellTypes.LabelLabel,
                EditableCellType = CellTypes.ListSelector,
                ListSelectionCommand = ListSelectorCommand,
                ListSelectorType = ACUtility.ListSelectorType.BatteryType
            };
            Batt_chargerTypeTOSAVE = new ListViewItem()
            {

                Index = 16,
                Title = AppResources.application_type,
                DefaultCellType = CellTypes.LabelLabel,
                EditableCellType = CellTypes.ListSelector,
                ListSelectionCommand = ListSelectorCommand,
                ListSelectorType = ACUtility.ListSelectorType.ApplicationType
            };
            Batt_WarrantedAHRTextBoxTOSAVE = new ListViewItem()
            {

                Index = 17,
                Title = AppResources.warranted,
                DefaultCellType = CellTypes.LabelLabel,
                EditableCellType = CellTypes.LabelTextEdit,
                TextInputType = ACUtility.InputType.Number,
                TextMaxLength = 7
            };

            try
            {
                Batt_loadBatterySettings();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Logger.AddLog(true, "X26" + ex.ToString());
            }

            if (BattViewBatterySettingsAccessApply() == 0)
            {
                BatterySettingsItemSource.Clear();
                ACUserDialogs.ShowAlertWithOkButtonsAction(AppResources.no_data_found, () => { ShowViewModel<BatterySettingsViewModel>(new { pop = "pop" }); });
                return;
            }

            if (BatterySettingsItemSource.Count > 0)
            {
                BatterySettingsItemSource = new ObservableCollection<ListViewItem>(BatterySettingsItemSource.OrderBy(o => o.Index));
            }
            RaisePropertyChanged(() => BatterySettingsItemSource);

        }

        private int BattViewBatterySettingsAccessApply()
        {
            UIAccessControlUtility accessControlUtility = new UIAccessControlUtility();

            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_InstallationDate, Batt_installationDatePickerTOSAVE, BatterySettingsItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_InstallationDate, Batt_ManufactringDatePicker, BatterySettingsItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_BatteryCapacity, Batt_defaultBatteryCapacityTextBox, BatterySettingsItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_BatteryVoltage, Batt_defaultBattreyVoltageComboBox, BatterySettingsItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_temperatureCompensation, Batt_temperatureCompensationTextBox, BatterySettingsItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_TempertureHigh, Batt_HighTemperatureThresholdTextBox, BatterySettingsItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_BatteryType, Batt_batteryTypeComboBox, BatterySettingsItemSource);


            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_batteryID, Batt_batteryIDTextBoxTOSAVE, BatterySettingsItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_batteryModelandSN, Batt_batteryModelTextBoxTOSAVE, BatterySettingsItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_batteryModelandSN, Batt_TruckIDTextBoxTOSAVE, BatterySettingsItemSource);

            if (CanShowStudyName())
                accessControlUtility
                    .DoApplyAccessControl
                        (AccessLevelConsts.write, Batt_StudyName, BatterySettingsItemSource);

            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_batteryModelandSN, Batt_batterySNTextBoxTOSAVE, BatterySettingsItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_TempertureHigh, Batt_trickleTemperatureTextBox, BatterySettingsItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_TempertureHigh, Batt_foldTemperatureTextBox, BatterySettingsItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_TempertureHigh, Batt_coolDownTemperatureTextLabel, BatterySettingsItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_chargerType, Batt_chargerTypeTOSAVE, BatterySettingsItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_TimeZone, Batt_timeZoneComboBox, BatterySettingsItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_OverrideWarrantedAHR, Batt_WarrantedAHRTextBoxTOSAVE, BatterySettingsItemSource);

            if (accessControlUtility.GetSavedCount() == 0)
            {
                //Batt_BatterySettingsSaveButton.Hide();
                ShowEdit = false;
            }

            return accessControlUtility.GetVisibleCount(); ;
        }

        bool CanShowStudyName()
        {
            var config = BattViewQuantum.Instance.GetBATTView().Config;

            bool canShowStudyName = Convert.ToBoolean(config.isPA);

            return canShowStudyName;
        }

        /// <summary>
        /// The battery settings item source.
        /// </summary>
        private ObservableCollection<ListViewItem> _batterySettingsItemSource;

        public ObservableCollection<ListViewItem> BatterySettingsItemSource
        {
            get { return _batterySettingsItemSource; }
            set
            {
                _batterySettingsItemSource = value;
                RaisePropertyChanged(() => BatterySettingsItemSource);

            }
        }
        private bool _editMode;
        public bool EditingMode
        {
            get
            {
                return _editMode;
            }
            set
            {
                _editMode = value;
                RaisePropertyChanged(() => EditingMode);

            }
        }
        public IMvxCommand EditBtnClickCommand
        {
            get { return new MvxCommand(OnEditClick); }
        }

        void OnEditClick()
        {
            EditingMode = true;
            RefreshList();
            RaisePropertyChanged(() => BatterySettingsItemSource);
        }

        public IMvxCommand SaveBtnClickCommand
        {
            get
            {
                return new MvxCommand(async () =>
                {
                    await OnSaveClick();
                });
            }
        }

        async Task OnSaveClick()
        {
            if (NetworkCheck())
            {
                if (IsBattView)
                {
                    if (Batt_VerfiyBatterySettings())
                    {
                        ACUserDialogs.ShowProgress();
                        BattViewCommunicationTypes caller = BattViewCommunicationTypes.NOCall;
                        bool arg1 = false;
                        try
                        {
                            arg1 = Batt_SaveIntoBatterySettings();
                            caller = BattViewCommunicationTypes.saveConfigAndTime;
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine(ex.Message);
                        }

                        List<object> arguments = new List<object>();
                        arguments.Add(caller);
                        arguments.Add(arg1);
                        List<object> results = new List<object>();
                        try
                        {
                            results = await BattViewQuantum.Instance.CommunicateBATTView(arguments);
                            if (results.Count > 0)
                            {
                                var status = (CommunicationResult)results[2];
                                if (status == CommunicationResult.OK)
                                {
                                    EditingMode = false;
                                    RefreshList();

                                    try
                                    {
                                        ResetOldData();
                                        Batt_loadBatterySettings();
                                        RaisePropertyChanged(() => BatterySettingsItemSource);
                                    }

                                    catch (Exception ex)
                                    {
                                        Debug.WriteLine(ex.Message);
                                        Logger.AddLog(true, "X24" + ex.ToString());
                                    }
                                }
                                else
                                {
                                    //Saving to BATTView failed.
                                    //Be always in the Edit Screen
                                    ACUserDialogs.ShowAlertWithTitleAndOkButton(AppResources.save_failed);
                                }
                            }
                            else
                            {
                                ACUserDialogs.ShowAlertWithTitleAndOkButton(AppResources.save_failed);
                            }

                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine(ex.Message);
                        }
                        ACUserDialogs.HideProgress();

                    }
                    else
                    {
                        //TODO Show Correct Errors Alert
                        ACUserDialogs.ShowAlert(AppResources.alert_enter_valid + " " + verifyControl.GetErrorString());
                    }
                }
                else
                {
                    if (MCB_VerfiyBatterySettings())
                    {
                        ACUserDialogs.ShowProgress();
                        McbCommunicationTypes caller = McbCommunicationTypes.NOCall;
                        bool arg1 = false;
                        try
                        {
                            MCB_SaveIntoBatterySettings();
                            caller = McbCommunicationTypes.saveConfig;
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine(ex.Message);
                        }

                        List<object> arguments = new List<object>();
                        arguments.Add(caller);
                        arguments.Add(arg1);
                        List<object> results = new List<object>();
                        try
                        {
                            results = await MCBQuantum.Instance.CommunicateMCB(arguments);
                            if (results.Count > 0)
                            {
                                var status = (CommunicationResult)results[2];
                                if (status == CommunicationResult.OK)
                                {
                                    EditingMode = false;
                                    RefreshList();

                                    try
                                    {
                                        ResetOldData();
                                        MCB_loadBatterySettings();
                                        RaisePropertyChanged(() => BatterySettingsItemSource);
                                    }

                                    catch (Exception ex)
                                    {
                                        Debug.WriteLine(ex.Message);
                                        Logger.AddLog(true, "X24" + ex.ToString());
                                    }
                                }
                                else
                                {
                                    //Saving to BATTView failed.
                                    //Be always in the Edit Screen
                                    ACUserDialogs.ShowAlertWithTitleAndOkButton(AppResources.save_failed);
                                }
                            }
                            else
                            {
                                ACUserDialogs.ShowAlertWithTitleAndOkButton(AppResources.save_failed);
                            }

                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine(ex.Message);
                        }
                        ACUserDialogs.HideProgress();

                    }
                    else
                    {
                        //TODO Show Correct Errors Alert
                        ACUserDialogs.ShowAlert(AppResources.alert_enter_valid + " " + verifyControl.GetErrorString());
                    }

                }
            }
        }

        void ResetOldData()
        {
            foreach (var item in BatterySettingsItemSource)
            {
                item.SubTitle = string.Empty;
            }
        }

        public IMvxCommand BackBtnClickCommand
        {
            get { return new MvxCommand(OnBackClick); }
        }

        /// <summary>
        /// Gets the cancel button click command.
        /// </summary>
        /// <value>The cancel button click command.</value>
        public IMvxCommand CancelBtnClickCommand
        {
            get { return new MvxCommand(OnBackClick); }
        }

        void OnBackClick()
        {
            if (CheckForEditedChanges())
            {
                ACUserDialogs.ShowAlertWithTwoButtons(AppResources.cancel_confirmation, "", AppResources.yes, AppResources.no, () => OnYesClick(), null);
            }
            else
            {
                OnYesClick();
            }
        }

        private void OnYesClick()
        {
            EditingMode = false;
            //CreateList();
            foreach (var item in BatterySettingsItemSource)
            {
                if (IsBattView)
                {
                    if (item.Title == Batt_ManufactringDatePicker.Title)
                    {
                        if (Batt_ManufactringDatePicker.SubTitle.Equals("Yes"))
                        {
                            Batt_ManufactringDatePicker.Date = Batt_installationDatePickerTOSAVE.Date;
                            Batt_ManufactringDatePicker.Text = string.Empty;
                        }
                        else
                        {
                            Batt_ManufactringDatePicker.Date = DateTime.ParseExact(Batt_ManufactringDatePicker.SubTitle, ACConstants.DATE_TIME_FORMAT_IOS_UI, CultureInfo.InvariantCulture);
                        }
                    }
                }

                if (item.EditableCellType == ACUtility.CellTypes.LabelSwitch)
                {
                    item.IsSwitchEnabled = (item.Text == "Yes");
                }
                item.IsEditable = EditingMode;
            }
            RaisePropertyChanged(() => BatterySettingsItemSource);
        }

        private bool CheckForEditedChanges()
        {
            bool textChanged = false;
            foreach (var item in BatterySettingsItemSource)
            {
                if (item.CellType == CellTypes.DatePickerWithSwitch)
                {
                    if (item.SubTitle == "Yes")
                    {
                        if (!item.IsSwitchEnabled)
                        {
                            textChanged = true;
                        }
                    }
                    else
                    {
                        if (item.IsSwitchEnabled)
                        {
                            textChanged = true;
                        }
                        else
                        {
                            if (item.SubTitle != item.Text)
                            {
                                textChanged = true;
                            }
                        }
                    }
                }
                else
                {
                    if (item.Text != item.SubTitle)
                    {
                        textChanged = true;
                    }
                }
            }

            return textChanged;
        }

        /// <summary>
        /// Gets the list selector command.
        /// </summary>
        /// <value>The list selector command.</value>
        public IMvxCommand ListSelectorCommand
        {
            get { return new MvxCommand<ListViewItem>(ExecuteListSelectorCommand); }
        }


        private void ExecuteListSelectorCommand(ListViewItem item)
        {
            if (item.CellType == ACUtility.CellTypes.ListSelector)
            {
                _mListSelector = Mvx.Resolve<IMvxMessenger>().Subscribe<ListSelectorMessage>(OnListSelectorMessage);
                ShowViewModel<ListSelectorViewModel>(new { title = item.Title, type = item.ListSelectorType, selectedItemIndex = BatterySettingsItemSource.IndexOf(item), ItemSourceStr = JsonConvert.SerializeObject(item.Items) });
            }
        }
        /// <summary>
        /// Ons the list selector message.
        /// </summary>
        /// <param name="obj">Object.</param>
        private void OnListSelectorMessage(ListSelectorMessage obj)
        {
            if (_mListSelector != null)
            {
                Mvx.Resolve<IMvxMessenger>().Unsubscribe<ListSelectorMessage>(_mListSelector);
                _mListSelector = null;
                BatterySettingsItemSource[obj.SelectedItemindex].Text = obj.SelectedItem;
                BatterySettingsItemSource[obj.SelectedItemindex].SelectedIndex = obj.SelectedIndex;
                if (!IsBattView)
                {
                    if (MCB_nominalTemperatureRangeComboBox.SelectedIndex == 0)
                    {
                        MCB_nominalTemperatureValueTextBox.Text = "18.0";
                    }
                    else if (MCB_nominalTemperatureRangeComboBox.SelectedIndex == 1)
                    {
                        MCB_nominalTemperatureValueTextBox.Text = "25.0";

                    }
                    else if (MCB_nominalTemperatureRangeComboBox.SelectedIndex == 2)
                    {
                        MCB_nominalTemperatureValueTextBox.Text = "32.0";
                    }

                }
                RaisePropertyChanged(() => BatterySettingsItemSource);

            }
        }
        public IMvxCommand SwitchValueChanged
        {
            get
            {
                return new MvxCommand<ListViewItem>(ExecuteSwitchValueChanged);
            }
        }

        void ExecuteSwitchValueChanged(ListViewItem item)
        {
            item.IsVisible = !item.IsSwitchEnabled;
            item.Text = item.IsSwitchEnabled ? (EditingMode ? "" : "Yes") : "" + ((EditingMode ? Batt_ManufactringDatePicker.Date = DateTime.Now : Batt_ManufactringDatePicker.Date)).ToString(ACConstants.DATE_TIME_FORMAT_IOS_UI);
            RaisePropertyChanged(() => BatterySettingsItemSource);
        }
        private void RefreshList()
        {
            foreach (var item in BatterySettingsItemSource)
            {
                try
                {
                    item.IsEditable = item.IsEditEnabled && EditingMode;
                    item.Text = item.SubTitle;
                    if (item.CellType == ACUtility.CellTypes.LabelSwitch)
                    {
                        item.IsSwitchEnabled = (item.SubTitle == "Yes") ? true : false;
                    }

                    // enable manufacturing date settings based on installation date
                    if (IsBattView)
                    {

                        if (item.Title.Equals(Batt_ManufactringDatePicker.Title))
                        {
                            if (Batt_ManufactringDatePicker.Date == Batt_installationDatePickerTOSAVE.Date)
                            {
                                Batt_ManufactringDatePicker.IsSwitchEnabled = true;
                            }
                            else
                            {
                                Batt_ManufactringDatePicker.IsSwitchEnabled = false;
                            }

                            //Batt_ManufactringDatePicker.SubTitle = Batt_ManufactringDatePicker.IsSwitchEnabled ? (EditingMode ? "" : "Yes") : "" + (EditingMode ? Batt_ManufactringDatePicker.Date = DateTime.Now : Batt_ManufactringDatePicker.Date);
                            item.Text = Batt_ManufactringDatePicker.IsSwitchEnabled ? (EditingMode ? "" : "Yes") : "" + ((EditingMode ? Batt_ManufactringDatePicker.Date = DateTime.Now : Batt_ManufactringDatePicker.Date)).ToString(ACConstants.DATE_TIME_FORMAT_IOS_UI);
                            if (Batt_ManufactringDatePicker.IsSwitchEnabled && EditingMode)
                            {
                                Batt_ManufactringDatePicker.IsVisible = false;
                            }
                            else
                            {
                                Batt_ManufactringDatePicker.IsVisible = true;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
                //item.Text = item.SubTitle;
            }
        }

        /// <summary>
        /// Loads the Battery Settings
        /// </summary>
        void Batt_loadBatterySettings()
        {
            BattViewObject currentBattView = BattViewQuantum.Instance.GetBATTView();

            Batt_batteryIDTextBoxTOSAVE.SubTitle = Batt_batteryIDTextBoxTOSAVE.Text = currentBattView.Config.batteryID;
            Batt_batteryModelTextBoxTOSAVE.SubTitle = Batt_batteryModelTextBoxTOSAVE.Text = currentBattView.Config.batterymodel;
            Batt_batterySNTextBoxTOSAVE.SubTitle = Batt_batterySNTextBoxTOSAVE.Text = currentBattView.Config.batterysn;


            Batt_installationDatePickerTOSAVE.MaxDate = DateTime.MaxValue.AddYears(-2);
            Batt_installationDatePickerTOSAVE.MinDate = new DateTime(2015, 9, 1, 0, 0, 0, DateTimeKind.Utc);
            Batt_installationDatePickerTOSAVE.MaxDate = DateTime.UtcNow.Add(new TimeSpan(180, 0, 0, 0, 0));


            Batt_ManufactringDatePicker.MaxDate = DateTime.MaxValue.AddYears(-2);
            Batt_ManufactringDatePicker.MinDate = DateTime.Now.AddYears(-10);
            Batt_ManufactringDatePicker.MaxDate = DateTime.UtcNow.Add(new TimeSpan(180, 0, 0, 0, 0));

            //Batt_ManufactringDateCheckBox_CheckedChangedInternal
            DateTime toSelect = DateTime.Now;
            //RemoveAllControlEvents(Batt_ManufactringDateCheckBox, "EVENT_CHECKEDCHANGED");

            if (currentBattView.Config.batteryManfacturingDate.Year <= 1970)
            {
                toSelect = currentBattView.Config.installationDate;
                Batt_ManufactringDateCheckBox.IsSwitchEnabled = true;
                //Batt_ManufactringDatePicker.IsEditable = false;
            }
            else
            {
                toSelect = currentBattView.Config.batteryManfacturingDate;
                Batt_ManufactringDateCheckBox.IsSwitchEnabled = false;
                //Batt_ManufactringDatePicker.IsEditable = true;
            }

            if (toSelect < Batt_ManufactringDatePicker.MinDate ||
                toSelect > Batt_ManufactringDatePicker.MaxDate)
            {
                device_LoadWarnings.Add("Battery Manufacturing Date Value is out of Range");
                toSelect = DateTime.Now;
            }
            Batt_ManufactringDatePicker.Date = toSelect;
            Batt_ManufactringDatePicker.SubTitle = Batt_ManufactringDatePicker.Date.ToString(ACConstants.DATE_TIME_FORMAT_IOS_UI);

            Batt_TruckIDTextBoxTOSAVE.SubTitle = Batt_TruckIDTextBoxTOSAVE.Text = currentBattView.Config.TruckId;

            Batt_StudyName.SubTitle = currentBattView.Config.studyName;

            if (currentBattView.Config.installationDate < Batt_installationDatePickerTOSAVE.MinDate ||
                currentBattView.Config.installationDate > Batt_installationDatePickerTOSAVE.MaxDate)
            {
                Batt_installationDatePickerTOSAVE.Date = DateTime.UtcNow;
                Batt_installationDatePickerTOSAVE.SubTitle = DateTime.UtcNow.Date.ToString(ACConstants.DATE_TIME_FORMAT_IOS_UI);
                device_LoadWarnings.Add("Installation Date Value is out of Range");
            }
            else
            {
                Batt_installationDatePickerTOSAVE.Date = currentBattView.Config.installationDate;
                Batt_installationDatePickerTOSAVE.SubTitle = Batt_installationDatePickerTOSAVE.Date.ToString(ACConstants.DATE_TIME_FORMAT_IOS_UI);
            }
            //compare dates and changes manufacturing date selection settigns
            if (Batt_ManufactringDatePicker.Date == Batt_installationDatePickerTOSAVE.Date)
            {
                Batt_ManufactringDatePicker.IsSwitchEnabled = true;
            }
            Batt_ManufactringDatePicker.SubTitle = Batt_ManufactringDatePicker.IsSwitchEnabled ? (EditingMode ? "" : "Yes") : "" + ((EditingMode ? Batt_ManufactringDatePicker.Date = DateTime.Now : Batt_ManufactringDatePicker.Date)).ToString(ACConstants.DATE_TIME_FORMAT_IOS_UI);


            if (currentBattView.myZone == 0)
            {
                Batt_timeZoneComboBox.SelectedIndex = -1;
                device_LoadWarnings.Add("BATTView Time is not correct");
            }
            else
            {
                Batt_timeZoneComboBox.SelectedIndex = -1;
                Batt_timeZoneComboBox.Items = JsonConvert.DeserializeObject<List<object>>(JsonConvert.SerializeObject(StaticDataAndHelperFunctions.GetZonesList()));
                Batt_timeZoneComboBox.SelectedItem = StaticDataAndHelperFunctions.getZoneByID(currentBattView.myZone).ToString();
                Batt_timeZoneComboBox.SelectedIndex = StaticDataAndHelperFunctions.GetZonesList().FindIndex(o => o.display_name == Batt_timeZoneComboBox.SelectedItem);
            }

            Batt_defaultBatteryCapacityTextBox.SubTitle = Batt_defaultBatteryCapacityTextBox.Text = currentBattView.Config.ahrcapacity.ToString();


            Batt_defaultBattreyVoltageComboBox.Items = new List<object>();
            if (!ControlObject.isDebugMaster && !ControlObject.isACTOem && ControlObject.UserAccess.Batt_onlyForEnginneringTeam != AccessLevelConsts.write)
            {
                Batt_defaultBattreyVoltageComboBox.Items.Clear();
                Batt_defaultBattreyVoltageComboBox.Items.Add("24");
                Batt_defaultBattreyVoltageComboBox.Items.Add("36");
                Batt_defaultBattreyVoltageComboBox.Items.Add("48");
                Batt_defaultBattreyVoltageComboBox.Items.Add("72");
                Batt_defaultBattreyVoltageComboBox.Items.Add("80");
                if (currentBattView.Config.nominalvoltage != 24 && currentBattView.Config.nominalvoltage != 36 && currentBattView.Config.nominalvoltage != 48 && currentBattView.Config.nominalvoltage != 72 && currentBattView.Config.nominalvoltage != 80)
                {
                    if (currentBattView.Config.nominalvoltage % 2 == 0)
                    {

                        Batt_defaultBattreyVoltageComboBox.Items.Add(currentBattView.Config.nominalvoltage.ToString());
                    }
                }
            }
            else
            {
                Batt_defaultBattreyVoltageComboBox.Items.AddRange(new object[] {
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
            "80"});
            }

            Batt_defaultBattreyVoltageComboBox.SelectedItem = currentBattView.Config.nominalvoltage.ToString();
            Batt_defaultBattreyVoltageComboBox.SelectedIndex = Batt_defaultBattreyVoltageComboBox.Items.FindIndex(o => ((string)o).Equals(Batt_defaultBattreyVoltageComboBox.SelectedItem));
            //TODO Uncomment this line after implementing the Voltage ListView
            //this.Batt_defaultBattreyVoltage.SelectedIndexChanged += new System.EventHandler(this.Batt_defaultBattreyVoltageComboBox_SelectedIndexChanged);

            Batt_temperatureCompensationTextBox.SubTitle = Batt_temperatureCompensationTextBox.Text = (currentBattView.Config.batteryTemperatureCompesnation / 10.0f).ToString("N1").TrimEnd('0').TrimEnd(System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator[0]);

            Batt_HighTemperatureThresholdTextBox.SubTitle = Batt_HighTemperatureThresholdTextBox.Text = ((currentBattView.Config.batteryHighTemperature / 10.0f) * 1.8 + 32).ToString("N0");
            Batt_foldTemperatureTextBox.SubTitle = Batt_foldTemperatureTextBox.Text = ((currentBattView.Config.foldTemperature / 10.0f) * 1.8 + 32).ToString("N0");
            Batt_coolDownTemperatureTextLabel.SubTitle = Batt_coolDownTemperatureTextLabel.Text = ((currentBattView.Config.coolDownTemperature / 10.0f) * 1.8 + 32).ToString("N0");
            Batt_trickleTemperatureTextBox.SubTitle = Batt_trickleTemperatureTextBox.Text = ((currentBattView.Config.TRTemperature / 10.0f) * 1.8 + 32).ToString("N0");

            Batt_batteryTypeComboBox.Items = new List<object> { "Lead Acid", "AGM", "GEL" };
            Batt_chargerTypeTOSAVE.Items = new List<object> { "Fast", "Conventional", "Opportunity" };
            if (Batt_batteryTypeComboBox.Items.Count <= currentBattView.Config.batteryType)
            {
                device_LoadWarnings.Add("Battery Type is invalid");
            }
            else
            {
                Batt_batteryTypeComboBox.SelectedIndex = currentBattView.Config.batteryType;
                Batt_batteryTypeComboBox.SelectedItem = Batt_batteryTypeComboBox.Items[Batt_batteryTypeComboBox.SelectedIndex].ToString();
            }
            if (Batt_chargerTypeTOSAVE.Items.Count <= currentBattView.Config.chargerType)
            {
                device_LoadWarnings.Add("Battery Application is invalid");
            }
            else
            {
                Batt_chargerTypeTOSAVE.SelectedIndex = currentBattView.Config.chargerType;
                Batt_chargerTypeTOSAVE.SelectedItem = Batt_chargerTypeTOSAVE.Items[Batt_chargerTypeTOSAVE.SelectedIndex].ToString();

            }
            //Batt_chargerTypeTOSAVE.Items.Count
            Batt_WarrantedAHRTextBoxTOSAVE.SubTitle = Batt_WarrantedAHRTextBoxTOSAVE.Text = currentBattView.Config.warrantedAHR.ToString();
            Batt_VerfiyBatterySettings();
        }

        bool Batt_VerfiyBatterySettings()
        {
            if (!BattViewQuantum.Instance.BattView_quickSerialNumberCheckStrict())
                return false;
            verifyControl = new VerifyControl();
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
                {
                    verifyControl.InsertRemoveFault(true, Batt_HighTemperatureThresholdTextBox);
                }
                if (float.Parse(Batt_foldTemperatureTextBox.Text) <= float.Parse(Batt_coolDownTemperatureTextLabel.Text))
                {
                    verifyControl.InsertRemoveFault(true, Batt_foldTemperatureTextBox);
                }
            }

            return !verifyControl.HasErrors();
        }

        bool Batt_SaveIntoBatterySettings()
        {

            bool isDirty = false;

            if (!BattViewQuantum.Instance.BattView_quickSerialNumberCheckStrict())
                return false;
            bool requireRefresh = BattViewQuantum.Instance.GetBATTView().Config.batteryID != Batt_batteryIDTextBoxTOSAVE.Text;

            if (BattViewQuantum.Instance.GetBATTView().Config.batteryID != Batt_batteryIDTextBoxTOSAVE.Text)
            {
                isDirty = true;
                BattViewQuantum.Instance.GetBATTView().Config.batteryID = Batt_batteryIDTextBoxTOSAVE.Text;
            }


            if (BattViewQuantum.Instance.GetBATTView().Config.batterymodel != Batt_batteryModelTextBoxTOSAVE.Text)
            {
                isDirty = true;
                BattViewQuantum.Instance.GetBATTView().Config.batterymodel = Batt_batteryModelTextBoxTOSAVE.Text;
            }


            if (BattViewQuantum.Instance.GetBATTView().Config.batterysn != Batt_batterySNTextBoxTOSAVE.Text)
            {
                isDirty = true;
                BattViewQuantum.Instance.GetBATTView().Config.batterysn = Batt_batterySNTextBoxTOSAVE.Text;
            }

            if (BattViewQuantum.Instance.GetBATTView().Config.installationDate.Date != Batt_installationDatePickerTOSAVE.Date.Date)
            {
                isDirty = true;
                BattViewQuantum.Instance.GetBATTView().Config.installationDate = Batt_installationDatePickerTOSAVE.Date.Date;
            }
            JsonZone info = StaticDataAndHelperFunctions.getZoneByText(Batt_timeZoneComboBox.Text);
            if (BattViewQuantum.Instance.GetBATTView().myZone != info.id)
            {
                isDirty = true;
                BattViewQuantum.Instance.GetBATTView().myZone = info.id;
            }



            if (BattViewQuantum.Instance.GetBATTView().Config.warrantedAHR != UInt32.Parse(Batt_WarrantedAHRTextBoxTOSAVE.Text))
            {
                isDirty = true;
                BattViewQuantum.Instance.GetBATTView().Config.warrantedAHR = UInt32.Parse(Batt_WarrantedAHRTextBoxTOSAVE.Text);
            }

            if (BattViewQuantum.Instance.GetBATTView().Config.ahrcapacity != ushort.Parse(Batt_defaultBatteryCapacityTextBox.Text))
            {
                isDirty = true;
                BattViewQuantum.Instance.GetBATTView().Config.ahrcapacity = ushort.Parse(Batt_defaultBatteryCapacityTextBox.Text);
            }

            if (BattViewQuantum.Instance.GetBATTView().Config.nominalvoltage != byte.Parse((string)Batt_defaultBattreyVoltageComboBox.Text))
            {
                isDirty = true;
                BattViewQuantum.Instance.GetBATTView().Config.nominalvoltage = byte.Parse((string)Batt_defaultBattreyVoltageComboBox.Text);
            }

            if (BattViewQuantum.Instance.GetBATTView().Config.batteryTemperatureCompesnation != (byte)Math.Round(float.Parse(Batt_temperatureCompensationTextBox.Text) * 10.0f))
            {
                isDirty = true;
                BattViewQuantum.Instance.GetBATTView().Config.batteryTemperatureCompesnation = (byte)Math.Round(float.Parse(Batt_temperatureCompensationTextBox.Text) * 10.0f);
            }

            if (BattViewQuantum.Instance.GetBATTView().Config.batteryHighTemperature != (ushort)Math.Round(((float.Parse(Batt_HighTemperatureThresholdTextBox.Text) - 32) / 1.8) * 10.0f))
            {
                isDirty = true;
                BattViewQuantum.Instance.GetBATTView().Config.batteryHighTemperature = (ushort)Math.Round(((float.Parse(Batt_HighTemperatureThresholdTextBox.Text) - 32) / 1.8) * 10.0f);
            }

            if (BattViewQuantum.Instance.GetBATTView().Config.foldTemperature != (short)Math.Round(((float.Parse(Batt_foldTemperatureTextBox.Text) - 32) / 1.8) * 10.0f))
            {
                isDirty = true;
                BattViewQuantum.Instance.GetBATTView().Config.foldTemperature = (short)Math.Round(((float.Parse(Batt_foldTemperatureTextBox.Text) - 32) / 1.8) * 10.0f);
            }

            if (BattViewQuantum.Instance.GetBATTView().Config.coolDownTemperature != (short)Math.Round(((float.Parse(Batt_coolDownTemperatureTextLabel.Text) - 32) / 1.8) * 10.0f))
            {
                isDirty = true;
                BattViewQuantum.Instance.GetBATTView().Config.coolDownTemperature = (short)Math.Round(((float.Parse(Batt_coolDownTemperatureTextLabel.Text) - 32) / 1.8) * 10.0f);
            }

            if (BattViewQuantum.Instance.GetBATTView().Config.TRTemperature != (short)Math.Round(((float.Parse(Batt_trickleTemperatureTextBox.Text) - 32) / 1.8) * 10.0f))
            {
                isDirty = true;
                BattViewQuantum.Instance.GetBATTView().Config.TRTemperature = (short)Math.Round(((float.Parse(Batt_trickleTemperatureTextBox.Text) - 32) / 1.8) * 10.0f);
            }
            if (BattViewQuantum.Instance.GetBATTView().Config.batteryType != (byte)Batt_batteryTypeComboBox.SelectedIndex)
            {
                isDirty = true;
                BattViewQuantum.Instance.GetBATTView().Config.batteryType = (byte)Batt_batteryTypeComboBox.SelectedIndex;
            }



            if (Batt_ManufactringDatePicker.IsSwitchEnabled)
            {
                if (BattViewQuantum.Instance.GetBATTView().Config.batteryManfacturingDate != new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).Date)
                {
                    isDirty = true;
                    BattViewQuantum.Instance.GetBATTView().Config.batteryManfacturingDate = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).Date;
                }
            }
            else if (BattViewQuantum.Instance.GetBATTView().Config.batteryManfacturingDate != new DateTime(Batt_ManufactringDatePicker.Date.Year, Batt_ManufactringDatePicker.Date.Month, Batt_ManufactringDatePicker.Date.Day, 0, 0, 0, DateTimeKind.Utc).Date)
            {
                isDirty = true;
                BattViewQuantum.Instance.GetBATTView().Config.batteryManfacturingDate = new DateTime(Batt_ManufactringDatePicker.Date.Year, Batt_ManufactringDatePicker.Date.Month, Batt_ManufactringDatePicker.Date.Day, 0, 0, 0, DateTimeKind.Utc).Date;
            }



            if (BattViewQuantum.Instance.GetBATTView().Config.chargerType != (byte)Batt_chargerTypeTOSAVE.SelectedIndex)
            {
                isDirty = true;
                BattViewQuantum.Instance.GetBATTView().Config.chargerType = (byte)Batt_chargerTypeTOSAVE.SelectedIndex;
                if (!Batt_compareWithDefaultChargeProfile())
                {
                    BattViewQuantum.Instance.Batt_saveDefaultChargeProfile();
                }

            }
            if (BattViewQuantum.Instance.GetBATTView().Config.TruckId != Batt_TruckIDTextBoxTOSAVE.Text)
            {
                if (BattViewQuantum.Instance.GetBATTView().Config.firmwareversion < 2.09f && !isDirty)
                {
                    //before 2.09 truckid was not introduced , so we have to same something to let the battview take it
                    if (BattViewQuantum.Instance.GetBATTView().Config.warrantedAHR % 2 == 0)
                        BattViewQuantum.Instance.GetBATTView().Config.warrantedAHR++;
                    else
                        BattViewQuantum.Instance.GetBATTView().Config.warrantedAHR--;
                }
                isDirty = true;
                BattViewQuantum.Instance.GetBATTView().Config.TruckId = Batt_TruckIDTextBoxTOSAVE.Text;
            }


            return requireRefresh;

        }


        bool Batt_compareWithDefaultChargeProfile()
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
            if (BattViewQuantum.Instance.GetBATTView().Config.chargerType == 0)
            {
                //FAST
                if (BattViewQuantum.Instance.GetBATTView().Config.CCrate != 4000 ||
                BattViewQuantum.Instance.GetBATTView().Config.CVTargetVoltage != 242 ||
                    BattViewQuantum.Instance.GetBATTView().Config.FIcloseWindow != 86400 ||
                    BattViewQuantum.Instance.GetBATTView().Config.FIdaysMask != (0x01 | 0x40) ||
                    BattViewQuantum.Instance.GetBATTView().Config.EQdaysMask != (0x01))
                    return false;
            }
            else if (BattViewQuantum.Instance.GetBATTView().Config.chargerType == 1)
            {
                if (BattViewQuantum.Instance.GetBATTView().Config.CCrate != 1700 ||
                BattViewQuantum.Instance.GetBATTView().Config.CVTargetVoltage != 237 ||
                    BattViewQuantum.Instance.GetBATTView().Config.FIcloseWindow != 86400 ||
                    BattViewQuantum.Instance.GetBATTView().Config.FIdaysMask != 0x7f ||
                    BattViewQuantum.Instance.GetBATTView().Config.EQdaysMask != (0x01))
                    return false;
                //Conventional
            }
            else if (BattViewQuantum.Instance.GetBATTView().Config.chargerType == 2)
            {
                //Opp
                if (BattViewQuantum.Instance.GetBATTView().Config.CCrate != 2500 ||
                BattViewQuantum.Instance.GetBATTView().Config.CVTargetVoltage != 240 ||
                    BattViewQuantum.Instance.GetBATTView().Config.FIcloseWindow != 8 * 3600 ||
                    BattViewQuantum.Instance.GetBATTView().Config.FIdaysMask != 0x7f ||
                    BattViewQuantum.Instance.GetBATTView().Config.EQdaysMask != (0x01))
                    return false;
            }

            return true;
        }

        void MCB_MultiVoltageEnableRadio_CheckedChangedInternal()
        {
            if (!MCBQuantum.Instance.MCB_quickSerialNumberCheckStrict())
            {
                return;
            }
            if (MCBQuantum.Instance.GetMCB().FirmwareRevision < 2.04f)
                return;
            if (ControlObject.UserAccess.MCB_BatteryCapacity != AccessLevelConsts.noAccess)
            {

                if (MCBQuantum.Instance.GetMCB().Config.enableAutoDetectMultiVoltage)
                {

                    MCB_defaultBatteryCapacity24TextBox.IsVisible = true;
                    MCB_defaultBatteryCapacity36TextBox.IsVisible = true;

                    if (MCBQuantum.Instance.GetMCB().Config.PMvoltage == "48")
                    {
                        MCB_defaultBatteryCapacity48TextBox.IsVisible = true;
                    }
                    else
                    {
                        MCB_defaultBatteryCapacity48TextBox.IsVisible = false;
                    }

                    MCB_defaultBatteryCapacityTextBox.IsVisible = false;

                }
                else
                {
                    MCB_defaultBatteryCapacity24TextBox.IsVisible = false;
                    MCB_defaultBatteryCapacity36TextBox.IsVisible = false;
                    MCB_defaultBatteryCapacity48TextBox.IsVisible = false;

                    MCB_defaultBatteryCapacityTextBox.IsVisible = true;
                }
            }

            if (ControlObject.UserAccess.MCB_BatteryVoltage != AccessLevelConsts.noAccess)
            {
                if (MCBQuantum.Instance.GetMCB().Config.enableAutoDetectMultiVoltage)
                {
                    MCB_defaultBattreyVoltageComboBox.IsVisible = false;
                }
                else
                {
                    MCB_defaultBattreyVoltageComboBox.IsVisible = true;
                }
            }
        }

        void MCB_loadBatterySettings()
        {
            if (!MCBQuantum.Instance.MCB_quickSerialNumberCheckStrict())
                return;


            if (MCBQuantum.Instance.GetMCB().FirmwareRevision < 2.04f)
            {

                MCB_defaultBatteryCapacity24TextBox.IsVisible = false;
                MCB_defaultBatteryCapacity36TextBox.IsVisible = false;
                MCB_defaultBatteryCapacity48TextBox.IsVisible = false;
                if (ControlObject.UserAccess.MCB_BatteryCapacity != AccessLevelConsts.noAccess)
                {
                    MCB_defaultBatteryCapacityTextBox.IsVisible = true;
                }
                if (ControlObject.UserAccess.MCB_BatteryVoltage != AccessLevelConsts.noAccess)
                {
                    MCB_defaultBattreyVoltageComboBox.IsVisible = true;
                }
            }

            MCB_defaultBatteryCapacity24TextBox.SubTitle = MCB_defaultBatteryCapacity24TextBox.Text = MCBQuantum.Instance.GetMCB().Config.batteryCapacity24.ToString();

            MCB_defaultBatteryCapacity36TextBox.SubTitle = MCB_defaultBatteryCapacity36TextBox.Text = MCBQuantum.Instance.GetMCB().Config.batteryCapacity36.ToString();


            MCB_defaultBatteryCapacity48TextBox.SubTitle = MCB_defaultBatteryCapacity48TextBox.Text = MCBQuantum.Instance.GetMCB().Config.batteryCapacity48.ToString();


            MCB_defaultBatteryCapacityTextBox.SubTitle = MCB_defaultBatteryCapacityTextBox.Text = MCBQuantum.Instance.GetMCB().Config.batteryCapacity;

            MCB_defaultBattreyVoltageComboBox.Items = new List<object>();

            this.MCB_nominalTemperatureRangeComboBox.Items = new List<object> {
            "<65",
            "65-90",
            ">90"};

            double c = MCBQuantum.Instance.GetMCB().Config.nominal_temperature_shift / 2.0;
            if (c > 0.1)
            {
                MCB_nominalTemperatureRangeComboBox.SelectedIndex = 2;
            }
            else if (c < -0.1)
            {
                MCB_nominalTemperatureRangeComboBox.SelectedIndex = 0;
            }
            else
            {
                MCB_nominalTemperatureRangeComboBox.SelectedIndex = 1;
            }
            //MCB_nominalTemperatureValueTextBox.Text = (25.0 + c).ToString("N1");
            if (MCB_nominalTemperatureRangeComboBox.SelectedIndex == 0)
            {
                MCB_nominalTemperatureValueTextBox.SubTitle = MCB_nominalTemperatureValueTextBox.Text = "18.0";
            }
            else if (MCB_nominalTemperatureRangeComboBox.SelectedIndex == 1)
            {
                MCB_nominalTemperatureValueTextBox.SubTitle = MCB_nominalTemperatureValueTextBox.Text = "25.0";

            }
            else if (MCB_nominalTemperatureRangeComboBox.SelectedIndex == 2)
            {
                MCB_nominalTemperatureValueTextBox.SubTitle = MCB_nominalTemperatureValueTextBox.Text = "32.0";
            }
            if (MCBQuantum.Instance.GetMCB().FirmwareRevision < 2.14f || ControlObject.UserAccess.MCB_onlyForEnginneringTeam == AccessLevelConsts.noAccess)
            {
                MCB_nominalTemperatureValueTextBox.IsVisible = false;

            }
            else
            {
                MCB_nominalTemperatureValueTextBox.IsVisible = true;
                //if (!BatterySettingsItemSource.Contains(MCB_nominalTemperatureValueTextBox)){
                //    MCB_nominalTemperatureValueTextBox.IsEditable = EditingMode;
                //    BatterySettingsItemSource.Add(MCB_nominalTemperatureValueTextBox);
                //}
            }

            if (MCBQuantum.Instance.GetMCB().FirmwareRevision < 2.14f)
            {
                MCB_nominalTemperatureRangeComboBox.IsVisible = false;
            }
            else
            {
                MCB_nominalTemperatureRangeComboBox.IsVisible = true;
                if (MCB_nominalTemperatureRangeComboBox.SelectedIndex != -1)
                {
                    MCB_nominalTemperatureRangeComboBox.SelectedItem = MCB_nominalTemperatureRangeComboBox.Items[MCB_nominalTemperatureRangeComboBox.SelectedIndex].ToString();

                }
                //if (!BatterySettingsItemSource.Contains(MCB_nominalTemperatureRangeComboBox))
                //{
                //    MCB_nominalTemperatureRangeComboBox.IsEditable = EditingMode;
                //    BatterySettingsItemSource.Add(MCB_nominalTemperatureRangeComboBox);
                //}
            }
            if ((!ControlObject.isDebugMaster && !ControlObject.isACTOem && ControlObject.UserAccess.Batt_onlyForEnginneringTeam != AccessLevelConsts.write) || MCBQuantum.Instance.GetMCB().FirmwareRevision < 2.11f)
            {
                MCB_defaultBattreyVoltageComboBox.Items.Add("24");
                MCB_defaultBattreyVoltageComboBox.Items.Add("36");
                if (MCBQuantum.Instance.GetMCB().Config.PMvoltage == "48")
                    MCB_defaultBattreyVoltageComboBox.Items.Add("48");

                int volt = int.Parse(MCBQuantum.Instance.GetMCB().Config.batteryVoltage);


                if (volt != 24 && volt != 36 && volt != 48 && MCBQuantum.Instance.GetMCB().FirmwareRevision > 2.1f)
                {
                    if (volt % 2 == 0)
                    {
                        MCB_defaultBattreyVoltageComboBox.Items.Add(MCBQuantum.Instance.GetMCB().Config.batteryVoltage);
                    }
                }
            }
            else
            {
                for (int i = 24; i <= int.Parse(MCBQuantum.Instance.GetMCB().Config.PMvoltage); i += 2)
                    MCB_defaultBattreyVoltageComboBox.Items.Add(i.ToString());
            }

            MCB_defaultBattreyVoltageComboBox.SubTitle = MCB_defaultBattreyVoltageComboBox.SelectedItem = MCBQuantum.Instance.GetMCB().Config.batteryVoltage;
            if (MCB_defaultBattreyVoltageComboBox.SelectedIndex == -1)
            {
                device_LoadWarnings.Add("Battery Voltage is invalid");
            }

            MCB_temperatureCompensationTextBox.SubTitle = MCB_temperatureCompensationTextBox.Text = MCBQuantum.Instance.GetMCB().Config.temperatureVoltageCompensation;
            if (MCBQuantum.Instance.GetMCB().Config.temperatureFormat)
            {
                MCB_HighTemperatureThresholdTextBox.SubTitle = MCB_HighTemperatureThresholdTextBox.Text = MCBQuantum.Instance.GetMCB().Config.maxTemperatureFault;
            }
            else
            {
                MCB_HighTemperatureThresholdTextBox.SubTitle = MCB_HighTemperatureThresholdTextBox.Text = (float.Parse(MCBQuantum.Instance.GetMCB().Config.maxTemperatureFault) * 1.8 + 32).ToString("N0");

            }
            if (MCBQuantum.Instance.GetMCB().Config.temperatureFormat)
            {
                MCB_trickleTemperatureTextBox.SubTitle = MCB_trickleTemperatureTextBox.Text = MCBQuantum.Instance.GetMCB().Config.TRtemperature;
            }
            else
            {
                MCB_trickleTemperatureTextBox.SubTitle = MCB_trickleTemperatureTextBox.Text = (float.Parse(MCBQuantum.Instance.GetMCB().Config.TRtemperature) * 1.8 + 32).ToString("N0");

            }

            if (MCBQuantum.Instance.GetMCB().Config.temperatureFormat)
            {
                MCB_foldTemperatureTextBox.SubTitle = MCB_foldTemperatureTextBox.Text = MCBQuantum.Instance.GetMCB().Config.foldTemperature;
            }
            else
            {
                MCB_foldTemperatureTextBox.SubTitle = MCB_foldTemperatureTextBox.Text = (float.Parse(MCBQuantum.Instance.GetMCB().Config.foldTemperature) * 1.8 + 32).ToString("N0");

            }
            if (MCBQuantum.Instance.GetMCB().Config.temperatureFormat)
            {
                MCB_coolDownTemperatureTextLabel.SubTitle = MCB_coolDownTemperatureTextLabel.Text = MCBQuantum.Instance.GetMCB().Config.coolDownTemperature;
            }
            else
            {
                MCB_coolDownTemperatureTextLabel.SubTitle = MCB_coolDownTemperatureTextLabel.Text = (float.Parse(MCBQuantum.Instance.GetMCB().Config.coolDownTemperature) * 1.8 + 32).ToString("N0");

            }

            if (MCBQuantum.Instance.GetMCB().FirmwareRevision < 2.14f || ControlObject.UserAccess.MCB_onlyForEnginneringTeam == AccessLevelConsts.noAccess)
            {
                MCB_nominalTemperatureValueTextBox.IsVisible = false;
            }
            else
            {
                MCB_nominalTemperatureValueTextBox.IsVisible = true;
            }

            MCB_batteryTypeComboBox.Items = new List<object> { "Lead Acid", "Lithium-ion", "GEL" };

            MCB_batteryTypeComboBox.SubTitle = MCB_batteryTypeComboBox.SelectedItem = MCBQuantum.Instance.GetMCB().Config.batteryType;
            if (MCB_batteryTypeComboBox.SelectedIndex == -1)
            {
                device_LoadWarnings.Add("Battery Type is invalid");
            }
            MCB_VerfiyBatterySettings();
        }

        bool MCB_VerfiyBatterySettings()
        {
            //if (!MCB_quickSerialNumberCheckStrict())
            //    return false;

            verifyControl = new VerifyControl();
            verifyControl.VerifyInteger(MCB_defaultBatteryCapacityTextBox, MCB_defaultBatteryCapacityTextBox, 40, 5000);
            verifyControl.VerifyInteger(MCB_defaultBatteryCapacity24TextBox, MCB_defaultBatteryCapacity24TextBox, 40, 5000);
            verifyControl.VerifyInteger(MCB_defaultBatteryCapacity36TextBox, MCB_defaultBatteryCapacity36TextBox, 40, 5000);
            verifyControl.VerifyInteger(MCB_defaultBatteryCapacity48TextBox, MCB_defaultBatteryCapacity48TextBox, 40, 5000);

            verifyControl.VerifyComboBox(MCB_defaultBattreyVoltageComboBox);
            verifyControl.VerifyFloatNumber(MCB_temperatureCompensationTextBox, MCB_temperatureCompensationTextBox, 3.0f, 8.0f);
            if (MCBQuantum.Instance.GetMCB().Config.temperatureFormat)
            {
                verifyControl.VerifyFloatNumber(MCB_HighTemperatureThresholdTextBox, MCB_HighTemperatureThresholdTextBox, 25, 125);
            }
            else
            {
                verifyControl.VerifyFloatNumber(MCB_HighTemperatureThresholdTextBox, MCB_HighTemperatureThresholdTextBox, 77, 255);
            }



            if (MCBQuantum.Instance.GetMCB().Config.temperatureFormat)
            {
                verifyControl.VerifyFloatNumber(MCB_trickleTemperatureTextBox, MCB_trickleTemperatureTextBox, -5, 25);
            }
            else
            {
                verifyControl.VerifyFloatNumber(MCB_trickleTemperatureTextBox, MCB_trickleTemperatureTextBox, 23, 77);
            }

            if (MCBQuantum.Instance.GetMCB().Config.temperatureFormat)
            {
                verifyControl.VerifyFloatNumber(MCB_foldTemperatureTextBox, MCB_foldTemperatureTextBox, 25, 125);
            }
            else
            {
                verifyControl.VerifyFloatNumber(MCB_foldTemperatureTextBox, MCB_foldTemperatureTextBox, 77, 255);
            }

            if (MCBQuantum.Instance.GetMCB().Config.temperatureFormat)
            {
                verifyControl.VerifyFloatNumber(MCB_coolDownTemperatureTextLabel, MCB_coolDownTemperatureTextLabel, 25, 125);
            }
            else
            {
                verifyControl.VerifyFloatNumber(MCB_coolDownTemperatureTextLabel, MCB_coolDownTemperatureTextLabel, 77, 255);
            }


            verifyControl.VerifyComboBox(MCB_batteryTypeComboBox);


            //verifyControl.verifyComboBox(MCB_finishTimerComboBox);
            //verifyControl.verifyComboBox(MCB_equalizeTimerComboBox);


            if (!verifyControl.HasErrors())
            {

                if (float.Parse(MCB_HighTemperatureThresholdTextBox.Text) <= float.Parse(MCB_foldTemperatureTextBox.Text))
                {
                    verifyControl.InsertRemoveFault(true, MCB_foldTemperatureTextBox);
                }
                if (float.Parse(MCB_foldTemperatureTextBox.Text) <= float.Parse(MCB_coolDownTemperatureTextLabel.Text))
                {
                    verifyControl.InsertRemoveFault(true, MCB_foldTemperatureTextBox);
                }
            }
            verifyControl.VerifyFloatNumber(MCB_nominalTemperatureValueTextBox, MCB_nominalTemperatureValueTextBox, -25.0f, 50.0f);

            return !verifyControl.HasErrors();
        }

        void MCB_SaveIntoBatterySettings()
        {
            if (!MCBQuantum.Instance.MCB_quickSerialNumberCheckStrict())
                return;
            if (MCBQuantum.Instance.GetMCB().Config.enableAutoDetectMultiVoltage && MCBQuantum.Instance.GetMCB().FirmwareRevision > 2.03f)
            {

                MCBQuantum.Instance.GetMCB().Config.batteryCapacity24 = UInt16.Parse(MCB_defaultBatteryCapacity24TextBox.Text);
                MCBQuantum.Instance.GetMCB().Config.batteryCapacity36 = UInt16.Parse(MCB_defaultBatteryCapacity36TextBox.Text);
                MCBQuantum.Instance.GetMCB().Config.batteryCapacity48 = UInt16.Parse(MCB_defaultBatteryCapacity48TextBox.Text);


            }
            else
            {
                MCBQuantum.Instance.GetMCB().Config.batteryCapacity = MCB_defaultBatteryCapacityTextBox.Text;

            }


            MCBQuantum.Instance.GetMCB().Config.batteryVoltage = (string)MCB_defaultBattreyVoltageComboBox.Text;
            MCBQuantum.Instance.GetMCB().Config.temperatureVoltageCompensation = MCB_temperatureCompensationTextBox.Text;

            if (MCBQuantum.Instance.GetMCB().Config.temperatureFormat)
            {
                MCBQuantum.Instance.GetMCB().Config.maxTemperatureFault = MCB_HighTemperatureThresholdTextBox.Text;

            }
            else
            {
                MCBQuantum.Instance.GetMCB().Config.maxTemperatureFault = ((float.Parse(MCB_HighTemperatureThresholdTextBox.Text) - 32) / 1.8f).ToString("N1").ToString();
            }
            MCBQuantum.Instance.GetMCB().Config.batteryType = (string)MCB_batteryTypeComboBox.Text;


            if (MCBQuantum.Instance.GetMCB().Config.temperatureFormat)
            {
                MCBQuantum.Instance.GetMCB().Config.TRtemperature = MCB_trickleTemperatureTextBox.Text;
            }
            else
            {
                MCBQuantum.Instance.GetMCB().Config.TRtemperature = ((float.Parse(MCB_trickleTemperatureTextBox.Text) - 32) / 1.8f).ToString("N1").ToString();

            }

            if (MCBQuantum.Instance.GetMCB().Config.temperatureFormat)
            {
                MCBQuantum.Instance.GetMCB().Config.foldTemperature = MCB_foldTemperatureTextBox.Text;
            }
            else
            {
                MCBQuantum.Instance.GetMCB().Config.foldTemperature = ((float.Parse(MCB_foldTemperatureTextBox.Text) - 32) / 1.8f).ToString("N1").ToString();


            }
            if (MCBQuantum.Instance.GetMCB().Config.temperatureFormat)
            {
                MCBQuantum.Instance.GetMCB().Config.coolDownTemperature = MCB_coolDownTemperatureTextLabel.Text;
            }
            else
            {
                MCBQuantum.Instance.GetMCB().Config.coolDownTemperature = ((float.Parse(MCB_coolDownTemperatureTextLabel.Text) - 32) / 1.8f).ToString("N1").ToString();
            }

            if (MCBQuantum.Instance.GetMCB().FirmwareRevision > 2.13f)
            {

                double c = double.Parse(MCB_nominalTemperatureValueTextBox.Text);
                sbyte v = (sbyte)Math.Round(2.0 * (c - 25.0));
                MCBQuantum.Instance.GetMCB().Config.nominal_temperature_shift = v;

            }

        }

    }
}
