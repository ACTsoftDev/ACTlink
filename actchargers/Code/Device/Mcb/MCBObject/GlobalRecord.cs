using System;
using Newtonsoft.Json;

namespace actchargers
{
    public class GlobalRecord
    {
        #region variables

        [JsonIgnore]
        private UInt32 _AHR;
        [JsonProperty]
        public UInt32 AHR
        {
            get
            {
                lock (myLock)
                {

                    return _AHR;
                }
            }
        }

        [JsonIgnore]
        private UInt32 _KWHr;
        [JsonProperty]
        public UInt32 KWHR
        {
            get
            {
                lock (myLock)
                {

                    return _KWHr;
                }
            }
        }

        [JsonIgnore]
        private UInt32 _TotalChargeTime;
        [JsonProperty]
        public UInt32 totalChargeSeconds
        {
            get
            {
                lock (myLock)
                {

                    return _TotalChargeTime;
                }
            }
        }

        [JsonIgnore]
        private UInt32 _CycleHistoryCounter;
        [JsonProperty]
        public UInt32 chargeCycles
        {
            get
            {
                lock (myLock)
                {

                    return _CycleHistoryCounter;
                }
            }
        }

        [JsonIgnore]
        private UInt32 _totalPMfaults;
        [JsonProperty]
        public UInt32 PMfaults
        {
            get
            {
                lock (myLock)
                {

                    return _totalPMfaults;
                }
            }
        }

        [JsonIgnore]
        private UInt32 _totalPowerSnapShots;
        [JsonIgnore]
        public UInt32 totalPowerSnapShots
        {
            get
            {
                lock (myLock)
                {

                    return _totalPowerSnapShots;
                }
            }
        }
        [JsonIgnore]
        private UInt32 _totalDebugRecords;
        [JsonIgnore]
        public UInt32 totalDebugRecords
        {
            get
            {
                lock (myLock)
                {

                    return _totalDebugRecords;
                }
            }
        }

        #endregion

        #region private

        [JsonIgnore]
        private const int dataSize = 62;
        [JsonIgnore]
        private object myLock;

        #endregion

        #region utilities

        internal CommunicationResult DeSerilaizeMe(byte[] result)
        {
            lock (myLock)
            {
                _AHR = BitConverter.ToUInt32(result, 0);
                _KWHr = BitConverter.ToUInt32(result, 4);
                _TotalChargeTime = BitConverter.ToUInt32(result, 8);
                _CycleHistoryCounter = BitConverter.ToUInt32(result, 12);
                _totalPMfaults = BitConverter.ToUInt32(result, 16);
                _totalPowerSnapShots = BitConverter.ToUInt32(result, 22);
                _totalDebugRecords = BitConverter.ToUInt32(result, 26);
            }
            //unused bytes
            return CommunicationResult.OK;
        }

        internal void ResetMe()
        {
            lock (myLock)
            {
                _AHR = 0;
                _KWHr = 0;
                _TotalChargeTime = 0;
                _TotalChargeTime = 0;
                _CycleHistoryCounter = 0;
                _totalPMfaults = 0;
                _totalPowerSnapShots = 0;
                _totalDebugRecords = 0;
            }
        }

        public GlobalRecord()
        {
            myLock = new object();
            _AHR = 0;
            _KWHr = 0;
            _TotalChargeTime = 0;
            _TotalChargeTime = 0;
            _CycleHistoryCounter = 0;
            _totalPMfaults = 0;
            _totalPowerSnapShots = 0;
            _totalDebugRecords = 0;
        }

        #endregion

        public string TOJSON()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
