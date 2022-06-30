namespace actchargers
{
    public class CableSettingsController : SiteViewSettingsBaseController
    {
        public CableSettingsController() : base(false, false)
        {
            subController = new CableSettingsSubController();
        }
    }
}
