using Foundation;
using MvvmCross.Binding.BindingContext;
using UIKit;

namespace actchargers.iOS
{
    public partial class FinishAndEQSettingsView : ListViewBaseView
    {
        public FinishAndEQSettingsView() : base("FinishAndEQSettingsView", null)
        {
        }

        internal override void InitData()
        {
            listTableView.RegisterNibForCellReuse(LabelLabelTableViewCell.Nib, LabelLabelTableViewCell.Key);
            listTableView.RegisterNibForCellReuse(ListSelectorTabelViewCell.Nib, ListSelectorTabelViewCell.Key);
            listTableView.RegisterNibForCellReuse(LabelTableViewCell.Nib, LabelTableViewCell.Key);
            listTableView.RegisterNibForCellReuse(DaysNewTableViewCell.Nib, DaysNewTableViewCell.Key);
            listTableView.RegisterNibForCellReuse(SectionHeaderTableViewCell.Nib, SectionHeaderTableViewCell.Key);

            var source = new PlainItemTableViewSource(listTableView, null);
            this.CreateBinding(source).For(s => s.ListItemsSource).To((FinishAndEQSettingsViewModel vm) => vm.ItemSource).Apply();
            listTableView.Source = source;

            ListTableManager.SetHeight(listTableView, 60);

            listTableView.KeyboardDismissMode = UIScrollViewKeyboardDismissMode.OnDrag;
        }

        internal override void OnKeyboardHide(NSNotification obj)
        {
        }

        internal override void OnKeyBoardShow(NSNotification obj)
        {
        }
    }
}