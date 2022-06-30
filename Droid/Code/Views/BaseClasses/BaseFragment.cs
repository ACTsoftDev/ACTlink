using Android.OS;
using Android.Views;
using MvvmCross.Droid.Support.V4;

namespace actchargers.Droid
{
    public class BaseFragment : MvxFragment
    {
        internal BaseViewModel viewModel;
        internal BaseActivity activity;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            RetainInstance = true;
        }

        public override View OnCreateView
        (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            InitActivity();

            return base.OnCreateView(inflater, container, savedInstanceState);
        }

        void InitActivity()
        {
            viewModel = ViewModel as BaseViewModel;

            activity = (BaseActivity)Activity;
            activity.UpdateTitle(viewModel.ViewTitle);
            activity.SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            activity.SupportActionBar.SetHomeButtonEnabled(true);
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

            viewModel.OnAndroidBackButtonClick();
        }
    }
}
