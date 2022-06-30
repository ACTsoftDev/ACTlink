using System.Linq;
using MvvmCross.Platform;

namespace actchargers
{
    public static class ManagementUsers
    {
        static int THIKER_USER_ID = 18;
        static int BILAL_USER_ID = 471;
        static int CARLOS_USER_ID = 965;

        static readonly int[] Managers =
        {
            THIKER_USER_ID,
            BILAL_USER_ID,
            CARLOS_USER_ID
        };

        public static bool IsManagementUser()
        {
            int userId = GetCachedUserId();
            bool isManagementUser = IsInArray(userId);

            return isManagementUser;
        }

        static int GetCachedUserId()
        {
            return Mvx.Resolve<IUserPreferences>()
                      .GetInt(ACConstants.USER_PREFS_USER_ID);
        }

        static bool IsInArray(int userId)
        {
            bool isInArray = Managers.Contains(userId);

            return isInArray;
        }
    }
}
