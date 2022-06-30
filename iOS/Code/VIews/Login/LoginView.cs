using System;
using actchargers.ViewModels;
using MvvmCross.Binding.BindingContext;
using MvvmCross.iOS.Views;
using UIKit;

namespace actchargers.iOS
{
    public partial class LoginView : BaseView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:actchargers.iOS.LoginView"/> class.
        /// </summary>
        public LoginView() : base("LoginView", null)
        {
        }

        /// <summary>
        /// Views the did load.
        /// </summary>
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.

            NavigationController.NavigationBarHidden = false;
            NavigationItem.HidesBackButton = true;
            this.CreateBinding(emailIdTF).To((LoginViewModel vm) => vm.EmailId).Apply();
            this.CreateBinding(passwordTF).To((LoginViewModel vm) => vm.Password).Apply();
            this.CreateBinding(emailIdTF).For(emailIdTF => emailIdTF.Placeholder).To((LoginViewModel vm) => vm.EmailIdPlaceholder).Apply();
            this.CreateBinding(passwordTF).For(passwordTF => passwordTF.Placeholder).To((LoginViewModel vm) => vm.PasswordPlaceholder).Apply();
            this.CreateBinding(havingTroubleLbl).To((LoginViewModel vm) => vm.HavingTrouble).Apply();
            this.CreateBinding(contactUsBtn).For("Title").To((LoginViewModel vm) => vm.ContactUs).Apply();
            this.CreateBinding(notRegisteredLbl).To((LoginViewModel vm) => vm.NotRegistered).Apply();
            this.CreateBinding(RegisterAtBtn).For("Title").To((LoginViewModel vm) => vm.RegisterAt).Apply();
            this.CreateBinding(contactUsBtn).To((LoginViewModel vm) => vm.ContactUsBtnClickCommand).Apply();

            LoginViewModel currentViewModel = ViewModel as LoginViewModel;
            /// <summary>
            /// ShouldReturn.
            /// </summary>
            emailIdTF.ShouldReturn += delegate
            {
                passwordTF.BecomeFirstResponder();
                return true;
            };

            /// <summary>
            /// ShouldReturn
            /// </summary>
            passwordTF.ShouldReturn += delegate
            {
                passwordTF.ResignFirstResponder();
                currentViewModel.LoginBtnClickCommand.Execute();
                return true;
            };

            /// <summary>
            /// TouchUpInside for RegisterAtBtn
            /// </summary>
            RegisterAtBtn.TouchUpInside += delegate
            {
                Foundation.NSUrl registerUrl = new Foundation.NSUrl(currentViewModel.RegisterAtUrl);
                /// <summary>
                /// SharedApplication will open the url in external browser
                /// </summary>
                if (UIApplication.SharedApplication.CanOpenUrl(registerUrl))
                {
                    UIApplication.SharedApplication.OpenUrl(registerUrl);
                }
            };

            View.AddGestureRecognizer(new UITapGestureRecognizer(() =>
            {
                View.EndEditing(true);
            }));
        }

        /// <summary>
        /// Views the will appear.
        /// </summary>
        /// <param name="animated">If set to <c>true</c> animated.</param>
        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            emailIdTF.BecomeFirstResponder();
        }
    }
}