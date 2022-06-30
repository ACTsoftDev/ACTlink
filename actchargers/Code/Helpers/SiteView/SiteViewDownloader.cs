using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace actchargers
{
    public class SiteViewDownloader
    {
        public event EventHandler OnRefresh;

        readonly List<SiteViewDeviceObject> devicesList;

        bool hasToStop;

        public SiteViewDownloader()
        {
            this.devicesList = GlobalLists.GetGlobalConnectedList();
        }

        #region Download

        public async Task DownloadDevices()
        {
            ChangeStatusToAll();

            await StartDownloadDevices();
        }

        void ChangeStatusToAll()
        {
            foreach (var item in devicesList)
            {
                CheckToChangeStatusToOneDevice(item);
            }
        }

        void CheckToChangeStatusToOneDevice(SiteViewDeviceObject siteViewDeviceObject)
        {
            if (siteViewDeviceObject.CanDownloadOrUpdateDevice())
                ChangeStatusToOneDevice(siteViewDeviceObject);
        }

        void ChangeStatusToOneDevice(SiteViewDeviceObject siteViewDeviceObject)
        {
            siteViewDeviceObject.ProgressCompleted = 0;

            ChangeDeviceStatus(siteViewDeviceObject, true);
        }

        async Task StartDownloadDevices()
        {
            var list = GetUpdatableList();

            if (IsEmptyList())
                ShowNoSelectionMessage();
            else
                await IterateToDownloadDevices();
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

        async Task IterateToDownloadDevices()
        {
            foreach (var item in devicesList)
            {
                if (hasToStop)
                    break;

                await DownloadDeviceIfAccepted(item);
            }
        }

        async Task DownloadDeviceIfAccepted(SiteViewDeviceObject siteViewDeviceObject)
        {
            if (siteViewDeviceObject.CanDownloadOrUpdateDevice())
                await DownloadDevice(siteViewDeviceObject);
        }

        async Task DownloadDevice(SiteViewDeviceObject siteViewDeviceObject)
        {
            ChangeDeviceStatus(siteViewDeviceObject, true);

            await siteViewDeviceObject.Download();

            ChangeDeviceStatus(siteViewDeviceObject, false);
        }

        void ChangeDeviceStatus
        (SiteViewDeviceObject siteViewDeviceObject, bool isDownloading)
        {
            if (hasToStop)
                return;

            siteViewDeviceObject.IsDownloading = isDownloading;

            FireOnRefresh();
        }

        #endregion

        #region Close

        public void Close()
        {
            hasToStop = true;

            AbortDownloadAllDevices();
        }

        void AbortDownloadAllDevices()
        {
            try
            {
                TryAbortDownloadAllDevices();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        void TryAbortDownloadAllDevices()
        {
            foreach (var item in devicesList)
                AbortOneDevice(item);
        }

        void AbortOneDevice(SiteViewDeviceObject siteViewDeviceObject)
        {
            siteViewDeviceObject.AbortDownload();
        }

        #endregion

        void FireOnRefresh()
        {
            OnRefresh?.Invoke(this, EventArgs.Empty);
        }
    }
}
