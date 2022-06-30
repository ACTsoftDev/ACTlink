using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using MvvmCross.Core.ViewModels;
using static actchargers.ACUtility;

namespace actchargers
{
    public abstract class FinishAndEQSettingsBaseSubController : WithBattViewMobileBaseSubController
    {
        const string START_TIME_REGIX = @"^(\d{1,2}){1}:(\d{1,2}){1}$";

        const int ALWAYS_PARENT_INDEX = 1;
        const int FINISH_PARENT_INDEX = 2;

        internal ListViewItem FinishSettings;

        #region Always Items

        internal ListViewItem AlwaysHeader;
        internal ListViewItem AlwaysStartTime;
        internal ListViewItem AlwaysDuration;
        internal ListViewItem AlwaysEqFrequencyDays;
        internal ListViewItem AlwaysDays;
        internal ListViewItem AlwaysInfo;

        #endregion

        #region Finish Items

        internal ListViewItem FinishHeader;
        internal ListViewItem FinishStartTime;
        internal ListViewItem FinishDuration;
        internal ListViewItem FinishEqFrequencyDays;
        internal ListViewItem FinishDays;
        internal ListViewItem FinishInfo;

        #endregion

        protected FinishAndEQSettingsBaseSubController
        (bool isBattView, bool isBattViewMobile, bool isSiteView)
            : base(isBattView, isBattViewMobile, isSiteView)
        {
        }

        #region Init Items

        internal override void InitSharedBattViewMobileItems()
        {
            InitCrossSharedBattViewItems();
        }

        internal override void InitSharedRegularBattViewItems()
        {
            InitCrossSharedBattViewItems();
        }

        void InitCrossSharedBattViewItems()
        {
            InitCrossSharedItems();

            AlwaysEqFrequencyDays = InitEqFrequencyDays(ALWAYS_PARENT_INDEX, 4);
            FinishEqFrequencyDays = InitEqFrequencyDays(FINISH_PARENT_INDEX, 10);
        }

        internal override void InitSharedMcbItems()
        {
            InitCrossSharedItems();
        }

        void InitCrossSharedItems()
        {
            FinishSettings = new ListViewItem
            {
                Index = 0,
                Title = AppResources.finish_cycle_settings,
                DefaultCellType = CellTypes.LabelLabel,
                EditableCellType = CellTypes.ListSelector,
                ListSelectorType = ListSelectorType.FinishCycleSettingsType,
                ListSelectionCommand = ListSelectorCommand,
                IsEditable = EditingMode,
                IsVisible = true,
                ParentIndex = 0
            };
            FinishSettings.Items = new List<object>()
            {
                AppResources.always,
                AppResources.custom
            };

            AlwaysHeader = InitHeader(ALWAYS_PARENT_INDEX, 1, AppResources.equalize_cycle);
            AlwaysStartTime = InitStartTime(ALWAYS_PARENT_INDEX, 2);
            AlwaysDuration = InitDuration(ALWAYS_PARENT_INDEX, 3);
            AlwaysDays = InitDays(ALWAYS_PARENT_INDEX, 5);
            AlwaysInfo = InitInfo(ALWAYS_PARENT_INDEX, 6);

            FinishHeader = InitHeader(FINISH_PARENT_INDEX, 7, AppResources.finish_cycle);
            FinishStartTime = InitStartTime(FINISH_PARENT_INDEX, 8);
            FinishDuration = InitDuration(FINISH_PARENT_INDEX, 9);
            FinishDays = InitDays(FINISH_PARENT_INDEX, 11);
            FinishInfo = InitInfo(FINISH_PARENT_INDEX, 12);
        }

        ListViewItem InitHeader(int parentIndex, int index, string title)
        {
            return new ListViewItem()
            {
                Index = index,
                ParentIndex = parentIndex,
                Title = title,
                DefaultCellType = CellTypes.SectionHeader,
                EditableCellType = CellTypes.SectionHeader,
                IsVisible = true
            };
        }

        ListViewItem InitStartTime(int parentIndex, int index)
        {
            return new ListViewItem
            {
                Index = index,
                ParentIndex = parentIndex,
                Title = AppResources.starts,
                DefaultCellType = CellTypes.LabelLabel,
                EditableCellType = CellTypes.TimePicker,
                IsEditable = EditingMode,
                TextValueChanged = TextValueChanged,
                IsVisible = true,
            };
        }

