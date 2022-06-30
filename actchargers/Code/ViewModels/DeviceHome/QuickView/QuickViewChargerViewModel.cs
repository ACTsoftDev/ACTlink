using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;
using Newtonsoft.Json;
using OxyPlot;
using OxyPlot.Series;

namespace actchargers
{
    public class QuickViewChargerViewModel : BaseViewModel
    {
        readonly MCBobject activeMcb;

        ACTimer quickViewTimer;

        bool isLithiumAnd2_5OrAbove;

        MvxSubscriptionToken _mListSelector;
        ListViewItem MCB_LCDView_SCREEN_ChargerMain_EQControl;
        ListViewItem MCB_LCDView_SCREEN_ChargerMain_desulfateEnable;
        ListViewItem MCB_LCDView_SCREEN_ChargerMain_desulfateVoltage;
        ListViewItem MCB_LCDView_SCREEN_ChargerMain_desulfateDuration;
        ListViewItem MCB_LCDView_SCREEN_ChargerMain_desulfateCapacity;
        ListViewItem MCB_LCDView_SCREEN_ChargerMain_desulfatSaveBtn;
        ListViewItem MCB_LCDView_SCREEN_ChargerMain_StatusLabel;
        TableHeaderItem MCB_LCDView_SCREEN_EQ_panel;
        TableHeaderItem MCB_LCDView_SCREEN_Charger_Ready_panel;
        ListViewItem MCB_LCDView_SCREEN_Charger_Ready_panel_ListItem;
        TableHeaderItem MCB_LCDView_SCREEN_simulation_mode_panel;
        TableHeaderItem MCB_LCDView_SCREEN_Faulty_Screen_panel;
        ListViewItem MCB_LCDView_SCREEN_ChargerMain_panel;
        ListViewItem MCB_LCDView_SCREEN_ChargerValues_panel;
        ListViewItem MCB_LCDView_SCREEN_simulation_mode_TextLabel;
        ListViewItem MCB_LCDView_SCREEN_Faulty_Label;
        ListViewItem MCB_LCDView_SCREEN_ChargerButtons_StatusLabel;
        ListViewItem MCB_LCDView_SCREEN_ChargerMain_startStopButton;
        ListViewItem MCB_LCDView_SCREEN_ChargerMain_exitButton;

        private ObservableCollection<TableHeaderItem> _ListItemSource;
        public ObservableCollection<TableHeaderItem> ListItemSource
        {
            get { return _ListItemSource; }
            set
            {
                _ListItemSource = value;
                SetProperty(ref _ListItemSource, value);
            }
        }

        public QuickViewChargerViewModel()
        {
            ViewTitle = AppResources.quick_view;

            ConnectionManager connectionManager = MCBQuantum.Instance.GetConnectionManager();
            activeMcb = connectionManager.activeMCB;

            ListItemSource = new ObservableCollection<TableHeaderItem>();

            InitItems();

            CreateData();
        }

        void InitItems()
        {
            ListItemSource.Clear();

            MCB_LCDView_SCREEN_ChargerMain_EQControl = new ListViewItem()
            {
                Index = 0,
                Title = "EQ",
                DefaultCellType = ACUtility.CellTypes.LabelSwitch,
                EditableCellType = ACUtility.CellTypes.LabelSwitch,
                IsEditable = true,
                ParentIndex = 0,
                IsVisible = false
            };

            MCB_LCDView_SCREEN_ChargerMain_desulfateEnable = new ListViewItem()
            {
                Index = 1,
                Title = "Desulphate",
                DefaultCellType = ACUtility.CellTypes.LabelSwitch,
                EditableCellType = ACUtility.CellTypes.LabelSwitch,
                IsEditable = true,
                ParentIndex = 0,
                IsVisible = false
            };

            MCB_LCDView_SCREEN_ChargerMain_desulfateVoltage = new ListViewItem()
            {
                Index = 2,
                Title = "Voltage",
                DefaultCellType = ACUtility.CellTypes.ListSelector,
                EditableCellType = ACUtility.CellTypes.ListSelector,
                ListSelectionCommand = ListSelectorCommand,
                ParentIndex = 0,
                IsEditable = true,
                IsVisible = false
            };

            MCB_LCDView_SCREEN_ChargerMain_desulfateDuration = new ListViewItem()
            {
                Index = 3,
                Title = "Duration",
                DefaultCellType = ACUtility.CellTypes.ListSelector,
                EditableCellType = ACUtility.CellTypes.ListSelector,
                ListSelectionCommand = ListSelectorCommand,
                ParentIndex = 0,
                IsEditable = true,
                IsVisible = false
            };

            MCB_LCDView_SCREEN_ChargerMain_desulfateCapacity = new ListViewItem()
            {
                Index = 4,
                Title = "Capacity",
                DefaultCellType = ACUtility.CellTypes.LabelTextEdit,
                EditableCellType = ACUtility.CellTypes.LabelTextEdit,
                IsEditable = true,
                ParentIndex = 0,
                IsVisible = false,
                TextMaxLength = 4
            };

            MCB_LCDView_SCREEN_ChargerMain_desulfatSaveBtn = new ListViewItem()
            {
                Index = 5,
                Title = "Save",
                DefaultCellType = ACUtility.CellTypes.Button,
                EditableCellType = ACUtility.CellTypes.Button,
                ListSelectionCommand = SwitchValueChanged,
                IsEditable = true,
                ParentIndex = 0,
                IsVisible = false
            };

            MCB_LCDView_SCREEN_Charger_Ready_panel = new TableHeaderItem()
            {
                SectionHeader = "",
                IsVisible = false,
                ID = "MAIN"
            };

            MCB_LCDView_SCREEN_simulation_mode_panel = new TableHeaderItem()
            {
                SectionHeader = "",
                IsVisible = false,
                ID = "SIMULATION"
            };

            MCB_LCDView_SCREEN_Faulty_Screen_panel = new TableHeaderItem()
            {
                SectionHeader = "",
                IsVisible = false,
                ID = "FAULT"
            };

            MCB_LCDView_SCREEN_EQ_panel = new TableHeaderItem()
            {
                SectionHeader = "",
                IsVisible = false,
                ID = "EQ"
            };

            MCB_LCDView_SCREEN_Charger_Ready_panel_ListItem = new ListViewItem()
            {
                Title = "Connect",
                Index = 0,
                DefaultCellType = ACUtility.CellTypes.Image,
                EditableCellType = ACUtility.CellTypes.Image,
                IsEditable = false,
                ParentIndex = 1,
                //IsVisible = false,
                Text = "chargerquickview"
            };

            MCB_LCDView_SCREEN_ChargerMain_panel = new ListViewItem()
            {
                Title = "Plots",
                Index = 0,
                DefaultCellType = ACUtility.CellTypes.QuickViewPlotCollection,
                EditableCellType = ACUtility.CellTypes.QuickViewPlotCollection,
                IsEditable = false,
                ParentIndex = 1,
                IsVisible = false
            };

            MCB_LCDView_SCREEN_ChargerValues_panel = new ListViewItem()
            {
                Index = 0,
                DefaultCellType = ACUtility.CellTypes.QuickViewThreeLabel,
                EditableCellType = ACUtility.CellTypes.QuickViewThreeLabel,
                IsEditable = false,
                IsVisible = false,
                Title = "AHr",
                Title2 = "KWHr",
                Title3 = "Duration"
            };

            MCB_LCDView_SCREEN_simulation_mode_TextLabel = new ListViewItem()
            {
                Index = 0,
                Title = "Simualtion Mode",
                DefaultCellType = ACUtility.CellTypes.Default,
                EditableCellType = ACUtility.CellTypes.Default,
                IsEditable = false,
                IsVisible = false
            };

            MCB_LCDView_SCREEN_Faulty_Label = new ListViewItem()
            {
                Index = 0,
                //Title = "Fault",
                DefaultCellType = ACUtility.CellTypes.Default,
                EditableCellType = ACUtility.CellTypes.Default,
                IsEditable = false,
                IsVisible = false
            };

            MCB_LCDView_SCREEN_ChargerButtons_StatusLabel = new ListViewItem()
            {
                Index = 0,
                Text = "MCB_LCDView_SCREEN_ChargerButtons_StatusLabel",
                SubTitle = "",
                DefaultCellType = ACUtility.CellTypes.LabelCenter,
                EditableCellType = ACUtility.CellTypes.LabelCenter,
                IsEditable = false,
                IsVisible = false
            };

            MCB_LCDView_SCREEN_ChargerMain_startStopButton = new ListViewItem()
            {
                Index = 1,
                Title = "Start",
                DefaultCellType = ACUtility.CellTypes.Button,
                EditableCellType = ACUtility.CellTypes.Button,
                ListSelectionCommand = SwitchValueChanged,
                IsEditable = true,
                IsVisible = false
            };

            MCB_LCDView_SCREEN_ChargerMain_exitButton = new ListViewItem()
            {
                Index = 1,
                Title = "Exit",
                DefaultCellType = ACUtility.CellTypes.Button,
                EditableCellType = ACUtility.CellTypes.Button,
                ListSelectionCommand = SwitchValueChanged,
                IsEditable = true,
                IsVisible = false
            };

            MCB_LCDView_SCREEN_ChargerMain_StatusLabel = new ListViewItem()
            {
                Index = 0,
                Text = "Status",
                DefaultCellType = ACUtility.CellTypes.LabelCenter,
                EditableCellType = ACUtility.CellTypes.LabelCenter,
                IsEditable = false,
                IsVisible = false
            };
        }

