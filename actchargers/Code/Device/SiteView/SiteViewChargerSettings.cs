using System;
namespace actchargers
{
    public class ChargerSettings
    {
        #region settings
        private byte _zoneID;
        private bool zoneID_isDirty;
        public byte zoneID
        {
            get { lock (myLock) { return _zoneID; } }
            set
            {
                lock (myLock)
                {

                    _zoneID = value;
                    zoneID_isDirty = true;
                }
            }
        }

        public string model
        {
            get { lock (myLock) { return _model; } }
            set
            {
                lock (myLock)
                {

                    _model = value;
                    model_isDirty = true;
                }
            }
        }
        private string _model;
        private bool model_isDirty;

        public string actAccessSSID
        {
            get { lock (myLock) { return _actAccessSSID; } }
            set
            {
                lock (myLock)
                {

                    _actAccessSSID = value;
                    actAccessSSID_isDirty = true;
                }
            }
        }
        private string _actAccessSSID;
        private bool actAccessSSID_isDirty;

        public string mobileAccessSSID
        {
            get { lock (myLock) { return _mobileAccessSSID; } }
            set
            {
                lock (myLock)
                {

                    _mobileAccessSSID = value;
                    mobileAccessSSID_isDirty = true;
                }
            }
        }
        private string _mobileAccessSSID;
        private bool mobileAccessSSID_isDirty;

        public string actViewIP
        {
            get { lock (myLock) { return _actViewIP; } }
            set
            {
                lock (myLock)
                {

                    _actViewIP = value;
                    actViewIP_isDirty = true;
                }
            }
        }
        private string _actViewIP;
        private bool actViewIP_isDirty;

        public string actAccessPassword
        {
            get { lock (myLock) { return _actAccessPassword; } }
            set
            {
                lock (myLock)
                {

                    _actAccessPassword = value;
                    actAccessPassword_isDirty = true;
                }
            }
        }
        private string _actAccessPassword;
        private bool actAccessPassword_isDirty;

        public string mobileAccessPassword
        {
            get { lock (myLock) { return _mobileAccessPassword; } }
            set
            {
                lock (myLock)
                {

                    _mobileAccessPassword = value;
                    mobileAccessPassword_isDirty = true;
                }
            }
        }
        private string _mobileAccessPassword;
        private bool mobileAccessPassword_isDirty;

        private bool _actViewEnable;
        private bool actViewEnable_isDirty;
        public bool actViewEnable
        {
            get { lock (myLock) { return _actViewEnable; } }
            set
            {
                lock (myLock)
                {

                    _actViewEnable = value;
                    actViewEnable_isDirty = true;
                }
            }
        }

        private string _actViewConnectFrequency;
        private bool actViewConnectFrequency_isDirty;
        public string actViewConnectFrequency
        {
            get { lock (myLock) { return _actViewConnectFrequency; } }
            set
            {
                lock (myLock)
                {

                    _actViewConnectFrequency = value;
                    actViewConnectFrequency_isDirty = true;
                }
            }
        }


        private DateTime _InstallationDate;
        private bool InstallationDate_isDirty;
        public DateTime InstallationDate
        {
            get { lock (myLock) { return new DateTime(_InstallationDate.Ticks); } }
            set
            {
                lock (myLock)
                {

                    _InstallationDate = value;
                    InstallationDate_isDirty = true;
                }
            }
        }


        private string _trickleVoltage;
        private bool trickleVoltage_isDirty;
        public string trickleVoltage
        {
            get { lock (myLock) { return _trickleVoltage; } }
            set
            {
                lock (myLock)
                {

                    _trickleVoltage = value;
                    trickleVoltage_isDirty = true;
                }
            }
        }

        private string _CVvoltage;
        private bool CVvoltage_isDirty;
        public string CVvoltage
        {
            get { lock (myLock) { return _CVvoltage; } }
            set
            {
                lock (myLock)
                {

                    _CVvoltage = value;
                    CVvoltage_isDirty = true;
                }
            }
        }

        private string _FIvoltage;
        private bool FIvoltage_isDirty;
        public string FIvoltage
        {
            get { lock (myLock) { return _FIvoltage; } }
            set
            {
                lock (myLock)
                {

                    _FIvoltage = value;
                    FIvoltage_isDirty = true;
                }
            }
        }

        private string _EQvoltage;
        private bool EQvoltage_isDirty;
        public string EQvoltage
        {
            get { lock (myLock) { return _EQvoltage; } }
            set
            {
                lock (myLock)
                {

                    _EQvoltage = value;
                    EQvoltage_isDirty = true;
                }
            }
        }

        private string _batteryType;
        private bool batteryType_isDirty;
        public string batteryType
        {
            get { lock (myLock) { return _batteryType; } }
            set
            {
                lock (myLock)
                {

                    _batteryType = value;
                    batteryType_isDirty = true;
                }
            }
        }

        public bool IsbatteryTypeDirty()
        {
            lock (myLock) { return batteryType_isDirty; }
        }
        private string _temperatureVoltageCompensation;
        private bool temperatureVoltageCompensation_isDirty;
        public string temperatureVoltageCompensation
        {
            get { lock (myLock) { return _temperatureVoltageCompensation; } }
            set
            {
                lock (myLock)
                {

                    _temperatureVoltageCompensation = value;
                    temperatureVoltageCompensation_isDirty = true;
                }
            }
        }

        private string _maxTemperatureFault;
        private bool maxTemperatureFault_isDirty;
        public string maxTemperatureFault
        {
            get { lock (myLock) { return _maxTemperatureFault; } }
            set
            {
                lock (myLock)
                {

                    _maxTemperatureFault = value;
                    maxTemperatureFault_isDirty = true;
                }
            }
        }

        private string _batteryVoltage;
        private bool batteryVoltage_isDirty;
        public string batteryVoltage
        {
            get { lock (myLock) { return _batteryVoltage; } }
            set
            {
                lock (myLock)
                {

                    _batteryVoltage = value;
                    batteryVoltage_isDirty = true;
                }
            }
        }

        private UInt16 _CCrate;
        private bool CCrate_isDirty;
        public UInt16 CCrate
        {
            get { lock (myLock) { return _CCrate; } }
            set
            {
                lock (myLock)
                {

                    _CCrate = value;
                    CCrate_isDirty = true;
                }
            }
        }

