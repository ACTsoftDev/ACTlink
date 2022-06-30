using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace actchargers
{
    public class EventDataRangeHistoryViewModel : BaseViewModel, IMvxPageViewModel
    {
        ListViewItem Batt_ReadEventsHistoryStartId;
        ListViewItem Batt_ReadEventsHistoryStartDate;
        ListViewItem Batt_ReadEventsHistoryEndDate;

        public List<BaseViewModel> TabsViewModels { get; private set; }
        private BatteryUsageSummaryViewModel _batteryUsageSummaryViewModel;
        private ChargeSummaryViewModel _chargeSummaryViewModel;
        private ExceptionsViewModel _exceptionsViewModel;
        private HistoryChartsViewModel _historyChartViewModel;
        private BatteryDailyUsageViewModel _batteryDailyUsageViewModel;

        string dayFormat = "dd/MM";

        public EventDataRangeHistoryViewModel()
        {
            Batt_ReadEventsHistoryStartId = new ListViewItem()
            {
                Title = "Start from",
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.LabelTextEdit,
                Text = "0"
            };

            Batt_ReadEventsHistoryStartDate = new ListViewItem();
            Batt_ReadEventsHistoryEndDate = new ListViewItem();
        }

        public void Init(string eventDataRange, string fromDate, string toDate)
        {
            ViewTitle = eventDataRange;
            TabsViewModels = new List<BaseViewModel>();
            _batteryUsageSummaryViewModel = new BatteryUsageSummaryViewModel();
            _exceptionsViewModel = new ExceptionsViewModel();
            _historyChartViewModel = new HistoryChartsViewModel();
            _chargeSummaryViewModel = new ChargeSummaryViewModel();
            _batteryDailyUsageViewModel = new BatteryDailyUsageViewModel();

            if (eventDataRange != null && eventDataRange.Equals("Date Range"))
            {
                Batt_ReadEventsHistoryStartDate.Date = DateTime.ParseExact(fromDate, ACConstants.DATE_TIME_FORMAT, CultureInfo.InvariantCulture);
                Batt_ReadEventsHistoryEndDate.Date = DateTime.ParseExact(toDate, ACConstants.DATE_TIME_FORMAT, CultureInfo.InvariantCulture);
            }
            else
            {
                AssignDates(eventDataRange);
            }



            //To Make a Call to BattView to get the History details
            bool isDevInProgress = false;
            if (!isDevInProgress)
            {
                UInt32 startRecord = 0;
                bool byID = true;
                if (!BattViewQuantum.Instance.BattView_quickSerialNumberCheckStrict())
                    return;
                if (ControlObject.UserAccess.Batt_canReadEventsByID == AccessLevelConsts.noAccess)
                {
                    byID = false;
                }
                if (byID && ControlObject.UserAccess.Batt_canReadEventsByTime != AccessLevelConsts.noAccess)
                {
                    if (Batt_ReadEventsHistoryStartId.Text.Trim() == "")
                        byID = false;
                }
                if (byID)
                {
                    if (startRecord < BattViewQuantum.Instance.GetBATTView().eventsRecordsStartID)
                    {
                        Batt_ReadEventsHistoryStartId.Text = BattViewQuantum.Instance.GetBATTView().eventsRecordsStartID.ToString();
                    }
                    UInt32.TryParse(Batt_ReadEventsHistoryStartId.Text, out startRecord);

                }
                if (!byID && ControlObject.UserAccess.Batt_canReadEventsByTime == AccessLevelConsts.noAccess)
                    return;
                if (!byID)
                {
                    if (Batt_ReadEventsHistoryStartDate.Date >= Batt_ReadEventsHistoryEndDate.Date)
                        return;
                }

                List<object> arguments = new List<object>();
                arguments.Add(startRecord);
                arguments.Add(byID);
                arguments.Add(Batt_ReadEventsHistoryStartDate.Date);
                arguments.Add(Batt_ReadEventsHistoryEndDate.Date);
                arguments.Add(1);

                UInt32 maxdayEventsDuration = 0;

                List<object> results = new List<object>();
                try
                {
                    ACUserDialogs.ShowProgress();
                    Task.Run(async () =>
                    {
                        results = await BattViewQuantum.Instance.ReadHistoryRecords(arguments);
                        var status = (CommunicationResult)results[2];
                        if (status == CommunicationResult.OK)
                        {
                            try
                            {
                                List<BattViewObjectEvent> events = BattViewQuantum.Instance.GetBATTView().getBattViewEvent();

                                BATTViewReporting b = new BATTViewReporting(events, Batt_ReadEventsHistoryStartDate.Date, Batt_ReadEventsHistoryEndDate.Date, BattViewQuantum.Instance.GetBATTView().Config);


                                _historyChartViewModel.BATT_resetReportCharts(b.days.First().Key.AddDays(-1), b.days.Last().Key.AddDays(1), maxdayEventsDuration);


                                //int daysCount = 0;

                                //if (b.days.Values.Count > 10)
                                //{
                                //    daysCount = b.days.Values.Count / 10;
                                //}


                                //if (daysCount == 0)
                                //{
                                foreach (BATTViewDailyDetails d in b.days.Values)
                                {

                                    string InHoursCategoryAccessLabel = d.date.ToString(dayFormat);

                                    double ChargeDurationSeriesValue = Convert.ToDouble(TimeSpan.Parse(new DateTime(TimeSpan.FromSeconds(d.charge_duration > 86399 ? 86399 : d.charge_duration).Ticks).ToString("HH:mm")).TotalHours);

                                    double InUseDurationSeriesValue = Convert.ToDouble(TimeSpan.Parse(new DateTime(TimeSpan.FromSeconds(d.inuse_duration > 86399 ? 86399 : d.inuse_duration).Ticks).ToString("HH:mm")).TotalHours);

                                    double IdleDurationSeriesValue = Convert.ToDouble(TimeSpan.Parse(new DateTime(TimeSpan.FromSeconds(d.idle_duration > 86399 ? 86399 : d.idle_duration).Ticks).ToString("HH:mm")).TotalHours);

                                    _historyChartViewModel.SetChargeinHoursSeries(InHoursCategoryAccessLabel, ChargeDurationSeriesValue, InUseDurationSeriesValue, IdleDurationSeriesValue);


                                    //Batt_report_EBUsChart.Series["EBUs"].Points.AddXY(d.date, d.inuse_as / (0.8 * connManager.activeBattView.config.ahrcapacity * 3600.0));
                                    //Batt_report_DailyAHrsChart.Series["Charge AHrs"].Points.AddXY(d.date, (UInt32)(d.charge_as / 3600));
                                    //Batt_report_DailyAHrsChart.Series["In Use AHrs"].Points.AddXY(d.date, (UInt32)(d.inuse_as / 3600));
                                    //Batt_report_DailyAHrsChart.Series["Available AHrs"].Points.AddXY(d.date, connManager.activeBattView.config.ahrcapacity);


                                    double ChargeDurationAHRSeriesValue = (UInt32)(d.charge_as / 3600);

                                    double InUseDurationAHRSeriesValue = (UInt32)(d.inuse_as / 3600);

                                    double IdleDurationAHRSeriesValue = BattViewQuantum.Instance.GetBATTView().Config.ahrcapacity;

                                    _historyChartViewModel.SetChargeinAHRSeries(InHoursCategoryAccessLabel, ChargeDurationAHRSeriesValue, InUseDurationAHRSeriesValue, IdleDurationAHRSeriesValue);


                                    double EBUSeriesValue = d.inuse_as / (0.8 * BattViewQuantum.Instance.GetBATTView().Config.ahrcapacity * 3600.0);
                                    _historyChartViewModel.SetChargeinEBUSeries(InHoursCategoryAccessLabel, EBUSeriesValue);

                                    BATTDailyUsageModel battDailyUsageModel = new BATTDailyUsageModel();

                                    battDailyUsageModel.date = d.date.ToString(@"M\/d\/yy");
                                    battDailyUsageModel.is_working_day = (d.is_working_day ? "W " : "") + (d.total_charge_events > 0 ? "C" : "");
                                    battDailyUsageModel.total_charge_events = d.total_charge_events.ToString();
                                    battDailyUsageModel.charge_as = (d.charge_as / 3600).ToString("N1").TrimEnd('0').TrimEnd(System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator[0]);
                                    battDailyUsageModel.charge_ws = (d.charge_ws / 3600000).ToString("N2").TrimEnd('0').TrimEnd(System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator[0]);
                                    battDailyUsageModel.total_inuse_events = d.total_inuse_events.ToString();
                                    battDailyUsageModel.inuse_as = (d.inuse_as / 3600).ToString("N1").TrimEnd('0').TrimEnd(System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator[0]);
                                    battDailyUsageModel.inuse_ws = (d.inuse_ws / 3600000).ToString("N2").TrimEnd('0').TrimEnd(System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator[0]);

                                    battDailyUsageModel.inuse_duration = String.Format("{0:00}:{1:00}", d.inuse_duration / 3600, (d.inuse_duration % 3600) / 60);
                                    battDailyUsageModel.charge_duration = String.Format("{0:00}:{1:00}", d.charge_duration / 3600, (d.idle_duration % 3600) / 60);
                                    battDailyUsageModel.total_idle_events = d.total_idle_events.ToString();
                                    battDailyUsageModel.idle_duration = String.Format("{0:00}:{1:00}", d.idle_duration / 3600, (d.idle_duration % 3600) / 60);

                                    battDailyUsageModel.expected_eq = d.expected_eq ? (d.count_of_eqs == 0 ? "YES" : "") : "";
                                    battDailyUsageModel.expected_fi = d.expected_fi ? (d.count_of_finishes == 0 ? "YES" : "") : "";
                                    battDailyUsageModel.ahrcapacity = ((d.inuse_as / 3600) / (0.8 * BattViewQuantum.Instance.GetBATTView().Config.ahrcapacity)).ToString("N2").TrimEnd('0').TrimEnd(System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator[0]);
                                    _batteryDailyUsageViewModel.DailyUsageItemSource.Add(battDailyUsageModel);

                                }
                                //}
                                //else
                                //{
                                //    int count = 1;
                                //    double ChargeDurationSeriesValue = 0;
                                //    double InUseDurationSeriesValue = 0;
                                //    double IdleDurationSeriesValue = 0;

                                //    for (int i = 0; i < b.days.Values.Count; i++)
                                //    {

                                //        var d = b.days.Sum(o => o.);
                                //        string InHoursCategoryAccessLabel = d.date.ToString(dayFormat);


                                //        if (i == b.days.Values.Count)
                                //        {

                                //        }
                                //        else
                                //        {
                                //            if (count == daysCount)
                                //            {

                                //            }
                                //            else
                                //            {
                                //                ++count;
                                //                ChargeDurationSeriesValue += Convert.ToDouble(TimeSpan.Parse(new DateTime(TimeSpan.FromSeconds(d.charge_duration > 86399 ? 86399 : d.charge_duration).Ticks).ToString("HH:mm")).TotalHours);
                                //                InUseDurationSeriesValue += Convert.ToDouble(TimeSpan.Parse(new DateTime(TimeSpan.FromSeconds(d.inuse_duration > 86399 ? 86399 : d.inuse_duration).Ticks).ToString("HH:mm")).TotalHours);
                                //                IdleDurationSeriesValue += Convert.ToDouble(TimeSpan.Parse(new DateTime(TimeSpan.FromSeconds(d.idle_duration > 86399 ? 86399 : d.idle_duration).Ticks).ToString("HH:mm")).TotalHours);
                                //            }
                                //        }
                                //    }
                                //}








                                //Batt_report_dailyUsageChart.Series["Charge Duration"].Points.AddXY(d.date, new DateTime(TimeSpan.FromSeconds(d.charge_duration > 86399 ? 86399 : d.charge_duration).Ticks));
                                //Batt_report_dailyUsageChart.Series["In Use Duration"].Points.AddXY(d.date, new DateTime(TimeSpan.FromSeconds(d.inuse_duration > 86399 ? 86399 : d.inuse_duration).Ticks));
                                //Batt_report_dailyUsageChart.Series["Idle Duration"].Points.AddXY(d.date, new DateTime(TimeSpan.FromSeconds(d.idle_duration > 86399 ? 86399 : d.idle_duration).Ticks));
                                //Batt_report_EBUsChart.Series["EBiUs"].Points.AddXY(d.date, d.inuse_as / (0.8 * connManager.activeBattView.config.ahrcapacity * 3600.0));
                                //Batt_report_DailyAHrsChart.Series["Charge AHrs"].Points.AddXY(d.date, (UInt32)(d.charge_as / 3600));
                                //Batt_report_DailyAHrsChart.Series["In Use AHrs"].Points.AddXY(d.date, (UInt32)(d.inuse_as / 3600));
                                //Batt_report_DailyAHrsChart.Series["Available AHrs"].Points.AddXY(d.date, connManager.activeBattView.config.ahrcapacity);


                                _batteryDailyUsageViewModel.loadList();
                                _historyChartViewModel.LoadCharts();

                                //Assigning the Text fields
                                _batteryUsageSummaryViewModel.Batt_report_cus_charge_hrs.SubTitle = (b.summary.charge_duration / 3600).ToString("N0") + " (" + b.summary.charge_duration_percent + ")";
                                _batteryUsageSummaryViewModel.Batt_report_cus_charge_hrs.SubTitle2 = (b.summary.inuse_duration / 3600).ToString("N0") + " (" + b.summary.inuse_duration_percent + ")";
                                _batteryUsageSummaryViewModel.Batt_report_cus_charge_hrs.SubTitle3 = (b.summary.idle_duration / 3600).ToString("N0") + " (" + b.summary.idle_duration_percent + ")";


                                _batteryUsageSummaryViewModel.Batt_report_cus_charge_ahrs.SubTitle = (b.summary.charge_as / 3600).ToString("N0") + " (" + b.summary.charge_as_percent + ")";
                                _batteryUsageSummaryViewModel.Batt_report_cus_charge_ahrs.SubTitle2 = (b.summary.inuse_as / 3600).ToString("N0") + " (" + b.summary.inuse_as_percent.ToString() + ")";


                                if (b.summary.inUseExists)
                                {
                                    _batteryUsageSummaryViewModel.Batt_report_dui_min_hrs.SubTitle = String.Format("{0:00}:{1:00}", b.summary.minInUseDuration / 3600, (b.summary.minInUseDuration % 3600) / 60);
                                    _batteryUsageSummaryViewModel.Batt_report_dui_min_hrs.SubTitle2 = String.Format("{0:00}:{1:00}", b.summary.avgInUseDuration / 3600, (b.summary.avgInUseDuration % 3600) / 60);
                                    _batteryUsageSummaryViewModel.Batt_report_dui_min_hrs.SubTitle3 = String.Format("{0:00}:{1:00}", b.summary.maxInUseDuration / 3600, (b.summary.maxInUseDuration % 3600) / 60);

                                    _batteryUsageSummaryViewModel.Batt_report_dui_min_ahrs.SubTitle3 = (b.summary.maxInUseAS / 3600.0).ToString("N1").TrimEnd('0').TrimEnd(System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator[0]);
                                    _batteryUsageSummaryViewModel.Batt_report_dui_min_ahrs.SubTitle2 = (b.summary.avgInUseAS / 3600.0).ToString("N1").TrimEnd('0').TrimEnd(System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator[0]);
                                    _batteryUsageSummaryViewModel.Batt_report_dui_min_ahrs.SubTitle = (b.summary.minInUseAS / 3600).ToString("N1").TrimEnd('0').TrimEnd(System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator[0]);


                                    _batteryUsageSummaryViewModel.Batt_report_duue_avg_ebus.SubTitle2 = (b.summary.maxInUseAS / (0.8 * BattViewQuantum.Instance.GetBATTView().Config.ahrcapacity * 3600.0)).ToString("N1").TrimEnd('0').TrimEnd(System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator[0]);
                                    _batteryUsageSummaryViewModel.Batt_report_duue_avg_ebus.SubTitle = (b.summary.avgInUseAS / (0.8 * BattViewQuantum.Instance.GetBATTView().Config.ahrcapacity * 3600.0)).ToString("N1").TrimEnd('0').TrimEnd(System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator[0]);
                                }
                                else
                                {
                                    _batteryUsageSummaryViewModel.Batt_report_dui_min_hrs.SubTitle = "N/A";
                                    _batteryUsageSummaryViewModel.Batt_report_dui_min_hrs.SubTitle2 = "N/A";
                                    _batteryUsageSummaryViewModel.Batt_report_dui_min_hrs.SubTitle3 = "N/A";

                                    _batteryUsageSummaryViewModel.Batt_report_dui_min_ahrs.SubTitle3 = "N/A";
                                    _batteryUsageSummaryViewModel.Batt_report_dui_min_ahrs.SubTitle2 = "N/A";
                                    _batteryUsageSummaryViewModel.Batt_report_dui_min_ahrs.SubTitle = "N/A";


                                    _batteryUsageSummaryViewModel.Batt_report_duue_avg_ebus.SubTitle2 = "N/A";
                                    _batteryUsageSummaryViewModel.Batt_report_duue_avg_ebus.SubTitle = "N/A";
                                }
                                if (b.days.Count > 0)
                                {
                                    if (b.summary.inUseExists)
                                    {
                                        _batteryUsageSummaryViewModel.Batt_report_haus_min_ahrs_hr.SubTitle3 = (b.summary.maxHourlyAHUsage).ToString("N0");
                                        _batteryUsageSummaryViewModel.Batt_report_haus_min_ahrs_hr.SubTitle = (b.summary.minHourlyAHUsage).ToString("N0");
                                        _batteryUsageSummaryViewModel.Batt_report_haus_min_ahrs_hr.SubTitle2 = (b.summary.avgHourlyAHUsage).ToString("N0");
                                    }
                                    else
                                    {
                                        _batteryUsageSummaryViewModel.Batt_report_haus_min_ahrs_hr.SubTitle = "N/A";
                                        _batteryUsageSummaryViewModel.Batt_report_haus_min_ahrs_hr.SubTitle2 = "N/A";
                                        _batteryUsageSummaryViewModel.Batt_report_haus_min_ahrs_hr.SubTitle3 = "N/A";
                                    }
                                    if (b.summary.idle_exists)
                                    {
                                        _batteryUsageSummaryViewModel.Batt_report_dhafc_min_hrs.SubTitle = String.Format("{0:00}:{1:00}", b.summary.minChargeOppurtiniyDuration / 3600, (b.summary.minChargeOppurtiniyDuration % 3600) / 60);
                                        _batteryUsageSummaryViewModel.Batt_report_dhafc_min_hrs.SubTitle2 = String.Format("{0:00}:{1:00}", b.summary.avgChargeOppurtiniyDuration / 3600, (b.summary.avgChargeOppurtiniyDuration % 3600) / 60);
                                    }
                                    else
                                    {
                                        _batteryUsageSummaryViewModel.Batt_report_dhafc_min_hrs.SubTitle = "N/A";
                                        _batteryUsageSummaryViewModel.Batt_report_dhafc_min_hrs.SubTitle2 = "N/A";
                                    }

                                    _chargeSummaryViewModel.Batt_report_cs_days.SubTitle = b.days.Count.ToString("N0");
                                    _chargeSummaryViewModel.Batt_report_cs_days.SubTitle2 = b.summary.total_charge_events.ToString("N0");
                                    _chargeSummaryViewModel.Batt_report_cs_missedFinishes.SubTitle = b.summary.missedFI ? "Yes" : "NO";
                                    _chargeSummaryViewModel.Batt_report_cs_missedFinishes.SubTitle2 = b.summary.missedEQ ? "Yes" : "NO";
                                    _chargeSummaryViewModel.Batt_report_cs_charge_ahr.SubTitle = (b.summary.charge_as / 3600).ToString("N0"); ;
                                    _chargeSummaryViewModel.Batt_report_cs_charge_ahr.SubTitle2 = (b.summary.inuse_as / 3600).ToString("N0");
                                    if (b.summary.maxTemperatureValue != UInt32.MinValue)
                                        _chargeSummaryViewModel.Batt_report_cs_max_temp.SubTitle = ((UInt32)(b.summary.maxTemperatureValue * 1.8 + 32)).ToString("N0");
                                    else
                                        _chargeSummaryViewModel.Batt_report_cs_max_temp.SubTitle = "N/A";
                                    _chargeSummaryViewModel.Batt_report_cs_max_temp.SubTitle2 = b.summary.minSOC.ToString();
                                    _chargeSummaryViewModel.Batt_report_eqs.SubTitle = b.summary.totalEQ.ToString("N0");
                                    _chargeSummaryViewModel.Batt_report_eqs.SubTitle2 = b.summary.totalEQWaterOK.ToString("N0");



                                    _exceptionsViewModel.Batt_report_ex_missedEQs.SubTitle = b.summary.missedEQ ? "Yes" : "NO";

                                    _exceptionsViewModel.Batt_report_ex_missedEQs.SubTitle2 = b.summary.missedFI ? "Yes" : "NO";

                                    _exceptionsViewModel.Batt_report_ex_dischargeLimitExceeded.SubTitle = b.summary.deepDischarged ? "Yes" : "NO";

                                    _exceptionsViewModel.Batt_report_ex_dischargeLimitExceeded.SubTitle2 = b.summary.totalOverTemperature > 0 ? "Yes" : "NO";

                                    if (b.summary.charge_as - 3600 * BattViewQuantum.Instance.GetBATTView().Config.ahrcapacity > 1.3 * b.summary.inuse_as)
                                        _exceptionsViewModel.Batt_report_ex_ahr_return.SubTitle = "LOW";
                                    else if (b.summary.charge_as + 3600 * BattViewQuantum.Instance.GetBATTView().Config.ahrcapacity < b.summary.inuse_as)
                                        _exceptionsViewModel.Batt_report_ex_ahr_return.SubTitle = "HIGH";
                                    else
                                        _exceptionsViewModel.Batt_report_ex_ahr_return.SubTitle = "OK";

                                    _exceptionsViewModel.Batt_report_ex_ahr_return.SubTitle2 = b.summary.sensorIssue ? "Yes" : "NO";

                                    _exceptionsViewModel.Batt_report_ex_deep_discharge.SubTitle = b.summary.deepDischarged ? "Yes" : "NO";

                                    //TODO Implement Charts
                                    //CHARTS

                                    //BATT_resetReportCharts(b.days.First().Key.AddDays(-1), b.days.Last().Key.AddDays(1), maxdayEventsDuration);

                                    //foreach (BATTViewDailyDetails d in b.days.Values)
                                    //{
                                    //    Batt_report_dailyUsageChart.Series["Charge Duration"].Points.AddXY(d.date, new DateTime(TimeSpan.FromSeconds(d.charge_duration > 86399 ? 86399 : d.charge_duration).Ticks));
                                    //    Batt_report_dailyUsageChart.Series["In Use Duration"].Points.AddXY(d.date, new DateTime(TimeSpan.FromSeconds(d.inuse_duration > 86399 ? 86399 : d.inuse_duration).Ticks));
                                    //    Batt_report_dailyUsageChart.Series["Idle Duration"].Points.AddXY(d.date, new DateTime(TimeSpan.FromSeconds(d.idle_duration > 86399 ? 86399 : d.idle_duration).Ticks));
                                    //    Batt_report_EBUsChart.Series["EBUs"].Points.AddXY(d.date, d.inuse_as / (0.8 * connManager.activeBattView.config.ahrcapacity * 3600.0));
                                    //    Batt_report_DailyAHrsChart.Series["Charge AHrs"].Points.AddXY(d.date, (UInt32)(d.charge_as / 3600));
                                    //    Batt_report_DailyAHrsChart.Series["In Use AHrs"].Points.AddXY(d.date, (UInt32)(d.inuse_as / 3600));
                                    //    Batt_report_DailyAHrsChart.Series["Available AHrs"].Points.AddXY(d.date, connManager.activeBattView.config.ahrcapacity);

                                    //}

                                }
                                else
                                {
                                    _batteryUsageSummaryViewModel.Batt_report_haus_min_ahrs_hr.SubTitle2 = "-";
                                    _batteryUsageSummaryViewModel.Batt_report_haus_min_ahrs_hr.SubTitle = "-";
                                    _batteryUsageSummaryViewModel.Batt_report_haus_min_ahrs_hr.SubTitle3 = "-";
                                    _batteryUsageSummaryViewModel.Batt_report_dhafc_min_hrs.SubTitle = "-";
                                    _batteryUsageSummaryViewModel.Batt_report_dhafc_min_hrs.SubTitle2 = "-";
                                    _chargeSummaryViewModel.Batt_report_cs_days.SubTitle = "-";
                                    _chargeSummaryViewModel.Batt_report_cs_days.SubTitle2 = "-";
                                    _chargeSummaryViewModel.Batt_report_cs_missedFinishes.SubTitle = "-";
                                    _chargeSummaryViewModel.Batt_report_cs_missedFinishes.SubTitle2 = "-";
                                    _chargeSummaryViewModel.Batt_report_cs_charge_ahr.SubTitle = "-";
                                    _chargeSummaryViewModel.Batt_report_cs_charge_ahr.SubTitle2 = "-";
                                    _chargeSummaryViewModel.Batt_report_cs_max_temp.SubTitle = "-";
                                    _chargeSummaryViewModel.Batt_report_cs_max_temp.SubTitle2 = "-";
                                    _chargeSummaryViewModel.Batt_report_eqs.SubTitle = "-";
                                    _chargeSummaryViewModel.Batt_report_eqs.SubTitle2 = "-";

                                    _exceptionsViewModel.Batt_report_ex_missedEQs.SubTitle = "-";
                                    _exceptionsViewModel.Batt_report_ex_missedEQs.SubTitle2 = "-";
                                    _exceptionsViewModel.Batt_report_ex_dischargeLimitExceeded.SubTitle = "-";
                                    _exceptionsViewModel.Batt_report_ex_dischargeLimitExceeded.SubTitle2 = "-";
                                    _exceptionsViewModel.Batt_report_ex_ahr_return.SubTitle = "-";
                                    _exceptionsViewModel.Batt_report_ex_ahr_return.SubTitle2 = "-";
                                    _exceptionsViewModel.Batt_report_ex_deep_discharge.SubTitle = "-";

                                    //TODO Implement Charts
                                    //BATT_resetReportCharts(DateTime.UtcNow.AddDays(-7), DateTime.UtcNow, 86400);

                                }

                            }

                            catch (Exception ex)
                            {
                                Debug.WriteLine(ex.Message);
                                Logger.AddLog(true, "X24" + ex.ToString());
                                ShowViewModel<EventDataRangeHistoryViewModel>(new { pop = "pop" });
                                ACUserDialogs.ShowAlert(AppResources.please_try_again);
                            }

                            _batteryUsageSummaryViewModel.LoadList();
                            _chargeSummaryViewModel.LoadList();
                            _exceptionsViewModel.LoadList();
                        }
                        else
                        {
                            ShowViewModel<EventDataRangeHistoryViewModel>(new { pop = "pop" });
                            ACUserDialogs.ShowAlert(AppResources.please_try_again);

                        }
                        ACUserDialogs.HideProgress();
                    });
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    ACUserDialogs.HideProgress();
                }
            }
            else
            {
                _batteryUsageSummaryViewModel.LoadList();
                _chargeSummaryViewModel.LoadList();
                _exceptionsViewModel.LoadList();
                ACUserDialogs.ShowAlert(AppResources.dev_in_progress);
            }

            TabsViewModels.Add(_batteryUsageSummaryViewModel);
            TabsViewModels.Add(_chargeSummaryViewModel);
            TabsViewModels.Add(_historyChartViewModel);
            TabsViewModels.Add(_exceptionsViewModel);
            TabsViewModels.Add(_batteryDailyUsageViewModel);
        }

        public IMvxPagedViewModel GetDefaultViewModel()
        {
            return (_batteryUsageSummaryViewModel);
        }

        public IMvxPagedViewModel GetNextViewModel(IMvxPagedViewModel currentViewModel)
        {
            if (currentViewModel is BatteryUsageSummaryViewModel)
                return (_chargeSummaryViewModel);
            else if (currentViewModel is ChargeSummaryViewModel)
                return (_historyChartViewModel);
            else if (currentViewModel is HistoryChartsViewModel)
                return (_exceptionsViewModel);
            else if (currentViewModel is ExceptionsViewModel)
                return (_batteryDailyUsageViewModel);
            return (null);

        }

        public IMvxPagedViewModel GetPreviousViewModel(IMvxPagedViewModel currentViewModel)
        {
            if (currentViewModel is BatteryDailyUsageViewModel)
                return (_exceptionsViewModel);
            else if (currentViewModel is ExceptionsViewModel)
                return (_historyChartViewModel);
            else if (currentViewModel is HistoryChartsViewModel)
                return (_chargeSummaryViewModel);
            else if (currentViewModel is ChargeSummaryViewModel)
                return (_batteryUsageSummaryViewModel);
            return (null);
        }

        void AssignDates(string eventDataRange)
        {
            switch (eventDataRange)
            {
                case "This Week":
                    Batt_ReadEventsHistoryEndDate.Date = DateTime.Now.Date;
                    Batt_ReadEventsHistoryStartDate.Date = DateTime.Now.Date.AddDays(-7);
                    break;
                case "Last Week":
                    Batt_ReadEventsHistoryEndDate.Date = DateTime.Now.Date.AddDays(-7);
                    Batt_ReadEventsHistoryStartDate.Date = DateTime.Now.Date.AddDays(-14);
                    break;
                case "This Month":
                    Batt_ReadEventsHistoryEndDate.Date = DateTime.Now.Date;
                    Batt_ReadEventsHistoryStartDate.Date = DateTime.Now.Date.AddMonths(-1);
                    break;
                case "Last Month":
                    Batt_ReadEventsHistoryEndDate.Date = DateTime.Now.Date.AddMonths(-1);
                    Batt_ReadEventsHistoryStartDate.Date = DateTime.Now.Date.AddMonths(-2);
                    break;
                case "Last 3 Months":
                    Batt_ReadEventsHistoryEndDate.Date = DateTime.Now.Date;
                    Batt_ReadEventsHistoryStartDate.Date = DateTime.Now.Date.AddMonths(-3);
                    break;
                case "Last 6 Months":
                    Batt_ReadEventsHistoryEndDate.Date = DateTime.Now.Date;
                    Batt_ReadEventsHistoryStartDate.Date = DateTime.Now.Date.AddMonths(-6);
                    break;
                case "Last 12 Months":
                    Batt_ReadEventsHistoryEndDate.Date = DateTime.Now.Date;
                    Batt_ReadEventsHistoryStartDate.Date = DateTime.Now.Date.AddMonths(-12);
                    break;
                default:
                    break;
            }
        }
    }
}