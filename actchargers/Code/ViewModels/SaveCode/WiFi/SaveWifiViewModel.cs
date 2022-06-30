using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using MvvmCross.Core.ViewModels;

namespace actchargers
{
    public class SaveWifiViewModel : BaseViewModel
    {
        UIAccessControlUtility accessControlUtility;

        ListViewItem MobileAccessSsid;
        ListViewItem MobileAccessSsidPassword;
        ListViewItem MobileViewPort;
        ListViewItem SoftApEnabled;
        ListViewItem SoftApPasswordTextBox;
        ListViewItem ConnectToActViewEnabled;
        ListViewItem ActViewIp;
        ListViewItem ActViewPort;
        ListViewItem ActViewConnectFrequency;
        ListViewItem ActAccessSsid;
        ListViewItem ActAccessSsidPassword;
        ListViewItem RssiItem;
        ListViewItem AtVersionItem;
        ListViewItem IpItem;
        ListViewItem ConnectedToItem;
        ListViewItem IsCommunicatingItem;
        ListViewItem RestartStatusItem;
        ListViewItem GatewayAddressItem;
        ListViewItem MacAddressItem;

        VerifyControl verifyControl;

        ObservableCollection<ListViewItem> _wifiViewItemSource;
        public ObservableCollection<ListViewItem> WifiViewItemSource
        {
            get { return _wifiViewItemSource; }
            set
            {
                _wifiViewItemSource = value;
                RaisePropertyChanged(() => WifiViewItemSource);
            }
        }

        string _restoreToDefault;
        public string RestoreToDefault
        {
            get { return _restoreToDefault; }
            set
            {
                _restoreToDefault = value;
                RaisePropertyChanged(() => RestoreToDefault);
            }
        }

        bool _showEdit;
        public bool ShowEdit
        {
            get
            {
                return _showEdit;
            }
            set
            {
                _showEdit = value;
                RaisePropertyChanged(() => ShowEdit);
            }
        }

        bool _editMode;
        public bool EditingMode
        {
            get
            {
                return _editMode = _editMode && _editMode;

            }
            set
            {
                _editMode = value;
                RaisePropertyChanged(() => EditingMode);
            }
        }

        bool _isRestoreEnable;
        public bool ISRestoreEnable
        {
            get
            {
                return _isRestoreEnable;

            }
            set
            {
                _isRestoreEnable = value;
                RaisePropertyChanged(() => ISRestoreEnable);
            }
        }

        public SaveWifiViewModel()
        {
            ViewTitle = AppResources.wifi;
            RestoreToDefault = AppResources.restore_to_defaults;
            EditingMode = false;
            ISRestoreEnable = false;
            ShowEdit = true;
            WifiViewItemSource = new ObservableCollection<ListViewItem>();

            CreateList();
        }

        void CreateList()
        {
            CreateUnifiedList();

            Task.Run(async () => await CreateListForWiFiDebugInfo());
        }

        void CreateUnifiedList()
        {
            MobileAccessSsid = new ListViewItem()
            {
                Index = 0,
                Title = AppResources.mobile_ssid,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.LabelTextEdit,
                TextMinLength = 8,
                TextMaxLength = 31
            };
            MobileAccessSsidPassword = new ListViewItem()
            {
                Index = 1,
                Title = AppResources.mobile_ssid_password,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.LabelTextEdit,
                TextMinLength = 8,
                TextMaxLength = 13
            };
            MobileViewPort = new ListViewItem()
            {
                Index = 2,
                Title = AppResources.mobile_ssid_port,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.LabelTextEdit,
                TextMaxLength = 5,
                TextInputType = ACUtility.InputType.Number
            };
            SoftApEnabled = new ListViewItem()
            {
                Index = 3,
                Title = AppResources.direct_mode,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.LabelSwitch,
                SwitchValueChanged = SwitchValueChanged,
                EnableItemsList = new List<int>() { 4 }
            };
            SoftApPasswordTextBox = new ListViewItem()
            {
                Index = 4,
                Title = AppResources.soft_ap_password,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.LabelTextEdit,
                TextMinLength = 8,
                TextMaxLength = 13
            };
            ConnectToActViewEnabled = new ListViewItem()
            {
                Index = 5,
                Title = AppResources.act_intelligent,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.LabelSwitch,
                SwitchValueChanged = SwitchValueChanged,
                EnableItemsList = new List<int>() { 6, 7, 8, 9, 10 }
            };
            ActViewIp = new ListViewItem()
            {
                Index = 6,
                Title = AppResources.actview_ip,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.LabelTextEdit,
                TextMinLength = 7,
                TextMaxLength = 63
            };
            ActViewPort = new ListViewItem()
            {
                Index = 7,
                Title = AppResources.actview_port,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.LabelTextEdit,
                TextMaxLength = 5,
                TextInputType = ACUtility.InputType.Number
            };
            ActViewConnectFrequency = new ListViewItem()
            {
                Index = 8,
                Title = AppResources.actview_connect_frequency,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.LabelTextEdit,
                TextInputType = ACUtility.InputType.Number,
                TextMaxLength = 8
            };
            ActAccessSsid = new ListViewItem()
            {
                Index = 9,
                Title = AppResources.act_access_ssid,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.LabelTextEdit,
                TextMinLength = 8,
                TextMaxLength = 31
            };
            ActAccessSsidPassword = new ListViewItem()
            {
                Index = 10,
                Title = AppResources.act_access_ssid_password,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.LabelTextEdit,
                TextMinLength = 4,
                TextMaxLength = 31
            };

            LoadAndAdd();

            RaisePropertyChanged(() => WifiViewItemSource);
        }

