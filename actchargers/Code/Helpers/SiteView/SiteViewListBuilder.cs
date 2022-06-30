using System;
using System.Collections.Generic;
using System.Linq;

namespace actchargers
{
    public class SiteViewListBuilder
    {
        readonly uint siteId;
        readonly bool isWithSynchSites;

        SessionDevices siteView;
        ConnectionManager connectionManager;

        List<string> connectedDevicesSourceList;
        List<SynchObjectsBufferedData> synchSiteDevicesSourceList;

        public SiteViewListBuilder(uint siteId, bool isWithSynchSites)
        {
            this.siteId = siteId;
            this.isWithSynchSites = isWithSynchSites;

            Init();
        }

        void Init()
        {
            siteView = SiteViewQuantum.Instance.GetConnectionManager().siteView;
            connectionManager = SiteViewQuantum.Instance.GetConnectionManager();
        }

        #region Lists

        public List<SiteViewDeviceObject> GetFullList()
        {
            List<SiteViewDeviceObject> fullList;

            if (isWithSynchSites)
                fullList = GetFullListWithSynchSites();
            else
                fullList = GetFullListWithoutSynchSites();

            return fullList;
        }

        List<SiteViewDeviceObject> GetFullListWithSynchSites()
        {
            connectedDevicesSourceList = GetConnectedDevicesSourceList();
            synchSiteDevicesSourceList = GetSynchSiteDevicesSourceList();

            List<SiteViewDeviceObject> connectedDevicesList =
                GetConnectedDevicesList();
            List<SiteViewDeviceObject> synchSiteDevicesList =
                GetSynchSiteDevicesList();

            List<SiteViewDeviceObject> fullList = ConcatLists
                (connectedDevicesList, synchSiteDevicesList);

            return fullList;
        }

        List<SiteViewDeviceObject> GetFullListWithoutSynchSites()
        {
            connectedDevicesSourceList = GetConnectedDevicesSourceList();

            List<SiteViewDeviceObject> connectedDevicesList =
                GetConnectedDevicesList();

            return connectedDevicesList;
        }

        List<string> GetConnectedDevicesSourceList()
        {
            List<string> devices = siteView.getDevicesInterfaceSNs();

            return devices;
        }

        List<SynchObjectsBufferedData> GetSynchSiteDevicesSourceList()
        {
            return DbSingleton
                .DBManagerServiceInstance
                .GetSynchObjectsBufferedDataLoader()
                .GetAllBySiteId(siteId);
        }

        List<SiteViewDeviceObject> ConcatLists
        (List<SiteViewDeviceObject> connectedDevicesList,
         List<SiteViewDeviceObject> synchSiteDevicesLis)
        {
            List<SiteViewDeviceObject> fullList =
                new List<SiteViewDeviceObject>();

            fullList.AddRange(connectedDevicesList);
            fullList.AddRange(synchSiteDevicesLis);

            return fullList;
        }

        #endregion

        #region Connected Devices

        List<SiteViewDeviceObject> GetConnectedDevicesList()
        {
            List<SiteViewDeviceObject> connectedDevicesList =
                new List<SiteViewDeviceObject>();

            foreach (string item in connectedDevicesSourceList)
                connectedDevicesList.Add(GetConnectedDevice(item));

            return connectedDevicesList;
        }

        SiteViewDeviceObject GetConnectedDevice(string serialNumber)
        {
            SiteViewDevice siteViewDevice = siteView.getDevice(serialNumber);

            SiteViewDeviceObject siteViewDeviceObject =
                CreateConnectedSiteViewDeviceObject(siteViewDevice);

            SetIfNotSite(siteViewDeviceObject);

            return siteViewDeviceObject;
        }

        SiteViewDeviceObject CreateConnectedSiteViewDeviceObject(SiteViewDevice siteViewDevice)
        {
            SiteViewDeviceObject siteViewDeviceObject = null;

            switch (siteViewDevice.deviceType)
            {
                case DeviceTypes.mcb:
                    siteViewDeviceObject =
                        ConvertToSiteViewMcb(siteViewDevice, false);

                    break;

                case DeviceTypes.battview:
                    siteViewDeviceObject =
                        ConvertToSiteViewBattview(siteViewDevice, false);

                    break;

                case DeviceTypes.battviewMobile:
                    siteViewDeviceObject =
                        ConvertToSiteViewBattview(siteViewDevice, false);

                    break;
            }

            RefreshDevice(siteViewDeviceObject, false);

            return siteViewDeviceObject;
        }

        void SetIfNotSite(SiteViewDeviceObject siteViewDeviceObject)
        {
            bool notFoundInSynchSite;

            if (isWithSynchSites)
                notFoundInSynchSite = NotFoundInSynchSite(siteViewDeviceObject);
            else
                notFoundInSynchSite = false;

            siteViewDeviceObject.NotSite = notFoundInSynchSite;
        }

        bool NotFoundInSynchSite(SiteViewDeviceObject siteViewDeviceObject)
        {
            string serialNumber = siteViewDeviceObject.serialNumber;

            bool found = synchSiteDevicesSourceList.Any(
                (arg) =>
                arg.SerialNumber == serialNumber);

            return !(found);
        }

        #endregion

        #region SynchSite Devices

