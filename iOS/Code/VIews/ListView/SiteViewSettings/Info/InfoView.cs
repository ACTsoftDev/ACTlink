using Foundation;
using MvvmCross.Binding.BindingContext;
using UIKit;

namespace actchargers.iOS
{
    public partial class InfoView : ListViewBaseView
    {
        public InfoView() : base("InfoView", null)
        {
        }

        internal override void InitData()
        {
            listTableView.RegisterNibForCellReuse(LabelLabelTableViewCell.Nib, LabelLabelTableViewCell.Key);
            listTableView.RegisterNibForCellReuse(LabelTextFieldTableViewCell.Nib, LabelTextFieldTableViewCell.Key);
            listTableView.RegisterNibForCellReuse(DatePickerTableViewCell.Nib, DatePickerTableViewCell.Key);
            listTableView.RegisterNibForCellReuse(ListSelectorTabelViewCell.Nib, ListSelectorTabelViewCell.Key);

            var source = new PlainItemTableViewSource(listTableView, null);
            this.CreateBinding(source).For(s => s.ListItemsSource).To((InfoViewModel vm) => vm.ItemSource).Apply();
            listTableView.Source = source;
            listTableView.SeparatorColor = UIColor.Clear;

            ListTableManager.SetHeight(listTableView, 60);

            listTableView.KeyboardDismissMode = UIScrollViewKeyboardDismissMode.OnDrag;
        }

        internal override void OnKeyBoardShow(NSNotification obj)
        {
            tableViewBottomConstraint.Constant = KEYBOARD_HEIGHT;
        }

        internal override void OnKeyboardHide(NSNotification obj)
        {
            tableViewBottomConstraint.Constant = 0;
        }
    }
}

