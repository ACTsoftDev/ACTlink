using System.Collections.Generic;
using System.Linq;
using SQLite;

namespace actchargers
{
    public class FirmwareDownloadedLoader : LoadersBase<FirmwareDownloaded>
    {
        FirmwareDownloaded backupData;

        public FirmwareDownloadedLoader(SQLiteConnection database) : base(database)
        {
        }

        public override List<FirmwareDownloaded> GetAll()
        {
            return database.Table<FirmwareDownloaded>().ToList();
        }

        public float GetMaxFirmwareVersion(bool isMCB)
        {
            string deviceName = DeviceNameDeterminer.GetDeviceName(isMCB);

            var query =
                GetAll()
                        .Where(item =>
                               item.Name == deviceName
                              )
                        .OrderBy(item => item.Version);

            var last = query.Last();
            if (last == null)
                return 0.0f;
            else
                return float.Parse(last.Version);
        }

        public string GetFirmwareByVersion(bool isMCB, float version)
        {
            string deviceName = DeviceNameDeterminer.GetDeviceName(isMCB);

            var query =
                GetAll()
                        .Where(item =>
                               item.Name == deviceName
                               && item.Version == version.ToString()
                              );

            var first = query.First();
            if (first == null)
                return "";
            else
                return first.FirmwareFile;
        }

        public void InsertOrUpdateUsingFields
        (bool isMCB, float version, string hexFile)
        {
            string deviceName = DeviceNameDeterminer.GetDeviceName(isMCB);

            var item = new FirmwareDownloaded()
            {
                Name = deviceName,
                Version = version.ToString(),
                FirmwareFile = hexFile
            };

            InsertOrUpdate(item);
        }

        public FirmwareDownloaded GetMax()
        {
            return GetAll().OrderBy(item => float.Parse(item.Version)).LastOrDefault();
        }

        public void Backup()
        {
            backupData = GetMax();
        }

        public void Restore()
        {
            if (backupData != null)
                InsertOrUpdate(backupData);
        }
    }
}
