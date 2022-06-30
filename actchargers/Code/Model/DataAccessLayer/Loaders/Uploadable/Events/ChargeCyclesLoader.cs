using System;
using System.Collections.Generic;
using System.Linq;
using SQLite;

namespace actchargers
{
    public class ChargeCyclesLoader : UploadableLoadersBase<ChargeCycles>
    {
        List<ChargeCycles> backupData;

        public ChargeCyclesLoader(SQLiteConnection database) : base(database)
        {
        }

        public override List<ChargeCycles> GetAll()
        {
            return database.Table<ChargeCycles>().ToList();
        }

        public void MarkAsUploaded(UInt32 chargerId, UInt32[] eventIds)
        {
            var query =
                GetAll()
                        .Where(item =>
                               item.Id == chargerId
                               && eventIds.Contains(item.EventId)
                           )
                        .Select(item =>
                        {
                            item.IsUploaded = true;
                            item.EventText = "";
                            return item;
                        }
                               );

            database.UpdateAll(query);
        }

        public void DeleteUsingIds(UInt32 chargerId)
        {
            var toDelete =
                GetAll()
                    .FirstOrDefault(
                        item =>
                        item.Id == chargerId
                    );

            Delete(toDelete);
        }

        public void DeleteAllById(UInt32 chargerId)
        {
            var query =
                database.Table<ChargeCycles>()
                        .Where(item =>
                               item.Id == chargerId
                              ).ToList();

            foreach (ChargeCycles item in query)
                Delete(item);
        }

        public void InsertUsingFields
        (UInt32 chargerId, UInt32 eventId, string eventJson)
        {
            ChargeCycles toInsert = new ChargeCycles()
            {
                EventText = eventJson,
                EventId = eventId,
                Id = chargerId,
                IsUploaded = false
            };

            InsertOrUpdate(toInsert);
        }

        public Dictionary<UInt32, string> GetSortedNotUploaded
        (UInt32 chargerId, int batchSize)
        {
            var query =
                GetAll()
                        .Where(item =>
                               item.Id == chargerId
                               && item.IsUploaded == false
                              ).OrderBy(item => item.EventId)
                        .Take(batchSize)
                        .ToList();

            Dictionary<UInt32, string> all = new Dictionary<uint, string>();

            foreach (var item in query)
            {
                if (!all.ContainsKey(item.EventId))
                    all.Add(item.EventId, item.EventText);
            }

            return all;
        }

        public void GetLimits
        (ref UInt32 minId, ref UInt32 maxId, UInt32 chargerId)
        {
            var query =
                GetAll()
                        .Where(item =>
                               item.Id == chargerId
                              ).OrderBy(item => item.EventId);

            if (query.Any())
            {
                WriteLimitsValues(query, ref minId, ref maxId);
            }
            else
            {
                WriteLimitsDefaults(ref minId, ref maxId);
            }
        }

        void WriteLimitsValues
        (IOrderedEnumerable<ChargeCycles> query, ref UInt32 minId, ref UInt32 maxId)
        {
            var first = query.First();
            var last = query.Last();

            minId = first.Id;
            maxId = last.Id;
        }

        void WriteLimitsDefaults
        (ref UInt32 minId, ref UInt32 maxId)
        {
            minId = 0;
            maxId = 0;
        }

        public List<ChargeCycles> GetIdAndMaxEventIdList()
        {
            var list = new List<ChargeCycles>();

            var query =
                GetAll().GroupBy(item => item.Id);

            foreach (var groupItem in query)
            {
                var max = groupItem.OrderBy(item => item.EventId).LastOrDefault();
                list.Add(max);
            }

            return list;
        }

        public override List<ChargeCycles> GetNotUploaded()
        {
            return GetAll()
                .Where(item =>
                       item.IsUploaded == false
                      ).ToList();
        }

        public void Backup()
        {
            backupData = GetIdAndMaxEventIdList();
        }

        public void Restore()
        {
            if (backupData != null)
                InsertAll(backupData);
        }
    }
}