        /// <summary>
        /// Gets the list selector command.
        /// </summary>
        /// <value>The list selector command.</value>
        public IMvxCommand ListSelectorCommand
        {
            get { return new MvxCommand<ListViewItem>(ExecuteListSelectorCommand); }
        }


        public void ExecuteListSelectorCommand(ListViewItem item)
        {
            if (item.CellType == ACUtility.CellTypes.ListSelector)
            {
                _mListSelector = Mvx.Resolve<IMvxMessenger>().Subscribe<ListSelectorMessage>(OnListSelectorMessage);
                ShowViewModel<ListSelectorViewModel>(new { title = item.Title, type = item.ListSelectorType, selectedItemIndex = item.ParentIndex, selectedChildPosition = ListItemSource[item.ParentIndex].IndexOf(item), ItemSourceStr = JsonConvert.SerializeObject(item.Items) });
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
                ListItemSource[obj.SelectedItemindex][obj.SelectedChildItemindex].Text = obj.SelectedItem;
                ListItemSource[obj.SelectedItemindex][obj.SelectedChildItemindex].SelectedIndex = obj.SelectedIndex;
                RaisePropertyChanged(() => ListItemSource);


            }
        }

        /// <summary>
        /// Gets the switch value changed.
        /// </summary>
        /// <value>The switch value changed.</value>
        public IMvxCommand SwitchValueChanged
        {
            get
            {
                return new MvxCommand<ListViewItem>(ExecuteSwitchValueChanged);
            }
        }

        /// <summary>
        /// Executes the switch value changed.
        /// </summary>
        /// <param name="item">Item.</param>
        async void ExecuteSwitchValueChanged(ListViewItem item)
        {
            if (NetworkCheck())
            {
                if (quickViewTimer != null)
                {
                    quickViewTimer.Dispose();
                    quickViewTimer = null;
                }

                //if (!item.IsFailed)
                //{
                if (!MCBQuantum.Instance.MCB_quickSerialNumberCheckStrict())
                    return;
                bool okz = false;
                LcdRequest req = new LcdRequest();
                if (item.CellType == ACUtility.CellTypes.LabelSwitch)
                {
                    if (item.Title == "EQ")
                    {
                        okz = true;
                        if (MCB_LCDView_SCREEN_ChargerMain_EQControl.Text == "Enable EQ")
                        {
                            req.EQRequest = 1;
                        }
                        else
                        {
                            req.EQRequest = 2;
                        }
                    }
                    else if (item.Title == "Desulphate")
                    {
                        if (item.IsSwitchEnabled)
                        {
                            okz = true;

                            req.desulfateRequest = 1;
                        }
                        else
                        {
                            okz = true;

                            req.desulfateRequest = 2;
                        }
                        okz = true;
                    }
                }
                else if (item.CellType == ACUtility.CellTypes.Button)
                {
                    if (item.Title == "Save")
                    {
                        VerifyControl v = new VerifyControl();
                        v.VerifyComboBox(MCB_LCDView_SCREEN_ChargerMain_desulfateVoltage);
                        v.VerifyComboBox(MCB_LCDView_SCREEN_ChargerMain_desulfateDuration);
                        v.VerifyInteger(MCB_LCDView_SCREEN_ChargerMain_desulfateCapacity, MCB_LCDView_SCREEN_ChargerMain_desulfateCapacity, 50, 5000);
                        if (v.HasErrors())
                        {
                            ACUserDialogs.ShowAlert(v.GetErrorString());
                            return;
                        }
                        req.desulfateRequest = 3;
                        req.desulfateVoltage = (byte)MCB_LCDView_SCREEN_ChargerMain_desulfateVoltage.SelectedIndex;
                        req.desulfateCapacity = UInt16.Parse(MCB_LCDView_SCREEN_ChargerMain_desulfateCapacity.Text);
                        req.desulfateLength = (UInt32)(24 + MCB_LCDView_SCREEN_ChargerMain_desulfateDuration.SelectedIndex) * 15 * 60;
                        okz = true;
                    }
                    if (item.Title == "Exit")
                    {
                        req.chargeScreenExit = 1;
                        okz = true;

                    }
                    else if (MCB_LCDView_SCREEN_ChargerMain_startStopButton.Title == "Start" || MCB_LCDView_SCREEN_ChargerMain_startStopButton.Title == "Stop" || MCB_LCDView_SCREEN_ChargerMain_startStopButton.Title == "Return")
                    {
                        if (MCB_lcdChargeScreenButtonVal == 0)
                        {

                        }
                        if (MCB_LCDView_SCREEN_ChargerMain_startStopButton.Title == "Stop")
                        //if (MCB_lcdChargeScreenButtonVal == 1)
                        {
                            req.chargeScreenStop = 1;
                            okz = true;

                        }
                        else if (MCB_LCDView_SCREEN_ChargerMain_startStopButton.Title == "Start")

                        //if (MCB_lcdChargeScreenButtonVal == 2)
                        {
                            req.chargeScreenStart = 1;
                            okz = true;

                        }
                        else if (MCB_LCDView_SCREEN_ChargerMain_startStopButton.Title == "Return")
                        //if (MCB_lcdChargeScreenButtonVal == 3)
                        {
                            req.chargeScreenReturn = 1;
                            okz = true;

                        }

                    }
                    else if (MCB_LCDView_SCREEN_ChargerMain_startStopButton.Title == "Resume")
                    {
                        req.chargeScreenResume = 1;
                        okz = true;

                    }

                }

                if (!okz)
                    return;

                List<object> arguments = new List<object>();
                arguments.Add(McbCommunicationTypes.lcdReq);
                arguments.Add(req);

                ACUserDialogs.ShowProgress();

                List<object> results = await MCBQuantum.Instance.CommunicateMCB(arguments);

                ACUserDialogs.HideProgress();

                if (quickViewTimer == null)
                {
                    quickViewTimer = new ACTimer(RefreshQuickView, null, ACConstants.QUICKVIEW_REFRESH_INTERVAL, ACConstants.QUICKVIEW_REFRESH_INTERVAL);
                }

            }
        }

        public override void OnBackButtonClick()
        {
            base.OnBackButtonClick();

            ClearTimer();

            ShowViewModel<QuickViewChargerViewModel>(new { pop = "pop" });
        }

        public void ClearTimer()
        {
            if (quickViewTimer != null)
            {
                quickViewTimer.Cancel();
                quickViewTimer.Dispose();
                quickViewTimer = null;
            }
        }


        public void CreateData()
        {
            MCB_ResetLCDView();

            List<object> arguments = new List<object>();
            arguments.Add(McbCommunicationTypes.lcdView);
            arguments.Add(null);

            Task.Run(async () =>
            {
                ACUserDialogs.ShowProgress();

                var result = await MCBQuantum.Instance.CommunicateMCB(arguments);

                if (result.Count > 0)
                {
                    if (result[2].Equals(CommunicationResult.OK))
                    {
                        try
                        {
                            MCB_LoadLCDView();
                            MCBQuantum.Instance.GetConnectionManager().siteView.updateMCBLCDSimulator(MCBQuantum.Instance.GetConnectionManager().getWorkingSerialNumber(), MCBQuantum.Instance.GetConnectionManager().activeMCB.getLCDInfo());
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine(ex.Message);
                        }
                    }
                }
                else
                {
                    ACUserDialogs.ShowAlert(AppResources.please_try_again);
                }

                ACUserDialogs.HideProgress();

                quickViewTimer = new ACTimer(RefreshQuickView, null, ACConstants.QUICKVIEW_REFRESH_INTERVAL, ACConstants.QUICKVIEW_REFRESH_INTERVAL);
            });
        }

        async void RefreshQuickView(object state)
        {
            await Tick();
        }

