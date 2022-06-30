namespace actchargers
{
    public abstract class SiteViewSettingsBaseController : ListViewBaseController
    {
        internal readonly bool isSiteView;

        protected SiteViewSettingsBaseController(bool isSiteView, bool isBattView) : base(isBattView)
        {
            this.isSiteView = isSiteView;
        }
    }
}
