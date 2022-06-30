using System;
using System.Diagnostics;
using System.Threading.Tasks;
using MvvmCross.Core.ViewModels;

namespace actchargers
{
    public class SettingsViewModel : SiteViewSettingsBaseViewModel
    {
        public string ResetLcdCalibrationTitle
        {
            get
            {
                return AppResources.reset_lcd;
            }
        }

        bool isResetLcdCalibrationVisible;
        public bool IsResetLcdCalibrationVisible
        {
            get
            {
                return isResetLcdCalibrationVisible;
            }
            set
            {
                SetProperty(ref isResetLcdCalibrationVisible, value);

                RaisePropertyChanged(() => IsResetLcdCalibrationHidden);
            }
        }

        public bool IsResetLcdCalibrationHidden
        {
            get
            {
                return !IsResetLcdCalibrationVisible;
            }
        }

        bool isResetLcdCalibrationEditEnabled;
        public bool IsResetLcdCalibrationEditEnabled
        {
            get
            {
                return isResetLcdCalibrationEditEnabled;
            }
            set
            {
                SetProperty(ref isResetLcdCalibrationEditEnabled, value);
            }
        }

        public SettingsViewModel()
        {
            if (IsBattView)
                ViewTitle = AppResources.battview_settings;
            else
                ViewTitle = AppResources.charger_settings;
        }

        internal override void InitController()
        {
            controller = new SettingsController(IsSiteView, IsBattView);
        }

        internal override void InitEvents()
        {
            base.InitEvents();

            InitSettingsEvents();
        }

        void InitSettingsEvents()
        {
            try
            {
                TryInitSettingsEvents();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        void TryInitSettingsEvents()
        {
            SettingsController settingsController = controller as SettingsController;

            settingsController.OnIsResetLcdCalibrationVisibleChanged += SettingsController_OnIsResetLcdCalibrationVisibleChanged;
            settingsController.OnIsResetLcdCalibrationEditEnabledChanged += SettingsController_OnIsResetLcdCalibrationEditEnabledChanged;
            settingsController.OnNavigatingToCableSettings += SettingsController_OnNavigatingToCableSettings;
        }

        void SettingsController_OnIsResetLcdCalibrationVisibleChanged(object sender, bool e)
        {
            IsResetLcdCalibrationVisible = e;
        }

        void SettingsController_OnIsResetLcdCalibrationEditEnabledChanged(object sender, bool e)
        {
            IsResetLcdCalibrationEditEnabled = e;
        }

        void SettingsController_OnNavigatingToCableSettings(object sender, EventArgs e)
        {
            ShowViewModel<CableSettingsViewModel>();
        }

        public IMvxCommand McbResetLcdCalibrationCommand
        {
            get
            {
                return new MvxAsyncCommand(OnMcbResetLcdCalibration);
            }
        }

        async Task OnMcbResetLcdCalibration()
        {
            try
            {
                await TryOnMcbResetLcdCalibration();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        async Task TryOnMcbResetLcdCalibration()
        {
            SettingsController settingsController = controller as SettingsController;

            await settingsController.McbResetLcdCalibration();
        }

        public override void OnBackButtonClick()
        {
            base.OnBackButtonClick();

            ShowViewModel<SettingsViewModel>(new { pop = "pop" });
        }
    }
}
