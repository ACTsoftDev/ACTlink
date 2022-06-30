using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace actchargers
{
    public class BatteryUsageSummaryViewModel : BaseViewModel, IMvxPagedViewModel
    {


        public ListViewItem Batt_report_cus_charge_hrs;
        public ListViewItem Batt_report_cus_charge_ahrs;
        public ListViewItem Batt_report_dui_min_ahrs;
        public ListViewItem Batt_report_dui_min_hrs;
        public ListViewItem Batt_report_duue_avg_ebus;
        public ListViewItem Batt_report_haus_min_ahrs_hr;
        public ListViewItem Batt_report_dhafc_min_hrs;

        /// <summary>
        /// Gets or sets the usage summary item source.
        /// </summary>
        /// <value>The usage summary item source.</value>
        private ObservableCollection<TableHeaderItem> _usageSummaryItemSource;
        public ObservableCollection<TableHeaderItem> UsageSummaryItemSource
        {
            get { return _usageSummaryItemSource; }

            set
            {
                _usageSummaryItemSource = value;
                RaisePropertyChanged(() => UsageSummaryItemSource);
            }
        }

        public string PagedViewId
        {
            get
            {
                return AppResources.battery_usage_summary;
            }
        }

        public BatteryUsageSummaryViewModel()
        {
            ViewTitle = AppResources.battery_usage_summary;
            UsageSummaryItemSource = new ObservableCollection<TableHeaderItem>();
            CreateList();
        }
        private void CreateList()
        {
            this.UsageSummaryItemSource.Clear();
           

            Batt_report_cus_charge_hrs = new ListViewItem
            {
                ItemHeader = AppResources.in_hours,
                Title = AppResources.charge,
                Title2 = AppResources.in_use,
                Title3 = AppResources.idle,
                DefaultCellType = ACUtility.CellTypes.ThreeLabel
            };

            Batt_report_cus_charge_ahrs = new ListViewItem
            {
                ItemHeader = AppResources.in_ahrs,
                Title = AppResources.charge,
                Title2 = AppResources.in_use,
                Title3 = AppResources.idle,
                DefaultCellType = ACUtility.CellTypes.ThreeLabel
            };

            Batt_report_dui_min_hrs = new ListViewItem
            {
                ItemHeader = AppResources.in_hours,
                Title = AppResources.min,
                Title2 = AppResources.avg,
                Title3 = AppResources.max,
                DefaultCellType = ACUtility.CellTypes.ThreeLabel
            };

            Batt_report_dui_min_ahrs = new ListViewItem
            {
                ItemHeader = AppResources.in_ahrs,
                Title = AppResources.min,
                Title2 = AppResources.avg,
                Title3 = AppResources.max,
                DefaultCellType = ACUtility.CellTypes.ThreeLabel
            };

            Batt_report_duue_avg_ebus = new ListViewItem
            {
                ItemHeader = AppResources.in_ebus,
                Title = AppResources.avg,
                Title2 = AppResources.max,
                DefaultCellType = ACUtility.CellTypes.TwoLabel
            };

            Batt_report_dhafc_min_hrs = new ListViewItem
            {
                ItemHeader = AppResources.in_hours,
                Title = AppResources.min,
                Title2 = AppResources.avg,
                DefaultCellType = ACUtility.CellTypes.TwoLabel
            };

            Batt_report_haus_min_ahrs_hr = new ListViewItem
            {
                ItemHeader = AppResources.ahr_hour,
                Title = AppResources.min,
                Title2 = "Avg",
                DefaultCellType = ACUtility.CellTypes.ThreeLabel
            };
        }

        public void LoadList()
        {
            UsageSummaryItemSource.Add(new TableHeaderItem
            {
                SectionHeader = AppResources.cummulative_usage_summary,
            });
            this.UsageSummaryItemSource.Add(new TableHeaderItem
            {
                SectionHeader = AppResources.daily_usage,
            });
            this.UsageSummaryItemSource.Add(new TableHeaderItem
            {
                SectionHeader = AppResources.daily_ebu_usage,
            });
            this.UsageSummaryItemSource.Add(new TableHeaderItem
            {
                SectionHeader = AppResources.daily_cahrge_availablitiy,
            });
            this.UsageSummaryItemSource.Add(new TableHeaderItem
            {
                SectionHeader = AppResources.ahr_usage_per_hour,
            });

            UsageSummaryItemSource[0].Add(Batt_report_cus_charge_hrs);
            UsageSummaryItemSource[0].Add(Batt_report_cus_charge_ahrs);
            UsageSummaryItemSource[1].Add(Batt_report_dui_min_hrs);
            UsageSummaryItemSource[1].Add(Batt_report_dui_min_ahrs);
            UsageSummaryItemSource[2].Add(Batt_report_duue_avg_ebus);
            UsageSummaryItemSource[3].Add(Batt_report_dhafc_min_hrs);
            UsageSummaryItemSource[4].Add(Batt_report_haus_min_ahrs_hr);
            RaisePropertyChanged(() => UsageSummaryItemSource);
        }
    }
}