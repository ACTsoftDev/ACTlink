using System;
using System.Collections;
using System.Collections.Specialized;
using System.Linq;
using Android.Content;
using Android.Views;
using Android.Widget;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Binding.Droid.Views;
using MvvmCross.Binding.ExtensionMethods;
using MvvmCross.Platform;

namespace actchargers.Droid
{
    public class RtRecordsSectionAdapter : MvxAdapter, IExpandableListAdapter
    {
        IList _itemsSource;
        private BaseViewModel _viewModel;

        public RtRecordsSectionAdapter(Context context, IMvxAndroidBindingContext bindingContext, IList itemSource, BaseViewModel viewModel)
            : base(context, bindingContext)
        {
            _itemsSource = itemSource;
            this._viewModel = viewModel;
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
            View view = null;
            TableHeaderItem tItem = (TableHeaderItem)item;
            view = base.GetBindableView(convertView, item, GroupTemplateId);
            var headerItem = view.FindViewById(Resource.Id.itemTitle);
            var sectionSeperator = view.FindViewById(Resource.Id.section_seperator_view);
            //hiding section seperator for first element
            if (groupPosition != 0)
            {
                sectionSeperator.Visibility = ViewStates.Visible;
            }
            headerItem.Visibility = ViewStates.Gone;
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
            View view = null;
            ListViewItem tItem = (ListViewItem)item;
            int childTemplateID = Resource.Layout.item_title;
            if (tItem.CellType == ACUtility.CellTypes.LabelLabel)
            {
                childTemplateID = Resource.Layout.item_label;
            }
            else if (tItem.CellType == ACUtility.CellTypes.LabelTextEdit)
            {
                childTemplateID = Resource.Layout.item_edit;
            }
            else if (tItem.CellType == ACUtility.CellTypes.ListSelector)
            {
                childTemplateID = Resource.Layout.item_list;
            }
            else if (tItem.CellType == ACUtility.CellTypes.Button)
            {
                childTemplateID = Resource.Layout.item_button;
            }
            else if (tItem.CellType == ACUtility.CellTypes.Plot)
            {
                childTemplateID = Resource.Layout.item_chart;
            }
            else if (tItem.CellType == ACUtility.CellTypes.RTrecordsPlot)
            {
                childTemplateID = Resource.Layout.item_rtrecords_plots;
            }

            view = base.GetBindableView(convertView, item, childTemplateID);

            View sectionSepeartor = view.FindViewById(Resource.Id.item_seperator);
            //View halfWidthSeperator = view.FindViewById(Resource.Id.item_half_width_seperator);
            //if (halfWidthSeperator != null)
            //{
            //    halfWidthSeperator.Visibility = ViewStates.Gone;
            //}
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


            return view;
        }
        /// <summary>
        /// Gets the children count.
        /// </summary>
        /// <returns>The children count.</returns>
        /// <param name="groupPosition">Group position.</param>
        public int GetChildrenCount(int groupPosition)
        {
            return (GetRawGroup(groupPosition) as IEnumerable).Cast<object>().ToList().Count();
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

    }
}