        void LoadAndAdd()
        {
            if (IsBattView)
                LoadAndAddBattView();
            else
                LoadAndAddMcb();
        }

        void LoadAndAddBattView()
        {
            try
            {
                Batt_loadWiFiSettings();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Logger.AddLog(true, "X25" + ex);
            }

            if (BattViewWIFIAccessApply() == 0)
            {
                WifiViewItemSource.Clear();
                ACUserDialogs.ShowAlertWithOkButtonsAction(AppResources.no_data_found, () => { ShowViewModel<WiFiViewModel>(new { pop = "pop" }); });
                return;
            }

            if (WifiViewItemSource.Count > 0)
            {
                WifiViewItemSource = new ObservableCollection<ListViewItem>(WifiViewItemSource.OrderBy(o => o.Index));
            }
        }

        void LoadAndAddMcb()
        {
            try
            {
                MCB_loadWiFiSettings();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Logger.AddLog(true, "X25" + ex);
            }

            if (ChargerWIFIAccessApply() == 0)
            {
                WifiViewItemSource.Clear();
                ACUserDialogs.ShowAlertWithOkButtonsAction(AppResources.no_data_found, () => { ShowViewModel<WiFiViewModel>(new { pop = "pop" }); });
                return;
            }

            if (WifiViewItemSource.Count > 0)
            {
                WifiViewItemSource = new ObservableCollection<ListViewItem>(WifiViewItemSource.OrderBy(o => o.Index));
            }
        }

        void MCB_loadWiFiSettings()
        {
            if (!MCBQuantum.Instance.MCB_quickSerialNumberCheckStrict())
                return;

            MCBobject activeMCB = MCBQuantum.Instance.GetMCB();

            MobileAccessSsidPassword.SubTitle = MobileAccessSsidPassword.Text = activeMCB.Config.mobileAccessPassword;
            var mobileAccessSSID = activeMCB.Config.mobileAccessSSID;

            if (!string.IsNullOrEmpty(mobileAccessSSID))
            {
                mobileAccessSSID = mobileAccessSSID.Trim();
            }

            MobileAccessSsid.SubTitle = MobileAccessSsid.Text = mobileAccessSSID;
            MobileViewPort.SubTitle = MobileViewPort.Text = activeMCB.Config.mobilePort.ToString();

            if (activeMCB.Config.softAPenabled)
            {
                SoftApEnabled.IsSwitchEnabled = true;
            }
            else
            {
                SoftApEnabled.IsSwitchEnabled = false;
            }

            SoftApPasswordTextBox.SubTitle = SoftApPasswordTextBox.Text = activeMCB.Config.softAPpassword;

            if (activeMCB.Config.actViewEnable)
            {
                ConnectToActViewEnabled.IsSwitchEnabled = true;
            }
            else
            {
                ConnectToActViewEnabled.IsSwitchEnabled = false;
            }

            ActViewIp.SubTitle = ActViewIp.Text = activeMCB.Config.actViewIP;
            ActViewPort.SubTitle = ActViewPort.Text = activeMCB.Config.actViewPort.ToString();
            ActViewConnectFrequency.SubTitle = ActViewConnectFrequency.Text = activeMCB.Config.actViewConnectFrequency.ToString();
            ActAccessSsidPassword.SubTitle = ActAccessSsidPassword.Text = activeMCB.Config.actAccessPassword;
            ActAccessSsid.SubTitle = ActAccessSsid.Text = activeMCB.Config.actAccessSSID;

            MCB_VerfiyWiFiSettings();
            if (MCB_WIFI_checkDefault() && ControlObject.UserAccess.MCB_onlyForEnginneringTeam != AccessLevelConsts.noAccess)
            {
                ISRestoreEnable = true;
            }
            else
            {
                ISRestoreEnable = false;
            }
        }

