using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace actchargers
{
    public class WiFiDeviceSubController : WiFiBaseSubController
    {
        ListViewItem RssiItem;
        ListViewItem AtVersionItem;
        ListViewItem IpItem;
        ListViewItem ConnectedToItem;
        ListViewItem IsCommunicatingItem;
        ListViewItem RestartStatusItem;
        ListViewItem GatewayAddressItem;
        ListViewItem MacAddressItem;

        public WiFiDeviceSubController(bool isBattView) : base(isBattView, false)
        {
        }

        internal override void InitExclusiveBattViewItems()
        {
            InitWiFiDebugInfoItems();
        }

        internal override void InitExclusiveMcbItems()
        {
            InitWiFiDebugInfoItems();
        }

        #region Init WiFi Debug

        void InitWiFiDebugInfoItems()
        {
            if (CanAddWiFiDebugList())
                InitWiFiDebugInfoItemsForEngineer();
        }

        void InitWiFiDebugInfoItemsForEngineer()
        {
            RssiItem = new ListViewItem()
            {
                Index = 11,
                Title = AppResources.wifi_rssi,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.LabelLabel,
            };
            AtVersionItem = new ListViewItem()
            {
                Index = 12,
                Title = AppResources.wifi_at_version,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.LabelLabel,
            };
            IpItem = new ListViewItem()
            {
                Index = 13,
                Title = AppResources.wifi_ip,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.LabelLabel,
            };
            ConnectedToItem = new ListViewItem()
            {
                Index = 14,
                Title = AppResources.wifi_connected_to,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.LabelLabel,
                IsEditable = false,
            };
            IsCommunicatingItem = new ListViewItem()
            {
                Index = 15,
                Title = AppResources.wifi_is_communicating,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.LabelLabel,
                IsEditable = false,
            };
            RestartStatusItem = new ListViewItem()
            {
                Index = 16,
                Title = AppResources.wifi_restart_status,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.LabelLabel,
                IsEditable = false,
            };
            GatewayAddressItem = new ListViewItem()
            {
                Index = 17,
                Title = AppResources.wifi_gateway_address,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.LabelLabel,
            };
            MacAddressItem = new ListViewItem()
            {
                Index = 18,
                Title = AppResources.wifi_mac_address,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.LabelLabel,
            };
        }

        #endregion

        internal override void LoadExclusiveValues()
        {
            if (CanAddWiFiDebugList())
                LoadWiFiInfo();
        }

        #region Load BattView

        internal override void LoadBattViewValues()
        {
            try
            {
                TryLoadBattViewValues();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);

                Logger.AddLog(true, "X25" + ex);
            }
        }

        void TryLoadBattViewValues()
        {
            if (!BattViewQuantum.Instance.BattView_quickSerialNumberCheckStrict())
                return;

            var config = BattViewQuantum.Instance.GetBATTView().Config;

            MobileAccessSsid.SubTitle = MobileAccessSsid.Text = config.mobileAccessSSID;
            MobileAccessSsidPassword.SubTitle = MobileAccessSsidPassword.Text = config.mobileAccessSSIDpassword;
            MobileViewPort.SubTitle = MobileViewPort.Text = config.mobilePort.ToString();
            SoftApEnabled.IsSwitchEnabled = config.IsSoftApEnable;
            SoftApPasswordTextBox.SubTitle = SoftApPasswordTextBox.Text = config.softAPpassword;
            ConnectToActViewEnabled.IsSwitchEnabled = config.ActViewEnabled;
            ActViewIp.SubTitle = ActViewIp.Text = config.actViewIP;
            ActViewPort.SubTitle = ActViewPort.Text = config.actViewPort.ToString();
            ActViewConnectFrequency.SubTitle = ActViewConnectFrequency.Text = config.actViewConnectFrequency.ToString();

            SetAccessSsidPasswordMaxLengthBasedOnVersion(config.battviewVersion);
            ActAccessSsidPassword.SubTitle = ActAccessSsidPassword.Text = config.actAccessSSIDpassword;

            ActAccessSsid.SubTitle = ActAccessSsid.Text = config.actAccessSSID;

            IsRestoreEnable =
                (CheckDefaults()) &&
                (ControlObject.UserAccess.Batt_onlyForEnginneringTeam == AccessLevelConsts.write);
        }

        #endregion

        #region Load MCB

        internal override void LoadMcbValues()
        {
            try
            {
                TryLoadMcbValues();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Logger.AddLog(true, "X25" + ex);
            }
        }

        void TryLoadMcbValues()
        {
            if (!MCBQuantum.Instance.MCB_quickSerialNumberCheckStrict())
                return;

            var config = MCBQuantum.Instance.GetMCB().Config;

            MobileAccessSsid.SubTitle = MobileAccessSsid.Text = config.mobileAccessSSID;
            MobileAccessSsidPassword.SubTitle = MobileAccessSsidPassword.Text = config.mobileAccessPassword;
            MobileViewPort.SubTitle = MobileViewPort.Text = config.mobilePort;
            SoftApEnabled.IsSwitchEnabled = config.softAPenabled;
            SoftApPasswordTextBox.SubTitle = SoftApPasswordTextBox.Text = config.softAPpassword;
            ConnectToActViewEnabled.IsSwitchEnabled = config.actViewEnable;
            ActViewIp.SubTitle = ActViewIp.Text = config.actViewIP;
            ActViewPort.SubTitle = ActViewPort.Text = config.actViewPort;
            ActViewConnectFrequency.SubTitle = ActViewConnectFrequency.Text = config.actViewConnectFrequency;

            SetAccessSsidPasswordMaxLengthBasedOnVersion(config.version);
            ActAccessSsidPassword.SubTitle = ActAccessSsidPassword.Text = config.actAccessPassword;

            ActAccessSsid.SubTitle = ActAccessSsid.Text = config.actAccessSSID;

            IsRestoreEnable = (CheckDefaults()) &&
                (ControlObject.UserAccess.MCB_onlyForEnginneringTeam != AccessLevelConsts.noAccess);
        }

        #endregion

        #region Load WiFi Info

        void LoadWiFiInfo()
        {
            Task.Run(LoadWiFiInfoAsync);
        }

        async Task LoadWiFiInfoAsync()
        {
            await ReadWiFiInfo();

            DeviceLoadWiFiInfo();

            FireOnListChanged();
        }

        async Task ReadWiFiInfo()
        {
            try
            {
                await TryReadWiFiInfo();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        async Task TryReadWiFiInfo()
        {
            if (isBattView)
                await BattViewQuantum.Instance.GetBATTView().ReadWiFiInfo();
            else
                await MCBQuantum.Instance.GetMCB().ReadWiFiInfo();
        }

        void DeviceLoadWiFiInfo()
        {
            WiFiDebug wifi;

            if (isBattView)
                wifi = BattViewQuantum.Instance.GetBATTView().WiFiInfo;
            else
                wifi = MCBQuantum.Instance.GetMCB().WiFiInfo;

            LoadWiFiInfo(wifi);
        }

        void LoadWiFiInfo(WiFiDebug wifi)
        {
            if (wifi == null)
                return;

            RssiItem.SubTitle = RssiItem.Text = wifi.Rssi.ToString();
            AtVersionItem.SubTitle = AtVersionItem.Text = wifi.AtVersion;
            IpItem.SubTitle = IpItem.Text = wifi.Ip;
            ConnectedToItem.SubTitle = ConnectedToItem.Text = wifi.ConnectionInfo;
            IsCommunicatingItem.IsSwitchEnabled = wifi.IsCommunicating;
            RestartStatusItem.SubTitle = RestartStatusItem.Text = wifi.RestartStatusString;
            GatewayAddressItem.SubTitle = GatewayAddressItem.Text = wifi.Gateway;
            MacAddressItem.SubTitle = MacAddressItem.Text = wifi.MacAddress;
        }

        #endregion

        internal override int GetActViewEnabledAccessLevel()
        {
            int actViewEnabledAccessLevel;

            BattViewObject currentBattView = BattViewQuantum.Instance.GetBATTView();

            if (currentBattView.Config.IsBattViewMobile())
                actViewEnabledAccessLevel = AccessLevelConsts.write;
            else
                actViewEnabledAccessLevel = ControlObject.UserAccess.Batt_actViewEnabled;

            return actViewEnabledAccessLevel;
        }

        #region Add WiFi Info

        internal override void AddExclusiveItems()
        {
            if (CanAddWiFiDebugList())
                AddWiFiInfoItems();
        }

        void AddWiFiInfoItems()
        {
            WiFiInfoAccessApply();
        }

        int WiFiInfoAccessApply()
        {
            accessControlUtility
                    .DoApplyAccessControl
                    (AccessLevelConsts.write, RssiItem, ItemSource);
            accessControlUtility
                .DoApplyAccessControl
                (AccessLevelConsts.write, AtVersionItem, ItemSource);
            accessControlUtility
                .DoApplyAccessControl
                (AccessLevelConsts.write, IpItem, ItemSource);
            accessControlUtility
                .DoApplyAccessControl
                (AccessLevelConsts.write, ConnectedToItem, ItemSource);
            accessControlUtility
                .DoApplyAccessControl
                (AccessLevelConsts.write, IsCommunicatingItem, ItemSource);
            accessControlUtility
                .DoApplyAccessControl
                (AccessLevelConsts.write, RestartStatusItem, ItemSource);
            accessControlUtility
                .DoApplyAccessControl
                (AccessLevelConsts.write, GatewayAddressItem, ItemSource);
            accessControlUtility
                .DoApplyAccessControl
                (AccessLevelConsts.write, MacAddressItem, ItemSource);

            ShowEdit &= accessControlUtility.GetSavedCount() != 0;

            return accessControlUtility.GetVisibleCount();
        }

        #endregion

        bool CanAddWiFiDebugList()
        {
            if (isBattView)
                return
                    (ControlObject.UserAccess.Batt_onlyForEnginneringTeam != AccessLevelConsts.noAccess);

            return
                (ControlObject.UserAccess.MCB_onlyForEnginneringTeam != AccessLevelConsts.noAccess);
        }
    }
}