        private UInt16 _TRrate;
        private bool TRrate_isDirty;
        public UInt16 TRrate
        {
            get { lock (myLock) { return _TRrate; } }
            set
            {
                lock (myLock)
                {

                    _TRrate = value;
                    TRrate_isDirty = true;
                }
            }
        }

        private UInt16 _FIrate;
        private bool FIrate_isDirty;
        public UInt16 FIrate
        {
            get { lock (myLock) { return _FIrate; } }
            set
            {
                lock (myLock)
                {

                    _FIrate = value;
                    FIrate_isDirty = true;
                }
            }
        }

        private UInt16 _EQrate;
        private bool EQrate_isDirty;
        public UInt16 EQrate
        {
            get { lock (myLock) { return _EQrate; } }
            set
            {
                lock (myLock)
                {

                    _EQrate = value;
                    EQrate_isDirty = true;
                }
            }
        }

        private string _PMefficiency;
        private bool PMefficiency_isDirty;
        public string PMefficiency
        {
            get { lock (myLock) { return _PMefficiency; } }
            set
            {
                lock (myLock)
                {

                    _PMefficiency = value;
                    PMefficiency_isDirty = true;
                }
            }
        }

        private string _actViewPort;
        private bool actViewPort_isDirty;
        public string actViewPort
        {
            get { lock (myLock) { return _actViewPort; } }
            set
            {
                lock (myLock)
                {

                    _actViewPort = value;
                    actViewPort_isDirty = true;
                }
            }
        }

        private string _mobilePort;
        private bool mobilePort_isDirty;
        public string mobilePort
        {
            get { lock (myLock) { return _mobilePort; } }
            set
            {
                lock (myLock)
                {

                    _mobilePort = value;
                    mobilePort_isDirty = true;
                }
            }
        }

        private byte _finishDV;
        private bool finishDV_isDirty;
        public byte finishDV
        {
            get { lock (myLock) { return _finishDV; } }
            set
            {
                lock (myLock)
                {

                    _finishDV = value;
                    finishDV_isDirty = true;
                }
            }
        }

        private byte _CVcurrentStep;
        private bool CVcurrentStep_isDirty;
        public byte CVcurrentStep
        {
            get { lock (myLock) { return _CVcurrentStep; } }
            set
            {
                lock (myLock)
                {

                    _CVcurrentStep = value;
                    CVcurrentStep_isDirty = true;
                }
            }
        }

        private byte _CVfinishCurrent;
        private bool CVfinishCurrent_isDirty;
        public byte CVfinishCurrent
        {
            get { lock (myLock) { return _CVfinishCurrent; } }
            set
            {
                lock (myLock)
                {

                    _CVfinishCurrent = value;
                    CVfinishCurrent_isDirty = true;
                }
            }
        }
        //setting

        private float _IR;
        private bool IR_isDirty;
        public float IR
        {
            get { lock (myLock) { return _IR; } }
            set
            {
                lock (myLock)
                {

                    _IR = value;
                    IR_isDirty = true;
                }
            }
        }

        private bool _enableAutoDetectMultiVoltage;
        private bool enableAutoDetectMultiVoltage_isDirty;
        public bool enableAutoDetectMultiVoltage
        {
            get { lock (myLock) { return _enableAutoDetectMultiVoltage; } }
            set
            {
                lock (myLock)
                {

                    _enableAutoDetectMultiVoltage = value;
                    enableAutoDetectMultiVoltage_isDirty = true;
                }
            }
        }


        private bool _enableRefreshCycleAfterFI;
        private bool enableRefreshCycleAfterFI_isDirty;
        public bool enableRefreshCycleAfterFI
        {
            get { lock (myLock) { return _enableRefreshCycleAfterFI; } }
            set
            {
                lock (myLock)
                {

                    _enableRefreshCycleAfterFI = value;
                    enableRefreshCycleAfterFI_isDirty = true;
                }
            }
        }

        private bool _enableRefreshCycleAfterEQ;
        private bool enableRefreshCycleAfterEQ_isDirty;
        public bool enableRefreshCycleAfterEQ
        {
            get { lock (myLock) { return _enableRefreshCycleAfterEQ; } }
            set
            {
                lock (myLock)
                {

                    _enableRefreshCycleAfterEQ = value;
                    enableRefreshCycleAfterEQ_isDirty = true;
                }
            }
        }


        private byte _chargerType;
        private bool chargerType_isDirty;
        public byte chargerType
        {
            get { lock (myLock) { return _chargerType; } }
            set
            {
                lock (myLock)
                {

                    _chargerType = value;
                    chargerType_isDirty = true;
                }
            }
        }

        public bool ischargerTypeDirty()
        {
            lock (myLock) { return chargerType_isDirty; }

        }
        private string _batteryCapacity;
        private bool batteryCapacity_isDirty;
        public string batteryCapacity
        {
            get { lock (myLock) { return _batteryCapacity; } }
            set
            {
                lock (myLock)
                {

                    _batteryCapacity = value;
                    batteryCapacity_isDirty = true;
                }
            }
        }

        private string _EQstartWindow;
        private bool EQstartWindow_isDirty;
        public string EQstartWindow
        {
            get { lock (myLock) { return _EQstartWindow; } }
            set
            {
                lock (myLock)
                {

                    _EQstartWindow = value;
                    EQstartWindow_isDirty = true;
                }
            }
        }

        private string _FIstartWindow;
        private bool FIstartWindow_isDirty;
        public string FIstartWindow
        {
            get { lock (myLock) { return _FIstartWindow; } }
            set
            {
                lock (myLock)
                {

                    _FIstartWindow = value;
                    FIstartWindow_isDirty = true;
                }
            }
        }

        private string _autoStartCountDownTimer;
        private bool autoStartCountDownTimer_isDirty;
        public string autoStartCountDownTimer
        {
            get { lock (myLock) { return _autoStartCountDownTimer; } }
            set
            {
                lock (myLock)
                {

                    _autoStartCountDownTimer = value;
                    autoStartCountDownTimer_isDirty = true;
                }
            }
        }

