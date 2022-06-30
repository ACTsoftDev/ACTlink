using Foundation;
using MvvmCross.Binding.BindingContext;
using UIKit;

namespace actchargers.iOS
{
    public partial class WiFiView : ListViewBaseView
    {
        public WiFiView() : base("WiFiView", null)
        {
        }

        internal override void InitData()
        {
            listTableView.SeparatorColor = UIColor.Clear;

            listTableView.RegisterNibForCellReuse(LabelLabelTableViewCell.Nib, LabelLabelTableViewCell.Key);
            listTableView.RegisterNibForCellReuse(LabelSwitchTableViewCell.Nib, LabelSwitchTableViewCell.Key);
            listTableView.RegisterNibForCellReuse(LabelTextFieldTableViewCell.Nib, LabelTextFieldTableViewCell.Key);

            this.CreateBinding(restoreDefaultsBtn).For("Title").To((WiFiViewModel vm) => vm.RestoreToDefaultTitle).Apply();
            this.CreateBinding(restoreDefaultsBtn).For(arg => arg.Hidden).To((WiFiViewModel vm) => vm.IsRestoreDisable).Apply();
            this.CreateBinding(restoreDefaultsBtn).For(arg => arg.Enabled).To((WiFiViewModel vm) => vm.EditingMode).Apply();
            this.CreateBinding(restoreDefaultsBtn).For(arg => arg.Alpha).To((WiFiViewModel vm) => vm.EditingMode).WithConversion("BoolToAlpha").Apply();
            this.CreateBinding(restoreDefaultsBtn).To((WiFiViewModel vm) => vm.RestoreCommand).Apply();

            restoreDefaultsBtn.Enabled = false;

            var source = new PlainItemTableViewSource(listTableView, null);
            this.CreateBinding(source).For(s => s.ListItemsSource).To((WiFiViewModel vm) => vm.ItemSource).Apply();
            listTableView.Source = source;

            ListTableManager.SetHeight(listTableView, 60);

            listTableView.KeyboardDismissMode = UIScrollViewKeyboardDismissMode.OnDrag;
        }

        internal override void OnKeyBoardShow(NSNotification obj)
        {
            tableViewBottomConstraint.Constant = KEYBOARD_HEIGHT - btnView.Frame.Size.Height;
        }

        internal override void OnKeyboardHide(NSNotification obj)
        {
            tableViewBottomConstraint.Constant = 0;
        }
    }
}