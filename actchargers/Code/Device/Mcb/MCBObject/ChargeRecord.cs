using System;
using Newtonsoft.Json;

namespace actchargers
{
    public class ChargeRecord
    {
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        [JsonIgnore]
        private bool _valid;
        [JsonProperty]
        public bool isValidCRC7
        {
            get
            {

                return _valid;

            }
        }
        [JsonIgnore]
        private UInt32 _cycleid;
        [JsonProperty]
        public UInt32 cycleID
        {
            get
            {

                return _cycleid;

            }
        }

        [JsonIgnore]
        private UInt32 _pmerror1;
        [JsonProperty]
        public UInt32 PMerror1
        {
            get
            {

                return _pmerror1;
            }
        }
        [JsonIgnore]
        private UInt32 _pmerror2;
        [JsonProperty]
        public UInt32 PMerror2
        {
            get
            {

                return _pmerror2;
            }
        }
        [JsonIgnore]
        private UInt32 _pmerror3;
        [JsonProperty]
        public UInt32 PMerror3
        {
            get
            {

                return _pmerror3;
            }
        }
        [JsonIgnore]
        private UInt32 _pmerror4;
        [JsonProperty]
        public UInt32 PMerror4
        {
            get
            {

                return _pmerror4;
            }
        }
        [JsonIgnore]
        private UInt32 _maxWs;
        [JsonProperty]
        public UInt32 maxWS
        {
            get
            {

                return _maxWs;

            }
        }

        [JsonIgnore]
        private DateTime _cycleTime;
        [JsonProperty]
        [JsonConverter(typeof(ToTimeStamp))]
        public DateTime cycleTime
        {
            get
            {

                return new DateTime(_cycleTime.Ticks);

            }
        }

        [JsonIgnore]
        private DateTime _originalcycletime;
        [JsonProperty]
        [JsonConverter(typeof(ToTimeStamp))]
        public DateTime originalCycleTime
        {
            get
            {

                return new DateTime(_originalcycletime.Ticks);

            }
        }

        [JsonIgnore]
        private UInt32 _duration;
        [JsonProperty]
        public UInt32 duration
        {
            get
            {

                return _duration;

            }
        }

        [JsonIgnore]
        private UInt32 _battviewid;
        [JsonProperty]
        public UInt32 battViewID
        {
            get
            {

                return _battviewid;

            }
        }


        [JsonIgnore]
        private UInt32 _totalas;
        [JsonProperty]
        public UInt32 totalAS
        {
            get
            {

                return _totalas;

            }
        }
        [JsonIgnore]
        private UInt32 _totalws;
        [JsonProperty]
        public UInt32 totalWS
        {
            get
            {

                return _totalws;

            }
        }
        [JsonIgnore]
        private UInt16 _lastvoltage;
        [JsonProperty]
        public UInt16 lastVoltage
        {
            get
            {

                return _lastvoltage;

            }
        }

        [JsonIgnore]
        private UInt16 _startvoltage;
        [JsonProperty]
        public UInt16 startVoltage
        {
            get
            {

                return _startvoltage;

            }
        }


        [JsonIgnore]
        private Int16 _maxtemperature;
        [JsonProperty]
        public Int16 maxTemperature
        {
            get
            {

                return _maxtemperature;

            }
        }
        [JsonIgnore]
        private byte _cycles;
        [JsonProperty]
        public byte cycles
        {
            get
            {

                return _cycles;

            }
        }

        [JsonIgnore]
        private byte _status;
        [JsonProperty]
        internal byte status
        {
            get
            {

                return _status;

            }
        }

        [JsonIgnore]
        private byte _numberofcells;
        [JsonProperty]
        public byte numberOfCells
        {
            get
            {

                return _numberofcells;

            }
        }
        [JsonIgnore]
        private byte _batteryType;
        [JsonProperty]
        public byte batteryType
        {
            get
            {

                return _batteryType;

            }
        }

        [JsonIgnore]
        public string BatteryTypeText
        {
            get
            {
                return SharedTexts.GetBatteryTypeText(batteryType);
            }
        }

        [JsonIgnore]
        internal const int dataSize = 64;

