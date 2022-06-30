using System;
using System.Collections.Generic;
using System.Linq;

namespace actchargers
{
    public static class DownloadUpload
    {
       public static string getCompositeKye(bool isMCB, uint id, uint studyId)
        {
            return (isMCB ? "CHRG" : "BATT") + id.ToString() + "_" + studyId.ToString();
        }
        public static object[] desolveCompositteKey(string k)
        {
            bool isMCB;
            uint id;
            uint studyId;
            if (k.Substring(0, 4) == "BATT")
                isMCB = false;
            else
                isMCB = true;
            string[] idS = k.Substring(4, k.Length - 4).Split(new char[] { '_' });

            if (!uint.TryParse(idS[0], out id))
            {
                id = 0;
            }
            if (!uint.TryParse(idS[1], out studyId))
            {
                studyId = 0;
            }
            return new object[] { isMCB, id, studyId };
        }
    }

    internal class GenericUploadControlStats
    {
        List<string> _notAuthorizedIds;
        Dictionary<string, string> _failedToUpload;
        List<string> _challanged;
        public void addNotAuthorized(string k)
        {
            lock (myLock) _notAuthorizedIds.Add(k);

        }
        public void addChallenged(string k)
        {
            lock (myLock)
            {
                if (!_challanged.Exists(x => x == k))
                    _challanged.Add(k);
            }

        }
        public void removeChallenged(string k)
        {
            lock (myLock)
            {
                if (_challanged.Exists(x => x == k))
                    _challanged.Remove(k);
            }

        }
        public List<string> getChallangedList()
        {
            List<string> temp = new List<string>();
            lock (myLock)
            {
                foreach (string k in _challanged)
                {
                    temp.Add(k);
                }
            }
            return temp;

        }
        public void addFailed(string id, string extraInfo)
        {
            lock (myLock) _failedToUpload.Add(id, extraInfo);

        }
        public Dictionary<string, string> getFailedList()
        {
            Dictionary<string, string> temp = new Dictionary<string, string>();
            lock (myLock)
            {
                foreach (string id in _failedToUpload.Keys)
                {
                    temp.Add(id, _failedToUpload[id]);
                }
            }
            return temp;

        }
        public List<string> getNotAuthorizedList()
        {
            List<string> temp = new List<string>();
            lock (myLock)
            {
                foreach (string id in _notAuthorizedIds)
                {
                    temp.Add(id);
                }
            }
            return temp;

        }

        bool _lostTheInternet;
        public bool lostTheInternet
        {
            get
            {
                lock (myLock) return _lostTheInternet;
            }
            set
            {
                lock (myLock) _lostTheInternet = value;
            }
        }
        int _challangedState;
        public int challangedState
        {
            get
            {
                lock (myLock) return _challangedState;
            }
            set
            {
                lock (myLock) _challangedState = value;
            }
        }
        bool _actViewOff;
        public bool actViewOff
        {
            get
            {
                lock (myLock) return _actViewOff;
            }
            set
            {
                lock (myLock) _actViewOff = value;
            }
        }
        uint _totalUploadedPatches;
        public uint totalUploadedPatches
        {
            get
            {
                lock (myLock) return _totalUploadedPatches;
            }
            set
            {
                lock (myLock) _totalUploadedPatches = value;
            }
        }
        uint _totalUploadPatches;
        public uint totalUploadPatches
        {
            get
            {
                lock (myLock) return _totalUploadPatches;
            }
            set
            {
                lock (myLock) _totalUploadPatches = value;
            }
        }
        object myLock;

        public GenericUploadControlStats()
        {
            myLock = new object();
            _notAuthorizedIds = new List<string>();
            _failedToUpload = new Dictionary<string, string>();
            _challanged = new List<string>();
            _lostTheInternet = false;
            _actViewOff = false;
            _totalUploadedPatches = 0;
            _totalUploadPatches = 0;
            challangedState = 0;
        }

    }

