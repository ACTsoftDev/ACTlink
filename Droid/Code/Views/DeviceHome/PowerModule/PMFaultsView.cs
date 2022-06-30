using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Binding.Droid.Views;

namespace actchargers.Droid
{
    public class PMFaultsView : BaseFragmentWithOutMenu
    {
        private View view;
        private PMFaultsViewModel _pmFaultsViewModel;
        private BaseActivity activity;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            HasOptionsMenu = true;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            view = this.BindingInflate(Resource.Layout.PMFaultsLayout, null);
            activity = (BaseActivity)Activity;
            activity.SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            activity.SupportActionBar.SetDisplayShowHomeEnabled(true);
            _pmFaultsViewModel = ViewModel as PMFaultsViewModel;
            //set actionbar title
            activity.UpdateTitle(_pmFaultsViewModel.ViewTitle);
            var mListView = view.FindViewById<MvxListView>(Resource.Id.pmFaultsListView);
            return view;

        }

        public override void OnResume()
        {
            base.OnResume();

            Activity.RequestedOrientation = OrientationManager.WIDE_ORIENTATION;
        }

        public override void OnPause()
        {
            base.OnPause();

            Activity.RequestedOrientation = OrientationManager.DEFAULT_ORIENTATION;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                //handling backpress of actionbar back here
                case Android.Resource.Id.Home:
                    // this takes the user 'back', as if they pressed the left-facing triangle icon on the main android toolbar.
                    activity.OnBackPressed();
                    return true;

                default:
                    return base.OnOptionsItemSelected(item);
            }

        }


    }
}

