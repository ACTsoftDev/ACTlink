using Foundation;
using MvvmCross.Binding.BindingContext;
using UIKit;

namespace actchargers.iOS
{
    public partial class PowerModuleView : ListViewBaseView
    {
        public PowerModuleView() : base("PowerModuleView", null)
        {
        }

        internal override void InitData()
        {
            listTableView.RegisterNibForCellReuse(LabelLabelTableViewCell.Nib, LabelLabelTableViewCell.Key);

            var source = new PlainItemTableViewSource(listTableView, null);
            this.CreateBinding(source).For(s => s.ListItemsSource).To((PowerModuleViewModel vm) => vm.ItemSource).Apply();
            this.CreateBinding(source).For(s => s.SelectionChangedCommand).To((PowerModuleViewModel vm) => vm.ItemClickCommand).Apply();
            listTableView.Source = source;

            ListTableManager.SetHeight(listTableView, 60);

            UIView footerView = new UIView(new CoreGraphics.CGRect(15, 0, listTableView.Frame.Size.Width, 1))
            {
                BackgroundColor = listTableView.SeparatorColor
            };

            listTableView.TableFooterView = footerView;

            listTableView.KeyboardDismissMode = UIScrollViewKeyboardDismissMode.OnDrag;
        }

        internal override void OnKeyBoardShow(NSNotification obj)
        {
        }

        internal override void OnKeyboardHide(NSNotification obj)
        {
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            UIDevice.CurrentDevice.SetValueForKey(new NSNumber((int)UIDeviceOrientation.Portrait), new NSString("orientation"));
        }
    }
}

