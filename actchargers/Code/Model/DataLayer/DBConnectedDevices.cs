using System;

namespace actchargers
{
    public class DBConnectedDevices : DBModel
    {
        public string IPAddress { get; set; }

        public string PrettyName { get; set; }

        public Boolean IsBattview { get; set; }

        public Boolean IsConnected { get; set; }

        public Boolean IsReplacement { get; set; }

        public override bool KeysEqual(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var i = (DBConnectedDevices)obj;
            return IPAddress.Equals(i.IPAddress);
        }
    }
}
