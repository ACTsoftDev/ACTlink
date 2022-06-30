
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using MvvmCross.Binding.Droid.BindingContext;

namespace actchargers.Droid
{
	public class TestingBattViewView : BaseFragmentWithOutMenu
	{
		private View view;
		private TestingBattViewViewModel _testingBattViewViewModel;
		private BaseActivity activity;
		public override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			//Enabled options menu for this fragment
			HasOptionsMenu = true;		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			base.OnCreateView(inflater, container, savedInstanceState);
			view = this.BindingInflate(Resource.Layout.TestingBattViewLayout, null);
			activity = (BaseActivity)Activity;
			activity.SupportActionBar.SetDisplayHomeAsUpEnabled(true);
			activity.SupportActionBar.SetDisplayShowHomeEnabled(true);
			_testingBattViewViewModel = ViewModel as TestingBattViewViewModel;
			//set actionbar title
			activity.UpdateTitle(_testingBattViewViewModel.ViewTitle);
			//mAdapter = new HistoryViewInfoAdapter(this.Activity, (IMvxAndroidBindingContext)BindingContext);
			//mListView.Adapter = mAdapter;
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
	}
}
