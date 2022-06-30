using System;
using System.Text;

namespace actchargers
{
    public class LcdSimulator
    {
        private bool _battConnected_p;
        public bool battConnected_p
        {
            get
            {

                return _battConnected_p;
            }
        }
        private bool _showLastChargeCycleDetails_p;
        public bool showLastChargeCycleDetails_p
        {
            get
            {

                return _showLastChargeCycleDetails_p;
            }
        }
        private bool _batteryCharging_p;
        public bool batteryCharging_p
        {
            get
            {

                return _batteryCharging_p;
            }
        }
        private byte _currentRunningProfile;
        public byte currentRunningProfile
        {
            get
            {

                return _currentRunningProfile;
            }
        }
        private bool _batteryDisCharging_p;
        public bool batteryDisCharging_p
        {
            get
            {

                return _batteryDisCharging_p;
            }
        }
        private byte _EQ_p;
        public byte EQ_p
        {
            get
            {

                return _EQ_p;
            }
        }
        private bool _desulfate_p;
        public bool desulfate_p
        {
            get
            {

                return _desulfate_p;
            }
        }
        private bool _refresh_p;
        public bool refresh_p
        {
            get
            {

                return _refresh_p;
            }
        }
        private UInt32 _refreshTimer;
        public UInt32 refreshTimer
        {
            get
            {

                return _refreshTimer;
            }
        }
        private bool _wifiExist_p;
        public bool wifiExist_p
        {
            get
            {

                return _wifiExist_p;
            }
        }
        private byte _wifiStatus_p;
        public byte wifiStatus_p
        {
            get
            {

                return _wifiStatus_p;
            }
        }
        private bool _ethernetExist_p;
        public bool ethernetExist_p
        {
            get
            {

                return _ethernetExist_p;
            }
        }
        private byte _ethernetStatus_p;
        public byte ethernetStatus_p
        {
            get
            {

                return _ethernetStatus_p;
            }
        }
        private byte _currentSOC;
        public byte currentSOC
        {
            get
            {

                return _currentSOC;
            }
        }
        private byte _showWarning_p;
        public byte showWarning_p
        {
            get
            {

                return _showWarning_p;
            }
        }
        private bool _autoStart_p;
        public bool autoStart_p
        {
            get
            {

                return _autoStart_p;
            }
        }
        private byte _autoSatrtTimer;
        public byte autoSatrtTimer
        {
            get
            {

                return _autoSatrtTimer;
            }
        }
        private byte _voltageRangeStatus;
        public byte voltageRangeStatus
        {
            get
            {

                return _voltageRangeStatus;
            }
        }
        private byte _energyManagmentType;
        public byte energyManagmentType
        {
            get
            {

                return _energyManagmentType;
            }
        }
        private DateTime _timeStamp;
        public DateTime timeStamp
        {
            get
            {

                return new DateTime(_timeStamp.Ticks);
            }
        }

        private string _batteryID;
        public string batteryID
        {
            get
            {

                return _batteryID;
            }
        }

        private byte _batviewOn;
        public byte batviewOn
        {
            get
            {

                return _batviewOn;
            }
        }
        private byte _simulationMode;

        public byte simulationMode
        {
            get
            {

                return _simulationMode;
            }
        }
        private UInt32 _duration;
        public UInt32 duration
        {
            get
            {
                return _duration;

            }

        }
        private UInt32 _current;
        public UInt32 current
        {
            get
            {
                return _current;

            }

        }
        private UInt32 _AS;
        public UInt32 AS
        {
            get
            {
                return _AS;

            }

        }
        private UInt32 _WS;
        public UInt32 WS
        {
            get
            {
                return _WS;

            }

        }
        private UInt16 _voltageVal;
        public UInt16 voltageVal
        {
            get
            {
                return _voltageVal;

            }

        }
        private Int16 _temperatureVal;
        public Int16 temperatureVal
        {
            get
            {
                return _temperatureVal;

            }

        }
        private bool _temperatureConnected_p;
        public bool temperatureConnected_p
        {
            get
            {
                return _temperatureConnected_p;

            }

        }
        private byte _status;
        public byte status
        {
            get
            {
                return _status;

            }

        }
        private bool _paused;
        public bool paused
        {
            get
            {
                return _paused;

            }

        }
        private byte _batteryVoltage;
        public byte batteryVoltage
        {
            get
            {

                return _batteryVoltage;
            }
        }
        private UInt16 _TRvoltageVal;
        public UInt16 TRvoltageVal
        {
            get
            {
                return _TRvoltageVal;

            }

        }
        private UInt16 _batteryCapacity;
        public UInt16 batteryCapacity
        {
            get
            {
                return _batteryCapacity;

            }

        }
        private UInt16 _EQvoltageVal;
        public UInt16 EQvoltageVal
        {
            get
            {
                return _EQvoltageVal;

            }

        }
        private UInt32 _DesulfationTimer_ToUse;
        public UInt32 DesulfationTimer_ToUse
        {
            get
            {
                return _DesulfationTimer_ToUse;

            }

        }

