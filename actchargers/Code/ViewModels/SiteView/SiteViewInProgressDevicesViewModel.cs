using System;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace actchargers
{
    public class SiteViewInProgressDevicesViewModel : BaseViewModel
    {
        SiteViewInProgressDevicesController siteViewInProgressDevicesController;

        ObservableCollection<SiteViewDeviceObject> listItemSource;
        public ObservableCollection<SiteViewDeviceObject> ListItemSource
        {
            get { return listItemSource; }
            set
            {
                SetProperty(ref listItemSource, value);
            }
        }

        public SiteViewInProgressDevicesViewModel()
        {
            ViewTitle = AppResources.site_view;
        }

        public void Init(SiteViewInProgressTypes type)
        {
            siteViewInProgressDevicesController =
                new SiteViewInProgressDevicesController(IsBattView, type);
            siteViewInProgressDevicesController
                .OnRefresh += SiteViewInProgressDevicesController_OnRefresh;

            ListItemSource = siteViewInProgressDevicesController.GetConnectedCollection();
        }

        void SiteViewInProgressDevicesController_OnRefresh(object sender, EventArgs e)
        {
            InvokeOnMainThread(Refresh);
        }

        void Refresh()
        {
            RaisePropertyChanged(() => ListItemSource);
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

            ShowViewModel<SiteViewInProgressDevicesViewModel>(new { pop = "pop" });
        }

        public void Close()
        {
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
            siteViewInProgressDevicesController.Close();
        }
    }
}
