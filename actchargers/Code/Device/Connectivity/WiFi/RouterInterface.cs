using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;
using Newtonsoft.Json;
using Plugin.Connectivity;
using Plugin.Connectivity.Abstractions;
using Plugin.DeviceInfo;
using Sockets.Plugin;

namespace actchargers
{
    public class RouterInterface
    {
        const int STACKED_COMMAND_DELAY = 100;
        const int STACKED_COMMAND_DELAY_ERROR = 3000;

        readonly bool ADD_FIRST_IPS;
        const int FIRST_IPS_MAX = 10;
        readonly bool SHOW_PINGED_IP;

        public event EventHandler OnProgressCompletedChanged;
        public event EventHandler<Tuple<byte, int>> OnLastSucceededIndexChanged;

        bool hasToCancel;

        bool killMeTheSoonest;
        bool stopScanning;
        object snslocker;
        object varsLock;
        string[] addressList = new string[3];

        TcpSocketClient stackedSocketClient;

        Dictionary<string, RouterInterfaceDevice> tempDevices;

        int _progressMax;
        public int ProgressMax
        {
            get
            {
                return _progressMax;
            }
            set
            {
                _progressMax = value;
            }
        }

        int progressCompleted;
        public int ProgressCompleted
        {
            get
            {
                return progressCompleted;
            }
            set
            {
                progressCompleted = value;

                FireOnProgressCompletedChanged();
            }
        }

        void FireOnProgressCompletedChanged()
        {
            OnProgressCompletedChanged?.Invoke(this, EventArgs.Empty);
        }

        int GetProgressCompleted(int progress, int max)
        {
            if (max == 0)
                return 0;
            if (progress == 0)
                return 0;

            return (int)(progress / (double)max * 100.0);
        }

        int generalProgressCompleted;
        public int GeneralProgressCompleted
        {
            get
            {
                return generalProgressCompleted;
            }
            set
            {
                generalProgressCompleted = value;
            }
        }

        void FireOnLastSucceededIndexChanged(byte commandByte, int index)
        {
            OnLastSucceededIndexChanged?.Invoke(this, new Tuple<byte, int>(commandByte, index));
        }

        List<DefineResult> defineList;

        Plugin.DeviceInfo.Abstractions.Platform DevicePlatform;
        List<string> ExistingScannedIP = new List<string>();
        List<string> ignoreList;

        public CancellationToken TCPClientCancellationToken { get; set; }

        public CancellationToken ScanningCancellationToken { get; set; }

        Dictionary<string, RouterInterfaceDevice> connectedDevices;

        int _doScanCount;
        int DoScanCount
        {
            get
            {
                lock (varsLock)
                {
                    return _doScanCount;
                }
            }
            set
            {
                lock (varsLock)
                {
                    _doScanCount = value;
                }
            }
        }

        #region Scan

