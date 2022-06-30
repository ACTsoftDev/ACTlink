using System;
using CoreGraphics;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using MvvmCross.iOS.Views;
using UIKit;

namespace actchargers.iOS
{
    public partial class QuickView : BackViewController, IUICollectionViewDelegateFlowLayout
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:actchargers.iOS.QuickView"/> class.
        /// </summary>
        public QuickView() : base("QuickView", null)
        {
        }

        /// <summary>
        /// Views the did load.
        /// </summary>
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.

            this.CreateBinding(AHrTitle).To((QuickViewModel vm) => vm.AHRPlaceholder).Apply();
            this.CreateBinding(AHrValue).To((QuickViewModel vm) => vm.AHR).Apply();
            this.CreateBinding(KWHrTitle).To((QuickViewModel vm) => vm.KWHRPlaceholder).Apply();
            this.CreateBinding(KHWrValue).To((QuickViewModel vm) => vm.KWHR).Apply();
            this.CreateBinding(IDLETitle).To((QuickViewModel vm) => vm.IdlePlaceholder).Apply();
            this.CreateBinding(IDLEValue).To((QuickViewModel vm) => vm.IdleHour).Apply();
            this.CreateBinding(IDLEMins).To((QuickViewModel vm) => vm.IdleMin).Apply();
            this.CreateBinding(HrTitle).To((QuickViewModel vm) => vm.HrTitle).Apply();

            pieChartCollectionView.RegisterNibForCell(UINib.FromName("PieChartCollectionViewCell", NSBundle.MainBundle), "PieChartCollectionViewCell");
            var source = new ChartCollectionViewItemSource(pieChartCollectionView, new NSString("PieChartCollectionViewCell"));
            this.CreateBinding(source).For(s => s.ListItemsSource).To((QuickViewModel vm) => vm.QuickViewItemSource).Apply();
            pieChartCollectionView.Source = source;

            //IUICollectionViewDelegateFlowLayout delegate
            pieChartCollectionView.Delegate = this;

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
            return new CGSize((pieChartCollectionView.Frame.Size.Width - 10) / 2, (pieChartCollectionView.Frame.Size.Height - 10) / 2);
        }


        public override void BackButton_TouchUpInside(object sender, EventArgs e)
        {
            base.BackButton_TouchUpInside(sender, e);
            (ViewModel as QuickViewModel).OnBackButtonClick();
        }
    }
}