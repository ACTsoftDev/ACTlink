using System;
using System.Collections.Generic;
using System.Linq;
using SQLite;

namespace actchargers
{
    public class ReplaceDevicesLoaders : UploadableLoadersBase<ReplaceDevices>
    {
        public ReplaceDevicesLoaders(SQLiteConnection database) : base(database)
        {
        }

        public override List<ReplaceDevices> GetAll()
        {
            return database.Table<ReplaceDevices>().ToList();
        }

        public int DeleteUsingIds
        (UInt32 originalDeviceID, UInt32 newID, bool isMCB)
        {
            string deviceName = DeviceNameDeterminer.GetDeviceName(isMCB);

            var item = new ReplaceDevices()
            {
                OriginalDeviceID = originalDeviceID,
                NewDeviceID = newID,
                Name = deviceName
            };

            return Delete(item);
        }

        public void InsertOrUpdateMcbUsingFielfds
        (UInt32 originalDeviceID, string originalDeviceSN,
         UInt32 newDeviceID, string newDeviceSN)
        {
            InsertOrUpdateUsingFielfds
            (ACConstants.MCB, originalDeviceID, originalDeviceSN,
             newDeviceID, newDeviceSN);
        }

        public void InsertOrUpdateBattviewUsingFielfds
        (UInt32 originalDeviceID, string originalDeviceSN,
         UInt32 newDeviceID, string newDeviceSN)
        {
            InsertOrUpdateUsingFielfds
            (ACConstants.BATTVIEW, originalDeviceID, originalDeviceSN,
             newDeviceID, newDeviceSN);
        }

        public void InsertOrUpdateUsingFielfds
        (string deviceName, UInt32 originalDeviceID, string originalDeviceSN,
         UInt32 newDeviceID, string newDeviceSN)
        {
            var item = new ReplaceDevices()
            {
                OriginalDeviceID = originalDeviceID,
                OriginalDeviceSN = originalDeviceSN,
                NewDeviceID = newDeviceID,
                NewDeviceSN = newDeviceSN,
                Name = deviceName,
                IsUploaded = false
            };

            InsertOrUpdate(item);
        }

        public void MarkAsUploaded
        (UInt32 originalDeviceID, UInt32 newID, bool isMCB)
        {
            string deviceName = DeviceNameDeterminer.GetDeviceName(isMCB);

            var query =
                GetAll()
                    .Where(
                        item =>
                        item.OriginalDeviceID == originalDeviceID
                        && item.NewDeviceID == newID
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

        public void MarkAsSecret
        (UInt32 originalDeviceID, UInt32 newID, bool isMCB)
        {
            string deviceName = DeviceNameDeterminer.GetDeviceName(isMCB);

            var item = new ReplaceDevices()
            {
                OriginalDeviceID = originalDeviceID,
                NewDeviceID = newID,
                Name = deviceName,
                SecretUploaded = true
            };

            Update(item);
        }

        public override List<ReplaceDevices> GetNotUploaded()
        {
            return GetAll()
                .Where(item =>
                       item.IsUploaded == false
                      ).ToList();
        }

        public List<ReplaceDevices> GetSecretUploaded()
        {
            return GetAll()
                .Where(item =>
                       item.SecretUploaded == false
                      ).ToList();
        }
    }
}
