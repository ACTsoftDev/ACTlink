using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;

namespace actchargers
{
    public class SiteViewDevicesViewModel : BaseViewModel
    {
        SiteViewDevicesController siteViewDevicesController;

        MvxSubscriptionToken actionMenuSubscriptionToken;

        bool BackupIsBattView;
        bool BackupIsBattViewMobile;

        DevicesTabs selectedTabIndex;
        public DevicesTabs SelectedTabIndex
        {
            get
            {
                return selectedTabIndex;
            }
            set
            {
                SetProperty(ref selectedTabIndex, value);

                OnTabChanged();
            }
        }

        ObservableCollection<SiteViewDeviceObject> listItemSource;
        public ObservableCollection<SiteViewDeviceObject> ListItemSource
        {
            get { return listItemSource; }
            set
            {
                SetProperty(ref listItemSource, value);

                RaisePropertyChanged(() => SelectAllVisibility);
                RaisePropertyChanged(() => SelectAllVisibilityIos);
            }
        }

        public bool SelectAllVisibility
        {
            get => (ListItemSource != null) && (ListItemSource.Count > 0);
        }

        public bool SelectAllVisibilityIos
        {
            get => !(SelectAllVisibility);
        }

        bool selectAllChecked;
        public bool SelectAllChecked
        {
            get
            {
                return selectAllChecked;
            }
            set
            {
                SetProperty(ref selectAllChecked, value);
            }
        }

        public string SelectAllTitle
        {
            get
            {
                return AppResources.select_all;
            }
        }

        public SiteViewDevicesViewModel()
        {
            ViewTitle = AppResources.site_view;

            BackupIsBattView = IsBattView;
            BackupIsBattViewMobile = IsBattViewMobile;
        }

        public void Init(uint siteId, bool isWithSynchSites)
        {
            siteViewDevicesController =
                new SiteViewDevicesController(siteId, isWithSynchSites);
            siteViewDevicesController.OnListUpdated += SiteViewDevicesController_OnListUpdated;

            actionMenuSubscriptionToken =
                Mvx.Resolve<IMvxMessenger>()
                   .Subscribe<ActionsMenuMessage>(OnActionMenuMessage);

            OnTabChanged();
        }

        void OnActionMenuMessage(ActionsMenuMessage obj)
        {
            switch (obj.ActionsMenuType)
            {
                case ACUtility.ActionsMenuType.SITE_VIEW_DOWNLOAD:
                    DownloadCommand.Execute();

                    break;

                case ACUtility.ActionsMenuType.SITE_VIEW_UPDATE:
                    UpdateCommand.Execute();

                    break;

                case ACUtility.ActionsMenuType.SITE_VIEW_SETTINGS:
                    SettingsCommand.Execute();

                    break;
            }
        }

        void OnTabChanged()
        {
            SelectAllChecked = false;

            IsBattView = GetIsBattViewTab();
            IsBattViewMobile = GetIsBattViewMobileTab();

            UpdateList();
        }

        bool GetIsBattViewTab()
        {
            bool isBattViewTab =
                (SelectedTabIndex == DevicesTabs.BATTVIEW) ||
                (SelectedTabIndex == DevicesTabs.BATTVIEW_MOBILE);

            return isBattViewTab;
        }


        bool GetIsBattViewMobileTab()
        {
            bool isBattViewTab = SelectedTabIndex == DevicesTabs.BATTVIEW_MOBILE;

            return isBattViewTab;
        }

        void SiteViewDevicesController_OnListUpdated(object sender, EventArgs e)
        {
            InvokeOnMainThread(UpdateList);
        }

        void UpdateList()
        {
            ListItemSource = siteViewDevicesController.GetListByType(SelectedTabIndex);
        }

        public IMvxCommand UpdateCommand
        {
            get
            {
                return new MvxCommand(OnUpdateCommandClicked);
            }
        }

        void OnUpdateCommandClicked()
        {
            ShowViewModel<SiteViewInProgressDevicesViewModel>
            (
                new
                {
                    type = SiteViewInProgressTypes.UPDATE
                }
            );
        }

        public IMvxCommand DownloadCommand
        {
            get
            {
                return new MvxCommand(OnDownloadCommandClicked);
            }
        }

        void OnDownloadCommandClicked()
        {
            ShowViewModel<SiteViewInProgressDevicesViewModel>
            (
                new
                {
                    type = SiteViewInProgressTypes.DOWNLOAD
                }
            );
        }

        public IMvxCommand SettingsCommand
        {
            get
            {
                return new MvxCommand(OnSettingsCommandClicked);
            }
        }

        void OnSettingsCommandClicked()
        {
            ShowViewModel<SiteViewSettingsHomeViewModel>();
        }

        public IMvxCommand SelectAllCommand
        {
            get
            {
                return new MvxCommand(OnSelectAllCommandClicked);
            }
        }

        void OnSelectAllCommandClicked()
        {
            SelectAllChecked = !SelectAllChecked;

            SelectAll();
        }

        void SelectAll()
        {
            foreach (var item in ListItemSource)
                item.IsChecked = SelectAllChecked;
        }

        public override void OnAndroidBackButtonClick()
        {
            base.OnAndroidBackButtonClick();

            Close();
        }

        public override void OnBackButtonClick()
        {
            base.OnBackButtonClick();

            Close();

            ShowViewModel<SiteViewDevicesViewModel>(new { pop = "pop" });
        }

        public void Close()
        {
            IsBattView = BackupIsBattView;
            IsBattViewMobile = BackupIsBattViewMobile;

            try
            {
                TryClose();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        void TryClose()
        {
            siteViewDevicesController.Close();
        }
    }
}
