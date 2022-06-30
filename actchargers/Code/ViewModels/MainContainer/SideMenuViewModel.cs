using System.Collections.Generic;
using System.Windows.Input;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;

namespace actchargers.ViewModels
{
    public class SideMenuViewModel : BaseViewModel
    {
        public string UserNameImageName { get; set; }

        public string UserName { get; set; }

        public string EmailId { get; set; }

        public string LogoutTitle { get; set; }

        public string LogoutImageName { get; set; }

        List<MenuViewItem> homeScreenMenuItems;
        public List<MenuViewItem> HomeScreenMenuItems
        {
            get
            {
                return homeScreenMenuItems;
            }
        }

        public SideMenuViewModel()
        {
            LogoutClicked = false;
            Initialize();
        }

        void Initialize()
        {
            UserNameImageName = "avatar";

            int userId = Mvx.Resolve<IUserPreferences>().GetInt(ACConstants.USER_PREFS_USER_ID);

            DBUser currentUser =
                DbSingleton.DBManagerServiceInstance
                           .GetDBUserLoader()
                           .GetCurrentUser(userId);

            UserName = currentUser.Name;
            EmailId = currentUser.EmailId;
            LogoutTitle = AppResources.logout;
            LogoutImageName = AppResources.logout_img;
            InvokeOnMainThread(() =>
            {
                SetResourcesStrings();
                this.RaisePropertyChanged(() => this.HomeScreenMenuItems);
            });
        }

        void SetResourcesStrings()
        {
            homeScreenMenuItems = new List<MenuViewItem> {
                new MenuViewItem {
                    Title = AppResources.contact_us,
                    ViewModelType = typeof(ContactUsViewModel),
                    DefaultImage = "conatct_us",
                    SelectedImage = "conatct_us"
                },
                new MenuViewItem {
                    Title = AppResources.about_us,
                    ViewModelType = typeof(AboutUsViewModel),
                    DefaultImage = "about_us",
                    SelectedImage = "about_us"
                }
            };
        }

        MvxCommand<MenuViewItem> m_SelectMenuItemCommand;
        public ICommand SelectMenuItemCommand
        {
            get
            {
                return this.m_SelectMenuItemCommand ?? (this.m_SelectMenuItemCommand = new MvxCommand<MenuViewItem>(this.ExecuteSelectMenuItemCommand));
            }
        }

        void ExecuteSelectMenuItemCommand(MenuViewItem item)
        {
            if (item.ViewModelType == typeof(ContactUsViewModel))
                ShowViewModel<ContactUsViewModel>(new { isForContactUs = (item.Title == AppResources.contact_us) ? "true" : "false" });
            else if (item.ViewModelType == typeof(AboutUsViewModel))
                ShowViewModel<AboutUsViewModel>();
            else
                ACUserDialogs.ShowAlert(AppResources.dev_in_progress);
        }

        public IMvxCommand LogoutBtnClicked
        {
            get { return new MvxCommand(OnLogoutBtnClicked); }
        }

        void OnLogoutBtnClicked()
        {
            ACUserDialogs.ShowAlertWithTwoButtons(AppResources.logout_confirmation, "", AppResources.logout, AppResources.cancel, () => OnLogoutClick(), null);
        }

        bool OnLogoutClick()
        {
            Logout();

            return true;
        }
    }
}