        List<SiteViewDeviceObject> GetSynchSiteDevicesList()
        {
            List<SiteViewDeviceObject> synchSiteDevicesList =
                new List<SiteViewDeviceObject>();

            List<SynchObjectsBufferedData> synchSiteDevicesDistinct =
                GetSynchSiteDevicesDistinct();

            foreach (var item in synchSiteDevicesDistinct)
                synchSiteDevicesList.Add(GetSiteViewDevice(item));

            return synchSiteDevicesList;
        }

        List<SynchObjectsBufferedData> GetSynchSiteDevicesDistinct()
        {
            List<SynchObjectsBufferedData> synchSiteDevicesDistinct =
                new List<SynchObjectsBufferedData>();

            foreach (var item in synchSiteDevicesSourceList)
            {
                if (NotFoundInConnectedDevices(item))
                    synchSiteDevicesDistinct.Add(item);
            }

            return synchSiteDevicesDistinct;
        }

        bool NotFoundInConnectedDevices
        (SynchObjectsBufferedData item)
        {
            bool isFound = connectedDevicesSourceList.Contains(item.InterfaceSn);

            return !(isFound);
        }

        SiteViewDeviceObject GetSiteViewDevice
        (SynchObjectsBufferedData synchObjectsBufferedData)
        {
            SiteViewDeviceObject siteViewDeviceObject = null;

            bool isSynchSiteDevice =
                GetIsSiteViewDevice(synchObjectsBufferedData.InterfaceSn);

            switch (synchObjectsBufferedData.GetDeviceType())
            {
                case DeviceType.MCB:
                    siteViewDeviceObject =
                        ConvertToSiteViewMcb(synchObjectsBufferedData, isSynchSiteDevice);

                    break;

                case DeviceType.BATTVIEW:
                    siteViewDeviceObject =
                        ConvertToSiteViewBattview(synchObjectsBufferedData, isSynchSiteDevice);

                    break;

                case DeviceType.BATTVIEW_MOBILE:
                    siteViewDeviceObject =
                        ConvertToSiteViewBattview(synchObjectsBufferedData, isSynchSiteDevice);

                    break;
            }

            RefreshDevice(siteViewDeviceObject, true);

            return siteViewDeviceObject;
        }

        bool GetIsSiteViewDevice(string serialNumber)
        {
            bool isFoundInInterfaceSNs =
                connectedDevicesSourceList.Any(
                    (arg) =>
                    arg.Equals(serialNumber, StringComparison.OrdinalIgnoreCase));

            return !(isFoundInInterfaceSNs);
        }

        #endregion

        #region Converters

        SiteViewDeviceObject ConvertToSiteViewMcb
        (SiteViewDevice siteViewDevice, bool isSynchSiteDevice)
        {
            var siteViewDeviceObject =
                new SiteViewChargerObject
                (siteViewDevice.serialnumber,
                 siteViewDevice.interfaceSN,
                 siteViewDevice.id, isSynchSiteDevice,
                 siteViewDevice.userNamed)
                {
                    actViewEnabled = siteViewDevice.actViewEnabled,
                    IsConnected = siteViewDevice.deviceConnected
                };

            return siteViewDeviceObject;
        }

        SiteViewDeviceObject ConvertToSiteViewMcb
        (SynchObjectsBufferedData synchObjectsBufferedData, bool isSynchSiteDevice)
        {
            var siteViewDeviceObject =
                new SiteViewChargerObject
                (synchObjectsBufferedData.SerialNumber,
                 synchObjectsBufferedData.InterfaceSn,
                 synchObjectsBufferedData.Id, isSynchSiteDevice,
                 synchObjectsBufferedData.DeviceName)
                {
                    IsConnected = false
                };

            return siteViewDeviceObject;
        }

        SiteViewDeviceObject ConvertToSiteViewBattview
        (SiteViewDevice siteViewDevice, bool isSynchSiteDevice)
        {
            var siteViewDeviceObject =
                new SiteViewBattviewObject
                (siteViewDevice.serialnumber,
                 siteViewDevice.interfaceSN,
                 siteViewDevice.id, isSynchSiteDevice,
                 siteViewDevice.userNamed,
                 siteViewDevice.deviceType, 0)
                {
                    actViewEnabled = siteViewDevice.actViewEnabled,
                    IsConnected = siteViewDevice.deviceConnected
                };

            return siteViewDeviceObject;
        }

        SiteViewDeviceObject ConvertToSiteViewBattview
        (SynchObjectsBufferedData synchObjectsBufferedData, bool isSynchSiteDevice)
        {
            var siteViewDeviceObject =
                new SiteViewBattviewObject
                (synchObjectsBufferedData.SerialNumber,
                 synchObjectsBufferedData.InterfaceSn,
                 synchObjectsBufferedData.Id, isSynchSiteDevice,
                 synchObjectsBufferedData.DeviceName,
                 DeviceTypes.battview, 0)
                {
                    IsConnected = false
                };

            return siteViewDeviceObject;
        }

        #endregion

        void RefreshDevice
        (SiteViewDeviceObject siteViewDeviceObject, bool siteOverlay)
        {
            if (!siteOverlay)
                siteViewDeviceObject.CheckForUpdate();

            siteViewDeviceObject.SimplyUpdateUi();
        }
    }
}
