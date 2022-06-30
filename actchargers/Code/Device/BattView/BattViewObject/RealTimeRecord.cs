using System;

namespace actchargers
{//RealTimeRecord
    public class RealTimeRecord : IEquatable<RealTimeRecord>, IComparable<RealTimeRecord>
    {
        private UInt32 _timestamp;
        public UInt32 timestamp
        {
            get
            {
                return _timestamp;
            }
        }
        private UInt32 _voltage;
        public UInt32 voltage
        {
            get
            {
                return _voltage;
            }
        }

        public UInt32 recordID;
        private Int16 _current;
        public Int16 current
        {
            get
            {
                return _current;
            }
        }
        private Int16 _temperature;
        public Int16 temperature
        {
            get
            {
                return _temperature;
            }
        }
        private byte _soc;
        public byte soc
        {
            get
            {
                return _soc;
            }
        }
        private bool _noTemperature;
        public bool noTemperature
        {
            get
            {
                return _noTemperature;
            }
        }
        private bool _valid;
        public bool valid
        {
            get
            {
                return _valid;
            }
        }
        public RealTimeRecord()
        {
            _valid = false;
        }
        public void loadFromBuffer(byte[] resultArray, uint Copyloc)
        {
            byte[] result = new byte[16];
            Array.Copy(resultArray, Convert.ToInt32(Copyloc), result, 0, 16);
            int loc = 0;

            //TimeStamp
            _timestamp = BitConverter.ToUInt32(result, loc);
            loc += 4;


            //voltage
            _voltage = BitConverter.ToUInt16(result, loc);
            loc += 2;
            //current
            _current = BitConverter.ToInt16(result, loc);
            loc += 2;
            //temperature
            _temperature = BitConverter.ToInt16(result, loc);
            loc += 2;
            _soc = result[loc++];
            if (timestamp != 0)
            {
                _valid = true;
            }
            byte zoneID = result[loc++];
            bool dayLightSaving = (zoneID & 0x80) != 0;
            zoneID &= 0x7F;
            _timestamp = StaticDataAndHelperFunctions.getZoneUnixTimeStampFromUTC(zoneID, timestamp, dayLightSaving);

            _noTemperature = (result[loc] & 0x01) != 0;

            if (!CRC7.isValidCRC(result, result[15]))
            {
                _valid = false;
            }

        }
        public void loadFromRecord(RealTimeRecord r)
        {
            this._timestamp = r._timestamp;
            this._voltage = r._voltage;
            this.recordID = r.recordID;
            this._current = r._current;
            this._temperature = r._temperature;
            this._soc = r._soc;
            this._noTemperature = r._noTemperature;
            this._valid = r._valid;
        }
        public override string ToString()
        {
            return "ID: " + recordID.ToString() + "," + timestamp.ToString();
        }
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            RealTimeRecord objAsPart = obj as RealTimeRecord;
            if (objAsPart == null)
            {
                return false;
            }
            return Equals(objAsPart);
        }
        public int SortByIDAscending(uint id1, uint id2)
        {

            return id1.CompareTo(id2);
        }
        public int CompareTo(RealTimeRecord comparePart)
        {
            // A null value means that this object is greater.
            if (comparePart == null)
            {
                return 1;
            }
            return this.recordID.CompareTo(comparePart.recordID);
        }
        public override int GetHashCode()
        {
            return recordID.GetHashCode();
        }
        public bool Equals(RealTimeRecord other)
        {
            if (other == null)
            {
                return false;
            }
            return (this.recordID.Equals(other.recordID));
        }
    }
}
