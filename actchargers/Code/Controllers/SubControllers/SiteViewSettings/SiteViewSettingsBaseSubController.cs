using System.Threading.Tasks;

namespace actchargers
{
    public abstract class SiteViewSettingsBaseSubController : ListViewBaseSubController
    {
        internal readonly bool isSiteView;

        protected SiteViewSettingsBaseSubController(bool isBattView, bool isSiteView) : base(isBattView)
        {
            this.isSiteView = isSiteView;
        }

        public override async Task Start()
        {
            await base.Start();

            if (isSiteView)
                await ChangeEditMode(true);
        }

        internal override void InitItems()
        {
            base.InitItems();

            InitExclusiveItems();
        }

        internal void InitExclusiveItems()
        {
            if (isBattView)
                InitExclusiveBattViewItems();
            else
                InitExclusiveMcbItems();
        }

        internal abstract void InitExclusiveBattViewItems();

        internal abstract void InitExclusiveMcbItems();

        #region Save BattView

        internal override async Task SaveBattViewSettings()
        {
            if (isSiteView)
                await SaveBattViewSiteViewSettings();
            else
                await SaveBattViewDeviceSettings();
        }

        async Task SaveBattViewSiteViewSettings()
        {
            var devices = GlobalLists.GetActiveBattViewList();

            foreach (var item in devices)
                SaveOneBattViewSiteViewValue(item);

            await OnAfterSiteViewSaved();
        }

        void SaveOneBattViewSiteViewValue(BattViewObject item)
        {
            item.PrepareSiteViewConfig();

            SaveBattViewToConfigObject(item);
        }

        #endregion

        #region Save MCB

        internal override async Task SaveMcbSettings()
        {
            if (isSiteView)
                await SaveMcbSiteViewSettings();
            else
                await SaveMcbDeviceSettings();
        }

        async Task SaveMcbSiteViewSettings()
        {
            var devices = GlobalLists.GetActiveMcbList();

            foreach (var item in devices)
                SaveOneMcbSiteViewValue(item);

            await OnAfterSiteViewSaved();
        }

        void SaveOneMcbSiteViewValue(MCBobject item)
        {
            item.PrepareSiteViewConfig();

            SaveMcbToConfigObject(item);
        }

        #endregion

        async Task OnAfterSiteViewSaved()
        {
            await ChangeEditMode(false);
        }
    }
}