        public async Task Tick()
        {
            isLithiumAnd2_5OrAbove = McbHelper.IsLithiumAnd2_5OrAbove(activeMcb);

            InitItems();
            MCB_ResetLCDView();

            List<object> arguments = new List<object>();
            arguments.Add(McbCommunicationTypes.lcdView);
            arguments.Add(null);

            var result = await MCBQuantum.Instance.CommunicateMCB(arguments);

            if (result.Count > 0)
            {
                if (result[2].Equals(CommunicationResult.OK))
                {
                    try
                    {
                        MCB_LoadLCDView();
                        //MCB_LoadLCDView(true);
                        MCBQuantum.Instance.GetConnectionManager().siteView.updateMCBLCDSimulator(MCBQuantum.Instance.GetConnectionManager().getWorkingSerialNumber(), MCBQuantum.Instance.GetConnectionManager().activeMCB.getLCDInfo());

                        Debug.WriteLine("Timer Completed");
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }
                }
            }
        }

        byte MCB_ViewLCDLastEnergyManagment;
        int MCB_ViewLCDChargerScreenID;
        int MCB_ViewLCDLasvoltageRangeMessageId;
        byte MCB_ViewLCDLastSIMMode;
        byte MCB_ViewLCDLastSOC;
        bool MCB_ViewLCDLasttemperatureConnected_p;
        Int16 MCB_ViewLCDLasttemperatureVal;
        UInt16 MCB_ViewLCDLastvoltageVal;
        UInt32 MCB_ViewLCDLastduration;
        UInt32 MCB_ViewLCDLastcurrent;
        UInt32 MCB_ViewLCDLastAS;
        UInt32 MCB_ViewLCDLastWS;
        byte MCB_ViewLCDLastrunningProfile;
        bool MCB_ViewLCDLastChargingStatus;
        bool MCB_ViewLCDLastshowLastChargeCycleDetails_p;
        byte MCB_lcdChargeScreenButtonVal;

        void MCB_ResetLCDView()
        {
            MCB_LCDView_SCREEN_ChargerButtons_StatusLabel.SubTitle = "";
            MCB_LCDView_SCREEN_ChargerButtons_StatusLabel.IsVisible = false;

            MCB_ViewLCDLastEnergyManagment = 0xFF;
            MCB_ViewLCDChargerScreenID = -1;
            MCB_ViewLCDLastSIMMode = 0x22;
            MCB_ViewLCDLastSOC = 0;
            MCB_ViewLCDLasttemperatureConnected_p = false;
            MCB_ViewLCDLasttemperatureVal = 0;
            MCB_ViewLCDLastvoltageVal = 0;
            MCB_ViewLCDLastduration = 0;
            MCB_ViewLCDLastcurrent = 0;

            MCB_ViewLCDLastAS = 0;
            MCB_ViewLCDLastWS = 0;
            MCB_ViewLCDLastrunningProfile = 0;
            MCB_ViewLCDLastChargingStatus = false;

            MCB_ViewLCDLasvoltageRangeMessageId = 1;
            MCB_ViewLCDLastChargingStatus = false;
            MCB_ViewLCDLastshowLastChargeCycleDetails_p = false;
            MCB_lcdChargeScreenButtonVal = 0;

            MCB_LCDView_SCREEN_Charger_Ready_panel.IsVisible = false;
            MCB_LCDView_SCREEN_ChargerMain_EQControl.IsVisible = false;
        }

