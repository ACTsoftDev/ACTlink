using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace actchargers
{
    public class toUnixTimeStamp : Newtonsoft.Json.Converters.DateTimeConverterBase
    {
        private static readonly DateTime _epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteRawValue(((DateTime)value - _epoch).TotalSeconds.ToString());
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.Value == null) { return null; }
            return _epoch.AddSeconds((long)reader.Value);
        }
    }
    class BATTViewEvent : IEquatable<BATTViewEvent>, IComparable<BATTViewEvent>
    {
        public string toJSON()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }

        public UInt32 event_id;
        public UInt32 charge_ws;
        public UInt32 inuse_ws;
        public UInt32 charge_as;
        public UInt32 inuse_as;
        [Newtonsoft.Json.JsonConverter(typeof(toUnixTimeStamp))]
        public DateTime start_time;

        public UInt32 original_start_time;
        public UInt32 duration;
        public UInt32 charger_id;
        [Newtonsoft.Json.JsonConverter(typeof(toUnixTimeStamp))]
        public DateTime max_temperature_time;
        public float max_temperature;
        public float min_voltage;
        public float start_voltage;
        public float end_voltage;
        public float event_max_current;
        public float event_average_current;
        public byte start_soc;
        public byte end_soc;
        public bool is_ok_record;
        public byte event_name;
        public bool has_finish;
        public bool has_eq;
        public bool water_sensor_enabled;
        public bool water_ok;
        public bool temperature_sensor_enabled;
        public float start_temperature;
        public bool start_temperature_enabled;
        public string event_notes;


        public bool ShouldSerializeevent_version()
        {
            if (this.event_version >= 1)
                return true;
            else
                return false;
        }
        public byte event_version;

        public bool ShouldSerializeevent_with_charger_at_start()
        {
            if (this.event_version >= 1 && event_id != 1)
                return true;
            else
                return false;
        }
        public bool event_with_charger_at_start;
        public bool ShouldSerializeevent_with_charger_at_end()
        {
            if (this.event_version >= 1 && event_id != 1)
                return true;
            else
                return false;
        }
        public bool event_with_charger_at_end;


        public bool ShouldSerializecharge_with_finish_start()
        {
            if (this.event_version >= 1 && event_id == 1)
                return true;
            else
                return false;
        }
        public bool charge_with_finish_start;

        public bool ShouldSerializecharge_with_eq_start()
        {
            if (this.event_version >= 1 && event_id == 1)
                return true;
            else
                return false;
        }
        public bool charge_with_eq_start;

        public bool ShouldSerializecharge_cycle_type()
        {
            if (this.event_version >= 1 && event_id == 1)
                return true;
            else
                return false;
        }
        public byte charge_cycle_type;

        public bool ShouldSerializecharge_split_req()
        {
            if (this.event_version >= 1 && event_id == 1)
                return true;
            else
                return false;
        }
        public bool charge_split_req;


        public bool ShouldSerializesoc_set()
        {
            if (this.event_version >= 1 && event_id == 2)
                return true;
            else
                return false;
        }
        public bool soc_set;
        public bool ShouldSerializefi_done_reason()
        {
            if (this.event_version >= 1 && event_id == 1)
                return true;
            else
                return false;
        }
        public byte fi_done_reason;


        public bool ShouldSerializeafter_restart()
        {
            if (this.event_version >= 1)
                return true;
            else
                return false;
        }
        public bool after_restart;

        public bool ShouldSerializefirmware_req()
        {
            if (this.event_version >= 1)
                return true;
            else
                return false;
        }
        public bool firmware_req;

        public bool ShouldSerializecharger_disconnect()
        {
            if (this.event_version >= 1)
                return true;
            else
                return false;
        }
        public bool charger_disconnect;


        public bool ShouldSerializecalibration_changed()
        {
            if (this.event_version >= 1)
                return true;
            else
                return false;
        }
        public bool calibration_changed;


        public bool ShouldSerializeexitStatus()
        {
            if (this.event_version >= 1)
                return true;
            else
                return false;
        }
        public byte exitStatus;




        public BATTViewEvent(BattViewObjectEvent ev, bool isFahrenheit)
        {
            this.event_id = ev.eventID;
            this.charge_ws = ev.chargeWS;
            this.inuse_ws = ev.inUseWS;
            this.charge_as = ev.chargeAS;
            this.inuse_as = ev.inUseAS;
            this.start_time = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(ev.startTime);
            this.duration = ev.duration;
            this.charger_id = ev.chargerID;
            this.max_temperature_time = start_time.AddSeconds(ev.maxTemperatureTimeStamp);
            this.max_temperature = (float)TemperatureManager.GetCorrectTemperature(ev.maxTemperature / 10.0f, isFahrenheit);
            this.min_voltage = ev.minVoltage / 100.0f;
            this.start_voltage = ev.startVoltage / 100.0f;
            this.end_voltage = ev.endVoltageSeconds / 100.0f;
            this.event_max_current = ev.eventMaxCurrent / 10.0f;
            this.event_average_current = ev.eventAverageCurrent / 10.0f;
            this.start_soc = ev.startSOC;
            this.end_soc = ev.endSOC;
            this.is_ok_record = ev.valid;
            this.event_name = ev.eventTypeID;
            this.has_finish = ev.has_FI;
            this.has_eq = ev.has_EQ;
            this.water_sensor_enabled = ev.has_Electrolyte_sensor;
            this.water_ok = ev.water_high;
            this.event_notes = ev.eventNotes;
            this.original_start_time = ev.original_start_time;
            this.temperature_sensor_enabled = ev.temperature_sensor_enabled;
            this.start_temperature_enabled = ev.start_temperatureEnabled;
            this.start_temperature = ev.startTemperature / 10.0f;
            //Since version 2.09 (event version 1)
            this.event_version = ev.event_version;
            this.event_with_charger_at_start = ev.event_with_charger_at_start;
            this.event_with_charger_at_end = ev.event_with_charger_at_end;

            this.charge_with_finish_start = ev.charge_with_FI_Start;
            this.charge_with_eq_start = ev.charge_with_EQ_Start;
            this.charge_cycle_type = (byte)ev.chargeCycleType;
            this.charge_split_req = ev.charge_split_req;
            this.soc_set = ev.soc_set;
            this.fi_done_reason = (byte)ev.fiDoneReason;
            this.after_restart = ev.after_restart;
            this.calibration_changed = ev.calibration_changed;
            this.firmware_req = ev.firmware_req;
            this.exitStatus = ev.exitStatus;
            this.charger_disconnect = ev.charger_disconnect;

        }
        public override string ToString()
        {
            return "ID: " + event_id.ToString() + "," + event_name;
        }
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            BATTViewEvent objAsPart = obj as BATTViewEvent;
            if (objAsPart == null) return false;
            else return Equals(objAsPart);
        }
        public int SortByIDAscending(uint id1, int id2)
        {
            return id1.CompareTo(Convert.ToUInt32(id2));
        }
        public int CompareTo(BATTViewEvent comparePart)
        {
            // A null value means that this object is greater.
            if (comparePart == null)
                return 1;

            else
                return this.event_id.CompareTo(comparePart.event_id);
        }
        public override int GetHashCode()
        {
            return event_id.GetHashCode();
        }
        public bool Equals(BATTViewEvent other)
        {
            if (other == null) return false;
            return (this.event_id.Equals(other.event_id));
        }
    }
    public class BATTViewDailyDetails
    {
        public string toJSON()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }
        // public List<BATTViewEvent> events;
        public BATTViewDailyDetails(DateTime eventTime, BattViewConfig config)
        {
            //events = new List<BATTViewEvent>();
            date = eventTime.Date;
            is_complete = false;
            if (eventTime.Date.DayOfWeek == DayOfWeek.Sunday || eventTime.Date.DayOfWeek == DayOfWeek.Saturday)
                is_working_day = false;
            else
                is_working_day = true;

            total_charge_events = 0;
            charge_events_as = 0;
            charge_as = 0;
            charge_events_ws = 0;
            charge_ws = 0;
            charge_duration = 0;
            total_inuse_events = 0;
            inuse_events_as = 0;
            inuse_as = 0;
            inuse_events_ws = 0;
            inuse_ws = 0;
            inuse_duration = 0;
            total_idle_events = 0;
            idle_duration = 0;
            count_of_eqs = 0;
            count_of_eqs_waterok = 0;
            count_of_eqs_water_not_ok = 0;
            count_of_finishes = 0;
            min_soc = 100;
            max_temperature_value = 25;
            max_temperature_exceeded = 0;
            temperature_sensor_enabled = false;
            deep_discharge_value = config.nominalvoltage;
            deep_discharge_exceeded = 0;
            inuse_max_current = 0;
            charge_max_current = 0;
            water_level_low = false;
            charge_oppurtinity = 0;
            charge_oppurtinity_duration = 0;
            if (eventTime.Date.DayOfWeek == DayOfWeek.Sunday && (config.EQdaysMask & 0x01) != 0 ||
                eventTime.Date.DayOfWeek == DayOfWeek.Monday && (config.EQdaysMask & 0x02) != 0 ||
                eventTime.Date.DayOfWeek == DayOfWeek.Tuesday && (config.EQdaysMask & 0x04) != 0 ||
                eventTime.Date.DayOfWeek == DayOfWeek.Wednesday && (config.EQdaysMask & 0x08) != 0 ||
                eventTime.Date.DayOfWeek == DayOfWeek.Thursday && (config.EQdaysMask & 0x10) != 0 ||
                eventTime.Date.DayOfWeek == DayOfWeek.Friday && (config.EQdaysMask & 0x20) != 0 ||
                eventTime.Date.DayOfWeek == DayOfWeek.Monday && (config.EQdaysMask & 0x40) != 0
                )
            {
                expected_eq = true;
            }
            else
            {
                expected_eq = false;
            }
            if (eventTime.Date.DayOfWeek == DayOfWeek.Sunday && (config.FIdaysMask & 0x01) != 0 ||
                eventTime.Date.DayOfWeek == DayOfWeek.Monday && (config.FIdaysMask & 0x02) != 0 ||
                eventTime.Date.DayOfWeek == DayOfWeek.Tuesday && (config.FIdaysMask & 0x04) != 0 ||
                eventTime.Date.DayOfWeek == DayOfWeek.Wednesday && (config.FIdaysMask & 0x08) != 0 ||
                eventTime.Date.DayOfWeek == DayOfWeek.Thursday && (config.FIdaysMask & 0x10) != 0 ||
                eventTime.Date.DayOfWeek == DayOfWeek.Friday && (config.FIdaysMask & 0x20) != 0 ||
                eventTime.Date.DayOfWeek == DayOfWeek.Monday && (config.FIdaysMask & 0x40) != 0
                )
            {
                expected_fi = true;
            }
            else
            {
                expected_fi = false;
            }
            sensor_issue = false;
        }
        [Newtonsoft.Json.JsonConverter(typeof(toUnixTimeStamp))]
        public DateTime date;
        public bool is_complete;//  fill into isedge column a value indicates if all sequential records are there, (a record started from mid night and a record ends at next day midnight) , in case of corrupted day records , check if we have right sequence before that day and after
        public bool is_working_day;//   at the time record added, was this day is a working day (based on bfd days)
        public UInt16 total_charge_events;//    count of total charge cycles in that day
        public UInt32 charge_events_as;//   the SUM AS of charge AS from charge events ONLY;
        public UInt32 charge_as;//  the SUM AS of charges AS from all events;
        public UInt32 charge_events_ws;//   the SUM WS of charge WS from charge events ONLY
        public UInt32 charge_ws;//  the SUM WS of charges WS from all events
        public UInt32 charge_duration;//    the SUM of duration of charge events
        public UInt16 total_inuse_events;// count of total inuse cycles in that day
        public UInt32 inuse_events_as;//    the SUM AS of inuse AS from inuse events ONLY;
        public UInt32 inuse_as;//   the SUM AS of inuse AS from all events;
        public UInt32 inuse_events_ws;//    the SUM WS of inuse WS from charge events ONLY
        public UInt32 inuse_ws;//   the SUM WS of inuse WS from all events
        public UInt32 inuse_duration;// the SUM of duration of inuse events
        public UInt16 total_idle_events;//  count of total Idle cycles in that day
        public UInt32 idle_duration;//  the SUM of duration of idle events
        public UInt16 count_of_eqs;//   count of charge cycles that day with Equalize Profile
        public UInt16 count_of_eqs_waterok;//   count of charge cycles that day with Equalize Profile and electrolye sensor enable and water is OK
        public UInt16 count_of_eqs_water_not_ok;//  count of charge cycles that day with Equalize Profile and electrolye sensor enable and water is Low
        public UInt16 count_of_finishes;//  count of charge cycles that day with Finish Profile
        public byte min_soc;//  minumum start/end SOC of that day events
        public float max_temperature_value;//   maximum temperature of all events that day
        public bool temperature_sensor_enabled;
        public UInt16 max_temperature_exceeded;//   count if (at any event if max temperature > batteryHighTemperature value )
        public float deep_discharge_value;//    minumum of min voltage
        public UInt32 deep_discharge_exceeded;//    count of (at inuse event end voltage if the voltage were below 1.75 VPC )
        public float inuse_max_current;//   max current of inuse events (0 if no inuse exists)
        public float charge_max_current;//  max current of charge events (0 if no inuse exists)
        public bool water_level_low;//  set to low if we have events with electrolye sensor enable and all of them have water low
        public UInt16 charge_oppurtinity;// count (of idle events which is longer than 15 mins and came after discharge event)
        public UInt32 charge_oppurtinity_duration;//    SUM(idle events durations which is longer than 15 mins and came after discharge duration)
        public bool expected_eq;//  based on EQ schudling were we expecting EQ (true,false)
        public bool expected_fi;//  based on FI schudling were we expecting FI (true,false)
        public bool sensor_issue;// true if we have a charge event max current > 40% of bfd ahrcapacity
    }
    class BATTViewSummaryAndExceptions
    {
        public UInt32 charge_duration;
        public UInt32 inuse_duration;
        public UInt32 idle_duration;
        public byte charge_duration_percent;
        public byte inuse_duration_percent;
        public byte idle_duration_percent;

        public UInt32 charge_as;
        public UInt32 inuse_as;
        public byte charge_as_percent;
        public byte inuse_as_percent;

        public bool inUseExists;
        public UInt32 minInUseDuration;
        public UInt32 avgInUseDuration;
        public UInt32 maxInUseDuration;
        public UInt32 minInUseAS;
        public UInt32 avgInUseAS;
        public UInt32 maxInUseAS;

        public bool idle_exists;
        public UInt32 minChargeOppurtiniyDuration;
        public UInt32 avgChargeOppurtiniyDuration;
        public UInt32 maxChargeOppurtiniyDuration;

        public bool hourlyASUsageExists;
        public UInt32 minHourlyAHUsage;
        public UInt32 avgHourlyAHUsage;
        public UInt32 maxHourlyAHUsage;

        public bool missedEQ;
        public bool missedFI;
        public UInt32 totalOverTemperature;
        public bool sensorIssue;
        public bool deepDischarged;

        public UInt32 total_charge_events;
        public float maxTemperatureValue;
        public byte minSOC;
        public UInt32 totalEQ;
        public UInt32 totalEQWaterOK;

        public BATTViewSummaryAndExceptions(Dictionary<DateTime, BATTViewDailyDetails> days)
        {
            charge_duration = 0;
            inuse_duration = 0;
            idle_duration = 0;
            charge_as = 0;
            inuse_as = 0;

            totalEQ = 0;
            totalEQWaterOK = 0;
            total_charge_events = 0;

            missedEQ = false;
            inUseExists = false;
            idle_exists = false;
            hourlyASUsageExists = false;
            missedFI = false;
            sensorIssue = false;
            deepDischarged = false;
            minInUseDuration = UInt32.MaxValue;
            maxInUseDuration = UInt32.MinValue;
            maxInUseAS = UInt32.MinValue;
            minChargeOppurtiniyDuration = UInt32.MaxValue;
            maxChargeOppurtiniyDuration = UInt32.MinValue;
            minHourlyAHUsage = UInt32.MaxValue;
            maxHourlyAHUsage = UInt32.MinValue;
            maxTemperatureValue = UInt32.MinValue;
            minSOC = byte.MinValue;
            UInt32 totalChargeOpp = 0;
            UInt32 totalHourlyASUsage = 0;

            foreach (BATTViewDailyDetails day in days.Values)
            {
                if (day.deep_discharge_exceeded > 0)
                    deepDischarged = true;
                totalEQ += day.count_of_eqs;
                totalEQWaterOK = day.count_of_eqs_waterok;
                if (minSOC > day.min_soc)
                    minSOC = day.min_soc;

                if (day.temperature_sensor_enabled && maxTemperatureValue < day.max_temperature_value)
                    maxTemperatureValue = day.max_temperature_value;
                if (day.sensor_issue)
                    sensorIssue = true;
                totalOverTemperature += day.max_temperature_exceeded;
                if (day.expected_eq && day.count_of_eqs == 0)
                    missedEQ = true;
                if (day.expected_fi && day.count_of_finishes == 0)
                    missedFI = true;
                total_charge_events += day.total_charge_events;
                if (minInUseDuration > day.inuse_duration)
                    minInUseDuration = day.inuse_duration;
                if (maxInUseDuration < day.inuse_duration)
                    maxInUseDuration = day.inuse_duration;
                charge_duration += day.charge_duration;
                inuse_duration += day.inuse_duration;
                idle_duration += day.idle_duration;
                charge_as += day.charge_as;
                inuse_as += day.inuse_as;
                if (maxInUseAS < day.inuse_as)
                    maxInUseAS = day.inuse_as;
                if (minInUseAS > day.inuse_as)
                    minInUseAS = day.inuse_as;
                if (minChargeOppurtiniyDuration > day.charge_oppurtinity_duration)
                    minChargeOppurtiniyDuration = day.charge_oppurtinity_duration;
                if (maxChargeOppurtiniyDuration < day.charge_oppurtinity_duration)
                    maxChargeOppurtiniyDuration = day.charge_oppurtinity_duration;
                totalChargeOpp += day.charge_oppurtinity_duration;
                totalHourlyASUsage += day.inuse_events_as;
                if (day.inuse_duration > 0)
                {
                    if (minHourlyAHUsage > day.inuse_events_as / day.inuse_duration)
                        minHourlyAHUsage = day.inuse_events_as / day.inuse_duration;
                    if (maxHourlyAHUsage < day.inuse_events_as / day.inuse_duration)
                        maxHourlyAHUsage = day.inuse_events_as / day.inuse_duration;
                }
            }


            if (days.Count > 0 && inuse_duration > 0)
            {
                inUseExists = true;
                avgInUseDuration = (UInt32)(inuse_duration / days.Count);
                avgInUseAS = (UInt32)(inuse_as / days.Count);
            }
            if (inuse_duration > 0 && totalHourlyASUsage > 0)
            {
                inUseExists = true;
                avgHourlyAHUsage = (UInt32)(totalHourlyASUsage / inuse_duration);
            }
            if (days.Count > 0 && totalChargeOpp > 0)
            {
                idle_exists = true;
                avgChargeOppurtiniyDuration = (UInt32)(totalChargeOpp / days.Count);
            }

            if (inuse_duration + idle_duration + charge_duration > 0)
            {
                charge_duration_percent = (byte)(100 * charge_duration / (inuse_duration + idle_duration + charge_duration));
                inuse_duration_percent = (byte)(100 * inuse_duration / (inuse_duration + idle_duration + charge_duration));
                idle_duration_percent = (byte)(100 - charge_duration_percent - inuse_duration_percent);
            }
            else
            {
                idle_duration_percent = 0;
                inuse_duration_percent = 0;
                charge_duration_percent = 0;
            }
            if (inuse_as + charge_as > 0)
            {
                charge_as_percent = (byte)(100 * charge_as / (inuse_as + charge_as));
                inuse_as_percent = (byte)(100 - charge_as_percent);
            }
            else
            {
                charge_as_percent = 0;
                inuse_as_percent = 0;
            }


        }
    }
    class BATTViewReporting
    {
        public List<BATTViewEvent> events;
        public Dictionary<DateTime, BATTViewDailyDetails> days;
        public BATTViewSummaryAndExceptions summary;


        //public 
        public BATTViewReporting(List<BattViewObjectEvent> evs, DateTime startDate, DateTime endDate, BattViewConfig config)
        {

            float numberOfcells = config.nominalvoltage / 2;

            startDate = startDate.Date;
            endDate = endDate.Date;
            events = new List<BATTViewEvent>();
            foreach (BattViewObjectEvent ev in evs)
            {
                //if (!ControlObject.isDebugMaster && !ev.valid)
                //    continue;
                BATTViewEvent bev = new BATTViewEvent(ev, Convert.ToBoolean(config.temperatureFormat));
                if (startDate > bev.start_time)
                    continue;
                if (endDate.AddDays(1).AddSeconds(-1) < bev.start_time && bev.is_ok_record)
                    break;
                events.Add(bev);
            }
            events.Sort();
            //get daily details
            days = new Dictionary<DateTime, BATTViewDailyDetails>();

            for (int i = 0; i <= (endDate - startDate).TotalDays; i++)
            {
                BATTViewDailyDetails day = new BATTViewDailyDetails(startDate.AddDays((double)i), config);
                days.Add(day.date, day);
            }
            //bool startDay = false;
            bool waterisHigh = false;
            int eventsIndex = -1;
            bool startEventOK = false;
            bool notFirstEventEver = false;
            foreach (BATTViewEvent ev in events)
            {
                eventsIndex++;
                if (!ev.is_ok_record)
                    continue;
                BATTViewDailyDetails day = days[ev.start_time.Date];
                //day.events.Add(ev);
                if (day.total_charge_events == 0 && day.total_inuse_events == 0 && day.total_idle_events == 0)
                {
                    if (notFirstEventEver)
                    {
                        if (startEventOK && ev.start_time.AddSeconds(ev.duration) >= day.date.AddDays(1).AddSeconds(-1))
                        {
                            day.is_complete = true;
                        }
                    }
                    if (ev.start_time == day.date)
                        startEventOK = true;
                    else
                        startEventOK = false;
                    waterisHigh = false;
                    day.max_temperature_value = float.MinValue;
                    day.deep_discharge_value = float.MaxValue;
                }
                notFirstEventEver = true;
                //Lumb
                switch (ev.event_name)
                {
                    case 1:
                        {
                            day.total_charge_events++;
                            day.charge_events_as += ev.charge_as;
                            day.charge_events_ws += ev.charge_ws;
                            day.charge_duration += ev.duration;
                            if (day.charge_max_current < ev.event_max_current)
                                day.charge_max_current = ev.event_max_current;
                            if (ev.has_eq)
                                day.count_of_eqs++;
                            if (ev.water_sensor_enabled)
                            {
                                if (ev.water_ok)
                                    day.count_of_eqs_waterok++;
                                else
                                    day.count_of_eqs_water_not_ok++;
                            }
                            if (ev.has_finish)
                                day.count_of_finishes++;
                            if (ev.event_max_current > 0.4f * config.ahrcapacity)
                                day.sensor_issue = true;
                        }
                        break;
                    case 2:
                        {
                            day.total_idle_events++;
                            day.idle_duration += ev.duration;
                            if (ev.duration > 900 && eventsIndex != 0)
                            {
                                int temp = eventsIndex;
                                while (temp > 0)
                                {
                                    temp--;
                                    if (!ev.is_ok_record)
                                        continue;
                                    if (events[temp].event_name == 3 && events[temp].start_time.AddSeconds(events[temp].duration) == ev.start_time)
                                    {
                                        day.charge_oppurtinity++;
                                        day.charge_oppurtinity_duration += ev.duration;
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }

                            }
                        }
                        break;
                    case 3:
                        {
                            day.total_inuse_events++;
                            day.inuse_events_as += ev.inuse_as;
                            day.inuse_events_ws += ev.inuse_ws;
                            day.inuse_duration += ev.duration;
                            if (1.0f * ev.end_voltage / numberOfcells <= 1.75f)
                                day.deep_discharge_exceeded++;
                            if (day.inuse_max_current < ev.event_max_current)
                                day.inuse_max_current = ev.event_max_current;

                        }
                        break;
                }
                day.charge_as += ev.charge_as;
                day.charge_ws += ev.charge_ws;
                day.inuse_as += ev.inuse_as;
                day.inuse_ws += ev.inuse_ws;

                if (day.min_soc < ev.start_soc)
                    day.min_soc = ev.start_soc;
                if (day.min_soc < ev.end_soc)
                    day.min_soc = ev.end_soc;
                if (ev.temperature_sensor_enabled)
                {
                    if (day.max_temperature_value < ev.max_temperature)
                        day.max_temperature_value = ev.max_temperature;
                    if (ev.max_temperature >= config.batteryHighTemperature / 10.0f)
                        day.max_temperature_exceeded++;
                    day.temperature_sensor_enabled = true;
                }

                if (day.deep_discharge_value > ev.end_voltage)
                    day.deep_discharge_value = ev.end_voltage;

                if (ev.water_sensor_enabled)
                {
                    if (ev.water_ok)
                    {
                        waterisHigh = true;
                        day.water_level_low = false;
                    }
                    else
                    if (!waterisHigh)
                        day.water_level_low = true;
                }

            }
            foreach (BATTViewDailyDetails day in days.Values)
            {
                if (day.max_temperature_value == float.MinValue)
                    day.max_temperature_value = 25;
            }
            summary = new BATTViewSummaryAndExceptions(days);
        }
    }
}