        bool MCB_VerfiyWiFiSettings()
        {
            if (!MCBQuantum.Instance.MCB_quickSerialNumberCheckStrict())
                return false;
            verifyControl = new VerifyControl();
            MobileAccessSsidPassword.Text = StaticDataAndHelperFunctions.cleanWifiChars(MobileAccessSsidPassword.Text);
            MobileAccessSsid.Text = StaticDataAndHelperFunctions.cleanWifiChars(MobileAccessSsid.Text);
            SoftApPasswordTextBox.Text = StaticDataAndHelperFunctions.cleanWifiChars(SoftApPasswordTextBox.Text);
            ActAccessSsidPassword.Text = StaticDataAndHelperFunctions.cleanWifiChars(ActAccessSsidPassword.Text);
            ActAccessSsid.Text = StaticDataAndHelperFunctions.cleanWifiChars(ActAccessSsid.Text);

            verifyControl.VerifyTextBox(MobileAccessSsidPassword, MobileAccessSsidPassword, 8, 13);
            verifyControl.VerifyTextBox(MobileAccessSsid, MobileAccessSsid, 8, 31);
            verifyControl.VerifyInteger(MobileViewPort, MobileViewPort, 49152, 65535);
            //MCB_softAPEnableRadio check
            verifyControl.VerifyTextBox(SoftApPasswordTextBox, SoftApPasswordTextBox, 8, 13);
            //MCB_connectToACTViewEnableRadio check
            verifyControl.VerifyTextBox(ActViewIp, ActViewIp, 7, 63);
            verifyControl.VerifyInteger(ActViewPort, ActViewPort, 1024, 65535);
            verifyControl.VerifyInteger(ActViewConnectFrequency, ActViewConnectFrequency, 1, 65535);
            verifyControl.VerifyTextBox(ActAccessSsidPassword, ActAccessSsidPassword, 8, 13);
            verifyControl.VerifyTextBox(ActAccessSsid, ActAccessSsid, 4, 31);
            return !verifyControl.HasErrors();
        }

        int ChargerWIFIAccessApply()
        {
            accessControlUtility = new UIAccessControlUtility();
            accessControlUtility
                .DoApplyAccessControl
                (ControlObject.UserAccess.MCB_actViewEnabled,
                 ConnectToActViewEnabled, WifiViewItemSource);

            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_softAPEnable, SoftApEnabled, WifiViewItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_softAPpassword, SoftApPasswordTextBox, WifiViewItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_mobileAccessSSID, MobileAccessSsid, WifiViewItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_mobileAccessSSIDpassword, MobileAccessSsidPassword, WifiViewItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_mobilePort, MobileViewPort, WifiViewItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_actViewIP, ActViewIp, WifiViewItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_actViewConnectFrequency, ActViewConnectFrequency, WifiViewItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_actViewPort, ActViewPort, WifiViewItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_actAccessSSID, ActAccessSsid, WifiViewItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_actAccessSSIDpassword, ActAccessSsidPassword, WifiViewItemSource);

            ShowEdit &= accessControlUtility.GetSavedCount() != 0;

            return accessControlUtility.GetVisibleCount();
        }

