using System;
using CoreGraphics;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using UIKit;

namespace actchargers.iOS
{
    public partial class QuickViewTableViewCell : MvxTableViewCell, IUICollectionViewDelegateFlowLayout
    {
        public static readonly NSString Key = new NSString("QuickViewTableViewCell");
        public static readonly UINib Nib;

        static QuickViewTableViewCell()
        {
            Nib = UINib.FromName("QuickViewTableViewCell", NSBundle.MainBundle);
        }

        protected QuickViewTableViewCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
            this.SelectionStyle = UITableViewCellSelectionStyle.None;
            this.DelayBind(() =>
            {
                listCollectionView.RegisterNibForCell(UINib.FromName("PieChartCollectionViewCell", NSBundle.MainBundle), "PieChartCollectionViewCell");
                var source = new MvxCollectionViewSource(listCollectionView, new NSString("PieChartCollectionViewCell"));


                var set = this.CreateBindingSet<QuickViewTableViewCell, ListViewItem>();
                set.Bind(source).For(s => s.ItemsSource).To(item => item.Items).Apply();
                //set.Bind(titleLabel).To(item => item.Title).Apply();
                listCollectionView.Source = source;

                //IUICollectionViewDelegateFlowLayout delegate
                listCollectionView.Delegate = this;

            });
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
            return new CGSize((collectionView.Frame.Size.Width - 10) / 2, (collectionView.Frame.Size.Height - 10) / 2);
        }
    }
}
