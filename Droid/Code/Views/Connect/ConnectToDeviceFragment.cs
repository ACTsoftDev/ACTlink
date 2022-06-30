using System;
using System.Collections.Generic;
using System.ComponentModel;
using actchargers.Droid.Code.Views.Connect;
using Android.Content;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Views;
using Android.Widget;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Core.ViewModels;

namespace actchargers.Droid
{
    public class ConnectToDeviceFragment : BaseFragmentWithOutMenu
    {
        readonly string[] _tabHeader = { AppResources.battview, AppResources.chargers, AppResources.replacements };
        private ConnectToDeviceViewModel mConnectToDeviceViewModel;
        private TabLayout _tabLayout;
        private BaseActivity activity;
        private TabAdapter _adapter;
        private const int BATTVIEW_TAB_POSITION = 0;
        private const int CHARGER_TAB_POSITION = 1;
        private const int REPLACEMENT_TAB_POSITION = 2;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            HasOptionsMenu = true;
            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            View view = this.BindingInflate(Resource.Layout.ConnectToDeviceViewFragmentLayout, null);
            mConnectToDeviceViewModel = ViewModel as ConnectToDeviceViewModel;
            activity = (BaseActivity)Activity;
            activity.UpdateTitle(mConnectToDeviceViewModel.ViewTitle);
            activity.SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            activity.SupportActionBar.SetHomeButtonEnabled(true);
            var fragments = new List<MvxViewPagerFragmentAdapter.FragmentInfo> {
                new MvxViewPagerFragmentAdapter.FragmentInfo {
                    FragmentType = typeof(Battview),
                    ViewModel = mConnectToDeviceViewModel
                },
                new MvxViewPagerFragmentAdapter.FragmentInfo {
                    FragmentType = typeof(ChargerView),
                    ViewModel = mConnectToDeviceViewModel
                },
                new MvxViewPagerFragmentAdapter.FragmentInfo {
                    FragmentType = typeof(ReplacementView),
                    ViewModel = mConnectToDeviceViewModel
                }
            };
            var _viewPager = view.FindViewById<ViewPager>(Resource.Id.viewPagerDevice);
            _adapter = new TabAdapter(activity, ChildFragmentManager, fragments);
            _tabLayout = view.FindViewById<TabLayout>(Resource.Id.tabsDevice);
            // Set adapter to view pager
            _viewPager.Adapter = _adapter;
            // Setup tablayout with view pager
            _tabLayout.SetupWithViewPager(_viewPager);

            // Iterate over all tabs and set the custom view
            for (int i = 0; i < _tabLayout.TabCount; i++)
            {
                TabLayout.Tab tab = _tabLayout.GetTabAt(i);
                int count = 0;
                if (i == BATTVIEW_TAB_POSITION)
                {
                    count = mConnectToDeviceViewModel.BattViewCount;
                }
                else if (i == CHARGER_TAB_POSITION)
                {
                    count = mConnectToDeviceViewModel.Chargerscount;
                }
                else if (i == REPLACEMENT_TAB_POSITION)
                {
                    count = mConnectToDeviceViewModel.ReplacementCount;
                }
                tab.SetCustomView(_adapter.GetTabView(i, tab.IsSelected, _tabHeader[i], count));
            }


            _tabLayout.TabSelected += (sender, e) =>
            {
                switch (e.Tab.Position)
                {
                    case 0:
                        mConnectToDeviceViewModel.SelectedTabIndex = 0;
                        break;
                    case 1:
                        mConnectToDeviceViewModel.SelectedTabIndex = 1;
                        break;
                    case 2:
                        mConnectToDeviceViewModel.SelectedTabIndex = 2;
                        break;
                    default:
                        break;
                }
            };
            ((MvxNotifyPropertyChanged)this.ViewModel).PropertyChanged += OnPropertyChanged;

            return view;
        }

