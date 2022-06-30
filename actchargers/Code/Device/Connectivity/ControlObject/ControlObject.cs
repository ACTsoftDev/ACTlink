using System;
using System.Diagnostics;
using actchargers.Code.Utility;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace actchargers
{
    public static class ControlObject
    {
        static readonly uint[] ACT_OEM_ACCOUNTS = { 688, 689, 690, 691, 971, 972, 45 };

        //[System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int GetSystemMetrics(int nIndex);

        static int _numberOfThreads = 32;
        public static int numberOfThreads
        {
            get
            {
                lock (myLock)
                {
                    return _numberOfThreads;
                }
            }
            set
            {
                lock (myLock)
                {
                    _numberOfThreads = value;
                }
            }
        }

        static double _CPU_POWER = 1;
        public static double cpuPowerFactor
        {
            get
            {
                lock (myLock)
                {
                    return _CPU_POWER;
                }
            }
            set
            {
                lock (myLock)
                {
                    _CPU_POWER = value;
                }
            }
        }

        static UInt32 _userID = 0;
        static object myLock = new object();
        public static UInt32 userID
        {
            get
            {
                lock (myLock)
                {
                    return _userID;
                }
            }
            set
            {
                lock (myLock)
                {
                    _userID = value;
                }
            }
        }
        static bool _isDebugMaster = false;
        public static bool isDebugMaster
        {
            get
            {
                lock (myLock)
                {
                    return _isDebugMaster;
                }
            }
            set
            {
                lock (myLock)
                {
                    _isDebugMaster = value;
                }
            }
        }


        static bool _isHWMnafacturer = false;
        public static bool isHWMnafacturer
        {
            get
            {
                lock (myLock)
                {
                    return _isHWMnafacturer;
                }
            }
            set
            {
                lock (myLock)
                {
                    _isHWMnafacturer = value;
                }
            }
        }
        static bool _isACTOem = false;
        public static bool isACTOem
        {
            get
            {
                lock (myLock)
                {
                    return _isACTOem;
                }
            }
            set
            {
                lock (myLock)
                {
                    _isACTOem = value;
                }
            }
        }
        static bool _internetConnectionDetected = true;
        public static bool internetConnectionDetected
        {
            get
            {
                lock (myLock)
                {
                    return _internetConnectionDetected;
                }
            }
            set
            {
                lock (myLock)
                {
                    _internetConnectionDetected = value;
                }
            }
        }
        static bool _justALogOut = true;
        public static bool justALogOut
        {
            get
            {
                lock (myLock)
                {
                    return _justALogOut;
                }
            }
            set
            {
                lock (myLock)
                {
                    _justALogOut = value;
                }
            }
        }
        static bool _justaConnectvityType = true;
        public static bool justaConnectvityType
        {
            get
            {
                lock (myLock)
                {
                    return _justaConnectvityType;
                }
            }
            set
            {
                lock (myLock)
                {
                    _justaConnectvityType = value;
                }
            }
        }


        static bool _goodByeMessageForm = false;
        public static bool goodByeMessageForm
        {
            get
            {
                lock (myLock)
                {
                    return _goodByeMessageForm;
                }
            }
            set
            {
                lock (myLock)
                {
                    _goodByeMessageForm = value;
                }
            }
        }

        static string _goodByeMessageFormString = "";
        public static string goodByeMessageFormString
        {
            get
            {
                lock (myLock)
                {
                    return _goodByeMessageFormString;
                }
            }
            set
            {
                lock (myLock)
                {
                    _goodByeMessageFormString = value;
                }
            }
        }
        static bool _AccessMCB = true;
        public static bool AccessMCB
        {
            get
            {
                lock (myLock)
                {
                    return _AccessMCB;
                }
            }
            set
            {
                lock (myLock)
                {
                    _AccessMCB = value;
                }
            }
        }
        static bool _filterMCB = true;
        public static bool filterMCBIn
        {
            get
            {
                lock (myLock)
                {
                    return _filterMCB;
                }
            }
            set
            {
                lock (myLock)
                {
                    _filterMCB = value;
                }
            }
        }

        static bool _lazyLoading = true;
        public static bool lazyLoading
        {
            get
            {
                lock (myLock)
                {
                    return _lazyLoading;
                }
            }
            set
            {
                lock (myLock)
                {
                    _lazyLoading = value;
                }
            }
        }
        static bool _filterBattView = true;
        public static bool filterBattViewIn
        {
            get
            {
                lock (myLock)
                {
                    return _filterBattView;
                }
            }
            set
            {
                lock (myLock)
                {
                    _filterBattView = value;
                }
            }
        }
        static bool _AccessBattView = true;
        public static bool AccessBattView
        {
            get
            {
                lock (myLock)
                {
                    return _AccessBattView;
                }
            }
            set
            {
                lock (myLock)
                {
                    _AccessBattView = value;
                }
            }
        }
        public const int maxBattViewSynchDays = 180;

        static string _name = "";
        public static string name
        {
            get
            {
                lock (myLock)
                {
                    return _name;
                }
            }
            set
            {
                lock (myLock)
                {
                    _name = value;
                }
            }
        }
        static UInt32 _userType = 255;
        public static UInt32 userType
        {
            get
            {
                lock (myLock)
                {
                    return _userType;
                }
            }
            set
            {
                lock (myLock)
                {
                    _userType = value;
                }
            }
        }

        public static ConnectionTypes connectMethods = new ConnectionTypes();
        public static bool isSynchSitesForm;

        static AccessLevel userAccess = new AccessLevel();
        public static AccessLevel UserAccess
        {
            get
            {
                return userAccess;
            }
        }

        public static ValuesLimits FormLimits = new ValuesLimits(false);
        static int parseAccess(dynamic val)
        {
            try
            {
                lock (myLock)
                {
                    if (val.ToString() == "write")
                        return AccessLevelConsts.write;
                    if (val.ToString() == "readOnly")
                        return AccessLevelConsts.readOnly;
                    return AccessLevelConsts.noAccess;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return AccessLevelConsts.noAccess;
            }

        }

        public static void LoadCached()
        {
            try
            {
                TryLoadCached();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        static void TryLoadCached()
        {
            string cached = AppUserPreferences.GetString(ACConstants.PREF_LOGIN_RESPONSE);

            if (cached == null)
                return;

            LoadAccess(cached);
        }

        static void LoadAccess(string source)
        {
            var responseJObject = (JObject)JsonParser.DeserializeObject<object>(source);

            LoadAccess(responseJObject);
        }

        static void LoadAccess(JObject access)
        {
            lock (myLock)
            {
                //Get User ID and UserType
                if (access["userID"] == null || access["name"] == null ||
                    access["roleid"] == null || access["AccessObject"] == null ||
                    access["userID"].Type != JTokenType.Integer ||
                    access["name"].Type != JTokenType.String ||
                    access["roleid"].Type != JTokenType.Integer ||
                    access["AccessObject"].Type != JTokenType.Object)

                    return;

                userID = (UInt32)access["userID"];
                isDebugMaster = userID == 18 || userID == 360;
                isHWMnafacturer = userID == 39 || userID == 46;

                isACTOem = Array.Exists(ACT_OEM_ACCOUNTS, item => item == userID);

                userType = (UInt32)access["roleid"];
                name = (string)access["name"];
                var access_object = (JObject)access["AccessObject"];

                UserAccess.AdminRestart = parseAccess(access_object["AdminRestart"]);
                UserAccess.CanReadNonCommissioned = parseAccess(access_object["CanReadNonCommissioned"]);


                //ACCESS Charger Info
                UserAccess.MCB_SN = parseAccess(access_object["MCB_SN"]);
                UserAccess.MCB_Model = parseAccess(access_object["MCB_Model"]);
                UserAccess.MCB_HWRevision = parseAccess(access_object["MCB_HWRevision"]);
                UserAccess.MCB_UserNamedID = parseAccess(access_object["MCB_UserNamedID"]);
                UserAccess.MCB_InstallationDate = parseAccess(access_object["MCB_InstallationDate"]);
                UserAccess.MCB_chargerType = parseAccess(access_object["MCB_chargerType"]);
                UserAccess.MCB_TimeZone = parseAccess(access_object["MCB_TimeZone"]);
                UserAccess.MCB_setup_version = parseAccess(access_object["MCB_setup_version"]);
                UserAccess.MCB_readFrimWareVersion = parseAccess(access_object["MCB_readFrimWareVersion"]);
                UserAccess.MCB_memorySignature = parseAccess(access_object["MCB_memorySignature"]);
                UserAccess.MCB_EditOriginalSerialNumber = parseAccess(access_object["MCB_EditOriginalSerialNumber"]);
                UserAccess.MCB_EnableLED = parseAccess(access_object["MCB_EnableLED"]);
                //ACCESS Charger Settings
                UserAccess.MCB_IR = parseAccess(access_object["MCB_IR"]);
                UserAccess.MCB_autoStart_count = parseAccess(access_object["MCB_autoStart_count"]);
                UserAccess.MCB_autoStart_Enable = parseAccess(access_object["MCB_autoStart_Enable"]);
                UserAccess.MCB_autoSatrt_DaysMask = parseAccess(access_object["MCB_autoSatrt_DaysMask"]);
                UserAccess.MCB_refreshEnable = parseAccess(access_object["MCB_refreshEnable"]);
                UserAccess.MCB_refreshTimer = parseAccess(access_object["MCB_refreshTimer"]);
                UserAccess.MCB_TemperatureFormat = parseAccess(access_object["MCB_TemperatureFormat"]);
                UserAccess.MCB_TemperatureSensorEnable = parseAccess(access_object["MCB_TemperatureSensorEnable"]);
                UserAccess.MCB_multiVoltage = parseAccess(access_object["MCB_multiVoltage"]);
                UserAccess.MCB_dayLightSaving_Enable = parseAccess(access_object["MCB_dayLightSaving_Enable"]);
                UserAccess.MCB_enablePLC = parseAccess(access_object["MCB_enablePLC"]);
                UserAccess.MCB_HWversionControl = parseAccess(access_object["MCB_HWversionControl"]);
                UserAccess.MCB_BMS_FW_CHANGE = parseAccess(access_object["MCB_BMS_FW_CHANGE"]);
                UserAccess.MCB_enableLauncher = parseAccess(access_object["MCB_enableLauncher"]);
                UserAccess.MCB_DisablePushButton = parseAccess(access_object["MCB_DisablePushButton"]);
                UserAccess.MCB_DaughterCardEnabled = parseAccess(access_object["MCB_daughtercardenabled"]);
                UserAccess.PMMaxCurrent = parseAccess(access_object["MCB_pmmaxcurrent"]);
                UserAccess.MCB_defualtBrughtness = parseAccess(access_object["MCB_defaultBrightness"]);

                //WIFI
                UserAccess.MCB_actViewEnabled = parseAccess(access_object["MCB_actViewEnabled"]);
                UserAccess.MCB_softAPEnable = parseAccess(access_object["MCB_softAPEnable"]);
                UserAccess.MCB_softAPpassword = parseAccess(access_object["MCB_softAPpassword"]);
                UserAccess.MCB_mobileAccessSSID = parseAccess(access_object["MCB_mobileAccessSSID"]);
                UserAccess.MCB_mobileAccessSSIDpassword = parseAccess(access_object["MCB_mobileAccessSSIDpassword"]);
                UserAccess.MCB_mobilePort = parseAccess(access_object["MCB_mobilePort"]);
                UserAccess.MCB_actViewIP = parseAccess(access_object["MCB_actViewIP"]);
                UserAccess.MCB_actViewPort = parseAccess(access_object["MCB_actViewPort"]);
                UserAccess.MCB_actViewConnectFrequency = parseAccess(access_object["MCB_actViewConnectFrequency"]);
                UserAccess.MCB_actAccessSSID = parseAccess(access_object["MCB_actAccessSSID"]);
                UserAccess.MCB_actAccessSSIDpassword = parseAccess(access_object["MCB_actAccessSSIDpassword"]);
                //Battery Settings
                UserAccess.MCB_BatteryCapacity = parseAccess(access_object["MCB_BatteryCapacity"]);
                UserAccess.MCB_BatteryVoltage = parseAccess(access_object["MCB_BatteryVoltage"]);
                UserAccess.MCB_temperatureCompensation = parseAccess(access_object["MCB_temperatureCompensation"]);
                UserAccess.MCB_TempertureHigh = parseAccess(access_object["MCB_TempertureHigh"]);
                UserAccess.MCB_BatteryType = parseAccess(access_object["MCB_BatteryType"]);
                //MCB_RunBattViewCal
                UserAccess.MCB_RunBattViewCal = parseAccess(access_object["MCB_RunBattViewCal"]);

                //Charge Profile
                UserAccess.MCB_TR_CurrentRate = parseAccess(access_object["MCB_TR_CurrentRate"]);
                UserAccess.MCB_CC_CurrentRate = parseAccess(access_object["MCB_CC_CurrentRate"]);
                UserAccess.MCB_FI_CurrentRate = parseAccess(access_object["MCB_FI_CurrentRate"]);
                UserAccess.MCB_EQ_CurrentRate = parseAccess(access_object["MCB_EQ_CurrentRate"]);
                UserAccess.MCB_TrickleVoltage = parseAccess(access_object["MCB_TrickleVoltage"]);
                UserAccess.MCB_CVVoltage = parseAccess(access_object["MCB_CVVoltage"]);
                UserAccess.MCB_finishVoltage = parseAccess(access_object["MCB_finishVoltage"]);
                UserAccess.MCB_EqualaizeVoltage = parseAccess(access_object["MCB_EqualaizeVoltage"]);
                UserAccess.MCB_cvCurrentStep = parseAccess(access_object["MCB_cvCurrentStep"]);
                UserAccess.MCB_cvFinishCurrent = parseAccess(access_object["MCB_cvFinishCurrent"]);
                UserAccess.MCB_CVMaxTimer = parseAccess(access_object["MCB_CVMaxTimer"]);
                UserAccess.MCB_finishTimer = parseAccess(access_object["MCB_finishTimer"]);
                UserAccess.MCB_EqualizeTimer = parseAccess(access_object["MCB_EqualizeTimer"]);
                UserAccess.MCB_desulfationTimer = parseAccess(access_object["MCB_desulfationTimer"]);
                UserAccess.MCB_finishdVdT = parseAccess(access_object["MCB_finishdVdT"]);

                //FI and EQ scheduling
                UserAccess.MCB_FI_sched_Settings = parseAccess(access_object["MCB_FI_sched_Settings"]);
                UserAccess.MCB_FI_sched_CustomSettings = parseAccess(access_object["MCB_FI_sched_CustomSettings"]);
                UserAccess.MCB_EQ_sched_CustomSettings = parseAccess(access_object["MCB_EQ_sched_CustomSettings"]);
                UserAccess.MCB_FI_EQ_sched_CustomSettings = parseAccess(access_object["MCB_FI_EQ_sched_CustomSettings"]);
                UserAccess.MCB_ignorebattviewsoc = parseAccess(access_object["MCB_ignorebattviewsoc"]);

                //Global Records
                UserAccess.MCB_ViewGlobalRecords = parseAccess(access_object["MCB_ViewGlobalRecords"]);
                UserAccess.MCB_ViewTotalPMFaults = parseAccess(access_object["MCB_ViewTotalPMFaults"]);
                UserAccess.MCB_canResetGlobalRecords = parseAccess(access_object["MCB_canResetGlobalRecords"]);

                //Cycles History
                UserAccess.MCB_canReadCyclesHistory = parseAccess(access_object["MCB_canReadCyclesHistory"]);
                //PM 
                UserAccess.MCB_PM_effieciency = parseAccess(access_object["MCB_PM_effieciency"]);
                UserAccess.MCB_numberOfInstalledPMs = parseAccess(access_object["MCB_numberOfInstalledPMs"]);
                UserAccess.MCB_PM_Voltage = parseAccess(access_object["MCB_PM_Voltage"]);
                UserAccess.MCB_PM_canReadFaults = parseAccess(access_object["MCB_PM_canReadFaults"]);
                UserAccess.MCB_PM_LiveView = parseAccess(access_object["MCB_PM_LiveView"]);

                //Calibration

                UserAccess.MCB_Calibration = parseAccess(access_object["MCB_Calibration"]);
                UserAccess.MCB_view_Raw_values = parseAccess(access_object["MCB_view_Raw_values"]);
                UserAccess.MCB_ViRdiv = parseAccess(access_object["MCB_ViRdiv"]);
                UserAccess.MCB_Steinhart = parseAccess(access_object["MCB_Steinhart"]);
                UserAccess.MCB_Calibration_manual = parseAccess(access_object["MCB_Calibration_manual"]);

                //Firmware Update
                UserAccess.MCB_FirmwareUpdate = parseAccess(access_object["MCB_FirmwareUpdate"]);

                UserAccess.MCB_manualFirmwareUpdate = parseAccess(access_object["MCB_manualFirmwareUpdate"]);
                UserAccess.MCB_FirmwareRequestUpdateDebug = parseAccess(access_object["MCB_FirmwareRequestUpdateDebug"]);

                //Admin
                //   user_access.MCB_AdminLCD = parseAccess(access_object["MCB_AdminLCD"]);
                UserAccess.MCB_ResetLCDCalibration = parseAccess(access_object["MCB_ResetLCDCalibration"]);
                UserAccess.MCB_AdminSimulation = parseAccess(access_object["MCB_AdminSimulation"]);
                UserAccess.MCB_AdminACTViewID = parseAccess(access_object["MCB_AdminACTViewID"]);
                UserAccess.MCB_AdminPMSimulation = parseAccess(access_object["MCB_AdminPMSimulation"]);
                UserAccess.MCB_onlyForEnginneringTeam = parseAccess(access_object["MCB_onlyForEnginneringTeam"]);
                UserAccess.MCB_COMMISSION = parseAccess(access_object["MCB_COMMISSION"]);

                UserAccess.MCB_EnergyManagment = parseAccess(access_object["MCB_EnergyManagment"]);

                if (UserAccess.MCB_onlyForEnginneringTeam == AccessLevelConsts.write)
                {
                    FormLimits = null;
                    FormLimits = new ValuesLimits(true);
                }

                //Connect Methods
                connectMethods.usb = false;
                connectMethods.debug = false;
                connectMethods.mobile = false;
                connectMethods.direct = false;
                if (parseAccess(access_object["USB"]) == AccessLevelConsts.write)
                {
                    connectMethods.usb = true;
                }
                if (parseAccess(access_object["Direct_Mode"]) == AccessLevelConsts.write)
                {
                    connectMethods.direct = true;
                }
                if (parseAccess(access_object["Master_debug_Mode"]) == AccessLevelConsts.write)
                {
                    connectMethods.debug = true;
                }
                if (parseAccess(access_object["Mobile_Mode"]) == AccessLevelConsts.write)
                {
                    connectMethods.mobile = true;
                }
                if (parseAccess(access_object["Mobile_Router_Mode"]) == AccessLevelConsts.write)
                {
                    connectMethods.mobile_router = true;
                }
                if (parseAccess(access_object["AccessMCB"]) != AccessLevelConsts.noAccess)
                {
                    AccessMCB = true;
                }
                else
                {
                    AccessMCB = false;
                }
                if (parseAccess(access_object["AccessBattView"]) != AccessLevelConsts.noAccess)
                {
                    AccessBattView = true;
                }
                else
                {
                    AccessBattView = false;
                }


                //Battview Info

                UserAccess.Batt_SN = parseAccess(access_object["Batt_SN"]);
                UserAccess.Batt_batteryID = parseAccess(access_object["Batt_batteryID"]);
                UserAccess.Batt_batteryModelandSN = parseAccess(access_object["Batt_batteryModelandSN"]);
                UserAccess.Batt_HWRevision = parseAccess(access_object["Batt_HWRevision"]);
                UserAccess.Batt_InstallationDate = parseAccess(access_object["Batt_InstallationDate"]);
                UserAccess.Batt_TimeZone = parseAccess(access_object["Batt_TimeZone"]);
                UserAccess.Batt_setup_version = parseAccess(access_object["Batt_setup_version"]);
                UserAccess.Batt_readFrimWareVersion = parseAccess(access_object["Batt_readFrimWareVersion"]);
                UserAccess.Batt_memorySignature = parseAccess(access_object["Batt_memorySignature"]);
                UserAccess.Batt_OverrideWarrantedAHR = parseAccess(access_object["Batt_OverrideWarrantedAHR"]);

                //BattView settings
                UserAccess.Batt_RTSampleTime = parseAccess(access_object["Batt_RTSampleTime"]);
                UserAccess.Batt_setPA = parseAccess(access_object["Batt_setPA"]);
                UserAccess.Batt_enablePostSensor = parseAccess(access_object["Batt_enablePostSensor"]);
                UserAccess.Batt_EnableEL = parseAccess(access_object["Batt_EnableEL"]);
                UserAccess.Batt_setHallEffect = parseAccess(access_object["Batt_setHallEffect"]);
                UserAccess.Batt_enablePLC = parseAccess(access_object["Batt_enablePLC"]);

                //WIFI
                UserAccess.Batt_actViewEnabled = parseAccess(access_object["Batt_actViewEnabled"]);
                UserAccess.Batt_softAPEnable = parseAccess(access_object["Batt_softAPEnable"]);
                UserAccess.Batt_softAPpassword = parseAccess(access_object["Batt_softAPpassword"]);
                UserAccess.Batt_mobileAccessSSID = parseAccess(access_object["Batt_mobileAccessSSID"]);
                UserAccess.Batt_mobileAccessSSIDpassword = parseAccess(access_object["Batt_mobileAccessSSIDpassword"]);
                UserAccess.Batt_mobilePort = parseAccess(access_object["Batt_mobilePort"]);
                UserAccess.Batt_actViewIP = parseAccess(access_object["Batt_actViewIP"]);
                UserAccess.Batt_actViewPort = parseAccess(access_object["Batt_actViewPort"]);
                UserAccess.Batt_actViewConnectFrequency = parseAccess(access_object["Batt_actViewConnectFrequency"]);
                UserAccess.Batt_actAccessSSID = parseAccess(access_object["Batt_actAccessSSID"]);
                UserAccess.Batt_actAccessSSIDpassword = parseAccess(access_object["Batt_actAccessSSIDpassword"]);





                //Battery Settings
                UserAccess.Batt_BatteryCapacity = parseAccess(access_object["Batt_BatteryCapacity"]);
                UserAccess.Batt_BatteryVoltage = parseAccess(access_object["Batt_BatteryVoltage"]);
                UserAccess.Batt_temperatureCompensation = parseAccess(access_object["Batt_temperatureCompensation"]);
                UserAccess.Batt_TempertureHigh = parseAccess(access_object["Batt_TempertureHigh"]);
                UserAccess.Batt_BatteryType = parseAccess(access_object["Batt_BatteryType"]);
                UserAccess.Batt_TemperatureFormat = parseAccess(access_object["Batt_TemperatureFormat"]);
                UserAccess.Batt_dayLightSaving_Enable = parseAccess(access_object["Batt_dayLightSaving_Enable"]);
                UserAccess.Batt_chargerType = parseAccess(access_object["Batt_chargerType"]);
                UserAccess.Batt_CreateStudy = parseAccess(access_object["Batt_CreateStudy"]);





                //BattView charge profile

                UserAccess.Batt_TR_CurrentRate = parseAccess(access_object["Batt_TR_CurrentRate"]);
                UserAccess.Batt_CC_CurrentRate = parseAccess(access_object["Batt_CC_CurrentRate"]);
                UserAccess.Batt_FI_CurrentRate = parseAccess(access_object["Batt_FI_CurrentRate"]);
                UserAccess.Batt_EQ_CurrentRate = parseAccess(access_object["Batt_EQ_CurrentRate"]);
                UserAccess.Batt_TrickleVoltage = parseAccess(access_object["Batt_TrickleVoltage"]);
                UserAccess.Batt_CVVoltage = parseAccess(access_object["Batt_CVVoltage"]);
                UserAccess.Batt_finishVoltage = parseAccess(access_object["Batt_finishVoltage"]);
                UserAccess.Batt_EqualaizeVoltage = parseAccess(access_object["Batt_EqualaizeVoltage"]);
                UserAccess.Batt_cvCurrentStep = parseAccess(access_object["Batt_cvCurrentStep"]);
                UserAccess.Batt_cvFinishCurrent = parseAccess(access_object["Batt_cvFinishCurrent"]);
                UserAccess.Batt_CVMaxTimer = parseAccess(access_object["Batt_CVMaxTimer"]);
                UserAccess.Batt_finishTimer = parseAccess(access_object["Batt_finishTimer"]);
                UserAccess.Batt_EqualizeTimer = parseAccess(access_object["Batt_EqualizeTimer"]);
                UserAccess.Batt_desulfationTimer = parseAccess(access_object["Batt_desulfationTimer"]);
                UserAccess.Batt_finishdVdT = parseAccess(access_object["Batt_finishdVdT"]);

                //FI and EQ scheduling
                UserAccess.Batt_FI_sched_Settings = parseAccess(access_object["Batt_FI_sched_Settings"]);
                UserAccess.Batt_FI_sched_CustomSettings = parseAccess(access_object["Batt_FI_sched_CustomSettings"]);
                UserAccess.Batt_EQ_sched_CustomSettings = parseAccess(access_object["Batt_EQ_sched_CustomSettings"]);
                UserAccess.Batt_FI_EQ_sched_CustomSettings = parseAccess(access_object["Batt_FI_EQ_sched_CustomSettings"]);

                //History
                UserAccess.Batt_ViewGlobalRecords = parseAccess(access_object["Batt_ViewGlobalRecords"]);
                UserAccess.Batt_ViewDebugGlobalRecords = parseAccess(access_object["Batt_ViewDebugGlobalRecords"]);
                UserAccess.Batt_canResetGlobalRecords = parseAccess(access_object["Batt_canResetGlobalRecords"]);
                UserAccess.Batt_canReadEventsByID = parseAccess(access_object["Batt_canReadEventsByID"]);
                UserAccess.Batt_canReadEventsByTime = parseAccess(access_object["Batt_canReadEventsByTime"]);
                UserAccess.Batt_canReadRTrecordsByID = parseAccess(access_object["Batt_canReadRTrecordsByID"]);
                UserAccess.Batt_canReadRTrecordsByTime = parseAccess(access_object["Batt_canReadRTrecordsByTime"]);
                UserAccess.Batt_canReadDebugrecordsByID = parseAccess(access_object["Batt_canReadDebugrecordsByID"]);
                UserAccess.Batt_canReadDebugrecordsByTime = parseAccess(access_object["Batt_canReadDebugrecordsByTime"]);
                //Quick View
                UserAccess.Batt_realDataCharts = parseAccess(access_object["Batt_realDataCharts"]);
                UserAccess.Batt_realDataCharts_detailed = parseAccess(access_object["Batt_realDataCharts_detailed"]);
                //Firmware update
                UserAccess.Batt_FirmwareUpdate = parseAccess(access_object["Batt_FirmwareUpdate"]);
                UserAccess.Batt_manualFirmwareUpdate = parseAccess(access_object["Batt_manualFirmwareUpdate"]);
                UserAccess.Batt_FirmwareRequestUpdateDebug = parseAccess(access_object["Batt_FirmwareRequestUpdateDebug"]);
                //Calibration
                UserAccess.Batt_Calibration = parseAccess(access_object["Batt_Calibration"]);
                UserAccess.Batt_view_Raw_values = parseAccess(access_object["Batt_view_Raw_values"]);
                UserAccess.Batt_setSOC = parseAccess(access_object["Batt_setSOC"]);
                UserAccess.Batt_coefficients = parseAccess(access_object["Batt_coefficients"]);
                UserAccess.Batt_Calibration_manual = parseAccess(access_object["Batt_Calibration_manual"]);

                UserAccess.Batt_onlyForEnginneringTeam = parseAccess(access_object["Batt_onlyForEnginneringTeam"]);
                UserAccess.Batt_AdminACTViewID = parseAccess(access_object["Batt_AdminACTViewID"]);
                UserAccess.Batt_eventsControl = parseAccess(access_object["Batt_eventsControl"]);
                UserAccess.Batt_COMMISSION = parseAccess(access_object["Batt_COMMISSION"]);
            }

        }

    }

}
