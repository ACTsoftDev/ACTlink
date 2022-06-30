using Android.OS;
using Android.Views;
using MvvmCross.Binding.Droid.BindingContext;

namespace actchargers.Droid
{
    public class AboutUsView : BaseFragmentWithOutMenu
    {
        AboutUsViewModel currentViewModel;

        public override View OnCreateView
        (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = this.BindingInflate(Resource.Layout.AboutUsLayout, null);

            currentViewModel = ViewModel as AboutUsViewModel;

            return view;
        }
    }
}
