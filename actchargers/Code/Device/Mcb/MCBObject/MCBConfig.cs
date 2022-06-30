using System;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace actchargers
{
    public class MCBConfig
    {
        [JsonIgnore]
        public bool LoadedFromArray { get; private set; }

        [JsonIgnore]
        public bool NotLoaded { get { return !LoadedFromArray; } }

        #region variables
        [JsonIgnore]
        internal const int dataSize = 512;
        //definitions
        [JsonIgnore]
        private UInt32 _oemid;
        [JsonProperty]
        public UInt32 oemid
        {
            get
            {
                lock (myLock)
                {
                    return _oemid;
                }
            }
            set
            {
                lock (myLock)
                {
                    _oemid = value;
                }
            }
        }
        [JsonIgnore]
        private UInt32 _dealerid;
        [JsonProperty]
        public UInt32 dealerid
        {
            get
            {
                lock (myLock)
                {
                    return _dealerid;
                }
            }
            set
            {
                lock (myLock)
                {
                    _dealerid = value;
                }
            }
        }
        [JsonIgnore]
        private UInt32 _servicedealerid;
        [JsonProperty]
        public UInt32 servicedealerid
        {
            get
            {
                lock (myLock)
                {
                    return _servicedealerid;
                }
            }
            set
            {
                lock (myLock)
                {
                    _servicedealerid = value;
                }
            }
        }
        [JsonIgnore]
        private UInt32 _siteid;
        [JsonProperty]
        public UInt32 siteid
        {
            get
            {
                lock (myLock)
                {
                    return _siteid;
                }
            }
            set
            {
                lock (myLock)
                {
                    _siteid = value;
                }
            }
        }
        [JsonIgnore]
        private UInt32 _customerid;
        [JsonProperty]
        public UInt32 customerid
        {
            get
            {
                lock (myLock)
                {
                    return _customerid;
                }
            }
            set
            {
                lock (myLock)
                {
                    _customerid = value;
                }
            }
        }
        [JsonIgnore]
        private float _firmwareVersion;
        [JsonProperty]
        public float firmwareVersion
        {
            get
            {
                lock (myLock)
                {
                    return _firmwareVersion;
                }
            }
            set
            {
                lock (myLock)
                {
                    _firmwareVersion = value;
                }
            }
        }
        [JsonIgnore]
        private byte _zoneID;
        [JsonProperty]
        public byte zoneID
        {
            get
            {
                lock (myLock)
                {
                    return _zoneID;
                }
            }
            set
            {
                lock (myLock)
                {
                    _zoneID = value;
                }
            }
        }

        [JsonIgnore]
        private string _id;
        [JsonProperty]
        public string id
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
                    _id = value;
                }
            }
        }

        [JsonIgnore]
        private byte _setup;
        [JsonProperty]
        public byte setup
        {
            get
            {
                lock (myLock)
                {
                    return _setup;
                }
            }
            set
            {
                lock (myLock)
                {
                    _setup = value;
                }
            }
        }

        [JsonConverter(typeof(ToNormalString))]
        [JsonProperty]
        public string originalSerialNumber
        {
            get
            {
                lock (myLock)
                {
                    return _originalSerialNumber;
                }
            }
            set
            {
                lock (myLock)
                {
                    _originalSerialNumber = value;
                }
            }
        }
        [JsonIgnore]
        private string _originalSerialNumber;
        [JsonConverter(typeof(ToNormalString))]
        [JsonProperty]
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
                    _serialNumber = value;
                }
            }
        }
        [JsonIgnore]
        private string _serialNumber;
        [JsonConverter(typeof(ToNormalString))]
        [JsonProperty]
        public string model
        {
            get
            {
                lock (myLock)
                {
                    return _model;
                }
            }
            set
            {
                lock (myLock)
                {
                    _model = value;
                }
            }
        }
        [JsonIgnore]
        private string _model;
        [JsonConverter(typeof(ToNormalString))]
        [JsonProperty]
        public string HWRevision
        {
            get
            {
                lock (myLock)
                {
                    return _HWRevision;
                }
            }
            set
            {
                lock (myLock)
                {
                    _HWRevision = value;
                }
            }
        }
        [JsonIgnore]
        private string _HWRevision;
        [JsonConverter(typeof(ToNormalString))]
        [JsonProperty]
        public string chargerUserName
        {
            get
            {
                lock (myLock)
                {
                    return _chargerUserName;
                }
            }
            set
            {
                lock (myLock)
                {
                    _chargerUserName = value;
                }
            }
        }
        [JsonIgnore]
        private string _chargerUserName;
        [JsonConverter(typeof(ToNormalString))]
        [JsonProperty]
        public string softAPpassword
        {
            get
            {
                lock (myLock)
                {
                    return _softAPpassword;
                }
            }
            set
            {
                lock (myLock)
                {
                    _softAPpassword = value;
                }
            }
        }
        [JsonIgnore]
        private string _softAPpassword;
        [JsonConverter(typeof(ToNormalString))]
        [JsonProperty]
        public string actAccessSSID
        {
            get
            {
                lock (myLock)
                {
                    return _actAccessSSID;
                }
            }
            set
            {
                lock (myLock)
                {
                    _actAccessSSID = value;
                }
            }
        }
        [JsonIgnore]
        private string _actAccessSSID;
        [JsonConverter(typeof(ToNormalString))]
        [JsonProperty]
        public string mobileAccessSSID
        {
            get
            {
                lock (myLock)
                {
                    return _mobileAccessSSID;
                }
            }
            set
            {
                lock (myLock)
                {
                    _mobileAccessSSID = value;
                }
            }
        }
        [JsonIgnore]
        private string _mobileAccessSSID;
        [JsonConverter(typeof(ToNormalString))]
        [JsonProperty]
        public string actViewIP
        {
            get
            {
                lock (myLock)
                {
                    return _actViewIP;
                }
            }
            set
            {
                lock (myLock)
                {
                    _actViewIP = value;
                }
            }
        }
        [JsonIgnore]
        private string _actViewIP;
        [JsonConverter(typeof(ToNormalString))]
        [JsonProperty]
        public string actAccessPassword
        {
            get
            {
                lock (myLock)
                {
                    return _actAccessPassword;
                }
            }
            set
            {
                lock (myLock)
                {
                    _actAccessPassword = value;
                }
            }
        }
        [JsonIgnore]
        private string _actAccessPassword;
        [JsonConverter(typeof(ToNormalString))]
        [JsonProperty]
        public string mobileAccessPassword
        {
            get
            {
                lock (myLock)
                {
                    return _mobileAccessPassword;
                }
            }
            set
            {
                lock (myLock)
                {
                    _mobileAccessPassword = value;
                }
            }
        }
        [JsonIgnore]
        private string _mobileAccessPassword;



        [JsonIgnore]
        private bool _actViewEnable;
        [JsonProperty]
        public bool actViewEnable
        {
            get
            {
                lock (myLock)
                {
                    return _actViewEnable;
                }
            }
            set
            {
                lock (myLock)
                {
                    _actViewEnable = value;
                }
            }
        }
        [JsonIgnore]
        private bool _softAPenabled;
        [JsonProperty]
        public bool softAPenabled
        {
            get
            {
                lock (myLock)
                {
                    return _softAPenabled;
                }
            }
            set
            {
                lock (myLock)
                {
                    _softAPenabled = value;
                }
            }
        }
        [JsonIgnore]
        private string _actViewConnectFrequency;
        [JsonProperty]
        public string actViewConnectFrequency
        {
            get
            {
                lock (myLock)
                {
                    return _actViewConnectFrequency;
                }
            }
            set
            {
                lock (myLock)
                {
                    _actViewConnectFrequency = value;
                }
            }
        }
        [JsonIgnore]
        private string _memorySignature;
        [JsonProperty]
        public string memorySignature
        {
            get
            {
                lock (myLock)
                {
                    return _memorySignature;
                }
            }
            set
            {
                lock (myLock)
                {
                    _memorySignature = value;
                }
            }
        }
        [JsonIgnore]
        private DateTime _InstallationDate;
        [JsonConverter(typeof(ToTimeStamp))]
        [JsonProperty]
        public DateTime InstallationDate
        {
            get
            {
                lock (myLock)
                {
                    return new DateTime(_InstallationDate.Ticks);
                }
            }
            set
            {
                lock (myLock)
                {
                    _InstallationDate = value;
                }
            }
        }

        //paramters
        [JsonIgnore]
        private byte _version;
        [JsonProperty]
        public byte version
        {
            get
            {
                lock (myLock)
                {
                    return _version;
                }
            }
            set
            {
                lock (myLock)
                {
                    _version = value;
                }
            }
        }
        [JsonIgnore]
        private string _trickleVoltage;
        [JsonProperty]
        public string trickleVoltage
        {
            get
            {
                lock (myLock)
                {
                    return _trickleVoltage;
                }
            }
            set
            {
                lock (myLock)
                {
                    _trickleVoltage = value;
                }
            }
        }
        [JsonIgnore]
        private string _CVvoltage;
        [JsonProperty]
        public string CVvoltage
        {
            get
            {
                lock (myLock)
                {
                    return _CVvoltage;
                }
            }
            set
            {
                lock (myLock)
                {
                    _CVvoltage = value;
                }
            }
        }
        [JsonIgnore]
        private string _FIvoltage;
        [JsonProperty]
        public string FIvoltage
        {
            get
            {
                lock (myLock)
                {
                    return _FIvoltage;
                }
            }
            set
            {
                lock (myLock)
                {
                    _FIvoltage = value;
                }
            }
        }
        [JsonIgnore]
        private string _EQvoltage;
        [JsonProperty]
        public string EQvoltage
        {
            get
            {
                lock (myLock)
                {
                    return _EQvoltage;
                }
            }
            set
            {
                lock (myLock)
                {
                    _EQvoltage = value;
                }
            }
        }

        [JsonIgnore]
        private string _batteryType;
        [JsonConverter(typeof(ToMcbConfigBatteryType))]
        [JsonProperty]
        public string batteryType
        {
            get
            {
                lock (myLock)
                {
                    return _batteryType;
                }
            }
            set
            {
                lock (myLock)
                {
                    _batteryType = value;
                }
            }
        }

        public bool IsLithuimIonBatteryType()
        {
            return Array.IndexOf(batteryTypes, batteryType) == 1;
        }

        [JsonIgnore]
        private string _temperatureVoltageCompensation;
        [JsonProperty]
        public string temperatureVoltageCompensation
        {
            get
            {
                lock (myLock)
                {
                    return _temperatureVoltageCompensation;
                }
            }
            set
            {
                lock (myLock)
                {
                    _temperatureVoltageCompensation = value;
                }
            }
        }
        [JsonIgnore]
        private string _maxTemperatureFault;
        [JsonProperty]
        public string maxTemperatureFault
        {
            get
            {
                lock (myLock)
                {
                    return _maxTemperatureFault;
                }
            }
            set
            {
                lock (myLock)
                {
                    _maxTemperatureFault = value;
                }
            }
        }
        [JsonIgnore]
        private string _batteryVoltage;
        [JsonProperty]
        public string batteryVoltage
        {
            get
            {
                lock (myLock)
                {
                    return _batteryVoltage;
                }
            }
            set
            {
                lock (myLock)
                {
                    _batteryVoltage = value;
                }
            }
        }

        [JsonIgnore]
        private UInt16 _CCrate;
        [JsonConverter(typeof(ToMcbConfigRateFrom16Bits))]
        [JsonProperty]
        public UInt16 CCrate
        {
            get
            {
                lock (myLock)
                {
                    return _CCrate;
                }
            }
            set
            {
                lock (myLock)
                {
                    _CCrate = value;
                }
            }
        }

        [JsonIgnore]
        private UInt16 _TRrate;
        [JsonConverter(typeof(ToMcbConfigRateFrom16Bits))]
        [JsonProperty]
        public UInt16 TRrate
        {
            get
            {
                lock (myLock)
                {
                    return _TRrate;
                }
            }
            set
            {
                lock (myLock)
                {
                    _TRrate = value;
                }
            }
        }

        [JsonIgnore]
        private UInt16 _FIrate;
        [JsonConverter(typeof(ToMcbConfigRateFrom16Bits))]
        [JsonProperty]
        public UInt16 FIrate
        {
            get
            {
                lock (myLock)
                {
                    return _FIrate;
                }
            }
            set
            {
                lock (myLock)
                {
                    _FIrate = value;
                }
            }
        }

        [JsonIgnore]
        private UInt16 _EQrate;
        [JsonConverter(typeof(ToMcbConfigRateFrom16Bits))]
        [JsonProperty]
        public UInt16 EQrate
        {
            get
            {
                lock (myLock)
                {
                    return _EQrate;
                }
            }
            set
            {
                lock (myLock)
                {
                    _EQrate = value;
                }
            }
        }
        [JsonIgnore]
        private string _PMefficiency;
        [JsonProperty]
        public string PMefficiency
        {
            get
            {
                lock (myLock)
                {
                    return _PMefficiency;
                }
            }
            set
            {
                lock (myLock)
                {
                    _PMefficiency = value;
                }
            }
        }
        [JsonIgnore]
        private string _actViewPort;
        [JsonProperty]
        public string actViewPort
        {
            get
            {
                lock (myLock)
                {
                    return _actViewPort;
                }
            }
            set
            {
                lock (myLock)
                {
                    _actViewPort = value;
                }
            }
        }
        [JsonIgnore]
        private string _mobilePort;
        [JsonProperty]
        public string mobilePort
        {
            get
            {
                lock (myLock)
                {
                    return _mobilePort;
                }
            }
            set
            {
                lock (myLock)
                {
                    _mobilePort = value;
                }
            }
        }
        [JsonIgnore]
        private byte _finishDV;
        [JsonProperty]
        public byte finishDV
        {
            get
            {
                lock (myLock)
                {
                    return _finishDV;
                }
            }
            set
            {
                lock (myLock)
                {
                    _finishDV = value;
                }
            }
        }
        [JsonIgnore]
        private byte _CVcurrentStep;
        [JsonProperty]
        public byte CVcurrentStep
        {
            get
            {
                lock (myLock)
                {
                    return _CVcurrentStep;
                }
            }
            set
            {
                lock (myLock)
                {
                    _CVcurrentStep = value;
                }
            }
        }
        [JsonIgnore]
        private byte _CVfinishCurrent;
        [JsonProperty]
        public byte CVfinishCurrent
        {
            get
            {
                lock (myLock)
                {
                    return _CVfinishCurrent;
                }
            }
            set
            {
                lock (myLock)
                {
                    _CVfinishCurrent = value;
                }
            }
        }

        //setting
        [JsonIgnore]
        private float _voltageCalA;
        [JsonProperty]
        public float voltageCalA
        {
            get
            {
                lock (myLock)
                {
                    return _voltageCalA;
                }
            }
            set
            {
                lock (myLock)
                {
                    _voltageCalA = value;
                }
            }
        }
        [JsonIgnore]
        private float _voltageCalB;
        [JsonProperty]
        public float voltageCalB
        {
            get
            {
                lock (myLock)
                {
                    return _voltageCalB;
                }
            }
            set
            {
                lock (myLock)
                {
                    _voltageCalB = value;
                }
            }
        }
        [JsonIgnore]
        private float _voltageCalALow;
        [JsonProperty]
        public float voltageCalALow
        {
            get
            {
                lock (myLock)
                {
                    return _voltageCalALow;
                }
            }
            set
            {
                lock (myLock)
                {
                    _voltageCalALow = value;
                }
            }
        }
        [JsonIgnore]
        private float _voltageCalBLow;
        [JsonProperty]
        public float voltageCalBLow
        {
            get
            {
                lock (myLock)
                {
                    return _voltageCalBLow;
                }
            }
            set
            {
                lock (myLock)
                {
                    _voltageCalBLow = value;
                }
            }
        }
        [JsonIgnore]
        private float _temperatureCalALow;
        [JsonProperty]
        public float temperatureCalALow
        {
            get
            {
                lock (myLock)
                {
                    return _temperatureCalALow;
                }
            }
            set
            {
                lock (myLock)
                {
                    _temperatureCalALow = value;
                }
            }
        }
        [JsonIgnore]
        private float _temperatureCalBLow;
        [JsonProperty]
        public float temperatureCalBLow
        {
            get
            {
                lock (myLock)
                {
                    return _temperatureCalBLow;
                }
            }
            set
            {
                lock (myLock)
                {
                    _temperatureCalBLow = value;
                }
            }
        }
        [JsonIgnore]
        private float _temperatureVI;
        [JsonProperty]
        public float temperatureVI
        {
            get
            {
                lock (myLock)
                {
                    return _temperatureVI;
                }
            }
            set
            {
                lock (myLock)
                {
                    _temperatureVI = value;
                }
            }
        }
        [JsonIgnore]
        private float _temperatureR;
        [JsonProperty]
        public float temperatureR
        {
            get
            {
                lock (myLock)
                {
                    return _temperatureR;
                }
            }
            set
            {
                lock (myLock)
                {
                    _temperatureR = value;
                }
            }
        }
        [JsonIgnore]
        private float _IR;
        [JsonProperty]
        public float IR
        {
            get
            {
                lock (myLock)
                {
                    return _IR;
                }
            }
            set
            {
                lock (myLock)
                {
                    _IR = value;
                }
            }
        }

        [JsonIgnore]
        private float _temperatureSensorSHCa;
        [JsonProperty]
        public float temperatureSensorSHCa
        {
            get
            {
                lock (myLock)
                {
                    return _temperatureSensorSHCa;
                }
            }
            set
            {
                lock (myLock)
                {
                    _temperatureSensorSHCa = value;
                }
            }
        }
        [JsonIgnore]
        private float _temperatureSensorSHCb;
        [JsonProperty]
        public float temperatureSensorSHCb
        {
            get
            {
                lock (myLock)
                {
                    return _temperatureSensorSHCb;
                }
            }
            set
            {
                lock (myLock)
                {
                    _temperatureSensorSHCb = value;
                }
            }
        }
        [JsonIgnore]
        private float _temperatureSensorSHCc;
        [JsonProperty]
        public float temperatureSensorSHCc
        {
            get
            {
                lock (myLock)
                {
                    return _temperatureSensorSHCc;
                }
            }
            set
            {
                lock (myLock)
                {
                    _temperatureSensorSHCc = value;
                }
            }
        }

        [JsonIgnore]
        private string _lastChangeUserId;
        [JsonProperty]
        public string lastChangeUserId
        {
            get
            {
                lock (myLock)
                {
                    return _lastChangeUserId;
                }
            }
            set
            {
                lock (myLock)
                {
                    _lastChangeUserId = value;
                }
            }
        }

        [JsonIgnore]
        private DateTime _lastChangeTime;
        [JsonConverter(typeof(ToTimeStamp))]
        [JsonProperty]
        public DateTime lastChangeTime
        {
            get
            {
                lock (myLock)
                {
                    return _lastChangeTime;
                }
            }
            set
            {
                lock (myLock)
                {
                    _lastChangeTime = value;
                }
            }
        }

        [JsonIgnore]
        private bool _enableChargerSimulationMode;
        [JsonProperty]
        public bool enableChargerSimulationMode
        {
            get
            {
                lock (myLock)
                {
                    return _enableChargerSimulationMode;
                }
            }
            set
            {
                lock (myLock)
                {
                    _enableChargerSimulationMode = value;
                }
            }
        }
        [JsonIgnore]
        private bool _enableAutoDetectMultiVoltage;
        [JsonProperty]
        public bool enableAutoDetectMultiVoltage
        {
            get
            {
                lock (myLock)
                {
                    return _enableAutoDetectMultiVoltage;
                }
            }
            set
            {
                lock (myLock)
                {
                    _enableAutoDetectMultiVoltage = value;
                }
            }
        }
        [JsonIgnore]
        private bool _temperatureSensorInstalled;
        [JsonProperty]
        public bool temperatureSensorInstalled
        {
            get
            {
                lock (myLock)
                {
                    return _temperatureSensorInstalled;
                }
            }
            set
            {
                lock (myLock)
                {
                    _temperatureSensorInstalled = value;
                }
            }
        }
        [JsonIgnore]
        private bool _enableRefreshCycleAfterFI;
        [JsonProperty]
        public bool enableRefreshCycleAfterFI
        {
            get
            {
                lock (myLock)
                {
                    return _enableRefreshCycleAfterFI;
                }
            }
            set
            {
                lock (myLock)
                {
                    _enableRefreshCycleAfterFI = value;
                }
            }
        }
        [JsonIgnore]
        private bool _enableRefreshCycleAfterEQ;
        [JsonProperty]
        public bool enableRefreshCycleAfterEQ
        {
            get
            {
                lock (myLock)
                {
                    return _enableRefreshCycleAfterEQ;
                }
            }
            set
            {
                lock (myLock)
                {
                    _enableRefreshCycleAfterEQ = value;
                }
            }
        }
        [JsonIgnore]
        private bool _enablePMsimulation;
        [JsonProperty]
        public bool enablePMsimulation
        {
            get
            {
                lock (myLock)
                {
                    return _enablePMsimulation;
                }
            }
            set
            {
                lock (myLock)
                {
                    _enablePMsimulation = value;
                }
            }
        }


        [JsonIgnore]
        private byte _chargerType;
        [JsonProperty]
        public byte chargerType
        {
            get
            {
                lock (myLock)
                {
                    return _chargerType;
                }
            }
            set
            {
                lock (myLock)
                {
                    _chargerType = value;
                }
            }
        }


        [JsonIgnore]
        private string _batteryCapacity;
        [JsonProperty]
        public string batteryCapacity
        {
            get
            {
                lock (myLock)
                {
                    return _batteryCapacity;
                }
            }
            set
            {
                lock (myLock)
                {
                    _batteryCapacity = value;
                }
            }
        }

        [JsonIgnore]
        private string _EQstartWindow;
        [JsonConverter(typeof(ToMcbConfigEqfiStartTime))]

        [JsonProperty]
        public string EQstartWindow
        {
            get
            {
                lock (myLock)
                {
                    return _EQstartWindow;
                }
            }
            set
            {
                lock (myLock)
                {
                    _EQstartWindow = value;
                }
            }
        }

        [JsonIgnore]
        private string _FIstartWindow;
        [JsonConverter(typeof(ToMcbConfigEqfiStartTime))]
        [JsonProperty]
        public string FIstartWindow
        {
            get
            {
                lock (myLock)
                {
                    return _FIstartWindow;
                }
            }
            set
            {
                lock (myLock)
                {
                    _FIstartWindow = value;
                }
            }
        }
        [JsonIgnore]
        private string _autoStartCountDownTimer;
        [JsonProperty]
        public string autoStartCountDownTimer
        {
            get
            {
                lock (myLock)
                {
                    return _autoStartCountDownTimer;
                }
            }
            set
            {
                lock (myLock)
                {
                    _autoStartCountDownTimer = value;
                }
            }
        }

        [JsonIgnore]
        private DaysMask _finishDaysMask;
        [JsonConverter(typeof(ToMcbConfigDaysMask))]
        [JsonProperty]
        public DaysMask finishDaysMask
        {
            get
            {
                lock (myLock)
                {
                    return _finishDaysMask;
                }
            }
            set
            {
                lock (myLock)
                {
                    _finishDaysMask = value;
                }
            }
        }
        [JsonIgnore]
        private bool _autoStartEnable;
        [JsonProperty]
        public bool autoStartEnable
        {
            get
            {
                lock (myLock)
                {
                    return _autoStartEnable;
                }
            }
            set
            {
                lock (myLock)
                {
                    _autoStartEnable = value;
                }
            }
        }


        [JsonIgnore]
        private DaysMask _autoStartMask;
        [JsonConverter(typeof(ToMcbConfigDaysMask))]
        [JsonProperty]
        public DaysMask autoStartMask
        {
            get
            {
                lock (myLock)
                {
                    return _autoStartMask;
                }
            }
            set
            {
                lock (myLock)
                {
                    _autoStartMask = value;
                }
            }
        }
        [JsonIgnore]
        private bool _FIschedulingMode;
        [JsonProperty]
        public bool FIschedulingMode
        {
            get
            {
                lock (myLock)
                {
                    return _FIschedulingMode;
                }
            }
            set
            {
                lock (myLock)
                {
                    _FIschedulingMode = value;
                }
            }
        }

        //
        [JsonIgnore]
        private string _finishWindow;
        [JsonConverter(typeof(ToMcbConfigEqfiWindow))]
        [JsonProperty]
        public string finishWindow
        {
            get
            {
                lock (myLock)
                {
                    return _finishWindow;
                }
            }
            set
            {
                lock (myLock)
                {
                    _finishWindow = value;
                }
            }
        }

        [JsonIgnore]
        private DaysMask _EQdaysMask;
        [JsonConverter(typeof(ToMcbConfigDaysMask))]
        [JsonProperty]
        public DaysMask EQdaysMask
        {
            get
            {
                lock (myLock)
                {
                    return _EQdaysMask;
                }
            }
            set
            {
                lock (myLock)
                {
                    _EQdaysMask = value;
                }
            }
        }


        [JsonIgnore]
        private string _EQwindow;
        [JsonConverter(typeof(ToMcbConfigEqfiWindow))]
        [JsonProperty]
        public string EQwindow
        {
            get
            {
                lock (myLock)
                {
                    return _EQwindow;
                }
            }
            set
            {
                lock (myLock)
                {
                    _EQwindow = value;
                }
            }
        }


        [JsonIgnore]
        private bool _daylightSaving;
        [JsonProperty]
        public bool daylightSaving
        {
            get
            {
                lock (myLock)
                {
                    return _daylightSaving;
                }
            }
            set
            {
                lock (myLock)
                {
                    _daylightSaving = value;
                }
            }
        }
        [JsonIgnore]
        private bool _temperatureFormat;
        [JsonProperty]
        public bool temperatureFormat
        {
            get
            {
                lock (myLock)
                {
                    return _temperatureFormat;
                }
            }
            set
            {
                lock (myLock)
                {
                    _temperatureFormat = value;
                }
            }
        }
        [JsonIgnore]
        private string _numberOfInstalledPMs;
        [JsonProperty]
        public string numberOfInstalledPMs
        {
            get
            {
                lock (myLock)
                {
                    return _numberOfInstalledPMs;
                }
            }
            set
            {
                lock (myLock)
                {
                    _numberOfInstalledPMs = value;
                }
            }
        }
        [JsonIgnore]
        private string _PMvoltage;
        [JsonProperty]
        public string PMvoltage
        {
            get
            {
                lock (myLock)
                {
                    return _PMvoltage;
                }
            }
            set
            {
                lock (myLock)
                {
                    _PMvoltage = value;
                }
            }
        }

        [JsonIgnore]
        private string _CVtimer;
        [JsonConverter(typeof(ToMcbConfigEqfiStartTime))]
        [JsonProperty]
        public string CVtimer
        {
            get
            {
                lock (myLock)
                {
                    return _CVtimer;
                }
            }
            set
            {
                lock (myLock)
                {
                    _CVtimer = value;
                }
            }
        }

        [JsonIgnore]
        private string _finishTimer;
        [JsonConverter(typeof(ToMcbConfigEqfiStartTime))]
        [JsonProperty]
        public string finishTimer
        {
            get
            {
                lock (myLock)
                {
                    return _finishTimer;
                }
            }
            set
            {
                lock (myLock)
                {
                    _finishTimer = value;
                }
            }
        }
        [JsonIgnore]
        private string _finishDT;
        [JsonProperty]
        public string finishDT
        {
            get
            {
                lock (myLock)
                {
                    return _finishDT;
                }
            }
            set
            {
                lock (myLock)
                {
                    _finishDT = value;
                }
            }
        }

        [JsonIgnore]
        private string _EQtimer;
        [JsonConverter(typeof(ToMcbConfigEqfiStartTime))]
        [JsonProperty]
        public string EQtimer
        {
            get
            {
                lock (myLock)
                {
                    return _EQtimer;
                }
            }
            set
            {
                lock (myLock)
                {
                    _EQtimer = value;
                }
            }
        }

        [JsonIgnore]
        private string _refreshTimer;
        [JsonConverter(typeof(ToMcbConfigEqfiStartTime))]
        [JsonProperty]
        public string refreshTimer
        {
            get
            {
                lock (myLock)
                {
                    return _refreshTimer;
                }
            }
            set
            {
                lock (myLock)
                {
                    _refreshTimer = value;
                }
            }
        }

        [JsonIgnore]
        private string _desulfationTimer;
        [JsonConverter(typeof(ToMcbConfigEqfiStartTime))]
        [JsonProperty]
        public string desulfationTimer
        {
            get
            {
                lock (myLock)
                {
                    return _desulfationTimer;
                }
            }
            set
            {
                lock (myLock)
                {
                    _desulfationTimer = value;
                }
            }
        }

        [JsonIgnore]
        private string _lcdMemoryVersion;
        [JsonProperty]
        public string lcdMemoryVersion
        {
            get
            {
                lock (myLock)
                {
                    return _lcdMemoryVersion;
                }
            }
            set
            {
                lock (myLock)
                {
                    _lcdMemoryVersion = value;
                }
            }
        }
        [JsonIgnore]
        private string _wifiFirmwareVersion;
        [JsonProperty]
        public string wifiFirmwareVersion
        {
            get
            {
                lock (myLock)
                {
                    return _wifiFirmwareVersion;
                }
            }
            set
            {
                lock (myLock)
                {
                    _wifiFirmwareVersion = value;
                }
            }
        }
        [JsonIgnore]
        private bool _enablePLC;
        [JsonProperty]
        public bool enablePLC
        {
            get
            {
                lock (myLock)
                {
                    return _enablePLC;
                }
            }
            set
            {
                lock (myLock)
                {
                    _enablePLC = value;
                }
            }
        }
        [JsonIgnore]
        private bool _enableManualEQ;
        [JsonProperty]
        public bool enableManualEQ
        {
            get
            {
                lock (myLock)
                {
                    return _enableManualEQ;
                }
            }
            set
            {
                lock (myLock)
                {
                    _enableManualEQ = value;
                }
            }
        }
        [JsonIgnore]
        private bool _enableManualDesulfate;
        [JsonProperty]
        public bool enableManualDesulfate
        {
            get
            {
                lock (myLock)
                {
                    return _enableManualDesulfate;
                }
            }
            set
            {
                lock (myLock)
                {
                    _enableManualDesulfate = value;
                }
            }
        }

        [JsonIgnore]
        private DaysMask _energyDaysMask;
        [JsonConverter(typeof(ToMcbConfigDaysMask))]
        [JsonProperty]
        public DaysMask energyDaysMask
        {
            get
            {
                lock (myLock)
                {
                    return _energyDaysMask;
                }
            }
            set
            {
                lock (myLock)
                {
                    _energyDaysMask = value;
                }
            }
        }
        [JsonIgnore]
        private UInt32 _lockoutStartTime;
        [JsonProperty]
        public UInt32 lockoutStartTime
        {
            get
            {
                lock (myLock)
                {
                    return _lockoutStartTime;
                }
            }
            set
            {
                lock (myLock)
                {
                    _lockoutStartTime = value;
                }
            }
        }
        [JsonIgnore]
        private UInt32 _lockoutCloseTime;
        [JsonProperty]
        public UInt32 lockoutCloseTime
        {
            get
            {
                lock (myLock)
                {
                    return _lockoutCloseTime;
                }
            }
            set
            {
                lock (myLock)
                {
                    _lockoutCloseTime = value;
                }
            }
        }

        [JsonIgnore]
        private UInt32 _energyStartTime;
        [JsonProperty]
        public UInt32 energyStartTime
        {
            get
            {
                lock (myLock)
                {
                    return _energyStartTime;
                }
            }
            set
            {
                lock (myLock)
                {
                    _energyStartTime = value;
                }
            }
        }
        [JsonIgnore]
        private UInt32 _energyCloseTime;
        [JsonProperty]
        public UInt32 energyCloseTime
        {
            get
            {
                lock (myLock)
                {
                    return _energyCloseTime;
                }
            }
            set
            {
                lock (myLock)
                {
                    _energyCloseTime = value;
                }
            }
        }


        [JsonIgnore]
        private DaysMask _lockoutDaysMask;
        [JsonConverter(typeof(ToMcbConfigDaysMask))]
        [JsonProperty]
        public DaysMask lockoutDaysMask
        {
            get
            {
                lock (myLock)
                {
                    return _lockoutDaysMask;
                }
            }
            set
            {
                lock (myLock)
                {
                    _lockoutDaysMask = value;
                }
            }
        }
        [JsonIgnore]
        private byte _energyDecreaseValue;
        [JsonProperty]
        public byte energyDecreaseValue
        {
            get
            {
                lock (myLock)
                {
                    return _energyDecreaseValue;
                }
            }
            set
            {
                lock (myLock)
                {
                    _energyDecreaseValue = value;
                }
            }
        }
        [JsonIgnore]
        private byte _PMvoltageInputValue;
        [JsonProperty]
        public byte PMvoltageInputValue
        {
            get
            {
                lock (myLock)
                {
                    return _PMvoltageInputValue;
                }
            }
            set
            {
                lock (myLock)
                {
                    _PMvoltageInputValue = value;
                }
            }
        }
        [JsonIgnore]
        private bool _disablePushButton;
        [JsonProperty]
        public bool disablePushButton
        {
            get
            {
                lock (myLock)
                {
                    return _disablePushButton;
                }
            }
            set
            {
                lock (myLock)
                {
                    _disablePushButton = value;
                }
            }
        }
        [JsonIgnore]
        private string _TRtemperature;
        [JsonProperty]
        public string TRtemperature
        {
            get
            {
                lock (myLock)
                {
                    return _TRtemperature;
                }
            }
            set
            {
                lock (myLock)
                {
                    _TRtemperature = value;
                }
            }
        }
        [JsonIgnore]
        private UInt32 _afterCommissionBoardID;
        [JsonProperty]
        public UInt32 afterCommissionBoardID
        {
            get
            {
                lock (myLock)
                {
                    return _afterCommissionBoardID;
                }
            }
            set
            {
                lock (myLock)
                {
                    _afterCommissionBoardID = value;
                }
            }
        }
        [JsonIgnore]
        private string _foldTemperature;
        [JsonProperty]
        public string foldTemperature
        {
            get
            {
                lock (myLock)
                {
                    return _foldTemperature;
                }
            }
            set
            {
                lock (myLock)
                {
                    _foldTemperature = value;
                }
            }
        }
        [JsonIgnore]
        private string _coolDownTemperature;
        [JsonProperty]
        public string coolDownTemperature
        {
            get
            {
                lock (myLock)
                {
                    return _coolDownTemperature;
                }
            }
            set
            {
                lock (myLock)
                {
                    _coolDownTemperature = value;
                }
            }
        }

        [JsonIgnore]
        private byte _ledcontrol;
        [JsonProperty]
        public byte ledcontrol
        {
            get
            {
                lock (myLock)
                {
                    return _ledcontrol;
                }
            }
            set
            {
                lock (myLock)
                {
                    _ledcontrol = value;
                }
            }
        }


        [JsonIgnore]
        private UInt16 _BatteryCapacity24;
        [JsonProperty]
        public UInt16 batteryCapacity24
        {
            get
            {
                lock (myLock)
                {
                    return _BatteryCapacity24;
                }
            }
            set
            {
                lock (myLock)
                {
                    _BatteryCapacity24 = value;
                }
            }
        }

        [JsonIgnore]
        private UInt16 _BatteryCapacity36;
        [JsonProperty]
        public UInt16 batteryCapacity36
        {
            get
            {
                lock (myLock)
                {
                    return _BatteryCapacity36;
                }
            }
            set
            {
                lock (myLock)
                {
                    _BatteryCapacity36 = value;
                }
            }
        }


        [JsonIgnore]
        private UInt16 _BatteryCapacity48;
        [JsonProperty]
        public UInt16 batteryCapacity48
        {
            get
            {
                lock (myLock)
                {
                    return _BatteryCapacity48;
                }
            }
            set
            {
                lock (myLock)
                {
                    _BatteryCapacity48 = value;
                }
            }
        }

        [JsonIgnore]
        private bool _forceFinishTimeout;
        [JsonProperty]
        public bool forceFinishTimeout
        {
            get
            {
                lock (myLock)
                {
                    return _forceFinishTimeout;
                }
            }
            set
            {
                lock (myLock)
                {
                    _forceFinishTimeout = value;
                }
            }
        }
        [JsonIgnore]
        private bool _replacementPart;
        [JsonProperty]
        public bool replacmentPart
        {
            get
            {
                lock (myLock)
                {
                    return _replacementPart;
                }
            }
            set
            {
                lock (myLock)
                {
                    _replacementPart = value;
                }
            }
        }
        [JsonIgnore]
        private bool _chargerOverrideBattviewFIEQsched;
        [JsonProperty]
        public bool chargerOverrideBattviewFIEQsched
        {
            get
            {
                lock (myLock)
                {
                    return _chargerOverrideBattviewFIEQsched;
                }
            }
            set
            {
                lock (myLock)
                {
                    _chargerOverrideBattviewFIEQsched = value;
                }
            }
        }
        [JsonIgnore]
        private bool _ignoreBATTViewSOC;
        [JsonProperty]
        public bool ignoreBATTViewSOC
        {
            get
            {
                lock (myLock)
                {
                    return _ignoreBATTViewSOC;
                }
            }
            set
            {
                lock (myLock)
                {
                    _ignoreBATTViewSOC = value;
                }
            }
        }
        [JsonIgnore]
        private bool _battviewAutoCalibrationEnable;
        [JsonProperty]
        public bool battviewAutoCalibrationEnable
        {
            get
            {
                lock (myLock)
                {
                    return _battviewAutoCalibrationEnable;
                }
            }
            set
            {
                lock (myLock)
                {
                    _battviewAutoCalibrationEnable = value;
                }
            }
        }
        [JsonIgnore]
        private byte _cc_ramping_min_steps;
        [JsonProperty]
        public byte cc_ramping_min_steps
        {
            get
            {
                lock (myLock)
                {
                    return _cc_ramping_min_steps;
                }
            }
            set
            {
                lock (myLock)
                {
                    _cc_ramping_min_steps = value;
                }
            }
        }

        private sbyte _nominal_temperature_shift;
        [JsonProperty]
        public sbyte nominal_temperature_shift
        {
            get
            {
                lock (myLock)
                {
                    return _nominal_temperature_shift;
                }
            }
            set
            {
                lock (myLock)
                {
                    _nominal_temperature_shift = value;
                }
            }
        }
        [JsonIgnore]
        private UInt16 _BatteryCapacity80;
        [JsonProperty]
        public UInt16 batteryCapacity80
        {
            get
            {
                lock (myLock)
                {
                    return _BatteryCapacity80;
                }
            }
            set
            {
                lock (myLock)
                {
                    _BatteryCapacity80 = value;
                }
            }
        }

        [JsonIgnore]
        private byte _FIStopCurrent;
        [JsonIgnore]
        public byte FIStopCurrent
        {
            get
            {
                lock (myLock)
                {
                    return _FIStopCurrent;
                }
            }
            set
            {
                lock (myLock)
                {
                    _FIStopCurrent = value;
                }
            }
        }
        [JsonIgnore]
        private bool _useNewEastPennProfile;
        [JsonIgnore]
        public bool useNewEastPennProfile
        {
            get
            {
                lock (myLock)
                {
                    return _useNewEastPennProfile;
                }
            }
            set
            {
                lock (myLock)
                {
                    _useNewEastPennProfile = value;
                }
            }
        }
        //OCD_Remote

        [JsonIgnore]
        private byte _OCD_Remote;
        [JsonProperty]
        public byte OCD_Remote
        {
            get
            {
                lock (myLock)
                {
                    return _OCD_Remote;
                }
            }
            set
            {
                lock (myLock)
                {
                    _OCD_Remote = value;
                }
            }
        }
        [JsonIgnore]
        private byte _Enable_72V;
        [JsonProperty]
        public byte Enable_72V
        {
            get
            {
                lock (myLock)
                {
                    return _Enable_72V;
                }
            }
            set
            {
                lock (myLock)
                {
                    _Enable_72V = value;
                }
            }
        }
        [JsonIgnore]
        private UInt16 _LiIon_CellVoltage;
        [JsonProperty]
        public UInt16 LiIon_CellVoltage
        {
            get
            {
                lock (myLock)
                {
                    return _LiIon_CellVoltage;
                }
            }
            set
            {
                lock (myLock)
                {
                    _LiIon_CellVoltage = value;
                }
            }
        }

        [JsonIgnore]
        private byte _LiIon_numberOfCells;
        [JsonProperty]
        public byte LiIon_numberOfCells
        {
            get
            {
                lock (myLock)
                {
                    return _LiIon_numberOfCells;
                }
            }
            set
            {
                lock (myLock)
                {
                    _LiIon_numberOfCells = value;
                }
            }
        }
        [JsonIgnore]
        private bool _doPLCStackCheck;
        [JsonProperty]
        public bool doPLCStackCheck
        {
            get
            {
                lock (myLock)
                {
                    return _doPLCStackCheck;
                }
            }
            set
            {
                lock (myLock)
                {
                    _doPLCStackCheck = value;
                }
            }
        }
        [JsonIgnore]
        private UInt16 _ReconnectTimer;
        [JsonProperty]
        public UInt16 ReconnectTimer
        {
            get
            {
                lock (myLock)
                {
                    return _ReconnectTimer;
                }
            }
            set
            {
                lock (myLock)
                {
                    _ReconnectTimer = value;
                }
            }
        }
        [JsonIgnore]
        private bool _LCD_FQ;
        [JsonProperty]
        public bool LCD_FQ
        {
            get
            {
                lock (myLock)
                {
                    return _LCD_FQ;
                }
            }
            set
            {
                lock (myLock)
                {
                    _LCD_FQ = value;
                }
            }
        }
        [JsonIgnore]
        private byte _DaughterCardEnabled;
        [JsonProperty]
        public byte DaughterCardEnabled
        {
            get
            {
                lock (myLock)
                {
                    return _DaughterCardEnabled;
                }
            }
            set
            {
                lock (myLock)
                {
                    _DaughterCardEnabled = value;
                }
            }
        }

        [JsonIgnore]
        private byte _PMMaxCurrent;
        [JsonProperty]
        public byte PMMaxCurrent
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

        [JsonIgnore]
        private byte _defaultBrightness;
        [JsonProperty]
        public byte defaultBrightness
        {
            get
            {
                lock (myLock)
                {
                    return _defaultBrightness;
                }
            }
            set
            {
                lock (myLock)
                {
                    _defaultBrightness = value;
                }
            }
        }

        [JsonIgnore]
        private UInt16 _bmsBitRate;
        [JsonProperty]
        public UInt16 bmsBitRate
        {
            get
            {
                lock (myLock)
                {
                    return _bmsBitRate;
                }
            }
            set
            {
                lock (myLock)
                {
                    _bmsBitRate = value;
                }
            }
        }

        [JsonIgnore]
        private byte _plc_gain;
        [JsonProperty]
        public byte plc_gain
        {
            get
            {
                lock (myLock)
                {
                    return _plc_gain;
                }
            }
            set
            {
                lock (myLock)
                {
                    _plc_gain = value;
                }
            }
        }

        [JsonIgnore]
        private byte _truck_image_id;
        [JsonProperty]
        public byte truck_image_id
        {
            get
            {
                lock (myLock)
                {
                    return _truck_image_id;
                }
            }
            set
            {
                lock (myLock)
                {
                    _truck_image_id = value;
                }
            }
        }

        [JsonIgnore]
        private byte[] unused;

        #endregion

        #region constructor

        object myLock;
        [JsonIgnore]
        const int configV1UnusedLength = 78;
        [JsonIgnore]
        const int configV2UnusedLength = 84;
        [JsonIgnore]
        const int maxSupportedVersion = 2;
        internal MCBConfig()
        {
            myLock = new object();

            originalSerialNumber = "";
            serialNumber = "";
            model = "";
            HWRevision = "";
            chargerUserName = "";
            softAPpassword = "";
            actAccessSSID = "";
            mobileAccessSSID = "";
            actViewIP = "";
            actAccessPassword = "";
            mobileAccessPassword = "";
            voltageCalA = 0;
            voltageCalB = 0;
            voltageCalALow = 0;
            voltageCalBLow = 0;
            temperatureCalALow = 0;
            temperatureCalBLow = 0;
            temperatureVI = 0;
            temperatureR = 0;
            IR = 0;
            InstallationDate = new DateTime();
            this.finishDaysMask = new DaysMask();
            this.autoStartMask = new DaysMask();
            this.EQdaysMask = new DaysMask();
            this.energyDaysMask = new DaysMask();
            this.lockoutDaysMask = new DaysMask();
            this.id = "0";
            this.oemid = 0;
            this.customerid = 0;
            this.siteid = 0;
            this.cc_ramping_min_steps = 0;
            this.nominal_temperature_shift = 0;
            unused = new byte[0];


        }
        #endregion

        private bool validateAutoStartTimer(string startTime)
        {
            int temp;
            if (!int.TryParse(startTime, out temp))
                return false;
            if (temp < 0 && temp > 200)
                return false;
            return true;

        }

        [JsonIgnore]
        public static string[] batteryTypes = SharedTexts.ALL_BATTERIES;

        private const float VOLTAGE_CAL_A_ERROR = -6f;
        private const float VOLTAGE_CAL_B_ERROR = 2.965544f;
        private const float VOLTAGE_CAL_A_LOW_ERROR = 16.40163f;
        private const float VOLTAGE_CAL_B_LOW_ERROR = 1.735948f;

        private bool validateDurationsFormat(string duration)
        {
            float temp;
            if (float.TryParse((duration.Remove(2, 1)).Insert(2, "."), out temp))
                if (temp < 24)
                    return true;
            return false;

        }
        internal byte[] SerializeMe(bool forSynch = false)
        {
            byte[] result = new byte[dataSize];
            for (int i = 0; i < result.Length; i++)
                result[i] = 0;
            float tempFloat;
            byte tempByte;
            UInt16 tempShort;
            int loc;
            if (forSynch)
            {
                loc = 0;
                result[loc++] = setup;
                result[loc++] = version;
                Array.Copy(BitConverter.GetBytes(UInt16.Parse(memorySignature)), 0, result, loc, 2);
                loc += 2;
                Array.Copy(BitConverter.GetBytes(UInt32.Parse(id)), 0, result, loc, 4);

                loc += 4;
                UInt32 unixTimestamp_lastChangeTime = (UInt32)(lastChangeTime.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc))).TotalSeconds;
                Array.Copy(BitConverter.GetBytes(unixTimestamp_lastChangeTime), 0, result, loc, 4);
                loc += 4;

            }
            else
            {
                loc = 12;
            }

            //last change User ID
            this.lastChangeUserId = ControlObject.userID.ToString();
            Array.Copy(BitConverter.GetBytes(ControlObject.userID), 0, result, loc, 4);
            loc += 4;
            //installation Date
            UInt32 unixTimestamp = (UInt32)(InstallationDate.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc))).TotalSeconds;
            Array.Copy(BitConverter.GetBytes(unixTimestamp), 0, result, loc, 4);
            loc += 4;
            //Voltage calibration A
            Array.Copy(BitConverter.GetBytes(voltageCalA), 0, result, loc, 4);
            loc += 4;
            //Voltage Calibration B
            Array.Copy(BitConverter.GetBytes(voltageCalB), 0, result, loc, 4);
            loc += 4;
            //Voltage calibration A LOW
            Array.Copy(BitConverter.GetBytes(voltageCalALow), 0, result, loc, 4);
            loc += 4;
            //Voltage Calibration B LOW
            Array.Copy(BitConverter.GetBytes(voltageCalBLow), 0, result, loc, 4);
            loc += 4;
            //Temperature Calibration A
            //Voltage calibration A
            Array.Copy(BitConverter.GetBytes(temperatureCalALow), 0, result, loc, 4);
            loc += 4;
            //Voltage Calibration B
            Array.Copy(BitConverter.GetBytes(temperatureCalBLow), 0, result, loc, 4);
            loc += 4;
            //Voltage Vi
            Array.Copy(BitConverter.GetBytes(temperatureVI), 0, result, loc, 4);
            loc += 4;
            //Temperature Res
            Array.Copy(BitConverter.GetBytes(temperatureR), 0, result, loc, 4);
            loc += 4;
            //Input Resistance
            Array.Copy(BitConverter.GetBytes(IR), 0, result, loc, 4);
            loc += 4;
            //Temp Fa
            Array.Copy(BitConverter.GetBytes(temperatureSensorSHCa), 0, result, loc, 4);
            loc += 4;
            //Temp FB
            Array.Copy(BitConverter.GetBytes(temperatureSensorSHCb), 0, result, loc, 4);
            loc += 4;
            //Temp FC
            Array.Copy(BitConverter.GetBytes(temperatureSensorSHCc), 0, result, loc, 4);
            loc += 4;



            //configurationWord
            if (enableChargerSimulationMode)
                result[loc] |= 0x02;
            if (enableAutoDetectMultiVoltage)
                result[loc] |= 0x40;
            if (temperatureSensorInstalled)
                result[loc] |= 0x04;

            if (enableRefreshCycleAfterFI)
                result[loc] |= 0x08;
            if (enableRefreshCycleAfterEQ)
                result[loc] |= 0x10;
            if (enablePMsimulation)
                result[loc] |= 0x20;
            loc++;
            // (configurationWord)
            result[loc] = (byte)(this.chargerType & 0x03);
            loc++;
            //Battery Capacity
            Array.Copy(BitConverter.GetBytes(short.Parse(batteryCapacity)), 0, result, loc, 2);
            loc += 2;
            //TR voltage
            float.TryParse(trickleVoltage, out tempFloat);
            Array.Copy(BitConverter.GetBytes((UInt16)(Math.Round(tempFloat * 100, 0))), 0, result, loc, 2);
            loc += 2;
            //CV voltage
            float.TryParse(CVvoltage, out tempFloat);
            Array.Copy(BitConverter.GetBytes((UInt16)(Math.Round(tempFloat * 100, 0))), 0, result, loc, 2);
            loc += 2;
            //FI voltage
            float.TryParse(FIvoltage, out tempFloat);
            Array.Copy(BitConverter.GetBytes((UInt16)(Math.Round(tempFloat * 100, 0))), 0, result, loc, 2);
            loc += 2;
            //EQ voltage
            float.TryParse(this.EQvoltage, out tempFloat);
            Array.Copy(BitConverter.GetBytes((UInt16)(Math.Round(tempFloat * 100, 0))), 0, result, loc, 2);
            loc += 2;
            //max temperature
            tempFloat = float.Parse(this.maxTemperatureFault);
            Array.Copy(BitConverter.GetBytes((UInt16)(Math.Round(tempFloat * 10, 0))), 0, result, loc, 2);
            loc += 2;
            //TR rate
            tempFloat = TRrate;
            tempShort = (UInt16)tempFloat;
            Array.Copy(BitConverter.GetBytes(tempShort), 0, result, loc, 2);
            loc += 2;
            //CC rate
            tempFloat = CCrate;
            tempShort = (UInt16)tempFloat;
            Array.Copy(BitConverter.GetBytes(tempShort), 0, result, loc, 2);
            loc += 2;
            //FI rate
            tempFloat = 5;
            tempFloat = FIrate;
            tempShort = (UInt16)tempFloat;
            Array.Copy(BitConverter.GetBytes(tempShort), 0, result, loc, 2);
            loc += 2;
            //EQ rate
            tempFloat = 4;
            tempFloat = EQrate;
            tempShort = (UInt16)tempFloat;
            Array.Copy(BitConverter.GetBytes(tempShort), 0, result, loc, 2);
            loc += 2;
            //PM EFF
            tempFloat = 4;
            float.TryParse(PMefficiency, out tempFloat);
            tempFloat *= 100;
            tempShort = (UInt16)tempFloat;
            Array.Copy(BitConverter.GetBytes(tempShort), 0, result, loc, 2);
            loc += 2;
            //actViewConnectFrequency
            tempShort = 5;
            UInt16.TryParse(actViewConnectFrequency, out tempShort);
            Array.Copy(BitConverter.GetBytes(tempShort), 0, result, loc, 2);
            loc += 2;

            //actViewPort
            tempShort = 32826;
            UInt16.TryParse(actViewPort, out tempShort);
            Array.Copy(BitConverter.GetBytes(tempShort), 0, result, loc, 2);
            loc += 2;
            //mobileViewPort
            tempShort = 32826;
            UInt16.TryParse(mobilePort, out tempShort);
            Array.Copy(BitConverter.GetBytes(tempShort), 0, result, loc, 2);
            loc += 2;
            //actViewIP
            if (version == 1)
            {
                Array.Copy(Encoding.UTF8.GetBytes(actViewIP), 0, result, loc, actViewIP.Length);
                result[loc + actViewIP.Length] = 0;
                loc += 64;
            }
            else
            {
                Array.Copy(Encoding.UTF8.GetBytes(actViewIP), 0, result, loc, actViewIP.Length);
                result[loc + actViewIP.Length] = 0;
                loc += 32;

                //actAccessSSIDpassword
                Array.Copy(Encoding.UTF8.GetBytes(actAccessPassword), 0, result, loc, actAccessPassword.Length);
                result[loc + actAccessPassword.Length] = 0;
                loc += 32;
            }



            //actAccessSSID
            Array.Copy(Encoding.UTF8.GetBytes(actAccessSSID), 0, result, loc, actAccessSSID.Length);
            result[loc + actAccessSSID.Length] = 0;
            loc += 32;
            //mobileAccessSSID
            Array.Copy(Encoding.UTF8.GetBytes(mobileAccessSSID), 0, result, loc, mobileAccessSSID.Length);
            result[loc + mobileAccessSSID.Length] = 0;
            loc += 32;

            //Serial number
            Array.Copy(Encoding.UTF8.GetBytes(serialNumber), 0, result, loc, serialNumber.Length);
            result[loc + serialNumber.Length] = 0;
            loc += 15;
            result[loc++] = 0;
            //Model
            if (model.Length < 15)
                model = model.Insert(model.Length, new string(' ', 15 - model.Length));
            Array.Copy(Encoding.UTF8.GetBytes(model), 0, result, loc, 15);
            loc += 15;
            result[loc++] = 0;
            //softAPpassword
            Array.Copy(Encoding.UTF8.GetBytes(softAPpassword), 0, result, loc, softAPpassword.Length);
            result[loc + softAPpassword.Length] = 0;
            loc += 13;
            result[loc++] = 0;
            if (version == 1)
            {
                //actAccessSSIDpassword
                Array.Copy(Encoding.UTF8.GetBytes(actAccessPassword), 0, result, loc, actAccessPassword.Length);
                result[loc + actAccessPassword.Length] = 0;
                loc += 14;
            }
            //mobileAccessSSIDpassword
            Array.Copy(Encoding.UTF8.GetBytes(mobileAccessPassword), 0, result, loc, mobileAccessPassword.Length);
            result[loc + mobileAccessPassword.Length] = 0;
            loc += 14;
            //USER NAME
            Array.Copy(Encoding.UTF8.GetBytes(chargerUserName), 0, result, loc, chargerUserName.Length);
            result[loc + chargerUserName.Length] = 0;
            loc += 24;
            //HW Version
            if (HWRevision.Length == 0)
                HWRevision = "A ";
            else if (HWRevision.Length == 1)
            {
                HWRevision += " ";
            }
            result[loc++] = (byte)(HWRevision[0]);
            result[loc++] = (byte)(HWRevision[1]);
            //EQ Start Time
            Match match;
            ushort st;
            match = Regex.Match(EQstartWindow, @"^(\d{1,2}){1}:(\d{1,2}){1}$", RegexOptions.IgnoreCase);
            st = (ushort)(ushort.Parse(match.Groups[1].Value) * 60 + ushort.Parse(match.Groups[2].Value));
            Array.Copy(BitConverter.GetBytes(st), 0, result, loc, 2);
            loc += 2;
            //Finish Start Time
            match = Regex.Match(FIstartWindow, @"^(\d{1,2}){1}:(\d{1,2}){1}$", RegexOptions.IgnoreCase);
            st = (ushort)(ushort.Parse(match.Groups[1].Value) * 60 + ushort.Parse(match.Groups[2].Value));
            Array.Copy(BitConverter.GetBytes(st), 0, result, loc, 2);
            loc += 2;
            //auto start
            if (validateAutoStartTimer(autoStartCountDownTimer))
            {
                result[loc] = (byte)int.Parse(autoStartCountDownTimer);
            }
            else
            {
                result[loc] = 12;
            }
            loc++;
            //Finish Sdays mask
            result[loc] = this.finishDaysMask.SerializeMe();
            loc++;
            //Delay Start Enable byte
            if (this.autoStartEnable)
                result[loc] = 1;
            else result[loc] = 0;
            loc++;
            //Delay Start Delay Mask
            result[loc] = 0x7F;// this.DelayedStartDaysMask.SerializeMe();
            loc++;
            //Finish Enable
            if (this.FIschedulingMode)
                result[loc] = 1;
            else result[loc] = 0;
            loc++;
            //Finish Duration


            match = Regex.Match(finishWindow, @"^(\d{1,2}){1}:(\d{1,2}){1}$", RegexOptions.IgnoreCase);
            st = (ushort)(ushort.Parse(match.Groups[1].Value) * 60 + ushort.Parse(match.Groups[2].Value));
            st /= 15;
            result[loc++] = (byte)st;



            //EQ Days Mask
            result[loc] = EQdaysMask.SerializeMe();
            loc++;
            //EQ Duration

            match = Regex.Match(this.EQwindow, @"^(\d{1,2}){1}:(\d{1,2}){1}$", RegexOptions.IgnoreCase);
            st = (ushort)(ushort.Parse(match.Groups[1].Value) * 60 + ushort.Parse(match.Groups[2].Value));
            st /= 15;
            result[loc++] = (byte)st;

            //Days light saving enable
            if (this.daylightSaving)
                result[loc] = 1;
            else
                result[loc] = 0;
            loc++;
            //Temperature format
            if (this.temperatureFormat)
                result[loc] = 1;
            else
                result[loc] = 0;
            loc++;
            //number of installed PMs
            result[loc] = byte.Parse(this.numberOfInstalledPMs);
            loc++;
            //PM voltage
            result[loc] = byte.Parse(this.PMvoltage);
            loc++;
            //Battery type
            result[loc] = (byte)Array.IndexOf(batteryTypes, batteryType);
            if (result[loc] > 2 || result[loc] < 0)
                result[loc] = 0;
            loc++;
            //temperatureCompensation
            tempFloat = 3;
            float.TryParse(temperatureVoltageCompensation, out tempFloat);
            result[loc++] = (byte)Math.Round(tempFloat * 10);
            //battery voltage
            byte.TryParse(batteryVoltage, out result[loc++]);
            //finishdV
            result[loc++] = finishDV;//byte.TryParse(finishdV, out );
            //cv current step
            result[loc] = CVcurrentStep;

            loc++;
            //cv Finish Current
            result[loc++] = CVfinishCurrent;
            //CV timer

            byte.TryParse(CVtimer.Remove(2, 3), out tempByte);
            byte.TryParse(CVtimer.Remove(0, 3), out result[loc]);
            result[loc] = (byte)((byte)(result[loc] / 15) + (byte)(tempByte * 4));


            loc++;
            //finish timer

            byte.TryParse(finishTimer.Remove(2, 3), out tempByte);
            byte.TryParse(finishTimer.Remove(0, 3), out result[loc]);
            result[loc] = (byte)((byte)(result[loc] / 15) + (byte)(tempByte * 4));


            loc++;
            //finish dt
            byte.TryParse(finishDT, out result[loc]);
            if (result[loc] == 0)
                result[loc] = 60;
            loc++;

            byte.TryParse(EQtimer.Remove(2, 3), out tempByte);
            byte.TryParse(EQtimer.Remove(0, 3), out result[loc]);
            result[loc] = (byte)((byte)(result[loc] / 15) + (byte)(tempByte * 4));


            loc++;
            //Refresh Timer

            byte.TryParse(refreshTimer.Remove(2, 3), out tempByte);
            byte.TryParse(refreshTimer.Remove(0, 3), out result[loc]);
            result[loc] = (byte)((byte)(result[loc] / 15) + (byte)(tempByte * 4));


            loc++;
            //desulfation timer

            byte.TryParse(desulfationTimer.Remove(2, 3), out tempByte);
            byte.TryParse(desulfationTimer.Remove(0, 3), out result[loc]);
            result[loc] = (byte)((byte)(result[loc] / 15) + (byte)(tempByte * 4));


            loc++;
            result[loc++] = (byte)(softAPenabled ? 1 : 0);
            result[loc++] = (byte)(actViewEnable ? 1 : 0);
            byte.TryParse(lcdMemoryVersion, out result[loc++]);
            byte.TryParse(wifiFirmwareVersion, out result[loc++]);

            byte plc_control = (byte)(enablePLC ? 1 : 0);
            if (!doPLCStackCheck && this.firmwareVersion >= 2.51f)
                plc_control |= 0x10;
            //doPLCStackCheck
            result[loc++] = plc_control;
            result[loc++] = (byte)(enableManualEQ ? 1 : 0);
            result[loc++] = (byte)(enableManualDesulfate ? 1 : 0);
            result[loc++] = energyDaysMask.SerializeMe();
            Array.Copy(BitConverter.GetBytes(lockoutStartTime), 0, result, loc, 4);
            loc += 4;
            Array.Copy(BitConverter.GetBytes(lockoutCloseTime), 0, result, loc, 4);
            loc += 4;
            Array.Copy(BitConverter.GetBytes(energyStartTime), 0, result, loc, 4);
            loc += 4;
            Array.Copy(BitConverter.GetBytes(energyCloseTime), 0, result, loc, 4);
            loc += 4;
            result[loc++] = lockoutDaysMask.SerializeMe();
            result[loc++] = energyDecreaseValue;
            result[loc++] = PMvoltageInputValue;
            result[loc++] = (byte)(disablePushButton ? 1 : 0);
            //Serial number
            if (originalSerialNumber.Length < 16)
                originalSerialNumber = originalSerialNumber.Insert(originalSerialNumber.Length, new string(' ', 16 - originalSerialNumber.Length));
            Array.Copy(Encoding.UTF8.GetBytes(originalSerialNumber), 0, result, loc, 16);
            loc += 16;




            tempFloat = float.Parse(this.TRtemperature);
            Array.Copy(BitConverter.GetBytes((UInt16)(Math.Round(tempFloat * 10, 0))), 0, result, loc, 2);
            loc += 2;
            Array.Copy(BitConverter.GetBytes(afterCommissionBoardID), 0, result, loc, 4);
            loc += 4;
            //afterCommissionBoardID
            tempFloat = float.Parse(this.foldTemperature);
            Array.Copy(BitConverter.GetBytes((UInt16)(Math.Round(tempFloat * 10, 0))), 0, result, loc, 2);
            loc += 2;
            tempFloat = float.Parse(this.coolDownTemperature);
            Array.Copy(BitConverter.GetBytes((UInt16)(Math.Round(tempFloat * 10, 0))), 0, result, loc, 2);
            loc += 2;
            result[loc++] = ledcontrol;

            result[loc++] = cc_ramping_min_steps;



            Array.Copy(BitConverter.GetBytes(batteryCapacity24), 0, result, loc, 2);
            loc += 2;
            Array.Copy(BitConverter.GetBytes(batteryCapacity36), 0, result, loc, 2);
            loc += 2;
            Array.Copy(BitConverter.GetBytes(batteryCapacity48), 0, result, loc, 2);
            loc += 2;


            result[loc++] = (byte)(forceFinishTimeout ? 0x01 : 0x00);
            result[loc++] = (byte)(replacmentPart ? 0x01 : 0x00);
            result[loc++] = (byte)(chargerOverrideBattviewFIEQsched ? 0x01 : 0x00);
            result[loc++] = (byte)(ignoreBATTViewSOC ? 0x01 : 0x00);
            result[loc++] = (byte)(battviewAutoCalibrationEnable ? 0x01 : 0x00);

            result[loc++] = (byte)this.nominal_temperature_shift;
            Array.Copy(BitConverter.GetBytes(batteryCapacity80), 0, result, loc, 2);
            loc += 2;
            result[loc++] = FIStopCurrent;
            result[loc++] = (byte)(useNewEastPennProfile ? 0x01 : 0x00);
            result[loc++] = OCD_Remote;
            result[loc++] = Enable_72V;
            Array.Copy(BitConverter.GetBytes(LiIon_CellVoltage), 0, result, loc, 2);
            loc += 2;
            result[loc++] = LiIon_numberOfCells;
            loc++;  //Unused byte for memory allignment
            Array.Copy(BitConverter.GetBytes(ReconnectTimer), 0, result, loc, 2);
            loc += 2;

            result[loc++] = (byte)(LCD_FQ ? 0x01 : 0x00);
            result[loc++] = DaughterCardEnabled;
            result[loc++] = PMMaxCurrent;
            result[loc++] = defaultBrightness;
            Array.Copy(BitConverter.GetBytes(bmsBitRate), 0, result, loc, 2);
            loc += 2;
            result[loc++] = plc_gain;
            result[loc++] = truck_image_id;
            if (forSynch || 512 - loc != unused.Length)
            {
                for (; loc < result.Length; loc++)
                    result[loc] = 0;
            }
            else
            {
                Array.Copy(unused, 0, result, loc, unused.Length);

            }
            //unsued

            return result;


        }


        internal CommunicationResult LoadFromArray(byte[] result, DeviceBaseType dType, float knowFW = 0.0f)
        {
            int loc = 0;

            if (knowFW > this.firmwareVersion)
                this.firmwareVersion = knowFW;
            setup = result[loc++];
            version = result[loc++];
            if (version > maxSupportedVersion)
            {
                return CommunicationResult.ReadSomethingElse;
            }
            memorySignature = BitConverter.ToUInt16(result, loc).ToString();
            loc += 2;
            //InternalChargerID
            id = BitConverter.ToUInt32(result, loc).ToString();
            loc += 4;
            //lastChangeTime
            lastChangeTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(BitConverter.ToUInt32(result, loc));
            loc += 4;
            //lastChangeTime
            lastChangeUserId = BitConverter.ToUInt32(result, loc).ToString();
            loc += 4;

            //installation date
            UInt32 inst = BitConverter.ToUInt32(result, loc);
            InstallationDate = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(inst);
            loc += 4;
            //Voltage Calibraton A
            voltageCalA = BitConverter.ToSingle(result, loc);
            loc += 4;
            //Voltage Calibration B
            voltageCalB = BitConverter.ToSingle(result, loc);
            loc += 4;
            //Voltage Calibraton A LOW
            voltageCalALow = BitConverter.ToSingle(result, loc);
            loc += 4;
            //Voltage Calibration B LOW
            voltageCalBLow = BitConverter.ToSingle(result, loc);
            loc += 4;
            //Temperature Calibraton A
            temperatureCalALow = BitConverter.ToSingle(result, loc);
            loc += 4;
            //Temperature Calibration B
            temperatureCalBLow = BitConverter.ToSingle(result, loc);
            loc += 4;
            //Temperature Vi
            temperatureVI = BitConverter.ToSingle(result, loc);
            loc += 4;
            //Temperature Res
            temperatureR = BitConverter.ToSingle(result, loc);
            loc += 4;
            //IR
            IR = BitConverter.ToSingle(result, loc);
            loc += 4;
            //Temp FA
            temperatureSensorSHCa = BitConverter.ToSingle(result, loc);
            loc += 4;
            //Temp FB
            temperatureSensorSHCb = BitConverter.ToSingle(result, loc);
            loc += 4;
            //Temp FC
            temperatureSensorSHCc = BitConverter.ToSingle(result, loc);
            loc += 4;

            //configuration word
            if ((result[loc] & 0x02) != 0)
                enableChargerSimulationMode = true;
            else enableChargerSimulationMode = false;

            if ((result[loc] & 0x40) != 0)
                enableAutoDetectMultiVoltage = true;
            else enableAutoDetectMultiVoltage = false;
            if ((result[loc] & 0x04) != 0)
                temperatureSensorInstalled = true;
            else temperatureSensorInstalled = false;

            if ((result[loc] & 0x08) != 0)
                enableRefreshCycleAfterFI = true;
            else
                enableRefreshCycleAfterFI = false;

            if ((result[loc] & 0x10) != 0)
                enableRefreshCycleAfterEQ = true;
            else
                enableRefreshCycleAfterEQ = false;
            if ((result[loc] & 0x20) != 0)
                enablePMsimulation = true;
            else
                enablePMsimulation = false;
            loc++;
            chargerType = (byte)(result[loc] & 0x03);
            loc++;
            //battery capacity
            batteryCapacity = (BitConverter.ToUInt16(result, loc).ToString());
            loc += 2;
            //TR voltage
            trickleVoltage = (BitConverter.ToInt16(result, loc) / 100.0).ToString();
            loc += 2;
            //CV voltage
            CVvoltage = (BitConverter.ToInt16(result, loc) / 100.0).ToString();
            loc += 2;
            //FI voltage
            FIvoltage = (BitConverter.ToInt16(result, loc) / 100.0).ToString();
            loc += 2;
            //EQ voltage
            EQvoltage = (BitConverter.ToInt16(result, loc) / 100.0).ToString();
            loc += 2;
            //max temperature
            maxTemperatureFault = (BitConverter.ToInt16(result, loc) / 10.0).ToString();
            loc += 2;
            //TR
            TRrate = BitConverter.ToUInt16(result, loc);
            loc += 2;
            //CC rate
            CCrate = BitConverter.ToUInt16(result, loc);
            loc += 2;
            //FI rate
            FIrate = BitConverter.ToUInt16(result, loc);
            loc += 2;
            //EQ rate
            EQrate = BitConverter.ToUInt16(result, loc);
            loc += 2;
            //PM_effieciency
            PMefficiency = (BitConverter.ToInt16(result, loc) / 100.0).ToString("f");
            loc += 2;
            //actViewConnectFrequency
            actViewConnectFrequency = (BitConverter.ToInt16(result, loc)).ToString();
            loc += 2;
            //actViewPort
            actViewPort = BitConverter.ToUInt16(result, loc).ToString();
            loc += 2;
            //mobile Port
            mobilePort = BitConverter.ToUInt16(result, loc).ToString();
            loc += 2;
            //actViewIP
            if (version == 1)
            {
                actViewIP = Encoding.UTF8.GetString(result, loc, 64).TrimEnd('\0');
                loc += 64;
            }
            else
            {
                actViewIP = Encoding.UTF8.GetString(result, loc, 32).TrimEnd('\0');
                loc += 32;
                actAccessPassword = Encoding.UTF8.GetString(result, loc, 32).TrimEnd('\0');
                loc += 32;
            }
            //actAccessSSID
            actAccessSSID = Encoding.UTF8.GetString(result, loc, 32).TrimEnd('\0');
            loc += 32;
            //mobileAccessSSID
            mobileAccessSSID = Encoding.UTF8.GetString(result, loc, 32).TrimEnd('\0');
            loc += 32;
            //Serial number
            serialNumber = Encoding.UTF8.GetString(result, loc, 12).TrimEnd('\0');
            loc += 16;
            //Model
            model = Encoding.UTF8.GetString(result, loc, 16).TrimEnd('\0');
            loc += 16;
            model = model.Trim().Trim('\0').Trim();
            //softAPpassword
            softAPpassword = Encoding.UTF8.GetString(result, loc, 14).TrimEnd('\0');
            loc += 14;
            //actAccessSSIDpassword
            if (version == 1)
            {
                actAccessPassword = Encoding.UTF8.GetString(result, loc, 14).TrimEnd('\0');
                loc += 14;
            }
            //mobileAccessSSIDpassword
            mobileAccessPassword = Encoding.UTF8.GetString(result, loc, 14).TrimEnd('\0');
            loc += 14;
            //USER NAME
            chargerUserName = Encoding.UTF8.GetString(result, loc, 24).TrimEnd('\0');
            loc += 24;
            //HW version
            HWRevision = ((char)result[loc++]).ToString();
            HWRevision += ((char)result[loc++]).ToString();

            //EQ start window
            ushort st;
            st = BitConverter.ToUInt16(result, loc);
            EQstartWindow = String.Format("{0:00}:{1:00}", st / 60, (st % 60));

            loc += 2;
            //FI start window
            st = BitConverter.ToUInt16(result, loc);
            FIstartWindow = String.Format("{0:00}:{1:00}", st / 60, (st % 60));
            loc += 2;
            //autostart 
            autoStartCountDownTimer = (((UInt16)result[loc])).ToString();
            loc++;
            //FI mask
            finishDaysMask.DeSerializeMe(result[loc]);
            loc++;
            //auto start enable
            if (result[loc] == 1)
                autoStartEnable = true;
            else autoStartEnable = false;
            loc++;
            //autostart mask
            this.autoStartMask.DeSerializeMe(result[loc]);
            loc++;
            //FI enable
            if (result[loc] == 0)
                FIschedulingMode = false;
            else FIschedulingMode = true;
            loc++;
            //FI duration
            finishWindow = String.Format("{0:00}:{1:00}", result[loc] / 4, (result[loc] % 4) * 15);
            loc++;
            //EQ days mask
            EQdaysMask.DeSerializeMe(result[loc]);
            loc++;
            //EQ duration
            EQwindow = String.Format("{0:00}:{1:00}", result[loc] / 4, (result[loc] % 4) * 15);
            loc++;
            //days light 
            if (result[loc] == 1)
                daylightSaving = true;
            else
                daylightSaving = false;
            loc++;
            //temp format
            if (result[loc] == 1)
                temperatureFormat = true;
            else
                temperatureFormat = false;
            loc++;
            //#of installed PMs
            this.numberOfInstalledPMs = result[loc].ToString();
            if (this.numberOfInstalledPMs.Length == 1)
            {
                numberOfInstalledPMs = "0" + numberOfInstalledPMs;
            }
            loc++;
            //PM voltage
            this.PMvoltage = result[loc++].ToString();

            //battery type
            if (result[loc] > 2)
                result[loc] = 0;
            batteryType = batteryTypes[result[loc]];
            loc++;
            //temperatureCompensation
            temperatureVoltageCompensation = (result[loc++] / 10.0f).ToString();
            //battery voltage
            batteryVoltage = result[loc++].ToString();
            //FI dv voltage
            finishDV = result[loc++];
            //CV current step
            CVcurrentStep = result[loc];

            loc++;
            //CV finish current
            CVfinishCurrent = result[loc++];

            CVtimer = String.Format("{0:00}:{1:00}", result[loc] / 4, (result[loc] % 4) * 15);
            loc++;

            finishTimer = String.Format("{0:00}:{1:00}", result[loc] / 4, (result[loc] % 4) * 15);
            loc++;
            finishDT = result[loc].ToString();
            loc++;
            EQtimer = String.Format("{0:00}:{1:00}", result[loc] / 4, (result[loc] % 4) * 15);
            loc++;
            refreshTimer = String.Format("{0:00}:{1:00}", result[loc] / 4, (result[loc] % 4) * 15);
            loc++;
            desulfationTimer = String.Format("{0:00}:{1:00}", result[loc] / 4, (result[loc] % 4) * 15);
            loc++;
            softAPenabled = result[loc++] != 0;
            actViewEnable = result[loc++] != 0;
            lcdMemoryVersion = result[loc++].ToString();
            wifiFirmwareVersion = result[loc++].ToString();
            byte plc_control = result[loc++];
            if ((plc_control & 0x01) != 0)
                enablePLC = true;
            else
                enablePLC = false;

            if ((plc_control & 0x10) == 0 || this.firmwareVersion < 2.51f)
                this.doPLCStackCheck = true;
            else
                this.doPLCStackCheck = false;


            enableManualEQ = result[loc++] != 0;
            enableManualDesulfate = result[loc++] != 0;

            energyDaysMask.DeSerializeMe(result[loc++]);
            lockoutStartTime = BitConverter.ToUInt32(result, loc);
            loc += 4;
            lockoutCloseTime = BitConverter.ToUInt32(result, loc);
            loc += 4;
            energyStartTime = BitConverter.ToUInt32(result, loc);
            loc += 4;
            energyCloseTime = BitConverter.ToUInt32(result, loc);
            loc += 4;
            lockoutDaysMask.DeSerializeMe(result[loc++]);
            energyDecreaseValue = result[loc++];
            PMvoltageInputValue = result[loc++];
            disablePushButton = result[loc++] > 0;
            originalSerialNumber = Encoding.UTF8.GetString(result, loc, 16).TrimEnd('\0');
            loc += 16;
            originalSerialNumber.Trim();
            TRtemperature = (BitConverter.ToInt16(result, loc) / 10.0).ToString();
            loc += 2;


            afterCommissionBoardID = BitConverter.ToUInt32(result, loc);
            loc += 4;
            //afterCommissionBoardID
            foldTemperature = (BitConverter.ToInt16(result, loc) / 10.0).ToString();
            loc += 2;
            coolDownTemperature = (BitConverter.ToInt16(result, loc) / 10.0).ToString();
            loc += 2;
            ledcontrol = result[loc++];
            //1 byte missed aligned
            cc_ramping_min_steps = result[loc++];

            batteryCapacity24 = BitConverter.ToUInt16(result, loc);
            loc += 2;
            batteryCapacity36 = BitConverter.ToUInt16(result, loc);
            loc += 2;
            batteryCapacity48 = BitConverter.ToUInt16(result, loc);
            loc += 2;
            if (batteryCapacity24 == 0)
                batteryCapacity24 = UInt16.Parse(batteryCapacity);
            if (batteryCapacity36 == 0)
                batteryCapacity36 = UInt16.Parse(batteryCapacity);
            if (batteryCapacity48 == 0)
                batteryCapacity48 = UInt16.Parse(batteryCapacity);


            forceFinishTimeout = (result[loc++] != 0);
            if (dType == DeviceBaseType.MCB)
                replacmentPart = (result[loc++] != 0);
            else
                replacmentPart = false;
            if (UInt32.Parse(id) < 10000)
                replacmentPart = false;
            chargerOverrideBattviewFIEQsched = (result[loc++] != 0);
            ignoreBATTViewSOC = (result[loc++] != 0);

            battviewAutoCalibrationEnable = (result[loc++] != 0);
            this.nominal_temperature_shift = (sbyte)result[loc++];
            batteryCapacity80 = BitConverter.ToUInt16(result, loc);
            loc += 2;
            FIStopCurrent = result[loc++];
            useNewEastPennProfile = result[loc++] > 0;
            OCD_Remote = result[loc++];
            Enable_72V = (byte)(result[loc++] != 0 ? 1 : 0);
            if (batteryCapacity80 == 0)
                batteryCapacity80 = UInt16.Parse(batteryCapacity);
            LiIon_CellVoltage = BitConverter.ToUInt16(result, loc);
            loc += 2;
            LiIon_numberOfCells = result[loc++];
            loc++;  //Unused byte for memory allignment
            ReconnectTimer = BitConverter.ToUInt16(result, loc);
            loc += 2;
            if (version != 1)
            {
                LCD_FQ = result[loc++] != 0;
                DaughterCardEnabled = result[loc++];
                PMMaxCurrent = result[loc++];
                defaultBrightness = result[loc++];
                bmsBitRate = BitConverter.ToUInt16(result, loc);
                loc += 2;
                plc_gain = result[loc++];
                truck_image_id = result[loc++];

            }

            if (version == 1)
            {
                unused = new byte[configV1UnusedLength];
            }
            else
            {
                unused = new byte[configV2UnusedLength];

            }
            Array.Copy(result, loc, unused, 0, unused.Length);

            return CommunicationResult.OK;

        }

        public bool HasCalibrationError()
        {
            return (voltageCalA == VOLTAGE_CAL_A_ERROR && voltageCalB == VOLTAGE_CAL_B_ERROR) ||
                (voltageCalALow == VOLTAGE_CAL_A_LOW_ERROR && voltageCalBLow == VOLTAGE_CAL_B_LOW_ERROR);
        }

        public string ToJson()
        {
            CorrectValues();

            return JsonConvert.SerializeObject(this);
        }

        void CorrectValues()
        {
            HWRevision = HWRevision.Trim();
        }
    }
}
