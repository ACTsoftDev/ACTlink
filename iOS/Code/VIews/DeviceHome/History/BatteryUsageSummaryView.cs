using System;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Core.ViewModels;
using MvvmCross.iOS.Views;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;
using UIKit;

namespace actchargers.iOS
{
    public partial class BatteryUsageSummaryView : BaseView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:actchargers.iOS.BatteryUsageSummaryView"/> class.
        /// </summary>
        public BatteryUsageSummaryView() : base("BatteryUsageSummaryView", null)
        {
        }

        /// <summary>
        /// Views the did load.
        /// </summary>
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.

            groupTableView.RegisterNibForCellReuse(HeaderThreeLabelTableViewCell.Nib, HeaderThreeLabelTableViewCell.Key);
            groupTableView.RegisterNibForCellReuse(HeaderTwoLabelTableViewCell.Nib, HeaderTwoLabelTableViewCell.Key);
            var source = new GroupItemTableViewSource(groupTableView, null);
            this.CreateBinding(source).For(s => s.ListItemsSource).To((BatteryUsageSummaryViewModel vm) => vm.UsageSummaryItemSource).Apply();
            groupTableView.SeparatorColor = UIColor.Clear;
            groupTableView.Source = source;
            groupTableView.EstimatedRowHeight = 90;
            groupTableView.ContentInset = new UIEdgeInsets(-18, 0, 0, 0);

            titleViewLabel.Text = (ViewModel as BatteryUsageSummaryViewModel).PagedViewId;

            nextButton.TouchUpInside += delegate {
                Mvx.Resolve<IMvxMessenger>().Publish(new EventsHistoryMessage(this, true));
            };
        }


        /// <summary>
        /// Views the did layout subviews.
        /// </summary>
        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();
            groupTableView.LayoutIfNeeded();
        }
    }
}