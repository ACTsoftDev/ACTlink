using System;

namespace actchargers
{
    public class PowerSnapshot
    {
        internal const int READ_SIZE = 16;

        UInt32 _id;
        public UInt32 Id
        {
            get
            {
                return _id;
            }
        }

        internal void VerifyID(UInt32 sequence)
        {
            _is_valid &= sequence == _id;
        }

        DateTime _time;
        public DateTime Time
        {
            get
            {
                return _time;
            }
        }

        UInt16 _voltage;
        public UInt16 Voltage
        {
            get
            {
                return _voltage;
            }
        }

        UInt16 _current;
        public UInt16 Current
        {
            get
            {
                return _current;
            }
        }

        bool _is_valid;
        public bool IsValid
        {
            get
            {
                return _is_valid;
            }
        }

        internal PowerSnapshot()
        {
            _id = 0;
            _time = DateTime.UtcNow;
            _voltage = 0;
            _current = 0;
            _is_valid = false;
        }

        internal PowerSnapshot(PowerSnapshot powerSnapshot)
        {
            _id = powerSnapshot._id;
            _time = powerSnapshot._time;
            _voltage = powerSnapshot._voltage;
            _current = powerSnapshot._current;
            _is_valid = powerSnapshot._is_valid;
        }
        internal void LoadFromArray(byte[] data)
        {

            DateTime t = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            _is_valid = true;

            _is_valid &= CRC7.isValidCRC(data, 0, 15, data[15]);

            _id = BitConverter.ToUInt32(data, 0);

            UInt32 ticks = BitConverter.ToUInt32(data, 4);
            _time = t.AddSeconds(ticks);
            _current = BitConverter.ToUInt16(data, 8);
            _voltage = BitConverter.ToUInt16(data, 10);
        }
    }
}
