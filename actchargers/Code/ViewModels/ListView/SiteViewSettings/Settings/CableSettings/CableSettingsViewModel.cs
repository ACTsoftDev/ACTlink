namespace actchargers
{
    public class CableSettingsViewModel : SiteViewSettingsBaseViewModel
    {
        public CableSettingsViewModel()
        {
            ViewTitle = AppResources.cable_settings;
        }

        internal override void InitController()
        {
            controller = new CableSettingsController();
        }

        public override void OnBackButtonClick()
        {
            base.OnBackButtonClick();

            ShowViewModel<CableSettingsViewModel>(new { pop = "pop" });
        }
    }
}
