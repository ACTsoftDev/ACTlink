using System;
using System.Collections.Generic;
using System.Linq;
using SQLite;

namespace actchargers
{
    public class IncompleteDownloadLoader : LoadersBase<IncompleteDownload>
    {
        public IncompleteDownloadLoader(SQLiteConnection database) : base(database)
        {
        }

        public override List<IncompleteDownload> GetAll()
        {
            return database.Table<IncompleteDownload>().ToList();
        }

        public void InsertOrUpdateUsingSerialNumber(string serialNumber, byte command, int lastSucceededIndex)
        {
            var item = new IncompleteDownload()
            {
                SerialNumber = serialNumber,
                Command = command,
                LastSucceededIndex = lastSucceededIndex
            };

            InsertOrUpdate(item);
        }

        public int GetLastSucceededIndexsingSerialNumber(byte command, string serialNumber)
        {
            var item = GetAll().FirstOrDefault((arg) => arg.Command == command && arg.SerialNumber == serialNumber);

            if (item == null)
                return 0;

            return item.LastSucceededIndex;
        }

        internal void DeleteUsingSerialNumber(byte command, string serialNumber)
        {
            var item = GetAll().FirstOrDefault((arg) => arg.Command == command && arg.SerialNumber == serialNumber);

            if (item != null)
                Delete(item);
        }
    }
}
