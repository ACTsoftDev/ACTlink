using System;
using System.Collections.ObjectModel;
using Foundation;
using MvvmCross.Binding.iOS.Views;
using OxyPlot;
using UIKit;

namespace actchargers.iOS
{
    public class PlotTableViewItemSource : MvxStandardTableViewSource
    {
        private ObservableCollection<GroupChartViewItem> _itemsSource;

        public PlotTableViewItemSource(UITableView tableView, string bindingText)
                : base(tableView, bindingText)
        {
        }

        public ObservableCollection<GroupChartViewItem> ListItemsSource
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
        /// Numbers the of sections.
        /// </summary>
        /// <returns>The of sections.</returns>
        /// <param name="tableView">Table view.</param>
        public override nint NumberOfSections(UITableView tableView)
        {
            return _itemsSource.Count;
        }

        /// <summary>
        /// Rowses the in section.
        /// </summary>
        /// <returns>The in section.</returns>
        /// <param name="tableview">Tableview.</param>
        /// <param name="section">Section.</param>
        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return _itemsSource[(int)section].Count;
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

            return _itemsSource[indexPath.Section][indexPath.Row];
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
            UITableViewCell cell;

            ChartViewItem tableItem = item as ChartViewItem;

            if (tableItem.CellType == ACUtility.CellTypes.Button)
            {
                cell = TableView.DequeueReusableCell(ButtonTableViewCell.Key, indexPath);
            }
            else if (tableItem.CellType == ACUtility.CellTypes.LabelTextEdit)
            {
                cell = TableView.DequeueReusableCell(LabelTextFieldTableViewCell.Key, indexPath);
            }
            else if (tableItem.CellType == ACUtility.CellTypes.Plot)
            {
                cell = TableView.DequeueReusableCell(PlotTableViewCell.Key, indexPath);
            }
            else if (tableItem.CellType == ACUtility.CellTypes.ListSelector)
            {
                cell = TableView.DequeueReusableCell(ListSelectorTabelViewCell.Key, indexPath);
            }
            else if (tableItem.CellType == ACUtility.CellTypes.RTrecordsPlot)
            {
                cell = TableView.DequeueReusableCell(RTRecordsPlotsTableViewCell.Key, indexPath);
            }
            else
            {
                cell = tableView.DequeueReusableCell(CellIdentifier);
                if (cell == null)
                {
                    cell = new UITableViewCell(UITableViewCellStyle.Default, CellIdentifier);
                }
                cell.Accessory = UITableViewCellAccessory.DisclosureIndicator;
                cell.TextLabel.Text = tableItem.Title;
            }

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
            ChartViewItem tableItem = _itemsSource[indexPath.Section][indexPath.Row];
            //if (tableItem.CellType != ACUtility.CellTypes.Plot)
            //{
            //    return tableView.EstimatedRowHeight;
            //}
            //else
            //{
            //    return 250;
            //}

            if (tableItem.CellType == ACUtility.CellTypes.Plot)
            {
                return 250;
            }
            else if (tableItem.CellType == ACUtility.CellTypes.RTrecordsPlot)
            {
                return 1000;
            }
            else
            {
                return tableView.EstimatedRowHeight;

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

        /// <summary>
        /// Rows the selected.
        /// </summary>
        /// <param name="tableView">Table view.</param>
        /// <param name="indexPath">Index path.</param>
        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            base.RowSelected(tableView, indexPath);
            ChartViewItem tableItem = _itemsSource[indexPath.Section][indexPath.Row];
            if (tableItem.CellType == ACUtility.CellTypes.ListSelector)
            {
                tableItem.SelectionCommand.Execute(tableItem);
            }
        }
    }
}
