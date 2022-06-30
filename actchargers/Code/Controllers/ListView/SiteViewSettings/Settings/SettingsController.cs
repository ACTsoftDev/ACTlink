using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace actchargers
{
    public class SettingsController : SiteViewSettingsBaseController
    {
        public event EventHandler<bool> OnIsResetLcdCalibrationVisibleChanged;
        public event EventHandler<bool> OnIsResetLcdCalibrationEditEnabledChanged;
        public event EventHandler OnNavigatingToCableSettings;

        bool isResetLcdCalibrationVisible;
        public bool IsResetLcdCalibrationVisible
        {
            get
            {
                return isResetLcdCalibrationVisible;
            }
            set
            {
                isResetLcdCalibrationVisible = value;

                FireOnIsResetLcdCalibrationVisibleChanged();
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
                isResetLcdCalibrationEditEnabled = value;

                FireOnIsResetLcdCalibrationEditEnabledChanged();
            }
        }

        public SettingsController(bool isSiteView, bool isBattView) : base(isSiteView, isBattView)
        {
            if (isSiteView)
                subController = new SettingsSiteViewSubController(isBattView);
            else
                subController = new SettingsDeviceSubController(isBattView);
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
            SettingsBaseSubController settingsSubController = subController as SettingsBaseSubController;

            settingsSubController.OnIsResetLcdCalibrationVisibleChanged += SettingsSubController_OnIsResetLcdCalibrationVisibleChanged;
            settingsSubController.OnIsResetLcdCalibrationEditEnabledChanged += SettingsSubController_OnIsResetLcdCalibrationEditEnabledChanged;
            settingsSubController.OnNavigatingToCableSettings += SettingsSubController_OnNavigatingToCableSettings;
        }

        void SettingsSubController_OnIsResetLcdCalibrationVisibleChanged(object sender, bool e)
        {
            IsResetLcdCalibrationVisible = e;
        }

        void SettingsSubController_OnIsResetLcdCalibrationEditEnabledChanged(object sender, bool e)
        {
            IsResetLcdCalibrationEditEnabled = e;
        }

        void SettingsSubController_OnNavigatingToCableSettings(object sender, EventArgs e)
        {
            FireOnNavigatingToCableSettings();
        }

        public async Task McbResetLcdCalibration()
        {
            try
            {
                await TryMcbResetLcdCalibration();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        async Task TryMcbResetLcdCalibration()
        {
            SettingsBaseSubController settingsSubController = subController as SettingsBaseSubController;

            await settingsSubController.McbResetLcdCalibration();
        }

        void FireOnIsResetLcdCalibrationVisibleChanged()
        {
            OnIsResetLcdCalibrationVisibleChanged?.Invoke(this, IsResetLcdCalibrationVisible);
        }

        void FireOnIsResetLcdCalibrationEditEnabledChanged()
        {
            OnIsResetLcdCalibrationEditEnabledChanged?.Invoke(this, IsResetLcdCalibrationEditEnabled);
        }

        void FireOnNavigatingToCableSettings()
        {
            OnNavigatingToCableSettings?.Invoke(this, EventArgs.Empty);
        }
    }
}