        ListViewItem InitDuration(int parentIndex, int index)
        {
            return new ListViewItem
            {
                Index = index,
                ParentIndex = parentIndex,
                Title = AppResources.duration,
                DefaultCellType = CellTypes.LabelLabel,
                EditableCellType = CellTypes.ListSelector,
                ListSelectorType = ListSelectorType.DurationIntervalTypes,
                Items = Instance.DurationIntervalObjectsTypes,
                ListSelectionCommand = ListSelectorCommand,
                IsEditable = EditingMode,
                TextValueChanged = TextValueChanged,
                IsVisible = true
            };
        }

        ListViewItem InitEqFrequencyDays(int parentIndex, int index)
        {
            ListViewItem listViewItem = new ListViewItem
            {
                Index = index,
                ParentIndex = parentIndex,
                Title = AppResources.eq_frequency_days,
                DefaultCellType = CellTypes.LabelLabel,
                EditableCellType = CellTypes.ListSelector,
                ListSelectionCommand = ListSelectorCommand,
                IsVisible = true
            };

            listViewItem.Items = new List<object>();
            listViewItem.Items.AddRange(
                new object[]
            {
                "0",
                "5",
                "6",
                "7",
                "8",
                "9"
            });

            return listViewItem;
        }

        ListViewItem InitDays(int parentIndex, int index)
        {
            ListViewItem listViewItem = new ListViewItem
            {
                Index = index,
                ParentIndex = parentIndex,
                Title = AppResources.days,
                SubTitle = "",
                DefaultCellType = CellTypes.Days,
                EditableCellType = CellTypes.Days,
                TextValueChanged = TextValueChanged,
                IsVisible = true
            };

            AddDays(listViewItem);

            return listViewItem;
        }

        void AddDays(ListViewItem listViewItem)
        {
            listViewItem.Items = new List<object>
            {
                new DayViewItem { Title = AppResources.monday, id = 0, DayButtonClicked = DaysButtonClickCommand },
                new DayViewItem { Title = AppResources.tuesday, id = 1, DayButtonClicked = DaysButtonClickCommand },
                new DayViewItem { Title = AppResources.wednesday, id = 2, DayButtonClicked = DaysButtonClickCommand },
                new DayViewItem { Title = AppResources.thursday, id = 3, DayButtonClicked = DaysButtonClickCommand },
                new DayViewItem { Title = AppResources.friday, id = 4, DayButtonClicked = DaysButtonClickCommand },
                new DayViewItem { Title = AppResources.saturday, id = 5, DayButtonClicked = DaysButtonClickCommand },
                new DayViewItem { Title = AppResources.sunday, id = 6, DayButtonClicked = DaysButtonClickCommand }
            };
        }

        ListViewItem InitInfo(int parentIndex, int index)
        {
            return new ListViewItem
            {
                Index = index,
                ParentIndex = parentIndex,
                DefaultCellType = CellTypes.LabelText,
                EditableCellType = CellTypes.LabelText,
                IsEditable = false,
                IsVisible = true
            };
        }

        #endregion

        #region List Item Selected

        internal override void OnListItemSelected(ListSelectorMessage selectedListItem)
        {
            base.OnListItemSelected(selectedListItem);

            EnableFinishItemsBasedOnSelectedSettings();
        }

        internal void EnableFinishItemsBasedOnSelectedSettings()
        {
            bool isOn = (FinishSettings.Text == AppResources.custom);

            ChangeAbilityForAllFinishItems(isOn);
        }

        void ChangeAbilityForAllFinishItems(bool isOn)
        {
            var list = ItemSource.Where((arg) => arg.ParentIndex == FINISH_PARENT_INDEX);

            foreach (var item in list)
                ChangeAbilityForOneItem(item, isOn);
        }

        void ChangeAbilityForOneItem(ListViewItem item, bool isOn)
        {
            item.ChangeEditMode(isOn);

            item.IsVisible = isOn;
        }

        #endregion

        #region Text Value Changed

        IMvxCommand TextValueChanged
        {
            get
            {
                return new MvxCommand(ExecuteTextValueChanged);
            }
        }

        void ExecuteTextValueChanged()
        {
            SetInfoText(true);
        }

        internal void SetInfoText(bool isReloading)
        {
            SetInfoTextForItems
            (isReloading, AlwaysStartTime, AlwaysDuration, AlwaysDays, AlwaysInfo);

            SetInfoTextForItems
            (isReloading, FinishStartTime, FinishDuration, FinishDays, FinishInfo);
        }

        void SetInfoTextForItems
         (bool isReloading, ListViewItem alwaysStartTime, ListViewItem alwaysDuration,
          ListViewItem alwaysEqFrequencyDays, ListViewItem alwaysInfo)
        {
            var infoString = "Equalize cycle starts at " + alwaysStartTime.Text + " and ends in " + alwaysDuration.Text + " on every";

            infoString += GetDaysText(alwaysEqFrequencyDays);

            alwaysInfo.Text = infoString;

            if (!isReloading)
                alwaysInfo.SubTitle = alwaysInfo.Text;
        }

