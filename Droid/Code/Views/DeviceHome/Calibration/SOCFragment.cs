using Android.OS;
using Android.Views;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Core.ViewModels;
using MvvmCross.Droid.Support.V4;

namespace actchargers.Droid
{
    public class SOCFragment : MvxFragment
    {
        private CalibrationViewModel _mCalibrationViewModel;
        private View view;
        private CiExpandableListView _mListView;
        private CustomSectionEditAdapter adapter;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);
            base.OnCreateView(inflater, container, savedInstanceState);
            view = this.BindingInflate(Resource.Layout.CalibrationcontentLayout, null);
            _mCalibrationViewModel = ViewModel as CalibrationViewModel;
           _mListView = view.FindViewById<CiExpandableListView>(Resource.Id.section_list);
            adapter = new CustomSectionEditAdapter(view.Context, (IMvxAndroidBindingContext)BindingContext, _mCalibrationViewModel.CalibrationSOCItemSource, _mCalibrationViewModel);
            _mListView.SetAdapter(adapter);
            _mListView.ItemsCanFocus = true;
            ((MvxNotifyPropertyChanged)this.ViewModel).PropertyChanged += OnPropertyChanged;
            return view;
        }
        /// <summary>
        /// Ons the property changed.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        void OnPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("CalibrationSOCItemSource"))
            {
               _mListView.ExpandAllGroups();
                adapter.ItemsSource = _mCalibrationViewModel.CalibrationSOCItemSource;
                adapter.NotifyDataSetChanged();
            }

        }
    }

}
