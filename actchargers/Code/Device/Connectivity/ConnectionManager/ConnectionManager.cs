using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;

namespace actchargers
{
    public class ConnectionManager
    {
        // Protected implementation of Dispose pattern.
        public void Close()
        {
            if (ControlObject.connectMethods.Connectiontype == ConnectionTypesEnum.UPLOAD_NOT_CONNECT || ControlObject.isSynchSitesForm)
                return;

            lock (myLock)
            {
                _requestCancelScan = true;
                _StopScanning = true;
            }

            MyCommunicator.Close();

        }

        static object myLock = new object();
        static private bool _profileErrorRecorded = false;
        static private bool profileErrorRecorded
        {
            get
            {
                lock (myLock)
                {
                    return _profileErrorRecorded;
                }
            }
            set
            {
                lock (myLock)
                {
                    _profileErrorRecorded = value;
                }
            }
        }
        private int _mobileRouterReconnect;
        private int mobileRouterReconnect
        {
            get
            {
                lock (myLock)
                {
                    return _mobileRouterReconnect;
                }
            }
            set
            {
                lock (myLock)
                {
                    _mobileRouterReconnect = value;
                }
            }
        }
        private string _mobileReconnectResult;
        private string mobileReconnectResult
        {
            get
            {
                lock (myLock)
                {
                    return _mobileReconnectResult;
                }
            }
            set
            {
                lock (myLock)
                {
                    _mobileReconnectResult = value;
                }
            }
        }
        private DateTime _lastInternalScanTime;
        private DateTime lastInternalScanTime
        {
            get
            {
                lock (myLock)
                {
                    return new DateTime(_lastInternalScanTime.Ticks);
                }
            }
            set
            {
                lock (myLock)
                {
                    _lastInternalScanTime = value;
                }
            }
        }
        private DateTime _userRequestScanTime;
        private DateTime userRequestScanTime
        {
            get
            {
                lock (myLock)
                {
                    return new DateTime(_userRequestScanTime.Ticks);
                }
            }
            set
            {
                lock (myLock)
                {
                    _userRequestScanTime = value;
                }
            }
        }
        private bool _isInternalCall;
        private bool isInternalCall
        {
            get
            {
                lock (myLock)
                {
                    return _isInternalCall;
                }
            }
            set
            {
                lock (myLock)
                {
                    _isInternalCall = value;
                }
            }
        }
        private bool _connected;
        private bool connected
        {
            get
            {
                lock (myLock)
                {
                    return _connected;
                }
            }
            set
            {
                lock (myLock)
                {
                    _connected = value;
                }
            }
        }
        private bool _inInternalScan;
        private bool inInternalScan
        {
            get
            {
                lock (myLock)
                {
                    return _inInternalScan;
                }
            }
            set
            {
                lock (myLock)
                {
                    _inInternalScan = value;
                }
            }
        }
        private string _workingSerialNumber;
        public string workingSerialNumber
        {
            get
            {
                lock (myLock)
                {
                    return _workingSerialNumber;
                }
            }
            set
            {
                lock (myLock)
                {
                    _workingSerialNumber = value;
                }
            }
        }
        private bool _selectedIsCharger;
        private bool selectedIsCharger
        {
            get
            {
                lock (myLock)
                {
                    return _selectedIsCharger;
                }
            }
            set
            {
                lock (myLock)
                {
                    _selectedIsCharger = value;
                }
            }
        }
        private int _CONNECTERRORCOUNT;
        private int CONNECTERRORCOUNT
        {
            get
            {
                lock (myLock)
                {
                    return _CONNECTERRORCOUNT;
                }
            }
            set
            {
                lock (myLock)
                {
                    _CONNECTERRORCOUNT = value;
                }
            }
        }
        private bool _requestCancelScan;
        private bool requestCancelScan
        {
            get
            {
                lock (myLock)
                {
                    return _requestCancelScan;
                }
            }
            set
            {
                lock (myLock)
                {
                    _requestCancelScan = value;
                }
            }
        }
        private bool _isRunningBackgroundScan;
        private bool isRunningBackgroundScan
        {
            get
            {
                lock (myLock)
                {
                    return _isRunningBackgroundScan;
                }
            }
            set
            {
                lock (myLock)
                {
                    _isRunningBackgroundScan = value;
                }
            }
        }

        private bool _scanRunning;
        private bool scanRunning
        {
            get
            {
                lock (myLock)
                {
                    return _scanRunning;
                }
            }
            set
            {
                lock (myLock)
                {
                    _scanRunning = value;
                }
            }
        }
        private bool _StopScanning;
        private bool StopScanning
        {
            get
            {
                lock (myLock)
                {
                    return _StopScanning;
                }
            }
            set
            {
                lock (myLock)
                {
                    _StopScanning = value;
                }
            }
        }
        private int _failedCheckRequest;
        private int failedCheckRequest
        {
            get
            {
                lock (myLock)
                {
                    return _failedCheckRequest;
                }
            }
            set
            {
                lock (myLock)
                {
                    _failedCheckRequest = value;
                }
            }
        }

