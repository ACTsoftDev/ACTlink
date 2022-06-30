using Android.Content.PM;
using Android.OS;
using Android.Views;
using MvvmCross.Binding.Droid.BindingContext;

namespace actchargers.Droid
{
    public class PmLiveView : BaseFragmentWithOutMenu
    {
        PmLiveViewModel viewModel;
        BaseActivity activity;

        public override View OnCreateView
        (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            View rootView = this.BindingInflate(Resource.Layout.PmLiveLayout, null);

            activity = (BaseActivity)Activity;
            activity.SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            activity.SupportActionBar.SetDisplayShowHomeEnabled(true);

            viewModel = ViewModel as PmLiveViewModel;

            activity.UpdateTitle(viewModel.ViewTitle);

            return rootView;
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
                case Android.Resource.Id.Home:
                    activity.OnBackPressed();

                    return true;

                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        public override void OnDestroy()
        {
            base.OnDestroy();

            viewModel.OnBackButtonClickDroid();
        }
    }
}
