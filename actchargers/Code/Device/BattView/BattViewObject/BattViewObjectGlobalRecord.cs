using System;
using Newtonsoft.Json;

namespace actchargers
{
    public class BattViewObjectGlobalRecord
    {
        [JsonIgnore]
        private object myLock;

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        [JsonIgnore]
        private UInt16 _signature;
        [JsonProperty]
        public UInt16 signature
        {
            get
            {
                lock (myLock)
                {
                    return _signature;
                }
            }
        }
        [JsonIgnore]
        private UInt32 _seq;
        [JsonProperty]
        public UInt32 seq
        {
            get
            {
                lock (myLock)
                {
                    return _seq;
                }
            }
        }
        [JsonIgnore]
        private UInt32 _eventsCount;
        [JsonProperty]
        public UInt32 eventsCount
        {
            get
            {
                lock (myLock)
                {
                    return _eventsCount;
                }
            }
        }
        [JsonIgnore]
        private UInt32 _RTrecordsCount;
        [JsonProperty]
        public UInt32 RTrecordsCount
        {
            get
            {
                lock (myLock)
                {
                    return _RTrecordsCount;
                }
            }
        }
        [JsonIgnore]
        private UInt32 _inUseAHR;
        [JsonProperty]
        public UInt32 inUseAHR
        {
            get
            {
                lock (myLock)
                {
                    return _inUseAHR;
                }
            }
        }
        [JsonIgnore]
        private UInt32 _inUseKWHR;
        [JsonProperty]
        public UInt32 inUseKWHR
        {
            get
            {
                lock (myLock)
                {
                    return _inUseKWHR;
                }
            }
        }
        [JsonIgnore]
        private UInt32 _chargeAHR;
        [JsonProperty]
        public UInt32 chargeAHR
        {
            get
            {
                lock (myLock)
                {
                    return _chargeAHR;
                }
            }
        }
        [JsonIgnore]
        private UInt32 _chargeKWHR;
        [JsonProperty]
        public UInt32 chargeKWHR
        {
            get
            {
                lock (myLock)
                {
                    return _chargeKWHR;
                }
            }
        }
        [JsonIgnore]
        private UInt32 _inUseSeconds;
        [JsonProperty]
        public UInt32 inUseSeconds
        {
            get
            {
                lock (myLock)
                {
                    return _inUseSeconds;
                }
            }
        }
        [JsonIgnore]
        private UInt32 _chargeSeconds;
        [JsonProperty]
        public UInt32 chargeSeconds
        {
            get
            {
                lock (myLock)
                {
                    return _chargeSeconds;
                }
            }
        }
        [JsonIgnore]
        private UInt32 _idleSeconds;
        [JsonProperty]
        public UInt32 idleSeconds
        {
            get
            {
                lock (myLock)
                {
                    return _idleSeconds;
                }
            }
        }
        [JsonIgnore]
        private UInt32 _leftoverinuseas;
        [JsonProperty]
        public UInt32 leftoverinuseas
        {
            get
            {
                lock (myLock)
                {
                    return _leftoverinuseas;
                }
            }
        }
        [JsonIgnore]
        private UInt32 _leftoverinusews;
        [JsonProperty]
        public UInt32 leftoverinusews
        {
            get
            {
                lock (myLock)
                {
                    return _leftoverinusews;
                }
            }
        }
        [JsonIgnore]
        private UInt32 _leftoverchargeas;
        [JsonProperty]
        public UInt32 leftoverchargeas
        {
            get
            {
                lock (myLock)
                {
                    return _leftoverchargeas;
                }
            }
        }
        [JsonIgnore]
        private UInt32 _leftoverchargews;
        [JsonProperty]
        public UInt32 leftoverchargews
        {
            get
            {
                lock (myLock)
                {
                    return _leftoverchargews;
                }
            }
        }
        [JsonIgnore]
        private UInt32 _debugCount;
        [JsonProperty]
        public UInt32 debugCount
        {
            get
            {
                lock (myLock)
                {
                    return _debugCount;
                }
            }
        }
        [JsonIgnore]
        private byte _endSignature;
        [JsonProperty]
        public byte endSignature
        {
            get
            {
                lock (myLock)
                {
                    return _endSignature;
                }
            }
        }
        [JsonIgnore]
        private float _lastfirmwareversion0;
        [JsonProperty]
        public float lastfirmwareversion0
        {
            get
            {
                lock (myLock)
                {
                    return _lastfirmwareversion0;
                }
            }
        }
        [JsonIgnore]
        private float _lastfirmwareversion1;
        [JsonProperty]
        public float lastfirmwareversion1
        {
            get
            {
                lock (myLock)
                {
                    return _lastfirmwareversion1;
                }
            }
        }

