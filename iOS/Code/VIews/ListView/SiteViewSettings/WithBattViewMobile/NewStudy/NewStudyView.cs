using Foundation;
using MvvmCross.Binding.BindingContext;
using UIKit;

namespace actchargers.iOS
{
    public partial class NewStudyView : ListViewBaseView
    {
        public NewStudyView() : base("NewStudyView", null)
        {
        }

        internal override void InitData()
        {
            NavigationItem.RightBarButtonItem = null;

            listTableView.RegisterNibForCellReuse(LabelTextFieldTableViewCell.Nib, LabelTextFieldTableViewCell.Key);
            listTableView.RegisterNibForCellReuse(ButtonTableViewCell.Nib, ButtonTableViewCell.Key);

            var source = new PlainItemTableViewSource(listTableView, null);
            this.CreateBinding(source).For(s => s.ListItemsSource).To((NewStudyViewModel vm) => vm.ItemSource).Apply();
            listTableView.Source = source;
            ListTableManager.SetHeight(listTableView, 60);
            listTableView.SeparatorColor = UIColor.Clear;
        }

        internal override void OnKeyboardHide(NSNotification obj)
        {
        }

        internal override void OnKeyBoardShow(NSNotification obj)
        {
        }
    }
}