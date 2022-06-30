using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;
using Plugin.Connectivity;

namespace actchargers
{
    public class ConnectedDevicesPinger
    {
        const int TRIALS_COUNT = 3;

        public event EventHandler OnDeviceDisconnected;

        bool isAndroid;
        bool isBATTView;

        bool isRuning;
        bool isOperationInProgress;

        bool triedToConnect;

        ConnectionManager connectionManager;

        Dictionary<string, DefineObjectInfo> connectedDevicesDictionary;
        Dictionary<string, int> connectedDevicesCounter;

        public ConnectedDevicesPinger()
        {
            Init();
        }

        public ConnectedDevicesPinger(bool isBATTView)
        {
            this.isBATTView = isBATTView;

            Init();
        }

        void Init()
        {
            isAndroid = CrossDeviceInfoManager.IsAndroid();

            connectionManager = SiteViewQuantum.Instance.GetConnectionManager();

            connectedDevicesDictionary =
                connectionManager.MyCommunicator.GetSerialNumbersWithDefineInfo();
            connectedDevicesCounter = new Dictionary<string, int>();
        }

        public void StopTimer()
        {
            isRuning = false;
        }

        bool CanStartTimer()
        {
            bool canStartTimer =
                (isRuning) &&
                (ControlObject.connectMethods.Connectiontype == ConnectionTypesEnum.ROUTER);

            return canStartTimer;
        }

        #region Active Device

        public async Task StartTimerForActiveDevice()
        {
            isRuning = true;

            if (CanStartTimer())
            {
                await PingDevice();
            }
        }

        async Task PingDevice()
        {
            if (!isRuning)
                return;

            Debug.WriteLine("PingToConnectedDevice " + " # Time -" + DateTime.Now);

            if (CrossConnectivity.Current.IsConnected)
                await PingToConnectedDevice();

            await Task.Delay(ACConstants.PERIODIC_PING_INTERVAL);

            if (isRuning)
                await PingDevice();
        }

        async Task PingToConnectedDevice()
        {
            if (isOperationInProgress)
                return;

            var connectedDevice = GetConnectedDevice();
            string connectedDeviceIP = connectedDevice.IPAddress;
            string serialNumber = connectedDevice.SerialNumber;

            await PingOneDevice(connectedDeviceIP, serialNumber);
        }

        DeviceObjectParent GetConnectedDevice()
        {
            DeviceObjectParent connectedDevice;

            if (isBATTView)
            {
                connectedDevice = connectionManager.activeBattView;
            }
            else
            {
                connectedDevice = connectionManager.activeMCB;
            }

            return connectedDevice;
        }

        #endregion

        #region Connected Devices

        public async Task StartTimerForConnectedDevices()
        {
            isRuning = true;

            if (CanStartTimer())
            {
                await PingConnectedDevices();
            }
        }

