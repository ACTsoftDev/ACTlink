using System;
using MvvmCross.Binding.BindingContext;
using UIKit;

namespace actchargers.iOS
{
    public partial class CommissionNextView : BackViewController
    {
        public CommissionNextView() : base("CommissionNextView", null)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.

            listTableView.RegisterNibForCellReuse(ButtonTableViewCell.Nib, ButtonTableViewCell.Key);
            listTableView.RegisterNibForCellReuse(ListSelectorTabelViewCell.Nib, ListSelectorTabelViewCell.Key);

            var source = new PlainItemTableViewSource(listTableView, null);
            this.CreateBinding(source).For(s => s.ListItemsSource).To((CommissionNextViewModel vm) => vm.CommissionNextItemSource).Apply();
            listTableView.Source = source;
            listTableView.SeparatorColor = UIColor.Clear;
            ListTableManager.SetHeight(listTableView, 60);
            listTableView.KeyboardDismissMode = UIScrollViewKeyboardDismissMode.OnDrag;
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        /// <summary>
        /// Backs the button touch up inside.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        public override void BackButton_TouchUpInside(object sender, EventArgs e)
        {
            base.BackButton_TouchUpInside(sender, e);
            this.NavigationController.PopViewController(true);
        }
    }
}

