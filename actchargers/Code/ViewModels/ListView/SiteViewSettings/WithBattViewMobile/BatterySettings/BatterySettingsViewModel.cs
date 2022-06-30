namespace actchargers
{
    public class BatterySettingsViewModel : SiteViewSettingsBaseViewModel
    {
        public BatterySettingsViewModel()
        {
            ViewTitle = AppResources.battery_settings;
        }

        internal override void InitController()
        {
            controller = new BatterySettingsController(IsSiteView, IsBattView, IsBattViewMobile);
        }

        public override void OnBackButtonClick()
        {
            base.OnBackButtonClick();

            ShowViewModel<BatterySettingsViewModel>(new { pop = "pop" });
        }
    }
}
