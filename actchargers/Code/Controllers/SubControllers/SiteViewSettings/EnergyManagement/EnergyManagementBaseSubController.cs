using System.Collections.Generic;
using System.Text.RegularExpressions;
using MvvmCross.Core.ViewModels;

namespace actchargers
{
    public abstract class EnergyManagementBaseSubController : SiteViewSettingsBaseSubController
    {
        const string START_TIME_REGIX = @"^(\d{1,2}){1}:(\d{1,2}){1}$";

        const int LOCOUT_PARENT_INDEX = 1;
        const int LIMITING_PARENT_INDEX = 2;

        #region Lockout Items

        internal ListViewItem LockoutHeader;
        internal ListViewItem MCB_Energy_lockoutStartTime;
        internal ListViewItem MCB_Energy_lockoutWindow;
        internal ListViewItem MCB_Energy_lockoutDays;
        internal ListViewItem MCB_Energy_lockoutInfoTextBox;

        #endregion

        #region Power Limiting

        internal ListViewItem LimitingHeader;
        internal ListViewItem MCB_Energy_powerfactor;
        internal ListViewItem MCB_Energy_powerStartTime;
        internal ListViewItem MCB_Energy_powerWindow;
        internal ListViewItem MCB_Energy_powerDays;
        internal ListViewItem MCB_Energy_powerInfoTextBox;

        #endregion

        protected EnergyManagementBaseSubController
        (bool isSiteView)
            : base(false, isSiteView)
        {
        }

        #region Init Items

        internal override void InitSharedBattViewItems()
        {
        }

        internal override void InitSharedMcbItems()
        {
            LockoutHeader = InitHeader(LOCOUT_PARENT_INDEX, 0, AppResources.lock_out);

            MCB_Energy_lockoutStartTime = InitStartTime(LOCOUT_PARENT_INDEX, 1);

            MCB_Energy_lockoutWindow = InitDuration(LOCOUT_PARENT_INDEX, 2, AppResources.lockout_window);

            MCB_Energy_lockoutDays = InitDays(LOCOUT_PARENT_INDEX, 3);

            MCB_Energy_lockoutInfoTextBox = InitInfo(LOCOUT_PARENT_INDEX, 4);

            LimitingHeader = InitHeader(LIMITING_PARENT_INDEX, 5, AppResources.power_limiting);

            MCB_Energy_powerfactor = InitPowerFactor(LIMITING_PARENT_INDEX, 6);

            MCB_Energy_powerStartTime = InitStartTime(LIMITING_PARENT_INDEX, 7);

            MCB_Energy_powerWindow = InitDuration(LIMITING_PARENT_INDEX, 8, AppResources.power_limit_window);

            MCB_Energy_powerDays = InitDays(LIMITING_PARENT_INDEX, 9);

            MCB_Energy_powerInfoTextBox = InitInfo(LIMITING_PARENT_INDEX, 10);
        }

        ListViewItem InitHeader(int parentIndex, int index, string title)
        {
            return new ListViewItem()
            {
                Index = index,
                ParentIndex = parentIndex,
                Title = title,
                DefaultCellType = ACUtility.CellTypes.SectionHeader,
                EditableCellType = ACUtility.CellTypes.SectionHeader,
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
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.TimePicker,
                IsEditable = EditingMode,
                TextValueChanged = TextValueChanged,
                IsVisible = true,
            };
        }

        ListViewItem InitDuration(int parentIndex, int index, string title)
        {
            return new ListViewItem
            {
                Index = index,
                ParentIndex = parentIndex,
                Title = title,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.ListSelector,
                ListSelectorType = ACUtility.ListSelectorType.DurationIntervalTypes,
                Items = ACUtility.Instance.DurationIntervalObjectsTypes,
                ListSelectionCommand = ListSelectorCommand,
                IsEditable = EditingMode,
                TextValueChanged = TextValueChanged,
                IsVisible = true
            };
        }

