using Android.OS;
using Android.Views;

namespace actchargers.Droid
{
    public class BaseFragmentWithOutMenu : BaseFragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            HasOptionsMenu = true;
        }

        public override View OnCreateView
        (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = base.OnCreateView(inflater, container, savedInstanceState);

            activity.DisableDrawer();

            return view;
        }

        public override void OnDestroyView()
        {
            base.OnDestroyView();

            activity.EnableDrawer();
        }
    }
}
