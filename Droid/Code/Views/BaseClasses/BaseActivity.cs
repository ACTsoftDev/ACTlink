
using System;
using actchargers.Droid.DrawerToggle;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Support.V4.Widget;
using Android.Util;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;

namespace actchargers.Droid
{
    [Activity(Theme = "@style/AppTheme", ScreenOrientation = OrientationManager.DEFAULT_ORIENTATION, Label = "@string/app_name")]
    public abstract class BaseActivity : MvvmCross.Droid.Support.V7.AppCompat.MvxCachingFragmentCompatActivity
    {
        public DrawerLayout _mdrawerLayout { get; set; }
        public SlideMenuActionBarDrawerToggle _mdrawerToggle { get; set; }

        public TextView mTitle;
        public static Activity topActivity;
        public Android.Support.V7.Widget.Toolbar CustomToolBar { get; set; }

        public static Activity GetTopActivity()
        {
            return topActivity;
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            //here setting subclass layout and inflating here so that we can access all actionbar views in base class.
            SetContentView(GetLayoutResource());
            Window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);
            ConfigureToolbar();

            KeepScreenOn();

            CheckAllPermissionsIfNeeded();
        }

        void CheckAllPermissionsIfNeeded()
        {
            if (Build.VERSION.SdkInt > BuildVersionCodes.M)
                CheckAllPermissions();
        }

        void CheckAllPermissions()
        {
            bool noAccess = CheckSelfPermission(Android.Manifest.Permission.AccessWifiState) != Permission.Granted;
            if (noAccess)
                ForceRequestPermissions(Android.Manifest.Permission.AccessWifiState);

            noAccess = CheckSelfPermission(Android.Manifest.Permission.ChangeWifiState) != Permission.Granted;
            if (noAccess)
                ForceRequestPermissions(Android.Manifest.Permission.ChangeWifiState);

            noAccess = CheckSelfPermission(Android.Manifest.Permission.ChangeWifiMulticastState) != Permission.Granted;
            if (noAccess)
                ForceRequestPermissions(Android.Manifest.Permission.ChangeWifiMulticastState);

            noAccess = CheckSelfPermission(Android.Manifest.Permission.AccessNetworkState) != Permission.Granted;
            if (noAccess)
                ForceRequestPermissions(Android.Manifest.Permission.AccessNetworkState);

            noAccess = CheckSelfPermission(Android.Manifest.Permission.ChangeNetworkState) != Permission.Granted;
            if (noAccess)
                ForceRequestPermissions(Android.Manifest.Permission.ChangeNetworkState);

            noAccess = CheckSelfPermission(Android.Manifest.Permission.AccessFineLocation) != Permission.Granted;
            if (noAccess)
                ForceRequestPermissions(Android.Manifest.Permission.AccessFineLocation);

            noAccess = CheckSelfPermission(Android.Manifest.Permission.AccessCoarseLocation) != Permission.Granted;
            if (noAccess)
                ForceRequestPermissions(Android.Manifest.Permission.AccessCoarseLocation);
        }

        void ForceRequestPermissions(string permission)
        {
#pragma warning disable XA0001 // Find issues with Android API usage
            RequestPermissions(new string[] { permission }, 10);
#pragma warning restore XA0001 // Find issues with Android API usage
        }

        protected abstract int GetLayoutResource();

        //inflate app toolbar and set title
        private void ConfigureToolbar()
        {
            CustomToolBar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.app_toolbar);
            mTitle = (TextView)CustomToolBar.FindViewById(Resource.Id.toolbar_title);
            if (CustomToolBar != null)
            {
                CustomToolBar.SetTitle(Resource.String.actionbar_tile);
                SetSupportActionBar(CustomToolBar);
            }
        }

        void KeepScreenOn()
        {
            try
            {
                TryKeepScreenOn();
            }
            catch (Exception ex)
            {
                Log.Error("BaseActivity", ex.ToString());
            }
        }

        void TryKeepScreenOn()
        {
            Window.SetFlags(WindowManagerFlags.KeepScreenOn, WindowManagerFlags.KeepScreenOn);
        }

        //Setting ACtionbar title from here for all screens
        public void UpdateTitle(String title)
        {
            if (CustomToolBar != null)
            {
                mTitle.Text = title;
            }
        }
        //to hide keyboard
        public void HideKeybord()
        {
            View view = CurrentFocus;
            if (view != null)
            {
                InputMethodManager imm = (InputMethodManager)GetSystemService(Context.InputMethodService);
                imm.HideSoftInputFromWindow(view.WindowToken, HideSoftInputFlags.None);
            }
        }

        public void DisableDrawer()
        {
            if (_mdrawerLayout != null && _mdrawerToggle != null)
            {
                _mdrawerLayout?.SetDrawerLockMode(DrawerLayout.LockModeLockedClosed);
                _mdrawerToggle.DrawerIndicatorEnabled = false;
                _mdrawerToggle.SyncState();
            }
        }
        public void EnableDrawer()
        {
            if (_mdrawerLayout != null && _mdrawerToggle != null)
            {
                _mdrawerLayout?.SetDrawerLockMode(DrawerLayout.LockModeUnlocked);
                _mdrawerToggle.DrawerIndicatorEnabled = true;
                _mdrawerToggle.SyncState();
            }
        }

    }
}
