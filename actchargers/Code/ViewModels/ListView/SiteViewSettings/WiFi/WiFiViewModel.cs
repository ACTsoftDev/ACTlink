namespace actchargers
{
    public class WiFiViewModel : SiteViewSettingsBaseViewModel
    {
        public WiFiViewModel()
        {
            ViewTitle = AppResources.wifi;
        }

        internal override void InitController()
        {
            controller = new WiFiController(IsSiteView, IsBattView);
        }

        public override void OnBackButtonClick()
        {
            base.OnBackButtonClick();

            ShowViewModel<WiFiViewModel>(new { pop = "pop" });
        }
    }
}
