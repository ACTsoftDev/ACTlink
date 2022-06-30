namespace actchargers
{
    public class DefaultChargeProfileController : WithBattViewMobileBaseController
    {
        public DefaultChargeProfileController
        (bool isSiteView, bool isBattView, bool isBattViewMobile)
            : base(isSiteView, isBattView, isBattViewMobile)
        {
            if (isSiteView)
                subController = new DefaultChargeProfileSiteViewSubController(isBattView, isBattViewMobile);
            else
                subController = new DefaultChargeProfileDeviceSubController(isBattView, isBattViewMobile);
        }
    }
}