    internal class downlloadUploadControlObject
    {
        object downloadUploadLock;
        public downlloadUploadControlObject()
        {
            downloadUploadLock = new object();
            allUploads = new GenericUploadControlStats();
            download_fromDevices_flag = false;
            upload_operations = false;
            download_stillCalculating = true;
            firmware_Update_flag = false;
            firmware_operation_busy = false;
            resetDoeanlodUI = false;
            resetFirmwareUI = false;



        }
        public GenericUploadControlStats allUploads;

        bool _download_operations;
        public bool download_fromDevices_flag
        {
            set { lock (downloadUploadLock) { _download_operations = value; } }
            get { lock (downloadUploadLock) { return _download_operations; } }
        }
        bool _download_operations_busy;
        public bool download_operation_busy
        {
            set { lock (downloadUploadLock) { _download_operations_busy = value; } }
            get { lock (downloadUploadLock) { return _download_operations_busy; } }
        }

        bool _download_updateUI_busy;
        public bool download_updateUI_busy
        {
            set { lock (downloadUploadLock) { _download_updateUI_busy = value; } }
            get { lock (downloadUploadLock) { return _download_updateUI_busy; } }
        }

        bool _download_stillCalculating;
        public bool download_stillCalculating
        {
            set { lock (downloadUploadLock) { _download_stillCalculating = value; } }
            get { lock (downloadUploadLock) { return _download_stillCalculating; } }
        }


        bool _upload_operations;
        public bool upload_operations
        {
            set { lock (downloadUploadLock) { _upload_operations = value; } }
            get { lock (downloadUploadLock) { return _upload_operations; } }
        }
        bool _batt_events_upload_stat_rsynchAgain;
        public bool batt_events_upload_stat_rsynchAgain
        {
            set { lock (downloadUploadLock) { _batt_events_upload_stat_rsynchAgain = value; } }
            get { lock (downloadUploadLock) { return _batt_events_upload_stat_rsynchAgain; } }
        }
        bool _uploadIsRunning;
        public bool uploadIsRunning
        {
            set { lock (downloadUploadLock) { _uploadIsRunning = value; } }
            get { lock (downloadUploadLock) { return _uploadIsRunning; } }
        }

        bool _firmware_Update_flags;
        public bool firmware_Update_flag
        {
            set { lock (downloadUploadLock) { _firmware_Update_flags = value; } }
            get { lock (downloadUploadLock) { return _firmware_Update_flags; } }
        }
        bool _firmware_operations_busy;
        public bool firmware_operation_busy
        {
            set { lock (downloadUploadLock) { _firmware_operations_busy = value; } }
            get { lock (downloadUploadLock) { return _firmware_operations_busy; } }
        }
        bool _firmware_updateUI_busy;
        public bool firmware_updateUI_busy
        {
            set { lock (downloadUploadLock) { _firmware_updateUI_busy = value; } }
            get { lock (downloadUploadLock) { return _firmware_updateUI_busy; } }
        }

        bool _resetDoeanlodUI;
        public bool resetDoeanlodUI
        {
            set { lock (downloadUploadLock) { _resetDoeanlodUI = value; } }
            get { lock (downloadUploadLock) { return _resetDoeanlodUI; } }
        }

        bool _resetFirmwareUI;
        public bool resetFirmwareUI
        {
            set { lock (downloadUploadLock) { _resetFirmwareUI = value; } }
            get { lock (downloadUploadLock) { return _resetFirmwareUI; } }
        }
    }

    class downloadOneDeviceStats
    {
        private object lockObj = new object();
        bool isMCB;
        string myName;
        uint id;
        uint startIDThisSession;
        uint maxIDSeen;
        uint downloadedID;

        uint startPMIDThisSession;
        uint maxPMIDSeen;
        uint downloadedPMID;

        bool configLoaded;
        bool actViewEnabled;
        bool _isRunning;
        public bool isRunning
        {
            get
            {
                lock (lockObj)
                {
                    return _isRunning;
                }
            }
            set
            {
                lock (lockObj)
                {
                    _isRunning = value;
                }
            }
        }

