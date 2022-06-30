using System.Collections.Generic;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Views;
using MvvmCross.Binding.Droid.BindingContext;

namespace actchargers.Droid
{
    public class SiteViewDevicesView : BaseFragmentWithOutMenu
    {
        SiteViewDevicesViewModel currentViewModel;

        TabLayout tabLayout;
        GenericTabAdapter adapter;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            HasOptionsMenu = true;
        }

        public override View OnCreateView
        (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            View view = this.BindingInflate(Resource.Layout.SiteViewDevicesLayout, null);

            currentViewModel = ViewModel as SiteViewDevicesViewModel;

            var fragments = new List<MvxViewPagerFragmentAdapter.FragmentInfo> {
                new MvxViewPagerFragmentAdapter.FragmentInfo {
                    FragmentType = typeof(SiteViewDevicesListFragment),
                    ViewModel = currentViewModel
                },
                new MvxViewPagerFragmentAdapter.FragmentInfo {
                    FragmentType = typeof(SiteViewDevicesListFragment),
                    ViewModel = currentViewModel
                },
                new MvxViewPagerFragmentAdapter.FragmentInfo {
                    FragmentType = typeof(SiteViewDevicesListFragment),
                    ViewModel = currentViewModel
                }
            };
            var _viewPager = view.FindViewById<ViewPager>(Resource.Id.viewPagerDevice);
            adapter =
                new GenericTabAdapter(activity, ChildFragmentManager, fragments);
            tabLayout = view.FindViewById<TabLayout>(Resource.Id.tabsDevice);
            _viewPager.Adapter = adapter;
            tabLayout.SetupWithViewPager(_viewPager);

            for (int i = 0; i < tabLayout.TabCount; i++)
            {
                TabLayout.Tab tab = tabLayout.GetTabAt(i);
                int count = 0;

                switch (i)
                {
                    case (int)DevicesTabs.CHARGER:
                        tab.SetText(AppResources.chargers);

                        break;

                    case (int)DevicesTabs.BATTVIEW:
                        tab.SetText(AppResources.battview);

                        break;

                    case (int)DevicesTabs.BATTVIEW_MOBILE:
                        tab.SetText(AppResources.batt_mobile);

                        break;
                }

                tab
                    .SetCustomView
                    (adapter
                     .GetTabView
                     (i, tab.IsSelected, ACConstants.DEVICES_TABS_TITLES[i], count));
            }

            tabLayout.TabSelected += (sender, e) =>
            {
                currentViewModel.SelectedTabIndex = (DevicesTabs)e.Tab.Position;
            };

            return view;
        }

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            menu.Clear();

            inflater.Inflate(Resource.Menu.siteview_devices, menu);

            base.OnCreateOptionsMenu(menu, inflater);

        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    activity.OnBackPressed();

                    return true;

                case Resource.Id.download:
                    currentViewModel.DownloadCommand.Execute();

                    return true;

                case Resource.Id.update_site_firmware:
                    currentViewModel.UpdateCommand.Execute();

                    return true;

                case Resource.Id.settings:
                    currentViewModel.SettingsCommand.Execute();

                    return true;

                default:
                    return base.OnOptionsItemSelected(item);
            }
        }
    }
}
