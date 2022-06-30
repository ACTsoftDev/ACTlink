using System;
using System.Threading.Tasks;

namespace actchargers.Code.Helpers.Login
{
    public class AuthenticationHelper
    {
        string emailId;
        string password;

        LoginAuthenticationHelper loginAuthenticationHelper;

        public AuthenticationHelper()
        {
            Init();
        }

        void Init()
        {
            loginAuthenticationHelper = new LoginAuthenticationHelper();
        }

        public DBUser User
        {
            get
            {
                return loginAuthenticationHelper?.User;
            }
        }

        public async Task<AuthenticationStatus> AuthenticateCachedAndProcessResult()
        {
            string cachedEmailId = AppUserPreferences.GetString(ACConstants.PREF_USERNAME);
            string cachedPassword = AppUserPreferences.GetString(ACConstants.PREF_PASSWORD);

            return await AuthenticateAndProcessResult
                (cachedEmailId, cachedPassword);
        }

        public async Task<AuthenticationStatus>
        AuthenticateAndProcessResult(string emailId, string password)
        {
            this.emailId = emailId;
            this.password = password;

            var authenticationStatus = await loginAuthenticationHelper
                .AuthenticateByCredentials(emailId, password);

            ProcessResult(authenticationStatus);

            return authenticationStatus;
        }

        void ProcessResult(AuthenticationStatus authenticationStatus)
        {
            switch (authenticationStatus)
            {
                case AuthenticationStatus.REJECTED:
                    OnNotAuthenticated();

                    break;

                case AuthenticationStatus.REJECTED_AND_MUST_REMOVED:
                    OnNotAuthenticated();

                    break;

                case AuthenticationStatus.MUST_UPDATE:
                    OnExpiredApi();

                    break;

                case AuthenticationStatus.OK_WITH_AGREEMENT:
                    SaveUserInfo();

                    break;

                case AuthenticationStatus.OK:
                    SaveAndUpdateUserInfo();

                    break;

                case AuthenticationStatus.ERROR:
                    ShowConnectionError();

                    break;
            }
        }

        void OnNotAuthenticated()
        {
            ACUserDialogs.ShowAlertWithTitleAndOkButton(AppResources.invalid_username_password);
        }

        void OnExpiredApi()
        {
            SoftwareUpdateHelper.ShowUpdateWarning();
        }

        void SaveAndUpdateUserInfo()
        {
            SaveUserInfo();
            UpdateUserDetails();
        }

        void SaveUserInfo()
        {
            try
            {
                TrySaveUserInfo();
            }
            catch (Exception ex)
            {
                Logger.AddLog(true, "X79" + ex);
            }
        }

        void TrySaveUserInfo()
        {
            DBUser user = loginAuthenticationHelper.User;
            string returnValue = loginAuthenticationHelper.Response.returnValue.ToString();

            user.EmailId = emailId;

            AppUserPreferences.SetString(ACConstants.PREF_LOGIN_RESPONSE,
                                      returnValue);

            AppUserPreferences.SetString(ACConstants.PREF_USERNAME, emailId);
            AppUserPreferences.SetString(ACConstants.PREF_PASSWORD, password);

            WriteUserInfoToLog(user);
        }

        void WriteUserInfoToLog(DBUser user)
        {
            Logger.AddLog(false, "Logged In user:" + user.UserID);
            Logger.AddLog(false, "Admin:" + user.IsAllowEditing());
            Logger.AddLog(false, "Logged In user:"
                          + ControlObject.userID.ToString());
        }

        void UpdateUserDetails()
        {
            DBUser user = loginAuthenticationHelper.User;
            user.Password = AppUserPreferences.GetString(ACConstants.PREF_PASSWORD);

            AppUserPreferences.SetInt(ACConstants.USER_PREFS_USER_ID, user.UserID);

            DbSingleton.DBManagerServiceInstance
                       .GetDBUserLoader()
                       .DeleteAll();

            DbSingleton.DBManagerServiceInstance
                       .GetDBUserLoader()
                       .InsertOrReplaceWithChildren(user);

            AppUserPreferences.SetBool(ACConstants.USER_PREFS_USER_LOGGED_IN,
                                    true);
            AppUserPreferences.SetBool(ACConstants.USER_PREFS_ALLOW_EDITING,
                                    user.IsAllowEditing());
        }

        public void ClearUserDetails()
        {
            ControlObject.userID = 0;

            AppUserPreferences.Clear();
        }

        public int GetCachedUserId()
        {
            return AppUserPreferences.GetInt(ACConstants.USER_PREFS_USER_ID);
        }

        public bool IsThereCachedUser()
        {
            int userId = AppUserPreferences.GetInt(ACConstants.USER_PREFS_USER_ID);

            return userId != 0;
        }

        void ShowConnectionError()
        {
            ACUserDialogs.ShowAlertWithTitleAndOkButton
                         ("Can't Connect to ACTView");
        }
    }
}
