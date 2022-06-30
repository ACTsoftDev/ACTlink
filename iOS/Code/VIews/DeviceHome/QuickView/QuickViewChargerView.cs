using System;
using MvvmCross.Binding.BindingContext;
using UIKit;

namespace actchargers.iOS
{
    public partial class QuickViewChargerView : BackViewController
    {
        QuickViewChargerViewModel currentViewModel;
        public QuickViewChargerView() : base("QuickViewChargerView", null)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.

            currentViewModel = ViewModel as QuickViewChargerViewModel;

            listTableView.RegisterNibForCellReuse(LabelLabelTableViewCell.Nib, LabelLabelTableViewCell.Key);
            listTableView.RegisterNibForCellReuse(LabelTextFieldTableViewCell.Nib, LabelTextFieldTableViewCell.Key);
            listTableView.RegisterNibForCellReuse(ListSelectorTabelViewCell.Nib, ListSelectorTabelViewCell.Key);
            listTableView.RegisterNibForCellReuse(LabelSwitchTableViewCell.Nib, LabelSwitchTableViewCell.Key);
            listTableView.RegisterNibForCellReuse(QuickViewThreeLabelTableViewCell.Nib, QuickViewThreeLabelTableViewCell.Key);
            listTableView.RegisterNibForCellReuse(LabelTableViewCell.Nib, LabelTableViewCell.Key);
            listTableView.RegisterNibForCellReuse(ButtonTableViewCell.Nib, ButtonTableViewCell.Key);
            listTableView.RegisterNibForCellReuse(QuickViewTableViewCell.Nib, QuickViewTableViewCell.Key);
            listTableView.RegisterNibForCellReuse(ImageTableViewCell.Nib, ImageTableViewCell.Key);

            var source = new GroupItemTableViewSource(listTableView, null);
            this.CreateBinding(source).For(s => s.ListItemsSource).To((QuickViewChargerViewModel vm) => vm.ListItemSource).Apply();
            listTableView.Source = source;

            ListTableManager.SetHeight(listTableView, 60);
            listTableView.SeparatorColor = UIColor.Clear;
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
            currentViewModel.OnBackButtonClick();
        }
    }
}

