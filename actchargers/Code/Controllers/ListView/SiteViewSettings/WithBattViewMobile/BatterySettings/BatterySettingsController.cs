namespace actchargers
{
    public class BatterySettingsController : WithBattViewMobileBaseController
    {
        public BatterySettingsController
        (bool isSiteView, bool isBattView, bool isBattViewMobile)
            : base(isSiteView, isBattView, isBattViewMobile)
        {
            if (isSiteView)
                subController = new BatterySettingsSiteViewSubController(isBattView, isBattViewMobile);
            else
                subController = new BatterySettingsDeviceSubController(isBattView, isBattViewMobile);
        }
    }
}
