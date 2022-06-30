using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using actchargers.Code.Helpers.SynchSites;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using Plugin.Connectivity;
using Plugin.Connectivity.Abstractions;
using Plugin.DeviceInfo;

namespace actchargers.ViewModels
{
    public class ConnectViewModel : BaseViewModel
    {
        ConnectionManager connManager;
        readonly BackupAndCleaner backupAndCleaner;

        public string SSIDTitle { get; set; }

        public string SSIDTitleMobile { get; set; }

        //SSIDText is kept globally in baseviewmodel to give access rights to other viewmodels.
        public string PassowordTitle { get; set; }
        public string PassowordTitleMobile { get; set; }

        bool _showStationaryRouterModeFields;
        public bool ShowStationaryRouterModeFields
        {
            get { return _showStationaryRouterModeFields; }
            set
            {
                SetProperty(ref _showStationaryRouterModeFields, value);
            }
        }

        string _passwordText;
        public string PasswordText
        {
            get { return _passwordText; }
            set
            {
                SetProperty(ref _passwordText, value);
            }
        }

        string _passwordTextMobile;
        public string PasswordTextMobile
        {
            get { return _passwordTextMobile; }
            set
            {
                SetProperty(ref _passwordTextMobile, value);
            }
        }

        string _connectButtonTitle;
        public string ConnectButtonTitle
        {
            get { return _connectButtonTitle; }
            set
            {
                SetProperty(ref _connectButtonTitle, value);
            }
        }

        string _ssidName;
        public string SSIDName
        {
            get { return _ssidName; }
            set
            {
                SetProperty(ref _ssidName, value);
            }
        }

        string _ssidText;
        public string SSIDText
        {
            get { return _ssidText; }
            set
            {
                SetProperty(ref _ssidText, value);
            }
        }



        string _ssidNameMobile;
        public string SSIDNameMobile
        {
            get { return _ssidNameMobile; }
            set
            {
                SetProperty(ref _ssidNameMobile, value);
            }
        }

        string _ssidTextMobile;
        public string SSIDTextMobile
        {
            get { return _ssidTextMobile; }
            set
            {
                SetProperty(ref _ssidTextMobile, value);
            }
        }

        string _portTitle;
        public string PortTitle
        {
            get { return _portTitle; }
            set
            {
                SetProperty(ref _portTitle, value);
            }
        }

        string _portText;
        public string PortText
        {
            get { return _portText; }
            set
            {
                SetProperty(ref _portText, value);
            }
        }


        public string MessageText { get; set; }
        public string UploadTitle { get; set; }

        public string UploadSubTitle
        {
            get
            {
                return TimeToTextHelper.GetUploadTimeText();
            }
        }

        public string SyncSiteTitle { get; set; }

        public string SyncSiteSubTitle
        {
            get
            {
                return TimeToTextHelper.GetSynchSiteTimeText();
            }
        }

        public string PushBackupTitle { get; set; }
        public string UploadImage { get; set; }
        public string SyncImage { get; set; }
        public string PushBackupImage { get; set; }
        public string PrettyName { get; set; }

        public ConnectViewModel()
        {
            backupAndCleaner = new BackupAndCleaner();

            ViewTitle = AppResources.connect;
            SSIDTitle = AppResources.ssid;

            SSIDTitleMobile = AppResources.ssid;
            PortTitle = AppResources.port;
            ControlObject.connectMethods.Connectiontype = ConnectionTypesEnum.ROUTER;

            TabChanged(UiConnectionType.ROUTER);

            PassowordTitle = AppResources.password;
            PassowordTitleMobile = AppResources.password;

            ConnectButtonTitle = AppResources.connect;
            MessageText = AppResources.upload_data_or_sync_device;
            UploadTitle = AppResources.upload_data;
            SyncSiteTitle = AppResources.sync_sites;
            PushBackupTitle = AppResources.push_backup;
            UploadImage = "upload";
            SyncImage = "sync_sites";

            Logger.AddLog(false, "ConnectScreen");

            Firmware.LoadLatestFirmWareIDFromDB();
        }

