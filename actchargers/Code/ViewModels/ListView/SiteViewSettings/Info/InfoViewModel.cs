namespace actchargers
{
    public class InfoViewModel : SiteViewSettingsBaseViewModel
    {
        public InfoViewModel()
        {
            if (IsBattView)
                ViewTitle = AppResources.batt_view_info;
            else
                ViewTitle = AppResources.charger_info;
        }

        internal override void InitController()
        {
            controller = new InfoController(IsSiteView, IsBattView);
        }

        public override void OnBackButtonClick()
        {
            base.OnBackButtonClick();

            ShowViewModel<InfoViewModel>(new { pop = "pop" });
        }
    }
}