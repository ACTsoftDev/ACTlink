using Newtonsoft.Json;

namespace actchargers
{
    public class DeviceConfig
    {
        [JsonProperty(PropertyName = "chargerUserName")]
        public string ChargerUserName { get; set; }

        [JsonProperty(PropertyName = "batteryID")]
        public string BatteryID { get; set; }

        [JsonProperty(PropertyName = "serialNumber")]
        public string SerialNumber { get; set; }

        [JsonProperty(PropertyName = "battViewSN")]
        public string BattViewSN { get; set; }
    }
}
