using Android.Content;
using Android.OS;
using Android.Views;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Binding.Droid.Views;

namespace actchargers.Droid
{
    public class QuickView : BaseFragmentWithOutMenu
	{
		private View view;
		private QuickViewModel _quickViewModel;
		private BaseActivity activity;
		public override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			HasOptionsMenu = true;
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			base.OnCreateView(inflater, container, savedInstanceState);
			view = this.BindingInflate(Resource.Layout.QuickViewLayout, null);
            var mGridView = view.FindViewById<MvxGridView>(Resource.Id.charts_grid);
            activity = (BaseActivity)Activity;
			activity.SupportActionBar.SetDisplayHomeAsUpEnabled(true);
			activity.SupportActionBar.SetDisplayShowHomeEnabled(true);
			_quickViewModel = ViewModel as QuickViewModel;
			//Adapter binding
			//mAdapter = new QuickViewAdapter(this.Activity, (IMvxAndroidBindingContext)BindingContext);
			//set actionbar title
			activity.UpdateTitle(_quickViewModel.ViewTitle);
			//mGridView.Adapter=mAdapter;

			return view;
		}


        public override void OnPause()
        {
            _quickViewModel.ClearTimer();
            base.OnPause();
        }

		public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
		{
			//clear activity menu items to add  specific fragment menu items
			base.OnCreateOptionsMenu(menu, inflater);

		}
		//constructs the options menu on right top of screen.
		public override bool OnOptionsItemSelected(IMenuItem item)
		{
			switch (item.ItemId)
			{
				//handling backpress of actionbar back here
				case Android.Resource.Id.Home:
                    // this takes the user 'back', as if they pressed the left-facing triangle icon on the main android toolbar.
                    _quickViewModel.ClearTimer();
					activity.OnBackPressed();
					return true;

				default:
					return base.OnOptionsItemSelected(item);
			}

		}

		private class QuickViewAdapter : MvxAdapter
		{

			public QuickViewAdapter(Context context, IMvxAndroidBindingContext bindingContext) : base(context, bindingContext)
			{

			}

			protected override View GetBindableView(View convertView, object dataContext, int templateId)
			{
				
				templateId = Resource.Layout.item_chart;

				return base.GetBindableView(convertView, dataContext, templateId);
			}
			public override int ViewTypeCount
			{
				get
				{
					return 1;
				}
			}
			public override int GetItemViewType(int position)
			{
				//ListViewItem listItem = (ListViewItem)GetRawItem(position);
				//if (listItem.CellType == ACUtility.CellTypes.LabelSwitch)
				//	return 0;
				//else if (listItem.CellType == ACUtility.CellTypes.LabelLabel)
				//	return 1;
				//else if (listItem.CellType == ACUtility.CellTypes.LabelTextEdit)
				//	return 2;

				return 1;
			}


		
		}
	}
}
