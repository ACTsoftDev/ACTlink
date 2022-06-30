using Android.OS;
using Android.Views;
using MvvmCross.Binding.Droid.BindingContext;
namespace actchargers.Droid
{

    public class ChargerView : BaseFragment
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
            // inflate charger view
            view = this.BindingInflate(Resource.Layout.ChargerLayout, null);
            _mConnectToDeviceViewModel = ViewModel as ConnectToDeviceViewModel;
            return view;
        }
    }
}