        public bool _isConnecting = false;
        public bool isConnecting
        {
            get
            {
                lock (myLock)
                {
                    return _isConnecting;
                }
            }
            set
            {
                lock (myLock)
                {
                    _isConnecting = value;
                }
            }
        }

        public Communicator MyCommunicator;

        private DateTime _lastBattViewSynchTime;
        private DateTime lastBattViewSynchTime
        {
            get
            {
                lock (myLock)
                {
                    return new DateTime(_lastBattViewSynchTime.Ticks);
                }
            }
            set
            {
                lock (myLock)
                {
                    _lastBattViewSynchTime = value;
                }
            }
        }

        public void ForceSoftDisconnectDevice(string sn, bool fullDeviceRemoval = false, bool withoutLock = false)
        {
            try
            {
                TryForceSoftDisconnectDevice(sn, fullDeviceRemoval, withoutLock);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        void TryForceSoftDisconnectDevice(string sn, bool fullDeviceRemoval = false, bool withoutLock = false)
        {
            if (sn == null)
                return;

            if (workingSerialNumber == sn)
                workingSerialNumber = "";

            string siteViewInterfaceSn = sn;

            if ((ControlObject.connectMethods.Connectiontype == ConnectionTypesEnum.USB))
                sn = sn.Substring(5);

            if (!withoutLock)
                LockDevice(sn);

            if (ControlObject.isDebugMaster)
                Logger.AddLog(false, "SOFT DISCONNECT:" + fullDeviceRemoval + ",sn:" + sn);

            if (fullDeviceRemoval)
                siteView.RemoveDevice(siteViewInterfaceSn);
            else
                siteView.DisconnectDevice(siteViewInterfaceSn);

            MyCommunicator.RequestSoftDisconnect(sn);
        }

        void LockDevice(string sn)
        {
            GenericDevice device = managedMCBs.getDeviceByKey(sn);
            if (device == null)
                device = managedBATTViews.getDeviceByKey(sn);

            if (device == null)
                return;

            if (device.deviceLockME(99999))
                device.BATTunlockMe();
        }

        Dictionary<string, int> usbBlackList;

        public ConnectionManagerGenericList<DeviceMCBObject> managedMCBs;
        ConnectionManagerGenericList<DeviceMCBObject> internalScanMCBList;

        public ConnectionManagerGenericList<DeviceBattViewObject> managedBATTViews;
        private ConnectionManagerGenericList<DeviceBattViewObject> internalScanBattViewList;

        public MCBobject activeMCB;
        public BattViewObject activeBattView;

        public SessionDevices siteView;



        public bool isUnerlyingConnected()
        {
            if (ControlObject.connectMethods.Connectiontype == ConnectionTypesEnum.UPLOAD_NOT_CONNECT || ControlObject.isSynchSitesForm)
                return false;
            return connected;
        }

        private bool applyDiffWithInternal()
        {
            if (ControlObject.connectMethods.Connectiontype == ConnectionTypesEnum.UPLOAD_NOT_CONNECT || ControlObject.isSynchSitesForm)
                return false;

            if (internalScanMCBList.listCompare(managedMCBs))
                return true;
            if (internalScanBattViewList.listCompare(managedBATTViews))
                return true;
            return false;
        }
        public bool updateIfRequired()
        {
            if (ControlObject.connectMethods.Connectiontype == ConnectionTypesEnum.UPLOAD_NOT_CONNECT || ControlObject.isSynchSitesForm)
                return false;

            StopScanning = true;
            while (scanRunning) Task.Delay(250);


            bool requestUpdate = false;

            try
            {
                if (lastInternalScanTime > userRequestScanTime)
                {
                    if (applyDiffWithInternal())
                    {
                        if (ControlObject.isDebugMaster)
                        { }
                        //Logger.addLog(false, "Just Got Something");

                        DeviceBattViewObject toUpgradeBatt = null;
                        if (!selectedIsCharger && managedBATTViews.keyExists(workingSerialNumber))
                        {
                            toUpgradeBatt = managedBATTViews.getDeviceByKey(workingSerialNumber);
                        }
                        DeviceMCBObject toUpgradeMCB = null;
                        if (selectedIsCharger && managedMCBs.keyExists(workingSerialNumber))
                        {
                            toUpgradeMCB = managedMCBs.getDeviceByKey(workingSerialNumber);
                        }


                        managedMCBs.assignListFromList(internalScanMCBList);
                        managedBATTViews.assignListFromList(internalScanBattViewList);
                        //battViewList = new Dictionary<string, deviceBattViewObject>(internalScanBattViewList);
                        if (workingSerialNumber != "")
                        {
                            if (selectedIsCharger)
                            {
                                if (!managedMCBs.keyExists(workingSerialNumber))
                                {
                                    workingSerialNumber = "";

                                }
                                else
                                {
                                    if (toUpgradeMCB != null)
                                        managedMCBs.addDevice(workingSerialNumber, toUpgradeMCB);
                                }
                            }
                            else
                            {
                                if (!managedBATTViews.keyExists(workingSerialNumber))
                                {
                                    workingSerialNumber = "";

                                }
                                else
                                {
                                    if (toUpgradeBatt != null)
                                        managedBATTViews.addDevice(workingSerialNumber, toUpgradeBatt);
                                }
                            }
                        }
                        requestUpdate = true;
                    }

                }
            }
            catch (Exception ex)
            {
                Logger.AddLog(true, "X43" + ex.ToString());
            }
            lock (myLock)
            {
                _StopScanning = false;
                _isRunningBackgroundScan = true;
            }


            return requestUpdate;
        }

        internal Task<DoLoadResult> doLoadAsync(
              object device, bool isMCB, string sn, DefineObjectInfo info, bool useInfo, DeviceBaseType dType)
        {
            return Task.Run(async () =>
            {

                try
                {
                    bool result = false;
                    DoLoadResult x = new DoLoadResult(isMCB, sn, device);
                    x.deviceType = dType;
                    if (isMCB)
                    {
                        MCBobject mcb;
                        mcb = (MCBobject)device;
                        if (useInfo)
                            result = await mcb.RouterDoLoad(info.firmwareVersion, info.zoneID, info.lostRTC);
                        else
                            result = await mcb.DoLoad();

                    }
                    else
                    {
                        BattViewObject batt;
                        batt = (BattViewObject)device;
                        if (useInfo)
                        {
                            result = await batt.RouterDoLoad(info.firmwareVersion, info.zoneID, info.lostRTC);

                        }
                        else
                        {
                            result = await batt.DoLoad();

                        }

                    }
                    x.res = result;
                    return x;
                }
                catch (Exception ex)
                {
                    Logger.AddLog(true, ex.ToString());
                    DoLoadResult x = new DoLoadResult(isMCB, sn, device);
                    x.deviceType = dType;
                    x.res = false;
                    return x;
                }

            });
        }
        private async Task internalScan(bool isInternal)
        {

            List<Task<DoLoadResult>> deloadList = new List<Task<DoLoadResult>>();

            if (ControlObject.connectMethods.Connectiontype == ConnectionTypesEnum.UPLOAD_NOT_CONNECT || ControlObject.isSynchSitesForm)
                return;
            lock (myLock)
            {
                if (_requestCancelScan || _scanRunning)
                    return;
                _scanRunning = true;
            }
            try
            {
                bool copiedFromInternal = false;
                string[] serialNumbers = new string[0];

                Dictionary<string, DefineObjectInfo> defineInfo = new Dictionary<string, DefineObjectInfo>();
                //System.Collections.Generic.Dictionary<string, MCBObject.MCBobject> tempMCBList = new Dictionary<string, MCBObject.MCBobject>();
                //System.Collections.Generic.Dictionary<string, deviceBattViewObject> tempBattViewList = new Dictionary<string, deviceBattViewObject>();
                ConnectionManagerGenericList<DeviceBattViewObject> tempBattViewList;
                ConnectionManagerGenericList<DeviceMCBObject> tempMCBList;

                DeviceBaseType deviceType;


                if (isInternal)
                {
                    tempMCBList = internalScanMCBList;
                    tempBattViewList = internalScanBattViewList;
                }
                else
                {
                    tempMCBList = managedMCBs;

                    tempBattViewList = managedBATTViews;

                }

                serialNumbers = MyCommunicator.GetDevicesSerialNumbers();

                if (ControlObject.connectMethods.Connectiontype == ConnectionTypesEnum.ROUTER)
                {
                    defineInfo = MyCommunicator.GetSerialNumbersWithDefineInfo();
                }

                if (requestCancelScan)
                {
                    return;
                }


                double maxToSend = ControlObject.numberOfThreads;
                maxToSend = Math.Round(ControlObject.numberOfThreads * ControlObject.cpuPowerFactor);
                if (maxToSend < 2)
                    maxToSend = 2;

                int c = 0;
                foreach (string s in serialNumbers)
                {
                    if (s == null || s.Trim() == "")
                        continue;
                    if (c >= maxToSend)
                    {
                        if (ControlObject.isDebugMaster)
                        {
                            Logger.AddLog(false, "FULL LOAD CAP REACHED:" + maxToSend.ToString());
                        }
                        break;
                    }
                    string battS = ((ControlObject.connectMethods.Connectiontype == ConnectionTypesEnum.USB) ? "BATT_" : "") + s;
                    string mcbS = ((ControlObject.connectMethods.Connectiontype == ConnectionTypesEnum.USB) ? "CHRG_" : "") + s;

                    if (s != null && !tempMCBList.keyExists(mcbS) && !tempBattViewList.keyExists(battS))
                    {
                        if (requestCancelScan)
                        {
                            return;
                        }
                        MCBobject mcb = null;// = new MCBObject.MCBobject(s);
                        BattViewObject batt = null;
                        bool thisIsMCB = true;
                        string bakedSerial = s;
                        if (bakedSerial.StartsWith("CHRG_"))
                            thisIsMCB = true;
                        else
                            thisIsMCB = false;
                        if (ControlObject.connectMethods.Connectiontype == ConnectionTypesEnum.USB)
                        {
                            // in case of USB , we don't know if it's battview or MCB
                            try
                            {
                                ValidateDeviceResponse res = await MyCommunicator.IsThisAValidDevice("UNKA_" + s);
                                if (res.communicateResult == CommunicationResult.OK)
                                {
                                    deviceType = res.deviceType;
                                    if (res.isCharger)
                                    {
                                        ACConstants.IsUSBBattView = false;
                                        bakedSerial = "CHRG_" + s;

                                    }
                                    else
                                    {
                                        ACConstants.IsUSBBattView = true;
                                        bakedSerial = "BATT_" + s;
                                    }
                                }
                                else
                                {

                                    if (usbBlackList.Keys.Contains(s))
                                    {
                                        usbBlackList[s]++;
                                    }
                                    else
                                    {
                                        usbBlackList[s] = 0;
                                    }

                                    continue;
                                }
                            }
                            catch (Exception ex)
                            {
                                Logger.AddLog(true, "X44" + ex.ToString());
                                continue;
                            }
                            if (bakedSerial.StartsWith("CHRG_"))
                                thisIsMCB = true;
                            else
                                thisIsMCB = false;
                        }
                        else if (ControlObject.connectMethods.Connectiontype == ConnectionTypesEnum.ROUTER)
                        {
                            deviceType = defineInfo[s].deviceType;
                        }
                        else
                        {
                            if (thisIsMCB)
                                deviceType = DeviceBaseType.MCB;
                            else
                                deviceType = DeviceBaseType.BATTVIEW;

                        }

                        if (bakedSerial.StartsWith("CHRG_"))
                            thisIsMCB = true;
                        else
                            thisIsMCB = false;

                        try
                        {
                            if (thisIsMCB)
                            {
                                ACConstants.IsUSBBattView = false;
                                mcb = new MCBobject(bakedSerial, this, deviceType);
                                //todo: find serial number.
                                ACConstants.USBConnectedSerialNumber = bakedSerial;//mcb.MCBConfig.serialNumber;
                                if (ControlObject.lazyLoading && defineInfo.Keys.Contains(bakedSerial))
                                {
                                    mcb.doLazyLoad(defineInfo[s].deviceSerialNumber, defineInfo[s].id, defineInfo[s].firmwareVersion, defineInfo[s].FirmwareWiFiVersion, defineInfo[s].zoneID, defineInfo[s].lostRTC, defineInfo[s].replacementPart, defineInfo[s].name, defineInfo[s].IpAddress, defineInfo[s].DcId, defineInfo[s].FirmwareDcVersion);
                                    if (!tempMCBList.keyExists(bakedSerial))
                                    {
                                        DeviceMCBObject toAdd = new DeviceMCBObject(mcb, deviceType);
                                        tempMCBList.addDevice(bakedSerial, toAdd);
                                    }
                                    siteView.AddDevice(UInt32.Parse(mcb.Config.id), SessionDevices.getMCBDeviceType(deviceType, mcb.Config.replacmentPart), mcb.Config.serialNumber, mcb.Config.chargerUserName, bakedSerial, mcb.DcId, mcb.FirmwareRevision, mcb.FirmwareWiFiVersion, mcb.FirmwareDcVersion);
                                }
                                else
                                {
                                    if (ControlObject.isDebugMaster)
                                        Logger.AddLog(false, "Queue do Load For charger:" + bakedSerial);
                                    if (ControlObject.connectMethods.Connectiontype == ConnectionTypesEnum.ROUTER && defineInfo.Keys.Contains(bakedSerial))
                                    {
                                        deloadList.Add(doLoadAsync(mcb, true, bakedSerial, defineInfo[s], true, deviceType));

                                    }
                                    else
                                    {
                                        deloadList.Add(doLoadAsync(mcb, true, bakedSerial, null, false, deviceType));

                                    }
                                    c++;
                                }

                            }
                            else
                            {
                                ACConstants.IsUSBBattView = true;
                                batt = new BattViewObject(bakedSerial, this);
                                ACConstants.USBConnectedSerialNumber = batt.SerialNumber;

                                if (ControlObject.lazyLoading && defineInfo.Keys.Contains(bakedSerial))
                                {
                                    batt.doLazyLoad(defineInfo[s].deviceSerialNumber, defineInfo[s].id, defineInfo[s].firmwareVersion, defineInfo[s].FirmwareWiFiVersion, defineInfo[s].zoneID, defineInfo[s].lostRTC, defineInfo[s].studyID, defineInfo[s].replacementPart, defineInfo[s].name, defineInfo[s].IpAddress);

                                    if (!tempBattViewList.keyExists(bakedSerial))
                                    {
                                        DeviceBattViewObject toAdd = new DeviceBattViewObject(batt);
                                        tempBattViewList.addDevice(bakedSerial, toAdd);
                                    }
                                    siteView.AddDevice(batt.Config.id, SessionDevices.getBATTviewDeviceType(batt.Config.studyId, batt.Config.replacmentPart, batt.Config.battViewSN), batt.Config.battViewSN, batt.Config.batteryID, bakedSerial, batt.DcId, batt.FirmwareRevision, batt.FirmwareWiFiVersion, batt.FirmwareDcVersion);
                                }
                                else
                                {
                                    if (ControlObject.isDebugMaster)
                                        if (ControlObject.isDebugMaster)
                                        { }
                                    Logger.AddLog(false, "Queue do Load For battview:" + bakedSerial);
                                    if (ControlObject.connectMethods.Connectiontype == ConnectionTypesEnum.ROUTER && defineInfo.Keys.Contains(bakedSerial))
                                    {
                                        deloadList.Add(doLoadAsync(batt, false, bakedSerial, defineInfo[s], true, deviceType));

                                    }
                                    else
                                    {
                                        deloadList.Add(doLoadAsync(batt, false, bakedSerial, null, false, deviceType));

                                    }
                                    c++;
                                }

                            }


                        }
                        catch (Exception ex)
                        {
                            Logger.AddLog(true, "X45" + ex.ToString());
                            continue;
                        }

                    }
                }
                //after this point it's meaningless to cancel scan
                if (deloadList.Count > 0)
                {
                    isConnecting = true;
                    Task.WaitAll(deloadList.ToArray());//No cancel HERE
                }

                bool anyPassed = false;
                foreach (var r in deloadList)
                {
                    if (r.Result.res)
                    {
                        anyPassed = true;
                        if (r.Result.isMCB)
                        {
                            DeviceMCBObject toAdd = new DeviceMCBObject((MCBobject)r.Result.device, r.Result.deviceType);


                            if (!tempMCBList.keyExists(r.Result.InterfaceSN))
                            {
                                tempMCBList.addDevice(r.Result.InterfaceSN, toAdd);
                            }
                            siteView.AddDevice(UInt32.Parse(toAdd.mcb.Config.id), SessionDevices.getMCBDeviceType(toAdd.mcb.DeviceType, toAdd.mcb.Config.replacmentPart), toAdd.mcb.Config.serialNumber, toAdd.mcb.Config.chargerUserName, r.Result.InterfaceSN, toAdd.mcb.DcId, toAdd.mcb.FirmwareRevision, toAdd.mcb.FirmwareWiFiVersion, toAdd.mcb.FirmwareDcVersion);
                            siteView.setDeviceConfigurationRead(r.Result.InterfaceSN, true, toAdd.mcb.DcId, toAdd.mcb.FirmwareRevision, toAdd.mcb.FirmwareWiFiVersion, toAdd.mcb.FirmwareDcVersion);

                            if (ControlObject.isDebugMaster)
                            {
                                Logger.AddLog(false, " doLoad done for charger:" + r.Result.InterfaceSN);
                            }


                        }
                        else
                        {
                            DeviceBattViewObject toAdd = new DeviceBattViewObject((BattViewObject)r.Result.device);

                            if (!tempBattViewList.keyExists(r.Result.InterfaceSN))
                            {
                                tempBattViewList.addDevice(r.Result.InterfaceSN, toAdd);
                            }
                            siteView.AddDevice(toAdd.battview.Config.id, SessionDevices.getBATTviewDeviceType(toAdd.battview.Config.studyId, toAdd.battview.Config.replacmentPart, toAdd.battview.Config.battViewSN), toAdd.battview.Config.battViewSN, toAdd.battview.Config.batteryID, r.Result.InterfaceSN, toAdd.battview.DcId, toAdd.battview.FirmwareRevision, toAdd.battview.FirmwareWiFiVersion, toAdd.battview.FirmwareDcVersion);
                            siteView.setDeviceConfigurationRead(r.Result.InterfaceSN, true, toAdd.battview.DcId, toAdd.battview.FirmwareRevision, toAdd.battview.FirmwareWiFiVersion, toAdd.battview.FirmwareDcVersion);

                            if (ControlObject.isDebugMaster)
                            {
                                Logger.AddLog(false, " doLoad done for battview:" + r.Result.InterfaceSN);
                            }

                        }
                    }
                    else
                    {
                        Logger.AddLog(true, "X46" + " doLoad faile for:" + r.Result.InterfaceSN);
                    }

                }
                if (deloadList.Count > 0)
                {
                    if (!anyPassed)
                        isConnecting = false;
                }
                string[] keys;
                keys = tempMCBList.getListKeys();
                foreach (string s in keys)
                {
                    string mcbS = (ControlObject.connectMethods.Connectiontype == ConnectionTypesEnum.USB) ? s.Remove(0, 5) : s;
                    DeviceMCBObject d = tempMCBList.getDeviceByKey(s);
                    if (!serialNumbers.Contains(mcbS))
                    {

                        d.flagOfRemoval = true;
                        if (d != null && d.deviceLockME())
                        {
                            siteView.DisconnectDevice(UInt32.Parse(d.mcb.Config.id), SessionDevices.getMCBDeviceType(d.DeviceType, d.mcb.Config.replacmentPart));
                            tempMCBList.removeDevice(s);
                            d.BATTunlockMe();
                        }

                    }
                    else
                    {
                        d.flagOfRemoval = false;
                    }

                }
                //removing stuff
                keys = tempBattViewList.getListKeys();
                foreach (string s in keys)
                {
                    string battS = (ControlObject.connectMethods.Connectiontype == ConnectionTypesEnum.USB) ? s.Remove(0, 5) : s;
                    DeviceBattViewObject d = tempBattViewList.getDeviceByKey(s);
                    if (!serialNumbers.Contains(battS))
                    {

                        d.flagOfRemoval = true;
                        if (d != null && d.deviceLockME())
                        {
                            siteView.DisconnectDevice(d.battview.Config.id, SessionDevices.getBATTviewDeviceType(d.battview.Config.studyId, d.battview.Config.replacmentPart, d.battview.Config.battViewSN));

                            tempBattViewList.removeDevice(s);
                            d.BATTunlockMe();
                        }

                    }
                    else
                    {
                        d.flagOfRemoval = false;
                    }
                }


                if (!isInternal)
                {

                    managedMCBs.assignListFromList(tempMCBList);
                    managedBATTViews.assignListFromList(tempBattViewList);

                    if ((!managedMCBs.keyExists(workingSerialNumber) && selectedIsCharger) ||
                        (!managedBATTViews.keyExists(workingSerialNumber) && !selectedIsCharger))
                    {
                        workingSerialNumber = "";

                    }
                    userRequestScanTime = DateTime.UtcNow;
                    if (!copiedFromInternal)
                        lastInternalScanTime = DateTime.UtcNow;
                    isRunningBackgroundScan = true;//cancel isInternal if it get called from main app

                }
                else
                {

                    lastInternalScanTime = DateTime.UtcNow;
                    lastBattViewSynchTime = DateTime.UtcNow;
                }

                internalScanMCBList.assignListFromList(tempMCBList);
                internalScanBattViewList.assignListFromList(tempBattViewList);

            }
            catch (Exception ex)
            {
                Logger.AddLog(true, "X47" + ex.ToString());
            }
            finally
            {
                scanRunning = false;
            }


        }
        public async Task Scan(bool isInternal)
        {
            if (ControlObject.connectMethods.Connectiontype == ConnectionTypesEnum.UPLOAD_NOT_CONNECT || ControlObject.isSynchSitesForm)
                return;
            lock (myLock)
            {
                if (_StopScanning)
                    return;
                _StopScanning = true;

                if (!isInternal && _isRunningBackgroundScan && _scanRunning)
                {
                    _requestCancelScan = true;//background scan still running...
                }
                else if (!isInternal && !_isRunningBackgroundScan && _scanRunning)
                {
                    _StopScanning = false;//app scan still running, no need to do scan again
                    return;
                }
                else if (isInternal && _scanRunning)
                {
                    _StopScanning = false;//background scan still running, no need to run another background scan
                    return;
                }
            }
            while (scanRunning) await Task.Delay(250);
            requestCancelScan = false;
            isRunningBackgroundScan = isInternal;
            try
            {
                await internalScan(isInternal);
            }
            catch (Exception ex)
            {
                Logger.AddLog(true, "X48" + ex.Message);

            }
            finally
            {
                isRunningBackgroundScan = true;
                StopScanning = false;
            }
        }

        public string getWorkingSerialNumber()
        {
            if (ControlObject.connectMethods.Connectiontype == ConnectionTypesEnum.UPLOAD_NOT_CONNECT || ControlObject.isSynchSitesForm)
                return "";
            return workingSerialNumber;
        }

        public bool selectDevice(string SN)
        {
            if (ControlObject.connectMethods.Connectiontype == ConnectionTypesEnum.UPLOAD_NOT_CONNECT || ControlObject.isSynchSitesForm)
                return false;
            lock (myLock)
            {
                if (_scanRunning && !isRunningBackgroundScan)
                    return false;
                _StopScanning = true;
                if (_scanRunning)
                {
                    _requestCancelScan = true;//background scan still running...
                }
            }
            while (scanRunning) Task.Delay(250);
            requestCancelScan = false;
            try
            {
                workingSerialNumber = SN;
                if (SN == "")
                    return false;
                if (managedMCBs.keyExists(workingSerialNumber))
                {
                    selectedIsCharger = true;
                    DeviceMCBObject d = managedMCBs.getDeviceByKey(workingSerialNumber);
                    if (d == null)
                        return false;
                    activeMCB = d.mcb;
                    return true;
                }
                else if (managedBATTViews.keyExists(workingSerialNumber))
                {
                    selectedIsCharger = false;
                    DeviceBattViewObject d = managedBATTViews.getDeviceByKey(workingSerialNumber);
                    if (d == null)
                        return false;
                    activeBattView = d.battview;
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Logger.AddLog(true, "X49" + ex.Message);
                return false;
            }
            finally
            {
                StopScanning = false;
            }


        }

        public int getconnectVerifyErrorCount()
        {
            if (ControlObject.connectMethods.Connectiontype == ConnectionTypesEnum.UPLOAD_NOT_CONNECT || ControlObject.isSynchSitesForm)
                return 0;
            return CONNECTERRORCOUNT;

        }
        public string getMobileRouterStatus()
        {
            return mobileReconnectResult;
        }

        public void SynchSiteViewEveryThing()
        {
            if (ControlObject.connectMethods.Connectiontype == ConnectionTypesEnum.UPLOAD_NOT_CONNECT)
                return;
            lock (myLock)
            {
                _StopScanning = true;

                if (_isRunningBackgroundScan && _scanRunning)
                {
                    _requestCancelScan = true;//background scan still running...
                }
            }
            while (scanRunning) Task.Delay(100);
            List<string> removeTheseDevices = new List<string>();
            try
            {
                lock (myLock)
                {
                    foreach (string sn in siteView.getDevicesInterfaceSNs())
                    {
                        SiteViewDevice d = siteView.getDevice(sn);
                        if (d.deviceType == DeviceTypes.mcb)
                        {
                            DeviceMCBObject b = managedMCBs.getDeviceByKey(sn);
                            if (b == null)
                            {
                                if (d.deviceConnected)
                                {
                                    if (ControlObject.isDebugMaster)
                                        Logger.AddLog(false, "Device Removed and not disconnected");
                                    siteView.DisconnectDevice(sn);

                                }
                            }
                            else
                            {

                                try
                                {
                                    if (b.deviceLockME(50000))
                                    {
                                        if (!d.deviceConnected)
                                        {
                                            if (ControlObject.isDebugMaster)
                                                Logger.AddLog(false, "SYNC)Device Added and still disconnected");
                                            d.deviceConnected = true;

                                        }
                                        if (d.serialnumber != b.mcb.Config.serialNumber)
                                        {
                                            //this is so weird and shouldn't happen at all...
                                            if (ControlObject.isDebugMaster)
                                                Logger.AddLog(false, "SYNC)SN chnage:" + d.serialnumber + "-" + b.mcb.Config.serialNumber);
                                            removeTheseDevices.Add(sn);
                                            continue;
                                        }
                                        if (d.id != UInt32.Parse(b.mcb.Config.id))
                                        {
                                            //this is so weird and shouldn't happen at all...
                                            if (ControlObject.isDebugMaster)
                                                Logger.AddLog(false, "SYNC)SN ID:" + d.id.ToString() + "-" + b.mcb.Config.id);
                                            removeTheseDevices.Add(sn);
                                            continue;
                                        }
                                        if (b.mcb.DeviceType != DeviceBaseType.MCB)
                                        {
                                            //this is so weird and shouldn't happen at all...
                                            if (ControlObject.isDebugMaster)
                                                Logger.AddLog(false, "SYNC)Device Type:" + d.deviceType.ToString() + "-" + b.mcb.DeviceType.ToString());
                                            removeTheseDevices.Add(sn);
                                            continue;
                                        }

                                        if (b.mcb.configRead)
                                        {
                                            d.actViewEnabled = b.mcb.Config.actViewEnable;
                                            d.setConfigurationsRead(b.mcb.deviceIsLoaded, 0, 0, 0, 0);

                                        }
                                        d.userNamed = b.mcb.Config.chargerUserName;

                                    }
                                    else
                                    {
                                        continue;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Logger.AddLog(true, ex.ToString());
                                }
                                finally
                                {
                                    b.deviceUnlockMe();
                                }
                                d.firmwareVersion = b.mcb.FirmwareRevision;//last read shows this version

                            }
                            if (!Firmware.DoesMcbRequireFirmwareUpdate(d.firmwareVersion) && d.firmwareStage != FirmwareUpdateStage.updateIsNotNeeded)
                                d.firmwareStage = FirmwareUpdateStage.updateCompleted;


                        }
                        else if (d.deviceType == DeviceTypes.battview || d.deviceType == DeviceTypes.battviewMobile)
                        {
                            DeviceBattViewObject b = managedBATTViews.getDeviceByKey(sn);
                            if (b == null)
                            {
                                if (d.deviceConnected)
                                {
                                    if (ControlObject.isDebugMaster)
                                        Logger.AddLog(false, "Device Removed and not disconnected");
                                    siteView.DisconnectDevice(sn);

                                }
                            }
                            else
                            {

                                try
                                {
                                    if (b.deviceLockME(50000))
                                    {
                                        if (!d.deviceConnected)
                                        {
                                            if (ControlObject.isDebugMaster)
                                                Logger.AddLog(false, "SYNC)Device Added and still disconnected");
                                            d.deviceConnected = true;

                                        }
                                        if (d.serialnumber != b.battview.Config.battViewSN)
                                        {
                                            //this is so weird and shouldn't happen at all...
                                            if (ControlObject.isDebugMaster)
                                                Logger.AddLog(false, "SYNC)SN chnage:" + d.serialnumber + "-" + b.battview.Config.battViewSN);
                                            removeTheseDevices.Add(sn);
                                            continue;
                                        }
                                        if (d.id != b.battview.Config.id)
                                        {
                                            //this is so weird and shouldn't happen at all...
                                            if (ControlObject.isDebugMaster)
                                                Logger.AddLog(false, "SYNC)SN ID:" + d.id.ToString() + "-" + b.battview.Config.id.ToString());
                                            removeTheseDevices.Add(sn);
                                            continue;
                                        }

                                        if (b.battview.configLoaded)
                                        {
                                            d.actViewEnabled = b.battview.Config.ActViewEnabled;
                                            d.setConfigurationsRead(b.battview.deviceIsLoaded, 0, 0, 0, 0);

                                        }
                                        d.userNamed = b.battview.Config.batteryID;

                                    }
                                    else
                                    {
                                        continue;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Logger.AddLog(true, ex.ToString());
                                }
                                finally
                                {
                                    b.deviceUnlockMe();
                                }
                                d.firmwareVersion = b.battview.FirmwareRevision;//last read shows this version

                            }
                            if (!Firmware.DoesBattViewRequireUpdate(d.firmwareVersion, d.firmwareWiFiVersion) && d.firmwareStage != FirmwareUpdateStage.updateIsNotNeeded)
                                d.firmwareStage = FirmwareUpdateStage.updateCompleted;

                        }

                    }
                }


            }
            catch
            {

            }
            finally
            {
                requestCancelScan = false;
                StopScanning = false;

            }


        }

        public ConnectionManager()
        {
            _lastBattViewSynchTime = DateTime.UtcNow;
            _CONNECTERRORCOUNT = 0;
            _mobileRouterReconnect = 0;
            _mobileReconnectResult = "OK";
            requestCancelScan = false;
            isRunningBackgroundScan = true;
            scanRunning = false;
            StopScanning = false;
            failedCheckRequest = 0;

            siteView = new SessionDevices();

            selectDevice("");
            inInternalScan = false;
            managedMCBs = new ConnectionManagerGenericList<DeviceMCBObject>();
            internalScanMCBList = new ConnectionManagerGenericList<DeviceMCBObject>();
            managedBATTViews = new ConnectionManagerGenericList<DeviceBattViewObject>();
            internalScanBattViewList = new ConnectionManagerGenericList<DeviceBattViewObject>();
            usbBlackList = new Dictionary<string, int>();

            lastInternalScanTime = DateTime.UtcNow.AddDays(-1);
            userRequestScanTime = DateTime.UtcNow;

            try
            {
                if (ControlObject.isDebugMaster)
                {
                    Logger.AddLog(false, "Start Communication Engine");
                }

                MyCommunicator =
                    new Communicator
                    (ControlObject.connectMethods.Connectiontype,
                     ControlObject.AccessMCB, ControlObject.AccessBattView);
            }
            catch (Exception ex)
            {
                Logger.AddLog(true, "X54" + ex.Message);
                connected = false;
                return;
            }
            connected = true;
            if (ControlObject.connectMethods.Connectiontype != ConnectionTypesEnum.UPLOAD_NOT_CONNECT && !ControlObject.isSynchSitesForm)
            {
            }
        }

        public async Task refresh_timer_prepare()
        {
            if (ControlObject.connectMethods.Connectiontype == ConnectionTypesEnum.UPLOAD_NOT_CONNECT || ControlObject.isSynchSitesForm)
                return;
            if (StopScanning || requestCancelScan || scanRunning)
                return;
            isRunningBackgroundScan = true;
            await Scan(true);

        }
    }
}