        bool _isWaiting;
        public bool isWaiting
        {
            get
            {
                lock (lockObj)
                {
                    return _isWaiting;
                }
            }
            set
            {
                lock (lockObj)
                {
                    _isWaiting = value;
                }
            }
        }
        public downloadOneDeviceStats(bool isMCB, string prettyName, uint id, uint startIDThisSession, uint maxIDSeen, uint downloadedID, bool actViewEnabled, uint downloadedPMID, uint startPMIDThisSession, uint maxPMIDSeen)
        {
            this.isMCB = isMCB;
            this.myName = prettyName;
            this.id = id;
            this.startIDThisSession = startIDThisSession;


            this.maxIDSeen = maxIDSeen;
            this.downloadedID = downloadedID;
            configLoaded = false;
            this.actViewEnabled = actViewEnabled;
            this.isRunning = false;
            if (isMCB)
            {
                this.maxPMIDSeen = maxPMIDSeen;
                this.startPMIDThisSession = startPMIDThisSession;
                this.downloadedPMID = downloadedPMID;
            }
            else
            {
                this.maxPMIDSeen = 1;
                this.startPMIDThisSession = 1;
                this.downloadedPMID = 1;
            }
        }
        public bool refresh(bool actViewEnabled, string prettyName, uint startIDThisSession, uint startPMfault)
        {
            lock (lockObj)
            {
                bool r = false;
                if (this.myName != prettyName)
                    r = true;
                this.myName = prettyName;
                this.actViewEnabled = actViewEnabled;
                if (this.startIDThisSession == UInt32.MaxValue)
                    this.startIDThisSession = startIDThisSession;
                if (this.startPMIDThisSession == UInt32.MaxValue)
                    this.startPMIDThisSession = startPMfault;
                return r;
            }
        }
        public bool isActViewEnabled()
        {
            lock (lockObj)
            {
                return actViewEnabled;
            }
        }
        public double getDownloadPercentage()
        {
            lock (lockObj)
            {
                double p;
                if (!isMCB)
                {
                    if (maxIDSeen + 1 - startIDThisSession == 0)
                    {
                        return 100;
                    }
                    p = 100 * (downloadedID + (configLoaded ? 1 : 0) - startIDThisSession) / (maxIDSeen + 1 - startIDThisSession);

                }
                else


                {
                    double p1 = 0;
                    double p2 = 0;
                    if (maxIDSeen + 1 - startIDThisSession == 0)
                    {
                        p1 = 50;
                    }
                    else
                    {
                        p1 = 50 * (downloadedID + (configLoaded ? 1 : 0) - startIDThisSession) / (maxIDSeen + 1 - startIDThisSession);

                    }
                    if (maxPMIDSeen - startPMIDThisSession == 0)
                    {
                        p2 = 50;
                    }
                    else
                    {
                        p2 = 50 * (downloadedPMID + startPMIDThisSession) / (maxPMIDSeen - startPMIDThisSession);

                    }
                    p = p1 + p2;

                }
                if (p > 100)
                    p = 100;
                if (p < 0)
                    p = 0;
                return p;
            }
        }
        public void setconfigLoaded()
        {
            lock (lockObj)
            {
                configLoaded = true;

            }
        }
        public void setMaxIDSeen(uint max)
        {
            lock (lockObj)
            {
                maxIDSeen = max;

            }
        }
        public void setMaxPMIDSeen(uint max)
        {
            lock (lockObj)
            {
                maxPMIDSeen = max;

            }
        }
        public void setdownloadedId(uint recordID)
        {
            lock (lockObj)
            {
                downloadedID = recordID;

            }
        }
        public void setPMdownloadedId(uint recordID)
        {
            lock (lockObj)
            {
                downloadedPMID = recordID;

            }
        }
        private bool _flagToStopSynch;
        public bool flagToStopSynch
        {
            get
            {
                lock (lockObj)
                {
                    return _flagToStopSynch;
                }
            }
            set
            {
                lock (lockObj)
                {
                    _flagToStopSynch = value;
                }
            }
        }
        public UInt32 getdownloadedId()
        {
            lock (lockObj)
            {
                return downloadedID;

            }
        }
        public UInt32 getPMdownloadedId()
        {
            lock (lockObj)
            {
                return downloadedPMID;

            }
        }
        public string getMyPrettyName()
        {
            lock (lockObj)
            {
                return myName;
            }
        }

