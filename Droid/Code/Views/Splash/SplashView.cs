using Android.App;
using MvvmCross.Droid.Views;

namespace actchargers.Droid.Code.Views.Splash
{
    [Activity(Label = "@string/app_name", NoHistory = true, MainLauncher = true, ScreenOrientation = OrientationManager.DEFAULT_ORIENTATION, Theme = "@style/SplashTheme")]
    public class SplashView : MvxSplashScreenActivity
    {
        public SplashView() : base(Resource.Layout.SplashView)
        {
        }
    }
}
