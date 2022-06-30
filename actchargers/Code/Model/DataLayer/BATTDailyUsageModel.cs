using System;
namespace actchargers
{
    public class BATTDailyUsageModel
    {
        private string _date=string.Empty;
        public string date
        {
            get
            {
                return _date;
            }
            set
            {
                _date = value;
            }
        }


        private string _is_working_day=string.Empty;
        public string is_working_day
        {
            get
            {
                return _is_working_day;
            }

            set
            {
                _is_working_day = value;
            }
        }



        private string _total_charge_events=string.Empty;
        public string total_charge_events
        {
            get
            {
                return _total_charge_events;

            }

            set
            {
                _total_charge_events = value;

            }
        }



        private string _charge_as=string.Empty;
        public string charge_as
        {
            get
            {
                return _charge_as;

            }

            set
            {
                _charge_as = value;

            }
        }



        private string _charge_ws=string.Empty;
        public string charge_ws
        {
            get
            {
                return _charge_ws;

            }

            set
            {
                _charge_ws = value;

            }
        }



        private string _total_inuse_events=string.Empty;
        public string total_inuse_events
        {
            get
            {
                return _total_inuse_events;

            }

            set
            {
                _total_inuse_events = value;

            }
        }



        private string _inuse_as=string.Empty;
        public string inuse_as
        {
            get
            {
                return _inuse_as;

            }

            set
            {
                _inuse_as = value;

            }
        }


        private string _inuse_ws=string.Empty;
        public string inuse_ws
        {
            get
            {
                return _inuse_ws;

            }

            set
            {
                _inuse_ws = value;

            }
        }



        private string _inuse_duration=string.Empty;
        public string inuse_duration
        {
            get
            {
                return _inuse_duration;

            }

            set
            {
                _inuse_duration = value;

            }
        }



        private string _charge_duration=string.Empty;
        public string charge_duration
        {
            get
            {
                return _charge_duration;

            }

            set
            {
                _charge_duration = value;

            }
        }



        private string _total_idle_events=string.Empty;
        public string total_idle_events
        {
            get
            {
                return _total_idle_events;

            }

            set
            {
                _total_idle_events = value;

            }
        }



        private string _idle_duration=string.Empty;
        public string idle_duration
        {
            get
            {
                return _idle_duration;

            }

            set
            {
                _idle_duration = value;

            }
        }



        private string _expected_eq=string.Empty;
        public string expected_eq
        {
            get
            {
                return _expected_eq;

            }

            set
            {
                _expected_eq = value;

            }
        }



        private string _expected_fi=string.Empty;
        public string expected_fi
        {
            get
            {
                return _expected_fi;

            }

            set
            {
                _expected_fi = value;

            }
        }



        private string _ahrcapacity=string.Empty;
        public string ahrcapacity
        {
            get
            {
                return _ahrcapacity;

            }

            set
            {
                _ahrcapacity = value;

            }
        }



    }
}