        void MCB_LoadLCDView(bool refresh = false)
        {
            if (!MCBQuantum.Instance.MCB_quickSerialNumberCheckStrict())
                return;

            MCB_LCDView_SCREEN_ChargerButtons_StatusLabel.SubTitle = "";
            MCB_LCDView_SCREEN_ChargerButtons_StatusLabel.IsVisible = false;

            MCB_LCDView_SCREEN_ChargerMain_desulfateEnable.SwitchValueChanged = null;
            MCB_LCDView_SCREEN_ChargerMain_EQControl.SwitchValueChanged = null;

            LcdSimulator lcdsim = activeMcb.getLCDInfo();

            if (isLithiumAnd2_5OrAbove)
            {
                MCB_LCDView_SCREEN_EQ_panel.IsVisible = false;
            }
            else if (!MCBQuantum.Instance.GetMCB().Config.enableManualEQ && ControlObject.UserAccess.MCB_enableLauncher != AccessLevelConsts.write)
            {
                // Hide the EnableEQ Button.
                MCB_LCDView_SCREEN_EQ_panel.IsVisible = false;
                MCB_LCDView_SCREEN_ChargerMain_EQControl.IsVisible = false;
            }
            else
            {
                //EQ control enabled??
                if (lcdsim.simulationMode > 0 || lcdsim.batteryCharging_p || lcdsim.batteryDisCharging_p || lcdsim.EQ_p == 2)
                {
                    MCB_LCDView_SCREEN_ChargerMain_EQControl.IsVisible = false;
                    MCB_LCDView_SCREEN_EQ_panel.IsVisible = false;
                }
                else
                {
                    MCB_LCDView_SCREEN_ChargerMain_EQControl.IsVisible = true;
                    MCB_LCDView_SCREEN_EQ_panel.IsVisible = true;
                    if (lcdsim.EQ_p == 1)
                    {
                        if (MCB_LCDView_SCREEN_ChargerMain_EQControl.Text != "Disable EQ")
                        {
                            MCB_LCDView_SCREEN_ChargerMain_EQControl.IsSwitchEnabled = true;
                            MCB_LCDView_SCREEN_ChargerMain_EQControl.Text = "Disable EQ";
                            MCB_LCDView_SCREEN_ChargerMain_EQControl.ForeColor = Color.Wheat;
                        }

                    }
                    else if (lcdsim.EQ_p == 0)
                    {
                        if (MCB_LCDView_SCREEN_ChargerMain_EQControl.Text != "Enable EQ")
                        {
                            MCB_LCDView_SCREEN_ChargerMain_EQControl.IsSwitchEnabled = false;
                            MCB_LCDView_SCREEN_ChargerMain_EQControl.Text = "Enable EQ";
                            MCB_LCDView_SCREEN_ChargerMain_EQControl.ForeColor = Color.White;
                        }

                    }

                    if (refresh)
                    {
                        if (!MCB_LCDView_SCREEN_ChargerMain_EQControl.IsVisible)
                        {
                            var item = MCB_LCDView_SCREEN_EQ_panel.FirstOrDefault(o => o.Title == MCB_LCDView_SCREEN_ChargerMain_EQControl.Title);
                            if (item != null)
                            {
                                MCB_LCDView_SCREEN_EQ_panel.Remove(item);
                            }
                        }
                        else
                        {
                            var item = MCB_LCDView_SCREEN_EQ_panel.FirstOrDefault(o => o.Title == MCB_LCDView_SCREEN_ChargerMain_EQControl.Title);
                            if (item == null)
                            {
                                MCB_LCDView_SCREEN_EQ_panel.Add(MCB_LCDView_SCREEN_ChargerMain_EQControl);
                            }
                        }
                    }
                    else
                    {
                        if (MCB_LCDView_SCREEN_ChargerMain_EQControl.IsVisible)
                        {
                            MCB_LCDView_SCREEN_EQ_panel.Add(MCB_LCDView_SCREEN_ChargerMain_EQControl);
                        }
                    }
                }
            }

            if (!MCBQuantum.Instance.GetMCB().Config.enableManualDesulfate && ControlObject.UserAccess.MCB_enableLauncher != AccessLevelConsts.write)
            {
                MCB_LCDView_SCREEN_ChargerMain_desulfateEnable.IsVisible = false;
            }
            else
            {
                if (lcdsim.simulationMode > 0 || lcdsim.batteryCharging_p || lcdsim.batteryDisCharging_p)
                {
                    MCB_LCDView_SCREEN_ChargerMain_desulfateEnable.IsVisible = false;
                }
                else
                {
                    MCB_LCDView_SCREEN_ChargerMain_desulfateEnable.IsVisible = true;

                    if (refresh)
                    {
                        if (!MCB_LCDView_SCREEN_ChargerMain_desulfateEnable.IsVisible)
                        {
                            var item = MCB_LCDView_SCREEN_EQ_panel.FirstOrDefault(o => o.Title == MCB_LCDView_SCREEN_ChargerMain_desulfateEnable.Title);
                            if (item != null)
                            {
                                MCB_LCDView_SCREEN_EQ_panel.Remove(item);
                            }
                        }
                        else
                        {
                            var item = MCB_LCDView_SCREEN_EQ_panel.FirstOrDefault(o => o.Title == MCB_LCDView_SCREEN_ChargerMain_desulfateEnable.Title);
                            if (item == null)
                            {
                                MCB_LCDView_SCREEN_EQ_panel.Add(MCB_LCDView_SCREEN_ChargerMain_desulfateEnable);
                            }
                        }
                    }
                    else
                    {
                        if (MCB_LCDView_SCREEN_ChargerMain_desulfateEnable.IsVisible)
                        {
                            MCB_LCDView_SCREEN_EQ_panel.Add(MCB_LCDView_SCREEN_ChargerMain_desulfateEnable);
                        }
                    }


                    if (!lcdsim.desulfate_p)
                    {
                        if (MCB_LCDView_SCREEN_ChargerMain_desulfateEnable.IsSwitchEnabled)
                        {
                            MCB_LCDView_SCREEN_ChargerMain_desulfateEnable.IsSwitchEnabled = false;
                        }

                        MCB_LCDView_SCREEN_ChargerMain_desulfateVoltage.IsVisible = false;
                        MCB_LCDView_SCREEN_ChargerMain_desulfateDuration.IsVisible = false;
                        MCB_LCDView_SCREEN_ChargerMain_desulfateCapacity.IsVisible = false;
                    }
                    else
                    {
                        if (!MCB_LCDView_SCREEN_ChargerMain_desulfateEnable.IsSwitchEnabled)
                        {
                            MCB_LCDView_SCREEN_ChargerMain_desulfateEnable.IsSwitchEnabled = true;
                        }
                        if (lcdsim.batviewOn != 0)
                        {
                            MCB_LCDView_SCREEN_ChargerMain_desulfateVoltage.IsVisible = false;
                            MCB_LCDView_SCREEN_ChargerMain_desulfateDuration.IsVisible = false;
                            MCB_LCDView_SCREEN_ChargerMain_desulfateCapacity.IsVisible = false;
                        }
                        else if (!MCB_LCDView_SCREEN_ChargerMain_desulfateVoltage.IsVisible)
                        {

                            MCB_LCDView_SCREEN_ChargerMain_desulfateVoltage.IsVisible = true;
                            MCB_LCDView_SCREEN_ChargerMain_desulfateDuration.IsVisible = true;
                            MCB_LCDView_SCREEN_ChargerMain_desulfateCapacity.IsVisible = true;

                            MCB_LCDView_SCREEN_ChargerMain_desulfateVoltage.Items = new List<object>();
                            if (MCBQuantum.Instance.GetMCB().FirmwareRevision > 2.10f)
                            {
                                for (int i = 24; i <= int.Parse(MCBQuantum.Instance.GetMCB().Config.PMvoltage); i += 2)
                                    MCB_LCDView_SCREEN_ChargerMain_desulfateVoltage.Items.Add(i.ToString());
                            }
                            else
                            {
                                MCB_LCDView_SCREEN_ChargerMain_desulfateVoltage.Items.Add("24");
                                MCB_LCDView_SCREEN_ChargerMain_desulfateVoltage.Items.Add("36");
                                if (MCBQuantum.Instance.GetMCB().Config.PMvoltage == "48")
                                    MCB_LCDView_SCREEN_ChargerMain_desulfateVoltage.Items.Add("48");
                            }

                            if (lcdsim.DesulfationVoltage_ToUse < MCB_LCDView_SCREEN_ChargerMain_desulfateVoltage.Items.Count)
                            {
                                MCB_LCDView_SCREEN_ChargerMain_desulfateVoltage.SelectedIndex = lcdsim.DesulfationVoltage_ToUse;
                                MCB_LCDView_SCREEN_ChargerMain_desulfateVoltage.SelectedItem = MCB_LCDView_SCREEN_ChargerMain_desulfateVoltage.Items[MCB_LCDView_SCREEN_ChargerMain_desulfateVoltage.SelectedIndex].ToString();
                            }

                            MCB_LCDView_SCREEN_ChargerMain_desulfateCapacity.Text = lcdsim.DesulfationCapacity_ToUse.ToString();
                            UInt32 len = lcdsim.DesulfationTimer_ToUse;
                            len /= (15 * 60);
                            len -= (6 * 4);
                            MCB_LCDView_SCREEN_ChargerMain_desulfateDuration.Items = new List<object>();
                            MCB_LCDView_SCREEN_ChargerMain_desulfateDuration.Items.AddRange(new object[] {
            "06:00",
            "06:15",
            "06:30",
            "06:45",
            "07:00",
            "07:15",
            "07:30",
            "07:45",
            "07:00",
            "08:15",
            "08:30",
            "08:45",
            "09:00",
            "09:15",
            "09:30",
            "09:45",
            "10:00",
            "10:15",
            "10:30",
            "10:45",
            "11:00",
            "11:15",
            "11:30",
            "11:45",
            "12:00",
            "12:15",
            "12:30",
            "12:45",
            "12:00",
            "13:15",
            "13:30",
            "13:45",
            "14:00",
            "14:15",
            "14:30",
            "14:45",
            "15:00",
            "15:15",
            "15:30",
            "15:45",
            "16:00",
            "16:15",
            "16:30",
            "16:45",
            "17:00",
            "17:15",
            "17:30",
            "17:45",
            "18:00"});

                            if (MCB_LCDView_SCREEN_ChargerMain_desulfateDuration.SelectedIndex != len)
                            {
                                MCB_LCDView_SCREEN_ChargerMain_desulfateDuration.SelectedIndex = (int)len;
                                MCB_LCDView_SCREEN_ChargerMain_desulfateDuration.SelectedItem = MCB_LCDView_SCREEN_ChargerMain_desulfateDuration.Items[MCB_LCDView_SCREEN_ChargerMain_desulfateDuration.SelectedIndex].ToString();
                            }
                            VerifyControl v = new VerifyControl();
                            v.VerifyComboBox(MCB_LCDView_SCREEN_ChargerMain_desulfateVoltage);
                            v.VerifyComboBox(MCB_LCDView_SCREEN_ChargerMain_desulfateDuration);
                            v.VerifyInteger(MCB_LCDView_SCREEN_ChargerMain_desulfateCapacity, MCB_LCDView_SCREEN_ChargerMain_desulfateCapacity, 50, 5000);
                        }
                    }

                    if (refresh)
                    {
                        if (!MCB_LCDView_SCREEN_ChargerMain_desulfateVoltage.IsVisible)
                        {
                            var item = MCB_LCDView_SCREEN_EQ_panel.FirstOrDefault(o => o.Title == MCB_LCDView_SCREEN_ChargerMain_desulfateVoltage.Title);
                            if (item != null)
                            {
                                MCB_LCDView_SCREEN_EQ_panel.Remove(item);
                                MCB_LCDView_SCREEN_EQ_panel.Remove(MCB_LCDView_SCREEN_ChargerMain_desulfateDuration);
                                MCB_LCDView_SCREEN_EQ_panel.Remove(MCB_LCDView_SCREEN_ChargerMain_desulfateCapacity);
                                MCB_LCDView_SCREEN_EQ_panel.Remove(MCB_LCDView_SCREEN_ChargerMain_desulfatSaveBtn);
                            }
                        }
                        else
                        {
                            var item = MCB_LCDView_SCREEN_EQ_panel.FirstOrDefault(o => o.Title == MCB_LCDView_SCREEN_ChargerMain_desulfateVoltage.Title);
                            if (item == null)
                            {
                                MCB_LCDView_SCREEN_EQ_panel.Add(MCB_LCDView_SCREEN_ChargerMain_desulfateVoltage);
                                MCB_LCDView_SCREEN_EQ_panel.Add(MCB_LCDView_SCREEN_ChargerMain_desulfateDuration);
                                MCB_LCDView_SCREEN_EQ_panel.Add(MCB_LCDView_SCREEN_ChargerMain_desulfateCapacity);
                                MCB_LCDView_SCREEN_EQ_panel.Add(MCB_LCDView_SCREEN_ChargerMain_desulfatSaveBtn);
                            }
                        }
                    }
                    else
                    {
                        if (MCB_LCDView_SCREEN_ChargerMain_desulfateVoltage.IsVisible)
                        {
                            MCB_LCDView_SCREEN_EQ_panel.Add(MCB_LCDView_SCREEN_ChargerMain_desulfateVoltage);
                            MCB_LCDView_SCREEN_EQ_panel.Add(MCB_LCDView_SCREEN_ChargerMain_desulfateDuration);
                            MCB_LCDView_SCREEN_EQ_panel.Add(MCB_LCDView_SCREEN_ChargerMain_desulfateCapacity);
                            MCB_LCDView_SCREEN_EQ_panel.Add(MCB_LCDView_SCREEN_ChargerMain_desulfatSaveBtn);
                        }

                    }
                }
            }

            MCB_LCDView_SCREEN_ChargerMain_desulfateEnable.SwitchValueChanged = SwitchValueChanged;
            MCB_LCDView_SCREEN_ChargerMain_EQControl.SwitchValueChanged = SwitchValueChanged;

            if (!refresh)
            {
                if (MCB_LCDView_SCREEN_EQ_panel.IsVisible)
                {
                    ListItemSource.Add(MCB_LCDView_SCREEN_EQ_panel);
                }
            }
            else
            {
                if (!MCB_LCDView_SCREEN_EQ_panel.IsVisible)
                {
                    var item = ListItemSource.FirstOrDefault(o => o.ID == MCB_LCDView_SCREEN_EQ_panel.ID);
                    if (item != null)
                    {
                        ListItemSource.Remove(item);
                    }
                }
                else
                {
                    var item = ListItemSource.FirstOrDefault(o => o.ID == MCB_LCDView_SCREEN_EQ_panel.ID);
                    if (item == null)
                    {
                        ListItemSource.Insert(0, MCB_LCDView_SCREEN_EQ_panel);
                    }
                }
            }

            int chargerScreen = -1;
            if (lcdsim.voltageRangeStatus == 1 || !lcdsim.battConnected_p)
            {
                if (lcdsim.simulationMode > 0)
                {
                    chargerScreen = 0;
                }
                else if (!lcdsim.battConnected_p && !lcdsim.showLastChargeCycleDetails_p)
                {
                    chargerScreen = 1;
                }
                else
                {
                    chargerScreen = 2;
                }
            }
            else
            {
                chargerScreen = 3;
            }
            bool forceRefresh = false;
            Random r = new Random();

            if (chargerScreen != MCB_ViewLCDChargerScreenID || r.Next(0, 1000) > 900 || MCB_ViewLCDLastChargingStatus != lcdsim.batteryCharging_p || lcdsim.showLastChargeCycleDetails_p != MCB_ViewLCDLastshowLastChargeCycleDetails_p)
            {
                MCB_ViewLCDLastChargingStatus = lcdsim.batteryCharging_p;
                MCB_ViewLCDLastshowLastChargeCycleDetails_p = lcdsim.showLastChargeCycleDetails_p;
                forceRefresh = true;

                MCB_ViewLCDLasvoltageRangeMessageId = 1;
                switch (chargerScreen)
                {
                    case 0:
                        MCB_LCDView_SCREEN_simulation_mode_panel.IsVisible = true;
                        MCB_LCDView_SCREEN_simulation_mode_panel.Clear();

                        break;
                    case 1:
                        MCB_LCDView_SCREEN_simulation_mode_panel.IsVisible = false;
                        MCB_LCDView_SCREEN_Faulty_Screen_panel.IsVisible = false;
                        MCB_LCDView_SCREEN_Charger_Ready_panel_ListItem.IsVisible = true;

                        // Assign Image for the Connect Battery Indication

                        if (refresh)
                        {
                            var item = MCB_LCDView_SCREEN_Charger_Ready_panel.FirstOrDefault(o => o.Title == MCB_LCDView_SCREEN_Charger_Ready_panel_ListItem.Title);
                            if (item == null)
                            {
                                MCB_LCDView_SCREEN_Charger_Ready_panel.Clear();
                                MCB_LCDView_SCREEN_Charger_Ready_panel.Add(MCB_LCDView_SCREEN_Charger_Ready_panel_ListItem);
                            }
                        }
                        else
                        {
                            MCB_LCDView_SCREEN_Charger_Ready_panel.Add(MCB_LCDView_SCREEN_Charger_Ready_panel_ListItem);
                            ListItemSource.Add(MCB_LCDView_SCREEN_Charger_Ready_panel);
                        }

                        break;
                    case 2:
                        MCB_LCDView_SCREEN_simulation_mode_panel.IsVisible = false;
                        MCB_LCDView_SCREEN_Faulty_Screen_panel.IsVisible = false;
                        MCB_LCDView_SCREEN_ChargerMain_panel.IsVisible = true;

                        if (refresh)
                        {

                        }
                        else
                        {
                            ListItemSource.Add(MCB_LCDView_SCREEN_Charger_Ready_panel);
                        }

                        break;
                    case 3:
                        MCB_LCDView_SCREEN_Faulty_Screen_panel.IsVisible = true;
                        MCB_LCDView_SCREEN_Faulty_Screen_panel.Clear();

                        break;
                }
                MCB_ViewLCDChargerScreenID = chargerScreen;
            }

            if (!refresh)
            {
                if (MCB_LCDView_SCREEN_simulation_mode_panel.IsVisible)
                {
                    ListItemSource.Add(MCB_LCDView_SCREEN_simulation_mode_panel);

                }

            }
            else
            {
                if (!MCB_LCDView_SCREEN_simulation_mode_panel.IsVisible)
                {
                    var item = ListItemSource.FirstOrDefault(o => o.ID == MCB_LCDView_SCREEN_simulation_mode_panel.ID);
                    if (item != null)
                    {
                        ListItemSource.Remove(item);
                    }
                }
                else
                {
                    var item = ListItemSource.FirstOrDefault(o => o.ID == MCB_LCDView_SCREEN_simulation_mode_panel.ID);
                    if (item == null)
                    {
                        ListItemSource.Add(MCB_LCDView_SCREEN_simulation_mode_panel);
                    }
                }

            }

            if (!refresh)
            {
                if (MCB_LCDView_SCREEN_Faulty_Screen_panel.IsVisible)
                {
                    ListItemSource.Add(MCB_LCDView_SCREEN_Faulty_Screen_panel);

                }

            }
            else
            {
                if (!MCB_LCDView_SCREEN_Faulty_Screen_panel.IsVisible)
                {
                    var item = ListItemSource.FirstOrDefault(o => o.ID == MCB_LCDView_SCREEN_Faulty_Screen_panel.ID);
                    if (item != null)
                    {
                        ListItemSource.Remove(item);
                    }
                }
                else
                {
                    var item = ListItemSource.FirstOrDefault(o => o.ID == MCB_LCDView_SCREEN_Faulty_Screen_panel.ID);
                    if (item == null)
                    {
                        ListItemSource.Add(MCB_LCDView_SCREEN_Faulty_Screen_panel);
                    }
                }

            }


            if (chargerScreen == 0)
            {
                //SIM MODE

                if (lcdsim.simulationMode != MCB_ViewLCDLastSIMMode || forceRefresh)
                {
                    MCB_ViewLCDLastSIMMode = lcdsim.simulationMode;
                    switch (MCB_ViewLCDLastSIMMode)
                    {
                        case 1:
                            MCB_LCDView_SCREEN_simulation_mode_TextLabel.Text = "Simulation Mode";
                            break;
                        case 2:
                            MCB_LCDView_SCREEN_simulation_mode_TextLabel.Text = "Burn In";
                            break;
                        case 3:
                            MCB_LCDView_SCREEN_simulation_mode_TextLabel.Text = "Battview Calibration";
                            break;

                    }
                    MCB_LCDView_SCREEN_simulation_mode_panel.Add(MCB_LCDView_SCREEN_simulation_mode_TextLabel);
                }
            }

            string buttonsText = "";

            if (chargerScreen == 3)
            {
                if (lcdsim.voltageRangeStatus != MCB_ViewLCDLasvoltageRangeMessageId || forceRefresh)
                {
                    if (lcdsim.voltageRangeStatus != 0 && lcdsim.voltageRangeStatus != 1 && lcdsim.battConnected_p)
                    {
                        RemoveButtons();

                        switch (lcdsim.voltageRangeStatus)
                        {
                            case 0xB0:
                                MCB_LCDView_SCREEN_Faulty_Label.SubTitle = "BMS not detected";

                                break;

                            case 0xA6:
                                MCB_LCDView_SCREEN_Faulty_Label.SubTitle = "BMS daughter card error";

                                break;

                            case 0x9C:
                                MCB_LCDView_SCREEN_Faulty_Label.SubTitle = "BMS daughter card error";

                                break;

                            default:
                                MCB_LCDView_SCREEN_Faulty_Label.SubTitle = "Incorrect Battery Voltage";

                                break;
                        }

                    }

                    MCB_ViewLCDLasvoltageRangeMessageId = lcdsim.voltageRangeStatus;

                    MCB_LCDView_SCREEN_Faulty_Screen_panel.Add(MCB_LCDView_SCREEN_Faulty_Label);
                }
            }

            if (chargerScreen == 2)
            {
                bool drawSOC = false;
                bool drawDuration = false;
                bool drawVoltage = false;
                bool drawTemperature = false;
                bool drawCurrent = false;
                bool drawAHR = false;
                bool drawRunningProfile = false;
                bool drawExitStatus = false;
                bool draw_WATTS = false;
                double factor;

                UInt32 duration = lcdsim.duration;

                if (forceRefresh ||
                    (!lcdsim.showLastChargeCycleDetails_p && !lcdsim.batteryCharging_p && MCB_ViewLCDLastEnergyManagment != lcdsim.energyManagmentType))
                {
                    drawSOC = true;
                    drawVoltage = true;
                    drawTemperature = true;
                    drawDuration = true;
                    drawCurrent = true;
                    drawAHR = true;
                    drawRunningProfile = true;
                    draw_WATTS = true;

                    //HIDE SHOIW......

                    MCB_LCDView_SCREEN_ChargerMain_exitButton.IsVisible = false;
                    if (lcdsim.batteryCharging_p)
                    {
                        MCB_LCDView_SCREEN_ChargerMain_startStopButton.IsVisible = true;
                        MCB_lcdChargeScreenButtonVal = 1;
                        MCB_LCDView_SCREEN_ChargerMain_startStopButton.Title = "Stop";
                    }
                    else
                    {
                        MCB_lcdChargeScreenButtonVal = 0;
                        drawRunningProfile = false;

                        if (!lcdsim.showLastChargeCycleDetails_p)
                        {
                            duration = 0;
                            drawAHR = false;
                            draw_WATTS = false;
                            if (lcdsim.energyManagmentType != 1)
                            {
                                MCB_LCDView_SCREEN_ChargerMain_startStopButton.IsVisible = true;

                                MCB_lcdChargeScreenButtonVal = 2;
                                MCB_LCDView_SCREEN_ChargerMain_startStopButton.Title = "Start";

                            }
                            else
                            {
                                MCB_LCDView_SCREEN_ChargerMain_startStopButton.IsVisible = false;

                                MCB_LCDView_SCREEN_ChargerMain_startStopButton.Title = "Start";
                            }
                        }
                        else
                        {
                            if (lcdsim.paused)
                            {
                                MCB_LCDView_SCREEN_ChargerMain_startStopButton.IsVisible = true;
                                MCB_LCDView_SCREEN_ChargerMain_startStopButton.Title = "Resume";
                                //Add exit button also
                                MCB_LCDView_SCREEN_ChargerMain_exitButton.IsVisible = true;
                            }
                            else
                            {
                                drawExitStatus = true;
                                MCB_LCDView_SCREEN_ChargerMain_startStopButton.IsVisible = true;
                                MCB_lcdChargeScreenButtonVal = 3;

                                MCB_LCDView_SCREEN_ChargerMain_startStopButton.Title = "Return";
                            }
                        }
                    }
                }
                else
                {
                    if (lcdsim.batteryCharging_p || !lcdsim.showLastChargeCycleDetails_p)
                    {
                        if (MCB_ViewLCDLastSOC != lcdsim.currentSOC)
                        {
                            drawSOC = true;
                        }
                        if (lcdsim.temperatureConnected_p != MCB_ViewLCDLasttemperatureConnected_p ||
                            (lcdsim.temperatureConnected_p && (MCB_ViewLCDLasttemperatureVal / 10.0).ToString("N0") != (lcdsim.temperatureVal / 10.0).ToString("N0")))
                            drawTemperature = true;
                        if ((MCB_ViewLCDLastvoltageVal / 100.0).ToString("N1") != (lcdsim.voltageVal / 100.0).ToString("N1"))
                        {
                            drawVoltage = true;
                        }
                    }
                    if (lcdsim.batteryCharging_p)
                    {
                        if (MCB_ViewLCDLastduration != duration)
                        {
                            drawDuration = true;
                        }
                        if (MCB_ViewLCDLastcurrent != lcdsim.current)
                        {
                            drawCurrent = true;
                        }
                        if ((UInt32)(lcdsim.AS / 360.0) != (UInt32)(MCB_ViewLCDLastAS / 360.0f))
                        {
                            drawAHR = true;
                        }
                        if (MCB_ViewLCDLastrunningProfile != lcdsim.currentRunningProfile)
                            drawRunningProfile = true;
                        if ((UInt32)(lcdsim.WS / 360000.0) != (UInt32)(MCB_ViewLCDLastWS / 360000.0))
                        {
                            draw_WATTS = true;
                        }
                    }
                }

                if (drawAHR)
                {
                    MCB_LCDView_SCREEN_ChargerValues_panel.SubTitle = (lcdsim.AS / 3600.0).ToString("N1");// + " AHR";
                }
                else if (forceRefresh)
                {
                }
                if (draw_WATTS)
                {
                    MCB_LCDView_SCREEN_ChargerValues_panel.SubTitle2 = (lcdsim.WS / 3600000.0).ToString("N1");// + " KWHr";
                }
                else if (forceRefresh)
                {
                }

                if (drawDuration)
                {
                    MCB_LCDView_SCREEN_ChargerValues_panel.SubTitle3 = string.Format("{0:0}", lcdsim.duration / 3600);
                    MCB_LCDView_SCREEN_ChargerValues_panel.Text = string.Format("{0:00}", ((lcdsim.duration % 3600) / 60)) + " min";


                    MCB_LCDView_SCREEN_ChargerValues_panel.Seconds = string.Format("{0:00}", (lcdsim.duration % 60)) + " sec";
                }

                MCB_LCDView_SCREEN_Charger_Ready_panel.Clear();

                if (drawAHR)
                {
                    if (refresh)
                    {

                        var item = MCB_LCDView_SCREEN_Charger_Ready_panel.FirstOrDefault(o => o.Title == MCB_LCDView_SCREEN_ChargerValues_panel.Title);
                        if (item == null)
                        {
                            MCB_LCDView_SCREEN_Charger_Ready_panel.Insert(0, MCB_LCDView_SCREEN_ChargerValues_panel);
                        }
                    }
                    else
                    {
                        MCB_LCDView_SCREEN_Charger_Ready_panel.Insert(0, MCB_LCDView_SCREEN_ChargerValues_panel);
                    }
                }

                if (MCB_LCDView_SCREEN_ChargerMain_panel.IsVisible)
                {
                    MCB_LCDView_SCREEN_ChargerMain_panel = new ListViewItem()
                    {
                        Title = "Plots",
                        Index = 0,
                        DefaultCellType = ACUtility.CellTypes.QuickViewPlotCollection,
                        EditableCellType = ACUtility.CellTypes.QuickViewPlotCollection,
                        IsEditable = false,
                        ParentIndex = 1,
                        IsVisible = false
                    };

                    MCB_LCDView_SCREEN_Charger_Ready_panel.Add(MCB_LCDView_SCREEN_ChargerMain_panel);
                }

                if (refresh)
                {
                    if (!MCB_LCDView_SCREEN_ChargerMain_startStopButton.IsVisible)
                    {
                        var item = MCB_LCDView_SCREEN_Charger_Ready_panel.FirstOrDefault(o => o.Title == MCB_LCDView_SCREEN_ChargerMain_startStopButton.Title);
                        if (item != null)
                        {
                            MCB_LCDView_SCREEN_Charger_Ready_panel.Remove(item);
                        }
                    }
                    else
                    {
                        var item = MCB_LCDView_SCREEN_Charger_Ready_panel.FirstOrDefault(o => o.Title == MCB_LCDView_SCREEN_ChargerMain_startStopButton.Title);
                        if (item == null)
                        {
                            MCB_LCDView_SCREEN_Charger_Ready_panel.Add(MCB_LCDView_SCREEN_ChargerMain_startStopButton);
                        }
                    }
                }
                else
                {
                    if (MCB_LCDView_SCREEN_ChargerMain_startStopButton.IsVisible)
                    {
                        MCB_LCDView_SCREEN_Charger_Ready_panel.Add(MCB_LCDView_SCREEN_ChargerMain_startStopButton);
                    }


                }

                if (refresh)
                {
                    if (!MCB_LCDView_SCREEN_ChargerMain_exitButton.IsVisible)
                    {
                        var item = MCB_LCDView_SCREEN_Charger_Ready_panel.FirstOrDefault(o => o.Title == MCB_LCDView_SCREEN_ChargerMain_exitButton.Title);
                        if (item != null)
                        {
                            MCB_LCDView_SCREEN_Charger_Ready_panel.Remove(item);
                        }
                    }
                    else
                    {
                        var item = MCB_LCDView_SCREEN_Charger_Ready_panel.FirstOrDefault(o => o.Title == MCB_LCDView_SCREEN_ChargerMain_exitButton.Title);
                        if (item == null)
                        {
                            MCB_LCDView_SCREEN_Charger_Ready_panel.Add(MCB_LCDView_SCREEN_ChargerMain_exitButton);
                        }
                    }
                }
                else
                {
                    if (MCB_LCDView_SCREEN_ChargerMain_exitButton.IsVisible)
                    {
                        MCB_LCDView_SCREEN_Charger_Ready_panel.Add(MCB_LCDView_SCREEN_ChargerMain_exitButton);
                    }
                }

                MCB_LCDView_SCREEN_ChargerMain_panel.Items = new List<object>();

                double currentSOC = lcdsim.currentSOC;
                double maxSOC = 100;

                double currentCurr = lcdsim.current / 10.0;
                double maxCurr = 100;


                double currentVol = lcdsim.voltageVal / 100.0;
                double maxVol = lcdsim.TRvoltageVal;

                double currentTemp = TemperatureManager.GetCorrectTemperature(lcdsim.temperatureVal, activeMcb.Config.temperatureFormat);
                double maxTemp = (9 * 550 / 50.0 + 32);

                if (drawSOC)
                {
                    MCB_LCDView_SCREEN_ChargerMain_panel.Items.Clear();
                    MCB_LCDView_SCREEN_ChargerMain_panel.Items.Add(GeneratePlotPoints_Soc(lcdsim));
                }

                if (drawCurrent)
                {
                    factor = (9 * lcdsim.current) / (10.0 * int.Parse(MCBQuantum.Instance.GetMCB().Config.numberOfInstalledPMs));

                    var currentText = (lcdsim.current / 10.0).ToString("N0") + " A";
                    MCB_LCDView_SCREEN_ChargerMain_panel.Items.Add(GeneratePlotPoints_Current(lcdsim));
                }

                if (drawVoltage)
                {
                    factor = lcdsim.voltageVal / 100.0;
                    factor /= (lcdsim.batteryVoltage / 2.0);
                    factor -= (lcdsim.TRvoltageVal / 100.0);
                    factor /= ((lcdsim.EQvoltageVal - lcdsim.TRvoltageVal) / 1000.0);
                    factor *= 0.8;

                    MCB_LCDView_SCREEN_ChargerMain_panel.Items.Add(GeneratePlotPoints_Volt(lcdsim));
                }
                if (drawTemperature)
                {
                    MCB_LCDView_SCREEN_ChargerMain_panel.Items.Add(GeneratePlotPoints_Temp(lcdsim));
                }

                if (drawRunningProfile)
                {
                    string status = "";
                    string imageName = "";
                    if ((lcdsim.currentRunningProfile & 0x40) != 0)
                    {
                        imageName = "IMAGE_CHARGE_D";
                        status = "D";
                    }
                    else if ((lcdsim.currentRunningProfile & 0x20) != 0)
                    {
                        imageName = "IMAGE_CHARGE_RF";
                        status = "RF";
                    }
                    else if ((lcdsim.currentRunningProfile & 0x10) != 0)
                    {
                        imageName = "IMAGE_CHARGE_EQ";
                        status = "EQ";
                    }
                    else if ((lcdsim.currentRunningProfile & 0x08) != 0)
                    {
                        imageName = "IMAGE_CHARGE_FI";
                        status = "FI";
                    }
                    else if ((lcdsim.currentRunningProfile & 0x04) != 0)
                    {
                        imageName = "IMAGE_CHARGE_CV";
                        status = "CV";
                    }
                    else if ((lcdsim.currentRunningProfile & 0x01) != 0 || ((lcdsim.currentRunningProfile & 0x02) != 0))
                    {
                        imageName = "IMAGE_CHARGE_CC";
                        status = "CC";
                    }

                    MCB_LCDView_SCREEN_ChargerMain_StatusLabel.Title = status;

                    if (refresh)
                    {
                        var item = MCB_LCDView_SCREEN_Charger_Ready_panel.FirstOrDefault(o => o.Text == MCB_LCDView_SCREEN_ChargerMain_StatusLabel.Text);
                        if (item == null)
                        {
                            MCB_LCDView_SCREEN_Charger_Ready_panel.Insert(1, MCB_LCDView_SCREEN_ChargerMain_StatusLabel);
                        }

                    }
                    else
                    {
                        MCB_LCDView_SCREEN_Charger_Ready_panel.Insert(1, MCB_LCDView_SCREEN_ChargerMain_StatusLabel);
                    }
                }
                else if (drawExitStatus)
                {
                    if (!MCB_LCDView_SCREEN_ChargerMain_StatusLabel.IsVisible)
                        MCB_LCDView_SCREEN_ChargerMain_StatusLabel.IsVisible = true;

                    string status = "";
                    bool error = true;

                    status = ChargeRecord.ExitCodes(lcdsim.status, isLithiumAnd2_5OrAbove, false);

                    MCB_LCDView_SCREEN_ChargerMain_StatusLabel.Title = status;

                    if (error)
                        MCB_LCDView_SCREEN_ChargerMain_StatusLabel.ForeColor = Color.Red;
                    else
                        MCB_LCDView_SCREEN_ChargerMain_StatusLabel.ForeColor = Color.Green;

                    if (refresh)
                    {
                        var item = MCB_LCDView_SCREEN_Charger_Ready_panel.FirstOrDefault(o => o.Text == MCB_LCDView_SCREEN_ChargerMain_StatusLabel.Text);

                        if (item == null)
                        {
                            MCB_LCDView_SCREEN_Charger_Ready_panel.Insert(1, MCB_LCDView_SCREEN_ChargerMain_StatusLabel);
                        }
                    }
                    else
                    {
                        MCB_LCDView_SCREEN_Charger_Ready_panel.Insert(1, MCB_LCDView_SCREEN_ChargerMain_StatusLabel);
                    }
                }

                MCB_ViewLCDLastWS = lcdsim.WS;
                MCB_ViewLCDLastAS = lcdsim.AS;
                MCB_ViewLCDLastrunningProfile = lcdsim.currentRunningProfile;
                MCB_ViewLCDLastcurrent = lcdsim.current;
                MCB_ViewLCDLastSOC = lcdsim.currentSOC;
                MCB_ViewLCDLastduration = duration;
                MCB_ViewLCDLastvoltageVal = lcdsim.voltageVal;
                MCB_ViewLCDLasttemperatureConnected_p = lcdsim.temperatureConnected_p;
                MCB_ViewLCDLasttemperatureVal = lcdsim.temperatureVal;
                MCB_ViewLCDLastEnergyManagment = lcdsim.energyManagmentType;
            }

            buttonsText = "";

            if (lcdsim.voltageRangeStatus != 0 && lcdsim.voltageRangeStatus != 1 && lcdsim.battConnected_p)
            {
                RemoveButtons();

                switch (lcdsim.voltageRangeStatus)
                {
                    case 0xB0:
                        buttonsText = "BMS not detected" + Environment.NewLine;

                        break;

                    case 0xA6:
                        buttonsText = "BMS daughter card error" + Environment.NewLine;

                        break;

                    case 0x9C:
                        buttonsText = "BMS daughter card error" + Environment.NewLine;

                        break;

                    default:
                        buttonsText = "Incorrect Battery Voltage" + Environment.NewLine;

                        break;
                }

                MCB_LCDView_SCREEN_ChargerButtons_StatusLabel.SubTitle = buttonsText.Trim();
                MCB_LCDView_SCREEN_ChargerButtons_StatusLabel.IsVisible = true;
            }

            if (lcdsim.EQ_p == 1 && lcdsim.RunWithBMS != 0 && lcdsim.battConnected_p && !lcdsim.showLastChargeCycleDetails_p)
            {
                RemoveButtons();

                buttonsText = "Waiting For BMS..." + Environment.NewLine;

                MCB_LCDView_SCREEN_ChargerButtons_StatusLabel.SubTitle = buttonsText.Trim();
                MCB_LCDView_SCREEN_ChargerButtons_StatusLabel.IsVisible = true;
            }

            if (MCB_LCDView_SCREEN_ChargerButtons_StatusLabel.IsVisible)
            {
                var statusItem = MCB_LCDView_SCREEN_Charger_Ready_panel.FirstOrDefault(o => o.Title == MCB_LCDView_SCREEN_ChargerButtons_StatusLabel.Title);
                if (statusItem == null)
                {
                    int index = 0;

                    if (MCB_LCDView_SCREEN_Charger_Ready_panel.Count >= 1)
                        index = 1;

                    MCB_LCDView_SCREEN_Charger_Ready_panel.Insert(index, MCB_LCDView_SCREEN_ChargerButtons_StatusLabel);
                }
            }

            RaisePropertyChanged(() => ListItemSource);
        }

