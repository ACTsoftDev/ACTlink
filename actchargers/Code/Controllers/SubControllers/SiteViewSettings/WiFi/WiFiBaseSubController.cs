using System.Collections.Generic;

namespace actchargers
{
    public abstract class WiFiBaseSubController : SiteViewSettingsBaseSubController
    {
        const int ACCESS_PASSWORD_MAX_LENGTH_V_1 = 13;
        const int ACCESS_PASSWORD_MAX_LENGTH_V_2 = 31;

        protected VerifyControl verifyControl;

        protected ListViewItem MobileAccessSsid;
        protected ListViewItem MobileAccessSsidPassword;
        protected ListViewItem MobileViewPort;
        protected ListViewItem SoftApEnabled;
        protected ListViewItem SoftApPasswordTextBox;
        protected ListViewItem ConnectToActViewEnabled;
        protected ListViewItem ActViewIp;
        protected ListViewItem ActViewPort;
        protected ListViewItem ActViewConnectFrequency;
        protected ListViewItem ActAccessSsid;
        protected ListViewItem ActAccessSsidPassword;

        protected WiFiBaseSubController(bool isBattView, bool isSiteView) : base(isBattView, isSiteView)
        {
        }

        #region Shared Items

        internal override void InitSharedBattViewItems()
        {
            InitCrossDeviceItems();
        }

        internal override void InitSharedMcbItems()
        {
            InitCrossDeviceItems();
        }

        void InitCrossDeviceItems()
        {
            MobileAccessSsid = new ListViewItem()
            {
                Index = 0,
                Title = AppResources.mobile_ssid,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.LabelTextEdit,
                TextMinLength = 8,
                TextMaxLength = 32
            };
            MobileAccessSsidPassword = new ListViewItem()
            {
                Index = 1,
                Title = AppResources.mobile_ssid_password,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.LabelTextEdit,
                TextMinLength = 8,
                TextMaxLength = 32
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
                EnableItemsList = new List<int> { 4 }
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
                EnableItemsList = new List<int> { 6, 7, 8, 9, 10 }
            };
            ActViewIp = new ListViewItem()
            {
                Index = 6,
                Title = AppResources.actview_ip,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.LabelTextEdit,
                TextMinLength = 7,
                TextMaxLength = 64
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
                TextMinLength = 1,
                TextMaxLength = ACCESS_PASSWORD_MAX_LENGTH_V_1
            };
        }

        #endregion

        #region Add BattView

        internal override int BattViewAccessApply()
        {
            int actViewEnabledAccessLevel = GetActViewEnabledAccessLevel();

            accessControlUtility = new UIAccessControlUtility();

            accessControlUtility
                .DoApplyAccessControl
                (actViewEnabledAccessLevel, ConnectToActViewEnabled, ItemSource);

            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_softAPEnable, SoftApEnabled, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_softAPpassword, SoftApPasswordTextBox, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_mobileAccessSSID, MobileAccessSsid, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_mobileAccessSSIDpassword, MobileAccessSsidPassword, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_mobilePort, MobileViewPort, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_actViewIP, ActViewIp, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_actViewConnectFrequency, ActViewConnectFrequency, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_actViewPort, ActViewPort, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_actAccessSSID, ActAccessSsid, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_actAccessSSIDpassword, ActAccessSsidPassword, ItemSource);

            ShowEdit &= accessControlUtility.GetSavedCount() != 0;

            return accessControlUtility.GetVisibleCount();
        }

        internal abstract int GetActViewEnabledAccessLevel();

        #endregion

        #region Add MCB

        internal override int McbAccessApply()
        {
            accessControlUtility = new UIAccessControlUtility();

            accessControlUtility
                .DoApplyAccessControl
                (ControlObject.UserAccess.MCB_actViewEnabled,
                 ConnectToActViewEnabled, ItemSource);

            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_softAPEnable, SoftApEnabled, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_softAPpassword, SoftApPasswordTextBox, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_mobileAccessSSID, MobileAccessSsid, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_mobileAccessSSIDpassword, MobileAccessSsidPassword, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_mobilePort, MobileViewPort, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_actViewIP, ActViewIp, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_actViewConnectFrequency, ActViewConnectFrequency, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_actViewPort, ActViewPort, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_actAccessSSID, ActAccessSsid, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_actAccessSSIDpassword, ActAccessSsidPassword, ItemSource);

            ShowEdit &= accessControlUtility.GetSavedCount() != 0;

