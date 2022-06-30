using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;

namespace actchargers
{
    public class DeviceHomeViewModel : BaseViewModel
    {
        static ConnectedDevicesPinger connectedDevicesPinger;

        readonly BattviewDeviceWarningManager battviewDeviceWarningManager;
        readonly McbDeviceWarningManager mcbDeviceWarningManager;

        public DeviceHomeViewModel()
        {
            ACUserDialogs.ShowProgress();

            _deviceDetailsItemSource = new ObservableCollection<DeviceDeatilsItem>();

            if (IsBattView)
            {
                CreateBattViewData();

                battviewDeviceWarningManager = new BattviewDeviceWarningManager();
                battviewDeviceWarningManager.ShowWarningsIfAny();
            }
            else
            {
                CreateChargerData();

                mcbDeviceWarningManager = new McbDeviceWarningManager();
                mcbDeviceWarningManager.ShowWarningsIfAny();
            }

            ACUserDialogs.HideProgress();

            InitConnectedDevicesPinger();



            ActionMenuToken = Mvx.Resolve<IMvxMessenger>().Subscribe<ActionsMenuMessage>(OnActionMenuMessage);
        }

        void InitConnectedDevicesPinger()
        {
            if (ControlObject.connectMethods.Connectiontype == ConnectionTypesEnum.ROUTER)
                InitConnectedDevicesPingerOnlyIfRouter();
        }

        void InitConnectedDevicesPingerOnlyIfRouter()
        {
            connectedDevicesPinger = new ConnectedDevicesPinger(IsBattView);
            connectedDevicesPinger.OnDeviceDisconnected += ConnectedDevicesPinger_OnDeviceDisconnected;

            Task.Run(connectedDevicesPinger.StartTimerForActiveDevice);
        }

        void ConnectedDevicesPinger_OnDeviceDisconnected(object sender, EventArgs e)
        {
            ShowViewModel<ConnectToDeviceViewModel>(new { popto = "popto" });

            InvokeOnMainThread(() =>
            {
                ACUserDialogs.ShowAlert(AppResources.device_disconnected);
            });
        }

        void OnActionMenuMessage(ActionsMenuMessage obj)
        {
            switch (obj.ActionsMenuType)
            {
                case ACUtility.ActionsMenuType.Refresh:
                    RefreshCommand.Execute(true);
                    break;
                case ACUtility.ActionsMenuType.Restart:
                    RestartDevice.Execute();
                    break;
            }
        }


        public bool IsForBattViewDetails { get; set; }

        ObservableCollection<DeviceDeatilsItem> _deviceDetailsItemSource = new ObservableCollection<DeviceDeatilsItem>();

        public ObservableCollection<DeviceDeatilsItem> DeviceDetailsItemSource
        {
            get { return _deviceDetailsItemSource; }
            set
            {
                _deviceDetailsItemSource = value;
                this.RaisePropertyChanged(() => this.DeviceDetailsItemSource);
            }
        }


        //collection for batt view details
        ObservableCollection<DeviceDeatilsItem> _battViewDetailsItemSource = new ObservableCollection<DeviceDeatilsItem>();

        public ObservableCollection<DeviceDeatilsItem> BattViewDetailsItemSource
        {
            get { return _battViewDetailsItemSource; }
            set
            {
                _battViewDetailsItemSource = value;
                this.RaisePropertyChanged(() => this.DeviceDetailsItemSource);
            }
        }