        ListViewItem InitDays(int parentIndex, int index)
        {
            ListViewItem listViewItem = new ListViewItem
            {
                Index = index,
                ParentIndex = parentIndex,
                Title = AppResources.days,
                SubTitle = "",
                DefaultCellType = ACUtility.CellTypes.Days,
                EditableCellType = ACUtility.CellTypes.Days,
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

        ListViewItem InitPowerFactor(int parentIndex, int index)
        {
            ListViewItem listViewItem = new ListViewItem
            {
                Index = index,
                ParentIndex = parentIndex,
                Title = AppResources.power_limit_factor,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.ListSelector,
                TextValueChanged = TextValueChanged,
                ListSelectionCommand = ListSelectorCommand
            };

            listViewItem.Items = new List<object>();

            for (int i = ControlObject.FormLimits.energyFactorMin; i <= ControlObject.FormLimits.energyFactorMax; i += ControlObject.FormLimits.energyFactorStep)
                listViewItem.Items.Add(string.Format(i.ToString()));

            return listViewItem;
        }

        ListViewItem InitInfo(int parentIndex, int index)
        {
            return new ListViewItem
            {
                Index = index,
                ParentIndex = parentIndex,
                DefaultCellType = ACUtility.CellTypes.LabelText,
                EditableCellType = ACUtility.CellTypes.LabelText,
                IsEditable = false,
                IsVisible = true
            };
        }

        #endregion

        #region List Item Selected

        internal override void OnListItemSelected(ListSelectorMessage selectedListItem)
        {
            base.OnListItemSelected(selectedListItem);

            SetInfoText(true);
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
            string lockoutInfoText = "Lockout cycle starts at " + MCB_Energy_lockoutStartTime.Text + " and ends in " + MCB_Energy_lockoutWindow.Text + " on every";

            lockoutInfoText += GetDaysText(MCB_Energy_lockoutDays);

            MCB_Energy_lockoutInfoTextBox.Text = lockoutInfoText;

            if (!isReloading)
                MCB_Energy_lockoutInfoTextBox.SubTitle = MCB_Energy_lockoutInfoTextBox.Text;


            string limitingInfoText = "Power limit factor is " + MCB_Energy_powerfactor.Text + ". Cycle starts at " + MCB_Energy_powerStartTime.Text + " and ends in " + MCB_Energy_powerWindow.Text + " on every";

            limitingInfoText += GetDaysText(MCB_Energy_powerDays);

            MCB_Energy_powerInfoTextBox.Text = limitingInfoText;

            if (!isReloading)
                MCB_Energy_powerInfoTextBox.SubTitle = MCB_Energy_powerInfoTextBox.Text;
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

        internal override void InitExclusiveBattViewItems()
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

        internal override int BattViewAccessApply()
        {
            return 0;
        }

        #region Add MCB

        internal override int McbAccessApply()
        {
            accessControlUtility = new UIAccessControlUtility();

            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_EnergyManagment, LockoutHeader, ItemSource);

            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_EnergyManagment, MCB_Energy_lockoutStartTime, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_EnergyManagment, MCB_Energy_lockoutWindow, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_EnergyManagment, MCB_Energy_lockoutDays, ItemSource);

            foreach (DayViewItem dayItems in MCB_Energy_lockoutDays.Items)
                dayItems.IsEditable = MCB_Energy_lockoutDays.IsEditEnabled;

            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_EnergyManagment, MCB_Energy_lockoutInfoTextBox, ItemSource);

            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_EnergyManagment, LimitingHeader, ItemSource);

            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_EnergyManagment, MCB_Energy_powerfactor, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_EnergyManagment, MCB_Energy_powerStartTime, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_EnergyManagment, MCB_Energy_powerWindow, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_EnergyManagment, MCB_Energy_powerDays, ItemSource);

