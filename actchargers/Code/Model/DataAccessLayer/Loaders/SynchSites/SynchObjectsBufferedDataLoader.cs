using System;
using System.Collections.Generic;
using System.Linq;
using SQLite;

namespace actchargers
{
    public class SynchObjectsBufferedDataLoader : LoadersBase<SynchObjectsBufferedData>
    {
        List<SynchObjectsBufferedData> backupData;

        public SynchObjectsBufferedDataLoader(SQLiteConnection database) : base(database)
        {
        }

        public override List<SynchObjectsBufferedData> GetAll()
        {
            return database.Table<SynchObjectsBufferedData>().ToList();
        }

        public List<SynchObjectsBufferedData> GetAllBySiteId(uint siteId)
        {
            return GetAll()
                .Where((arg) =>
                       arg.SiteId == siteId)
                .ToList();
        }

        public List<SynchObjectsBufferedData> GetSynchedDevices
        (bool isMCB, UInt32 id = 0)
        {
            if (id == 0)
            {
                return GetAll();
            }
            else
            {
                return GetSynchedDevicesByFields(isMCB, id);
            }
        }

        List<SynchObjectsBufferedData> GetSynchedDevicesByFields
        (bool isMCB, uint id)
        {
            string deviceName = DeviceNameDeterminer.GetDeviceName(isMCB);

            return GetAll()
                           .Where(item =>
                                  item.Id == id
                                  && item.DeviceTypeName == deviceName
                                 ).ToList();
        }

        public List<SynchObjectsBufferedData> GetChargers(uint siteId)
        {
            return GetDevicesByType(DeviceType.MCB, siteId);
        }

        public List<SynchObjectsBufferedData> GetBattviews(uint siteId)
        {
            return GetDevicesByType(DeviceType.BATTVIEW, siteId);
        }

        public List<SynchObjectsBufferedData> GetBattviewMobiles(uint siteId)
        {
            return GetDevicesByType(DeviceType.BATTVIEW_MOBILE, siteId);
        }

        List<SynchObjectsBufferedData> GetDevicesByType
        (DeviceType deviceType, uint siteId)
        {
            List<SynchObjectsBufferedData> all;
            if (siteId == 0)
                all = GetAll();
            else
                all = GetAllBySiteId(siteId);

            var list = all
                .Where(item =>
                       item.GetDeviceType() == deviceType
                      ).ToList();

            AddSynchSiteObjectsToList(list);

            return list;
        }

        void AddSynchSiteObjectsToList(List<SynchObjectsBufferedData> list)
        {
            var sites = DbSingleton
                .DBManagerServiceInstance
                .GetSynchSiteObjectsLoader()
                .GetAll();

            foreach (var item in list)
            {
                AddSynchSiteObjectToItem(item, sites);
            }
        }

        void AddSynchSiteObjectToItem
        (SynchObjectsBufferedData item, List<SynchSiteObjects> sites)
        {
            var site = sites
                .FirstOrDefault(i =>
                       i.SiteId == item.SiteId
                               );

            item.SynchSite = site;
        }

        public void Backup()
        {
            backupData = GetAll();
        }

        public void Restore()
        {
            if (backupData != null)
                InsertAll(backupData);
        }
    }
}
