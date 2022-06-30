using actchargers.ViewModels;
using MvvmCross.Core.ViewModels;
using MvvmCross.iOS.Views;
using MvvmCross.iOS.Views.Presenters;
using MvvmCross.Platform;
using UIKit;

namespace actchargers.iOS
{
    /// <summary>
    /// Custom presenter.
    /// </summary>
    public interface ICustomPresenter
    {
        void Register(MainContainerView host);
    }

    public class CustomPresenter : MvxIosViewPresenter, ICustomPresenter
    {
        public MainContainerView Host { get; set; }
        UIViewController previousController = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:actchargers.iOS.CustomPresenter"/> class.
        /// </summary>
        /// <param name="applicationDelegate">Application delegate.</param>
        /// <param name="window">Window.</param>
        public CustomPresenter(UIApplicationDelegate applicationDelegate, UIWindow window)
            : base(applicationDelegate, window)
        {

        }

        /// <summary>
        /// Show the specified request.
        /// </summary>
        /// <param name="request">Request.</param>
        public override void Show(MvxViewModelRequest request)
        {

            //if Host null then set the rootview  
            //Handling viewmodel navigations from LoginViewModel
            if ((Host == null) && (request.ViewModelType == typeof(LoginViewModel) || request.ViewModelType == typeof(MainContainerViewModel) || request.ViewModelType == typeof(UsageAgreementViewModel) || request.ViewModelType == typeof(ContactUsViewModel)))
            {
                if (request.ViewModelType == typeof(LoginViewModel) || request.ViewModelType == typeof(MainContainerViewModel))
                {
                    MasterNavigationController = null;
                }

                if (request.ParameterValues != null && request.ParameterValues.Keys.Contains("pop"))
                {
                    MasterNavigationController.PopViewController(true);
                    return;
                }

                //if (request.ViewModelType == typeof(ContactUsViewModel))
                //{
                //    var viewController = (UIViewController)Mvx.Resolve<IMvxIosViewCreator>().CreateView(request);
                //    MasterNavigationController.PresentViewController(CreateACNavigationController(viewController), true, null);
                //    return;
                //}


                base.Show(request);
                return;
            }


            //MainContanier as rootView
            if (Host != null)
            {
                //Close the sidebar if it is open
                if (Host.SidebarControler.IsOpen)
                    Host.SidebarControler.ToggleMenu();

                var viewController = (UIViewController)Mvx.Resolve<IMvxIosViewCreator>().CreateView(request);//IMvxTouchViewCreator
                var currentNav = Host.SidebarControler.ContentAreaController as UINavigationController;

                if (request.ParameterValues != null)
                {
                    if (request.ParameterValues.Keys.Contains("popto"))
                    {
                        foreach (UIViewController poptoController in currentNav.ViewControllers)
                        {
                            if (poptoController.GetType() == viewController.GetType())
                            {
                                currentNav.PopToViewController(poptoController, true);
                                MvxViewController sideView = Host.SidebarControler.MenuAreaController as MvxViewController;
                                (sideView.ViewModel as ActionsMenuViewModel).ListType = ActionsMenuTypes.SITE_ACTION;
                                Host.SidebarControler.MenuWidth = 150;
                                previousController = null;
                                return;
                            }
                        }
                    }
                    else if (request.ParameterValues.Keys.Contains("pop"))
                    {
                        //shwoing actions menu screen when going back to DeviceHomeView
                        if (previousController != null && previousController is DeviceHomeView)
                        {
                            Host.SidebarControler.MenuWidth = 150;
                            //previousController = null;
                        }

                        if (viewController is UploadView || viewController is SyncSitesView || viewController is ReplacementView)
                        {
                            Host.SidebarControler.MenuWidth = 150;
                        }

                        if (viewController is DeviceHomeView)
                        {
                            MvxViewController sideView = Host.SidebarControler.MenuAreaController as MvxViewController;
                            (sideView.ViewModel as ActionsMenuViewModel).ListType = ActionsMenuTypes.SITE_ACTION;
                        }
                        if (viewController is ConnectToDeviceView || viewController is ContactUsView)
                        {
                            UIViewController sideMenuView = (UIViewController)Mvx.Resolve<IMvxIosViewCreator>().CreateView(new SideMenuViewModel());
                            Host.SidebarControler.ChangeMenuView(sideMenuView);
                            Host.SidebarControler.MenuWidth = 250;
                            previousController = null;
                        }

                        Host.NavController.PopViewController(true);
                        return;
                    }
                    else
                    {
                        //Sidemenu ContactUsView
                        if (viewController is ContactUsView || viewController is UploadView || viewController is ReplacementView)
                        {
                            Host.SidebarControler.MenuWidth = 0;
                        }

                        //Hidding actions menu screen when going to other screens from DeviceHomeView
                        if (previousController != null && previousController is DeviceHomeView)
                        {
                            Host.SidebarControler.MenuWidth = 0;
                        }

                        //refresh list in ActionMenuViewModel
                        if (viewController is DeviceHomeView)
                        {
                            MvxViewController sideView = Host.SidebarControler.MenuAreaController as MvxViewController;
                            (sideView.ViewModel as ActionsMenuViewModel).ListType = ActionsMenuTypes.DEVICE;
                            //saving only DeviceHomeView in previousController
                            previousController = viewController;
                            Host.SidebarControler.MenuWidth = 150;
                        }

                        if (viewController is SiteViewDevicesView)
                        {
                            MvxViewController sideView = Host.SidebarControler.MenuAreaController as MvxViewController;
                            (sideView.ViewModel as ActionsMenuViewModel).ListType = ActionsMenuTypes.SITE_VIEW;
                            previousController = viewController;
                            Host.SidebarControler.MenuWidth = 150;
                        }
                    }
                }
                else
                {
                    if (viewController is SyncSitesView)
                    {
                        Host.SidebarControler.MenuWidth = 0;
                    }
                    //ConnectToDeviceView change the sidemenu to ActionsMenu
                    if (viewController is ConnectToDeviceView)
                    {
                        UIViewController sideMenuView = (UIViewController)Mvx.Resolve<IMvxIosViewCreator>().CreateView(new ActionsMenuViewModel());
                        Host.SidebarControler.ChangeMenuView(sideMenuView);
                        Host.SidebarControler.MenuWidth = 150; //Showing only 150px for the ActionsMenu
                    }

                    //Hidding actions menu screen when going to other screens from DeviceHomeView
                    if (previousController != null && previousController is DeviceHomeView)
                    {
                        Host.SidebarControler.MenuWidth = 0;
                    }

                    //refresh list in ActionMenuViewModel
                    if (viewController is DeviceHomeView)
                    {
                        MvxViewController sideView = Host.SidebarControler.MenuAreaController as MvxViewController;
                        (sideView.ViewModel as ActionsMenuViewModel).ListType = ActionsMenuTypes.DEVICE;
                        //saving only DeviceHomeView in previousController
                        previousController = viewController;
                    }
                }

                Host.NavController.PushViewController(viewController, true);
            }

            //Added this code when we click on ActionbarMenuButtons the sidebar is stayed open. to close the menu this code is added
            if (Host != null && Host.SidebarControler != null)
            {
                Host.SidebarControler.CloseMenu();
            }

        }

        /// <summary>
        /// Creates the navigation controller.
        /// </summary>
        /// <returns>The navigation controller.</returns>
        /// <param name="viewController">View controller.</param>
        protected override UINavigationController CreateNavigationController(UIViewController viewController)
        {
            var navController = base.CreateNavigationController(viewController);
            navController.NavigationBarHidden = true;
            navController.NavigationBar.TintColor = UIColorUtility.FromHex(ACColors.DARK_BLUE_COLOR);
            return navController;
        }

        public void Register(MainContainerView host)
        {
            Host = host;
        }

        /// <summary>
        /// Creates the ACT Chargers customized Navigation controller.
        /// </summary>
        /// <returns>The AC Navigation controller.</returns>
        /// <param name="viewController">View controller.</param>
        public UINavigationController CreateACNavigationController(UIViewController viewController)
        {
            UINavigationController acNavController = new UINavigationController(viewController);
            acNavController.NavigationBar.TintColor = UIColorUtility.FromHex(ACColors.DARK_BLUE_COLOR);
            return acNavController;
        }
    }
}
