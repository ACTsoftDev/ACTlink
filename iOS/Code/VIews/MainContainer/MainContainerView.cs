using System;
using actchargers.ViewModels;
using MvvmCross.iOS.Views;
using MvvmCross.Platform;
using SidebarNavigation;
using UIKit;

namespace actchargers.iOS
{
    public partial class MainContainerView : BaseView
    {

        /// <summary>
        /// Gets or sets the sidebar controler.
        /// </summary>
        /// <value>The sidebar controler.</value>
        public SidebarController SidebarControler { get; set; }

        /// <summary>
        /// Gets or sets the nav controller.
        /// </summary>
        /// <value>The nav controller.</value>
        // the navigation controller
        public UINavigationController NavController { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:actchargers.iOS.MainContainerView"/> class.
        /// </summary>
        public MainContainerView() : base("MainContainerView", null)
        {
        }

        /// <summary>
        /// Views the did load.
        /// </summary>
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.

            View.Frame = UIScreen.MainScreen.Bounds;

            ///setting ConnectViewModel as Container view
            /// Setting SideMenuViewModel as Menu view
            UIViewController mainView = (UIViewController)Mvx.Resolve<IMvxIosViewCreator>().CreateView(new ConnectViewModel());
            UIViewController sideMenuView = (UIViewController)Mvx.Resolve<IMvxIosViewCreator>().CreateView(new SideMenuViewModel());

            NavController = new UINavigationController(mainView);
            NavController.NavigationBar.Translucent = false;

            ///Initiating SidebarControler and setting mainview and sidemenu
            SidebarControler = new SidebarNavigation.SidebarController(this, NavController, sideMenuView);
            SidebarControler.MenuWidth = 250;
            SidebarControler.MenuLocation = MenuLocations.Right;

            var customPresenter = Mvx.Resolve<ICustomPresenter>();

            customPresenter.Register(this);

            (UIApplication.SharedApplication.Delegate as AppDelegate).SideBar = SidebarControler;           
            (UIApplication.SharedApplication.Delegate as AppDelegate).NavController = NavController;
        }
    }
}