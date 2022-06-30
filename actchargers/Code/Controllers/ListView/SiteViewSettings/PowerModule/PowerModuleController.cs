namespace actchargers
{
    public class PowerModuleController : SiteViewSettingsBaseController
    {
        public PowerModuleController(bool isSiteView) : base(isSiteView, false)
        {
            if (isSiteView)
                subController = new PowerModuleSiteViewSubController();
            else
                subController = new PowerModuleDeviceSubController();
        }
    }
}
