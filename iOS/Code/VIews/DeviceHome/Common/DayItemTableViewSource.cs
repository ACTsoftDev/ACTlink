using System;
using System.Collections.ObjectModel;
using Foundation;
using MvvmCross.Binding.iOS.Views;
using UIKit;

namespace actchargers.iOS
{
    public class DayItemTableViewSource : MvxStandardTableViewSource
    {
        public DayItemTableViewSource(UITableView tableView, string bindingText)
                : base(tableView, bindingText)
        {
        }

        private ObservableCollection<TableHeaderItem> _itemsSource;
        public ObservableCollection<TableHeaderItem> ListItemsSource
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
        protected override object GetItemAt(Foundation.NSIndexPath indexPath)
        {
            if (_itemsSource == null)
            {
                return null;
            }
            return _itemsSource[indexPath.Section][indexPath.Row];
        }

        /// <summary>
        /// Gets the height for header.
        /// </summary>
        /// <returns>The height for header.</returns>
        /// <param name="tableView">Table view.</param>
        /// <param name="section">Section.</param>
        public override nfloat GetHeightForHeader(UITableView tableView, nint section)
        {
            return 20;
        }

        /// <summary>
        /// Titles for header.
        /// </summary>
        /// <returns>The for header.</returns>
        /// <param name="tableView">Table view.</param>
        /// <param name="section">Section.</param>
        public override string TitleForHeader(UITableView tableView, nint section)
        {
            TableHeaderItem item = _itemsSource[(int)section];
            return item.SectionHeader;
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

            //DayListViewItem tableItem = item as DayListViewItem;

            //if (tableItem.CellType == ACUtility.CellTypes.LabelLabel)
            //{
            //    cell = TableView.DequeueReusableCell(LabelLabelTableViewCell.Key, indexPath);
            //}
            //else if (tableItem.CellType == ACUtility.CellTypes.ListSelector)
            //{
            //    cell = TableView.DequeueReusableCell(ListSelectorTabelViewCell.Key, indexPath);
            //}
            //else if (tableItem.CellType == ACUtility.CellTypes.TimePicker)
            //{
            //    cell = TableView.DequeueReusableCell(DatePickerTableViewCell.Key, indexPath);
            //}
            //else if (tableItem.CellType == ACUtility.CellTypes.Days)
            //{
            //    cell = TableView.DequeueReusableCell(DaysNewTableViewCell.Key, indexPath);
            //}
            //else if (tableItem.CellType == ACUtility.CellTypes.LabelText)
            //{
            //    cell = TableView.DequeueReusableCell(LabelTableViewCell.Key, indexPath);
            //}
            //else if (tableItem.CellType == ACUtility.CellTypes.LabelTextEdit)
            //{
            //    cell = TableView.DequeueReusableCell(LabelTextFieldTableViewCell.Key, indexPath);
            //}
            //else if (tableItem.CellType == ACUtility.CellTypes.Button)
            //{
            //    cell = TableView.DequeueReusableCell(ButtonTableViewCell.Key, indexPath);
            //}
            //else
            //{
                cell = new UITableViewCell();
            //}

            return cell;
        }

        /// <summary>
        /// Cells the displaying ended.
        /// </summary>
        /// <param name="tableView">Table view.</param>
        /// <param name="cell">Cell.</param>
        /// <param name="indexPath">Index path.</param>
        public override void CellDisplayingEnded(UITableView tableView, UITableViewCell cell, Foundation.NSIndexPath indexPath)
        {
            //base.CellDisplayingEnded(tableView, cell, indexPath);
        }

        public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            //base.GetHeightForRow(tableView, indexPath);

            ListViewItem tableItem = _itemsSource[indexPath.Section][indexPath.Row];

            if (tableItem.CellType == ACUtility.CellTypes.Days)
            {
                return 180;
            }

            return 60;
        }

        /// <summary>
        /// Rows the selected.
        /// </summary>
        /// <param name="tableView">Table view.</param>
        /// <param name="indexPath">Index path.</param>
        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            base.RowSelected(tableView, indexPath);

            ListViewItem tableItem = ListItemsSource[indexPath.Section][indexPath.Row] as ListViewItem;

            if (tableItem.CellType == ACUtility.CellTypes.ListSelector)
            {
                tableItem.ListSelectionCommand.Execute(tableItem);
            }
        }

    }
}
