using Android.Content;
using Android.Views;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Binding.Droid.Views;

namespace actchargers.Droid
{
    public class CumulativeDataViewAdapter : MvxAdapter
    {
        public CumulativeDataViewAdapter(Context context, IMvxAndroidBindingContext bindingContext)
            : base(context, bindingContext)
        {
        }

        protected override View GetBindableView(View convertView, object dataContext, int templateId)
        {
            ListViewItem listItem = (ListViewItem)dataContext;

            switch (listItem.CellType)
            {
                case ACUtility.CellTypes.LabelLabel:
                    templateId = Resource.Layout.item_label;

                    break;

                case ACUtility.CellTypes.TwoLabel:
                    templateId = Resource.Layout.item_two_label;

                    break;

                case ACUtility.CellTypes.ThreeLabel:
                    templateId = Resource.Layout.item_three_label;

                    break;
            }

            return base.GetBindableView(convertView, dataContext, templateId);
        }

        public override int ViewTypeCount
        {
            get
            {
                return 3;
            }
        }

        public override int GetItemViewType(int position)
        {
            ListViewItem listItem = (ListViewItem)GetRawItem(position);

            switch (listItem.CellType)
            {
                case ACUtility.CellTypes.LabelLabel:
                    return 0;

                case ACUtility.CellTypes.TwoLabel:
                    return 1;

                case ACUtility.CellTypes.ThreeLabel:
                    return 2;
            }

            return 1;
        }
    }
}
