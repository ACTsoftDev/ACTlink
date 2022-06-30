using System;
using System.Diagnostics;
using System.Threading.Tasks;
using actchargers.ViewModels;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;
using Plugin.DeviceInfo.Abstractions;

namespace actchargers
{
    public class BaseViewModel : MvxViewModel
    {
        public static Platform DevicePlatform;
        public static bool IsLoggingOut;

        int _progressDialogReferenceCounter;

        BackgroundSessionManager backgroundSessionManager;

        internal MvxSubscriptionToken ActionMenuToken;

        public BaseViewModel()
        {
            DevicePlatform = CrossDeviceInfoManager.GetDevicePlatform();
            PropertyChanged += OnPropertyChanged;

            InitBackgroundSessionManager();
        }

        void InitBackgroundSessionManager()
        {
            backgroundSessionManager = new BackgroundSessionManager();
            backgroundSessionManager.LoggingOut += (sender, e) => LogoutIfLoggedIn();

            Task.Run(async () => await backgroundSessionManager.CheckSession());
        }

        string _viewTitle;
        public string ViewTitle
        {
            get { return _viewTitle; }
            set
            {
                _viewTitle = value;
                RaisePropertyChanged(() => ViewTitle);
            }
        }

        public bool IsAdmin
        {
            get
            {
                return Mvx.Resolve<IUserPreferences>()
                          .GetBool(ACConstants.USER_PREFS_ALLOW_EDITING);
            }
        }

        static bool _IsBattView;
        public static bool IsBattView
        {
            get
            {
                return _IsBattView;
            }
            set
            {
                _IsBattView = value;
            }
        }

        static bool _IsBattViewMobile;
        public static bool IsBattViewMobile
        {
            get
            {
                return _IsBattViewMobile;
            }
            set
            {
                _IsBattViewMobile = value;
            }
        }

        static bool _IsReplacement;
        public static bool IsReplacement
        {
            get
            {
                return _IsReplacement;
            }
            set
            {
                _IsReplacement = value;
            }
        }

        public bool NetworkCheck(bool scan = false)
        {
            return InternetConnectivityManager.NetworkCheck(scan);
        }

        bool _isBusy;
        public bool IsBusy
        {
            get
            {
                return _isBusy;
            }
            set
            {

                if (_isBusy)
                {
                    if (value)
                    {
                        _progressDialogReferenceCounter++;
                    }
                    else
                    {
                        _progressDialogReferenceCounter--;

                        if (_progressDialogReferenceCounter <= 0)
                        {
                            SetProperty(ref _isBusy, value);
                        }
                    }
                }
                else
                {
                    if (value)
                    {
                        _progressDialogReferenceCounter++;

                        SetProperty(ref _isBusy, value);
                    }
                    else
                    {
                        _progressDialogReferenceCounter = 0;
                    }
                }

            }
        }

        bool _isLoading;
        public bool IsLoading
        {
            get
            {
                return _isLoading;
            }
            set
            {
                SetProperty(ref _isLoading, value);

                ShowHideLoading(value);
            }
        }

        bool _logoutClicked;

        byte MCB_ViewLCDlastshowWarning_p;

        int MCB_ViewLCDlastDrawnSOC;

        bool MCB_ViewLCDlastTimeItWasCharging;

        DateTime MCB_ViewLCDLastSOCDrawTime;

        byte MCB_ViewLCDLastBatteryStage;

        byte MCB_ViewLCDLastSchedState;

        int MCB_ViewLCDLastWifiStatus;

        UInt32 MCB_ViewLCDLastrfTimerInMins;
        byte MCB_ViewLCDLastAutoStartCount;
        byte MCB_ViewLCDLastbattViewOn;
        DateTime MCB_ViewLCDLastTimeStamp;
        string MCB_ViewLCDLastBattViewID;
        bool MCB_ViewLCDloadedChargerName;
        public bool LogoutClicked
        {
            get { return _logoutClicked; }
            set
            {
                _logoutClicked = value;
                RaisePropertyChanged(() => LogoutClicked);
            }
        }

        void ShowHideLoading(bool show)
        {
            InvokeOnMainThread(new Action(() => ShowHideLoadingAction(show)));
        }

        void ShowHideLoadingAction(bool show)
        {
            if (show)
                ACUserDialogs.ShowProgress();
            else
                ACUserDialogs.HideProgress();
        }

        internal async Task DelayToLoad()
        {
            await Task.Delay(10);
        }

        public void LogoutIfLoggedIn()
        {
            if (!LogoutClicked)
                Logout();
        }

        virtual protected void Logout()
        {
            try
            {
                TryLogout();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        void TryLogout()
        {
            Mvx.Resolve<IUserPreferences>().Clear();

            DbSingleton
                .DBManagerServiceInstance
                .GetDBUserLoader()
                .DeleteAll();
            DbSingleton
                .DBManagerServiceInstance
                .GetGenericObjectsLoader()
                .DeleteAll();

            LogoutClicked = true;

            ShowViewModel<LoginViewModel>(new { logout = AppResources.yes });
        }

        internal void DisconnectDevice()
        {
            if (ActionMenuToken != null)
            {
                Mvx.Resolve<IMvxMessenger>().Unsubscribe<ActionsMenuMessage>(ActionMenuToken);

                ActionMenuToken = null;
            }

            string ipAddress = IsBattView ? BattViewQuantum.Instance.GetBATTView().IPAddress : MCBQuantum.Instance.GetMCB().IPAddress;

            Mvx.Resolve<IMvxMessenger>().Publish(new DeviceResetMessage(this, ipAddress));

            ShowViewModel<ConnectToDeviceViewModel>(new { popto = "popto" });
        }

        public virtual void Cancel()
        {

        }

        public virtual void OnBackButtonClick()
        {
        }

        public virtual void OnAndroidBackButtonClick()
        {
        }

        public void OnPropertyChanged
        (object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("IsBusy"))
            {
                try
                {
                    if (IsBusy)
                    {
                        ACUserDialogs.ShowProgress();
                    }
                    else
                    {
                        ACUserDialogs.HideProgress();
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }
            }
        }
    }
}
