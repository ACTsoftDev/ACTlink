using System;
using System.Collections.Generic;
using System.Linq;
using MvvmCross.Platform;
using SQLite;

namespace actchargers
{
    public class GenericObjectsLoader : LoadersBase<GenericObjects>
    {
        const string LATEST_FULL_UPLOAD_DATE = "LATEST_FULL_UPLOAD_TIME";
        const string LATEST_SYNCH_SITE_DATE = "LATEST_SYNCH_SITE_DATE";
        const string ENABLE_USB = "Enable_USB";
        const string NO = "NO";
        const string AGREEMENT = "Agreement_{0}";

        List<GenericObjects> backupData;

        public GenericObjectsLoader(SQLiteConnection database) : base(database)
        {
        }

        public override List<GenericObjects> GetAll()
        {
            return database.Table<GenericObjects>().ToList();
        }

        public void SetLastUploadDate()
        {
            long nowLong = DateTime.Now.Ticks / 1000;

            InsertOrUpdateUsingFields(LATEST_FULL_UPLOAD_DATE,
                                      nowLong.ToString());
        }

        public long GetLastUploadDate()
        {
            string val = GetValueOrDefault(LATEST_FULL_UPLOAD_DATE);

            if (!long.TryParse(val, out long time))
            {
                return 0;
            }

            return time * 1000;
        }

        public void SetLastSynchSiteDate()
        {
            long nowLong = DateTime.Now.Ticks / 1000;

            InsertOrUpdateUsingFields(LATEST_SYNCH_SITE_DATE,
                                      nowLong.ToString());
        }

        public long GetLastSynchSiteDate()
        {
            string val = GetValueOrDefault(LATEST_SYNCH_SITE_DATE);

            if (!long.TryParse(val, out long time))
            {
                return 0;
            }

            return time * 1000;
        }

        public void DisableUSB()
        {
            InsertOrUpdateUsingFields(ENABLE_USB, NO);
        }

        public bool IsUsbEnabled()
        {
            return !IsUsbDisabled();
        }

        bool IsUsbDisabled()
        {
            string val = GetValueOrDefault(ENABLE_USB);

            return val == NO;
        }

        public void SetAsAgreed()
        {
            int userId = Mvx.Resolve<IUserPreferences>()
                            .GetInt(ACConstants.USER_PREFS_USER_ID);
            string agreementKey = string.Format(AGREEMENT, userId);

            string nowText = DateTime.UtcNow.ToString();

            InsertOrUpdateUsingFields(agreementKey, nowText);
        }

        public void InsertOrUpdateUsingFields(string type, string val)
        {
            var item = new GenericObjects()
            {
                Type = type,
                Value = val
            };

            InsertOrUpdate(item);
        }

        public string GetValueOrDefault(string type)
        {
            var query =
                GetAll()
                    .FirstOrDefault(
                        item =>
                        item.Type == type
                    );

            if (query == null)
                return "";
            else
                return query.Value;
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
