using Android.OS;
using Android.Views;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Binding.Droid.Views;

namespace actchargers.Droid
{
    public class CumulativeDataView : BaseFragmentWithOutMenu
    {
        View view;
        CumulativeDataViewModel currentViewModel;

        MvxListView mListView;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            HasOptionsMenu = true;
        }

        public override View OnCreateView
        (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            view = this.BindingInflate(Resource.Layout.CumulativeDataLayout, null);

            mListView = view.FindViewById<MvxListView>(Resource.Id.cumulativeDataViewList);

            currentViewModel = ViewModel as CumulativeDataViewModel;

            var mAdapter = new CustomViewAdapter(Activity, (IMvxAndroidBindingContext)BindingContext, currentViewModel, currentViewModel.CumulativeDataViewItemSource);
            mListView.Adapter = mAdapter;

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
