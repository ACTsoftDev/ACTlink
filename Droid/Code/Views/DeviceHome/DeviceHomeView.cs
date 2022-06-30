using Android.OS;
using Android.Views;
using MvvmCross.Binding.Droid.BindingContext;

namespace actchargers.Droid
{
    public class DeviceHomeView : BaseFragmentWithOutMenu
    {
        DeviceHomeViewModel currentViewModel;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            HasOptionsMenu = true;
        }

        public override View OnCreateView
        (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = this.BindingInflate(Resource.Layout.DeviceHomeLayout, null);

            currentViewModel = ViewModel as DeviceHomeViewModel;

            return view;
        }

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            menu.Clear();

            inflater.Inflate(Resource.Menu.device_home, menu);

            base.OnCreateOptionsMenu(menu, inflater);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    currentViewModel.OnAndroidBackButtonClick();
                    activity.OnBackPressed();

                    return true;

                case Resource.Id.restart:
                    currentViewModel.RestartDevice.Execute();

                    return true;

                case Resource.Id.refresh:
                    currentViewModel.RefreshCommand.Execute(false);

                    return true;

                case Resource.Id.download:
                    currentViewModel.DownloadCommand.Execute();

                    return true;

                default:
                    return base.OnOptionsItemSelected(item);
            }
        }
    }
}
