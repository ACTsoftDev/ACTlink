using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace actchargers
{
    public class SessionDevices
    {

        public static DeviceTypes getMCBDeviceType(DeviceBaseType knownHW, bool replacement)
        {
            if (knownHW == DeviceBaseType.CALIBRATOR)
                return DeviceTypes.calibrator;
            else if (replacement)
                return DeviceTypes.mcbReplacement;
            return DeviceTypes.mcb;
        }
        public static DeviceTypes getBATTviewDeviceType(UInt32 studyID, bool replacement, string sn)
        {
            if (sn[1] == '3')
                return DeviceTypes.battviewMobile;
            else if (replacement)
                return DeviceTypes.battviewReplacement;
            return DeviceTypes.battview;
        }

        object myLock;

        readonly Dictionary<string, SiteViewDevice> allSessionDevices;

        public SessionDevices()
        {
            myLock = new object();
            allSessionDevices = new Dictionary<string, SiteViewDevice>();


        }
        public List<string> getDevicesInterfaceSNs()
        {
            List<string> keys = new List<string>();
            lock (myLock)
            {
                keys = allSessionDevices.Keys.ToList();
            }
            return keys;
        }
        private SiteViewDevice getDevice(UInt32 deviceUniqueID, DeviceTypes type)
        {
            lock (myLock)
            {
                SiteViewDevice temp = allSessionDevices.Values.ToList().Find(x => x.id == deviceUniqueID && x.deviceType == type);
                return temp;
            }
        }
        public SiteViewDevice getDevice(string interfaceSN)
        {
            lock (myLock)
            {
                SiteViewDevice temp = null;
                if (allSessionDevices.Keys.Contains(interfaceSN))
                    temp = allSessionDevices[interfaceSN];
                return temp;
            }
        }

        internal void AddDevice
        (UInt32 deviceUniqueID, DeviceTypes type, string deviceSN, string userNamed,
         string IntrefaceSN, byte dcId, float firmwareVersion, float firmwareWiFiVersion, float firmwareDcVersion)
        {
            userNamed = userNamed.Trim(new char[] { '\0' });

            bool existed = false;
            FirmwareUpdateStage firmwareUpdateStage = FirmwareUpdateStage.connectting;
            int firmwareUpdateStep = 0;

            uint startdownloadId = 0;
            uint enddownloadId = 0;
            uint reacheddownloadId = 0;

            uint startPMdownloadId = 0;
            uint endPMdownloadId = 0;
            uint reachedPMdownloadId = 0;
            bool actviewEnabled = false;
            bool configSaved = false;
            bool limitsLoaded = false;
            SiteViewDevice temp1 = getDevice(deviceUniqueID, type);
            if (temp1 != null)
            {

                lock (myLock)
                {
                    existed = true;
                    firmwareUpdateStage = temp1.firmwareStage;
                    firmwareUpdateStep = temp1.firmwareUpdateStep;
                    startdownloadId = temp1.startEventID;
                    enddownloadId = temp1.endEventID;
                    reacheddownloadId = temp1.downloadEventID;
                    startPMdownloadId = temp1.startPMID;
                    endPMdownloadId = temp1.endPMID;
                    reachedPMdownloadId = temp1.downloadPMID;
                    actviewEnabled = temp1.actViewEnabled;
                    configSaved = temp1.configurationsSaved;
                    limitsLoaded = temp1.limitsLoaded;
                    allSessionDevices.Remove(temp1.interfaceSN);
                }


            }
            temp1 = getDevice(IntrefaceSN);
            if (temp1 != null)
            {

                lock (myLock)
                {
                    existed = true;
                    firmwareUpdateStage = temp1.firmwareStage;
                    firmwareUpdateStep = temp1.firmwareUpdateStep;

                    startdownloadId = temp1.startEventID;
                    enddownloadId = temp1.endEventID;
                    reacheddownloadId = temp1.downloadEventID;

                    startPMdownloadId = temp1.startPMID;
                    endPMdownloadId = temp1.endPMID;
                    reachedPMdownloadId = temp1.downloadPMID;
                    actviewEnabled = temp1.actViewEnabled;
                    configSaved = temp1.configurationsSaved;
                    limitsLoaded = temp1.limitsLoaded;
                    allSessionDevices.Remove(temp1.interfaceSN);
                }


            }



            SiteViewDevice temp = new SiteViewDevice(deviceUniqueID, type, deviceSN, userNamed, IntrefaceSN, dcId, firmwareVersion, firmwareWiFiVersion, firmwareDcVersion);
            lock (myLock)
            {
                if (existed)
                {
                    temp.firmwareUpdateStep = firmwareUpdateStep;
                    temp.firmwareStage = firmwareUpdateStage;
                    temp.actViewEnabled = actviewEnabled;

                    if (type != DeviceTypes.battviewMobile)
                    {
                        temp.setLimits(startdownloadId, enddownloadId, startPMdownloadId, endPMdownloadId);
                        temp.downloadEventID = reacheddownloadId;
                        temp.downloadPMID = reachedPMdownloadId;
                        temp.limitsLoaded = limitsLoaded;
                        temp.configurationsSaved = configSaved;
                    }

                }
                allSessionDevices.Add(IntrefaceSN, temp);

            }


        }
        internal void RemoveDevice(UInt32 deviceUniqueID, DeviceTypes type)
        {
            SiteViewDevice temp = getDevice(deviceUniqueID, type);

            if (temp == null)
                return;

            lock (myLock)
            {
                allSessionDevices.Remove(temp.interfaceSN);
            }
        }

        internal void RemoveDevice(string interfaceSN)
        {
            SiteViewDevice temp = getDevice(interfaceSN);

            if (temp == null)
                return;

            lock (myLock)
            {
                allSessionDevices.Remove(temp.interfaceSN);
            }
        }

        internal void DisconnectDevice(UInt32 deviceUniqueID, DeviceTypes type)
        {
            SiteViewDevice temp = getDevice(deviceUniqueID, type);

            if (temp == null)
                return;

            temp.deviceConnected = false;
        }

        internal void DisconnectDevice(string interfaceSN)
        {
            SiteViewDevice temp = getDevice(interfaceSN);

            if (temp == null)
                return;

            temp.deviceConnected = false;
        }


        #region firmware update area

        public List<string> getDevicesForFirmwareUpdate(DeviceTypes type)
        {
            List<string> res = new List<string>();
            Dictionary<string, SiteViewDevice> m = new Dictionary<string, SiteViewDevice>();
            if (ControlObject.isHWMnafacturer)
                return res;
            lock (myLock)
            {
                foreach (string d in allSessionDevices.Keys)
                {

                    if (allSessionDevices[d].deviceType == type &&
                        allSessionDevices[d].deviceConnected &&
                        allSessionDevices[d].firmwareStage != FirmwareUpdateStage.updateCompleted &&
                        allSessionDevices[d].firmwareStage != FirmwareUpdateStage.updateIsNotNeeded)
                    {
                        if (allSessionDevices[d].firmwareStage == FirmwareUpdateStage.sentRequestPassed || allSessionDevices[d].firmwareStage == FirmwareUpdateStage.sentRequestDelayed)
                        {
                            if (allSessionDevices[d].lastKnownReadTime > DateTime.UtcNow.AddSeconds(-10))
                                continue;
                        }
                        m.Add(d, allSessionDevices[d]);
                    }
                }
                var items = from pair in m
                            orderby pair.Value.configurationsRead, pair.Value.firmwareStage, pair.Value.firmwareUpdateStep ascending
                            select pair;

                foreach (var k in items)
                {
                    res.Add(k.Key);
                }

            }


            return res;
        }
        public List<string> getMCBsForFirmwareUpdate()
        {

            return getDevicesForFirmwareUpdate(DeviceTypes.mcb);

        }
        public List<string> getBattviewsForFirmwareUpdate()
        {

            return getDevicesForFirmwareUpdate(DeviceTypes.battview);
        }
        public List<string> getBattviewsMobileForFirmwareUpdate()
        {

            return getDevicesForFirmwareUpdate(DeviceTypes.battviewMobile);
        }
        #endregion

        #region download area
        public List<string> getChargersForDownload()
        {
            List<string> res = new List<string>();
            res.AddRange(getdevicesswithNoSaveConfig(DeviceTypes.mcb));
            res.AddRange(getDevicesswithNoLimit(DeviceTypes.mcb));
            res.AddRange(getMCBswithNoPMsPending());
            res.AddRange(getDeviceswithNoCyclesPending(DeviceTypes.mcb));

            return res;
        }
        public List<string> getBattviewsForDownload()
        {
            List<string> res = new List<string>();
            res.AddRange(getdevicesswithNoSaveConfig(DeviceTypes.battview));
            res.AddRange(getDevicesswithNoLimit(DeviceTypes.battview));
            res.AddRange(getDeviceswithNoCyclesPending(DeviceTypes.battview));

            return res;
        }
        public List<string> getBattviewMobileForDownload()
        {
            List<string> res = new List<string>();
            res.AddRange(getdevicesswithNoSaveConfig(DeviceTypes.battviewMobile));
            res.AddRange(getDevicesswithNoLimit(DeviceTypes.battviewMobile));
            res.AddRange(getDeviceswithNoCyclesPending(DeviceTypes.battviewMobile));

            return res;
        }
        private List<string> getdevicesswithNoSaveConfig(DeviceTypes type)
        {
            List<string> res = new List<string>();
            Dictionary<string, SiteViewDevice> m = new Dictionary<string, SiteViewDevice>();
            if (ControlObject.isHWMnafacturer)
                return res;
            lock (myLock)
            {
                foreach (string d in allSessionDevices.Keys)
                {

                    if (allSessionDevices[d].deviceType == type &&
                        allSessionDevices[d].deviceConnected &&
                        !allSessionDevices[d].configurationsSaved &&
                        allSessionDevices[d].id >= 10000 &&
                        !allSessionDevices[d].actViewEnabled)
                    {
                        m.Add(d, allSessionDevices[d]);
                    }
                }
                var items = from pair in m
                            orderby pair.Value.configurationsRead ascending
                            select pair;

                foreach (var k in items)
                {
                    res.Add(k.Key);
                }

            }


            return res;

        }

        private List<string> getDevicesswithNoLimit(DeviceTypes type)
        {
            List<string> res = new List<string>();
            Dictionary<string, SiteViewDevice> m = new Dictionary<string, SiteViewDevice>();
            if (ControlObject.isHWMnafacturer)
                return res;
            lock (myLock)
            {
                foreach (string d in allSessionDevices.Keys)
                {

                    if (allSessionDevices[d].deviceType == type &&
                        allSessionDevices[d].deviceConnected &&
                        allSessionDevices[d].configurationsSaved &&
                        allSessionDevices[d].id >= 10000 &&
                        !allSessionDevices[d].actViewEnabled &&
                        !allSessionDevices[d].limitsLoaded)
                    {
                        m.Add(d, allSessionDevices[d]);
                    }
                }
                var items = from pair in m
                            orderby pair.Value.limitsLoaded ascending
                            select pair;

                foreach (var k in items)
                {
                    res.Add(k.Key);
                }

            }


            return res;

        }
        private List<string> getMCBswithNoPMsPending()
        {
            List<string> res = new List<string>();
            Dictionary<string, SiteViewDevice> m = new Dictionary<string, SiteViewDevice>();
            if (!(ControlObject.AccessMCB && !ControlObject.isHWMnafacturer))
                return res;
            lock (myLock)
            {
                foreach (string d in allSessionDevices.Keys)
                {

                    if (allSessionDevices[d].deviceType == DeviceTypes.mcb &&
                        allSessionDevices[d].deviceConnected &&
                        allSessionDevices[d].configurationsSaved &&
                        allSessionDevices[d].id >= 10000 &&
                        !allSessionDevices[d].actViewEnabled &&
                        allSessionDevices[d].limitsLoaded &&
                        allSessionDevices[d].endPMID >= allSessionDevices[d].downloadPMID &&
                        !(allSessionDevices[d].startPMID == 0xFFFFFFFF || allSessionDevices[d].endPMID == 0xFFFFFFFF))
                    {
                        m.Add(d, allSessionDevices[d]);
                    }
                }

                var items = from pair in m
                            orderby pair.Value.endPMID - pair.Value.downloadPMID descending
                            select pair;

                foreach (var k in items)
                {
                    res.Add(k.Key);
                }

            }


            return res;

        }

        private List<string> getDeviceswithNoCyclesPending(DeviceTypes type)
        {
            List<string> res = new List<string>();
            Dictionary<string, SiteViewDevice> m = new Dictionary<string, SiteViewDevice>();
            if (!(!ControlObject.isHWMnafacturer))
                return res;
            lock (myLock)
            {
                foreach (string d in allSessionDevices.Keys)
                {

                    if (allSessionDevices[d].deviceType == type &&
                        allSessionDevices[d].deviceConnected &&
                        allSessionDevices[d].configurationsSaved &&
                        allSessionDevices[d].id >= 10000 &&
                        !allSessionDevices[d].actViewEnabled &&
                        allSessionDevices[d].limitsLoaded &&
                        (allSessionDevices[d].endPMID < allSessionDevices[d].downloadPMID || allSessionDevices[d].deviceType != DeviceTypes.mcb) &&
                        allSessionDevices[d].endEventID > allSessionDevices[d].downloadEventID
                        && !(allSessionDevices[d].startEventID == 0xFFFFFFFF || allSessionDevices[d].endEventID == 0xFFFFFFFF))
                    {
                        m.Add(d, allSessionDevices[d]);
                    }
                }
                var items = from pair in m
                            orderby pair.Value.endEventID - pair.Value.downloadEventID descending
                            select pair;

                foreach (var k in items)
                {
                    res.Add(k.Key);
                }

            }
            return res;
        }

        public void setDeviceConfigurationRead(string interfaceSN, bool isRead)
        {
            SiteViewDevice temp = getDevice(interfaceSN);

            if (temp == null)
                return;

            temp.setConfigurationsRead(isRead, 0, 0, 0, 0);
        }

        public void setDeviceConfigurationRead(string interfaceSN, bool isRead, byte dcId, float firmwareVersion, float firmwareWiFiVersion, float firmwareDcVersion)
        {
            SiteViewDevice temp = getDevice(interfaceSN);

            if (temp == null)
                return;

            temp.setConfigurationsRead(isRead, dcId, firmwareVersion, firmwareWiFiVersion, firmwareDcVersion);
        }

        public void setDeviceConfigurationSaved(string interfaceSN, bool isRead)
        {


            SiteViewDevice temp = getDevice(interfaceSN);
            if (temp == null)
            {

                return;
            }



            temp.configurationsSaved = isRead;

        }
        #endregion

        #region MCB LCD SIM AREA
        public void updateMCBLCDSimulator(string interfaceSN, LcdSimulator mcbProfile)
        {
            SiteViewDevice temp = getDevice(interfaceSN);
            if (temp.deviceType != DeviceTypes.mcb)
                return;
            if (temp == null)
            {

                return;
            }
            temp.updateMCBLCDSimulator(mcbProfile);

        }
        public LcdSimulator getMCBLCD(string interfaceSN)
        {
            SiteViewDevice temp = getDevice(interfaceSN);
            if (temp.deviceType != DeviceTypes.mcb)
                return null;
            if (temp == null)
            {

                return null;
            }

            return temp.getMCBLCD();

        }
        public List<string> getMCBsWithOldestSIM()
        {
            List<string> res = new List<string>();
            Dictionary<string, SiteViewDevice> m = new Dictionary<string, SiteViewDevice>();
            lock (myLock)
            {
                foreach (string d in allSessionDevices.Keys)
                {
                    if (allSessionDevices[d].deviceType == DeviceTypes.mcb && allSessionDevices[d].deviceConnected)
                    {
                        m.Add(d, allSessionDevices[d]);
                    }

                    var items = from pair in m
                                orderby pair.Value.lcdSimTime ascending
                                select pair;

                    foreach (var k in items)
                    {
                        res.Add(k.Key);
                    }
                }


            }


            return res;

        }

        #endregion
    }


}
