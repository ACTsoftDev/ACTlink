using System.Collections.Generic;
using System.Linq;
using SQLite;

namespace actchargers
{
    public class DBConnectedDevicesLoader : LoadersBase<DBConnectedDevices>
    {
        public DBConnectedDevicesLoader(SQLiteConnection database) : base(database)
        {
        }

        public override List<DBConnectedDevices> GetAll()
        {
            return database.Table<DBConnectedDevices>().ToList();
        }

        public int DeleteUsingId(string IpAddress)
        {
            var toDelete =
                GetAll()
                    .FirstOrDefault(
                        item =>
                        item.IPAddress == IpAddress
                    );

            return Delete(toDelete);
        }
    }
}