            foreach (DayViewItem dayItems in MCB_Energy_powerDays.Items)
                dayItems.IsEditable = MCB_Energy_powerDays.IsEditEnabled;

            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_EnergyManagment, MCB_Energy_powerInfoTextBox, ItemSource);

            ShowEdit &= accessControlUtility.GetSavedCount() != 0;

            return accessControlUtility.GetVisibleCount();
        }

        #endregion

        internal override void AddExclusiveItems()
        {
        }

        internal override VerifyControl VerfiyBattViewSettings()
        {
            return new VerifyControl();
        }

        #region Verfiy MCB Settings

        internal override VerifyControl VerfiyMcbSettings()
        {
            var verifyControl = new VerifyControl();

            Match match;
            match = Regex.Match(MCB_Energy_lockoutStartTime.Text, START_TIME_REGIX, RegexOptions.IgnoreCase);

            if (!match.Success || byte.Parse(match.Groups[1].Value) >= 24 || byte.Parse(match.Groups[2].Value) >= 60)
                verifyControl.InsertRemoveFault(true, MCB_Energy_lockoutStartTime);
            else
            {
                uint tempLong = uint.Parse(match.Groups[1].Value) * 3600 + uint.Parse(match.Groups[2].Value) * 60;

                if (tempLong > 86400)
                    verifyControl.InsertRemoveFault(true, MCB_Energy_lockoutStartTime);
                else
                    verifyControl.InsertRemoveFault(false, MCB_Energy_lockoutStartTime);
            }

            verifyControl.VerifyComboBox(MCB_Energy_lockoutWindow, MCB_Energy_lockoutWindow);

            match = Regex.Match(MCB_Energy_powerStartTime.Text, START_TIME_REGIX, RegexOptions.IgnoreCase);

            if (!match.Success || byte.Parse(match.Groups[1].Value) >= 24 || byte.Parse(match.Groups[2].Value) >= 60)
                verifyControl.InsertRemoveFault(true, MCB_Energy_powerStartTime);
            else
            {
                uint tempLong = uint.Parse(match.Groups[1].Value) * 3600 + uint.Parse(match.Groups[2].Value) * 60;

                if (tempLong > 86400)
                    verifyControl.InsertRemoveFault(true, MCB_Energy_powerStartTime);
                else
                    verifyControl.InsertRemoveFault(false, MCB_Energy_powerStartTime);
            }

            verifyControl.VerifyComboBox(MCB_Energy_powerWindow, MCB_Energy_powerWindow);
            verifyControl.VerifyComboBox(MCB_Energy_powerfactor, MCB_Energy_powerfactor);

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

        internal override void SaveBattViewToConfigObject(BattViewObject device)
        {
        }

        #region Save MCB

        internal override void SaveMcbToConfigObject(MCBobject device)
        {
            var config = device.Config;

            Match match;

            match = Regex.Match(MCB_Energy_lockoutStartTime.Text, START_TIME_REGIX, RegexOptions.IgnoreCase);
            config.lockoutStartTime = uint.Parse(match.Groups[1].Value) * 3600 + uint.Parse(match.Groups[2].Value) * 60;

            match = Regex.Match(MCB_Energy_lockoutWindow.Text, START_TIME_REGIX, RegexOptions.IgnoreCase);
            config.lockoutCloseTime = config.lockoutStartTime + uint.Parse(match.Groups[1].Value) * 3600 + uint.Parse(match.Groups[2].Value) * 60;

            if (config.lockoutCloseTime != 86400)
                config.lockoutCloseTime %= (86400);

            config.lockoutDaysMask.Sunday = (MCB_Energy_lockoutDays.Items[6] as DayViewItem).IsSelected;
            config.lockoutDaysMask.Monday = (MCB_Energy_lockoutDays.Items[0] as DayViewItem).IsSelected;
            config.lockoutDaysMask.Tuesday = (MCB_Energy_lockoutDays.Items[1] as DayViewItem).IsSelected;
            config.lockoutDaysMask.Wednesday = (MCB_Energy_lockoutDays.Items[2] as DayViewItem).IsSelected;
            config.lockoutDaysMask.Thursday = (MCB_Energy_lockoutDays.Items[3] as DayViewItem).IsSelected;
            config.lockoutDaysMask.Friday = (MCB_Energy_lockoutDays.Items[4] as DayViewItem).IsSelected;
            config.lockoutDaysMask.Saturday = (MCB_Energy_lockoutDays.Items[5] as DayViewItem).IsSelected;

            match = Regex.Match(MCB_Energy_powerStartTime.Text, START_TIME_REGIX, RegexOptions.IgnoreCase);
            config.energyStartTime = uint.Parse(match.Groups[1].Value) * 3600 + uint.Parse(match.Groups[2].Value) * 60;

            match = Regex.Match(MCB_Energy_powerWindow.Text, START_TIME_REGIX, RegexOptions.IgnoreCase);
            config.energyCloseTime = config.energyStartTime + uint.Parse(match.Groups[1].Value) * 3600 + uint.Parse(match.Groups[2].Value) * 60;

            if (config.energyCloseTime != 86400)
                config.energyCloseTime %= (86400);

            config.energyDecreaseValue = byte.Parse(MCB_Energy_powerfactor.Text);
            config.energyDaysMask.Sunday = (MCB_Energy_powerDays.Items[6] as DayViewItem).IsSelected;
            config.energyDaysMask.Monday = (MCB_Energy_powerDays.Items[0] as DayViewItem).IsSelected;
            config.energyDaysMask.Tuesday = (MCB_Energy_powerDays.Items[1] as DayViewItem).IsSelected;
            config.energyDaysMask.Wednesday = (MCB_Energy_powerDays.Items[2] as DayViewItem).IsSelected;
            config.energyDaysMask.Thursday = (MCB_Energy_powerDays.Items[3] as DayViewItem).IsSelected;
            config.energyDaysMask.Friday = (MCB_Energy_powerDays.Items[4] as DayViewItem).IsSelected;
            config.energyDaysMask.Saturday = (MCB_Energy_powerDays.Items[5] as DayViewItem).IsSelected;
        }

        #endregion
    }
}
