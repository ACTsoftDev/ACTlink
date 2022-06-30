using System;
using SQLite;

namespace actchargers
{
    public class ReplaceDevices : UploadableBase
    {
        public UInt32 OriginalDeviceID { get; set; }

        public string OriginalDeviceSN { get; set; }

        public DevicesObjects GetOriginalDevice()
        {
            return GetDeviceById(OriginalDeviceID);
        }

        public UInt32 NewDeviceID { get; set; }

        [MaxLength(20)]
        public string NewDeviceSN { get; set; }

        public DevicesObjects GetNewDevice()
        {
            return GetDeviceById(NewDeviceID);
        }

        [MaxLength(20)]
        public string Name { get; set; }

        public bool IsUploaded { get; set; }

        public bool SecretUploaded { get; set; }

        DevicesObjects GetDeviceById(uint deviceId)
        {
            return DbSingleton
                .DBManagerServiceInstance
                .GetDevicesObjectsLoader()
                .GetDevice(deviceId, Name);
        }

        public override bool KeysEqual(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var i = (ReplaceDevices)obj;
            return
                OriginalDeviceID == i.OriginalDeviceID
                                        && NewDeviceID == i.NewDeviceID
                                        && Name == i.Name;
        }
    }
}
