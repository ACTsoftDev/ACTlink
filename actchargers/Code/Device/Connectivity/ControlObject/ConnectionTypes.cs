using System;

namespace actchargers
{
    public class ConnectionTypes
    {
        private object myLock;
        private bool _usb;
        public bool usb
        {
            get
            {
                lock (myLock)
                {
                    return _usb;
                }
            }
            set
            {
                lock (myLock)
                {
                    _usb = value;
                }
            }
        }
        private bool _debug;
        public bool debug
        {
            get
            {
                lock (myLock)
                {
                    return _debug;
                }
            }
            set
            {
                lock (myLock)
                {
                    _debug = value;
                }
            }
        }
        private bool _mobile;
        public bool mobile
        {
            get
            {
                lock (myLock)
                {
                    return _mobile;
                }
            }
            set
            {
                lock (myLock)
                {
                    _mobile = value;
                }
            }
        }
        private bool _mobile_router;
        public bool mobile_router
        {
            get
            {
                lock (myLock)
                {
                    return _mobile_router;
                }
            }
            set
            {
                lock (myLock)
                {
                    _mobile_router = value;
                }
            }
        }
        private bool _direct;
        public bool direct
        {
            get
            {
                lock (myLock)
                {
                    return _direct;
                }
            }
            set
            {
                lock (myLock)
                {
                    _direct = value;
                }
            }
        }
        private ConnectionTypesEnum _Connectiontype;
        public ConnectionTypesEnum Connectiontype
        {
            get
            {
                lock (myLock)
                {
                    return _Connectiontype;
                }
            }
            set
            {
                lock (myLock)
                {
                    _Connectiontype = value;
                }
            }
        }
        private string _ssid;
        public string ssid
        {
            get
            {
                lock (myLock)
                {
                    return _ssid;
                }
            }
            set
            {
                lock (myLock)
                {
                    _ssid = value;
                }
            }
        }
        private string _ssidPassword;
        public string ssidPassword
        {
            get
            {
                lock (myLock)
                {
                    return _ssidPassword;
                }
            }
            set
            {
                lock (myLock)
                {
                    _ssidPassword = value;
                }
            }
        }
        private UInt16 _port;
        public UInt16 port
        {
            get
            {
                lock (myLock)
                {
                    return _port;
                }
            }
            set
            {
                lock (myLock)
                {
                    _port = value;
                }
            }
        }
        public ConnectionTypes()
        {
            myLock = new object();
            usb = false;
            debug = false;
            mobile = false;
            mobile_router = false;
            direct = false;
            Connectiontype = ConnectionTypesEnum.INVALID;
        }
    }

}
