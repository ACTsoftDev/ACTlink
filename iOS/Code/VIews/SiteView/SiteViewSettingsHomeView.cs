using MvvmCross.Binding.iOS.Views;
using UIKit;
using Foundation;
using MvvmCross.Binding.BindingContext;
using CoreGraphics;
using System;

namespace actchargers.iOS
{
    public partial class SiteViewSettingsHomeView :
    BackViewController, IUICollectionViewDelegateFlowLayout
    {
        SiteViewSettingsHomeViewModel currentViewModel;

        public SiteViewSettingsHomeView() : base("SiteViewSettingsHomeView", null)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.

            currentViewModel = ViewModel as SiteViewSettingsHomeViewModel;

            deviceCollectionView.RegisterNibForCell(UINib.FromName("DeviceCollectionViewCell", NSBundle.MainBundle), "DeviceCollectionViewCell");
            var source = new MvxCollectionViewSource(deviceCollectionView, new NSString("DeviceCollectionViewCell"));
            this.CreateBinding(source).For(s => s.ItemsSource).To((SiteViewSettingsHomeViewModel vm) => vm.ItemSource).Apply();
            deviceCollectionView.Source = source;

            deviceCollectionView.Delegate = this;

            this.CreateBinding(applySettingsButton).For("Title").To((SiteViewSettingsHomeViewModel vm) => vm.ApplySettingsTitle).Apply();
            this.CreateBinding(applySettingsButton).To((SiteViewSettingsHomeViewModel vm) => vm.ApplySettingsCommand).Apply();

            this.CreateBinding(cancelButton).For("Title").To((SiteViewSettingsHomeViewModel vm) => vm.CancelTitle).Apply();
            this.CreateBinding(cancelButton).To((SiteViewSettingsHomeViewModel vm) => vm.CancelSettingsCommand).Apply();
        }

        [Export("collectionView:layout:sizeForItemAtIndexPath:")]
        public virtual CGSize GetSizeForItem(UICollectionView collectionView, UICollectionViewLayout layout, NSIndexPath indexPath)
        {
            return new CGSize(90, 100);
        }

        [Export("collectionView:didSelectItemAtIndexPath:")]
        public void ItemSelected(UICollectionView collectionView, NSIndexPath indexPath)
        {
            currentViewModel.SelectGridItemCommand.Execute(currentViewModel.ItemSource[indexPath.Row]);
        }

        public override void BackButton_TouchUpInside(object sender, EventArgs e)
        {
            base.BackButton_TouchUpInside(sender, e);

            currentViewModel.OnBackButtonClick();
        }
    }
}