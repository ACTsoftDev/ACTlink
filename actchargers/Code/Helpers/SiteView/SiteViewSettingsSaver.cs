using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace actchargers
{
    public class SiteViewSettingsSaver
    {
        public event EventHandler OnRefresh;

        readonly bool isBattView;

        readonly List<SiteViewDeviceObject> devicesList;

        public SiteViewSettingsSaver(bool isBattView)
        {
            this.isBattView = isBattView;

            devicesList = GlobalLists.List;
        }

        public async Task ApplySettings()
        {
            ChangeStatusToAll();

            if (isBattView)
                await ApplyBattViewSettings();
            else
                await ApplyMcbSettings();

            FireOnRefresh();
        }

        void ChangeStatusToAll()
        {
            foreach (var item in devicesList)
                PrepareOneDeviceStatus(item);

            FireOnRefresh();
        }

        void PrepareOneDeviceStatus(SiteViewDeviceObject item)
        {
            item.ProgressCompleted = 20;
            item.DeviceStatus = AppResources.saving;
        }

        #region Apply Settings for BattView

        async Task ApplyBattViewSettings()
        {
            var devices = GlobalLists.GetActiveBattViewList();

            foreach (var item in devices)
                await ApplySettingsForOneBattView(item);
        }

        async Task ApplySettingsForOneBattView(BattViewObject item)
        {
            bool result;

            if (item.Config.RequireRebootForPlc())
            {
                var plcResult = await item.SaveConfigForPlc();

                result = plcResult.Item1;
            }
            else
            {
                result = await item.SaveSiteViewConfigAndTime();
            }

            if (NeedsLoad(result, item.RequireRefresh))
                result = await BattViewDoLoad(item);

            ChangeStatusForOneDevice(item.SerialNumber, result);
        }

        async Task<bool> BattViewDoLoad(BattViewObject device)
        {
            bool result = await device.DoLoad();

            if (result)
                device.RequireRefresh = false;

            return result;
        }

        #endregion

        #region Apply Settings for MCB

        async Task ApplyMcbSettings()
        {
            var devices = GlobalLists.GetActiveMcbList();

            foreach (var item in devices)
                await ApplySettingsForOneMcb(item);
        }

        async Task ApplySettingsForOneMcb(MCBobject item)
        {
            bool result = await item.SaveSiteViewConfigAndTime();

            if (NeedsLoad(result, item.RequireRefresh))
                result = await McbDoLoad(item);

            ChangeStatusForOneDevice(item.SerialNumber, result);
        }

        async Task<bool> McbDoLoad(MCBobject device)
        {
            bool result = await device.DoLoad();

            if (result)
                device.RequireRefresh = false;

            return result;
        }

        #endregion

        bool NeedsLoad(bool result, bool requireRefresh)
        {
            return result && requireRefresh;
        }

        void ChangeStatusForOneDevice(string serialNumber, bool saved)
        {
            var device = devicesList.FirstOrDefault((arg) => arg.InterfaceSn == serialNumber);

            if (device == null)
                return;

            if (saved)
                OnDeviceSaved(device);
            else
                OnDeviceFailed(device);
        }

        void OnDeviceSaved(SiteViewDeviceObject device)
        {
            device.ProgressCompleted = 100;
        }

        void OnDeviceFailed(SiteViewDeviceObject device)
        {
            device.ProgressCompleted = 0;
            device.DeviceStatus = AppResources.failed;
        }

        void FireOnRefresh()
        {
            OnRefresh?.Invoke(this, EventArgs.Empty);
        }
    }
}
