using Foundation;
using MvvmCross.Binding.BindingContext;
using UIKit;

namespace actchargers.iOS
{
    public partial class BatterySettingsView : ListViewBaseView
    {
        public BatterySettingsView() : base("BatterySettingsView", null)
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

            var source = new PlainItemTableViewSource(listTableView, null);
            this.CreateBinding(source).For(s => s.ListItemsSource).To((BatterySettingsViewModel vm) => vm.ItemSource).Apply();
            listTableView.Source = source;

            ListTableManager.SetHeight(listTableView, 60);

            listTableView.KeyboardDismissMode = UIScrollViewKeyboardDismissMode.OnDrag;
        }

        internal override void OnKeyBoardShow(NSNotification obj)
        {
            tableViewBottomConstraint.Constant = 0;
        }

        internal override void OnKeyboardHide(NSNotification obj)
        {
            tableViewBottomConstraint.Constant = KEYBOARD_HEIGHT;
        }
    }
}

