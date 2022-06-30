using System;
using actchargers.ViewModels;
using Foundation;
using MvvmCross.iOS.Views;
using MvvmCross.Platform;
using SidebarNavigation;
using UIKit;

namespace actchargers.iOS
{
    public class BackViewController : BaseView
    {
        public static int KEYBOARD_HEIGHT = 216;

        protected SidebarController SidebarController
        {
            get
            {
                return (UIApplication.SharedApplication.Delegate as AppDelegate).SideBar;
            }
        }

        protected BackViewController(string nibName, Foundation.NSBundle bundle)
            : base(nibName, bundle)
        {

        }

        public BackViewController() : base("BackViewController", null)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.

            CreateBackButton();
        }

        protected void CreateBackButton()
        {
            UIBarButtonItem backBarButton = new UIBarButtonItem(UIImage.FromBundle("back_arrow.png"), UIBarButtonItemStyle.Plain, delegate
            {
                BackButton_TouchUpInside(null, null);
            })
            {
                TintColor = UIColorUtility.FromHex(ACColors.DARK_BLUE_COLOR)
            };

            NavigationItem.LeftBarButtonItem = backBarButton;
        }

        public virtual void BackButton_TouchUpInside(object sender, EventArgs e)
        {
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillShowNotification, OnKeyBoardShown);
        }

        public void OnKeyBoardShown(NSNotification notification)
        {
            var keyboardBounds = (NSValue)notification.UserInfo.ObjectForKey(UIKeyboard.BoundsUserInfoKey);
            var keyboardSize = keyboardBounds.RectangleFValue;
            KEYBOARD_HEIGHT = (int)keyboardSize.Height;
        }
    }
}

