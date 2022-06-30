using System;
using System.Collections.ObjectModel;
using Foundation;
using MvvmCross.Binding.iOS.Views;
using MvvmCross.Core.ViewModels;
using UIKit;

namespace actchargers.iOS
{
    public class GroupItemTableViewSource : MvxStandardTableViewSource
    {
        MvxViewModel currentViewModel;

        public GroupItemTableViewSource(UITableView tableView, string bindingText, MvxViewModel viewModel)
                : base(tableView, bindingText)
        {
            currentViewModel = viewModel;
        }

        public GroupItemTableViewSource(UITableView tableView, string bindingText)
            : base(tableView, bindingText)
        {

        }

        ObservableCollection<TableHeaderItem> _itemsSource;
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
        protected override object GetItemAt(NSIndexPath indexPath)
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
        protected override UITableViewCell GetOrCreateCellFor(UITableView tableView, NSIndexPath indexPath, object item)
        {
            UITableViewCell cell;

            ListViewItem tableItem = item as ListViewItem;
            switch (tableItem.CellType)
            {
                case ACUtility.CellTypes.ThreeLabel:
                    if (string.IsNullOrEmpty(tableItem.ItemHeader))
                        cell = TableView.DequeueReusableCell(ThreeLabelTableViewCell.Key, indexPath);
                    else
                        cell = TableView.DequeueReusableCell(HeaderThreeLabelTableViewCell.Key, indexPath);
                    break;
                case ACUtility.CellTypes.TwoLabel:
                    if (string.IsNullOrEmpty(tableItem.ItemHeader))
                        cell = TableView.DequeueReusableCell(TwoLabelTableViewCell.Key, indexPath);
                    else
                        cell = TableView.DequeueReusableCell(HeaderTwoLabelTableViewCell.Key, indexPath);
                    break;
                case ACUtility.CellTypes.DatePicker:
                    cell = TableView.DequeueReusableCell(DatePickerTableViewCell.Key, indexPath);
                    break;
                case ACUtility.CellTypes.LabelLabel:
                    cell = TableView.DequeueReusableCell(LabelLabelTableViewCell.Key, indexPath);
                    break;
                case ACUtility.CellTypes.ListSelector:
                    cell = TableView.DequeueReusableCell(ListSelectorTabelViewCell.Key, indexPath);
                    break;
                case ACUtility.CellTypes.TimePicker:
                    cell = TableView.DequeueReusableCell(DatePickerTableViewCell.Key, indexPath);
                    break;
                case ACUtility.CellTypes.Days:
                    DaysNewTableViewCell daysCell = (DaysNewTableViewCell)TableView.DequeueReusableCell(DaysNewTableViewCell.Key, indexPath);
                    daysCell.CellEditingMode = tableItem.IsEditable;
                    cell = daysCell;
                    break;
                case ACUtility.CellTypes.LabelText:
                    cell = TableView.DequeueReusableCell(LabelTableViewCell.Key, indexPath);
                    break;
                case ACUtility.CellTypes.LabelTextEdit:
                    cell = TableView.DequeueReusableCell(LabelTextFieldTableViewCell.Key, indexPath);
                    break;
                case ACUtility.CellTypes.Button:
                    if (currentViewModel != null)
                    {
                        if (currentViewModel.GetType() == typeof(CalibrationViewModel))
                        {
                            cell = TableView.DequeueReusableCell(EnableDisableButtonTableViewCell.Key, indexPath);
                        }
                        else
                        {
                            cell = TableView.DequeueReusableCell(ButtonTableViewCell.Key, indexPath);
                        }
                    }
                    else
                    {
                        cell = TableView.DequeueReusableCell(ButtonTableViewCell.Key, indexPath);
                    }

                    break;
                case ACUtility.CellTypes.LabelSwitch:
                    cell = TableView.DequeueReusableCell(LabelSwitchTableViewCell.Key, indexPath);
                    break;
                case ACUtility.CellTypes.Image:
                    cell = TableView.DequeueReusableCell(ImageTableViewCell.Key, indexPath);
                    break;
                case ACUtility.CellTypes.QuickViewPlotCollection:
                    cell = TableView.DequeueReusableCell(QuickViewTableViewCell.Key, indexPath);
                    break;
                case ACUtility.CellTypes.QuickViewThreeLabel:
                    cell = TableView.DequeueReusableCell(QuickViewThreeLabelTableViewCell.Key, indexPath);
                    break;
                case ACUtility.CellTypes.Label:
                    cell = tableView.DequeueReusableCell(CellIdentifier);
                    if (cell == null)
                        cell = new UITableViewCell(UITableViewCellStyle.Default, CellIdentifier);
                    cell.Accessory = UITableViewCellAccessory.DisclosureIndicator;
                    cell.TextLabel.Text = tableItem.Title;
                    break;
                case ACUtility.CellTypes.Default:
                    cell = tableView.DequeueReusableCell(CellIdentifier);
                    if (cell == null)
                        cell = new UITableViewCell(UITableViewCellStyle.Default, CellIdentifier);
                    cell.TextLabel.Text = tableItem.Title;
                    break;
                case ACUtility.CellTypes.LabelCenter:
                    cell = tableView.DequeueReusableCell(CellIdentifier);
                    if (cell == null)
                        cell = new UITableViewCell(UITableViewCellStyle.Default, CellIdentifier);
                    cell.TextLabel.Text = tableItem.Title;
                    cell.TextLabel.TextColor = UIColor.Gray;
                    cell.TextLabel.TextAlignment = UITextAlignment.Center;
                    cell.TextLabel.Font = UIFont.FromName("Helvetica-Bold", 22f);
                    cell.SelectionStyle = UITableViewCellSelectionStyle.None;
                    break;
                case ACUtility.CellTypes.SectionHeader:
                    cell = TableView.DequeueReusableCell(SectionHeaderTableViewCell.Key, indexPath);
                    break;
                default:
                    cell = new UITableViewCell();
                    break;
            }

            return cell;
        }

        public override void CellDisplayingEnded
        (UITableView tableView, UITableViewCell cell, NSIndexPath indexPath)
        {
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            base.RowSelected(tableView, indexPath);

            ListViewItem tableItem = ListItemsSource[indexPath.Section][indexPath.Row] as ListViewItem;

            if (tableItem.CellType == ACUtility.CellTypes.ListSelector)
            {
                tableItem.ListSelectionCommand.Execute(tableItem);
            }
            else if (tableItem.CellType == ACUtility.CellTypes.Label && tableItem.ListSelectionCommand != null)
            {
                tableItem.ListSelectionCommand.Execute(tableItem);
            }
        }

        public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            ListViewItem tableItem = _itemsSource[indexPath.Section][indexPath.Row];
            switch (tableItem.CellType)
            {
                case ACUtility.CellTypes.Days:
                    return 180;
                case ACUtility.CellTypes.QuickViewPlotCollection:
                    return 340;
                case ACUtility.CellTypes.QuickViewThreeLabel:
                    return 120;
                case ACUtility.CellTypes.Image:
                    return 400;
            }

            return TableView.EstimatedRowHeight;
        }
    }
}
