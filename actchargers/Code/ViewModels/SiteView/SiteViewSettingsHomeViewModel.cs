using System.Collections.ObjectModel;
using System.Windows.Input;
using MvvmCross.Core.ViewModels;

namespace actchargers
{
    public class SiteViewSettingsHomeViewModel : BaseViewModel
    {
        readonly SiteViewSettingsHomeController controller;

        public SiteViewSettingsHomeViewModel()
        {
            controller = new SiteViewSettingsHomeController(IsBattView);

            ViewTitle = AppResources.settings;
        }

        public ObservableCollection<DeviceDeatilsItem> ItemSource
        {
            get => controller.ItemSource;
        }

        public string ApplySettingsTitle
        {
            get => AppResources.apply_settings;
        }

        public string CancelTitle
        {
            get => AppResources.cancel;
        }

        MvxCommand<DeviceDeatilsItem> selectGridItemCommand;
        public IMvxCommand SelectGridItemCommand
        {
            get
            {
                return selectGridItemCommand ??
                    (selectGridItemCommand =
                     new MvxCommand<DeviceDeatilsItem>(ExecuteSelectGridItemCommand));
            }
        }

        void ExecuteSelectGridItemCommand(DeviceDeatilsItem item)
        {
            if (item.ViewModelType == typeof(SettingsViewModel))
            {
                ShowViewModel<SettingsViewModel>(new { isSiteView = true });
            }
            else if (item.ViewModelType == typeof(InfoViewModel))
            {
                ShowViewModel<InfoViewModel>(new { isSiteView = true });
            }
            else if (item.ViewModelType == typeof(WiFiViewModel))
            {
                ShowViewModel<WiFiViewModel>(new { isSiteView = true });
            }
            else if (item.ViewModelType == typeof(BatteryInfoViewModel))
            {
                ShowViewModel<BatteryInfoViewModel>(new { isSiteView = true });
            }
            else if (item.ViewModelType == typeof(EnergyManagementViewModel))
            {
                ShowViewModel<EnergyManagementViewModel>(new { isSiteView = true });
            }
            else if (item.ViewModelType == typeof(PmInfoViewModel))
            {
                // In SiteView, Power Module is PM Info
                ShowViewModel<PmInfoViewModel>(new { isSiteView = true });
            }
        }

        public IMvxCommand ApplySettingsCommand
        {
            get
            {
                return
                    new MvxCommand<DeviceDeatilsItem>
                    ((obj) => ExecuteApplySettingsCommand());
            }
        }

        void ExecuteApplySettingsCommand()
        {
            ShowViewModel<SiteViewInProgressDevicesViewModel>
            (
                new
                {
                    type = SiteViewInProgressTypes.SAVE_SETTINGS
                }
            );
        }

        public IMvxCommand CancelSettingsCommand
        {
            get
            {
                return new MvxCommand<DeviceDeatilsItem>(ExecuteCancelSettingsCommand);
            }
        }

        void ExecuteCancelSettingsCommand(DeviceDeatilsItem obj)
        {
            controller.CancelSettings();

            GoBack();
        }

        public override void OnBackButtonClick()
        {
            base.OnBackButtonClick();

            GoBack();
        }

        void GoBack()
        {
            ShowViewModel<SiteViewSettingsHomeViewModel>(new { pop = "pop" });
        }
    }
}
