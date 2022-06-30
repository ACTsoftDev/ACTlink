using Android.OS;
using Android.Views;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Binding.Droid.Views;
using MvvmCross.Core.ViewModels;

namespace actchargers.Droid
{
    public class ExceptionsView : BaseFragment
    {
        private ExceptionsViewModel _mExceptionsViewModel;
        private MvxListView mListView;
        private BaseActivity activity;


        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);
            base.OnCreateView(inflater, container, savedInstanceState);
            View view = this.BindingInflate(Resource.Layout.ExceptionsLayout, null);
            //inflating battery settings listview
            mListView = view.FindViewById<MvxListView>(Resource.Id.exceptionsList);
            ((MvxNotifyPropertyChanged)this.ViewModel).PropertyChanged += OnPropertyChanged;
            //activity reference to set actionbar components
            activity = (BaseActivity)Activity;
            _mExceptionsViewModel = ViewModel as ExceptionsViewModel;
            var mAdapter = new CustomViewAdapter(this.Activity, (IMvxAndroidBindingContext)BindingContext, _mExceptionsViewModel, _mExceptionsViewModel.ExceptionsItemSource);
            mListView.Adapter = mAdapter;
            return view;
        }
        void OnPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("ExceptionsItemSource"))
            {
                mListView.InvalidateViews();

            }
        }
    }
}
