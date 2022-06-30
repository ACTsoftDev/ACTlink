using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;
using Newtonsoft.Json;
using static actchargers.ACUtility;

namespace actchargers
{
    public class CommissionViewModel : BaseViewModel
    {
        VerifyControl verifyControl;
        MvxSubscriptionToken _mListSelector;
        #region BATTView list
        ListViewItem Batt_Commisssion_ElectrolytePanel;//Batt_Commisssion_ElectrolyteDisableRadio//Batt_Commisssion_ElectrolyteEnableRadio//Batt_Commisssion_ElectrolyteLabel;//Enable Electrolyte Sensor
        ListViewItem Batt_AdminActionsCommissionStep1Button;//Next
        ListViewItem Batt_Commisssion_BattViewTypePanel;//Batt_Commisssion_BattViewTypeLabel;//BattView Type//BattView Type//Batt_Commisssion_BattViewTypeMobileRadio//Batt_Commisssion_BattViewStandardRadio//Batt_Commisssion_BattViewTypeProRadio
        ListViewItem Batt_CommisssionSerialNumberTextBox;//Batt_CommisssionSerialNumberLabel;//Serial Number
        ListViewItem Batt_CommisssionApplicationTypeComboBox;//Batt_CommisssionApplicationTypeLabel;//Application Type
        ListViewItem Batt_Commisssion_ZoneComboBox;//Batt_Commisssion_ZoneLabel;//Time Zone
        ListViewItem Batt_Commisssion_ActViewPanel;//Batt_Commisssion_ActViewDisableRadio//Batt_Commisssion_ActViewEnableRadio//Batt_Commisssion_ActViewLabel;//Connect to ACTView
        ListViewItem Batt_Commisssion_CapacityTextBox;//Batt_Commisssion_CapacityLabel;//Battery Capacity (AHRs)
        ListViewItem Batt_Commisssion_VoltageTextBox;//Batt_Commisssion_VoltageLabel;//Battery Voltage
                                                     //ListViewItem Batt_AdminActionsCancelCommissionPanel;
        #endregion

        #region Charger list
        private ListViewItem MCB_Commission_MCB_SN_TextBox;
        private ListViewItem MCB_Commission_ChargerType_comboBox;
        private ListViewItem MCB_Commission_SN_textBox;
        private ListViewItem MCB_Commission_QPMmodel_ComboBox;
        private ListViewItem MCB_Commission_PMs_comboBox;
        private ListViewItem MCB_Commission_timezone_comboBox;
        private ListViewItem MCB_Commission_actview_panel;
        private ListViewItem MCB_Commission_capacity_textBox;
        private ListViewItem MCB_Commission_voltage_combobox;
        private ListViewItem MCB_Commission_cableLength_textbox;
        private ListViewItem MCB_Commission_cableGauge_combobox;
        private ListViewItem MCB_Commission_cablesCount_Panel;
        private ListViewItem MCB_Commission_IsBasic_Panel;
        private ListViewItem MCB_Commission_multiVoltage_panel;

        #endregion

        private ObservableCollection<ListViewItem> _commissionItemSource;
        public ObservableCollection<ListViewItem> CommissionItemSource
        {
            get { return _commissionItemSource; }
            set
            {
                _commissionItemSource = value;
                RaisePropertyChanged(() => CommissionItemSource);
            }
        }

        public CommissionViewModel()
        {
            CommissionItemSource = new ObservableCollection<ListViewItem>();
            CreateList();
        }

        void CreateList()
        {
            if (IsBattView)
            {
                ViewTitle = AppResources.commission_a_battview;
                CreateListForBattView();
            }
            else
            {
                ViewTitle = AppResources.commission_a_charger;
                CreateListForChargers();
            }
        }

        #region BattView create data
        void CreateListForBattView()
        {
            Batt_CommisssionApplicationTypeComboBox = new ListViewItem
            {
                Index = 0,
                Title = "Application Type",
                DefaultCellType = CellTypes.ListSelector,
                EditableCellType = CellTypes.ListSelector,
                ListSelectionCommand = ListSelectorCommand
            };
            CommissionItemSource.Add(Batt_CommisssionApplicationTypeComboBox);

            Batt_CommisssionSerialNumberTextBox = new ListViewItem
            {
                Index = 1,
                Title = "Serial Number",
                DefaultCellType = CellTypes.LabelTextEdit,
                EditableCellType = CellTypes.LabelTextEdit,
                IsEditable = true
            };
            CommissionItemSource.Add(Batt_CommisssionSerialNumberTextBox);

            Batt_Commisssion_ZoneComboBox = new ListViewItem
            {
                Index = 2,
                Title = "Time Zone",
                DefaultCellType = CellTypes.ListSelector,
                EditableCellType = CellTypes.ListSelector,
                ListSelectorType = ACUtility.ListSelectorType.Timezone,
                ListSelectionCommand = ListSelectorCommand
            };
            CommissionItemSource.Add(Batt_Commisssion_ZoneComboBox);

            Batt_Commisssion_ActViewPanel = new ListViewItem
            {
                Index = 3,
                Title = "ACT Intelligent",
                DefaultCellType = CellTypes.LabelSwitch,
                EditableCellType = CellTypes.LabelSwitch
            };
            CommissionItemSource.Add(Batt_Commisssion_ActViewPanel);

            Batt_Commisssion_CapacityTextBox = new ListViewItem
            {
                Index = 4,
                Title = "Battery Capacity (AHRs)",
                IsEditable = true,
                DefaultCellType = CellTypes.LabelTextEdit,
                EditableCellType = CellTypes.LabelTextEdit
            };
            CommissionItemSource.Add(Batt_Commisssion_CapacityTextBox);

            Batt_Commisssion_VoltageTextBox = new ListViewItem
            {
                Index = 5,
                Title = "Battery Voltage",
                IsEditable = true,
                DefaultCellType = CellTypes.LabelTextEdit,
                EditableCellType = CellTypes.LabelTextEdit
            };
            CommissionItemSource.Add(Batt_Commisssion_VoltageTextBox);

            Batt_Commisssion_ElectrolytePanel = new ListViewItem
            {
                Index = 7,
                Title = "Enable Electrolyte Sensor",
                DefaultCellType = CellTypes.LabelSwitch,
                EditableCellType = CellTypes.LabelSwitch
            };
            CommissionItemSource.Add(Batt_Commisssion_ElectrolytePanel);

            Batt_Commisssion_BattViewTypePanel = new ListViewItem
            {
                Index = 6,
                Title = "BattView Type",
                DefaultCellType = CellTypes.ListSelector,
                EditableCellType = CellTypes.ListSelector,
                ListSelectionCommand = ListSelectorCommand
            };
            CommissionItemSource.Add(Batt_Commisssion_BattViewTypePanel);

            try
            {
                Batt_LoadCommissionPanel();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Logger.AddLog(true, "X25" + ex);
            }

            if (CommissionItemSource.Count > 0)
            {
                CommissionItemSource = new ObservableCollection<ListViewItem>(CommissionItemSource.OrderBy(o => o.Index));
            }
        }
        #endregion

