using MvvmCross.Core.ViewModels;
using MvvmCross.iOS.Platform;
using MvvmCross.iOS.Views.Presenters;
using MvvmCross.Platform;
using MvvmCross.Platform.Platform;
using UIKit;

namespace actchargers.iOS
{
    public class Setup : MvxIosSetup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:actchargers.iOS.Setup"/> class.
        /// </summary>
        /// <param name="applicationDelegate">Application delegate.</param>
        /// <param name="window">Window.</param>
        public Setup(MvxApplicationDelegate applicationDelegate, UIWindow window)
            : base(applicationDelegate, window)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:actchargers.iOS.Setup"/> class.
        /// </summary>
        /// <param name="applicationDelegate">Application delegate.</param>
        /// <param name="presenter">Presenter.</param>
        public Setup(MvxApplicationDelegate applicationDelegate, IMvxIosViewPresenter presenter)
            : base(applicationDelegate, presenter)
        {
        }

        /// <summary>
        /// Creates the app.
        /// </summary>
        /// <returns>The app.</returns>
        protected override IMvxApplication CreateApp()
        {
            Mvx.RegisterSingleton<ISQLite>(() => new SQLite());
            Mvx.RegisterSingleton<IUserPreferences>(() => new IOSUserPreferences());
            // Mvx.RegisterSingleton<IAnalyticsService>(() => new AnalyticsService());
            Mvx.RegisterSingleton<IWifiManagerService>(() => new WifiManagerService());
            Mvx.RegisterSingleton<IProfileCheckService>(() => new ProfileCheckService());
            Mvx.RegisterSingleton<ICustomAlert>(() => new CustomAlert());
			Mvx.RegisterSingleton<IPlatformVersion>(() => new PlatformVersion());
			Mvx.RegisterSingleton<IDatabaseBackupManager>(() => new DatabaseBackupManager());

			return new actchargers.App();
        }
    }
}
