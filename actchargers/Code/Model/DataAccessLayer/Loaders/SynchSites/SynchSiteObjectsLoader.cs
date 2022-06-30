using System;
using System.Collections.Generic;
using System.Linq;
using SQLite;

namespace actchargers
{
    public class SynchSiteObjectsLoader : LoadersBase<SynchSiteObjects>
    {
        List<SynchSiteObjects> backupData;

        public SynchSiteObjectsLoader(SQLiteConnection database) : base(database)
        {
        }

        public override List<SynchSiteObjects> GetAll()
        {
            return database.Table<SynchSiteObjects>().ToList();
        }

        public List<ACTViewSite> GetAllAsACTViewSiteList()
        {
            var all = database.Table<SynchSiteObjects>().ToList();

            return all.Select(item => new ACTViewSite(
                item.SiteName, item.SiteId, item.CustomerId, item.CustomerName)
                             ).ToList();
        }

        public void AddUsingFields
        (string siteName, string customerName, UInt32 siteId, UInt32 customerId)
        {
            var item = new SynchSiteObjects()
            {
                SiteName = siteName,
                CustomerName = customerName,
                SiteId = siteId,
                CustomerId = customerId
            };

            InsertOrUpdate(item);
        }

        public bool HasSites()
        {
            return GetAll().Any();
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