        public DownloadrawStat getRawStat()
        {
            lock (lockObj)
            {
                return new DownloadrawStat(isMCB, myName, id, startIDThisSession, maxIDSeen, downloadedID, configLoaded, actViewEnabled, flagToStopSynch, isRunning, startPMIDThisSession, maxPMIDSeen, downloadedPMID, isWaiting);
            }
        }
    }
    public class DownloadrawStat
    {
        private string _myName;
        public string myName
        {
            get
            {
                return _myName;
            }
            private set
            {
                _myName = value;

            }
        }
        private uint _id;
        public uint id
        {
            get
            {
                return _id;
            }
            private set
            {
                _id = value;

            }
        }
        private uint _startIDThisSession;
        public uint startIDThisSession
        {
            get
            {
                return _startIDThisSession;
            }
            private set
            {
                _startIDThisSession = value;

            }
        }
        private uint _maxIDSeen;
        public uint maxIDSeen
        {
            get
            {
                return _maxIDSeen;
            }
            private set
            {
                _maxIDSeen = value;

            }
        }
        private uint _downloadedID;
        public uint downloadedID
        {
            get
            {
                return _downloadedID;
            }
            private set
            {
                _downloadedID = value;

            }
        }
        private bool _configLoaded;
        public bool configLoaded
        {
            get
            {
                return _configLoaded;
            }
            private set
            {
                _configLoaded = value;

            }
        }
        private bool _actViewEnabled;
        public bool actViewEnabled
        {
            get
            {
                return _actViewEnabled;
            }
            private set
            {
                _actViewEnabled = value;

            }
        }
        private bool _isMCB;
        public bool isMCB
        {
            get
            {
                return _isMCB;
            }
            private set
            {
                _isMCB = value;

            }
        }
        private bool _isRunning;
        public bool isRunning
        {
            get
            {
                return _isRunning;
            }
            private set
            {
                _isRunning = value;

            }
        }
        private bool _flagToStopSynch;
        public bool flagToStopSynch
        {
            get
            {
                return _flagToStopSynch;
            }
            private set
            {
                _flagToStopSynch = value;

            }
        }

        private uint _startPMIDThisSession;
        public uint startPMIDThisSession
        {
            get
            {
                return _startPMIDThisSession;
            }
            private set
            {
                _startPMIDThisSession = value;

            }
        }
        private uint _maxPMIDSeen;
        public uint maxPMIDSeen
        {
            get
            {
                return _maxPMIDSeen;
            }
            private set
            {
                _maxPMIDSeen = value;

            }
        }
        private uint _downloadedPMID;
        public uint downloadedPMID
        {
            get
            {
                return _downloadedPMID;
            }
            private set
            {
                _downloadedPMID = value;

            }
        }


        private bool _isWaiting;
        public bool isWaiting
        {
            get
            {
                return _isWaiting;
            }
            private set
            {
                _isWaiting = value;

            }
        }
        public DownloadrawStat(bool isMCB, string myName, uint id, uint startIDThisSession, uint maxIDSeen, uint downloadedID, bool configLoaded, bool actViewEnabled, bool flagToStopSynch, bool isRunning, uint startPMIDThisSession, uint maxPMIDSeen, uint downloadedPMID, bool isWaiting)
        {
            this.myName = myName;
            this.id = id;
            this.startIDThisSession = startIDThisSession;
            this.maxIDSeen = maxIDSeen;
            this.downloadedID = downloadedID;
            this.configLoaded = configLoaded;
            this.actViewEnabled = actViewEnabled;
            this.flagToStopSynch = flagToStopSynch;
            this.isRunning = isRunning;

            this.startPMIDThisSession = startPMIDThisSession;
            this.maxPMIDSeen = maxPMIDSeen;
            this.downloadedPMID = downloadedPMID;
            this.isMCB = isMCB;
            this._isWaiting = isWaiting;
        }
    }

