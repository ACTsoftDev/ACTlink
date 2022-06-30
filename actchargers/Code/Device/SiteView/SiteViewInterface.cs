using System;
using System.Linq;

namespace actchargers
{
    public enum siteViewActionType
    {
        none,
        settings,
        download,
        firmwareUpdate,
    }

    public enum siteViewViewType
    {
        all,
        charger,
        battview,
        battviewMobile
    }

    public class SiteViewInterface : BaseViewModel
    {
        //ConnectionManager connManager;
        public SiteViewInterfaceGenericVariables vars;

        public SiteViewInterface(siteViewViewType view_type, siteViewActionType action_type)
        {

            vars = new SiteViewInterfaceGenericVariables(view_type, action_type);
            vars.doScan = true;
            //if (IsBATTView)
            //{
            //    SiteViewQuantum.Instance.SetConnectionManager(BATTQuantum.Instance.GetConnectionManager());
            //}
            //else
            //{
            //    SiteViewQuantum.Instance.SetConnectionManager(MCBQuantum.Instance.GetConnectionManager());
            //}
        }

    }

    public class SiteViewInterfaceGenericVariables
    {

        object variablesLock;
        public void setSerials(byte[] battSerial, byte[] mcbSerial)
        {
            lock (variablesLock)
            {
                this.battSerials = new byte[battSerial.Length];
                Array.Copy(battSerial, this.battSerials, battSerial.Length);
                this.mcbSerials = new byte[mcbSerial.Length];
                Array.Copy(mcbSerial, this.mcbSerials, mcbSerial.Length);
            }
        }
        public SiteViewInterfaceGenericVariables(siteViewViewType runType, siteViewActionType action_type)
        {
            variablesLock = new object();
            charger = new ChargerSettings();
            battview = new BattviewSettings();
            battviewMobile = new BattviewSettings();
            //this.chargerAction = siteViewActionType.none;
            //this.battviewAction = siteViewActionType.none;
            //this.battviewMobileAction = siteViewActionType.none;
            this.chargerAction = action_type;
            this.battviewAction = action_type;
            this.battviewMobileAction = action_type;
            this.battviewIssueStop = false;
            this.battviewMobileIssueStop = false;
            this.chargerIssueStop = false;
            this.applicationExit = false;
            this.ChargerSiteCheckBox = this.BATTViewMobileSiteCheckBox = this.BATTViewSiteCheckBox = false;
            this._runType = runType;
            this._appActionType = action_type;

            routerLastWarningTime = DateTime.UtcNow.AddDays(-1);
            chargerFilterLink1 = false;
            chargerFilterLink2 = false;
            chargerFilterLink3 = false;
            chargerFilterLink4 = false;
            chargerFilterLink5 = false;
            chargerFilterLink6 = false;

            chargerFilterText = "";

            battviewFilterLink1 = false;
            battviewFilterLink2 = false;
            battviewFilterLink3 = false;
            battviewFilterLink4 = false;
            battviewFilterLink5 = false;
            battviewFilterLink6 = false;

            battviewFilterText = "";

            battviewMobileFilterLink1 = false;
            battviewMobileFilterLink2 = false;
            battviewMobileFilterLink3 = false;
            battviewMobileFilterLink4 = false;
            battviewMobileFilterLink5 = false;
            battviewMobileFilterLink6 = false;

            battviewMobileFilterText = "";

            threadPool = ControlObject.numberOfThreads;
            threadPool = (int)Math.Round(0.6 * threadPool * ControlObject.cpuPowerFactor);
            if (threadPool < 7)
                threadPool = 7;
        }
        public UInt32 siteID;
        bool _ChargerSiteCheckBox;
        public bool ChargerSiteCheckBox
        {
            set
            {
                lock (variablesLock)
                {
                    _ChargerSiteCheckBox = value;
                }
            }
            get
            {
                lock (variablesLock)
                {
                    return _ChargerSiteCheckBox;
                }
            }
        }
        bool _BATTViewSiteCheckBox;
        public bool BATTViewSiteCheckBox
        {
            set
            {
                lock (variablesLock)
                {
                    _BATTViewSiteCheckBox = value;
                }
            }
            get
            {
                lock (variablesLock)
                {
                    return _BATTViewSiteCheckBox;
                }
            }
        }
        bool _BATTViewMobileSiteCheckBox;
        public bool BATTViewMobileSiteCheckBox
        {
            set
            {
                lock (variablesLock)
                {
                    _BATTViewMobileSiteCheckBox = value;
                }
            }
            get
            {
                lock (variablesLock)
                {
                    return _BATTViewMobileSiteCheckBox;
                }
            }
        }
        bool _siteOverlay;
        public bool siteOverlay
        {
            set
            {
                lock (variablesLock)
                {
                    _siteOverlay = value;
                }
            }
            get
            {
                lock (variablesLock)
                {
                    return _siteOverlay;
                }
            }
        }

