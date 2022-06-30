using System;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using UIKit;

namespace actchargers.iOS
{
    public partial class SiteViewInProgressDevicesView : BackViewController
    {
        SiteViewInProgressDevicesViewModel currentViewModel;

        public SiteViewInProgressDevicesView() : base("SiteViewInProgressDevicesView", null)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            currentViewModel = ViewModel as SiteViewInProgressDevicesViewModel;

            listTableView.SeparatorColor = UIColor.Clear;
            listTableView.RegisterNibForCellReuse(SiteViewInProgressDevicesTableViewCell.Nib, SiteViewInProgressDevicesTableViewCell.Key);
            var source = new MvxSimpleTableViewSource(listTableView, "SiteViewInProgressDevicesTableViewCell");
            this.CreateBinding(source).For(s => s.ItemsSource).To((SiteViewInProgressDevicesViewModel vm) => vm.ListItemSource).Apply();
            listTableView.Source = source;
            ListTableManager.SetHeight(listTableView, 80, 80);
        }

        public override void BackButton_TouchUpInside(object sender, EventArgs e)
        {
            base.BackButton_TouchUpInside(sender, e);

            currentViewModel.OnBackButtonClick();
        }
    }
}