        private DaysMask _finishDaysMask;
        private bool finishDaysMask_isDirty;
        public DaysMask finishDaysMask
        {
            get { lock (myLock) { return _finishDaysMask; } }
            set
            {
                lock (myLock)
                {

                    _finishDaysMask = value;
                    finishDaysMask_isDirty = true;
                }
            }
        }


        private bool _autoStartEnable;
        private bool autoStartEnable_isDirty;
        public bool autoStartEnable
        {
            get { lock (myLock) { return _autoStartEnable; } }
            set
            {
                lock (myLock)
                {

                    _autoStartEnable = value;
                    autoStartEnable_isDirty = true;
                }
            }
        }


        private DaysMask _autoStartMask;
        private bool autoStartMask_isDirty;
        public DaysMask autoStartMask
        {
            get { lock (myLock) { return _autoStartMask; } }
            set
            {
                lock (myLock)
                {

                    _autoStartMask = value;
                    autoStartMask_isDirty = true;
                }
            }
        }

        private bool _FIschedulingMode;
        private bool FIschedulingMode_isDirty;
        public bool FIschedulingMode
        {
            get { lock (myLock) { return _FIschedulingMode; } }
            set
            {
                lock (myLock)
                {

                    _FIschedulingMode = value;
                    FIschedulingMode_isDirty = true;
                }
            }
        }


        private string _finishWindow;
        private bool finishWindow_isDirty;
        public string finishWindow
        {
            get { lock (myLock) { return _finishWindow; } }
            set
            {
                lock (myLock)
                {

                    _finishWindow = value;
                    finishWindow_isDirty = true;
                }
            }
        }

        private DaysMask _EQdaysMask;
        private bool EQdaysMask_isDirty;
        public DaysMask EQdaysMask
        {
            get { lock (myLock) { return _EQdaysMask; } }
            set
            {
                lock (myLock)
                {

                    _EQdaysMask = value;
                    EQdaysMask_isDirty = true;
                }
            }
        }

        private string _EQwindow;
        private bool EQwindow_isDirty;
        public string EQwindow
        {
            get { lock (myLock) { return _EQwindow; } }
            set
            {
                lock (myLock)
                {

                    _EQwindow = value;
                    EQwindow_isDirty = true;
                }
            }
        }

        private bool _daylightSaving;
        private bool daylightSaving_isDirty;
        public bool daylightSaving
        {
            get { lock (myLock) { return _daylightSaving; } }
            set
            {
                lock (myLock)
                {

                    _daylightSaving = value;
                    daylightSaving_isDirty = true;
                }
            }
        }

        private bool _temperatureFormat;
        private bool temperatureFormat_isDirty;
        public bool temperatureFormat
        {
            get { lock (myLock) { return _temperatureFormat; } }
            set
            {
                lock (myLock)
                {

                    _temperatureFormat = value;
                    temperatureFormat_isDirty = true;
                }
            }
        }

        private string _PMvoltage;
        private bool PMvoltage_isDirty;
        public string PMvoltage
        {
            get { lock (myLock) { return _PMvoltage; } }
            set
            {
                lock (myLock)
                {

                    _PMvoltage = value;
                    PMvoltage_isDirty = true;
                }
            }
        }

        private string _CVtimer;
        private bool CVtimer_isDirty;
        public string CVtimer
        {
            get { lock (myLock) { return _CVtimer; } }
            set
            {
                lock (myLock)
                {

                    _CVtimer = value;
                    CVtimer_isDirty = true;
                }
            }
        }

        private string _finishTimer;
        private bool finishTimer_isDirty;
        public string finishTimer
        {
            get { lock (myLock) { return _finishTimer; } }
            set
            {
                lock (myLock)
                {

                    _finishTimer = value;
                    finishTimer_isDirty = true;
                }
            }
        }

        private string _finishDT;
        private bool finishDT_isDirty;
        public string finishDT
        {
            get { lock (myLock) { return _finishDT; } }
            set
            {
                lock (myLock)
                {

                    _finishDT = value;
                    finishDT_isDirty = true;
                }
            }
        }

        private string _EQtimer;
        private bool EQtimer_isDirty;
        public string EQtimer
        {
            get { lock (myLock) { return _EQtimer; } }
            set
            {
                lock (myLock)
                {

                    _EQtimer = value;
                    EQtimer_isDirty = true;
                }
            }
        }

        private string _refreshTimer;
        private bool refreshTimer_isDirty;
        public string refreshTimer
        {
            get { lock (myLock) { return _refreshTimer; } }
            set
            {
                lock (myLock)
                {

                    _refreshTimer = value;
                    refreshTimer_isDirty = true;
                }
            }
        }

        private string _desulfationTimer;
        private bool desulfationTimer_isDirty;
        public string desulfationTimer
        {
            get { lock (myLock) { return _desulfationTimer; } }
            set
            {
                lock (myLock)
                {

                    _desulfationTimer = value;
                    desulfationTimer_isDirty = true;
                }
            }
        }

        private string _lcdMemoryVersion;
        private bool lcdMemoryVersion_isDirty;
        public string lcdMemoryVersion
        {
            get { lock (myLock) { return _lcdMemoryVersion; } }
            set
            {
                lock (myLock)
                {

                    _lcdMemoryVersion = value;
                    lcdMemoryVersion_isDirty = true;
                }
            }
        }

        private string _wifiFirmwareVersion;
        private bool wifiFirmwareVersion_isDirty;
        public string wifiFirmwareVersion
        {
            get { lock (myLock) { return _wifiFirmwareVersion; } }
            set
            {
                lock (myLock)
                {

                    _wifiFirmwareVersion = value;
                    wifiFirmwareVersion_isDirty = true;
                }
            }
        }

        private bool _enablePLC;
        private bool enablePLC_isDirty;
        public bool enablePLC
        {
            get { lock (myLock) { return _enablePLC; } }
            set
            {
                lock (myLock)
                {

                    _enablePLC = value;
                    enablePLC_isDirty = true;
                }
            }
        }

        private bool _enableManualEQ;
        private bool enableManualEQ_isDirty;
        public bool enableManualEQ
        {
            get { lock (myLock) { return _enableManualEQ; } }
            set
            {
                lock (myLock)
                {

                    _enableManualEQ = value;
                    enableManualEQ_isDirty = true;
                }
            }
        }

