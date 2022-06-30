using System;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using UIKit;

namespace actchargers.iOS
{
    public partial class SiteViewDevicesView : BackViewController
    {
        SiteViewDevicesViewModel currentViewModel;

        MvxImageViewLoader checkImageloader;

        public SiteViewDevicesView() : base("SiteViewDevicesView", null)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            currentViewModel = ViewModel as SiteViewDevicesViewModel;

            checkImageloader = new MvxImageViewLoader(() => checkImage);

            UIButton menuButton = new UIButton(new CoreGraphics.CGRect(0, 0, 19, 17));
            menuButton.SetImage(UIImage.FromBundle("dots.png"), UIControlState.Normal);
            menuButton.ShowsTouchWhenHighlighted = true;
            menuButton.TouchUpInside += delegate
            {
                (UIApplication.SharedApplication.Delegate as AppDelegate).SideBar.ToggleMenu();
            };
            var menuItem = new UIBarButtonItem(menuButton);
            NavigationItem.RightBarButtonItem = menuItem;

            segmentController.TintColor = UIColorUtility.FromHex(ACColors.DARK_BLUE_COLOR);
            segmentController.SetTitle(AppResources.chargers, 0);
            segmentController.SetTitle(AppResources.battview, 1);
            segmentController.SetTitle(AppResources.battview_mobile, 2);

            this.CreateBinding(selectAllContainer).For(o => o.Hidden).To((SiteViewDevicesViewModel vm) => vm.SelectAllVisibilityIos).Apply();
            this.CreateBinding(selectAllTitle).To((SiteViewDevicesViewModel vm) => vm.SelectAllTitle).Apply();
            this.CreateBinding(checkImageloader).To((SiteViewDevicesViewModel vm) => vm.SelectAllChecked).WithConversion("BoolToImage", 1).Apply();
            this.CreateBinding(checkBtn).To((SiteViewDevicesViewModel vm) => vm.SelectAllCommand).Apply();

            listTableView.SeparatorColor = UIColor.Clear;
            listTableView.RegisterNibForCellReuse(SiteViewDevicesTableViewCell.Nib, SiteViewDevicesTableViewCell.Key);
            var source = new MvxSimpleTableViewSource(listTableView, "SiteViewDevicesTableViewCell");
            this.CreateBinding(source).For(s => s.ItemsSource).To((SiteViewDevicesViewModel vm) => vm.ListItemSource).Apply();
            listTableView.Source = source;
            ListTableManager.SetHeight(listTableView, 80, 80);

            this.CreateBinding(segmentController).For(o => o.SelectedSegment).To((SiteViewDevicesViewModel vm) => vm.SelectedTabIndex).Apply();
        }

        public override void BackButton_TouchUpInside(object sender, EventArgs e)
        {
            base.BackButton_TouchUpInside(sender, e);

            currentViewModel.OnBackButtonClick();
        }
    }
}

