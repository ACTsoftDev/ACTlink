using System;
using System.Collections.Generic;
using CoreGraphics;
using Foundation;
using MvvmCross.Binding.iOS.Views;
using UIKit;

namespace actchargers.iOS
{
    public class SideMenuTableViewSource : MvxStandardTableViewSource
    {
        private List<MenuViewItem> _itemsSource;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:actchargers.iOS.SideMenuTableViewSource"/> class.
        /// </summary>
        /// <param name="tableView">Table view.</param>
        /// <param name="bindingText">Binding text.</param>
        public SideMenuTableViewSource(UITableView tableView, string bindingText)
            : base(tableView, bindingText)
        {
            tableView.RegisterClassForCellReuse(typeof(UITableViewCell), new NSString("UITableViewCell"));
        }

        /// <summary>
        /// Gets or sets the items source.
        /// </summary>
        /// <value>The items source.</value>
        public List<MenuViewItem> TableItemsSource
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
        /// <returns>The <see cref="System.Object"/>.</returns>
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
            UITableViewCell cell = TableView.DequeueReusableCell("UITableViewCell", indexPath);
            MenuViewItem menuItem = _itemsSource[indexPath.Row];
            cell.TextLabel.Text = menuItem.Title;
            cell.TextLabel.TextColor = UIColorUtility.FromHex(ACColors.WHITE_COLOR);
            cell.TextLabel.Font = UIFont.SystemFontOfSize(18, UIFontWeight.Regular);
            cell.BackgroundColor = UIColor.Clear;
            cell.SelectionStyle = UITableViewCellSelectionStyle.None;
            return cell;
        }

        /// <summary>
        /// Gets the height for row.
        /// </summary>
        /// <returns>The height for row.</returns>
        /// <param name="tableView">Table view.</param>
        /// <param name="indexPath">Index path.</param>
        public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            return 40;
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
        /// Gets the height for footer.
        /// </summary>
        /// <returns>The height for footer.</returns>
        /// <param name="tableView">Table view.</param>
        /// <param name="section">Section.</param>
        public override nfloat GetHeightForFooter(UITableView tableView, nint section)
        {
            return 1;
        }

        /// <summary>
        /// Gets the view for footer.
        /// </summary>
        /// <returns>The view for footer.</returns>
        /// <param name="tableView">Table view.</param>
        /// <param name="section">Section.</param>
        public override UIView GetViewForFooter(UITableView tableView, nint section)
        {
            UIView mainView = new UIView(new CGRect(0, 0, tableView.Frame.Size.Width, 1));
            mainView.BackgroundColor = tableView.BackgroundColor;

            return mainView;
        }
    }
}
