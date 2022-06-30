using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using OxyPlot;
using OxyPlot.Series;

namespace actchargers
{
    public class QuickViewModel : BaseViewModel
    {
        BattViewObject activeBattView;
        QuickView quickView;

        private ObservableCollection<ChartViewItem> _quickViewItemSource;
        public ObservableCollection<ChartViewItem> QuickViewItemSource
        {
            get { return _quickViewItemSource; }
            set
            {
                _quickViewItemSource = value;
                RaisePropertyChanged(() => QuickViewItemSource);
            }
        }

        ACTimer quickViewTimer;

        public string AHRPlaceholder { get; set; }
        private string _aHr;
        public string AHR
        {
            get { return _aHr; }
            set
            {
                _aHr = value;
                //Use raise property if it dynamically chages from backdend
                RaisePropertyChanged(() => AHR);
            }
        }

        public string KWHRPlaceholder { get; set; }
        private string _kWHr;
        public string KWHR
        {
            get { return _kWHr; }
            set
            {
                _kWHr = value;
                //Use raise property if it dynamically chages from backdend
                RaisePropertyChanged(() => KWHR);
            }
        }

        private string _IdlePlaceholder;
        public string IdlePlaceholder
        {
            get
            {
                return _IdlePlaceholder;
            }
            set
            {
                _IdlePlaceholder = value;
                RaisePropertyChanged(() => IdlePlaceholder);

            }
        }


        private string _idleHour;
        public string IdleHour
        {
            get { return _idleHour; }
            set
            {
                _idleHour = value;
                //Use raise property if it dynamically chages from backdend
                RaisePropertyChanged(() => IdleHour);
            }
        }


        public string HrTitle { get; set; }

        private string _idleMin;
        public string IdleMin
        {
            get { return _idleMin; }
            set
            {
                _idleMin = value;
                //Use raise property if it dynamically chages from backdend
                RaisePropertyChanged(() => IdleMin);
            }
        }
        public QuickViewModel()
        {
            ViewTitle = AppResources.quick_view;
            if (ControlObject.UserAccess.Batt_realDataCharts == AccessLevelConsts.noAccess)
            {
                ACUserDialogs.ShowAlertWithOkButtonsAction(AppResources.no_data_found, () => { ShowViewModel<QuickViewModel>(new { pop = "pop" }); });
                return;
            }

            HrTitle = AppResources.hour_title;

            ConnectionManager connectionManager = BattViewQuantum.Instance.GetConnectionManager();
            activeBattView = connectionManager.activeBattView;
            quickView = activeBattView.quickView;

            CreateDataForBATTView();
        }

        /// <summary>
        /// Creates the data for BATTView.
        /// </summary>
        void CreateDataForBATTView()
        {

            AHRPlaceholder = AppResources.ahr;
            KWHRPlaceholder = AppResources.kwhr;


            List<object> arguments = new List<object>
            {
                BattViewCommunicationTypes.quickViewDirect,
                null
            };
            Task.Run(async () => await InitQuickView(arguments));

            quickViewTimer = new ACTimer(RefreshQuickView, null, 5000, 5000);
        }

        async Task InitQuickView(List<object> arguments)
        {
            ACUserDialogs.ShowProgress();
            var result = await BattViewQuantum.Instance.CommunicateBATTView(arguments);

            if (result.Count > 0)
            {
                if (result[2].Equals(CommunicationResult.OK))
                {
                    try
                    {
                        Batt_loadQuickView();
                    }
                    catch (Exception ex)
                    {

                        Debug.WriteLine(ex.Message);
                    }
                }
            }
            else
            {
                InvokeOnMainThread(() =>
                {
                    ACUserDialogs.ShowAlert(AppResources.please_try_again);
                });
            }
            ACUserDialogs.HideProgress();
        }

        async void RefreshQuickView(object state)
        {
            List<object> arguments = new List<object>
            {
                BattViewCommunicationTypes.quickViewDirect,
                null
            };
            Debug.WriteLine("Timer Started");

            var result = await BattViewQuantum.Instance.CommunicateBATTView(arguments);

            if (result.Count > 0)
            {
                if (result[2].Equals(CommunicationResult.OK))
                {
                    try
                    {
                        Batt_loadQuickView(true);

                        Debug.WriteLine("Timer Completed");
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }
                }
            }
            Debug.WriteLine("Timer Completed");
        }