        void RemoveButtons()
        {
            var item1 = MCB_LCDView_SCREEN_Charger_Ready_panel.FirstOrDefault(o => o.Title == MCB_LCDView_SCREEN_ChargerMain_startStopButton.Title);
            if (item1 != null)
            {
                MCB_LCDView_SCREEN_Charger_Ready_panel.Remove(MCB_LCDView_SCREEN_ChargerMain_startStopButton);
            }

            var item2 = MCB_LCDView_SCREEN_Charger_Ready_panel.FirstOrDefault(o => o.Title == MCB_LCDView_SCREEN_ChargerMain_exitButton.Title);
            if (item2 != null)
            {
                MCB_LCDView_SCREEN_Charger_Ready_panel.Remove(MCB_LCDView_SCREEN_ChargerMain_exitButton);
            }
        }

        ChartViewItem GeneratePlotPoints_Soc(LcdSimulator lcdsim)
        {
            if (isLithiumAnd2_5OrAbove || lcdsim.RunWithBMS == 1)
                return GeneratePlotPoints_Soc_Lithium(lcdsim);

            return GeneratePlotPoints_Soc_General(lcdsim);
        }

        ChartViewItem GeneratePlotPoints_Soc_General(LcdSimulator lcdsim)
        {
            Contract.Ensures(Contract.Result<PlotModel>() != null);

            double value = lcdsim.currentSOC;
            double max = 100;

            string valueLabel = value <= 0 ? "0" : value.ToString("####");
            valueLabel += AppResources.percentage_chart;

            ChartViewItem ch = new ChartViewItem
            {
                ChartType = "SOC%",
                ChartImageName = "charger",
                Text = valueLabel,
                PlotObject = new PlotModel()
            };

            var series = BuildPieSeries();

            series.Slices.Add(new PieSlice(valueLabel, value)
            {
                Fill = OxyColor.Parse(ACColors.CHART_ORANGE_COLOR)
            });

            double min = max - value;
            series.Slices.Add(new PieSlice("", min)
            {
                Fill = OxyColor.Parse(ACColors.CHART_GRAY_COLOR)
            });

            ch.PlotObject.Series.Add(series);

            return ch;
        }

