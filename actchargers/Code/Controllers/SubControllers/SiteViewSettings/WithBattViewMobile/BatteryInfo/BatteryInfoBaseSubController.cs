using System.Threading.Tasks;
using System.Diagnostics;

namespace actchargers
{
    public abstract class BatteryInfoBaseSubController : WithBattViewMobileBaseSubController
    {
        internal ListViewItem BatterySettings;
        internal ListViewItem DefaultChargeProfile;
        internal ListViewItem FinishEqScheduling;

        protected BatteryInfoBaseSubController
        (bool isBattView, bool isBattViewMobile, bool isSiteView)
            : base(isBattView, isBattViewMobile, isSiteView)
        {
        }

        public override async Task Start()
        {
            try
            {
                await TryStart();
            }
            catch (System.Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        async Task TryStart()
        {
            await base.Start();

            ShowEdit = false;

            await ChangeEditMode(false);
        }

        internal override void InitSharedBattViewMobileItems()
        {
            InitBatterySettings();
        }

        internal override void InitSharedRegularBattViewItems()
        {
            InitCrossDeviceItems();
        }

        internal override void InitSharedMcbItems()
        {
            InitCrossDeviceItems();
        }

        internal void InitCrossDeviceItems()
        {
            InitBatterySettings();
            InitCrossSharedDeviceItems();
        }

        void InitBatterySettings()
        {
            BatterySettings = new ListViewItem
            {
                Title = AppResources.battery_settings,
                ViewModelType = typeof(BatterySettingsViewModel),
                DefaultCellType = ACUtility.CellTypes.Label
            };
        }

        void InitCrossSharedDeviceItems()
        {
            DefaultChargeProfile = new ListViewItem
            {
                Title = AppResources.default_charge_profile,
                ViewModelType = typeof(DefaultChargeProfileViewModel),
                DefaultCellType = ACUtility.CellTypes.Label
            };
            FinishEqScheduling = new ListViewItem
            {
                Title = AppResources.finish_eq_scheduling,
                ViewModelType = typeof(FinishAndEQSettingsViewModel),
                DefaultCellType = ACUtility.CellTypes.Label
            };
        }

        internal override void LoadRegularBattViewValues()
        {
        }

        internal override void LoadMcbValues()
        {
        }

        internal override void LoadExclusiveValues()
        {
        }

        internal void AddCrossDeviceItems()
        {
            AddBatterySettings();
            AddCrossSharedDeviceItems();
        }

        internal void AddBatterySettings()
        {
            ItemSource.Add(BatterySettings);
        }

        internal void AddCrossSharedDeviceItems()
        {
            ItemSource.Add(DefaultChargeProfile);
            ItemSource.Add(FinishEqScheduling);
        }

        public override void LoadDefaults()
        {
        }

        internal override void AddExclusiveItems()
        {
        }

        internal override VerifyControl VerfiyBattViewMobileSettings()
        {
            return new VerifyControl();
        }

        internal override VerifyControl VerfiyRegularBattViewSettings()
        {
            return new VerifyControl();
        }

        internal override VerifyControl VerfiyMcbSettings()
        {
            return new VerifyControl();
        }

        internal override void SaveBattViewMobileToConfigObject(BattViewObject device)
        {
        }

        internal override void SaveBattViewRegularToConfigObject(BattViewObject device)
        {
        }

        internal override void SaveMcbToConfigObject(MCBobject device)
        {
        }
    }
}
