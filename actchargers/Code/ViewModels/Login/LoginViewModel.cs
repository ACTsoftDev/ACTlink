using System;
using System.Diagnostics;
using System.Threading.Tasks;
using actchargers.Code.Helpers.Login;
using actchargers.Code.Utility;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using Plugin.Connectivity;

namespace actchargers.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        AuthenticationHelper authenticationHelper;

        IUserPreferences userPreferences;

        public string EmailIDText { get; set; }
        public string PassowordTitle { get; set; }

        string _emailId;
        public string EmailId
        {
            get { return _emailId; }
            set
            {
                _emailId = value;
                RaisePropertyChanged(() => EmailId);
            }
        }

        public string EmailIdPlaceholder { get; set; }

        string _password;
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                RaisePropertyChanged(() => Password);
            }
        }

        public bool IsLoginInProgress { get; private set; }

        public string PasswordPlaceholder { get; set; }

        public string HavingTrouble { get; set; }

        public string ContactUs { get; set; }

        public string NotRegistered { get; set; }

        public string RegisterAt { get; set; }

        public string RegisterAtUrl { get; set; }

        public LoginViewModel()
        {
            authenticationHelper = new AuthenticationHelper();

            userPreferences = Mvx.Resolve<IUserPreferences>();

            EmailId = string.Empty;
            Password = string.Empty;

#if DEBUG

            // EmailId = "innominds@act-view.com";
            // Password = "innominds";
            EmailId = "bsammour@act-chargers.com";
            Password = "act123";

#endif

            ViewTitle = AppResources.login;
            EmailIdPlaceholder = AppResources.enter_email_id;
            PasswordPlaceholder = AppResources.enter_password;
            HavingTrouble = AppResources.having_trouble;
            ContactUs = AppResources.contact_us;
            NotRegistered = AppResources.not_registered;
            RegisterAt = AppResources.register_at_act_view;
            RegisterAtUrl = AppResources.register_at_url;
        }

        public IMvxCommand LoginBtnClickCommand
        {
            get
            {
                return new MvxCommand(async () =>
                {
                    await OnLoginClick();
                });
            }
        }

        #region Login

        async Task OnLoginClick()
        {
            if (IsLoginInProgress)
                return;

            IsLoginInProgress = true;

            if (LoginDataValidator
                .ShowValidationErrorIfExist(EmailId, Password))
            {
                IsLoginInProgress = false;
                return;
            }

            bool notConnectted = !CrossConnectivity.Current.IsConnected;
            if (notConnectted)
            {
                IsLoginInProgress = false;
                ACUserDialogs.ShowAlert("Network connectivity unavailable.");

                return;
            }

            ACUserDialogs.ShowProgress();

            try
            {
                await AuthenticateUserCredentials();
            }
            catch (Exception ex)
            {
                IsLoginInProgress = false;

                Debug.WriteLine(GetType() + ex.Message);

                ACUserDialogs.ShowAlert("Login Failed.");
            }

            ACUserDialogs.HideProgress();
        }

        async Task AuthenticateUserCredentials()
        {
            var status = await authenticationHelper
                .AuthenticateAndProcessResult(EmailId, Password);

            IsLoginInProgress = false;

            switch (status)
            {
                case AuthenticationStatus.OK_WITH_AGREEMENT:
                    {
                        IsLoggingOut = false;
                        GoToAgrrement();
                    }

                    break;

                case AuthenticationStatus.OK:
                    {
                        IsLoggingOut = false;
                        ShowViewModel<MainContainerViewModel>();
                    }

                    break;
            }
        }

        void GoToAgrrement()
        {
            string userText = JsonParser
                .SerializeObject(authenticationHelper.User);

            ShowViewModel<UsageAgreementViewModel>
            (new { userObj = userText });
        }

        #endregion

        public IMvxCommand ContactUsBtnClickCommand
        {
            get { return new MvxCommand(OnContactUsBtnClick); }
        }

        void OnContactUsBtnClick()
        {
            ShowViewModel<ContactUsViewModel>(new { isForContactUs = "true" });
        }
    }
}
