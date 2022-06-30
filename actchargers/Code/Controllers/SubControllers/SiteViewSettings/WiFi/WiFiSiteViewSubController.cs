namespace actchargers
{
    public class WiFiSiteViewSubController : WiFiBaseSubController
    {
        public WiFiSiteViewSubController(bool isBattView) : base(isBattView, true)
        {
        }

        internal override int GetActViewEnabledAccessLevel()
        {
            return ControlObject.UserAccess.Batt_actViewEnabled;
        }

        internal override void InitExclusiveBattViewItems()
        {
        }

        internal override void InitExclusiveMcbItems()
        {
        }

        internal override void LoadBattViewValues()
        {
            LoadSharedValuesValues();
        }

        internal override void LoadMcbValues()
        {
            LoadSharedValuesValues();
        }

        void LoadSharedValuesValues()
        {
            LoadDefaults();

            FireOnListChanged();
        }

        internal override void LoadExclusiveValues()
        {
        }

        internal override void AddExclusiveItems()
        {
        }
    }
}
