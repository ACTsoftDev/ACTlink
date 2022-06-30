namespace actchargers
{
    public class WiFiController : SiteViewSettingsBaseController
    {
        public WiFiController(bool isSiteView, bool isBattView) : base(isSiteView, isBattView)
        {
            if (isSiteView)
                subController = new WiFiSiteViewSubController(isBattView);
            else
                subController = new WiFiDeviceSubController(isBattView);
        }
    }
}
