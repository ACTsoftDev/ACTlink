using System;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;
using UIKit;

namespace actchargers.iOS
{
    public partial class BatteryDailyUsageView : BaseView
    {
        public BatteryDailyUsageView() : base("BatteryDailyUsageView", null)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.

            titleViewLabel.Text = (ViewModel as BatteryDailyUsageViewModel).PagedViewId;

            listTableView.SeparatorColor = UIColor.Clear;

            listTableView.RegisterNibForCellReuse(BatteryDailyUsageTableCell.Nib, BatteryDailyUsageTableCell.Key);
            var source = new MvxSimpleTableViewSource(listTableView, "BatteryDailyUsageTableCell");

            this.CreateBinding(source).For(s => s.ItemsSource).To((BatteryDailyUsageViewModel vm) => vm.DailyUsageItemSource).Apply();

            listTableView.Source = source;
            ListTableManager.SetHeight(listTableView, 60);

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