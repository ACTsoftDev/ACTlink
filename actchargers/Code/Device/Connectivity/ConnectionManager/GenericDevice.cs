using System;
using System.Threading;
using System.Threading.Tasks;

namespace actchargers
{
    public abstract class GenericDevice : IDisposable
    {
        public event EventHandler OnFirmwareUpdateStepChanged;
        public event EventHandler OnProgressCompletedChanged;

        FirmwareUpdateStage firmwareUpdateStep;
        public FirmwareUpdateStage FirmwareUpdateStep
        {
            get
            {
                return firmwareUpdateStep;
            }
            set
            {
                firmwareUpdateStep = value;

                FireOnFirmwareUpdateStepChanged();
            }
        }

        int progressCompleted;
        public int ProgressCompleted
        {
            get
            {
                return progressCompleted;
            }
            set
            {
                progressCompleted = value;

                FireOnProgressCompletedChanged();
            }
        }

        void FireOnFirmwareUpdateStepChanged()
        {
            OnFirmwareUpdateStepChanged?.Invoke(this, EventArgs.Empty);
        }

        void FireOnProgressCompletedChanged()
        {
            OnProgressCompletedChanged?.Invoke(this, EventArgs.Empty);
        }

        private UInt32 _startSynchID;
        public UInt32 startSynchID
        {
            get
            {
                lock (myLock)
                {
                    return _startSynchID;
                }
            }
            set
            {
                lock (myLock)
                {
                    _startSynchID = value;
                }
            }
        }

        private bool _configAndglobalSaved;
        public bool configAndglobalSaved
        {
            get
            {
                lock (myLock)
                {
                    return _configAndglobalSaved;
                }
            }
            set
            {
                lock (myLock)
                {
                    _configAndglobalSaved = value;
                }
            }
        }
        private bool _flagOfRemoval;
        public bool flagOfRemoval
        {
            get
            {
                lock (myLock)
                {
                    return _flagOfRemoval;
                }
            }
            set
            {
                lock (myLock)
                {
                    _flagOfRemoval = value;
                }
            }
        }
        private bool _toggle;
        public bool toggle
        {
            get
            {
                lock (myLock)
                {
                    return _toggle;
                }
            }
            set
            {
                lock (myLock)
                {
                    _toggle = value;
                }
            }
        }
        Mutex accessLock;

        private int _limitsRefreshVal;
        public int limitsRefreshVal
        {
            get
            {
                lock (myLock)
                {
                    return _limitsRefreshVal;
                }
            }
            set
            {
                lock (myLock)
                {
                    _limitsRefreshVal = value;
                }
            }
        }
        abstract public CommunicationResult readGlobal();
        public bool limitRefresh(bool checkOnly = false)
        {
            if (checkOnly)
            {
                if (limitsRefreshVal + 1 >= 10)
                    return true;
                else
                    return false;
            }
            if (limitsRefreshVal++ >= 10)
            {
                if (this.readGlobal() == CommunicationResult.OK)
                {
                    limitsRefreshVal = 0;

                }
                return true;

            }
            return false;
        }
        public bool deviceLockME(int milliSeconds = 50)
        {
            return this.accessLock.WaitOne(milliSeconds);
        }
        public void BATTunlockMe()
        {
            this.accessLock.ReleaseMutex();
        }
        public void resetVars()
        {
            startSynchID = UInt32.MaxValue;
            limitsRefreshVal = 0;
            toggle = false;
        }

        protected object myLock;
        public GenericDevice()
        {
            myLock = new object();
            startSynchID = UInt32.MaxValue;
            limitsRefreshVal = 0;
            configAndglobalSaved = false;
            accessLock = new Mutex(false);
        }

        public void deviceUnlockMe()
        {
            this.accessLock.ReleaseMutex();
        }

        public abstract DeviceObjectParent GetDeviceObjectParent();

        public abstract Task<Tuple<bool, string>> SiteViewUpdate();

        public abstract void AbortUpdate();

        public void Dispose()
        {
            accessLock.Dispose();
        }
    }
}