        siteViewViewType _modeSelected;
        public siteViewViewType modeSelected
        {
            set
            {
                lock (variablesLock)
                {
                    _modeSelected = value;
                }
            }
            get
            {
                lock (variablesLock)
                {
                    return _modeSelected;
                }
            }
        }

        siteViewActionType _chargerAction;
        public siteViewActionType chargerAction
        {
            set
            {
                lock (variablesLock)
                {
                    _chargerAction = value;
                }
            }
            get
            {
                lock (variablesLock)
                {
                    return _chargerAction;
                }
            }
        }
        siteViewActionType _battviewAction;
        public siteViewActionType battviewAction
        {
            set
            {
                lock (variablesLock)
                {
                    _battviewAction = value;
                }
            }
            get
            {
                lock (variablesLock)
                {
                    return _battviewAction;
                }
            }
        }

        siteViewActionType _battviewMobileAction;
        public siteViewActionType battviewMobileAction
        {
            set
            {
                lock (variablesLock)
                {
                    _battviewMobileAction = value;
                }
            }
            get
            {
                lock (variablesLock)
                {
                    return _battviewMobileAction;
                }
            }
        }

        bool _chargerIssueStop;
        public bool chargerIssueStop
        {
            set
            {
                lock (variablesLock)
                {
                    _chargerIssueStop = value;
                }
            }
            get
            {
                lock (variablesLock)
                {
                    return _chargerIssueStop;
                }
            }
        }

        bool _battviewIssueStop;
        public bool battviewIssueStop
        {
            set
            {
                lock (variablesLock)
                {
                    _battviewIssueStop = value;
                }
            }
            get
            {
                lock (variablesLock)
                {
                    return _battviewIssueStop;
                }
            }
        }
        bool _battviewMobileIssueStop;
        public bool battviewMobileIssueStop
        {
            set
            {
                lock (variablesLock)
                {
                    _battviewMobileIssueStop = value;
                }
            }
            get
            {
                lock (variablesLock)
                {
                    return _battviewMobileIssueStop;
                }
            }
        }

        int _threadPool;
        public int threadPool
        {
            set
            {
                lock (variablesLock)
                {
                    _threadPool = value;
                }
            }
            get
            {
                lock (variablesLock)
                {
                    return _threadPool;
                }
            }
        }
        public bool applicationExit
        {
            set
            {
                lock (variablesLock)
                {
                    _applicationExit = value;
                }
            }
            get
            {
                lock (variablesLock)
                {
                    return _applicationExit;
                }
            }
        }
        bool _applicationExit;


        byte[] battSerials;
        byte[] mcbSerials;

        public byte[] getBattSerials(int step)
        {
            if (battFirmwareLoadedIssue)
                return null;
            lock (variablesLock)
                return battSerials.Skip(step * 1024).Take(1024).ToArray();
        }
        public int getBattserialsLength()
        {
            if (battFirmwareLoadedIssue)
                return 0;
            lock (variablesLock)
                return (int)Math.Ceiling(battSerials.Length / 1024.0);
        }
        public int getMCBserialsLength()
        {
            if (mcbFirmwareLoadedIssue)
                return 0;
            lock (variablesLock)
                return (int)Math.Ceiling(mcbSerials.Length / 1024.0);
        }
        public byte[] getMCBSerials(int step)
        {
            if (mcbFirmwareLoadedIssue)
                return null;
            lock (variablesLock)
                return mcbSerials.Skip(step * 1024).Take(1024).ToArray();
        }


        public bool battFirmwareLoadedIssue
        {
            set
            {
                lock (variablesLock)
                {
                    _battFirmwareLoadedIssue = value;
                }
            }
            get
            {
                lock (variablesLock)
                {
                    return _battFirmwareLoadedIssue;
                }
            }
        }
        bool _battFirmwareLoadedIssue;

