using System;
using System.Collections.Generic;

namespace actchargers
{
    public class ChargeSummaryViewModel : BaseViewModel, IMvxPagedViewModel
    {
        public ListViewItem Batt_report_cs_days;
        public ListViewItem Batt_report_cs_missedFinishes;
        public ListViewItem Batt_report_cs_charge_ahr;
        public ListViewItem Batt_report_cs_max_temp;
        public ListViewItem Batt_report_eqs;
        public ChargeSummaryViewModel()
        {
            ViewTitle = AppResources.charge_summary;

            _chargeSummaryItemSource = new List<ListViewItem>();
            CreateList();
        }

        private List<ListViewItem> _chargeSummaryItemSource;

        public List<ListViewItem> ChargeSummaryItemSource
        {
            get { return _chargeSummaryItemSource; }
            set
            {
                _chargeSummaryItemSource = value;
                RaisePropertyChanged(() => ChargeSummaryItemSource);
            }
        }

        public string PagedViewId
        {
            get
            {
                return AppResources.charge_summary;
            }
        }

        void CreateList()
        {
            this.ChargeSummaryItemSource.Clear();
            Batt_report_cs_days = new ListViewItem
            {
                Title = AppResources.days,
                Title2 = AppResources.charges,
                DefaultCellType = ACUtility.CellTypes.TwoLabel
            };
            Batt_report_cs_missedFinishes = new ListViewItem
            {
                Title = AppResources.misseded_finishes,
                Title2 = AppResources.missed_eq,
                DefaultCellType = ACUtility.CellTypes.TwoLabel
            };
            Batt_report_cs_charge_ahr = new ListViewItem
            {
                Title = AppResources.total_charge_ahr,
                Title2 = AppResources.total_in_use_ahr,
                DefaultCellType = ACUtility.CellTypes.TwoLabel
            };
            Batt_report_cs_max_temp = new ListViewItem
            {
                Title = AppResources.max_temperature,
                Title2 = AppResources.min_soc,
                SubTitle2 = AppResources.no,
                DefaultCellType = ACUtility.CellTypes.TwoLabel
            };
            Batt_report_eqs = new ListViewItem
            {
                Title = AppResources.eqs,
                Title2 = AppResources.eqs_with_water,
                DefaultCellType = ACUtility.CellTypes.TwoLabel
            };
        }

        public void LoadList()
        {
            ChargeSummaryItemSource.Add(Batt_report_cs_days);
            ChargeSummaryItemSource.Add(Batt_report_cs_missedFinishes);
            ChargeSummaryItemSource.Add(Batt_report_cs_charge_ahr);
            ChargeSummaryItemSource.Add(Batt_report_cs_max_temp);
            ChargeSummaryItemSource.Add(Batt_report_eqs);
            RaisePropertyChanged(() => ChargeSummaryItemSource);
        }
    }
}