        public async Task DoScan(object ipList)
        {
            List<string> newIps = new List<string>();

            tempDevices = new Dictionary<string, RouterInterfaceDevice>();

            if (ipList == null)
            {
                ResetProgress();

                try
                {
                    CrossConnectivity.Current.ConnectivityChanged -= Current_ConnectivityChanged;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
                CrossConnectivity.Current.ConnectivityChanged += Current_ConnectivityChanged;

                //Checking end
                int totalExpectedConnections = 0;
                Dictionary<string, string> ipSn = new Dictionary<string, string>();

                lock (snslocker)
                {
                    totalExpectedConnections = connectedDevices.Keys.Count;

                    foreach (string sn in connectedDevices.Keys)
                        ipSn.Add(sn, connectedDevices[sn].ipAddress);
                }

                foreach (string s in ipSn.Keys)
                {
                    RouterInterfaceDevice r;
                    lock (snslocker)
                    {
                        if (connectedDevices.ContainsKey(s))
                            r = connectedDevices[s];
                        else continue;
                    }
                    //force clean up
                    if ((r.isDead || r.softKill) && r.lockME(false))
                    {
                        if (ControlObject.isDebugMaster)
                            Logger.AddLog(false, "Removed(55)-" + r.ipAddress + "-:" + s);

                        lock (snslocker)
                            connectedDevices.Remove(s);

                        r.unlockMe();
                    }
                }

                newIps = new List<string>();

#if DEBUG
                ACConstants.IS_DEBUGGING = false;
#endif

                if (ACConstants.IS_DEBUGGING)
                {
                    newIps = new List<string>();

                    newIps = JsonConvert.DeserializeObject<List<string>>("[\"192.168.33.53\",\"192.168.33.118\"]");
                    var tempIps = newIps.Where(o => !ExistingScannedIP.Contains(o)).ToList();
                    InitialiseProgressBar(0, tempIps.Count);
                }
                else
                {
                    newIps = await DoPing(new List<string>(ipSn.Values));//fetch all connected IPs (except the ones we have)
                }

                DoScanCount++;

                totalExpectedConnections += newIps.Count;
            }
            else
            {
                List<DBConnectedDevices> connectedIPs = (List<DBConnectedDevices>)ipList;

                newIps = connectedIPs.Select(o => o.IPAddress).ToList();

                //filter list of stored ips based on connected statte

                List<string> connctedIps = new List<string>();
                try
                {
                    foreach (var item in newIps)
                    {
                        if (!ExistingScannedIP.Contains(item))
                        {
                            Tuple<string, bool> result = await Task.Run(async () =>
                            {
                                return await PingConnctedDeviceAsync(item);
                            }, ScanningCancellationToken);
                            if (result.Item2)
                            {
                                connctedIps.Add(item);
                            }
                            else
                            {
                                if (ExistingScannedIP.Contains(item))
                                {
                                    ExistingScannedIP.Remove(item);
                                }

                                DbSingleton
                                    .DBManagerServiceInstance
                                    .GetDBConnectedDevicesLoader()
                                    .DeleteUsingId(item);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }

                defineList = new List<DefineResult>();
                try
                {
                    foreach (var item in connctedIps)
                    {
                        if (!ExistingScannedIP.Contains(item))
                        {

                            defineList.Add(await Task.Run(async () =>
                            {
                                return await DoDefineAsync(item);
                            }, ScanningCancellationToken));
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }


                foreach (var defineResult in defineList)
                {
                    if (!defineResult.validRes)
                    {
                        //Remove from DB if the result is false
                        if (ExistingScannedIP.Contains(defineResult.ip))
                        {
                            ExistingScannedIP.Remove(defineResult.ip);
                        }

                        DbSingleton
                            .DBManagerServiceInstance
                            .GetDBConnectedDevicesLoader()
                            .DeleteUsingId(defineResult.ip);

                        if (ControlObject.isDebugMaster)
                            Logger.AddLog(false, "(" + defineResult.ip + "):NOT ACT Device! s:" + defineResult.res.ToString() + " " + defineResult.validResNote.ToString());

                        continue;
                    }

                    if (killMeTheSoonest)
                        break;

                    if ((!allowMCB && defineResult.isCharger) || (!allowBattView && !defineResult.isCharger) || (defineResult.isCharger && !ControlObject.filterMCBIn) || (!defineResult.isCharger && !ControlObject.filterBattViewIn))
                    {
                        if (ControlObject.isDebugMaster)
                            Logger.AddLog(false, "NO Access AmIcharger: " + defineResult.isCharger.ToString());

                        lock (varsLock)
                            ignoreList.Add(defineResult.ip);

                        continue;
                    }

                    if (!string.IsNullOrEmpty(defineResult.deviceName))
                    {
                        RouterInterfaceDevice routerInterfaceDevice = new RouterInterfaceDevice(defineResult.ip, defineResult.isCharger, defineResult.id, defineResult.deviceSerialNumber, defineResult.lostRTC, defineResult.zoneID, defineResult.firmwareVersion, defineResult.firmwareWiFiVersion, defineResult.studyID, defineResult.name, defineResult.DcId, defineResult.FirmwareDcVersion, defineResult.replacementPart, defineResult.deviceType);

                        if (!connectedDevices.ContainsKey(defineResult.deviceName))
                        {
                            tempDevices.Add(defineResult.deviceName, routerInterfaceDevice);
                            Debug.WriteLine("DoScan - New Device Found - " + defineResult.deviceName);
                            ExistingScannedIP.Add(defineResult.ip);
                        }
                    }
                }

                lock (snslocker)
                {
                    foreach (string s in tempDevices.Keys)
                    {
                        if (connectedDevices.ContainsKey(s))
                            continue;
                        connectedDevices.Add(s, tempDevices[s]);
                        if (ControlObject.isDebugMaster)
                        {
                            Logger.AddLog(false, "Added-" + tempDevices[s].ipAddress + "-:" + s);
                        }
                    }
                }
            }
        }

        async Task<List<string>> DoPing(List<string> exceptionIPs)
        {
            List<string> newIps = new List<string>();

            try
            {
                if (string.IsNullOrEmpty(addressList[0]))
                    addressList = await GetMyIpaddress();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }

            string myAddress = addressList[0];
            string gateWay = addressList[1];

            if (string.IsNullOrEmpty(gateWay))
                gateWay = myAddress;

            string netmask = GetSubnetMask(myAddress, addressList[2]);

            if (myAddress == "" || gateWay == "" || netmask == "")
                return newIps;

            int ipToScanCount = (int)GetMaxIpsCountFromNetMask(netmask);

            if (ipToScanCount > 1024)
                ipToScanCount = 1024;

            List<int> ipsByIndex = GetIpsByIndex(ipToScanCount);

            return await DoPingAll(exceptionIPs, myAddress, gateWay, netmask, ipToScanCount, ipsByIndex);
        }

        async Task<List<string>> DoPingAll
        (List<string> exceptionIPs, string myAddress, string gateWay,
         string netmask, int ipToScanCount, List<int> ipsByIndex)
        {
            List<string> newIps = new List<string>();

            List<string> ignoreListTemp = new List<string>();

            InitialiseProgressBar(0, ipToScanCount);

            try
            {
                await TryDoPingAllIps(exceptionIPs, myAddress, gateWay, netmask, ipsByIndex);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return newIps;
        }

        async Task TryDoPingAllIps
        (List<string> exceptionIPs, string myAddress, string gateWay, string netmask, List<int> ipsByIndex)
        {
            await Task.Run(() => TryDoPingAllIpsTask(exceptionIPs, myAddress, gateWay, netmask, ipsByIndex));
        }

        void TryDoPingAllIpsTask
        (List<string> exceptionIPs, string myAddress, string gateWay, string netmask, List<int> ipsByIndex)
        {
            var allTasks = new List<Task>();

            foreach (int ipByIndex in ipsByIndex)
            {
                var item =
                    Task
                        .Factory
                        .StartNew(
                            () =>
                            PinOneIp(exceptionIPs, myAddress, gateWay, netmask, ipByIndex));

                allTasks.Add(item);
            }

            Task.WhenAll(allTasks).Wait(ScanningCancellationToken);
        }

        async Task<bool> PinOneIp
        (List<string> exceptionIPs, string myAddress, string gateWay, string netmask, int ipByIndex)
        {
            string tempAddress = GetFormattedAdress(gateWay, netmask, ipByIndex);

            if (string.IsNullOrEmpty(tempAddress) || tempAddress == myAddress || tempAddress == gateWay || exceptionIPs.Contains(tempAddress) || ExistingScannedIP.Contains(tempAddress))
                return true;

            if (stopScanning)
            {
                ++GeneralProgressCompleted;

                return true;
            }

            try
            {
                return await TryPingOneIp(tempAddress);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);

                return false;
            }
        }

        async Task<bool> TryPingOneIp(string tempAddress)
        {
            var result = await PingAsync(tempAddress);

            if (result.Item2)
                return await PingRepliedIp(result.Item1);

            return true;
        }

        async Task<bool> PingRepliedIp(string ip)
        {
            ++ProgressMax;
            //Call the define Async Method
            var defineResult = await DoDefineAsync(ip);

            if (!defineResult.validRes)
            {
                //Remove from DB if the result is false
                if (ExistingScannedIP.Contains(defineResult.ip))
                    ExistingScannedIP.Remove(defineResult.ip);

                DbSingleton.DBManagerServiceInstance
                           .GetDBConnectedDevicesLoader()
                           .DeleteUsingId(defineResult.ip);

                if (ControlObject.isDebugMaster)
                    Logger.AddLog(false, "(" + defineResult.ip + "):NOT ACT Device! s:" + defineResult.res.ToString() + " " + defineResult.validResNote.ToString());

                return true;
            }

            if (defineResult.validRes)
            {
                if (killMeTheSoonest)
                    return false;

                if ((!allowMCB && defineResult.isCharger) || (!allowBattView && !defineResult.isCharger) || (defineResult.isCharger && !ControlObject.filterMCBIn) || (!defineResult.isCharger && !ControlObject.filterBattViewIn))
                {
                    if (ControlObject.isDebugMaster)
                        Logger.AddLog(false, "NO Access AmIcharger: " + defineResult.isCharger.ToString());

                    lock (varsLock)
                        ignoreList.Add(defineResult.ip);

                    return true;
                }

                // Valid device
                if (!string.IsNullOrEmpty(defineResult.deviceName))
                {
                    RouterInterfaceDevice routerInterfaceDevice = new RouterInterfaceDevice(defineResult.ip, defineResult.isCharger, defineResult.id, defineResult.deviceSerialNumber, defineResult.lostRTC, defineResult.zoneID, defineResult.firmwareVersion, defineResult.firmwareWiFiVersion, defineResult.studyID, defineResult.name, defineResult.DcId, defineResult.FirmwareDcVersion, defineResult.replacementPart, defineResult.deviceType);

                    if (!connectedDevices.ContainsKey(defineResult.deviceName))
                    {
                        tempDevices.Add(defineResult.deviceName, routerInterfaceDevice);

                        Debug.WriteLine("New Device Found - " + defineResult.deviceName);

                        ExistingScannedIP.Add(defineResult.ip);
                    }
                }

                lock (snslocker)
                {
                    foreach (string s in tempDevices.Keys)
                    {
                        if (connectedDevices.ContainsKey(s))
                            continue;

                        connectedDevices.Add(s, tempDevices[s]);

                        if (ControlObject.isDebugMaster)
                            Logger.AddLog(false, "Added-" + tempDevices[s].ipAddress + "-:" + s);
                    }
                }

                Mvx.Resolve<IMvxMessenger>().Publish(new AddDeviceMessage(this));
            }

            return true;
        }

        string GetSubnetMask(string myAddress, string broadcast)
        {
            string subnetMaskAdd = string.Empty;

            if (string.IsNullOrEmpty(broadcast))
                return subnetMaskAdd;

            string[] ioArray = myAddress.Split('.');
            string[] broadcastArr = broadcast.Split('.');
            byte[] bytes = new byte[4];

            bytes[0] += (byte)(byte.Parse(ioArray[0]) | ~byte.Parse(broadcastArr[0]));
            bytes[1] += (byte)(byte.Parse(ioArray[1]) | ~byte.Parse(broadcastArr[1]));
            bytes[2] += (byte)(byte.Parse(ioArray[2]) | ~byte.Parse(broadcastArr[2]));
            bytes[3] += byte.Parse("0");

            subnetMaskAdd = bytes[0].ToString() + "." + bytes[1].ToString() + "." + bytes[2].ToString() + "." + bytes[3].ToString();

            return subnetMaskAdd;
        }

        public UInt32 GetMaxIpsCountFromNetMask(string netmask)
        {
            string[] mask = netmask.Split('.');
            if (mask.Length != 4)
            {
                return 0;
            }
            UInt32 c = 16777216 * (UInt32)byte.Parse(mask[0]);
            c += 65536 * (UInt32)byte.Parse(mask[1]);
            c += 256 * (UInt32)byte.Parse(mask[2]);
            c += 1 * (UInt32)byte.Parse(mask[3]);

            c = ~c;

            return c;
        }

        List<int> GetIpsByIndex(int ipToScanCount)
        {
            List<int> ipsByIndex = new List<int>();

            if (ADD_FIRST_IPS)
                for (int i = 0; i <= FIRST_IPS_MAX; i++)
                    ipsByIndex.Add(i);

            List<int> oneBulk = new List<int>();
            for (int i = 0; i < ipToScanCount; i++)
                oneBulk.Add(i);

            ipsByIndex.AddRange(oneBulk);
            ipsByIndex.AddRange(oneBulk);

            return ipsByIndex;
        }

        public string GetFormattedAdress(string gateway, string netmask, int id)
        {
            string[] add = gateway.Split('.');
            string[] mask = netmask.Split('.');
            byte[] bytes = new byte[4];

            bytes[0] += (byte)(byte.Parse(add[0]) & byte.Parse(mask[0]));
            bytes[1] += (byte)(byte.Parse(add[1]) & byte.Parse(mask[1]));
            bytes[2] += (byte)(byte.Parse(add[2]) & byte.Parse(mask[2]));
            bytes[3] += (byte)(byte.Parse(add[3]) & byte.Parse(mask[3]));

            if (id > 0)
            {
                bytes[3] |= (byte)id;
                id &= ~255;
            }
            if (id > 0)
            {
                int temp = id >> 8;
                bytes[2] |= (byte)temp;
                id &= ~65535;
            }
            if (id > 0)
            {
                int temp = id >> 16;
                bytes[1] |= (byte)temp;
                id &= ~16777215;
            }
            if (id > 0)
            {
                int temp = id >> 24;
                bytes[0] |= (byte)temp;
            }

            return bytes[0].ToString() + "." + bytes[1].ToString() + "." + bytes[2].ToString() + "." + bytes[3].ToString();
        }

        #endregion

        internal void RemoveIP(string ip)
        {
            if (ExistingScannedIP.Contains(ip))
            {
                ExistingScannedIP.Remove(ip);
            }

            string keytoRemove = string.Empty;

            foreach (var key in connectedDevices.Keys)
            {
                if (connectedDevices[key].ipAddress == ip)
                {
                    keytoRemove = key;
                }
            }

            if (!string.IsNullOrEmpty(keytoRemove))
            {
                connectedDevices.Remove(keytoRemove);
            }

            DbSingleton
                .DBManagerServiceInstance
                .GetDBConnectedDevicesLoader()
                .DeleteUsingId(ip);
        }

        internal void InitialiseScanningToken()
        {
            ScanningCancellationToken = new CancellationToken();
            stopScanning = false;
            //Wiring the Network Changed Event
            try
            {
                CrossConnectivity.Current.ConnectivityChanged -= Current_ConnectivityChanged;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            CrossConnectivity.Current.ConnectivityChanged += Current_ConnectivityChanged;
        }

        internal void InitialiseTCPToken()
        {
            TCPClientCancellationToken = new CancellationToken();
            killMeTheSoonest = false;
        }

        internal void Close()
        {
            try
            {
                defineList = new List<DefineResult>();
                ResetProgress();
                ScanningCancellationToken = new CancellationToken(true);

                CrossConnectivity.Current.ConnectivityChanged -= Current_ConnectivityChanged;
                ScanningCancellationToken.ThrowIfCancellationRequested();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

        }

        public string[] GetSerialNumbers()
        {
            List<string> keys = new List<string>();
            foreach (var entry in connectedDevices)
            {
                if (entry.Value.softKill || entry.Value.isDead)
                    continue;
                keys.Add(entry.Key);
            }

            return keys.ToArray();
        }

        public Dictionary<string, DefineObjectInfo> GetSerialNumbersWithDefineInfo()
        {
            Dictionary<string, DefineObjectInfo> temp = new Dictionary<string, DefineObjectInfo>();
            lock (snslocker)
            {
                foreach (var entry in connectedDevices)
                {
                    if (entry.Value.softKill || entry.Value.isDead)
                        continue;
                    temp.Add(entry.Key, entry.Value.def);
                }
            }
            return temp;
        }

        void InitialiseProgressBar(int mProgressCompleted, int progressMax)
        {
            ProgressMax = progressMax;
            GeneralProgressCompleted = mProgressCompleted;
        }

        public async Task<string[]> GetMyIpaddress()
        {
            if (DevelopmentProfileHelper.IsEmulator())
                return new string[] { "192.168.0.3", "192.168.0.1", "255.255.255.0" };

            string localIP = "";
            string gatewayIP = "";
            string broadcastIP = "";

            try
            {
                //Get all the connected Networks to the Device
                var allInterfaces = await CommsInterface.GetAllInterfacesAsync();

                //Iterate through all connections
                foreach (var item in allInterfaces)
                {
                    if (!string.IsNullOrEmpty(item.Name) && !string.IsNullOrEmpty(item.NativeInterfaceId))
                    {
                        //Check whether the Connection is Wifi through the Name
                        if (item.Name.Equals("wlan0") || item.Name.Equals("en0") || item.Name.Equals("en1")) // uncomment while testing in iOS simulator 
                        {
                            if (!string.IsNullOrEmpty(item.IpAddress))
                            {
                                localIP = item.IpAddress;
                                if (!string.IsNullOrEmpty(item.GatewayAddress))
                                {
                                    gatewayIP = item.GatewayAddress;
                                }
                                if (!string.IsNullOrEmpty(item.BroadcastAddress))
                                {
                                    broadcastIP = item.BroadcastAddress;
                                }
                                else if (!string.IsNullOrEmpty(ACConstants.gatewayAddr_Andoid))
                                {
                                    broadcastIP = ACConstants.gatewayAddr_Andoid;
                                }

                            }
                        }
                    }
                }
                if (gatewayIP == null)
                {
                    gatewayIP = "";
                }
                return new string[] { localIP, gatewayIP, broadcastIP };
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());

                return new string[] { localIP, gatewayIP, broadcastIP };
            }
        }

        string GetMaskAddress(string ip)
        {
            string[] add = ip.Split('.');
            if (add.Length == 4)
                return add[0] + "." + add[1] + "." + add[2] + ".";

            return "";
        }

        bool allowBattView;
        bool allowMCB;

        public RouterInterface(bool allowBattView, bool allowMCB)
        {
            this.allowBattView = allowBattView;
            this.allowMCB = allowMCB;
            varsLock = new object();
            snslocker = new object();
            killMeTheSoonest = false;
            connectedDevices = new Dictionary<string, RouterInterfaceDevice>();
            ignoreList = new List<string>();
            DevicePlatform = CrossDeviceInfo.Current.Platform;

            ADD_FIRST_IPS = false;
            SHOW_PINGED_IP = false;
        }

        public void ResetProgress()
        {
            GeneralProgressCompleted = 0;
            ProgressMax = 0;
        }

        void Current_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            if (e.IsConnected)
            {
                Mvx.Resolve<IMvxMessenger>().Publish(new StartScanMessage(this));
            }
            else if (!ScanningCancellationToken.IsCancellationRequested)
            {
                defineList = new List<DefineResult>();
                ResetProgress();
                ScanningCancellationToken = new CancellationToken(true);

                if (ACConstants.ConnectionType != ConnectionTypesEnum.ROUTER)
                {
                    ACUserDialogs.ShowAlert("Connection lost");
                }
            }
        }

        public async Task<Tuple<string, bool>> PingConnctedDeviceAsync(string address)
        {
            bool IsReachable = false;
            int Counter = 0;
            if (!stopScanning)
            {
                try
                {
                    while (Counter < 3)
                    {
                        if (DevicePlatform == Plugin.DeviceInfo.Abstractions.Platform.Android)
                        {

                            IsReachable = await Task.Run(async () =>
                            {
                                return await CrossConnectivity.Current.IsReachable(address, 5000);
                            });
                        }
                        else
                        {
                            IsReachable = await Task.Run(async () =>
                            {
                                return await CrossConnectivity.Current.IsRemoteReachable(address, 9308, 5000);
                            });
                        }
                        if (!IsReachable)
                        {
                            Counter++;
                        }
                        else
                        {
                            break;
                        }
                    }

                    Debug.WriteLine("Ping Completed - " + address);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Ping Error - " + address + ex.Message);
                }
            }
            else
            {
                CrossConnectivity.Current.Dispose();
            }

            return new Tuple<string, bool>(address, IsReachable);
        }

        async Task<Tuple<string, bool>> PingAsync(string address)
        {
            bool IsReachable = false;

            if (ScanningCancellationToken.IsCancellationRequested)
                return new Tuple<string, bool>(address, IsReachable);

            if (stopScanning)
            {
                CrossConnectivity.Current.Dispose();

                return new Tuple<string, bool>(address, IsReachable);
            }

            try
            {
                if (DevicePlatform == Plugin.DeviceInfo.Abstractions.Platform.Android)
                {
                    IsReachable = await Task.Run(async () =>
                    {
                        return await CrossConnectivity.Current.IsReachable(address, ACConstants.PING_ASYNC_TIMEOUT);
                    }, ScanningCancellationToken);

                }
                else
                {
                    IsReachable = await Task.Run(async () =>
                    {
                        return await CrossConnectivity.Current.IsRemoteReachable(address, 9308, ACConstants.PING_ASYNC_TIMEOUT);
                    }, ScanningCancellationToken);

                }

                if (SHOW_PINGED_IP)
                    Debug.WriteLine("Ping Completed - " + address);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Ping Error - " + address + " " + ex.Message);
            }

            ++GeneralProgressCompleted;

            return new Tuple<string, bool>(address, IsReachable);
        }

        async internal Task<DefineResult> DoDefineAsync(string ip)
        {
            DefineResult defineResult = new DefineResult(ip);
            byte[] resultArray = new byte[200];
            CommunicationResult status = CommunicationResult.internalFailure;

            if (!stopScanning)
            {
                try
                {
                    var tuple = await SendRecieveInternal(ip, CommProtocol.defineCommand, null, 0, false, TimeoutLevel.normal);

                    defineResult.res = status = tuple.Item1;
                    resultArray = tuple.Item2;
                    if (status != CommunicationResult.OK || (resultArray[0] != CommProtocol.chargerDefineKey && resultArray[0] != CommProtocol.battViewDefineKey && resultArray[0] != CommProtocol.CalibratorDefineKey))
                        defineResult.validRes = false;
                    else
                    {
                        defineResult.validRes = true;
                        defineResult.validResNote = resultArray[0];
                        defineResult.isCharger = resultArray[0] == CommProtocol.chargerDefineKey || resultArray[0] == CommProtocol.CalibratorDefineKey;

                        if (defineResult.isCharger)
                        {
                            if (resultArray[0] == CommProtocol.chargerDefineKey)
                                defineResult.deviceType = DeviceBaseType.MCB;
                            else if (resultArray[0] == CommProtocol.CalibratorDefineKey)
                                defineResult.deviceType = DeviceBaseType.CALIBRATOR;

                            defineResult.deviceSerialNumber = Encoding.UTF8.GetString(resultArray, 1, 13).TrimEnd('\0');
                            defineResult.deviceName = "CHRG_" + BitConverter.ToUInt32(resultArray, 19) + ":" + defineResult.deviceSerialNumber;
                            defineResult.id = BitConverter.ToUInt32(resultArray, 19);
                            defineResult.lostRTC = (resultArray[39] == 1);
                            defineResult.zoneID = resultArray[44];

                            defineResult.firmwareVersion = ((10 * (resultArray[27] & 0xF0) >> 4) + (resultArray[27] & 0x0F)) + (((10 * (resultArray[28] & 0xF0) >> 4) + (resultArray[28] & 0x0F)) / 100.0f);
                            if (defineResult.firmwareVersion >= 2.09f)
                            {
                                defineResult.replacementPart = resultArray[52] != 0;

                                defineResult.name = Encoding.UTF8.GetString(resultArray, 53, 24).TrimEnd('\0');
                            }

                            defineResult.firmwareWiFiVersion = FirmwareWiFiVersionUtility.GetFirmwareWiFiVersion(defineResult.firmwareVersion, resultArray);

                            int loc = 87;
                            if (defineResult.firmwareVersion > 2.87f && resultArray.Length >= 160)
                            {
                                defineResult.DcId = resultArray[loc++];
                                defineResult.FirmwareDcVersion = BitConverter.ToSingle(resultArray, loc);
                                loc += 4;
                            }
                        }
                        else
                        {
                            defineResult.deviceType = DeviceBaseType.BATTVIEW;
                            defineResult.deviceSerialNumber = "3" + Encoding.UTF8.GetString(resultArray, 1, 12).TrimEnd('\0');
                            defineResult.deviceName = "BATT_" + BitConverter.ToUInt32(resultArray, 15) + ":" + defineResult.deviceSerialNumber;
                            defineResult.id = BitConverter.ToUInt32(resultArray, 15);
                            defineResult.lostRTC = (resultArray[39] == 1);
                            defineResult.zoneID = resultArray[40];
                            defineResult.firmwareVersion = ((10 * (resultArray[31] & 0xF0) >> 4) + (resultArray[31] & 0x0F)) + (((10 * (resultArray[32] & 0xF0) >> 4) + (resultArray[32] & 0x0F)) / 100.0f);

                            defineResult.studyID = BitConverter.ToUInt32(resultArray, 41);

                            if (defineResult.firmwareVersion >= 2.07f)
                            {
                                defineResult.replacementPart = resultArray[45] != 0;

                                defineResult.name = Encoding.UTF8.GetString(resultArray, 46, 16).TrimEnd('\0');
                            }

                            defineResult.firmwareWiFiVersion = FirmwareWiFiVersionUtility.GetFirmwareWiFiVersion(defineResult.firmwareVersion, resultArray);
                        }
                    }

                    ++GeneralProgressCompleted;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    ++GeneralProgressCompleted;
                }
            }

            return defineResult;
        }

        #region SendRecieve

        async Task<Tuple<CommunicationResult, byte[]>> SendRecieveInternal
        (string ipAddress, byte commandBytes, byte[] data,
         int expectedSize, bool verifyExpectedSize,
         TimeoutLevel timeoutLevel = TimeoutLevel.normal)
        {
            Tuple<CommunicationResult, byte[]> finalResult;

            CommandObject commandObject = new CommandObject()
            {
                CommandBytes = commandBytes,
                Data = data,
                ExpectedSize = expectedSize,
                VerifyExpectedSize = verifyExpectedSize
            };

            using (var client = new TcpSocketClient())
            {
                await client.ConnectAsync
                            (ipAddress, 9308, false, TCPClientCancellationToken);

                finalResult = await SendRecieveUsingClient
                    (client, ipAddress, commandObject, timeoutLevel);

                await CloseClient(client);
            }

            return finalResult;
        }

        async Task<Tuple<CommunicationResult, byte[]>> SendRecieveUsingClient
        (TcpSocketClient client, string ipAddress, CommandObject commandObject,
         TimeoutLevel timeoutLevel)
        {
            Tuple<CommunicationResult, byte[]> finalResult;

            try
            {
                finalResult = await SendRecieveOneCommand
                    (client, ipAddress, commandObject, timeoutLevel);
            }
            catch (Exception ex)
            {
                finalResult = new Tuple<CommunicationResult, byte[]>
                (CommunicationResult.RECEIVING_ERROR, new byte[0]);

                Debug.WriteLine(ex.Message);
            }

            return finalResult;
        }

        async Task<List<Tuple<CommunicationResult, byte[]>>> SendRecieveStackedInternal
        (string ipAddress, List<CommandObject> commands, int startFrom,
          TimeoutLevel timeoutLevel = TimeoutLevel.normal)
        {
            if (commands == null)
            {
                return new List<Tuple<CommunicationResult, byte[]>>();
            }

            var finalResults = new List<Tuple<CommunicationResult, byte[]>>();

            await ConnectTcpSocket(ipAddress);

            finalResults = await SendRecieveStackedUsingClient
                (stackedSocketClient, ipAddress, commands, startFrom, timeoutLevel);

            await CloseClient(stackedSocketClient);

            return finalResults;
        }

        async Task<List<Tuple<CommunicationResult, byte[]>>> SendRecieveStackedUsingClient
        (TcpSocketClient client, string ipAddress, List<CommandObject> commands,
         int startFrom, TimeoutLevel timeoutLevel)
        {
            var finalResults = new List<Tuple<CommunicationResult, byte[]>>();

            try
            {
                finalResults = await TrySendRecieveStackedUsingClient
                    (client, ipAddress, commands, startFrom, timeoutLevel);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }

            return finalResults;
        }

        async Task<List<Tuple<CommunicationResult, byte[]>>> TrySendRecieveStackedUsingClient
        (TcpSocketClient client, string ipAddress, List<CommandObject> commands,
         int startFrom, TimeoutLevel timeoutLevel)
        {
            var finalResults = new List<Tuple<CommunicationResult, byte[]>>();

            int count = commands.Count;

            int firstIndex = GetFirstIndex(startFrom, count);

            for (int i = firstIndex; i < count; i++)
            {
                if (hasToCancel)
                {
                    break;
                }

                var commandItem = commands[i];
                var result = await SendRecieveOneStackedCommand
                        (client, ipAddress, commandItem, timeoutLevel);

                if (result.Item1 == CommunicationResult.OK)
                {
                    finalResults.Add(result);

                    OnCommandSucceeded(commandItem, i, count);
                }
                else
                {
                    finalResults.Clear();
                    finalResults.Add(result);

                    return finalResults;
                }
            }

            return finalResults;
        }

        int GetFirstIndex(int startFrom, int count)
        {
            int firstIndex;

            if (startFrom <= 0 || startFrom >= count - 1)
                firstIndex = 0;
            else
                firstIndex = startFrom + 1;

            return firstIndex;
        }

        async Task<Tuple<CommunicationResult, byte[]>> SendRecieveOneStackedCommand
        (TcpSocketClient client, string ipAddress, CommandObject commandObject,
         TimeoutLevel timeoutLevel)
        {
            var result = new Tuple<CommunicationResult, byte[]>
            (CommunicationResult.internalFailure, new byte[0]);
            bool hasError = false;
            bool shouldReconnect = false;

            try
            {
                await Task.Delay(STACKED_COMMAND_DELAY);

                result = await SendRecieveOneCommand
                    (client, ipAddress, commandObject, timeoutLevel);
            }
            catch (IOException ex)
            {
                hasError = true;
                shouldReconnect = true;

                Debug.WriteLine(ex.ToString());
            }
            catch (Exception ex)
            {
                hasError = true;

                Debug.WriteLine(ex.ToString());
            }

            if (hasError || result.Item1 != CommunicationResult.OK)
            {
                commandObject.IncreaseTrials();

                if (commandObject.CanTry())
                {
                    await Task.Delay(STACKED_COMMAND_DELAY_ERROR);

                    if (shouldReconnect)
                        await ConnectTcpSocket(ipAddress);

                    return await
                        SendRecieveOneStackedCommand(client, ipAddress, commandObject, timeoutLevel);
                }
                else
                {
                    Debug.WriteLine("Not OK the status is: " + result.Item1);
                }
            }

            return result;
        }

        async Task<Tuple<CommunicationResult, byte[]>> SendRecieveOneCommand
        (TcpSocketClient client, string ipAddress, CommandObject commandObject,
         TimeoutLevel timeoutLevel)
        {
            var data = commandObject.Data;

            int numBytesRead = 0;
            int bytesCount;

            var resultArray = commandObject.ResultArray;

            //the whole packet to be sent
            byte[] readData = new byte[CommProtocol.MAX_PACKET_SIZE];

            //the whole packet to be sent
            byte[] packet = new byte[CommProtocol.MAX_PACKET_SIZE];

            int numberOfBytesToSend =
                CommProtocol.GetPacket
                            (commandObject.Data, packet, commandObject.CommandBytes);

            if (killMeTheSoonest)
            {
                return new Tuple<CommunicationResult, byte[]>
                    (CommunicationResult.NOT_EXIST, resultArray);
            }

            if (stopScanning)
                return new Tuple<CommunicationResult, byte[]>
                        (CommunicationResult.internalFailure, resultArray);

            ChangeTimeoutByTimeoutLevel(timeoutLevel, ref client);

            //Write the Data
            await client.WriteStream.WriteAsync
                        (packet, 0, numberOfBytesToSend,
                         TCPClientCancellationToken);

            numBytesRead = client.ReadStream.Read(readData, 0, 2);

            if (killMeTheSoonest)
            {
                return new Tuple<CommunicationResult, byte[]>
                    (CommunicationResult.NOT_EXIST, resultArray);
            }

            //for commission
            if (numBytesRead != 2)
            {
                return new Tuple<CommunicationResult, byte[]>
                    (CommunicationResult.RECEIVING_ERROR, resultArray);
            }

            bytesCount = BitConverter.ToInt16(readData, 0);
            if (bytesCount < CommProtocol.MIN_EXPECTED_PACKET_SIZE - 2)
            {
                return new Tuple<CommunicationResult, byte[]>
                    (CommunicationResult.RECEIVING_ERROR, resultArray);
            }

            if (killMeTheSoonest)
            {
                return new Tuple<CommunicationResult, byte[]>
                    (CommunicationResult.NOT_EXIST, resultArray);
            }

            try
            {
                numBytesRead = client.ReadStream.Read(readData, 2, bytesCount);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }

            if (numBytesRead < bytesCount)
            {
                int newArrivedBytes = 0;
                int maxReceiveFragmentation = 5;

                do
                {
                    newArrivedBytes = 0;

                    newArrivedBytes = client.ReadStream.Read(readData, 2 + numBytesRead, bytesCount - numBytesRead);

                    numBytesRead += newArrivedBytes;

                    if (numBytesRead < bytesCount && newArrivedBytes != 0 && maxReceiveFragmentation-- > 0)
                        await Task.Delay(100);

                    if (killMeTheSoonest)
                    {
                        return new Tuple<CommunicationResult, byte[]>
                            (CommunicationResult.NOT_EXIST, resultArray);
                    }
                } while (numBytesRead < bytesCount && newArrivedBytes != 0 && maxReceiveFragmentation-- > 0);
            }

            //for commission
            if (numBytesRead != bytesCount)
            {
                return new Tuple<CommunicationResult, byte[]>
                    (CommunicationResult.EXPECTED_DATA_COUNT_ERROR, resultArray);
            }

            bool lastCommandStatus = true;

            CommunicationResult res = CommProtocol
                .ValidateRecievdedPacket
                (commandObject.CommandBytes, bytesCount, readData,
                 ref lastCommandStatus, commandObject.ExpectedSize,
                 commandObject.VerifyExpectedSize);

            if (killMeTheSoonest)
            {
                return new Tuple<CommunicationResult, byte[]>
                    (CommunicationResult.NOT_EXIST, resultArray);
            }

            if (lastCommandStatus)
            {
                resultArray = new byte[bytesCount + 2 - CommProtocol.MIN_EXPECTED_PACKET_SIZE];
                Array.Copy(readData, 4, resultArray, 0, resultArray.Length);
            }

            if (SHOW_PINGED_IP)
                Debug.WriteLine("Define Call Completed for " + ipAddress);

            var finalResult = new Tuple<CommunicationResult, byte[]>
            (res, resultArray);

            return finalResult;
        }

        void OnCommandSucceeded(CommandObject command, int index, int count)
        {
            ProgressCompleted = GetProgressCompleted(index + 1, count);

            if (command.SaveLastSucceededIndex)
                FireOnLastSucceededIndexChanged(command.CommandBytes, index);
        }

        async Task ConnectTcpSocket(string ipAddress)
        {
            stackedSocketClient = new TcpSocketClient();

            await stackedSocketClient.ConnectAsync(ipAddress, 9308, false, TCPClientCancellationToken);
        }

        async Task CloseClient(TcpSocketClient client)
        {
            if (client == null)
                return;

            try
            {
                await client.DisconnectAsync();
                client.Dispose();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        void ChangeTimeoutByTimeoutLevel
        (TimeoutLevel timeoutLevel, ref TcpSocketClient client)
        {
            switch (timeoutLevel)
            {
                case TimeoutLevel.shortTimeout:
                    client.WriteStream.WriteTimeout = 2500;
                    client.WriteStream.ReadTimeout = 5000;
                    client.ReadStream.ReadTimeout = 5000;
                    client.ReadStream.WriteTimeout = 2500;

                    break;

                case TimeoutLevel.normal:
                    client.WriteStream.WriteTimeout = 4000;
                    client.WriteStream.ReadTimeout = 10000;
                    client.ReadStream.ReadTimeout = 10000;
                    client.ReadStream.WriteTimeout = 4000;

                    break;

                case TimeoutLevel.extended:
                    client.WriteStream.WriteTimeout = 8000;
                    client.WriteStream.ReadTimeout = 20000;
                    client.ReadStream.ReadTimeout = 20000;
                    client.ReadStream.WriteTimeout = 8000;

                    break;
            }
        }

        #endregion

        #region My Send Recieve

        public async Task<Tuple<CommunicationResult, byte[]>> MySendRecieve
        (string serialNumber, byte commandBytes, byte[] data, int expectedSize,
         bool verifyExpectedSize, byte[] resultArray, TimeoutLevel timeoutLevel)
        {
            RouterInterfaceDevice routerInterfaceDevice;
            int expectedConnectionsCount = 0;

            lock (snslocker)
            {
                expectedConnectionsCount = connectedDevices.Keys.Count;
                if (connectedDevices.ContainsKey(serialNumber))
                    routerInterfaceDevice = connectedDevices[serialNumber];
                else
                    return new Tuple<CommunicationResult, byte[]>
                        (CommunicationResult.NOT_EXIST, resultArray);
            }

            try
            {
                var tuple = await SendRecieveInternal
                    (routerInterfaceDevice.ipAddress, commandBytes, data,
                     expectedSize, verifyExpectedSize, timeoutLevel);

                CommunicationResult communicationResult = tuple.Item1;
                resultArray = tuple.Item2;

                if (communicationResult == CommunicationResult.RECEIVING_ERROR)
                {
                    await Task.Delay(ACConstants.SENDING_DELAY);

                    tuple = await SendRecieveInternal
                        (routerInterfaceDevice.ipAddress, commandBytes, data,
                         expectedSize, verifyExpectedSize, timeoutLevel);

                    communicationResult = tuple.Item1;
                    resultArray = tuple.Item2;
                }
                if (communicationResult != CommunicationResult.OK
                    && communicationResult != CommunicationResult.CHARGER_BUSY
                    && communicationResult != CommunicationResult.COMMAND_DELAYED)
                {
                    routerInterfaceDevice.setResult(true);

                    if (communicationResult == CommunicationResult.NOT_EXIST)
                    {
                        bool iky = PingAsync(routerInterfaceDevice.ipAddress).Result.Item2;

                        if (!iky)
                            routerInterfaceDevice.setResult(true);

                        if (ControlObject.isDebugMaster)
                        {
                            Logger.AddLog(false, "Ping Passed-" + routerInterfaceDevice.ipAddress + "-:" + iky);
                        }
                    }
                }
                else
                    routerInterfaceDevice.setResult(false);

                return new Tuple<CommunicationResult, byte[]>
                    (communicationResult, resultArray);
            }
            catch (Exception ex)
            {
                Logger.AddLog(true, "X106" + ex);

                return new Tuple<CommunicationResult, byte[]>
                    (CommunicationResult.internalFailure, resultArray);
            }
            finally
            {
                FinalizeRouterInterfaceDevice(routerInterfaceDevice, serialNumber);
            }
        }

        public async Task<List<Tuple<CommunicationResult, byte[]>>>
        MySendRecieveStacked
        (string serialNumber, List<CommandObject> commands, int startFrom,
         TimeoutLevel timeoutLevel)
        {
            hasToCancel = false;

            List<Tuple<CommunicationResult, byte[]>> finalResult =
                new List<Tuple<CommunicationResult, byte[]>>();

            RouterInterfaceDevice routerInterfaceDevice;
            int expectedConnectionsCount = 0;

            lock (snslocker)
            {
                expectedConnectionsCount = connectedDevices.Keys.Count;

                if (connectedDevices.ContainsKey(serialNumber))
                    routerInterfaceDevice = connectedDevices[serialNumber];
                else
                    return new List<Tuple<CommunicationResult, byte[]>>();
            }

            try
            {
                finalResult = await TryMySendRecieveStacked
                    (commands, timeoutLevel, startFrom, routerInterfaceDevice);
            }
            catch (Exception ex)
            {
                Logger.AddLog(true, "X106" + ex);

                return new List<Tuple<CommunicationResult, byte[]>>();
            }
            finally
            {
                FinalizeRouterInterfaceDevice(routerInterfaceDevice, serialNumber);
            }

            if (hasToCancel)
            {
                finalResult = new List<Tuple<CommunicationResult, byte[]>>();

                hasToCancel = false;
            }

            return finalResult;
        }

        public async Task<List<Tuple<CommunicationResult, byte[]>>>
        TryMySendRecieveStacked
        (List<CommandObject> commands, TimeoutLevel timeoutLevel, int startFrom,
         RouterInterfaceDevice routerInterfaceDevice)
        {
            var tuple = await SendRecieveStackedInternal
                (routerInterfaceDevice.ipAddress, commands, startFrom, timeoutLevel);

            routerInterfaceDevice.setResult(false);

            return tuple;
        }

        void FinalizeRouterInterfaceDevice
        (RouterInterfaceDevice routerInterfaceDevice, string serialNumber)
        {
            if (routerInterfaceDevice.isDead || routerInterfaceDevice.softKill)
            {
                lock (snslocker)
                {
                    if (ControlObject.isDebugMaster)
                    {
                        Logger.AddLog(false, "Removed(1)-" + routerInterfaceDevice.ipAddress + "-:" + serialNumber);
                    }
                }
            }
        }

        #endregion

        internal void RequestSoftKill(string sn)
        {
            try
            {
                lock (snslocker)
                {
                    if (connectedDevices.ContainsKey(sn))
                    {
                        connectedDevices[sn].softKill = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.AddLog(true, "X113" + ex.ToString());
            }
        }

        public void Cancel()
        {
            hasToCancel = true;
        }
    }
}