        string GetDaysText(ListViewItem alwaysEqFrequencyDays)
        {
            string text = "";

            if (alwaysEqFrequencyDays == null || alwaysEqFrequencyDays.Items == null)
                return text;

            bool isOneDayAdded = false;
            foreach (DayViewItem item in alwaysEqFrequencyDays.Items)
            {
                if (item.IsSelected)
                {
                    text = text + (isOneDayAdded ? ", " : " ") + item.Title;
                    isOneDayAdded = true;
                }
            }

            return text;
        }

        #endregion

        #region Days

        IMvxCommand DaysButtonClickCommand
        {
            get { return new MvxCommand<DayViewItem>(OnDaysButton); }
        }

        void OnDaysButton(DayViewItem item)
        {
            if (EditingMode && item.IsEditable)
            {
                item.IsSelected = !item.IsSelected;

                SetInfoText(true);
            }
        }

        #endregion

        internal override void InitExclusiveBattViewMobileItems()
        {
        }

        internal override void InitExclusiveRegularBattViewItems()
        {
        }

        internal override void InitExclusiveMcbItems()
        {
        }

        internal override void LoadExclusiveValues()
        {
        }

        public override void LoadDefaults()
        {
        }

        #region Add BattView

        internal override int BattViewMobileAccessApply()
        {
            return CrossSharedBattViewAccessApply();
        }

        internal override int RegularBattViewAccessApply()
        {
            return CrossSharedBattViewAccessApply();
        }

        int CrossSharedBattViewAccessApply()
        {
            accessControlUtility = new UIAccessControlUtility();

            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_FI_sched_Settings, FinishSettings, ItemSource);

