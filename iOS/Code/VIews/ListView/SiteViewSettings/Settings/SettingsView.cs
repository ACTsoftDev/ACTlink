using Foundation;
using MvvmCross.Binding.BindingContext;
using UIKit;

namespace actchargers.iOS
{
    public partial class SettingsView : ListViewBaseView
    {
        SettingsViewModel mySettingsViewModel;

        public SettingsView() : base("SettingsView", null)
        {
        }

        internal override void InitData()
        {
            mySettingsViewModel = ViewModel as SettingsViewModel;

            listTableView.RegisterNibForCellReuse(LabelTextFieldTableViewCell.Nib, LabelTextFieldTableViewCell.Key);
            listTableView.RegisterNibForCellReuse(LabelSwitchTableViewCell.Nib, LabelSwitchTableViewCell.Key);
            listTableView.RegisterNibForCellReuse(LabelLabelTableViewCell.Nib, LabelLabelTableViewCell.Key);
            listTableView.RegisterNibForCellReuse(ListSelectorTabelViewCell.Nib, ListSelectorTabelViewCell.Key);
            listTableView.RegisterNibForCellReuse(ButtonTableViewCell.Nib, ButtonTableViewCell.Key);

            var source = new PlainItemTableViewSource(listTableView, null);
            this.CreateBinding(source).For(s => s.ListItemsSource).To((SettingsViewModel vm) => vm.ItemSource).Apply();
            listTableView.Source = source;
            listTableView.SeparatorColor = UIColor.Clear;

            ListTableManager.SetHeight(listTableView, 60);

            listTableView.KeyboardDismissMode = UIScrollViewKeyboardDismissMode.OnDrag;

            this.CreateBinding(resetLCDCalibrationButton).For("Title").To((SettingsViewModel vm) => vm.ResetLcdCalibrationTitle).Apply();
            this.CreateBinding(resetLCDCalibrationButton).For(arg => arg.Hidden).To((SettingsViewModel vm) => vm.IsResetLcdCalibrationHidden).Apply();
            this.CreateBinding(resetLCDCalibrationButton).For(arg => arg.Enabled).To((SettingsViewModel vm) => vm.IsResetLcdCalibrationEditEnabled).Apply();
            this.CreateBinding(resetLCDCalibrationButton).For(arg => arg.Alpha).To((SettingsViewModel vm) => vm.IsResetLcdCalibrationEditEnabled).WithConversion("BoolToAlpha").Apply();
            this.CreateBinding(resetLCDCalibrationButton).To((SettingsViewModel vm) => vm.McbResetLcdCalibrationCommand).Apply();
        }

        internal override void OnKeyBoardShow(NSNotification obj)
        {
            tableViewBottomConstraint
                .Constant =
                    mySettingsViewModel.IsResetLcdCalibrationVisible ?
                    KEYBOARD_HEIGHT - resetLCDView.Frame.Size.Height : KEYBOARD_HEIGHT;
        }

        internal override void OnKeyboardHide(NSNotification obj)
        {
            tableViewBottomConstraint.Constant = 0;
        }
    }
}

