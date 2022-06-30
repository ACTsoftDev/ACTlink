namespace actchargers
{
    public class FinishAndEQSettingsSiteViewSubController : FinishAndEQSettingsBaseSubController
    {
        public FinishAndEQSettingsSiteViewSubController
        (bool isBattView, bool isBattViewMobile) : base(isBattView, isBattViewMobile, true)
        {
        }

        internal override void LoadBattViewMobileValues()
        {
        }

        internal override void LoadRegularBattViewValues()
        {
        }

        internal override void LoadMcbValues()
        {
        }

        internal override int McbAccessApply()
        {
            return GetSharedMcbAccessApply(false).GetVisibleCount();
        }
    }
}
