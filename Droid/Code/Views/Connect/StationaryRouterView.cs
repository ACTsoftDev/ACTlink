using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using actchargers.ViewModels;
using Android.App;
using Android.Net.Wifi;
using Android.OS;
using Android.Util;
using Android.Views;
using Android.Widget;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Droid.Support.V4;

namespace actchargers.Droid
{
    public class StationaryRouterView : MvxFragment
    {
        private ConnectViewModel _mConnectViewModel;
        private Button connectButton;
        private BaseActivity activity;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            //base.OnCreateView(inflater, container, savedInstanceState);
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);

            // ConnectViewModel vv = ViewModel as ConnectViewModel;
            base.OnCreateView(inflater, container, savedInstanceState);
            View view = this.BindingInflate(Resource.Layout.StationaryRouterLayout, null);
            activity = (BaseActivity)Activity;
            _mConnectViewModel = (ViewModel as ConnectViewModel);
            //update title
            activity.UpdateTitle(_mConnectViewModel.ConnectButtonTitle);
            connectButton = (Button)view.FindViewById(Resource.Id.connectButton);

            connectButton.Click += async (sender, e) =>
             {
                 try
                 {
                     if (LocationSwitchManager.PrepareLocation(activity))
                         return;

                     bool isWifiConnected = false;
                     bool isWifiisinList = false;
                     WifiConfiguration wifiConfig = new WifiConfiguration();
                     wifiConfig.Ssid = string.Format("\"{0}\"", _mConnectViewModel.SSIDText);
                     wifiConfig.PreSharedKey = string.Format("\"{0}\"", _mConnectViewModel.PasswordText);

                     WifiManager wifiManager = (WifiManager)(Application.Context.GetSystemService(Android.Content.Context.WifiService));
                     var currentConnection = wifiManager.ConnectionInfo;
                     if (wifiManager != null && wifiManager.WifiState == Android.Net.WifiState.Disabled)
                     {
                         ACUserDialogs.ShowAlert(AppResources.enable_wifi_error);
                     }
                     else if (string.IsNullOrEmpty(_mConnectViewModel.SSIDText) || string.IsNullOrEmpty(_mConnectViewModel.PasswordText))
                     {
                         ACUserDialogs.ShowAlert(AppResources.invalid_ssid_password);
                     }
                     else
                     {
                         bool isConnectedtoCorrectWifi = false;
                         if (currentConnection != null && !string.IsNullOrEmpty((currentConnection.SSID)))
                         {
                             if (currentConnection.SSID.Trim('\"') == _mConnectViewModel.SSIDText && Plugin.Connectivity.CrossConnectivity.Current.IsConnected)
                             {
                                 isConnectedtoCorrectWifi = true;
                             }
                         }

                         if (isConnectedtoCorrectWifi)
                         {
                             var d = wifiManager.DhcpInfo;
                             ACConstants.gatewayAddr_Andoid = Android.Text.Format.Formatter.FormatIpAddress(d.Gateway);
                             _mConnectViewModel.ConnectBtnClicked.Execute();
                         }
                         else
                         {
                             ACUserDialogs.ShowProgress();

                             var wifiinList = wifiManager.ConfiguredNetworks.FirstOrDefault(o => o.Ssid == wifiConfig.Ssid);
                             if (wifiinList != null)
                             {
                                 isWifiisinList = true;
                                 if (wifiinList.StatusField == WifiStatus.Current)
                                 {
                                     isWifiConnected = true;
                                 }
                             }

                             if (!isWifiConnected)
                             {
                                 int netId = -1;
                                 if (!isWifiisinList)
                                 {
                                     netId = wifiManager.AddNetwork(wifiConfig);
                                 }
                                 else
                                 {
                                     //check for the internet connection, delete the Network and add again if no internet
                                     if (Plugin.Connectivity.CrossConnectivity.Current.IsConnected)
                                     {
                                         netId = wifiinList.NetworkId;
                                     }
                                     else
                                     {
                                         wifiManager.RemoveNetwork(netId);
                                         netId = wifiManager.AddNetwork(wifiConfig);
                                     }


                                 }
                                 wifiManager.Disconnect();
                                 if (netId != -1)
                                 {
                                     wifiManager.EnableNetwork(netId, true);
                                     wifiManager.Reconnect();
                                     await Task.Delay(10000);
                                 }

                             }
                             var d = wifiManager.DhcpInfo;
                             ACConstants.gatewayAddr_Andoid = Android.Text.Format.Formatter.FormatIpAddress(d.Gateway);
                             _mConnectViewModel.ConnectBtnClicked.Execute();
                             ACUserDialogs.HideProgress();
                         }

                     }
                 }
                 catch (System.Exception ex)
                 {
                     Log.Debug("Wifi", ex.Message);
                     ACUserDialogs.HideProgress();
                 }
             };

            _mConnectViewModel.PropertyChanged += PropertyChanged;




            return view;
        }
        /// <summary>
        /// Properties the changed.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        void PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("ConnectButtonTitle"))
            {
                activity.UpdateTitle(_mConnectViewModel.ConnectButtonTitle);
                if (_mConnectViewModel.ConnectButtonTitle == AppResources.connect)
                {
                    connectButton.SetBackgroundColor(Resources.GetColor(Resource.Color.colorPrimary));
                }
                else if (_mConnectViewModel.ConnectButtonTitle == AppResources.disconnect)
                {
                    connectButton.SetBackgroundColor(Resources.GetColor(Resource.Color.red));
                }
            }
        }

        public string intToIp(int i)
        {

            return (i & 0xFF) + "." +
                        ((i >> 8) & 0xFF) + "." +
                        ((i >> 16) & 0xFF) + "." +
                        ((i >> 24) & 0xFF);
        }
    }
}
