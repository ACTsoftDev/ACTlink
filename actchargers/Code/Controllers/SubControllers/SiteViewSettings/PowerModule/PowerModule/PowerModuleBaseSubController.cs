namespace actchargers
{
    public abstract class PowerModuleBaseSubController : SiteViewSettingsBaseSubController
    {
        protected PowerModuleBaseSubController(bool isSiteView) : base(false, isSiteView)
        {
        }

        internal override void InitSharedBattViewItems()
        {
        }

        internal override void InitSharedMcbItems()
        {
        }

        internal override void InitExclusiveBattViewItems()
        {
        }

        internal override void LoadBattViewValues()
        {
        }

        internal override void LoadExclusiveValues()
        {
        }

        public override void LoadDefaults()
        {
        }

        internal override int BattViewAccessApply()
        {
            return 0;
        }

        internal override void AddExclusiveItems()
        {
        }

        internal override VerifyControl VerfiyBattViewSettings()
        {
            return new VerifyControl();
        }

        internal override void SaveBattViewToConfigObject(BattViewObject device)
        {
        }
    }
}
