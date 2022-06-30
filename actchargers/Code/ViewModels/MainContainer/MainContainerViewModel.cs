using MvvmCross.Core.ViewModels;

namespace actchargers.ViewModels
{
    public class MainContainerViewModel : BaseViewModel
    {
        public IMvxCommand OpenConnectViewCommand
        {
            get
            {
                return new MvxCommand(OpenConnectView);
            }
        }

        void OpenConnectView()
        {
            ShowViewModel<ConnectViewModel>();
        }

        public IMvxCommand ShowDrawerViewCommand
        {
            get
            {
                return new MvxCommand(ShowDrawerView);
            }
        }

        void ShowDrawerView()
        {
            ShowViewModel<SideMenuViewModel>();
        }

        public void ShowLogin()
        {
            ShowViewModel<LoginViewModel>();
        }
    }
}