    public class downloadDevicesxStats
    {
        Dictionary<string, downloadOneDeviceStats> stat = new Dictionary<string, downloadOneDeviceStats>();
        private object lockObj = new object();



        public void setIsRunningFlag(bool isMCB, uint id, uint studyid, bool val)
        {
            string k = DownloadUpload.getCompositeKye(isMCB, id, studyid);
            lock (lockObj)
            {
                if (stat.Keys.Contains(k))
                {
                    stat[k].isRunning = val;
                }
            }
        }
        public bool getIsRunningFlag(bool isMCB, uint id, uint studyid)
        {
            string k = DownloadUpload.getCompositeKye(isMCB, id, studyid);
            lock (lockObj)
            {
                if (stat.Keys.Contains(k))
                {
                    return stat[k].isRunning;
                }
            }
            return false;
        }
        public void setStopSynchFlag(bool isMCB, uint id, uint studyid, bool val)
        {
            string k = DownloadUpload.getCompositeKye(isMCB, id, studyid);
            lock (lockObj)
            {
                if (stat.Keys.Contains(k))
                {
                    stat[k].flagToStopSynch = val;
                }
            }
        }
        public bool getStopSynchFlag(bool isMCB, uint id, uint studyid)
        {
            string k = DownloadUpload.getCompositeKye(isMCB, id, studyid);
            lock (lockObj)
            {
                if (stat.Keys.Contains(k))
                {
                    return stat[k].flagToStopSynch;
                }
            }
            return false;
        }
        public bool addIfNotExist(bool isMCB, string prettyName, uint id, uint startIDThisSession, uint maxIDSeen, uint downloadedID, bool actViewEnabled, uint studyid, uint startPMIDThisSession, uint maxPMIDSeen, uint downloadedPMID)
        {
            string k = DownloadUpload.getCompositeKye(isMCB, id, studyid);
            lock (lockObj)
            {
                if (!stat.Keys.Contains(k))
                {
                    downloadOneDeviceStats d = new downloadOneDeviceStats(isMCB, prettyName, id, startIDThisSession, maxIDSeen, downloadedID, actViewEnabled, downloadedPMID, startPMIDThisSession, maxPMIDSeen);

                    stat.Add(k, d);
                    return true;
                }
            }
            return false;
        }
        public void removeKey(bool isMCB, uint id, uint studyid)
        {
            string k = DownloadUpload.getCompositeKye(isMCB, id, studyid);
            lock (lockObj)
            {
                if (stat.Keys.Contains(k))
                {
                    stat.Remove(k);
                }
            }
        }

        public void switchStudyID(bool isMCB, uint id, uint oldStudyId, uint newStudyID)
        {
            string k = DownloadUpload.getCompositeKye(isMCB, id, oldStudyId);
            lock (lockObj)
            {
                if (stat.Keys.Contains(k))
                {
                    downloadOneDeviceStats d = stat[k];
                    stat.Remove(k);
                    k = DownloadUpload.getCompositeKye(isMCB, id, newStudyID);
                    if (!stat.Keys.Contains(k))
                        stat.Add(k, d);
                }
            }

        }

