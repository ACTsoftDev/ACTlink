using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using MvvmCross.Core.ViewModels;

namespace actchargers
{
    public class ViewCyclesHistoryViewModel : BaseViewModel
    {
        public string MCB_cycleHistoryGridColumn_CycleID { get; set; }
        public string MCB_cycleHistoryGridColumn_Date { get; set; }
        public string MCB_cycleHistoryGridColumn_Duration { get; set; }
        public string MCB_cycleHistoryGridColumn_AHRs { get; set; }
        public string MCB_cycleHistoryGridColumn_KWHrs { get; set; }
        public string MCB_cycleHistoryGridColumn_maxWHR { get; set; }
        public string MCB_cycleHistoryGridColumn_maxTemperature { get; set; }
        public string MCB_cycleHistoryGridColumn_startVoltage { get; set; }
        public string MCB_cycleHistoryGridColumn_endVoltage { get; set; }
        public string MCB_cycleHistoryGridColumn_exitStatus { get; set; }
        public string MCB_cycleHistoryGridColumn_Profiles { get; set; }
        public string MCB_cycleHistoryGridColumn_PMFaulted { get; set; }
        public string MCB_cycleHistoryGridColumn_BatteryType { get; set; }

        ListViewItem MCB_ReadCyclesHistoryStartId;

        ObservableCollection<ViewCycle> historyGrid = new ObservableCollection<ViewCycle>();
        public ObservableCollection<ViewCycle> MCB_cyclesHistoryGrid
        {
            get { return historyGrid; }

            set
            {
                historyGrid = value;
                RaisePropertyChanged(() => MCB_cyclesHistoryGrid);
            }
        }

        bool _showReadRecords;
        public bool ShowReadRecords
        {
            get
            {
                return _showReadRecords;
            }
            set
            {
                _showReadRecords = value;
                RaisePropertyChanged(() => ShowReadRecords);
            }
        }

        public ViewCyclesHistoryViewModel()
        {
            CreateViewCycleHistoryData();
        }

        void CreateViewCycleHistoryData()
        {
            ViewTitle = AppResources.view_cycles_history;

            if (ControlObject.UserAccess.MCB_canReadCyclesHistory == AccessLevelConsts.noAccess)
            {
                ShowReadRecords = false;
                ACUserDialogs.ShowAlertWithOkButtonsAction(AppResources.no_data_found, () => { ShowViewModel<ViewCyclesHistoryViewModel>(new { pop = "pop" }); });

                return;
            }

            MCB_cycleHistoryGridColumn_CycleID = AppResources.history_cycle_id;
            MCB_cycleHistoryGridColumn_Date = AppResources.history_date;
            MCB_cycleHistoryGridColumn_Duration = AppResources.history_duration;
            MCB_cycleHistoryGridColumn_AHRs = AppResources.history_ahrs;
            MCB_cycleHistoryGridColumn_KWHrs = AppResources.history_kwhrs;
            MCB_cycleHistoryGridColumn_maxWHR = AppResources.history_max_whr;
            MCB_cycleHistoryGridColumn_maxTemperature = AppResources.history_max_temperature;
            MCB_cycleHistoryGridColumn_startVoltage = AppResources.history_start_voltage;
            MCB_cycleHistoryGridColumn_endVoltage = AppResources.history_end_voltage;
            MCB_cycleHistoryGridColumn_exitStatus = AppResources.history_exit_status;
            MCB_cycleHistoryGridColumn_Profiles = AppResources.history_profiles;
            MCB_cycleHistoryGridColumn_PMFaulted = AppResources.history_pm_faulted;
            MCB_cycleHistoryGridColumn_BatteryType = AppResources.battery_type;

            MCB_ReadCyclesHistoryStartId = new ListViewItem
            {
                Index = 0,
                Title = "",
                IsEditable = true,
                DefaultCellType = ACUtility.CellTypes.LabelTextEdit,
                EditableCellType = ACUtility.CellTypes.LabelTextEdit
            };
            MCB_ReadCyclesHistoryButton_Click();
        }

        public IMvxCommand ReadRecordCommand
        {
            get
            {
                return new MvxCommand(MCB_ReadCyclesHistoryButton_Click);
            }
        }

        async void MCB_ReadCyclesHistoryButton_Click()
        {
            try
            {
                UInt32 startRecord = 0;

                if (!MCBQuantum.Instance.MCB_quickSerialNumberCheckStrict())
                    return;

                MCBobject activeMCB = MCBQuantum.Instance.GetMCB();

                ACUserDialogs.ShowProgress();

                if (ControlObject.isDebugMaster)
                    UInt32.TryParse(MCB_ReadCyclesHistoryStartId.Text, out startRecord);

                if (startRecord < activeMCB.minChargeRecordID)
                {
                    startRecord = activeMCB.minChargeRecordID;

                    if (ControlObject.isDebugMaster)
                        MCB_ReadCyclesHistoryStartId.Text = activeMCB.minChargeRecordID.ToString();
                }

                List<object> results = new List<object>();

                List<object> arguments = new List<object>
                {
                    startRecord
                };

                results = await MCB_ReadCyclesHistoryButton_Click_doWork(arguments);
                MCB_ReadCyclesHistoryButton_Click_doneWork(results);
            }
            catch (Exception ex)
            {
                Logger.AddLog(true, "X86" + ex);
                ACUserDialogs.HideProgress();
            }
        }

