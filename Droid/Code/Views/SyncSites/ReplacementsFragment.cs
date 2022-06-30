using Android.OS;
using Android.Runtime;
using Android.Support.V4.View;
using Android.Support.V7.Widget;
using Android.Views;
using MvvmCross.Binding.Droid.BindingContext;


namespace actchargers.Droid
{
	public class ReplacementsFragment : BaseFragmentWithOutMenu
    {
        private ReplacementViewModel currentVM;
        private BaseActivity activity;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            HasOptionsMenu = true;
            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);
            currentVM = ViewModel as ReplacementViewModel;
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = this.BindingInflate(Resource.Layout.ReplacementsFragmentLayout, null);
            activity = (MainContainerView)Activity;

            activity.SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            activity.SupportActionBar.SetDisplayShowHomeEnabled(true);
            activity.UpdateTitle(currentVM.ViewTitle);

            return view;
        }

		public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
		{
			menu.Clear();
			inflater.Inflate(Resource.Menu.replacement_menu, menu);
			var search = menu.FindItem(Resource.Id.search);
			var searchView = MenuItemCompat.GetActionView(search);
			var _searchView = searchView.JavaCast<SearchView>();

			_searchView.QueryTextChange += (s, e) =>
			{
				e.Handled = true;
				currentVM.SearchText = e.NewText;
			};

			_searchView.QueryTextSubmit += (s, e) =>
			{
				e.Handled = true;
				currentVM.SearchText = e.Query;
			};

			base.OnCreateOptionsMenu(menu, inflater);
		}

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                //handling backpress of actionbar back here
                case Android.Resource.Id.Home:
                    // this takes the user 'back', as if they pressed the left-facing triangle icon on the main android toolbar.
                    currentVM.OnBackButtonClick();
                    return true;
				//case Android.Resource.Id.action_search:
				//	// this takes the user 'back', as if they pressed the left-facing triangle icon on the main android toolbar.
				//	currentVM.OnBackButtonClick();
				//	return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }

        }
    }
}
