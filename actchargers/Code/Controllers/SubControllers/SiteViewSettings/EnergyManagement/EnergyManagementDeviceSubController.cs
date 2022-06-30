namespace actchargers
{
    public class EnergyManagementDeviceSubController : EnergyManagementBaseSubController
    {
        public EnergyManagementDeviceSubController() : base(false)
        {
        }

        internal override void LoadBattViewValues()
        {
        }

        #region Load MCB

        internal override void LoadMcbValues()
        {
            var config = MCBQuantum.Instance.GetMCB().Config;

            uint diff;

            (MCB_Energy_lockoutDays.Items[6] as DayViewItem).IsSelected = (MCB_Energy_lockoutDays.Items[6] as DayViewItem).OriginalValue = config.lockoutDaysMask.Sunday;
            (MCB_Energy_lockoutDays.Items[0] as DayViewItem).IsSelected = (MCB_Energy_lockoutDays.Items[0] as DayViewItem).OriginalValue = config.lockoutDaysMask.Monday;
            (MCB_Energy_lockoutDays.Items[1] as DayViewItem).IsSelected = (MCB_Energy_lockoutDays.Items[1] as DayViewItem).OriginalValue = config.lockoutDaysMask.Tuesday;
            (MCB_Energy_lockoutDays.Items[2] as DayViewItem).IsSelected = (MCB_Energy_lockoutDays.Items[2] as DayViewItem).OriginalValue = config.lockoutDaysMask.Wednesday;
            (MCB_Energy_lockoutDays.Items[3] as DayViewItem).IsSelected = (MCB_Energy_lockoutDays.Items[3] as DayViewItem).OriginalValue = config.lockoutDaysMask.Thursday;
            (MCB_Energy_lockoutDays.Items[4] as DayViewItem).IsSelected = (MCB_Energy_lockoutDays.Items[4] as DayViewItem).OriginalValue = config.lockoutDaysMask.Friday;
            (MCB_Energy_lockoutDays.Items[5] as DayViewItem).IsSelected = (MCB_Energy_lockoutDays.Items[5] as DayViewItem).OriginalValue = config.lockoutDaysMask.Saturday;

            MCB_Energy_lockoutStartTime.SubTitle = MCB_Energy_lockoutStartTime.Text = string.Format("{0:00}:{1:00}", config.lockoutStartTime / 3600, ((config.lockoutStartTime % 3600) / 60));

            diff = config.lockoutStartTime < config.lockoutCloseTime ? config.lockoutCloseTime - config.lockoutStartTime : 86400 + config.lockoutCloseTime - config.lockoutStartTime;
            MCB_Energy_lockoutWindow.SelectedItem = string.Format("{0:00}:{1:00}", (diff / 3600), (diff % 3600) / 60);

            (MCB_Energy_powerDays.Items[6] as DayViewItem).IsSelected = (MCB_Energy_powerDays.Items[6] as DayViewItem).OriginalValue = config.energyDaysMask.Sunday;
            (MCB_Energy_powerDays.Items[0] as DayViewItem).IsSelected = (MCB_Energy_powerDays.Items[0] as DayViewItem).OriginalValue = config.energyDaysMask.Monday;
            (MCB_Energy_powerDays.Items[1] as DayViewItem).IsSelected = (MCB_Energy_powerDays.Items[1] as DayViewItem).OriginalValue = config.energyDaysMask.Tuesday;
            (MCB_Energy_powerDays.Items[2] as DayViewItem).IsSelected = (MCB_Energy_powerDays.Items[2] as DayViewItem).OriginalValue = config.energyDaysMask.Wednesday;
            (MCB_Energy_powerDays.Items[3] as DayViewItem).IsSelected = (MCB_Energy_powerDays.Items[3] as DayViewItem).OriginalValue = config.energyDaysMask.Thursday;
            (MCB_Energy_powerDays.Items[4] as DayViewItem).IsSelected = (MCB_Energy_powerDays.Items[4] as DayViewItem).OriginalValue = config.energyDaysMask.Friday;
            (MCB_Energy_powerDays.Items[5] as DayViewItem).IsSelected = (MCB_Energy_powerDays.Items[5] as DayViewItem).OriginalValue = config.energyDaysMask.Saturday;

            MCB_Energy_powerfactor.SelectedItem = config.energyDecreaseValue.ToString();

            MCB_Energy_powerStartTime.SubTitle = MCB_Energy_powerStartTime.Text = string.Format("{0:00}:{1:00}", config.energyStartTime / 3600, ((config.energyStartTime % 3600) / 60));
            MCB_Energy_lockoutStartTime.SubTitle = MCB_Energy_lockoutStartTime.Text = string.Format("{0:00}:{1:00}", config.lockoutStartTime / 3600, ((config.lockoutStartTime % 3600) / 60));

            diff = config.energyStartTime < config.energyCloseTime ? config.energyCloseTime - config.energyStartTime : 86400 + config.energyCloseTime - config.energyStartTime;

            MCB_Energy_powerWindow.SelectedItem = string.Format("{0:00}:{1:00}", (diff / 3600), (diff % 3600) / 60);

            SetInfoText(false);
        }

        #endregion
    }
}
