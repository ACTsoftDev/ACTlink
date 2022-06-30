using System;
using MvvmCross.Binding.BindingContext;
using UIKit;

namespace actchargers.iOS
{
    public partial class FirmwareUpdateView : BackViewController    
    {
        public FirmwareUpdateView() : base("FirmwareUpdateView", null)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib

            listTableView.RegisterNibForCellReuse(ButtonTableViewCell.Nib, ButtonTableViewCell.Key);

            var source = new PlainItemTableViewSource(listTableView, null);
            this.CreateBinding(source).For(s => s.ListItemsSource).To((FirmwareUpdateViewModel vm) => vm.ItemSource).Apply();
            listTableView.Source = source;
            listTableView.SeparatorColor = UIColor.Clear;
            ListTableManager.SetHeight(listTableView, 60);
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        public override void BackButton_TouchUpInside(object sender, EventArgs e)
        {
            base.BackButton_TouchUpInside(sender, e);
            (ViewModel as FirmwareUpdateViewModel).OnBackButtonClick();
        }
    }
}

