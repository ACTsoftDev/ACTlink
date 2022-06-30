using System.IO;
using Foundation;

namespace actchargers.iOS
{
    public class SQLite : ISQLite
    {
        public string GetDatabasePath()
        {
            var libFile = NSFileManager
                .DefaultManager
                .GetUrls(NSSearchPathDirectory.LibraryDirectory,
                         NSSearchPathDomain.User)[0];
            string libPath = libFile.Path;

            return Path
                .Combine
                (libPath, ACConstants.DATABASE_FILE_NAME);
        }
    }
}
