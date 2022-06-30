using System;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.iOS.Views;
using UIKit;

namespace actchargers.iOS
{
    public partial class ContactUsView : BaseView
    {
        ContactUsViewModel currentViewModel;
        /// <summary>
        /// Initializes a new instance of the <see cref="T:actchargers.iOS.ContactUsView"/> class.
        /// </summary>
        public ContactUsView() : base("ContactUsView", null)
        {
        }

        /// <summary>
        /// Views the did load.
        /// </summary>
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.
            currentViewModel = ViewModel as ContactUsViewModel;

            UIBarButtonItem cancelBtn = new UIBarButtonItem(UIBarButtonSystemItem.Cancel, delegate
            {
                currentViewModel.OnBackButtonClick();
            });
            NavigationItem.LeftBarButtonItem = cancelBtn;

            string htmlFile = NSBundle.MainBundle.PathForResource(currentViewModel.HTMLFileName, "html");
            webView.LoadRequest(new NSUrlRequest(new NSUrl(htmlFile)));
            webView.BackgroundColor = UIColor.White;
        }

        /// <summary>
        /// Shoulds the start load.
        /// </summary>
        /// <returns><c>true</c>, if start load was shoulded, <c>false</c> otherwise.</returns>
        /// <param name="webView">Web view.</param>
        /// <param name="request">Request.</param>
        /// <param name="navigationType">Navigation type.</param>
        [Export("webView:shouldStartLoadWithRequest:navigationType:")]
        public virtual bool ShouldStartLoad(UIWebView webView, NSUrlRequest request, UIWebViewNavigationType navigationType)
        {
            if (navigationType == UIWebViewNavigationType.LinkClicked)
            {
                if (request.ToString().Contains("tel:"))
                {
                    //Clicked on telephone number
                }
                else
                {
                    //Clicked on Email
                    currentViewModel.EmailLinkClickCommand.Execute();
                }
            }
            return true;
        }
    }
}

