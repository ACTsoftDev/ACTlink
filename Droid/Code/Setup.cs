using Android.Content;
using MvvmCross.Droid.Platform;
using MvvmCross.Core.ViewModels;
using MvvmCross.Droid.Views;
using MvvmCross.Platform;

namespace actchargers.Droid
{
    public class Setup : MvxAndroidSetup
    {
        public Setup(Context applicationContext)
            : base(applicationContext)
        {
        }

        protected override IMvxApplication CreateApp()
        {
            Mvx.RegisterSingleton<ISQLite>(() => new SQLite());
            Mvx.RegisterSingleton<IUserPreferences>(() => new AndroidUserPreferences());
            Mvx.RegisterSingleton<IWifiManagerService>(() => new WifiSManagerService());
            Mvx.RegisterSingleton<IUSBInterface>(() => new USBInterface());
            Mvx.RegisterSingleton<ICustomAlert>(() => new CustomAlert());
			Mvx.RegisterSingleton<IPlatformVersion>(() => new PlatformVersion());
			Mvx.RegisterSingleton<IDatabaseBackupManager>(() => new DatabaseBackupManager());

            return new App();
        }

        protected override void InitializeLastChance()
        {
            base.InitializeLastChance();

            MvvmCross.Plugins.DownloadCache.PluginLoader.Instance.EnsureLoaded();
            MvvmCross.Plugins.File.PluginLoader.Instance.EnsureLoaded();
        }

        protected override IMvxAndroidViewPresenter CreateViewPresenter()
        {
            var customPresenter = new CustomPresenter();
            Mvx.RegisterSingleton<ICustomPresenter>(customPresenter);
            return customPresenter;
        }

    }
}
