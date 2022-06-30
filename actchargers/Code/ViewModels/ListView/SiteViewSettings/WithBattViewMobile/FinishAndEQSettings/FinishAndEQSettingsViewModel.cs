namespace actchargers
{
    public class FinishAndEQSettingsViewModel : SiteViewSettingsBaseViewModel
    {
        public FinishAndEQSettingsViewModel()
        {
            ViewTitle = AppResources.finish_eq_settings;
        }

        internal override void InitController()
        {
            controller = new FinishAndEQSettingsController(IsSiteView, IsBattView, IsBattViewMobile);
        }

        public override void OnBackButtonClick()
        {
            base.OnBackButtonClick();

            ShowViewModel<FinishAndEQSettingsViewModel>(new { pop = "pop" });
        }
    }
}
