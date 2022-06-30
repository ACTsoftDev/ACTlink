using System;
namespace actchargers
{
    public class RouterInterfaceDevice
    {

        object myLock;
        bool accessLock;

        private string _ipAddress;
        public string ipAddress
        {
            get
            {
                lock (myLock)
                {
                    return _ipAddress;
                }
            }
            private set
            {
                lock (myLock)
                {
                    _ipAddress = value;
                }
            }
        }
        private bool _isDead;
        public bool isDead
        {
            get
            {
                lock (myLock)
                {
                    return _isDead;
                }
            }
            private set
            {
                lock (myLock)
                {
                    _isDead = value;
                }
            }
        }

        private bool _softKill;
        public bool softKill
        {
            get
            {
                lock (myLock)
                {
                    return _softKill;
                }
            }
            set
            {
                lock (myLock)
                {
                    _softKill = value;
                }
            }
        }
        private bool _AmICharger;
        public bool AmICharger
        {
            get
            {
                lock (myLock)
                {
                    return _AmICharger;
                }
            }
            set
            {
                lock (myLock)
                {
                    _AmICharger = value;
                }
            }
        }
        private DateTime _lastCommunicationTime;
        private DateTime lastCommunicationTime
        {
            get
            {
                lock (myLock)
                {
                    return new DateTime(_lastCommunicationTime.Ticks);
                }
            }
            set
            {
                lock (myLock)
                {
                    _lastCommunicationTime = value;
                }
            }
        }
        private int _failureCount;
        private int failureCount
        {
            get
            {
                lock (myLock)
                {
                    return _failureCount;
                }
            }
            set
            {
                lock (myLock)
                {
                    _failureCount = value;
                }
            }
        }
        public byte[] resultArrray;
        private int _contCount;
        private int contCount
        {
            get
            {
                lock (myLock)
                {
                    return _contCount;
                }
            }
            set
            {
                lock (myLock)
                {
                    _contCount = value;
                }
            }
        }

        private uint _delayX;
        private uint delayX
        {
            get
            {
                lock (myLock)
                {
                    return _delayX;
                }
            }
            set
            {
                lock (myLock)
                {
                    _delayX = value;
                }
            }
        }

        internal DefineObjectInfo def;

        public RouterInterfaceDevice
        (string ip, bool isCharger, UInt32 id, string sn, bool lostRTC,
         byte zoneID, float firmwareVersion, float firmwareWiFiVersion,
         UInt32 studyID, string name, byte dcId, float firmwareDcVersion, bool replacementPart, DeviceBaseType deviceType)
        {
            myLock = new object();
            accessLock = false;
            _delayX = 250;
            this._ipAddress = ip;
            _lastCommunicationTime = DateTime.UtcNow;
            _initTime = DateTime.UtcNow;
            _failureCount = 0;

            this._AmICharger = isCharger;
            resultArrray = new byte[0];
            _contCount = 0;
            this._failureCount = 0;
            def =
                new DefineObjectInfo
                (id, sn, lostRTC, zoneID, firmwareVersion, firmwareWiFiVersion,
                 isCharger, studyID, name, replacementPart, deviceType, _ipAddress, dcId, firmwareDcVersion);
        }

        public double getLastConnectTime()
        {
            return (DateTime.UtcNow - lastCommunicationTime).TotalSeconds;
        }
        public bool lockME(bool shortPeriod)
        {
            DateTime now = DateTime.UtcNow;
            TimeSpan s = TimeSpan.FromSeconds(999);
            if (shortPeriod)
            {
                s = TimeSpan.FromMilliseconds(50);
            }
            do
            {
                lock (myLock)
                {
                    if (!accessLock)
                    {
                        accessLock = true;
                        return true;
                    }
                }
            } while (DateTime.UtcNow - now < s);
            return false;
        }
        public void unlockMe()
        {
            lock (myLock)
            {
                if (!accessLock)
                {
                    throw new Exception("Unlocking an unlocked object");
                }
                accessLock = false;
            }
        }
        public bool hasFailures()
        {
            return (failureCount > 0);

        }
        public bool setResult(bool failed)
        {
            contCount++;
            if (failed)
            {
                failureCount++;
                delayX = 1500;
            }
            else
                failureCount = 0;
            if (failureCount >= 3)
            {
                isDead = true;
                return true;

            }
            lastCommunicationTime = DateTime.UtcNow;
            return false;

        }

        private DateTime _initTime;
        private DateTime initTime
        {
            get
            {
                lock (myLock)
                {
                    return new DateTime(_initTime.Ticks);
                }
            }
            set
            {
                lock (myLock)
                {
                    _initTime = value;
                }
            }
        }
        public bool shouldCommunicate()
        {

            if ((DateTime.UtcNow - lastCommunicationTime).TotalSeconds >= 12 && (DateTime.UtcNow - _initTime).TotalSeconds >= 60)
                return true;
            return false;
        }
        public double minSleepTimeLeft()
        {
            uint milli = (uint)(DateTime.UtcNow - lastCommunicationTime).TotalMilliseconds;
            uint r = 0;
            if (milli <= delayX)
                r = delayX - milli;
            delayX = 250;
            return r;

        }
    }
}
