using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace actchargers
{
    public class SiteviewChargerDownloader : SiteviewDeviceDownloader
    {
        internal override async Task<bool> TryDownloadDevice(SiteViewDevice siteViewDevice)
        {
            string serialNumber = siteViewDevice.interfaceSN;

            var genericDevice =
                SiteViewQuantum.Instance.GetConnectionManager()
                               .managedMCBs.getDeviceByKey(serialNumber);

            var device = genericDevice.mcb;

            if (siteViewDevice == null)
                return false;

            if (genericDevice == null)
                return false;

            if (genericDevice.flagOfRemoval)
                return false;

            UInt32 id = UInt32.Parse(device.Config.id);

            if (!siteViewDevice.configurationsRead)
            {
                bool IsLoaded = await Task.Run(async () =>
                {
                    return await device.DoLoad();
                }, cancellationToken);

                if (IsLoaded)
                    SiteViewQuantum.Instance.GetConnectionManager().siteView.setDeviceConfigurationRead(serialNumber, true, device.DcId, device.FirmwareRevision, device.FirmwareWiFiVersion, device.FirmwareDcVersion);
                else
                    return false;

                ProgressCompleted = 15;

                await device.ReadConfig();

                ProgressCompleted = 25;
            }
            else
            {
                ProgressCompleted = 25;
            }

            await device.ReadRecordslimits();

            InsertOrUpdateMcb(device);

            if (device.Config.actViewEnable)
                return false;

            UInt32 cyclesStartId = 0;
            uint minID = 0;

            DbSingleton.DBManagerServiceInstance
                       .GetChargeCyclesLoader()
                       .GetLimits
                       (ref minID, ref cyclesStartId, id);

            if (cyclesStartId < device.minChargeRecordID)
                cyclesStartId = device.minChargeRecordID;

            uint pmsStartId = 0;

            DbSingleton.DBManagerServiceInstance
                       .GetPMFaultsLoader()
                       .GetLimits
                       (ref minID, ref pmsStartId, id);

            pmsStartId += 1;

            if (pmsStartId < device.minPMFaultRecordID)
                pmsStartId = device.minPMFaultRecordID;

            siteViewDevice
                .setLimits
                (cyclesStartId, device.globalRecord.chargeCycles, pmsStartId,
                 device.globalRecord.PMfaults);

            ProgressCompleted = 35;

            UInt32 endPMID = siteViewDevice.endPMID;
            UInt32 downloadPMID = siteViewDevice.downloadPMID;

            if (downloadPMID <= endPMID)
            {
                List<PMfault> events = await Task.Run(async () =>
                {
                    return await device.readPMs_synchRight(downloadPMID);
                }, cancellationToken);

                var toInsertPms = new List<PmFaults>();

                foreach (PMfault ev in events)
                {
                    string stringEvent = ev.TOJSON();

                    if (downloadPMID != ev.faultID && ev.isValidCRC7)
                    {
                        throw new Exception("Right,PM MCB,ID," + ev.faultID.ToString() + "," + downloadPMID.ToString());
                    }

                    var toInsert = new PmFaults()
                    {
                        EventText = stringEvent,
                        EventId = downloadPMID,
                        Id = id,
                        IsUploaded = false
                    };
                    toInsertPms.Add(toInsert);

                    downloadPMID++;
                    siteViewDevice.downloadPMID = downloadPMID;
                }

                InsertPmsList(toInsertPms);
            }

            UInt32 downloadEventID = siteViewDevice.downloadEventID;
            UInt32 endEventID = siteViewDevice.endEventID;

            if (downloadEventID < endEventID)
            {
                List<ChargeRecord> events = await Task.Run(async () =>
                {
                    return await device.readchargeCycles_synchRightx(downloadEventID, endEventID);
                }, cancellationToken);

                var toInsertChargeCycles = new List<ChargeCycles>();

                foreach (ChargeRecord ev in events)
                {
                    string stringEvent = ev.ToJson();
                    if (downloadEventID + 1 != ev.cycleID && ev.isValidCRC7)
                    {
                        throw new Exception("Right,MCB,ID," + ev.cycleID.ToString() + "," + downloadEventID.ToString());
                    }

                    if ((ev.status != 0xAA || downloadEventID != endEventID - 1))
                    {
                        var toInsert = new ChargeCycles()
                        {
                            EventText = stringEvent,
                            EventId = downloadEventID,
                            Id = id,
                            IsUploaded = false
                        };
                        toInsertChargeCycles.Add(toInsert);
                    }
                    else
                    {
                        if (ev.status == 0xAA)
                        {
                            downloadEventID--;
                            siteViewDevice.endEventID = downloadEventID + 1;
                        }
                    }

                    downloadEventID++;
                    siteViewDevice.downloadEventID = downloadEventID;
                }

                InsertChargeCycles(toInsertChargeCycles);
            }

            return true;
        }

        void InsertOrUpdateMcb(MCBobject device)
        {
            bool isMcb = true;

            var globalRecord = device.globalRecord;
            var mCBConfig = device.Config;

            uint id = uint.Parse(mCBConfig.id);
            string configJson = device.GetConfigWithPMsJson();
            string globalRecordJson = globalRecord.TOJSON();
            ushort memorySignature = ushort.Parse(mCBConfig.memorySignature);
            uint eventsCount = globalRecord.chargeCycles;
            float firmwareVersion = mCBConfig.firmwareVersion;
            byte zone = mCBConfig.zoneID;
            uint battviewStudyID = 0;

            DbSingleton.DBManagerServiceInstance
                       .GetDevicesObjectsLoader()
                       .InsertOrUpdateDevice
                       (isMcb, id, configJson, globalRecordJson,
                        memorySignature, eventsCount,
                        firmwareVersion, zone, battviewStudyID);
        }

        void InsertPmsList(List<PmFaults> toInsertPms)
        {
            if (toInsertPms.Count > 0)
                DoInsertPmsList(toInsertPms);
        }

        void DoInsertPmsList(List<PmFaults> toInsertPms)
        {
            DbSingleton
                .DBManagerServiceInstance
                .GetPMFaultsLoader()
                .InsertAll(toInsertPms);
        }

        void InsertChargeCycles(List<ChargeCycles> toInsertChargeCycles)
        {
            if (toInsertChargeCycles.Count > 0)
                DoInsertChargeCycles(toInsertChargeCycles);
        }

        void DoInsertChargeCycles(List<ChargeCycles> toInsertChargeCycles)
        {
            DbSingleton
                .DBManagerServiceInstance
                .GetChargeCyclesLoader()
                .InsertAll(toInsertChargeCycles);
        }
    }
}
