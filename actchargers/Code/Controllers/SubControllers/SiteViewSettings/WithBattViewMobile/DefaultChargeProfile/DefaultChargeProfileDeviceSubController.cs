using System;
using System.Linq;
using static actchargers.ACUtility;
using System.Threading.Tasks;
using System.Diagnostics;

namespace actchargers
{
    public class DefaultChargeProfileDeviceSubController : DefaultChargeProfileBaseSubController
    {
        ListViewItem MCB_NewEastPennProfile;

        public DefaultChargeProfileDeviceSubController
        (bool isBattView, bool isBattViewMobile) : base(isBattView, isBattViewMobile, false)
        {
        }

        public async override Task Start()
        {
            await base.Start();

            IsRestoreEnable = true;
        }

        internal override void InitExclusiveBattViewMobileItems()
        {
        }

        internal override void InitExclusiveRegularBattViewItems()
        {
        }

        internal override void InitExclusiveMcbItems()
        {
            MCB_NewEastPennProfile = new ListViewItem()
            {
                Index = 13,
                Title = AppResources.new_east_penn_profile,
                DefaultCellType = CellTypes.LabelLabel,
                EditableCellType = CellTypes.LabelSwitch,
            };
        }

        internal override void LoadBattViewMobileValues()
        {
        }

        #region Load BattView Values

        internal override void LoadRegularBattViewValues()
        {
            Task.Run(LoadRegularBattViewValuesTask);
        }

        async Task LoadRegularBattViewValuesTask()
        {
            await LoadLists(false);

            var config = BattViewQuantum.Instance.GetBATTView().Config;

            Batt_trCurrentRateComboBox.SelectedItem = FormatRate((config.trickleCurrentRate / 100.0f), 1);

            Batt_ccCurrentRateComboBox.SelectedItem = FormatRate((config.CCrate / 100.0f), 1);

            Batt_fiCurrentRateComboBox.SelectedItem = FormatRate((config.FIcurrentRate / 100.0f), 1);

            Batt_eqCurrentRateComboBox.SelectedItem = FormatRate((config.EQcurrentRate / 100.0f), 1);

            Batt_trickleVoltageTextBox.SubTitle = Batt_trickleVoltageTextBox.Text = (config.trickleVoltage / 100.0f).ToString();
            Batt_cvVoltageTextBox.SubTitle = Batt_cvVoltageTextBox.Text = (config.CVTargetVoltage / 100.0f).ToString();
            Batt_finishVoltageTextBox.SubTitle = Batt_finishVoltageTextBox.Text = (config.FItargetVoltage / 100.0f).ToString();
            Batt_equalizeVoltageTextBox.SubTitle = Batt_equalizeVoltageTextBox.Text = (config.EQvoltage / 100.0f).ToString();

            if (config.CVcurrentStep == 0)
                Batt_cvCurrentStepComboBox.SelectedItem = "Default";

            Batt_cvFinishCurrentRateComboBox.SelectedItem = FormatRate((config.CVendCurrentRate / 2.0f), 1);

            BattViewCvCurrentStepLoad(false);

            Batt_cvTimerComboBox.SelectedItem = string.Format("{0:00}:{1:00}", (config.cvMaxDuration / 3600), (config.cvMaxDuration % 3600) / 60);

            Batt_finishTimerComboBox.SelectedItem = string.Format("{0:00}:{1:00}", (config.FIduration / 3600), (config.FIduration % 3600) / 60);

            Batt_equalizeTimerComboBox.SelectedItem = string.Format("{0:00}:{1:00}", (config.EQduration / 3600), (config.EQduration % 3600) / 60);

            Batt_DesulfationTimerComboBox.SelectedItem = string.Format("{0:00}:{1:00}", (config.desulfation / 3600), (config.desulfation % 3600) / 60);

            Batt_finishDVVoltageComboBox.SelectedItem = config.FIdv.ToString();

            Batt_finishDTVoltageComboBox.SelectedItem = config.FIdt.ToString();
        }

