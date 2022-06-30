using System;
using actchargers.ViewModels;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.iOS.Views;
using UIKit;

namespace actchargers.iOS
{
    public partial class UsageAgreementView : BaseView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:actchargers.iOS.UsageAgreementView"/> class.
        /// </summary>
        public UsageAgreementView() : base("UsageAgreementView", null)
        {
        }

        /// <summary>
        /// Views the did load.
        /// </summary>
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.

            //Hiding back button 
            NavigationItem.HidesBackButton = true;

            /// <summary>
            /// Decline button
            /// </summary>
            UIBarButtonItem declineBtn = new UIBarButtonItem(AppResources.decline, UIBarButtonItemStyle.Plain, delegate
            {
                UsageAgreementViewModel currentViewModel = ViewModel as UsageAgreementViewModel;
                currentViewModel.DeclineBtnClickCommand.Execute();
            });
            NavigationItem.RightBarButtonItem = declineBtn;

            //Binding 
            this.CreateBinding(acceptBtn).For("Title").To((UsageAgreementViewModel vm) => vm.AcceptBtnTitle).Apply();
            this.CreateBinding(acceptBtn).To((UsageAgreementViewModel vm) => vm.AcceptBtnClickCommand).Apply();
            //this.CreateBinding(agreementTV).For(agreementTV => agreementTV.Text).To((UsageAgreementViewModel vm) => vm.UsageAgreementText).Apply();

            string htmlFile = NSBundle.MainBundle.PathForResource("usage_agreement", "html");
            agreementWebView.LoadRequest(new NSUrlRequest(new NSUrl(htmlFile)));
            agreementWebView.BackgroundColor = UIColor.White;
          
        }
    }
}