using System;
using SQLite;

namespace actchargers
{
    public class SynchObjectsBufferedData : DBModel
    {
        public UInt32 Id { get; set; }

        [MaxLength(20)]
        public string DeviceTypeName { get; set; }

        [MaxLength(3072)]
        public string Config { get; set; }

        [MaxLength(32)]
        public string SerialNumber { get; set; }

        [Ignore]
        public string SerialNumberSuffix
        {
            get
            {
                return ACConstants.SN + SerialNumber;
            }
        }

        [MaxLength(32)]
        public string DeviceName { get; set; }

        [MaxLength(32)]
        public string DeviceFullName { get; set; }

        public UInt32 SiteId { get; set; }

        public UInt32 CustomerId { get; set; }

        [Ignore]
        public SynchSiteObjects SynchSite { get; set; }

        public int Zone { get; set; }

        [Ignore]
        public string InterfaceSn
        {
            get
            {
                return GetInterfaceSn();
            }
        }

        string GetInterfaceSn()
        {
            string prefix;
            if (GetDeviceType() == DeviceType.MCB)
                prefix = ACConstants.INTERFACE_MCB_PREFIX;
            else
                prefix = ACConstants.INTERFACE_BATTVIEW_PREFIX;

            string interfaceSn =
                string.Format("{0}_{1}:{2}", prefix, Id, SerialNumber);

            return interfaceSn;
        }

        public DeviceType GetDeviceType()
        {
            if (DeviceTypeName == null)
                return DeviceType.MCB;

            if (DeviceTypeName.Equals(ACConstants.MCB))
                return DeviceType.MCB;

            else if (DeviceTypeName.Equals(ACConstants.BATTVIEW))
                return GetBattviewType();

            else
                return DeviceType.MCB;
        }

        DeviceType GetBattviewType()
        {
            return DeviceType.BATTVIEW;
        }

        public override bool KeysEqual(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var i = (SynchObjectsBufferedData)obj;
            return Id == i.Id;
        }
    }
}
