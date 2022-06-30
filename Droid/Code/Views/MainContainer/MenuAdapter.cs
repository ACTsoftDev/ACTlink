using System.Collections;
using Android.Content;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Binding.Droid.Views;

namespace actchargers.Droid
{
	public class MenuAdapter : MvxAdapter
	{
		private readonly IList _itemsSource;
		public MenuAdapter(Context context, IMvxAndroidBindingContext bindingContext, IList itemSource)
		   : base(context, bindingContext)
		{
			_itemsSource = itemSource;
		}
		/// <summary>
		/// Gets the view.
		/// </summary>
		/// <returns>The view.</returns>
		/// <param name="position">Position.</param>
		/// <param name="convertView">Convert view.</param>
		/// <param name="parent">Parent.</param>
		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			View itemView = base.GetView(position, convertView, parent);
			TextView textView = itemView.FindViewById<TextView>(Resource.Id.title);
			//LinearLayout menuBackground = ((LinearLayout)itemView.FindViewById(Resource.Id.menu_background));

			var item = _itemsSource[position];
			MenuViewItem itemMenu = item as MenuViewItem;
			if (itemMenu.ShowAsHighlated)
			{
				textView.SetTextColor(Color.ParseColor(ACColors.DARK_BLUE_COLOR));
			}
			else {
				textView.SetTextColor(Color.ParseColor(ACColors.DARK_BLUE_COLOR));
			}
			return itemView;
		}
		/// <summary>
		/// Gets the bindable view.
		/// </summary>
		/// <returns>The bindable view.</returns>
		/// <param name="convertView">Convert view.</param>
		/// <param name="dataContext">Data context.</param>
		/// <param name="templateId">Template identifier.</param>
		protected override View GetBindableView(View convertView, object dataContext, int templateId)
		{
			int itemTemplateId = Resource.Layout.item_menu_notification;
			return base.GetBindableView(convertView, dataContext, itemTemplateId);
		}

	}
}