        private bool _enableManualDesulfate;
        private bool enableManualDesulfate_isDirty;
        public bool enableManualDesulfate
        {
            get { lock (myLock) { return _enableManualDesulfate; } }
            set
            {
                lock (myLock)
                {

                    _enableManualDesulfate = value;
                    enableManualDesulfate_isDirty = true;
                }
            }
        }

        private DaysMask _energyDaysMask;
        private bool energyDaysMask_isDirty;
        public DaysMask energyDaysMask
        {
            get { lock (myLock) { return _energyDaysMask; } }
            set
            {
                lock (myLock)
                {

                    _energyDaysMask = value;
                    energyDaysMask_isDirty = true;
                }
            }
        }

        private UInt32 _lockoutStartTime;
        private bool lockoutStartTime_isDirty;
        public UInt32 lockoutStartTime
        {
            get { lock (myLock) { return _lockoutStartTime; } }
            set
            {
                lock (myLock)
                {

                    _lockoutStartTime = value;
                    lockoutStartTime_isDirty = true;
                }
            }
        }

        private UInt32 _lockoutCloseTime;
        private bool lockoutCloseTime_isDirty;
        public UInt32 lockoutCloseTime
        {
            get { lock (myLock) { return _lockoutCloseTime; } }
            set
            {
                lock (myLock)
                {

                    _lockoutCloseTime = value;
                    lockoutCloseTime_isDirty = true;
                }
            }
        }

        private UInt32 _energyStartTime;
        private bool energyStartTime_isDirty;
        public UInt32 energyStartTime
        {
            get { lock (myLock) { return _energyStartTime; } }
            set
            {
                lock (myLock)
                {

                    _energyStartTime = value;
                    energyStartTime_isDirty = true;
                }
            }
        }

        private UInt32 _energyCloseTime;
        private bool energyCloseTime_isDirty;
        public UInt32 energyCloseTime
        {
            get { lock (myLock) { return _energyCloseTime; } }
            set
            {
                lock (myLock)
                {

                    _energyCloseTime = value;
                    energyCloseTime_isDirty = true;
                }
            }
        }

        private DaysMask _lockoutDaysMask;
        private bool lockoutDaysMask_isDirty;
        public DaysMask lockoutDaysMask
        {
            get { lock (myLock) { return _lockoutDaysMask; } }
            set
            {
                lock (myLock)
                {

                    _lockoutDaysMask = value;
                    lockoutDaysMask_isDirty = true;
                }
            }
        }

        private byte _energyDecreaseValue;
        private bool energyDecreaseValue_isDirty;
        public byte energyDecreaseValue
        {
            get { lock (myLock) { return _energyDecreaseValue; } }
            set
            {
                lock (myLock)
                {

                    _energyDecreaseValue = value;
                    energyDecreaseValue_isDirty = true;
                }
            }
        }

        private byte _PMvoltageInputValue;
        private bool PMvoltageInputValue_isDirty;
        public byte PMvoltageInputValue
        {
            get { lock (myLock) { return _PMvoltageInputValue; } }
            set
            {
                lock (myLock)
                {

                    _PMvoltageInputValue = value;
                    PMvoltageInputValue_isDirty = true;
                }
            }
        }

        private bool _disablePushButton;
        private bool disablePushButton_isDirty;
        public bool disablePushButton
        {
            get { lock (myLock) { return _disablePushButton; } }
            set
            {
                lock (myLock)
                {

                    _disablePushButton = value;
                    disablePushButton_isDirty = true;
                }
            }
        }

        private string _TRtemperature;
        private bool TRtemperature_isDirty;
        public string TRtemperature
        {
            get { lock (myLock) { return _TRtemperature; } }
            set
            {
                lock (myLock)
                {

                    _TRtemperature = value;
                    TRtemperature_isDirty = true;
                }
            }
        }

        private UInt32 _afterCommissionBoardID;
        private bool afterCommissionBoardID_isDirty;
        public UInt32 afterCommissionBoardID
        {
            get { lock (myLock) { return _afterCommissionBoardID; } }
            set
            {
                lock (myLock)
                {

                    _afterCommissionBoardID = value;
                    afterCommissionBoardID_isDirty = true;
                }
            }
        }

        private string _foldTemperature;
        private bool foldTemperature_isDirty;
        public string foldTemperature
        {
            get { lock (myLock) { return _foldTemperature; } }
            set
            {
                lock (myLock)
                {

                    _foldTemperature = value;
                    foldTemperature_isDirty = true;
                }
            }
        }

        private string _coolDownTemperature;
        private bool coolDownTemperature_isDirty;
        public string coolDownTemperature
        {
            get { lock (myLock) { return _coolDownTemperature; } }
            set
            {
                lock (myLock)
                {

                    _coolDownTemperature = value;
                    coolDownTemperature_isDirty = true;
                }
            }
        }

        private byte _ledcontrol;
        private bool ledcontrol_isDirty;
        public byte ledcontrol
        {
            get { lock (myLock) { return _ledcontrol; } }
            set
            {
                lock (myLock)
                {

                    _ledcontrol = value;
                    ledcontrol_isDirty = true;
                }
            }
        }

        private UInt16 _BatteryCapacity24;
        private bool BatteryCapacity24_isDirty;
        public UInt16 batteryCapacity24
        {
            get { lock (myLock) { return _BatteryCapacity24; } }
            set
            {
                lock (myLock)
                {

                    _BatteryCapacity24 = value;
                    BatteryCapacity24_isDirty = true;
                }
            }
        }

        private UInt16 _BatteryCapacity36;
        private bool BatteryCapacity36_isDirty;
        public UInt16 batteryCapacity36
        {
            get { lock (myLock) { return _BatteryCapacity36; } }
            set
            {
                lock (myLock)
                {

                    _BatteryCapacity36 = value;
                    BatteryCapacity36_isDirty = true;
                }
            }
        }

