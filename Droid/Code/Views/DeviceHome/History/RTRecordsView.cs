using Android.OS;
using Android.Views;
using Android.Widget;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Core.ViewModels;

namespace actchargers.Droid
{
    public class RTRecordsView : BaseFragmentWithOutMenu, ExpandableListView.IOnChildClickListener
	{
		private View view;
		public BaseActivity activity;
		private CiExpandableListView mListView;
		RtRecordsViewModel _rtRecordsViewModel;
        RtRecordsSectionAdapter adapter;
		public override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			HasOptionsMenu = true;
			// Create your fragment here
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			base.OnCreateView(inflater, container, savedInstanceState);
			 view = this.BindingInflate(Resource.Layout.RTRecordsLayout, null);
            mListView = view.FindViewById<CiExpandableListView>(Resource.Id.section_list);
			activity = (BaseActivity)Activity;
			activity.SupportActionBar.SetDisplayHomeAsUpEnabled(true);
			activity.SupportActionBar.SetDisplayShowHomeEnabled(true);
			_rtRecordsViewModel = ViewModel as RtRecordsViewModel;
			//set actionbar title
			activity.UpdateTitle(_rtRecordsViewModel.ViewTitle);
            ((MvxNotifyPropertyChanged)this.ViewModel).PropertyChanged += OnPropertyChanged;
            //Setting adpter
            adapter = new RtRecordsSectionAdapter(view.Context, (IMvxAndroidBindingContext)BindingContext, _rtRecordsViewModel.RTRecordsViewItemSource, _rtRecordsViewModel);
            mListView.SetAdapter(adapter);
            mListView.SetOnChildClickListener(this);
            return view;

		}

        public bool OnChildClick(ExpandableListView parent, View clickedView, int groupPosition, int childPosition, long id)
        {
            ListViewItem item = (actchargers.ListViewItem)_rtRecordsViewModel.RTRecordsViewItemSource[groupPosition][childPosition];
            _rtRecordsViewModel.ExecuteChartSelectionCommand(item);
            return true;
        }

		public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
		{
			//clear activity menu items to add  specific fragment menu items
			base.OnCreateOptionsMenu(menu, inflater);

		}

        /// <summary>
        /// Ons the property changed.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        void OnPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("RTRecordsViewItemSource"))
            {
                mListView.ExpandAllGroups();
                adapter.ItemsSource = _rtRecordsViewModel.RTRecordsViewItemSource;
                adapter.NotifyDataSetChanged();

            }
            else if (e.PropertyName.Equals("EditingMode"))
            {
                activity.SupportInvalidateOptionsMenu();
            }
        }

		//constructs the options menu on right top of screen.
		public override bool OnOptionsItemSelected(IMenuItem item)
		{
			switch (item.ItemId)
			{
				//handling backpress of actionbar back here
				case Android.Resource.Id.Home:
					// this takes the user 'back', as if they pressed the left-facing triangle icon on the main android toolbar.
					activity.OnBackPressed();
					return true;

				default:
					return base.OnOptionsItemSelected(item);
			}

		}

	}
}
