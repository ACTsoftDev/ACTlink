using System;
using System.Collections.Generic;
using Foundation;
using MvvmCross.Binding.iOS.Views;
using UIKit;

namespace actchargers.iOS
{
    public class StringListTableViewSource : MvxStandardTableViewSource
    {
        private List<string> _itemsSource;


        public StringListTableViewSource(UITableView tableView, string bindingText)
                : base(tableView, bindingText)
        {
        }

        public List<string> ListItemsSource
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
            string ListSelectorCellIdentifier = "ListSelectorCellIdentifier";
            UITableViewCell cell = tableView.DequeueReusableCell(ListSelectorCellIdentifier);
            if (cell == null)
            {
                cell = new UITableViewCell(UITableViewCellStyle.Default, ListSelectorCellIdentifier);
            }
            cell.TextLabel.Text = ListItemsSource[indexPath.Row];
            cell.TextLabel.Lines = 0;
            cell.TextLabel.Font = UIFont.SystemFontOfSize(14);
            cell.TextLabel.LineBreakMode = UILineBreakMode.WordWrap;

            return cell;
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
