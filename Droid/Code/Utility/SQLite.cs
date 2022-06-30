using System;
using System.IO;

namespace actchargers.Droid
{
    public class SQLite : ISQLite
    {
        public string GetDatabasePath()
        {
            string documentsPath =
                Environment
                    .GetFolderPath(Environment.SpecialFolder.Personal);

            return Path
                .Combine
                (documentsPath, ACConstants.DATABASE_FILE_NAME);
        }
    }
}
