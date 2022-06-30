using System.Collections.ObjectModel;
using System.Windows.Input;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;

namespace actchargers
{
    public class ActionsMenuViewModel : BaseViewModel
    {
        ActionsMenuTypes listType;
        public ActionsMenuTypes ListType
        {
            get
            {
                return listType;
            }
            set
            {
                SetProperty(ref listType, value);

                ChooseList();
            }
        }

        void ChooseList()
        {
            switch (ListType)
            {
                case ActionsMenuTypes.SITE_ACTION:
                    CreateListForSiteAction();

                    break;

                case ActionsMenuTypes.DEVICE:
                    CreateListForDeviceAction();

                    break;

                case ActionsMenuTypes.SITE_VIEW:

                    CreateListForSiteViewAction();

                    break;
            }
        }

        ObservableCollection<DeviceDeatilsItem> actionsMenuItems =
            new ObservableCollection<DeviceDeatilsItem>();
        public ObservableCollection<DeviceDeatilsItem> ActionsMenuItems
        {
            get
            {
                return actionsMenuItems;
            }
        }

        public ActionsMenuViewModel()
        {
            ListType = ActionsMenuTypes.SITE_ACTION;
        }

        void CreateListForSiteAction()
        {
            ActionsMenuItems.Clear();

            ActionsMenuItems.Add(new DeviceDeatilsItem
            {
                DeviceTitle = AppResources.download,
                DeviceImage = "download_devicedata",
                ActionType = ACUtility.ActionsMenuType.Download
            });
            ActionsMenuItems.Add(new DeviceDeatilsItem
            {
                DeviceTitle = AppResources.upload_data,
                DeviceImage = "upload_data_whitebg",
                ActionType = ACUtility.ActionsMenuType.Upload
            });
            ActionsMenuItems.Add(new DeviceDeatilsItem
            {
                DeviceTitle = AppResources.sync_sites,
                DeviceSubTitle = AppResources.today,
                DeviceImage = "sync_sites_whitebg",
                ActionType = ACUtility.ActionsMenuType.SyncSites
            });
            if (CanShowSiteView())
            {
                ActionsMenuItems.Add(new DeviceDeatilsItem
                {
                    DeviceTitle = AppResources.site_view,
                    DeviceImage = "site_view_icon",
                    ActionType = ACUtility.ActionsMenuType.SiteView
                });
            }
            ActionsMenuItems.Add(new DeviceDeatilsItem
            {
                DeviceTitle = AppResources.update_site_firmware,
                DeviceImage = "update_site_firmware",
                ActionType = ACUtility.ActionsMenuType.Update
            });
        }

        bool CanShowSiteView()
        {
            if (!ACConstants.SHOW_SITEVIEW)
            {
                return false;
            }

            bool hasSites =
                DbSingleton
                    .DBManagerServiceInstance
                    .GetSynchSiteObjectsLoader()
                    .HasSites();

            return hasSites;
        }

        void CreateListForDeviceAction()
        {
            ActionsMenuItems.Clear();

            ActionsMenuItems.Add(new DeviceDeatilsItem
            {
                DeviceTitle = AppResources.restart_device,
                DeviceImage = "restat_device",
                ActionType = ACUtility.ActionsMenuType.Restart
            });

            ActionsMenuItems.Add(new DeviceDeatilsItem
            {
                DeviceTitle = AppResources.refresh,
                DeviceImage = "refresh_device",
                ActionType = ACUtility.ActionsMenuType.Refresh
            });
        }

        void CreateListForSiteViewAction()
        {
            ActionsMenuItems.Clear();

            ActionsMenuItems.Add(new DeviceDeatilsItem
            {
                DeviceTitle = AppResources.download,
                DeviceImage = "download_devicedata",
                ActionType = ACUtility.ActionsMenuType.SITE_VIEW_DOWNLOAD
            });
            ActionsMenuItems.Add(new DeviceDeatilsItem
            {
                DeviceTitle = AppResources.update_site_firmware,
                DeviceImage = "update_site_firmware",
                ActionType = ACUtility.ActionsMenuType.SITE_VIEW_UPDATE
            });
            ActionsMenuItems.Add(new DeviceDeatilsItem
            {
                DeviceTitle = AppResources.settings,
                DeviceImage = "settings_icon",
                ActionType = ACUtility.ActionsMenuType.SITE_VIEW_SETTINGS
            });
        }

        ICommand selectItemCommand;
        public ICommand SelectItemCommand
        {
            get
            {
                return selectItemCommand ??
                    (selectItemCommand = new MvxCommand<DeviceDeatilsItem>(ExecuteSelectItemCommand));
            }
        }

        void ExecuteSelectItemCommand(DeviceDeatilsItem item)
        {
            Mvx.Resolve<IMvxMessenger>()
               .Publish(new ActionsMenuMessage(this, item.ActionType));
        }
    }
}