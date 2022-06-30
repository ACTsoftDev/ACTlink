using System;
using System.Threading.Tasks;
using Android.OS;
using Android.Views;
using Android.Widget;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Core.ViewModels;

namespace actchargers.Droid
{
    public class QuickViewChargerView : BaseFragmentWithOutMenu, ExpandableListView.IOnChildClickListener
    {
        private CiExpandableListView _mListView;
        private QuickViewChargerViewModel _mQuickViewChargerViewModel;
        private CustomSectionEditAdapter adapter;
        private View view;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            HasOptionsMenu = true;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            view = this.BindingInflate(Resource.Layout.QuickViewChargerLayout, null);

            _mListView = view.FindViewById<CiExpandableListView>(Resource.Id.section_list);

            _mQuickViewChargerViewModel = ViewModel as QuickViewChargerViewModel;

            activity.SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            activity.SupportActionBar.SetDisplayShowHomeEnabled(true);
            activity.UpdateTitle(_mQuickViewChargerViewModel.ViewTitle);

            ((MvxNotifyPropertyChanged)ViewModel).PropertyChanged += OnPropertyChanged;

            adapter = new CustomSectionEditAdapter(view.Context, (IMvxAndroidBindingContext)BindingContext, _mQuickViewChargerViewModel.ListItemSource, _mQuickViewChargerViewModel);
            _mListView.SetOnChildClickListener(this);
            _mListView.SetAdapter(adapter);

            return view;
        }

        public override void OnDestroy()
        {
            _mQuickViewChargerViewModel.ClearTimer();

            base.OnDestroy();
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    _mQuickViewChargerViewModel.ClearTimer();

                    activity.OnBackPressed();

                    return true;

                default:
                    return base.OnOptionsItemSelected(item);
            }

        }

        void OnPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("ListItemSource"))
            {
                _mListView.ExpandAllGroups();

                adapter.ItemsSource = _mQuickViewChargerViewModel.ListItemSource;
                adapter.NotifyDataSetChanged();
            }

            if (e.PropertyName.Equals("EditingMode"))
            {
                activity.SupportInvalidateOptionsMenu();
            }
        }

        public bool OnChildClick(ExpandableListView parent, View clickedView, int groupPosition, int childPosition, long id)
        {
            ListViewItem item = _mQuickViewChargerViewModel.ListItemSource[groupPosition][childPosition];

            _mQuickViewChargerViewModel.ExecuteListSelectorCommand(item);

            return true;
        }
    }
}
