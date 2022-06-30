using System.Collections.Generic;
using SQLite;

namespace actchargers
{
    public abstract class UploadableLoadersBase<T> : LoadersBase<T>
    where T : UploadableBase
    {
        protected UploadableLoadersBase(SQLiteConnection database) : base(database)
        {
        }

        public abstract List<T> GetNotUploaded();

        public bool DoWeHaveUploads()
        {
            var notUploaded = GetNotUploaded();

            if (notUploaded == null)
                return false;

            return notUploaded.Count > 0;
        }
    }
}
