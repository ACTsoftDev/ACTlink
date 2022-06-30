using System;
namespace actchargers
{
    public class BattviewSettings
    {
        #region settings

        private byte _zoneid;
        private bool zoneid_isDirty;
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
                    zoneid_isDirty = true;
                }
            }
        }

        private DateTime _installationDate;
        private bool installationDate_isDirty;
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
                    installationDate_isDirty = true;
                }
            }
        }

        private UInt32 _warrantedAHR;
        private bool warrantedAHR_isDirty;
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
                    warrantedAHR_isDirty = true;
                }
            }
        }

        private UInt32 _cvMaxDuration;
        private bool cvMaxDuration_isDirty;
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
                    cvMaxDuration_isDirty = true;
                }
            }
        }

        private UInt32 _EQstartWindow;
        private bool EQstartWindow_isDirty;
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
                    EQstartWindow_isDirty = true;
                }
            }
        }

        private UInt32 _EQcloseWindow;
        private bool EQcloseWindow_isDirty;
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
                    EQcloseWindow_isDirty = true;
                }
            }
        }

        private UInt32 _FIstartWindow;
        private bool FIstartWindow_isDirty;
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
                    FIstartWindow_isDirty = true;
                }
            }
        }

        private UInt32 _FIcloseWindow;
        private bool FIcloseWindow_isDirty;
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
                    FIcloseWindow_isDirty = true;
                }
            }
        }

        private UInt32 _FIduration;
        private bool FIduration_isDirty;
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
                    FIduration_isDirty = true;
                }
            }
        }

        private UInt32 _EQduration;
        private bool EQduration_isDirty;
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
                    EQduration_isDirty = true;
                }
            }
        }

        private UInt32 _desulfation;
        private bool desulfation_isDirty;
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
                    desulfation_isDirty = true;
                }
            }
        }

        private UInt16 _actViewConnectFrequency;
        private bool actViewConnectFrequency_isDirty;
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
                    actViewConnectFrequency_isDirty = true;
                }
            }
        }

        private UInt16 _ahrcapacity;
        private bool ahrcapacity_isDirty;
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
                    ahrcapacity_isDirty = true;
                }
            }
        }

        private UInt16 _batteryHighTemperature;
        private bool batteryHighTemperature_isDirty;
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
                    batteryHighTemperature_isDirty = true;
                }
            }
        }

        private UInt16 _trickleVoltage;
        private bool trickleVoltage_isDirty;
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
                    trickleVoltage_isDirty = true;
                }
            }
        }

        private UInt16 _CVTargetVoltage;
        private bool CVTargetVoltage_isDirty;
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
                    CVTargetVoltage_isDirty = true;
                }
            }
        }

        private UInt16 _trickleCurrentRate;
        private bool trickleCurrentRate_isDirty;
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
                    trickleCurrentRate_isDirty = true;
                }
            }
        }

        private UInt16 _CCrate;
        private bool CCrate_isDirty;
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
                    CCrate_isDirty = true;
                }
            }
        }

        private UInt16 _CVendCurrentRate;
        private bool CVendCurrentRate_isDirty;
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
                    CVendCurrentRate_isDirty = true;
                }
            }
        }

        private UInt16 _CVcurrentStep;
        private bool CVcurrentStep_isDirty;
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
                    CVcurrentStep_isDirty = true;
                }
            }
        }

        private UInt16 _FItargetVoltage;
        private bool FItargetVoltage_isDirty;
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
                    FItargetVoltage_isDirty = true;
                }
            }
        }

        private UInt16 _FIcurrentRate;
        private bool FIcurrentRate_isDirty;
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
                    FIcurrentRate_isDirty = true;
                }
            }
        }

        private UInt16 _EQvoltage;
        private bool EQvoltage_isDirty;
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
                    EQvoltage_isDirty = true;
                }
            }
        }

        private UInt16 _EQcurrentRate;
        private bool EQcurrentRate_isDirty;
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
                    EQcurrentRate_isDirty = true;
                }
            }
        }

        private UInt16 _autoLogTime;
        private bool autoLogTime_isDirty;
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
                    autoLogTime_isDirty = true;
                }
            }
        }

        private UInt16 _mobilePort;
        private bool mobilePort_isDirty;
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
                    mobilePort_isDirty = true;
                }
            }
        }

        private UInt16 _actViewPort;
        private bool actViewPort_isDirty;
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
                    actViewPort_isDirty = true;
                }
            }
        }

        private string _actViewIP;
        private bool actViewIP_isDirty;
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
                    actViewIP_isDirty = true;
                }
            }
        }

        private string _actAccessSSID;
        private bool actAccessSSID_isDirty;
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
                    actAccessSSID_isDirty = true;
                }
            }
        }

        private string _mobileAccessSSID;
        private bool mobileAccessSSID_isDirty;
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
                    mobileAccessSSID_isDirty = true;
                }
            }
        }

        private string _actAccessSSIDpassword;
        private bool actAccessSSIDpassword_isDirty;
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
                    actAccessSSIDpassword_isDirty = true;
                }
            }
        }

        private string _mobileAccessSSIDpassword;
        private bool mobileAccessSSIDpassword_isDirty;
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
                    mobileAccessSSIDpassword_isDirty = true;
                }
            }
        }

        private bool _actViewEnabled;
        private bool actViewEnabled_isDirty;
        public bool actViewEnabled
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
                    actViewEnabled_isDirty = true;
                }
            }
        }

        private byte _enableElectrolyeSensing;
        private bool enableElectrolyeSensing_isDirty;
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
                    enableElectrolyeSensing_isDirty = true;
                }
            }
        }

        private byte _enableHallEffectSensing;
        private bool enableHallEffectSensing_isDirty;
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
                    enableHallEffectSensing_isDirty = true;
                }
            }
        }

        private byte _enableExtTempSensing;
        private bool enableExtTempSensing_isDirty;
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
                    enableExtTempSensing_isDirty = true;
                }
            }
        }

        private byte _nominalvoltage;
        private bool nominalvoltage_isDirty;
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
                    nominalvoltage_isDirty = true;
                }
            }
        }

        private byte _batteryType;
        private bool batteryType_isDirty;
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
                    batteryType_isDirty = true;
                }
            }
        }

        private byte _batteryTemperatureCompesnation;
        private bool batteryTemperatureCompesnation_isDirty;
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
                    batteryTemperatureCompesnation_isDirty = true;
                }
            }
        }

        private byte _EQdaysMask;
        private bool EQdaysMask_isDirty;
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
                    EQdaysMask_isDirty = true;
                }
            }
        }

        private byte _FIdaysMask;
        private bool FIdaysMask_isDirty;
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
                    FIdaysMask_isDirty = true;
                }
            }
        }

        private byte _FIdv;
        private bool FIdv_isDirty;
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
                    FIdv_isDirty = true;
                }
            }
        }

        private byte _FIdt;
        private bool FIdt_isDirty;
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
                    FIdt_isDirty = true;
                }
            }
        }

        private byte _enablePLC;
        private bool enablePLC_isDirty;
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
                    enablePLC_isDirty = true;
                }
            }
        }

        private byte _enableDayLightSaving;
        private bool enableDayLightSaving_isDirty;
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
                    enableDayLightSaving_isDirty = true;
                }
            }
        }

        private byte _temperatureFormat;
        private bool temperatureFormat_isDirty;
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
                    temperatureFormat_isDirty = true;
                }
            }
        }

        private byte _chargerType;
        private bool chargerType_isDirty;
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
                    chargerType_isDirty = true;
                }
            }
        }

        private short _TRTemperature;
        private bool TRTemperature_isDirty;
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
                    TRTemperature_isDirty = true;
                }
            }
        }

        private short _foldTemperature;
        private bool foldTemperature_isDirty;
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
                    foldTemperature_isDirty = true;
                }
            }
        }

        private short _coolDownTemperature;
        private bool coolDownTemperature_isDirty;
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
                    coolDownTemperature_isDirty = true;
                }
            }
        }

        private byte _temperatureControl;
        private bool temperatureControl_isDirty;
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
                    temperatureControl_isDirty = true;
                }
            }
        }

        private byte _blockedEQDays;
        private bool blockedEQDays_isDirty;
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
                    blockedEQDays_isDirty = true;
                }
            }
        }

        private byte _blockedFIDays;
        private bool blockedFIDays_isDirty;
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
                    blockedFIDays_isDirty = true;
                }
            }
        }
        #endregion

        #region constructor
        object myLock;
        internal BattviewSettings()
        {
            myLock = new object();
            _installationDate = new DateTime();
            //this.finishDaysMask = new battviewObject.daysMask();
            //this.autoStartMask = new battviewObject.daysMask();
            //this.EQdaysMask = new battviewObject.daysMask();
            //this.energyDaysMask = new battviewObject.daysMask();
            //this.lockoutDaysMask = new battviewObject.daysMask();

            zoneid_isDirty = false;
            installationDate_isDirty = false;
            warrantedAHR_isDirty = false;
            cvMaxDuration_isDirty = false;
            EQstartWindow_isDirty = false;
            EQcloseWindow_isDirty = false;
            FIstartWindow_isDirty = false;
            FIcloseWindow_isDirty = false;
            FIduration_isDirty = false;
            EQduration_isDirty = false;
            desulfation_isDirty = false;
            actViewConnectFrequency_isDirty = false;
            ahrcapacity_isDirty = false;
            batteryHighTemperature_isDirty = false;
            trickleVoltage_isDirty = false;
            CVTargetVoltage_isDirty = false;
            trickleCurrentRate_isDirty = false;
            CCrate_isDirty = false;
            CVendCurrentRate_isDirty = false;
            CVcurrentStep_isDirty = false;
            FItargetVoltage_isDirty = false;
            FIcurrentRate_isDirty = false;
            EQvoltage_isDirty = false;
            EQcurrentRate_isDirty = false;
            autoLogTime_isDirty = false;
            mobilePort_isDirty = false;
            actViewPort_isDirty = false;
            actViewIP_isDirty = false;
            actAccessSSID_isDirty = false;
            mobileAccessSSID_isDirty = false;
            actAccessSSIDpassword_isDirty = false;
            mobileAccessSSIDpassword_isDirty = false;
            actViewEnabled_isDirty = false;
            enableElectrolyeSensing_isDirty = false;
            enableHallEffectSensing_isDirty = false;
            enableExtTempSensing_isDirty = false;
            nominalvoltage_isDirty = false;
            batteryType_isDirty = false;
            batteryTemperatureCompesnation_isDirty = false;
            EQdaysMask_isDirty = false;
            FIdaysMask_isDirty = false;
            FIdv_isDirty = false;
            FIdt_isDirty = false;
            enablePLC_isDirty = false;
            enableDayLightSaving_isDirty = false;
            temperatureFormat_isDirty = false;
            chargerType_isDirty = false;
            TRTemperature_isDirty = false;
            foldTemperature_isDirty = false;
            coolDownTemperature_isDirty = false;
            temperatureControl_isDirty = false;
            blockedEQDays_isDirty = false;
            blockedFIDays_isDirty = false;

        }
        #endregion
        #region battview reset

        public void ResetbattviewSettings()
        {
            lock (myLock)
            {
                autoLogTime_isDirty = false;
                this.enableElectrolyeSensing_isDirty = false;
                this.enableExtTempSensing_isDirty = false;
                this.enableHallEffectSensing_isDirty = false;
                this.enablePLC_isDirty = false;
                this.temperatureFormat_isDirty = false;
                this.enableDayLightSaving_isDirty = false;
                this.temperatureControl_isDirty = false;

            }
        }


        public void ResetWiFi()
        {
            lock (myLock)
            {
                this.mobileAccessSSID_isDirty = false;
                this.mobileAccessSSIDpassword_isDirty = false;
                this.mobilePort_isDirty = false;
                this.actViewEnabled_isDirty = false;
                this.actViewIP_isDirty = false;
                this.actViewPort_isDirty = false;
                this.actViewConnectFrequency_isDirty = false;
                this.actAccessSSID_isDirty = false;
                this.actAccessSSIDpassword_isDirty = false;

            }
        }
        public void ResetBatterySettings()
        {
            lock (myLock)
            {
                this.installationDate_isDirty = false;
                this.zoneid_isDirty = false;
                this.ahrcapacity_isDirty = false;
                this.nominalvoltage_isDirty = false;
                this.batteryTemperatureCompesnation_isDirty = false;
                this.batteryHighTemperature_isDirty = false;
                this.batteryType_isDirty = false;
                this.warrantedAHR_isDirty = false;
                this.coolDownTemperature_isDirty = false;
                this.foldTemperature_isDirty = false;
                this.TRTemperature_isDirty = false;
                this.chargerType_isDirty = false;
            }

        }
        public void resetChargeProfile()
        {
            trickleCurrentRate_isDirty = CCrate_isDirty = FIcurrentRate_isDirty = EQcurrentRate_isDirty = trickleVoltage_isDirty = CVTargetVoltage_isDirty = FItargetVoltage_isDirty = EQvoltage_isDirty
                = CVendCurrentRate_isDirty = CVcurrentStep_isDirty = cvMaxDuration_isDirty = this.FIduration_isDirty = this.EQduration_isDirty
                = this.desulfation_isDirty = this.FIdv_isDirty = FIdt_isDirty = false;


        }
        public void resetChargeFIEQ()
        {
            this.FIdaysMask_isDirty = FIstartWindow_isDirty = FIcloseWindow_isDirty
            = EQstartWindow_isDirty = EQcloseWindow_isDirty = EQdaysMask_isDirty =
            this.blockedEQDays_isDirty = this.blockedFIDays_isDirty = false;


        }

        #endregion
        public bool anyChangeTobattview()
        {
            lock (myLock) return zoneid_isDirty || installationDate_isDirty || warrantedAHR_isDirty || cvMaxDuration_isDirty || EQstartWindow_isDirty || EQcloseWindow_isDirty || FIstartWindow_isDirty || FIcloseWindow_isDirty || FIduration_isDirty || EQduration_isDirty || desulfation_isDirty || actViewConnectFrequency_isDirty || ahrcapacity_isDirty || batteryHighTemperature_isDirty || trickleVoltage_isDirty || CVTargetVoltage_isDirty || trickleCurrentRate_isDirty || CCrate_isDirty || CVendCurrentRate_isDirty || CVcurrentStep_isDirty || FItargetVoltage_isDirty || FIcurrentRate_isDirty || EQvoltage_isDirty || EQcurrentRate_isDirty || autoLogTime_isDirty || mobilePort_isDirty || actViewPort_isDirty || actViewIP_isDirty || actAccessSSID_isDirty || mobileAccessSSID_isDirty || actAccessSSIDpassword_isDirty || mobileAccessSSIDpassword_isDirty || actViewEnabled_isDirty || enableElectrolyeSensing_isDirty || enableHallEffectSensing_isDirty || enableExtTempSensing_isDirty || nominalvoltage_isDirty || batteryType_isDirty || batteryTemperatureCompesnation_isDirty || EQdaysMask_isDirty || FIdaysMask_isDirty || FIdv_isDirty || FIdt_isDirty || enablePLC_isDirty || enableDayLightSaving_isDirty || temperatureFormat_isDirty || chargerType_isDirty || TRTemperature_isDirty || foldTemperature_isDirty || coolDownTemperature_isDirty || temperatureControl_isDirty || blockedEQDays_isDirty || blockedFIDays_isDirty;


        }

        public bool saveTime()
        {
            lock (myLock)
                return zoneid_isDirty;

        }

        public bool loadToConfig(BattViewConfig config)
        {
            bool warning = false;
            lock (myLock)
            {
                if (this.zoneid_isDirty)
                    config.zoneid = this._zoneid;
                if (this.installationDate_isDirty)
                    config.installationDate = this._installationDate;
                if (this.warrantedAHR_isDirty)
                    config.warrantedAHR = this._warrantedAHR;
                if (this.cvMaxDuration_isDirty)
                    config.cvMaxDuration = this._cvMaxDuration;
                if (this.EQstartWindow_isDirty)
                    config.EQstartWindow = this._EQstartWindow;
                if (this.EQcloseWindow_isDirty)
                    config.EQcloseWindow = this._EQcloseWindow;
                if (this.FIstartWindow_isDirty)
                    config.FIstartWindow = this._FIstartWindow;
                if (this.FIcloseWindow_isDirty)
                    config.FIcloseWindow = this._FIcloseWindow;
                if (this.FIduration_isDirty)
                    config.FIduration = this._FIduration;
                if (this.EQduration_isDirty)
                    config.EQduration = this._EQduration;
                if (this.desulfation_isDirty)
                    config.desulfation = this._desulfation;
                if (this.actViewConnectFrequency_isDirty)
                    config.actViewConnectFrequency = this._actViewConnectFrequency;
                if (this.ahrcapacity_isDirty)
                    config.ahrcapacity = this._ahrcapacity;
                if (this.batteryHighTemperature_isDirty)
                    config.batteryHighTemperature = this._batteryHighTemperature;

                bool cv_tr_ok = (this.CVTargetVoltage_isDirty ? _CVTargetVoltage : config.CVTargetVoltage) >= (trickleVoltage_isDirty ? _trickleVoltage : config.trickleVoltage);
                bool fi_cv_ok = (FItargetVoltage_isDirty ? _FItargetVoltage : config.FItargetVoltage) >= (CVTargetVoltage_isDirty ? _CVTargetVoltage : config.CVTargetVoltage);
                bool eq_fi_ok = (EQvoltage_isDirty ? _EQvoltage : config.EQvoltage) >= (FItargetVoltage_isDirty ? _FItargetVoltage : config.FItargetVoltage);


                if ((!cv_tr_ok && (trickleVoltage_isDirty || CVTargetVoltage_isDirty)) ||
                    (!fi_cv_ok && (EQvoltage_isDirty || CVTargetVoltage_isDirty)) ||
                    (!eq_fi_ok && (EQvoltage_isDirty || EQvoltage_isDirty)))
                    warning = true;
                else
                {
                    if (this.trickleVoltage_isDirty)
                        config.trickleVoltage = this._trickleVoltage;
                    if (this.CVTargetVoltage_isDirty)
                        config.CVTargetVoltage = this._CVTargetVoltage;
                    if (this.FItargetVoltage_isDirty)
                        config.FItargetVoltage = this._FItargetVoltage;
                    if (this.EQvoltage_isDirty)
                        config.EQvoltage = this._EQvoltage;
                }

                bool cc_tr_ok = ((CCrate_isDirty ? _CCrate : config.CCrate) >= (trickleCurrentRate_isDirty ? _trickleCurrentRate : config.trickleCurrentRate));
                bool cc_fi_ok = ((CCrate_isDirty ? _CCrate : config.CCrate) >= (FIcurrentRate_isDirty ? _FIcurrentRate : config.FIcurrentRate));
                bool cc_cv_ok = ((CCrate_isDirty ? _CCrate : config.CCrate) >= (50.0 * (CVendCurrentRate_isDirty ? _CVendCurrentRate : config.CVendCurrentRate)));
                bool fi_eq_ok = ((FIcurrentRate_isDirty ? _FIcurrentRate : config.FIcurrentRate) >= (EQcurrentRate_isDirty ? _EQcurrentRate : config.EQcurrentRate));

                if ((!cc_tr_ok && (trickleCurrentRate_isDirty || CCrate_isDirty)) ||
                (!cc_fi_ok && (FIcurrentRate_isDirty || CCrate_isDirty)) ||
                (!cc_cv_ok && (CVendCurrentRate_isDirty || CCrate_isDirty)) ||
                (!fi_eq_ok && (FIcurrentRate_isDirty || EQcurrentRate_isDirty)))
                {
                    warning = true;

                }
                else
                {

                    if (this.trickleCurrentRate_isDirty)
                        config.trickleCurrentRate = this._trickleCurrentRate;
                    if (this.CCrate_isDirty)
                        config.CCrate = this._CCrate;
                    if (this.FIcurrentRate_isDirty)
                        config.FIcurrentRate = this._FIcurrentRate;
                    if (this.CVendCurrentRate_isDirty)
                        config.CVendCurrentRate = this._CVendCurrentRate;
                    if (this.EQcurrentRate_isDirty)
                        config.EQcurrentRate = this._EQcurrentRate;

                    if (this.CVcurrentStep_isDirty)
                        config.CVcurrentStep = this._CVcurrentStep;

                }



                if (this.autoLogTime_isDirty)
                    config.autoLogTime = this._autoLogTime;
                if (this.mobilePort_isDirty)
                    config.mobilePort = this._mobilePort;
                if (this.actViewPort_isDirty)
                    config.actViewPort = this._actViewPort;
                if (this.actViewIP_isDirty)
                    config.actViewIP = this._actViewIP;
                if (this.actAccessSSID_isDirty)
                    config.actAccessSSID = this._actAccessSSID;
                if (this.mobileAccessSSID_isDirty)
                    config.mobileAccessSSID = this._mobileAccessSSID;
                if (this.actAccessSSIDpassword_isDirty)
                    config.actAccessSSIDpassword = this._actAccessSSIDpassword;
                if (this.mobileAccessSSIDpassword_isDirty)
                    config.mobileAccessSSIDpassword = this._mobileAccessSSIDpassword;
                if (this.actViewEnabled_isDirty)
                    config.ActViewEnabled = this._actViewEnabled;
                if (this.enableElectrolyeSensing_isDirty)
                    config.enableElectrolyeSensing = this._enableElectrolyeSensing;
                if (this.enableHallEffectSensing_isDirty)
                    config.enableHallEffectSensing = this._enableHallEffectSensing;
                if (this.enableExtTempSensing_isDirty)
                    config.enableExtTempSensing = this._enableExtTempSensing;
                if (this.nominalvoltage_isDirty)
                    config.nominalvoltage = this._nominalvoltage;
                if (this.batteryType_isDirty)
                    config.batteryType = this._batteryType;
                if (this.batteryTemperatureCompesnation_isDirty)
                    config.batteryTemperatureCompesnation = this._batteryTemperatureCompesnation;
                if (this.EQdaysMask_isDirty)
                    config.EQdaysMask = this._EQdaysMask;
                if (this.FIdaysMask_isDirty)
                    config.FIdaysMask = this._FIdaysMask;
                if (this.FIdv_isDirty)
                    config.FIdv = this._FIdv;
                if (this.FIdt_isDirty)
                    config.FIdt = this._FIdt;
                if (this.enablePLC_isDirty)
                    config.enablePLC = this._enablePLC;
                if (this.enableDayLightSaving_isDirty)
                    config.enableDayLightSaving = this._enableDayLightSaving;
                if (this.temperatureFormat_isDirty)
                    config.temperatureFormat = this._temperatureFormat;
                if (this.chargerType_isDirty)
                    config.chargerType = this._chargerType;
                if (this.TRTemperature_isDirty)
                    config.TRTemperature = this._TRTemperature;
                if (this.foldTemperature_isDirty)
                    config.foldTemperature = this._foldTemperature;
                if (this.coolDownTemperature_isDirty)
                    config.coolDownTemperature = this._coolDownTemperature;
                if (this.temperatureControl_isDirty)
                    config.temperatureControl = this._temperatureControl;
                if (this.blockedEQDays_isDirty)
                    config.blockedEQDays = this._blockedEQDays;
                if (this.blockedFIDays_isDirty)
                    config.blockedFIDays = this._blockedFIDays;
            }
            return warning;
        }

    }
}
