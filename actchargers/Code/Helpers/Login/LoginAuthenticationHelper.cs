using System;
using System.Diagnostics;
using System.Threading.Tasks;
using actchargers.Code.Utility;

namespace actchargers.Code.Helpers.Login
{
    public class LoginAuthenticationHelper
    {
        public ACTViewResponse Response
        {
            get;
            private set;
        }

        public DBUser User
        {
            get;
            private set;
        }

        public async Task<AuthenticationStatus> AuthenticateByCredentials
        (string emailId, string password)
        {
            return await Authenticate(emailId, password);
        }

        async Task<AuthenticationStatus> Authenticate
        (string emailId, string password)
        {
            Response = await ACTViewConnect
                .AuthenticateUser(emailId, password);

            var responseType = Response.responseType;

            ParseUser();

            return GetAuthenticationStatus(responseType, User);
        }

        void ParseUser()
        {
            try
            {
                TryParseUser();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        void TryParseUser()
        {
            User = JsonParser.DeserializeObject<DBUser>
                                 (Response.returnValue.ToString());
        }

        AuthenticationStatus GetAuthenticationStatus
        (ActviewResponseType responseType, DBUser user)
        {
            switch (responseType)
            {
                case ActviewResponseType.notAuthenticated:
                    return AuthenticationStatus.REJECTED;

                case ActviewResponseType.expiredAPI:
                    return AuthenticationStatus.MUST_UPDATE;

                case ActviewResponseType.validResponse:
                    return GetOkOrAgreement(user);

                default:
                    return AuthenticationStatus.ERROR;
            }
        }

        AuthenticationStatus GetOkOrAgreement(DBUser user)
        {
            if (IsPreviousUser(user))
                return AuthenticationStatus.OK;
            else
                return AuthenticationStatus.OK_WITH_AGREEMENT;
        }

        bool IsPreviousUser(DBUser user)
        {
            var previousUser = GetPreviousUser(user.UserID);

            return (previousUser != null);
        }

        DBUser GetPreviousUser(int userID)
        {
            return
                DbSingleton.DBManagerServiceInstance
                           .GetDBUserLoader()
                           .GetCurrentUser(userID);
        }
    }
}