        public void TabChanged(UiConnectionType connType)
        {
            switch (connType)
            {
                case UiConnectionType.ROUTER:
                    ACConstants.ConnectionType = ConnectionTypesEnum.ROUTER;
                    if (ControlObject.isDebugMaster)
                    {
                        SSIDText = "thikMobile" + (ControlObject.connectMethods.Connectiontype == ConnectionTypesEnum.ROUTER ? "Local" : "");
                        PasswordText = "tetoMobile" + (ControlObject.connectMethods.Connectiontype == ConnectionTypesEnum.ROUTER ? "Local" : "");
                    }
                    else
                    {
                        SSIDText = "act24mobile" + (ControlObject.connectMethods.Connectiontype == ConnectionTypesEnum.ROUTER ? "Local" : "");
                        PasswordText = "shlonak5al" + (ControlObject.connectMethods.Connectiontype == ConnectionTypesEnum.ROUTER ? "Local" : "");
                    }

                    ACConstants.WIFI_SSID = SSIDText;
                    ACConstants.WIFI_PASSWORD = PasswordText;
                    break;

                case UiConnectionType.USB:
                    ACConstants.ConnectionType = ConnectionTypesEnum.USB;
                    break;

                case UiConnectionType.STATIONARY:
                    if (ControlObject.UserAccess.Batt_onlyForEnginneringTeam
                        == AccessLevelConsts.write
                        && ControlObject.UserAccess.MCB_onlyForEnginneringTeam
                        == AccessLevelConsts.write)
                    {
                        ShowStationaryRouterModeFields = IsAdmin;
                    }
                    else
                    {
                        ShowStationaryRouterModeFields = IsAdmin;
                    }

                    if (ControlObject.isDebugMaster)
                    {
                        SSIDText = "thikActAccess";
                        PasswordText = "hActAccess";
                    }
                    else
                    {
                        SSIDText = "actAccess24";
                        PasswordText = "hala3ami102";
                    }
                    //PortText = "9308";
                    ACConstants.ConnectionType = ConnectionTypesEnum.ROUTER;
                    ACConstants.WIFI_SSID = SSIDText;
                    ACConstants.WIFI_PASSWORD = PasswordText;
                    break;
            }
        }

        /// <summary>
        /// Gets the connect button clicked.
        /// </summary>
        /// <value>The connect button clicked.</value>
        public IMvxCommand ConnectBtnClicked
        {
            get
            {
                return new MvxCommand(OnConnectBtnClicked);
            }
        }

