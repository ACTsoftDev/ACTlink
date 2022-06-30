using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Android.Content;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Binding.Droid.Views;
using MvvmCross.Binding.ExtensionMethods;
using MvvmCross.Platform;

namespace actchargers.Droid
{
    public class CustomSectionEditAdapter : MvxAdapter, IExpandableListAdapter
    {
        private IList _itemsSource;
        BaseViewModel model;

        Context _mContext;

        public CustomSectionEditAdapter(Context context, IMvxAndroidBindingContext bindingContext, IList itemSource,BaseViewModel viewModel)
            : base(context, bindingContext)
        {
            _mContext = context;
            _itemsSource = itemSource;
             model = viewModel;
            _mContext = context;

        }

        int _groupTemplateId = Resource.Layout.section_header;

        public int GroupTemplateId
        {
            get { return _groupTemplateId; }
            set
            {
                if (_groupTemplateId == value)
                    return;

                _groupTemplateId = value;
                // since the template has changed then let's force the list to redisplay by firing NotifyDataSetChanged()
                if (ItemsSource != null)
                    NotifyDataSetChanged();
            }
        }

        protected override void SetItemsSource(IEnumerable value)
        {
            Mvx.Trace("Setting itemssource");

            if (_itemsSource == value)
                return;

            var existingObservable = _itemsSource as INotifyCollectionChanged;
            if (existingObservable != null)
                existingObservable.CollectionChanged -= OnItemsSourceCollectionChanged;

            _itemsSource = value as IList;

            var newObservable = _itemsSource as INotifyCollectionChanged;
            if (newObservable != null)
                newObservable.CollectionChanged += OnItemsSourceCollectionChanged;

            base.SetItemsSource(value);
        }

        public int GroupCount
        {
            get { return (_itemsSource != null ? _itemsSource.Count : 0); }
        }

        public void OnGroupExpanded(int groupPosition)
        {
            // do nothing

        }

        public void OnGroupCollapsed(int groupPosition)
        {
            // do nothing
        }

        public bool IsChildSelectable(int groupPosition, int childPosition)
        {
            return true;
        }
        /// <summary>
        /// Gets the group view.
        /// </summary>
        /// <returns>The group view.</returns>
        /// <param name="groupPosition">Group position.</param>
        /// <param name="isExpanded">If set to <c>true</c> is expanded.</param>
        /// <param name="convertView">Convert view.</param>
        /// <param name="parent">Parent.</param>
		public View GetGroupView(int groupPosition, bool isExpanded, View convertView, ViewGroup parent)
        {
            var item = _itemsSource[groupPosition];
            TableHeaderItem tItem = (TableHeaderItem)item;
            View view = base.GetBindableView(convertView, item, GroupTemplateId);
            var headerItem = view.FindViewById(Resource.Id.itemTitle);
            var sectionSeperator = view.FindViewById(Resource.Id.section_seperator_view);

            if (tItem.SectionHeader==null || string.IsNullOrEmpty(tItem.SectionHeader.Trim()))
            {
                headerItem.Visibility = ViewStates.Gone;
                if (groupPosition != 0 && GetChildrenCount(groupPosition)>0)
                {
                    sectionSeperator.Visibility = ViewStates.Visible;
                }else{
                    sectionSeperator.Visibility = ViewStates.Gone;
                }
            }
            else {
                //hiding section seperator for first element
                if (groupPosition != 0 && GetChildrenCount(groupPosition) > 0)
                {
                    sectionSeperator.Visibility = ViewStates.Visible;
                }
                else
                {
                    sectionSeperator.Visibility = ViewStates.Gone;
                }
                headerItem.Visibility = ViewStates.Visible;
            }

            return view;
        }
        /// <summary>
        /// Gets the group identifier.
        /// </summary>
        /// <returns>The group identifier.</returns>
        /// <param name="groupPosition">Group position.</param>
		public long GetGroupId(int groupPosition)
        {
            return groupPosition;
        }

        public Java.Lang.Object GetGroup(int groupPosition)
        {
            return null;
        }

        // Base implementation returns a long (from BaseExpandableListAdapter.java):
        // bit 0: Whether this ID points to a child (unset) or group (set), so for this method
        //        this bit will be 1.
        // bit 1-31: Lower 31 bits of the groupId
        // bit 32-63: Lower 32 bits of the childId.
        public long GetCombinedGroupId(long groupId)
        {
            return (groupId & 0x7FFFFFFF) << 32;
        }

        // Base implementation returns a long:
        // bit 0: Whether this ID points to a child (unset) or group (set), so for this method
        //        this bit will be 0.
        // bit 1-31: Lower 31 bits of the groupId
        // bit 32-63: Lower 32 bits of the childId.
        public long GetCombinedChildId(long groupId, long childId)
        {
            return (long)(0x8000000000000000UL | (ulong)((groupId & 0x7FFFFFFF) << 32) | (ulong)(childId & 0xFFFFFFFF));
        }

        public object GetRawItem(int groupPosition, int position)
        {
            return (GetRawGroup(groupPosition) as IEnumerable).Cast<object>().ToList()[position];
        }