        void BattViewCvCurrentStepLoad(bool keepIndex)
        {
            if (Batt_ccCurrentRateComboBox.SelectedIndex == -1)
                return;

            var config = BattViewQuantum.Instance.GetBATTView().Config;

            int selectedIndex = 0;

            if (keepIndex)
                selectedIndex = Batt_cvCurrentStepComboBox.SelectedIndex;

            float ccRate = GetValueFromRates(Batt_ccCurrentRateComboBox.Text);
            ccRate /= 100;

            Batt_cvCurrentStepComboBox.Items = GetBattViewCvCurrentStepList(ccRate);

            if (keepIndex)
            {
                Batt_cvCurrentStepComboBox.SelectedIndex = selectedIndex;

                return;
            }

            if (config.CVcurrentStep == 0)
                Batt_cvCurrentStepComboBox.SelectedItem = AppResources.default_title;
            else
                Batt_cvCurrentStepComboBox.SelectedItem = FormatRate(config.CVcurrentStep / 2.0f, ccRate);
        }

        internal override string BattViewFormatRate(float percent, float select)
        {
            return percent.ToString() + "% - " + Math.Round(0.01f * percent * BattViewQuantum.Instance.GetBATTView().Config.ahrcapacity * select).ToString() + "A";
        }

        #endregion

        #region Load MCB Values

        internal override void LoadMcbValues()
        {
            Task.Run(LoadMcbValuesTask);
        }

        async Task LoadMcbValuesTask()
        {
            var currentMcb = MCBQuantum.Instance.GetMCB();
            var config = currentMcb.Config;

            bool isLithiumAnd2_5OrAbove = McbHelper.IsLithiumAnd2_5OrAbove(currentMcb);

            await LoadLists(isLithiumAnd2_5OrAbove);

            if (isLithiumAnd2_5OrAbove)
            {
                Batt_trCurrentRateComboBox.Title = AppResources.tr_current_rate_lithium;
                Batt_trickleVoltageTextBox.Title = AppResources.tr_target_voltage_lithium;
            }
            else
            {
                Batt_trCurrentRateComboBox.Title = AppResources.tr_current_rate;
                Batt_trickleVoltageTextBox.Title = AppResources.tr_target_voltage;
            }

            MCB_NewEastPennProfile.IsSwitchEnabled = config.useNewEastPennProfile;

            Batt_equalizeVoltageTextBox.SubTitle = Batt_equalizeVoltageTextBox.Text = config.EQvoltage;

            if (!isLithiumAnd2_5OrAbove)
            {
                Batt_trickleVoltageTextBox.SubTitle = Batt_trickleVoltageTextBox.Text = config.trickleVoltage;
                Batt_cvVoltageTextBox.SubTitle = Batt_cvVoltageTextBox.Text = config.CVvoltage;
                Batt_finishVoltageTextBox.SubTitle = Batt_finishVoltageTextBox.Text = config.FIvoltage;
            }
            else
            {
                float factor;

                if (config.enableAutoDetectMultiVoltage)
                    factor = 1.5f;
                else
                    factor = config.LiIon_CellVoltage / 200.0f;

                Batt_trickleVoltageTextBox.SubTitle = Batt_trickleVoltageTextBox.Text = Math.Round(float.Parse(config.trickleVoltage) * factor, 2).ToString();
                Batt_cvVoltageTextBox.SubTitle = Batt_cvVoltageTextBox.Text = Math.Round(float.Parse(config.CVvoltage) * factor, 2).ToString();
                Batt_finishVoltageTextBox.SubTitle = Batt_finishVoltageTextBox.Text = Math.Round(float.Parse(config.FIvoltage) * factor, 2).ToString();
            }

            Batt_trCurrentRateComboBox.SelectedItem = FormatRate((config.TRrate / 100.0f), 1);
            Batt_trCurrentRateComboBox.SelectedIndex = Batt_trCurrentRateComboBox.Items.IndexOf(Batt_trCurrentRateComboBox.Items.FirstOrDefault(o => o.ToString() == Batt_trCurrentRateComboBox.SelectedItem));

            Batt_ccCurrentRateComboBox.SelectedItem = FormatRate((config.CCrate / 100.0f), 1);
            Batt_ccCurrentRateComboBox.SelectedIndex = Batt_ccCurrentRateComboBox.Items.IndexOf(Batt_ccCurrentRateComboBox.Items.FirstOrDefault(o => o.ToString() == Batt_ccCurrentRateComboBox.SelectedItem));

            Batt_fiCurrentRateComboBox.SelectedItem = FormatRate((config.FIrate / 100.0f), 1);
            Batt_fiCurrentRateComboBox.SelectedIndex = Batt_fiCurrentRateComboBox.Items.IndexOf(Batt_fiCurrentRateComboBox.Items.FirstOrDefault(o => o.ToString() == Batt_fiCurrentRateComboBox.SelectedItem));

            Batt_eqCurrentRateComboBox.SelectedItem = FormatRate((config.EQrate / 100.0f), 1);
            Batt_eqCurrentRateComboBox.SelectedIndex = Batt_eqCurrentRateComboBox.Items.IndexOf(Batt_eqCurrentRateComboBox.Items.FirstOrDefault(o => o.ToString() == Batt_eqCurrentRateComboBox.SelectedItem));

            Batt_cvFinishCurrentRateComboBox.SelectedItem = FormatRate((config.CVfinishCurrent / 2.0f), 1);
            Batt_cvFinishCurrentRateComboBox.SelectedIndex = Batt_cvFinishCurrentRateComboBox.Items.IndexOf(Batt_cvFinishCurrentRateComboBox.Items.FirstOrDefault(o => o.ToString() == Batt_cvFinishCurrentRateComboBox.SelectedItem));

            McbCvCurrentStepLoad(false);

            Batt_cvTimerComboBox.SelectedItem = config.CVtimer;

            Batt_finishTimerComboBox.SelectedItem = config.finishTimer;

            MCB_ForceFinishDurationDisableRadio.IsSwitchEnabled = config.forceFinishTimeout;

            Batt_equalizeTimerComboBox.SelectedItem = config.EQtimer;

            Batt_DesulfationTimerComboBox.SelectedItem = config.desulfationTimer;

            Batt_finishDVVoltageComboBox.SelectedItem = (config.finishDV).ToString();

            Batt_finishDTVoltageComboBox.SelectedItem = config.finishDT;
        }

