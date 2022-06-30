namespace actchargers
{
    public abstract class WithBattViewMobileBaseSubController : SiteViewSettingsBaseSubController
    {
        internal bool isBattViewMobile;

        protected WithBattViewMobileBaseSubController
        (bool isBattView, bool isBattViewMobile, bool isSiteView) : base(isBattView, isSiteView)
        {
            this.isBattViewMobile = isBattViewMobile;
        }

        internal override void InitSharedBattViewItems()
        {
            if (isBattViewMobile)
                InitSharedBattViewMobileItems();
            else
                InitSharedRegularBattViewItems();
        }

        internal abstract void InitSharedBattViewMobileItems();

        internal abstract void InitSharedRegularBattViewItems();

        internal override void InitExclusiveBattViewItems()
        {
            if (isBattViewMobile)
                InitExclusiveBattViewMobileItems();
            else
                InitExclusiveRegularBattViewItems();
        }

        internal abstract void InitExclusiveBattViewMobileItems();

        internal abstract void InitExclusiveRegularBattViewItems();

        internal override void LoadBattViewValues()
        {
            if (isBattViewMobile)
                LoadBattViewMobileValues();
            else
                LoadRegularBattViewValues();
        }

        internal abstract void LoadBattViewMobileValues();

        internal abstract void LoadRegularBattViewValues();

        internal override int BattViewAccessApply()
        {
            if (isBattViewMobile)
                return BattViewMobileAccessApply();

            return RegularBattViewAccessApply();
        }

        internal abstract int BattViewMobileAccessApply();

        internal abstract int RegularBattViewAccessApply();

        internal override VerifyControl VerfiyBattViewSettings()
        {
            if (isBattViewMobile)
                return VerfiyBattViewMobileSettings();

            return VerfiyRegularBattViewSettings();
        }

        internal abstract VerifyControl VerfiyBattViewMobileSettings();

        internal abstract VerifyControl VerfiyRegularBattViewSettings();

        internal override void SaveBattViewToConfigObject(BattViewObject device)
        {
            if (isBattViewMobile)
                SaveBattViewMobileToConfigObject(device);
            else
                SaveBattViewRegularToConfigObject(device);
        }

        internal abstract void SaveBattViewMobileToConfigObject(BattViewObject device);

        internal abstract void SaveBattViewRegularToConfigObject(BattViewObject device);
    }
}
