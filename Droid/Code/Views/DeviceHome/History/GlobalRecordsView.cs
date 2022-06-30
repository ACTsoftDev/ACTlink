using Android.OS;
using Android.Views;
using MvvmCross.Binding.Droid.BindingContext;

namespace actchargers.Droid
{
    public class GlobalRecordsView : BaseFragmentWithOutMenu
    {
        View view;

        ViewGlobalRecordsViewModel currentViewModel;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            HasOptionsMenu = true;
        }

        public override View OnCreateView
        (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            view = this.BindingInflate(Resource.Layout.ViewGlobalRecordsLayout, null);

            currentViewModel = ViewModel as ViewGlobalRecordsViewModel;

            activity.UpdateTitle(currentViewModel.ViewTitle);

            return view;
        }

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            base.OnCreateOptionsMenu(menu, inflater);

            menu.Clear();

            inflater.Inflate(Resource.Menu.reset_menu, menu);

            var resetItem = menu.FindItem(Resource.Id.reset);

            resetItem.SetVisible(currentViewModel.IsResetButtonVisible);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    activity.OnBackPressed();

                    return true;

                case Resource.Id.reset:

                    currentViewModel.ResetBtnClickCommand.Execute();

                    return true;

                default:
                    return base.OnOptionsItemSelected(item);
            }
        }
    }
}
