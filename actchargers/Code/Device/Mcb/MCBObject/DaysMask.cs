namespace actchargers
{
    public class DaysMask
    {
        #region Days

        private bool _Sunday;
        public bool Sunday
        {
            get
            {
                lock (myLock)
                {
                    return _Sunday;
                }
            }
            set
            {
                lock (myLock)
                {
                    _Sunday = value;
                }
            }
        }
        private bool _Monday;
        public bool Monday
        {
            get
            {
                lock (myLock)
                {
                    return _Monday;
                }
            }
            set
            {
                lock (myLock)
                {
                    _Monday = value;
                }
            }
        }
        private bool _Tuesday;
        public bool Tuesday
        {
            get
            {
                lock (myLock)
                {
                    return _Tuesday;
                }
            }
            set
            {
                lock (myLock)
                {
                    _Tuesday = value;
                }
            }
        }
        private bool _Wednesday;
        public bool Wednesday
        {
            get
            {
                lock (myLock)
                {
                    return _Wednesday;
                }
            }
            set
            {
                lock (myLock)
                {
                    _Wednesday = value;
                }
            }
        }
        private bool _Thursday;
        public bool Thursday
        {
            get
            {
                lock (myLock)
                {
                    return _Thursday;
                }
            }
            set
            {
                lock (myLock)
                {
                    _Thursday = value;
                }
            }
        }
        public bool _Friday;
        public bool Friday
        {
            get
            {
                lock (myLock)
                {
                    return _Friday;
                }
            }
            set
            {
                lock (myLock)
                {
                    _Friday = value;
                }
            }
        }
        private bool _Saturday;
        public bool Saturday
        {
            get
            {
                lock (myLock)
                {
                    return _Saturday;
                }
            }
            set
            {
                lock (myLock)
                {
                    _Saturday = value;
                }
            }
        }

        #endregion

        internal byte SerializeMe()
        {
            byte result = 0;

            lock (myLock)
            {
                if (_Sunday)
                    result |= 0x01;
                if (_Monday)
                    result |= 0x02;
                if (_Tuesday)
                    result |= 0x04;
                if (_Wednesday)
                    result |= 0x08;
                if (_Thursday)
                    result |= 0x10;
                if (_Friday)
                    result |= 0x20;
                if (_Saturday)
                    result |= 0x40;
            }
            return result;
        }
        internal void DeSerializeMe(byte mask)
        {
            lock (myLock)
            {
                _Sunday = (mask & 0x01) != 0;
                _Monday = (mask & 0x02) != 0;
                _Tuesday = (mask & 0x04) != 0;
                _Wednesday = (mask & 0x08) != 0;
                _Thursday = (mask & 0x10) != 0;
                _Friday = (mask & 0x20) != 0;
                _Saturday = (mask & 0x40) != 0;
            }
        }
        internal void CopyToMe(DaysMask other)
        {
            lock (myLock)
            {
                this._Sunday = other.Sunday;
                this._Monday = other.Monday;
                this._Tuesday = other.Tuesday;
                this._Wednesday = other.Wednesday;
                this._Thursday = other.Thursday;
                this._Friday = other.Friday;
                this._Saturday = other.Saturday;
            }
        }
        object myLock;
        internal DaysMask()
        {
            myLock = new object();
            _Sunday = false;
            _Monday = false;
            _Tuesday = false;
            _Wednesday = false;
            _Thursday = false;
            _Friday = false;
            _Saturday = false;
        }
    }
}
