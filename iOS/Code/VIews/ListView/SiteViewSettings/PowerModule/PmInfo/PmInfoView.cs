using Foundation;
using MvvmCross.Binding.BindingContext;
using UIKit;

namespace actchargers.iOS
{
    public partial class PmInfoView : ListViewBaseView
    {
        public PmInfoView() : base("PmInfoView", null)
        {
        }

        internal override void InitData()
        {
            listTableView.RegisterNibForCellReuse(LabelTextFieldTableViewCell.Nib, LabelTextFieldTableViewCell.Key);
            listTableView.RegisterNibForCellReuse(LabelLabelTableViewCell.Nib, LabelLabelTableViewCell.Key);
            listTableView.RegisterNibForCellReuse(ListSelectorTabelViewCell.Nib, ListSelectorTabelViewCell.Key);

            var source = new PlainItemTableViewSource(listTableView, null);
            this.CreateBinding(source).For(s => s.ListItemsSource).To((PmInfoViewModel vm) => vm.ItemSource).Apply();
            listTableView.Source = source;

            listTableView.SeparatorColor = UIColor.Clear;

            ListTableManager.SetHeight(listTableView, 60);

            listTableView.KeyboardDismissMode = UIScrollViewKeyboardDismissMode.OnDrag;
        }

        internal override void OnKeyBoardShow(NSNotification obj)
        {
        }

        internal override void OnKeyboardHide(NSNotification obj)
        {
        }
    }
}

