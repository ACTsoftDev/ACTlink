using Android.Content.PM;
using Android.OS;
using Android.Views;
using MvvmCross.Binding.Droid.BindingContext;

namespace actchargers.Droid
{
    public class PowerSnapshotsView : BaseFragmentWithOutMenu
    {
        PowerSnapshotsViewModel currentViewModel;

        public override View OnCreateView
        (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            View rootView = this.BindingInflate(Resource.Layout.PowerSnapshotsLayout, null);

            currentViewModel = ViewModel as PowerSnapshotsViewModel;

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
    }
}
