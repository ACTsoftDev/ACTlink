using System;
using MvvmCross.Binding.BindingContext;
using UIKit;

namespace actchargers.iOS
{
    public partial class CumulativeDataView : BackViewController
    {
        public CumulativeDataView() : base("CumulativeDataView", null)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.

            if ((ViewModel as CumulativeDataViewModel).IsResetButtonVisible)
            {
                UIBarButtonItem editButton = new UIBarButtonItem((ViewModel as CumulativeDataViewModel).ResetButtonTitle, UIBarButtonItemStyle.Plain, delegate
                {
                    (ViewModel as CumulativeDataViewModel).ResetBtnClickCommand.Execute();
                });
                NavigationItem.RightBarButtonItem = editButton;
            }

            listTableView.RegisterNibForCellReuse(LabelLabelTableViewCell.Nib, LabelLabelTableViewCell.Key);
            listTableView.RegisterNibForCellReuse(TwoLabelTableViewCell.Nib, TwoLabelTableViewCell.Key);
            listTableView.RegisterNibForCellReuse(ThreeLabelTableViewCell.Nib, ThreeLabelTableViewCell.Key);

            this.CreateBinding(resetButton).For("Title").To((CumulativeDataViewModel vm) => vm.ResetButtonTitle).Apply();

            var source = new PlainItemTableViewSource(listTableView, null);
            this.CreateBinding(source).For(s => s.ListItemsSource).To((CumulativeDataViewModel vm) => vm.CumulativeDataViewItemSource).Apply();
            listTableView.SeparatorColor = UIColor.Clear;
            listTableView.Source = source;
            ListTableManager.SetHeight(listTableView, 100);
        }

        public override void BackButton_TouchUpInside(object sender, EventArgs e)
        {
            base.BackButton_TouchUpInside(sender, e);

            this.NavigationController.PopViewController(true);
        }
    }
}