        #region BattView Implementation

        void Batt_LoadCommissionPanel()
        {
            if (!BattViewQuantum.Instance.BattView_quickSerialNumberCheckStrict())
                return;
            BattViewObject activeBattView = BattViewQuantum.Instance.GetBATTView();
            Batt_CommisssionSerialNumberTextBox.Text = activeBattView.Config.battViewSN;

            string model = "";
            bool snERROR = !BattViewQuantum.Instance.batt_verifyBAttViewSerialNumber(Batt_CommisssionSerialNumberTextBox.Text, ref model);

            if (snERROR)
            {
                Batt_CommisssionSerialNumberTextBox.Text = "";
                Batt_CommisssionApplicationTypeComboBox.SelectedIndex = -1;
                Batt_Commisssion_ActViewPanel.IsSwitchEnabled = false;
                Batt_Commisssion_ZoneComboBox.SelectedIndex = -1;
                Batt_Commisssion_VoltageTextBox.SelectedItem = "36";
                Batt_Commisssion_CapacityTextBox.Text = "850";
                Batt_Commisssion_BattViewTypePanel.SelectedIndex = -1;
                Batt_Commisssion_ElectrolytePanel.IsSwitchEnabled = false;
                return;
            }

            Batt_CommisssionApplicationTypeComboBox.Items = new List<object>();
            Batt_CommisssionApplicationTypeComboBox.Items.AddRange(new object[] {
            "Fast",
            "Conventional",
            "Opportunity"});

            if (activeBattView.Config.chargerType < Batt_CommisssionApplicationTypeComboBox.Items.Count)
            {
                Batt_CommisssionApplicationTypeComboBox.SelectedIndex = activeBattView.Config.chargerType;
                Batt_CommisssionApplicationTypeComboBox.SelectedItem = Batt_CommisssionApplicationTypeComboBox.Items[Batt_CommisssionApplicationTypeComboBox.SelectedIndex].ToString();
            }
            Batt_Commisssion_ActViewPanel.IsSwitchEnabled = activeBattView.Config.ActViewEnabled;

            Batt_Commisssion_ZoneComboBox.Items = JsonConvert.DeserializeObject<List<object>>(JsonConvert.SerializeObject(StaticDataAndHelperFunctions.GetZonesList()));
            Batt_Commisssion_ZoneComboBox.SelectedItem = StaticDataAndHelperFunctions.getZoneByID(activeBattView.myZone).ToString();
            Batt_Commisssion_ZoneComboBox.SelectedIndex = StaticDataAndHelperFunctions.GetZonesList().FindIndex(o => o.display_name == Batt_Commisssion_ZoneComboBox.SelectedItem);

            Batt_Commisssion_VoltageTextBox.Items = new List<object>();
            Batt_Commisssion_VoltageTextBox.Items.AddRange(new object[] {
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
            "44",
            "46",
            "48",
            "72",
            "80"});
            Batt_Commisssion_VoltageTextBox.SelectedItem = activeBattView.Config.nominalvoltage.ToString();
            Batt_Commisssion_VoltageTextBox.SelectedIndex = Batt_Commisssion_VoltageTextBox.Items.FindIndex(o => ((string)o).Equals(Batt_Commisssion_VoltageTextBox.SelectedItem));

            Batt_Commisssion_CapacityTextBox.Text = activeBattView.Config.ahrcapacity.ToString();
            Batt_Commisssion_BattViewTypePanel.Items = new List<object>();
            Batt_Commisssion_BattViewTypePanel.Items.AddRange(new object[] {
            "Standard",
            "Pro",
            "Mobile"});

            Batt_Commisssion_BattViewTypePanel.SelectedIndex = -1;
            if (model == "30")
            {
                Batt_Commisssion_BattViewTypePanel.SelectedIndex = 2;
            }
            else if (model == "20")
            {
                Batt_Commisssion_BattViewTypePanel.SelectedIndex = 1;
            }
            else
            {
                Batt_Commisssion_BattViewTypePanel.SelectedIndex = 0;
            }
            Batt_Commisssion_BattViewTypePanel.SelectedItem = Batt_Commisssion_BattViewTypePanel.Items[Batt_Commisssion_BattViewTypePanel.SelectedIndex].ToString();

            Batt_Commisssion_ElectrolytePanel.IsSwitchEnabled = activeBattView.Config.enableElectrolyeSensing != 0;
        }

