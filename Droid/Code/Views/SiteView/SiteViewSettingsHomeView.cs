using Android.OS;
using Android.Views;
using MvvmCross.Binding.Droid.BindingContext;

namespace actchargers.Droid
{
    public class SiteViewSettingsHomeView : BaseFragmentWithOutMenu
    {
        public override View OnCreateView
        (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = this.BindingInflate(Resource.Layout.SiteViewSettingsHomeLayout, null);

            return view;
        }
    }
}
