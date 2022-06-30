
using System;
using actchargers.ViewModels;
using Android.App;
using Android.Content;
using Android.Hardware.Usb;
using Android.OS;
using Android.Views;
using Android.Widget;
using MvvmCross.Binding.Droid.BindingContext;
using Com.Ftdi.J2xx;
using Acr.UserDialogs;

namespace actchargers.Droid
{
    public class USBView : BaseFragment
    {
        ConnectViewModel _mConnectViewModel;
        Button usbConnectButton;
        BaseActivity activity;
        View view;
        static String ACTION_USB_PERMISSION = "com.android.example.USB_PERMISSION";
        UsbDevice device;
        UsbManager manager;
        UsbReciever usbReciever;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            view = this.BindingInflate(Resource.Layout.USBLayout, null);
            activity = (BaseActivity)Activity;
            _mConnectViewModel = (ViewModel as ConnectViewModel);
            //update title
            activity.UpdateTitle(_mConnectViewModel.ConnectButtonTitle);
            usbConnectButton = (Button)view.FindViewById(Resource.Id.usbconnectButton);
            D2xxManager ftdid2xx = D2xxManager.GetInstance(activity);
            usbConnectButton.Click += (sender, e) =>
              {
                  ACConstants.ConnectionType = ConnectionTypesEnum.USB;
                  if (sender != null)
                  {
                      manager = (UsbManager)activity.GetSystemService(Context.UsbService);
                      if (manager != null)
                      {
                          var DeviceList = manager.DeviceList;

                          if (DeviceList != null)
                          {
                              foreach (var item in DeviceList)
                              {
                                  device = item.Value;
                              }
                          }
                          else
                          {
                              Toast.MakeText(activity, "Device list is null", ToastLength.Long).Show();
                          }
                          if (device != null)
                          {
                              //UsbCommunication();
                              if (!manager.HasPermission(device))
                              {
                                  // UsbCommunication();
                                  RequestUSBPermission();
                              }
                              else
                              {
                                  UsbCommunication();
                              }
                          }
                          else
                          {
                              UserDialogs.Instance.Alert("Please connect to a Charger or Battery");
                          }
                      }
                      else
                      {

                      }

                  }
                  else
                  {
                      Toast.MakeText(activity, "button click is not proper", ToastLength.Long).Show();
                  }
              };
            return view;
        }

        public void UsbCommunication()
        {
            //Check of FTDI device
            //D2xxManager ftdid2xx = D2xxManager.GetInstance(activity);
            if (manager.HasPermission(device))
            {
                _mConnectViewModel.USBConnectBtnClicked.Execute();
            }
            else
            {
                RequestUSBPermission();
            }

        }

        void RequestUSBPermission()
        {
            //getting permission variables
            usbReciever = new UsbReciever();
            PendingIntent mPermissionIntent = PendingIntent.GetBroadcast(activity, 0, new Intent(
            ACTION_USB_PERMISSION), 0);
            IntentFilter filter = new IntentFilter(ACTION_USB_PERMISSION);
            activity.RegisterReceiver(usbReciever, filter);
            manager.RequestPermission(device, mPermissionIntent);
            usbReciever.UsbCommunicationEvent += delegate
            {
                UsbCommunication();
            };
        }

        class UsbReciever : BroadcastReceiver
        {
            public delegate void UsbCommunicationEventHandler();
            public event UsbCommunicationEventHandler UsbCommunicationEvent;
            public override void OnReceive(Context context, Intent intent)
            {
                String action = intent.Action;
                if (ACTION_USB_PERMISSION.Equals(action))
                {
                    lock (this)
                    {
                        UsbDevice device = (UsbDevice)intent
                                .GetParcelableExtra(UsbManager.ExtraDevice);

                        if (intent.GetBooleanExtra(
                                UsbManager.ExtraPermissionGranted, false))
                        {
                            if (device != null)
                            {
                                UsbCommunicationEvent();

                            }
                        }
                        else
                        {
                            Toast.MakeText(context, "Please grant USB permission to continue further", ToastLength.Long).Show();
                        }
                    }
                }
            }
        }
    }
}
