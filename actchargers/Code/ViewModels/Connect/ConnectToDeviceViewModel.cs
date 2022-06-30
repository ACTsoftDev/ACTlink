using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;
using Newtonsoft.Json;

namespace actchargers
{
    public class ConnectToDeviceViewModel : BaseViewModel
    {
        static object myLock = new object();
        ConnectionManager connManager;
        MvxSubscriptionToken progressSubscriptionToken;
        MvxSubscriptionToken addDeviceSubscriptionToken;
        MvxSubscriptionToken ResetSubscriptionToken;
        MvxSubscriptionToken ActionMenuSubscriptionToken;
        MvxSubscriptionToken backScanActionSubsciptionToken;
        MvxSubscriptionToken StartScanSubsciptionToken;

        List<DBConnectedDevices> dbDevices;
        public string ScanBtnText { get; set; }

        /// <summary>
        /// The scan status message.
        /// </summary>
        string _scanStatusMessage;
        public string ScanStatusMessage
        {
            get { return _scanStatusMessage; }
            set
            {
                _scanStatusMessage = value;
                RaisePropertyChanged(() => ScanStatusMessage);
            }
        }

        int _SelectedTabIndex;
        public int SelectedTabIndex
        {
            get { return _SelectedTabIndex; }
            set
            {
                _SelectedTabIndex = value;
                if (value == 0)
                {
                    IsBattView = true;
                    IsReplacement = false;
                    BattViewCount = BattViewItemSource.Count;
                    RaisePropertyChanged(() => BattViewCount);
                }
                else if (value == 1)
                {
                    IsBattView = false;
                    IsReplacement = false;
                    Chargerscount = ChargerItemSource.Count;
                    RaisePropertyChanged(() => Chargerscount);
                }
                else if (value == 2)
                {
                    IsReplacement = true;
                    ReplacementCount = ReplacementItemSource.Count;
                    RaisePropertyChanged(() => ReplacementCount);
                }

                if (IsReplacement)
                {
                    ListItemSource = ReplacementItemSource;
                }
                else if (IsBattView)
                {
                    ListItemSource = BattViewItemSource;
                }
                else
                {
                    ListItemSource = ChargerItemSource;
                }

                RaisePropertyChanged(() => SelectedTabIndex);
            }
        }
        int _ChargersCount;
        public int Chargerscount
        {
            get { return _ChargersCount; }
            set
            {
                _ChargersCount = value;
                RaisePropertyChanged(() => Chargerscount);
            }
        }
        int _ReplacementCount;
        public int ReplacementCount
        {
            get { return _ReplacementCount; }
            set
            {
                _ReplacementCount = value;
                RaisePropertyChanged(() => ReplacementCount);
            }
        }
        int _BattViewCount;
        public int BattViewCount
        {
            get { return _BattViewCount; }
            set
            {
                _BattViewCount = value;
                RaisePropertyChanged(() => BattViewCount);
            }
        }


        bool _scanBtnVisibility;
        public bool ScanBtnVisibility
        {
            get
            {
                return _scanBtnVisibility;
            }
            set
            {
                _scanBtnVisibility = value;
                //ScanBtnVisibilityIOS = !value;
                RaisePropertyChanged(() => ScanBtnVisibility);
            }
        }

        // bool _scanBtnVisibilityIOS;
        //public bool ScanBtnVisibilityIOS
        //{
        //    get
        //    {
        //        return _scanBtnVisibilityIOS;
        //    }
        //    set
        //    {
        //        _scanBtnVisibilityIOS = value;
        //        RaisePropertyChanged(() => ScanBtnVisibilityIOS);
        //    }
        //}

        /// <summary>
        /// The found devices count.
        /// </summary>
        string _foundDevicesCount;
        public string FoundDevicesCount
        {
            get { return _foundDevicesCount; }
            set
            {
                _foundDevicesCount = value;
                RaisePropertyChanged(() => FoundDevicesCount);
            }
        }

        /// <summary>
        /// The progress completed.
        /// </summary>
        float _progressCompleted;

        public float ProgressCompleted
        {
            get { return _progressCompleted; }
            set
            {
                _progressCompleted = value;
                ProgressCompletedIOS = ProgressMax - 0 < 0.1 ? 0 : value / ProgressMax;
                //Debug.WriteLine("ProgressCompleted - " + _progressCompleted);
                RaisePropertyChanged(() => ProgressCompleted);
            }
        }

        /// <summary>
        /// The progress completed at ios end.
        /// </summary>
        float _progressCompletedIOS = 0;
        public float ProgressCompletedIOS
        {
            get { return _progressCompletedIOS; }
            set
            {
                _progressCompletedIOS = value;
                Debug.WriteLine("ProgressCompleted - " + _progressCompleted + "  " + _progressCompletedIOS + "  " + ProgressMax);
                RaisePropertyChanged(() => ProgressCompletedIOS);
            }
        }

        float _progressMax;
        public float ProgressMax

        {
            get { return _progressMax; }
            set
            {
                _progressMax = value;
                RaisePropertyChanged(() => ProgressMax);
            }
        }

        ObservableCollection<string> _BATTViewNames;
        public ObservableCollection<string> BATTViewNames
        {
            get { return _BATTViewNames; }
            set
            {
                _BATTViewNames = value;
                RaisePropertyChanged(() => BATTViewNames);
            }
        }

        ObservableCollection<DeviceInfoItem> _ListItemSource;
        public ObservableCollection<DeviceInfoItem> ListItemSource
        {
            get { return _ListItemSource; }
            set
            {
                _ListItemSource = value;
                RaisePropertyChanged(() => ListItemSource);
            }
        }

        ObservableCollection<DeviceInfoItem> _battViewItemSource;
        public ObservableCollection<DeviceInfoItem> BattViewItemSource
        {
            get { return _battViewItemSource; }
            set
            {
                _battViewItemSource = value;
                RaisePropertyChanged(() => BattViewItemSource);
            }
        }

        ObservableCollection<DeviceInfoItem> _chargerItemSource;
        public ObservableCollection<DeviceInfoItem> ChargerItemSource
        {
            get { return _chargerItemSource; }
            set
            {
                _chargerItemSource = value;
                RaisePropertyChanged(() => ChargerItemSource);
            }
        }

        ObservableCollection<DeviceInfoItem> _replacementItemSource;
        public ObservableCollection<DeviceInfoItem> ReplacementItemSource
        {
            get { return _replacementItemSource; }
            set
            {
                _replacementItemSource = value;
                RaisePropertyChanged(() => ReplacementItemSource);
            }
        }


