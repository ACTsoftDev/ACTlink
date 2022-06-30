using System;
using System.Text;
using Newtonsoft.Json;

namespace actchargers
{
    public class BattViewConfig
    {
        [JsonIgnore]
        public bool LoadedFromArray { get; private set; }

        [JsonIgnore]
        public bool NotLoaded { get { return !LoadedFromArray; } }

        [JsonIgnore]
        public bool IsSoftApEnable
        {
            get
            {
                lock (myLock)
                {
                    return Convert.ToBoolean(softAPEnable);
                }
            }
            set
            {
                lock (myLock)
                {
                    softAPEnable = Convert.ToByte(value);
                }
            }
        }

        public bool IsBattViewMobile()
        {
            return (isPA != 0);
        }

        [JsonIgnore]
        public bool ActViewEnabled
        {
            get
            {
                lock (myLock)
                {
                    return Convert.ToBoolean(actViewEnabled);
                }
            }
            set
            {
                lock (myLock)
                {
                    actViewEnabled = Convert.ToByte(value);
                }
            }
        }

        [JsonIgnore]
        private string _studyName;
        [JsonProperty]
        public string studyName
        {
            get
            {
                lock (myLock)
                {
                    return _studyName;
                }
            }
            set
            {
                lock (myLock)
                {
                    _studyName = value;
                }
            }
        }
        [JsonIgnore]
        private string _TruckId;
        [JsonProperty]
        public string TruckId
        {
            get
            {
                lock (myLock)
                {
                    return _TruckId;
                }
            }
            set
            {
                lock (myLock)
                {
                    _TruckId = value;
                }
            }
        }
        [JsonIgnore]
        public bool commissionRequest;
        [JsonIgnore]
        private object myLock;
        [JsonIgnore]
        const int configV1UnusedLength = 12;
        [JsonIgnore]
        const int configV2UnusedLength = 26;
        [JsonIgnore]
        const int maxSupportedVersion = 2;
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
        private byte _zoneid;
        [JsonProperty]
        public byte zoneid
        {
            get
            {
                lock (myLock)
                {
                    return _zoneid;
                }
            }
            set
            {
                lock (myLock)
                {
                    _zoneid = value;
                }
            }
        }
        [JsonIgnore]
        private float _firmwareversion;
        [JsonProperty]
        public float firmwareversion
        {
            get
            {
                lock (myLock)
                {
                    return _firmwareversion;
                }
            }
            set
            {
                lock (myLock)
                {
                    _firmwareversion = value;
                }
            }
        }
        //public bool ShouldSerializestudyName() {
        //    if (this.isPA != 0 && studyName != null  && studyName != "")
        //        return true;
        //    else
        //        return false;
        //}
        //public bool ShouldSerializeTruckId()
        //{
        //    if (this.isPA != 0 && TruckId!=null && TruckId != "")
        //        return true;
        //    else
        //        return false;
        //}