        public bool refreshVars(bool isMCB, bool actViewEnabled, string prettyName, uint startIDThisSession, uint id, uint studyid, uint startPMfaults)
        {
            string k = DownloadUpload.getCompositeKye(isMCB, id, studyid);
            lock (lockObj)
            {
                if (stat.Keys.Contains(k))
                {
                    return stat[k].refresh(actViewEnabled, prettyName, startIDThisSession, startPMfaults);
                }
                else
                {
                    return false;
                }
            }
        }
        public double getDownloadPercentage(bool isMCB, uint id, uint studyid)
        {
            string k = DownloadUpload.getCompositeKye(isMCB, id, studyid);
            lock (lockObj)
            {
                if (stat.Keys.Contains(k))
                    return stat[k].getDownloadPercentage();
                else
                    return 0;
            }
        }
        public bool keyExists(bool isMCB, uint id, uint studyid)
        {
            string k = DownloadUpload.getCompositeKye(isMCB, id, studyid);
            lock (lockObj)
            {
                return stat.Keys.Contains(k);

            }
        }
        public void setconfigLoaded(bool isMCB, uint id, uint studyid)
        {
            string k = DownloadUpload.getCompositeKye(isMCB, id, studyid);
            lock (lockObj)
            {
                if (stat.Keys.Contains(k))
                    stat[k].setconfigLoaded();
            }
        }
        public bool isActViewEnabled(bool isMCB, uint id, uint studyid)
        {
            string k = DownloadUpload.getCompositeKye(isMCB, id, studyid);
            lock (lockObj)
            {
                if (stat.Keys.Contains(k))
                    return stat[k].isActViewEnabled();
                else
                    return true;
            }
        }
        public void setMaxIDSeen(bool isMCB, uint id, uint max, uint studyid)
        {
            string k = DownloadUpload.getCompositeKye(isMCB, id, studyid);
            lock (lockObj)
            {
                if (stat.Keys.Contains(k))
                    stat[k].setMaxIDSeen(max);
            }
        }
        public void setMaxPMIDSeen(bool isMCB, uint id, uint max, uint studyid)
        {
            string k = DownloadUpload.getCompositeKye(isMCB, id, studyid);
            lock (lockObj)
            {
                if (stat.Keys.Contains(k))
                    stat[k].setMaxPMIDSeen(max);
            }
        }
        public void setdownloadedId(bool isMCB, uint id, uint max, uint studyid)
        {
            string k = DownloadUpload.getCompositeKye(isMCB, id, studyid);
            lock (lockObj)
            {
                if (stat.Keys.Contains(k))
                    stat[k].setdownloadedId(max);
            }
        }
        public void setPMdownloadedId(bool isMCB, uint id, uint max, uint studyid)
        {
            string k = DownloadUpload.getCompositeKye(isMCB, id, studyid);
            lock (lockObj)
            {
                if (stat.Keys.Contains(k))
                    stat[k].setPMdownloadedId(max);
            }
        }

        public UInt32 getdownloadedId(bool isMCB, uint id, uint studyid)
        {
            string k = DownloadUpload.getCompositeKye(isMCB, id, studyid);
            lock (lockObj)
            {
                if (stat.Keys.Contains(k))
                    return stat[k].getdownloadedId();
                return 0;
            }
        }
        public UInt32 getPMdownloadedId(bool isMCB, uint id, uint studyid)
        {
            string k = DownloadUpload.getCompositeKye(isMCB, id, studyid);
            lock (lockObj)
            {
                if (stat.Keys.Contains(k))
                    return stat[k].getPMdownloadedId();
                return 0;
            }
        }
        public Dictionary<string, DownloadrawStat> getAllRawStats()
        {
            Dictionary<string, DownloadrawStat> d = new Dictionary<string, DownloadrawStat>();
            lock (lockObj)
            {
                foreach (string k in stat.Keys)
                {
                    if (!d.ContainsKey(k))

                        d.Add(k, stat[k].getRawStat());
                }
            }
            return d;
        }

        public void setisWaiting(bool isMCB, uint id, uint studyid, bool isWaiting)
        {
            string k = DownloadUpload.getCompositeKye(isMCB, id, studyid);
            lock (lockObj)
            {
                if (stat.Keys.Contains(k))
                    stat[k].isWaiting = isWaiting;
            }
        }
    }

