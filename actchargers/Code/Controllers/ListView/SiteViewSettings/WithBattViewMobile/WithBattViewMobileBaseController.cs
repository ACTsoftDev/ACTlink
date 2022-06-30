namespace actchargers
{
    public abstract class WithBattViewMobileBaseController : SiteViewSettingsBaseController
    {
        internal bool isBattViewMobile;

        protected WithBattViewMobileBaseController
        (bool isSiteView, bool isBattView, bool isBattViewMobile) : base(isSiteView, isBattView)
        {
            this.isBattViewMobile = isBattViewMobile;
        }
    }
}