        public ConnectToDeviceViewModel()
        {
            ChargerItemSource = new ObservableCollection<DeviceInfoItem>();
            ReplacementItemSource = new ObservableCollection<DeviceInfoItem>();
            BattViewItemSource = new ObservableCollection<DeviceInfoItem>();
            ListItemSource = new ObservableCollection<DeviceInfoItem>();
            ViewTitle = AppResources.connect_to_device;
            //Initialise the IsBattview to true
            //IsBATTView = true;
            //Todo: Consideringg this wil not create any problem, as isUSBBattview is always true except when usb and charger. needs further testing  on this
            IsBattView = ACConstants.IsUSBBattView;
            FoundDevicesCount = "0";
            ProgressCompleted = 0;
            ProgressMax = 0;
            ProgressCompletedIOS = 0;
            Chargerscount = 0;
            BattViewCount = 0;
            ReplacementCount = 0;
            SelectedTabIndex = 0;
            ScanStatusMessage = AppResources.scanning_for_devices;
            ScanBtnText = AppResources.scan_again;
            //Initialise the ConnectionManager
            connManager = new ConnectionManager();
            SiteViewQuantum.Instance.SetConnectionManager(connManager);

            ResetSubscriptionToken = Mvx.Resolve<IMvxMessenger>().Subscribe<DeviceResetMessage>(OnDeviceResetMessage);
            ActionMenuSubscriptionToken = Mvx.Resolve<IMvxMessenger>().Subscribe<ActionsMenuMessage>(OnActionMenuMessage);
            StartScanSubsciptionToken = Mvx.Resolve<IMvxMessenger>().Subscribe<StartScanMessage>(OnStartScanMessage);

            Task.Run(() =>
            {
                PingDevices();
            });

        }


        async void PingDevices()
        {
            await Task.Delay(1000);

            if (ACConstants.ConnectionType != ConnectionTypesEnum.ROUTER)
                return;

            connManager.MyCommunicator.Router.InitialiseScanningToken();

            dbDevices =
                DbSingleton
                    .DBManagerServiceInstance
                    .GetDBConnectedDevicesLoader()
                    .GetAll();

            if (dbDevices != null && dbDevices.Count > 0)
            {
                ACUserDialogs.ShowProgress();

                await connManager.MyCommunicator.Router.DoScan(dbDevices);
                await connManager.refresh_timer_prepare();

                if (IsReplacement)
                    ListItemSource = ReplacementItemSource;
                else if (IsBattView)
                    ListItemSource = BattViewItemSource;
                else
                    ListItemSource = ChargerItemSource;

                ACUserDialogs.HideProgress();
            }

            await Scan();
        }

