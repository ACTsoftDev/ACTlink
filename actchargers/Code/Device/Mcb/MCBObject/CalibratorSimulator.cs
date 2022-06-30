using System;
using System.Text;

namespace actchargers
{
    public class CalibratorSimulator
    {
        private float _loadNTCRefADCAvg;
        public float loadNTCRefADCAvg
        {
            get { return _loadNTCRefADCAvg; }
        }

        private float _loadNTCRefADCInstant;
        public float loadNTCRefADCInstant
        {
            get { return _loadNTCRefADCInstant; }
        }
        private float _LoadCurrent;
        public float LoadCurrent
        {
            get { return _LoadCurrent; }
        }
        private float _loadVoltage;
        public float loadVoltage
        {
            get { return _loadVoltage; }
        }
        private float _loadTemperature;
        public float loadTemperature
        {
            get { return _loadTemperature; }
        }
        private float _LoadCurrentADCAvg;
        public float LoadCurrentADCAvg
        {
            get { return _LoadCurrentADCAvg; }
        }
        private float _loadVoltageADCAvg;
        public float loadVoltageADCAvg
        {
            get { return _loadVoltageADCAvg; }
        }
        private float _loadTemperatureADCAvg;
        public float loadTemperatureADCAvg
        {
            get { return _loadTemperatureADCAvg; }
        }
        private float _LoadCurrentADCInstant;
        public float LoadCurrentADCInstant
        {
            get { return _LoadCurrentADCInstant; }
        }
        private float _loadVoltageADCInstant;
        public float loadVoltageADCInstant
        {
            get { return _loadVoltageADCInstant; }
        }
        private float _loadTemperatureADCInstant;
        public float loadTemperatureADCInstant
        {
            get { return _loadTemperatureADCInstant; }
        }
        private float _battviewCurrent;
        public float battviewCurrent
        {
            get { return _battviewCurrent; }
        }
        private float _battviewVoltage;
        public float battviewVoltage
        {
            get { return _battviewVoltage; }
        }
        private float _battviewTemperature;
        public float battviewTemperature
        {
            get { return _battviewTemperature; }
        }
        private float _currentControl;
        public float currentControl
        {
            get { return _currentControl; }
        }
        private UInt32 _battviewID;
        public UInt32 battviewID
        {
            get { return _battviewID; }
        }
        private string _battviewSN;
        public string battviewSN
        {
            get { return _battviewSN; }
        }
        private byte _setCalibratorManualMode;
        public byte setCalibratorManualMode
        {
            get { return _setCalibratorManualMode; }
        }
        private byte _driveBitsControl;
        public byte driveBitsControl
        {
            get { return _driveBitsControl; }
        }
        private bool _disablePLC;
        public bool disablePLC
        {
            get { return _disablePLC; }
        }
        private byte _state;
        public byte state
        {
            get { return _state; }
        }
        private float _battViewVersion;
        public float battViewVersion
        {
            get { return _battViewVersion; }
        }

        private byte _zoneId;
        public byte zoneId
        {
            get { return _zoneId; }
        }

        private byte _bufferHasValidBVConfig;
        public byte bufferHasValidBVConfig
        {
            get { return _bufferHasValidBVConfig; }
        }

