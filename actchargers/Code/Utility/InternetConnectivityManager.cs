using System.Threading.Tasks;
using MvvmCross.Platform;
using Plugin.Connectivity;
using Plugin.Connectivity.Abstractions;
using System.Linq;
using System;

namespace actchargers
{
    public static class InternetConnectivityManager
    {
        const string TESTURL = "https://www.google.com";

        public static async Task<bool> IsReachableAsync()
        {
            bool isReachable =
                await CrossConnectivity
                    .Current.IsRemoteReachable(TESTURL);

            return isReachable;
        }

        public static bool NetworkCheck(bool scan = false)
        {
            if (ACConstants.ConnectionType == ConnectionTypesEnum.USB)
                return true;

            if (DevelopmentProfileHelper.IsEmulator())
                return true;

            bool check = false;

            if (CrossConnectivity.Current.IsConnected)
            {
                ACUserDialogs.ShowProgress();

                var connectionTypes = CrossConnectivity.Current.ConnectionTypes;

                if (connectionTypes == null)
                {
                    if (!scan)
                        ACUserDialogs.ShowAlert(AppResources.connect_to_wifi);

                    return check;
                }

                if (!connectionTypes.Contains(ConnectionType.WiFi))
                    return check;

                foreach (ConnectionType type in connectionTypes)
                {
                    if (type == ConnectionType.WiFi)
                    {
                        string wifiSSID = Mvx.Resolve<IWifiManagerService>().GetConnectedWifiSSID().Trim('"');

                        if (wifiSSID.Equals(ACConstants.WIFI_SSID) || (wifiSSID.StartsWith("<", StringComparison.CurrentCulture
                            )))
                        {
                            check = true;
                        }
                        else
                        {
                            string msg = string.Format(AppResources.please_connect_to_v_network, ACConstants.WIFI_SSID);

                            ACUserDialogs.ShowAlert(msg);

                            check = false;
                        }
                    }
                }
            }
            else
            {
                ACUserDialogs.ShowAlert(AppResources.network_unavailable);
                check = false;
            }

            ACUserDialogs.HideProgress();

            return check;
        }
    }
}
