using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Foundation;
using MvvmCross.Binding.iOS.Views;
using OxyPlot;
using UIKit;

namespace actchargers.iOS
{
    public class PlotViewListItemSource : MvxStandardTableViewSource
    {
        private ObservableCollection<ChartViewItem> _itemsSource;

        public PlotViewListItemSource(UITableView tableView, string bindingText)
                : base(tableView, bindingText)
        {
        }

        public ObservableCollection<ChartViewItem> ListItemsSource
        {
            get
            {
                return _itemsSource;
            }

            set
            {
                _itemsSource = value;
                ReloadTableData();
            }
        }

        /// <summary>
        /// Rowses the in section.
        /// </summary>
        /// <returns>The in section.</returns>
        /// <param name="tableview">Tableview.</param>
        /// <param name="section">Section.</param>
        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return _itemsSource.Count;
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
        /// Gets the or create cell for.
        /// </summary>
        /// <returns>The or create cell for.</returns>
        /// <param name="tableView">Table view.</param>
        /// <param name="indexPath">Index path.</param>
        /// <param name="item">Item.</param>
        protected override UITableViewCell GetOrCreateCellFor(UITableView tableView, Foundation.NSIndexPath indexPath, object item)
        {
            UITableViewCell cell = tableView.DequeueReusableCell(PlotTableViewCell.Key,indexPath);
            (cell as PlotTableViewCell).PlotViewButton.Hidden = false;
            return cell;
        }

        /// <summary>
        /// Gets the height for row.
        /// </summary>
        /// <returns>The height for row.</returns>
        /// <param name="tableView">Table view.</param>
        /// <param name="indexPath">Index path.</param>
        public override nfloat GetHeightForRow(UIKit.UITableView tableView, Foundation.NSIndexPath indexPath)
        {
            ChartViewItem tableItem = _itemsSource[indexPath.Row];
            if (tableItem.CellType != ACUtility.CellTypes.Plot)
            {
                return tableView.EstimatedRowHeight;
            }
            else
            {
                return 350;
            }
        }

        /// <summary>
        /// Cells the displaying ended.
        /// </summary>
        /// <param name="tableView">Table view.</param>
        /// <param name="cell">Cell.</param>
        /// <param name="indexPath">Index path.</param>
        public override void CellDisplayingEnded(UITableView tableView, UITableViewCell cell, NSIndexPath indexPath)
        {
            //            base.CellDisplayingEnded (tableView, cell, indexPath);
        }
    }
}