        private UInt32 _BVtimeSinceLastRead;
        public UInt32 BVtimeSinceLastRead
        {
            get { return _BVtimeSinceLastRead; }
        }
        private byte _writeDone;
        public byte writeDone
        {
            get { return _writeDone; }
        }
        private byte _replacementPart;
        public byte replacementPart
        {
            get { return _replacementPart; }
        }
        private byte _readWriteStep;
        public byte readWriteStep
        {
            get { return _readWriteStep; }
        }
        internal CalibratorSimulator getACopy()
        {
            CalibratorSimulator a = new CalibratorSimulator();
            a._loadNTCRefADCAvg = this._loadNTCRefADCAvg;
            a._loadNTCRefADCInstant = this._loadNTCRefADCInstant;
            a._LoadCurrent = this._LoadCurrent;
            a._loadVoltage = this._loadVoltage;
            a._loadTemperature = this._loadTemperature;
            a._LoadCurrentADCAvg = this._LoadCurrentADCAvg;
            a._loadVoltageADCAvg = this._loadVoltageADCAvg;
            a._loadTemperatureADCAvg = this._loadTemperatureADCAvg;
            a._LoadCurrentADCInstant = this._LoadCurrentADCInstant;
            a._loadVoltageADCInstant = this._loadVoltageADCInstant;
            a._loadTemperatureADCInstant = this._loadTemperatureADCInstant;
            a._battviewCurrent = this._battviewCurrent;
            a._battviewVoltage = this._battviewVoltage;
            a._battviewTemperature = this._battviewTemperature;
            a._currentControl = this._currentControl;
            a._battviewID = this._battviewID;
            a._battviewSN = this._battviewSN;
            a._setCalibratorManualMode = this._setCalibratorManualMode;
            a._driveBitsControl = this._driveBitsControl;
            a._disablePLC = this._disablePLC;
            a._state = this._state;
            a._battViewVersion = this._battViewVersion;
            a._zoneId = this._zoneId;
            a._bufferHasValidBVConfig = this._bufferHasValidBVConfig;
            a._BVtimeSinceLastRead = this._BVtimeSinceLastRead;
            a._writeDone = this._writeDone;
            a._replacementPart = this._replacementPart;
            a._readWriteStep = this._readWriteStep;

            return a;
        }
        internal void loadFromArray(byte[] arr)
        {
            int loc = 0;

            this._loadNTCRefADCAvg = BitConverter.ToSingle(arr, loc);
            loc += 4;
            this._loadNTCRefADCInstant = BitConverter.ToSingle(arr, loc);
            loc += 4;
            this._LoadCurrent = BitConverter.ToSingle(arr, loc);
            loc += 4;


            this._loadVoltage = BitConverter.ToSingle(arr, loc);
            loc += 4;

            this._loadTemperature = BitConverter.ToSingle(arr, loc);
            loc += 4;

            this._LoadCurrentADCAvg = BitConverter.ToSingle(arr, loc);
            loc += 4;

            this._loadVoltageADCAvg = BitConverter.ToSingle(arr, loc);
            loc += 4;

            this._loadTemperatureADCAvg = BitConverter.ToSingle(arr, loc);
            loc += 4;

            this._LoadCurrentADCInstant = BitConverter.ToSingle(arr, loc);
            loc += 4;

            this._loadVoltageADCInstant = BitConverter.ToSingle(arr, loc);
            loc += 4;

            this._loadTemperatureADCInstant = BitConverter.ToSingle(arr, loc);
            loc += 4;

            this._battviewCurrent = BitConverter.ToSingle(arr, loc);
            loc += 4; ;
            this._battviewVoltage = BitConverter.ToSingle(arr, loc);
            loc += 4; ;
            this._battviewTemperature = BitConverter.ToSingle(arr, loc);
            loc += 4; ;

            this._currentControl = BitConverter.ToSingle(arr, loc);
            loc += 4;

            this._battviewID = BitConverter.ToUInt32(arr, loc);
            loc += 4;

            this._battviewSN = Encoding.UTF8.GetString(arr, loc, 12).TrimEnd(new char[] { '\0', ' ' });
            loc += 12;

            this._setCalibratorManualMode = arr[loc++];
            this._driveBitsControl = arr[loc++];
            this._disablePLC = arr[loc++] > 0;
            this._state = arr[loc++];
            this._battViewVersion = BitConverter.ToSingle(arr, loc);
            loc += 4;


            this._zoneId = arr[loc++];
            this._bufferHasValidBVConfig = arr[loc++];
            this._BVtimeSinceLastRead = BitConverter.ToUInt32(arr, loc);
            loc += 4;

            this._writeDone = arr[loc++];

            this._replacementPart = arr[loc++];
            this._readWriteStep = arr[loc++];

        }
    }

}
