using System;

namespace actchargers
{
    public class ChargeState
    {
        private string _leftTime;
        public string leftTime
        {
            get
            {
                lock (myLock)
                {

                    return _leftTime;
                }
            }
        }

        private string _totalCurrent;
        public string totalCurrent
        {
            get
            {
                lock (myLock)
                {

                    return _totalCurrent;
                }
            }
        }

        private string _state;
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

        private string _voltage;
        public string voltage
        {
            get
            {
                lock (myLock)
                {

                    return _voltage;
                }
            }
        }

        private string _status;
        public string status
        {
            get
            {
                lock (myLock)
                {

                    return _status;
                }
            }
        }

        private string _lastErrorCode;
        public string lastErrorCode
        {
            get
            {
                lock (myLock)
                {

                    return _lastErrorCode;
                }
            }
        }

        private bool _isinSimulationMode;
        public bool isinSimulationMode
        {
            get
            {
                lock (myLock)
                {

                    return _isinSimulationMode;
                }
            }
        }

        internal void DeSerialize(byte[] result)
        {
            lock (myLock)
            {
                _isinSimulationMode = false;
                switch (result[12])
                {
                    case 0:
                        _status = "";
                        break;
                    case 0xFF:
                        _status = "Simulation Mode";
                        _isinSimulationMode = true;
                        break;
                    case 0x01:
                        _status = "In start Up";
                        break;
                    case 0x02:
                        _status = "In Charge";
                        break;
                    case 0x03:
                        _status = "In ERR-Timeout";
                        break;
                    case 0x04:
                        _status = "In Cycle Done";
                        break;
                    case 0x05:
                        _status = "In Batt-Diss";
                        break;
                    case 0x06:
                        _status = "In Batt-OT";
                        break;
                    case 0x07:
                        _status = "In Paused";
                        break;
                    case 0x08:
                        _status = "In Internal ERR";
                        break;
                    case 0x09:
                        _status = "In Fault";
                        break;
                }

                UInt16 stateBits;
                _leftTime = (BitConverter.ToUInt32(result, 0)).ToString();
                _totalCurrent = (BitConverter.ToUInt32(result, 4) / 10.0).ToString();
                stateBits = BitConverter.ToUInt16(result, 8);
                _voltage = (BitConverter.ToInt16(result, 10) / 100.0).ToString();


                _state = "";
                if ((stateBits & 0x0001) != 0)
                    _state += "CAN BUS busy";
                if ((stateBits & 0x0002) != 0)
                    _state += "Recommend to Recycle Power";
                if ((stateBits & 0x0004) != 0)
                    _state += "ERROR encountered";
                if ((stateBits & 0x0008) != 0)
                    _state += "CC Mode: Desired voltage has been reached, CV: minumum Current has been reached";
                if ((stateBits & 0x0010) != 0)
                    _state += "Cycle Timer Expired";
                if ((stateBits & 0x0020) != 0)
                    _state += "Cycle Suspended";
                if ((stateBits & 0x0040) != 0)
                    _state += "dv/dt requirments has been met";

                switch (result[13])
                {
                    case 0x00:
                        _lastErrorCode = "NO ERRORS";
                        break;
                    case 0x05:
                        _lastErrorCode = "iout_OV_FAULT";
                        break;
                    case 0x06:
                        _lastErrorCode = "vout_OV_FAULT";
                        break;
                    case 0x07:
                        _lastErrorCode = "in_fail_FAULT";
                        break;
                    case 0x08:
                        _lastErrorCode = "temp_OL_FAULT";
                        break;
                    case 0x09:
                        _lastErrorCode = "timeout_FAULT";
                        break;
                    case 0x0A:
                        _lastErrorCode = "vout_UV_FAULT";
                        break;
                    case 0x0B:
                        _lastErrorCode = "OCP_FAULT";
                        break;

                    case 0x0E:
                        _lastErrorCode = "CONTROL SATURATION";
                        break;
                    case 0x0F:
                        _lastErrorCode = "OVDS";
                        break;
                    case 0x10:
                        _lastErrorCode = "FAN FAULT";
                        break;
                    case 0x11:
                        _lastErrorCode = "Battery OverTemperature";
                        break;
                    case 0x0C:
                        _lastErrorCode = "Disconnection_FAULT";
                        break;
                }
            }
        }
        object myLock;
        public ChargeState()
        {
            myLock = new object();
        }



    }

}
