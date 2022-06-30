namespace actchargers
{
    public class PmInfoController : SiteViewSettingsBaseController
    {
        public PmInfoController(bool isSiteView) : base(isSiteView, false)
        {
            if (isSiteView)
                subController = new PmInfoSiteViewSubController();
            else
                subController = new PmInfoDeviceSubController();
        }
    }
}
