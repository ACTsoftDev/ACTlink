using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using MvvmCross.Core.ViewModels;

namespace actchargers
{
    public class EventsDateRangeViewModel : BaseViewModel
    {
        ListViewItem Batt_ReadEventsHistoryStartDate;
        ListViewItem Batt_ReadEventsHistoryEndDate;

        ObservableCollection<TableHeaderItem> _eventsDateViewItemSource;
        public ObservableCollection<TableHeaderItem> EventsDateRangeViewItemSource
        {
            get { return _eventsDateViewItemSource; }
            set
            {
                _eventsDateViewItemSource = value;
                RaisePropertyChanged(() => EventsDateRangeViewItemSource);
            }

        }

        string _viewEventsText;
        public string ViewEventsText
        {
            get { return _viewEventsText; }
            set
            {
                _viewEventsText = value;
                RaisePropertyChanged(() => ViewEventsText);
            }
        }

        public EventsDateRangeViewModel()
        {
            ViewTitle = AppResources.events_date_range;
            EventsDateRangeViewItemSource = new ObservableCollection<TableHeaderItem>();

            if (BattViewEventsHistoryAccessApply() == 0)
            {
                ACUserDialogs.ShowAlertWithOkButtonsAction(AppResources.no_data_found, () => { ShowViewModel<EventsDateRangeViewModel>(new { pop = "pop" }); });
                return;
            }

            CreateList();
            ViewEventsText = AppResources.view_events;
        }

        MvxCommand<ListViewItem> _mItemClickCommand;

        public ICommand ItemClickCommand
        {
            get
            {
                return this._mItemClickCommand ?? (this._mItemClickCommand = new MvxCommand<ListViewItem>(this.ExecuteItemClickCommnad));
            }
        }

        public void ExecuteItemClickCommnad(ListViewItem item)
        {
            if (item.ViewModelType == null)
            {
                return;
            }
            if (item.CellType == ACUtility.CellTypes.Label && item.ViewModelType == typeof(EventDataRangeHistoryViewModel))
            {
                ShowViewModel<EventDataRangeHistoryViewModel>(new { eventDataRange = "" + item.Title });
            }

        }
        public IMvxCommand ViewEventsButtonClickCommand
        {
            get { return new MvxCommand(OnViewEventsClick); }
        }

        void OnViewEventsClick()
        {
            string fromDate = Batt_ReadEventsHistoryStartDate.Date.ToString(ACConstants.DATE_TIME_FORMAT);
            string toDate = Batt_ReadEventsHistoryEndDate.Date.ToString(ACConstants.DATE_TIME_FORMAT);
            if (Batt_ReadEventsHistoryStartDate.Date >= Batt_ReadEventsHistoryEndDate.Date)
            {
                ACUserDialogs.ShowAlert(AppResources.startdate_greater_than_enddate);
            }
            else
            {
                int DateRange = (Batt_ReadEventsHistoryEndDate.Date - Batt_ReadEventsHistoryStartDate.Date).Days;
                if (DateRange <= 366)
                {
                    ShowViewModel<EventDataRangeHistoryViewModel>(new { eventDataRange = "Date Range", fromDate = fromDate, toDate = toDate });
                }
                else
                {
                    ACUserDialogs.ShowAlert("Date Range should be with in 1 year");
                }
            }
        }

        public void CreateList()
        {
            if (IsBattView)
                CreateListForBATTView();
            else
                CreateListForCharger();
        }

        void CreateListForBATTView()
        {
            this.EventsDateRangeViewItemSource.Clear();

            this._eventsDateViewItemSource.Add(new TableHeaderItem
            {
                SectionHeader = AppResources.preset_date_range,
            });
            this._eventsDateViewItemSource.Add(new TableHeaderItem
            {
                SectionHeader = AppResources.custom_date_range,
            });

            var thisWeekItem = (new ListViewItem
            {
                Title = AppResources.this_week,
                DefaultCellType = ACUtility.CellTypes.Label,
                ViewModelType = typeof(EventDataRangeHistoryViewModel)
            });
            this._eventsDateViewItemSource[0].Add(thisWeekItem);
            var lastWeekItem = (new ListViewItem
            {
                Title = AppResources.last_week,
                DefaultCellType = ACUtility.CellTypes.Label,
                ViewModelType = typeof(EventDataRangeHistoryViewModel)
            });
            this._eventsDateViewItemSource[0].Add(lastWeekItem);
            var thisMonthItem = (new ListViewItem
            {
                Title = AppResources.this_month,
                DefaultCellType = ACUtility.CellTypes.Label,
                ViewModelType = typeof(EventDataRangeHistoryViewModel)
            });
            this._eventsDateViewItemSource[0].Add(thisMonthItem);
            var lastMonthItem = (new ListViewItem
            {
                Title = AppResources.last_month,
                DefaultCellType = ACUtility.CellTypes.Label,
                ViewModelType = typeof(EventDataRangeHistoryViewModel)
            });
            this._eventsDateViewItemSource[0].Add(lastMonthItem);
            var lastThreeMonthsItem = (new ListViewItem
            {

                Title = AppResources.last_three_month,
                DefaultCellType = ACUtility.CellTypes.Label,
                ViewModelType = typeof(EventDataRangeHistoryViewModel)
            });
            this._eventsDateViewItemSource[0].Add(lastThreeMonthsItem);
            var lastSixMonthsItem = (new ListViewItem
            {

                Title = AppResources.last_six_month,
                DefaultCellType = ACUtility.CellTypes.Label,
                ViewModelType = typeof(EventDataRangeHistoryViewModel)
            });
            this._eventsDateViewItemSource[0].Add(lastSixMonthsItem);
            var lastTwelveMonthsItem = (new ListViewItem
            {

                Title = AppResources.last_twelve_month,
                DefaultCellType = ACUtility.CellTypes.Label,
                ViewModelType = typeof(EventDataRangeHistoryViewModel)
            });
            this._eventsDateViewItemSource[0].Add(lastTwelveMonthsItem);
            Batt_ReadEventsHistoryStartDate = (new ListViewItem
            {
                Title = AppResources.from,
                SubTitle = DateTime.Now.Date.ToString(ACConstants.DATE_TIME_FORMAT_IOS_UI),
                Text = DateTime.Now.Date.ToString(ACConstants.DATE_TIME_FORMAT_IOS_UI),
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.DatePicker,
                Date = DateTime.Now.AddYears(-1),
                ViewModelType = typeof(EventDataRangeHistoryViewModel),
            });
            this._eventsDateViewItemSource[1].Add(Batt_ReadEventsHistoryStartDate);
            Batt_ReadEventsHistoryEndDate = (new ListViewItem
            {
                Title = AppResources.to,
                SubTitle = DateTime.Now.Date.ToString(ACConstants.DATE_TIME_FORMAT_IOS_UI),
                Text = DateTime.Now.Date.ToString(ACConstants.DATE_TIME_FORMAT_IOS_UI),
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.DatePicker,
                Date = DateTime.Now,
                ViewModelType = typeof(EventDataRangeHistoryViewModel)
            });
            this._eventsDateViewItemSource[1].Add(Batt_ReadEventsHistoryEndDate);
            var viewEvents = (new ListViewItem
            {
                Title = AppResources.view_events,
                DefaultCellType = ACUtility.CellTypes.Button,
                ListSelectionCommand = ViewEventsButtonClickCommand,
                ViewModelType = typeof(EventDataRangeHistoryViewModel)
            });
            this._eventsDateViewItemSource[1].Add(viewEvents);
            Batt_LoadHistoryEdges();
            RaisePropertyChanged(() => EventsDateRangeViewItemSource);
        }

