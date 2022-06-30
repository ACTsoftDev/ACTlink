using System;
using MvvmCross.Binding.BindingContext;
using UIKit;

namespace actchargers.iOS
{
    public partial class HistoryView : BackViewController
    {
        public HistoryView() : base("HistoryView", null)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.

            var source = new PlainItemTableViewSource(listTableView, null);
            this.CreateBinding(source).For(s => s.ListItemsSource).To((HistoryViewModel vm) => vm.HistoryViewItemSource).Apply();
            this.CreateBinding(source).For(s => s.SelectionChangedCommand).To((HistoryViewModel vm) => vm.SelectItemCommand).Apply();
            listTableView.Source = source;
            ListTableManager.SetHeight(listTableView, 40);

            UIView footerView = new UIView(new CoreGraphics.CGRect(15, 0, listTableView.Frame.Size.Width, 1))
            {
                BackgroundColor = listTableView.SeparatorColor
            };

            listTableView.TableFooterView = footerView;
        }

        public override void BackButton_TouchUpInside(object sender, EventArgs e)
        {
            base.BackButton_TouchUpInside(sender, e);

            (ViewModel as HistoryViewModel).OnBackButtonClick();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            UIDevice.CurrentDevice.SetValueForKey(new Foundation.NSNumber((int)UIDeviceOrientation.Portrait), new Foundation.NSString("orientation"));
        }
    }
}