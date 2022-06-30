using Foundation;
using MvvmCross.Binding.BindingContext;
using UIKit;

namespace actchargers.iOS
{
    public partial class DefaultChargeProfileView : ListViewBaseView
    {
        public DefaultChargeProfileView() : base("DefaultChargeProfileView", null)
        {
        }

        internal override void InitData()
        {
            listTableView.RegisterNibForCellReuse(LabelTextFieldTableViewCell.Nib, LabelTextFieldTableViewCell.Key);
            listTableView.RegisterNibForCellReuse(LabelSwitchTableViewCell.Nib, LabelSwitchTableViewCell.Key);
            listTableView.RegisterNibForCellReuse(LabelLabelTableViewCell.Nib, LabelLabelTableViewCell.Key);
            listTableView.RegisterNibForCellReuse(ListSelectorTabelViewCell.Nib, ListSelectorTabelViewCell.Key);
            listTableView.RegisterNibForCellReuse(DatePickerTableViewCell.Nib, DatePickerTableViewCell.Key);
            listTableView.RegisterNibForCellReuse(DatePickerSwitchTableViewCell.Nib, DatePickerSwitchTableViewCell.Key);

            this.CreateBinding(loadDefaultsButton).For("Title").To((DefaultChargeProfileViewModel vm) => vm.RestoreToDefaultTitle).Apply();
            this.CreateBinding(loadDefaultsButton).For(arg => arg.Hidden).To((DefaultChargeProfileViewModel vm) => vm.IsRestoreDisable).Apply();
            this.CreateBinding(loadDefaultsButton).For(arg => arg.Enabled).To((DefaultChargeProfileViewModel vm) => vm.EditingMode).Apply();
            this.CreateBinding(loadDefaultsButton).For(arg => arg.Alpha).To((DefaultChargeProfileViewModel vm) => vm.EditingMode).WithConversion("BoolToAlpha").Apply();
            this.CreateBinding(loadDefaultsButton).To((DefaultChargeProfileViewModel vm) => vm.RestoreCommand).Apply();

            loadDefaultsButton.Enabled = false;

            var source = new PlainItemTableViewSource(listTableView, null);
            this.CreateBinding(source).For(s => s.ListItemsSource).To((DefaultChargeProfileViewModel vm) => vm.ItemSource).Apply();
            listTableView.Source = source;

            ListTableManager.SetHeight(listTableView, 60);

            listTableView.KeyboardDismissMode = UIScrollViewKeyboardDismissMode.OnDrag;
        }

        internal override void OnKeyBoardShow(NSNotification obj)
        {
            listTableViewBottomConstraint.Constant = KEYBOARD_HEIGHT - btnView.Frame.Size.Height;
        }

        internal override void OnKeyboardHide(NSNotification obj)
        {
            listTableViewBottomConstraint.Constant = 0;
        }
    }
}