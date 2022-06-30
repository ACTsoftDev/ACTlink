using System;
using System.Threading.Tasks;
using actchargers.Code.Helpers.Login;
using actchargers.Code.Helpers.Versions;

namespace actchargers
{
    public class BackgroundSessionManager
    {
        const int CHECK_SESSION_PERIOD_IN_HOURS = 2;

        AuthenticationHelper authenticationHelper;
        VersionsChecker versionsChecker;

        public delegate void LoggingOutEventHandler(object sender, EventArgs e);
        public event LoggingOutEventHandler LoggingOut;

        public BackgroundSessionManager()
        {
            Init();
        }

        void Init()
        {
            authenticationHelper = new AuthenticationHelper();
            versionsChecker = new VersionsChecker();
        }

        public async Task CheckSession()
        {
            if (CanCheckSession())
                await CheckSessionIfOk();

            ControlObject.LoadCached();
        }

        bool CanCheckSession()
        {
            DateTime now = DateTime.Now;
            DateTime lastCheckSessionWithPeriod =
                StaticDataAndHelperFunctions
                    .lastCheckSession.AddHours(CHECK_SESSION_PERIOD_IN_HOURS);

            return now > lastCheckSessionWithPeriod;
        }

        async Task CheckSessionIfOk()
        {
            StaticDataAndHelperFunctions.lastCheckSession = DateTime.Now;

            if (IsThereCachedUser())
                await CheckAll();
        }

        public bool IsThereCachedUser()
        {
            return authenticationHelper.IsThereCachedUser();
        }

        async Task CheckAll()
        {
            if (ExitByLoginPeriod())
                return;

            bool isReachable =
                await InternetConnectivityManager.IsReachableAsync();
            if (isReachable)
                await CheckCachedSession();

            CheckVersion();
        }

        bool ExitByLoginPeriod()
        {
            bool isNotValidLoginDate = IsNotValidLoginDate();
            if (isNotValidLoginDate)
                WarnAndLogout();

            return isNotValidLoginDate;
        }

        bool IsNotValidLoginDate()
        {
            return !StaticDataAndHelperFunctions.ValidateLoginDate();
        }

        void WarnAndLogout()
        {
            ACUserDialogs.ShowAlert(AppResources.login_period_error);
        }

        async Task CheckCachedSession()
        {
            var status = await authenticationHelper
                .AuthenticateCachedAndProcessResult();
            ProcessResult(status);
        }

        void ProcessResult(AuthenticationStatus authenticationStatus)
        {
            switch (authenticationStatus)
            {
                case AuthenticationStatus.REJECTED:
                    Logout();

                    break;

                case AuthenticationStatus.REJECTED_AND_MUST_REMOVED:
                    LogoutAndRemove();

                    break;

                case AuthenticationStatus.MUST_UPDATE:
                    ShowUpdateWarningAndLogout();

                    break;
            }
        }

        public void Logout()
        {
            FireLogout();
        }

        void FireLogout()
        {
            LoggingOut?.Invoke(this, EventArgs.Empty);
        }

        void LogoutAndRemove()
        {
            RemoveUserInfo();

            FireLogout();
        }

        void RemoveUserInfo()
        {
            authenticationHelper.ClearUserDetails();
        }

        void ShowUpdateWarningAndLogout()
        {
            Action handleAction = new Action(Logout);
            SoftwareUpdateHelper.ShowUpdateWarningWithAction(handleAction);
        }

        void CheckVersion()
        {
            versionsChecker.Check();
        }
    }
}
