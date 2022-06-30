using System;
using MvvmCross.Binding.iOS.Views;
using UIKit;
using Foundation;
using MvvmCross.iOS.Views;
using MvvmCross.Binding.BindingContext;
using CoreGraphics;

namespace actchargers.iOS
{
	public partial class DeviceHomeView : BackViewController, IUICollectionViewDelegateFlowLayout
	{
		MvxUIRefreshControl refreshControl;
		DeviceHomeViewModel currentViewModel;
		public DeviceHomeView() : base("DeviceHomeView", null)
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			// Perform any additional setup after loading the view, typically from a nib.

			currentViewModel = ViewModel as DeviceHomeViewModel;

			UIButton menuButton = new UIButton(new CGRect(0, 0, 19, 17));
			menuButton.SetImage(UIImage.FromBundle("dots.png"), UIControlState.Normal);
			menuButton.ShowsTouchWhenHighlighted = true;
			menuButton.TouchUpInside += delegate
			{
				(UIApplication.SharedApplication.Delegate as AppDelegate).SideBar.ToggleMenu();
			};
			var menuItem = new UIBarButtonItem(menuButton);
			NavigationItem.RightBarButtonItem = menuItem;

            //refresh view 
            refreshControl = new MvxUIRefreshControl();

			//refreshControl.AddTarget(delegate
			//{
			//	refreshControl.EndRefreshing();
			//}, UIControlEvent.ValueChanged);
			deviceCollectionView.AddSubview(refreshControl);
            this.CreateBinding(refreshControl).For(o => o.RefreshCommand).To(((DeviceHomeViewModel vm) => vm.RefreshCommand)).Apply();
            this.CreateBinding(refreshControl).For(o => o.IsRefreshing).To(((DeviceHomeViewModel vm) => vm.IsRefreshing)).Apply();

			deviceCollectionView.RegisterNibForCell(UINib.FromName("DeviceCollectionViewCell", NSBundle.MainBundle), "DeviceCollectionViewCell");
			var source = new MvxCollectionViewSource(deviceCollectionView, new NSString("DeviceCollectionViewCell"));
			this.CreateBinding(source).For(s => s.ItemsSource).To((DeviceHomeViewModel vm) => vm.DeviceDetailsItemSource).Apply();
			deviceCollectionView.Source = source;

			//IUICollectionViewDelegateFlowLayout delegate
			deviceCollectionView.Delegate = this;
		}

		/// <summary>
		/// Gets the size for item.
		/// </summary>
		/// <returns>The size for item.</returns>
		/// <param name="collectionView">Collection view.</param>
		/// <param name="layout">Layout.</param>
		/// <param name="indexPath">Index path.</param>
		[Export("collectionView:layout:sizeForItemAtIndexPath:")]
		public virtual CoreGraphics.CGSize GetSizeForItem(UICollectionView collectionView, UICollectionViewLayout layout, NSIndexPath indexPath)
		{
			return new CGSize(90, 100);
		}

		
		/// <summary>
		/// Items the selected.
		/// </summary>
		/// <param name="collectionView">Collection view.</param>
		/// <param name="indexPath">Index path.</param>
		[Export("collectionView:didSelectItemAtIndexPath:")]
		public void ItemSelected(UICollectionView collectionView, NSIndexPath indexPath)
		{
			currentViewModel.SelectGridItemCommand.Execute(currentViewModel.DeviceDetailsItemSource[indexPath.Row]);
		}

		/// <summary>
		/// Backs the button touch up inside.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
		public override void BackButton_TouchUpInside(object sender, EventArgs e)
		{
			base.BackButton_TouchUpInside(sender, e);
			currentViewModel.OnBackButtonClick();
		}
	}
}