    class uploadDevicesInfo
    {
        uint _id;
        public uint id
        {
            get
            {
                lock (myLock)
                {
                    return _id;
                }
            }
        }
        uint _studyID;
        public uint studyID
        {
            get
            {
                lock (myLock)
                {
                    return _studyID;
                }
            }
        }
        string _studyName;
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
                lock (myLock) _studyName = value;
            }
        }

        string _truckID;
        public string truckID
        {
            get
            {
                lock (myLock)
                {
                    return _truckID;
                }
            }
            set
            {
                lock (myLock) _truckID = value;
            }
        }
        object myLock;
        bool _notAuthorized;
        public bool notAuthorized
        {
            get
            {
                lock (myLock) return _notAuthorized;
            }
            set
            {
                lock (myLock) _notAuthorized = value;
            }
        }
        bool _failed;
        public bool failed
        {
            get
            {
                lock (myLock) return _failed;
            }
            set
            {
                lock (myLock) _failed = value;
            }
        }
        bool _configLoaded;
        public bool configLoaded
        {
            get
            {
                lock (myLock) return _configLoaded;
            }
            set
            {
                lock (myLock) _configLoaded = value;
            }
        }
        uint _totalUploaded;
        public uint totalUploaded
        {
            get
            {
                lock (myLock) return _totalUploaded;
            }
            set
            {
                lock (myLock) _totalUploaded = value;
            }
        }
        uint _totalNotUploaded;


        public uint totalNotUploaded
        {
            get
            {
                lock (myLock) return _totalNotUploaded;
            }
            set
            {
                lock (myLock) _totalNotUploaded = value;
            }
        }


        uint _totalExtraUploaded;
        public uint totalExtraUploaded
        {
            get
            {
                lock (myLock) return _totalExtraUploaded;
            }
            set
            {
                lock (myLock) _totalExtraUploaded = value;
            }
        }
        uint _totalExtraNotUploaded;


        public uint totalExtraNotUploaded
        {
            get
            {
                lock (myLock) return _totalExtraNotUploaded;
            }
            set
            {
                lock (myLock) _totalExtraNotUploaded = value;
            }
        }


        private string _config;
        public string config
        {
            get { lock (myLock) return _config; }
            set { lock (myLock) _config = value; }

        }
        private string _globalRecord;
        public string globalRecord
        {
            get { lock (myLock) return _globalRecord; }
            set { lock (myLock) _globalRecord = value; }

        }
        private bool _isMCB;
        public bool isMCB
        {
            get { lock (myLock) return _isMCB; }
            set { lock (myLock) _isMCB = value; }

        }
        private UInt32 _eventsCount;
        public UInt32 eventsCount
        {
            get { lock (myLock) return _eventsCount; }
            set { lock (myLock) _eventsCount = value; }

        }
        private UInt16 _memsig;
        public UInt16 memsig
        {
            get { lock (myLock) return _memsig; }
            set { lock (myLock) _memsig = value; }

        }
        private float _firmwareV;
        public float firmwareV
        {
            get { lock (myLock) return _firmwareV; }
            set { lock (myLock) _firmwareV = value; }

        }
        private byte _zone;
        public byte zone
        {
            get { lock (myLock) return _zone; }
            set { lock (myLock) _zone = value; }

        }
        private bool _configUploadChallanged;
        public bool configUploadChallanged
        {
            get { lock (myLock) return _configUploadChallanged; }
            set { lock (myLock) _configUploadChallanged = value; }

        }
        private bool _eventsUploadChallanged;
        public bool eventsUploadChallanged
        {
            get { lock (myLock) return _eventsUploadChallanged; }
            set { lock (myLock) _eventsUploadChallanged = value; }

        }
        public uploadDevicesInfo(uint id, uint studyID, bool isMCB)
        {
            myLock = new object();
            _id = id;
            _studyID = studyID;
            this.configLoaded = true;
            this.notAuthorized = false;
            this.totalNotUploaded = 0;
            this.totalUploaded = 0;
            this.failed = failed;
            this.totalExtraUploaded = 0;
            this.totalExtraNotUploaded = 0;

            zone = 0;
            firmwareV = 0;
            memsig = 0;
            eventsCount = 0;
            this.isMCB = isMCB;
            globalRecord = "";
            config = "";
            studyName = "";
            truckID = "";
        }
    }

}