        void MCB_SaveIntoWiFiSettings()
        {
            if (!MCBQuantum.Instance.MCB_quickSerialNumberCheckStrict())
                return;
            MCBobject activeMCB = MCBQuantum.Instance.GetMCB();
            activeMCB.Config.mobileAccessPassword = MobileAccessSsidPassword.Text;
            activeMCB.Config.mobileAccessSSID = MobileAccessSsid.Text;
            activeMCB.Config.mobilePort = MobileViewPort.Text;
            activeMCB.Config.softAPenabled = SoftApEnabled.IsSwitchEnabled;
            activeMCB.Config.softAPpassword = SoftApPasswordTextBox.Text;
            activeMCB.Config.actViewEnable = ConnectToActViewEnabled.IsSwitchEnabled;
            activeMCB.Config.actViewIP = ActViewIp.Text;
            activeMCB.Config.actViewPort = ActViewPort.Text;
            activeMCB.Config.actViewConnectFrequency = ActViewConnectFrequency.Text;
            activeMCB.Config.actAccessPassword = ActAccessSsidPassword.Text;
            activeMCB.Config.actAccessSSID = ActAccessSsid.Text;
        }

        bool MCB_WIFI_checkDefault()
        {
            if (!MCBQuantum.Instance.MCB_quickSerialNumberCheckStrict())
                return false;
            if (MobileAccessSsid.Text != "act24mobile" ||
            MobileAccessSsidPassword.Text != "shlonak5al" ||
            MobileViewPort.Text != "50000" ||
                SoftApEnabled.IsSwitchEnabled != false ||
            SoftApPasswordTextBox.Text != "actDirmank" ||
                ConnectToActViewEnabled.IsSwitchEnabled != true ||
            ActViewIp.Text != "act-view.com" ||
            ActViewPort.Text != "9309" ||
            ActViewConnectFrequency.Text != "60" ||
            ActAccessSsidPassword.Text != "hala3ami102" ||
            ActAccessSsid.Text != "actAccess24")
            {
                return true;
            }
            return false;
        }

        void Batt_loadWiFiSettings()
        {
            if (!BattViewQuantum.Instance.BattView_quickSerialNumberCheckStrict())
                return;

            BattViewObject currentBattView = BattViewQuantum.Instance.GetBATTView();
            MobileAccessSsidPassword.SubTitle = MobileAccessSsidPassword.Text = currentBattView.Config.mobileAccessSSIDpassword;
            MobileAccessSsid.SubTitle = MobileAccessSsid.Text = currentBattView.Config.mobileAccessSSID;
            MobileViewPort.SubTitle = MobileViewPort.Text = currentBattView.Config.mobilePort.ToString();
            SoftApEnabled.IsSwitchEnabled = currentBattView.Config.IsSoftApEnable;
            SoftApPasswordTextBox.SubTitle = SoftApPasswordTextBox.Text = currentBattView.Config.softAPpassword;
            ConnectToActViewEnabled.IsSwitchEnabled = currentBattView.Config.ActViewEnabled;
            ActViewIp.SubTitle = ActViewIp.Text = currentBattView.Config.actViewIP;
            ActViewPort.SubTitle = ActViewPort.Text = currentBattView.Config.actViewPort.ToString();
            ActViewConnectFrequency.SubTitle = ActViewConnectFrequency.Text = currentBattView.Config.actViewConnectFrequency.ToString();
            ActAccessSsidPassword.SubTitle = ActAccessSsidPassword.Text = currentBattView.Config.actAccessSSIDpassword;
            ActAccessSsid.SubTitle = ActAccessSsid.Text = currentBattView.Config.actAccessSSID;
            Batt_VerfiyWiFiSettings();
            if (Batt_WIFI_CheckDefaults() && ControlObject.UserAccess.Batt_onlyForEnginneringTeam == AccessLevelConsts.write)
            {
                ISRestoreEnable = true;
            }
            else
            {
                ISRestoreEnable = false;
            }
        }

        bool Batt_WIFI_CheckDefaults()
        {
            if (!BattViewQuantum.Instance.BattView_quickSerialNumberCheckStrict())
                return false;

            if (MobileAccessSsidPassword.Text != "shlonak5al" ||
            MobileAccessSsid.Text != "act24mobile" ||
            MobileViewPort.Text != "50000" ||
                SoftApEnabled.IsSwitchEnabled != true ||
            SoftApPasswordTextBox.Text != "actDirmank" ||
                ConnectToActViewEnabled.IsSwitchEnabled != true ||
            ActViewIp.Text != "act-view.com" ||
            ActViewPort.Text != "9309" ||
            ActViewConnectFrequency.Text != "900" ||
            ActAccessSsidPassword.Text != "hala3ami102" ||
            ActAccessSsid.Text != "actAccess24")
            {
                return true;
            }
            return false;
        }

