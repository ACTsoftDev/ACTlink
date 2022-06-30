using MvvmCross.Binding.BindingContext;
using UIKit;

namespace actchargers.iOS
{
    public partial class AboutUsView : BaseView
    {
        AboutUsViewModel currentViewModel;

        public AboutUsView() : base("AboutUsView", null)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.

            currentViewModel = ViewModel as AboutUsViewModel;

            UIBarButtonItem cancelBtn = new UIBarButtonItem(UIBarButtonSystemItem.Cancel, delegate
            {
                currentViewModel.OnBackButtonClick();
            });
            NavigationItem.LeftBarButtonItem = cancelBtn;

            this.CreateBinding(appVersion).To((AboutUsViewModel vm) => vm.AppVersionWithTitle).Apply();
            this.CreateBinding(mcbVersion).To((AboutUsViewModel vm) => vm.McbVersionWithTitle).Apply();
            this.CreateBinding(battViewVersion).To((AboutUsViewModel vm) => vm.BattViewVersionWithTitle).Apply();
            this.CreateBinding(calibratorVersion).To((AboutUsViewModel vm) => vm.CalibratorVersionWithTitle).Apply();
            this.CreateBinding(copyright).To((AboutUsViewModel vm) => vm.CopyrightVersionWithTitle).Apply();
            this.CreateBinding(aboutAct).To((AboutUsViewModel vm) => vm.AboutAct).Apply();
            this.CreateBinding(poweredBy).To((AboutUsViewModel vm) => vm.PoweredTitle).Apply();
        }
    }
}