        public bool mcbFirmwareLoadedIssue
        {
            set
            {
                lock (variablesLock)
                {
                    _mcbFirmwareLoadedIssue = value;
                }
            }
            get
            {
                lock (variablesLock)
                {
                    return _mcbFirmwareLoadedIssue;
                }
            }
        }
        bool _mcbFirmwareLoadedIssue;

        public BattviewSettings battview;
        public BattviewSettings battviewMobile;

        public ChargerSettings charger;
        public bool chargerFilterLink1
        {
            set
            {
                lock (variablesLock)
                {
                    _chargerFilterLink1 = value;
                }
            }
            get
            {
                lock (variablesLock)
                {
                    return _chargerFilterLink1;
                }
            }
        }
        bool _chargerFilterLink1;

        public bool chargerFilterLink2
        {
            set
            {
                lock (variablesLock)
                {
                    _chargerFilterLink2 = value;
                }
            }
            get
            {
                lock (variablesLock)
                {
                    return _chargerFilterLink2;
                }
            }
        }
        bool _chargerFilterLink2;
        public bool chargerFilterLink3
        {
            set
            {
                lock (variablesLock)
                {
                    _chargerFilterLink3 = value;
                }
            }
            get
            {
                lock (variablesLock)
                {
                    return _chargerFilterLink3;
                }
            }
        }
        bool _chargerFilterLink3;

        public bool chargerFilterLink4
        {
            set
            {
                lock (variablesLock)
                {
                    _chargerFilterLink4 = value;
                }
            }
            get
            {
                lock (variablesLock)
                {
                    return _chargerFilterLink4;
                }
            }
        }
        bool _chargerFilterLink4;

        public bool chargerFilterLink5
        {
            set
            {
                lock (variablesLock)
                {
                    _chargerFilterLink5 = value;
                }
            }
            get
            {
                lock (variablesLock)
                {
                    return _chargerFilterLink5;
                }
            }
        }
        bool _chargerFilterLink5;

        public bool chargerFilterLink6
        {
            set
            {
                lock (variablesLock)
                {
                    _chargerFilterLink6 = value;
                }
            }
            get
            {
                lock (variablesLock)
                {
                    return _chargerFilterLink6;
                }
            }
        }
        bool _chargerFilterLink6;

        public string chargerFilterText
        {
            set
            {
                lock (variablesLock)
                {
                    _chargerFilterText = value;
                }
            }
            get
            {
                lock (variablesLock)
                {
                    return _chargerFilterText;
                }
            }
        }
        string _chargerFilterText;

        public bool battviewFilterLink1
        {
            set
            {
                lock (variablesLock)
                {
                    _battviewFilterLink1 = value;
                }
            }
            get
            {
                lock (variablesLock)
                {
                    return _battviewFilterLink1;
                }
            }
        }
        bool _battviewFilterLink1;

        public bool battviewFilterLink2
        {
            set
            {
                lock (variablesLock)
                {
                    _battviewFilterLink2 = value;
                }
            }
            get
            {
                lock (variablesLock)
                {
                    return _battviewFilterLink2;
                }
            }
        }
        bool _battviewFilterLink2;
        public bool battviewFilterLink3
        {
            set
            {
                lock (variablesLock)
                {
                    _battviewFilterLink3 = value;
                }
            }
            get
            {
                lock (variablesLock)
                {
                    return _battviewFilterLink3;
                }
            }
        }
        bool _battviewFilterLink3;

        public bool battviewFilterLink4
        {
            set
            {
                lock (variablesLock)
                {
                    _battviewFilterLink4 = value;
                }
            }
            get
            {
                lock (variablesLock)
                {
                    return _battviewFilterLink4;
                }
            }
        }
        bool _battviewFilterLink4;

        public bool battviewFilterLink5
        {
            set
            {
                lock (variablesLock)
                {
                    _battviewFilterLink5 = value;
                }
            }
            get
            {
                lock (variablesLock)
                {
                    return _battviewFilterLink5;
                }
            }
        }
        bool _battviewFilterLink5;