        public object GetRawGroup(int groupPosition)
        {
            return _itemsSource[groupPosition];
        }
        /// <summary>
        /// Gets the child view.
        /// </summary>
        /// <returns>The child view.</returns>
        /// <param name="groupPosition">Group position.</param>
        /// <param name="childPosition">Child position.</param>
        /// <param name="isLastChild">If set to <c>true</c> is last child.</param>
        /// <param name="convertView">Convert view.</param>
        /// <param name="parent">Parent.</param>
		public View GetChildView(int groupPosition, int childPosition, bool isLastChild, View convertView, ViewGroup parent)
        {
            var sublist = (GetRawGroup(groupPosition) as IEnumerable).Cast<object>().ToList();
            var item = sublist[childPosition];

            ListViewItem tItem = (ListViewItem)item;

            int childTemplateID = Resource.Layout.item_title;

            if (tItem.CellType == ACUtility.CellTypes.LabelLabel)
            {
                childTemplateID = Resource.Layout.item_label;
            }
			else if (tItem.CellType == ACUtility.CellTypes.Default)
			{
				childTemplateID = Resource.Layout.item_label;
			}
            else if (tItem.CellType == ACUtility.CellTypes.LabelCenter)
            {
                childTemplateID = Resource.Layout.item_labelcenter;
            }
            else if (tItem.CellType == ACUtility.CellTypes.LabelTextEdit)
            {
                childTemplateID = Resource.Layout.item_edit;
            }
            else if (tItem.CellType == ACUtility.CellTypes.LabelText)
            {
                childTemplateID = Resource.Layout.item_label_text;
            }
            else if (tItem.CellType == ACUtility.CellTypes.TimePicker)
            {
                childTemplateID = Resource.Layout.item_time_picker;
            }
            else if (tItem.CellType == ACUtility.CellTypes.ListSelector)
            {
                childTemplateID = Resource.Layout.item_list;
            }
            else if (tItem.CellType == ACUtility.CellTypes.Days)
            {
                childTemplateID = Resource.Layout.item_days_grid;

            }
            else if (tItem.CellType == ACUtility.CellTypes.Button)

            {
                childTemplateID = Resource.Layout.item_button;
            }

            else if (tItem.CellType == ACUtility.CellTypes.ThreeLabel)
            {
                childTemplateID = Resource.Layout.section_three_label_item;
            }
            else if (tItem.CellType == ACUtility.CellTypes.TwoLabel)
            {
                childTemplateID = Resource.Layout.section_three_label_item;
            }
            else if (tItem.CellType == ACUtility.CellTypes.Label)
            {
                childTemplateID = Resource.Layout.item_title;
            }
            else if (tItem.CellType == ACUtility.CellTypes.DatePicker)
            {
                childTemplateID = Resource.Layout.item_date_picker;
            }
            else if (tItem.CellType == ACUtility.CellTypes.LabelSwitch)
            {
                childTemplateID = Resource.Layout.item_switch;
            }
			else if (tItem.CellType == ACUtility.CellTypes.Image)
			{
				childTemplateID = Resource.Layout.item_image;
			}
			else if (tItem.CellType == ACUtility.CellTypes.QuickViewPlotCollection)
			{
				childTemplateID = Resource.Layout.item_quickviewplotcollection;
			}
			else if (tItem.CellType == ACUtility.CellTypes.QuickViewThreeLabel)
			{
				childTemplateID = Resource.Layout.item_quickviewthreelable;
			}


            convertView= base.GetBindableView(convertView, item, childTemplateID);

            if (tItem.CellType == ACUtility.CellTypes.DatePickerWithSwitch || tItem.CellType == ACUtility.CellTypes.DatePicker)
            {
                AcCustomDateTextView datePickerTextView = convertView.FindViewById<AcCustomDateTextView>(Resource.Id.dateTextView);
                datePickerTextView.DataContext = tItem;
            }
            if (tItem.DefaultCellType==ACUtility.CellTypes.LabelCenter)
            {
                convertView.FindViewById<TextView>(Resource.Id.key).TextAlignment = TextAlignment.Center;
                convertView.FindViewById<TextView>(Resource.Id.key).Gravity = GravityFlags.CenterHorizontal;
                convertView.FindViewById<TextView>(Resource.Id.key).TextSize = 20;
            }
            if (tItem.CellType == ACUtility.CellTypes.TimePicker)
            {
                AcCustomTimePickerTextView timePickerTextView = convertView.FindViewById<AcCustomTimePickerTextView>(Resource.Id.timePickerText);
                timePickerTextView.DataContext = tItem;
            }
            if (tItem.CellType == ACUtility.CellTypes.LabelTextEdit)
            {
                AcEditText editTextView = convertView.FindViewById<AcEditText>(Resource.Id.value);
                if (tItem.TextInputType == ACUtility.InputType.Decimal)
                {
                    editTextView.InputType = Android.Text.InputTypes.NumberFlagDecimal;
                }
                else if (tItem.TextInputType == ACUtility.InputType.Number)
                {
                    editTextView.InputType = Android.Text.InputTypes.ClassNumber;
                }
                else if (tItem.TextInputType == ACUtility.InputType.Default)
                {
                    editTextView.InputType = Android.Text.InputTypes.ClassText;
                }

            }
            if (tItem.CellType == ACUtility.CellTypes.Days)
            {
                CustomGrid daysGrid = convertView.FindViewById<CustomGrid>(Resource.Id.days_grid);
                DaysGridAdapter adapter = new DaysGridAdapter(_mContext, (IMvxAndroidBindingContext)BindingContext);
                daysGrid.Adapter = adapter;
            }
             
            if (model!=null && model.GetType() == typeof(CalibrationViewModel))
            {

                if (tItem.IsEditable)
                {
                    convertView.Enabled = true;
                    if (tItem.CellType == ACUtility.CellTypes.LabelSwitch)
                    {
                        SwitchCompat switchButton = convertView.FindViewById<SwitchCompat>(Resource.Id.switchValue);
                        switchButton.Enabled = true;
                    }else if (tItem.CellType == ACUtility.CellTypes.LabelTextEdit)
                    {
                        AcEditText editTextView = convertView.FindViewById<AcEditText>(Resource.Id.value);
                        editTextView.Enabled = true;
                    }else if (tItem.CellType == ACUtility.CellTypes.Button)
                    {
                        Button button = convertView.FindViewById<Button>(Resource.Id.viewEvents);
                        button.Enabled = true;
                        button.SetBackgroundColor(_mContext.Resources.GetColor(Resource.Color.colorPrimary));
                    }
                }
                else {
                    convertView.Enabled = false;
                    if (tItem.CellType == ACUtility.CellTypes.LabelSwitch)
                    {
                        SwitchCompat switchButton = convertView.FindViewById<SwitchCompat>(Resource.Id.switchValue);
                        switchButton.Enabled = false;
                    }else if (tItem.CellType == ACUtility.CellTypes.LabelTextEdit)
                    {
                        AcEditText editTextView = convertView.FindViewById<AcEditText>(Resource.Id.value);
                        editTextView.Enabled = false;
                    }else if (tItem.CellType == ACUtility.CellTypes.Button)
                    {
                        Button button = convertView.FindViewById<Button>(Resource.Id.viewEvents);
                        button.Enabled = false;
                        button.SetBackgroundColor(_mContext.Resources.GetColor(Resource.Color.button_disabled_color));
                    }
                }

              
            }

            View sectionSepeartor = convertView.FindViewById(Resource.Id.item_seperator);

            if (sectionSepeartor != null)
            {
                if (isLastChild)
                {
                    sectionSepeartor.Visibility = ViewStates.Gone;
                }
                else {
                    sectionSepeartor.Visibility = ViewStates.Visible;
                }
            }
           
            return convertView;
        }

