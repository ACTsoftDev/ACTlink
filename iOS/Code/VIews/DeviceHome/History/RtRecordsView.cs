using System;
using CoreGraphics;
using MvvmCross.Binding.BindingContext;
using UIKit;

namespace actchargers.iOS
{
    public partial class RtRecordsView : BackViewController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:actchargers.iOS.RtRecordsView"/> class.
        /// </summary>
        public RtRecordsView() : base("RtRecordsView", null)
        {
        }

        /// <summary>
        /// Views the did load.
        /// </summary>
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.

            listTableView.RegisterNibForCellReuse(PlotTableViewCell.Nib, PlotTableViewCell.Key);

            listTableView.RegisterNibForCellReuse(ButtonTableViewCell.Nib, ButtonTableViewCell.Key);
            listTableView.RegisterNibForCellReuse(ListSelectorTabelViewCell.Nib, ListSelectorTabelViewCell.Key);
            listTableView.RegisterNibForCellReuse(LabelTextFieldTableViewCell.Nib, LabelTextFieldTableViewCell.Key);

            listTableView.RegisterNibForCellReuse(RTRecordsPlotsTableViewCell.Nib, RTRecordsPlotsTableViewCell.Key);

            var source = new GroupItemTableViewSource(listTableView, null);
            this.CreateBinding(source).For(s => s.ListItemsSource).To((RtRecordsViewModel vm) => vm.RTRecordsViewItemSource).Apply();
            listTableView.Source = source;

            UIView footerView = new UIView(new CGRect(0, 0, listTableView.Frame.Size.Width, 1));
            listTableView.TableFooterView = footerView;
            listTableView.SeparatorColor = UIColor.Gray;

            ListTableManager.SetHeight(listTableView, 60);
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

