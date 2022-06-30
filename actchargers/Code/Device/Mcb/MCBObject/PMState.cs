using System;
using Newtonsoft.Json;

namespace actchargers
{
    public class PMState
    {
        const string NO_PM = "NO PM";

        [JsonIgnore]
        private bool _valid;
        [JsonIgnore]
        public bool valid
        {
            get
            {
                lock (myLock)
                {

                    return _valid;
                }
            }
        }
        [JsonIgnore]
        private string _id;
        [JsonIgnore]
        private string _macAddress;
        [JsonIgnore]
        public string macAddress
        {
            get
            {
                lock (myLock)
                {

                    return _macAddress;
                }
            }
        }
        [JsonIgnore]
        private UInt32 _pmID;
        [JsonProperty]
        public UInt32 pmID
        {
            get
            {
                lock (myLock)
                {

                    return _pmID;
                }
            }
        }


        [JsonIgnore]
        private string _lastReadingCurrent;
        [JsonIgnore]
        public string lastReadingCurrent
        {
            get
            {
                lock (myLock)
                {

                    return _lastReadingCurrent;
                }
            }
        }
        [JsonIgnore]
        private string _lastReadingVoltage;
        [JsonIgnore]
        public string lastReadingVoltage
        {
            get
            {
                lock (myLock)
                {

                    return _lastReadingVoltage;
                }
            }
        }
        [JsonIgnore]
        private string _lastCurrentSent;
        [JsonIgnore]
        public string lastCurrentSent
        {
            get
            {
                lock (myLock)
                {

                    return _lastCurrentSent;
                }
            }
        }
        [JsonIgnore]
        public string Current
        {
            get
            {
                lock (myLock)
                {
                    if (lastReadingCurrent.Equals("0"))
                        return "0";
                    else
                    {
                        if (state != "Runing")
                            return "0" + " / " + lastCurrentSent;

                        return lastReadingCurrent + " / " + lastCurrentSent;
                    }
                }
            }
        }
        [JsonIgnore]
        public string _Buffer1ErrorCounter;
        [JsonIgnore]
        public string Buffer1ErrorCounter
        {
            get
            {
                lock (myLock)
                {

                    return _Buffer1ErrorCounter;
                }
            }
        }
        [JsonIgnore]
        private string _PM_CAN_iout_OV_FAULT;
        [JsonIgnore]
        public string PM_CAN_iout_OV_FAULT
        {
            get
            {
                lock (myLock)
                {

                    return _PM_CAN_iout_OV_FAULT;
                }
            }
        }
        [JsonIgnore]
        private string _PM_CAN_vout_OV_FAULT;
        [JsonIgnore]
        public string PM_CAN_vout_OV_FAULT
        {
            get
            {
                lock (myLock)
                {

                    return _PM_CAN_vout_OV_FAULT;
                }
            }
        }
        [JsonIgnore]
        private string _PM_CAN_in_fail_FAULT;
        [JsonIgnore]
        public string PM_CAN_in_fail_FAULT
        {
            get
            {
                lock (myLock)
                {

                    return _PM_CAN_in_fail_FAULT;
                }
            }
        }
        [JsonIgnore]
        private string _PM_CAN_temp_OL_FAULT;
        [JsonIgnore]
        public string PM_CAN_temp_OL_FAULT
        {
            get
            {
                lock (myLock)
                {

                    return _PM_CAN_temp_OL_FAULT;
                }
            }
        }
        [JsonIgnore]
        private string _PM_CAN_timeout_FAULT;
        [JsonIgnore]
        public string PM_CAN_timeout_FAULT
        {
            get
            {
                lock (myLock)
                {

                    return _PM_CAN_timeout_FAULT;
                }
            }
        }
        [JsonIgnore]
        private string _PM_CAN_OCP_FAULT;
        [JsonIgnore]
        public string PM_CAN_OCP_FAULT
        {
            get
            {
                lock (myLock)
                {

                    return _PM_CAN_OCP_FAULT;
                }
            }
        }
        [JsonIgnore]
        private string _PM_CAN_FAN_FAULT;
        [JsonIgnore]
        public string PM_CAN_FAN_FAULT
        {
            get
            {
                lock (myLock)
                {

                    return _PM_CAN_FAN_FAULT;
                }
            }
        }
        [JsonIgnore]
        private string _instantError;
        [JsonIgnore]
        public string instantError
        {
            get
            {
                lock (myLock)
                {

                    return _instantError;
                }
            }
        }
        [JsonIgnore]
        private bool _PM_reported_running;
        [JsonIgnore]
        public bool PM_reported_running
        {
            get
            {
                lock (myLock)
                {

                    return _PM_reported_running;
                }
            }
        }



