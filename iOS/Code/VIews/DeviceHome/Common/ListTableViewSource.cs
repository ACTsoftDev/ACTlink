using System;
using System.Collections.Generic;
using Foundation;
using MvvmCross.Binding.iOS.Views;
using UIKit;

namespace actchargers.iOS
{
	public class ListTableViewSource: MvxStandardTableViewSource
	{
		private List<ListViewItem> _itemsSource;

		public ListTableViewSource(UITableView tableView, string bindingText)
				: base(tableView, bindingText)
		{
		}

		public List<ListViewItem> ListItemsSource
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
		protected override UITableViewCell GetOrCreateCellFor(UITableView tableView, NSIndexPath indexPath, object item)
		{
			UITableViewCell cell;

			ListViewItem tableItem = item as ListViewItem;

			if (tableItem.CellType == ACUtility.CellTypes.LabelLabel)
			{
				cell = TableView.DequeueReusableCell(LabelLabelTableViewCell.Key, indexPath);
			}
			else if (tableItem.CellType == ACUtility.CellTypes.TwoLabel)
			{
				cell = TableView.DequeueReusableCell(TwoLabelTableViewCell.Key, indexPath);

			}
			else if (tableItem.CellType == ACUtility.CellTypes.ThreeLabel)
			{
				cell = TableView.DequeueReusableCell(ThreeLabelTableViewCell.Key, indexPath);
			}
			else if (tableItem.CellType == ACUtility.CellTypes.LabelTextEdit)
			{
				cell = TableView.DequeueReusableCell(LabelTextFieldTableViewCell.Key, indexPath);
			}
			else if (tableItem.CellType == ACUtility.CellTypes.LabelSwitch)
			{
				cell = TableView.DequeueReusableCell(LabelSwitchTableViewCell.Key, indexPath);
			}
			else if (tableItem.CellType == ACUtility.CellTypes.ListSelector)
			{
				cell = TableView.DequeueReusableCell(ListSelectorTabelViewCell.Key, indexPath);
			}
			else {
				//string CellIdentifier = "CellIdentifier";
				cell = tableView.DequeueReusableCell(CellIdentifier);
				if (cell == null)
					cell = new UITableViewCell(UITableViewCellStyle.Default, CellIdentifier);
				cell.Accessory = UITableViewCellAccessory.DisclosureIndicator;
				cell.TextLabel.Text = tableItem.Title;
			}

			return cell;
		}

        //public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        //{
        //    base.GetHeightForRow(tableView, indexPath);

        //    ListViewItem tableItem = _itemsSource[indexPath.Row];

        //    if(tableItem.CellType == ACUtility.CellTypes.ListSelector)
        //    {
        //        return 
        //    }
        //}

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

			ListViewItem tableItem = ListItemsSource[indexPath.Row] as ListViewItem;

			if (tableItem.CellType == ACUtility.CellTypes.ListSelector)
			{
				tableItem.ListSelectionCommand.Execute(tableItem);
			}
		}
	}
}
