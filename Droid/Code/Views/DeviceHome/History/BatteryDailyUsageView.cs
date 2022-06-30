using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Binding.Droid.Views;

namespace actchargers.Droid
{
	public class BatteryDailyUsageView : BaseFragment
	{
		private View view;
		private BaseActivity activity;
		private BatteryDailyUsageViewModel _mBatteryDailyUsageViewModel;
		public override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			// Create your fragment here
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			base.OnCreateView(inflater, container, savedInstanceState);
			view = this.BindingInflate(Resource.Layout.BatteryDailyUsageLayout, null);
			activity = (BaseActivity)Activity;
			activity.SupportActionBar.SetDisplayHomeAsUpEnabled(true);
			activity.SupportActionBar.SetDisplayShowHomeEnabled(true);
			_mBatteryDailyUsageViewModel = ViewModel as BatteryDailyUsageViewModel;
			//set actionbar title
			activity.UpdateTitle(_mBatteryDailyUsageViewModel.ViewTitle);
			var mListView = view.FindViewById<MvxListView>(Resource.Id.battery_usage_list);

			return view;

		}

	}
}

