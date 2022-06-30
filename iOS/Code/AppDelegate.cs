using System;
using actchargers.ViewModels;
using Foundation;
using HockeyApp.iOS;
using MvvmCross.Core.ViewModels;
using MvvmCross.iOS.Platform;
using MvvmCross.Platform;
using SidebarNavigation;
using UIKit;

namespace actchargers.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the
    // User Interface of the application, as well as listening (and optionally responding) to application events from iOS.
    [Register("AppDelegate")]
    public class AppDelegate : MvxApplicationDelegate
    {
        // class-level declarations
        public SidebarController SideBar { get; set; }
        public UINavigationController NavController { get; set; }
        public CustomPresenter presenter { get; set; }

        public override UIWindow Window
        {
            get;
            set;
        }

        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            // Override point for customization after application launch.
            // If not required for your application you can safely delete this method

            Window = new UIWindow(UIScreen.MainScreen.Bounds);

            presenter = new CustomPresenter(this, Window);

            var setup = new Setup(this, presenter);
            setup.Initialize();

            var startup = Mvx.Resolve<IMvxAppStart>();
            startup.Start();

            Mvx.RegisterSingleton<ICustomPresenter>(presenter);

            Window.MakeKeyAndVisible();

            //Hockey app integration
            var manager = BITHockeyManager.SharedHockeyManager;
            manager.Configure("85e97be5cd0041e38a04dbf1f6fb8f4d");
            manager.StartManager();
            manager.Authenticator.AuthenticateInstallation(); // This line is obsolete in crash only builds

            return true;
        }

        public override void OnResignActivation(UIApplication application)
        {
            // Invoked when the application is about to move from active to inactive state.
            // This can occur for certain types of temporary interruptions (such as an incoming phone call or SMS message) 
            // or when the user quits the application and it begins the transition to the background state.
            // Games should use this method to pause the game.
        }

        public override void DidEnterBackground(UIApplication application)
        {
            // Use this method to release shared resources, save user data, invalidate timers and store the application state.
            // If your application supports background exection this method is called instead of WillTerminate when the user quits.
        }

        public override void WillEnterForeground(UIApplication application)
        {
            // Called as part of the transiton from background to active state.
            // Here you can undo many of the changes made on entering the background.
        }

        public override void OnActivated(UIApplication application)
        {
            // Restart any tasks that were paused (or not yet started) while the application was inactive. 
            // If the application was previously in the background, optionally refresh the user interface.
            if (!StaticDataAndHelperFunctions.ValidateLoginDate())
            {
                //14 Days Session Time Out
                presenter.MasterNavigationController.RemoveFromParentViewController();
                //presenter.MasterNavigationController = null;
                if (presenter.Host != null && presenter.Host.NavController != null)
                {
                    foreach (var item in presenter.Host.NavController.ViewControllers)
                    {
                        item.RemoveFromParentViewController();
                    }
                }

                Mvx.Resolve<IUserPreferences>().SetInt(ACConstants.USER_PREFS_USER_ID, 0);
                Mvx.Resolve<IUserPreferences>().SetBool(ACConstants.USER_PREFS_USER_LOGGED_IN, false);

                System.Collections.Generic.Dictionary<string, string> param = new System.Collections.Generic.Dictionary<string, string>();
                param.Add("logout", AppResources.yes);

                presenter.Show(new MvxViewModelRequest() { ViewModelType = typeof(LoginViewModel), ParameterValues = param });

                presenter.Host = null;
            }
        }

        public override void WillTerminate(UIApplication application)
        {
            // Called when the application is about to terminate. Save data, if needed. See also DidEnterBackground.
        }

        public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations(UIApplication application, UIWindow forWindow)
        {
            if (NavController != null)
            {
                var presentedViewController = NavController.TopViewController;
                if (presentedViewController != null)
                {
                    Type type = presentedViewController.GetType();
                    if (IsLandscapeView(type))
                    {
                        return UIInterfaceOrientationMask.LandscapeLeft;
                    }
                    else
                    {
                        return UIInterfaceOrientationMask.Portrait;
                    }
                }
            }

            return UIInterfaceOrientationMask.Portrait;
        }

        bool IsLandscapeView(Type type)
        {
            bool isLandscapeView =
                (type == typeof(PMFaultsView)) ||
                (type == typeof(ViewCyclesHistoryView)) ||
                (type == typeof(PmLiveView)) ||
                (type == typeof(PowerSnapshotsView));

            return isLandscapeView;
        }
    }
}

