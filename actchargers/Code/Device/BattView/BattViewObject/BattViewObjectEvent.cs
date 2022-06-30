using System;

namespace actchargers
{
    public class BattViewObjectEvent : IEquatable<BattViewObjectEvent>, IComparable<BattViewObjectEvent>
    {
        private UInt32 _eventID;
        public UInt32 eventID
        {
            get
            {
                return _eventID;
            }
            internal set
            {
                _eventID = value;
            }
        }

        private UInt32 _chargeWS;
        public UInt32 chargeWS
        {
            get
            {
                return _chargeWS;
            }
        }
        private UInt32 _inUseWS;
        public UInt32 inUseWS
        {
            get
            {
                return _inUseWS;
            }
        }
        private UInt32 _chargeAS;
        public UInt32 chargeAS
        {
            get
            {
                return _chargeAS;
            }
        }
        private UInt32 _inUseAS;
        public UInt32 inUseAS
        {
            get
            {
                return _inUseAS;
            }
        }
        private UInt32 _startTime;
        public UInt32 startTime
        {
            get
            {
                return _startTime;
            }
        }
        private UInt32 _original_start_time;
        public UInt32 original_start_time
        {
            get
            {
                return _original_start_time;
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
        private UInt32 _chargerID;
        public UInt32 chargerID
        {
            get
            {
                return _chargerID;
            }
        }
        private UInt32 _maxTemperatureTimeStamp;
        public UInt32 maxTemperatureTimeStamp
        {
            get
            {
                return _maxTemperatureTimeStamp;
            }
        }
        private Int16 _maxTemperature;
        public Int16 maxTemperature
        {
            get
            {
                return _maxTemperature;
            }
        }
        private UInt16 _minVoltage;
        public UInt16 minVoltage
        {
            get
            {
                return _minVoltage;
            }
        }
        private UInt16 _startVoltage;
        public UInt16 startVoltage
        {
            get
            {
                return _startVoltage;
            }
        }
        private UInt16 _endVoltageSeconds;
        public UInt16 endVoltageSeconds
        {
            get
            {
                return _endVoltageSeconds;
            }
        }
        private UInt16 _eventMaxCurrent;
        public UInt16 eventMaxCurrent
        {
            get
            {
                return _eventMaxCurrent;
            }
        }
        private UInt16 _eventAverageCurrent;
        public UInt16 eventAverageCurrent
        {
            get
            {
                return _eventAverageCurrent;
            }
        }
        private byte _startSOC;
        public byte startSOC
        {
            get
            {
                return _startSOC;
            }
        }
        private byte _endSOC;
        public byte endSOC
        {
            get
            {
                return _endSOC;
            }
        }

        private byte _eventFlags;
        public byte eventFlags
        {
            get
            {
                return _eventFlags;
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
        private byte[] unused;
        private bool _valid;
        public bool valid
        {
            get
            {
                return _valid;
            }
        }

        private string _eventType;
        public string eventType
        {
            get
            {
                return _eventType;
            }
        }
        private byte _eventTypeID;
        public byte eventTypeID
        {
            get
            {
                return _eventTypeID;
            }
        }
        private string _eventNotes;
        public string eventNotes
        {
            get
            {
                return _eventNotes;
            }
        }
        private bool _has_FI;
        public bool has_FI
        {
            get
            {
                return _has_FI;
            }
        }
        private bool _has_EQ;
        public bool has_EQ
        {
            get
            {
                return _has_EQ;
            }
        }
        private bool _has_Electrolyte_sensor;
        public bool has_Electrolyte_sensor
        {
            get
            {
                return _has_Electrolyte_sensor;
            }
        }
        private bool _water_high;
        public bool water_high
        {
            get
            {
                return _water_high;
            }
        }
        private bool _temperature_sensor_enabled;
        public bool temperature_sensor_enabled
        {
            get
            {
                return _temperature_sensor_enabled;
            }
        }
        private bool _start_temperatureEnabled;
        public bool start_temperatureEnabled
        {
            get
            {
                return _start_temperatureEnabled;
            }
        }
        private Int16 _startTemperature;
        public Int16 startTemperature
        {
            get
            {
                return _startTemperature;
            }
        }


        private bool _event_with_charger_at_start;
        public bool event_with_charger_at_start
        {
            get
            {
                return _event_with_charger_at_start;
            }
        }
        private bool _event_with_charger_at_end;
        public bool event_with_charger_at_end
        {
            get
            {
                return _event_with_charger_at_end;
            }
        }
        private bool _charge_with_FI_Start;
        public bool charge_with_FI_Start
        {
            get
            {
                return _charge_with_FI_Start;
            }
        }
        private bool _charge_with_EQ_Start;
        public bool charge_with_EQ_Start
        {
            get
            {
                return _charge_with_EQ_Start;
            }
        }
        private ChargeCycleTypes _chargeCycleType;
        public ChargeCycleTypes chargeCycleType
        {
            get
            {
                return _chargeCycleType;
            }
        }
        private bool _charge_split_req;
        public bool charge_split_req
        {
            get
            {
                return _charge_split_req;
            }
        }
        private bool _soc_set;
        public bool soc_set
        {
            get
            {
                return _soc_set;
            }
        }
        private byte _status2;
        public byte status2
        {
            get
            {
                return _status2;
            }
        }
        private FiDoneReason _fiDoneReason;
        public FiDoneReason fiDoneReason
        {
            get
            {
                return _fiDoneReason;
            }
        }
        private bool _after_restart;
        public bool after_restart
        {
            get
            {
                return _after_restart;
            }
        }

        private bool _calibration_changed;
        public bool calibration_changed
        {
            get
            {
                return _calibration_changed;
            }
        }

        private bool _firmware_req;
        public bool firmware_req
        {
            get
            {
                return _firmware_req;
            }
        }

        private bool _charger_disconnect;
        public bool charger_disconnect
        {
            get
            {
                return _charger_disconnect;
            }
        }

        private byte _exitStatus;
        public byte exitStatus
        {
            get
            {
                return _exitStatus;
            }
        }


        private byte _event_version;
        public byte event_version
        {
            get
            {
                return _event_version;
            }
        }

        private byte _lostChargerConnection;
        public byte lostChargerConnection
        {
            get
            {
                return _lostChargerConnection;
            }
        }
        public BattViewObjectEvent()
        {
            unused = new byte[5];
            _valid = false;
        }
        public void loadFromBuffer(byte[] resultArray, int Copyloc)
        {
            byte[] result = new byte[64];
            Array.Copy(resultArray, Copyloc, result, 0, 64);
            int loc = 0;
            _valid = true;
            //eventID
            _eventID = BitConverter.ToUInt32(result, loc);
            loc += 4;
            //chargeWS
            _chargeWS = BitConverter.ToUInt32(result, loc);
            loc += 4;
            //inUseWS
            _inUseWS = BitConverter.ToUInt32(result, loc);
            loc += 4;
            //chargeAS
            _chargeAS = BitConverter.ToUInt32(result, loc);
            loc += 4;
            //inUseAS
            _inUseAS = BitConverter.ToUInt32(result, loc);
            loc += 4;
            //startTime
            _startTime = BitConverter.ToUInt32(result, loc);
            loc += 4;
            //duration
            _duration = BitConverter.ToUInt32(result, loc);
            loc += 4;
            //chargerID
            _chargerID = BitConverter.ToUInt32(result, loc);
            loc += 4;
            //maxTemperatureTimeStamp
            _maxTemperatureTimeStamp = BitConverter.ToUInt32(result, loc);
            loc += 4;
            //maxTemperature
            _maxTemperature = BitConverter.ToInt16(result, loc);
            loc += 2;
            //minVoltage
            _minVoltage = BitConverter.ToUInt16(result, loc);
            loc += 2;
            //startVoltage
            _startVoltage = BitConverter.ToUInt16(result, loc);
            loc += 2;
            //endVoltage30Seconds
            _endVoltageSeconds = BitConverter.ToUInt16(result, loc);
            loc += 2;
            //eventMaxCurrent
            _eventMaxCurrent = BitConverter.ToUInt16(result, loc);
            loc += 2;
            //eventAverageCurrent
            _eventAverageCurrent = BitConverter.ToUInt16(result, loc);
            loc += 2;
            _startSOC = result[loc++];
            _endSOC = result[loc++];
            _eventFlags = result[loc++];

            if ((_eventFlags & 0x03) == 0)
            {
                _valid = false;
            }
            if ((eventFlags & 0x03) == 0x01)
            {
                _eventType = "Charge";
                _eventTypeID = 1;
                if ((_eventFlags & 0x04) != 0)
                {
                    _has_FI = true;
                }
                if ((_eventFlags & 0x08) != 0)
                {
                    _has_EQ = true;
                }
            }
            else if ((_eventFlags & 0x03) == 0x02)
            {
                _eventTypeID = 2;
                _eventType = "Idle";
                _has_FI = false;
                _has_EQ = false;
            }
            else if ((_eventFlags & 0x03) == 0x03)
            {
                _eventTypeID = 3;
                _eventType = "In Use";
                _has_FI = false;
                _has_EQ = false;
            }
            _has_Electrolyte_sensor = false;
            if ((_eventFlags & 0x20) != 0)
            {
                _has_Electrolyte_sensor = true;
                if ((_eventFlags & 0x10) != 0)
                {
                    _water_high = true;
                }
                else
                {
                    _water_high = false;
                }
            }
            _eventNotes = "";
            if ((_eventFlags & 0x40) != 0)
            {
                _eventNotes += "-MID NIGHT";
            }
            if ((_eventFlags & 0x80) != 0)
            {
                _eventNotes += "-Time Change";
            }
            _status = result[loc++];
            _start_temperatureEnabled = false;
            if ((_status & 0x01) != 0)
            {
                _temperature_sensor_enabled = true;
                if ((_status & 0x02) != 0)
                    _start_temperatureEnabled = true;
            }
            else
                _temperature_sensor_enabled = false;

            byte zoneID = result[loc++];
            _startTemperature = BitConverter.ToInt16(result, loc);
            loc += 2;




            bool dayLightSaving = (zoneID & 0x80) != 0;
            zoneID &= 0x7F;
            _original_start_time = startTime;
            _startTime = StaticDataAndHelperFunctions.getZoneUnixTimeStampFromUTC(zoneID, _startTime, dayLightSaving);

            _status2 = result[loc++];
            //read exit status
            this._exitStatus = result[loc++];
            //read version
            this._event_version = result[loc++];
            //startign from 2.09 & record version 1
            if (_event_version == 1)
            {



                if (_eventTypeID != 1)
                {
                    this._event_with_charger_at_start = ((_status & 0x04) != 0);
                    this._event_with_charger_at_end = ((_status & 0x08) != 0);
                    if (_eventTypeID == 2 && (_status & 0x80) != 0)
                        this._soc_set = true;
                    //idle & Inuse
                }
                else
                {
                    this._charge_with_FI_Start = ((_status & 0x04) != 0);
                    this._charge_with_EQ_Start = ((_status & 0x08) != 0);

                    if ((_status & 0x10) != 0)
                    {
                        if ((_status & 0x20) != 0)
                            this._chargeCycleType = ChargeCycleTypes.desulfate_charge_cycle;
                        else
                            this._chargeCycleType = ChargeCycleTypes.refresh_charge_cycle;


                    }
                    else
                    {
                        this._chargeCycleType = ChargeCycleTypes.normal_charge_cycle;
                    }
                    if ((_status & 0x40) != 0)
                        this._charge_split_req = true;
                    switch ((_status2 & 0x07))
                    {
                        case 0x01: this._fiDoneReason = FiDoneReason.timeout; break;
                        case 0x02: this._fiDoneReason = FiDoneReason.dv_dt; break;
                        case 0x03: this._fiDoneReason = FiDoneReason.target_voltage_reached; break;
                        case 0x04: this._fiDoneReason = FiDoneReason.target_voltage_reached_regulated; break;

                    }


                }
                if ((_status2 & 0x40) != 0)
                {
                    this._charger_disconnect = true;
                }
                if ((_status2 & 0x08) != 0)
                {
                    this._after_restart = true;
                }
                if ((_status2 & 0x10) != 0)
                {
                    this._calibration_changed = true;
                }
                if ((_status2 & 0x20) != 0)
                {
                    this._firmware_req = true;
                }


            }


            if (!CRC7.isValidCRC(result, result[63]))
            {
                _valid = false;
            }
            if (_valid && duration < 10)
                _valid = false;

            if (!_valid)
                _eventNotes += "-Corrupted";




        }
        public void loadFromEvent(BattViewObjectEvent r)
        {
            this._eventID = r._eventID;
            this._chargeWS = r._chargeWS;
            this._inUseWS = r._inUseWS;
            this._chargeAS = r._chargeAS;
            this._inUseAS = r._inUseAS;
            this._startTime = r._startTime;
            this._original_start_time = r._original_start_time;
            this._duration = r._duration;
            this._maxTemperatureTimeStamp = r._maxTemperatureTimeStamp;
            this._maxTemperature = r._maxTemperature;
            this._minVoltage = r._minVoltage;
            this._startVoltage = r._startVoltage;
            this._endVoltageSeconds = r._endVoltageSeconds;
            this._eventMaxCurrent = r._eventMaxCurrent;
            this._eventAverageCurrent = r._eventAverageCurrent;
            this._startSOC = r._startSOC;
            this._endSOC = r._endSOC;
            this._eventFlags = r._eventFlags;
            this._status = r._status;
            this._valid = r._valid;
            this._eventType = r._eventType;
            this._eventTypeID = r._eventTypeID;
            this._eventNotes = r._eventNotes;
            this._has_FI = r._has_FI;

            this._has_EQ = r._has_EQ;
            this._has_Electrolyte_sensor = r._has_Electrolyte_sensor;
            this._water_high = r._water_high;
            this._temperature_sensor_enabled = r._temperature_sensor_enabled;

            this._start_temperatureEnabled = r._start_temperatureEnabled;
            this._startTemperature = r._startTemperature;

            this._event_with_charger_at_end = r._event_with_charger_at_end;
            this._event_with_charger_at_start = r._event_with_charger_at_start;
            this._charger_disconnect = r._charger_disconnect;
            this._event_version = r._event_version;
        }
        public override string ToString()
        {
            return "ID: " + eventID.ToString() + "," + eventType;
        }
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            BattViewObjectEvent objAsPart = obj as BattViewObjectEvent;
            if (objAsPart == null) return false;
            else return Equals(objAsPart);
        }
        public int SortByIDAscending(uint id1, uint id2)
        {

            return id1.CompareTo(id2);
        }
        public int CompareTo(BattViewObjectEvent comparePart)
        {
            // A null value means that this object is greater.
            if (comparePart == null)
                return 1;

            else
                return this.eventID.CompareTo(comparePart.eventID);
        }
        public override int GetHashCode()
        {
            return eventID.GetHashCode();
        }
        public bool Equals(BattViewObjectEvent other)
        {
            if (other == null) return false;
            return (this.eventID.Equals(other.eventID));
        }
    }
}
