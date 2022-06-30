using System;
using CoreGraphics;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using UIKit;

namespace actchargers.iOS
{
    public partial class ViewGlobalRecordsView : BackViewController, IUICollectionViewDelegateFlowLayout
    {
        public ViewGlobalRecordsView() : base("ViewGlobalRecordsView", null)
        {
        }

        /// <summary>
        /// Views the did load.
        /// </summary>
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.

            if((ViewModel as ViewGlobalRecordsViewModel).IsResetButtonVisible)
            {
                UIBarButtonItem editButton = new UIBarButtonItem((ViewModel as ViewGlobalRecordsViewModel).ResetButtonTitle, UIBarButtonItemStyle.Plain, delegate {
                    (ViewModel as ViewGlobalRecordsViewModel).ResetBtnClickCommand.Execute();
                });
                NavigationItem.RightBarButtonItem = editButton;
            }

            deviceCollectionView.RegisterNibForCell(UINib.FromName("GlobalRecordsCollectionViewCell", NSBundle.MainBundle), "GlobalRecordsCollectionViewCell");
            var source = new MvxCollectionViewSource(deviceCollectionView, new NSString("GlobalRecordsCollectionViewCell"));
            this.CreateBinding(source).For(s => s.ItemsSource).To((ViewGlobalRecordsViewModel vm) => vm.ViewGlobalRecordsItemSource).Apply();
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
        public virtual CGSize GetSizeForItem(UICollectionView collectionView, UICollectionViewLayout layout, NSIndexPath indexPath)
        {
            return new CGSize((this.View.Frame.Size.Width / 2) - 5, 120);
        }

        /// <summary>
        /// Backs the button touch up inside.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        public override void BackButton_TouchUpInside(object sender, EventArgs e)
        {
            base.BackButton_TouchUpInside(sender, e);
            this.NavigationController.PopViewController(true);
        }
    }
}