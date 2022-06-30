using Android.OS;
using Android.Views;
using MvvmCross.Binding.Droid.BindingContext;

namespace actchargers.Droid
{
    public class CommissionAReplacementPartView : ListFragment
    {
        private View view;
        private CommissionAReplacementPartViewModel _mCommissionAReplacementPartViewModel;
        private BaseActivity activity;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            HasOptionsMenu = true;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            view = this.BindingInflate(Resource.Layout.BaseListLayout, null);
            activity = (BaseActivity)Activity;
            activity.SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            activity.SupportActionBar.SetDisplayShowHomeEnabled(true);
            _mCommissionAReplacementPartViewModel = ViewModel as CommissionAReplacementPartViewModel;
            //set actionbar title
            activity.UpdateTitle(_mCommissionAReplacementPartViewModel.ViewTitle);
            InitListViewUI(view, _mCommissionAReplacementPartViewModel.ReplacementPartItemSource);
            return view;
        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                //handling backpress of actionbar back here
                case Android.Resource.Id.Home:
                    activity.HideKeybord();
                    // this takes the user 'back', as if they pressed the left-facing triangle icon on the main android toolbar.
                    activity.OnBackPressed();
                    return true;

                default:
                    return base.OnOptionsItemSelected(item);
            }

        }
    }
}
