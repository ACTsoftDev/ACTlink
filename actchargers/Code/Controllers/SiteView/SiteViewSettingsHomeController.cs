using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

namespace actchargers
{
    public class SiteViewSettingsHomeController
    {
        readonly bool isBattView;

        public SiteViewSettingsHomeController(bool isBattView)
        {
            this.isBattView = isBattView;

            Init();
        }

        void Init()
        {
            ItemSource = new ObservableCollection<DeviceDeatilsItem>();

            if (GlobalLists.IsEmptyGlobalConnected())
                ShowNoSelectionMessage();

            CreateData();
        }

        void ShowNoSelectionMessage()
        {
            ACUserDialogs.ShowAlert(AppResources.select_connected_devices);
        }

        public ObservableCollection<DeviceDeatilsItem> ItemSource { get; set; }

        void CreateData()
        {
            if (isBattView)
                CreateBattViewData();
            else
                CreateChargerData();

            ReadConfigForAllDevices();
        }

        void CreateBattViewData()
        {
            ItemSource.Add(new DeviceDeatilsItem
            {
                DeviceTitle = AppResources.battview_settings,
                DeviceImage = "battview_settings",
                ViewModelType = typeof(SettingsViewModel)
            });
            ItemSource.Add(new DeviceDeatilsItem
            {
                DeviceTitle = AppResources.wifi,
                DeviceImage = "wifi",
                ViewModelType = typeof(WiFiViewModel)
            });
            ItemSource.Add(new DeviceDeatilsItem
            {
                DeviceTitle = AppResources.battery_info,
                DeviceImage = "battery_info",
                ViewModelType = typeof(BatteryInfoViewModel)
            });
        }

        void CreateChargerData()
        {
            ItemSource.Add(new DeviceDeatilsItem
            {
                DeviceTitle = AppResources.charger_info,
                DeviceImage = "charger_info",
                ViewModelType = typeof(InfoViewModel)
            });
            ItemSource.Add(new DeviceDeatilsItem
            {
                DeviceTitle = AppResources.charger_settings,
                DeviceImage = "charger_settings",
                ViewModelType = typeof(SettingsViewModel)
            });
            ItemSource.Add(new DeviceDeatilsItem
            {
                DeviceTitle = AppResources.battery_info,
                DeviceImage = "battery_info",
                ViewModelType = typeof(BatteryInfoViewModel)
            });
            ItemSource.Add(new DeviceDeatilsItem
            {
                DeviceTitle = AppResources.energy_management,
                DeviceImage = "energy_management",
                ViewModelType = typeof(EnergyManagementViewModel)
            });
            ItemSource.Add(new DeviceDeatilsItem
            {
                DeviceTitle = AppResources.wifi,
                DeviceImage = "wifi",
                ViewModelType = typeof(WiFiViewModel)
            });
            ItemSource.Add(new DeviceDeatilsItem
            {
                DeviceTitle = AppResources.power_module,
                DeviceImage = "power_module",
                ViewModelType = typeof(PmInfoViewModel) // In SiteView, Power Module is PM Info
            });
        }

        #region Config

        void ReadConfigForAllDevices()
        {
            try
            {
                TryReadConfigForAllDevices();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        void TryReadConfigForAllDevices()
        {
            Task.Run(async () => await ReadConfigForAllDevicesTask());
        }

        async Task ReadConfigForAllDevicesTask()
        {
            await ReadConfigForAllBattViwes();

            await ReadConfigForAllMcbs();
        }

        async Task ReadConfigForAllBattViwes()
        {
            foreach (var item in GlobalLists.GetActiveBattViewList())
                await item.ReadConfigIfNotLoaded();
        }

        async Task ReadConfigForAllMcbs()
        {
            foreach (var item in GlobalLists.GetActiveMcbList())
                await item.ReadConfigIfNotLoaded();
        }

        #endregion

        public void CancelSettings()
        {
            if (isBattView)
                CancelBattViewSettings();
            else
                CancelMcbSettings();
        }

        #region Cancel Settings for BattView

        void CancelBattViewSettings()
        {
            var devices = GlobalLists.GetActiveBattViewList();

            foreach (var item in devices)
                item.ResetSiteViewConfig();
        }

        #endregion

        #region Cancel Settings for MCB

        void CancelMcbSettings()
        {
            var devices = GlobalLists.GetActiveMcbList();

            foreach (var item in devices)
                item.ResetSiteViewConfig();
        }

        #endregion
    }
}
