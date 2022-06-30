using System;
using System.Threading;

namespace actchargers
{
    public class SiteViewDevice
    {
        private Mutex accessLock;
        private UInt32 _id;

        private DeviceTypes _deviceType;
        public DeviceTypes deviceType
        {
            get
            {
                DeviceTypes temp;
                LockME();
                temp = _deviceType;
                unlockMe();
                return temp;
            }
            set
            {
                LockME();
                _deviceType = value;
                unlockMe();
            }
        }
        public UInt32 id
        {
            get
            {
                UInt32 temp;
                LockME();
                temp = _id;
                unlockMe();
                return temp;
            }
        }
        private string _userNamed;
        public string userNamed
        {
            get
            {
                string temp;
                LockME();
                temp = _userNamed;
                unlockMe();
                return temp;
            }
            set
            {
                LockME();
                _userNamed = value;
                unlockMe();
            }
        }
        private string _serialnumber;
        public string serialnumber
        {
            get
            {
                string temp;
                LockME();
                temp = _serialnumber;
                unlockMe();
                return temp;
            }
            set
            {
                LockME();
                _serialnumber = value;
                unlockMe();
            }
        }


        private bool _configurationsSaved;
        public bool configurationsSaved
        {
            get
            {
                bool temp;
                LockME();
                temp = _configurationsSaved;
                unlockMe();
                return temp;

            }
            set
            {
                LockME();
                _configurationsSaved = value;
                unlockMe();
            }
        }
        private bool _configurationsRead;
        public bool configurationsRead
        {
            get
            {
                bool temp;
                LockME();
                temp = _configurationsRead;
                unlockMe();
                return temp;

            }
        }
        public void setConfigurationsRead(bool isRead, byte dcId, float firmwareVersion, float firmwareWiFiVersion, float firmwareDcVersion)
        {
            LockME();

            _configurationsRead = isRead;
            _lastKnownReadTime = DateTime.UtcNow;
            if (isRead && firmwareVersion > 1)
            {
                this.firmwareVersion = firmwareVersion;
                bool requireUpdate = false;
                if (_deviceType == DeviceTypes.mcb || _deviceType == DeviceTypes.mcbReplacement)
                    requireUpdate = Firmware.DoesMcbRequireUpdate(dcId, firmwareVersion, firmwareWiFiVersion, firmwareDcVersion);
                else
                    requireUpdate = Firmware.DoesBattViewRequireUpdate(firmwareVersion, firmwareWiFiVersion);

                if (!requireUpdate && _FirmwareUpdateStage != FirmwareUpdateStage.updateIsNotNeeded)
                    _FirmwareUpdateStage = FirmwareUpdateStage.updateCompleted;
            }
            unlockMe();

        }
        private bool _limitsLoaded;
        public bool limitsLoaded
        {
            get
            {
                bool temp;
                LockME();
                temp = _limitsLoaded;
                unlockMe();
                return temp;

            }
            set
            {
                LockME();
                _limitsLoaded = value;
                unlockMe();
            }
        }


        private UInt32 _startEventID;
        public UInt32 startEventID
        {
            get
            {
                UInt32 temp;
                LockME();
                temp = _startEventID;
                unlockMe();
                return temp;

            }
            private set
            {
                LockME();
                _startEventID = value;
                unlockMe();
            }
        }

        private UInt32 _endEventID;
        public UInt32 endEventID
        {
            get
            {
                UInt32 temp;
                LockME();
                temp = _endEventID;
                unlockMe();
                return temp;

            }
            set
            {
                LockME();
                _endEventID = value;
                unlockMe();
            }
        }

        private UInt32 _downloadEventID;
        public UInt32 downloadEventID
        {
            get
            {
                UInt32 temp;
                LockME();
                temp = _downloadEventID;
                unlockMe();
                return temp;

            }
            set
            {
                LockME();
                _downloadEventID = value;
                unlockMe();
            }
        }

        private UInt32 _startPMID;
        public UInt32 startPMID
        {
            get
            {
                UInt32 temp;
                LockME();
                temp = _startPMID;
                unlockMe();
                return temp;

            }
            private set
            {
                LockME();
                _startPMID = value;
                unlockMe();
            }
        }

        private UInt32 _endPMID;
        public UInt32 endPMID
        {
            get
            {
                UInt32 temp;
                LockME();
                temp = _endPMID;
                unlockMe();
                return temp;

            }
            private set
            {
                LockME();
                _endPMID = value;
                unlockMe();
            }
        }

        private UInt32 _downloadPMID;
        public UInt32 downloadPMID
        {
            get
            {
                UInt32 temp;
                LockME();
                temp = _downloadPMID;
                unlockMe();
                return temp;

            }
            set
            {
                LockME();
                _downloadPMID = value;
                unlockMe();
            }
        }
        private bool _actViewEnabled;
        public bool actViewEnabled
        {
            get
            {
                bool temp;
                LockME();
                temp = _actViewEnabled;
                unlockMe();
                return temp;

            }
            set
            {
                LockME();
                _actViewEnabled = value;
                unlockMe();
            }
        }
        public void setLimits(UInt32 startId, UInt32 endId, UInt32 PMStartId, UInt32 PMEndID)
        {
            LockME();
            this._startEventID = startId;
            this._endEventID = endId;
            this._startPMID = PMStartId;
            this._endPMID = PMEndID;
            this._limitsLoaded = true;
            this._downloadEventID = startId;
            this._downloadPMID = PMStartId;

            unlockMe();
        }
        public bool LockME(int milliSeconds = 50)
        {
            return this.accessLock.WaitOne(milliSeconds);
        }
        private bool LockME()
        {
            return this.accessLock.WaitOne();
        }
        public void unlockMe()
        {
            this.accessLock.ReleaseMutex();
        }