        internal override int MultiplyMaxFiIfuseNewEastPennProfile(int maxFI)
        {
            if (!isBattView && MCBQuantum.Instance.GetMCB().Config.useNewEastPennProfile)
                return maxFI * 2;

            return maxFI;
        }

        void McbCvCurrentStepLoad(bool keepIndex)
        {
            if (!MCBQuantum.Instance.MCB_quickSerialNumberCheckStrict())
                return;

            var config = MCBQuantum.Instance.GetMCB().Config;

            if (Batt_ccCurrentRateComboBox.SelectedIndex == -1)
                return;

            int selectedIndex = 0;

            if (keepIndex)
                selectedIndex = Batt_cvCurrentStepComboBox.SelectedIndex;

            float ccRate = GetValueFromRates(Batt_ccCurrentRateComboBox.SelectedItem);
            ccRate /= 100;

            if (keepIndex)
            {
                Batt_cvCurrentStepComboBox.SelectedIndex = selectedIndex;

                return;
            }

            if (config.CVcurrentStep == 0)
                Batt_cvCurrentStepComboBox.SelectedItem = AppResources.default_title;
            else
                Batt_cvCurrentStepComboBox.SelectedItem = FormatRate(config.CVcurrentStep / 2.0f, ccRate);
        }

        internal override float GetBattViewCcRate()
        {
            float ccRate = BattViewQuantum.Instance.GetBATTView().Config.CCrate;

            return GetValueFromRates(FormatRate((ccRate / 100.0f), 1));
        }

        internal override float GetMcbCcRate()
        {
            float ccRate = MCBQuantum.Instance.GetMCB().Config.CCrate;

            return GetValueFromRates(FormatRate((ccRate / 100.0f), 1));
        }

        internal override string McbFormatRate(float percent, float select)
        {
            var currentMcb = MCBQuantum.Instance.GetMCB();
            var config = currentMcb.Config;

            ushort ahr = 0;

            if (config.enableAutoDetectMultiVoltage && currentMcb.FirmwareRevision > 2.03f)
            {
                string rx = percent.ToString() + "% - ";

                rx += Math.Round(0.01f * percent * config.batteryCapacity24 * select).ToString("N0") + "A";

                rx += ", ";
                rx += Math.Round(0.01f * percent * config.batteryCapacity36 * select).ToString("N0") + "A";

                if (byte.Parse(config.PMvoltage) >= 48)
                {
                    rx += ", ";
                    rx += Math.Round(0.01f * percent * config.batteryCapacity48 * select).ToString("N0") + "A";
                }

                if (byte.Parse(config.PMvoltage) >= 80)
                {
                    rx += ", ";
                    rx += Math.Round(0.01f * percent * config.batteryCapacity80 * select).ToString("N0") + "A";
                }

                return rx;
            }

            ahr = UInt16.Parse(config.batteryCapacity);

            return percent.ToString() + "% - " + Math.Round(0.01f * percent * ahr * select).ToString("N0") + "A";
        }

