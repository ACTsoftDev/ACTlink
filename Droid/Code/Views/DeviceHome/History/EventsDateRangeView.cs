using Android.OS;
using Android.Views;
using Android.Widget;
using MvvmCross.Binding.Droid.BindingContext;

namespace actchargers.Droid
{
    public class EventsDateRangeView : BaseFragmentWithOutMenu,ExpandableListView.IOnChildClickListener
	{
		private View view;
		private EventsDateRangeViewModel _eventsDateRangeViewModel;
		private BaseActivity activity;
		private CiExpandableListView mListView;
		public override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			//Enabled options menu for this fragment
			HasOptionsMenu = true;
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			base.OnCreateView(inflater, container, savedInstanceState);
			view = this.BindingInflate(Resource.Layout.EventsDateRange, null);
			mListView = view.FindViewById<CiExpandableListView>(Resource.Id.section_list);
			activity = (BaseActivity)Activity;
			activity.SupportActionBar.SetDisplayHomeAsUpEnabled(true);
			activity.SupportActionBar.SetDisplayShowHomeEnabled(true);
			_eventsDateRangeViewModel = ViewModel as EventsDateRangeViewModel;
			//set actionbar title
			activity.UpdateTitle(_eventsDateRangeViewModel.ViewTitle);

            CustomSectionEditAdapter adapter = new CustomSectionEditAdapter(view.Context, (IMvxAndroidBindingContext)BindingContext, _eventsDateRangeViewModel.EventsDateRangeViewItemSource,_eventsDateRangeViewModel);
			mListView.SetOnChildClickListener(this);
			mListView.SetAdapter(adapter);

			return view;
		}
		public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
		{
			//clear activity menu items to add  specific fragment menu items
			base.OnCreateOptionsMenu(menu, inflater);

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

		public bool OnChildClick(ExpandableListView parent, View clickedView, int groupPosition, int childPosition, long id)
		{
			ListViewItem item = _eventsDateRangeViewModel.EventsDateRangeViewItemSource[groupPosition][childPosition];
			_eventsDateRangeViewModel.ExecuteItemClickCommnad(item);
			return true;
		}

	}
}
