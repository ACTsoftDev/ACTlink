using System;
using System.Collections.ObjectModel;
using Foundation;
using MvvmCross.Binding.iOS.Views;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform.Core;
using ObjCRuntime;
using UIKit;

namespace actchargers.iOS
{
    public class SyncSitesTableViewSource : MvxStandardTableViewSource
    {
        private MvxViewModel currentViewModel;


        public SyncSitesTableViewSource(UITableView tableView, string bindingText, MvxViewModel viewModel)
                : base(tableView, bindingText)
        {
            currentViewModel = viewModel;
        }

        public SyncSitesTableViewSource(UITableView tableView, string bindingText)
            : base(tableView, bindingText)
        {

        }

        private ObservableCollection<SyncSitesHeaderItem> _itemsSource;
        public ObservableCollection<SyncSitesHeaderItem> ListItemsSource
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
            return _itemsSource[(int)section].ChildItems.Count;
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
            //return _itemsSource[indexPath.Section][indexPath.Row];
            return _itemsSource[indexPath.Section].ChildItems[indexPath.Row];
        }

        /// <summary>
        /// Gets the height for header.
        /// </summary>
        /// <returns>The height for header.</returns>
        /// <param name="tableView">Table view.</param>
        /// <param name="section">Section.</param>
        public override nfloat GetHeightForHeader(UITableView tableView, nint section)
        {
            return 40;
        }

        /// <summary>
        /// Gets the view for header.
        /// </summary>
        /// <returns>The view for header.</returns>
        /// <param name="tableView">Table view.</param>
        /// <param name="section">Section.</param>
        public override UIView GetViewForHeader(UITableView tableView, nint section)
        {
            var cell = TableView.DequeueReusableCell(SyncSitesHeaderTableViewCell.Key);

            var bindable = cell as IMvxDataConsumer;
            if (bindable != null)
                bindable.DataContext = _itemsSource[(int)section];

            return cell;

            //var views = NSBundle.MainBundle.LoadNib("SyncSitesHeaderTableViewCell", this, null);
            //SyncSitesHeaderTableViewCell headerView = Runtime.GetNSObject(views.ValueAt(0)) as SyncSitesHeaderTableViewCell;

            //headerView.HeaderLabel.Text = _itemsSource[(int)section].Name;


            //var tapGestureRecognizer = new UITapGestureRecognizer((obj) =>
            //{
            //    if (_itemsSource[(int)section].Sites != null && _itemsSource[(int)section].Sites.Count > 0)
            //    {
            //        if (_itemsSource[(int)section].IsExpanded)
            //        {
            //            _itemsSource[(int)section].Clear();
            //            _itemsSource[(int)section].IsExpanded = false;
            //        }
            //        else
            //        {
            //            _itemsSource[(int)section].AddRange(_itemsSource[(int)section].Sites);
            //            _itemsSource[(int)section].IsExpanded = true;
            //        }
            //        NSIndexSet indexset = NSIndexSet.FromIndex(section);
            //        tableView.ReloadSections(indexset, UITableViewRowAnimation.Automatic);
            //    }

            //});

            //tapGestureRecognizer.NumberOfTapsRequired = 1;
            //headerView.ExpandBtn.AddGestureRecognizer(tapGestureRecognizer);
            ////headerView.HeaderLabel.AddGestureRecognizer(tapGestureRecognizer);

            //return headerView;
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
            ACTViewSite tableItem = item as ACTViewSite;
            cell = TableView.DequeueReusableCell(SyncSitesItemTableViewCell.Key, indexPath);
            return cell;
        }

        public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            return 40;
        }

        //public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        //{
        //    var item = GetItemAt(indexPath);
        //    var cell = GetOrCreateCellFor(tableView, indexPath, item);

        //    var bindable = cell as IMvxDataConsumer;
        //    if (bindable != null)
        //        bindable.DataContext = item;

        //    return cell;
        //}

    }
}