        bool Batt_VerifyAndAutoLoadCommissionPanel()
        {
            if (!BattViewQuantum.Instance.BattView_quickSerialNumberCheckStrict())
                return false;

            bool snERROR = false;
            VerifyControl v = new VerifyControl();

            string model = "";
            snERROR = !BattViewQuantum.Instance.batt_verifyBAttViewSerialNumber(Batt_CommisssionSerialNumberTextBox.Text, ref model);

            if (snERROR)
                v.InsertRemoveFault(true, Batt_CommisssionSerialNumberTextBox);
            else
                v.InsertRemoveFault(false, Batt_CommisssionSerialNumberTextBox);

            v.VerifyComboBox(Batt_CommisssionApplicationTypeComboBox, Batt_CommisssionApplicationTypeComboBox);
            v.VerifyComboBox(Batt_Commisssion_ActViewPanel, Batt_Commisssion_ActViewPanel);
            v.VerifyComboBox(Batt_Commisssion_ZoneComboBox, Batt_Commisssion_ZoneComboBox);
            v.VerifyInteger(Batt_Commisssion_CapacityTextBox, Batt_Commisssion_CapacityTextBox, 40, 2000);
            if (!(Batt_Commisssion_BattViewTypePanel.SelectedIndex == 3 ? true : false) &&
                !(Batt_Commisssion_BattViewTypePanel.SelectedIndex == 2 ? true : false) &&
                !(Batt_Commisssion_BattViewTypePanel.SelectedIndex == 1 ? true : false))
            {
                v.InsertRemoveFault(true, Batt_Commisssion_BattViewTypePanel);
            }
            else
            {
                v.InsertRemoveFault(false, Batt_Commisssion_BattViewTypePanel);
            }
            if (v.HasErrors())
                return false;
            //check if the SN match the model
            if ((model == "10" && !(Batt_Commisssion_BattViewTypePanel.SelectedIndex == 0 ? true : false)) ||
                (model == "20" && !(Batt_Commisssion_BattViewTypePanel.SelectedIndex == 1 ? true : false)) ||
                (model == "30" && !(Batt_Commisssion_BattViewTypePanel.SelectedIndex == 2 ? true : false))
                )
            {
                v.InsertRemoveFault(true, Batt_Commisssion_BattViewTypePanel);
                return false;
            }
            else
            {
                v.InsertRemoveFault(false, Batt_Commisssion_BattViewTypePanel);
            }

            BattViewObject activeBattView = BattViewQuantum.Instance.GetBATTView();
            activeBattView.Config.battViewSN = Batt_CommisssionSerialNumberTextBox.Text;

            activeBattView.Config.installationDate = DateTime.Now;
            activeBattView.Config.ahrcapacity = ushort.Parse(Batt_Commisssion_CapacityTextBox.Text);
            activeBattView.Config.warrantedAHR = (UInt32)1250 * (UInt32)activeBattView.Config.ahrcapacity;
            activeBattView.myZone = (byte)Batt_Commisssion_ZoneComboBox.SelectedIndex;
            activeBattView.Config.chargerType = (byte)Batt_CommisssionApplicationTypeComboBox.SelectedIndex;
            BattViewQuantum.Instance.Batt_saveDefaultChargeProfile();
            //activeBattView.config.id;
            activeBattView.Config.tempFa = 0.001138722f;
            activeBattView.Config.tempFb = 0.000232569f; ;
            activeBattView.Config.tempFc = 0.00000009375905123f; ;
            activeBattView.Config.intercellCoefficient = 0.004f;
            //activeBattView.config.voltageCalA;
            //activeBattView.config.voltageCalB;
            activeBattView.Config.NTCcalA = 52302.14f;
            activeBattView.Config.NTCcalB = -41598.6f;
            //activeBattView.config.currentCalA;
            //activeBattView.config.currentCalB;
            activeBattView.Config.intercellTemperatureCALa = 52302.14f;
            activeBattView.Config.intercellTemperatureCALb = -41598.6f;
            activeBattView.Config.chargeToIdleTimer = 300;
            activeBattView.Config.chargeToInUseTimer = 30;
            activeBattView.Config.inUseToChargeTimer = 60;
            activeBattView.Config.inUsetoIdleTimer = 600;
            activeBattView.Config.idleToChargeTimer = 10;
            activeBattView.Config.idleToInUseTimer = 10;
            activeBattView.Config.electrolyteHLT = 120;
            activeBattView.Config.electrolyteLHT = 120;
            activeBattView.Config.actViewConnectFrequency = 900;

            activeBattView.Config.batteryHighTemperature = 544;
            activeBattView.Config.autoLogTime = 15;
            BattViewQuantum.Instance.Batt_loadDefaultWifiSettings();
            //Batt_SaveIntoWiFiSettings();
            activeBattView.Config.studyId = 0;
            activeBattView.Config.replacmentPart = false;
            activeBattView.Config.currentIdleToCharge = 110;
            activeBattView.Config.currentIdleToInUse = -110;
            activeBattView.Config.currentChargeToIdle = 90;
            activeBattView.Config.currentChargeToInUse = -110;
            activeBattView.Config.currentInUseToCharge = 110;
            activeBattView.Config.currentInUseToIdle = -90;
            activeBattView.Config.batteryID = "My Battery";

            activeBattView.Config.isPA = (byte)(Batt_Commisssion_BattViewTypePanel.SelectedIndex == 2 ? 1 : 0);
            activeBattView.Config.enableHallEffectSensing = activeBattView.Config.isPA;

            activeBattView.Config.ActViewEnabled = Batt_Commisssion_ActViewPanel.IsSwitchEnabled;
            activeBattView.Config.nominalvoltage = byte.Parse((string)Batt_Commisssion_VoltageTextBox.SelectedItem);

            activeBattView.Config.enableElectrolyeSensing = (byte)(Batt_Commisssion_ElectrolytePanel.IsSwitchEnabled ? 1 : 0); ;
            activeBattView.Config.enableExtTempSensing = (byte)(!(Batt_Commisssion_BattViewTypePanel.SelectedIndex == 2) ? 1 : 0);
            activeBattView.Config.temperatureControl = 0;
            activeBattView.Config.batteryType = 0;
            activeBattView.Config.batteryTemperatureCompesnation = 60;

            activeBattView.Config.eventDetectVoltagePercentage = 2;
            activeBattView.Config.eventDetectCurrentRangePercentage = 40;
            activeBattView.Config.eventDetectTimeRangePercentage = 67;

            activeBattView.Config.enablePLC = (byte)(Batt_Commisssion_BattViewTypePanel.SelectedIndex == 1 ? 1 : 0); ;

            activeBattView.Config.enableDayLightSaving = 1;
            activeBattView.Config.temperatureFormat = 1;
            activeBattView.Config.TRTemperature = 100;
            activeBattView.Config.foldTemperature = 516;
            activeBattView.Config.coolDownTemperature = 461;

            activeBattView.Config.currentClampCalA = 0.5016603f;
            activeBattView.Config.currentClampCalB = -927.2034f;
            activeBattView.Config.currentClamp2CalA = 4.960397f;
            activeBattView.Config.currentClamp2CalB = -9187.869f;

            return true;
        }
        #endregion

        #region BattView button actions

        //Next button command
        public IMvxCommand AdminActionsCommissionStep1ButtonCommand
        {
            get
            {
                return new MvxCommand(async () =>
                {
                    if (IsBattView)
                    {
                        await Batt_AdminActionsCommissionStep1ButtonClick();
                    }
                    else
                    {
                        await MCB_AdminActionsCommissionStep1ButtonClick();
                    }
                });
            }
        }

