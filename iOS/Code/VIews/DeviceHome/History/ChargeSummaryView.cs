using System;
using MvvmCross.Binding.BindingContext;
using MvvmCross.iOS.Views;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;
using UIKit;

namespace actchargers.iOS
{
    public partial class ChargeSummaryView : BaseView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:actchargers.iOS.ChargeSummaryView"/> class.
        /// </summary>
        public ChargeSummaryView() : base("ChargeSummaryView", null)
        {
        }

        /// <summary>
        /// Views the did load.
        /// </summary>
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.

            listTableView.RegisterNibForCellReuse(TwoLabelTableViewCell.Nib, TwoLabelTableViewCell.Key);

            //ListTableViewSource tableviewsource
            var source = new ListTableViewSource(listTableView, null);
            this.CreateBinding(source).For(s => s.ListItemsSource).To((ChargeSummaryViewModel vm) => vm.ChargeSummaryItemSource).Apply();
            listTableView.SeparatorColor = UIColor.Clear;
            listTableView.Source = source;
            ListTableManager.SetHeight(listTableView, 100);

            titleViewLabel.Text = (ViewModel as ChargeSummaryViewModel).PagedViewId;

            nextButton.TouchUpInside += delegate
            {
                Mvx.Resolve<IMvxMessenger>().Publish(new EventsHistoryMessage(this, true));
            };

            previousButton.TouchUpInside += delegate
            {
                Mvx.Resolve<IMvxMessenger>().Publish(new EventsHistoryMessage(this, false));
            };
        }
    }
}