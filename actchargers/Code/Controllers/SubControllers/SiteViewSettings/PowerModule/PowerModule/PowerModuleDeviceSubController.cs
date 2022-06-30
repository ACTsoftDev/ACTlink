using System.Threading.Tasks;

namespace actchargers
{
    public class PowerModuleDeviceSubController : PowerModuleBaseSubController
    {
        ListViewItem PmInfo;
        ListViewItem PmFaults;
        ListViewItem PmLive;

        public PowerModuleDeviceSubController() : base(false)
        {
        }

        public override async Task Start()
        {
            await base.Start();

            ShowEdit = false;
        }

        #region Init Items

        internal override void InitExclusiveMcbItems()
        {
            PmInfo = new ListViewItem
            {
                Index = 0,
                Title = AppResources.pm_info,
                ViewModelType = typeof(PmInfoViewModel),
                DefaultCellType = ACUtility.CellTypes.Label,
                EditableCellType = ACUtility.CellTypes.Label
            };

            PmFaults = new ListViewItem
            {
                Index = 1,
                Title = AppResources.pm_faults,
                ViewModelType = typeof(PMFaultsViewModel),
                DefaultCellType = ACUtility.CellTypes.Label,
                EditableCellType = ACUtility.CellTypes.Label
            };

            PmLive = new ListViewItem
            {
                Index = 2,
                Title = AppResources.pm_live_title,
                ViewModelType = typeof(PmLiveViewModel),
                DefaultCellType = ACUtility.CellTypes.Label,
                EditableCellType = ACUtility.CellTypes.Label
            };
        }

        #endregion

        internal override void LoadMcbValues()
        {
        }

        internal override int McbAccessApply()
        {
            accessControlUtility = new UIAccessControlUtility();

            accessControlUtility.DoApplyAccessControl(AccessLevelConsts.readOnly, PmInfo, ItemSource);
            accessControlUtility.DoApplyAccessControl(AccessLevelConsts.readOnly, PmFaults, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_PM_LiveView, PmLive, ItemSource);

            return accessControlUtility.GetVisibleCount();
        }

        internal override VerifyControl VerfiyMcbSettings()
        {
            return new VerifyControl();
        }

        internal override void SaveMcbToConfigObject(MCBobject device)
        {
        }
    }
}
