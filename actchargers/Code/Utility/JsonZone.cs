using System;

namespace actchargers
{
    public class JsonZone
    {
        public string display_name;
        public byte id;
        public int base_utc;
        public UInt32[] changes_time;
        public int changes_value;

        public JsonZone(string name, Int32 baseUTCShift, byte anID)
        {
            id = anID;
            display_name = name;
            base_utc = baseUTCShift;
            changes_time = new UInt32[50];
            for (int i = 0; i < 50; i++)
            {
                changes_time[i] = UInt32.MaxValue;
            }
        }

        public override bool Equals(Object obj)
        {
            JsonZone zone = obj as JsonZone;
            if (zone == null)
                return false;
            else
                return this.id.Equals(zone.id);
        }

        public override int GetHashCode()
        {
            return this.id.GetHashCode();
        }
        public override string ToString()
        {
            return display_name;
        }
    }
}
