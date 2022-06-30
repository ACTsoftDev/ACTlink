using actchargers.ViewModels;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;

namespace actchargers
{
    public class CustomAppStart : MvxNavigatingObject, IMvxAppStart
    {
        public void Start(object hint = null)
        {
            bool userLoggedIn = Mvx.Resolve<IUserPreferences>().GetBool(ACConstants.USER_PREFS_USER_LOGGED_IN);

            if (userLoggedIn)
                ShowViewModel<MainContainerViewModel>();
            else
                ShowViewModel<LoginViewModel>();
        }
    }
}
