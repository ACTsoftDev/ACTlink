using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using UIKit;

namespace actchargers.iOS
{
    public partial class PmLiveView : BackViewController
    {
        PmLiveViewModel pmLiveViewModel;

        public PmLiveView() : base("PmLiveView", null)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.

            pmLiveViewModel = ViewModel as PmLiveViewModel;

            listTableView.SeparatorColor = UIColor.Clear;

            listTableView.RegisterNibForCellReuse(PmLiveTableViewCell.Nib, PmLiveTableViewCell.Key);

            var source = new MvxSimpleTableViewSource(listTableView, "PmLiveTableViewCell");
            this.CreateBinding(source).For(s => s.ItemsSource).To((PmLiveViewModel vm) => vm.ListItemSource).Apply();

            listTableView.Source = source;
            ListTableManager.SetHeight(listTableView, 60);

            this.CreateBinding(IdTitle).To((PmLiveViewModel vm) => vm.IdTitle).Apply();
            this.CreateBinding(PmStateTitle).To((PmLiveViewModel vm) => vm.PmStateTitle).Apply();
            this.CreateBinding(CurrentTitle).To((PmLiveViewModel vm) => vm.CurrentTitle).Apply();
            this.CreateBinding(PmVoltageTitle).To((PmLiveViewModel vm) => vm.PmVoltageTitle).Apply();
            this.CreateBinding(RatingTitle).To((PmLiveViewModel vm) => vm.RatingTitle).Apply();
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            UIDevice
                .CurrentDevice
                .SetValueForKey
                (new NSNumber((int)UIDeviceOrientation.LandscapeLeft),
                 new NSString("orientation"));
        }

        public override void BackButton_TouchUpInside(object sender, System.EventArgs e)
        {
            base.BackButton_TouchUpInside(sender, e);

            pmLiveViewModel.OnBackButtonClick();
        }
    }
}

