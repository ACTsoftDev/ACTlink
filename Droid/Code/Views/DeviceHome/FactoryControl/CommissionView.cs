using Android.OS;
using Android.Views;
using MvvmCross.Binding.Droid.BindingContext;

namespace actchargers.Droid
{
    public class CommissionView : ListFragment
    {
        private View view;
        private CommissionViewModel _mCommissionViewModel;
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
            _mCommissionViewModel = ViewModel as CommissionViewModel;
            //set actionbar title
            activity.UpdateTitle(_mCommissionViewModel.ViewTitle);
            InitListViewUI(view, _mCommissionViewModel.CommissionItemSource);
            return view;
        }
        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            //clear activity menu items to add  specific fragment menu items
            menu.Clear();
            inflater.Inflate(Resource.Menu.next, menu);
            base.OnCreateOptionsMenu(menu, inflater);

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
                case Resource.Id.next:
                    _mCommissionViewModel.AdminActionsCommissionStep1ButtonCommand.Execute();
                    activity.SupportInvalidateOptionsMenu();
                    return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }

        }
    }
}
