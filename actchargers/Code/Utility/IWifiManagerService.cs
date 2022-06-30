using System.Threading.Tasks;

namespace actchargers
{
    public interface IWifiManagerService
    {
        string GetConnectedWifiSSID();

        Task<bool> ConnectToWifiNetwork(string ssid, string password);
    }
}