        void CreateListForCharger()
        {
        }

        void Batt_LoadHistoryEdges()
        {
            if (!BattViewQuantum.Instance.BattView_quickSerialNumberCheckStrict())
                return;

            BattViewObject activeBattView = BattViewQuantum.Instance.GetBATTView();

            if (ControlObject.UserAccess.Batt_canReadEventsByTime != AccessLevelConsts.noAccess)
            {
                if (activeBattView.eventsRecordsStartID != 0xFFFFFFFF)
                {
                    Batt_ReadEventsHistoryStartDate.IsEditable = true;
                    Batt_ReadEventsHistoryEndDate.IsEditable = true;

                    if (activeBattView.eventsRecordsLastIDTime.Subtract(new TimeSpan(7, 0, 0, 0)) > activeBattView.eventsRecordsStartIDTime)
                    {
                        Batt_ReadEventsHistoryStartDate.Date = activeBattView.eventsRecordsLastIDTime.Subtract(new TimeSpan(7, 0, 0, 0));
                    }
                    else
                    {
                        Batt_ReadEventsHistoryStartDate.Date = Batt_ReadEventsHistoryStartDate.MinDate;
                    }

                    Batt_ReadEventsHistoryStartDate.Text = Batt_ReadEventsHistoryStartDate.SubTitle = Batt_ReadEventsHistoryStartDate.Date.ToString(ACConstants.DATE_TIME_FORMAT_IOS_UI);

                    Batt_ReadEventsHistoryEndDate.Date = activeBattView.eventsRecordsLastIDTime.AddDays(1);

                    Batt_ReadEventsHistoryEndDate.Text = Batt_ReadEventsHistoryEndDate.SubTitle = Batt_ReadEventsHistoryEndDate.Date.ToString(ACConstants.DATE_TIME_FORMAT_IOS_UI);
                }
                else
                {
                    Batt_ReadEventsHistoryStartDate.MaxDate = DateTime.MaxValue.AddYears(-10);
                    Batt_ReadEventsHistoryStartDate.MinDate = DateTime.UtcNow.AddDays(-1);
                    Batt_ReadEventsHistoryStartDate.MaxDate = DateTime.UtcNow;
                    Batt_ReadEventsHistoryEndDate.MaxDate = DateTime.MaxValue.AddYears(-2);
                    Batt_ReadEventsHistoryEndDate.MinDate = DateTime.UtcNow.AddDays(-1);
                    Batt_ReadEventsHistoryEndDate.MaxDate = DateTime.UtcNow;
                    Batt_ReadEventsHistoryStartDate.IsEditable = false;
                    Batt_ReadEventsHistoryEndDate.IsEditable = false;
                    Batt_ReadEventsHistoryStartDate.Date = Batt_ReadEventsHistoryStartDate.MinDate;
                    Batt_ReadEventsHistoryEndDate.Date = Batt_ReadEventsHistoryEndDate.MaxDate;
                    Batt_ReadEventsHistoryStartDate.Text = Batt_ReadEventsHistoryStartDate.SubTitle = Batt_ReadEventsHistoryStartDate.Date.ToString(ACConstants.DATE_TIME_FORMAT_IOS_UI);
                    Batt_ReadEventsHistoryEndDate.Text = Batt_ReadEventsHistoryEndDate.SubTitle = Batt_ReadEventsHistoryEndDate.Date.ToString(ACConstants.DATE_TIME_FORMAT_IOS_UI);
                }
            }
        }

        int BattViewEventsHistoryAccessApply()
        {
            return ControlObject.UserAccess.Batt_canReadEventsByTime;
        }
    }
}
