using System;
using MvvmCross.Binding.BindingContext;
using UIKit;

namespace actchargers.iOS
{
    public partial class TestingBattViewView : BackViewController
    {
        public TestingBattViewView() : base("TestingBattViewView", null)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.

            listTableView.RegisterNibForCellReuse(ImageLabelTableViewCell.Nib, ImageLabelTableViewCell.Key);

            var source = new PlainItemTableViewSource(listTableView, null);
            this.CreateBinding(source).For(s => s.ListItemsSource).To((TestingBattViewViewModel vm) => vm.ListItemSource).Apply();
            listTableView.Source = source;
            //listTableView.SeparatorColor = UIColor.Clear;
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
            this.NavigationController.PopViewController(true);
        }
    }
}