        void CreateBattViewData()
        {
            bool isDisabled = (ControlObject.UserAccess.CanReadNonCommissioned == AccessLevelConsts.noAccess) &&
            (!BattViewQuantum.Instance.GetBATTView().IsCommissioned());

            this._deviceDetailsItemSource.Add(new DeviceDeatilsItem
            {
                DeviceTitle = AppResources.batt_view_info,
                DeviceImage = "battview_info",
                ViewModelType = typeof(InfoViewModel)
            });
            this._deviceDetailsItemSource.Add(new DeviceDeatilsItem
            {
                DeviceTitle = AppResources.battview_settings,
                DeviceImage = "battview_settings",
                ViewModelType = typeof(SettingsViewModel),
                IsDisabled = isDisabled
            });
            this._deviceDetailsItemSource.Add(new DeviceDeatilsItem
            {
                DeviceTitle = AppResources.battery_info,
                DeviceImage = "battery_info",
                ViewModelType = typeof(BatteryInfoViewModel),
                IsDisabled = isDisabled
            });

            this._deviceDetailsItemSource.Add(new DeviceDeatilsItem
            {
                DeviceTitle = AppResources.quick_view,
                DeviceImage = "quick_view",
                ViewModelType = typeof(QuickViewModel),
                IsDisabled = isDisabled
            });
            this._deviceDetailsItemSource.Add(new DeviceDeatilsItem
            {
                DeviceTitle = AppResources.history,
                DeviceImage = "history",
                ViewModelType = typeof(HistoryViewModel),
                IsDisabled = isDisabled
            });
            this._deviceDetailsItemSource.Add(new DeviceDeatilsItem
            {
                DeviceTitle = AppResources.wifi,
                DeviceImage = "wifi",
                ViewModelType = typeof(WiFiViewModel),
                IsDisabled = isDisabled
            });
            if (ControlObject.UserAccess.Batt_Calibration != AccessLevelConsts.noAccess)
            {
                this._deviceDetailsItemSource.Add(new DeviceDeatilsItem
                {
                    DeviceTitle = AppResources.calibration,
                    DeviceImage = "caliberation",
                    ViewModelType = typeof(CalibrationViewModel),
                    IsDisabled = isDisabled
                });
            }
            if (IsManagementUser())
            {
                this._deviceDetailsItemSource.Add(new DeviceDeatilsItem
                {
                    DeviceTitle = AppResources.factory_control,
                    DeviceImage = "factory_control",
                    ViewModelType = typeof(FactoryControlViewModel)
                });
            }
            this._deviceDetailsItemSource.Add(new DeviceDeatilsItem
            {
                DeviceTitle = AppResources.firmware_update,
                DeviceImage = "firmware_update",
                ViewModelType = typeof(FirmwareUpdateViewModel)
            });
            if (IsManagementUser())
            {
                this._deviceDetailsItemSource.Add(new DeviceDeatilsItem
                {
                    DeviceTitle = AppResources.site_view,
                    DeviceImage = "site_view",
                    ViewModelType = typeof(SiteViewSitesViewModel)
                });
            }

        }

        void CreateChargerData()
        {
            bool isDisabled = (ControlObject.UserAccess.CanReadNonCommissioned == AccessLevelConsts.noAccess) &&
            (!MCBQuantum.Instance.GetMCB().IsCommissioned());

            this._deviceDetailsItemSource.Add(new DeviceDeatilsItem
            {
                DeviceTitle = AppResources.charger_info,
                DeviceImage = "charger_info",
                ViewModelType = typeof(InfoViewModel)
            });
            this._deviceDetailsItemSource.Add(new DeviceDeatilsItem
            {
                DeviceTitle = AppResources.charger_settings,
                DeviceImage = "charger_settings",
                ViewModelType = typeof(SettingsViewModel),
                IsDisabled = isDisabled
            });
            this._deviceDetailsItemSource.Add(new DeviceDeatilsItem
            {
                DeviceTitle = AppResources.power_module,
                DeviceImage = "power_module",
                ViewModelType = typeof(PowerModuleViewModel),
                IsDisabled = isDisabled
            });

            this._deviceDetailsItemSource.Add(new DeviceDeatilsItem
            {
                DeviceTitle = AppResources.battery_info,
                DeviceImage = "battery_info",
                ViewModelType = typeof(BatteryInfoViewModel),
                IsDisabled = isDisabled
            });

            this._deviceDetailsItemSource.Add(new DeviceDeatilsItem
            {
                DeviceTitle = AppResources.energy_management,
                DeviceImage = "energy_management",
                ViewModelType = typeof(EnergyManagementViewModel),
                IsDisabled = isDisabled
            });
            this._deviceDetailsItemSource.Add(new DeviceDeatilsItem
            {
                DeviceTitle = AppResources.quick_view,
                DeviceImage = "quick_view",
                ViewModelType = typeof(QuickViewModel),
                IsDisabled = isDisabled
            });
            this._deviceDetailsItemSource.Add(new DeviceDeatilsItem
            {
                DeviceTitle = AppResources.history,
                DeviceImage = "history",
                ViewModelType = typeof(HistoryViewModel),
                IsDisabled = isDisabled
            });
            this._deviceDetailsItemSource.Add(new DeviceDeatilsItem
            {
                DeviceTitle = AppResources.wifi,
                DeviceImage = "wifi",
                ViewModelType = typeof(WiFiViewModel),
                IsDisabled = isDisabled
            });
            if (ControlObject.UserAccess.MCB_Calibration != AccessLevelConsts.noAccess)
            {
                this._deviceDetailsItemSource.Add(new DeviceDeatilsItem
                {
                    DeviceTitle = AppResources.calibration,
                    DeviceImage = "caliberation",
                    ViewModelType = typeof(CalibrationViewModel),
                    IsDisabled = isDisabled
                });
            }
            if (IsManagementUser())
            {
                this._deviceDetailsItemSource.Add(new DeviceDeatilsItem
                {
                    DeviceTitle = AppResources.factory_control,
                    DeviceImage = "factory_control",
                    ViewModelType = typeof(FactoryControlViewModel)
                });
            }
            this._deviceDetailsItemSource.Add(new DeviceDeatilsItem
            {
                DeviceTitle = AppResources.firmware_update,
                DeviceImage = "firmware_update",
                ViewModelType = typeof(FirmwareUpdateViewModel)
            });
            if (IsManagementUser())
            {
                this._deviceDetailsItemSource.Add(new DeviceDeatilsItem
                {
                    DeviceTitle = AppResources.site_view,
                    DeviceImage = "site_view",
                    ViewModelType = typeof(SiteViewSitesViewModel)
                });
            }
        }