        void OnConnectBtnClicked()
        {
            try
            {
                TryOnConnectBtnClicked();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        void TryOnConnectBtnClicked()
        {
            if (IsEmptyWifiInfo())
            {
                ACUserDialogs.ShowAlert(AppResources.invalid_ssid_password);

                return;
            }

            Logger.AddLog(false, "Type:" + ControlObject.connectMethods.Connectiontype.ToString());

            ACConstants.WIFI_SSID = SSIDText;
            ACConstants.WIFI_PASSWORD = PasswordText;

            if (DevelopmentProfileHelper.IsEmulator())
            {
                ShowViewModel<ConnectToDeviceViewModel>();

                return;
            }

            if (IsNotUsb())
                ProcessWiFi();
            else
                ShowViewModel<ConnectToDeviceViewModel>();
        }

        bool IsEmptyWifiInfo()
        {
            return string.IsNullOrEmpty(SSIDText) || string.IsNullOrEmpty(PasswordText);
        }

        bool IsNotUsb()
        {
            return ACConstants.ConnectionType != ConnectionTypesEnum.USB;
        }

        void ProcessWiFi()
        {
            if (CrossConnectivity.Current.IsConnected)
                ProcessConnected();
            else
                ShowConnectToWiFiNetworkAlert(SSIDText);

            ACUserDialogs.HideProgress();
        }

        void ProcessConnected()
        {
            ACUserDialogs.ShowProgress();

            var allNetworks = CrossConnectivity.Current.ConnectionTypes;

            if (allNetworks == null)
            {
                ShowConnectToWiFiNetworkAlert(SSIDText);

                return;
            }

            if (allNetworks.Contains(ConnectionType.WiFi))
                ProcessWifiConnection();
        }

        void ProcessWifiConnection()
        {
            string wifiSSID = Mvx.Resolve<IWifiManagerService>().GetConnectedWifiSSID().Trim('"');

            if (IsIos())
            {
                ProcessIos(wifiSSID);
            }
            else
            {
                ProcessNotIos(wifiSSID);
            }
        }

        #region iOS

        bool IsIos()
        {
            return CrossDeviceInfo.Current.Platform == Plugin.DeviceInfo.Abstractions.Platform.iOS;
        }

        void ProcessIos(string wifiSSID)
        {
            if (IsCorrectWifiNetwork())
            {
                ProcesPredefinedWifiNetwork(wifiSSID);
            }
            else
            {
                ProcesNotPredefinedWifiNetwork(wifiSSID);
            }
        }

        bool IsCorrectWifiNetwork()
        {
            return SSIDText == ACConstants.ACT_24_MOBILE_LOCAL || SSIDText == ACConstants.ACT_ACCESS_24;
        }

        void ProcesPredefinedWifiNetwork(string wifiSSID)
        {
            bool profileChecked = Mvx.Resolve<IProfileCheckService>()
                .CheckForProfile(ACConstants.ACT_IOS_CERTIFICATE_NAME);

            if (profileChecked)
            {
                ProcesCorrectIosWifiCertificate(wifiSSID);
            }
            else
            {
                ACUserDialogs.ShowAlertThenOpenUrl(AppResources.connect_alert_for_profile, AppResources.network_profile_url);
            }
        }

        void ProcesCorrectIosWifiCertificate(string wifiSSID)
        {
            if (SSIDText.Equals(wifiSSID))
            {
                ShowViewModel<ConnectToDeviceViewModel>();
            }
            else
            {
                ShowConnectToWiFiNetworkAlert(SSIDText);
            }
        }

        void ProcesNotPredefinedWifiNetwork(string wifiSSID)
        {
            if (wifiSSID.Equals(ACConstants.WIFI_SSID))
            {
                ShowViewModel<ConnectToDeviceViewModel>();
            }
            else
            {
                ShowConnectToWiFiNetworkAlert(SSIDText);
            }
        }

        #endregion

        #region Not iOS

        void ProcessNotIos(string wifiSSID)
        {
            if (wifiSSID.Equals(ACConstants.WIFI_SSID) || wifiSSID.StartsWith("<", StringComparison.CurrentCulture))
            {
                ShowViewModel<ConnectToDeviceViewModel>();
            }
            else
            {
                ShowConnectToWiFiNetworkAlert(SSIDText);
            }
        }

        #endregion

        void ShowConnectToWiFiNetworkAlert(string ssid)
        {
            string msg = string.Format(AppResources.please_connect_to_v_network, ssid);

            ACUserDialogs.ShowAlert(msg);
        }

        public IMvxCommand USBConnectBtnClicked
        {
            get
            {
                return new MvxCommand(OnUSBConnectBtnClicked);
            }
        }

        #region for usb scan

        void OnUSBConnectBtnClicked()
        {
            connManager = new ConnectionManager();

            SiteViewQuantum.Instance.SetConnectionManager(connManager);

            StartUsbScan();
        }

        void StartUsbScan()
        {
            if (ACConstants.ConnectionType == ConnectionTypesEnum.USB)
            {
                Task.Run(async () =>
                {
                    ACUserDialogs.ShowProgress();
                    await ScanUSB();

                    try
                    {
                        if (ACConstants.IsUSBBattView)
                        {
                            IsBattView = true;

                            PrettyName = "Battery:" + connManager.activeBattView.Config.batteryID.Trim(new char[] { '\0' }) + " (" + connManager.activeBattView.Config.battViewSN.Trim(new char[] { '\0' }) + ")";

                        }
                        else
                        {
                            IsBattView = false;

                            PrettyName = "Charger:" + connManager.activeMCB.Config.chargerUserName.Trim(new char[] { '\0' }) + " (" + connManager.activeMCB.Config.serialNumber.Trim(new char[] { '\0' }) + ")";

                        }

                    }
                    catch (Exception ex)
                    {
                        ACUserDialogs.HideProgress();
                        ACUserDialogs.ShowAlert("Connection problem");

                        Debug.WriteLine(ex.ToString());

                        return;
                    }

                    ACUserDialogs.HideProgress();
                    ShowViewModel<DeviceHomeViewModel>(new { title = PrettyName });

                });

            }
        }

        async Task ScanUSB()
        {
            await connManager.refresh_timer_prepare();
            bool requireUpdate = connManager.updateIfRequired();
            connManager.selectDevice(ACConstants.USBConnectedSerialNumber);

            if (ACConstants.IsUSBBattView)
            {
                BattViewQuantum.Instance.SetConnectionManager(connManager);
            }
            else
            {
                MCBQuantum.Instance.SetConnectionManager(connManager);
            }
        }


        #endregion

        /// <summary>
        /// Gets the upload data button clicked.
        /// </summary>
        /// <value>The upload data button clicked.</value>
        public IMvxCommand UploadDataBtnClicked
        {
            get { return new MvxCommand(OnUploadBtnClicked); }
        }

        void OnUploadBtnClicked()
        {
            ShowViewModel<UploadViewModel>();
        }

        /// <summary>
        /// Gets the sync sites button clicked.
        /// </summary>
        /// <value>The sync sites button clicked.</value>
        public IMvxCommand SyncSitesBtnClicked
        {
            get { return new MvxCommand(OnSyncSitesBtnClicked); }
        }

        void OnSyncSitesBtnClicked()
        {
            if (SynchSitesHelpers.CanDoSynchSites())
                ShowViewModel<SyncSitesViewModel>();
            else
                SynchSitesHelpers.ShowUploadsError();
        }

        void GoToSyncSites()
        {
            ShowViewModel<SyncSitesViewModel>();
        }

        public IMvxCommand PushBackupBtnClicked
        {
            get { return new MvxCommand(OnPushBackupBtnClicked); }
        }

        void OnPushBackupBtnClicked()
        {
            Task.Run(PushBackupAndAlert);
        }

        async Task PushBackupAndAlert()
        {
            await backupAndCleaner.PushBackup();

            ACUserDialogs.ShowAlert(AppResources.backup_pushed_alert);
        }
    }
}