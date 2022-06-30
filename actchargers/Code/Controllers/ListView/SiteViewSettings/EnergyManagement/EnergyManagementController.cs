namespace actchargers
{
    public class EnergyManagementController : SiteViewSettingsBaseController
    {
        public EnergyManagementController(bool isSiteView) : base(isSiteView, false)
        {
            if (isSiteView)
                subController = new EnergyManagementSiteViewSubController();
            else
                subController = new EnergyManagementDeviceSubController();
        }
    }
}