        async Task PingConnectedDevices()
        {
            try
            {
                await TryPingConnectedDevices();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        async Task TryPingConnectedDevices()
        {
            if (!isRuning)
                return;

            Debug.WriteLine("TryPingCoccectedDevices " + " # Time -" + DateTime.Now);

            if (CrossConnectivity.Current.IsConnected)
                await IterateAllConnectedDevice();

            await Task.Delay(ACConstants.PERIODIC_PING_INTERVAL);
            if (isRuning)
                await PingConnectedDevices();
        }

        async Task IterateAllConnectedDevice()
        {
            var connectedDevicesSource = GetConnectedDevicesSourceList();

            foreach (string item in connectedDevicesSource)
                await PindOneDeviceBySerialNumber(item);
        }

        List<string> GetConnectedDevicesSourceList()
        {
            var devices =
                connectionManager.MyCommunicator.GetDevicesSerialNumbers().ToList();

            return devices;
        }

        async Task PindOneDeviceBySerialNumber(string serialNumber)
        {
            try
            {
                await TryPindOneDeviceBySerialNumber(serialNumber);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        async Task TryPindOneDeviceBySerialNumber(string serialNumber)
        {
            if (!connectedDevicesDictionary.ContainsKey(serialNumber))
                return;

            var device = connectedDevicesDictionary[serialNumber];

            string ip = device.IpAddress;

            await PingOneDevice(ip, serialNumber);
        }

        #endregion

        #region Device

        async Task PingOneDevice(string ip, string serialNumber)
        {
            try
            {
                await TryPingOneDevice(ip, serialNumber);
            }
            catch (Exception ex)
            {
                isOperationInProgress = false;

                Debug.WriteLine("Ping Error - " + ip
                                + ex.Message + " # Time -" + DateTime.Now);
            }
        }

        async Task TryPingOneDevice(string ip, string serialNumber)
        {
            isOperationInProgress = true;

            bool isReachable = await GetIsReachable(ip);

            isOperationInProgress = false;

            if (isReachable)
            {
                ResetCounter(serialNumber);

                Debug.WriteLine("connected - " + ip + " # Time -" + DateTime.Now);
            }
            else
            {
                await DoNotReachableDevice(ip, serialNumber);
            }
        }

        async Task<bool> GetIsReachable(string ip)
        {
            try
            {
                return await TryGetIsReachable(ip);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("GetIsReachable excption");
                Debug.WriteLine(ex.ToString());

                return false;
            }
        }

        async Task<bool> TryGetIsReachable(string ip)
        {
            if (isAndroid)
                return await GetIsReachableAndroid(ip);
            else
                return await GetIsReachableGeneral(ip);
        }

        async Task<bool> GetIsReachableAndroid(string ip)
        {
            return await
                CrossConnectivity
                    .Current
                    .IsReachable(ip);
        }

        async Task<bool> GetIsReachableGeneral(string ip)
        {
            return await
                CrossConnectivity
                    .Current
                    .IsRemoteReachable
                    (ip, 9308,
                     ACConstants.PERIODIC_PING_TIMEOUT);
        }

        async Task DoNotReachableDevice(string ip, string serialNumber)
        {
            int pingsCounter = IncreaseCounter(serialNumber);

            await ConnectToGlobalAndroidWifiNetworkOnce();

            Debug.WriteLine("Ping Failed - " + ip + " # Counter -" + pingsCounter);

            if (pingsCounter >= TRIALS_COUNT)
            {
                PrepareToDisconnectDevice(ip, serialNumber);
            }
        }

        void PrepareToDisconnectDevice(string ip, string serialNumber)
        {
            DisconnectDevice(ip, serialNumber);

            Debug.WriteLine("Disconnected - " + ip + " # Time -" + DateTime.Now);

            Mvx.Resolve<IMvxMessenger>().Publish(new DeviceResetMessage(this, ip, serialNumber));

            FireOnDeviceDisconnected();
        }

        void DisconnectDevice(string ip, string serialNumber)
        {
            connectionManager.MyCommunicator.RemoveIP(ip);

            connectionManager.siteView.DisconnectDevice(serialNumber);
        }

        #endregion

        #region Android Wifi

        async Task ConnectToGlobalAndroidWifiNetworkOnce()
        {
            if (isAndroid)
                await ConnectToGlobalWifiNetworkOnce();
        }

        async Task ConnectToGlobalWifiNetworkOnce()
        {
            if (!triedToConnect)
                await ConnectToGlobalWifiNetwork();

            triedToConnect = true;
        }

        async Task ConnectToGlobalWifiNetwork()
        {
            string ssid = ACConstants.WIFI_SSID;
            string password = ACConstants.WIFI_PASSWORD;

            await Mvx.Resolve<IWifiManagerService>().ConnectToWifiNetwork(ssid, password);
        }

        #endregion

        void ResetCounter(string serialNumber)
        {
            if (connectedDevicesCounter.ContainsKey(serialNumber))
                connectedDevicesCounter[serialNumber] = 0;
            else
                connectedDevicesCounter.Add(serialNumber, 0);
        }

        int IncreaseCounter(string serialNumber)
        {
            if (connectedDevicesCounter.ContainsKey(serialNumber))
                connectedDevicesCounter[serialNumber]++;
            else
                connectedDevicesCounter.Add(serialNumber, 1);

            return connectedDevicesCounter[serialNumber];
        }

        void FireOnDeviceDisconnected()
        {
            OnDeviceDisconnected?.Invoke(this, EventArgs.Empty);
        }
    }
}
