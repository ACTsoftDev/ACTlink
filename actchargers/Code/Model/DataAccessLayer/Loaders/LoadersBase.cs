using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using SQLite;

namespace actchargers
{
    public abstract class LoadersBase<T>
    where T : DBModel
    {
        protected readonly SQLiteConnection database;

        protected LoadersBase(SQLiteConnection database)
        {
            this.database = database;
        }

        public abstract List<T> GetAll();

        public JProperty SerializeToJsonProperty()
        {
            var all = GetAll();

            return ListToJson.SerializeToJsonProperty(all);
        }

        public bool IsFound(T item)
        {
            return GetAll()
                .Any(i => i.KeysEqual(item));
        }

        public T FindItemById(T item)
        {
            return GetAll()
                .First(i => i.KeysEqual(item));
        }

        public int InsertOrUpdate(T item)
        {
            if (IsFound(item))
                return Update(item);
            else
                return Insert(item);
        }

        public int Insert(T item)
        {
            return database.Insert(item);
        }

        public int InsertAll(List<T> items)
        {
            return database.InsertAll(items);
        }

        public int Update(T item)
        {
            T correctItem = SetKeyFromDatabase(item);

            return database.Update(item);
        }

        T SetKeyFromDatabase(T item)
        {
            T foundedItem = FindItemById(item);
            item.Key = foundedItem.Key;

            return item;
        }

        public int UpdateAll(List<T> items)
        {
            return database.UpdateAll(items);
        }

        public int Delete(T item)
        {
            if (item == null)
                return 0;

            return database.Delete<T>(item.Key);
        }

        public void DeleteAll()
        {
            database.DeleteAll<T>();
        }
    }
}
