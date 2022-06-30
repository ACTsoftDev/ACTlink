using System.Collections.Generic;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Views;
using MvvmCross.Binding.Droid.BindingContext;

namespace actchargers.Droid
{
    public class UploadView : BaseFragmentWithOutMenu
    {
        UploadViewModel viewModel;

        TabLayout tabLayout;
        BaseActivity activity;
        GenericTabAdapter adapter;

        public override View OnCreateView
        (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            View view = this.BindingInflate(Resource.Layout.UploadLayout, null);

            InitActivity();

            var fragments = new List<MvxViewPagerFragmentAdapter.FragmentInfo> {
                new MvxViewPagerFragmentAdapter.FragmentInfo {
                    FragmentType = typeof(UploadFragment),
                    ViewModel = viewModel
                },
                new MvxViewPagerFragmentAdapter.FragmentInfo {
                    FragmentType = typeof(UploadFragment),
                    ViewModel = viewModel
                },
                new MvxViewPagerFragmentAdapter.FragmentInfo {
                    FragmentType = typeof(UploadFragment),
                    ViewModel = viewModel
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
                viewModel.SelectedTabIndex = (DevicesTabs)e.Tab.Position;
            };

            return view;
        }

        void InitActivity()
        {
            viewModel = ViewModel as UploadViewModel;

            activity = (BaseActivity)Activity;
            activity.UpdateTitle(viewModel.ViewTitle);
            activity.SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            activity.SupportActionBar.SetHomeButtonEnabled(true);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    activity.OnBackPressed();

                    return true;

                default:
                    return base.OnOptionsItemSelected(item);
            }
        }
    }
}