        ChartViewItem GeneratePlotPoints_Soc_Lithium(LcdSimulator lcdsim)
        {
            Contract.Ensures(Contract.Result<PlotModel>() != null);

            string text;
            if (lcdsim.RunWithBMS == 1)
                text = AppResources.battrey_type_lithium_ion_bms;
            else
                text = AppResources.battrey_type_lithium_ion;

            ChartViewItem ch = new ChartViewItem
            {
                ChartType = "SOC%",
                Text = text
            };

            return ch;
        }

        ChartViewItem GeneratePlotPoints_Current(LcdSimulator lcdsim)
        {
            Contract.Ensures(Contract.Result<PlotModel>() != null);

            double value = lcdsim.current / 10.0;
            double max = int.Parse(activeMcb.Config.numberOfInstalledPMs);

            switch (activeMcb.Config.PMvoltage)
            {
                case "36": max *= 50; break;
                case "48": max *= 40; break;
                case "80": max *= 25; break;
            }

            string valueLabel = value <= 0 ? "0" : value.ToString("####");
            valueLabel += " " + AppResources.a_chart;

            ChartViewItem ch = new ChartViewItem
            {
                ChartType = "Current",
                ChartImageName = "current_a_Icon",
                Text = valueLabel,
                PlotObject = new PlotModel()
            };

            var series = BuildPieSeries();

            series.Slices.Add(new PieSlice(valueLabel, value)
            {
                Fill = OxyColor.Parse(ACColors.CHART_BLUE_COLOR)
            });

            double min = max - value;
            series.Slices.Add(new PieSlice("", min)
            {
                Fill = OxyColor.Parse(ACColors.CHART_GRAY_COLOR)
            });

            ch.PlotObject.Series.Add(series);

            return ch;
        }