        private UInt16 _BatteryCapacity48;
        private bool BatteryCapacity48_isDirty;
        public UInt16 batteryCapacity48
        {
            get { lock (myLock) { return _BatteryCapacity48; } }
            set
            {
                lock (myLock)
                {

                    _BatteryCapacity48 = value;
                    BatteryCapacity48_isDirty = true;
                }
            }
        }
        private UInt16 _BatteryCapacity80;
        private bool BatteryCapacity80_isDirty;
        public UInt16 batteryCapacity80
        {
            get { lock (myLock) { return _BatteryCapacity80; } }
            set
            {
                lock (myLock)
                {

                    _BatteryCapacity80 = value;
                    BatteryCapacity80_isDirty = true;
                }
            }
        }
        private bool _forceFinishTimeout;
        private bool forceFinishTimeout_isDirty;
        public bool forceFinishTimeout
        {
            get { lock (myLock) { return _forceFinishTimeout; } }
            set
            {
                lock (myLock)
                {

                    _forceFinishTimeout = value;
                    forceFinishTimeout_isDirty = true;
                }
            }
        }

        private bool _chargerOverrideBattviewFIEQsched;
        private bool chargerOverrideBattviewFIEQsched_isDirty;
        public bool chargerOverrideBattviewFIEQsched
        {
            get { lock (myLock) { return _chargerOverrideBattviewFIEQsched; } }
            set
            {
                lock (myLock)
                {

                    _chargerOverrideBattviewFIEQsched = value;
                    chargerOverrideBattviewFIEQsched_isDirty = true;
                }
            }
        }

        private bool _ignoreBATTViewSOC;
        private bool ignoreBATTViewSOC_isDirty;
        public bool ignoreBATTViewSOC
        {
            get { lock (myLock) { return _ignoreBATTViewSOC; } }
            set
            {
                lock (myLock)
                {

                    _ignoreBATTViewSOC = value;
                    ignoreBATTViewSOC_isDirty = true;
                }
            }
        }

        private bool _battviewAutoCalibrationEnable;
        private bool battviewAutoCalibrationEnable_isDirty;
        public bool battviewAutoCalibrationEnable
        {
            get { lock (myLock) { return _battviewAutoCalibrationEnable; } }
            set
            {
                lock (myLock)
                {

                    _battviewAutoCalibrationEnable = value;
                    battviewAutoCalibrationEnable_isDirty = true;
                }
            }
        }

        private byte _cc_ramping_min_steps;
        private bool cc_ramping_min_steps_isDirty;
        public byte cc_ramping_min_steps
        {
            get { lock (myLock) { return _cc_ramping_min_steps; } }
            set
            {
                lock (myLock)
                {

                    _cc_ramping_min_steps = value;
                    cc_ramping_min_steps_isDirty = true;
                }
            }
        }

        public void cc_ramping_min_steps_reset()
        {
            lock (myLock)
                cc_ramping_min_steps_isDirty = false;
        }
        private sbyte _nominal_temperature_shift;
        private bool nominal_temperature_shift_isDirty;
        public sbyte nominal_temperature_shift
        {
            get { lock (myLock) { return _nominal_temperature_shift; } }
            set
            {
                lock (myLock)
                {

                    _nominal_temperature_shift = value;
                    nominal_temperature_shift_isDirty = true;
                }
            }
        }

        #endregion
        #region constructor
        object myLock;
        internal ChargerSettings()
        {
            myLock = new object();
            InstallationDate = new DateTime();
            this.finishDaysMask = new DaysMask();
            this.autoStartMask = new DaysMask();
            this.EQdaysMask = new DaysMask();
            this.energyDaysMask = new DaysMask();
            this.lockoutDaysMask = new DaysMask();

            zoneID_isDirty = false;
            model_isDirty = false;
            actAccessSSID_isDirty = false;
            mobileAccessSSID_isDirty = false;
            actViewIP_isDirty = false;
            actAccessPassword_isDirty = false;
            mobileAccessPassword_isDirty = false;
            actViewEnable_isDirty = false;
            actViewConnectFrequency_isDirty = false;
            InstallationDate_isDirty = false;
            trickleVoltage_isDirty = false;
            CVvoltage_isDirty = false;
            FIvoltage_isDirty = false;
            EQvoltage_isDirty = false;
            batteryType_isDirty = false;
            temperatureVoltageCompensation_isDirty = false;
            maxTemperatureFault_isDirty = false;
            batteryVoltage_isDirty = false;
            CCrate_isDirty = false;
            TRrate_isDirty = false;
            FIrate_isDirty = false;
            EQrate_isDirty = false;
            PMefficiency_isDirty = false;
            actViewPort_isDirty = false;
            mobilePort_isDirty = false;
            finishDV_isDirty = false;
            CVcurrentStep_isDirty = false;
            CVfinishCurrent_isDirty = false;
            IR_isDirty = false;
            enableAutoDetectMultiVoltage_isDirty = false;
            enableRefreshCycleAfterFI_isDirty = false;
            enableRefreshCycleAfterEQ_isDirty = false;
            chargerType_isDirty = false;
            batteryCapacity_isDirty = false;
            EQstartWindow_isDirty = false;
            FIstartWindow_isDirty = false;
            autoStartCountDownTimer_isDirty = false;
            finishDaysMask_isDirty = false;
            autoStartEnable_isDirty = false;
            autoStartMask_isDirty = false;
            FIschedulingMode_isDirty = false;
            finishWindow_isDirty = false;
            EQdaysMask_isDirty = false;
            EQwindow_isDirty = false;
            daylightSaving_isDirty = false;
            temperatureFormat_isDirty = false;
            PMvoltage_isDirty = false;
            CVtimer_isDirty = false;
            finishTimer_isDirty = false;
            finishDT_isDirty = false;
            EQtimer_isDirty = false;
            refreshTimer_isDirty = false;
            desulfationTimer_isDirty = false;
            lcdMemoryVersion_isDirty = false;
            wifiFirmwareVersion_isDirty = false;
            enablePLC_isDirty = false;
            enableManualEQ_isDirty = false;
            enableManualDesulfate_isDirty = false;
            energyDaysMask_isDirty = false;
            lockoutStartTime_isDirty = false;
            lockoutCloseTime_isDirty = false;
            energyStartTime_isDirty = false;
            energyCloseTime_isDirty = false;
            lockoutDaysMask_isDirty = false;
            energyDecreaseValue_isDirty = false;
            PMvoltageInputValue_isDirty = false;
            disablePushButton_isDirty = false;
            TRtemperature_isDirty = false;
            afterCommissionBoardID_isDirty = false;
            foldTemperature_isDirty = false;
            coolDownTemperature_isDirty = false;
            ledcontrol_isDirty = false;
            BatteryCapacity24_isDirty = false;
            BatteryCapacity36_isDirty = false;
            BatteryCapacity48_isDirty = false;
            BatteryCapacity80_isDirty = false;
            forceFinishTimeout_isDirty = false;
            chargerOverrideBattviewFIEQsched_isDirty = false;
            ignoreBATTViewSOC_isDirty = false;
            battviewAutoCalibrationEnable_isDirty = false;
            cc_ramping_min_steps_isDirty = false;
            nominal_temperature_shift_isDirty = false;

        }
        #endregion
        #region charger reset
        public void ResetChargerInfo()
        {
            lock (myLock)
            {
                InstallationDate_isDirty = false;
                chargerType_isDirty = false;
                zoneID_isDirty = false;
                model_isDirty = false;
                lcdMemoryVersion_isDirty = false;
                wifiFirmwareVersion_isDirty = false;
            }
        }

