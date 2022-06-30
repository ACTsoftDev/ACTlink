namespace actchargers
{
    public class PmInfoViewModel : SiteViewSettingsBaseViewModel
    {
        public PmInfoViewModel()
        {
            ViewTitle = AppResources.pm_info;
        }

        internal override void InitController()
        {
            controller = new PmInfoController(IsSiteView);
        }

        public override void OnBackButtonClick()
        {
            base.OnBackButtonClick();

            ShowViewModel<PmInfoViewModel>(new { pop = "pop" });
        }
    }
}
