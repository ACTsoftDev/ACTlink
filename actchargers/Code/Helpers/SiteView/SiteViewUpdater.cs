using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace actchargers
{
    public class SiteViewUpdater
    {
        public event EventHandler OnRefresh;

        readonly List<SiteViewDeviceObject> devicesList;

        bool hasToStop;

        public SiteViewUpdater()
        {
            devicesList = GlobalLists.GetGlobalConnectedList();
        }

        #region Update

        public async Task UpdateDevices()
        {
            InitAll();

            await StartUpdateDevices();
        }

        void InitAll()
        {
            foreach (var item in devicesList)
                CheckToInitOneDevice(item);
        }

        void CheckToInitOneDevice(SiteViewDeviceObject siteViewDeviceObject)
        {
            if (siteViewDeviceObject.CanDownloadOrUpdateDevice())
                InitOneDevice(siteViewDeviceObject);
        }

        void InitOneDevice(SiteViewDeviceObject siteViewDeviceObject)
        {
            siteViewDeviceObject.OnFirmwareUpdateStepChanged += SiteViewDeviceObject_OnFirmwareUpdateStepChanged;

            siteViewDeviceObject.ProgressCompleted = 0;

            ChangeDeviceStatus(siteViewDeviceObject, FirmwareUpdateStage.sendingRequest);
        }

        void SiteViewDeviceObject_OnFirmwareUpdateStepChanged(object sender, EventArgs e)
        {
            SiteViewDeviceObject siteViewDeviceObject = sender as SiteViewDeviceObject;

            siteViewDeviceObject.SimplyUpdateUi();

            FireOnRefresh();
        }

        async Task StartUpdateDevices()
        {
            var list = GetUpdatableList();

            if (IsEmptyList())
                ShowNoSelectionMessage();
            else
                await IterateToUpdateDevices();
        }

        List<SiteViewDeviceObject> GetUpdatableList()
        {
            var readyList =
                devicesList
                    .Where((arg) => arg.IsCheckedAndConnected()).ToList();

            return readyList;
        }

        bool IsEmptyList()
        {
            if (devicesList == null)
                return true;

            return devicesList.Count == 0;
        }

        void ShowNoSelectionMessage()
        {
            ACUserDialogs.ShowAlert(AppResources.select_connected_devices);
        }

        async Task IterateToUpdateDevices()
        {
            foreach (var item in devicesList)
            {
                if (hasToStop)
                    break;

                await UpdateDeviceIfAccepted(item);
            }
        }

        async Task UpdateDeviceIfAccepted(SiteViewDeviceObject siteViewDeviceObject)
        {
            if (siteViewDeviceObject.CanDownloadOrUpdateDevice())
                await UpdateDevice(siteViewDeviceObject);
        }

        async Task UpdateDevice(SiteViewDeviceObject siteViewDeviceObject)
        {
            ChangeDeviceStatus(siteViewDeviceObject);

            await siteViewDeviceObject.SiteViewUpdate();

            ChangeDeviceStatus(siteViewDeviceObject);
        }

        void ChangeDeviceStatus
        (SiteViewDeviceObject siteViewDeviceObject)
        {
            if (hasToStop)
                return;

            string interfaceSn = siteViewDeviceObject.InterfaceSn;
            SiteViewDevice siteViewDevice =
                SiteViewQuantum.Instance.GetConnectionManager()
                               .siteView.getDevice(interfaceSn);

            siteViewDevice.firmwareStage = siteViewDeviceObject.FirmwareUpdateStep;

            siteViewDeviceObject.SimplyUpdateUi();

            FireOnRefresh();
        }

        void ChangeDeviceStatus
        (SiteViewDeviceObject siteViewDeviceObject, FirmwareUpdateStage firmwareUpdateStage)
        {
            if (hasToStop)
                return;

            string interfaceSn = siteViewDeviceObject.InterfaceSn;
            SiteViewDevice siteViewDevice =
                SiteViewQuantum.Instance.GetConnectionManager()
                               .siteView.getDevice(interfaceSn);

            siteViewDeviceObject.FirmwareUpdateStep = firmwareUpdateStage;
            siteViewDevice.firmwareStage = siteViewDeviceObject.FirmwareUpdateStep;

            siteViewDeviceObject.SimplyUpdateUi();

            FireOnRefresh();
        }

        #endregion

        #region Close

        public void Close()
        {
            hasToStop = true;

            AbortUpdateAllDevices();
        }

        void AbortUpdateAllDevices()
        {
            try
            {
                TryAbortUpdateAllDevices();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        void TryAbortUpdateAllDevices()
        {
            foreach (var item in devicesList)
                AbortOneDevice(item);
        }

        void AbortOneDevice(SiteViewDeviceObject siteViewDeviceObject)
        {
            siteViewDeviceObject.AbortUpdate();

            siteViewDeviceObject.CheckToResetStatusForDevice();

            siteViewDeviceObject.SimplyUpdateUi();
        }

        #endregion

        void FireOnRefresh()
        {
            OnRefresh?.Invoke(this, EventArgs.Empty);
        }
    }
}