        public void ResetChargerSettings()
        {
            lock (myLock)
            {
                IR_isDirty = false;
                autoStartEnable_isDirty = false;
                autoStartCountDownTimer_isDirty = false;
                enableRefreshCycleAfterEQ_isDirty = false;
                enableRefreshCycleAfterFI_isDirty = false;
                refreshTimer_isDirty = false;
                enableAutoDetectMultiVoltage_isDirty = false;
                temperatureFormat_isDirty = false;
                daylightSaving_isDirty = false;
                enablePLC_isDirty = false;
                enableManualEQ_isDirty = false;
                enableManualDesulfate_isDirty = false;
                disablePushButton_isDirty = false;
                ledcontrol_isDirty = false;
                chargerOverrideBattviewFIEQsched_isDirty = false;
                ignoreBATTViewSOC_isDirty = false;
                battviewAutoCalibrationEnable_isDirty = false;
                cc_ramping_min_steps_isDirty = false;
            }
        }


        public void ResetWiFi()
        {
            lock (myLock)
            {
                mobileAccessPassword_isDirty = false;
                mobileAccessSSID_isDirty = false;
                mobilePort_isDirty = false;
                actViewEnable_isDirty = actViewIP_isDirty = actViewPort_isDirty
                    = actViewConnectFrequency_isDirty = actAccessPassword_isDirty = actAccessSSID_isDirty = false;

            }
        }
        public void ResetBatterySettings()
        {
            BatteryCapacity24_isDirty = BatteryCapacity36_isDirty = BatteryCapacity48_isDirty = BatteryCapacity80_isDirty = batteryCapacity_isDirty =
                nominal_temperature_shift_isDirty = batteryVoltage_isDirty = temperatureVoltageCompensation_isDirty =
                maxTemperatureFault_isDirty = TRtemperature_isDirty = foldTemperature_isDirty = coolDownTemperature_isDirty =
                batteryType_isDirty = false;
        }
        public void resetChargeProfile()
        {
            TRrate_isDirty = CCrate_isDirty = FIrate_isDirty = EQrate_isDirty = trickleVoltage_isDirty = CVvoltage_isDirty = FIvoltage_isDirty = EQvoltage_isDirty
                = CVfinishCurrent_isDirty = CVcurrentStep_isDirty = CVtimer_isDirty = finishTimer_isDirty = forceFinishTimeout_isDirty = EQtimer_isDirty
                = desulfationTimer_isDirty = finishDV_isDirty = finishDT_isDirty = false;


        }
        public void resetChargeFIEQ()
        {

            finishDaysMask_isDirty = FIstartWindow_isDirty = finishWindow_isDirty =
                            FIschedulingMode_isDirty = EQstartWindow_isDirty = EQwindow_isDirty = EQdaysMask_isDirty = false;


        }

        public void resetEnergyManagement()
        {
            lockoutDaysMask_isDirty =
            lockoutStartTime_isDirty = lockoutCloseTime_isDirty = energyDaysMask_isDirty = energyDecreaseValue_isDirty = energyStartTime_isDirty =
                energyDecreaseValue_isDirty = energyCloseTime_isDirty = false;
        }

        public void resetPMInfo()
        {
            PMvoltage_isDirty = PMvoltageInputValue_isDirty = PMefficiency_isDirty = false;
        }
        #endregion
        public bool anyChangeToCharger()
        {
            lock (myLock) return InstallationDate_isDirty || chargerType_isDirty || zoneID_isDirty
                    || lcdMemoryVersion_isDirty || wifiFirmwareVersion_isDirty ||
                    IR_isDirty || autoStartEnable_isDirty || autoStartCountDownTimer_isDirty ||
                    enableRefreshCycleAfterEQ_isDirty || enableRefreshCycleAfterFI_isDirty || refreshTimer_isDirty ||
                    enableAutoDetectMultiVoltage_isDirty || temperatureFormat_isDirty || daylightSaving_isDirty || enablePLC_isDirty
                    || enableManualEQ_isDirty || enableManualDesulfate_isDirty || disablePushButton_isDirty || ledcontrol_isDirty
                    || chargerOverrideBattviewFIEQsched_isDirty || ignoreBATTViewSOC_isDirty || battviewAutoCalibrationEnable_isDirty ||
                    cc_ramping_min_steps_isDirty || mobileAccessPassword_isDirty || mobileAccessSSID_isDirty || mobilePort_isDirty || actViewEnable_isDirty ||
                    actViewIP_isDirty || actViewPort_isDirty || actViewConnectFrequency_isDirty ||
                    actAccessPassword_isDirty || actAccessSSID_isDirty || BatteryCapacity24_isDirty || BatteryCapacity36_isDirty ||
                    BatteryCapacity48_isDirty || BatteryCapacity80_isDirty || batteryCapacity_isDirty || nominal_temperature_shift_isDirty || batteryVoltage_isDirty
                    || temperatureVoltageCompensation_isDirty || maxTemperatureFault_isDirty || TRtemperature_isDirty || foldTemperature_isDirty
                    || coolDownTemperature_isDirty || batteryType_isDirty || TRrate_isDirty || CCrate_isDirty || FIrate_isDirty
                    || EQrate_isDirty || trickleVoltage_isDirty || CVvoltage_isDirty || FIvoltage_isDirty || EQvoltage_isDirty ||
                    CVfinishCurrent_isDirty || CVcurrentStep_isDirty || CVtimer_isDirty || finishTimer_isDirty || forceFinishTimeout_isDirty ||
                    EQtimer_isDirty || desulfationTimer_isDirty || finishDV_isDirty || finishDT_isDirty || FIschedulingMode_isDirty ||
                    finishDaysMask_isDirty || FIstartWindow_isDirty || finishWindow_isDirty || EQstartWindow_isDirty || EQwindow_isDirty
                    || EQdaysMask_isDirty || lockoutDaysMask_isDirty || lockoutStartTime_isDirty || lockoutCloseTime_isDirty ||
                    energyDaysMask_isDirty || energyDecreaseValue_isDirty || energyStartTime_isDirty || energyDecreaseValue_isDirty
                    || energyCloseTime_isDirty || PMvoltage_isDirty || PMvoltageInputValue_isDirty || PMefficiency_isDirty;




        }

