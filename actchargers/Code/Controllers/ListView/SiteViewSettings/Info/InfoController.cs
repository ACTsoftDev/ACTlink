namespace actchargers
{
    public class InfoController : SiteViewSettingsBaseController
    {
        public InfoController(bool isSiteView, bool isBattView) : base(isSiteView, isBattView)
        {
            if (isSiteView)
                subController = new InfoSiteViewSubController(isBattView);
            else
                subController = new InfoDeviceSubController(isBattView);
        }
    }
}
