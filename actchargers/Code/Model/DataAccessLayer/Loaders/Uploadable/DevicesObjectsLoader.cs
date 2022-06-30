using System;
using System.Collections.Generic;
using System.Linq;
using SQLite;

namespace actchargers
{
    public class DevicesObjectsLoader : UploadableLoadersBase<DevicesObjects>
    {
        public DevicesObjectsLoader(SQLiteConnection database) : base(database)
        {
        }

        public override List<DevicesObjects> GetAll()
        {
            return database.Table<DevicesObjects>().ToList();
        }

        public void InsertOrUpdateDevice
        (bool isMCB, UInt32 id, string configJson, string globalRecordJson,
         int memorySignature, UInt32 eventsCount, float firmwareVersion, byte zone,
         UInt32 battviewStudyID, string studyName = "", string truckId = "")
        {
            string deviceName = DeviceNameDeterminer.GetDeviceName(isMCB);

            var item = new DevicesObjects()
            {
                Id = id,
                Name = deviceName,
                GlobalRecord = globalRecordJson,
                Config = configJson,
                IsUploaded = false,
                MemorySignature = (ushort)memorySignature,
                EventsCount = eventsCount,
                FirmwareVersion = firmwareVersion,
                Zone = zone.ToString(),
                BattviewStudyID = battviewStudyID
            };

            if (IsItemExistByIds(deviceName, id, battviewStudyID))
                Update(item);
            else
                InsertUsingFields(item, studyName, truckId);
        }

        public DevicesObjects GetDevice(UInt32 id, string deviceName)
        {
            return GetAll()
                .FirstOrDefault(item =>
                       item.Id == id
                       && item.Name == deviceName);
        }

        bool IsItemExistByIds(string deviceName, UInt32 id, UInt32 battviewStudyID)
        {
            return GetAll()
                .Any(item =>
                     item.Id == id
                     && item.BattviewStudyID == battviewStudyID
                     && item.Name == deviceName);
        }

        void InsertUsingFields
        (DevicesObjects item, string studyName, string truckId)
        {
            item.StudyName = studyName;
            item.TruckId = truckId;

            Insert(item);
        }

        public void MarkAsUploaded(bool isMCB, UInt32 id)
        {
            string deviceName = DeviceNameDeterminer.GetDeviceName(isMCB);

            var query =
                GetAll()
                        .Where(
                            item =>
                            item.Id == id
                            && item.Name == deviceName
                           )
                        .Select(item =>
                        {
                            item.IsUploaded = true;
                            return item;
                        }
                           );

            database.UpdateAll(query);
        }

        public void MarkAsUploaded(bool isMCB, UInt32 id, UInt32 battviewStudyID)
        {
            string deviceName = DeviceNameDeterminer.GetDeviceName(isMCB);

            var query =
                GetAll()
                        .Where(
                            item =>
                            item.Id == id
                            && item.BattviewStudyID == battviewStudyID
                            && item.Name == deviceName
                           )
                        .Select(item =>
                        {
                            item.IsUploaded = true;
                            return item;
                        }
                           );

            database.UpdateAll(query);
        }

        public override List<DevicesObjects> GetNotUploaded()
        {
            return GetAll()
                .Where(item =>
                       item.IsUploaded == false
                      ).ToList();
        }
    }
}
