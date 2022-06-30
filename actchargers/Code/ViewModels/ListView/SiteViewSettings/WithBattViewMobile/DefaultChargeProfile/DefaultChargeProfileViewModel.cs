namespace actchargers
{
    public class DefaultChargeProfileViewModel : SiteViewSettingsBaseViewModel
    {
        public DefaultChargeProfileViewModel()
        {
            ViewTitle = AppResources.default_charge_profile;
        }

        internal override void InitController()
        {
            controller = new DefaultChargeProfileController(IsSiteView, IsBattView, IsBattViewMobile);
        }

        public override void OnBackButtonClick()
        {
            base.OnBackButtonClick();

            ShowViewModel<DefaultChargeProfileViewModel>(new { pop = "pop" });
        }
    }
}