        public bool battviewFilterLink6
        {
            set
            {
                lock (variablesLock)
                {
                    _battviewFilterLink6 = value;
                }
            }
            get
            {
                lock (variablesLock)
                {
                    return _battviewFilterLink6;
                }
            }
        }
        bool _battviewFilterLink6;

        public string battviewFilterText
        {
            set
            {
                lock (variablesLock)
                {
                    _battviewFilterText = value;
                }
            }
            get
            {
                lock (variablesLock)
                {
                    return _battviewFilterText;
                }
            }
        }
        string _battviewFilterText;
        public bool battviewMobileFilterLink1
        {
            set
            {
                lock (variablesLock)
                {
                    _battviewMobileFilterLink1 = value;
                }
            }
            get
            {
                lock (variablesLock)
                {
                    return _battviewMobileFilterLink1;
                }
            }
        }
        bool _battviewMobileFilterLink1;

        public bool battviewMobileFilterLink2
        {
            set
            {
                lock (variablesLock)
                {
                    _battviewMobileFilterLink2 = value;
                }
            }
            get
            {
                lock (variablesLock)
                {
                    return _battviewMobileFilterLink2;
                }
            }
        }
        bool _battviewMobileFilterLink2;
        public bool battviewMobileFilterLink3
        {
            set
            {
                lock (variablesLock)
                {
                    _battviewMobileFilterLink3 = value;
                }
            }
            get
            {
                lock (variablesLock)
                {
                    return _battviewMobileFilterLink3;
                }
            }
        }
        bool _battviewMobileFilterLink3;

        public bool battviewMobileFilterLink4
        {
            set
            {
                lock (variablesLock)
                {
                    _battviewMobileFilterLink4 = value;
                }
            }
            get
            {
                lock (variablesLock)
                {
                    return _battviewMobileFilterLink4;
                }
            }
        }
        bool _battviewMobileFilterLink4;

        public bool battviewMobileFilterLink5
        {
            set
            {
                lock (variablesLock)
                {
                    _battviewMobileFilterLink5 = value;
                }
            }
            get
            {
                lock (variablesLock)
                {
                    return _battviewMobileFilterLink5;
                }
            }
        }
        bool _battviewMobileFilterLink5;

        public bool battviewMobileFilterLink6
        {
            set
            {
                lock (variablesLock)
                {
                    _battviewMobileFilterLink6 = value;
                }
            }
            get
            {
                lock (variablesLock)
                {
                    return _battviewMobileFilterLink6;
                }
            }
        }
        bool _battviewMobileFilterLink6;

        public string battviewMobileFilterText
        {
            set
            {
                lock (variablesLock)
                {
                    _battviewMobileFilterText = value;
                }
            }
            get
            {
                lock (variablesLock)
                {
                    return _battviewMobileFilterText;
                }
            }
        }
        string _battviewMobileFilterText;

        private DateTime _routerLastWarningTime;
        internal DateTime routerLastWarningTime
        {
            get
            {
                lock (variablesLock)
                {
                    return new DateTime(_routerLastWarningTime.Ticks);
                }
            }
            set
            {
                lock (variablesLock)
                {
                    _routerLastWarningTime = value;
                }
            }
        }



        private bool _doScan;
        internal bool doScan
        {
            get
            {
                lock (variablesLock)
                {
                    return _doScan;
                }
            }
            set
            {
                lock (variablesLock)
                {
                    _doScan = value;
                }
            }
        }

        private siteViewViewType _runType;
        internal siteViewViewType runType
        {
            get
            {
                lock (variablesLock)
                {
                    return _runType;
                }
            }
        }

        private siteViewActionType _appActionType;
        internal siteViewActionType appActionType
        {
            get
            {
                lock (variablesLock)
                {
                    return _appActionType;
                }
            }
        }


        private bool _showBattviewSettings;
        internal bool showBattviewSettings
        {
            get
            {
                lock (variablesLock)
                {
                    return _showBattviewSettings;
                }
            }
            set
            {
                lock (variablesLock)
                {
                    _showBattviewSettings = value;
                }
            }
        }

        private bool _showChargerSettings;
        internal bool showChargerSettings
        {
            get
            {
                lock (variablesLock)
                {
                    return _showChargerSettings;
                }
            }
            set
            {
                lock (variablesLock)
                {
                    _showChargerSettings = value;
                }
            }
        }
    }
}
