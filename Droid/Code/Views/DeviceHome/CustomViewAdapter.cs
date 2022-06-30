using System.Collections;
using Android.Content;
using Android.Views;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Binding.Droid.Views;

namespace actchargers.Droid
{
	class CustomViewAdapter : MvxAdapter
	{
		

		public CustomViewAdapter(Context context, IMvxAndroidBindingContext bindingContext, BaseViewModel _baseViewModel, IList itemSource) : base(context, bindingContext)
		{
		}
		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			View view = base.GetView(position, convertView, parent);
			return view;
		}
		protected override View GetBindableView(View convertView, object dataContext, int templateId)
		{
			ListViewItem listItem = (ListViewItem)dataContext;
			if (listItem.CellType == ACUtility.CellTypes.LabelLabel)
				templateId = Resource.Layout.item_label_without_seperator;
			else if (listItem.CellType == ACUtility.CellTypes.TwoLabel)
				templateId = Resource.Layout.item_two_label;
			else if (listItem.CellType == ACUtility.CellTypes.ThreeLabel)
				templateId = Resource.Layout.item_three_label;
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
			if (listItem.CellType == ACUtility.CellTypes.LabelLabel)
				return 0;
			else if (listItem.CellType == ACUtility.CellTypes.TwoLabel)
				return 1;
			else if (listItem.CellType == ACUtility.CellTypes.ThreeLabel)
				return 2;


			return 1;
		}

	}
}