        private bool _deviceConnected;
        public bool deviceConnected
        {
            get
            {
                bool temp;
                LockME();
                temp = _deviceConnected;
                unlockMe();
                return temp;

            }
            set
            {
                LockME();
                _deviceConnected = value;
                _deviceDisconnectTime = DateTime.UtcNow;
                unlockMe();
            }
        }

        private DateTime _deviceDisconnectTime;
        public DateTime deviceDisconnectTime
        {
            get
            {
                DateTime temp;
                LockME();
                temp = _deviceDisconnectTime;
                unlockMe();
                return temp;

            }

        }

        private string _interfaceSN;
        public string interfaceSN
        {
            get
            {
                string temp;
                LockME();
                temp = _interfaceSN;
                unlockMe();
                return temp;
            }

        }

        float _firmwareVersion;
        public float firmwareVersion
        {
            get
            {
                float temp;
                LockME();
                temp = _firmwareVersion;
                unlockMe();
                return temp;
            }
            set
            {
                LockME();
                _firmwareVersion = value;
                unlockMe();
            }
        }

        float _firmwareWiFiVersion;
        public float firmwareWiFiVersion
        {
            get
            {
                float temp;
                LockME();
                temp = _firmwareWiFiVersion;
                unlockMe();
                return temp;
            }
            set
            {
                LockME();
                _firmwareWiFiVersion = value;
                unlockMe();
            }
        }

        private FirmwareUpdateStage _FirmwareUpdateStage;
        public FirmwareUpdateStage firmwareStage
        {
            get
            {
                FirmwareUpdateStage temp;
                LockME();
                temp = _FirmwareUpdateStage;
                unlockMe();
                return temp;
            }
            set
            {
                LockME();
                _FirmwareUpdateStage = value;
                unlockMe();
            }
        }

        private int _firmwareUpdateStep;
        public int firmwareUpdateStep
        {
            get
            {
                int temp;
                LockME();
                temp = _firmwareUpdateStep;
                unlockMe();
                return temp;
            }
            set
            {
                LockME();
                _firmwareUpdateStep = value;
                unlockMe();
            }
        }
        private DateTime _lastKnownReadTime;
        public DateTime lastKnownReadTime
        {
            get
            {
                DateTime temp;
                LockME();
                temp = _lastKnownReadTime;
                unlockMe();
                return temp;

            }

        }

        private LcdSimulator _mcbProfile;
        private DateTime _lcdSimTime;
        public DateTime lcdSimTime
        {
            get
            {
                DateTime temp;
                LockME();
                temp = _lcdSimTime;
                unlockMe();
                return temp;
            }
            set
            {
                LockME();
                _lcdSimTime = value;
                unlockMe();
            }
        }
        public SiteViewDevice
        (UInt32 deviceUniqueID, DeviceTypes type, string deviceSN, string userNamed,
         string interfaceSN, byte dcId, float firmwareVersion, float firmwareWiFiVersion, float firmwareDcVersion)
        {
            this.accessLock = new Mutex();
            this._id = deviceUniqueID;
            this._deviceType = type;
            this._serialnumber = deviceSN;
            this._userNamed = userNamed;
            this._configurationsSaved = false;
            this._configurationsRead = false;
            this._deviceConnected = true;
            this._interfaceSN = interfaceSN;
            this.firmwareVersion = firmwareVersion;
            this._limitsLoaded = false;
            this._actViewEnabled = false;
            _mcbProfile = null;
            this._lcdSimTime = DateTime.UtcNow;
            this._firmwareUpdateStep = 0;
            bool requireUpdate;
            if (_deviceType == DeviceTypes.mcb || _deviceType == DeviceTypes.mcbReplacement)
                requireUpdate = Firmware.DoesMcbRequireUpdate(dcId, firmwareVersion, firmwareWiFiVersion, firmwareDcVersion);
            else
                requireUpdate = Firmware.DoesBattViewRequireUpdate(firmwareVersion, firmwareWiFiVersion);

            if (requireUpdate)
            {
                this._FirmwareUpdateStage = FirmwareUpdateStage.connectting;

            }
            else
            {
                this._FirmwareUpdateStage = FirmwareUpdateStage.updateIsNotNeeded;

            }
        }

        public void updateMCBLCDSimulator(LcdSimulator mcbProfile)
        {
            lock (accessLock)
            {
                this._mcbProfile = mcbProfile;
                this._lcdSimTime = DateTime.UtcNow;
            }

        }


        public LcdSimulator getMCBLCD()
        {
            if (this._mcbProfile == null)
                return null;
            lock (accessLock)
            {
                return _mcbProfile.GetAcopy();
            }

        }

    }
}
