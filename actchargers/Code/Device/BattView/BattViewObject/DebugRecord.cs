using System;

namespace actchargers
{
    public class DebugRecord : IEquatable<DebugRecord>, IComparable<DebugRecord>
    {
        private UInt32 _ID;
        public UInt32 ID
        {
            get
            {
                return _ID;
            }
        }
        private UInt32 _timestamp;
        public UInt32 timestamp
        {
            get
            {
                return _timestamp;
            }
        }
        private string _debugString;
        public string DebugString
        {
            get
            {
                return _debugString;
            }
        }
        private byte _debug;
        public byte debug
        {
            get
            {
                return _debug;
            }
        }
        private byte[] _info;
        public byte[] Info
        {
            get
            {
                byte[] temp_info = new byte[_info.Length];
                Array.Copy(_info, temp_info, _info.Length);
                return temp_info;
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


        public DebugRecord()
        {
            _valid = false;
            _info = new byte[6];
        }
        public void loadFromDebugRecord(DebugRecord r)
        {
            this._ID = r._ID;
            this._timestamp = r._timestamp;
            this._debugString = r._debugString;
            this._valid = r._valid;
            Array.Copy(r._info, this._info, r._info.Length);
        }
        public void loadFromBuffer(byte[] resultArray, int Copyloc)
        {
            byte[] result = new byte[16];
            Array.Copy(resultArray, Copyloc, result, 0, 16);
            int loc = 0;
            UInt32 longInfo;
            //ID
            _ID = BitConverter.ToUInt32(result, loc);
            loc += 4;
            //TimeStamp
            _timestamp = BitConverter.ToUInt32(result, loc);
            loc += 4;
            //info
            Array.Copy(result, loc, _info, 0, 6);
            longInfo = BitConverter.ToUInt32(_info, 0);
            loc += 4;
            _debug = result[loc++];

            if (timestamp != 0)
            {
                _valid = true;
            }
            byte zoneID = result[loc];
            bool dayLightSaving = (zoneID & 0x80) != 0;
            zoneID &= 0x7F;
            _timestamp = StaticDataAndHelperFunctions.getZoneUnixTimeStampFromUTC(zoneID, timestamp, dayLightSaving);
            if (!CRC7.isValidCRC(result, result[15]))
            {
                _valid = false;
            }
            switch (debug)
            {
                case 1:
                    _debugString = "CONNECT";
                    break;
                case 2:
                    _debugString = "SET SOC BY V";
                    break;
                case 3:
                    _debugString = "SET SOC BY USER: " + longInfo.ToString();
                    break;
                case 4:
                    _debugString = "CALIBRATE BY USER: " + longInfo.ToString();
                    break;
                case 5:
                    _debugString = "Firmware Update BY USER: " + longInfo.ToString();
                    break;
                case 6:
                    _debugString = "set RTC BY USER: " + longInfo.ToString();
                    break;
                case 7:
                    _debugString = "Defaults Loaded";
                    break;
                case 8:
                    _debugString = "RESTART";
                    break;
                case 9:
                    _debugString = "RTC FAILURE:";
                    if ((longInfo & 0x80) != 0)
                    {
                        _debugString += "OSCILLATOR FAILED;";
                    }
                    if ((longInfo & 0x40) != 0)
                    {
                        _debugString += "VBAT SWITCH FAILED;";
                    }
                    if ((longInfo & 0x20) != 0)
                    {
                        _debugString += "VBAT SWITCH during run time;";
                    }
                    if ((longInfo & 0x10) != 0)
                    {
                        _debugString += "VBAT BELOW Threshold!!!";
                    }
                    if (longInfo == 1)
                    {
                        _debugString += "RTC COMM";
                    }
                    break;
                case 10:
                    _debugString = "Module Restart: " + ((byte)longInfo).ToString("X");
                    break;
                case 11:
                    _debugString = "RESTART Issye @step  " + longInfo.ToString();
                    break;
                case 12:
                    _debugString = "SSID CONNECT TO  " + longInfo.ToString();
                    break;
                case 13:
                    _debugString = "Request Disconnect to : " + longInfo.ToString();
                    break;
                case 14:
                    _debugString = "CIP CLOSE WITH  " + longInfo.ToString();
                    break;
                case 15:
                    _debugString = "CIP START WITH : " + longInfo.ToString();
                    break;
                case 16:
                    _debugString = "SSID DISOCNNECTED  : " + longInfo.ToString();
                    break;
                case 17:
                    _debugString = "Abnormal disconnect : " + longInfo.ToString();
                    break;
                case 18:
                    _debugString = "CONNECTED TO SSID : " + longInfo.ToString();
                    break;
                default:
                    _debugString = "Unknown";
                    break;
            }

        }
        public override string ToString()
        {
            return "ID: " + ID.ToString() + "," + timestamp.ToString();
        }
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            DebugRecord objAsPart = obj as DebugRecord;
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
        public int CompareTo(DebugRecord comparePart)
        {
            // A null value means that this object is greater.
            if (comparePart == null)
            {
                return 1;
            }
            return this.ID.CompareTo(comparePart.ID);
        }
        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }
        public bool Equals(DebugRecord other)
        {
            if (other == null)
            {
                return false;
            }
            return (this.ID.Equals(other.ID));
        }
    }
}
