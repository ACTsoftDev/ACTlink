using SQLite;

namespace actchargers
{
    public abstract class DBModel
    {
        [PrimaryKey, AutoIncrement]
        public int Key { get; set; }

        public abstract bool KeysEqual(object obj);

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var i = (DBModel)obj;
            return Key == i.Key;
        }

        public override int GetHashCode()
        {
            return Key.GetHashCode();
        }
    }
}
