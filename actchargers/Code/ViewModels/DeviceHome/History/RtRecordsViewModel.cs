using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;
using Newtonsoft.Json;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace actchargers
{
    public class RtRecordsViewModel : BaseViewModel
    {
        MvxSubscriptionToken _mListSelector;
        TableHeaderItem listSection;
        ListViewItem Batt_ReadRTsHistoryStartId;
        ListViewItem Batt_ReadRTsHistorySELECTDate;
        ListViewItem Batt_ReadRTsHistoryReadRecords;

        List<ListViewItem> plotChartItem;
        List<RealTimeRecord> rtRecords;

        private ObservableCollection<TableHeaderItem> _rtRecordsViewItemSource;
        public ObservableCollection<TableHeaderItem> RTRecordsViewItemSource
        {
            get { return _rtRecordsViewItemSource; }
            set
            {
                _rtRecordsViewItemSource = value;
                RaisePropertyChanged(() => RTRecordsViewItemSource);
            }
        }

        //PlotModel dummyPlot;
        //LineSeries dummyLineSeries;
        //double dummyMaximum = 100;
        //DateTimeAxis dummyAxisVoltage;

        //DateTimeAxis dateAxisVoltage;
        //DateTimeAxis dateAxisCurrent;
        //DateTimeAxis dateAxisTemperature;
        //DateTimeAxis dateAxisSOC;
        string hourFormat = "HH:mm";
        string dayTimeFormat = "dd/MM HH:mm:ss";
        string dayFormat = "dd/MM";

        DateTime MinDay;
        DateTime MaxDay;


        public RtRecordsViewModel()
        {
            RTRecordsViewItemSource = new ObservableCollection<TableHeaderItem>();
            ViewTitle = AppResources.rt_records_title;
            if (BattViewRTAccessApply() == 0)
            {
                ACUserDialogs.ShowAlertWithOkButtonsAction(AppResources.no_data_found, () => { ShowViewModel<RtRecordsViewModel>(new { pop = "pop" }); });
                return;
            }

            ChartType = "ChartName";
            rtRecords = new List<RealTimeRecord>();
            Title = AppResources.rt_records;
            SubTitle = AppResources.rt_records_subTitle;
            createData();
        }

        PlotModel CreateVoltagePlot()
        {
            DateTimeAxis dateAxisVoltage = new DateTimeAxis { Position = AxisPosition.Bottom, StringFormat = dayTimeFormat, Minimum = DateTimeAxis.ToDouble(MinDay), Maximum = DateTimeAxis.ToDouble(MaxDay), AbsoluteMinimum = DateTimeAxis.ToDouble(MinDay), AbsoluteMaximum = DateTimeAxis.ToDouble(MaxDay) };
            LinearAxis yAxisVoltage = new LinearAxis() { Position = AxisPosition.Left, Minimum = 0, IsZoomEnabled = false, IsPanEnabled = false };

            LineSeries lineSeriesVoltage = new LineSeries()
            {
                Color = OxyColors.SkyBlue,
                MarkerType = MarkerType.Circle,
                MarkerSize = 5,
                MarkerStroke = OxyColors.White,
                MarkerFill = OxyColors.SkyBlue,
                MarkerStrokeThickness = 1.5
            };

            dateAxisVoltage.Zoom(DateTimeAxis.ToDouble(MinDay.AddSeconds(-1)), DateTimeAxis.ToDouble(MinDay.AddSeconds(3)));
            dateAxisVoltage.IsZoomEnabled = true;

            PlotModel voltagePlot = new PlotModel { Title = AppResources.voltage };
            voltagePlot.Axes.Add(dateAxisVoltage);
            voltagePlot.Axes.Add(yAxisVoltage);

            LineSeries voltageLineSeries = lineSeriesVoltage;

            var rtRecordsSorted = rtRecords.OrderBy(o => o.timestamp);
            foreach (RealTimeRecord r in rtRecordsSorted)
            {
                Debug.WriteLine(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(r.timestamp).ToString(dayTimeFormat));

                voltageLineSeries.Points.Add(new DataPoint(DateTimeAxis.ToDouble(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(r.timestamp)), r.voltage / 100.0f));
            }
            voltagePlot.Series.Add(voltageLineSeries);

            return voltagePlot;
        }

        PlotModel CreateCurrentPlot()
        {
            DateTimeAxis dateAxisCurrent = new DateTimeAxis() { Position = AxisPosition.Bottom, StringFormat = dayTimeFormat, Minimum = DateTimeAxis.ToDouble(MinDay), Maximum = DateTimeAxis.ToDouble(MaxDay), AbsoluteMinimum = DateTimeAxis.ToDouble(MinDay), AbsoluteMaximum = DateTimeAxis.ToDouble(MaxDay) };
            LinearAxis yAxisCurrent = new LinearAxis() { Position = AxisPosition.Left, Minimum = 0, IsZoomEnabled = false, IsPanEnabled = false };

            LineSeries lineSeriesCurrent = new LineSeries()
            {
                Color = OxyColors.SkyBlue,
                MarkerType = MarkerType.Circle,
                MarkerSize = 5,
                MarkerStroke = OxyColors.White,
                MarkerFill = OxyColors.SkyBlue,
                MarkerStrokeThickness = 1.5
            };

            dateAxisCurrent.Zoom(DateTimeAxis.ToDouble(MinDay.AddSeconds(-1)), DateTimeAxis.ToDouble(MinDay.AddSeconds(3)));
            dateAxisCurrent.IsZoomEnabled = true;

            PlotModel currentPlot = new PlotModel { Title = AppResources.current };
            currentPlot.Axes.Add(dateAxisCurrent);
            currentPlot.Axes.Add(yAxisCurrent);

            LineSeries currentLineSeries = lineSeriesCurrent;

            var rtRecordsSorted = rtRecords.OrderBy(o => o.timestamp);
            foreach (RealTimeRecord r in rtRecordsSorted)
            {
                currentLineSeries.Points.Add(new DataPoint(DateTimeAxis.ToDouble(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(r.timestamp)), r.current / 10.0f));
            }
            currentPlot.Series.Add(currentLineSeries);

            return currentPlot;
        }

        PlotModel CreateTemperaturePlot()
        {
            DateTimeAxis dateAxisTemperature = new DateTimeAxis() { Position = AxisPosition.Bottom, StringFormat = dayTimeFormat, Minimum = DateTimeAxis.ToDouble(MinDay), Maximum = DateTimeAxis.ToDouble(MaxDay), AbsoluteMinimum = DateTimeAxis.ToDouble(MinDay), AbsoluteMaximum = DateTimeAxis.ToDouble(MaxDay) };
            LinearAxis yAxisTemperature = new LinearAxis() { Position = AxisPosition.Left, Minimum = 0, IsZoomEnabled = false, IsPanEnabled = false };

            LineSeries lineSeriesTemperature = new LineSeries()
            {
                Color = OxyColors.SkyBlue,
                MarkerType = MarkerType.Circle,
                MarkerSize = 5,
                MarkerStroke = OxyColors.White,
                MarkerFill = OxyColors.SkyBlue,
                MarkerStrokeThickness = 1.5
            };

            dateAxisTemperature.Zoom(DateTimeAxis.ToDouble(MinDay.AddSeconds(-1)), DateTimeAxis.ToDouble(MinDay.AddSeconds(3)));
            dateAxisTemperature.IsZoomEnabled = true;

            PlotModel temperaturePlot = new PlotModel { Title = AppResources.temperature };
            temperaturePlot.Axes.Add(dateAxisTemperature);
            temperaturePlot.Axes.Add(yAxisTemperature);

            LineSeries temperatureLineSeries = lineSeriesTemperature;

            var rtRecordsSorted = rtRecords.OrderBy(o => o.timestamp);
            foreach (RealTimeRecord r in rtRecordsSorted)
            {
                temperatureLineSeries.Points.Add(new DataPoint(DateTimeAxis.ToDouble(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(r.timestamp)), r.noTemperature ? 0 : ((r.temperature * 1.8) / 10.0f + 32)));
            }
            temperaturePlot.Series.Add(temperatureLineSeries);

            return temperaturePlot;
        }

        PlotModel CreateSOCPlot()
        {
            DateTimeAxis dateAxisSOC = new DateTimeAxis() { Position = AxisPosition.Bottom, StringFormat = dayTimeFormat, Minimum = DateTimeAxis.ToDouble(MinDay), Maximum = DateTimeAxis.ToDouble(MaxDay), AbsoluteMinimum = DateTimeAxis.ToDouble(MinDay), AbsoluteMaximum = DateTimeAxis.ToDouble(MaxDay) };
            LinearAxis yAxisSOC = new LinearAxis() { Position = AxisPosition.Left, Minimum = 0, IsZoomEnabled = false, IsPanEnabled = false };

            LineSeries lineSeriesSOC = new LineSeries()
            {
                Color = OxyColors.SkyBlue,
                MarkerType = MarkerType.Circle,
                MarkerSize = 5,
                MarkerStroke = OxyColors.White,
                MarkerFill = OxyColors.SkyBlue,
                MarkerStrokeThickness = 1.5
            };

            dateAxisSOC.Zoom(DateTimeAxis.ToDouble(MinDay.AddSeconds(-1)), DateTimeAxis.ToDouble(MinDay.AddSeconds(3)));
            dateAxisSOC.IsZoomEnabled = true;

            PlotModel socPlot = new PlotModel { Title = AppResources.soc };
            socPlot.Axes.Add(dateAxisSOC);
            socPlot.Axes.Add(yAxisSOC);

            LineSeries socLineSeries = lineSeriesSOC;

            var rtRecordsSorted = rtRecords.OrderBy(o => o.timestamp);
            foreach (RealTimeRecord r in rtRecordsSorted)
            {
                socLineSeries.Points.Add(new DataPoint(DateTimeAxis.ToDouble(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(r.timestamp)), r.soc));
            }
            socPlot.Series.Add(socLineSeries);

            return socPlot;
        }

        void createData()
        {
            try
            {
                _rtRecordsViewItemSource.Add(new TableHeaderItem());
                listSection = new TableHeaderItem();

                Batt_ReadRTsHistoryStartId = new ListViewItem()
                {
                    Title = "Start from",
                    DefaultCellType = ACUtility.CellTypes.LabelTextEdit,
                    Text = "1"
                };
                //_rtRecordsViewItemSource[0].Add(Batt_ReadRTsHistoryStartId);

                Batt_ReadRTsHistorySELECTDate = new ListViewItem()
                {
                    Title = "Start from",
                    DefaultCellType = ACUtility.CellTypes.ListSelector,
                    Text = "Last Hour",
                    ListSelectionCommand = ListSelectorCommand,
                    ListSelectorType = ACUtility.ListSelectorType.RTRecordsList
                };
                _rtRecordsViewItemSource[0].Add(Batt_ReadRTsHistorySELECTDate);

                Batt_ReadRTsHistoryReadRecords = new ListViewItem()
                {
                    Title = "Read Records",
                    DefaultCellType = ACUtility.CellTypes.Button,
                    ListSelectionCommand = SelectionCommand
                };
                _rtRecordsViewItemSource[0].Add(Batt_ReadRTsHistoryReadRecords);
                RaisePropertyChanged(() => RTRecordsViewItemSource);

                ListViewItem VoltageItem = new ListViewItem()
                {
                    Title = AppResources.voltage,
                    DefaultCellType = ACUtility.CellTypes.Label,
                    ListSelectionCommand = ChartSelectionCommand
                };
                listSection.Add(VoltageItem);

                ListViewItem currentItem = new ListViewItem()
                {
                    Title = AppResources.current,
                    DefaultCellType = ACUtility.CellTypes.Label,
                    ListSelectionCommand = ChartSelectionCommand
                };
                listSection.Add(currentItem);

                ListViewItem temperatureItem = new ListViewItem()
                {
                    Title = AppResources.temperature,
                    DefaultCellType = ACUtility.CellTypes.Label,
                    ListSelectionCommand = ChartSelectionCommand
                };
                listSection.Add(temperatureItem);

                ListViewItem socItem = new ListViewItem()
                {
                    Title = AppResources.soc,
                    DefaultCellType = ACUtility.CellTypes.Label,
                    ListSelectionCommand = ChartSelectionCommand
                };
                listSection.Add(socItem);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private string _chartType;
        public string ChartType
        {
            get { return _chartType; }
            set
            {
                _chartType = value;
                RaisePropertyChanged(() => ChartType);
            }
        }

        private string _title;
        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                RaisePropertyChanged(() => Title);
            }
        }

        private string _subTtile;
        public string SubTitle
        {
            get { return _subTtile; }
            set
            {
                _subTtile = value;
                RaisePropertyChanged(() => SubTitle);
            }
        }

        public IMvxCommand ChartSelectionCommand
        {
            get { return new MvxCommand<ListViewItem>(ExecuteChartSelectionCommand); }
        }


        public void ExecuteChartSelectionCommand(ListViewItem item)
        {
            if (item.Title == AppResources.voltage)
            {
                Mvx.Resolve<ICustomAlert>().ShowPlotView(CreateVoltagePlot());
            }
            else if (item.Title == AppResources.current)
            {
                Mvx.Resolve<ICustomAlert>().ShowPlotView(CreateCurrentPlot());
            }
            else if (item.Title == AppResources.temperature)
            {
                Mvx.Resolve<ICustomAlert>().ShowPlotView(CreateTemperaturePlot());
            }
            else if (item.Title == AppResources.soc)
            {
                Mvx.Resolve<ICustomAlert>().ShowPlotView(CreateSOCPlot());
            }
        }

        /// <summary>
        /// Gets the list selector command.
        /// </summary>
        /// <value>The list selector command.</value>
        public IMvxCommand ListSelectorCommand
        {
            get { return new MvxCommand(ExecuteListSelectorCommand); }
        }

        private void ExecuteListSelectorCommand()
        {
            _mListSelector = Mvx.Resolve<IMvxMessenger>().Subscribe<ListSelectorMessage>(OnListSelectorMessage);
            ShowViewModel<ListSelectorViewModel>(new { type = ACUtility.ListSelectorType.RTRecordsList, selectedItemIndex = 0, selectedChildPosition = 0 });
        }


        /// <summary>
        /// Gets the selection command.
        /// </summary>
        /// <value>The selection command.</value>
        public IMvxCommand SelectionCommand
        {
            get
            {
                return new MvxCommand(async () =>
                {
                    await ExecuteSelectionCommand();
                });
            }
        }


        private async Task ExecuteSelectionCommand()
        {

            //Read Records Button Click 
            string startValue = RTRecordsViewItemSource[0][0].Text;

            //string startValue = "Last hour";

            //To Make a Call to BattView to get the History details
            bool isDevInProgress = false;
            if (!isDevInProgress)
            {
                UInt32 startRecord = 0;
                bool byID = true;

                if (!BattViewQuantum.Instance.BattView_quickSerialNumberCheckStrict())
                    return;
                if (ControlObject.UserAccess.Batt_canReadRTrecordsByID == AccessLevelConsts.noAccess)
                {
                    byID = false;
                }
                if (byID && ControlObject.UserAccess.Batt_canReadRTrecordsByTime != AccessLevelConsts.noAccess)
                {
                    if (Batt_ReadRTsHistoryStartId.Text.Trim() == "")
                        byID = false;
                }
                if (byID)
                {
                    if (startRecord < BattViewQuantum.Instance.GetBATTView().realtimeRecordsStartID)
                    {
                        Batt_ReadRTsHistoryStartId.Text = BattViewQuantum.Instance.GetBATTView().realtimeRecordsStartID.ToString();
                    }
                    UInt32.TryParse(Batt_ReadRTsHistoryStartId.Text, out startRecord);

                }
                if (!byID && ControlObject.UserAccess.Batt_canReadRTrecordsByTime == AccessLevelConsts.noAccess)
                    return;
                DateTime startTime = DateTime.UtcNow;

                List<object> arguments = new List<object>();
                arguments.Add(startRecord);
                arguments.Add(byID);
                arguments.Add(startTime);
                arguments.Add(DateTime.UtcNow);
                arguments.Add(2);

                List<object> results = new List<object>();
                try
                {
                    ACUserDialogs.ShowProgress();
                    results = await BattViewQuantum.Instance.ReadHistoryRecords(arguments);
                    if (results.Count > 0)
                    {
                        var status = (CommunicationResult)results[2];
                        if (status == CommunicationResult.OK)
                        {
                            try
                            {
                                rtRecords = BattViewQuantum.Instance.GetBATTView().getRTRecords();
                                var rtRecordsSorted = rtRecords.OrderBy(o => o.timestamp);
                                MinDay = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(rtRecordsSorted.FirstOrDefault().timestamp).AddMinutes(-1);
                                MaxDay = DateTime.UtcNow;

                                Debug.WriteLine("rtRecords - " + JsonConvert.SerializeObject(rtRecords));

                                if (!RTRecordsViewItemSource.Contains(listSection))
                                {
                                    RTRecordsViewItemSource.Add(listSection);
                                    RaisePropertyChanged(() => RTRecordsViewItemSource);
                                }

                            }
                            catch (Exception ex)
                            {
                                Logger.AddLog(true, "X12" + ex.Message);
                                Debug.WriteLine(ex.Message);

                            }
                        }
                        else
                        {
                            ACUserDialogs.ShowAlert(AppResources.please_try_again);
                        }

                        ACUserDialogs.HideProgress();
                    }



                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    ACUserDialogs.ShowProgress();
                }
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
                try
                {
                    Mvx.Resolve<IMvxMessenger>().Unsubscribe<ListSelectorMessage>(_mListSelector);
                    _mListSelector = null;
                    RTRecordsViewItemSource[obj.SelectedItemindex][obj.SelectedChildItemindex].Text = obj.SelectedItem;
                    RaisePropertyChanged(() => RTRecordsViewItemSource);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
        }

        private int BattViewRTAccessApply()
        {
            //UIAccessControlUtility accessControlUtility = new UIAccessControlUtility();

            //accessControlUtility.doApplyAccessControl(ControlObject.user_access.Batt_canReadRTrecordsByID, Batt_ReadRTsHistoryLabel, null);
            //accessControlUtility.doApplyAccessControl(ControlObject.user_access.Batt_canReadRTrecordsByID, Batt_ReadRTsHistoryStartId, null);

            //accessControlUtility.doApplyAccessControl(ControlObject.user_access.Batt_canReadRTrecordsByTime, Batt_ReadRTsHistorySELECTDate, null);

            //if (accessControlUtility.getVisibleCount() == 0)
            //{
            //    //Hide the RT Events Button in History
            //}
            return ControlObject.UserAccess.Batt_canReadRTrecordsByTime;
        }

    }
}