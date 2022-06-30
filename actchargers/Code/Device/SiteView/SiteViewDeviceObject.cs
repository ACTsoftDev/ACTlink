using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using MvvmCross.Core.ViewModels;

namespace actchargers
{
    public abstract class SiteViewDeviceObject : MvxViewModel
    {
        readonly FirmwareUpdateStage[] UPDATING_FIRMWARE_STAGES =
            {
                FirmwareUpdateStage.doingUpdate,
                FirmwareUpdateStage.sendingRequest,
                FirmwareUpdateStage.sentRequestDelayed
            };

        public event EventHandler OnFirmwareUpdateStepChanged;

        GenericDevice genericDeviceSource;

        SiteviewDeviceDownloader siteviewDeviceDownloader;

        protected virtual void Dispose(bool disposing)
        {
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected SiteViewDeviceObject
        (string serialNumber, string interfaceSn, UInt32 id, bool setFromSiteOverlay,
         string deviceName, SiteviewDeviceDownloader siteviewDeviceDownloader, DeviceTypes type)
        {
            myLock = new object();
            _requireRedraw = true;
            this._serialNumber = serialNumber;
            InterfaceSn = interfaceSn;
            this._id = id;
            this._fromSiteOverlay = this._OriginalfromSiteOverlay = setFromSiteOverlay;
            if (_fromSiteOverlay)
            {
                IsConnected = false;
            }
            else
            {
                IsConnected = true;
            }

            _busy = false;
            _actViewEnabled = false;
            _downloadPercentage = 0;
            _queueForRemoval = false;
            _containerVisible = true;
            _deviceName = deviceName;

            DeviceType = type;
            this.siteviewDeviceDownloader = siteviewDeviceDownloader;

            IsChecked = false;
            DeviceStatus = " ";
            ProgressCompleted = 0;
            ProgressBarVisibility = true;
            IsDownloading = false;
            ProgressMax = 100;

            Init();
        }

        void Init()
        {
            if (siteviewDeviceDownloader != null)
                siteviewDeviceDownloader.OnRefresh += SiteviewDeviceDownloader_OnRefresh;

            genericDeviceSource = BuildGenericDeviceSource();
            if (genericDeviceSource != null)
                InitEvents();
        }

        void InitEvents()
        {
            genericDeviceSource
                .OnFirmwareUpdateStepChanged += GenericDeviceSource_OnFirmwareUpdateStepChanged;
            genericDeviceSource
                .OnProgressCompletedChanged += GenericDeviceSource_OnProgressCompletedChanged;
        }

        void SiteviewDeviceDownloader_OnRefresh(object sender, EventArgs e)
        {
            int newProgressCompleted = siteviewDeviceDownloader.ProgressCompleted;

            if (newProgressCompleted > ProgressCompleted)
                ProgressCompleted = newProgressCompleted;
        }

        void GenericDeviceSource_OnFirmwareUpdateStepChanged(object sender, EventArgs e)
        {
            FirmwareUpdateStep = genericDeviceSource.FirmwareUpdateStep;
        }

        void GenericDeviceSource_OnProgressCompletedChanged(object sender, EventArgs e)
        {
            if (siteviewDeviceDownloader.IsDownloading)
                siteviewDeviceDownloader.SetStackedProgressCompleted(genericDeviceSource.ProgressCompleted);
            else
                ProgressCompleted = genericDeviceSource.ProgressCompleted;
        }

        public abstract GenericDevice BuildGenericDeviceSource();

        public GenericDevice GetGenericDeviceSource()
        {
            return genericDeviceSource;
        }

        public bool CanDownloadOrUpdateDevice()
        {
            bool canDownloadOrUpdateDevice =
                (IsCheckedAndConnected()) &&
                (!isBusy) &&
                (!queueForRemoval);

            return canDownloadOrUpdateDevice;
        }

        public async Task<Tuple<bool, string>> SiteViewUpdate()
        {
            if (genericDeviceSource == null)
                return new Tuple<bool, string>(false, AppResources.opration_failed);

            return await genericDeviceSource.SiteViewUpdate();
        }


        public void AbortUpdate()
        {
            GenericDevice genericDevice = BuildGenericDeviceSource();

            if (IsDeviceUpdating())
                genericDevice.AbortUpdate();
        }

        public async Task<bool> Download()
        {
            SiteViewDevice siteViewDevice =
                SiteViewQuantum.Instance.GetConnectionManager()
                               .siteView.getDevice(InterfaceSn);

            return await siteviewDeviceDownloader.DownloadDevice(siteViewDevice);
        }

        public void AbortDownload()
        {
            siteviewDeviceDownloader.Abort();
        }

        public IMvxCommand ItemCheckBtnCommand
        {
            get
            {
                return new MvxCommand(ItemCheckClick);
            }
        }

        void ItemCheckClick()
        {
            IsChecked = !IsChecked;
        }


        /// <summary>
        /// The progress bar visibility.
        /// </summary>
        private bool _ProgressBarVisibility;
        public bool ProgressBarVisibility
        {
            get { return _ProgressBarVisibility; }
            set
            {
                _ProgressBarVisibility = value;
                ProgressBarVisibilityIOS = !value;

                RaisePropertyChanged(() => ProgressBarVisibility);
                RaisePropertyChanged(() => ProgressBarVisibilityIOS);
            }
        }

        /// <summary>
        /// The progress bar visibility for ios.
        /// </summary>
        private bool _ProgressBarVisibilityIOS;
        public bool ProgressBarVisibilityIOS
        {
            get { return _ProgressBarVisibilityIOS; }
            set
            {
                _ProgressBarVisibilityIOS = value;
                RaisePropertyChanged(() => ProgressBarVisibilityIOS);
            }
        }

        int progressCompleted;
        public int ProgressCompleted
        {
            get { return progressCompleted; }
            set
            {
                if (value >= 100)
                {
                    DeviceStatus = AppResources.completed;
                }

                SetProperty(ref progressCompleted, value);

                RaisePropertyChanged(() => ProgressCompletedIOS);
            }
        }

        public float ProgressCompletedIOS
        {
            get
            {
                if (ProgressCompleted == 0)
                    return 0.0f;

                return ProgressCompleted / 100.0f;
            }
        }

        private float _ProgressMax;
        public float ProgressMax

        {
            get { return _ProgressMax; }
            set
            {
                _ProgressMax = value;
                RaisePropertyChanged(() => ProgressMax);
            }
        }



        protected object myLock;
        protected bool _requireRedraw;
        public bool requireRedraw
        {
            get
            {
                lock (myLock)
                    return _requireRedraw;
            }
            protected set
            {
                lock (myLock)
                    _requireRedraw = value;
            }
        }

        private bool _BattviewImageVisibility;
        public bool BattviewImageVisibility
        {
            get
            {
                return _BattviewImageVisibility;
            }
            set
            {
                _BattviewImageVisibility = value;
                RaisePropertyChanged(() => BattviewImageVisibility);
            }
        }

        private bool _ChargerImageVisibility;
        public bool ChargerImageVisibility
        {
            get
            {
                return _ChargerImageVisibility;
            }
            set
            {
                _ChargerImageVisibility = value;
                RaisePropertyChanged(() => ChargerImageVisibility);
            }
        }


        private string _DeviceStatus;
        public string DeviceStatus
        {
            get
            {
                return _DeviceStatus;
            }
            set
            {
                _DeviceStatus = value;
                RaisePropertyChanged(() => DeviceStatus);
            }
        }

        protected bool _fromSiteOverlay;
        public bool fromSiteOnly
        {
            get
            {
                lock (myLock)
                {
                    return _fromSiteOverlay;
                }
            }
            set
            {
                lock (myLock)
                {
                    if (value != _fromSiteOverlay)
                        _requireRedraw = true;
                    _fromSiteOverlay = value;
                }

            }
        }

        bool notSite;
        public bool NotSite
        {
            get
            {
                lock (myLock)
                {
                    return notSite;
                }
            }
            set
            {
                lock (myLock)
                {
                    if (value != notSite)
                        _requireRedraw = true;
                    notSite = value;
                }

            }
        }

        public bool IsSite
        {
            get
            {
                return !(NotSite);
            }
        }

        protected bool _OriginalfromSiteOverlay;
        public bool mappedToSite
        {
            get
            {
                lock (myLock)
                {
                    return _OriginalfromSiteOverlay;
                }
            }
        }

        protected string _serialNumber;
        public string serialNumber
        {
            get
            {
                lock (myLock)
                {
                    return _serialNumber;
                }
            }
            set
            {
                lock (myLock)
                {
                    if (value != _serialNumber)
                        _requireRedraw = true;
                    _serialNumber = value;
                }

            }
        }

        protected string _interfaceSn;
        public string InterfaceSn
        {
            get
            {
                lock (myLock)
                {
                    return _interfaceSn;
                }
            }
            set
            {
                lock (myLock)
                {
                    if (value != _serialNumber)
                        _requireRedraw = true;
                    _interfaceSn = value;
                }

            }
        }

        string GetInterfaceSn()
        {
            string prefix = GetInterfaceSnPrefix();

            string interfaceSn =
                string.Format("{0}_{1}:{2}", prefix, id, serialNumber);

            return interfaceSn;
        }

        internal abstract string GetInterfaceSnPrefix();

        protected string _deviceName;
        public string deviceName
        {
            get
            {
                lock (myLock)
                {
                    return _deviceName;
                }
            }
            set
            {
                lock (myLock)
                {
                    if (value != _deviceName)
                        _requireRedraw = true;
                    _deviceName = value;
                }

            }
        }
        protected UInt32 _id;
        public UInt32 id
        {
            get
            {
                lock (myLock)
                {
                    return _id;
                }
            }
            set
            {
                lock (myLock)
                {
                    if (value != _id)
                        _requireRedraw = true;
                    _id = value;
                }

            }
        }
        protected bool _busy;
        public bool isBusy
        {
            get
            {
                lock (myLock)
                {
                    return _busy;
                }
            }
            set
            {
                lock (myLock)
                {


                    _busy = value;
                }

            }
        }

        protected bool _IsDownloading;
        public bool IsDownloading
        {
            get
            {
                lock (myLock)
                {
                    return _IsDownloading;
                }
            }
            set
            {
                lock (myLock)
                {
                    if (value)
                    {
                        DeviceStatus = AppResources.downloading;
                    }

                    SetProperty(ref _IsDownloading, value);
                }

            }
        }
        protected bool _queueForRemoval;
        public bool queueForRemoval
        {
            get
            {
                lock (myLock)
                {
                    return _queueForRemoval;
                }
            }
            set
            {
                lock (myLock)
                {


                    _queueForRemoval = value;
                }

            }
        }
        protected bool _containerVisible;
        public bool containerVisible
        {
            get
            {
                lock (myLock)
                {
                    return _containerVisible;
                }
            }
            set
            {
                lock (myLock)
                {


                    _containerVisible = value;
                }

            }
        }
        bool isConnected;
        public bool IsConnected
        {
            get
            {
                lock (myLock)
                {
                    return isConnected;
                }
            }
            set
            {
                lock (myLock)
                {
                    _requireRedraw |= value != isConnected;

                    isConnected = value;

                    if (isConnected && _fromSiteOverlay)
                        _fromSiteOverlay = false;
                    if (!isConnected)
                    {
                        OnConnectTriger();

                        _busy = false;
                    }
                }
            }
        }

        abstract public void OnConnectTriger();

        protected bool _actViewEnabled;
        public bool actViewEnabled
        {
            get
            {
                lock (myLock)
                {
                    return _actViewEnabled;
                }
            }
            set
            {
                lock (myLock)
                {
                    if (value != _actViewEnabled)
                        _requireRedraw = true;

                    _actViewEnabled = value;

                }

            }
        }

        protected int _downloadPercentage;
        public int downloadPercentage
        {
            get
            {
                lock (myLock)
                {
                    return _downloadPercentage;
                }
            }
            set
            {
                lock (myLock)
                {
                    if (value != _downloadPercentage)
                        _requireRedraw = true;

                    _downloadPercentage = value;
                    ProgressCompleted = value;

                }

            }
        }

        private string _ImageString;
        public string ImageString
        {
            get
            {
                return _ImageString;
            }
            set
            {
                _ImageString = value;
                RaisePropertyChanged(() => ImageString);
            }
        }


        private string _CheckedImageString;
        public string CheckedImageString
        {
            get
            {
                return _CheckedImageString;
            }
            set
            {
                _CheckedImageString = value;
                RaisePropertyChanged(() => CheckedImageString);
            }
        }

        DeviceTypes _deviceType;
        public DeviceTypes DeviceType
        {
            get
            {
                return _deviceType;
            }
            set
            {
                _deviceType = value;
            }
        }

        protected bool _isChecked;
        public bool IsChecked
        {
            get
            {
                lock (myLock)
                {
                    return _isChecked;
                }
            }
            set
            {
                lock (myLock)
                {
                    if (value != _isChecked)
                        _requireRedraw = true;

                    _isChecked = value;
                    RaisePropertyChanged(() => IsChecked);
                }

                CheckedImageString = BoolToImageNameConverter.BoolToImageName(value);
            }
        }

        FirmwareUpdateStage firmwareUpdateStep;
        public FirmwareUpdateStage FirmwareUpdateStep
        {
            get
            {
                lock (myLock)
                    return firmwareUpdateStep;

            }
            set
            {
                lock (myLock)
                {
                    if (value != firmwareUpdateStep)
                        _requireRedraw = true;
                    firmwareUpdateStep = value;
                }

                FireOnFirmwareUpdateStepChanged();
            }
        }



        protected int firmwreUpdateProgress;
        public int FirmwreUpdateProgress
        {
            get
            {
                lock (myLock)
                    return firmwreUpdateProgress;

            }
            set
            {
                lock (myLock)
                {
                    if (value != firmwreUpdateProgress)
                        _requireRedraw = true;
                    firmwreUpdateProgress = value;
                }
            }
        }

        protected Color _myPanelColor;
        public Color myPanelColor
        {
            get
            {
                lock (myLock)
                    return _myPanelColor;

            }
            set
            {
                lock (myLock)
                {
                    if (value != _myPanelColor)
                        _requireRedraw = true;
                    _myPanelColor = value;
                }
            }
        }

        public bool IsCheckedAndConnected()
        {
            return IsChecked && IsConnected;
        }

        public void forceRefresh()
        {
            this.requireRedraw = true;
        }

        public abstract void LoadMyImage();

        public void CheckForUpdate()
        {
            SiteViewDevice siteViewDevice =
                SiteViewQuantum.Instance.GetConnectionManager()
                               .siteView.getDevice(InterfaceSn);

            if (siteViewDevice.firmwareStage == FirmwareUpdateStage.sentRequestPassed)
            {
                FirmwareUpdateStep = siteViewDevice.firmwareStage;

                return;
            }

            genericDeviceSource.GetDeviceObjectParent();

            if (RequireUpdate(genericDeviceSource.GetDeviceObjectParent()))
                FirmwareUpdateStep = FirmwareUpdateStage.UPDATE_IS_REQUIRED;
            else
                FirmwareUpdateStep = FirmwareUpdateStage.updateIsNotNeeded;
        }

        internal abstract bool RequireUpdate(DeviceObjectParent device);

        public void CheckToResetStatusForDevice()
        {
            if (IsDeviceUpdating())
                ResetStatusForDevice();
        }

        bool IsDeviceUpdating()
        {
            return UPDATING_FIRMWARE_STAGES.Contains(FirmwareUpdateStep);
        }

        void ResetStatusForDevice()
        {
            CheckForUpdate();
        }

        void FireOnFirmwareUpdateStepChanged()
        {
            OnFirmwareUpdateStepChanged?.Invoke(this, EventArgs.Empty);
        }

        public void SimplyUpdateUi()
        {
            LoadMyImage();

            ProgressBarVisibility = true;

            UpdateStatusFields();
        }

        void UpdateStatusFields()
        {
            string deviceStatus = "";

            switch (FirmwareUpdateStep)
            {
                case FirmwareUpdateStage.connectting:
                    deviceStatus = "";

                    break;

                case FirmwareUpdateStage.UPDATE_IS_REQUIRED:
                    deviceStatus = AppResources.require_update;

                    break;

                case FirmwareUpdateStage.sendingRequest:
                    deviceStatus = AppResources.updating;

                    break;

                case FirmwareUpdateStage.doingUpdate:
                    deviceStatus = AppResources.updating;

                    break;

                case FirmwareUpdateStage.sentRequestDelayed:
                case FirmwareUpdateStage.sentRequestPassed:
                    deviceStatus = AppResources.verifying;

                    break;

                case FirmwareUpdateStage.updateCompleted:
                    deviceStatus = AppResources.updated;

                    ProgressCompleted = 100;

                    break;

                case FirmwareUpdateStage.updateIsNotNeeded:
                    deviceStatus = AppResources.uptodate;

                    break;

                case FirmwareUpdateStage.FAILED:
                    deviceStatus = AppResources.failed;

                    ProgressCompleted = 0;

                    break;
            }

            DeviceStatus = deviceStatus;
        }
    }
}