        bool Batt_VerfiyWiFiSettings()
        {
            verifyControl = new VerifyControl();
            MobileAccessSsidPassword.Text = StaticDataAndHelperFunctions.cleanWifiChars(MobileAccessSsidPassword.Text);
            MobileAccessSsid.Text = StaticDataAndHelperFunctions.cleanWifiChars(MobileAccessSsid.Text);
            SoftApPasswordTextBox.Text = StaticDataAndHelperFunctions.cleanWifiChars(SoftApPasswordTextBox.Text);
            ActAccessSsidPassword.Text = StaticDataAndHelperFunctions.cleanWifiChars(ActAccessSsidPassword.Text);
            ActAccessSsid.Text = StaticDataAndHelperFunctions.cleanWifiChars(ActAccessSsid.Text);

            verifyControl.VerifyTextBox(MobileAccessSsidPassword, MobileAccessSsidPassword, 8, 13);
            verifyControl.VerifyTextBox(MobileAccessSsid, MobileAccessSsid, 8, 31);
            verifyControl.VerifyInteger(MobileViewPort, MobileViewPort, 49152, 65535);

            verifyControl.VerifyTextBox(SoftApPasswordTextBox, SoftApPasswordTextBox, 8, 13);

            verifyControl.VerifyTextBox(ActViewIp, ActViewIp, 7, 63);
            verifyControl.VerifyInteger(ActViewPort, ActViewPort, 1024, 65535);
            verifyControl.VerifyInteger(ActViewConnectFrequency, ActViewConnectFrequency, 1, 65535);
            verifyControl.VerifyTextBox(ActAccessSsidPassword, ActAccessSsidPassword, 8, 13);
            verifyControl.VerifyTextBox(ActAccessSsid, ActAccessSsid, 4, 31);
            return !verifyControl.HasErrors();
        }

        int BattViewWIFIAccessApply()
        {
            BattViewObject currentBattView = BattViewQuantum.Instance.GetBATTView();
            int actViewEnabled;

            if (currentBattView.Config.isPA != 0x00)
                actViewEnabled = AccessLevelConsts.write;
            else
                actViewEnabled = ControlObject.UserAccess.Batt_actViewEnabled;

            accessControlUtility = new UIAccessControlUtility();
            accessControlUtility
                .DoApplyAccessControl
                (actViewEnabled, ConnectToActViewEnabled, WifiViewItemSource);

            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_softAPEnable, SoftApEnabled, WifiViewItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_softAPpassword, SoftApPasswordTextBox, WifiViewItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_mobileAccessSSID, MobileAccessSsid, WifiViewItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_mobileAccessSSIDpassword, MobileAccessSsidPassword, WifiViewItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_mobilePort, MobileViewPort, WifiViewItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_actViewIP, ActViewIp, WifiViewItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_actViewConnectFrequency, ActViewConnectFrequency, WifiViewItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_actViewPort, ActViewPort, WifiViewItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_actAccessSSID, ActAccessSsid, WifiViewItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_actAccessSSIDpassword, ActAccessSsidPassword, WifiViewItemSource);

            ShowEdit &= accessControlUtility.GetSavedCount() != 0;

