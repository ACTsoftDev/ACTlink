using Android.OS;
using Android.Views;
using MvvmCross.Binding.Droid.BindingContext;

namespace actchargers.Droid
{
    public class SiteViewInProgressDevicesView : BaseFragmentWithOutMenu
    {
        public override View OnCreateView
        (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            View view = this.BindingInflate(Resource.Layout.SiteViewInProgressDevicesLayout, null);

            return view;
        }
    }
}
