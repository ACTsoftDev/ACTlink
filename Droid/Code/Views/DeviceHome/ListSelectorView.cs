using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Android.Widget;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Binding.Droid.Views;

namespace actchargers.Droid
{
    [Activity(Theme = "@style/AppTheme",Label = "@string/app_name",LaunchMode = LaunchMode.SingleTop, ScreenOrientation = OrientationManager.DEFAULT_ORIENTATION)]
    public class ListSelectorView : BaseActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetDisplayShowHomeEnabled(true);
            var _listSelectorViewModel = ViewModel as ListSelectorViewModel;
            //set actionbar title
            UpdateTitle(_listSelectorViewModel.ViewTitle);
            MvxListView list =FindViewById<MvxListView>(Resource.Id.itemList);
            var adapter = new ListAdapter(this, (IMvxAndroidBindingContext)BindingContext, _listSelectorViewModel.ListItemSource);
            list.Adapter = adapter;
        }
        public override void OnBackPressed()
        {
            base.OnBackPressed();
        }
       
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Android.Resource.Id.Home)
            {
                OnBackPressed();
                return true;
            }
            return base.OnOptionsItemSelected(item);
        }

        protected override int GetLayoutResource()
        {
            return Resource.Layout.list_selector;
        }

        class ListAdapter : MvxAdapter
        {
            List<string> listItemSource;

            public ListAdapter(Context context, IMvxAndroidBindingContext bindingContext, List<string> listItemSource) : base(context, bindingContext)
            {
                this.listItemSource = listItemSource;
            }
            public override View GetView(int position, View convertView, ViewGroup parent)
            {
                View itemView = base.GetView(position, convertView, parent);
                TextView textView = ((TextView)itemView.FindViewById<TextView>(Resource.Id.itemTitle));
                string item = listItemSource[position];
                textView.Text = item;
                return itemView;
            }
            protected override View GetBindableView(View convertView, object dataContext, int templateId)
            {
                templateId = Resource.Layout.list_selector_item;
                return base.GetBindableView(convertView, dataContext, templateId);
            }
        }

    }
}
