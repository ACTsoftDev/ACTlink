using System;
using actchargers.Droid.DrawerToggle;
using actchargers.ViewModels;
using Android.App;
using Android.Content.PM;
using Android.Content.Res;
using Android.OS;
using Android.Support.V4.Widget;
using Android.Views;
using Android.Widget;
using HockeyApp.Android;
using MvvmCross.Core.ViewModels;
using MvvmCross.Droid.Support.V4;
using MvvmCross.Platform;

namespace actchargers.Droid
{
    [Activity(Theme = "@style/AppTheme", Label = "@string/app_name", LaunchMode = LaunchMode.SingleTask, StateNotNeeded = true, ConfigurationChanges = ConfigChanges.Keyboard | ConfigChanges.Orientation | ConfigChanges.ScreenSize, ScreenOrientation = OrientationManager.DEFAULT_ORIENTATION)]


    public class MainContainerView : BaseActivity, DrawerLayout.IDrawerListener, IFragmentHost, Android.Support.V4.App.FragmentManager.IOnBackStackChangedListener
    {

        LinearLayout _drawerRootLayout;
        MainContainerViewModel _mainContainerViewModel;
        static string CONNECTTODEVICE_FRAGMENT_TAG_TYPE = "scanview";
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            _mainContainerViewModel = (ViewModel as MainContainerViewModel);
            topActivity = this;
            _mdrawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            _drawerRootLayout = FindViewById<LinearLayout>(Resource.Id.drawer_root);
            _mdrawerToggle = new SlideMenuActionBarDrawerToggle(this, _mdrawerLayout,
                Resource.String.drawer_open,
                Resource.String.drawer_close);
            _mdrawerLayout.AddDrawerListener(this);
            //enable home menu action and set menu icon to action bar
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            Window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);
            SupportActionBar.SetHomeButtonEnabled(true);
            SupportActionBar.SetHomeAsUpIndicator(Resource.Drawable.menu);
            RegisterCustomPresenterForNavigation();
            //show connect screen by using viewmodel command
            _mainContainerViewModel.OpenConnectViewCommand.Execute();
            //adding drawer to main screen by using viewmodel command
            _mainContainerViewModel.ShowDrawerViewCommand.Execute();
            //Hockey app Crash ID
            CrashManager.Register(this);

        }
        void RegisterCustomPresenterForNavigation()
        {
            var customPresenter = Mvx.Resolve<ICustomPresenter>();
            customPresenter.Register(this);
        }
        protected override void OnResume()
        {
            base.OnResume();
            //register custom presenter for navigation 
            RegisterCustomPresenterForNavigation();

        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
            DeviceHomeViewModel.ClearPingDeviceTimer();
        }
        /// <summary>
        /// Ons the drawer closed.
        /// </summary>
        /// <param name="drawerView">Drawer view.</param>
        public void OnDrawerClosed(View drawerView)
        {

        }
        /// <summary>
        /// Ons the drawer opened.
        /// </summary>
        /// <param name="drawerView">Drawer view.</param>
        public void OnDrawerOpened(View drawerView)
        {

        }
        /// <summary>
        /// Ons the drawer slide.
        /// </summary>
        /// <param name="drawerView">Drawer view.</param>
        /// <param name="slideOffset">Slide offset.</param>
        public void OnDrawerSlide(View drawerView, float slideOffset)
        {

        }

        public void OnDrawerStateChanged(int newState)
        {

        }
        /// <summary>
        /// Ons the post create.
        /// </summary>
        /// <param name="savedInstanceState">Saved instance state.</param>
        protected override void OnPostCreate(Bundle savedInstanceState)
        {
            base.OnPostCreate(savedInstanceState);
            _mdrawerToggle?.SyncState();
        }
        public override void OnConfigurationChanged(Configuration newConfig)
        {
            base.OnConfigurationChanged(newConfig);
            _mdrawerToggle?.OnConfigurationChanged(newConfig);
        }

        /// <summary>
        /// Ons the options item selected.
        /// </summary>
        /// <returns><c>true</c>, if options item selected was oned, <c>false</c> otherwise.</returns>
        /// <param name="item">Item.</param>
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            /* Hide keyboard while opening side menu */
            HideKeybord();
            if (_mdrawerToggle != null && _mdrawerToggle.OnOptionsItemSelected(item))
            {
                return true;
            }
            return base.OnOptionsItemSelected(item);
        }
        protected override void OnResumeFragments()
        {
            base.OnResumeFragments();
        }

        //handling navigation based on viewmodel request here
        public bool Show(MvxViewModelRequest request)
        {
            MvxFragment frag = null;
            bool replaceFragment = false;
            bool addToBackStack = true;

            var transaction = SupportFragmentManager.BeginTransaction();
            var loaderService = Mvx.Resolve<IMvxViewModelLoader>();
            IMvxViewModel viewModelLocal = null;

            if (request.ParameterValues == null || (request.ParameterValues != null && !request.ParameterValues.ContainsKey("pop") && !request.ParameterValues.ContainsKey("popto")))
            {
                viewModelLocal = loaderService.LoadViewModel(request, null /* saved state */);
            }

            try
            {
                if (request.ParameterValues != null && (request.ParameterValues.ContainsKey("pop") || request.ParameterValues.ContainsKey("popto")))
                {
                    return PopScreen(request);
                }
                //pushing screens from here
                else
                {
                    if (request.ViewModelType == typeof(ConnectViewModel))
                    {
                        addToBackStack = false;
                        replaceFragment = true;
                        frag = new ConnectView();
                    }
                    else if (request.ViewModelType == typeof(SideMenuViewModel))
                    {
                        addToBackStack = false;
                        replaceFragment = true;
                        frag = new SideMenuView();
                    }

                    else if (request.ViewModelType == typeof(SyncSitesViewModel))
                    {
                        addToBackStack = true;
                        replaceFragment = true;
                        frag = new SyncSitesView();
                    }
                    else if (request.ViewModelType == typeof(SiteViewSitesViewModel))
                    {
                        addToBackStack = true;
                        replaceFragment = true;
                        frag = new SiteViewSitesListFragment();
                    }
                    else if (request.ViewModelType == typeof(SiteViewDevicesViewModel))
                    {
                        addToBackStack = true;
                        replaceFragment = true;
                        frag = new SiteViewDevicesView();
                    }
                    else if (request.ViewModelType == typeof(SiteViewInProgressDevicesViewModel))
                    {
                        addToBackStack = true;
                        replaceFragment = true;
                        frag = new SiteViewInProgressDevicesView();
                    }
                    else if (request.ViewModelType == typeof(SiteViewSettingsHomeViewModel))
                    {
                        addToBackStack = true;
                        replaceFragment = true;
                        frag = new SiteViewSettingsHomeView();
                    }
                    else if (request.ViewModelType == typeof(AboutUsViewModel))
                    {
                        addToBackStack = true;
                        replaceFragment = true;
                        frag = new AboutUsView();
                    }
                    else if (request.ViewModelType == typeof(ContactUsViewModel))
                    {
                        addToBackStack = true;
                        replaceFragment = true;
                        frag = new ContactUsView();
                    }
                    else if (request.ViewModelType == typeof(ConnectToDeviceViewModel))
                    {
                        addToBackStack = true;
                        replaceFragment = true;
                        frag = new ConnectToDeviceFragment();
                    }
                    else if (request.ViewModelType == typeof(DeviceHomeViewModel))
                    {
                        addToBackStack = true;
                        replaceFragment = true;
                        frag = new DeviceHomeView();
                    }

                    else if (request.ViewModelType == typeof(HistoryViewModel))
                    {
                        addToBackStack = true;
                        replaceFragment = true;
                        frag = new HistoryView();
                    }
                    else if (request.ViewModelType == typeof(CumulativeDataViewModel))
                    {
                        addToBackStack = true;
                        replaceFragment = true;
                        frag = new CumulativeDataView();
                    }
                    else if (request.ViewModelType == typeof(RtRecordsViewModel))
                    {
                        addToBackStack = true;
                        replaceFragment = true;
                        frag = new RTRecordsView();
                    }
                    else if (request.ViewModelType == typeof(WiFiViewModel))
                    {
                        addToBackStack = true;
                        replaceFragment = true;
                        frag = new WifiView();
                    }
                    else if (request.ViewModelType == typeof(InfoViewModel))
                    {
                        addToBackStack = true;
                        replaceFragment = true;
                        frag = new InfoView();
                    }
                    else if (request.ViewModelType == typeof(SettingsViewModel))
                    {
                        addToBackStack = true;
                        replaceFragment = true;
                        frag = new SettingsView();
                    }
                    else if (request.ViewModelType == typeof(CableSettingsViewModel))
                    {
                        addToBackStack = true;
                        replaceFragment = true;
                        frag = new CableSettingsView();
                    }
                    else if (request.ViewModelType == typeof(BatteryInfoViewModel))
                    {
                        addToBackStack = true;
                        replaceFragment = true;
                        frag = new BatteryInfoView();
                    }

                    else if (request.ViewModelType == typeof(BatterySettingsViewModel))
                    {
                        addToBackStack = true;
                        replaceFragment = true;
                        frag = new BatterySettingsView();
                    }
                    else if (request.ViewModelType == typeof(DefaultChargeProfileViewModel))
                    {
                        addToBackStack = true;
                        replaceFragment = true;
                        frag = new DefaultChargeProfileView();
                    }
                    else if (request.ViewModelType == typeof(NewStudyViewModel))
                    {
                        addToBackStack = true;
                        replaceFragment = true;
                        frag = new NewStudyView();
                    }
                    else if (request.ViewModelType == typeof(EventDataRangeHistoryViewModel))
                    {
                        addToBackStack = true;
                        replaceFragment = true;
                        frag = new EventDataRangeHistoryView();
                    }
                    else if (request.ViewModelType == typeof(EventsDateRangeViewModel))
                    {
                        addToBackStack = true;
                        replaceFragment = true;
                        frag = new EventsDateRangeView();
                    }
                    else if (request.ViewModelType == typeof(QuickViewModel))
                    {
                        addToBackStack = true;
                        replaceFragment = true;
                        frag = new QuickView();
                    }
                    else if (request.ViewModelType == typeof(QuickViewChargerViewModel))
                    {
                        addToBackStack = true;
                        replaceFragment = true;
                        frag = new QuickViewChargerView();
                    }
                    else if (request.ViewModelType == typeof(FinishAndEQSettingsViewModel))
                    {
                        addToBackStack = true;
                        replaceFragment = true;
                        frag = new FinishAndEQSettingsView();
                    }
                    else if (request.ViewModelType == typeof(CalibrationViewModel))
                    {
                        addToBackStack = true;
                        replaceFragment = true;
                        frag = new CalibrationView();
                    }
                    else if (request.ViewModelType == typeof(PowerModuleViewModel))
                    {
                        addToBackStack = true;
                        replaceFragment = true;
                        frag = new PowerModuleView();
                    }
                    else if (request.ViewModelType == typeof(PmInfoViewModel))
                    {
                        addToBackStack = true;
                        replaceFragment = true;
                        frag = new PmInfoView();
                    }
                    else if (request.ViewModelType == typeof(ViewGlobalRecordsViewModel))
                    {
                        addToBackStack = true;
                        replaceFragment = true;
                        frag = new GlobalRecordsView();
                    }
                    else if (request.ViewModelType == typeof(EnergyManagementViewModel))
                    {
                        addToBackStack = true;
                        replaceFragment = true;
                        frag = new EnergyManagementView();
                    }
                    else if (request.ViewModelType == typeof(FactoryControlViewModel))
                    {
                        addToBackStack = true;
                        replaceFragment = true;
                        frag = new FactoryControlView();
                    }
                    else if (request.ViewModelType == typeof(CommissionViewModel))
                    {
                        addToBackStack = true;
                        replaceFragment = true;
                        frag = new CommissionView();
                    }
                    else if (request.ViewModelType == typeof(CommissionNextViewModel))
                    {
                        addToBackStack = true;
                        replaceFragment = true;
                        frag = new CommissionNextView();
                    }
                    else if (request.ViewModelType == typeof(EditEventControlViewModel))
                    {
                        addToBackStack = true;
                        replaceFragment = true;
                        frag = new EditEventControlView();
                    }
                    else if (request.ViewModelType == typeof(CommissionAReplacementPartViewModel))
                    {
                        addToBackStack = true;
                        replaceFragment = true;
                        frag = new CommissionAReplacementPartView();
                    }
                    else if (request.ViewModelType == typeof(TestingBattViewViewModel))
                    {
                        addToBackStack = true;
                        replaceFragment = true;
                        frag = new TestingBattViewView();
                    }

                    else if (request.ViewModelType == typeof(ReplacementViewModel))
                    {
                        addToBackStack = true;
                        replaceFragment = true;
                        frag = new ReplacementsFragment();
                    }
                    else if (request.ViewModelType == typeof(FirmwareUpdateViewModel))
                    {
                        addToBackStack = true;
                        replaceFragment = true;
                        frag = new FirmwareUpdateView();
                    }

                    else if (request.ViewModelType == typeof(UploadViewModel))
                    {
                        addToBackStack = true;
                        replaceFragment = true;
                        frag = new UploadView();
                    }

                    else if (request.ViewModelType == typeof(ViewCyclesHistoryViewModel))
                    {
                        addToBackStack = true;
                        replaceFragment = true;
                        frag = new ViewCyclesHistoryView();
                    }

                    else if (request.ViewModelType == typeof(PowerSnapshotsViewModel))
                    {
                        addToBackStack = true;
                        replaceFragment = true;
                        frag = new PowerSnapshotsView();
                    }

                    else if (request.ViewModelType == typeof(PMFaultsViewModel))
                    {
                        addToBackStack = true;
                        replaceFragment = true;
                        frag = new PMFaultsView();
                    }

                    else if (request.ViewModelType == typeof(PmLiveViewModel))
                    {
                        addToBackStack = true;
                        replaceFragment = true;
                        frag = new PmLiveView();
                    }
                }

                if (frag == null)
                {
                    return true;
                }
                frag.ViewModel = viewModelLocal;
                if (request.ViewModelType == typeof(SideMenuViewModel))
                {
                    transaction.Add(Resource.Id.drawer_root, frag, frag.GetType().Name);
                }
                else
                {
                    if (replaceFragment)
                    {
                        transaction.Replace(Resource.Id.content_frame, frag, frag.GetType().Name);
                    }
                    else
                    {
                        transaction.Add(Resource.Id.content_frame, frag, frag.GetType().Name);
                    }
                }
                if (addToBackStack)
                {

                    if (request.ViewModelType == typeof(ConnectToDeviceViewModel))
                    {
                        transaction.AddToBackStack(CONNECTTODEVICE_FRAGMENT_TAG_TYPE);
                    }
                    else
                    {
                        transaction.AddToBackStack(frag.GetType().Name);
                    }
                }

                new Handler().PostDelayed(new Action(() =>
                {
                    CommitTransaction(transaction);
                }), 200);


                return true;
            }
            catch (Exception ex)
            {
                Logger.AddLog(true, "X86" + ex);
                Console.WriteLine("exception " + ex);
                return true;
            }
            finally
            {
                if (_mdrawerLayout != null)
                {
                    _mdrawerLayout.CloseDrawer(_drawerRootLayout);
                }
            }
        }

        bool PopScreen(MvxViewModelRequest request)
        {
            bool isPopDone = false;
            if (request.ViewModelType == typeof(ConnectToDeviceViewModel) && request.ParameterValues.ContainsKey("popto"))
            {
                if (null != SupportFragmentManager)
                {
                    SupportFragmentManager.PopBackStack(CONNECTTODEVICE_FRAGMENT_TAG_TYPE, 0);
                    isPopDone = true;
                }
            }
            else if (request.ParameterValues.ContainsKey("pop"))
            {
                if (null != SupportFragmentManager)
                {
                    MvxFragment topFragment = (MvxFragment)SupportFragmentManager.FindFragmentById(Resource.Id.content_frame);
                    if (topFragment != null)
                    {
                        Close((topFragment.ViewModel as BaseViewModel));
                        SupportFragmentManager.PopBackStackImmediate();
                    }
                }
                isPopDone = true;
            }

            return isPopDone;
        }

        void CommitTransaction(Android.Support.V4.App.FragmentTransaction transaction)
        {
            transaction.CommitAllowingStateLoss();
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);
        }

        void IFragmentHost.Close(IMvxViewModel viewModel)
        {
        }

        public void ChangePresentation(MvxPresentationHint hint)
        {

        }

        public void OnBackStackChanged()
        {
        }

        protected override int GetLayoutResource()
        {
            return Resource.Layout.MainContainerLayout;
        }


    }
}
