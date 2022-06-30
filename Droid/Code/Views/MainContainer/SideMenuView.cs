using System;
using System.Collections.Generic;
using System.ComponentModel;
using actchargers.ViewModels;
using Android.OS;
using Android.Runtime;
using Android.Views;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Binding.Droid.Views;

namespace actchargers.Droid
{
	[Register("actchargers.Droid.SideMenuView")]
	public class SideMenuView : BaseFragment
	{
		public override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
		}

		MenuAdapter adapter;

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			base.OnCreateView(inflater, container, savedInstanceState);
			var view = this.BindingInflate(Resource.Layout.SideMenuLayout, null);
			var menuViewModel = (ViewModel as SideMenuViewModel);
		    MvxListView list = view.FindViewById<MvxListView>(Resource.Id.menuList);
			adapter = new MenuAdapter(this.Activity, (IMvxAndroidBindingContext)BindingContext,  menuViewModel.HomeScreenMenuItems);
			list.Adapter = adapter;
			menuViewModel.PropertyChanged += PropertyChanged;
			return view;
		}

		void PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName.Equals("HomeScreenMenuItems")){
				if (adapter != null)
				{
					adapter.NotifyDataSetChanged();
				}
			}
		}
}
}