        bool IsManagementUser()
        {
            return ManagementUsers.IsManagementUser();
        }

        public void Init(string title, bool isForBattViewDetails)
        {
            ViewTitle = title;
            IsForBattViewDetails = isForBattViewDetails;

        }

        bool _isRefreshing;
        public bool IsRefreshing
        {
            get
            {
                return _isRefreshing;
            }
            set
            {
                _isRefreshing = value;
                RaisePropertyChanged(() => IsRefreshing);
            }
        }

        public IMvxCommand RefreshCommand
        {
            get
            {
                return new MvxCommand<bool>(OnRefreshClicked);
            }
        }

        void OnRefreshClicked(bool actionMenu)
        {
            Task.Run(async () => await OnRefreshClickedTask(actionMenu));
        }

        async Task OnRefreshClickedTask(bool actionMenu)
        {
            IsRefreshing = true;

            if (actionMenu)
                ACUserDialogs.ShowProgress();

            if (!ACConstants.IS_STATIC_DATA_REQUIRED)
            {
                if (IsBattView)
                {
                    try
                    {
                        await CreateConnectionForBATTView();
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }
                }
                else
                {
                    try
                    {
                        await CreateConnectionForMCB();
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }
                }
            }

            IsRefreshing = false;

            if (actionMenu)
                ACUserDialogs.HideProgress();
        }

        public IMvxCommand DownloadCommand
        {
            get
            {
                return new MvxCommand(OnDownloadClicked);
            }
        }

        void OnDownloadClicked()
        {
            ShowViewModel<SiteViewDevicesViewModel>(
                new
                {
                    siteId = 0,
                    isWithSynchSites = false
                });
        }

        MvxCommand<DeviceDeatilsItem> m_SelectGridItemCommand;

        public ICommand SelectGridItemCommand
        {
            get
            {
                return this.m_SelectGridItemCommand ?? (this.m_SelectGridItemCommand = new MvxCommand<DeviceDeatilsItem>(this.ExecuteSelectGridItemCommand));
            }
        }

