namespace actchargers
{
    public class BatteryInfoController : WithBattViewMobileBaseController
    {
        public BatteryInfoController
        (bool isSiteView, bool isBattView, bool isBattViewMobile)
            : base(isSiteView, isBattView, isBattViewMobile)
        {
            if (isSiteView)
                subController = new BatteryInfoSiteViewSubController(isBattView, isBattViewMobile);
            else
                subController = new BatteryInfoDeviceSubController(isBattView, isBattViewMobile);
        }
    }
}
