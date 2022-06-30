using System.Threading.Tasks;
using Foundation;
using SystemConfiguration;

namespace actchargers.iOS
{
    public class WifiManagerService : IWifiManagerService
    {
        public string GetConnectedWifiSSID()         {             NSDictionary dict = new NSDictionary();             var status = CaptiveNetwork.TryCopyCurrentNetworkInfo("en0", out dict);             if (status == StatusCode.NoKey || dict == null)             {                 status = CaptiveNetwork.TryCopyCurrentNetworkInfo("en1", out dict);                 if (status == StatusCode.NoKey || dict == null)                     return "act24mobileLocal";             }              return dict["SSID"].ToString();

            //var bssid = dict[CaptiveNetwork.NetworkInfoKeyBSSID];
            //var ssid = dict[CaptiveNetwork.NetworkInfoKeySSID];
            //var ssiddata = dict[CaptiveNetwork.NetworkInfoKeySSIDData];

        }

        public Task<bool> ConnectToWifiNetwork(string ssid, string password)
        {
            return Task.Run(() => ReturnFalse());
        }

        bool ReturnFalse()
        {
            return false;
        }
    }
}