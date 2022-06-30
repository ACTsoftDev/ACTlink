using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace actchargers
{
    public class BatteryDailyUsageViewModel : BaseViewModel, IMvxPagedViewModel
    {

		public string BATT_Daily_Usage_Date { get; set; }
		public string BATT_Daily_Usage_DayType { get; set; }
		public string BATT_Daily_Usage_Charges { get; set; }
		public string BATT_Daily_Usage_InUse { get; set; }
		public string BATT_Daily_Usage_Data { get; set; }

        private ObservableCollection<BATTDailyUsageModel> _DailyUsageItemSource;

        public ObservableCollection<BATTDailyUsageModel> DailyUsageItemSource
        {
            get { return _DailyUsageItemSource; }
            set
            {
                _DailyUsageItemSource = value;
                RaisePropertyChanged(() => DailyUsageItemSource);
            }
        }

        internal void loadList()
        {
            InvokeOnMainThread(()=>{
                RaisePropertyChanged(() => DailyUsageItemSource);
            });
        }

        public BatteryDailyUsageViewModel()
        {
			BATT_Daily_Usage_Date = AppResources.batt_daily_usage_date;
			BATT_Daily_Usage_Data = AppResources.batt_daily_usage_data;
			BATT_Daily_Usage_Charges = AppResources.batt_daily_usage_charges;
			BATT_Daily_Usage_InUse = AppResources.batt_daily_usage_inuse;
			BATT_Daily_Usage_DayType = AppResources.batt_daily_usage_day_type;

            ViewTitle = AppResources.battery_daily_usage;
            DailyUsageItemSource = new ObservableCollection<BATTDailyUsageModel>();
        }

        public string PagedViewId
        {
            get
            {
                return AppResources.battery_daily_usage;
            }
        }

    }
}
