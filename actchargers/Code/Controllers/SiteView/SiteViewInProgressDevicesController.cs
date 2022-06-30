using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace actchargers
{
    public class SiteViewInProgressDevicesController
    {
        public event EventHandler OnRefresh;

        readonly bool isBattView;
        readonly SiteViewInProgressTypes type;
        readonly List<SiteViewDeviceObject> devicesList;

        SiteViewUpdater siteViewUpdater;
        SiteViewDownloader siteViewDownloader;
        SiteViewSettingsSaver siteViewSettingsSaver;

        public SiteViewInProgressDevicesController
        (bool isBattView, SiteViewInProgressTypes type)
        {
            this.isBattView = isBattView;
            this.type = type;

            devicesList = GlobalLists.GetGlobalConnectedList();

            Init();
        }

        void Init()
        {
            siteViewUpdater = new SiteViewUpdater();
            siteViewUpdater.OnRefresh += SiteViewUpdater_OnRefresh;

            siteViewDownloader = new SiteViewDownloader();
            siteViewDownloader.OnRefresh += SiteViewDownloader_OnRefresh;

            siteViewSettingsSaver = new SiteViewSettingsSaver(isBattView);
            siteViewSettingsSaver.OnRefresh += SiteViewSettingsSaver_OnRefresh;

            StartAction();
        }

        void SiteViewUpdater_OnRefresh(object sender, EventArgs e)
        {
            FireOnRefresh();
        }

        void SiteViewDownloader_OnRefresh(object sender, EventArgs e)
        {
            FireOnRefresh();
        }

        void SiteViewSettingsSaver_OnRefresh(object sender, EventArgs e)
        {
            FireOnRefresh();
        }

        void FireOnRefresh()
        {
            OnRefresh?.Invoke(this, EventArgs.Empty);
        }

        void StartAction()
        {
            Task.Run(async () => await StartActionAsync());
        }

        async Task StartActionAsync()
        {
            await WaitToLoadUi();

            switch (type)
            {
                case SiteViewInProgressTypes.DOWNLOAD:

                    await StartDownload();

                    break;

                case SiteViewInProgressTypes.UPDATE:

                    await StartUpdate();

                    break;

                case SiteViewInProgressTypes.SAVE_SETTINGS:

                    await ApplySettings();

                    break;
            }
        }

        async Task WaitToLoadUi()
        {
            await Task.Delay(1000);
        }

        async Task StartDownload()
        {
            await siteViewDownloader.DownloadDevices();
        }

        async Task StartUpdate()
        {
            await siteViewUpdater.UpdateDevices();
        }

        async Task ApplySettings()
        {
            await siteViewSettingsSaver.ApplySettings();
        }

        public ObservableCollection<SiteViewDeviceObject> GetConnectedCollection()
        {
            return new ObservableCollection<SiteViewDeviceObject>(devicesList);
        }

        public void Close()
        {
            siteViewUpdater.Close();
            siteViewDownloader.Close();
        }
    }
}
