using Android.OS;
using Android.Views;
using MvvmCross.Binding.Droid.BindingContext;

namespace actchargers.Droid
{
    public class ReplacementView : BaseFragment
    {
        private View view;
        /// <summary>
        /// The  m connect to device view model.
        /// </summary>
       private ConnectToDeviceViewModel _mConnectToDeviceViewModel;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            //inflate replacement view
            view = this.BindingInflate(Resource.Layout.ReplacementLayout, null);
            _mConnectToDeviceViewModel = ViewModel as ConnectToDeviceViewModel;
            return view;
        }
    }
}