        public bool saveTime()
        {
            lock (myLock)
                return zoneID_isDirty;

        }

        public bool loadToConfig(MCBConfig config)
        {
            bool warning = false;
            lock (myLock)
            {
                if (zoneID_isDirty)
                    config.zoneID = _zoneID;
                if (model_isDirty)
                    config.model = _model;
                if (actAccessSSID_isDirty)
                    config.actAccessSSID = _actAccessSSID;
                if (mobileAccessSSID_isDirty)
                    config.mobileAccessSSID = _mobileAccessSSID;
                if (actViewIP_isDirty)
                    config.actViewIP = _actViewIP;
                if (actAccessPassword_isDirty)
                    config.actAccessPassword = _actAccessPassword;
                if (mobileAccessPassword_isDirty)
                    config.mobileAccessPassword = _mobileAccessPassword;
                if (actViewEnable_isDirty)
                    config.actViewEnable = _actViewEnable;
                if (actViewConnectFrequency_isDirty)
                    config.actViewConnectFrequency = _actViewConnectFrequency;
                if (InstallationDate_isDirty)
                    config.InstallationDate = _InstallationDate;

                bool cv_tr_ok = (float.Parse(CVvoltage_isDirty ? _CVvoltage : config.CVvoltage) >= float.Parse(trickleVoltage_isDirty ? _trickleVoltage : config.trickleVoltage));
                bool fi_cv_ok = (float.Parse(FIvoltage_isDirty ? _FIvoltage : config.FIvoltage) >= float.Parse(CVvoltage_isDirty ? _CVvoltage : config.CVvoltage));
                bool eq_fi_ok = (float.Parse(EQvoltage_isDirty ? _EQvoltage : config.EQvoltage) >= float.Parse(FIvoltage_isDirty ? _FIvoltage : config.FIvoltage));


                if ((!cv_tr_ok && (trickleVoltage_isDirty || CVvoltage_isDirty)) ||
                    (!fi_cv_ok && (FIvoltage_isDirty || CVvoltage_isDirty)) ||
                    (!eq_fi_ok && (EQvoltage_isDirty || FIvoltage_isDirty)))
                    warning = true;
                else
                {
                    if (trickleVoltage_isDirty)
                        config.trickleVoltage = _trickleVoltage;
                    if (CVvoltage_isDirty)
                        config.CVvoltage = _CVvoltage;
                    if (FIvoltage_isDirty)
                        config.FIvoltage = _FIvoltage;
                    if (EQvoltage_isDirty)
                        config.EQvoltage = _EQvoltage;
                }

                if (batteryType_isDirty)
                    config.batteryType = _batteryType;
                if (temperatureVoltageCompensation_isDirty)
                    config.temperatureVoltageCompensation = _temperatureVoltageCompensation;
                if (maxTemperatureFault_isDirty)
                    config.maxTemperatureFault = _maxTemperatureFault;
                if (batteryVoltage_isDirty)
                {
                    int pm_voltage;
                    string battery_voltage;
                    pm_voltage = int.Parse(config.PMvoltage);
                    if (PMvoltage_isDirty)
                    {
                        pm_voltage = int.Parse(_PMvoltage);
                    }
                    battery_voltage = _batteryVoltage;
                    if (pm_voltage < int.Parse(battery_voltage))
                    {
                        warning = true;
                        battery_voltage = pm_voltage.ToString();
                    }

                    config.batteryVoltage = battery_voltage;
                }

                bool cc_tr_ok = ((CCrate_isDirty ? _CCrate : config.CCrate) >= (TRrate_isDirty ? _TRrate : config.TRrate));
                bool cc_fi_ok = ((CCrate_isDirty ? _CCrate : config.CCrate) >= (FIrate_isDirty ? _FIrate : config.FIrate));
                bool cc_cv_ok = ((CCrate_isDirty ? _CCrate : config.CCrate) >= (50.0 * (CVfinishCurrent_isDirty ? _CVfinishCurrent : config.CVfinishCurrent)));
                bool fi_eq_ok = ((FIrate_isDirty ? _FIrate : config.FIrate) >= (EQrate_isDirty ? _EQrate : config.EQrate));


                if ((!cc_tr_ok && (TRrate_isDirty || CCrate_isDirty)) || (!cc_fi_ok && (FIrate_isDirty || CCrate_isDirty)) ||
                    (!cc_cv_ok && (CVfinishCurrent_isDirty || CCrate_isDirty)) || (!fi_eq_ok && (FIrate_isDirty || EQrate_isDirty)))
                {
                    warning = true;

                }
                else
                {
                    if (CCrate_isDirty)
                    {
                        config.CCrate = _CCrate;
                    }
                    if (TRrate_isDirty)
                        config.TRrate = _TRrate;
                    if (FIrate_isDirty && fi_eq_ok)
                        config.FIrate = _FIrate;
                    if (EQrate_isDirty)
                        config.EQrate = _EQrate;
                    if (CVfinishCurrent_isDirty)
                        config.CVfinishCurrent = _CVfinishCurrent;

                }
                if (PMefficiency_isDirty)
                    config.PMefficiency = _PMefficiency;
                if (actViewPort_isDirty)
                    config.actViewPort = _actViewPort;
                if (mobilePort_isDirty)
                    config.mobilePort = _mobilePort;
                if (finishDV_isDirty)
                    config.finishDV = _finishDV;
                if (CVcurrentStep_isDirty)
                    config.CVcurrentStep = _CVcurrentStep;
                if (IR_isDirty)
                    config.IR = _IR;
                if (enableAutoDetectMultiVoltage_isDirty)
                    config.enableAutoDetectMultiVoltage = _enableAutoDetectMultiVoltage;
                if (enableRefreshCycleAfterFI_isDirty)
                    config.enableRefreshCycleAfterFI = _enableRefreshCycleAfterFI;
                if (enableRefreshCycleAfterEQ_isDirty)
                    config.enableRefreshCycleAfterEQ = _enableRefreshCycleAfterEQ;
                if (chargerType_isDirty)
                    config.chargerType = _chargerType;
                if (batteryCapacity_isDirty)
                    config.batteryCapacity = _batteryCapacity;
                if (EQstartWindow_isDirty)
                    config.EQstartWindow = _EQstartWindow;
                if (FIstartWindow_isDirty)
                    config.FIstartWindow = _FIstartWindow;
                if (autoStartCountDownTimer_isDirty)
                    config.autoStartCountDownTimer = _autoStartCountDownTimer;
                if (finishDaysMask_isDirty)
                    config.finishDaysMask = _finishDaysMask;
                if (autoStartEnable_isDirty)
                    config.autoStartEnable = _autoStartEnable;
                if (autoStartMask_isDirty)
                    config.autoStartMask = _autoStartMask;
                if (FIschedulingMode_isDirty)
                    config.FIschedulingMode = _FIschedulingMode;
                if (finishWindow_isDirty)
                    config.finishWindow = _finishWindow;
                if (EQdaysMask_isDirty)
                    config.EQdaysMask = _EQdaysMask;
                if (EQwindow_isDirty)
                    config.EQwindow = _EQwindow;
                if (daylightSaving_isDirty)
                    config.daylightSaving = _daylightSaving;
                if (temperatureFormat_isDirty)
                    config.temperatureFormat = _temperatureFormat;
                if (PMvoltage_isDirty)
                    config.PMvoltage = _PMvoltage;
                if (CVtimer_isDirty)
                    config.CVtimer = _CVtimer;
                if (finishTimer_isDirty)
                    config.finishTimer = _finishTimer;
                if (finishDT_isDirty)
                    config.finishDT = _finishDT;
                if (EQtimer_isDirty)
                    config.EQtimer = _EQtimer;
                if (refreshTimer_isDirty)
                    config.refreshTimer = _refreshTimer;
                if (desulfationTimer_isDirty)
                    config.desulfationTimer = _desulfationTimer;
                if (lcdMemoryVersion_isDirty)
                    config.lcdMemoryVersion = _lcdMemoryVersion;
                if (wifiFirmwareVersion_isDirty)
                    config.wifiFirmwareVersion = _wifiFirmwareVersion;
                if (enablePLC_isDirty)
                    config.enablePLC = _enablePLC;
                if (enableManualEQ_isDirty)
                    config.enableManualEQ = _enableManualEQ;
                if (enableManualDesulfate_isDirty)
                    config.enableManualDesulfate = _enableManualDesulfate;
                if (energyDaysMask_isDirty)
                    config.energyDaysMask = _energyDaysMask;
                if (lockoutStartTime_isDirty)
                    config.lockoutStartTime = _lockoutStartTime;
                if (lockoutCloseTime_isDirty)
                    config.lockoutCloseTime = _lockoutCloseTime;
                if (energyStartTime_isDirty)
                    config.energyStartTime = _energyStartTime;
                if (energyCloseTime_isDirty)
                    config.energyCloseTime = _energyCloseTime;
                if (lockoutDaysMask_isDirty)
                    config.lockoutDaysMask = _lockoutDaysMask;
                if (energyDecreaseValue_isDirty)
                    config.energyDecreaseValue = _energyDecreaseValue;
                if (PMvoltageInputValue_isDirty)
                    config.PMvoltageInputValue = _PMvoltageInputValue;
                if (disablePushButton_isDirty)
                    config.disablePushButton = _disablePushButton;
                if (TRtemperature_isDirty)
                    config.TRtemperature = _TRtemperature;
                if (afterCommissionBoardID_isDirty)
                    config.afterCommissionBoardID = _afterCommissionBoardID;
                if (foldTemperature_isDirty)
                    config.foldTemperature = _foldTemperature;
                if (coolDownTemperature_isDirty)
                    config.coolDownTemperature = _coolDownTemperature;
                if (ledcontrol_isDirty)
                    config.ledcontrol = ledcontrol;
                if (BatteryCapacity24_isDirty)
                    config.batteryCapacity24 = _BatteryCapacity24;
                if (BatteryCapacity36_isDirty)
                    config.batteryCapacity36 = _BatteryCapacity36;
                if (BatteryCapacity48_isDirty)
                    config.batteryCapacity48 = _BatteryCapacity48;
                if (BatteryCapacity80_isDirty)
                    config.batteryCapacity80 = _BatteryCapacity80;
                if (forceFinishTimeout_isDirty)
                    config.forceFinishTimeout = _forceFinishTimeout;
                if (chargerOverrideBattviewFIEQsched_isDirty)
                    config.chargerOverrideBattviewFIEQsched = _chargerOverrideBattviewFIEQsched;
                if (ignoreBATTViewSOC_isDirty)
                    config.ignoreBATTViewSOC = _ignoreBATTViewSOC;
                if (battviewAutoCalibrationEnable_isDirty)
                    config.battviewAutoCalibrationEnable = _battviewAutoCalibrationEnable;
                if (cc_ramping_min_steps_isDirty)
                    config.cc_ramping_min_steps = _cc_ramping_min_steps;
                if (nominal_temperature_shift_isDirty)
                    config.nominal_temperature_shift = _nominal_temperature_shift;

            }
            return warning;
        }
    }
}