        void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("BattViewCount"))
            {
                UpdateTabCount(BATTVIEW_TAB_POSITION, mConnectToDeviceViewModel.BattViewCount);
            }
            if (e.PropertyName.Equals("Chargerscount"))
            {
                UpdateTabCount(CHARGER_TAB_POSITION, mConnectToDeviceViewModel.Chargerscount);
            }
            if (e.PropertyName.Equals("ReplacementCount"))
            {
                UpdateTabCount(REPLACEMENT_TAB_POSITION, mConnectToDeviceViewModel.ReplacementCount);
            }

        }

        //update title for tabs here
        public void UpdateTabCount(int position, int deviceCount)
        {
            LinearLayout layout = (Android.Widget.LinearLayout)_tabLayout.GetTabAt(position).CustomView;
            TextView countTextView = (TextView)layout.FindViewById(Resource.Id.count);
            countTextView.Text = "(" + deviceCount + ")";
        }
        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            //clear activity menu items to add  specific fragment menu items
            menu.Clear();
            inflater.Inflate(Resource.Menu.connect_to_device, menu);

            var siteViewMenuItem = menu.FindItem(Resource.Id.site_view);
            ShowHideSeiteViewMenuItem(siteViewMenuItem);

            base.OnCreateOptionsMenu(menu, inflater);
        }

        void ShowHideSeiteViewMenuItem(IMenuItem siteViewMenuItem)
        {
            if (!ACConstants.SHOW_SITEVIEW)
            {
                siteViewMenuItem.SetVisible(false);

                return;
            }

            bool hasSites =
                DbSingleton
                    .DBManagerServiceInstance
                    .GetSynchSiteObjectsLoader()
                    .HasSites();

            siteViewMenuItem.SetVisible(hasSites);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                //handling backpress of actionbar back here
                case Android.Resource.Id.Home:
                    // this takes the user 'back', as if they pressed the left-facing triangle icon on the main android toolbar.
                    //mConnectToDeviceViewModel.ClearDataonBackPressed();
                    activity.OnBackPressed();

                    return true;

                case Resource.Id.download:
                    mConnectToDeviceViewModel.DownloadSiteData.Execute();

                    return true;

                case Resource.Id.upload_data:
                    mConnectToDeviceViewModel.UploadData.Execute();

                    return true;

                case Resource.Id.sync_sites:
                    mConnectToDeviceViewModel.SyncSites.Execute();

                    return true;

                case Resource.Id.site_view:
                    mConnectToDeviceViewModel.SiteView.Execute();

                    return true;

                case Resource.Id.update_site_firmware:
                    mConnectToDeviceViewModel.UpdateSiteFirmware.Execute();

                    return true;

                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        public override void OnPause()
        {
            base.OnPause();
        }
        public override void OnDestroy()
        {
            try
            {
                mConnectToDeviceViewModel.ClearDataonBackPressed();
            }
            catch (Exception ex)
            {

            }
            try
            {
                mConnectToDeviceViewModel.ClearTimers();
            }
            catch (Exception ex)
            {
                
            }

            base.OnDestroy();
        }

        class TabAdapter : MvxViewPagerFragmentAdapter
        {
            private readonly Context _context;

            public TabAdapter(
                Context context, Android.Support.V4.App.FragmentManager fragmentManager, IEnumerable<FragmentInfo> fragments)
                : base(context, fragmentManager, fragments)
            {
                _context = context;
            }

            public View GetTabView(int position, bool isSelected, string title, int count)
            {
                LinearLayout layout = (LinearLayout)LayoutInflater.From(_context).Inflate(Resource.Layout.CustomTabLayout, null);
                TextView tv = (TextView)layout.FindViewById(Resource.Id.tabTitle);
                tv.Text = title;
                TextView countTextView = (TextView)layout.FindViewById(Resource.Id.count);
                countTextView.Text = "(" + count + ")";

                return layout;
            }

        }
    }
}
