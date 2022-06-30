using System;
using System.Diagnostics;

namespace actchargers
{
    public class BatteryInfoViewModel : SiteViewSettingsBaseViewModel
    {
        public BatteryInfoViewModel()
        {
            ViewTitle = AppResources.battery_info;

            SetIsBattViewMobile();
        }

        void SetIsBattViewMobile()
        {
            try
            {
                TrySetIsBattViewMobile();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());

                IsBattViewMobile = false;
            }
        }

        void TrySetIsBattViewMobile()
        {
            if (IsLocalBattView())
                IsBattViewMobile = IsCurrentMobile();
            else
                IsBattViewMobile = false;
        }

        bool IsLocalBattView()
        {
            return IsBattView && !IsSiteView;
        }

        bool IsCurrentMobile()
        {
            var currentBattView = BattViewQuantum.Instance.GetBATTView();

            if (currentBattView == null)
                return false;

            return currentBattView.Config.IsBattViewMobile();
        }

        internal override void InitController()
        {
            controller = new BatteryInfoController(IsSiteView, IsBattView, IsBattViewMobile);
        }

        internal override void ExecuteItemClickCommnad(ListViewItem item)
        {
            base.ExecuteItemClickCommnad(item);

            if (item.ViewModelType == null)
                return;

            if (item.ViewModelType == typeof(BatterySettingsViewModel))
                ShowViewModel<BatterySettingsViewModel>(new { isSiteView = this.IsSiteView });

            else if (item.ViewModelType == typeof(NewStudyViewModel))
                ShowViewModel<NewStudyViewModel>(new { isSiteView = this.IsSiteView });

            else if (item.ViewModelType == typeof(FinishAndEQSettingsViewModel))
                ShowViewModel<FinishAndEQSettingsViewModel>(new { isSiteView = this.IsSiteView });

            else if (item.ViewModelType == typeof(DefaultChargeProfileViewModel))
                ShowViewModel<DefaultChargeProfileViewModel>(new { isSiteView = this.IsSiteView });
        }

        public override void OnBackButtonClick()
        {
            base.OnBackButtonClick();

            ShowViewModel<BatteryInfoViewModel>(new { pop = "pop" });
        }
    }
}
