using System.Collections.Generic;

namespace actchargers
{
    public static class ACConstants
    {
        public enum APIs
        {
            Login,
            MCB_commission,
            uploadBattViewEvents,
            uploadMCBCycles,
            uploadMCBfaults,
            uploadBattViewDeviceObject,
            uploadMCBDeviceObject,
            getQuantumVersions,
            downloadQuantum,
            userExit,
            getusersList,
            Batt_commission,
            getUserSites,
            downloadMCBConfig,
            downloadBattViewConfig,
            replaceMCBDevice,
            replaceBattViewDevice,
            uploadMCBDirectObject,
            uploadBattObjectDirect,
            uploadFailedDB,
        }




        public static ConnectionTypesEnum ConnectionType;
        public static readonly Dictionary<APIs, string> APIsURI = new Dictionary<APIs, string>()
        {
            {APIs.Login,"/quantumSoftware/Login/"},
            {APIs.MCB_commission,"/quantumSoftware/commissionMCB/"},
            {APIs.uploadBattViewEvents,"/quantumSoftware/uploadBattEvents2/"},
            {APIs.uploadMCBCycles,"/quantumSoftware/uploadMCBCycles2/" },
            {APIs.uploadMCBfaults,"/quantumSoftware/uploadMCBFaults2/" },
            {APIs.uploadBattViewDeviceObject,"/quantumSoftware/uploadBattObject2/"},
            {APIs.uploadMCBDeviceObject,"/quantumSoftware/uploadMCBObject2/"},
            {APIs.getQuantumVersions,"/quantumSoftware/getQuantumVersions2/"},
            {APIs.downloadQuantum,"/quantumSoftware/downloadQuantum/"},
            {APIs.userExit, "/quantumSoftware/AmIvalid/"},
            {APIs.getusersList,"/quantumSoftware/getUsersTypeList/"},
            {APIs.Batt_commission,"/quantumSoftware/commissionBattView/"},
            {APIs.getUserSites ,"/quantumSoftware/downloadUsersLeftSideMenu" },
            {APIs.downloadMCBConfig ,"/quantumSoftware/downloadMCBConfig" },
            {APIs.downloadBattViewConfig ,"/quantumSoftware/downloadBattviewConfig" },
            {APIs.replaceMCBDevice ,"/quantumSoftware/replaceMCBDevice" },
            {APIs.replaceBattViewDevice ,"/quantumSoftware/replaceBattViewDevice" },
            {APIs.uploadMCBDirectObject,"/quantumSoftware/uploadMCBDirectObject" },
            {APIs.uploadBattObjectDirect, "/quantumSoftware/uploadBattObjectDirect" },
            {APIs.uploadFailedDB, "/quantumSoftware/uploadFailedDB" },
        };

        public const string DATABASE_FILE_NAME = "ACSQLite.db3";
        public const string DATABASE_BACKUP_FILE_NAME = "DatabaseBackup.json";

        public const string MCB = "MCB";
        public const string BATTVIEW = "battView";

        public const string SN = "SN";
        public const string INTERFACE_MCB_PREFIX = "CHRG";
        public const string INTERFACE_BATTVIEW_PREFIX = "BATT";

        public const string BEARER = "Bearer ";

        public const string ACCEPT = "Accept";
        public const int SNACKBAR_TIMER = 3000;
        public const string ACCEPT_PARAMETER = " */*";

        public const string USERAGENT = "User-Agent";
        public const string APPLICATION_JSON = "application/json";

        public const string PRODUCTION_BASE_URL = "https://act-view.com";
        public const string DEVELOPMENT_BASE_URL = "http://act-view.org";

        public static bool ENABLE_UNTRUSTED_CERTICATE = true;

        public const string PREF_IS_LOGIN = @"IsLoggedIn";

        public static bool SHOW_SITEVIEW = true;

        // User Settings
        public const string USER_PREFS_FILE = "AndroidUserPreferences";

        //UserPreferences strings
        public const string USER_PREFS_USER_ID = @"USERID";
        public const string USER_PREFS_USER_LOGGED_IN = @"USERLOGGEDIN";
        public const string USER_PREFS_ALLOW_EDITING = @"USER_PREFS_ALLOW_EDITING";

        public const string PREF_LOGIN_RESPONSE = @"LoginResponse";
        public const string PREF_USERNAME = @"username";
        public const string PREF_PASSWORD = @"password";

        public const string PREF_UPDATE_ALERT_DATE = "PREF_ALERT_DATE";

        //Database Keys
        public const string DB_LOGIN_DATE = @"loginDate";

        public const string ACT_24_MOBILE_LOCAL = "act24mobileLocal";
        public const string ACT_ACCESS_24 = "actAccess24";
        public const string ACT_IOS_CERTIFICATE_NAME = "actCertificate";

        public static readonly string[] DEVICES_TABS_TITLES =
        {
            AppResources.charger,
            AppResources.battview,
            AppResources.batt_mobile
        };

        // Delta time interval - 6 hour in milli seconds
        //public const int REFRESH_TIME_INTERVAL = 6 * 60 * 60 * 1000;

        public const int SENDING_DELAY = 1250;

        // Check for every 10 seconds
        public const int REFRESH_TIME_INTERVAL = 1000;
        public const int DEVICE_SCAN_INTERVAL = 20 * 1000;
        public const int DEVICELIST_REFRESH_INTERVAL = 1000;
        public const int PROGRESS_REFRESH_INTERVAL = 500;
        public const int QUICKVIEW_REFRESH_INTERVAL = 5000;
        public const int SCAN_INTERVAL = 500;
        public const int PING_ASYNC_TIMEOUT = 500;
        public const int PERIODIC_PING_TIMEOUT = 5000;
        public const int PERIODIC_PING_INTERVAL = 30000;
        public const int DOWNLOAD_SCREEN_TIMER = 500;
        public static bool IS_LOGGING_REQUIRED;
        //Goolge Analytics ID

        /// <summary>
        /// Make this variable true if the Demo data need to display, for RealTime Data assign false
        /// </summary>
        public static bool IS_STATIC_DATA_REQUIRED;
        public static bool IS_DEBUGGING;
        public const string GoogleAnalyticsTrackingId = "UA-90543277-1";//This is previous ID "UA-89861321-1"
        public static readonly string DATE_TIME_FORMAT = "dd-MM-yyyy hh:mm:ss tt";
        public static readonly string DATE_TIME_FORMAT_IOS = "dd-MM-yyyy hh:mm:ss a";
        public static readonly string DATE_TIME_FORMAT_IOS_UI = "MMMM dd, yyyy";

        public static string WIFI_SSID;
        public static string WIFI_PASSWORD;
        public static string gatewayAddr_Andoid;
        public static string USBConnectedSerialNumber;
        public static bool IsUSBBattView = true;
        public static downloadDevicesxStats all_downloadStat = new downloadDevicesxStats();

        public const int NO_OF_DAYS_APP_TIMEOUT = -14;

        public enum APIExecutionMode
        {
            Unique,
            Distinct,
            Multiple
        }

        public enum APIRequestPriority
        {
            Immediate,
            Urgent,
            Normal,
            Low,
            Trivial
        }

        public enum MessageBoxFormTypes
        {
            Warning = 0,
            error,
            info,
            question,
        }
        public enum MessageBoxFormButtons
        {
            OK = 0,
            OkCancel,
            YesNo,
        }

        public enum DEVICE_COMMISSION_ACTIONS_LIST
        {
            nothing = 0,
            all,
            onlyCustomers,
            onlyOEMsAndDealers,
            getSites,
        }
    }

}