        [JsonIgnore]
        private bool _isPowerLimiting;
        [JsonIgnore]
        public bool isPowerLimiting
        {
            get
            {
                lock (myLock)
                {

                    return _isPowerLimiting;
                }
            }
        }
        [JsonIgnore]
        private string _current_rating;
        [JsonIgnore]
        public string current_rating
        {
            get
            {
                lock (myLock)
                {

                    return _current_rating;
                }
            }
        }
        [JsonIgnore]
        public string Rating
        {
            get
            {
                lock (myLock)
                {

                    return current_rating + " A, " + voltage_rating + " V";
                }
            }
        }
        [JsonIgnore]
        private string _voltage_rating;
        [JsonIgnore]
        public string voltage_rating
        {
            get
            {
                lock (myLock)
                {

                    return _voltage_rating;
                }
            }
        }
        [JsonIgnore]
        private string _revision;
        [JsonIgnore]
        public string revision
        {
            get
            {
                lock (myLock)
                {

                    return _revision;
                }
            }
        }
        [JsonIgnore]
        private string _version;
        [JsonIgnore]
        public string version
        {
            get
            {
                lock (myLock)
                {

                    return _version;
                }
            }
        }
        [JsonProperty]
        public string VersionAndRevision
        {
            get
            {
                lock (myLock)
                {

                    return version + "." + revision;
                }
            }
        }
        [JsonIgnore]
        private bool _errorRecorded;
        [JsonIgnore]
        public bool errorRecorded
        {
            get
            {
                lock (myLock)
                {

                    return _errorRecorded;
                }
            }
        }
        [JsonIgnore]
        private string _model;
        [JsonProperty]
        public string model
        {
            get
            {
                lock (myLock)
                {

                    return _model;
                }
            }
        }
        [JsonIgnore]
        private string _state;
        [JsonIgnore]
        public string state
        {
            get
            {
                lock (myLock)
                {

                    return _state;
                }
            }
        }
        [JsonIgnore]
        object myLock;
        internal PMState()
        {
            myLock = new object();
            _macAddress = "XXXXXXXXXXXX";
            _state = "N/A";
            _instantError = "N/A";
            _id = "";
        }

        public bool IsNoPm()
        {
            return state.Equals(NO_PM);
        }

        public bool HasPm()
        {
            return !IsNoPm();
        }


