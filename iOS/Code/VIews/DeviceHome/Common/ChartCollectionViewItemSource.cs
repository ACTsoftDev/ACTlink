using System;
using System.Collections.ObjectModel;
using Foundation;
using MvvmCross.Binding.iOS.Views;
using UIKit;

namespace actchargers.iOS
{
    public class ChartCollectionViewItemSource : MvxCollectionViewSource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:actchargers.iOS.ChartCollectionViewItemSource"/> class.
        /// </summary>
        /// <param name="collectionView">Collection view.</param>
        /// <param name="defaultCellIdentifier">Default cell identifier.</param>
        public ChartCollectionViewItemSource(UICollectionView collectionView, NSString defaultCellIdentifier) : base(collectionView, defaultCellIdentifier)
        {
        }

        /// <summary>
        /// The items source.
        /// </summary>
        private ObservableCollection<ChartViewItem> _itemsSource;
        /// <summary>
        /// Gets or sets the list items source.
        /// </summary>
        /// <value>The list items source.</value>
        public ObservableCollection<ChartViewItem> ListItemsSource
        {
            get
            {
                return _itemsSource;
            }

            set
            {
                _itemsSource = value;
                ReloadData();
            }
        }

        /// <summary>
        /// Gets the item at indexPath.
        /// </summary>
        /// <returns>The <see cref="T:System.Object"/>.</returns>
        /// <param name="indexPath">Index path.</param>
        protected override object GetItemAt(NSIndexPath indexPath)
        {
            if (_itemsSource == null)
            {
                return null;
            }

            return _itemsSource[indexPath.Row];
        }

        /// <summary>
        /// Gets the items count.
        /// </summary>
        /// <returns>The items count.</returns>
        /// <param name="collectionView">Collection view.</param>
        /// <param name="section">Section.</param>
        public override nint GetItemsCount(UICollectionView collectionView, nint section)
        {
            if (_itemsSource == null)
            {
                return 0;
            }

            return _itemsSource.Count;
        }

        /// <summary>
        /// Gets the or create cell for.
        /// </summary>
        /// <returns>The or create cell for.</returns>
        /// <param name="collectionView">Collection view.</param>
        /// <param name="indexPath">Index path.</param>
        /// <param name="item">Item.</param>
        protected override UICollectionViewCell GetOrCreateCellFor(UICollectionView collectionView, NSIndexPath indexPath, object item)
        {
            PieChartCollectionViewCell cell = (PieChartCollectionViewCell)collectionView.DequeueReusableCell(DefaultCellIdentifier, indexPath);
            //ChartViewItem chartItem = _itemsSource[indexPath.Row];
            //cell.PieChartView.Model = chartItem.PlotObject;
            return cell;
        }
    }
}
