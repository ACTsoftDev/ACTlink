using System;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using UIKit;

namespace actchargers.iOS
{
    public partial class PMFaultsView : BackViewController
    {
        PMFaultsViewModel pmFaultsViewModel;
        public PMFaultsView() : base("PMFaultsView", null)
        {
        }

        public override void ViewDidLoad()
        {

            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.

            pmFaultsViewModel = ViewModel as PMFaultsViewModel;

            listTableView.SeparatorColor = UIColor.Clear;

            listTableView.RegisterNibForCellReuse(PMFaultsTableViewCell.Nib, PMFaultsTableViewCell.Key);
            var source = new MvxSimpleTableViewSource(listTableView, "PMFaultsTableViewCell");


            this.CreateBinding(source).For(s => s.ItemsSource).To((PMFaultsViewModel vm) => vm.ListItemSource).Apply();


            listTableView.Source = source;
            ListTableManager.SetHeight(listTableView, 60);
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

        public override void BackButton_TouchUpInside(object sender, EventArgs e)
        {
            base.BackButton_TouchUpInside(sender, e);

            pmFaultsViewModel.OnBackButtonClick();
        }
    }
}

