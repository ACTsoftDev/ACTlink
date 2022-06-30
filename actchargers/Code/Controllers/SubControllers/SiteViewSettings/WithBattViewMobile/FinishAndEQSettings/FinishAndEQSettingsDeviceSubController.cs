using System;
using System.Threading.Tasks;

namespace actchargers
{
    public class FinishAndEQSettingsDeviceSubController : FinishAndEQSettingsBaseSubController
    {
        public FinishAndEQSettingsDeviceSubController
        (bool isBattView, bool isBattViewMobile) : base(isBattView, isBattViewMobile, false)
        {
        }

        #region Load BattView Values

        internal override void LoadBattViewMobileValues()
        {
            LoadCrossBattViewalues();
        }

        internal override void LoadRegularBattViewValues()
        {
            LoadCrossBattViewalues();
        }

        void LoadCrossBattViewalues()
        {
            Task.Run((Action)LoadCrossBattViewaluesTask);
        }

        void LoadCrossBattViewaluesTask()
        {
            BattViewObject activeBattView = BattViewQuantum.Instance.GetBATTView();
            var config = activeBattView.Config;

            bool alwaysFinish = false;
            uint diff;

            diff = config.FIstartWindow < config.FIcloseWindow ? config.FIcloseWindow - config.FIstartWindow : 86400 + config.FIcloseWindow - config.FIstartWindow;

            alwaysFinish |= (config.FIstartWindow == 0 && diff >= 24 * 60 * 60 - 1 && (config.FIdaysMask & 0x7f) == 0x7f);

            if (config.chargerType == 1)//Conventional 
            {
                FinishSettings.IsEditEnabled = true;

                if (alwaysFinish)
                {
                    FinishSettings.SelectedIndex = 0;
                    FinishSettings.SelectedItem = FinishSettings.Items[FinishSettings.SelectedIndex].ToString();
                }
                else
                {
                    FinishSettings.SelectedIndex = 1;
                    FinishSettings.SelectedItem = FinishSettings.Items[FinishSettings.SelectedIndex].ToString();
                }
            }
            else
            {
                FinishSettings.IsEditEnabled = false;

                FinishSettings.SelectedIndex = 1;
                FinishSettings.SelectedItem = FinishSettings.Items[FinishSettings.SelectedIndex].ToString();
            }

            //Load FI
            FinishStartTime.SubTitle = FinishStartTime.Text = string.Format("{0:00}:{1:00}", (config.FIstartWindow / 3600), (config.FIstartWindow % 3600) / 60);

            if (diff == 86400 - 1)
                diff = 86400;

            FinishDuration.SelectedItem = string.Format("{0:00}:{1:00}", (diff / 3600), (diff % 3600) / 60);
            FinishDuration.SelectedIndex = FinishDuration.Items.FindIndex(o => ((string)o).Equals(FinishDuration.SelectedItem));

            FinishDuration.IsEditEnabled = true;


            (FinishDays.Items[6] as DayViewItem).OriginalValue = (FinishDays.Items[6] as DayViewItem).IsSelected = (config.FIdaysMask & 0x01) != 0;
            (FinishDays.Items[0] as DayViewItem).OriginalValue = (FinishDays.Items[0] as DayViewItem).IsSelected = (config.FIdaysMask & 0x02) != 0;
            (FinishDays.Items[1] as DayViewItem).OriginalValue = (FinishDays.Items[1] as DayViewItem).IsSelected = (config.FIdaysMask & 0x04) != 0;
            (FinishDays.Items[2] as DayViewItem).OriginalValue = (FinishDays.Items[2] as DayViewItem).IsSelected = (config.FIdaysMask & 0x08) != 0;
            (FinishDays.Items[3] as DayViewItem).OriginalValue = (FinishDays.Items[3] as DayViewItem).IsSelected = (config.FIdaysMask & 0x10) != 0;
            (FinishDays.Items[4] as DayViewItem).OriginalValue = (FinishDays.Items[4] as DayViewItem).IsSelected = (config.FIdaysMask & 0x20) != 0;
            (FinishDays.Items[5] as DayViewItem).OriginalValue = (FinishDays.Items[5] as DayViewItem).IsSelected = (config.FIdaysMask & 0x40) != 0;

            FinishInfo.IsEditEnabled = false;

            //Load EQ
            diff = config.EQstartWindow < config.EQcloseWindow ? config.EQcloseWindow - config.EQstartWindow : 86400 + config.EQcloseWindow - config.EQstartWindow;

            if (diff == 86400 - 1)
                diff = 86400;

            AlwaysStartTime.SubTitle = AlwaysStartTime.Text = string.Format("{0:00}:{1:00}", (config.EQstartWindow / 3600), (config.EQstartWindow % 3600) / 60);

            AlwaysDuration.SelectedItem = string.Format("{0:00}:{1:00}", (diff / 3600), (diff % 3600) / 60);
            AlwaysDuration.SelectedIndex = AlwaysDuration.Items.FindIndex(o => ((string)o).Equals(AlwaysDuration.SelectedItem));

            (AlwaysDays.Items[6] as DayViewItem).OriginalValue = (AlwaysDays.Items[6] as DayViewItem).IsSelected = (config.EQdaysMask & 0x01) != 0;
            (AlwaysDays.Items[0] as DayViewItem).OriginalValue = (AlwaysDays.Items[0] as DayViewItem).IsSelected = (config.EQdaysMask & 0x02) != 0;
            (AlwaysDays.Items[1] as DayViewItem).OriginalValue = (AlwaysDays.Items[1] as DayViewItem).IsSelected = (config.EQdaysMask & 0x04) != 0;
            (AlwaysDays.Items[2] as DayViewItem).OriginalValue = (AlwaysDays.Items[2] as DayViewItem).IsSelected = (config.EQdaysMask & 0x08) != 0;
            (AlwaysDays.Items[3] as DayViewItem).OriginalValue = (AlwaysDays.Items[3] as DayViewItem).IsSelected = (config.EQdaysMask & 0x10) != 0;
            (AlwaysDays.Items[4] as DayViewItem).OriginalValue = (AlwaysDays.Items[4] as DayViewItem).IsSelected = (config.EQdaysMask & 0x20) != 0;
            (AlwaysDays.Items[5] as DayViewItem).OriginalValue = (AlwaysDays.Items[5] as DayViewItem).IsSelected = (config.EQdaysMask & 0x40) != 0;

            if (activeBattView.FirmwareRevision > 2.09f)
            {
                AlwaysEqFrequencyDays.IsVisible = true;
                FinishEqFrequencyDays.IsVisible = true;

                if (config.blockedEQDays % 24 == 0)
                    AlwaysEqFrequencyDays.SelectedItem = (config.blockedEQDays / 24).ToString();
                else if (config.blockedEQDays % 24 > 9)
                    AlwaysEqFrequencyDays.SelectedItem = (config.blockedEQDays / 24).ToString() + ":" + (config.blockedEQDays % 24).ToString();
                else
                    AlwaysEqFrequencyDays.SelectedItem = (config.blockedEQDays / 24).ToString() + ":0" + (config.blockedEQDays % 24).ToString();

                AlwaysEqFrequencyDays.SelectedIndex = AlwaysEqFrequencyDays.Items.FindIndex(o => ((string)o).Equals(AlwaysEqFrequencyDays.SelectedItem));

                if (config.blockedFIDays % 24 == 0)
                    FinishEqFrequencyDays.SelectedItem = (config.blockedFIDays / 24).ToString();
                else if (config.blockedFIDays % 24 > 9)
                    FinishEqFrequencyDays.SelectedItem = (config.blockedFIDays / 24).ToString() + ":" + (config.blockedEQDays % 24).ToString();
                else
                    FinishEqFrequencyDays.SelectedItem = (config.blockedFIDays / 24).ToString() + ":0" + (config.blockedEQDays % 24).ToString();

                FinishEqFrequencyDays.SelectedIndex = FinishEqFrequencyDays.Items.FindIndex(o => ((string)o).Equals(FinishEqFrequencyDays.SelectedItem));
            }
            else
            {
                AlwaysEqFrequencyDays.IsVisible = false;
                FinishEqFrequencyDays.IsVisible = false;
            }

            SetInfoText(false);

            EnableFinishItemsBasedOnSelectedSettings();
        }