        #endregion

        #region Load Defaults

        public override void LoadDefaults()
        {
            try
            {
                TryLoadDefaults();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());

                ACUserDialogs.HideProgress();
            }
        }

        void TryLoadDefaults()
        {
            Task.Run(async () => await LoadDefaultsTask());
        }

        async Task LoadDefaultsTask()
        {
            ACUserDialogs.ShowProgress();

            if (isBattView)
                await LoadBattViewDefaults();
            else
                await LoadMcbDefaults();

            await ChangeEditMode(false);

            ACUserDialogs.HideProgress();
        }

        async Task LoadBattViewDefaults()
        {
            var device = BattViewQuantum.Instance.GetBATTView();

            device.SaveDefaultChargeProfile();

            bool result = await SaveBattViewDefaults(device);

            if (result)
                LoadBattViewValues();
        }

        async Task<bool> SaveBattViewDefaults(BattViewObject device)
        {
            try
            {
                return await TrySaveBattViewDefaults(device);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());

                return false;
            }
        }

        async Task<bool> TrySaveBattViewDefaults(BattViewObject device)
        {
            return await device.SaveConfig();
        }

        async Task LoadMcbDefaults()
        {
            var device = MCBQuantum.Instance.GetMCB();

            device.SaveDefaultChargeProfile();

            bool result = await SaveMcbDefaults(device);

            if (result)
                LoadMcbValues();
        }

        async Task<bool> SaveMcbDefaults(MCBobject device)
        {
            try
            {
                return await TrySaveMcbDefaults(device);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());

                return false;
            }
        }

        async Task<bool> TrySaveMcbDefaults(MCBobject device)
        {
            return await device.SaveConfig();
        }

        #endregion

        internal override int BattViewMobileAccessApply()
        {
            return 0;
        }

        internal override int RegularBattViewAccessApply()
        {
            return GetSharedBattViewAccessApply().GetVisibleCount();
        }

        internal override int McbAccessApply()
        {
            var currentMcb = MCBQuantum.Instance.GetMCB();

            bool isLithiumAnd2_5OrAbove = McbHelper.IsLithiumAnd2_5OrAbove(currentMcb);

            accessControlUtility = GetSharedMcbAccessApply(isLithiumAnd2_5OrAbove);

            accessControlUtility
                .DoApplyAccessControl
                (AccessLevelConsts.write, MCB_NewEastPennProfile, ItemSource);

            return accessControlUtility.GetVisibleCount();
        }

        internal override VerifyControl VerfiyBattViewMobileSettings()
        {
            return new VerifyControl();
        }

        internal override VerifyControl VerfiyRegularBattViewSettings()
        {
            return VerfiyCrossSharedSettings(false);
        }

        internal override VerifyControl VerfiyMcbSettings()
        {
            var currentMcb = MCBQuantum.Instance.GetMCB();

            bool isLithiumAnd2_5OrAbove = McbHelper.IsLithiumAnd2_5OrAbove(currentMcb);

            return VerfiyCrossSharedSettings(isLithiumAnd2_5OrAbove);
        }

        internal override void SaveBattViewMobileToConfigObject(BattViewObject device)
        {
        }

        internal override void SaveBattViewRegularToConfigObject(BattViewObject device)
        {
            BattViewQuantum.Instance.SaveBATTViewData();

            SaveSharedBattViewToConfigObject(device);
        }

        internal override void SaveMcbToConfigObject(MCBobject device)
        {
            var currentMcb = MCBQuantum.Instance.GetMCB();

            bool isLithiumAnd2_5OrAbove = McbHelper.IsLithiumAnd2_5OrAbove(currentMcb);

            SaveSharedMcbToConfigObject(device, isLithiumAnd2_5OrAbove);

            device.Config.useNewEastPennProfile = MCB_NewEastPennProfile.IsSwitchEnabled;
        }
    }
}
