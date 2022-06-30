using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using MvvmCross.Core.ViewModels;

namespace actchargers
{
    public class HistoryViewModel : BaseViewModel
    {
        ObservableCollection<ListViewItem> historyViewItemSource;
        public ObservableCollection<ListViewItem> HistoryViewItemSource
        {
            get { return historyViewItemSource; }
            set
            {
                historyViewItemSource = value;
                RaisePropertyChanged(() => HistoryViewItemSource);
            }
        }

        public HistoryViewModel()
        {
            ViewTitle = AppResources.history;
            HistoryViewItemSource = new ObservableCollection<ListViewItem>();
            CreateList();
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
            HistoryViewItemSource.Clear();

            historyViewItemSource.Add(new ListViewItem
            {
                Title = AppResources.cummulative_data,
                ViewModelType = typeof(CumulativeDataViewModel)
            });
            historyViewItemSource.Add(new ListViewItem
            {
                Title = AppResources.events_history,
                ViewModelType = typeof(EventsDateRangeViewModel)
            });

            if (CanAddRtRecords())
            {
                historyViewItemSource.Add(new ListViewItem
                {
                    Title = AppResources.rt_records,
                    ViewModelType = typeof(RtRecordsViewModel)
                });
            }

            RaisePropertyChanged(() => HistoryViewItemSource);
        }

        bool CanAddRtRecords()
        {
            return
                (
                    (ControlObject.UserAccess.Batt_canReadRTrecordsByTime != AccessLevelConsts.noAccess) ||
                    (ControlObject.UserAccess.Batt_canReadRTrecordsByID != AccessLevelConsts.noAccess)
                );
        }

        void CreateListForCharger()
        {
            HistoryViewItemSource.Clear();

            historyViewItemSource.Add(new ListViewItem
            {
                Title = AppResources.view_global_records,
                ViewModelType = typeof(ViewGlobalRecordsViewModel)
            });
            historyViewItemSource.Add(new ListViewItem
            {
                Title = AppResources.view_cycles_history,
                ViewModelType = typeof(ViewCyclesHistoryViewModel)
            });

            if (CanAddPowerSnapshots())
            {
                historyViewItemSource.Add(new ListViewItem
                {
                    Title = AppResources.power_snapshots,
                    ViewModelType = typeof(PowerSnapshotsViewModel)
                });
            }

            RaisePropertyChanged(() => HistoryViewItemSource);
        }

        bool CanAddPowerSnapshots()
        {
            return
                (
                    (ControlObject.isACTOem) ||
                    (ControlObject.isDebugMaster) ||
                    (ControlObject.UserAccess.MCB_onlyForEnginneringTeam != AccessLevelConsts.noAccess)
                );
        }

        MvxCommand<ListViewItem> m_SelectItemCommand;
        public ICommand SelectItemCommand
        {
            get
            {
                return m_SelectItemCommand ?? (m_SelectItemCommand = new MvxCommand<ListViewItem>(ExecuteSelectItemCommand));
            }
        }

        public string PagedViewId
        {
            get
            {
                return ("History");
            }
        }

        void ExecuteSelectItemCommand(ListViewItem item)
        {
            if (item.ViewModelType == typeof(CumulativeDataViewModel))
            {
                ShowViewModel<CumulativeDataViewModel>();
            }
            else if (item.ViewModelType == typeof(EventsDateRangeViewModel))
            {
                ShowViewModel<EventsDateRangeViewModel>();
            }
            else if (item.ViewModelType == typeof(RtRecordsViewModel))
            {
                ShowViewModel<RtRecordsViewModel>();
            }
            else if (item.ViewModelType == typeof(ViewGlobalRecordsViewModel))
            {
                ShowViewModel<ViewGlobalRecordsViewModel>();
            }
            else if (item.ViewModelType == typeof(ViewCyclesHistoryViewModel))
            {
                ShowViewModel<ViewCyclesHistoryViewModel>();
            }
            else if (item.ViewModelType == typeof(PowerSnapshotsViewModel))
            {
                ShowViewModel<PowerSnapshotsViewModel>();
            }
        }

        public override void OnBackButtonClick()
        {
            base.OnBackButtonClick();

            ShowViewModel<HistoryViewModel>(new { pop = "pop" });
        }
    }
}
