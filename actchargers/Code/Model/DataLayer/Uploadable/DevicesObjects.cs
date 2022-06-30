using System;
using System.Collections.Generic;
using actchargers.Code.Utility;
using SQLite;

namespace actchargers
{
    public class DevicesObjects : UploadableBase
    {
        public uint Id { get; set; }

        [MaxLength(2048)]
        public string Config { get; set; }

        [MaxLength(2048)]
        public string GlobalRecord { get; set; }

        [MaxLength(20)]
        public string Name { get; set; }

        [Ignore]
        public string UploadTitle
        {
            get
            {
                return DeviceName + " #" + SerialNumber;
            }
        }

        [Ignore]
        public string DeviceName
        {
            get
            {
                return GetDeviceName();
            }
        }

        [Ignore]
        public string SerialNumber
        {
            get
            {
                return GetSerialNumber();
            }
        }

        public Boolean IsUploaded { get; set; }

        public ushort MemorySignature { get; set; }

        public float FirmwareVersion { get; set; }

        public uint EventsCount { get; set; }

        public string Zone { get; set; }

        [Column("battview_studyID")]
        public uint BattviewStudyID { get; set; }

        [MaxLength(255)]
        public string StudyName { get; set; }

        [MaxLength(255)]
        public string TruckId { get; set; }

        public DeviceType GetDeviceType()
        {
            if (Name == null)
                return DeviceType.MCB;

            if (Name.Equals(ACConstants.MCB))
                return DeviceType.MCB;

            else if (Name.Equals(ACConstants.BATTVIEW))
                return GetBattviewType();

            else
                return DeviceType.MCB;
        }

        DeviceType GetBattviewType()
        {
            if (BattviewStudyID == 0)
                return DeviceType.BATTVIEW;
            else
                return DeviceType.BATTVIEW_MOBILE;
        }

        public Dictionary<string, string> ToDictionary()
        {
            Dictionary<string, string> dictionary =
                new Dictionary<string, string>
            {
                { "id", Id.ToString() },
                { "config", Config },
                { "global", GlobalRecord },
                { "firmwareVersion", FirmwareVersion.ToString() },
                { "zone", Zone }
            };

            if (BattviewStudyID != 0)
                dictionary.Add("studyId", BattviewStudyID.ToString());

            return dictionary;
        }

        string GetDeviceName()
        {
            var deviceConfigObject =
                JsonParser.DeserializeObject<DeviceConfig>(Config);

            if (deviceConfigObject == null)
                return "";

            string correctProperty;

            if (GetDeviceType() == DeviceType.MCB)
                correctProperty = deviceConfigObject.ChargerUserName;
            else
                correctProperty = deviceConfigObject.BatteryID;

            if (correctProperty == null)
                return "";
            else
                return correctProperty;
        }

        string GetSerialNumber()
        {
            var deviceConfigObject =
                JsonParser.DeserializeObject<DeviceConfig>(Config);

            if (deviceConfigObject == null)
                return "";

            string correctProperty;

            if (GetDeviceType() == DeviceType.MCB)
                correctProperty = deviceConfigObject.SerialNumber;
            else
                correctProperty = deviceConfigObject.BattViewSN;

            if (correctProperty == null)
                return "";
            else
                return correctProperty;
        }

        public override bool KeysEqual(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var i = (DevicesObjects)obj;
            return
                Id == i.Id
                       && Name == i.Name;
        }
    }
}
