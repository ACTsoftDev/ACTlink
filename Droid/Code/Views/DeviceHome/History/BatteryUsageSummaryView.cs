using Android.OS;
using Android.Views;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Core.ViewModels;

namespace actchargers.Droid
{
    public class BatteryUsageSummaryView : BaseFragment
	{
		private CiExpandableListView _mListView;
		private BaseActivity activity;
		private BatteryUsageSummaryViewModel _mBatteryUsageSummaryViewModel;
		public override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			// Create your fragment here
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			// Use this to return your custom view for this Fragment
			// return inflater.Inflate(Resource.Layout.YourFragment, container, false);
			base.OnCreateView(inflater, container, savedInstanceState);
			View view = this.BindingInflate(Resource.Layout.UsageSummaryLayout, null);
            ((MvxNotifyPropertyChanged)this.ViewModel).PropertyChanged += OnPropertyChanged;
			activity = (BaseActivity)Activity;
			_mListView = view.FindViewById<CiExpandableListView>(Resource.Id.section_list);
			_mBatteryUsageSummaryViewModel = ViewModel as BatteryUsageSummaryViewModel;
            CustomSectionEditAdapter adapter = new CustomSectionEditAdapter(view.Context, (IMvxAndroidBindingContext)BindingContext, _mBatteryUsageSummaryViewModel.UsageSummaryItemSource,_mBatteryUsageSummaryViewModel);
			_mListView.SetAdapter(adapter);

			return view;
		}
           void OnPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("UsageSummaryItemSource"))
            {
                _mListView.ExpandAllGroups();
                _mListView.InvalidateViews();

            }
        }
	}
}