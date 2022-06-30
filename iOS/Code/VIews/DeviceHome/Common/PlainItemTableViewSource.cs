using System;
using System.Collections.ObjectModel;
using Foundation;
using MvvmCross.Binding.iOS.Views;
using MvvmCross.Platform.Core;
using UIKit;

namespace actchargers.iOS
{
    public class PlainItemTableViewSource : MvxStandardTableViewSource
    {
        ObservableCollection<ListViewItem> _itemsSource;

        public PlainItemTableViewSource(UITableView tableView, string bindingText)
                : base(tableView, bindingText)
        {
        }

        public ObservableCollection<ListViewItem> ListItemsSource
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

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return _itemsSource.Count;
        }

        protected override object GetItemAt(NSIndexPath indexPath)
        {
            if (_itemsSource == null)
                return null;

            return _itemsSource[indexPath.Row];
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var item = GetItemAt(indexPath);
            var cell = GetOrCreateCellFor(tableView, indexPath, item);

            var bindable = cell as IMvxDataConsumer;

            if (bindable != null)
                bindable.DataContext = item;

            return cell;
        }

        protected override UITableViewCell GetOrCreateCellFor(UITableView tableView, NSIndexPath indexPath, object item)
        {
            UITableViewCell cell;

            ListViewItem tableItem = item as ListViewItem;
            switch (tableItem.CellType)
            {
                case ACUtility.CellTypes.LabelLabel:
                    cell = TableView.DequeueReusableCell(LabelLabelTableViewCell.Key, indexPath);
                    break;
                case ACUtility.CellTypes.TwoLabel:
                    cell = TableView.DequeueReusableCell(TwoLabelTableViewCell.Key, indexPath);
                    break;
                case ACUtility.CellTypes.ThreeLabel:
                    cell = TableView.DequeueReusableCell(ThreeLabelTableViewCell.Key, indexPath);
                    break;
                case ACUtility.CellTypes.LabelTextEdit:
                    cell = TableView.DequeueReusableCell(LabelTextFieldTableViewCell.Key, indexPath);
                    break;
                case ACUtility.CellTypes.LabelSwitch:
                    cell = TableView.DequeueReusableCell(LabelSwitchTableViewCell.Key, indexPath);
                    break;
                case ACUtility.CellTypes.ListSelector:
                    cell = TableView.DequeueReusableCell(ListSelectorTabelViewCell.Key, indexPath);
                    break;
                case ACUtility.CellTypes.DatePicker:
                    cell = TableView.DequeueReusableCell(DatePickerTableViewCell.Key, indexPath);
                    break;
                case ACUtility.CellTypes.DatePickerWithSwitch:
                    cell = TableView.DequeueReusableCell(DatePickerSwitchTableViewCell.Key, indexPath);
                    break;
                case ACUtility.CellTypes.Button:
                    cell = TableView.DequeueReusableCell(ButtonTableViewCell.Key, indexPath);
                    break;
                case ACUtility.CellTypes.ButtonTextEdit:
                    cell = TableView.DequeueReusableCell(ButtonTextFieldTableViewCell.Key, indexPath);
                    break;
                case ACUtility.CellTypes.LabelSwitchButton:
                    cell = TableView.DequeueReusableCell(LabelSwitchButtonTableViewCell.Key, indexPath);
                    break;
                case ACUtility.CellTypes.ImageLabel:
                    cell = TableView.DequeueReusableCell(ImageLabelTableViewCell.Key, indexPath);
                    break;
                case ACUtility.CellTypes.LabelText:
                    cell = TableView.DequeueReusableCell(LabelTableViewCell.Key, indexPath);
                    break;
                case ACUtility.CellTypes.Days:
                    DaysNewTableViewCell daysCell = (DaysNewTableViewCell)TableView.DequeueReusableCell(DaysNewTableViewCell.Key, indexPath);
                    daysCell.CellEditingMode = tableItem.IsEditable;
                    cell = daysCell;
                    break;
                case ACUtility.CellTypes.SectionHeader:
                    cell = TableView.DequeueReusableCell(SectionHeaderTableViewCell.Key, indexPath);
                    break;
                default:
                    cell = tableView.DequeueReusableCell(CellIdentifier);

                    if (cell == null)
                        cell = new UITableViewCell(UITableViewCellStyle.Default, CellIdentifier);

                    cell.Accessory = UITableViewCellAccessory.DisclosureIndicator;
                    cell.TextLabel.Text = tableItem.Title;
                    break;
            }

            cell.Hidden = !tableItem.IsVisible;

            return cell;
        }

        public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            ListViewItem tableItem = _itemsSource[indexPath.Row];

            if (tableItem.CellType == ACUtility.CellTypes.Days)
                return 180;

            if (tableItem.CellType == ACUtility.CellTypes.ButtonTextEdit)
                return 92;

            return TableView.EstimatedRowHeight;
        }

        public override void CellDisplayingEnded
        (UITableView tableView, UITableViewCell cell, NSIndexPath indexPath)
        {
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            base.RowSelected(tableView, indexPath);

            ListViewItem tableItem = ListItemsSource[indexPath.Row];

            if (tableItem.CellType == ACUtility.CellTypes.ListSelector)
                tableItem.ListSelectionCommand.Execute(tableItem);
        }
    }
}
