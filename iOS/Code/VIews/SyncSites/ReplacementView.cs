using System;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using UIKit;

namespace actchargers.iOS
{
    public partial class ReplacementView : BackViewController
    {
        ReplacementViewModel currentViewModel;

        public ReplacementView() : base("ReplacementView", null)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            currentViewModel = ViewModel as ReplacementViewModel;

            listTableView.SeparatorColor = UIColor.Clear;

            listTableView.RegisterNibForCellReuse(ReplacementDeviceItemTableViewCell.Nib, ReplacementDeviceItemTableViewCell.Key);
            var source = new MvxSimpleTableViewSource(listTableView, "ReplacementDeviceItemTableViewCell");
            this.CreateBinding(source).For(s => s.ItemsSource).To((ReplacementViewModel vm) => vm.ListItemSource).Apply();
            this.CreateBinding(source).For(o => o.SelectionChangedCommand).To((ReplacementViewModel vm) => vm.SelectItemCommand).Apply();
            this.CreateBinding(searchBar).For(o => o.Text).To((ReplacementViewModel vm) => vm.SearchText).Apply();
            this.CreateBinding(updateFirmwareButton).For("Title").To((ReplacementViewModel vm) => vm.UpdateFirmwareTitle).Apply();
            this.CreateBinding(updateFirmwareButton).To((ReplacementViewModel vm) => vm.UpdateFirmwareCommand).Apply();

            listTableView.Source = source;

            ListTableManager.SetHeight(listTableView, 60);
        }

        public override void BackButton_TouchUpInside(object sender, EventArgs e)
        {
            currentViewModel.OnBackButtonClick();
        }
    }
}

