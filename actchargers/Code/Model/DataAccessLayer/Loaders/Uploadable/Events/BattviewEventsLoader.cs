using System;
using System.Collections.Generic;
using System.Linq;
using SQLite;

namespace actchargers
{
    public class BattviewEventsLoader : UploadableLoadersBase<BattviewEvents>
    {
        List<BattviewEvents> backupData;

        public BattviewEventsLoader(SQLiteConnection database) : base(database)
        {
        }

        public override List<BattviewEvents> GetAll()
        {
            return database.Table<BattviewEvents>().ToList();
        }

        public void MarkAsUploaded
        (UInt32 battViewID, UInt32[] eventIds, UInt32 studyId)
        {
            var query =
                GetAll()
                        .Where(item =>
                               item.Id == battViewID
                               && eventIds.Contains(item.EventId)
                               && item.BattviewStudyID == studyId.ToString()
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

        public void DeleteUsingIds(UInt32 studyId, UInt32 battViewID)
        {
            var toDelete =
                GetAll()
                    .FirstOrDefault(
                        item =>
                        item.Id == battViewID &&
                        item.BattviewStudyID == studyId.ToString()
                    );

            database.Delete<BattviewEvents>(toDelete.Key);
        }

        public void DeleteAllById(UInt32 battViewID)
        {
            var query =
                GetAll()
                        .Where(item =>
                               item.Id == battViewID
                              ).ToList();

            foreach (BattviewEvents item in query)
                Delete(item);
        }

        public void InsertUsingFields
        (UInt32 battViewID, UInt32 eventId, string eventJson,
         UInt32 originalStartTime, UInt32 studyId)
        {
            BattviewEvents toInsert = new BattviewEvents()
            {
                EventText = eventJson,
                EventId = eventId,
                Id = battViewID,
                IsUploaded = false,
                OriginalStartTime = originalStartTime,
                BattviewStudyID = studyId.ToString()
            };

            InsertOrUpdate(toInsert);
        }

        public Dictionary<UInt32, string> GetSortedNotUploaded
        (UInt32 battViewID, int batchSize, UInt32 studyId)
        {
            var query =
                GetAll()
                        .Where(item =>
                               item.Id == battViewID
                               && item.IsUploaded == false
                               && item.BattviewStudyID == studyId.ToString()
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
        (ref UInt32 minId, ref UInt32 maxId, ref UInt32 minDate,
         UInt32 battViewID, UInt32 studyId)
        {
            var query =
                GetAll()
                        .Where(item =>
                               item.Id == battViewID
                               && item.BattviewStudyID == studyId.ToString()
                              ).OrderBy(item => item.EventId);

            if (query.Any())
            {
                WriteLimitsValues(query, ref minId, ref maxId, ref minDate);
            }
            else
            {
                WriteLimitsDefaults(ref minId, ref maxId, ref minDate);
            }
        }

        void WriteLimitsValues
        (IOrderedEnumerable<BattviewEvents> query, ref UInt32 minId,
         ref UInt32 maxId, ref UInt32 minDate)
        {
            var first = query.First();
            var last = query.Last();

            minId = first.Id;
            maxId = last.Id;
            minDate = first.OriginalStartTime;
        }

        void WriteLimitsDefaults
        (ref UInt32 minId, ref UInt32 maxId, ref UInt32 minDate)
        {
            minId = 0;
            maxId = 0;
            minDate = 0;
        }

        public List<BattviewEvents> GetIdAndMaxEventIdList()
        {
            var list = new List<BattviewEvents>();

            var query =
                GetAll().GroupBy(item => new { item.Id, item.BattviewStudyID });

            foreach (var groupItem in query)
            {
                var max = groupItem.OrderBy(item => item.EventId).LastOrDefault();
                list.Add(max);
            }

            return list;
        }

        public override List<BattviewEvents> GetNotUploaded()
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
