using Android.OS;
using Android.Views;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Core.ViewModels;

namespace actchargers.Droid
{
    public class SyncSitesView : BaseFragmentWithOutMenu
    {
        private MainContainerView activity;
        private CiExpandableListView _mListView;
        private SyncSitesViewModel _mSyncSitesViewModel;
        private SyncSitesExpandableListAdapter adapter;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            HasOptionsMenu = true;
            ((MvxNotifyPropertyChanged)this.ViewModel).PropertyChanged += OnPropertyChanged;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = this.BindingInflate(Resource.Layout.SyncSite, null);
            activity = (MainContainerView)Activity;
            _mListView = view.FindViewById<CiExpandableListView>(Resource.Id.section_list);
            _mSyncSitesViewModel = ViewModel as SyncSitesViewModel;
            activity.SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            activity.SupportActionBar.SetDisplayShowHomeEnabled(true);
            activity.UpdateTitle(_mSyncSitesViewModel.ViewTitle);
            //Setting adpter
            adapter = new SyncSitesExpandableListAdapter(view.Context, (IMvxAndroidBindingContext)BindingContext, _mSyncSitesViewModel.ListItemSource);
            adapter.ItemsSource = _mSyncSitesViewModel.ListItemSource;
            _mListView.SetAdapter(adapter);
            return view;
        }
        void OnPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("ListItemSource"))
            {
                _mListView.ExpandAllGroups();
                adapter.ItemsSource = _mSyncSitesViewModel.ListItemSource;
                adapter.NotifyDataSetChanged();
            }

        }
        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            //clear activity menu items to add  specific fragment menu items
            menu.Clear();
            inflater.Inflate(Resource.Menu.save, menu);
            base.OnCreateOptionsMenu(menu, inflater);
        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                //handling backpress of actionbar back here
                case Android.Resource.Id.Home:
                    activity.OnBackPressed();
                    return true;

                case Resource.Id.save:
                    _mSyncSitesViewModel.SaveBtnClickCommand.Execute();
                    activity.SupportInvalidateOptionsMenu();
                    return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }

        }
    
    }
}