        ChartViewItem GeneratePlotPoints_Volt(LcdSimulator lcdsim)
        {
            Contract.Ensures(Contract.Result<PlotModel>() != null);

            double value = lcdsim.voltageVal / 100.0;

            double factor = (lcdsim.batteryVoltage / 2.0);
            factor -= (lcdsim.TRvoltageVal / 100.0);
            factor /= ((lcdsim.EQvoltageVal - lcdsim.TRvoltageVal) / 1000.0);
            factor *= 0.8;

            double max = factor;

            string voltageLabel = value <= 0 ? "0" : value.ToString("N1");
            voltageLabel += " " + AppResources.v_chart;

            double vpc = GetVpc(lcdsim);
            string vpcLabel = vpc.ToString("F") + " " + AppResources.vpc;

            if (CanAddVpc2(lcdsim))
                vpcLabel += GetVpc2Text(lcdsim);

            string valueLabel = voltageLabel;

            if (lcdsim.RunWithBMS != 1)
                valueLabel = voltageLabel + '\n' + vpcLabel;

            ChartViewItem ch = new ChartViewItem
            {
                ChartType = "Voltage",
                ChartImageName = "voltage",
                Text = valueLabel,
                PlotObject = new PlotModel()
            };

            var series = BuildPieSeries();

            series.Slices.Add(new PieSlice(voltageLabel, value)
            {
                Fill = OxyColor.Parse(ACColors.CHART_RED_COLOR)
            });

            double min = max - value;
            series.Slices.Add(new PieSlice("", min)
            {
                Fill = OxyColor.Parse(ACColors.CHART_GRAY_COLOR)
            });

            ch.PlotObject.Series.Add(series);

            return ch;
        }

