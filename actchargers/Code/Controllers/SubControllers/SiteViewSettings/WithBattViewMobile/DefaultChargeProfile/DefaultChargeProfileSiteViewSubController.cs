using System.Threading.Tasks;

namespace actchargers
{
    public class DefaultChargeProfileSiteViewSubController : DefaultChargeProfileBaseSubController
    {
        public DefaultChargeProfileSiteViewSubController
        (bool isBattView, bool isBattViewMobile) : base(isBattView, isBattViewMobile, true)
        {
        }

        internal override void InitExclusiveBattViewMobileItems()
        {
        }

        internal override void InitExclusiveRegularBattViewItems()
        {
        }

        internal override void InitExclusiveMcbItems()
        {
        }

        internal override void LoadBattViewMobileValues()
        {
        }

        #region Load BattView Values

        internal override void LoadRegularBattViewValues()
        {
            Task.Run(LoadRegularBattViewValuesTask);
        }

        async Task LoadRegularBattViewValuesTask()
        {
            await LoadLists(false);
        }

        internal override string BattViewFormatRate(float percent, float select)
        {
            return percent.ToString() + "%";
        }

        #endregion

        #region Load MCB Values

        internal override void LoadMcbValues()
        {
            Task.Run(LoadMcbValuesTask);
        }

        async Task LoadMcbValuesTask()
        {
            await LoadLists(false);
        }

        internal override int MultiplyMaxFiIfuseNewEastPennProfile(int maxFI)
        {
            return maxFI;
        }

        internal override float GetBattViewCcRate()
        {
            return 100.0f;
        }

        internal override float GetMcbCcRate()
        {
            return 100.0f;
        }

        internal override string McbFormatRate(float percent, float select)
        {
            return percent.ToString() + "%";
        }

        #endregion

        #region Load Defaults

        public override void LoadDefaults()
        {
        }

        #endregion

        internal override int BattViewMobileAccessApply()
        {
            return 0;
        }

        internal override int RegularBattViewAccessApply()
        {
            return GetSharedBattViewAccessApply().GetVisibleCount();
        }

        internal override int McbAccessApply()
        {
            return GetSharedMcbAccessApply(false).GetVisibleCount();
        }

        internal override VerifyControl VerfiyBattViewMobileSettings()
        {
            return new VerifyControl();
        }

        internal override VerifyControl VerfiyRegularBattViewSettings()
        {
            return VerfiyCrossSharedSettings(false);
        }

        internal override VerifyControl VerfiyMcbSettings()
        {
            return VerfiyCrossSharedSettings(false);
        }

        internal override void SaveBattViewMobileToConfigObject(BattViewObject device)
        {
        }

        internal override void SaveBattViewRegularToConfigObject(BattViewObject device)
        {
            SaveSharedBattViewToConfigObject(device);
        }

        internal override void SaveMcbToConfigObject(MCBobject device)
        {
            SaveSharedMcbToConfigObject(device, false);
        }
    }
}