        #endregion

        #region Load MCB Values

        internal override void LoadMcbValues()
        {
            Task.Run((Action)LoadMcbValuesTask);
        }

        void LoadMcbValuesTask()
        {
            var config = MCBQuantum.Instance.GetMCB().Config;

            if (config.chargerType == 1)//Conventional 
            {
                FinishSettings.IsEditEnabled = true;

                if (!config.FIschedulingMode)
                {
                    FinishSettings.SelectedIndex = 0;
                    FinishSettings.SelectedItem = FinishSettings.Items[FinishSettings.SelectedIndex].ToString();
                }
                else
                {
                    FinishSettings.SelectedIndex = 1;
                    FinishSettings.SelectedItem = FinishSettings.Items[FinishSettings.SelectedIndex].ToString();
                }
            }
            else
            {
                FinishSettings.IsEditEnabled = false;

                FinishSettings.SelectedIndex = 1;
                FinishSettings.SelectedItem = FinishSettings.Items[FinishSettings.SelectedIndex].ToString();
            }

            //Loaf FI
            FinishStartTime.SubTitle = FinishStartTime.Text = config.FIstartWindow;
            FinishStartTime.IsEditEnabled = true;

            FinishDuration.SelectedItem = config.finishWindow;
            FinishDuration.SelectedIndex = FinishDuration.Items.FindIndex(o => ((string)o).Equals(FinishDuration.SelectedItem));

            FinishDuration.IsEditEnabled = true;

            (FinishDays.Items[0] as DayViewItem).IsSelected = (FinishDays.Items[0] as DayViewItem).OriginalValue = config.finishDaysMask.Monday;
            (FinishDays.Items[1] as DayViewItem).IsSelected = (FinishDays.Items[1] as DayViewItem).OriginalValue = config.finishDaysMask.Tuesday;
            (FinishDays.Items[2] as DayViewItem).IsSelected = (FinishDays.Items[2] as DayViewItem).OriginalValue = config.finishDaysMask.Wednesday;
            (FinishDays.Items[3] as DayViewItem).IsSelected = (FinishDays.Items[3] as DayViewItem).OriginalValue = config.finishDaysMask.Thursday;
            (FinishDays.Items[4] as DayViewItem).IsSelected = (FinishDays.Items[4] as DayViewItem).OriginalValue = config.finishDaysMask.Friday;
            (FinishDays.Items[5] as DayViewItem).IsSelected = (FinishDays.Items[5] as DayViewItem).OriginalValue = config.finishDaysMask.Saturday;
            (FinishDays.Items[6] as DayViewItem).IsSelected = (FinishDays.Items[6] as DayViewItem).OriginalValue = config.finishDaysMask.Sunday;

            foreach (DayViewItem dayItems in FinishDays.Items)
                dayItems.IsEditable = FinishDays.IsEditEnabled;

            FinishInfo.IsEditEnabled = false;

            //Load EQ
            AlwaysStartTime.SubTitle = AlwaysStartTime.Text = config.EQstartWindow;
            AlwaysStartTime.IsEditEnabled = true;

            AlwaysDuration.SelectedItem = config.EQwindow;
            AlwaysDuration.SelectedIndex = AlwaysDuration.Items.FindIndex(o => ((string)o).Equals(AlwaysDuration.SelectedItem));
            AlwaysDuration.IsEditEnabled = true;

            (AlwaysDays.Items[0] as DayViewItem).IsSelected = (AlwaysDays.Items[0] as DayViewItem).OriginalValue = config.EQdaysMask.Monday;
            (AlwaysDays.Items[1] as DayViewItem).IsSelected = (AlwaysDays.Items[1] as DayViewItem).OriginalValue = config.EQdaysMask.Tuesday;
            (AlwaysDays.Items[2] as DayViewItem).IsSelected = (AlwaysDays.Items[2] as DayViewItem).OriginalValue = config.EQdaysMask.Wednesday;
            (AlwaysDays.Items[3] as DayViewItem).IsSelected = (AlwaysDays.Items[3] as DayViewItem).OriginalValue = config.EQdaysMask.Thursday;
            (AlwaysDays.Items[4] as DayViewItem).IsSelected = (AlwaysDays.Items[4] as DayViewItem).OriginalValue = config.EQdaysMask.Friday;
            (AlwaysDays.Items[5] as DayViewItem).IsSelected = (AlwaysDays.Items[5] as DayViewItem).OriginalValue = config.EQdaysMask.Saturday;
            (AlwaysDays.Items[6] as DayViewItem).IsSelected = (AlwaysDays.Items[6] as DayViewItem).OriginalValue = config.EQdaysMask.Sunday;

            foreach (DayViewItem dayItems in AlwaysDays.Items)
                dayItems.IsEditable = AlwaysDays.IsEditEnabled;

            AlwaysInfo.IsEditEnabled = false;

            SetInfoText(false);

            EnableFinishItemsBasedOnSelectedSettings();
        }

        #endregion

        internal override int McbAccessApply()
        {
            var currentMcb = MCBQuantum.Instance.GetMCB();

            bool isLithiumAnd2_5OrAbove = McbHelper.IsLithiumAnd2_5OrAbove(currentMcb);

            return GetSharedMcbAccessApply(isLithiumAnd2_5OrAbove).GetVisibleCount();
        }
    }
}
