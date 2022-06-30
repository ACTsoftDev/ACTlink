using System;
using actchargers.ViewModels;
using CoreGraphics;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Core.ViewModels;
using MvvmCross.iOS.Views;
using SidebarNavigation;
using UIKit;

namespace actchargers.iOS
{
    public partial class ConnectView : BaseView
    {
        ConnectViewModel currentViewModel;
        /// <summary>
        /// Gets the sidebar controller.
        /// </summary>
        /// <value>The sidebar controller.</value>
        protected SidebarController SidebarController
        {
            get
            {
                return (UIApplication.SharedApplication.Delegate as AppDelegate).SideBar;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:actchargers.iOS.ConnectView"/> class.
        /// </summary>
        public ConnectView() : base("ConnectView", null)
        {
        }

        /// <summary>
        /// Views the did load.
        /// </summary>
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.

            //Adding Burger button in  Navigation
            UIButton menuButton = new UIButton(new CGRect(0, 0, 19, 17));
            //menuButton.SetTitle("Menu", UIControlState.Normal);
            menuButton.SetImage(UIImage.FromBundle("menu.png"), UIControlState.Normal);
            menuButton.ShowsTouchWhenHighlighted = true;
            menuButton.TouchUpInside += delegate
            {
                //open/close sidemenu
                SidebarController.ToggleMenu();
            };

            var menuItem = new UIBarButtonItem(menuButton);
            NavigationItem.RightBarButtonItem = menuItem;

            currentViewModel = ViewModel as ConnectViewModel;
            //Bind ConnectView fields for Admin
            //else hide ConnectView fields

            this.CreateBinding(ssidLbl).To((ConnectViewModel vm) => vm.SSIDTitle).Apply();
            this.CreateBinding(ssidTF).To((ConnectViewModel vm) => vm.SSIDText).Apply();
            this.CreateBinding(passwordLbl).To((ConnectViewModel vm) => vm.PassowordTitle).Apply();
            this.CreateBinding(passwordTF).To((ConnectViewModel vm) => vm.PasswordText).Apply();



            updateConnectView(currentViewModel.IsAdmin);

            this.CreateBinding(connectBtn).For("Title").To((ConnectViewModel vm) => vm.ConnectButtonTitle).Apply();
            this.CreateBinding(messageLbl).To((ConnectViewModel vm) => vm.MessageText).Apply();
            this.CreateBinding(connectBtn).To((ConnectViewModel vm) => vm.ConnectBtnClicked).Apply();
            this.CreateBinding(uploadBtn).To((ConnectViewModel vm) => vm.UploadDataBtnClicked).Apply();
            this.CreateBinding(uploadTitleLbl).To((ConnectViewModel vm) => vm.UploadTitle).Apply();
            this.CreateBinding(uploadSubTitleLbl).To((ConnectViewModel vm) => vm.UploadSubTitle).Apply();
            this.CreateBinding(syncSitesBtn).To((ConnectViewModel vm) => vm.SyncSitesBtnClicked).Apply();
            this.CreateBinding(synSiteTitleLbl).To((ConnectViewModel vm) => vm.SyncSiteTitle).Apply();
            this.CreateBinding(synSiteSubTitleLbl).To((ConnectViewModel vm) => vm.SyncSiteSubTitle).Apply();
            this.CreateBinding(pushBackupBtn).For("Title").To((ConnectViewModel vm) => vm.PushBackupTitle).Apply();
            this.CreateBinding(pushBackupBtn).To((ConnectViewModel vm) => vm.PushBackupBtnClicked).Apply();

            /// <summary>
            /// Ons the property changed.
            /// </summary>
            /// <param name="sender">Sender.</param>
            /// <param name="e">E.</param>
            ((MvxNotifyPropertyChanged)this.ViewModel).PropertyChanged += OnPropertyChanged;

            segmentController.TintColor = UIColorUtility.FromHex(ACColors.DARK_BLUE_COLOR);
            segmentController.ValueChanged += SegmentController_ValueChanged;

            segmentController.SetTitle(AppResources.mobile_router_mode,0);
            segmentController.SetTitle(AppResources.stationary_router,1);
            segmentController.ApportionsSegmentWidthsByContent = true;
        }

        void SegmentController_ValueChanged(object sender, EventArgs e)
        {
            if(segmentController.SelectedSegment == 0)
            {
                currentViewModel.TabChanged(UiConnectionType.ROUTER);

                updateConnectView(currentViewModel.IsAdmin);
            }
            else
            {
                currentViewModel.TabChanged(UiConnectionType.STATIONARY);
            }
        }

        /// <summary>
        /// Ons the property changed.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        void OnPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("ConnectButtonTitle"))
            {
                //Changing the connect button color 
                //Selection/deselection color change
                if (currentViewModel.ConnectButtonTitle == AppResources.connect)
                {
					connectBtn.BackgroundColor = UIColor.FromRGB(65 / 255.0f, 140 / 255.0f, 211 / 255.0f);
					pushBackupBtn.BackgroundColor = UIColor.FromRGB(65 / 255.0f, 140 / 255.0f, 211 / 255.0f);
				}
                else
                {
					connectBtn.BackgroundColor = UIColor.FromRGB(228 / 255.0f, 81 / 255.0f, 85 / 255.0f);
					pushBackupBtn.BackgroundColor = UIColor.FromRGB(228 / 255.0f, 81 / 255.0f, 85 / 255.0f);
				}
            }
            else if (e.PropertyName.Equals("ShowStationaryRouterModeFields"))
            {
                //updateConnectView(currentViewModel.ShowStationaryRouterModeFields);
                updateConnectView(currentViewModel.IsAdmin);
            }
        }

        void updateConnectView(bool showFields)
        {
            if (showFields)
            {
                //show fields 
                connectViewHeight.Constant = 110;
                connectView.Hidden = false;
                connectButtonTopConstraint.Constant = 190;
            }
            else
            {
                //hide fields 
                connectViewHeight.Constant = 0;
                connectView.Hidden = true;
                connectButtonTopConstraint.Constant = 50;
            }
        }

        /// <summary>
        /// Toucheses the began.
        /// </summary>
        /// <param name="touches">Touches.</param>
        /// <param name="evt">Evt.</param>
        public override void TouchesBegan(NSSet touches, UIEvent evt)
        {
            base.TouchesBegan(touches, evt);
            View.EndEditing(true);
        }
    }
}

