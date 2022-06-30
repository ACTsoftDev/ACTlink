using System;
using Newtonsoft.Json;

namespace actchargers
{
    public class PMfault
    {
        [JsonIgnore]
        private UInt32 _sequence;
        [JsonProperty]
        public UInt32 faultID
        {
            set
            {
                _sequence = value;
            }
            get
            {
                return _sequence;

            }
        }
        [JsonIgnore]
        private DateTime _when;
        [JsonProperty]
        [Newtonsoft.Json.JsonConverter(typeof(ToTimeStamp))]
        public DateTime faultTime
        {
            get
            {
                return new DateTime(_when.Ticks);

            }
        }
        [JsonIgnore]
        private byte _fault;
        [JsonProperty]
        public byte fault
        {
            get
            {
                return _fault;

            }
        }
        [JsonIgnore]
        private bool _isError;
        [JsonIgnore]
        public bool isError
        {
            get
            {
                return _isError;

            }
        }
        private byte _isErrorRaw;

        public byte isErrorRaw
        {
            get
            {
                return _isErrorRaw;

            }
        }
        [JsonIgnore]
        private UInt32 _macAddress;
        [JsonProperty]
        public UInt32 PMmacAddress
        {
            get
            {
                return _macAddress;

            }
        }
        [JsonIgnore]
        private bool _is_valid;
        [JsonProperty]
        public bool isValidCRC7
        {
            get
            {
                return _is_valid;

            }
        }

        internal PMfault()
        {
            _sequence = 0;
            _when = DateTime.UtcNow;
            _fault = 0;
            _isError = true;
            _isErrorRaw = 0;
            _macAddress = 0;
            _is_valid = false;
        }
        internal const int recordSize = 16;
        internal void verifyID(UInt32 id)
        {
            if (id != _sequence)
            {
                this._is_valid = false;
            }
        }
        public static string getFaultString(byte f)
        {
            string faultString;
            switch (f)
            {
                case 0x01:
                    faultString = "Over current";
                    break;
                case 0x02:
                    faultString = "Over voltage";
                    break;
                case 0x04:
                    faultString = "AC input";
                    break;
                case 0x08:
                    faultString = "Over temperature";
                    break;
                case 0x10:
                    faultString = "Timeout";
                    break;
                case 0x20:
                    faultString = "Output UV";
                    break;
                case 0x40:
                    faultString = "OCP";
                    break;
                case 0x80:
                    faultString = "fan";
                    break;
                case 0xFB:
                    faultString = "Output CO";
                    break;
                case 0xFE:
                    faultString = "P.L.";
                    break;
                case 0xff:
                    faultString = "Communication";
                    break;
                default:
                    faultString = "Unkown (" + f.ToString() + ")";
                    break;

            }
            return faultString;
        }
        internal void loadFromArray(byte[] data, byte myZone, bool dayLightSaving)
        {

            DateTime t = StaticDataAndHelperFunctions.getZoneTimeFromUTC(myZone, new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc), dayLightSaving);

            _is_valid = true;


            if (!CRC7.isValidCRC(data, 0, recordSize - 1, data[recordSize - 1]))
            {
                _is_valid = false;
            }
            _sequence = BitConverter.ToUInt32(data, 0);

            UInt32 ticks = BitConverter.ToUInt32(data, 4);
            _when = t.AddSeconds(ticks);
            _macAddress = (BitConverter.ToUInt32(data, 8));
            _fault = data[12];
            if ((data[13] & 0x01) == 0x01)
                _isError = true;
            else
                _isError = false;

            _isErrorRaw = data[13];
        }
        internal void loadFromRecord(PMfault c)
        {
            this._fault = c._fault;
            this._isError = c._isError;
            this._is_valid = c._is_valid;
            this._macAddress = c._macAddress;
            this._sequence = c._sequence;
            this._when = c._when;
            this._isErrorRaw = c._isErrorRaw;
        }
        public string TOJSON()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);

        }
    }

}