            //equalize cycle
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_FI_EQ_sched_CustomSettings, AlwaysHeader, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_FI_EQ_sched_CustomSettings, AlwaysStartTime, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_FI_EQ_sched_CustomSettings, AlwaysDuration, ItemSource);

            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_FI_EQ_sched_CustomSettings, AlwaysDays, ItemSource);

            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_FI_EQ_sched_CustomSettings, AlwaysEqFrequencyDays, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_FI_EQ_sched_CustomSettings, AlwaysInfo, ItemSource);

            //finish cycle
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_FI_sched_CustomSettings, FinishHeader, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_FI_sched_CustomSettings, FinishStartTime, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_FI_sched_CustomSettings, FinishDuration, ItemSource);

            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_FI_sched_CustomSettings, FinishEqFrequencyDays, ItemSource);

            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_FI_sched_CustomSettings, FinishDays, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_FI_sched_CustomSettings, FinishInfo, ItemSource);

            FinishSettings.IsEditEnabled = isSiteView || BattViewQuantum.Instance.GetBATTView().Config.chargerType == 1;

            ShowEdit &= accessControlUtility.GetSavedCount() != 0;

            return accessControlUtility.GetVisibleCount();
        }

        #endregion

        #region Add MCB

        internal UIAccessControlUtility GetSharedMcbAccessApply(bool isLithiumAnd2_5OrAbove)
        {
            accessControlUtility = new UIAccessControlUtility();

            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_FI_sched_Settings, FinishSettings, ItemSource);

            //equalize cycle
            if (!isLithiumAnd2_5OrAbove)
            {
                accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_FI_EQ_sched_CustomSettings, AlwaysHeader, ItemSource);
                accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_FI_EQ_sched_CustomSettings, AlwaysStartTime, ItemSource);
                accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_FI_EQ_sched_CustomSettings, AlwaysDuration, ItemSource);
                accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_FI_EQ_sched_CustomSettings, AlwaysDays, ItemSource);
                accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_FI_EQ_sched_CustomSettings, AlwaysInfo, ItemSource);

                foreach (DayViewItem dayItems in AlwaysDays.Items)
                    dayItems.IsEditable = AlwaysDays.IsEditEnabled;
            }

            //finish cycle
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_FI_sched_CustomSettings, FinishHeader, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_FI_sched_CustomSettings, FinishStartTime, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_FI_sched_CustomSettings, FinishDuration, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_FI_sched_CustomSettings, FinishDays, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_FI_sched_CustomSettings, FinishInfo, ItemSource);

            foreach (DayViewItem dayItems in FinishDays.Items)
                dayItems.IsEditable = FinishDays.IsEditEnabled;
            ShowEdit &= accessControlUtility.GetSavedCount() != 0;

            FinishSettings.IsEditEnabled = isSiteView || MCBQuantum.Instance.GetMCB().Config.chargerType == 1;

            return accessControlUtility;
        }

        #endregion

        internal override void AddExclusiveItems()
        {
        }

        #region Verfiy Settings

        internal override VerifyControl VerfiyBattViewMobileSettings()
        {
            return VerfiyCrossSharedSettings();
        }

        internal override VerifyControl VerfiyRegularBattViewSettings()
        {
            return VerfiyCrossSharedSettings();
        }

        internal override VerifyControl VerfiyMcbSettings()
        {
            return VerfiyCrossSharedSettings();
        }

        VerifyControl VerfiyCrossSharedSettings()
        {
            var verifyControl = new VerifyControl();

            if (FinishSettings.SelectedIndex != 0)
                VerfiySection(verifyControl, FinishStartTime, FinishDuration);

            VerfiySection(verifyControl, AlwaysStartTime, AlwaysDuration);

            return verifyControl;
        }

        VerifyControl VerfiySection(VerifyControl verifyControl, ListViewItem startTime, ListViewItem duration)
        {
            Match match;

            match = Regex.Match(startTime.Text, START_TIME_REGIX, RegexOptions.IgnoreCase);

            if (!match.Success || byte.Parse(match.Groups[1].Value) >= 24 || byte.Parse(match.Groups[2].Value) >= 60)
                verifyControl.InsertRemoveFault(true, startTime);
            else
            {
                uint tempLong = uint.Parse(match.Groups[1].Value) * 3600 + uint.Parse(match.Groups[2].Value) * 60;

                if (tempLong > 86400)
                    verifyControl.InsertRemoveFault(true, startTime);
                else
                    verifyControl.InsertRemoveFault(false, startTime);
            }

            verifyControl.VerifyComboBox(duration);

            return verifyControl;
        }

        #endregion

        #region Save BattView

        internal override void SaveBattViewMobileToConfigObject(BattViewObject device)
        {
            SaveCrossBattViewToConfigObject(device);
        }

        internal override void SaveBattViewRegularToConfigObject(BattViewObject device)
        {
            SaveCrossBattViewToConfigObject(device);
        }

        void SaveCrossBattViewToConfigObject(BattViewObject device)
        {
            var config = device.Config;

            UInt32 window;
            Match match;

            if (FinishSettings.SelectedIndex == 0)
            {
                config.FIstartWindow = 0;
                config.FIcloseWindow = 86400;
                config.FIdaysMask = 0x7F;
            }
            else
            {
                match = Regex.Match(FinishStartTime.Text, START_TIME_REGIX, RegexOptions.IgnoreCase);
                config.FIstartWindow = UInt32.Parse(match.Groups[1].Value) * 3600 + UInt32.Parse(match.Groups[2].Value) * 60;

                match = Regex.Match(FinishDuration.Text, START_TIME_REGIX, RegexOptions.IgnoreCase);

                window = UInt32.Parse(match.Groups[1].Value) * 3600 + UInt32.Parse(match.Groups[2].Value) * 60;

                if (window == 86400 && config.FIstartWindow != 0)
                    window--;

                config.FIcloseWindow = config.FIstartWindow + window;

                if (config.FIcloseWindow != 86400)
                    config.FIcloseWindow %= 86400;

                config.FIdaysMask = 0;
                config.FIdaysMask |= (byte)((FinishDays.Items[6] as DayViewItem).IsSelected ? 0x01 : 0x00);
                config.FIdaysMask |= (byte)((FinishDays.Items[0] as DayViewItem).IsSelected ? 0x02 : 0x00);
                config.FIdaysMask |= (byte)((FinishDays.Items[1] as DayViewItem).IsSelected ? 0x04 : 0x00);
                config.FIdaysMask |= (byte)((FinishDays.Items[2] as DayViewItem).IsSelected ? 0x08 : 0x00);
                config.FIdaysMask |= (byte)((FinishDays.Items[3] as DayViewItem).IsSelected ? 0x10 : 0x00);
                config.FIdaysMask |= (byte)((FinishDays.Items[4] as DayViewItem).IsSelected ? 0x20 : 0x00);
                config.FIdaysMask |= (byte)((FinishDays.Items[5] as DayViewItem).IsSelected ? 0x40 : 0x00);
            }

            match = Regex.Match(AlwaysStartTime.Text, START_TIME_REGIX, RegexOptions.IgnoreCase);
            config.EQstartWindow = UInt32.Parse(match.Groups[1].Value) * 3600 + UInt32.Parse(match.Groups[2].Value) * 60;

            match = Regex.Match(AlwaysDuration.Text, START_TIME_REGIX, RegexOptions.IgnoreCase);

            window = UInt32.Parse(match.Groups[1].Value) * 3600 + UInt32.Parse(match.Groups[2].Value) * 60;

            if (window == 86400 && config.EQstartWindow != 0)
                window--;

            config.EQcloseWindow = config.EQstartWindow + window;

            if (config.EQcloseWindow != 86400)
                config.EQcloseWindow %= 86400;

            config.EQdaysMask = 0;
            config.EQdaysMask |= (byte)((AlwaysDays.Items[6] as DayViewItem).IsSelected ? 0x01 : 0x00);
            config.EQdaysMask |= (byte)((AlwaysDays.Items[0] as DayViewItem).IsSelected ? 0x02 : 0x00);
            config.EQdaysMask |= (byte)((AlwaysDays.Items[1] as DayViewItem).IsSelected ? 0x04 : 0x00);
            config.EQdaysMask |= (byte)((AlwaysDays.Items[2] as DayViewItem).IsSelected ? 0x08 : 0x00);
            config.EQdaysMask |= (byte)((AlwaysDays.Items[3] as DayViewItem).IsSelected ? 0x10 : 0x00);
            config.EQdaysMask |= (byte)((AlwaysDays.Items[4] as DayViewItem).IsSelected ? 0x20 : 0x00);
            config.EQdaysMask |= (byte)((AlwaysDays.Items[5] as DayViewItem).IsSelected ? 0x40 : 0x00);

            config.blockedFIDays = 0;
            config.blockedEQDays = 0;

            if (device.FirmwareRevision > 2.09f)
            {
                string tempS = FinishEqFrequencyDays.Text;
                string[] tempss = tempS.Split(new char[] { ':' });
                byte tempB = 0;
                if (tempss.Length > 0)
                {
                    if (byte.TryParse(tempss[0], out tempB))
                        config.blockedFIDays = (byte)(tempB * 24);

                    if (tempss.Length > 1 && byte.TryParse(tempss[1], out tempB))
                        config.blockedFIDays += tempB;
                }

                tempS = AlwaysEqFrequencyDays.Text;
                tempss = tempS.Split(new char[] { ':' });
                tempB = 0;

                if (tempss.Length > 0)
                {
                    if (byte.TryParse(tempss[0], out tempB))
                        config.blockedEQDays = (byte)(tempB * 24);

                    if (tempss.Length > 1 && byte.TryParse(tempss[1], out tempB))
                        config.blockedEQDays += tempB;
                }
            }
        }

        #endregion

        #region Save MCB

        internal override void SaveMcbToConfigObject(MCBobject device)
        {
            var config = device.Config;

            config.FIschedulingMode = FinishSettings.SelectedIndex == 1;

            //Loaf FI
            if (config.FIschedulingMode)
            {
                config.FIstartWindow = FinishStartTime.Text;
                config.finishWindow = FinishDuration.Text;

                config.finishDaysMask.Sunday = (FinishDays.Items[6] as DayViewItem).IsSelected;
                config.finishDaysMask.Monday = (FinishDays.Items[0] as DayViewItem).IsSelected;
                config.finishDaysMask.Tuesday = (FinishDays.Items[1] as DayViewItem).IsSelected;
                config.finishDaysMask.Wednesday = (FinishDays.Items[2] as DayViewItem).IsSelected;
                config.finishDaysMask.Thursday = (FinishDays.Items[3] as DayViewItem).IsSelected;
                config.finishDaysMask.Friday = (FinishDays.Items[4] as DayViewItem).IsSelected;
                config.finishDaysMask.Saturday = (FinishDays.Items[5] as DayViewItem).IsSelected;
            }

            config.EQstartWindow = AlwaysStartTime.Text;
            config.EQwindow = AlwaysDuration.Text;

            config.EQdaysMask.Sunday = (AlwaysDays.Items[6] as DayViewItem).IsSelected;
            config.EQdaysMask.Monday = (AlwaysDays.Items[0] as DayViewItem).IsSelected;
            config.EQdaysMask.Tuesday = (AlwaysDays.Items[1] as DayViewItem).IsSelected;
            config.EQdaysMask.Wednesday = (AlwaysDays.Items[2] as DayViewItem).IsSelected;
            config.EQdaysMask.Thursday = (AlwaysDays.Items[3] as DayViewItem).IsSelected;
            config.EQdaysMask.Friday = (AlwaysDays.Items[4] as DayViewItem).IsSelected;
            config.EQdaysMask.Saturday = (AlwaysDays.Items[5] as DayViewItem).IsSelected;
        }

        #endregion
    }
}