        private UInt16 _DesulfationCapacity_ToUse;
        public UInt16 DesulfationCapacity_ToUse
        {
            get
            {
                return _DesulfationCapacity_ToUse;

            }

        }
        private byte _DesulfationVoltage_ToUse;
        public byte DesulfationVoltage_ToUse
        {
            get
            {
                return _DesulfationVoltage_ToUse;

            }

        }

        private byte _numberOfCells;
        public byte numberOfCells
        {
            get
            {
                return _numberOfCells;

            }

        }
        private byte _calState;
        public byte calState
        {
            get
            {
                return _calState;

            }

        }

        private byte BMSconnected;

        private UInt32 _battviewID;
        public UInt32 battviewID
        {
            get
            {
                return _battviewID;

            }

        }
        private float _numberOfCells_new;
        public float numberOfCells_new
        {
            get
            {
                return _numberOfCells_new;

            }

        }

        private UInt32 _relaxedBatteryTimer;
        public UInt32 relaxedBatteryTimer
        {
            get
            {
                return _relaxedBatteryTimer;

            }

        }
        private UInt32 _LastCycleID;
        public UInt32 LastCycleID
        {
            get
            {
                return _LastCycleID;

            }

        }
        private byte _ReconnectFlag;
        public byte ReconnectFlag
        {
            get
            {
                return _ReconnectFlag;

            }

        }
        private byte _FirstTime;
        public byte FirstTime
        {
            get
            {
                return _FirstTime;

            }

        }

        public byte PowerFactorScale { get; set; }

        public byte RunWithBMS { get; set; }

        internal LcdSimulator GetAcopy()
        {
            LcdSimulator a = new LcdSimulator
            {
                _DesulfationTimer_ToUse = this._DesulfationTimer_ToUse,
                _DesulfationVoltage_ToUse = this._DesulfationVoltage_ToUse,
                _DesulfationCapacity_ToUse = this._DesulfationCapacity_ToUse,
                _batteryID = this._batteryID,
                _batviewOn = this._batviewOn,
                _timeStamp = this._timeStamp,
                _battConnected_p = this.battConnected_p,
                _showLastChargeCycleDetails_p = this.showLastChargeCycleDetails_p,
                _batteryCharging_p = this.batteryCharging_p,
                _currentRunningProfile = this.currentRunningProfile,
                _batteryDisCharging_p = this.batteryDisCharging_p,
                _EQ_p = this.EQ_p,
                _desulfate_p = this.desulfate_p,
                _refresh_p = this.refresh_p,
                _refreshTimer = this.refreshTimer,
                _wifiExist_p = this.wifiExist_p,
                _wifiStatus_p = this.wifiStatus_p,
                _ethernetExist_p = this.ethernetExist_p,
                _ethernetStatus_p = this.ethernetStatus_p,
                _currentSOC = this.currentSOC,
                _showWarning_p = this.showWarning_p,
                _autoStart_p = this.autoStart_p,
                _autoSatrtTimer = this.autoSatrtTimer,
                _voltageRangeStatus = this.voltageRangeStatus,
                _energyManagmentType = this.energyManagmentType,
                _simulationMode = this._simulationMode,

                _duration = this._duration,
                _current = this._current,
                _AS = this._AS,
                _WS = this._WS,
                _voltageVal = this._voltageVal,
                _temperatureVal = this._temperatureVal,
                _temperatureConnected_p = this._temperatureConnected_p,
                _status = this._status,
                _paused = this._paused,
                _batteryVoltage = this._batteryVoltage,
                _TRvoltageVal = this._TRvoltageVal,
                _EQvoltageVal = this._EQvoltageVal,
                _batteryCapacity = this._batteryCapacity,
                _numberOfCells = this._numberOfCells,
                _calState = this._calState,
                BMSconnected = this.BMSconnected,
                _battviewID = this._battviewID,
                _numberOfCells_new = this._numberOfCells_new,
                _relaxedBatteryTimer = this._relaxedBatteryTimer,
                _LastCycleID = this._LastCycleID,
                _ReconnectFlag = this._ReconnectFlag,
                _FirstTime = this._FirstTime,
                PowerFactorScale = this.PowerFactorScale,
                RunWithBMS = this.RunWithBMS
            };

            return a;
        }

