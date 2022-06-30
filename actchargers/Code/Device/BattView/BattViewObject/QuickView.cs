using System;

namespace actchargers
{
    public class QuickView
    {
        private UInt32 _event_AS;
        public UInt32 event_AS
        {
            get
            {
                lock (myLock)
                    return _event_AS;
            }
        }
        private UInt32 _event_WS;
        public UInt32 event_WS
        {
            get
            {
                lock (myLock)
                    return _event_WS;
            }
        }

        private float _voltage;
        public float voltage
        {
            get
            {
                lock (myLock)
                    return _voltage;
            }
        }
        private float _current;
        public float current
        {
            get
            {
                lock (myLock)
                    return _current;
            }
        }
        private float _temperature;
        public float temperature
        {
            get
            {
                lock (myLock)
                    return _temperature;
            }
        }
        private byte _soc;
        public byte soc
        {
            get
            {
                lock (myLock)
                    return _soc;
            }
        }
        private byte _event_type;
        public byte event_type
        {
            get
            {
                lock (myLock)
                    return _event_type;
            }
        }
        private DateTime _startTime;
        public DateTime startTime
        {
            get
            {
                lock (myLock)
                    return _startTime;
            }
        }
        private TimeSpan _duration;
        public TimeSpan duration
        {
            get
            {
                lock (myLock)
                    return _duration;
            }
        }
        private bool _EL_enabled;
        public bool EL_enabled
        {
            get
            {
                lock (myLock)
                    return _EL_enabled;
            }
        }
        private bool _waterOK;
        public bool WaterOK
        {
            get
            {
                lock (myLock)
                    return _waterOK;
            }
        }
        private byte _mainSenseErrorCode;
        public byte mainSenseErrorCode
        {
            get
            {
                lock (myLock)
                    return _mainSenseErrorCode;
            }
        }
        private readonly object myLock;
        public QuickView()
        {
            myLock = new object();
            _event_AS = 0;
            _event_WS = 0;
            _soc = 0;
            _voltage = 0;
            _current = 0;
            _temperature = 25;
            _event_type = 0;
            _startTime = DateTime.UtcNow;
            _duration = TimeSpan.FromMilliseconds(0);
            _EL_enabled = false;
            _mainSenseErrorCode = 0;
        }
        public void loadFromArray(byte[] resultArray, float firmwareVersion)
        {
            lock (myLock)
            {
                _event_AS = BitConverter.ToUInt32(resultArray, 0);
                _event_WS = BitConverter.ToUInt32(resultArray, 4);
                _soc = resultArray[8];
                _voltage = BitConverter.ToSingle(resultArray, 9);
                _current = BitConverter.ToSingle(resultArray, 13);
                _temperature = BitConverter.ToSingle(resultArray, 17);
                _event_type = resultArray[21];
                _startTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(BitConverter.ToUInt32(resultArray, 22));
                _duration = TimeSpan.FromSeconds(BitConverter.ToUInt32(resultArray, 26));
                _EL_enabled = resultArray[30] != 0;
                _waterOK = resultArray[31] != 0;
                if (firmwareVersion >= 2.09f)
                {
                    _mainSenseErrorCode = resultArray[32];
                }
                else
                {
                    _mainSenseErrorCode = 0;
                }

            }
        }
    }
}
