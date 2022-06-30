using SQLite;

namespace actchargers
{
    public class GenericObjects : DBModel
    {
        [MaxLength(20)]
        public string Type { get; set; }

        [MaxLength(20)]
        public string Value { get; set; }

        public override bool KeysEqual(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var i = (GenericObjects)obj;
            return Type.Equals(i.Type);
        }

    }
}