            return accessControlUtility.GetVisibleCount();
        }

        #endregion

        protected void SetAccessSsidPasswordMaxLengthBasedOnVersion(int version)
        {
            int length = version == 1 ? ACCESS_PASSWORD_MAX_LENGTH_V_1 : ACCESS_PASSWORD_MAX_LENGTH_V_2;

            ActAccessSsidPassword.TextMaxLength = length;
        }

        protected bool CheckDefaults()
        {
            if (MustStop())
                return false;

            return (MobileAccessSsid.Text != "act24mobile") ||
                (MobileAccessSsidPassword.Text != "shlonak5al") ||
                (MobileViewPort.Text != "50000") ||
                (!SoftApEnabled.IsSwitchEnabled) ||
                (SoftApPasswordTextBox.Text != "actDirmank") ||
                (!ConnectToActViewEnabled.IsSwitchEnabled) ||
                (ActViewIp.Text != "52.11.167.54") ||
                (ActViewPort.Text != "9309") ||
                (ActViewConnectFrequency.Text != "900") ||
                (ActAccessSsidPassword.Text != "hala3ami102") ||
                (ActAccessSsid.Text != "actAccess24");
        }

        public override void LoadDefaults()
        {
            MobileAccessSsid.SubTitle = MobileAccessSsid.Text = "act24mobile";
            MobileAccessSsidPassword.SubTitle = MobileAccessSsidPassword.Text = "shlonak5al";
            MobileViewPort.SubTitle = MobileViewPort.Text = "50000";
            SoftApEnabled.IsSwitchEnabled = false;
            SoftApPasswordTextBox.SubTitle = SoftApPasswordTextBox.Text = "actDirmank";
            ConnectToActViewEnabled.IsSwitchEnabled = false;
            ActViewIp.SubTitle = ActViewIp.Text = "52.11.167.54";
            ActViewPort.SubTitle = ActViewPort.Text = "9309";
            ActViewConnectFrequency.SubTitle = ActViewConnectFrequency.Text = "60";
            ActAccessSsidPassword.SubTitle = ActAccessSsidPassword.Text = "hala3ami102";
            ActAccessSsid.SubTitle = ActAccessSsid.Text = "actAccess24";
        }

        internal override VerifyControl VerfiyBattViewSettings()
        {
            return VerfiySettings();
        }

        internal override VerifyControl VerfiyMcbSettings()
        {
            return VerfiySettings();
        }

        internal VerifyControl VerfiySettings()
        {
            verifyControl = new VerifyControl();

            MobileAccessSsid.Text = StaticDataAndHelperFunctions.cleanWifiChars(MobileAccessSsid.Text);
            MobileAccessSsidPassword.Text = StaticDataAndHelperFunctions.cleanWifiChars(MobileAccessSsidPassword.Text);
            SoftApPasswordTextBox.Text = StaticDataAndHelperFunctions.cleanWifiChars(SoftApPasswordTextBox.Text);
            ActAccessSsidPassword.Text = StaticDataAndHelperFunctions.cleanWifiChars(ActAccessSsidPassword.Text);
            ActAccessSsid.Text = StaticDataAndHelperFunctions.cleanWifiChars(ActAccessSsid.Text);

            verifyControl.VerifyTextBox(MobileAccessSsidPassword, MobileAccessSsidPassword, 8, 32);
            verifyControl.VerifyTextBox(MobileAccessSsid, MobileAccessSsid, 8, 32);
            verifyControl.VerifyInteger(MobileViewPort, MobileViewPort, 49152, 65535);

            verifyControl.VerifyTextBox(SoftApPasswordTextBox, SoftApPasswordTextBox, 8, 13);

            verifyControl
                .VerifyTextBox
                (ActViewIp, ActViewIp,
                 ActViewIp.TextMinLength, ActViewIp.TextMaxLength);

            verifyControl.VerifyInteger(ActViewPort, ActViewPort, 1024, 65535);
            verifyControl.VerifyInteger(ActViewConnectFrequency, ActViewConnectFrequency, 1, 65535);

            verifyControl
                .VerifyTextBox
                (ActAccessSsidPassword, ActAccessSsidPassword,
                 ActAccessSsidPassword.TextMinLength, ActAccessSsidPassword.TextMaxLength);

            verifyControl
                .VerifyTextBox
                (ActAccessSsid, ActAccessSsid,
                 ActAccessSsid.TextMinLength, ActAccessSsid.TextMaxLength);

            return verifyControl;
        }

        internal override void SaveBattViewToConfigObject(BattViewObject device)
        {
            var config = device.Config;

            config.mobileAccessSSID = MobileAccessSsid.Text;
            config.mobileAccessSSIDpassword = MobileAccessSsidPassword.Text;
            config.mobilePort = ushort.Parse(MobileViewPort.Text);
            config.IsSoftApEnable = SoftApEnabled.IsSwitchEnabled;
            config.softAPpassword = SoftApPasswordTextBox.Text;
            config.ActViewEnabled = ConnectToActViewEnabled.IsSwitchEnabled;
            config.actViewIP = ActViewIp.Text;
            config.actViewPort = ushort.Parse(ActViewPort.Text);
            config.actViewConnectFrequency = ushort.Parse(ActViewConnectFrequency.Text);
            config.actAccessSSIDpassword = ActAccessSsidPassword.Text;
            config.actAccessSSID = ActAccessSsid.Text;
        }

        internal override void SaveMcbToConfigObject(MCBobject device)
        {
            var config = device.Config;

            config.mobileAccessSSID = MobileAccessSsid.Text;
            config.mobileAccessPassword = MobileAccessSsidPassword.Text;
            config.mobilePort = MobileViewPort.Text;
            config.softAPenabled = SoftApEnabled.IsSwitchEnabled;
            config.softAPpassword = SoftApPasswordTextBox.Text;
            config.actViewEnable = ConnectToActViewEnabled.IsSwitchEnabled;
            config.actViewIP = ActViewIp.Text;
            config.actViewPort = ActViewPort.Text;
            config.actViewConnectFrequency = ActViewConnectFrequency.Text;
            config.actAccessPassword = ActAccessSsidPassword.Text;
            config.actAccessSSID = ActAccessSsid.Text;
        }

        bool MustStop()
        {
            if (isBattView)
            {
                return !BattViewQuantum.Instance.BattView_quickSerialNumberCheckStrict();
            }

            return !MCBQuantum.Instance.MCB_quickSerialNumberCheckStrict();
        }
    }
}