        /// <summary>
        /// Gets the children count.
        /// </summary>
        /// <returns>The children count.</returns>
        /// <param name="groupPosition">Group position.</param>
		public int GetChildrenCount(int groupPosition)
        {
            int count = (GetRawGroup(groupPosition) as IEnumerable).Cast<object>().ToList().Count();
            return count;
        }
        /// <summary>
        /// Gets the child identifier.
        /// </summary>
        /// <returns>The child identifier.</returns>
        /// <param name="groupPosition">Group position.</param>
        /// <param name="childPosition">Child position.</param>
		public long GetChildId(int groupPosition, int childPosition)
        {
            return childPosition;
        }

        public Java.Lang.Object GetChild(int groupPosition, int childPosition)
        {
            return null;
        }
        /// <summary>
        /// Gets the positions.
        /// </summary>
        /// <returns>The positions.</returns>
        /// <param name="childItem">Child item.</param>
		public Tuple<int, int> GetPositions(object childItem)
        {
            int groupPosition = 0;

            foreach (var item in _itemsSource)
            {
                int childPosition = MvxEnumerableExtensions.GetPosition((IEnumerable)item, childItem);
                if (childPosition != -1)
                    return new Tuple<int, int>(groupPosition, childPosition);

                groupPosition++;
            }

            return null;
        }

      
        //grid adpater
        private class DaysGridAdapter : MvxAdapter
        {

            public DaysGridAdapter(Context context, IMvxAndroidBindingContext bindingContext) : base(context, bindingContext)
            {
            }
            protected override View GetBindableView(View convertView, object dataContext, int templateId)
            {
                //DayViewItem dayItem = (DayViewItem)dataContext;
                int itemTemplateId = templateId;
                itemTemplateId = Resource.Layout.item_day_button;
                View view = base.GetBindableView(convertView, dataContext, itemTemplateId);
                Button dayButton = (Button)view.FindViewById(Resource.Id.dayButton);
                dayButton.Enabled = true;
               
                return view;
            }


            public override int GetItemViewType(int position)
            {
                return 0;

            }
            public override int ViewTypeCount
            {
                get
                {
                    return 1;
                }
            }
        }

    }
}