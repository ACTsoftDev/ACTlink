using Android.OS;
using Android.Views;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Binding.Droid.Views;
using MvvmCross.Core.ViewModels;

namespace actchargers.Droid
{
	public class ChargeSummaryView : BaseFragment
	{
		private ChargeSummaryViewModel _mhargeSummaryViewModel;
		private MvxListView mListView;
		private BaseActivity activity;


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
			View view = this.BindingInflate(Resource.Layout.ChargeSummaryLayout, null);
            ((MvxNotifyPropertyChanged)this.ViewModel).PropertyChanged += OnPropertyChanged;
			//inflating battery settings listview
			mListView = view.FindViewById<MvxListView>(Resource.Id.chargeSummaryList);
			//activity reference to set actionbar components
			activity = (BaseActivity)Activity;
			_mhargeSummaryViewModel = ViewModel as ChargeSummaryViewModel;
			var mAdapter = new CustomViewAdapter(this.Activity, (IMvxAndroidBindingContext)BindingContext, _mhargeSummaryViewModel, _mhargeSummaryViewModel.ChargeSummaryItemSource);
			mListView.Adapter = mAdapter;
			return view;
		}
        void OnPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("ChargeSummaryItemSource"))
            {
                mListView.InvalidateViews();

            }
        }
	}
}
