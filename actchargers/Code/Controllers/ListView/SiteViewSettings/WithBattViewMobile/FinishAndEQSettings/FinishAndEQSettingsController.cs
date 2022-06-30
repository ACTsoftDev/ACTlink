namespace actchargers
{
    public class FinishAndEQSettingsController : WithBattViewMobileBaseController
    {
        public FinishAndEQSettingsController
        (bool isSiteView, bool isBattView, bool isBattViewMobile)
            : base(isSiteView, isBattView, isBattViewMobile)
        {
            if (isSiteView)
                subController = new FinishAndEQSettingsSiteViewSubController(isBattView, isBattViewMobile);
            else
                subController = new FinishAndEQSettingsDeviceSubController(isBattView, isBattViewMobile);
        }
    }
}
