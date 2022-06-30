using System;
using System.Collections.Generic;

namespace actchargers
{
    public class ExceptionsViewModel : BaseViewModel, IMvxPagedViewModel
    {
        public ListViewItem Batt_report_ex_missedEQs;
        public ListViewItem Batt_report_ex_dischargeLimitExceeded;
        public ListViewItem Batt_report_ex_ahr_return;
        public ListViewItem Batt_report_ex_deep_discharge; 
        public ExceptionsViewModel()
        {
            ViewTitle = AppResources.exceptions;

            _exceptionsItemSource = new List<ListViewItem>();
            CreateList();
        }

        private List<ListViewItem> _exceptionsItemSource;

        public List<ListViewItem> ExceptionsItemSource
        {
            get { return _exceptionsItemSource; }
            set
            {
                _exceptionsItemSource = value;
                RaisePropertyChanged(() => ExceptionsItemSource);
            }
        }

        public string PagedViewId
        {
            get
            {
                return AppResources.exceptions;
            }
        }

        void CreateList()
        {
            this.ExceptionsItemSource.Clear();
            Batt_report_ex_missedEQs = new ListViewItem
            {
                Title = AppResources.missed_eq,
                Title2 = AppResources.misseded_finishes,
                DefaultCellType = ACUtility.CellTypes.TwoLabel
            };
            Batt_report_ex_dischargeLimitExceeded = new ListViewItem
            {
                Title = AppResources.discharge_limit_exceeded,
                Title2 = AppResources.over_temperature,
                DefaultCellType = ACUtility.CellTypes.TwoLabel
            };
            Batt_report_ex_ahr_return = new ListViewItem
            {
                Title = AppResources.ahr_return,
                Title2 = AppResources.current_sense_issue,
                DefaultCellType = ACUtility.CellTypes.TwoLabel
            };
            Batt_report_ex_deep_discharge = new ListViewItem
            {
                Title = AppResources.deep_discharge,
                SubTitle = "Yes",
                DefaultCellType = ACUtility.CellTypes.TwoLabel
            };
        }

        public void LoadList()
        {
            ExceptionsItemSource.Add(Batt_report_ex_missedEQs);
            ExceptionsItemSource.Add(Batt_report_ex_dischargeLimitExceeded);
            ExceptionsItemSource.Add(Batt_report_ex_ahr_return);
            ExceptionsItemSource.Add(Batt_report_ex_deep_discharge);
            RaisePropertyChanged(() => ExceptionsItemSource);
        }
    }
}