        public static string ExitCodes(byte code, bool isLithium, bool isBattView)
        {
            switch (code)
            {
                case 0: if (isBattView) return "Disconnect"; else return "Recycling Power";
                case 1:
                    if (isLithium)
                        return "CC1 Time Out";
                    else
                        return "CC Time Out(TR)";
                case 2: return "CC Time Out";
                case 3: if (isBattView && !ControlObject.isDebugMaster) return "Disconnect"; else return "Battery Disconnection";
                case 4: return "User Request";
                case 5: return "Output Over Current";
                case 6: return "Output Over Voltage";
                case 7: return "AC line Fault";
                case 8: return "Over Temperature";
                case 9: return "TimeOut";
                case 0x0A: return "Output Under Voltage";
                case 0x0B: return "Input Over Current Protection";
                case 0x0C: return "Communication Error";
                case 0x0D: return "CV TIME Out";
                case 0x0E: return "Control Saturation";
                case 0x0F: return "OVDS";
                case 0x10: return "FAN FAULT";
                case 0x11: return "Battery Over Temperature";
                case 0x12: return "LockOut";
                case 0x13: return "Output Current Control";
                case 0x14: if (ControlObject.isDebugMaster) return "Disconnect/HVPC"; else return "Disconnect";
                case 0x16: if (ControlObject.isDebugMaster) return "Disconnect/PMOV"; else return "Disconnect";
                case 0x17: if (ControlObject.isDebugMaster) return "Disconnect/VI"; else return "Disconnect";
                case 0x18: if (ControlObject.isDebugMaster) return "Disconnect/VP"; else return "Disconnect";
                case 0x19: return "Completed: BMS";
                case 21: return "BATTview Disconnect";
                case 0xFE: if (isBattView && !ControlObject.isDebugMaster) return ""; else return "Unkown Error";
                case 0xFF: return "Completed";
                case 0xAA: if (isBattView && !ControlObject.isDebugMaster) return ""; else return "Running";
                default: if (isBattView && !ControlObject.isDebugMaster) return ""; else return "N/A(" + code + ")";
            }
        }

        public ChargeRecord()
        {
            _pmerror1 = _pmerror2 = _pmerror3 = _pmerror4 = 0;
            _cycleid = 0;
            _cycleTime = DateTime.UtcNow;
            _originalcycletime = DateTime.UtcNow;
            _battviewid = 0;
            _duration = 0;
            _totalas = 0;
            _totalws = 0;
            _maxtemperature = 32767;
            _lastvoltage = 0;
            _startvoltage = 0;
            _numberofcells = 1;
            _cycles = 0;
            _status = 0;
            _batteryType = 0;
        }

        internal void copyFromRecord(ChargeRecord c)
        {
            this._cycles = c._cycles;
            this._totalas = c._totalas;
            this._battviewid = c._battviewid;
            this._pmerror1 = c._pmerror1;
            this._pmerror2 = c._pmerror2;
            this._pmerror3 = c._pmerror3;
            this._pmerror4 = c._pmerror4;

            this._cycleTime = new DateTime(c._cycleTime.Ticks);
            this._originalcycletime = new DateTime(c._originalcycletime.Ticks);


            this._duration = c._duration;
            this._lastvoltage = c._lastvoltage;
            this._status = c._status;
            this._maxtemperature = c._maxtemperature;
            this._maxWs = c._maxWs;
            this._numberofcells = c._numberofcells;
            this._cycleid = c._cycleid;
            this._startvoltage = c._startvoltage;
            this._totalws = c._totalws;
            this._batteryType = c._batteryType;
            this._valid = c._valid;

        }

        public void loadFromArray(byte[] result)
        {
            this._valid = true;
            if (!CRC7.isValidCRC(result, 0, dataSize - 1, (byte)result[(int)dataSize - 1]))
            {
                _valid = false;

                return;
            }

            int loc = 0;

            _pmerror1 = BitConverter.ToUInt32(result, loc);
            loc += 4;
            _pmerror2 = BitConverter.ToUInt32(result, loc);
            loc += 4;
            _pmerror3 = BitConverter.ToUInt32(result, loc);
            loc += 4;
            _pmerror4 = BitConverter.ToUInt32(result, loc);
            loc += 4;

            _maxWs = BitConverter.ToUInt32(result, loc);
            loc += 4;
            _battviewid = BitConverter.ToUInt32(result, loc);
            loc += 4;

            _originalcycletime = (new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).AddSeconds(BitConverter.ToUInt32(result, loc));
            loc += 4;

            _duration = BitConverter.ToUInt32(result, loc);
            loc += 4;


            _totalas = BitConverter.ToUInt32(result, loc);
            loc += 4;
            _totalws = BitConverter.ToUInt32(result, loc);
            loc += 4;

            _cycleid = BitConverter.ToUInt32(result, loc);
            loc += 4;

            _lastvoltage = BitConverter.ToUInt16(result, loc);
            loc += 2;

            _startvoltage = BitConverter.ToUInt16(result, loc);
            loc += 2;

            _maxtemperature = BitConverter.ToInt16(result, loc);

            loc += 2;




            _cycles = result[loc++];
            _status = result[loc++];

            _numberofcells = result[loc++];
            byte zone = result[loc++];
            _cycleTime = StaticDataAndHelperFunctions.getZoneTimeFromUTC((byte)(zone & 0x7f), _originalcycletime, ((zone & 0x80) != 0x00));
            byte extraFlags = result[loc++];
            _batteryType = (byte)(extraFlags & 0x03);
        }
    }
}
