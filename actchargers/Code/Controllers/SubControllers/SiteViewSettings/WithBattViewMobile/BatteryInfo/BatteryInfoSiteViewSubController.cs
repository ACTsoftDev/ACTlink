namespace actchargers
{
    public class BatteryInfoSiteViewSubController : BatteryInfoBaseSubController
    {
        public BatteryInfoSiteViewSubController(bool isBattView, bool isBattViewMobile)
            : base(isBattView, isBattViewMobile, true)
        {
        }

        internal override void InitExclusiveBattViewMobileItems()
        {
            InitCrossDeviceItems();
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

        internal override int BattViewMobileAccessApply()
        {
            AddCrossDeviceItems();

            return ItemSource.Count;
        }

        internal override int RegularBattViewAccessApply()
        {
            AddCrossDeviceItems();

            return ItemSource.Count;
        }

        internal override int McbAccessApply()
        {
            AddCrossDeviceItems();

            return ItemSource.Count;
        }
    }
}