        internal void DeSerialize(byte[] result, string chargerSerialNumber, float firmwareRevision)
        {
            byte statusByte;
            int i = 0;
            ushort instantState;
            byte model;

            _state = "";

            lock (myLock)
            {
                _errorRecorded = false;

                if (firmwareRevision > 2.17f)
                {
                    int loc = 0;
                    _pmID = BitConverter.ToUInt32(result, loc);
                    loc += 4;
                    _Buffer1ErrorCounter = (BitConverter.ToUInt16(result, loc)).ToString();
                    loc += 2;
                    _lastCurrentSent = (BitConverter.ToUInt16(result, loc) / 10.0).ToString();
                    loc += 2;
                    _lastReadingVoltage = (BitConverter.ToUInt16(result, loc) / 100.0).ToString();
                    loc += 2;
                    _lastReadingCurrent = (BitConverter.ToUInt16(result, loc) / 10.0).ToString();
                    loc += 2;

                    instantState = BitConverter.ToUInt16(result, loc);
                    loc += 2;

                    _PM_CAN_vout_OV_FAULT = result[loc++].ToString();
                    _PM_CAN_iout_OV_FAULT = result[loc++].ToString();
                    _PM_CAN_in_fail_FAULT = result[loc++].ToString();
                    _PM_CAN_temp_OL_FAULT = result[loc++].ToString();
                    _PM_CAN_timeout_FAULT = result[loc++].ToString();
                    loc++;
                    _PM_CAN_OCP_FAULT = result[loc++].ToString();
                    _PM_CAN_FAN_FAULT = result[loc++].ToString();
                    statusByte = result[loc++];
                    this._current_rating = result[loc++].ToString();
                    this._voltage_rating = result[loc++].ToString();
                    this._revision = result[loc++].ToString();
                    this._version = result[loc++].ToString();
                    loc++;
                    loc++;
                    model = result[loc++];
                }
                else
                {

                    for (i = 0; i < 10; i++)
                        if (result[i] > 0)
                        {
                            _errorRecorded = true;
                            break;
                        }
                    i = 0;





                    _PM_CAN_vout_OV_FAULT = result[i++].ToString();
                    _PM_CAN_iout_OV_FAULT = result[i++].ToString();
                    _PM_CAN_in_fail_FAULT = result[i++].ToString();
                    _PM_CAN_temp_OL_FAULT = result[i++].ToString();
                    _PM_CAN_timeout_FAULT = result[i++].ToString();
                    i++;
                    _PM_CAN_OCP_FAULT = result[i++].ToString();
                    _PM_CAN_FAN_FAULT = result[i++].ToString();

                    _Buffer1ErrorCounter = (BitConverter.ToUInt16(result, 8)).ToString();
                    _lastCurrentSent = (BitConverter.ToUInt16(result, 10) / 10.0).ToString();
                    _lastReadingVoltage = (BitConverter.ToUInt16(result, 12) / 100.0).ToString();
                    _lastReadingCurrent = (BitConverter.ToUInt16(result, 14) / 10.0).ToString();
                    statusByte = result[16];

                    instantState = BitConverter.ToUInt16(result, 17);


                    //19
                    this._current_rating = result[19].ToString();
                    this._voltage_rating = result[20].ToString();
                    this._revision = result[21].ToString();
                    this._version = result[22].ToString();

                    _pmID = BitConverter.ToUInt32(result, 23);

                    model = result[29];

                }
                switch (model)
                {
                    case 1:
                        this._model = "QPM-24/36-50-480";
                        break;
                    case 2:
                        this._model = "QPM-48-40-480";
                        break;
                    case 3:
                        this._model = "QPM-80-25-480";
                        break;
                    case 10:
                    case 41:
                        this._model = "QPM-24/36-50-208";
                        break;
                    case 20:
                    case 42:
                        this._model = "QPM-48-40-208";
                        break;
                    case 43:
                        this._model = "QPM-80-25-208";
                        break;
                    case 51:
                        this._model = "QPM-24/36-50-600";
                        break;
                    case 52:
                        this._model = "QPM-48-40-600";
                        break;
                    case 53:
                        this._model = "QPM-80-25-600";
                        break;
                    case 61:
                        this._model = "QPM-24/36-50-380";
                        break;
                    case 62:
                        this._model = "QPM-48-40-380";
                        break;
                    case 63:
                        this._model = "QPM-80-25-380";
                        break;
                }

                _macAddress = MCBobject.AddressToSerial(_pmID, chargerSerialNumber);
                _instantError = "";

                if ((instantState & 0x0001) != 0)
                {
                    if (_instantError != "")
                        _instantError += ", ";
                    _instantError += "iout";
                }
                if ((instantState & 0x0002) != 0)
                {
                    if (_instantError != "")
                        _instantError += ", ";
                    _instantError += "vout";
                }
                if ((instantState & 0x0004) != 0)
                {
                    if (_instantError != "")
                        _instantError += ", ";
                    _instantError += "in_fail";
                }
                if ((instantState & 0x0008) != 0)
                {
                    if (_instantError != "")
                        _instantError += ", ";
                    _instantError += "temp_OL";
                }
                if ((instantState & 0x0010) != 0)
                {
                    if (_instantError != "")
                        _instantError += ", ";
                    _instantError += "Timeout";
                }
                if ((instantState & 0x0020) != 0)
                {
                    if (_instantError != "")
                        _instantError += ", ";
                    _instantError += "vout_UV";
                }
                if ((instantState & 0x0040) != 0)
                {
                    if (_instantError != "")
                        _instantError += ", ";
                    _instantError += "OCP";
                }
                if ((instantState & 0x0080) != 0)
                {
                    if (_instantError != "")
                        _instantError += ", ";
                    _instantError += "FAN";
                }
                if ((instantState & 0x0100) != 0)
                {

                    this._PM_reported_running = true;
                }
                else
                {
                    this._PM_reported_running = false;
                }
                if ((instantState & 0x0200) != 0)
                {

                    this._isPowerLimiting = true;
                }
                else
                {
                    this._isPowerLimiting = false;
                }

                if ((statusByte & 0x01) != 0x00)
                    _state = "Idle ";

                if ((statusByte & 0x04) != 0x00)
                    _state = "FAULT RESET";

                if ((statusByte & 0x10) != 0x00)
                    _state = "Dropped";

                if ((statusByte & 0x20) != 0x00)
                    _state = "Ramping";

                if ((statusByte & 0x40) != 0x00)
                    _state = "Shutting down";

                if ((statusByte & 0x80) != 0x00)
                    _state = "Runing";

                if (_state == "")
                {
                    _state = NO_PM;
                }

                if (_pmID != 0xffffffff && _state == NO_PM)
                    _state = "LOST";
                if (_state == NO_PM)
                {
                    _valid = false;
                }
                else
                {
                    _valid = true;
                }
            }
        }
    }

}
