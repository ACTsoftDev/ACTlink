using System;

namespace actchargers
{
    public class MCBHealthCheck
    {
        private byte _internalFailureCode;
        public byte internalFailureCode
        {
            get
            {
                return _internalFailureCode;

            }
        }

        private byte _PM_failure;
        public byte PM_failure
        {
            get
            {
                return _PM_failure;

            }
        }

        private byte _PLC_Failure;
        public byte PLC_Failure
        {
            get
            {
                return _PLC_Failure;

            }
        }

        private byte _WIFI_Failure;
        public byte WIFI_Failure
        {
            get
            {
                return _WIFI_Failure;

            }
        }

        private byte _PLC_communicated;
        public byte PLC_communicated
        {
            get
            {
                return _PLC_communicated;

            }
        }

        private byte _WIFI_communicated;
        public byte WIFI_communicated
        {
            get
            {
                return _WIFI_communicated;

            }
        }

        private byte _V_OK;
        public byte V_OK
        {
            get
            {
                return _V_OK;

            }
        }

        private DateTime _connectTime;
        public DateTime connectTime
        {
            get
            {
                return new DateTime(_connectTime.Ticks);

            }
        }

        public MCBHealthCheck()
        {
            _internalFailureCode = 0;
            _PM_failure = 0;
            _PLC_Failure = 0;
            _PLC_communicated = 0;
            _PLC_communicated = 0;
            _WIFI_communicated = 0;
            _V_OK = 0;
            _connectTime = DateTime.UtcNow;
        }

        public void loadFromArray(byte[] resultArray)
        {
            int loc = 0;
            _internalFailureCode = resultArray[loc++];
            _PM_failure = resultArray[loc++];
            _PLC_Failure = resultArray[loc++];
            _WIFI_Failure = resultArray[loc++];
            _PLC_communicated = resultArray[loc++];
            _WIFI_communicated = resultArray[loc++];
            _V_OK = resultArray[loc++];
        }
    }

}
