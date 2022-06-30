using System;
using CoreGraphics;
using MvvmCross.Binding.BindingContext;
using MvvmCross.iOS.Views;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;
using UIKit;

namespace actchargers.iOS
{
    public partial class HistoryChartsView : MvxViewController
    {
        HistoryChartsViewModel historyChartsViewModel;
        public HistoryChartsView() : base("HistoryChartsView", null)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.
            listTableView.RegisterNibForCellReuse(PlotTableViewCell.Nib, PlotTableViewCell.Key);

            historyChartsViewModel = ViewModel as HistoryChartsViewModel;
            var source = new PlotViewListItemSource(listTableView, null);
            this.CreateBinding(source).For(s => s.ListItemsSource).To((HistoryChartsViewModel vm) => vm.ChartsViewItemSource).Apply();
            listTableView.Source = source;

            //UIView footerView = new UIView(new CGRect(0, 0, listTableView.Frame.Size.Width, 1));
            //listTableView.TableFooterView = footerView;
            //listTableView.SeparatorColor = UIColor.Clear;

            ListTableManager.SetHeight(listTableView, 60);

            titleViewLabel.Text = (ViewModel as HistoryChartsViewModel).PagedViewId;

            nextButton.TouchUpInside += delegate
            {
                Mvx.Resolve<IMvxMessenger>().Publish(new EventsHistoryMessage(this, true));
            };

            previousButton.TouchUpInside += delegate
            {
                Mvx.Resolve<IMvxMessenger>().Publish(new EventsHistoryMessage(this, false));
            };
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }
    }
}

