using Foundation;
using MvvmCross.Binding.BindingContext;
using UIKit;

namespace actchargers.iOS
{
    public partial class BatteryInfoView : ListViewBaseView
    {
        public BatteryInfoView() : base("BatteryInfoView", null)
        {
        }

        internal override void InitData()
        {
            NavigationItem.RightBarButtonItem = null;

            listTableView.RegisterNibForCellReuse(ButtonTableViewCell.Nib, ButtonTableViewCell.Key);

            var source = new PlainItemTableViewSource(listTableView, null);
            this.CreateBinding(source).For(s => s.ListItemsSource).To((BatteryInfoViewModel vm) => vm.ItemSource).Apply();
            this.CreateBinding(source).For(s => s.SelectionChangedCommand).To((BatteryInfoViewModel vm) => vm.ItemClickCommand).Apply();
            listTableView.Source = source;

            ListTableManager.SetHeight(listTableView, 60);

            UIView footerView = new UIView(new CoreGraphics.CGRect(15, 0, listTableView.Frame.Size.Width, 1))
            {
                BackgroundColor = listTableView.SeparatorColor
            };

            listTableView.TableFooterView = footerView;

        }

        internal override void OnKeyboardHide(NSNotification obj)
        {
        }

        internal override void OnKeyBoardShow(NSNotification obj)
        {
        }
    }
}