        [JsonIgnore]
        public byte[] unused;

        [JsonIgnore]
        public byte[] unused2;

        [JsonIgnore]
        public byte CRC7;

        [JsonIgnore]
        public bool valid;

        [JsonIgnore]
        private bool _lostTime;
        [JsonIgnore]
        public bool lostTime
        {
            get
            {
                lock (myLock)
                {
                    return _lostTime;
                }
            }
        }
        public BattViewObjectGlobalRecord()
        {
            unused = new byte[1];
            unused2 = new byte[59];//version > 1.06
            valid = false;
            myLock = new object();

        }
        public void loadFromBuffer(byte[] result, float firmwareVersion)
        {
            lock (myLock)
            {
                int loc = 0;
                if (!actchargers.CRC7.isValidCRC(result, result[firmwareVersion > 1.05 ? 127 : 63]))
                {
                    valid = false;
                    return;
                }
                //signature
                _signature = BitConverter.ToUInt16(result, loc);
                loc += 2;
                //SEQ
                if (firmwareVersion > 1.05)
                {
                    _seq = BitConverter.ToUInt32(result, loc);
                    loc += 4;

                }
                else
                {
                    _seq = BitConverter.ToUInt16(result, loc);
                    loc += 2;
                }

                //event_sequence
                _eventsCount = BitConverter.ToUInt32(result, loc);
                loc += 4;
                //rt_sequence
                _RTrecordsCount = BitConverter.ToUInt32(result, loc);
                loc += 4;

                //inUSE_AHR
                _inUseAHR = BitConverter.ToUInt32(result, loc);
                loc += 4;
                //inUSE_KWHR
                _inUseKWHR = BitConverter.ToUInt32(result, loc);
                loc += 4;
                //charge_AHR
                _chargeAHR = BitConverter.ToUInt32(result, loc);
                loc += 4;
                //charge_KWHR
                _chargeKWHR = BitConverter.ToUInt32(result, loc);
                loc += 4;
                //inUSE_Seconds
                _inUseSeconds = BitConverter.ToUInt32(result, loc);
                loc += 4;
                //charge_Seconds
                _chargeSeconds = BitConverter.ToUInt32(result, loc);
                loc += 4;
                //idle_Seconds
                _idleSeconds = BitConverter.ToUInt32(result, loc);
                loc += 4;
                //leftOverInUSE_AS
                _leftoverinuseas = BitConverter.ToUInt32(result, loc);
                loc += 4;
                //leftOverInUSE_WS
                _leftoverinusews = BitConverter.ToUInt32(result, loc);
                loc += 4;
                //leftOverCharge_AS
                _leftoverchargeas = BitConverter.ToUInt32(result, loc);
                loc += 4;
                //leftOverCharge_WS
                _leftoverchargews = BitConverter.ToUInt32(result, loc);
                loc += 4;
                //debugCount
                _debugCount = BitConverter.ToUInt32(result, loc);
                loc += 4;
                //unused
                if (firmwareVersion > 1.05)
                {
                    _lastfirmwareversion0 = result[loc++] / 100.0f + result[loc++];
                    _lastfirmwareversion1 = result[loc++] / 100.0f + result[loc++];
                    loc += unused2.Length;
                }
                else
                {
                    loc += unused.Length;
                }
                _lostTime = result[loc++] != 0;
                _endSignature = result[loc++];
                valid = true;
            }

        }
    }
}
