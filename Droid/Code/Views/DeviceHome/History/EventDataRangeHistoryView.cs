using System.Collections.Generic;
using Android.Content;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Views;
using Android.Widget;
using MvvmCross.Binding.Droid.BindingContext;

namespace actchargers.Droid
{
    public class EventDataRangeHistoryView : BaseFragmentWithOutMenu
	{
		private BaseActivity activity;
		private EventDataRangeHistoryViewModel _mEventDataRangeHistoryViewModel;
		private ViewPager _viewPager;
		private EventDataRangeFragmentAdapter _adapter;
		private TabLayout _tabLayout;
		/// <summary>
		///  tab titles.
		/// </summary>
		string[] _tabHeader = { AppResources.battery_usage_summary, AppResources.charge_summary, AppResources.charts,AppResources.exceptions,AppResources.battery_daily_usage };

		public override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			HasOptionsMenu = true;
			// Create your fragment here
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			// Use this to return your custom view for this Fragment
			// return inflater.Inflate(Resource.Layout.YourFragment, container, false);
			base.OnCreateView(inflater, container, savedInstanceState);
			View view = this.BindingInflate(Resource.Layout.EventDataRangeHistoryLayout, null);
			activity = (BaseActivity)Activity;
			activity.SupportActionBar.SetDisplayHomeAsUpEnabled(true);
			activity.SupportActionBar.SetDisplayShowHomeEnabled(true);
			_mEventDataRangeHistoryViewModel = ViewModel as EventDataRangeHistoryViewModel;
			//set actionbar title
			activity.UpdateTitle(_mEventDataRangeHistoryViewModel.ViewTitle);
			InitTabs(view);
			return view;
		}


		private void InitTabs(View view)
		{
			var fragments = new List<MvxViewPagerFragmentAdapter.FragmentInfo> {
				new MvxViewPagerFragmentAdapter.FragmentInfo {
					FragmentType = typeof(BatteryUsageSummaryView),
					ViewModel = _mEventDataRangeHistoryViewModel.TabsViewModels[0]
				},
				new MvxViewPagerFragmentAdapter.FragmentInfo {
					FragmentType = typeof(ChargeSummaryView),
					ViewModel = _mEventDataRangeHistoryViewModel.TabsViewModels[1]
				},
				new MvxViewPagerFragmentAdapter.FragmentInfo {
					FragmentType = typeof(HistoryChartsView),
					ViewModel = _mEventDataRangeHistoryViewModel.TabsViewModels[2]
				},new MvxViewPagerFragmentAdapter.FragmentInfo {
					FragmentType = typeof(ExceptionsView),
					ViewModel = _mEventDataRangeHistoryViewModel.TabsViewModels[3]
				},
           new MvxViewPagerFragmentAdapter.FragmentInfo {
					FragmentType = typeof(BatteryDailyUsageView),
					ViewModel = _mEventDataRangeHistoryViewModel.TabsViewModels[4]
				}
			};
			_viewPager = view.FindViewById<ViewPager>(Resource.Id.eventDataRangeViewPager);
			_adapter = new EventDataRangeFragmentAdapter(view.Context, ChildFragmentManager, fragments);
			_tabLayout = view.FindViewById<TabLayout>(Resource.Id.eventDataRangeTabsView);
			// Set adapter to view pager
			_viewPager.Adapter = _adapter;
			// Setup tablayout with view pager
			_tabLayout.SetupWithViewPager(_viewPager);

			// Iterate over all tabs and set the custom view
			for (int i = 0; i < _tabLayout.TabCount; i++)
			{
				TabLayout.Tab tab = _tabLayout.GetTabAt(i);
				tab.SetCustomView(_adapter.GetTabView(i, tab.IsSelected, _tabHeader[i]));
			}

			_tabLayout.TabSelected += (sender, e) =>
			{
				switch (e.Tab.Position)
				{
					case 0:
					case 1:
					case 2:
					case 3:
					case 4:
						activity.SupportInvalidateOptionsMenu();
						break;
					default:
						break;
				}
			};
		}

		public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
		{
			//clear activity menu items to add  specific fragment menu items
			menu.Clear();
			inflater.Inflate(Resource.Menu.event_data_range_history, menu);
			IMenuItem tab_count_view = menu.FindItem(Resource.Id.update_tab_count);
			//update current highlighted tab position to menu here
			tab_count_view.SetTitle(""+(_tabLayout.SelectedTabPosition+1)+"/"+_tabLayout.TabCount);

			base.OnCreateOptionsMenu(menu, inflater);

		}
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

		class EventDataRangeFragmentAdapter: MvxViewPagerFragmentAdapter
		{
			Context context;

			public EventDataRangeFragmentAdapter(Context context, Android.Support.V4.App.FragmentManager childFragmentManager, List<MvxViewPagerFragmentAdapter.FragmentInfo> fragments): base(context, childFragmentManager, fragments)
			{
				this.context = context;
			}
			public View GetTabView(int position, bool isSelected, string title)
			{
				LinearLayout layout = (LinearLayout)LayoutInflater.From(context).Inflate(Resource.Layout.CustomTabLayout, null);
				TextView tv = (TextView)layout.FindViewById(Resource.Id.tabTitle);
				tv.Text = title;
				return tv;
			}
		}
	}
}
