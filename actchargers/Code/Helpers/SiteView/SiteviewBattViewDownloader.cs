using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using actchargers.Code.Utility;

namespace actchargers
{
    public class SiteviewBattViewDownloader : SiteviewDeviceDownloader
    {
        const int MAX_BATTVIEW_DOWNLOADS = 2000;

        internal override async Task<bool> TryDownloadDevice(SiteViewDevice siteViewDevice)
        {
            string serialNumber = siteViewDevice.interfaceSN;

            DeviceBattViewObject genericDevice =
                SiteViewQuantum.Instance.GetConnectionManager()
                               .managedBATTViews.getDeviceByKey(serialNumber);

            var device = genericDevice.battview;

            if (siteViewDevice == null)
                return false;

            if (genericDevice == null)
                return false;

            if (genericDevice.flagOfRemoval)
                return false;

            uint id = genericDevice.battview.Config.id;
            bool isMobile = genericDevice.battview.Config.IsBattViewMobile();
            uint knownStudyId = 0;

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

                if (device.Config.IsBattViewMobile())
                    knownStudyId = device.Config.studyId;
                else
                    knownStudyId = 0;

                ProgressCompleted = 15;

                await device.ReadConfig();

                ProgressCompleted = 25;
            }
            else
            {
                ProgressCompleted = 25;
            }

            await device.ReadRecordsLimits();

            InsertOrUpdateBattview(device.Config, device.globalRecord);

            if (device.Config.ActViewEnabled)
                return false;

            uint cyclesStartId = 0;
            uint minID = 0;

            uint min_time = 0;

            DbSingleton.DBManagerServiceInstance
                       .GetBattviewEventsLoader()
                       .GetLimits
                       (ref minID, ref cyclesStartId, ref min_time, id, knownStudyId);

            if (cyclesStartId == 0 || cyclesStartId < device.eventsRecordsStartID)
                cyclesStartId = device.eventsRecordsStartID;
            else
                cyclesStartId++;

            if (device.globalRecord.eventsCount - cyclesStartId > MAX_BATTVIEW_DOWNLOADS)
            {
                DbSingleton.DBManagerServiceInstance.GetBattviewEventsLoader()
                           .DeleteUsingIds(knownStudyId, id);

                cyclesStartId = device.globalRecord.eventsCount - MAX_BATTVIEW_DOWNLOADS;
            }

            siteViewDevice.setLimits(cyclesStartId, device.globalRecord.eventsCount, 0, 0);

            ProgressCompleted = 35;

            uint endEventID = siteViewDevice.endEventID;
            uint downloadEventID = siteViewDevice.downloadEventID;

            if (downloadEventID < endEventID)
            {
                List<BattViewObjectEvent> events = await Task.Run(async () =>
                {
                    return await device.readEvents_synchRightx(downloadEventID, endEventID);
                }, cancellationToken);

                var toInsertEvents = new List<BattviewEvents>();

                foreach (BattViewObjectEvent ev in events)
                {
                    string stringEvent = new BATTViewEvent(ev, Convert.ToBoolean(device.Config.temperatureFormat)).toJSON();

                    if (downloadEventID != ev.eventID && ev.valid)
                    {
                        throw new Exception("Right,Batt,ID," + ev.eventID.ToString() + "," + downloadEventID.ToString());
                    }

                    var toInsert = new BattviewEvents()
                    {
                        EventText = stringEvent,
                        EventId = downloadEventID,
                        Id = id,
                        IsUploaded = false,
                        OriginalStartTime = ev.original_start_time,
                        BattviewStudyID = knownStudyId.ToString()
                    };

                    toInsertEvents.Add(toInsert);

                    downloadEventID++;
                    siteViewDevice.downloadEventID = downloadEventID;
                }

                InsertEvent(toInsertEvents);
            }

            return true;
        }

        void InsertOrUpdateBattview
        (BattViewConfig conf, BattViewObjectGlobalRecord globalRecord)
        {
            bool isMcb = false;
            uint id = conf.id;
            string configJson = JsonParser.SerializeObject(conf);
            string globalRecordJson = globalRecord.ToJson();
            int memorySignature = conf.memorySignature;
            uint eventsCount = globalRecord.eventsCount;
            float firmwareVersion = conf.firmwareversion;
            byte zone = conf.zoneid;
            uint battviewStudyID = conf.studyId;
            string studyName = conf.studyName;
            string truckid = conf.TruckId;
            string name = conf.batteryID;

            DbSingleton.DBManagerServiceInstance
                       .GetDevicesObjectsLoader()
                       .InsertOrUpdateDevice
                       (isMcb, id, configJson, globalRecordJson,
                        memorySignature, eventsCount,
                        firmwareVersion, zone, battviewStudyID,
                        studyName, truckid);
        }

        void InsertEvent(List<BattviewEvents> toInsertEvents)
        {
            if (toInsertEvents.Count > 0)
                DoInsertEvent(toInsertEvents);
        }

        void DoInsertEvent(List<BattviewEvents> toInsertEvents)
        {
            DbSingleton
                .DBManagerServiceInstance
                .GetBattviewEventsLoader()
                .InsertAll(toInsertEvents);
        }
    }
}
