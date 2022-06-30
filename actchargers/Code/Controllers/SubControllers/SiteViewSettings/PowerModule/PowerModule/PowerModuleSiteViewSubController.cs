namespace actchargers
{
    public class PowerModuleSiteViewSubController : PowerModuleBaseSubController
    {
        public PowerModuleSiteViewSubController() : base(true)
        {
        }

        #region Init Items

        internal override void InitExclusiveMcbItems()
        {
        }

        #endregion

        internal override void LoadMcbValues()
        {
        }

        #region Add MCB

        internal override int McbAccessApply()
        {
            return 0;
        }

        #endregion

        #region Save MCB

        internal override VerifyControl VerfiyMcbSettings()
        {
            return new VerifyControl();
        }

        internal override void SaveMcbToConfigObject(MCBobject device)
        {
        }

        #endregion
    }
}