        async Task<List<object>> MCB_ReadCyclesHistoryButton_Click_doWork(List<object> genericlist)
        {
            List<object> results = new List<object>();
            MCBobject activeMCB = MCBQuantum.Instance.GetMCB();
            CommunicationResult status = CommunicationResult.NOT_EXIST;
            bool internalFailure = false;
            string internalFailureString = "";

            UInt32 startRecord = (UInt32)genericlist[0];
            try
            {
                status = await Task.Run(async () =>
                {
                    return await activeMCB.getAllRecords(activeMCB.globalRecord.chargeCycles, startRecord);
                });
            }
            catch (Exception ex)
            {
                internalFailure = true;
                internalFailureString = ex.ToString();
            }

            results.Add(internalFailure);
            results.Add(internalFailureString);
            results.Add(status);

            return results;
        }


        void MCB_ReadCyclesHistoryButton_Click_doneWork(List<object> results)
        {
            List<object> genericlist = results;
            bool internalFailure = (bool)genericlist[0];
            string internalFailureString = (string)genericlist[1];
            CommunicationResult status = (CommunicationResult)genericlist[2];

            if (internalFailure)
            {
                ACUserDialogs.HideProgress();

                return;
            }

            if (status != CommunicationResult.OK)
            {
                ACUserDialogs.HideProgress();

                return;
            }

            MCBobject activeMCB = MCBQuantum.Instance.GetMCB();
            MCB_cyclesHistoryGrid.Clear();

            if (!MCBQuantum.Instance.MCB_quickSerialNumberCheckStrict())
            {
                ACUserDialogs.HideProgress();

                return;
            }

            List<ChargeRecord> rec = activeMCB.getRecordsList();

            foreach (ChargeRecord r in rec)
            {
                ViewCycle item = new ViewCycle
                {
                    ViewCycleID = (r.isValidCRC7 ? "" : "Corrupted-") + r.cycleID.ToString(),
                    Date = r.cycleTime,
                    Duration = string.Format("{0:00}:{1:00}", r.duration / 3600, (r.duration % 3600) / 60),
                    AHRS = (r.totalAS / 3600).ToString("N1"),
                    KWHRS = (r.totalWS / 3600000).ToString("N1"),
                    MaxWHR = (r.maxWS / 3600.0).ToString("f"),
                    BatteryType = r.batteryType
                };

                string maxTemp = TemperatureManager.GetCorrectTemperature(r.maxTemperature / 10.0, activeMCB.Config.temperatureFormat).ToString();

                if (maxTemp == "3276.7")
                    maxTemp = "N/A";

                item.MAxTemperature = maxTemp;
                item.StartVoltage = (r.startVoltage / 100.0).ToString();
                item.Endvoltage = (r.lastVoltage / 100.0).ToString();
                item.EXitStatus = ChargeRecord.ExitCodes(r.status, r.batteryType == 1, false);

                string PMfaults = "";

                if (r.PMerror1 != 0 && r.PMerror1 != 0xFFFFFFFF)
                {
                    if (PMfaults.Length != 0)
                        PMfaults += Environment.NewLine;

                    PMfaults += MCBobject.AddressToSerial(r.PMerror1);

                }

                if (r.PMerror2 != 0 && r.PMerror2 != 0xFFFFFFFF)
                {
                    if (PMfaults.Length != 0)
                        PMfaults += Environment.NewLine;

                    PMfaults += MCBobject.AddressToSerial(r.PMerror2);

                }

                if (r.PMerror3 != 0 && r.PMerror3 != 0xFFFFFFFF)
                {
                    if (PMfaults.Length != 0)
                        PMfaults += Environment.NewLine;

                    PMfaults += MCBobject.AddressToSerial(r.PMerror3);
                }

                if (r.PMerror4 != 0 && r.PMerror4 != 0xFFFFFFFF)
                {
                    if (PMfaults.Length != 0)
                        PMfaults += Environment.NewLine;

                    PMfaults += MCBobject.AddressToSerial(r.PMerror4);
                }

                item.PMFaulted = PMfaults;

                string cycles = "";

                if ((r.cycles & 0x40) != 0)
                {
                    cycles += "Desulfate";
                }
                else
                {
                    if ((r.cycles & 0x03) != 0)
                        cycles += "CC";

                    if ((r.cycles & 0x04) != 0)
                        cycles += ", CV";

                    if ((r.cycles & 0x08) != 0)
                        cycles += ", FI";

                    if ((r.cycles & 0x10) != 0)
                        cycles += ", EQ";

                    if ((r.cycles & 0x20) != 0)
                        cycles += ", RF";
                }

                item.Profiles = cycles;
                MCB_cyclesHistoryGrid.Add(item);
            }

            InvokeOnMainThread(() =>
            {
                RaisePropertyChanged(() => MCB_cyclesHistoryGrid);
            });

            ACUserDialogs.HideProgress();
        }

        public override void OnBackButtonClick()
        {
            base.OnBackButtonClick();

            ShowViewModel<ViewCyclesHistoryViewModel>(new { pop = "pop" });
        }
    }
}
