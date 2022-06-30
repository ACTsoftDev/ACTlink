namespace actchargers
{
    public class EnergyManagementViewModel : SiteViewSettingsBaseViewModel
    {
        public EnergyManagementViewModel()
        {
            ViewTitle = AppResources.energy_management;
        }

        internal override void InitController()
        {
            controller = new EnergyManagementController(IsSiteView);
        }

        public override void OnBackButtonClick()
        {
            base.OnBackButtonClick();

            ShowViewModel<EnergyManagementViewModel>(new { pop = "pop" });
        }
    }
}