        public void OnStartScanMessage(StartScanMessage obj)
        {
            try
            {
                if (StartScanSubsciptionToken != null)
                {
                    Mvx.Resolve<IMvxMessenger>().Unsubscribe<StartScanMessage>(StartScanSubsciptionToken);
                    StartScanSubsciptionToken = null;
                    ScantBtnClicked.Execute();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

        }

        void OnActionMenuMessage(ActionsMenuMessage obj)
        {
            switch (obj.ActionsMenuType)
            {
                case ACUtility.ActionsMenuType.Download:
                    DownloadSiteData.Execute();

                    break;

                case ACUtility.ActionsMenuType.Upload:
                    UploadData.Execute();

                    break;

                case ACUtility.ActionsMenuType.SyncSites:
                    SyncSites.Execute();

                    break;

                case ACUtility.ActionsMenuType.SiteView:
                    SiteView.Execute();

                    break;

                case ACUtility.ActionsMenuType.Update:
                    UpdateSiteFirmware.Execute();

                    break;
            }
            if (backScanActionSubsciptionToken == null)
                backScanActionSubsciptionToken = Mvx.Resolve<IMvxMessenger>().Subscribe<BackScanMessage>(OnBackScanMessage);
        }

        async void OnDeviceResetMessage(DeviceResetMessage obj)
        {
            try
            {
                await TryOnDeviceResetMessage(obj);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        async Task TryOnDeviceResetMessage(DeviceResetMessage obj)
        {
            ACUserDialogs.HideProgress();

            var removeItem = dbDevices.FirstOrDefault(o => o.IPAddress == obj.CurrentConnectedDeviceID);
            if (removeItem != null)
            {
                dbDevices.Remove(removeItem);
            }
            DeviceHomeViewModel.ClearPingDeviceTimer();
            connManager.MyCommunicator.RemoveIP(obj.CurrentConnectedDeviceID);
            InvokeOnMainThread(() =>
            {
                if (SelectedTabIndex == 0)
                {
                    for (int i = 0; i < BattViewItemSource.Count; i++)
                    {
                        if (BattViewItemSource[i].IPAddress == obj.CurrentConnectedDeviceID)
                        {
                            BattViewItemSource.RemoveAt(i);
                        }
                    }
                    BattViewCount = BattViewItemSource.Count;

                    RaisePropertyChanged(() => BattViewItemSource);
                    RaisePropertyChanged(() => BattViewCount);
                }
                else if (SelectedTabIndex == 1)
                {
                    for (int i = 0; i < ChargerItemSource.Count; i++)
                    {
                        if (ChargerItemSource[i].IPAddress == obj.CurrentConnectedDeviceID)
                        {
                            ChargerItemSource.RemoveAt(i);
                        }
                    }

                    Chargerscount = ChargerItemSource.Count;
                    RaisePropertyChanged(() => ChargerItemSource);
                    RaisePropertyChanged(() => Chargerscount);
                }
                else if (SelectedTabIndex == 2)
                {
                    for (int i = 0; i < ReplacementItemSource.Count; i++)
                    {
                        if (ReplacementItemSource[i].IPAddress == obj.CurrentConnectedDeviceID)
                        {
                            ReplacementItemSource.RemoveAt(i);
                        }
                    }

                    ReplacementCount = ReplacementItemSource.Count;
                    RaisePropertyChanged(() => ReplacementItemSource);
                    RaisePropertyChanged(() => ReplacementCount);
                }

                if (IsReplacement)
                {
                    ListItemSource = ReplacementItemSource;
                }
                else if (IsBattView)
                {
                    ListItemSource = BattViewItemSource;
                }
                else
                {
                    ListItemSource = ChargerItemSource;
                }

                RaisePropertyChanged(() => ListItemSource);
            });

            Stop();

            await Task.Delay(3000);

            PingDevices();
        }

        public IMvxCommand DownloadSiteData
        {
            get
            {
                return new MvxCommand(ExecuteDownloadSiteDataCommand);
            }
        }

        void ExecuteDownloadSiteDataCommand()
        {
            if (NetworkCheck())
            {
                GoToSiteViewWithoutSychSite();
            }
        }

        public IMvxCommand UploadData
        {
            get
            {
                return new MvxCommand(async () =>
                {
                    await ExecuteUploadDataCommand();
                });
            }
        }

        async Task ExecuteUploadDataCommand()
        {
            if (NetworkCheck())
            {
                ACUserDialogs.ShowProgress();
                bool isReachable = await InternetConnectivityManager.IsReachableAsync();
                ACUserDialogs.HideProgress();
                if (isReachable)
                {
                    Stop();
                    ShowViewModel<UploadViewModel>();
                }
                else
                {
                    ACUserDialogs.ShowAlert(AppResources.no_internet_connection);
                }
            }
        }

        public IMvxCommand SyncSites
        {
            get
            {
                return new MvxCommand(ExecuteSyncSitesCommand);
            }
        }

        void ExecuteSyncSitesCommand()
        {
            Stop();
            ShowViewModel<SyncSitesViewModel>();
        }

        public IMvxCommand SiteView
        {
            get
            {
                return new MvxCommand(ExecuteSiteViewCommand);
            }
        }

        void ExecuteSiteViewCommand()
        {
            ShowViewModel<SiteViewSitesViewModel>();
        }

        public IMvxCommand UpdateSiteFirmware
        {
            get
            {
                return new MvxCommand(ExecuteUpdateSiteFirmwareCommand);
            }
        }

        void ExecuteUpdateSiteFirmwareCommand()
        {
            if (NetworkCheck())
            {
                GoToSiteViewWithoutSychSite();
            }
        }

        void GoToSiteViewWithoutSychSite()
        {
            ShowViewModel<SiteViewDevicesViewModel>(
                new
                {
                    siteId = 0,
                    isWithSynchSites = false
                });
        }

        public void Stop()
        {
            connManager.MyCommunicator.StopScan();

            InitialiseProgressBar(0, 0);

            ScanBtnVisibility = true;

            RaisePropertyChanged(() => ScanBtnVisibility);

            if (progressSubscriptionToken != null)
            {
                Mvx.Resolve<IMvxMessenger>().Unsubscribe<ProgressChangeMessage>(progressSubscriptionToken);
                progressSubscriptionToken = null;
            }
        }

        async Task Scan()
        {
            try
            {
                //Subscribe to the Progress Change
                progressSubscriptionToken = Mvx.Resolve<IMvxMessenger>().Subscribe<ProgressChangeMessage>(OnProgressChangeMessage);

                addDeviceSubscriptionToken = Mvx.Resolve<IMvxMessenger>().Subscribe<AddDeviceMessage>(OnAddDeviceMessageReceive);

                //Collapse the Scan Again Button
                ScanBtnVisibility = false;
                //Assign Scanning for Devices Status Text
                RaisePropertyChanged(() => ScanBtnVisibility);
                ScanStatusMessage = AppResources.scanning_for_devices;
                RaisePropertyChanged(() => ScanStatusMessage);

                await connManager.MyCommunicator.Router.DoScan(null);

                if (addDeviceSubscriptionToken != null)
                {
                    Mvx.Resolve<IMvxMessenger>().Unsubscribe<AddDeviceMessage>(addDeviceSubscriptionToken);
                    addDeviceSubscriptionToken = null;
                }
                //Reset the Progress variables in Router Class.
                connManager.MyCommunicator.Router.ResetProgress();
                //Reset the Progress variables in this Class.
                InitialiseProgressBar(0, 0);
                //Make the Scan Again Button visible
                ScanBtnVisibility = true;
                RaisePropertyChanged(() => ScanBtnVisibility);
                //Assign Empty Status Text
                //ScanStatusMessage = string.Empty;
                //UnSubscribe to the Progress Change
                if (progressSubscriptionToken != null)
                {
                    Mvx.Resolve<IMvxMessenger>().Unsubscribe<ProgressChangeMessage>(progressSubscriptionToken);
                    progressSubscriptionToken = null;
                }
                if (IsReplacement)
                {
                    ListItemSource = ReplacementItemSource;
                }
                else if (IsBattView)
                {
                    ListItemSource = BattViewItemSource;
                }
                else
                {
                    ListItemSource = ChargerItemSource;
                }
                if (StartScanSubsciptionToken == null)
                    StartScanSubsciptionToken = Mvx.Resolve<IMvxMessenger>().Subscribe<StartScanMessage>(OnStartScanMessage);

                if (!connManager.MyCommunicator.Router.ScanningCancellationToken.IsCancellationRequested)
                {
                    await Task.Delay(3000);

                    await Scan();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                if (addDeviceSubscriptionToken != null)
                {
                    Mvx.Resolve<IMvxMessenger>().Unsubscribe<AddDeviceMessage>(addDeviceSubscriptionToken);
                    addDeviceSubscriptionToken = null;
                }
                if (progressSubscriptionToken != null)
                {
                    Mvx.Resolve<IMvxMessenger>().Unsubscribe<ProgressChangeMessage>(progressSubscriptionToken);
                    progressSubscriptionToken = null;
                }
            }
            RaisePropertyChanged(() => ReplacementCount);
        }

        async void OnAddDeviceMessageReceive(AddDeviceMessage obj)
        {
            //Add the Scanned Devices to managedBATTViews variable
            await connManager.refresh_timer_prepare();
            // Add the Devives from managedBATTViews variable to BattViewItemSource
            DeviceRefreshTimer();

        }

        void OnProgressChangeMessage(ProgressChangeMessage obj)
        {
            InitialiseProgressBar(obj.ProgressCompleted, obj.ProgressMax);
        }

        void DeviceRefreshTimer()
        {
            bool requireUpdate = connManager.updateIfRequired();
            if (requireUpdate)
            {
                foreach (string m in connManager.managedBATTViews.getListKeys())
                {
                    //check
                    DeviceBattViewObject b = connManager.managedBATTViews.getDeviceByKey(m);
                    if (b == null)
                        continue;
                    string prettyName = BATT_MassActionsObjectsUpdate(m, b);

                    if (b.battview.Config.replacmentPart)
                    {
                        if (ReplacementItemSource.FirstOrDefault(o => o.DeviceID == m) == null)
                        {
                            lock (myLock)
                            {
                                InvokeOnMainThread(() =>
                                {
                                    DBConnectedDevices connectedDevice = GetDBDevice(b.battview.IPAddress, prettyName, true, false, true);
                                    var deviceinDB = dbDevices.FirstOrDefault(o => o.IPAddress == b.battview.IPAddress);
                                    if (deviceinDB == null)
                                    {
                                        // dbManager.Insert(connectedDevice);
                                    }
                                    else
                                    {
                                        if (deviceinDB.PrettyName != prettyName || deviceinDB.IsBattview != true || deviceinDB.IsReplacement != true)
                                        {
                                            deviceinDB.PrettyName = prettyName;

                                            DbSingleton
                                                .DBManagerServiceInstance
                                                .GetDBConnectedDevicesLoader()
                                                .InsertOrUpdate(deviceinDB);
                                        }

                                    }

                                    DeviceInfoItem deviceInfoItem = new DeviceInfoItem() { IPAddress = b.battview.IPAddress, PrettyName = prettyName, Title = "BATT Replacement", SubTitle = b.battview.Config.battViewSN, DeviceID = m, IsBATTReplacement = true };

                                    AddToList(deviceInfoItem, ReplacementItemSource, prettyName);

                                    ReplacementCount = ReplacementItemSource.Count;
                                    RaisePropertyChanged(() => ReplacementItemSource);
                                });

                            }
                        }

                    }
                    else
                    {
                        string[] DeviceInfo = prettyName.Split(':');
                        if (DeviceInfo.Count() > 1)
                        {
                            if (BattViewItemSource.FirstOrDefault(o => o.DeviceID == m) == null)
                            {
                                lock (myLock)
                                {
                                    InvokeOnMainThread(() =>
                                    {
                                        DBConnectedDevices connectedDevice = GetDBDevice(b.battview.IPAddress, prettyName, true, false, false);

                                        var deviceinDB = dbDevices.FirstOrDefault(o => o.IPAddress == b.battview.IPAddress);
                                        if (deviceinDB == null)
                                        {
                                            // dbManager.Insert(connectedDevice);
                                        }
                                        else
                                        {
                                            if (deviceinDB.PrettyName != prettyName || deviceinDB.IsBattview != true || deviceinDB.IsReplacement != false)
                                            {
                                                deviceinDB.PrettyName = prettyName;

                                                DbSingleton
                                                    .DBManagerServiceInstance
                                                    .GetDBConnectedDevicesLoader()
                                                    .InsertOrUpdate(deviceinDB);
                                            }
                                        }

                                        DeviceInfoItem deviceInfoItem = new DeviceInfoItem() { IPAddress = b.battview.IPAddress, PrettyName = prettyName, Title = DeviceInfo[0] + " - " + b.battview.Config.id, SubTitle = DeviceInfo[1], DeviceID = m };

                                        AddToList(deviceInfoItem, BattViewItemSource, prettyName);

                                        BattViewCount = BattViewItemSource.Count;
                                        RaisePropertyChanged(() => BattViewCount);
                                    });

                                }
                            }
                        }
                    }
                }

                foreach (string m in connManager.managedMCBs.getListKeys())
                {
                    //check
                    DeviceMCBObject b = connManager.managedMCBs.getDeviceByKey(m);
                    if (b == null)
                        continue;
                    string prettyName = MCB_MassActionsObjectsUpdate(m, b);
                    if (b.mcb.Config.replacmentPart)
                    {
                        if (ReplacementItemSource.FirstOrDefault(o => o.DeviceID == m) == null)
                        {
                            lock (myLock)
                            {
                                InvokeOnMainThread(() =>
                                {
                                    DBConnectedDevices connectedDevice = GetDBDevice(b.mcb.IPAddress, prettyName, false, false, true);
                                    var deviceinDB = dbDevices.FirstOrDefault(o => o.IPAddress == b.mcb.IPAddress);
                                    if (deviceinDB == null)
                                    {
                                        // dbManager.Insert(connectedDevice);
                                    }
                                    else
                                    {
                                        if (deviceinDB.PrettyName != prettyName || deviceinDB.IsBattview != false || deviceinDB.IsReplacement != true)
                                        {
                                            deviceinDB.PrettyName = prettyName;

                                            DbSingleton
                                                .DBManagerServiceInstance
                                                .GetDBConnectedDevicesLoader()
                                                .InsertOrUpdate(deviceinDB);
                                        }

                                    }

                                    DeviceInfoItem deviceInfoItem = new DeviceInfoItem() { IPAddress = b.mcb.IPAddress, PrettyName = prettyName, Title = "MCB Replacement", SubTitle = prettyName, DeviceID = m, IsBATTReplacement = false };

                                    AddToList(deviceInfoItem, ReplacementItemSource, prettyName);

                                    ReplacementCount = ReplacementItemSource.Count;
                                    RaisePropertyChanged(() => ReplacementItemSource);
                                });

                            }
                        }
                    }
                    else
                    {
                        string[] DeviceInfo = prettyName.Split(':');
                        if (DeviceInfo.Count() > 1)
                        {
                            if (ChargerItemSource.FirstOrDefault(o => o.DeviceID == m) == null)
                            {
                                lock (myLock)
                                {
                                    InvokeOnMainThread(() =>
                                    {
                                        DBConnectedDevices connectedDevice = GetDBDevice(b.mcb.IPAddress, prettyName, false, false, false);
                                        var deviceinDB = dbDevices.FirstOrDefault(o => o.IPAddress == b.mcb.IPAddress);
                                        if (deviceinDB == null)
                                        {
                                            // dbManager.Insert(connectedDevice);
                                        }
                                        else
                                        {
                                            if (deviceinDB.PrettyName != prettyName || deviceinDB.IsBattview != false || deviceinDB.IsReplacement != false)
                                            {
                                                deviceinDB.PrettyName = prettyName;

                                                DbSingleton
                                                    .DBManagerServiceInstance
                                                    .GetDBConnectedDevicesLoader()
                                                    .InsertOrUpdate(deviceinDB);
                                            }
                                        }

                                        DeviceInfoItem deviceInfoItem = new DeviceInfoItem() { IPAddress = b.mcb.IPAddress, PrettyName = prettyName, Title = DeviceInfo[0] + " - " + b.mcb.Config.id, SubTitle = DeviceInfo[1], DeviceID = m };

                                        AddToList(deviceInfoItem, ChargerItemSource, prettyName);

                                        Chargerscount = ChargerItemSource.Count;
                                        RaisePropertyChanged(() => Chargerscount);
                                    });

                                }
                            }
                        }
                    }
                }
            }
        }

        void AddToList
        (DeviceInfoItem deviceInfoItem, ObservableCollection<DeviceInfoItem> itemSource, string prettyName)
        {
            bool found = itemSource.Any(arg => arg.DeviceID == deviceInfoItem.DeviceID);

            if (!found)
            {
                itemSource.Add(deviceInfoItem);

                Debug.WriteLine("New Devices Added" + prettyName);
            }
        }

        DBConnectedDevices GetDBDevice(string IP, string prettyName, bool isbattview, bool isconnected, bool isreplacement)
        {
            var connectedDevice = new DBConnectedDevices();
            connectedDevice.IPAddress = IP;
            connectedDevice.IsBattview = isbattview;
            connectedDevice.IsConnected = isconnected;
            connectedDevice.PrettyName = prettyName;
            connectedDevice.IsReplacement = isreplacement;
            return connectedDevice;
        }



        /// <summary>
        /// To retreive the Name of the BattView
        /// </summary>
        /// <returns>The mass actions objects update.</returns>
        /// <param name="keyString">Key string.</param>
        /// <param name="b">The blue component.</param>
        string BATT_MassActionsObjectsUpdate(string keyString, DeviceBattViewObject b)
        {
            string prettyNamex;
            UInt32 eventsCount = 0;
            bool actViewEnabled = true;
            UInt32 deviceID = b.battview.Config.id;
            if (b.battview.deviceIsLoaded)
            {
                eventsCount = b.battview.globalRecord.eventsCount;
                actViewEnabled = b.battview.Config.ActViewEnabled;
            }
            if (!b.battview.Config.replacmentPart)
            {
                prettyNamex = b.battview.Config.batteryID + "(" + b.battview.Config.battViewSN + ")";
            }
            else
            {
                prettyNamex = "BATTView Replacement Part " + "(" + b.battview.Config.battViewSN + ")";
            }


            //if (ControlObject.user_access.Batt_FirmwareUpdate == access_level.write)
            //{
            //  firmwareUpdateCtrl.addDevice(deviceID, b.battview.firmwareRevision, false, prettyNamex, keyString);
            //  firmwareUpdateCtrl.updatePrettyName(deviceID, false, prettyNamex);
            //}
            if (!b.battview.Config.replacmentPart)
            {
                //TODO implement downloading and uncomment the below line
                bool isNew = ACConstants.all_downloadStat.addIfNotExist(false, prettyNamex, deviceID, b.startSynchID, eventsCount, b.startSynchID, actViewEnabled, b.battview.Config.studyId, 1, 1, 1);
                if (!isNew && b.battview.deviceIsLoaded)
                {
                    ACConstants.all_downloadStat.refreshVars(false, actViewEnabled, prettyNamex, b.startSynchID, deviceID, b.battview.Config.studyId, 0);
                    ACConstants.all_downloadStat.setMaxIDSeen(false, deviceID, eventsCount, b.battview.Config.studyId);
                }

                if (b.battview.deviceIsLoaded)
                {
                    if (!actViewEnabled && deviceID >= 10000 && !b.battview.Config.replacmentPart && ControlObject.AccessBattView && !ControlObject.isHWMnafacturer && !ControlObject.isACTOem)
                    {
                        if (isNew && ControlObject.isDebugMaster)
                            Logger.AddLog(false, "Start downlaoding from " + b.battview.Config.batteryID + "(" + b.battview.Config.battViewSN + ")" + ":" + b.battview.Config.id);

                        DbSingleton.DBManagerServiceInstance.GetDevicesObjectsLoader()
                                   .InsertOrUpdateDevice
                                   (false, b.battview.Config.id, b.battview.Config.ToJson(),
                             b.battview.globalRecord.ToJson(),
                             b.battview.Config.memorySignature,
                             b.battview.globalRecord.eventsCount,
                             b.battview.FirmwareRevision, b.battview.myZone,
                             b.battview.Config.studyId);
                    }

                    //get start ID
                    b.configAndglobalSaved = true;
                    ACConstants.all_downloadStat.setconfigLoaded(false, deviceID, b.battview.Config.studyId);
                }
            }


            string prettyName = "";
            if (!b.battview.Config.replacmentPart)
            {
                prettyName = "Battery:" + b.battview.Config.batteryID.Trim(new char[] { '\0' }) + " (" + b.battview.Config.battViewSN.Trim(new char[] { '\0' }) + ")";
            }
            else
            {
                prettyName = "BATTView Replacement Part " + " (" + b.battview.Config.battViewSN.Trim(new char[] { '\0' }) + ")";

            }
            return prettyName;

        }


        string MCB_MassActionsObjectsUpdate(string keyString, DeviceMCBObject b)
        {
            string prettyNamex;
            UInt32 chargeCycles = 0;
            bool actViewEnabled = true;
            bool replacment = false;
            UInt32 pmFaults = 0;
            if (b.mcb.deviceIsLoaded)
            {
                chargeCycles = b.mcb.globalRecord.chargeCycles;
                actViewEnabled = b.mcb.Config.actViewEnable;
                pmFaults = b.mcb.globalRecord.PMfaults;
                replacment = b.mcb.Config.replacmentPart;
            }

            UInt32 deviceID = UInt32.Parse(b.mcb.Config.id);
            if (b.DeviceType == DeviceBaseType.CALIBRATOR)
            {
                prettyNamex = "Calibrator:" + b.mcb.Config.serialNumber.Trim(new char[] { '\0' });
            }
            else if (!b.mcb.Config.replacmentPart)
            {
                prettyNamex = b.mcb.Config.chargerUserName.Trim(new char[] { '\0' }) + " (" + b.mcb.Config.serialNumber.Trim(new char[] { '\0' }) + ") ";
            }
            else
            {
                prettyNamex = "MCB Replacement Part " + " (" + b.mcb.Config.serialNumber.Trim(new char[] { '\0' }) + ") ";
            }

            //if (ControlObject.user_access.MCB_FirmwareUpdate == access_level.write)
            //{
            //    firmwareUpdateCtrl.addDevice(deviceID, b.mcb.firmwareRevision, true, prettyNamex, keyString);
            //    firmwareUpdateCtrl.updatePrettyName(deviceID, true, prettyNamex);
            //}
            if (!b.mcb.Config.replacmentPart && b.mcb.DeviceType == DeviceBaseType.MCB)
            {

                bool isNew = ACConstants.all_downloadStat.addIfNotExist(true, prettyNamex, deviceID, b.startSynchID, chargeCycles, b.startSynchID, actViewEnabled, 0, b.StartPMSynchID, pmFaults, b.StartPMSynchID);

                if (!isNew && b.mcb.deviceIsLoaded)
                {
                    ACConstants.all_downloadStat.refreshVars(true, actViewEnabled, prettyNamex, b.startSynchID, deviceID, 0, b.StartPMSynchID);
                    ACConstants.all_downloadStat.setMaxIDSeen(true, deviceID, chargeCycles, 0);
                    ACConstants.all_downloadStat.setMaxPMIDSeen(true, deviceID, pmFaults, 0);
                }

                if (b.mcb.deviceIsLoaded)
                {
                    if (!actViewEnabled && deviceID >= 10000 && ControlObject.AccessMCB && !ControlObject.isHWMnafacturer && !ControlObject.isACTOem)
                    {
                        if (isNew && ControlObject.isDebugMaster)
                            Logger.AddLog(false, "Start downlaoding from MCB" + "Charger:" + b.mcb.Config.chargerUserName.Trim(new char[] { '\0' }) + " (" + b.mcb.Config.serialNumber.Trim(new char[] { '\0' }) + ") ");
                        DbSingleton.DBManagerServiceInstance.GetDevicesObjectsLoader()
                                   .InsertOrUpdateDevice
                                   (true, UInt32.Parse(b.mcb.Config.id),
                             b.mcb.Config.ToJson(), b.mcb.globalRecord.TOJSON(),
                             int.Parse(b.mcb.Config.memorySignature),
                             b.mcb.globalRecord.totalChargeSeconds,
                             b.mcb.FirmwareRevision, b.mcb.myZone, 0);

                    }
                    //get start ID
                    b.configAndglobalSaved = true;
                    ACConstants.all_downloadStat.setconfigLoaded(true, UInt32.Parse(b.mcb.Config.id), 0);
                }
            }
            string prettyName = "";
            if (b.DeviceType == DeviceBaseType.CALIBRATOR)
            {
                prettyName = "Calibrator:" + b.mcb.Config.serialNumber.Trim(new char[] { '\0' });
            }
            else if (!b.mcb.Config.replacmentPart)
            {
                prettyName = "Charger:" + b.mcb.Config.chargerUserName.Trim(new char[] { '\0' }) + " (" + b.mcb.Config.serialNumber.Trim(new char[] { '\0' }) + ")";
            }
            else
            {
                prettyName = "MCB Replacement Part " + " (" + b.mcb.Config.serialNumber.Trim(new char[] { '\0' }) + ")";

            }

            return prettyName;
        }

        public IMvxCommand ScantBtnClicked
        {
            get
            {
                return new MvxCommand(async () =>
                {
                    await OnScantBtnClicked();
                });
            }
        }

        async Task OnScantBtnClicked()
        {
            try
            {
                connManager.MyCommunicator.Router.InitialiseScanningToken();
                if (!NetworkCheck(true))
                {
                    if (StartScanSubsciptionToken == null)
                        StartScanSubsciptionToken = Mvx.Resolve<IMvxMessenger>().Subscribe<StartScanMessage>(OnStartScanMessage);
                    return;
                }
                await connManager.MyCommunicator.Router.DoScan(dbDevices);

                if (IsReplacement)
                {
                    ListItemSource = ReplacementItemSource;
                }
                else if (IsBattView)
                {
                    ListItemSource = BattViewItemSource;
                }
                else
                {
                    ListItemSource = ChargerItemSource;
                }

                //await Scan();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Initialises the progress bar.
        /// </summary>
        /// <param name="progressCompleted">Progress completed.</param>
        /// <param name="progressMax">Progress max.</param>
        void InitialiseProgressBar(int progressCompleted, int progressMax)
        {
            if (Convert.ToInt16(ProgressMax) != progressMax)
            {
                ProgressMax = progressMax;
            }

            if (Convert.ToInt16(ProgressCompleted) != progressCompleted)
            {
                ProgressCompleted = progressCompleted;
            }
        }

        MvxCommand<DeviceInfoItem> m_SelectItemCommand;

        /// <summary>
        /// Gets the select item command.
        /// </summary>
        /// <value>The select item command.</value>
        public ICommand SelectItemCommand
        {
            get
            {
                return this.m_SelectItemCommand ?? (this.m_SelectItemCommand = new MvxCommand<DeviceInfoItem>(async (obj) =>
                {
                    await this.ExecuteSelectItemCommand(obj);
                }));
            }
        }



        /// <summary>
        /// Executes the select item command.
        /// </summary>
        /// <param name="item">Item.</param>
        async Task ExecuteSelectItemCommand(DeviceInfoItem item)
        {
            if (!NetworkCheck())
                return;

            if (!ACConstants.IS_STATIC_DATA_REQUIRED)
            {
                if (IsReplacement)
                {
                    ACUserDialogs.ShowProgress();

                    if (item.IsBATTReplacement)
                    {
                        await CreateConnectionForBATTView(item);
                    }
                    else
                    {
                        await CreateConnectionForMCB(item);
                    }
                }
                else if (IsBattView)
                {
                    if (BattViewQuantum.Instance.GetBATTView() == null)
                    {
                        ACUserDialogs.ShowProgress();

                        await CreateConnectionForBATTView(item);
                    }
                    else
                    {
                        BattViewObject battView = BattViewQuantum.Instance.GetBATTView();
                        //  Update connectionmanager working serial number here
                        BattViewQuantum.Instance.GetConnectionManager().workingSerialNumber = item.DeviceID;
                        if (battView.SerialNumber == item.DeviceID)
                        {
                            item.IsConnected = true;

                            Stop();

                            if (backScanActionSubsciptionToken == null)
                                backScanActionSubsciptionToken = Mvx.Resolve<IMvxMessenger>().Subscribe<BackScanMessage>(OnBackScanMessage);

                            ShowViewModel<DeviceHomeViewModel>(new { title = item.PrettyName, isForBattViewDetails = true });
                        }
                        else
                        {
                            ACUserDialogs.ShowProgress();
                            await CreateConnectionForBATTView(item);
                        }
                    }
                }
                else
                {
                    if (MCBQuantum.Instance.GetMCB() == null)
                    {
                        ACUserDialogs.ShowProgress();

                        await CreateConnectionForMCB(item);
                    }
                    else
                    {
                        MCBobject MCB = MCBQuantum.Instance.GetMCB();
                        //  Update connectionmanager working serial number here
                        MCBQuantum.Instance.GetConnectionManager().workingSerialNumber = item.DeviceID;

                        var idStr = item.DeviceID.Split(':');
                        if (idStr != null && idStr.Count() > 1 && idStr[1] == MCB.Config.serialNumber)
                        {
                            item.IsConnected = true;

                            Stop();

                            if (backScanActionSubsciptionToken == null)
                                backScanActionSubsciptionToken = Mvx.Resolve<IMvxMessenger>().Subscribe<BackScanMessage>(OnBackScanMessage);

                            ShowViewModel<DeviceHomeViewModel>(new { title = item.PrettyName });
                        }
                        else
                        {
                            ACUserDialogs.ShowProgress();
                            await CreateConnectionForMCB(item);
                        }
                    }
                }
            }
            else
            {
                // DEMOData
                connManager = new ConnectionManager();
                connManager.workingSerialNumber = "BATT_879:3EngSample1";
                connManager.isConnecting = false;
                connManager.activeBattView = JsonConvert.DeserializeObject<BattViewObject>("{\"battViewAdcRawObject\":{\"plc_state\":0,\"plc_status\":0,\"wifi_state\":0,\"wifi_status\":0,\"rtc_state\":0,\"austria_state\":0,\"clampValue\":0,\"intercellNTC\":0.0,\"electrolye\":0,\"current\":0.0,\"clampValueRef\":0,\"clampValueChannel2\":0,\"voltage\":0.0,\"internalNTC\":0.0,\"ntcRefrence_the10K\":0.0,\"ntcBattery\":0.0,\"intercellNTCFiltered\":0.0,\"currentFiltered\":0.0,\"voltageFiltered\":0.0,\"internalNTCFiltered\":0.0,\"ntcBatteryRefFiltered\":0.0,\"ntcBatteryFiltered\":0.0,\"clampValueFiltered\":0,\"electrolyeFiltered\":0,\"clampValueReftFiltered\":0,\"clampValueChannel2tFiltered\":0,\"hallEffectEnabled\":false},\"quickView\":{\"event_AS\":0,\"event_WS\":0,\"voltage\":11.99021,\"current\":0.0,\"temperature\":25.0,\"soc\":0,\"event_type\":2,\"startTime\":\"2017-01-22T06:19:07Z\",\"duration\":\"06:00:14\",\"EL_enabled\":false,\"waterOK\":false,\"mainSenseErrorCode\":0},\"config\":{\"studyName\":\"\",\"TruckId\":\"\",\"setup\":170,\"battviewVersion\":1,\"memorySignature\":36,\"lastChangeTime\":\"2017-01-18T03:03:06\",\"lastChangeUserID\":351,\"installationDate\":\"2016-01-08T19:32:03\",\"warrantedAHR\":1062500,\"cvMaxDuration\":14400,\"EQstartWindow\":0,\"EQcloseWindow\":43200,\"FIstartWindow\":0,\"FIcloseWindow\":43200,\"FIduration\":7200,\"EQduration\":7200,\"id\":879,\"desulfation\":43200,\"tempFa\":0.001138722,\"tempFb\":0.000232569,\"tempFc\":9.375905E-08,\"intercellCoefficient\":0.004,\"voltageCalA\":0.4182334,\"voltageCalB\":-35.85492,\"NTCcalA\":52302.14,\"NTCcalB\":-41598.6,\"currentCalA\":-1.399152,\"currentCalB\":8.474291,\"intercellTemperatureCALa\":52302.14,\"intercellTemperatureCALb\":-41598.6,\"chargeToIdleTimer\":300,\"chargeToInUseTimer\":5,\"inUseToChargeTimer\":10,\"inUsetoIdleTimer\":600,\"idleToChargeTimer\":10,\"idleToInUseTimer\":10,\"electrolyteHLT\":120,\"electrolyteLHT\":120,\"actViewConnectFrequency\":900,\"ahrcapacity\":850,\"batteryHighTemperature\":550,\"trickleVoltage\":195,\"CVTargetVoltage\":240,\"trickleCurrentRate\":300,\"CCrate\":1700,\"CVendCurrentRate\":25,\"CVcurrentStep\":0,\"FItargetVoltage\":260,\"FIcurrentRate\":500,\"EQvoltage\":265,\"EQcurrentRate\":400,\"autoLogTime\":60,\"mobilePort\":50000,\"actViewPort\":9309,\"currentIdleToCharge\":110,\"currentIdleToInUse\":-110,\"currentChargeToIdle\":90,\"currentChargeToInUse\":-110,\"currentInUseToCharge\":110,\"currentInUseToIdle\":-90,\"battViewSN\":\"3EngSample1\",\"batteryID\":\"battery1\",\"softAPpassword\":\"actDirmank\",\"actViewIP\":\"act-view.com\",\"HWversion\":\"C \",\"actAccessSSID\":\"actAccess24\",\"mobileAccessSSID\":\"act24mobile\",\"actAccessSSIDpassword\":\"hala3ami102\",\"mobileAccessSSIDpassword\":\"shlonak5al\",\"isPA\":true,\"actViewEnabled\":true,\"softAPEnable\":true,\"enableElectrolyeSensing\":false,\"enableHallEffectSensing\":true,\"enableExtTempSensing\":true,\"nominalvoltage\":36,\"batteryType\":0,\"batteryTemperatureCompesnation\":50,\"EQdaysMask\":1,\"FIdaysMask\":127,\"FIdv\":5,\"FIdt\":25,\"eventDetectVoltagePercentage\":2,\"eventDetectCurrentRangePercentage\":20,\"eventDetectTimeRangePercentage\":67,\"enablePLC\":true,\"enableDayLightSaving\":true,\"temperatureFormat\":1,\"chargerType\":0,\"TRTemperature\":150,\"foldTemperature\":517,\"coolDownTemperature\":461,\"studyId\":0,\"currentClampCalA\":0.0,\"currentClampCalB\":0.0,\"currentClamp2CalA\":0.0,\"currentClamp2CalB\":0.0,\"batterymodel\":\"\",\"batterysn\":\"\",\"batteryManfacturingDate\":\"1970-01-01T00:00:00\",\"replacmentPart\":false,\"temperatureControl\":0},\"globalRecord\":{\"signature\":42662,\"seq\":86576,\"eventsCount\":122,\"RTrecordsCount\":86134,\"inUseAHR\":0,\"inUseKWHR\":0,\"chargeAHR\":431,\"chargeKWHR\":5,\"inUseSeconds\":0,\"chargeSeconds\":0,\"idleSeconds\":5015949,\"leftoverinuseas\":0,\"leftoverinusews\":0,\"leftoverchargeas\":759,\"leftoverchargews\":1043441,\"debugCount\":318,\"endSignature\":74,\"lastfirmwareversion0\":2.02,\"lastfirmwareversion1\":0.0},\"debugRecordsStartID\":1,\"debugRecordsLastID\":317,\"debugRecordsStartIDTime\":\"2016-02-07T23:32:21\",\"debugRecordsLastIDTime\":\"2017-01-22T06:19:05\",\"eventsRecordsStartID\":1,\"eventsRecordsLastID\":121,\"eventsRecordsStartIDTime\":\"2016-02-07T19:20:27\",\"eventsRecordsLastIDTime\":\"2017-01-19T00:00:01\",\"realtimeRecordsStartID\":1,\"realtimeRecordsLastID\":86133,\"realtimeRecordsStartIDTime\":\"2016-02-07T23:33:23\",\"realtimeRecordsLastIDTime\":\"2017-01-22T04:19:07\",\"timeLost\":false,\"doLoadErrorCode\":8,\"myZone\":15,\"firmwareRevision\":2.02,\"deviceIsLoaded\":true");
                connManager.selectDevice(item.DeviceID);
                foreach (var BattView in BattViewItemSource)
                {
                    if (BattView.DeviceID != item.DeviceID)
                    {
                        if (BattView.IsConnected)
                        {
                            BattView.IsConnected = false;
                        }
                    }
                    else
                    {

                        item.IsConnected = true;
                    }
                }
                BattViewQuantum.Instance.SetConnectionManager(connManager);
                connManager.MyCommunicator.Close();
                if (backScanActionSubsciptionToken == null)
                    backScanActionSubsciptionToken = Mvx.Resolve<IMvxMessenger>().Subscribe<BackScanMessage>(OnBackScanMessage);

                ShowViewModel<DeviceHomeViewModel>(new { title = item.PrettyName, isForBattViewDetails = true });

            }
        }

        async void OnBackScanMessage(BackScanMessage obj)
        {
            //UnSubscribe to the Back Scan Click
            if (backScanActionSubsciptionToken != null)
            {
                Mvx.Resolve<IMvxMessenger>().Unsubscribe<BackScanMessage>(backScanActionSubsciptionToken);
                backScanActionSubsciptionToken = null;
            }

            await Task.Delay(3000);

            await Scan();
        }

        public override void OnBackButtonClick()
        {
            base.OnBackButtonClick();

            ClearDataonBackPressed();

            ShowViewModel<ConnectToDeviceViewModel>(new { pop = "pop" });
        }

        public void ClearDataonBackPressed()
        {
            Stop();

            if (ActionMenuSubscriptionToken != null)
            {
                Mvx.Resolve<IMvxMessenger>().Unsubscribe<ActionsMenuMessage>(ActionMenuSubscriptionToken);
                ActionMenuSubscriptionToken = null;
            }
            if (ResetSubscriptionToken != null)
            {
                Mvx.Resolve<IMvxMessenger>().Unsubscribe<ActionsMenuMessage>(ResetSubscriptionToken);
                ResetSubscriptionToken = null;
            }
            if (StartScanSubsciptionToken != null)
            {
                Mvx.Resolve<IMvxMessenger>().Unsubscribe<StartScanMessage>(StartScanSubsciptionToken);
                StartScanSubsciptionToken = null;
            }

            BattViewQuantum.Instance.Clear();
            MCBQuantum.Instance.Clear();
        }

        public void ClearTimers()
        {
            if (ACConstants.ConnectionType != ConnectionTypesEnum.USB && connManager != null)
            {
                connManager.MyCommunicator.StopScan();
            }
        }

        async Task CreateConnectionForBATTView(DeviceInfoItem item)
        {
            //If Battery
            //First Select the Device
            connManager.selectDevice(item.DeviceID);

            BattViewCommunicationTypes caller = BattViewCommunicationTypes.NOCall;
            object arg1 = null;

            caller = BattViewCommunicationTypes.connectCommand;
            BattViewQuantum.Instance.SetConnectionManager(connManager);
            List<object> arguments = new List<object>();
            arguments.Add(caller);
            arguments.Add(arg1);

            try
            {
                var result = await BattViewQuantum.Instance.CommunicateBATTView(arguments);

                if (result.Count > 0)
                {
                    if (result[2].Equals(CommunicationResult.OK) && BattViewQuantum.Instance.BattView_quickSerialNumberCheckStrict())
                    {

                        // Check whether this is replacement or not
                        if (IsReplacement)
                        {
                            foreach (var replacement in ReplacementItemSource)
                            {
                                if (replacement.DeviceID != item.DeviceID)
                                {
                                    if (replacement.IsConnected)
                                    {
                                        replacement.IsConnected = false;
                                    }
                                }
                                else
                                {

                                    item.IsConnected = true;
                                }
                            }

                            RaisePropertyChanged(() => ReplacementItemSource);

                            Stop();

                            ShowViewModel<ReplacementViewModel>(new { itemStr = JsonConvert.SerializeObject(item) });
                        }
                        else
                        {
                            foreach (var BattView in BattViewItemSource)
                            {
                                if (BattView.DeviceID != item.DeviceID)
                                {
                                    if (BattView.IsConnected)
                                    {
                                        BattView.IsConnected = false;
                                    }
                                }
                                else
                                {

                                    item.IsConnected = true;
                                }
                            }

                            RaisePropertyChanged(() => BattViewItemSource);

                            if (backScanActionSubsciptionToken == null)
                                backScanActionSubsciptionToken = Mvx.Resolve<IMvxMessenger>().Subscribe<BackScanMessage>(OnBackScanMessage);

                            Stop();

                            ShowViewModel<DeviceHomeViewModel>(new { title = item.PrettyName, isForBattViewDetails = true });
                        }

                    }
                    else
                    {
                        BattViewQuantum.Instance.SetConnectionManager(null);
                        ACUserDialogs.ShowAlert(AppResources.failed_to_connect);
                    }
                }
                else
                {
                    ACUserDialogs.ShowAlert("Unable to Establish Connection Please try again.");
                }
                ACUserDialogs.HideProgress();
            }
            catch (Exception ex)
            {
                ACUserDialogs.HideProgress();
                //AnalyticsManager.Track_App_Exception(ex.Message, false);
            }
        }

        async Task CreateConnectionForMCB(DeviceInfoItem item)
        {
            connManager.selectDevice(item.DeviceID);

            McbCommunicationTypes caller = McbCommunicationTypes.NOCall;
            object arg1 = null;

            caller = McbCommunicationTypes.connectcomamnd;
            MCBQuantum.Instance.SetConnectionManager(connManager);
            List<object> arguments = new List<object>();
            arguments.Add(caller);
            arguments.Add(arg1);
            try
            {
                var result = await MCBQuantum.Instance.CommunicateMCB(arguments);

                if (result.Count > 0)
                {
                    if (result[2].Equals(CommunicationResult.OK) && MCBQuantum.Instance.MCB_quickSerialNumberCheckStrict())
                    {

                        if (IsReplacement)
                        {
                            foreach (var replacement in ReplacementItemSource)
                            {
                                if (replacement.DeviceID != item.DeviceID)
                                {
                                    if (replacement.IsConnected)
                                    {
                                        replacement.IsConnected = false;
                                    }
                                }
                                else
                                {

                                    item.IsConnected = true;
                                }
                            }

                            RaisePropertyChanged(() => ReplacementItemSource);

                            Stop();

                            ShowViewModel<ReplacementViewModel>(new { itemStr = JsonConvert.SerializeObject(item) });
                        }
                        else
                        {
                            foreach (var mcb in ChargerItemSource)
                            {
                                if (mcb.DeviceID != item.DeviceID)
                                {
                                    if (mcb.IsConnected)
                                    {
                                        mcb.IsConnected = false;
                                    }
                                }
                                else
                                {
                                    item.IsConnected = true;
                                }
                            }

                            RaisePropertyChanged(() => ChargerItemSource);

                            connManager.MyCommunicator.Close();

                            if (backScanActionSubsciptionToken == null)
                                backScanActionSubsciptionToken = Mvx.Resolve<IMvxMessenger>().Subscribe<BackScanMessage>(OnBackScanMessage);

                            ShowViewModel<DeviceHomeViewModel>(new { title = item.PrettyName, isForBattViewDetails = false });
                        }

                    }
                    else
                    {
                        MCBQuantum.Instance.SetConnectionManager(null);
                        ACUserDialogs.ShowAlert(AppResources.failed_to_connect);
                    }
                }
                else
                {
                    ACUserDialogs.ShowAlert("Unable to Establish Connection Please try again.");
                }
                ACUserDialogs.HideProgress();
            }
            catch (Exception ex)
            {
                ACUserDialogs.HideProgress();
                //AnalyticsManager.Track_App_Exception(ex.Message, false);
            }
        }

        public void OnForeground()
        {
            if (NetworkCheck(true))
            {
                if (ScanBtnVisibility)
                    ScantBtnClicked.Execute();
            }
            else
            {
                Stop();
            }
        }
    }
}
