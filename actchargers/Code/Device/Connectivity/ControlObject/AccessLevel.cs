namespace actchargers
{
    public class AccessLevel
    {
        private readonly object myLock;

        public AccessLevel()
        {
            myLock = new object();
        }

        private int _AdminRestart;
        public int AdminRestart
        {
            get
            {
                lock (myLock)
                {
                    return _AdminRestart;
                }
            }
            set
            {
                lock (myLock)
                {
                    _AdminRestart = value;
                }
            }
        }

        private int _CanReadUncomissioned;
        public int CanReadNonCommissioned
        {
            get
            {
                lock (myLock)
                {
                    return _CanReadUncomissioned;
                }
            }
            set
            {
                lock (myLock)
                {
                    _CanReadUncomissioned = value;
                }
            }
        }

        #region Charger Info

        private int _MCB_SN;
        public int MCB_SN
        {
            get
            {
                lock (myLock)
                {
                    return _MCB_SN;
                }
            }
            set
            {
                lock (myLock)
                {
                    _MCB_SN = value;
                }
            }
        }
        private int _MCB_Model;
        public int MCB_Model
        {
            get
            {
                lock (myLock)
                {
                    return _MCB_Model;
                }
            }
            set
            {
                lock (myLock)
                {
                    _MCB_Model = value;
                }
            }
        }
        private int _MCB_HWRevision;
        public int MCB_HWRevision
        {
            get
            {
                lock (myLock)
                {
                    return _MCB_HWRevision;
                }
            }
            set
            {
                lock (myLock)
                {
                    _MCB_HWRevision = value;
                }
            }
        }
        private int _MCB_UserNamedID;
        public int MCB_UserNamedID
        {
            get
            {
                lock (myLock)
                {
                    return _MCB_UserNamedID;
                }
            }
            set
            {
                lock (myLock)
                {
                    _MCB_UserNamedID = value;
                }
            }
        }
        private int _MCB_InstallationDate;
        public int MCB_InstallationDate
        {
            get
            {
                lock (myLock)
                {
                    return _MCB_InstallationDate;
                }
            }
            set
            {
                lock (myLock)
                {
                    _MCB_InstallationDate = value;
                }
            }
        }
        private int _MCB_chargerType;
        public int MCB_chargerType
        {
            get
            {
                lock (myLock)
                {
                    return _MCB_chargerType;
                }
            }
            set
            {
                lock (myLock)
                {
                    _MCB_chargerType = value;
                }
            }
        }
        private int _MCB_TimeZone;
        public int MCB_TimeZone
        {
            get
            {
                lock (myLock)
                {
                    return _MCB_TimeZone;
                }
            }
            set
            {
                lock (myLock)
                {
                    _MCB_TimeZone = value;
                }
            }
        }
        private int _MCB_setup_version;
        public int MCB_setup_version
        {
            get
            {
                lock (myLock)
                {
                    return _MCB_setup_version;
                }
            }
            set
            {
                lock (myLock)
                {
                    _MCB_setup_version = value;
                }
            }
        }
        private int _MCB_readFrimWareVersion;
        public int MCB_readFrimWareVersion
        {
            get
            {
                lock (myLock)
                {
                    return _MCB_readFrimWareVersion;
                }
            }
            set
            {
                lock (myLock)
                {
                    _MCB_readFrimWareVersion = value;
                }
            }
        }
        private int _MCB_memorySignature;
        public int MCB_memorySignature
        {
            get
            {
                lock (myLock)
                {
                    return _MCB_memorySignature;
                }
            }
            set
            {
                lock (myLock)
                {
                    _MCB_memorySignature = value;
                }
            }
        }

        #endregion

        #region Charger Settings

        private int _MCB_ResetLCDCalibration;
        public int MCB_ResetLCDCalibration
        {
            get
            {
                lock (myLock)
                {
                    return _MCB_ResetLCDCalibration;
                }
            }
            set
            {
                lock (myLock)
                {
                    _MCB_ResetLCDCalibration = value;
                }
            }
        }
        private int _MCB_LoadPLC;
        public int MCB_LoadPLC
        {
            get
            {
                lock (myLock)
                {
                    return _MCB_LoadPLC;
                }
            }
            set
            {
                lock (myLock)
                {
                    _MCB_LoadPLC = value;
                }
            }
        }


        private int _MCB_IR;
        public int MCB_IR
        {
            get
            {
                lock (myLock)
                {
                    return _MCB_IR;
                }
            }
            set
            {
                lock (myLock)
                {
                    _MCB_IR = value;
                }
            }
        }
        private int _MCB_autoStart_count;
        public int MCB_autoStart_count
        {
            get
            {
                lock (myLock)
                {
                    return _MCB_autoStart_count;
                }
            }
            set
            {
                lock (myLock)
                {
                    _MCB_autoStart_count = value;
                }
            }
        }
        private int _MCB_autoStart_Enable;
        public int MCB_autoStart_Enable
        {
            get
            {
                lock (myLock)
                {
                    return _MCB_autoStart_Enable;
                }
            }
            set
            {
                lock (myLock)
                {
                    _MCB_autoStart_Enable = value;
                }
            }
        }
        private int _MCB_autoSatrt_DaysMask;
        public int MCB_autoSatrt_DaysMask
        {
            get
            {
                lock (myLock)
                {
                    return _MCB_autoSatrt_DaysMask;
                }
            }
            set
            {
                lock (myLock)
                {
                    _MCB_autoSatrt_DaysMask = value;
                }
            }
        }
        private int _MCB_refreshEnable;
        public int MCB_refreshEnable
        {
            get
            {
                lock (myLock)
                {
                    return _MCB_refreshEnable;
                }
            }
            set
            {
                lock (myLock)
                {
                    _MCB_refreshEnable = value;
                }
            }
        }
        private int _MCB_refreshTimer;
        public int MCB_refreshTimer
        {
            get
            {
                lock (myLock)
                {
                    return _MCB_refreshTimer;
                }
            }
            set
            {
                lock (myLock)
                {
                    _MCB_refreshTimer = value;
                }
            }
        }
        private int _MCB_TemperatureFormat;
        public int MCB_TemperatureFormat
        {
            get
            {
                lock (myLock)
                {
                    return _MCB_TemperatureFormat;
                }
            }
            set
            {
                lock (myLock)
                {
                    _MCB_TemperatureFormat = value;
                }
            }
        }
        private int _MCB_TemperatureSensorEnable;
        public int MCB_TemperatureSensorEnable
        {
            get
            {
                lock (myLock)
                {
                    return _MCB_TemperatureSensorEnable;
                }
            }
            set
            {
                lock (myLock)
                {
                    _MCB_TemperatureSensorEnable = value;
                }
            }
        }
        private int _MCB_multiVoltage;
        public int MCB_multiVoltage
        {
            get
            {
                lock (myLock)
                {
                    return _MCB_multiVoltage;
                }
            }
            set
            {
                lock (myLock)
                {
                    _MCB_multiVoltage = value;
                }
            }
        }
        private int _MCB_dayLightSaving_Enable;
        public int MCB_dayLightSaving_Enable
        {
            get
            {
                lock (myLock)
                {
                    return _MCB_dayLightSaving_Enable;
                }
            }
            set
            {
                lock (myLock)
                {
                    _MCB_dayLightSaving_Enable = value;
                }
            }
        }
        private int _MCB_enablePLC;
        public int MCB_enablePLC
        {
            get
            {
                lock (myLock)
                {
                    return _MCB_enablePLC;
                }
            }
            set
            {
                lock (myLock)
                {
                    _MCB_enablePLC = value;
                }
            }
        }
        private int _MCB_HWversionControl;
        public int MCB_HWversionControl
        {
            get
            {
                lock (myLock)
                {
                    return _MCB_HWversionControl;
                }
            }
            set
            {
                lock (myLock)
                {
                    _MCB_HWversionControl = value;
                }
            }
        }
        private int _MCB_BMS_FW_CHANGE;
        public int MCB_BMS_FW_CHANGE
        {
            get
            {
                lock (myLock)
                {
                    return _MCB_BMS_FW_CHANGE;
                }
            }
            set
            {
                lock (myLock)
                {
                    _MCB_BMS_FW_CHANGE = value;
                }
            }
        }
        private int _MCB_enableLauncher;
        public int MCB_enableLauncher
        {
            get
            {
                lock (myLock)
                {
                    return _MCB_enableLauncher;
                }
            }
            set
            {
                lock (myLock)
                {
                    _MCB_enableLauncher = value;
                }
            }
        }
        private int _MCB_DisablePushButton;
        public int MCB_DisablePushButton
        {
            get
            {
                lock (myLock)
                {
                    return _MCB_DisablePushButton;
                }
            }
            set
            {
                lock (myLock)
                {
                    _MCB_DisablePushButton = value;
                }
            }
        }
        private int _MCB_EditOriginalSerialNumber;
        public int MCB_EditOriginalSerialNumber
        {
            get
            {
                lock (myLock)
                {
                    return _MCB_EditOriginalSerialNumber;
                }
            }
            set
            {
                lock (myLock)
                {
                    _MCB_EditOriginalSerialNumber = value;
                }
            }
        }

        private int _MCB_EnableLED;
        public int MCB_EnableLED
        {
            get
            {
                lock (myLock)
                {
                    return _MCB_EnableLED;
                }
            }
            set
            {
                lock (myLock)
                {
                    _MCB_EnableLED = value;
                }
            }
        }

        int _MCB_DaughterCardEnabled;
        public int MCB_DaughterCardEnabled
        {
            get
            {
                lock (myLock)
                {
                    return _MCB_DaughterCardEnabled;
                }
            }
            set
            {
                lock (myLock)
                {
                    _MCB_DaughterCardEnabled = value;
                }
            }
        }

        int _PMMaxCurrent;
        public int PMMaxCurrent
        {
            get
            {
                lock (myLock)
                {
                    return _PMMaxCurrent;
                }
            }
            set
            {
                lock (myLock)
                {
                    _PMMaxCurrent = value;
                }
            }
        }

        int _MCB_defualtBrughtness;
        public int MCB_defualtBrughtness
        {
            get
            {
                lock (myLock)
                {
                    return _MCB_defualtBrughtness;
                }
            }
            set
            {
                lock (myLock)
                {
                    _MCB_defualtBrughtness = value;
                }
            }
        }

        #endregion

        #region WIFI

        private int _MCB_actViewEnabled;
        public int MCB_actViewEnabled
        {
            get
            {
                lock (myLock)
                {
                    return _MCB_actViewEnabled;
                }
            }
            set
            {
                lock (myLock)
                {
                    _MCB_actViewEnabled = value;
                }
            }
        }
        private int _MCB_softAPEnable;
        public int MCB_softAPEnable
        {
            get
            {
                lock (myLock)
                {
                    return _MCB_softAPEnable;
                }
            }
            set
            {
                lock (myLock)
                {
                    _MCB_softAPEnable = value;
                }
            }
        }
        private int _MCB_softAPpassword;
        public int MCB_softAPpassword
        {
            get
            {
                lock (myLock)
                {
                    return _MCB_softAPpassword;
                }
            }
            set
            {
                lock (myLock)
                {
                    _MCB_softAPpassword = value;
                }
            }
        }
        private int _MCB_mobileAccessSSID;
        public int MCB_mobileAccessSSID
        {
            get
            {
                lock (myLock)
                {
                    return _MCB_mobileAccessSSID;
                }
            }
            set
            {
                lock (myLock)
                {
                    _MCB_mobileAccessSSID = value;
                }
            }
        }
        private int _MCB_mobileAccessSSIDpassword;
        public int MCB_mobileAccessSSIDpassword
        {
            get
            {
                lock (myLock)
                {
                    return _MCB_mobileAccessSSIDpassword;
                }
            }
            set
            {
                lock (myLock)
                {
                    _MCB_mobileAccessSSIDpassword = value;
                }
            }
        }
        private int _MCB_mobilePort;
        public int MCB_mobilePort
        {
            get
            {
                lock (myLock)
                {
                    return _MCB_mobilePort;
                }
            }
            set
            {
                lock (myLock)
                {
                    _MCB_mobilePort = value;
                }
            }
        }
        private int _MCB_actViewIP;
        public int MCB_actViewIP
        {
            get
            {
                lock (myLock)
                {
                    return _MCB_actViewIP;
                }
            }
            set
            {
                lock (myLock)
                {
                    _MCB_actViewIP = value;
                }
            }
        }
        private int _MCB_actViewPort;
        public int MCB_actViewPort
        {
            get
            {
                lock (myLock)
                {
                    return _MCB_actViewPort;
                }
            }
            set
            {
                lock (myLock)
                {
                    _MCB_actViewPort = value;
                }
            }
        }
        private int _MCB_actViewConnectFrequency;
        public int MCB_actViewConnectFrequency
        {
            get
            {
                lock (myLock)
                {
                    return _MCB_actViewConnectFrequency;
                }
            }
            set
            {
                lock (myLock)
                {
                    _MCB_actViewConnectFrequency = value;
                }
            }
        }
        private int _MCB_actAccessSSID;
        public int MCB_actAccessSSID
        {
            get
            {
                lock (myLock)
                {
                    return _MCB_actAccessSSID;
                }
            }
            set
            {
                lock (myLock)
                {
                    _MCB_actAccessSSID = value;
                }
            }
        }
        private int _MCB_actAccessSSIDpassword;
        public int MCB_actAccessSSIDpassword
        {
            get
            {
                lock (myLock)
                {
                    return _MCB_actAccessSSIDpassword;
                }
            }
            set
            {
                lock (myLock)
                {
                    _MCB_actAccessSSIDpassword = value;
                }
            }
        }

        #endregion

        #region Battery Settings

        private int _MCB_BatteryCapacity;
        public int MCB_BatteryCapacity
        {
            get
            {
                lock (myLock)
                {
                    return _MCB_BatteryCapacity;
                }
            }
            set
            {
                lock (myLock)
                {
                    _MCB_BatteryCapacity = value;
                }
            }
        }
        private int _MCB_BatteryVoltage;
        public int MCB_BatteryVoltage
        {
            get
            {
                lock (myLock)
                {
                    return _MCB_BatteryVoltage;
                }
            }
            set
            {
                lock (myLock)
                {
                    _MCB_BatteryVoltage = value;
                }
            }
        }
        private int _MCB_temperatureCompensation;
        public int MCB_temperatureCompensation
        {
            get
            {
                lock (myLock)
                {
                    return _MCB_temperatureCompensation;
                }
            }
            set
            {
                lock (myLock)
                {
                    _MCB_temperatureCompensation = value;
                }
            }
        }
        private int _MCB_TempertureHigh;
        public int MCB_TempertureHigh
        {
            get
            {
                lock (myLock)
                {
                    return _MCB_TempertureHigh;
                }
            }
            set
            {
                lock (myLock)
                {
                    _MCB_TempertureHigh = value;
                }
            }
        }
        private int _MCB_BatteryType;
        public int MCB_BatteryType
        {
            get
            {
                lock (myLock)
                {
                    return _MCB_BatteryType;
                }
            }
            set
            {
                lock (myLock)
                {
                    _MCB_BatteryType = value;
                }
            }
        }
        private int _MCB_RunBattViewCal;
        public int MCB_RunBattViewCal
        {
            get
            {
                lock (myLock)
                {
                    return _MCB_RunBattViewCal;
                }
            }
            set
            {
                lock (myLock)
                {
                    _MCB_RunBattViewCal = value;
                }
            }
        }
        #endregion

        #region Charge Profile

        private int _MCB_TR_CurrentRate;
        public int MCB_TR_CurrentRate
        {
            get
            {
                lock (myLock)
                {
                    return _MCB_TR_CurrentRate;
                }
            }
            set
            {
                lock (myLock)
                {
                    _MCB_TR_CurrentRate = value;
                }
            }
        }
        private int _MCB_CC_CurrentRate;
        public int MCB_CC_CurrentRate
        {
            get
            {
                lock (myLock)
                {
                    return _MCB_CC_CurrentRate;
                }
            }
            set
            {
                lock (myLock)
                {
                    _MCB_CC_CurrentRate = value;
                }
            }
        }
        private int _MCB_FI_CurrentRate;
        public int MCB_FI_CurrentRate
        {
            get
            {
                lock (myLock)
                {
                    return _MCB_FI_CurrentRate;
                }
            }
            set
            {
                lock (myLock)
                {
                    _MCB_FI_CurrentRate = value;
                }
            }
        }
        private int _MCB_EQ_CurrentRate;
        public int MCB_EQ_CurrentRate
        {
            get
            {
                lock (myLock)
                {
                    return _MCB_EQ_CurrentRate;
                }
            }
            set
            {
                lock (myLock)
                {
                    _MCB_EQ_CurrentRate = value;
                }
            }
        }
        private int _MCB_TrickleVoltage;
        public int MCB_TrickleVoltage
        {
            get
            {
                lock (myLock)
                {
                    return _MCB_TrickleVoltage;
                }
            }
            set
            {
                lock (myLock)
                {
                    _MCB_TrickleVoltage = value;
                }
            }
        }
        private int _MCB_CVVoltage;
        public int MCB_CVVoltage
        {
            get
            {
                lock (myLock)
                {
                    return _MCB_CVVoltage;
                }
            }
            set
            {
                lock (myLock)
                {
                    _MCB_CVVoltage = value;
                }
            }
        }
        private int _MCB_finishVoltage;
        public int MCB_finishVoltage
        {
            get
            {
                lock (myLock)
                {
                    return _MCB_finishVoltage;
                }
            }
            set
            {
                lock (myLock)
                {
                    _MCB_finishVoltage = value;
                }
            }
        }
        private int _MCB_EqualaizeVoltage;
        public int MCB_EqualaizeVoltage
        {
            get
            {
                lock (myLock)
                {
                    return _MCB_EqualaizeVoltage;
                }
            }
            set
            {
                lock (myLock)
                {
                    _MCB_EqualaizeVoltage = value;
                }
            }
        }
        private int _MCB_cvCurrentStep;
        public int MCB_cvCurrentStep
        {
            get
            {
                lock (myLock)
                {
                    return _MCB_cvCurrentStep;
                }
            }
            set
            {
                lock (myLock)
                {
                    _MCB_cvCurrentStep = value;
                }
            }
        }
        private int _MCB_cvFinishCurrent;
        public int MCB_cvFinishCurrent
        {
            get
            {
                lock (myLock)
                {
                    return _MCB_cvFinishCurrent;
                }
            }
            set
            {
                lock (myLock)
                {
                    _MCB_cvFinishCurrent = value;
                }
            }
        }
        private int _MCB_CVMaxTimer;
        public int MCB_CVMaxTimer
        {
            get
            {
                lock (myLock)
                {
                    return _MCB_CVMaxTimer;
                }
            }
            set
            {
                lock (myLock)
                {
                    _MCB_CVMaxTimer = value;
                }
            }
        }
        private int _MCB_finishTimer;
        public int MCB_finishTimer
        {
            get
            {
                lock (myLock)
                {
                    return _MCB_finishTimer;
                }
            }
            set
            {
                lock (myLock)
                {
                    _MCB_finishTimer = value;
                }
            }
        }
        private int _MCB_EqualizeTimer;
        public int MCB_EqualizeTimer
        {
            get
            {
                lock (myLock)
                {
                    return _MCB_EqualizeTimer;
                }
            }
            set
            {
                lock (myLock)
                {
                    _MCB_EqualizeTimer = value;
                }
            }
        }
        private int _MCB_desulfationTimer;
        public int MCB_desulfationTimer
        {
            get
            {
                lock (myLock)
                {
                    return _MCB_desulfationTimer;
                }
            }
            set
            {
                lock (myLock)
                {
                    _MCB_desulfationTimer = value;
                }
            }
        }
        private int _MCB_finishdVdT;
        public int MCB_finishdVdT
        {
            get
            {
                lock (myLock)
                {
                    return _MCB_finishdVdT;
                }
            }
            set
            {
                lock (myLock)
                {
                    _MCB_finishdVdT = value;
                }
            }
        }
        private int _MCB_SetupNewProfile;
        public int MCB_SetupNewProfile
        {
            get
            {
                lock (myLock)
                {
                    return _MCB_SetupNewProfile;
                }
            }
            set
            {
                lock (myLock)
                {
                    _MCB_SetupNewProfile = value;
                }
            }
        }

        #endregion

        #region Finish And EQ scheduling

        private int _MCB_FI_sched_Settings;
        public int MCB_FI_sched_Settings
        {
            get
            {
                lock (myLock)
                {
                    return _MCB_FI_sched_Settings;
                }
            }
            set
            {
                lock (myLock)
                {
                    _MCB_FI_sched_Settings = value;
                }
            }
        }
        private int _MCB_FI_sched_CustomSettings;
        public int MCB_FI_sched_CustomSettings
        {
            get
            {
                lock (myLock)
                {
                    return _MCB_FI_sched_CustomSettings;
                }
            }
            set
            {
                lock (myLock)
                {
                    _MCB_FI_sched_CustomSettings = value;
                }
            }
        }
        private int _MCB_EQ_sched_CustomSettings;
        public int MCB_EQ_sched_CustomSettings
        {
            get
            {
                lock (myLock)
                {
                    return _MCB_EQ_sched_CustomSettings;
                }
            }
            set
            {
                lock (myLock)
                {
                    _MCB_EQ_sched_CustomSettings = value;
                }
            }
        }

        private int _MCB_FI_EQ_sched_CustomSettings;
        public int MCB_FI_EQ_sched_CustomSettings
        {
            get
            {
                lock (myLock)
                {
                    return _MCB_FI_EQ_sched_CustomSettings;
                }
            }
            set
            {
                lock (myLock)
                {
                    _MCB_FI_EQ_sched_CustomSettings = value;
                }
            }
        }

        private int _MCB_ignorebattviewsoc;
        public int MCB_ignorebattviewsoc
        {
            get
            {
                lock (myLock)
                {
                    return _MCB_ignorebattviewsoc;
                }
            }
            set
            {
                lock (myLock)
                {
                    _MCB_ignorebattviewsoc = value;
                }
            }
        }

        #endregion

        #region Global Records

        private int _MCB_ViewGlobalRecords;
        public int MCB_ViewGlobalRecords
        {
            get
            {
                lock (myLock)
                {
                    return _MCB_ViewGlobalRecords;
                }
            }
            set
            {
                lock (myLock)
                {
                    _MCB_ViewGlobalRecords = value;
                }
            }
        }
        private int _MCB_ViewTotalPMFaults;
        public int MCB_ViewTotalPMFaults
        {
            get
            {
                lock (myLock)
                {
                    return _MCB_ViewTotalPMFaults;
                }
            }
            set
            {
                lock (myLock)
                {
                    _MCB_ViewTotalPMFaults = value;
                }
            }
        }
        private int _MCB_canResetGlobalRecords;
        public int MCB_canResetGlobalRecords
        {
            get
            {
                lock (myLock)
                {
                    return _MCB_canResetGlobalRecords;
                }
            }
            set
            {
                lock (myLock)
                {
                    _MCB_canResetGlobalRecords = value;
                }
            }
        }

        #endregion

        #region Cycles History

        private int _MCB_canReadCyclesHistory;
        public int MCB_canReadCyclesHistory
        {
            get
            {
                lock (myLock)
                {
                    return _MCB_canReadCyclesHistory;
                }
            }
            set
            {
                lock (myLock)
                {
                    _MCB_canReadCyclesHistory = value;
                }
            }
        }

        #endregion

        #region PM infirmation

        private int _MCB_PM_effieciency;
        public int MCB_PM_effieciency
        {
            get
            {
                lock (myLock)
                {
                    return _MCB_PM_effieciency;
                }
            }
            set
            {
                lock (myLock)
                {
                    _MCB_PM_effieciency = value;
                }
            }
        }

        private int _MCB_PM_LiveView;
        public int MCB_PM_LiveView
        {
            get
            {
                lock (myLock)
                {
                    return _MCB_PM_LiveView;
                }
            }
            set
            {
                lock (myLock)
                {
                    _MCB_PM_LiveView = value;
                }
            }
        }
        private int _MCB_numberOfInstalledPMs;
        public int MCB_numberOfInstalledPMs
        {
            get
            {
                lock (myLock)
                {
                    return _MCB_numberOfInstalledPMs;
                }
            }
            set
            {
                lock (myLock)
                {
                    _MCB_numberOfInstalledPMs = value;
                }
            }
        }
        private int _MCB_PM_Voltage;
        public int MCB_PM_Voltage
        {
            get
            {
                lock (myLock)
                {
                    return _MCB_PM_Voltage;
                }
            }
            set
            {
                lock (myLock)
                {
                    _MCB_PM_Voltage = value;
                }
            }
        }
        private int _MCB_PM_canReadFaults;
        public int MCB_PM_canReadFaults
        {
            get
            {
                lock (myLock)
                {
                    return _MCB_PM_canReadFaults;
                }
            }
            set
            {
                lock (myLock)
                {
                    _MCB_PM_canReadFaults = value;
                }
            }
        }
        #endregion

        #region Calibration Module

        private int _MCB_Calibration;
        public int MCB_Calibration
        {
            get
            {
                lock (myLock)
                {
                    return _MCB_Calibration;
                }
            }
            set
            {
                lock (myLock)
                {
                    _MCB_Calibration = value;
                }
            }
        }
        private int _MCB_view_Raw_values;
        public int MCB_view_Raw_values
        {
            get
            {
                lock (myLock)
                {
                    return _MCB_view_Raw_values;
                }
            }
            set
            {
                lock (myLock)
                {
                    _MCB_view_Raw_values = value;
                }
            }
        }
        private int _MCB_ViRdiv;
        public int MCB_ViRdiv
        {
            get
            {
                lock (myLock)
                {
                    return _MCB_ViRdiv;
                }
            }
            set
            {
                lock (myLock)
                {
                    _MCB_ViRdiv = value;
                }
            }
        }
        private int _MCB_Steinhart;
        public int MCB_Steinhart
        {
            get
            {
                lock (myLock)
                {
                    return _MCB_Steinhart;
                }
            }
            set
            {
                lock (myLock)
                {
                    _MCB_Steinhart = value;
                }
            }
        }
        private int _MCB_Calibration_manual;
        public int MCB_Calibration_manual
        {
            get
            {
                lock (myLock)
                {
                    return _MCB_Calibration_manual;
                }
            }
            set
            {
                lock (myLock)
                {
                    _MCB_Calibration_manual = value;
                }
            }
        }

        #endregion

        #region Update Firmware

        private int _MCB_FirmwareUpdate;
        public int MCB_FirmwareUpdate
        {
            get
            {
                lock (myLock)
                {
                    return _MCB_FirmwareUpdate;
                }
            }
            set
            {
                lock (myLock)
                {
                    _MCB_FirmwareUpdate = value;
                }
            }
        }
        private int _MCB_manualFirmwareUpdate;
        public int MCB_manualFirmwareUpdate
        {
            get
            {
                lock (myLock)
                {
                    return _MCB_manualFirmwareUpdate;
                }
            }
            set
            {
                lock (myLock)
                {
                    _MCB_manualFirmwareUpdate = value;
                }
            }
        }
        private int _MCB_FirmwareRequestUpdateDebug;
        public int MCB_FirmwareRequestUpdateDebug
        {
            get
            {
                lock (myLock)
                {
                    return _MCB_FirmwareRequestUpdateDebug;
                }
            }
            set
            {
                lock (myLock)
                {
                    _MCB_FirmwareRequestUpdateDebug = value;
                }
            }
        }

        #endregion

        #region Admin

        private int _MCB_AdminSimulation;
        public int MCB_AdminSimulation
        {
            get
            {
                lock (myLock)
                {
                    return _MCB_AdminSimulation;
                }
            }
            set
            {
                lock (myLock)
                {
                    _MCB_AdminSimulation = value;
                }
            }
        }
        private int _MCB_AdminACTViewID;
        public int MCB_AdminACTViewID
        {
            get
            {
                lock (myLock)
                {
                    return _MCB_AdminACTViewID;
                }
            }
            set
            {
                lock (myLock)
                {
                    _MCB_AdminACTViewID = value;
                }
            }
        }
        private int _MCB_AdminPMSimulation;
        public int MCB_AdminPMSimulation
        {
            get
            {
                lock (myLock)
                {
                    return _MCB_AdminPMSimulation;
                }
            }
            set
            {
                lock (myLock)
                {
                    _MCB_AdminPMSimulation = value;
                }
            }
        }
        private int _MCB_onlyForEnginneringTeam;
        public int MCB_onlyForEnginneringTeam
        {
            get
            {
                lock (myLock)
                {
                    return _MCB_onlyForEnginneringTeam;
                }
            }
            set
            {
                lock (myLock)
                {
                    _MCB_onlyForEnginneringTeam = value;
                }
            }
        }
        private int _MCB_EnergyManagment;
        public int MCB_EnergyManagment
        {
            get
            {
                lock (myLock)
                {
                    return _MCB_EnergyManagment;
                }
            }
            set
            {
                lock (myLock)
                {
                    _MCB_EnergyManagment = value;
                }
            }
        }

        private int _MCB_COMMISSION;
        public int MCB_COMMISSION
        {
            get
            {
                lock (myLock)
                {
                    return _MCB_COMMISSION;
                }
            }
            set
            {
                lock (myLock)
                {
                    _MCB_COMMISSION = value;
                }
            }
        }

        #endregion

        #region BattView Info 

        private int _Batt_SN;
        public int Batt_SN
        {
            get
            {
                lock (myLock)
                {
                    return _Batt_SN;
                }
            }
            set
            {
                lock (myLock)
                {
                    _Batt_SN = value;
                }
            }
        }
        private int _Batt_batteryID;
        public int Batt_batteryID
        {
            get
            {
                lock (myLock)
                {
                    return _Batt_batteryID;
                }
            }
            set
            {
                lock (myLock)
                {
                    _Batt_batteryID = value;
                }
            }
        }
        private int _Batt_batteryModelandSN;
        public int Batt_batteryModelandSN
        {
            get
            {
                lock (myLock)
                {
                    return _Batt_batteryModelandSN;
                }
            }
            set
            {
                lock (myLock)
                {
                    _Batt_batteryModelandSN = value;
                }
            }
        }
        private int _Batt_HWRevision;
        public int Batt_HWRevision
        {
            get
            {
                lock (myLock)
                {
                    return _Batt_HWRevision;
                }
            }
            set
            {
                lock (myLock)
                {
                    _Batt_HWRevision = value;
                }
            }
        }
        private int _Batt_InstallationDate;
        public int Batt_InstallationDate
        {
            get
            {
                lock (myLock)
                {
                    return _Batt_InstallationDate;
                }
            }
            set
            {
                lock (myLock)
                {
                    _Batt_InstallationDate = value;
                }
            }
        }
        private int _Batt_TimeZone;
        public int Batt_TimeZone
        {
            get
            {
                lock (myLock)
                {
                    return _Batt_TimeZone;
                }
            }
            set
            {
                lock (myLock)
                {
                    _Batt_TimeZone = value;
                }
            }
        }
        private int _Batt_setup_version;
        public int Batt_setup_version
        {
            get
            {
                lock (myLock)
                {
                    return _Batt_setup_version;
                }
            }
            set
            {
                lock (myLock)
                {
                    _Batt_setup_version = value;
                }
            }
        }
        private int _Batt_readFrimWareVersion;
        public int Batt_readFrimWareVersion
        {
            get
            {
                lock (myLock)
                {
                    return _Batt_readFrimWareVersion;
                }
            }
            set
            {
                lock (myLock)
                {
                    _Batt_readFrimWareVersion = value;
                }
            }
        }
        private int _Batt_memorySignature;
        public int Batt_memorySignature
        {
            get
            {
                lock (myLock)
                {
                    return _Batt_memorySignature;
                }
            }
            set
            {
                lock (myLock)
                {
                    _Batt_memorySignature = value;
                }
            }
        }
        private int _Batt_OverrideWarrantedAHR;
        public int Batt_OverrideWarrantedAHR
        {
            get
            {
                lock (myLock)
                {
                    return _Batt_OverrideWarrantedAHR;
                }
            }
            set
            {
                lock (myLock)
                {
                    _Batt_OverrideWarrantedAHR = value;
                }
            }
        }

        #endregion

        #region BattView Settings

        private int _Batt_RTSampleTime;
        public int Batt_RTSampleTime
        {
            get
            {
                lock (myLock)
                {
                    return _Batt_RTSampleTime;
                }
            }
            set
            {
                lock (myLock)
                {
                    _Batt_RTSampleTime = value;
                }
            }
        }
        private int _Batt_setPA;
        public int Batt_setPA
        {
            get
            {
                lock (myLock)
                {
                    return _Batt_setPA;
                }
            }
            set
            {
                lock (myLock)
                {
                    _Batt_setPA = value;
                }
            }
        }
        private int _Batt_enablePostSensor;
        public int Batt_enablePostSensor
        {
            get
            {
                lock (myLock)
                {
                    return _Batt_enablePostSensor;
                }
            }
            set
            {
                lock (myLock)
                {
                    _Batt_enablePostSensor = value;
                }
            }
        }
        private int _Batt_EnableEL;
        public int Batt_EnableEL
        {
            get
            {
                lock (myLock)
                {
                    return _Batt_EnableEL;
                }
            }
            set
            {
                lock (myLock)
                {
                    _Batt_EnableEL = value;
                }
            }
        }
        private int _Batt_setHallEffect;
        public int Batt_setHallEffect
        {
            get
            {
                lock (myLock)
                {
                    return _Batt_setHallEffect;
                }
            }
            set
            {
                lock (myLock)
                {
                    _Batt_setHallEffect = value;
                }
            }
        }
        private int _Batt_enablePLC;
        public int Batt_enablePLC
        {
            get
            {
                lock (myLock)
                {
                    return _Batt_enablePLC;
                }
            }
            set
            {
                lock (myLock)
                {
                    _Batt_enablePLC = value;
                }
            }
        }
        #endregion

        #region WIFI

        private int _Batt_actViewEnabled;
        public int Batt_actViewEnabled
        {
            get
            {
                lock (myLock)
                {
                    return _Batt_actViewEnabled;
                }
            }
            set
            {
                lock (myLock)
                {
                    _Batt_actViewEnabled = value;
                }
            }
        }
        private int _Batt_softAPEnable;
        public int Batt_softAPEnable
        {
            get
            {
                lock (myLock)
                {
                    return _Batt_softAPEnable;
                }
            }
            set
            {
                lock (myLock)
                {
                    _Batt_softAPEnable = value;
                }
            }
        }
        private int _Batt_softAPpassword;
        public int Batt_softAPpassword
        {
            get
            {
                lock (myLock)
                {
                    return _Batt_softAPpassword;
                }
            }
            set
            {
                lock (myLock)
                {
                    _Batt_softAPpassword = value;
                }
            }
        }
        private int _Batt_mobileAccessSSID;
        public int Batt_mobileAccessSSID
        {
            get
            {
                lock (myLock)
                {
                    return _Batt_mobileAccessSSID;
                }
            }
            set
            {
                lock (myLock)
                {
                    _Batt_mobileAccessSSID = value;
                }
            }
        }
        private int _Batt_mobileAccessSSIDpassword;
        public int Batt_mobileAccessSSIDpassword
        {
            get
            {
                lock (myLock)
                {
                    return _Batt_mobileAccessSSIDpassword;
                }
            }
            set
            {
                lock (myLock)
                {
                    _Batt_mobileAccessSSIDpassword = value;
                }
            }
        }
        private int _Batt_mobilePort;
        public int Batt_mobilePort
        {
            get
            {
                lock (myLock)
                {
                    return _Batt_mobilePort;
                }
            }
            set
            {
                lock (myLock)
                {
                    _Batt_mobilePort = value;
                }
            }
        }
        private int _Batt_actViewIP;
        public int Batt_actViewIP
        {
            get
            {
                lock (myLock)
                {
                    return _Batt_actViewIP;
                }
            }
            set
            {
                lock (myLock)
                {
                    _Batt_actViewIP = value;
                }
            }
        }
        private int _Batt_actViewPort;
        public int Batt_actViewPort
        {
            get
            {
                lock (myLock)
                {
                    return _Batt_actViewPort;
                }
            }
            set
            {
                lock (myLock)
                {
                    _Batt_actViewPort = value;
                }
            }
        }
        private int _Batt_actViewConnectFrequency;
        public int Batt_actViewConnectFrequency
        {
            get
            {
                lock (myLock)
                {
                    return _Batt_actViewConnectFrequency;
                }
            }
            set
            {
                lock (myLock)
                {
                    _Batt_actViewConnectFrequency = value;
                }
            }
        }
        private int _Batt_actAccessSSID;
        public int Batt_actAccessSSID
        {
            get
            {
                lock (myLock)
                {
                    return _Batt_actAccessSSID;
                }
            }
            set
            {
                lock (myLock)
                {
                    _Batt_actAccessSSID = value;
                }
            }
        }
        private int _Batt_actAccessSSIDpassword;
        public int Batt_actAccessSSIDpassword
        {
            get
            {
                lock (myLock)
                {
                    return _Batt_actAccessSSIDpassword;
                }
            }
            set
            {
                lock (myLock)
                {
                    _Batt_actAccessSSIDpassword = value;
                }
            }
        }

        private int _Batt_TemperatureFormat;
        public int Batt_TemperatureFormat
        {
            get
            {
                lock (myLock)
                {
                    return _Batt_TemperatureFormat;
                }
            }
            set
            {
                lock (myLock)
                {
                    _Batt_TemperatureFormat = value;
                }
            }
        }
        private int _Batt_dayLightSaving_Enable;
        public int Batt_dayLightSaving_Enable
        {
            get
            {
                lock (myLock)
                {
                    return _Batt_dayLightSaving_Enable;
                }
            }
            set
            {
                lock (myLock)
                {
                    _Batt_dayLightSaving_Enable = value;
                }
            }
        }

        #endregion

        #region Battery Settings

        private int _Batt_BatteryCapacity;
        public int Batt_BatteryCapacity
        {
            get
            {
                lock (myLock)
                {
                    return _Batt_BatteryCapacity;
                }
            }
            set
            {
                lock (myLock)
                {
                    _Batt_BatteryCapacity = value;
                }
            }
        }
        private int _Batt_BatteryVoltage;
        public int Batt_BatteryVoltage
        {
            get
            {
                lock (myLock)
                {
                    return _Batt_BatteryVoltage;
                }
            }
            set
            {
                lock (myLock)
                {
                    _Batt_BatteryVoltage = value;
                }
            }
        }
        private int _Batt_temperatureCompensation;
        public int Batt_temperatureCompensation
        {
            get
            {
                lock (myLock)
                {
                    return _Batt_temperatureCompensation;
                }
            }
            set
            {
                lock (myLock)
                {
                    _Batt_temperatureCompensation = value;
                }
            }
        }
        private int _Batt_TempertureHigh;
        public int Batt_TempertureHigh
        {
            get
            {
                lock (myLock)
                {
                    return _Batt_TempertureHigh;
                }
            }
            set
            {
                lock (myLock)
                {
                    _Batt_TempertureHigh = value;
                }
            }
        }
        private int _Batt_BatteryType;
        public int Batt_BatteryType
        {
            get
            {
                lock (myLock)
                {
                    return _Batt_BatteryType;
                }
            }
            set
            {
                lock (myLock)
                {
                    _Batt_BatteryType = value;
                }
            }
        }
        private int _Batt_chargerType;
        public int Batt_chargerType
        {
            get
            {
                lock (myLock)
                {
                    return _Batt_chargerType;
                }
            }
            set
            {
                lock (myLock)
                {
                    _Batt_chargerType = value;
                }
            }
        }
        private int _Batt_CreateStudy;
        public int Batt_CreateStudy
        {
            get
            {
                lock (myLock)
                {
                    return _Batt_CreateStudy;
                }
            }
            set
            {
                lock (myLock)
                {
                    _Batt_CreateStudy = value;
                }
            }
        }
        #endregion

        #region BattView Charge Profile

        private int _Batt_TR_CurrentRate;
        public int Batt_TR_CurrentRate
        {
            get
            {
                lock (myLock)
                {
                    return _Batt_TR_CurrentRate;
                }
            }
            set
            {
                lock (myLock)
                {
                    _Batt_TR_CurrentRate = value;
                }
            }
        }
        private int _Batt_CC_CurrentRate;
        public int Batt_CC_CurrentRate
        {
            get
            {
                lock (myLock)
                {
                    return _Batt_CC_CurrentRate;
                }
            }
            set
            {
                lock (myLock)
                {
                    _Batt_CC_CurrentRate = value;
                }
            }
        }
        private int _Batt_FI_CurrentRate;
        public int Batt_FI_CurrentRate
        {
            get
            {
                lock (myLock)
                {
                    return _Batt_FI_CurrentRate;
                }
            }
            set
            {
                lock (myLock)
                {
                    _Batt_FI_CurrentRate = value;
                }
            }
        }
        private int _Batt_EQ_CurrentRate;
        public int Batt_EQ_CurrentRate
        {
            get
            {
                lock (myLock)
                {
                    return _Batt_EQ_CurrentRate;
                }
            }
            set
            {
                lock (myLock)
                {
                    _Batt_EQ_CurrentRate = value;
                }
            }
        }
        private int _Batt_TrickleVoltage;
        public int Batt_TrickleVoltage
        {
            get
            {
                lock (myLock)
                {
                    return _Batt_TrickleVoltage;
                }
            }
            set
            {
                lock (myLock)
                {
                    _Batt_TrickleVoltage = value;
                }
            }
        }
        private int _Batt_CVVoltage;
        public int Batt_CVVoltage
        {
            get
            {
                lock (myLock)
                {
                    return _Batt_CVVoltage;
                }
            }
            set
            {
                lock (myLock)
                {
                    _Batt_CVVoltage = value;
                }
            }
        }
        private int _Batt_finishVoltage;
        public int Batt_finishVoltage
        {
            get
            {
                lock (myLock)
                {
                    return _Batt_finishVoltage;
                }
            }
            set
            {
                lock (myLock)
                {
                    _Batt_finishVoltage = value;
                }
            }
        }
        private int _Batt_EqualaizeVoltage;
        public int Batt_EqualaizeVoltage
        {
            get
            {
                lock (myLock)
                {
                    return _Batt_EqualaizeVoltage;
                }
            }
            set
            {
                lock (myLock)
                {
                    _Batt_EqualaizeVoltage = value;
                }
            }
        }
        private int _Batt_cvCurrentStep;
        public int Batt_cvCurrentStep
        {
            get
            {
                lock (myLock)
                {
                    return _Batt_cvCurrentStep;
                }
            }
            set
            {
                lock (myLock)
                {
                    _Batt_cvCurrentStep = value;
                }
            }
        }
        private int _Batt_cvFinishCurrent;
        public int Batt_cvFinishCurrent
        {
            get
            {
                lock (myLock)
                {
                    return _Batt_cvFinishCurrent;
                }
            }
            set
            {
                lock (myLock)
                {
                    _Batt_cvFinishCurrent = value;
                }
            }
        }
        private int _Batt_CVMaxTimer;
        public int Batt_CVMaxTimer
        {
            get
            {
                lock (myLock)
                {
                    return _Batt_CVMaxTimer;
                }
            }
            set
            {
                lock (myLock)
                {
                    _Batt_CVMaxTimer = value;
                }
            }
        }
        private int _Batt_finishTimer;
        public int Batt_finishTimer
        {
            get
            {
                lock (myLock)
                {
                    return _Batt_finishTimer;
                }
            }
            set
            {
                lock (myLock)
                {
                    _Batt_finishTimer = value;
                }
            }
        }
        private int _Batt_EqualizeTimer;
        public int Batt_EqualizeTimer
        {
            get
            {
                lock (myLock)
                {
                    return _Batt_EqualizeTimer;
                }
            }
            set
            {
                lock (myLock)
                {
                    _Batt_EqualizeTimer = value;
                }
            }
        }
        private int _Batt_desulfationTimer;
        public int Batt_desulfationTimer
        {
            get
            {
                lock (myLock)
                {
                    return _Batt_desulfationTimer;
                }
            }
            set
            {
                lock (myLock)
                {
                    _Batt_desulfationTimer = value;
                }
            }
        }
        private int _Batt_finishdVdT;
        public int Batt_finishdVdT
        {
            get
            {
                lock (myLock)
                {
                    return _Batt_finishdVdT;
                }
            }
            set
            {
                lock (myLock)
                {
                    _Batt_finishdVdT = value;
                }
            }
        }

        #endregion

        #region Finish And EQ scheduling

        private int _Batt_FI_sched_Settings;
        public int Batt_FI_sched_Settings
        {
            get
            {
                lock (myLock)
                {
                    return _Batt_FI_sched_Settings;
                }
            }
            set
            {
                lock (myLock)
                {
                    _Batt_FI_sched_Settings = value;
                }
            }
        }
        private int _Batt_FI_sched_CustomSettings;
        public int Batt_FI_sched_CustomSettings
        {
            get
            {
                lock (myLock)
                {
                    return _Batt_FI_sched_CustomSettings;
                }
            }
            set
            {
                lock (myLock)
                {
                    _Batt_FI_sched_CustomSettings = value;
                }
            }
        }
        private int _Batt_EQ_sched_CustomSettings;
        public int Batt_EQ_sched_CustomSettings
        {
            get
            {
                lock (myLock)
                {
                    return _Batt_EQ_sched_CustomSettings;
                }
            }
            set
            {
                lock (myLock)
                {
                    _Batt_EQ_sched_CustomSettings = value;
                }
            }
        }
        private int _Batt_FI_EQ_sched_CustomSettings;
        public int Batt_FI_EQ_sched_CustomSettings
        {
            get
            {
                lock (myLock)
                {
                    return _Batt_FI_EQ_sched_CustomSettings;
                }
            }
            set
            {
                lock (myLock)
                {
                    _Batt_FI_EQ_sched_CustomSettings = value;
                }
            }
        }

        #endregion

        #region History

        private int _Batt_ViewGlobalRecords;
        public int Batt_ViewGlobalRecords
        {
            get
            {
                lock (myLock)
                {
                    return _Batt_ViewGlobalRecords;
                }
            }
            set
            {
                lock (myLock)
                {
                    _Batt_ViewGlobalRecords = value;
                }
            }
        }
        private int _Batt_ViewDebugGlobalRecords;
        public int Batt_ViewDebugGlobalRecords
        {
            get
            {
                lock (myLock)
                {
                    return _Batt_ViewDebugGlobalRecords;
                }
            }
            set
            {
                lock (myLock)
                {
                    _Batt_ViewDebugGlobalRecords = value;
                }
            }
        }
        private int _Batt_canResetGlobalRecords;
        public int Batt_canResetGlobalRecords
        {
            get
            {
                lock (myLock)
                {
                    return _Batt_canResetGlobalRecords;
                }
            }
            set
            {
                lock (myLock)
                {
                    _Batt_canResetGlobalRecords = value;
                }
            }
        }
        private int _Batt_canReadEventsByID;
        public int Batt_canReadEventsByID
        {
            get
            {
                lock (myLock)
                {
                    return _Batt_canReadEventsByID;
                }
            }
            set
            {
                lock (myLock)
                {
                    _Batt_canReadEventsByID = value;
                }
            }
        }
        private int _Batt_canReadEventsByTime;
        public int Batt_canReadEventsByTime
        {
            get
            {
                lock (myLock)
                {
                    return _Batt_canReadEventsByTime;
                }
            }
            set
            {
                lock (myLock)
                {
                    _Batt_canReadEventsByTime = value;
                }
            }
        }
        private int _Batt_canReadRTrecordsByID;
        public int Batt_canReadRTrecordsByID
        {
            get
            {
                lock (myLock)
                {
                    return _Batt_canReadRTrecordsByID;
                }
            }
            set
            {
                lock (myLock)
                {
                    _Batt_canReadRTrecordsByID = value;
                }
            }
        }
        private int _Batt_canReadRTrecordsByTime;
        public int Batt_canReadRTrecordsByTime
        {
            get
            {
                lock (myLock)
                {
                    return _Batt_canReadRTrecordsByTime;
                }
            }
            set
            {
                lock (myLock)
                {
                    _Batt_canReadRTrecordsByTime = value;
                }
            }
        }
        private int _Batt_canReadDebugrecordsByID;
        public int Batt_canReadDebugrecordsByID
        {
            get
            {
                lock (myLock)
                {
                    return _Batt_canReadDebugrecordsByID;
                }
            }
            set
            {
                lock (myLock)
                {
                    _Batt_canReadDebugrecordsByID = value;
                }
            }
        }
        private int _Batt_canReadDebugrecordsByTime;
        public int Batt_canReadDebugrecordsByTime
        {
            get
            {
                lock (myLock)
                {
                    return _Batt_canReadDebugrecordsByTime;
                }
            }
            set
            {
                lock (myLock)
                {
                    _Batt_canReadDebugrecordsByTime = value;
                }
            }
        }
        #endregion

        #region Quick View

        private int _Batt_realDataCharts;
        public int Batt_realDataCharts
        {
            get
            {
                lock (myLock)
                {
                    return _Batt_realDataCharts;
                }
            }
            set
            {
                lock (myLock)
                {
                    _Batt_realDataCharts = value;
                }
            }
        }
        private int _Batt_realDataCharts_detailed;
        public int Batt_realDataCharts_detailed
        {
            get
            {
                lock (myLock)
                {
                    return _Batt_realDataCharts_detailed;
                }
            }
            set
            {
                lock (myLock)
                {
                    _Batt_realDataCharts_detailed = value;
                }
            }
        }
        #endregion

        #region Update Firmware

        private int _Batt_FirmwareUpdate;
        public int Batt_FirmwareUpdate
        {
            get
            {
                lock (myLock)
                {
                    return _Batt_FirmwareUpdate;
                }
            }
            set
            {
                lock (myLock)
                {
                    _Batt_FirmwareUpdate = value;
                }
            }
        }
        private int _Batt_manualFirmwareUpdate;
        public int Batt_manualFirmwareUpdate
        {
            get
            {
                lock (myLock)
                {
                    return _Batt_manualFirmwareUpdate;
                }
            }
            set
            {
                lock (myLock)
                {
                    _Batt_manualFirmwareUpdate = value;
                }
            }
        }
        private int _Batt_FirmwareRequestUpdateDebug;
        public int Batt_FirmwareRequestUpdateDebug
        {
            get
            {
                lock (myLock)
                {
                    return _Batt_FirmwareRequestUpdateDebug;
                }
            }
            set
            {
                lock (myLock)
                {
                    _Batt_FirmwareRequestUpdateDebug = value;
                }
            }
        }

        private int _Batt_Calibration;
        public int Batt_Calibration
        {
            get
            {
                lock (myLock)
                {
                    return _Batt_Calibration;
                }
            }
            set
            {
                lock (myLock)
                {
                    _Batt_Calibration = value;
                }
            }
        }
        private int _Batt_view_Raw_values;
        public int Batt_view_Raw_values
        {
            get
            {
                lock (myLock)
                {
                    return _Batt_view_Raw_values;
                }
            }
            set
            {
                lock (myLock)
                {
                    _Batt_view_Raw_values = value;
                }
            }
        }
        private int _Batt_setSOC;
        public int Batt_setSOC
        {
            get
            {
                lock (myLock)
                {
                    return _Batt_setSOC;
                }
            }
            set
            {
                lock (myLock)
                {
                    _Batt_setSOC = value;
                }
            }
        }
        private int _Batt_coefficients;
        public int Batt_coefficients
        {
            get
            {
                lock (myLock)
                {
                    return _Batt_coefficients;
                }
            }
            set
            {
                lock (myLock)
                {
                    _Batt_coefficients = value;
                }
            }
        }
        private int _Batt_Calibration_manual;
        public int Batt_Calibration_manual
        {
            get
            {
                lock (myLock)
                {
                    return _Batt_Calibration_manual;
                }
            }
            set
            {
                lock (myLock)
                {
                    _Batt_Calibration_manual = value;
                }
            }
        }

        private int _Batt_onlyForEnginneringTeam;
        public int Batt_onlyForEnginneringTeam
        {
            get
            {
                lock (myLock)
                {
                    return _Batt_onlyForEnginneringTeam;
                }
            }
            set
            {
                lock (myLock)
                {
                    _Batt_onlyForEnginneringTeam = value;
                }
            }
        }
        private int _Batt_AdminACTViewID;
        public int Batt_AdminACTViewID
        {
            get
            {
                lock (myLock)
                {
                    return _Batt_AdminACTViewID;
                }
            }
            set
            {
                lock (myLock)
                {
                    _Batt_AdminACTViewID = value;
                }
            }
        }
        private int _Batt_eventsControl;
        public int Batt_eventsControl
        {
            get
            {
                lock (myLock)
                {
                    return _Batt_eventsControl;
                }
            }
            set
            {
                lock (myLock)
                {
                    _Batt_eventsControl = value;
                }
            }
        }
        private int _Batt_COMMISSION;
        public int Batt_COMMISSION
        {
            get
            {
                lock (myLock)
                {
                    return _Batt_COMMISSION;
                }
            }
            set
            {
                lock (myLock)
                {
                    _Batt_COMMISSION = value;
                }
            }
        }


        private int _Batt_Commission_Daughter_Card;
        public int Batt_Commission_Daughter_Card
        {
            get
            {
                lock (myLock)
                {
                    return _Batt_Commission_Daughter_Card;
                }
            }
            set
            {
                lock (myLock)
                {
                    _Batt_Commission_Daughter_Card = value;
                }
            }
        }
        #endregion

    }
}
