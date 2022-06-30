using Android.OS;
using Android.Views;
using MvvmCross.Binding.Droid.BindingContext;

namespace actchargers.Droid
{
    public class HistoryView : BaseFragmentWithOutMenu
    {
        View view;
        HistoryViewModel currentViewModel;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            HasOptionsMenu = true;
        }

        public override View OnCreateView
        (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            view = this.BindingInflate(Resource.Layout.HistoryLayout, null);

            currentViewModel = ViewModel as HistoryViewModel;

            return view;
        }
    }
}
