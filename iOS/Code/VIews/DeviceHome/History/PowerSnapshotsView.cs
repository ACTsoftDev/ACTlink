using System;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using UIKit;

namespace actchargers.iOS
{
    public partial class PowerSnapshotsView : BackViewController
    {
        PowerSnapshotsViewModel currentViewModel;

        public PowerSnapshotsView() : base("PowerSnapshotsView", null)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.

            currentViewModel = ViewModel as PowerSnapshotsViewModel;

            listTableView.SeparatorColor = UIColor.Clear;

            listTableView.RegisterNibForCellReuse(PowerSnapshotsTableViewCell.Nib, PowerSnapshotsTableViewCell.Key);
            var source = new MvxSimpleTableViewSource(listTableView, "PowerSnapshotsTableViewCell");

            this.CreateBinding(IdTitle).To((PowerSnapshotsViewModel vm) => vm.IdTitle).Apply();
            this.CreateBinding(TimeTitle).To((PowerSnapshotsViewModel vm) => vm.TimeTitle).Apply();
            this.CreateBinding(VoltageTitle).To((PowerSnapshotsViewModel vm) => vm.VoltageTitle).Apply();
            this.CreateBinding(CurrentTitle).To((PowerSnapshotsViewModel vm) => vm.CurrentTitle).Apply();
            this.CreateBinding(PowerTitle).To((PowerSnapshotsViewModel vm) => vm.PowerTitle).Apply();
            this.CreateBinding(StartFromTitle).To((PowerSnapshotsViewModel vm) => vm.StartFromTitle).Apply();
            this.CreateBinding(StartFrom).To((PowerSnapshotsViewModel vm) => vm.StartId).Apply();
            this.CreateBinding(ReadRecordsButton).To((PowerSnapshotsViewModel vm) => vm.ReadRecordsTitle).Apply();
            this.CreateBinding(ReadRecordsButton).To((PowerSnapshotsViewModel vm) => vm.ReadRecordsCommand).Apply();

            this.CreateBinding(source).For(s => s.ItemsSource).To((PowerSnapshotsViewModel vm) => vm.ListItemSource).Apply();

            listTableView.Source = source;
            ListTableManager.SetHeight(listTableView, 60);
        }

        public override void BackButton_TouchUpInside(object sender, EventArgs e)
        {
            currentViewModel.OnBackButtonClick();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            UIDevice.CurrentDevice.SetValueForKey(new NSNumber((int)UIDeviceOrientation.LandscapeRight), new NSString("orientation"));
        }
    }
}

