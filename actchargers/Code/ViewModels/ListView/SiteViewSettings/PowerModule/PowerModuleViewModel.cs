namespace actchargers
{
    public class PowerModuleViewModel : SiteViewSettingsBaseViewModel
    {
        public PowerModuleViewModel()
        {
            ViewTitle = AppResources.power_module;
        }

        internal override void InitController()
        {
            controller = new PowerModuleController(IsSiteView);
        }

        internal override void ExecuteItemClickCommnad(ListViewItem item)
        {
            base.ExecuteItemClickCommnad(item);

            if (item.ViewModelType == typeof(PmInfoViewModel))
            {
                ShowViewModel<PmInfoViewModel>();
            }
            else if (item.ViewModelType == typeof(PMFaultsViewModel))
            {
                ShowViewModel<PMFaultsViewModel>();
            }
            else if (item.ViewModelType == typeof(PmLiveViewModel))
            {
                ShowViewModel<PmLiveViewModel>();
            }
        }

        public override void OnBackButtonClick()
        {
            base.OnBackButtonClick();

            ShowViewModel<PowerModuleViewModel>(new { pop = "pop" });
        }
    }
}