        void ExecuteSelectGridItemCommand(DeviceDeatilsItem item)
        {
            if (item.IsDisabled)
                return;

            if (!IsRefreshing)
            {
                if (IsBattView)
                {
                    if (!BattViewQuantum.Instance.BattView_quickSerialNumberCheckStrict())
                    {
                        return;
                    }
                }
                else
                {
                    if (!MCBQuantum.Instance.MCB_quickSerialNumberCheckStrict())
                    {
                        return;
                    }
                }

                if (item.ViewModelType == typeof(WiFiViewModel))
                {
                    ShowViewModel<WiFiViewModel>();
                }
                else if (item.ViewModelType == typeof(InfoViewModel))
                {
                    if (IsBattView)
                    {
                        ShowViewModel<InfoViewModel>();
                    }
                    else
                    {
                        ShowViewModel<InfoViewModel>();
                    }

                }
                else if (item.ViewModelType == typeof(SettingsViewModel))
                {
                    ShowViewModel<SettingsViewModel>();
                }
                else if (item.ViewModelType == typeof(HistoryViewModel))
                {
                    if (IsBattView)
                    {
                        ShowViewModel<HistoryViewModel>();
                    }
                    else
                    {

                        ShowViewModel<HistoryViewModel>();
                    }
                }
                else if (item.ViewModelType == typeof(QuickViewModel))
                {
                    if (IsBattView)
                    {
                        ShowViewModel<QuickViewModel>();
                    }
                    else
                    {
                        ShowViewModel<QuickViewChargerViewModel>();
                    }
                }
                else if (item.ViewModelType == typeof(BatteryInfoViewModel))
                {

                    ShowViewModel<BatteryInfoViewModel>();
                }
                else if (item.ViewModelType == typeof(PowerModuleViewModel))
                {
                    //ACUserDialogs.ShowAlert(AppResources.dev_in_progress);
                    ShowViewModel<PowerModuleViewModel>();
                }
                else if (item.ViewModelType == typeof(CalibrationViewModel))
                {
                    //ACUserDialogs.ShowAlert(AppResources.dev_in_progress);
                    ShowViewModel<CalibrationViewModel>();
                }
                else if (item.ViewModelType == typeof(EnergyManagementViewModel))
                {
                    //ACUserDialogs.ShowAlert(AppResources.dev_in_progress);
                    ShowViewModel<EnergyManagementViewModel>();
                }
                else if (item.ViewModelType == typeof(FactoryControlViewModel))
                {
                    //ACUserDialogs.ShowAlert(AppResources.dev_in_progress);
                    ShowViewModel<FactoryControlViewModel>();
                }
                else if (item.ViewModelType == typeof(FirmwareUpdateViewModel))
                {
                    //ACUserDialogs.ShowAlert(AppResources.dev_in_progress);
                    ShowViewModel<FirmwareUpdateViewModel>();
                }
                else if (item.ViewModelType == typeof(SiteViewSitesViewModel))
                {
                    ShowViewModel<SiteViewSitesViewModel>();
                }
                else
                {
                    ACUserDialogs.ShowAlert(AppResources.dev_in_progress);
                }
            }
        }

        public IMvxCommand RestartDevice
        {
            get
            {
                return new MvxAsyncCommand(ExecuteRestartDeviceCommand);
            }
        }

        async Task ExecuteRestartDeviceCommand()
        {
            if (IsBattView)
            {
                await BATT_simpleCommunicationButtonAction();
            }
            else
            {
                await MCB_simpleCommunicationButtonAction();
            }
        }

        public IMvxCommand UpdateFirmware
        {
            get
            {
                return new MvxCommand(ExecuteUpdateFirmwareCommand);
            }
        }

        void ExecuteUpdateFirmwareCommand()
        {
            ACUserDialogs.ShowAlert(AppResources.dev_in_progress);
        }

