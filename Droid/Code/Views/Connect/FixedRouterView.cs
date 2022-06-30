using System;
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
using MvvmCross.Platform;

namespace actchargers.Droid
{
    public class FixedRouterView : MvxFragment
    {
        ConnectViewModel _mConnectViewModel;
        Button connectButton;
        BaseActivity activity;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            View view = this.BindingInflate(Resource.Layout.FixedRouterLayout, null);
            activity = (BaseActivity)Activity;
            _mConnectViewModel = (ViewModel as ConnectViewModel);
            //update title
            activity.UpdateTitle(_mConnectViewModel.ConnectButtonTitle);
            connectButton = (Button)view.FindViewById(Resource.Id.connectButton);

            connectButton.Click += ConnectButton_Click;

            _mConnectViewModel.PropertyChanged += PropertyChanged;

            return view;
        }

        async void ConnectButton_Click(object sender, EventArgs e)
        {
            try
            {
                await TryConnectButton_Click();
            }
            catch (Exception ex)
            {
                Log.Debug("Wifi", ex.Message);

                ACUserDialogs.HideProgress();
            }
        }

        async Task TryConnectButton_Click()
        {
            if (LocationSwitchManager.PrepareLocation(activity))
                return;

            string ssid = _mConnectViewModel.SSIDText;
            string password = _mConnectViewModel.PasswordText;

            if (DevelopmentProfileHelper.IsEmulator())
            {
                _mConnectViewModel.ConnectBtnClicked.Execute();

                return;
            }

            bool isOk = await Mvx.Resolve<IWifiManagerService>().ConnectToWifiNetwork(ssid, password);

            if (isOk)
                _mConnectViewModel.ConnectBtnClicked.Execute();
        }

        async Task TryConnectButton_Click_save()
        {
            WifiConfiguration wifiConfig = new WifiConfiguration
            {
                Ssid = string.Format("\"{0}\"", _mConnectViewModel.SSIDText),
                PreSharedKey = string.Format("\"{0}\"", _mConnectViewModel.PasswordText)
            };
            WifiManager wifiManager = (WifiManager)(Application.Context.GetSystemService(Android.Content.Context.WifiService));
            var currentConnection = wifiManager.ConnectionInfo;
            if (wifiManager != null && wifiManager.WifiState == Android.Net.WifiState.Disabled)
            {
                ACUserDialogs.ShowAlert(AppResources.enable_wifi_error);

                return;
            }
            if (string.IsNullOrEmpty(_mConnectViewModel.SSIDText) || string.IsNullOrEmpty(_mConnectViewModel.PasswordText))
            {
                ACUserDialogs.ShowAlert(AppResources.invalid_ssid_password);

                return;
            }

            bool isConnectedtoCorrectWifi = false;
            if (currentConnection != null && !string.IsNullOrEmpty((currentConnection.SSID)))
            {
                isConnectedtoCorrectWifi |=
                   (currentConnection.SSID.Trim('\"') == _mConnectViewModel.SSIDText &&
                    Plugin.Connectivity.CrossConnectivity.Current.IsConnected);
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

                //Test
                int netId = -1;
                var wifiList = wifiManager.ConfiguredNetworks.ToList();
                if (wifiList != null)
                {
                    for (int k = 0; k < wifiList.Count; k++)
                    {
                        WifiConfiguration config = wifiList[k];
                        if (config.Ssid == wifiConfig.Ssid)
                        {
                            netId = config.NetworkId;
                            wifiManager.RemoveNetwork(netId);
                        }
                    }
                }
                netId = wifiManager.AddNetwork(wifiConfig);
                wifiManager.Disconnect();
                if (netId != -1)
                {
                    wifiManager.EnableNetwork(netId, true);
                    wifiManager.Reconnect();
                    await Task.Delay(5000);
                }

                var d = wifiManager.DhcpInfo;
                ACConstants.gatewayAddr_Andoid = Android.Text.Format.Formatter.FormatIpAddress(d.Gateway);
                _mConnectViewModel.ConnectBtnClicked.Execute();
                ACUserDialogs.HideProgress();
            }
        }

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
    }
}
