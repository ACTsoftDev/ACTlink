using System;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using UIKit;

namespace actchargers.iOS
{
    public partial class UploadView : BackViewController
    {
        UploadViewModel currentViewModel;

        public UploadView() : base("UploadView", null)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            segmentController.TintColor = UIColorUtility.FromHex(ACColors.DARK_BLUE_COLOR);
            progressIndicator.ProgressTintColor = UIColorUtility.FromHex(ACColors.WHITE_COLOR);
            progressIndicator.TrackTintColor = UIColorUtility.FromHex(ACColors.PROGRESS_TRACK_COLOR);


            // Perform any additional setup after loading the view, typically from a nib.
            currentViewModel = ViewModel as UploadViewModel;

            this.CreateBinding(progressIndicator).For(ob => ob.Progress).To((UploadViewModel vm) => vm.ProgressCompletedIOS).Apply();

            this.CreateBinding(segmentController).For(o => o.SelectedSegment).To((UploadViewModel vm) => vm.SelectedTabIndex).Apply();

            this.CreateBinding(statusText).For(o => o.Text).To((UploadViewModel vm) => vm.StatusLabel).Apply();


            listTableView.SeparatorColor = UIColor.Clear;

            listTableView.RegisterNibForCellReuse(UploadItemTableViewCell.Nib, UploadItemTableViewCell.Key);


            var source = new MvxSimpleTableViewSource(listTableView, "UploadItemTableViewCell");

            this.CreateBinding(source).For(s => s.ItemsSource).To((UploadViewModel vm) => vm.ListItemSource).Apply();


            listTableView.Source = source;
            ListTableManager.SetHeight(listTableView, 60);


        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        public override void BackButton_TouchUpInside(object sender, EventArgs e)
        {
            currentViewModel.OnBackButtonClick();
        }
    }
}