        public override void OnBackButtonClick()
        {
            base.OnBackButtonClick();

            try
            {
                TryOnBackButtonClick();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        void TryOnBackButtonClick()
        {
            if (ActionMenuToken != null)
            {
                Mvx.Resolve<IMvxMessenger>().Unsubscribe<ActionsMenuMessage>(ActionMenuToken);
                ActionMenuToken = null;
            }

            ClearPingDeviceTimer();

            Mvx.Resolve<IMvxMessenger>().Publish(new BackScanMessage(this));

            ShowViewModel<DeviceHomeViewModel>(new { pop = "pop" });
        }

        public override void OnAndroidBackButtonClick()
        {
            base.OnAndroidBackButtonClick();

            ClearPingDeviceTimer();

            Mvx.Resolve<IMvxMessenger>().Publish(new BackScanMessage(this));
        }

        public static void ClearPingDeviceTimer()
        {
            if (connectedDevicesPinger != null)
                connectedDevicesPinger.StopTimer();
        }

        async Task CreateConnectionForBATTView()
        {
            BattViewCommunicationTypes caller = BattViewCommunicationTypes.NOCall;
            object arg1 = null;

            caller = BattViewCommunicationTypes.connectCommand;
            BattViewQuantum.Instance.SetConnectionManager(BattViewQuantum.Instance.GetConnectionManager());
            List<object> arguments = new List<object>
            {
                caller,
                arg1
            };
            try
            {
                var result = await BattViewQuantum.Instance.CommunicateBATTView(arguments);

                if (result.Count > 0)
                {
                    if (result[2].Equals(CommunicationResult.OK))
                    {
                        Debug.WriteLine("Refresh Completed");
                    }
                    else
                    {
                        ACUserDialogs.ShowAlert(AppResources.unable_to_refresh_battview);
                    }
                }
                else
                {
                    Debug.WriteLine(AppResources.unable_to_refresh_battview + ViewTitle);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        async Task CreateConnectionForMCB()
        {
            McbCommunicationTypes caller = McbCommunicationTypes.NOCall;
            object arg1 = null;

            caller = McbCommunicationTypes.connectcomamnd;
            MCBQuantum.Instance.SetConnectionManager(MCBQuantum.Instance.GetConnectionManager());
            List<object> arguments = new List<object>
            {
                caller,
                arg1
            };
            try
            {
                var result = await MCBQuantum.Instance.CommunicateMCB(arguments);

                if (result.Count > 0)
                {
                    if (result[2].Equals(CommunicationResult.OK))
                    {
                        Debug.WriteLine("Refresh Completed");
                    }
                    else
                    {
                        ACUserDialogs.ShowAlert(AppResources.unable_to_refresh_MCB);
                    }
                }
                else
                {
                    Debug.WriteLine(AppResources.unable_to_refresh_battview + ViewTitle);

                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        async Task MCB_simpleCommunicationButtonAction()
        {
            ACUserDialogs.ShowProgress();
            McbCommunicationTypes caller = McbCommunicationTypes.NOCall;
            object arg1 = null;

            caller = McbCommunicationTypes.restartDevice;

            MCBQuantum.Instance.SetConnectionManager(MCBQuantum.Instance.GetConnectionManager());

            List<object> arguments = new List<object>
            {
                caller,
                arg1
            };

            try
            {
                var result = await MCBQuantum.Instance.CommunicateMCB(arguments);

                if (result.Count > 0)
                {
                    var status = (CommunicationResult)result[2];

                    if (status == CommunicationResult.CHARGER_BUSY)
                    {
                        ACUserDialogs.ShowAlertWithTitleAndOkButton(AppResources.charger_is_busy);
                    }
                    else if (status == CommunicationResult.OK)
                    {
                        DisconnectDevice();

                        ACUserDialogs.ShowAlertWithTitleAndOkButton(AppResources.charger_restarting);
                    }
                }
                else
                {
                    Debug.WriteLine("Unable to restart MCB");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            ACUserDialogs.HideProgress();
        }


        async Task BATT_simpleCommunicationButtonAction()
        {
            ACUserDialogs.ShowProgress();
            BattViewCommunicationTypes caller = BattViewCommunicationTypes.NOCall;
            object arg1 = null;

            caller = BattViewCommunicationTypes.connectCommand;
            BattViewQuantum.Instance.SetConnectionManager(BattViewQuantum.Instance.GetConnectionManager());

            List<object> arguments = new List<object>
            {
                caller,
                arg1
            };

            try
            {
                var result = await BattViewQuantum.Instance.CommunicateBATTView(arguments);

                if (result.Count > 0)
                {
                    var status = (CommunicationResult)result[2];
                    if (status == CommunicationResult.COMMAND_DELAYED)
                    {
                        ACUserDialogs.ShowAlertWithTitleAndOkButton("BATTView is busy.");
                    }
                    else if (status == CommunicationResult.OK)
                    {
                        if (caller != BattViewCommunicationTypes.restartDeviceNoDisconnect)
                        {
                            DisconnectDevice();

                            ACUserDialogs.ShowAlertWithTitleAndOkButton(AppResources.batt_view_restarting);
                        }
                    }
                    else
                    {
                        ACUserDialogs.ShowAlertWithTitleAndOkButton("Unable to restart BattView");
                    }
                }
                else
                {
                    Debug.WriteLine("Unable to restart BattView");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                //AnalyticsManager.Track_App_Exception(ex.Message, false);
            }
            ACUserDialogs.HideProgress();
        }
    }
}