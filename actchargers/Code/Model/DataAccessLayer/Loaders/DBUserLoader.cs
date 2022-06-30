using System.Collections.Generic;
using System.Linq;
using SQLite;

namespace actchargers
{
    public class DBUserLoader : LoadersBase<DBUser>
    {
        public DBUserLoader(SQLiteConnection database) : base(database)
        {
        }

        public override List<DBUser> GetAll()
        {
            return database.Table<DBUser>().ToList();
        }

        public DBUser GetCurrentUser(int userID)
        {
            DBUser currentUser =
                GetAll()
                    .FirstOrDefault(
                        item =>
                        item.UserID == userID
                    );

            return currentUser;
        }

        public void InsertOrReplaceWithChildren(DBUser item)
        {
            InsertOrUpdate(item);
        }
    }
}