        private async Task Batt_AdminActionsCommissionStep1ButtonClick()
        {

            BattViewCommunicationTypes caller = BattViewCommunicationTypes.NOCall;
            object arg1 = null;
            string msg = string.Empty;
            if (!BattViewQuantum.Instance.BattView_quickSerialNumberCheckStrict())
                return;
            try
            {
                IsBusy = true;
                if (Batt_VerifyAndAutoLoadCommissionPanel())
                {
                    caller = BattViewCommunicationTypes.saveConfigCommission;
                }

                if (caller != BattViewCommunicationTypes.NOCall)
                {
                    List<object> arguments = new List<object>();
                    arguments.Add(caller);
                    arguments.Add(arg1);
                    List<object> results = new List<object>();
                    try
                    {
                        results = await BattViewQuantum.Instance.CommunicateBATTView(arguments);
                        if (results.Count > 0)
                        {
                            var callerStatus = results[3];
                            var status = (CommunicationResult)results[2];
                            if (callerStatus.Equals(BattViewCommunicationTypes.saveConfigCommission))
                            {
                                if (status == CommunicationResult.OK)
                                {
                                    //ACUserDialogs.ShowAlert("kkkkk");
                                    ShowViewModel<CommissionNextViewModel>();
                                    // Batt_CommissionAction(Batt_AdminActionsCommissionStep1Button, null);
                                    //showBusy = false;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.AddLog(true, "X86" + ex);
                        IsBusy = false;
                        return;
                    }
                }
                else
                {
                    ACUserDialogs.ShowAlertWithTitleAndOkButton(AppResources.invalid_input);
                }
                IsBusy = false;
            }
            catch (Exception ex)
            {
                IsBusy = false;
                Logger.AddLog(true, "X8" + ex);
                return;
            }
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
                ShowViewModel<ListSelectorViewModel>(new { title = item.Title, type = item.ListSelectorType, selectedItemIndex = CommissionItemSource.IndexOf(item), ItemSourceStr = JsonConvert.SerializeObject(item.Items) });
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
                CommissionItemSource[obj.SelectedItemindex].Text = obj.SelectedItem;
                CommissionItemSource[obj.SelectedItemindex].SelectedIndex = obj.SelectedIndex;
                //RaisePropertyChanged(() => CommissionItemSource);
            }
        }
        #endregion

        #region Charger create data

        void CreateListForChargers()
        {
            MCB_Commission_MCB_SN_TextBox = new ListViewItem
            {
                Index = 1,
                Title = "MCB Serial Number (Optional)",
                IsEditable = true,
                DefaultCellType = CellTypes.LabelTextEdit,
                EditableCellType = CellTypes.LabelTextEdit,
                TextMaxLength = 12
            };
            CommissionItemSource.Add(MCB_Commission_MCB_SN_TextBox);
            MCB_Commission_ChargerType_comboBox = new ListViewItem
            {
                Index = 2,
                Title = "Charger Type",
                DefaultCellType = CellTypes.ListSelector,
                EditableCellType = CellTypes.ListSelector,
                ListSelectionCommand = ListSelectorCommand

            };
            CommissionItemSource.Add(MCB_Commission_ChargerType_comboBox);
            MCB_Commission_SN_textBox = new ListViewItem
            {
                Index = 3,
                Title = "Serial Number",
                IsEditable = true,
                DefaultCellType = CellTypes.LabelTextEdit,
                EditableCellType = CellTypes.LabelTextEdit,
                TextMaxLength = 12
            };
            CommissionItemSource.Add(MCB_Commission_SN_textBox);
            MCB_Commission_QPMmodel_ComboBox = new ListViewItem
            {
                Index = 4,
                Title = "QPM model",
                DefaultCellType = CellTypes.ListSelector,
                EditableCellType = CellTypes.ListSelector,
                ListSelectionCommand = ListSelectorCommand
            };
            CommissionItemSource.Add(MCB_Commission_QPMmodel_ComboBox);
            MCB_Commission_PMs_comboBox = new ListViewItem
            {
                Index = 5,
                Title = "Number of PMs",
                DefaultCellType = CellTypes.ListSelector,
                EditableCellType = CellTypes.ListSelector,
                ListSelectionCommand = ListSelectorCommand
            };
            CommissionItemSource.Add(MCB_Commission_PMs_comboBox);
            MCB_Commission_timezone_comboBox = new ListViewItem
            {
                Index = 6,
                Title = "Time Zone",
                DefaultCellType = CellTypes.ListSelector,
                EditableCellType = CellTypes.ListSelector,
                ListSelectorType = ACUtility.ListSelectorType.Timezone,
                ListSelectionCommand = ListSelectorCommand
            };
            CommissionItemSource.Add(MCB_Commission_timezone_comboBox);
            MCB_Commission_actview_panel = new ListViewItem
            {
                Index = 7,
                Title = "ACT intelligent",
                DefaultCellType = CellTypes.LabelSwitch,
                EditableCellType = CellTypes.LabelSwitch
            };
            CommissionItemSource.Add(MCB_Commission_actview_panel);
            MCB_Commission_capacity_textBox = new ListViewItem
            {
                Index = 8,
                Title = "Battery Capacity (AHRs)",
                DefaultCellType = CellTypes.LabelTextEdit,
                EditableCellType = CellTypes.LabelTextEdit,
                IsEditable = true
            };
            CommissionItemSource.Add(MCB_Commission_capacity_textBox);
            MCB_Commission_voltage_combobox = new ListViewItem
            {
                Index = 9,
                Title = "Battery Voltage",
                DefaultCellType = CellTypes.ListSelector,
                EditableCellType = CellTypes.ListSelector,
                ListSelectionCommand = ListSelectorCommand
            };
            CommissionItemSource.Add(MCB_Commission_voltage_combobox);
            MCB_Commission_cableLength_textbox = new ListViewItem
            {
                Index = 10,
                Title = "Cable Length",
                DefaultCellType = CellTypes.LabelTextEdit,
                EditableCellType = CellTypes.LabelTextEdit,
                IsEditable = true,
                TextInputType = InputType.Number
            };
            CommissionItemSource.Add(MCB_Commission_cableLength_textbox);
            MCB_Commission_cableGauge_combobox = new ListViewItem
            {
                Index = 11,
                Title = "Cable Gauge",
                DefaultCellType = CellTypes.ListSelector,
                EditableCellType = CellTypes.ListSelector,
                ListSelectionCommand = ListSelectorCommand
            };
            CommissionItemSource.Add(MCB_Commission_cableGauge_combobox);
            MCB_Commission_cablesCount_Panel = new ListViewItem
            {
                Index = 12,
                Title = "Of Cables",
                DefaultCellType = CellTypes.ListSelector,//Single,Dual
                EditableCellType = CellTypes.ListSelector,
                ListSelectionCommand = ListSelectorCommand
            };
            CommissionItemSource.Add(MCB_Commission_cablesCount_Panel);
            MCB_Commission_IsBasic_Panel = new ListViewItem
            {
                Index = 13,
                Title = "Conventional High Frequency Chargers - 2 Year Warranty - No PLC/LED",
                DefaultCellType = CellTypes.LabelSwitch,
                EditableCellType = CellTypes.LabelSwitch
            };
            CommissionItemSource.Add(MCB_Commission_IsBasic_Panel);
            MCB_Commission_multiVoltage_panel = new ListViewItem
            {
                Index = 14,
                Title = "Multi Voltage",
                DefaultCellType = CellTypes.LabelSwitch,
                EditableCellType = CellTypes.LabelSwitch
            };

            CommissionItemSource.Add(MCB_Commission_multiVoltage_panel);

            try
            {
                MCB_LoadCommissionPanel();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Logger.AddLog(true, "X25" + ex);
            }

            if (CommissionItemSource.Count > 0)
            {
                CommissionItemSource = new ObservableCollection<ListViewItem>(CommissionItemSource.OrderBy(o => o.Index));
            }
        }

        void MCB_LoadCommissionPanel()
        {
            bool loadDefaultsFromMCB = MCBQuantum.Instance.MCB_quickSerialNumberCheckStrict();
            MCBobject activeMCB = MCBQuantum.Instance.GetMCB();
            if (loadDefaultsFromMCB)
            {
                if (activeMCB.Config.serialNumber.Length != 12)
                {
                    loadDefaultsFromMCB = false;
                }
                else
                {
                    List<string> validModels = new List<string> { "10", "11", "12", "13", "20", "21", "22", "23", "30", "31", "32", "33" };
                    string productFamily = activeMCB.Config.serialNumber.Substring(0, 1);
                    string model = activeMCB.Config.serialNumber.Substring(1, 2);
                    string month = activeMCB.Config.serialNumber.Substring(3, 2);
                    string year = activeMCB.Config.serialNumber.Substring(5, 2);
                    string subid = activeMCB.Config.serialNumber.Substring(7, 5);
                    int tempInt = 0;
                    if (productFamily != "2" || !validModels.Contains(model)
                        || !int.TryParse(month, out tempInt) || tempInt < 1 || tempInt > 12
                        || !int.TryParse(year, out tempInt) || tempInt < 16 || tempInt > 99
                        || !int.TryParse(subid, out tempInt) || tempInt < 0 || tempInt > 99999)
                    {
                        loadDefaultsFromMCB = false;
                    }

                }

            }


            if (loadDefaultsFromMCB)
            {
                MCB_Commission_multiVoltage_panel.IsSwitchEnabled = activeMCB.Config.enableAutoDetectMultiVoltage;

                MCB_Commission_voltage_combobox.Items = new List<object>();
                MCB_Commission_voltage_combobox.Items.AddRange(new object[] {
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
            "44",
            "46",
            "48",
            "72",
            "80"});
                MCB_Commission_voltage_combobox.SelectedItem = activeMCB.Config.batteryVoltage;
                MCB_Commission_voltage_combobox.SelectedIndex = MCB_Commission_voltage_combobox.Items.FindIndex(o => ((string)o).Equals(MCB_Commission_voltage_combobox.SelectedItem));

                MCB_Commission_capacity_textBox.Text = activeMCB.Config.batteryCapacity;
                MCB_Commission_actview_panel.IsSwitchEnabled = activeMCB.Config.actViewEnable;

                MCB_Commission_timezone_comboBox.Items = JsonConvert.DeserializeObject<List<object>>(JsonConvert.SerializeObject(StaticDataAndHelperFunctions.GetZonesList()));
                if (activeMCB.myZone == 0)
                {
                    MCB_Commission_timezone_comboBox.SelectedIndex = -1;
                }
                else
                {
                    MCB_Commission_timezone_comboBox.SelectedItem = StaticDataAndHelperFunctions.getZoneByID(activeMCB.myZone).ToString();
                    MCB_Commission_timezone_comboBox.SelectedIndex = StaticDataAndHelperFunctions.GetZonesList().FindIndex(o => o.display_name == MCB_Commission_timezone_comboBox.SelectedItem);

                }
                MCB_Commission_PMs_comboBox.Items = new List<object>();
                MCB_Commission_PMs_comboBox.Items.AddRange(new object[] {
            "01",
            "02",
            "03",
            "04",
            "05",
            "06",
            "07",
            "08",
            "09",
            "10",
            "11",
            "12"});
                MCB_Commission_PMs_comboBox.SelectedItem = activeMCB.Config.numberOfInstalledPMs;
                MCB_Commission_PMs_comboBox.SelectedIndex = MCB_Commission_PMs_comboBox.Items.FindIndex(o => ((string)o).Equals(MCB_Commission_PMs_comboBox.SelectedItem));


                int selctedIndex = 0;
                switch (activeMCB.Config.PMvoltage)
                {
                    case "36": selctedIndex += 0; break;
                    case "48": selctedIndex += 1; break;
                    case "80": selctedIndex += 2; break;
                }
                selctedIndex += activeMCB.Config.PMvoltageInputValue * 3;

                MCB_Commission_QPMmodel_ComboBox.Items = new List<object>();
                MCB_Commission_QPMmodel_ComboBox.Items.AddRange(new object[] {
            "QPM-24/36-50-480",
            "QPM-48-40-480",
            "QPM-80-25-480",
            "QPM-24/36-50-208",
            "QPM-48-40-208",
            "QPM-80-25-208",
            "QPM-24/36-50-600",
            "QPM-48-40-600",
            "QPM-80-25-600",
            "QPM-24/36-50-380",
            "QPM-48-40-380",
            "QPM-80-25-380"});

                MCB_Commission_QPMmodel_ComboBox.SelectedIndex = selctedIndex;

                MCB_Commission_QPMmodel_ComboBox.SelectedItem = MCB_Commission_QPMmodel_ComboBox.Items[MCB_Commission_QPMmodel_ComboBox.SelectedIndex].ToString();

                MCB_Commission_ChargerType_comboBox.Items = new List<object>();
                MCB_Commission_ChargerType_comboBox.Items.AddRange(new object[] {
            "Fast",
            "Conventional",
            "Opportunity"});
                MCB_Commission_ChargerType_comboBox.SelectedIndex = activeMCB.Config.chargerType;
                MCB_Commission_ChargerType_comboBox.SelectedItem = MCB_Commission_ChargerType_comboBox.Items[MCB_Commission_ChargerType_comboBox.SelectedIndex].ToString();


                MCB_Commission_SN_textBox.Text = activeMCB.Config.serialNumber;

                MCB_Commission_MCB_SN_TextBox.Text = activeMCB.Config.originalSerialNumber;

                if (activeMCB.Config.enablePLC && activeMCB.Config.chargerType != 1)
                {
                    MCB_Commission_IsBasic_Panel.IsSwitchEnabled = true;

                }
                else
                {
                    MCB_Commission_IsBasic_Panel.IsSwitchEnabled = false;
                }

            }
            else
            {
                MCB_Commission_actview_panel.IsSwitchEnabled = false;
                MCB_Commission_multiVoltage_panel.IsSwitchEnabled = false;
                MCB_Commission_voltage_combobox.SelectedIndex = -1;
                MCB_Commission_capacity_textBox.Text = "";
                MCB_Commission_timezone_comboBox.SelectedIndex = -1;
                MCB_Commission_PMs_comboBox.SelectedIndex = -1;
                MCB_Commission_QPMmodel_ComboBox.SelectedIndex = -1;
                MCB_Commission_ChargerType_comboBox.SelectedIndex = -1;
                MCB_Commission_MCB_SN_TextBox.Text = "";
                MCB_Commission_SN_textBox.Text = "";
                MCB_Commission_IsBasic_Panel.IsSwitchEnabled = false;
            }
            MCB_Commission_cablesCount_Panel.Items = new List<object>();
            MCB_Commission_cablesCount_Panel.Items.Add("Single");
            MCB_Commission_cablesCount_Panel.Items.Add("Dual");
            MCB_Commission_cablesCount_Panel.SelectedIndex = -1;

            MCB_Commission_cableGauge_combobox.Items = new List<object>();
            MCB_Commission_cableGauge_combobox.Items.AddRange(new object[] {
            "1/0",
            "2/0",
            "3/0",
            "4/0"});
            MCB_Commission_cableGauge_combobox.SelectedIndex = -1;
            MCB_Commission_cableLength_textbox.Text = "";

        }

        bool MCB_VerifyAndAutoLoadCommissionPanel()
        {
            if (!MCBQuantum.Instance.MCB_quickSerialNumberCheckStrict())
                return false;
            MCBobject activeMCB = MCBQuantum.Instance.GetMCB();
            verifyControl = new VerifyControl();
            bool snERROR = false;
            string model = "";
            int maxNumberOfPMs = 12;
            if (MCB_Commission_SN_textBox.Text.Length != 12)
            {
                snERROR = true;
            }
            else if (true || !ControlObject.isDebugMaster)
            {
                List<string> validModels = new List<string> { "10", "11", "12", "13", "20", "21", "22", "23", "30", "31", "32", "33" };
                string productFamily = MCB_Commission_SN_textBox.Text.Substring(0, 1);
                model = MCB_Commission_SN_textBox.Text.Substring(1, 2);
                string month = MCB_Commission_SN_textBox.Text.Substring(3, 2);
                string year = MCB_Commission_SN_textBox.Text.Substring(5, 2);
                string subid = MCB_Commission_SN_textBox.Text.Substring(7, 5);
                int tempInt = 0;
                if (productFamily != "2" || !validModels.Contains(model)
                    || !int.TryParse(month, out tempInt) || tempInt < 1 || tempInt > 12
                    || !int.TryParse(year, out tempInt) || tempInt < 16 || tempInt > 99
                    || !int.TryParse(subid, out tempInt) || tempInt < 0 || tempInt > 99999)
                {
                    snERROR = true;
                }

            }
            if (snERROR)
                verifyControl.InsertRemoveFault(true, MCB_Commission_SN_textBox);
            else
            {
                if (model == "10")
                    maxNumberOfPMs = 4;
                else if (model == "20")
                    maxNumberOfPMs = 6;
                else
                    maxNumberOfPMs = 12;
                verifyControl.InsertRemoveFault(false, MCB_Commission_SN_textBox);
            }

            verifyControl.VerifyComboBox(MCB_Commission_ChargerType_comboBox, MCB_Commission_ChargerType_comboBox);
            verifyControl.VerifyComboBox(MCB_Commission_PMs_comboBox, MCB_Commission_PMs_comboBox);
            if (MCB_Commission_PMs_comboBox.SelectedIndex + 1 > maxNumberOfPMs)
                verifyControl.InsertRemoveFault(true, MCB_Commission_PMs_comboBox);
            verifyControl.VerifyComboBox(MCB_Commission_QPMmodel_ComboBox, MCB_Commission_QPMmodel_ComboBox);
            verifyControl.VerifyComboBox(MCB_Commission_timezone_comboBox, MCB_Commission_timezone_comboBox);
            verifyControl.VerifyInteger(MCB_Commission_capacity_textBox, MCB_Commission_capacity_textBox, 40, 5000);
            verifyControl.VerifyComboBox(MCB_Commission_voltage_combobox, MCB_Commission_voltage_combobox);
            verifyControl.VerifyComboBox(MCB_Commission_cableGauge_combobox, MCB_Commission_cableGauge_combobox);
            verifyControl.VerifyFloatNumber(MCB_Commission_cableLength_textbox, MCB_Commission_cableLength_textbox, 8, 30);
            if (!verifyControl.HasErrors())
            {
                if ((MCB_Commission_QPMmodel_ComboBox.SelectedIndex == 0 || MCB_Commission_QPMmodel_ComboBox.SelectedIndex == 2)
                    && int.Parse((string)MCB_Commission_voltage_combobox.SelectedItem) > 36)
                {
                    verifyControl.InsertRemoveFault(true, MCB_Commission_voltage_combobox);
                }
                else
                {
                    verifyControl.InsertRemoveFault(false, MCB_Commission_voltage_combobox);
                }
            }

            if ((MCB_Commission_ChargerType_comboBox.SelectedIndex == 0
                 || MCB_Commission_ChargerType_comboBox.SelectedIndex == 2) && MCB_Commission_IsBasic_Panel.IsSwitchEnabled)
            {
                verifyControl.InsertRemoveFault(true, MCB_Commission_IsBasic_Panel);

            }
            if (!verifyControl.HasErrors())
            {

                //jsonZone info = (jsonZone)(MCB_Commission_timezone_comboBox.SelectedItem);

                JsonZone info = (JsonZone)StaticDataAndHelperFunctions.getZoneByID((byte)MCB_Commission_timezone_comboBox.SelectedIndex);
                activeMCB.myZone = info.id;
                //Load all config....
                if (!string.IsNullOrEmpty(MCB_Commission_MCB_SN_TextBox.Text))
                    activeMCB.Config.originalSerialNumber = MCB_Commission_MCB_SN_TextBox.Text;
                if (activeMCB.Config.originalSerialNumber.Trim() == "")
                {
                    activeMCB.Config.originalSerialNumber = "C:" + MCB_Commission_SN_textBox.Text;

                }
                activeMCB.Config.replacmentPart = false;

                activeMCB.Config.serialNumber = MCB_Commission_SN_textBox.Text;
                activeMCB.Config.numberOfInstalledPMs = (string)MCB_Commission_PMs_comboBox.SelectedItem;
                //connManager.activeMCB.MCBConfig.HardwareRevision = "";
                activeMCB.Config.chargerUserName = "My ACT Charger";
                //connManager.activeMCB.MCBConfig.InternalChargerID;
                MCBQuantum.Instance.MCB_loadDefaultWIFI();
                //MCBQuantum.Instance.MCB_SaveIntoWiFiSettings();

                activeMCB.Config.actViewEnable = MCB_Commission_actview_panel.IsSwitchEnabled;
                activeMCB.Config.InstallationDate = DateTime.Now;
                activeMCB.Config.batteryType = "Lead Acid";
                activeMCB.Config.batteryType = MCBConfig.batteryTypes[0];

                //connManager.activeMCB.MCBConfig.TrickleVoltage;
                //connManager.activeMCB.MCBConfig.CVVoltage;
                //connManager.activeMCB.MCBConfig.finishVoltage;
                //connManager.activeMCB.MCBConfig.EqualaizeVoltage;
                activeMCB.Config.temperatureVoltageCompensation = "6";
                activeMCB.Config.maxTemperatureFault = "54.4";
                activeMCB.Config.batteryVoltage = (string)MCB_Commission_voltage_combobox.SelectedItem;
                //connManager.activeMCB.MCBConfig.CCRate;
                //connManager.activeMCB.MCBConfig.TRRate;
                //connManager.activeMCB.MCBConfig.FIRate;
                //connManager.activeMCB.MCBConfig.EQRate;
                activeMCB.Config.PMefficiency = "93";
                //connManager.activeMCB.MCBConfig.finishdV;
                //connManager.activeMCB.MCBConfig.cvCurrentStep;
                //connManager.activeMCB.MCBConfig.cvFinishCurrent;
                //connManager.activeMCB.MCBConfig.voltageCalibrationA = 0;
                //connManager.activeMCB.MCBConfig.voltageCalibrationB = 0;
                //connManager.activeMCB.MCBConfig.voltageCalibrationA_LOW = 0;
                //connManager.activeMCB.MCBConfig.voltageCalibrationB_LOW = 0;
                //connManager.activeMCB.MCBConfig.temperatureCalibrationA = 0;
                //connManager.activeMCB.MCBConfig.temperatureCalibrationB = 0;
                //connManager.activeMCB.MCBConfig.tempVi = 0;
                //connManager.activeMCB.MCBConfig.tempR = 0;
                float irVal = 0;
                irVal = float.Parse(MCB_Commission_cableLength_textbox.Text);
                irVal += 5;

                if (MCB_Commission_cablesCount_Panel.SelectedIndex == 1)//dual should be checked here
                    irVal *= 2;
                switch (MCB_Commission_cableGauge_combobox.SelectedIndex)
                {
                    case 0:
                        irVal *= 0.105f;
                        break;
                    case 1:
                        irVal *= 0.084f;
                        break;
                    case 2:
                        irVal *= 0.067f;
                        break;
                    case 3:
                        irVal *= 0.053f;
                        break;
                }
                activeMCB.Config.IR = 2.0f * irVal;
                //connManager.activeMCB.MCBConfig.temp_fa;
                //connManager.activeMCB.MCBConfig.temp_fb;
                //connManager.activeMCB.MCBConfig.temp_fc;
                //connManager.activeMCB.MCBConfig.lastChangeUserId;
                //connManager.activeMCB.MCBConfig.lastChangeTime;
                activeMCB.Config.enableChargerSimulationMode = false; // 0x0002
                activeMCB.Config.enableAutoDetectMultiVoltage = MCB_Commission_multiVoltage_panel.IsSwitchEnabled; //0x0040
                activeMCB.Config.temperatureSensorInstalled = false;//0x0004
                activeMCB.Config.enableRefreshCycleAfterFI = false;//0x0008
                activeMCB.Config.enableRefreshCycleAfterEQ = false;//0x0010
                activeMCB.Config.enablePMsimulation = false;//0x0020
                activeMCB.Config.chargerType = (byte)MCB_Commission_ChargerType_comboBox.SelectedIndex;

                MCBQuantum.Instance.MCB_saveDefaultChargeProfile();

                activeMCB.Config.batteryCapacity = MCB_Commission_capacity_textBox.Text;//two bytes integer 
                //connManager.activeMCB.MCBConfig.EqualizeStartTime;//in mins
                //connManager.activeMCB.MCBConfig.finishStartTime;//(hh:mm)-->two bytes (h10|0x80,h1,m10,m1)
                //connManager.activeMCB.MCBConfig.DelayedStartTime;//(hh:mm)-->one byte (Seconds)
                //connManager.activeMCB.MCBConfig.finishDaysMask;
                activeMCB.Config.autoStartCountDownTimer = "12";
                if (maxNumberOfPMs == 12)
                    activeMCB.Config.autoStartCountDownTimer = "15";
                activeMCB.Config.autoStartEnable = true;
                activeMCB.Config.autoStartMask.Saturday = true;//byte
                activeMCB.Config.autoStartMask.Sunday = true;//byte
                activeMCB.Config.autoStartMask.Monday = true;//byte
                activeMCB.Config.autoStartMask.Tuesday = true;//byte
                activeMCB.Config.autoStartMask.Thursday = true;//byte
                activeMCB.Config.autoStartMask.Friday = true;//byte
                activeMCB.Config.autoStartMask.Wednesday = true;//byte
                //connManager.activeMCB.MCBConfig.AlwaysFinish;//true results zero
                //connManager.activeMCB.MCBConfig.finishDuration;//hh:mm
                //connManager.activeMCB.MCBConfig.EqualizeDaysMask;
                //connManager.activeMCB.MCBConfig.EqualaizeDuration;//hh:mm
                //connManager.activeMCB.MCBConfig.EnableDayLightSaving;
                activeMCB.Config.temperatureFormat = false;
                activeMCB.Config.chargerOverrideBattviewFIEQsched = false;
                activeMCB.Config.forceFinishTimeout = false;
                activeMCB.Config.ignoreBATTViewSOC = false;

                activeMCB.Config.battviewAutoCalibrationEnable = false;
                if (MCB_Commission_QPMmodel_ComboBox.SelectedIndex == 0 || MCB_Commission_QPMmodel_ComboBox.SelectedIndex == 2)
                    activeMCB.Config.PMvoltage = "36";
                else
                    activeMCB.Config.PMvoltage = "48";
                //connManager.activeMCB.MCBConfig.CVTimer;
                //connManager.activeMCB.MCBConfig.finishTimer;
                //connManager.activeMCB.MCBConfig.finishdTTimer;
                //connManager.activeMCB.MCBConfig.EqualizeTimer;
                activeMCB.Config.refreshTimer = "08:00";
                activeMCB.Config.desulfationTimer = "12:00";
                //connManager.activeMCB.MCBConfig.lcdVersion;
                //connManager.activeMCB.MCBConfig.wifiVersion;

                if (MCB_Commission_IsBasic_Panel.IsSwitchEnabled)
                    activeMCB.Config.enablePLC = false;
                else
                    activeMCB.Config.enablePLC = true;

                activeMCB.Config.ledcontrol = 0;

                activeMCB.Config.enableManualEQ = false;
                activeMCB.Config.enableManualDesulfate = false;
                activeMCB.Config.energyDaysMask.Saturday = false;
                activeMCB.Config.energyDaysMask.Sunday = false;
                activeMCB.Config.energyDaysMask.Monday = false;
                activeMCB.Config.energyDaysMask.Tuesday = false;
                activeMCB.Config.energyDaysMask.Wednesday = false;
                activeMCB.Config.energyDaysMask.Thursday = false;
                activeMCB.Config.energyDaysMask.Friday = false;
                activeMCB.Config.lockoutStartTime = 0;
                activeMCB.Config.lockoutCloseTime = 6 * 3600;
                activeMCB.Config.energyStartTime = 0;
                activeMCB.Config.energyCloseTime = 6 * 3600;
                activeMCB.Config.enableManualDesulfate = false;
                activeMCB.Config.lockoutDaysMask.Saturday = false;
                activeMCB.Config.lockoutDaysMask.Sunday = false;
                activeMCB.Config.lockoutDaysMask.Monday = false;
                activeMCB.Config.lockoutDaysMask.Tuesday = false;
                activeMCB.Config.lockoutDaysMask.Wednesday = false;
                activeMCB.Config.lockoutDaysMask.Thursday = false;
                activeMCB.Config.lockoutDaysMask.Friday = false;
                activeMCB.Config.energyDecreaseValue = 90;

                activeMCB.Config.PMvoltageInputValue = (byte)(MCB_Commission_QPMmodel_ComboBox.SelectedIndex / 3);

                switch (MCB_Commission_QPMmodel_ComboBox.SelectedIndex % 3)
                {
                    case 0: activeMCB.Config.PMvoltage = "36"; break;
                    case 1: activeMCB.Config.PMvoltage = "48"; break;
                    case 2: activeMCB.Config.PMvoltage = "80"; break;
                }

                activeMCB.Config.disablePushButton = false;
                activeMCB.Config.TRtemperature = "10.0";
                //connManager.activeMCB.MCBConfig.afterCommissionBoardID;
                activeMCB.Config.foldTemperature = "51.6";
                activeMCB.Config.coolDownTemperature = "46.1";

                activeMCB.Config.model = MCB_GenerateChargerModel(activeMCB.Config.serialNumber,
                    activeMCB.Config.PMvoltage,
                    activeMCB.Config.numberOfInstalledPMs,
                    activeMCB.Config.PMvoltageInputValue,
                    activeMCB.Config.ledcontrol,
                    activeMCB.Config.enablePLC);
                activeMCB.Config.batteryCapacity24 = activeMCB.Config.batteryCapacity36 = activeMCB.Config.batteryCapacity48 = activeMCB.Config.batteryCapacity80 = UInt16.Parse(activeMCB.Config.batteryCapacity);

            }
            return !verifyControl.HasErrors();
        }
        static string MCB_GenerateChargerModel
        (string chargerSN, string PMvoltage, string numberOfInstalledPMs,
            byte PMvoltageInputValue, byte ledControl, bool enablePLC)
        {
            int maxNumberOfPMs = 12;
            string chargerModel = "";
            string model = "";
            model = chargerSN.Substring(1, 2);
            if (model[0] == '1')
                maxNumberOfPMs = 4;
            else if (model[0] == '2')
                maxNumberOfPMs = 6;

            chargerModel = "Q" + maxNumberOfPMs.ToString() + "-";
            chargerModel += PMvoltage + "-";
            int currentRating = int.Parse(numberOfInstalledPMs);
            if (PMvoltage == "36")
                currentRating *= 50;
            else if (PMvoltage == "48")
                currentRating *= 40;
            else if (PMvoltage == "80")
            {
                currentRating *= 52;

            }
            chargerModel += currentRating.ToString() + "-";

            switch (PMvoltageInputValue)
            {
                case 0:
                    chargerModel += "480";
                    break;
                case 1:
                    chargerModel += "208";
                    break;
                case 2:
                    chargerModel += "600";
                    break;
                case 3:
                    chargerModel += "380";
                    break;
            }
            if (!enablePLC && Convert.ToBoolean(ledControl))
                chargerModel += "-B";
            return chargerModel;
        }

        //MCB_AdminActionsCommissionStep1Button action
        async Task MCB_AdminActionsCommissionStep1ButtonClick()
        {
            McbCommunicationTypes caller = McbCommunicationTypes.NOCall;
            object arg1 = null;
            if (!MCBQuantum.Instance.MCB_quickSerialNumberCheckStrict())
                return;
            try
            {
                if (MCB_VerifyAndAutoLoadCommissionPanel())
                {
                    caller = McbCommunicationTypes.saveConfigCommission;
                }
                if (caller != McbCommunicationTypes.NOCall)
                {
                    IsBusy = true;
                    List<object> arguments = new List<object>();
                    arguments.Add(caller);
                    arguments.Add(arg1);
                    List<object> results = new List<object>();
                    results = await MCBQuantum.Instance.CommunicateMCB(arguments);
                    IsBusy = false;
                    if (results.Count > 0)
                    {
                        bool internalFailure = (bool)results[0];
                        if (internalFailure)
                        {
                            return;
                        }
                        var callerStatus = results[3];
                        var status = (CommunicationResult)results[2];
                        if (callerStatus.Equals(McbCommunicationTypes.saveConfigCommission))
                        {
                            if (status == CommunicationResult.OK)
                            {
                                ShowViewModel<CommissionNextViewModel>();

                            }
                            else
                            {
                                ACUserDialogs.ShowAlert("" + status);
                            }
                        }
                    }
                }
                else
                {
                    ACUserDialogs.ShowAlert(AppResources.alert_enter_valid + " " + verifyControl.GetErrorString());
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Logger.AddLog(true, "X26" + ex.ToString());
            }
        }

        #endregion
    }

}
