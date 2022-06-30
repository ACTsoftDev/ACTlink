using System;
using System.Linq;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Net.Wifi;
using Android.Util;

namespace actchargers.Droid
{
    public class WifiSManagerService : IWifiManagerService
    {
        readonly WifiManager wifiManager;

        public WifiSManagerService()
        {
            wifiManager =
                (WifiManager)
                (Application.Context.GetSystemService(Context.WifiService));
        }

        public string GetConnectedWifiSSID()
        {
            if (wifiManager != null)
            {
                string ssid = wifiManager.ConnectionInfo.SSID.Trim('"');

                return ssid;
            }

            return string.Empty;
        }

        public async Task<bool> ConnectToWifiNetwork(string ssid, string password)
        {
            try
            {
                return await TryConnectToWifiNetwork(ssid, password);
            }
            catch (Exception ex)
            {
                Log.Debug("Wifi", ex.Message);
                ACUserDialogs.HideProgress();

                return false;
            }
        }

        async Task<bool> TryConnectToWifiNetwork(string ssid, string password)
        {
            WifiConfiguration wifiConfig = new WifiConfiguration
            {
                Ssid = string.Format("\"{0}\"", ssid),
                PreSharedKey = string.Format("\"{0}\"", password)
            };

            var currentConnection = wifiManager.ConnectionInfo;
            if (wifiManager != null && wifiManager.WifiState == Android.Net.WifiState.Disabled)
            {
                ACUserDialogs.ShowAlert(AppResources.enable_wifi_error);

                return false;
            }
            if (string.IsNullOrEmpty(ssid) || string.IsNullOrEmpty(password))
            {
                ACUserDialogs.ShowAlert(AppResources.invalid_ssid_password);

                return false;
            }

            bool isConnectedtoCorrectWifi = false;
            if (currentConnection != null && !string.IsNullOrEmpty((currentConnection.SSID)))
            {
                isConnectedtoCorrectWifi |=
                    (currentConnection.SSID.Trim('\"') == ssid &&
                    Plugin.Connectivity.CrossConnectivity.Current.IsConnected);
            }

            if (isConnectedtoCorrectWifi)
            {
                var d = wifiManager.DhcpInfo;
                ACConstants.gatewayAddr_Andoid =
                               Android.Text.Format.Formatter.FormatIpAddress(d.Gateway);

                return true;
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

                    await Task.Delay(10000);
                }

                var d = wifiManager.DhcpInfo;
                ACConstants.gatewayAddr_Andoid = Android.Text.Format.Formatter.FormatIpAddress(d.Gateway);
                ACUserDialogs.HideProgress();

                return true;
            }
        }
    }
}