        double GetVpc(LcdSimulator lcdsim)
        {
            double vpc = lcdsim.voltageVal / 100.0;

            var config = activeMcb.Config;

            if (isLithiumAnd2_5OrAbove && (lcdsim.batviewOn != 0 || !config.enableAutoDetectMultiVoltage))
            {
                vpc /= config.LiIon_numberOfCells;
            }
            else
            {
                vpc /= (lcdsim.numberOfCells);

                if (isLithiumAnd2_5OrAbove)
                    vpc *= 1.5;
            }

            return vpc;
        }

        bool CanAddVpc2(LcdSimulator lcdsim)
        {
            return (lcdsim.batteryCharging_p || lcdsim.showLastChargeCycleDetails_p) && lcdsim.battConnected_p && lcdsim.temperatureConnected_p && !Convert.ToBoolean(lcdsim.RunWithBMS);
        }

        string GetVpc2Text(LcdSimulator lcdsim)
        {
            double vpc2Factor = ((lcdsim.temperatureVal - 25.0f) * 50.0f) / 100000.0f;

            string vpc2Text = "\n" + vpc2Factor.ToString("F") + " " + AppResources.vpc;

            return vpc2Text;
        }

        ChartViewItem GeneratePlotPoints_Temp(LcdSimulator lcdsim)
        {
            Contract.Ensures(Contract.Result<PlotModel>() != null);

            bool isFahrenheit = activeMcb.Config.temperatureFormat;
            double max = TemperatureManager.GetCorrectTemperature(550.0, isFahrenheit);
            double value = TemperatureManager.GetCorrectTemperature(lcdsim.temperatureVal, isFahrenheit);

            string valueLabel = "";

            ChartViewItem ch;

            if (lcdsim.temperatureConnected_p)
            {
                valueLabel = value <= 0 ? "0" : value.ToString("####");
                valueLabel += " " + TemperatureManager.GetCorrectMark(isFahrenheit);

                ch = new ChartViewItem
                {
                    ChartType = "Temperature",
                    ChartImageName = "temp_icon",
                    Text = valueLabel,
                    PlotObject = new PlotModel()
                };

                var series = BuildPieSeries();

                series.Slices.Add(new PieSlice(valueLabel, value)
                {
                    Fill = OxyColor.Parse(ACColors.CHART_GREEN_COLOR)
                });

                double min = max - value;
                series.Slices.Add(new PieSlice("", min)
                {
                    Fill = OxyColor.Parse(ACColors.CHART_GRAY_COLOR)
                });

                ch.PlotObject.Series.Add(series);
            }
            else
            {
                valueLabel = "N/A";

                ch = new ChartViewItem
                {
                    ChartType = "Temperature",
                    ChartImageName = "temp_icon",
                    Text = valueLabel
                };
            }

            return ch;
        }

        PieSeries BuildPieSeries()
        {
            var series = new PieSeries
            {
                FontSize = 10,
                Diameter = .65,
                InnerDiameter = .5,
                StrokeThickness = 2.0,
                AngleSpan = 275,
                StartAngle = 135,
                InsideLabelFormat = null,
                OutsideLabelFormat = "{1}",
                Stroke = OxyColors.Transparent,
                TextColor = OxyColor.Parse(ACColors.CHART_BLUE_COLOR),
                TickLabelDistance = 10,
                TickRadialLength = 0,
                TickHorizontalLength = 0
            };

            return series;
        }
    }
}