            return accessControlUtility.GetVisibleCount();
        }

        void Batt_SaveIntoWiFiSettings()
        {
            if (!BattViewQuantum.Instance.BattView_quickSerialNumberCheckStrict())
            {
                return;
            }
            BattViewQuantum.Instance.GetBATTView().Config.mobileAccessSSIDpassword = MobileAccessSsidPassword.Text;
            BattViewQuantum.Instance.GetBATTView().Config.mobileAccessSSID = MobileAccessSsid.Text;
            BattViewQuantum.Instance.GetBATTView().Config.mobilePort = ushort.Parse(MobileViewPort.Text);
            BattViewQuantum.Instance.GetBATTView().Config.IsSoftApEnable = SoftApEnabled.IsSwitchEnabled;
            BattViewQuantum.Instance.GetBATTView().Config.softAPpassword = SoftApPasswordTextBox.Text;
            BattViewQuantum.Instance.GetBATTView().Config.ActViewEnabled = ConnectToActViewEnabled.IsSwitchEnabled;
            BattViewQuantum.Instance.GetBATTView().Config.actViewIP = ActViewIp.Text;
            BattViewQuantum.Instance.GetBATTView().Config.actViewPort = ushort.Parse(ActViewPort.Text);
            BattViewQuantum.Instance.GetBATTView().Config.actViewConnectFrequency = ushort.Parse(ActViewConnectFrequency.Text);
            BattViewQuantum.Instance.GetBATTView().Config.actAccessSSIDpassword = ActAccessSsidPassword.Text;
            BattViewQuantum.Instance.GetBATTView().Config.actAccessSSID = ActAccessSsid.Text;
        }

        #region WiFi Debug Info

        async Task CreateListForWiFiDebugInfo()
        {
            if (CanAddWiFiDebugList())
                await CreateListForWiFiDebugInfoForEngineer();
        }

        bool CanAddWiFiDebugList()
        {
            if (IsBattView)
                return
                    (ControlObject.UserAccess.Batt_onlyForEnginneringTeam != AccessLevelConsts.noAccess);

            return
                (ControlObject.UserAccess.MCB_onlyForEnginneringTeam != AccessLevelConsts.noAccess);
        }

        async Task CreateListForWiFiDebugInfoForEngineer()
        {
            await ReadWiFiInfo();

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

            WiFiInfoAccessApply();

            DeviceLoadWiFiInfo();

            RaisePropertyChanged(() => WifiViewItemSource);
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
            if (IsBattView)
                await BattViewQuantum.Instance.GetBATTView().ReadWiFiInfo();
            else
                await MCBQuantum.Instance.GetMCB().ReadWiFiInfo();
        }

        void DeviceLoadWiFiInfo()
        {
            WiFiDebug wifi;

            if (IsBattView)
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

        int WiFiInfoAccessApply()
        {
            accessControlUtility
                    .DoApplyAccessControl
                    (AccessLevelConsts.write, RssiItem, WifiViewItemSource);
            accessControlUtility
                .DoApplyAccessControl
                (AccessLevelConsts.write, AtVersionItem, WifiViewItemSource);
            accessControlUtility
                .DoApplyAccessControl
                (AccessLevelConsts.write, IpItem, WifiViewItemSource);
            accessControlUtility
                .DoApplyAccessControl
                (AccessLevelConsts.write, ConnectedToItem, WifiViewItemSource);
            accessControlUtility
                .DoApplyAccessControl
                (AccessLevelConsts.write, IsCommunicatingItem, WifiViewItemSource);
            accessControlUtility
                .DoApplyAccessControl
                (AccessLevelConsts.write, RestartStatusItem, WifiViewItemSource);
            accessControlUtility
                .DoApplyAccessControl
                (AccessLevelConsts.write, GatewayAddressItem, WifiViewItemSource);
            accessControlUtility
                .DoApplyAccessControl
                (AccessLevelConsts.write, MacAddressItem, WifiViewItemSource);

            ShowEdit &= accessControlUtility.GetSavedCount() != 0;

            return accessControlUtility.GetVisibleCount();
        }

        #endregion

        public void RefreshList()
        {
            if (IsBattView)
            {
                foreach (var item in WifiViewItemSource)
                {
                    if (item.EditableCellType == ACUtility.CellTypes.LabelSwitch)
                    {
                        item.SwitchValueChanged = SwitchValueChanged;
                        ExecuteSwitchValueChanged(item);
                        if (item.IsEditEnabled)
                        {
                            item.IsEditable = EditingMode;
                            item.Text = item.SubTitle;
                            item.IsSwitchEnabled = (item.SubTitle == AppResources.yes);

                        }
                    }

                    if (item.Index == 4 || item.Index == 6 || item.Index == 7 || item.Index == 8 || item.Index == 9 || item.Index == 10)
                    {

                    }
                    else
                    {
                        if (item.IsEditEnabled)
                        {
                            item.IsEditable = EditingMode;
                            item.Text = item.SubTitle;
                        }
                    }
                }
            }
            else
            {
                //charger editmode refresh list
                foreach (var item in WifiViewItemSource)
                {
                    if (item.EditableCellType == ACUtility.CellTypes.LabelSwitch)
                    {
                        item.SwitchValueChanged = SwitchValueChanged;
                        ExecuteSwitchValueChanged(item);
                        if (item.IsEditEnabled)
                        {
                            item.IsEditable = EditingMode;
                            item.Text = item.SubTitle;
                            item.IsSwitchEnabled = (item.SubTitle == AppResources.yes);

                        }
                    }

                    if (item.Index == 4 || item.Index == 6 || item.Index == 7 || item.Index == 8 || item.Index == 9 || item.Index == 10)
                    {

                    }
                    else
                    {
                        if (item.IsEditEnabled)
                        {
                            item.IsEditable = EditingMode;
                            item.Text = item.SubTitle;
                        }
                    }
                }
            }
        }

        public IMvxCommand EditBtnClickCommand
        {
            get { return new MvxCommand(OnEditClick); }
        }

        void OnEditClick()
        {
            EditingMode = true;
            RefreshList();
            RaisePropertyChanged(() => WifiViewItemSource);
        }

        public IMvxCommand BackBtnClickCommand
        {
            get { return new MvxCommand(OnBackClick); }
        }

        public IMvxCommand SaveBtnClickCommand
        {
            get
            {
                return new MvxCommand(async () =>
                {
                    await OnSaveClick();
                });
            }
        }

        async Task OnSaveClick()
        {
            if (!NetworkCheck())
                return;

            if (IsBattView)
            {
                if (Batt_VerfiyWiFiSettings())
                {
                    ACUserDialogs.ShowProgress();
                    BattViewCommunicationTypes caller = BattViewCommunicationTypes.NOCall;
                    bool arg1 = false;
                    try
                    {
                        Batt_SaveIntoWiFiSettings();
                        arg1 = false;
                        caller = BattViewCommunicationTypes.saveConfig;
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);

                    }

                    List<object> arguments = new List<object>
                        {
                            caller,
                            arg1
                        };
                    List<object> results = new List<object>();
                    try
                    {
                        results = await BattViewQuantum.Instance.CommunicateBATTView(arguments);
                        if (results.Count > 0)
                        {
                            var status = (CommunicationResult)results[2];
                            if (status == CommunicationResult.OK)
                            {
                                EditingMode = false;
                                RefreshList();

                                ResetOldData();
                                Batt_loadWiFiSettings();
                                RaisePropertyChanged(() => WifiViewItemSource);
                            }
                            else
                            {
                                ACUserDialogs.ShowAlertWithTitleAndOkButton(AppResources.save_failed);
                            }
                        }
                        else
                        {
                            ACUserDialogs.ShowAlertWithTitleAndOkButton(AppResources.save_failed);
                        }

                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }
                    ACUserDialogs.HideProgress();

                }
                else
                {
                    ACUserDialogs.ShowAlert(AppResources.alert_enter_valid + " " + verifyControl.GetErrorString());
                }
            }
            else
            {
                if (MCB_VerfiyWiFiSettings())
                {
                    ACUserDialogs.ShowProgress();
                    McbCommunicationTypes caller = McbCommunicationTypes.NOCall;
                    bool arg1 = false;
                    try
                    {
                        MCB_SaveIntoWiFiSettings();
                        caller = McbCommunicationTypes.saveConfig;
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);

                    }

                    List<object> arguments = new List<object>
                        {
                            caller,
                            arg1
                        };
                    List<object> results = new List<object>();
                    try
                    {
                        results = await MCBQuantum.Instance.CommunicateMCB(arguments);
                        if (results.Count > 0)
                        {
                            var status = (CommunicationResult)results[2];
                            if (status == CommunicationResult.OK)
                            {
                                EditingMode = false;
                                RefreshList();

                                ResetOldData();
                                MCB_loadWiFiSettings();
                                RaisePropertyChanged(() => WifiViewItemSource);
                            }
                            else
                            {
                                ACUserDialogs.ShowAlertWithTitleAndOkButton(AppResources.save_failed);
                            }
                        }
                        else
                        {
                            ACUserDialogs.ShowAlertWithTitleAndOkButton(AppResources.save_failed);
                        }

                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }
                    ACUserDialogs.HideProgress();

                }
                else
                {
                    ACUserDialogs.ShowAlert(AppResources.alert_enter_valid + " " + verifyControl.GetErrorString());
                }

            }
        }

        void ResetOldData()
        {
            foreach (var item in WifiViewItemSource)
            {
                if (item.IsEditable)
                    item.SubTitle = string.Empty;
            }
        }

        public IMvxCommand CancelBtnClickCommand
        {
            get { return new MvxCommand(OnBackClick); }
        }

        void OnBackClick()
        {
            if (CheckForEditedChanges())
            {
                ACUserDialogs.ShowAlertWithTwoButtons(AppResources.cancel_confirmation, "", AppResources.yes, AppResources.no, OnYesClick, null);
            }
            else
            {
                OnYesClick();
            }
        }

        void OnYesClick()
        {
            EditingMode = false;
            RefreshList();
            RaisePropertyChanged(() => WifiViewItemSource);
        }

        bool CheckForEditedChanges()
        {
            bool textChanged = false;
            foreach (var item in WifiViewItemSource)
            {
                textChanged |= item.Text != item.SubTitle;
            }

            return textChanged;
        }

        public IMvxCommand SwitchValueChanged
        {
            get
            {
                return new MvxCommand<ListViewItem>(ExecuteSwitchValueChanged);
            }
        }


        void ExecuteSwitchValueChanged(ListViewItem item)
        {
            if (item.EnableItemsList.Count > 0)
            {
                foreach (int editItemIndex in item.EnableItemsList)
                {
                    try
                    {
                        if (WifiViewItemSource.Contains(item))
                        {
                            ListViewItem editItem = WifiViewItemSource.FirstOrDefault(o => o.Index == editItemIndex);
                            if (editItem.IsEditEnabled)
                            {
                                editItem.IsEditable = item.IsSwitchEnabled && EditingMode;
                                editItem.Text = editItem.SubTitle;
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }
                }
                RaisePropertyChanged(() => WifiViewItemSource);
            }
        }

        public IMvxCommand RestoreBtnClickCommand
        {
            get { return new MvxCommand(LoadDefaultSettings); }
        }

        void LoadDefaultSettings()
        {
            if (IsBattView)
            {
                Batt_loadDefaultWifiSettings();
            }
            else
            {
                MCB_loadDefaultWIFI();
            }
        }

        void MCB_loadDefaultWIFI()
        {
            if (!MCBQuantum.Instance.MCB_quickSerialNumberCheckStrict())
                return;

            MobileAccessSsid.Text = "act24mobile";
            MobileAccessSsidPassword.Text = "shlonak5al";
            MobileViewPort.Text = "50000";
            SoftApEnabled.IsSwitchEnabled = false;
            SoftApPasswordTextBox.Text = "actDirmank";
            ConnectToActViewEnabled.IsSwitchEnabled = false;
            ActViewIp.Text = "act-view.com";
            ActViewPort.Text = "9309";
            ActViewConnectFrequency.Text = "60";
            ActAccessSsidPassword.Text = "hala3ami102";
            ActAccessSsid.Text = "actAccess24";
        }
        void Batt_loadDefaultWifiSettings()
        {
            if (!BattViewQuantum.Instance.BattView_quickSerialNumberCheckStrict())
                return;

            BattViewObject currentBattView = BattViewQuantum.Instance.GetBATTView();

            MobileAccessSsidPassword.Text = "shlonak5al";
            currentBattView.Config.mobileAccessSSIDpassword = MobileAccessSsidPassword.Text;
            MobileAccessSsid.Text = "act24mobile";
            currentBattView.Config.mobileAccessSSID = MobileAccessSsid.Text;


            MobileViewPort.Text = "50000";
            currentBattView.Config.mobilePort = ushort.Parse(MobileViewPort.Text);
            SoftApEnabled.IsSwitchEnabled = false;

            currentBattView.Config.IsSoftApEnable = SoftApEnabled.IsSwitchEnabled;
            SoftApPasswordTextBox.Text = "actDirmank";
            currentBattView.Config.softAPpassword = SoftApPasswordTextBox.Text;
            ConnectToActViewEnabled.IsSwitchEnabled = false;

            currentBattView.Config.ActViewEnabled = ConnectToActViewEnabled.IsSwitchEnabled;

            ActViewIp.Text = "act-view.com";
            currentBattView.Config.actViewIP = ActViewIp.Text;

            ActViewPort.Text = "9309";
            currentBattView.Config.actViewPort = ushort.Parse(ActViewPort.Text);


            ActViewConnectFrequency.Text = "900";
            currentBattView.Config.actViewConnectFrequency = ushort.Parse(ActViewConnectFrequency.Text);


            ActAccessSsidPassword.Text = "hala3ami102";
            currentBattView.Config.actAccessSSIDpassword = ActAccessSsidPassword.Text;

            ActAccessSsid.Text = "actAccess24";
            currentBattView.Config.actAccessSSID = ActAccessSsid.Text;

            RaisePropertyChanged(() => WifiViewItemSource);
        }

        public override void OnBackButtonClick()
        {
            base.OnBackButtonClick();

            ShowViewModel<WiFiViewModel>(new { pop = "pop" });
        }
    }
}