        void Batt_loadQuickView(bool refresh = false)
        {
            AHR = (BattViewQuantum.Instance.GetBATTView().quickView.event_AS / 3600).ToString("N1").TrimEnd('0').TrimEnd(System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator[0]);
            KWHR = (BattViewQuantum.Instance.GetBATTView().quickView.event_WS / 3600000).ToString("N1").TrimEnd('0').TrimEnd(System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator[0]);
            switch (BattViewQuantum.Instance.GetBATTView().quickView.event_type)
            {
                case 3:
                    IdlePlaceholder = "In Use";

                    break;

                case 2:
                    IdlePlaceholder = "Idle";

                    break;

                case 1:
                    IdlePlaceholder = "Charge";

                    break;
            }

            IdleHour = BattViewQuantum.Instance.GetBATTView().quickView.duration.TotalHours < 1 ? "0" : BattViewQuantum.Instance.GetBATTView().quickView.duration.TotalHours.ToString("##");
            IdleMin = BattViewQuantum.Instance.GetBATTView().quickView.duration.Minutes.ToString("00") + " " + AppResources.min;

            _quickViewItemSource = new ObservableCollection<ChartViewItem>
            {
                GeneratePlotPoints_Soc(),
                GeneratePlotPoints_Current(),
                GeneratePlotPoints_Volt(),
                GeneratePlotPoints_Temp()
            };

            if (!refresh)
            {
                if (BattViewQuantum.Instance.GetBATTView().quickView.mainSenseErrorCode == 0)
                {
                }
                else
                {
                    List<string> errors = new List<string>();
                    if ((BattViewQuantum.Instance.GetBATTView().quickView.mainSenseErrorCode & 0x03) != 0)
                    {
                        switch ((BattViewQuantum.Instance.GetBATTView().quickView.mainSenseErrorCode & 0x03))
                        {
                            case 0x03:
                                errors.Add("SOC High Error");
                                break;
                            case 0x02:
                                errors.Add("SOC Medium Error");
                                break;
                            case 0x01:
                                errors.Add("SOC Low Error");
                                break;
                        }
                    }
                    if ((BattViewQuantum.Instance.GetBATTView().quickView.mainSenseErrorCode & 0x04) != 0)
                    {
                        errors.Add("Too long Events");
                    }
                    if ((BattViewQuantum.Instance.GetBATTView().quickView.mainSenseErrorCode & 0x08) != 0)
                    {
                        errors.Add("Current Not calibrated");
                    }
                    if ((BattViewQuantum.Instance.GetBATTView().quickView.mainSenseErrorCode & 0x10) != 0)
                    {
                        errors.Add("Voltage Not calibrated");

                    }
                    if ((BattViewQuantum.Instance.GetBATTView().quickView.mainSenseErrorCode & 0x20) != 0)
                    {
                        errors.Add("Voltage Value is off");

                    }
                    if ((BattViewQuantum.Instance.GetBATTView().quickView.mainSenseErrorCode & 0x40) != 0)
                    {
                        errors.Add("External Temperature Sensor Damaged");

                    }
                    if ((BattViewQuantum.Instance.GetBATTView().quickView.mainSenseErrorCode & 0x80) != 0)
                    {
                        errors.Add("intercell Temperature Sensor Damaged");
                    }
                    InvokeOnMainThread(() =>
                    {
                        ACUserDialogs.ShowAlert(string.Join(", ", errors));
                    });
                }
            }

            RaisePropertyChanged(() => QuickViewItemSource);
        }

        ChartViewItem GeneratePlotPoints_Soc()
        {
            Contract.Ensures(Contract.Result<PlotModel>() != null);

            double value = quickView.soc;
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

        ChartViewItem GeneratePlotPoints_Current()
        {
            Contract.Ensures(Contract.Result<PlotModel>() != null);

            double value = Math.Abs(quickView.current);
            double max = activeBattView.Config.ahrcapacity;

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

        ChartViewItem GeneratePlotPoints_Volt()
        {
            Contract.Ensures(Contract.Result<PlotModel>() != null);

            double value = quickView.voltage;
            double nominalvoltage = activeBattView.Config.nominalvoltage;
            double numberOfcells = nominalvoltage / 2;
            double eqVoltage = activeBattView.Config.EQvoltage;

            double max = numberOfcells * eqVoltage / 100;

            string valueLabel = value <= 0 ? "0" : value.ToString("N1");
            valueLabel += " " + AppResources.v_chart;

            ChartViewItem ch = new ChartViewItem
            {
                ChartType = "Voltage",
                ChartImageName = "voltage",
                Text = valueLabel,
                PlotObject = new PlotModel()
            };

            var series = BuildPieSeries();

            series.Slices.Add(new PieSlice(valueLabel, value)
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

        ChartViewItem GeneratePlotPoints_Temp()
        {
            Contract.Ensures(Contract.Result<PlotModel>() != null);

            bool isFahrenheit = Convert.ToBoolean(activeBattView.Config.temperatureFormat);

            double currentTemp = quickView.temperature;
            double value = TemperatureManager.GetCorrectTemperature(currentTemp, isFahrenheit);
            double maxTemp = activeBattView.Config.batteryHighTemperature / 10.0;
            double max = TemperatureManager.GetCorrectTemperature(maxTemp, isFahrenheit);

            string valueLabel = value <= 0 ? "0" : value.ToString("####");
            valueLabel += " " + TemperatureManager.GetCorrectMark(isFahrenheit);

            ChartViewItem ch = new ChartViewItem
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

            return ch;
        }

        PieSeries BuildPieSeries()
        {
            var series = new PieSeries
            {
                FontSize = 10,
                Diameter = .7,
                InnerDiameter = .55,
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

        /// <summary>
        /// Ons the back button click.
        /// </summary>
        public void OnBackButtonClick()
        {
            ClearTimer();
            ShowViewModel<QuickViewModel>(new { pop = "pop" });
        }

        /// <summary>
        /// Clears the timer.
        /// </summary>
        public void ClearTimer()
        {
            if (quickViewTimer != null)
            {
                quickViewTimer.Cancel();
                quickViewTimer.Dispose();
                quickViewTimer = null;
            }
        }
    }
}