        internal void LoadFromArray(byte[] arr)
        {
            int loc = 0;

            this._refreshTimer = BitConverter.ToUInt32(arr, loc);
            loc += 4;

            _timeStamp = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(BitConverter.ToUInt32(arr, loc));
            loc += 4;

            _duration = BitConverter.ToUInt32(arr, loc);
            loc += 4;
            _current = BitConverter.ToUInt32(arr, loc);
            loc += 4;
            _AS = BitConverter.ToUInt32(arr, loc);
            loc += 4;
            _WS = BitConverter.ToUInt32(arr, loc);
            loc += 4;

            _DesulfationTimer_ToUse = BitConverter.ToUInt32(arr, loc);
            loc += 4;

            _voltageVal = BitConverter.ToUInt16(arr, loc);
            loc += 2;
            _temperatureVal = BitConverter.ToInt16(arr, loc);
            loc += 2;
            _TRvoltageVal = BitConverter.ToUInt16(arr, loc);
            loc += 2;
            _EQvoltageVal = BitConverter.ToUInt16(arr, loc);
            loc += 2;
            _batteryCapacity = BitConverter.ToUInt16(arr, loc);
            loc += 2;
            _DesulfationCapacity_ToUse = BitConverter.ToUInt16(arr, loc);
            loc += 2;
            _batteryID = Encoding.UTF8.GetString(arr, loc, 18);
            loc += 18;

            _batviewOn = arr[loc++];
            _battConnected_p = arr[loc++] != 0;
            _showLastChargeCycleDetails_p = arr[loc++] != 0;
            _batteryCharging_p = arr[loc++] != 0;
            _currentRunningProfile = arr[loc++];
            _batteryDisCharging_p = arr[loc++] != 0;
            _EQ_p = arr[loc++];
            _desulfate_p = arr[loc++] != 0;
            _refresh_p = arr[loc++] != 0;
            _wifiExist_p = arr[loc++] != 0;
            _wifiStatus_p = arr[loc++];
            _ethernetExist_p = arr[loc++] != 0;
            _ethernetStatus_p = arr[loc++];
            _currentSOC = arr[loc++];
            _showWarning_p = arr[loc++];
            _autoStart_p = arr[loc++] != 0;
            _autoSatrtTimer = arr[loc++];
            _voltageRangeStatus = arr[loc++];
            _energyManagmentType = arr[loc++];
            _simulationMode = arr[loc++];

            _temperatureConnected_p = arr[loc++] != 0;
            _status = arr[loc++];
            _paused = arr[loc++] != 0;
            _batteryVoltage = arr[loc++];
            _DesulfationVoltage_ToUse = arr[loc++];
            _numberOfCells = arr[loc++];
            _calState = arr[loc++];
            BMSconnected = arr[loc++];
            _battviewID = BitConverter.ToUInt32(arr, loc); loc += 4;
            _numberOfCells_new = BitConverter.ToSingle(arr, loc); loc += 4;
            _relaxedBatteryTimer = BitConverter.ToUInt32(arr, loc); loc += 4;
            _LastCycleID = BitConverter.ToUInt32(arr, loc); loc += 4;
            _ReconnectFlag = arr[loc++];
            _FirstTime = arr[loc++];
            PowerFactorScale = arr[loc++];
            RunWithBMS = arr[loc++];
        }
    }
}