        public bool ShouldSerializedealerid()
        {
            return commissionRequest;
        }
        public bool ShouldSerializeservicedealerid()
        {
            return commissionRequest;
        }
        public bool ShouldSerializesiteid()
        {
            return commissionRequest;
        }
        public bool ShouldSerializeoemid()
        {
            return commissionRequest;
        }
        public bool ShouldSerializecustomerid()
        {
            return commissionRequest;
        }
        public bool ShouldSerializefirmwareversion()
        {
            return commissionRequest;
        }
        public bool ShouldSerializezoneid()
        {
            return commissionRequest;
        }
        #region vars

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
        [JsonIgnore]
        private byte _battviewVersion;
        [JsonProperty]
        public byte battviewVersion
        {
            get
            {
                lock (myLock)
                {
                    return _battviewVersion;
                }
            }
            set
            {
                lock (myLock)
                {
                    _battviewVersion = value;
                }
            }
        }
        [JsonIgnore]
        private UInt16 _memorySignature;
        [JsonProperty]
        public UInt16 memorySignature
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
        private DateTime _lastChangeTime;
        [JsonProperty]
        public DateTime lastChangeTime
        {
            get
            {
                lock (myLock)
                {
                    return new DateTime(_lastChangeTime.Ticks);
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
        private UInt32 _lastChangeUserID;
        [JsonProperty]
        public UInt32 lastChangeUserID
        {
            get
            {
                lock (myLock)
                {
                    return _lastChangeUserID;
                }
            }
            set
            {
                lock (myLock)
                {
                    _lastChangeUserID = value;
                }
            }
        }
        [JsonIgnore]
        private DateTime _installationDate;
        [JsonProperty]
        public DateTime installationDate
        {
            get
            {
                lock (myLock)
                {
                    return new DateTime(_installationDate.Ticks);
                }
            }
            set
            {
                lock (myLock)
                {
                    _installationDate = value;
                }
            }
        }
        [JsonIgnore]
        private UInt32 _warrantedAHR;
        [JsonProperty]
        public UInt32 warrantedAHR
        {
            get
            {
                lock (myLock)
                {
                    return _warrantedAHR;
                }
            }
            set
            {
                lock (myLock)
                {
                    _warrantedAHR = value;
                }
            }
        }
        [JsonIgnore]
        private UInt32 _cvMaxDuration;
        [JsonProperty]
        public UInt32 cvMaxDuration
        {
            get
            {
                lock (myLock)
                {
                    return _cvMaxDuration;
                }
            }
            set
            {
                lock (myLock)
                {
                    _cvMaxDuration = value;
                }
            }
        }
        [JsonIgnore]
        private UInt32 _EQstartWindow;
        [JsonProperty]
        public UInt32 EQstartWindow
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
        private UInt32 _EQcloseWindow;
        [JsonProperty]
        public UInt32 EQcloseWindow
        {
            get
            {
                lock (myLock)
                {
                    return _EQcloseWindow;
                }
            }
            set
            {
                lock (myLock)
                {
                    _EQcloseWindow = value;
                }
            }
        }
        [JsonIgnore]
        private UInt32 _FIstartWindow;
        [JsonProperty]
        public UInt32 FIstartWindow
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
        private UInt32 _FIcloseWindow;
        [JsonProperty]
        public UInt32 FIcloseWindow
        {
            get
            {
                lock (myLock)
                {
                    return _FIcloseWindow;
                }
            }
            set
            {
                lock (myLock)
                {
                    _FIcloseWindow = value;
                }
            }
        }
        [JsonIgnore]
        private UInt32 _FIduration;
        [JsonProperty]
        public UInt32 FIduration
        {
            get
            {
                lock (myLock)
                {
                    return _FIduration;
                }
            }
            set
            {
                lock (myLock)
                {
                    _FIduration = value;
                }
            }
        }
        [JsonIgnore]
        private UInt32 _EQduration;
        [JsonProperty]
        public UInt32 EQduration
        {
            get
            {
                lock (myLock)
                {
                    return _EQduration;
                }
            }
            set
            {
                lock (myLock)
                {
                    _EQduration = value;
                }
            }
        }
        [JsonIgnore]
        private UInt32 _id;
        [JsonProperty]
        public UInt32 id
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
        private UInt32 _desulfation;
        [JsonProperty]
        public UInt32 desulfation
        {
            get
            {
                lock (myLock)
                {
                    return _desulfation;
                }
            }
            set
            {
                lock (myLock)
                {
                    _desulfation = value;
                }
            }
        }
        [JsonIgnore]
        private float _tempFa;
        [JsonProperty]
        public float tempFa
        {
            get
            {
                lock (myLock)
                {
                    return _tempFa;
                }
            }
            set
            {
                lock (myLock)
                {
                    _tempFa = value;
                }
            }
        }
        [JsonIgnore]
        private float _tempFb;
        [JsonProperty]
        public float tempFb
        {
            get
            {
                lock (myLock)
                {
                    return _tempFb;
                }
            }
            set
            {
                lock (myLock)
                {
                    _tempFb = value;
                }
            }
        }
        [JsonIgnore]
        private float _tempFc;
        [JsonProperty]
        public float tempFc
        {
            get
            {
                lock (myLock)
                {
                    return _tempFc;
                }
            }
            set
            {
                lock (myLock)
                {
                    _tempFc = value;
                }
            }
        }
        [JsonIgnore]
        private float _intercellCoefficient;
        [JsonProperty]
        public float intercellCoefficient
        {
            get
            {
                lock (myLock)
                {
                    return _intercellCoefficient;
                }
            }
            set
            {
                lock (myLock)
                {
                    _intercellCoefficient = value;
                }
            }
        }
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
        private float _NTCcalA;
        [JsonProperty]
        public float NTCcalA
        {
            get
            {
                lock (myLock)
                {
                    return _NTCcalA;
                }
            }
            set
            {
                lock (myLock)
                {
                    _NTCcalA = value;
                }
            }
        }
        [JsonIgnore]
        private float _NTCcalB;
        [JsonProperty]
        public float NTCcalB
        {
            get
            {
                lock (myLock)
                {
                    return _NTCcalB;
                }
            }
            set
            {
                lock (myLock)
                {
                    _NTCcalB = value;
                }
            }
        }
        [JsonIgnore]
        private float _currentCalA;
        [JsonProperty]
        public float currentCalA
        {
            get
            {
                lock (myLock)
                {
                    return _currentCalA;
                }
            }
            set
            {
                lock (myLock)
                {
                    _currentCalA = value;
                }
            }
        }
        [JsonIgnore]
        private float _currentCalB;
        [JsonProperty]
        public float currentCalB
        {
            get
            {
                lock (myLock)
                {
                    return _currentCalB;
                }
            }
            set
            {
                lock (myLock)
                {
                    _currentCalB = value;
                }
            }
        }
        [JsonIgnore]
        private float _intercellTemperatureCALa;
        [JsonProperty]
        public float intercellTemperatureCALa
        {
            get
            {
                lock (myLock)
                {
                    return _intercellTemperatureCALa;
                }
            }
            set
            {
                lock (myLock)
                {
                    _intercellTemperatureCALa = value;
                }
            }
        }
        [JsonIgnore]
        private float _intercellTemperatureCALb;
        [JsonProperty]
        public float intercellTemperatureCALb
        {
            get
            {
                lock (myLock)
                {
                    return _intercellTemperatureCALb;
                }
            }
            set
            {
                lock (myLock)
                {
                    _intercellTemperatureCALb = value;
                }
            }
        }
        [JsonIgnore]
        private UInt16 _chargeToIdleTimer;
        [JsonProperty]
        public UInt16 chargeToIdleTimer
        {
            get
            {
                lock (myLock)
                {
                    return _chargeToIdleTimer;
                }
            }
            set
            {
                lock (myLock)
                {
                    _chargeToIdleTimer = value;
                }
            }
        }
        [JsonIgnore]
        private UInt16 _chargeToInUseTimer;
        [JsonProperty]
        public UInt16 chargeToInUseTimer
        {
            get
            {
                lock (myLock)
                {
                    return _chargeToInUseTimer;
                }
            }
            set
            {
                lock (myLock)
                {
                    _chargeToInUseTimer = value;
                }
            }
        }
        [JsonIgnore]
        private UInt16 _inUseToChargeTimer;
        [JsonProperty]
        public UInt16 inUseToChargeTimer
        {
            get
            {
                lock (myLock)
                {
                    return _inUseToChargeTimer;
                }
            }
            set
            {
                lock (myLock)
                {
                    _inUseToChargeTimer = value;
                }
            }
        }
        [JsonIgnore]
        private UInt16 _inUsetoIdleTimer;
        [JsonProperty]
        public UInt16 inUsetoIdleTimer
        {
            get
            {
                lock (myLock)
                {
                    return _inUsetoIdleTimer;
                }
            }
            set
            {
                lock (myLock)
                {
                    _inUsetoIdleTimer = value;
                }
            }
        }
        [JsonIgnore]
        private UInt16 _idleToChargeTimer;
        [JsonProperty]
        public UInt16 idleToChargeTimer
        {
            get
            {
                lock (myLock)
                {
                    return _idleToChargeTimer;
                }
            }
            set
            {
                lock (myLock)
                {
                    _idleToChargeTimer = value;
                }
            }
        }
        [JsonIgnore]
        private UInt16 _idleToInUseTimer;
        [JsonProperty]
        public UInt16 idleToInUseTimer
        {
            get
            {
                lock (myLock)
                {
                    return _idleToInUseTimer;
                }
            }
            set
            {
                lock (myLock)
                {
                    _idleToInUseTimer = value;
                }
            }
        }
        [JsonIgnore]
        private UInt16 _electrolyteHLT;
        [JsonProperty]
        public UInt16 electrolyteHLT
        {
            get
            {
                lock (myLock)
                {
                    return _electrolyteHLT;
                }
            }
            set
            {
                lock (myLock)
                {
                    _electrolyteHLT = value;
                }
            }
        }
        [JsonIgnore]
        private UInt16 _electrolyteLHT;
        [JsonProperty]
        public UInt16 electrolyteLHT
        {
            get
            {
                lock (myLock)
                {
                    return _electrolyteLHT;
                }
            }
            set
            {
                lock (myLock)
                {
                    _electrolyteLHT = value;
                }
            }
        }
        [JsonIgnore]
        private UInt16 _actViewConnectFrequency;
        [JsonProperty]
        public UInt16 actViewConnectFrequency
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
        private UInt16 _ahrcapacity;
        [JsonProperty]
        public UInt16 ahrcapacity
        {
            get
            {
                lock (myLock)
                {
                    return _ahrcapacity;
                }
            }
            set
            {
                lock (myLock)
                {
                    _ahrcapacity = value;
                }
            }
        }
        [JsonIgnore]
        private UInt16 _batteryHighTemperature;
        [JsonProperty]
        public UInt16 batteryHighTemperature
        {
            get
            {
                lock (myLock)
                {
                    return _batteryHighTemperature;
                }
            }
            set
            {
                lock (myLock)
                {
                    _batteryHighTemperature = value;
                }
            }
        }
        [JsonIgnore]
        private UInt16 _trickleVoltage;
        [JsonProperty]
        public UInt16 trickleVoltage
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
        private UInt16 _CVTargetVoltage;
        [JsonProperty]
        public UInt16 CVTargetVoltage
        {
            get
            {
                lock (myLock)
                {
                    return _CVTargetVoltage;
                }
            }
            set
            {
                lock (myLock)
                {
                    _CVTargetVoltage = value;
                }
            }
        }
        [JsonIgnore]
        private UInt16 _trickleCurrentRate;
        [JsonProperty]
        public UInt16 trickleCurrentRate
        {
            get
            {
                lock (myLock)
                {
                    return _trickleCurrentRate;
                }
            }
            set
            {
                lock (myLock)
                {
                    _trickleCurrentRate = value;
                }
            }
        }
        [JsonIgnore]
        private UInt16 _CCrate;
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
        private UInt16 _CVendCurrentRate;
        [JsonProperty]
        public UInt16 CVendCurrentRate
        {
            get
            {
                lock (myLock)
                {
                    return _CVendCurrentRate;
                }
            }
            set
            {
                lock (myLock)
                {
                    _CVendCurrentRate = value;
                }
            }
        }
        [JsonIgnore]
        private UInt16 _CVcurrentStep;
        [JsonProperty]
        public UInt16 CVcurrentStep
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
        private UInt16 _FItargetVoltage;
        [JsonProperty]
        public UInt16 FItargetVoltage
        {
            get
            {
                lock (myLock)
                {
                    return _FItargetVoltage;
                }
            }
            set
            {
                lock (myLock)
                {
                    _FItargetVoltage = value;
                }
            }
        }
        [JsonIgnore]
        private UInt16 _FIcurrentRate;
        [JsonProperty]
        public UInt16 FIcurrentRate
        {
            get
            {
                lock (myLock)
                {
                    return _FIcurrentRate;
                }
            }
            set
            {
                lock (myLock)
                {
                    _FIcurrentRate = value;
                }
            }
        }
        [JsonIgnore]
        private UInt16 _EQvoltage;
        [JsonProperty]
        public UInt16 EQvoltage
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
        private UInt16 _EQcurrentRate;
        [JsonProperty]
        public UInt16 EQcurrentRate
        {
            get
            {
                lock (myLock)
                {
                    return _EQcurrentRate;
                }
            }
            set
            {
                lock (myLock)
                {
                    _EQcurrentRate = value;
                }
            }
        }
        [JsonIgnore]
        private UInt16 _autoLogTime;
        [JsonProperty]
        public UInt16 autoLogTime
        {
            get
            {
                lock (myLock)
                {
                    return _autoLogTime;
                }
            }
            set
            {
                lock (myLock)
                {
                    _autoLogTime = value;
                }
            }
        }
        [JsonIgnore]
        private UInt16 _mobilePort;
        [JsonProperty]
        public UInt16 mobilePort
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
        private UInt16 _actViewPort;
        [JsonProperty]
        public UInt16 actViewPort
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
        private short _currentIdleToCharge;
        [JsonProperty]
        public short currentIdleToCharge
        {
            get
            {
                lock (myLock)
                {
                    return _currentIdleToCharge;
                }
            }
            set
            {
                lock (myLock)
                {
                    _currentIdleToCharge = value;
                }
            }
        }
        [JsonIgnore]
        private short _currentIdleToInUse;
        [JsonProperty]
        public short currentIdleToInUse
        {
            get
            {
                lock (myLock)
                {
                    return _currentIdleToInUse;
                }
            }
            set
            {
                lock (myLock)
                {
                    _currentIdleToInUse = value;
                }
            }
        }
        [JsonIgnore]
        private short _currentChargeToIdle;
        [JsonProperty]
        public short currentChargeToIdle
        {
            get
            {
                lock (myLock)
                {
                    return _currentChargeToIdle;
                }
            }
            set
            {
                lock (myLock)
                {
                    _currentChargeToIdle = value;
                }
            }
        }
        [JsonIgnore]
        private short _currentChargeToInUse;
        [JsonProperty]
        public short currentChargeToInUse
        {
            get
            {
                lock (myLock)
                {
                    return _currentChargeToInUse;
                }
            }
            set
            {
                lock (myLock)
                {
                    _currentChargeToInUse = value;
                }
            }
        }
        [JsonIgnore]
        private short _currentInUseToCharge;
        [JsonProperty]
        public short currentInUseToCharge
        {
            get
            {
                lock (myLock)
                {
                    return _currentInUseToCharge;
                }
            }
            set
            {
                lock (myLock)
                {
                    _currentInUseToCharge = value;
                }
            }
        }
        [JsonIgnore]
        private short _currentInUseToIdle;
        [JsonProperty]
        public short currentInUseToIdle
        {
            get
            {
                lock (myLock)
                {
                    return _currentInUseToIdle;
                }
            }
            set
            {
                lock (myLock)
                {
                    _currentInUseToIdle = value;
                }
            }
        }
        [JsonIgnore]
        private string _battViewSN;
        [JsonProperty]
        public string battViewSN
        {
            get
            {
                lock (myLock)
                {
                    return _battViewSN;
                }
            }
            set
            {
                lock (myLock)
                {
                    _battViewSN = value;
                }
            }
        }
        [JsonIgnore]
        private string _batteryID;
        [JsonProperty]
        public string batteryID
        {
            get
            {
                lock (myLock)
                {
                    return _batteryID;
                }
            }
            set
            {
                lock (myLock)
                {
                    _batteryID = value;
                }
            }
        }
        [JsonIgnore]
        private string _softAPpassword;
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
        private string _actViewIP;
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
        private string _HWversion;
        [JsonProperty]
        public string HWversion
        {
            get
            {
                lock (myLock)
                {
                    return _HWversion;
                }
            }
            set
            {
                lock (myLock)
                {
                    _HWversion = value;
                }
            }
        }
        [JsonIgnore]
        private string _actAccessSSID;
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
        private string _mobileAccessSSID;
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
        private string _actAccessSSIDpassword;
        [JsonProperty]
        public string actAccessSSIDpassword
        {
            get
            {
                lock (myLock)
                {
                    return _actAccessSSIDpassword;
                }
            }
            set
            {
                lock (myLock)
                {
                    _actAccessSSIDpassword = value;
                }
            }
        }
        [JsonIgnore]
        private string _mobileAccessSSIDpassword;
        [JsonProperty]
        public string mobileAccessSSIDpassword
        {
            get
            {
                lock (myLock)
                {
                    return _mobileAccessSSIDpassword;
                }
            }
            set
            {
                lock (myLock)
                {
                    _mobileAccessSSIDpassword = value;
                }
            }
        }
        [JsonIgnore]
        private byte _isPA;
        [JsonProperty]
        [JsonConverter(typeof(ToBoolean))]
        public byte isPA
        {
            get
            {
                lock (myLock)
                {
                    return _isPA;
                }
            }
            set
            {
                lock (myLock)
                {
                    _isPA = value;
                }
            }
        }
        [JsonIgnore]
        private byte _actViewEnabled;
        [JsonProperty]
        [JsonConverter(typeof(ToBoolean))]
        public byte actViewEnabled
        {
            get
            {
                lock (myLock)
                {
                    return _actViewEnabled;
                }
            }
            set
            {
                lock (myLock)
                {
                    _actViewEnabled = value;
                }
            }
        }
        [JsonIgnore]
        private byte _softAPEnable;
        [JsonProperty]
        [JsonConverter(typeof(ToBoolean))]
        public byte softAPEnable
        {
            get
            {
                lock (myLock)
                {
                    return _softAPEnable;
                }
            }
            set
            {
                lock (myLock)
                {
                    _softAPEnable = value;
                }
            }
        }
        [JsonIgnore]
        private byte _enableElectrolyeSensing;
        [JsonProperty]
        [JsonConverter(typeof(ToBoolean))]
        public byte enableElectrolyeSensing
        {
            get
            {
                lock (myLock)
                {
                    return _enableElectrolyeSensing;
                }
            }
            set
            {
                lock (myLock)
                {
                    _enableElectrolyeSensing = value;
                }
            }
        }
        [JsonIgnore]
        private byte _enableHallEffectSensing;
        [JsonProperty]
        [JsonConverter(typeof(ToBoolean))]
        public byte enableHallEffectSensing
        {
            get
            {
                lock (myLock)
                {
                    return _enableHallEffectSensing;
                }
            }
            set
            {
                lock (myLock)
                {
                    _enableHallEffectSensing = value;
                }
            }
        }
        [JsonIgnore]
        private byte _enableExtTempSensing;
        [JsonProperty]
        [JsonConverter(typeof(ToBoolean))]
        public byte enableExtTempSensing
        {
            get
            {
                lock (myLock)
                {
                    return _enableExtTempSensing;
                }
            }
            set
            {
                lock (myLock)
                {
                    _enableExtTempSensing = value;
                }
            }
        }
        //toJSonDate

        [JsonIgnore]
        private byte _nominalvoltage;
        [JsonProperty]
        public byte nominalvoltage
        {
            get
            {
                lock (myLock)
                {
                    return _nominalvoltage;
                }
            }
            set
            {
                lock (myLock)
                {
                    _nominalvoltage = value;
                }
            }
        }
        [JsonIgnore]
        private byte _batteryType;
        [JsonProperty]
        public byte batteryType
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
        [JsonIgnore]
        private byte _batteryTemperatureCompesnation;
        [JsonProperty]
        public byte batteryTemperatureCompesnation
        {
            get
            {
                lock (myLock)
                {
                    return _batteryTemperatureCompesnation;
                }
            }
            set
            {
                lock (myLock)
                {
                    _batteryTemperatureCompesnation = value;
                }
            }
        }
        [JsonIgnore]
        private byte _EQdaysMask;
        [JsonProperty]
        public byte EQdaysMask
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
        private byte _FIdaysMask;
        [JsonProperty]
        public byte FIdaysMask
        {
            get
            {
                lock (myLock)
                {
                    return _FIdaysMask;
                }
            }
            set
            {
                lock (myLock)
                {
                    _FIdaysMask = value;
                }
            }
        }
        [JsonIgnore]
        private byte _FIdv;
        [JsonProperty]
        public byte FIdv
        {
            get
            {
                lock (myLock)
                {
                    return _FIdv;
                }
            }
            set
            {
                lock (myLock)
                {
                    _FIdv = value;
                }
            }
        }
        [JsonIgnore]
        private byte _FIdt;
        [JsonProperty]
        public byte FIdt
        {
            get
            {
                lock (myLock)
                {
                    return _FIdt;
                }
            }
            set
            {
                lock (myLock)
                {
                    _FIdt = value;
                }
            }
        }
        [JsonIgnore]
        private byte _eventDetectVoltagePercentage;
        [JsonProperty]
        public byte eventDetectVoltagePercentage
        {
            get
            {
                lock (myLock)
                {
                    return _eventDetectVoltagePercentage;
                }
            }
            set
            {
                lock (myLock)
                {
                    _eventDetectVoltagePercentage = value;
                }
            }
        }
        [JsonIgnore]
        private byte _eventDetectCurrentRangePercentage;
        [JsonProperty]
        public byte eventDetectCurrentRangePercentage
        {
            get
            {
                lock (myLock)
                {
                    return _eventDetectCurrentRangePercentage;
                }
            }
            set
            {
                lock (myLock)
                {
                    _eventDetectCurrentRangePercentage = value;
                }
            }
        }
        [JsonIgnore]
        private byte _eventDetectTimeRangePercentage;
        [JsonProperty]
        public byte eventDetectTimeRangePercentage
        {
            get
            {
                lock (myLock)
                {
                    return _eventDetectTimeRangePercentage;
                }
            }
            set
            {
                lock (myLock)
                {
                    _eventDetectTimeRangePercentage = value;
                }
            }
        }

        [JsonIgnore]
        private byte _enablePLC;
        [JsonProperty]
        [JsonConverter(typeof(ToBoolean))]
        public byte enablePLC
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
        byte _SaveEnablePlc;
        [JsonIgnore]
        public byte SaveEnablePlc
        {
            get
            {
                lock (myLock)
                {
                    return _SaveEnablePlc;
                }
            }
            set
            {
                lock (myLock)
                {
                    _SaveEnablePlc = value;
                }
            }
        }

        public bool EnablePlcChanged()
        {
            return enablePLC != SaveEnablePlc;
        }

        public void ResetSaveEnablePlc()
        {
            SaveEnablePlc = enablePLC;
        }

        public bool RequireRebootForPlc()
        {
            bool higherHw = string.Compare(HWversion, "BB", StringComparison.OrdinalIgnoreCase) >= 0;

            return EnablePlcChanged() && higherHw;
        }

        [JsonIgnore]
        private byte _enableDayLightSaving;
        [JsonProperty]
        [JsonConverter(typeof(ToBoolean))]
        public byte enableDayLightSaving
        {
            get
            {
                lock (myLock)
                {
                    return _enableDayLightSaving;
                }
            }
            set
            {
                lock (myLock)
                {
                    _enableDayLightSaving = value;
                }
            }
        }
        [JsonIgnore]
        private byte _temperatureFormat;
        [JsonProperty]
        public byte temperatureFormat
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
        private short _TRTemperature;
        [JsonProperty]
        public short TRTemperature
        {
            get
            {
                lock (myLock)
                {
                    return _TRTemperature;
                }
            }
            set
            {
                lock (myLock)
                {
                    _TRTemperature = value;
                }
            }
        }
        [JsonIgnore]
        private short _foldTemperature;
        [JsonProperty]
        public short foldTemperature
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
        private short _coolDownTemperature;
        [JsonProperty]
        public short coolDownTemperature
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
        private UInt32 _studyId;
        [JsonProperty]
        public UInt32 studyId
        {
            get
            {
                lock (myLock)
                {
                    return _studyId;
                }
            }
            set
            {
                lock (myLock)
                {
                    _studyId = value;
                }
            }
        }
        [JsonIgnore]
        private float _currentClampCalA;
        [JsonProperty]
        public float currentClampCalA
        {
            get
            {
                lock (myLock)
                {
                    return _currentClampCalA;
                }
            }
            set
            {
                lock (myLock)
                {
                    _currentClampCalA = value;
                }
            }
        }
        [JsonIgnore]
        private float _currentClampCalB;
        [JsonProperty]
        public float currentClampCalB
        {
            get
            {
                lock (myLock)
                {
                    return _currentClampCalB;
                }
            }
            set
            {
                lock (myLock)
                {
                    _currentClampCalB = value;
                }
            }
        }
        [JsonIgnore]
        private float _currentClamp2CalA;
        [JsonProperty]
        public float currentClamp2CalA
        {
            get
            {
                lock (myLock)
                {
                    return _currentClamp2CalA;
                }
            }
            set
            {
                lock (myLock)
                {
                    _currentClamp2CalA = value;
                }
            }
        }
        [JsonIgnore]
        private float _currentClamp2CalB;
        [JsonProperty]
        public float currentClamp2CalB
        {
            get
            {
                lock (myLock)
                {
                    return _currentClamp2CalB;
                }
            }
            set
            {
                lock (myLock)
                {
                    _currentClamp2CalB = value;
                }
            }
        }
        [JsonIgnore]
        private string _batterymodel;
        [JsonProperty]
        public string batterymodel
        {
            get
            {
                lock (myLock)
                {
                    return _batterymodel;
                }
            }
            set
            {
                lock (myLock)
                {
                    _batterymodel = value;
                }
            }
        }
        [JsonIgnore]
        private string _batterysn;
        [JsonProperty]
        public string batterysn
        {
            get
            {
                lock (myLock)
                {
                    return _batterysn;
                }
            }
            set
            {
                lock (myLock)
                {
                    _batterysn = value;
                }
            }
        }
        [JsonIgnore]
        public byte[] unused;


        [JsonIgnore]
        private DateTime _batteryManfacturingDate;
        [JsonProperty]
        public DateTime batteryManfacturingDate
        {
            get
            {
                lock (myLock)
                {
                    return new DateTime(_batteryManfacturingDate.Ticks);
                }
            }
            set
            {
                lock (myLock)
                {
                    _batteryManfacturingDate = value;
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
        private byte _temperatureControl;
        [JsonProperty]
        public byte temperatureControl
        {
            get
            {
                lock (myLock)
                {
                    return _temperatureControl;
                }
            }
            set
            {
                lock (myLock)
                {
                    _temperatureControl = value;
                }
            }
        }
        [JsonIgnore]
        private byte _blockedEQDays;
        [JsonProperty]
        public byte blockedEQDays
        {
            get
            {
                lock (myLock)
                {
                    return _blockedEQDays;
                }
            }
            set
            {
                lock (myLock)
                {
                    _blockedEQDays = value;
                }
            }
        }

        [JsonIgnore]
        private byte _blockedFIDays;
        [JsonProperty]
        public byte blockedFIDays
        {
            get
            {
                lock (myLock)
                {
                    return _blockedFIDays;
                }
            }
            set
            {
                lock (myLock)
                {
                    _blockedFIDays = value;
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

        [JsonIgnore]
        private float _HallEffectScale;
        [JsonProperty]
        public float HallEffectScale
        {
            get
            {
                lock (myLock)
                {
                    return _HallEffectScale;
                }
            }
            set
            {
                lock (myLock)
                {
                    _HallEffectScale = value;
                }
            }
        }
        private bool _disableIntercell;
        [JsonProperty]
        public bool disableIntercell
        {
            get
            {
                lock (myLock)
                {
                    return _disableIntercell;
                }
            }
            set
            {
                lock (myLock)
                {
                    _disableIntercell = value;
                }
            }
        }
        //disableIntercell
        [JsonIgnore]
        public UInt16 crc;
        #endregion
        public byte[] formatAll(bool forSynched = false)
        {
            UInt32 user_id = ControlObject.userID;
            int loc;
            byte[] outArray = new byte[512];
            for (int i = 0; i < outArray.Length; i++)
                outArray[i] = 0;
            //
            if (forSynched)
            {
                loc = 0;
                //setup_version
                outArray[loc++] = setup;
                outArray[loc++] = battviewVersion;
                Array.Copy(BitConverter.GetBytes(memorySignature), 0, outArray, loc, 2);
                loc += 2;
                //lastChangeTime
                UInt32 unixTimestamp_lastChangeTime = (UInt32)(lastChangeTime.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc))).TotalSeconds;
                Array.Copy(BitConverter.GetBytes(unixTimestamp_lastChangeTime), 0, outArray, loc, 4);
                loc += 4;
                //internalBattViewID
                Array.Copy(BitConverter.GetBytes(id), 0, outArray, loc, 4);

                loc += 4;
            }
            else
            {

                loc = 12;
            }
            //lastChangeUserID
            Array.Copy(BitConverter.GetBytes(user_id), 0, outArray, loc, 4);
            loc += 4;
            //installationTime
            UInt32 unixTimestamp = (UInt32)(installationDate.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc))).TotalSeconds;
            Array.Copy(BitConverter.GetBytes(unixTimestamp), 0, outArray, loc, 4);
            loc += 4;
            //WarantedAHR
            Array.Copy(BitConverter.GetBytes(warrantedAHR), 0, outArray, loc, 4);
            loc += 4;
            //cvMaxDuration
            Array.Copy(BitConverter.GetBytes(cvMaxDuration), 0, outArray, loc, 4);
            loc += 4;
            //EQstartWindow
            Array.Copy(BitConverter.GetBytes(EQstartWindow), 0, outArray, loc, 4);
            loc += 4;
            //EQcloseWindow
            Array.Copy(BitConverter.GetBytes(EQcloseWindow), 0, outArray, loc, 4);
            loc += 4;
            //FIstartWindow
            Array.Copy(BitConverter.GetBytes(FIstartWindow), 0, outArray, loc, 4);
            loc += 4;
            //FIcloseWindow
            Array.Copy(BitConverter.GetBytes(FIcloseWindow), 0, outArray, loc, 4);
            loc += 4;
            //FIDuration
            Array.Copy(BitConverter.GetBytes(FIduration), 0, outArray, loc, 4);
            loc += 4;
            //EQDuration
            Array.Copy(BitConverter.GetBytes(EQduration), 0, outArray, loc, 4);
            loc += 4;
            //desulfationDuration
            Array.Copy(BitConverter.GetBytes(desulfation), 0, outArray, loc, 4);
            loc += 4;

            Array.Copy(BitConverter.GetBytes(tempFa), 0, outArray, loc, 4);
            loc += 4;

            Array.Copy(BitConverter.GetBytes(tempFb), 0, outArray, loc, 4);
            loc += 4;

            Array.Copy(BitConverter.GetBytes(tempFc), 0, outArray, loc, 4);
            loc += 4;

            Array.Copy(BitConverter.GetBytes(intercellCoefficient), 0, outArray, loc, 4);
            loc += 4;

            Array.Copy(BitConverter.GetBytes(voltageCalA), 0, outArray, loc, 4);
            loc += 4;

            Array.Copy(BitConverter.GetBytes(voltageCalB), 0, outArray, loc, 4);
            loc += 4;

            Array.Copy(BitConverter.GetBytes(NTCcalA), 0, outArray, loc, 4);/*USED FOR NEW HW SHUNT CALA H*/
            loc += 4;

            Array.Copy(BitConverter.GetBytes(NTCcalB), 0, outArray, loc, 4);/*USED FOR NEW HW SHUNT CAL BH */
            loc += 4;

            Array.Copy(BitConverter.GetBytes(currentCalA), 0, outArray, loc, 4);
            loc += 4;

            Array.Copy(BitConverter.GetBytes(currentCalB), 0, outArray, loc, 4);
            loc += 4;

            Array.Copy(BitConverter.GetBytes(intercellTemperatureCALa), 0, outArray, loc, 4);/*NOT USED */
            loc += 4;

            Array.Copy(BitConverter.GetBytes(intercellTemperatureCALb), 0, outArray, loc, 4);/*NOT USED */
            loc += 4;

            Array.Copy(BitConverter.GetBytes(chargeToIdleTimer), 0, outArray, loc, 2);
            loc += 2;

            Array.Copy(BitConverter.GetBytes(chargeToInUseTimer), 0, outArray, loc, 2);
            loc += 2;

            Array.Copy(BitConverter.GetBytes(inUseToChargeTimer), 0, outArray, loc, 2);
            loc += 2;

            Array.Copy(BitConverter.GetBytes(inUsetoIdleTimer), 0, outArray, loc, 2);
            loc += 2;

            Array.Copy(BitConverter.GetBytes(idleToChargeTimer), 0, outArray, loc, 2);
            loc += 2;

            Array.Copy(BitConverter.GetBytes(idleToInUseTimer), 0, outArray, loc, 2);
            loc += 2;

            Array.Copy(BitConverter.GetBytes(electrolyteHLT), 0, outArray, loc, 2);
            loc += 2;

            Array.Copy(BitConverter.GetBytes(electrolyteLHT), 0, outArray, loc, 2);
            loc += 2;

            Array.Copy(BitConverter.GetBytes(actViewConnectFrequency), 0, outArray, loc, 2);
            loc += 2;

            Array.Copy(BitConverter.GetBytes(ahrcapacity), 0, outArray, loc, 2);
            loc += 2;

            Array.Copy(BitConverter.GetBytes(batteryHighTemperature), 0, outArray, loc, 2);
            loc += 2;

            Array.Copy(BitConverter.GetBytes(trickleVoltage), 0, outArray, loc, 2);
            loc += 2;

            Array.Copy(BitConverter.GetBytes(CVTargetVoltage), 0, outArray, loc, 2);
            loc += 2;

            Array.Copy(BitConverter.GetBytes(trickleCurrentRate), 0, outArray, loc, 2);
            loc += 2;

            Array.Copy(BitConverter.GetBytes(CCrate), 0, outArray, loc, 2);
            loc += 2;

            Array.Copy(BitConverter.GetBytes(CVendCurrentRate), 0, outArray, loc, 2);
            loc += 2;

            Array.Copy(BitConverter.GetBytes(CVcurrentStep), 0, outArray, loc, 2);
            loc += 2;

            Array.Copy(BitConverter.GetBytes(FItargetVoltage), 0, outArray, loc, 2);
            loc += 2;

            Array.Copy(BitConverter.GetBytes(FIcurrentRate), 0, outArray, loc, 2);
            loc += 2;

            Array.Copy(BitConverter.GetBytes(EQvoltage), 0, outArray, loc, 2);
            loc += 2;

            Array.Copy(BitConverter.GetBytes(EQcurrentRate), 0, outArray, loc, 2);
            loc += 2;

            Array.Copy(BitConverter.GetBytes(autoLogTime), 0, outArray, loc, 2);
            loc += 2;

            Array.Copy(BitConverter.GetBytes(mobilePort), 0, outArray, loc, 2);
            loc += 2;

            Array.Copy(BitConverter.GetBytes(actViewPort), 0, outArray, loc, 2);
            loc += 2;

            Array.Copy(BitConverter.GetBytes(currentIdleToCharge), 0, outArray, loc, 2);
            loc += 2;

            Array.Copy(BitConverter.GetBytes(currentIdleToInUse), 0, outArray, loc, 2);
            loc += 2;

            Array.Copy(BitConverter.GetBytes(currentChargeToIdle), 0, outArray, loc, 2);
            loc += 2;

            Array.Copy(BitConverter.GetBytes(currentChargeToInUse), 0, outArray, loc, 2);
            loc += 2;

            Array.Copy(BitConverter.GetBytes(currentInUseToCharge), 0, outArray, loc, 2);
            loc += 2;

            Array.Copy(BitConverter.GetBytes(currentInUseToIdle), 0, outArray, loc, 2);
            loc += 2;

            string tempString = battViewSN;
            if (!forSynched)
                tempString = tempString.Substring(1);
            Array.Copy(Encoding.UTF8.GetBytes(tempString), 0, outArray, loc, tempString.Length);
            if (!forSynched)
            {
                outArray[loc + tempString.Length] = 0;
                outArray[loc + 11] = 0;
            }
            loc += 12;

            Array.Copy(Encoding.UTF8.GetBytes(batteryID), 0, outArray, loc, batteryID.Length);
            outArray[loc + batteryID.Length] = 0;
            outArray[loc + 17] = 0;
            loc += 18;

            Array.Copy(Encoding.UTF8.GetBytes(softAPpassword), 0, outArray, loc, softAPpassword.Length);
            outArray[loc + softAPpassword.Length] = 0;
            outArray[loc + 13] = 0;
            loc += 14;


            if (this.battviewVersion == 1)
            {
                Array.Copy(Encoding.UTF8.GetBytes(actViewIP), 0, outArray, loc, actViewIP.Length);
                outArray[loc + actViewIP.Length] = 0;
                outArray[loc + 63] = 0;
                loc += 64;
            }
            else
            {
                Array.Copy(Encoding.UTF8.GetBytes(actViewIP), 0, outArray, loc, actViewIP.Length);
                outArray[loc + actViewIP.Length] = 0;
                outArray[loc + 31] = 0;
                loc += 32;

                //actAccessSSIDpassword
                Array.Copy(Encoding.UTF8.GetBytes(actAccessSSIDpassword), 0, outArray, loc, actAccessSSIDpassword.Length);
                outArray[loc + actAccessSSIDpassword.Length] = 0;
                outArray[loc + 31] = 0;
                loc += 32;
            }



            if (HWversion.Length == 0)
                HWversion = "A ";
            else if (HWversion.Length == 1)
            {
                HWversion += " ";
            }
            outArray[loc++] = (byte)(HWversion[0]);
            outArray[loc++] = (byte)(HWversion[1]);

            Array.Copy(Encoding.UTF8.GetBytes(actAccessSSID), 0, outArray, loc, actAccessSSID.Length);
            outArray[loc + actAccessSSID.Length] = 0;
            outArray[loc + 31] = 0;
            loc += 32;

            Array.Copy(Encoding.UTF8.GetBytes(mobileAccessSSID), 0, outArray, loc, mobileAccessSSID.Length);
            outArray[loc + mobileAccessSSID.Length] = 0;
            outArray[loc + 31] = 0;
            loc += 32;
            if (this.battviewVersion == 1)
            {
                Array.Copy(Encoding.UTF8.GetBytes(actAccessSSIDpassword), 0, outArray, loc, actAccessSSIDpassword.Length);
                outArray[loc + actAccessSSIDpassword.Length] = 0;
                outArray[loc + 13] = 0;
                loc += 14;
            }
            Array.Copy(Encoding.UTF8.GetBytes(mobileAccessSSIDpassword), 0, outArray, loc, mobileAccessSSIDpassword.Length);
            outArray[loc + mobileAccessSSIDpassword.Length] = 0;
            outArray[loc + 13] = 0;
            loc += 14;

            outArray[loc] = isPA;
            loc += 1;

            outArray[loc] = actViewEnabled;
            loc += 1;

            outArray[loc] = softAPEnable;
            loc += 1;

            outArray[loc] = enableElectrolyeSensing;
            loc += 1;

            outArray[loc] = enableHallEffectSensing;
            loc += 1;


            outArray[loc] = enableExtTempSensing;
            loc += 1;

            outArray[loc] = nominalvoltage;
            loc += 1;

            outArray[loc] = batteryType;
            loc += 1;

            outArray[loc] = batteryTemperatureCompesnation;
            loc += 1;

            outArray[loc] = EQdaysMask;
            loc += 1;

            outArray[loc] = FIdaysMask;
            loc += 1;

            outArray[loc] = FIdv;
            loc += 1;

            outArray[loc] = FIdt;
            loc += 1;

            outArray[loc] = eventDetectVoltagePercentage;
            loc += 1;

            outArray[loc] = eventDetectCurrentRangePercentage;
            loc += 1;

            outArray[loc] = eventDetectTimeRangePercentage;
            loc += 1;

            outArray[loc] = enablePLC;
            loc += 1;
            outArray[loc] = enableDayLightSaving;
            loc += 1;
            outArray[loc] = temperatureFormat;
            loc += 1;
            outArray[loc] = chargerType;
            loc += 1;

            Array.Copy(BitConverter.GetBytes(TRTemperature), 0, outArray, loc, 2);
            loc += 2;
            Array.Copy(BitConverter.GetBytes(foldTemperature), 0, outArray, loc, 2);
            loc += 2;

            Array.Copy(BitConverter.GetBytes(coolDownTemperature), 0, outArray, loc, 2);
            loc += 2;

            Array.Copy(BitConverter.GetBytes(studyId), 0, outArray, loc, 4);
            loc += 4;
            if (this.isPA == 0)
                studyId = 0;
            Array.Copy(BitConverter.GetBytes(currentClampCalA), 0, outArray, loc, 4);
            loc += 4;

            Array.Copy(BitConverter.GetBytes(currentClampCalB), 0, outArray, loc, 4);
            loc += 4;

            Array.Copy(BitConverter.GetBytes(currentClamp2CalA), 0, outArray, loc, 4);
            loc += 4;

            Array.Copy(BitConverter.GetBytes(currentClamp2CalB), 0, outArray, loc, 4);
            loc += 4;
            Array.Copy(Encoding.UTF8.GetBytes(batterymodel), 0, outArray, loc, batterymodel.Length);
            for (int x = loc + batterymodel.Length; x < loc + 18; x++)
            {
                outArray[x] = 0;
            }
            loc += 18;
            Array.Copy(Encoding.UTF8.GetBytes(batterysn), 0, outArray, loc, batterysn.Length);
            for (int x = loc + batterysn.Length; x < loc + 18; x++)
            {
                outArray[x] = 0;
            }
            loc += 18;




            unixTimestamp = (UInt32)(batteryManfacturingDate.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc))).TotalSeconds;
            Array.Copy(BitConverter.GetBytes(unixTimestamp), 0, outArray, loc, 4);
            loc += 4;
            outArray[loc++] = (byte)(replacmentPart ? 0x01 : 0x00);

            Array.Copy(Encoding.UTF8.GetBytes(TruckId), 0, outArray, loc, TruckId.Length);
            for (int x = loc + TruckId.Length; x < loc + 18; x++)
            {
                outArray[x] = 0;
            }
            outArray[loc + TruckId.Length] = 0;
            loc += 18;
            if (this.isPA == 0)
                studyName = "";
            Array.Copy(Encoding.UTF8.GetBytes(studyName), 0, outArray, loc, studyName.Length);
            for (int x = loc + studyName.Length; x < loc + 18; x++)
            {
                outArray[x] = 0;
            }
            outArray[loc + studyName.Length] = 0;
            loc += 18;

            outArray[loc++] = temperatureControl;
            outArray[loc++] = blockedEQDays;
            outArray[loc++] = blockedFIDays;
            outArray[loc++] = FIStopCurrent;
            outArray[loc++] = (byte)(useNewEastPennProfile ? 0x01 : 0x00);

            Array.Copy(BitConverter.GetBytes(HallEffectScale), 0, outArray, loc, 4);
            loc += 4;
            outArray[loc++] = (byte)(disableIntercell ? 0x01 : 0x00);
            if (forSynched || 512 - loc != unused.Length)
            {
                for (; loc < outArray.Length; loc++)
                    outArray[loc] = 0;


            }
            else
            {
                Array.Copy(unused, 0, outArray, loc, unused.Length);

            }

            return outArray;
        }


        public BattViewConfig()
        {
            myLock = new object();
            commissionRequest = false;
            unused = new byte[0];
        }
        public CommunicationResult LoadFromArray(byte[] result, float firmwareversion)
        {
            if (battviewVersion > maxSupportedVersion)
                return CommunicationResult.ReadSomethingElse;
            this.firmwareversion = firmwareversion;
            int loc = 0;
            //setup_version
            setup = result[loc++];
            battviewVersion = result[loc++];
            //memorySignature
            memorySignature = BitConverter.ToUInt16(result, loc);
            loc += 2;
            //lastChangeTime
            lastChangeTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(BitConverter.ToUInt32(result, loc));
            loc += 4;
            //internalBattViewID
            id = BitConverter.ToUInt32(result, loc);
            loc += 4;
            //lastChangeUserID
            lastChangeUserID = BitConverter.ToUInt32(result, loc);
            loc += 4;
            //installationTime
            installationDate = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(BitConverter.ToUInt32(result, loc));
            loc += 4;
            //WarantedAHR
            warrantedAHR = BitConverter.ToUInt32(result, loc);
            loc += 4;
            //cvMaxDuration
            cvMaxDuration = BitConverter.ToUInt32(result, loc);
            loc += 4;
            //EQstartWindow
            EQstartWindow = BitConverter.ToUInt32(result, loc);
            loc += 4;
            //EQcloseWindow
            EQcloseWindow = BitConverter.ToUInt32(result, loc);
            loc += 4;
            //FIstartWindow
            FIstartWindow = BitConverter.ToUInt32(result, loc);
            loc += 4;
            //FIcloseWindow
            FIcloseWindow = BitConverter.ToUInt32(result, loc);
            loc += 4;
            //FIDuration
            FIduration = BitConverter.ToUInt32(result, loc);
            loc += 4;
            //EQDuration
            EQduration = BitConverter.ToUInt32(result, loc);
            loc += 4;
            //desulfationDuration
            desulfation = BitConverter.ToUInt32(result, loc);
            loc += 4;
            //Temp_fa
            tempFa = BitConverter.ToSingle(result, loc);
            loc += 4;
            //Temp_fb
            tempFb = BitConverter.ToSingle(result, loc);
            loc += 4;
            //Temp_fc
            tempFc = BitConverter.ToSingle(result, loc);
            loc += 4;
            //intercell_coeffecient
            intercellCoefficient = BitConverter.ToSingle(result, loc);
            loc += 4;
            //voltage_calA
            voltageCalA = BitConverter.ToSingle(result, loc);
            loc += 4;
            //voltage_calB
            voltageCalB = BitConverter.ToSingle(result, loc);
            loc += 4;
            //ntc_calA
            NTCcalA = BitConverter.ToSingle(result, loc);
            loc += 4;
            //ntc_calB
            NTCcalB = BitConverter.ToSingle(result, loc);
            loc += 4;
            //currentClamp_calA
            currentCalA = BitConverter.ToSingle(result, loc);
            loc += 4;
            //currentClamp_calB
            currentCalB = BitConverter.ToSingle(result, loc);
            loc += 4;
            //intercell_calA
            intercellTemperatureCALa = BitConverter.ToSingle(result, loc);
            loc += 4;
            //intercell_calB
            intercellTemperatureCALb = BitConverter.ToSingle(result, loc);
            loc += 4;
            ////SG_calA
            //SG_calA = BitConverter.ToSingle(result, loc);
            //loc += 4;
            ////SG_calB
            //SG_calB = BitConverter.ToSingle(result, loc);
            //loc += 4;


            //chargeToIdleTimer
            chargeToIdleTimer = BitConverter.ToUInt16(result, loc);
            loc += 2;
            //chargeToInUseTimer
            chargeToInUseTimer = BitConverter.ToUInt16(result, loc);
            loc += 2;
            //inUseToChargeTimer
            inUseToChargeTimer = BitConverter.ToUInt16(result, loc);
            loc += 2;
            //inUsetoIdleTimer
            inUsetoIdleTimer = BitConverter.ToUInt16(result, loc);
            loc += 2;
            //idleTochargeTimer
            idleToChargeTimer = BitConverter.ToUInt16(result, loc);
            loc += 2;
            //idleToInUseTimer
            idleToInUseTimer = BitConverter.ToUInt16(result, loc);
            loc += 2;

            //electrolyte_HLT
            electrolyteHLT = BitConverter.ToUInt16(result, loc);
            loc += 2;
            //electrolyte_LHT
            electrolyteLHT = BitConverter.ToUInt16(result, loc);
            loc += 2;
            //actViewConnectFrequency
            actViewConnectFrequency = BitConverter.ToUInt16(result, loc);
            loc += 2;

            //batteryCapacity
            ahrcapacity = BitConverter.ToUInt16(result, loc);
            loc += 2;
            //batteryHighTemperature
            batteryHighTemperature = BitConverter.ToUInt16(result, loc);
            loc += 2;
            //trickleVoltage
            trickleVoltage = BitConverter.ToUInt16(result, loc);
            loc += 2;
            //CVTargetVoltage
            CVTargetVoltage = BitConverter.ToUInt16(result, loc);
            loc += 2;
            //trickleCurrentRate
            trickleCurrentRate = BitConverter.ToUInt16(result, loc);
            loc += 2;
            //CCRate
            CCrate = BitConverter.ToUInt16(result, loc);
            loc += 2;
            //CVEndCurrentRate
            CVendCurrentRate = BitConverter.ToUInt16(result, loc);
            loc += 2;
            //CVCurrentStep
            CVcurrentStep = BitConverter.ToUInt16(result, loc);
            loc += 2;
            //FItargetVoltage
            FItargetVoltage = BitConverter.ToUInt16(result, loc);
            loc += 2;
            //FICurrentRate
            FIcurrentRate = BitConverter.ToUInt16(result, loc);
            loc += 2;
            //EQvoltage
            EQvoltage = BitConverter.ToUInt16(result, loc);
            loc += 2;
            //EQcurrentRate
            EQcurrentRate = BitConverter.ToUInt16(result, loc);
            loc += 2;
            //autoLogTime
            autoLogTime = BitConverter.ToUInt16(result, loc);
            loc += 2;
            //mobilePort
            mobilePort = BitConverter.ToUInt16(result, loc);
            loc += 2;
            //actViewPort
            actViewPort = BitConverter.ToUInt16(result, loc);
            loc += 2;
            //currentIdleToCharge
            currentIdleToCharge = BitConverter.ToInt16(result, loc);
            loc += 2;
            //currentIdleToInUse
            currentIdleToInUse = BitConverter.ToInt16(result, loc);
            loc += 2;
            //currentChargeToIdle
            currentChargeToIdle = BitConverter.ToInt16(result, loc);
            loc += 2;
            //currentChargeToInUse
            currentChargeToInUse = BitConverter.ToInt16(result, loc);
            loc += 2;
            //currentInUseToCharge
            currentInUseToCharge = BitConverter.ToInt16(result, loc);
            loc += 2;
            //currentInUseToIdle
            currentInUseToIdle = BitConverter.ToInt16(result, loc);
            loc += 2;
            //battViewSN
            string tempString;
            tempString = Encoding.UTF8.GetString(result, loc, 12).TrimEnd('\0');
            battViewSN = "3" + tempString;
            loc += 12;
            //batteryID
            batteryID = Encoding.UTF8.GetString(result, loc, 18).TrimEnd('\0');
            loc += 18;
            //softAPpassword
            softAPpassword = Encoding.UTF8.GetString(result, loc, 14).TrimEnd('\0');
            loc += 14;
            //actViewIP
            if (battviewVersion == 1)
            {
                actViewIP = Encoding.UTF8.GetString(result, loc, 64).TrimEnd('\0');
                loc += 64;
            }
            else
            {
                actViewIP = Encoding.UTF8.GetString(result, loc, 32).TrimEnd('\0');
                loc += 32;
                actAccessSSIDpassword = Encoding.UTF8.GetString(result, loc, 32).TrimEnd('\0');
                loc += 32;
            }
            //HWVersion
            HWversion = ((char)result[loc++]).ToString();
            HWversion += ((char)result[loc++]).ToString();

            //actAccessSSID
            actAccessSSID = Encoding.UTF8.GetString(result, loc, 32).TrimEnd('\0');
            loc += 32;
            //mobileAccessSSID
            mobileAccessSSID = Encoding.UTF8.GetString(result, loc, 32).TrimEnd('\0');
            loc += 32;


            //actAccessSSIDpassword
            if (battviewVersion == 1)
            {
                actAccessSSIDpassword = Encoding.UTF8.GetString(result, loc, 14).TrimEnd('\0');
                loc += 14;
            }
            //mobileAccessSSIDpassword
            mobileAccessSSIDpassword = Encoding.UTF8.GetString(result, loc, 14).TrimEnd('\0');
            loc += 14;
            //isPA
            isPA = result[loc++];
            //actViewEnabled
            actViewEnabled = result[loc++];
            //softAPEnable
            softAPEnable = result[loc++];
            //enableElectrolyeSensing
            enableElectrolyeSensing = result[loc++];
            //enableHallEffectSensing
            enableHallEffectSensing = result[loc++];
            //enableSGSensing
            //enableSGSensing = result[loc++];
            //enableExtTempSensing
            enableExtTempSensing = result[loc++];
            //batteryNominalVoltage
            nominalvoltage = result[loc++];
            //batteryType
            batteryType = result[loc++];
            //batteryTmperatureCompesnation
            batteryTemperatureCompesnation = result[loc++];
            //EQdaysMask
            EQdaysMask = result[loc++];
            //FIdaysMask
            FIdaysMask = result[loc++];
            //FIdv
            FIdv = result[loc++];
            //FIdt
            FIdt = result[loc++];
            //eventDetectVoltagePercentage
            eventDetectVoltagePercentage = result[loc++];
            //eventDetectCurrentRangePercentage
            eventDetectCurrentRangePercentage = result[loc++];
            //eventDetectTimeRangePercentage
            eventDetectTimeRangePercentage = result[loc++];

            enablePLC = result[loc++];
            SaveEnablePlc = enablePLC;

            enableDayLightSaving = result[loc++];
            temperatureFormat = result[loc++];
            chargerType = result[loc++];


            TRTemperature = BitConverter.ToInt16(result, loc);
            loc += 2;
            foldTemperature = BitConverter.ToInt16(result, loc);
            loc += 2;
            coolDownTemperature = BitConverter.ToInt16(result, loc);
            loc += 2;
            if (isPA != 0)
                studyId = BitConverter.ToUInt32(result, loc);
            else
                studyId = 0;
            loc += 4;

            currentClampCalA = BitConverter.ToSingle(result, loc);
            loc += 4;
            currentClampCalB = BitConverter.ToSingle(result, loc);
            loc += 4;
            currentClamp2CalA = BitConverter.ToSingle(result, loc);
            loc += 4;
            currentClamp2CalB = BitConverter.ToSingle(result, loc);
            loc += 4;
            batterymodel = Encoding.UTF8.GetString(result, loc, 18).TrimEnd('\0');
            loc += 18;
            batterysn = Encoding.UTF8.GetString(result, loc, 18).TrimEnd('\0');
            loc += 18;




            batteryManfacturingDate = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(BitConverter.ToUInt32(result, loc));
            loc += 4;
            replacmentPart = result[loc++] != 0;
            if (id < 10000)
                replacmentPart = false;

            TruckId = Encoding.UTF8.GetString(result, loc, 18).TrimEnd('\0');
            loc += 18;

            studyName = Encoding.UTF8.GetString(result, loc, 18).TrimEnd('\0');
            loc += 18;
            if (this.isPA == 0)
                studyName = "";

            temperatureControl = result[loc++];
            blockedEQDays = result[loc++];
            blockedFIDays = result[loc++];
            FIStopCurrent = result[loc++];
            useNewEastPennProfile = result[loc++] > 0;
            HallEffectScale = BitConverter.ToSingle(result, loc);
            loc += 4;
            disableIntercell = result[loc++] != 0;

            if (battviewVersion == 1)
            {
                unused = new byte[configV1UnusedLength];
            }
            else
            {
                unused = new byte[configV2UnusedLength];

            }
            Array.Copy(result, loc, unused, 0, unused.Length);

            return CommunicationResult.OK;
            //no need to CRC
        }

        public string ToJson()
        {
            CorrectValues();

            return JsonConvert.SerializeObject(this);
        }

        void CorrectValues()
        {
            HWversion = HWversion.Trim();
        }
    }
}
