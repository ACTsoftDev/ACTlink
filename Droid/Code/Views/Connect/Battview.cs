using Android.OS;
using Android.Views;
using MvvmCross.Binding.Droid.BindingContext;

namespace actchargers.Droid.Code.Views.Connect
{

    /// <summary>
    /// Battview.
    /// </summary>
    class Battview : BaseFragment
    {
        private View view;
        /// <summary>
        /// instantiation of viewmodel.
        /// </summary>
       private ConnectToDeviceViewModel _mConnectToDeviceViewModel;
       
		public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {  
            base.OnCreateView(inflater, container, savedInstanceState);
            view = this.BindingInflate(Resource.Layout.BattViewLayout, null);
            _mConnectToDeviceViewModel = ViewModel as ConnectToDeviceViewModel;
             return view;
        }
    }
}