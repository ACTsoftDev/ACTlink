namespace actchargers
{
    public class PmInfoSiteViewSubController : PmInfoBaseSubController
    {
        ListViewItem MCB_NumberOfInstalledPMsTextBox;

        public PmInfoSiteViewSubController() : base(true)
        {
        }

        #region Init Items

        internal override void InitExclusiveMcbItems()
        {
        }

        #endregion

        #region Load MCB

        internal override void LoadMcbValues()
        {
        }

        #endregion

        #region Add MCB

        internal override int McbAccessApply()
        {
            accessControlUtility = SharedMcbAccessApply();

            ShowEdit &= accessControlUtility.GetSavedCount() != 0;

            return accessControlUtility.GetVisibleCount();
        }

        #endregion

        #region Save MCB

        internal override VerifyControl VerfiyMcbSettings()
        {
            return VerfiySharedMcbSettings();
        }

        internal override void SaveMcbToConfigObject(MCBobject device)
        {
            SaveSharedMcbToConfigObject(device);
        }

        #endregion
    }
}
