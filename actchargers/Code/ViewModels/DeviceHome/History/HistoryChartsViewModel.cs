using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using Newtonsoft.Json;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace actchargers
{
    public class SeriesObject
    {
        public string day;
        public double chargeSeriesValue;
        public double inUseSeriesValue;
        public double idleSeriesValue;
    }

    public class HistoryChartsViewModel : BaseViewModel, IMvxPagedViewModel
    {
        private ObservableCollection<ChartViewItem> _chartsViewItemSource;
        public ObservableCollection<ChartViewItem> ChartsViewItemSource
        {
            get { return _chartsViewItemSource; }
            set
            {
                _chartsViewItemSource = value;
                RaisePropertyChanged(() => ChartsViewItemSource);
            }
        }

        public string PagedViewId
        {
            get
            {
                return AppResources.history;
            }
        }


        public HistoryChartsViewModel()
        {
            ViewTitle = AppResources.history;

            ChargeinHoursSeriesList = new List<SeriesObject>();
            ChargeinAHRSeriesList = new List<SeriesObject>();
            ChargeinEBUSeriesList = new List<SeriesObject>();
        }


        ColumnSeries ChargeDurationSeries;
        ColumnSeries InUseDurationSeries;
        ColumnSeries IdleDurationSeries;
        ColumnSeries ChargeDurationinAHRSeries;
        ColumnSeries InUseDurationinAHRSeries;
        ColumnSeries IdleDurationinAHRSeries;
        ColumnSeries EBUSeries;

        CategoryAxis InHoursCategoryAccess;
        CategoryAxis InAHRCategoryAccess;
        CategoryAxis InEBUCategoryAccess;
        PlotModel UsageinHoursPlotModel;
        PlotModel UsageinAHRPlotModel;
        PlotModel UsageinEBUPlotModel;
        ChartViewItem UsageinHoursChartViewItem;
        ChartViewItem UsageinAHRChartViewItem;
        ChartViewItem UsageinEBUChartViewItem;

        List<SeriesObject> ChargeinHoursSeriesList;
        List<SeriesObject> ChargeinAHRSeriesList;
        List<SeriesObject> ChargeinEBUSeriesList;

        internal void SetChargeinHoursSeries(string day, double chargeDurationSeriesValue, double inUseDurationSeriesValue, double idleDurationSeriesValue)
        {
            InHoursCategoryAccess.Labels.Add(day);
            ChargeDurationSeries.Items.Add(new ColumnItem() { Value = chargeDurationSeriesValue });
            InUseDurationSeries.Items.Add(new ColumnItem() { Value = inUseDurationSeriesValue });
            IdleDurationSeries.Items.Add(new ColumnItem() { Value = idleDurationSeriesValue });

            SeriesObject obj = new SeriesObject();
            obj.day = day;
            obj.chargeSeriesValue = chargeDurationSeriesValue;
            obj.idleSeriesValue = idleDurationSeriesValue;
            obj.inUseSeriesValue = inUseDurationSeriesValue;
            ChargeinHoursSeriesList.Add(obj);
        }

        internal void SetChargeinAHRSeries(string day, double chargeDurationSeriesValue, double inUseDurationSeriesValue, double idleDurationSeriesValue)
        {
            InAHRCategoryAccess.Labels.Add(day);
            ChargeDurationinAHRSeries.Items.Add(new ColumnItem() { Value = chargeDurationSeriesValue });
            InUseDurationinAHRSeries.Items.Add(new ColumnItem() { Value = inUseDurationSeriesValue });
            IdleDurationinAHRSeries.Items.Add(new ColumnItem() { Value = idleDurationSeriesValue });

            SeriesObject obj = new SeriesObject();
            obj.day = day;
            obj.chargeSeriesValue = chargeDurationSeriesValue;
            obj.idleSeriesValue = idleDurationSeriesValue;
            obj.inUseSeriesValue = inUseDurationSeriesValue;
            ChargeinAHRSeriesList.Add(obj);
        }

        internal void SetChargeinEBUSeries(string day, double chargeDurationSeriesValue)
        {
            InEBUCategoryAccess.Labels.Add(day);
            EBUSeries.Items.Add(new ColumnItem() { Value = chargeDurationSeriesValue });

            SeriesObject obj = new SeriesObject();
            obj.day = day;
            obj.chargeSeriesValue = chargeDurationSeriesValue;
            ChargeinEBUSeriesList.Add(obj);
        }


        internal void LoadCharts()
        {
            UsageinHoursPlotModel.Axes.Add(InHoursCategoryAccess);
            UsageinHoursPlotModel.Series.Add(ChargeDurationSeries);
            UsageinHoursPlotModel.Series.Add(InUseDurationSeries);
            UsageinHoursPlotModel.Series.Add(IdleDurationSeries);

            UsageinAHRPlotModel.Axes.Add(InAHRCategoryAccess);
            UsageinAHRPlotModel.Series.Add(ChargeDurationinAHRSeries);
            UsageinAHRPlotModel.Series.Add(InUseDurationinAHRSeries);
            UsageinAHRPlotModel.Series.Add(IdleDurationinAHRSeries);

            UsageinEBUPlotModel.Axes.Add(InEBUCategoryAccess);
            UsageinEBUPlotModel.Series.Add(EBUSeries);

            CreateData();
        }

        internal void BATT_resetReportCharts(DateTime minDay, DateTime maxDay, uint maxdayEventsDuration)
        {
            
             
            InHoursCategoryAccess = new CategoryAxis { Position = AxisPosition.Bottom, FontSize = 10 };
            InHoursCategoryAccess.IsPanEnabled = true;

            UsageinHoursPlotModel = new PlotModel
            {
                Title = "Usage - In Hours",
                TitleColor = OxyColors.Green,
                TitleFontSize = 14,
                TitleFontWeight = 1,
                TitleHorizontalAlignment = TitleHorizontalAlignment.CenteredWithinPlotArea,
                LegendPlacement = LegendPlacement.Outside,
                LegendPosition = LegendPosition.TopLeft,
                LegendOrientation = LegendOrientation.Horizontal,
                LegendBorderThickness = 0,
                LegendFontSize = 10,
                LegendTextColor = OxyColors.Gray,
                PlotAreaBorderThickness = new OxyThickness(0, 1, 0, 1),
                PlotAreaBorderColor = OxyColors.Gray,
            };

            ChargeDurationSeries = new ColumnSeries { Title = "Charge Duration", StrokeColor = OxyColors.Transparent, StrokeThickness = 2, FillColor = OxyColors.BlueViolet };
            InUseDurationSeries = new ColumnSeries { Title = "In Use Duration", StrokeColor = OxyColors.Transparent, StrokeThickness = 2, FillColor = OxyColors.Orange };
            IdleDurationSeries = new ColumnSeries { Title = "Idle Duration", StrokeColor = OxyColors.Transparent, StrokeThickness = 2, FillColor = OxyColors.LightGray };

            var valueAxis = new LinearAxis { Position = AxisPosition.Left, Minimum = 0, Maximum = 24, IntervalLength = 4, MinorStep = 1, MajorStep = 4, MinimumPadding = 0, MaximumPadding = 0.06, AbsoluteMinimum = 0, IsZoomEnabled = false, IsPanEnabled = false, FontSize = 10 };
            UsageinHoursPlotModel.Axes.Add(valueAxis);

            InAHRCategoryAccess = new CategoryAxis { Position = AxisPosition.Bottom, FontSize = 10 };
            InAHRCategoryAccess.IsPanEnabled = true;

            UsageinAHRPlotModel = new PlotModel
            {
                Title = "Usage - In AHrs",
                TitleColor = OxyColors.DarkGreen,
                TitleFontSize = 14,
                TitleFontWeight = 1,
                TitleHorizontalAlignment = TitleHorizontalAlignment.CenteredWithinPlotArea,
                LegendPlacement = LegendPlacement.Outside,
                LegendPosition = LegendPosition.TopLeft,
                LegendOrientation = LegendOrientation.Horizontal,
                LegendBorderThickness = 0,
                LegendFontSize = 10,
                LegendTextColor = OxyColors.Gray,
                PlotAreaBorderThickness = new OxyThickness(0, 1, 0, 1),
                PlotAreaBorderColor = OxyColors.Gray,
            };

            ChargeDurationinAHRSeries = new ColumnSeries { Title = "Charge AHrs", StrokeColor = OxyColors.Transparent, StrokeThickness = 1, FillColor = OxyColors.DarkGreen };
            InUseDurationinAHRSeries = new ColumnSeries { Title = "In Use AHrs", StrokeColor = OxyColors.Transparent, StrokeThickness = 1, FillColor = OxyColors.DeepSkyBlue };
            IdleDurationinAHRSeries = new ColumnSeries { Title = "Available AHrs", StrokeColor = OxyColors.Transparent, StrokeThickness = 1, FillColor = OxyColors.Brown };

            var valueAxisAHR = new LinearAxis { Position = AxisPosition.Left, Minimum = 0, MinimumPadding = 0, MaximumPadding = 0.06, AbsoluteMinimum = 0, IsZoomEnabled = false, IsPanEnabled = false, FontSize = 10 };
            UsageinAHRPlotModel.Axes.Add(valueAxisAHR);

            InEBUCategoryAccess = new CategoryAxis { Position = AxisPosition.Bottom, FontSize = 10 };
            InEBUCategoryAccess.IsPanEnabled = true;

            UsageinEBUPlotModel = new PlotModel
            {
                Title = "Usage - EBUs",
                TitleColor = OxyColors.Blue,
                TitleFontSize = 14,
                TitleFontWeight = 1,
                TitleHorizontalAlignment = TitleHorizontalAlignment.CenteredWithinPlotArea,
                LegendPlacement = LegendPlacement.Outside,
                LegendPosition = LegendPosition.TopLeft,
                LegendOrientation = LegendOrientation.Horizontal,
                LegendBorderThickness = 0,
                LegendFontSize = 10,
                LegendTextColor = OxyColors.Gray,
                PlotAreaBorderThickness = new OxyThickness(0, 1, 0, 1),
                PlotAreaBorderColor = OxyColors.Gray,
            };

            EBUSeries = new ColumnSeries { Title = "Charge AHrs", StrokeColor = OxyColors.Transparent, StrokeThickness = 1, FillColor = OxyColors.Blue };

            var valueAxisEBU = new LinearAxis { Position = AxisPosition.Left, Minimum = 0, MinimumPadding = 0, MaximumPadding = 0.06, AbsoluteMinimum = 0, IsZoomEnabled = false, IsPanEnabled = false, FontSize = 10 };
            UsageinEBUPlotModel.Axes.Add(valueAxisEBU);
        }

        private void CreateData()
        {
            try
            {
                ChartsViewItemSource = new ObservableCollection<ChartViewItem>();

                InHoursCategoryAccess.Zoom(-0.5, 4.9);
                InAHRCategoryAccess.Zoom(-0.5, 4.9);
                InEBUCategoryAccess.Zoom(-0.5, 4.9);

                InHoursCategoryAccess.IsZoomEnabled = false;
                InAHRCategoryAccess.IsZoomEnabled = false;
                InEBUCategoryAccess.IsZoomEnabled = false;

                UsageinHoursChartViewItem = new ChartViewItem
                {
                    PlotObject = UsageinHoursPlotModel,
                    CellType = ACUtility.CellTypes.Plot,
                    SelectionCommand = PlotViewCommand
                };

                UsageinAHRChartViewItem = new ChartViewItem
                {
                    PlotObject = UsageinAHRPlotModel,
                    CellType = ACUtility.CellTypes.Plot,
                    SelectionCommand = PlotViewCommand
                };

                UsageinEBUChartViewItem = new ChartViewItem
                {
                    PlotObject = UsageinEBUPlotModel,
                    CellType = ACUtility.CellTypes.Plot,
                    SelectionCommand = PlotViewCommand
                };


                ChartsViewItemSource.Add(UsageinHoursChartViewItem);
                ChartsViewItemSource.Add(UsageinAHRChartViewItem);
                ChartsViewItemSource.Add(UsageinEBUChartViewItem);

                RaisePropertyChanged(() => ChartsViewItemSource);
            }
            catch (Exception ex)
            {
                var exception = ex.StackTrace;
            }
        }

        public IMvxCommand PlotViewCommand
        {
            get { return new MvxCommand<ChartViewItem>(ExecutePlotViewCommand); }
        }

        public void ExecutePlotViewCommand(ChartViewItem item)
        {
            if (item.PlotObject.Title == "Usage - In Hours")
                Mvx.Resolve<ICustomAlert>().ShowPlotView(GetUsageinHoursPlotModel());
            else if (item.PlotObject.Title == "Usage - In AHrs")
                Mvx.Resolve<ICustomAlert>().ShowPlotView(GetUsageinAHRPlotModel());
            else
                Mvx.Resolve<ICustomAlert>().ShowPlotView(GetUsageinEBUPlotModel());
        }

        PlotModel GetUsageinHoursPlotModel()
        {
            CategoryAxis InHoursCategoryAccessForView = new CategoryAxis { Position = AxisPosition.Bottom, FontSize = 10 };
            InHoursCategoryAccessForView.IsPanEnabled = true;

            PlotModel UsageinHoursPlotModelForView = new PlotModel
            {
                Title = "Usage - In Hours",
                TitleColor = OxyColors.Green,
                TitleFontSize = 14,
                TitleFontWeight = 1,
                TitleHorizontalAlignment = TitleHorizontalAlignment.CenteredWithinPlotArea,
                LegendPlacement = LegendPlacement.Outside,
                LegendPosition = LegendPosition.TopLeft,
                LegendOrientation = LegendOrientation.Horizontal,
                LegendBorderThickness = 0,
                LegendFontSize = 10,
                LegendTextColor = OxyColors.Gray,
                PlotAreaBorderThickness = new OxyThickness(0, 1, 0, 1),
                PlotAreaBorderColor = OxyColors.Gray,
            };

            ColumnSeries ChargeDurationSeriesForView = new ColumnSeries { Title = "Charge Duration", StrokeColor = OxyColors.Transparent, StrokeThickness = 2, FillColor = OxyColors.BlueViolet };
            ColumnSeries InUseDurationSeriesForView = new ColumnSeries { Title = "In Use Duration", StrokeColor = OxyColors.Transparent, StrokeThickness = 2, FillColor = OxyColors.Orange };
            ColumnSeries IdleDurationSeriesForView = new ColumnSeries { Title = "Idle Duration", StrokeColor = OxyColors.Transparent, StrokeThickness = 2, FillColor = OxyColors.LightGray };

            var valueAxis2 = new LinearAxis { Position = AxisPosition.Left, Minimum = 0, Maximum = 24, IntervalLength = 4, MinorStep = 1, MajorStep = 4, MinimumPadding = 0, MaximumPadding = 0.06, AbsoluteMinimum = 0, IsZoomEnabled = false, IsPanEnabled = false, FontSize = 10 };
            UsageinHoursPlotModelForView.Axes.Add(valueAxis2);

            foreach (SeriesObject obj in ChargeinHoursSeriesList)
            {
                InHoursCategoryAccessForView.Labels.Add(obj.day);
                ChargeDurationSeriesForView.Items.Add(new ColumnItem() { Value = obj.chargeSeriesValue });
                InUseDurationSeriesForView.Items.Add(new ColumnItem() { Value = obj.inUseSeriesValue });
                IdleDurationSeriesForView.Items.Add(new ColumnItem() { Value = obj.idleSeriesValue });
            }

            UsageinHoursPlotModelForView.Axes.Add(InHoursCategoryAccessForView);
            UsageinHoursPlotModelForView.Series.Add(ChargeDurationSeriesForView);
            UsageinHoursPlotModelForView.Series.Add(InUseDurationSeriesForView);
            UsageinHoursPlotModelForView.Series.Add(IdleDurationSeriesForView);

            InHoursCategoryAccessForView.Zoom(-0.5, 4.9);
            InHoursCategoryAccessForView.IsZoomEnabled = false;

            return UsageinHoursPlotModelForView;
        }

        PlotModel GetUsageinAHRPlotModel()
        {
            CategoryAxis InAHRCategoryAccessForView = new CategoryAxis { Position = AxisPosition.Bottom, FontSize = 10 };
            InAHRCategoryAccessForView.IsPanEnabled = true;

            PlotModel UsageinAHRPlotModelForView = new PlotModel
            {
                Title = "Usage - In AHrs",
                TitleColor = OxyColors.DarkGreen,
                TitleFontSize = 14,
                TitleFontWeight = 1,
                TitleHorizontalAlignment = TitleHorizontalAlignment.CenteredWithinPlotArea,
                LegendPlacement = LegendPlacement.Outside,
                LegendPosition = LegendPosition.TopLeft,
                LegendOrientation = LegendOrientation.Horizontal,
                LegendBorderThickness = 0,
                LegendFontSize = 10,
                LegendTextColor = OxyColors.Gray,
                PlotAreaBorderThickness = new OxyThickness(0, 1, 0, 1),
                PlotAreaBorderColor = OxyColors.Gray,
            };

            ColumnSeries ChargeDurationinAHRSeriesForView = new ColumnSeries { Title = "Charge AHrs", StrokeColor = OxyColors.Transparent, StrokeThickness = 1, FillColor = OxyColors.DarkGreen };
            ColumnSeries InUseDurationinAHRSeriesForView = new ColumnSeries { Title = "In Use AHrs", StrokeColor = OxyColors.Transparent, StrokeThickness = 1, FillColor = OxyColors.DeepSkyBlue };
            ColumnSeries IdleDurationinAHRSeriesForView = new ColumnSeries { Title = "Available AHrs", StrokeColor = OxyColors.Transparent, StrokeThickness = 1, FillColor = OxyColors.Brown };

            var valueAxisAHR2 = new LinearAxis { Position = AxisPosition.Left, Minimum = 0, MinimumPadding = 0, MaximumPadding = 0.06, AbsoluteMinimum = 0, IsZoomEnabled = false, IsPanEnabled = false, FontSize = 10 };
            UsageinAHRPlotModel.Axes.Add(valueAxisAHR2);

            foreach (SeriesObject obj in ChargeinAHRSeriesList)
            {
                InAHRCategoryAccessForView.Labels.Add(obj.day);
                ChargeDurationinAHRSeriesForView.Items.Add(new ColumnItem() { Value = obj.chargeSeriesValue });
                InUseDurationinAHRSeriesForView.Items.Add(new ColumnItem() { Value = obj.inUseSeriesValue });
                IdleDurationinAHRSeriesForView.Items.Add(new ColumnItem() { Value = obj.idleSeriesValue });
            }

            UsageinAHRPlotModelForView.Axes.Add(InAHRCategoryAccessForView);
            UsageinAHRPlotModelForView.Series.Add(ChargeDurationinAHRSeriesForView);
            UsageinAHRPlotModelForView.Series.Add(InUseDurationinAHRSeriesForView);
            UsageinAHRPlotModelForView.Series.Add(IdleDurationinAHRSeriesForView);

            InAHRCategoryAccessForView.Zoom(-0.5, 4.9);
            InAHRCategoryAccessForView.IsZoomEnabled = false;

            return UsageinAHRPlotModelForView;
        }

        PlotModel GetUsageinEBUPlotModel()
        {
            CategoryAxis InEBUCategoryAccessForView = new CategoryAxis { Position = AxisPosition.Bottom, FontSize = 10 };
            InEBUCategoryAccessForView.IsPanEnabled = true;

            PlotModel UsageinEBUPlotModelForView = new PlotModel
            {
                Title = "Usage - EBUs",
                TitleColor = OxyColors.Blue,
                TitleFontSize = 14,
                TitleFontWeight = 1,
                TitleHorizontalAlignment = TitleHorizontalAlignment.CenteredWithinPlotArea,
                LegendPlacement = LegendPlacement.Outside,
                LegendPosition = LegendPosition.TopLeft,
                LegendOrientation = LegendOrientation.Horizontal,
                LegendBorderThickness = 0,
                LegendFontSize = 10,
                LegendTextColor = OxyColors.Gray,
                PlotAreaBorderThickness = new OxyThickness(0, 1, 0, 1),
                PlotAreaBorderColor = OxyColors.Gray,
            };

            ColumnSeries EBUSeriesForView = new ColumnSeries { Title = "Charge AHrs", StrokeColor = OxyColors.Transparent, StrokeThickness = 1, FillColor = OxyColors.Blue };

            var valueAxisEBU2 = new LinearAxis { Position = AxisPosition.Left, Minimum = 0, MinimumPadding = 0, MaximumPadding = 0.06, AbsoluteMinimum = 0, IsZoomEnabled = false, IsPanEnabled = false, FontSize = 10 };
            UsageinEBUPlotModel.Axes.Add(valueAxisEBU2);

            foreach (SeriesObject obj in ChargeinEBUSeriesList)
            {
                InEBUCategoryAccessForView.Labels.Add(obj.day);
                EBUSeriesForView.Items.Add(new ColumnItem() { Value = obj.chargeSeriesValue });
            }

            UsageinEBUPlotModelForView.Axes.Add(InEBUCategoryAccessForView);
            UsageinEBUPlotModelForView.Series.Add(EBUSeriesForView);

            InEBUCategoryAccessForView.Zoom(-0.5, 4.9);
            InEBUCategoryAccessForView.IsZoomEnabled = false;

            return UsageinEBUPlotModelForView;
        }
    }
}
