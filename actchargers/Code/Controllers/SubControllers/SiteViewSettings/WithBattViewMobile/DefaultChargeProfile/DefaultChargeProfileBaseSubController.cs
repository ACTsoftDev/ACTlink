using static actchargers.ACUtility;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace actchargers
{
    public abstract class DefaultChargeProfileBaseSubController : WithBattViewMobileBaseSubController
    {
        #region Items

        internal ListViewItem Batt_trCurrentRateComboBox;
        internal ListViewItem Batt_ccCurrentRateComboBox;
        internal ListViewItem Batt_fiCurrentRateComboBox;
        internal ListViewItem Batt_eqCurrentRateComboBox;
        internal ListViewItem Batt_trickleVoltageTextBox;
        internal ListViewItem Batt_cvVoltageTextBox;
        internal ListViewItem Batt_finishVoltageTextBox;
        internal ListViewItem Batt_equalizeVoltageTextBox;
        internal ListViewItem Batt_cvFinishCurrentRateComboBox;
        internal ListViewItem Batt_cvCurrentStepComboBox;
        internal ListViewItem Batt_cvTimerComboBox;
        internal ListViewItem Batt_finishTimerComboBox;
        internal ListViewItem Batt_equalizeTimerComboBox;
        internal ListViewItem Batt_DesulfationTimerComboBox;
        internal ListViewItem Batt_finishDVVoltageComboBox;
        internal ListViewItem Batt_finishDTVoltageComboBox;

        internal ListViewItem MCB_ForceFinishDurationDisableRadio;

        #endregion

        protected DefaultChargeProfileBaseSubController
        (bool isBattView, bool isBattViewMobile, bool isSiteView)
            : base(isBattView, isBattViewMobile, isSiteView)
        {
        }

        internal override void InitSharedBattViewMobileItems()
        {
        }

        internal override void InitSharedRegularBattViewItems()
        {
            InitCrossSharedItems();
        }

        internal override void InitSharedMcbItems()
        {
            InitCrossSharedItems();

            MCB_ForceFinishDurationDisableRadio = new ListViewItem()
            {
                Index = 12,
                Title = AppResources.force_finish_timout,
                DefaultCellType = CellTypes.LabelLabel,
                EditableCellType = CellTypes.LabelSwitch,
            };
        }

        #region Init Cross Shared Items

        void InitCrossSharedItems()
        {
            Batt_trCurrentRateComboBox = new ListViewItem()
            {
                Index = 0,
                Title = AppResources.tr_current_rate,
                IsEditable = EditingMode,
                DefaultCellType = CellTypes.LabelLabel,
                EditableCellType = CellTypes.ListSelector,
                ListSelectionCommand = ListSelectorCommand,
            };
            Batt_ccCurrentRateComboBox = new ListViewItem()
            {
                Index = 1,
                Title = AppResources.cc_current_rate,
                IsEditable = EditingMode,
                DefaultCellType = CellTypes.LabelLabel,
                EditableCellType = CellTypes.ListSelector,
                ListSelectionCommand = ListSelectorCommand,
            };
            Batt_fiCurrentRateComboBox = new ListViewItem()
            {
                Index = 2,
                Title = AppResources.fi_current_rate,
                IsEditable = EditingMode,
                DefaultCellType = CellTypes.LabelLabel,
                EditableCellType = CellTypes.ListSelector,
                ListSelectionCommand = ListSelectorCommand,
            };
            Batt_eqCurrentRateComboBox = new ListViewItem()
            {
                Index = 3,
                Title = AppResources.eq_current_rate,
                IsEditable = EditingMode,
                DefaultCellType = CellTypes.LabelLabel,
                EditableCellType = CellTypes.ListSelector,
                ListSelectionCommand = ListSelectorCommand,
            };
            Batt_trickleVoltageTextBox = new ListViewItem()
            {
                Index = 4,
                Title = AppResources.tr_target_voltage,
                IsEditable = EditingMode,
                DefaultCellType = CellTypes.LabelLabel,
                EditableCellType = CellTypes.LabelTextEdit,
                TextInputType = InputType.Decimal
            };
            Batt_cvVoltageTextBox = new ListViewItem()
            {
                Index = 5,
                Title = AppResources.cv_target_voltage,
                IsEditable = EditingMode,
                DefaultCellType = CellTypes.LabelLabel,
                EditableCellType = CellTypes.LabelTextEdit,
                TextInputType = InputType.Decimal
            };
            Batt_finishVoltageTextBox = new ListViewItem()
            {
                Index = 6,
                Title = AppResources.fi_target_voltage,
                IsEditable = EditingMode,
                DefaultCellType = CellTypes.LabelLabel,
                EditableCellType = CellTypes.LabelTextEdit,
                TextInputType = InputType.Decimal
            };
            Batt_equalizeVoltageTextBox = new ListViewItem()
            {
                Index = 7,
                Title = AppResources.eq_target_voltage,
                IsEditable = EditingMode,
                DefaultCellType = CellTypes.LabelLabel,
                EditableCellType = CellTypes.LabelTextEdit,
                TextInputType = InputType.Decimal
            };
            Batt_cvFinishCurrentRateComboBox = new ListViewItem()
            {
                Index = 8,
                Title = AppResources.partial_charge_stop,
                IsEditable = EditingMode,
                DefaultCellType = CellTypes.LabelLabel,
                EditableCellType = CellTypes.ListSelector,
                ListSelectionCommand = ListSelectorCommand,
            };
            Batt_cvCurrentStepComboBox = new ListViewItem()
            {
                Index = 9,
                Title = AppResources.cv_current_step,
                IsEditable = EditingMode,
                DefaultCellType = CellTypes.LabelLabel,
                EditableCellType = CellTypes.ListSelector,
                ListSelectionCommand = ListSelectorCommand,
                KeepIndex = 1
            };
            Batt_cvTimerComboBox = new ListViewItem()
            {
                Index = 10,
                Title = AppResources.cv_timeout,
                IsEditable = EditingMode,
                DefaultCellType = CellTypes.LabelLabel,
                EditableCellType = CellTypes.ListSelector,
                ListSelectionCommand = ListSelectorCommand,

            };
            Batt_finishTimerComboBox = new ListViewItem()
            {
                Index = 11,
                Title = AppResources.finish_timeout,
                IsEditable = EditingMode,
                DefaultCellType = CellTypes.LabelLabel,
                EditableCellType = CellTypes.ListSelector,
                ListSelectionCommand = ListSelectorCommand,
            };
            Batt_equalizeTimerComboBox = new ListViewItem()
            {
                Index = 13,
                Title = AppResources.equilize_timeout,
                IsEditable = EditingMode,
                DefaultCellType = CellTypes.LabelLabel,
                EditableCellType = CellTypes.ListSelector,
                ListSelectionCommand = ListSelectorCommand,
            };
            Batt_DesulfationTimerComboBox = new ListViewItem()
            {
                Index = 14,
                Title = AppResources.desulfation_duration,
                IsEditable = EditingMode,
                DefaultCellType = CellTypes.LabelLabel,
                EditableCellType = CellTypes.ListSelector,
                ListSelectionCommand = ListSelectorCommand,
            };
            Batt_finishDVVoltageComboBox = new ListViewItem()
            {
                Index = 15,
                Title = AppResources.finish_dv_voltage,
                IsEditable = EditingMode,
                DefaultCellType = CellTypes.LabelLabel,
                EditableCellType = CellTypes.ListSelector,
                ListSelectionCommand = ListSelectorCommand,
            };
            Batt_finishDTVoltageComboBox = new ListViewItem()
            {
                Index = 16,
                Title = AppResources.finish_dt_time,
                IsEditable = EditingMode,
                DefaultCellType = CellTypes.LabelLabel,
                EditableCellType = CellTypes.ListSelector,
                ListSelectionCommand = ListSelectorCommand,
            };
        }

        #endregion

        internal async Task LoadLists(bool isLithiumAnd2_5OrAbove)
        {
            await Task.Run(() => LoadListsTask(isLithiumAnd2_5OrAbove));
        }

        void LoadListsTask(bool isLithiumAnd2_5OrAbove)
        {
            Batt_trCurrentRateComboBox.Items = GetTrCurrentRateList(isLithiumAnd2_5OrAbove);
            Batt_ccCurrentRateComboBox.Items = GetCcCurrentRateList(isLithiumAnd2_5OrAbove);
            Batt_fiCurrentRateComboBox.Items = GetFiCurrentRateList(isLithiumAnd2_5OrAbove);
            Batt_eqCurrentRateComboBox.Items = GetEqCurrentRateList();
            Batt_cvFinishCurrentRateComboBox.Items = GetCvFinishCurrentRateList();
            Batt_cvCurrentStepComboBox.Items = GetMcbCvCurrentStepList();

            McbLoadUIControlsFromLimits();
        }

        internal override void LoadExclusiveValues()
        {
        }

        internal override void AddExclusiveItems()
        {
        }

        internal List<object> GetBattViewCvCurrentStepList(float ccRate)
        {
            List<object> list = new List<object> { AppResources.default_title };

            for (float i = ControlObject.FormLimits.cvCurrentStep_min; i <= ControlObject.FormLimits.cvCurrentStep_max; i += 0.5f)
                list.Add(FormatRate(i, ccRate));

            return list;
        }

        internal List<object> GetTrCurrentRateList(bool isLithiumAnd2_5OrAbove)
        {
            List<object> list = new List<object>();

            int max = isLithiumAnd2_5OrAbove ? 100 : ControlObject.FormLimits.trCurrentRate_max;

            for (float i = ControlObject.FormLimits.trCurrentRate_min; i <= max; i++)
                list.Add(FormatRate(i, 1));

            return list;
        }

        internal List<object> GetCcCurrentRateList(bool isLithiumAnd2_5OrAbove)
        {
            List<object> list = new List<object>();

            int max = isLithiumAnd2_5OrAbove ? 100 : ControlObject.FormLimits.ccCurrentRate_max;

            for (float i = ControlObject.FormLimits.ccCurrentRate_min; i <= max; i++)
                list.Add(FormatRate(i, 1));

            return list;
        }

        internal List<object> GetFiCurrentRateList(bool isLithiumAnd2_5OrAbove)
        {
            List<object> list = new List<object>();

            int maxFI = ControlObject.FormLimits.fiCurrentRate_max;

            maxFI = MultiplyMaxFiIfuseNewEastPennProfile(maxFI);

            if (maxFI > 99)
                maxFI = 99;

            int max = maxFI;

            if (isLithiumAnd2_5OrAbove)
            {
                max = maxFI * 2;

                if (max > 50)
                    max = 50;
            }

            for (float i = ControlObject.FormLimits.fiCurrentRate_min; i < max; i += 0.5f)
                list.Add(FormatRate(i, 1));

            return list;
        }

        internal List<object> GetEqCurrentRateList()
        {
            List<object> list = new List<object>();

            for (float i = ControlObject.FormLimits.eqCurrentRate_min; i < ControlObject.FormLimits.eqCurrentRate_max; i++)
                list.Add(FormatRate(i, 1));

            return list;
        }

        internal List<object> GetCvFinishCurrentRateList()
        {
            List<object> list = new List<object>();

            Batt_cvFinishCurrentRateComboBox.Items = new List<object>();
            for (float i = ControlObject.FormLimits.cvFinishCurrentRate_min; i <= ControlObject.FormLimits.cvFinishCurrentRate_max; i += 0.5f)
                list.Add(FormatRate(i, 1));

            return list;
        }

        internal List<object> GetMcbCvCurrentStepList()
        {
            List<object> list = new List<object> { AppResources.default_title };

            float ccRate = GetCcRate();
            ccRate /= 100;

            for (float i = ControlObject.FormLimits.cvCurrentStep_min; i <= ControlObject.FormLimits.cvCurrentStep_max; i += 0.5f)
                list.Add(FormatRate(i, ccRate));

            return list;
        }

        float GetCcRate()
        {
            if (isBattView)
                return GetBattViewCcRate();

            return GetMcbCcRate();
        }

        internal abstract float GetBattViewCcRate();

        internal abstract float GetMcbCcRate();

        internal abstract int MultiplyMaxFiIfuseNewEastPennProfile(int maxFI);

        internal float GetValueFromRates(string value)
        {
            return float.Parse(value.Split(new char[] { '%' })[0]);
        }

        internal string FormatRate(float percent, float select)
        {
            if (isBattView)
                return BattViewFormatRate(percent, select);

            return McbFormatRate(percent, select);
        }

        internal abstract string BattViewFormatRate(float percent, float select);

        internal abstract string McbFormatRate(float percent, float select);

        internal void McbLoadUIControlsFromLimits()
        {
            Batt_cvTimerComboBox.Items = new List<object>();
            Batt_finishTimerComboBox.Items = new List<object>();
            Batt_equalizeTimerComboBox.Items = new List<object>();
            Batt_DesulfationTimerComboBox.Items = new List<object>();
            Batt_finishDTVoltageComboBox.Items = new List<object>();
            Batt_finishDVVoltageComboBox.Items = new List<object>();

            for (int i = ControlObject.FormLimits.cvTimerStart; i <= ControlObject.FormLimits.cvTimerEnd; i += ControlObject.FormLimits.cvTimerStep)
            {
                if (i != 0)
                    Batt_cvTimerComboBox.Items.Add(string.Format("{0:00}:{1:00}", i / 4, (i % 4) * 15));
                else
                    Batt_cvTimerComboBox.Items.Add(string.Format("00:01"));
            }

            for (int i = ControlObject.FormLimits.fiTimerStart; i <= ControlObject.FormLimits.fiTimerEnd; i += ControlObject.FormLimits.fiTimerStep)
            {
                if (i != 0)
                    Batt_finishTimerComboBox.Items.Add(string.Format("{0:00}:{1:00}", i / 4, (i % 4) * 15));
                else
                    Batt_finishTimerComboBox.Items.Add(string.Format("00:01"));
            }

            for (int i = ControlObject.FormLimits.eqTimerStart; i <= ControlObject.FormLimits.eqTimerEnd; i += ControlObject.FormLimits.eqTimerStep)
            {
                if (i != 0)
                    Batt_equalizeTimerComboBox.Items.Add(string.Format("{0:00}:{1:00}", i / 4, (i % 4) * 15));
                else
                    Batt_equalizeTimerComboBox.Items.Add(string.Format("00:01"));
            }

            for (int i = ControlObject.FormLimits.desTimerStart; i <= ControlObject.FormLimits.desTimerEnd; i += ControlObject.FormLimits.desTimerStep)
            {
                if (i != 0)
                    Batt_DesulfationTimerComboBox.Items.Add(string.Format("{0:00}:{1:00}", i / 4, (i % 4) * 15));
                else
                    Batt_DesulfationTimerComboBox.Items.Add(string.Format("00:01"));
            }

            for (int i = ControlObject.FormLimits.fidtStart; i <= ControlObject.FormLimits.fidtEnd; i++)
                Batt_finishDTVoltageComboBox.Items.Add(i.ToString());

            for (int i = ControlObject.FormLimits.fidVStart; i <= ControlObject.FormLimits.fidVEnd; i++)
                Batt_finishDVVoltageComboBox.Items.Add((i * 5).ToString());
        }

        #region Add BattView

        internal UIAccessControlUtility GetSharedBattViewAccessApply()
        {
            accessControlUtility = new UIAccessControlUtility();

            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_TR_CurrentRate, Batt_trCurrentRateComboBox, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_CC_CurrentRate, Batt_ccCurrentRateComboBox, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_FI_CurrentRate, Batt_fiCurrentRateComboBox, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_EQ_CurrentRate, Batt_eqCurrentRateComboBox, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_TrickleVoltage, Batt_trickleVoltageTextBox, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_CVVoltage, Batt_cvVoltageTextBox, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_finishVoltage, Batt_finishVoltageTextBox, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_EqualaizeVoltage, Batt_equalizeVoltageTextBox, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_cvCurrentStep, Batt_cvCurrentStepComboBox, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_cvFinishCurrent, Batt_cvFinishCurrentRateComboBox, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_CVMaxTimer, Batt_cvTimerComboBox, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_finishTimer, Batt_finishTimerComboBox, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_EqualizeTimer, Batt_equalizeTimerComboBox, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_desulfationTimer, Batt_DesulfationTimerComboBox, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_finishdVdT, Batt_finishDTVoltageComboBox, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_finishdVdT, Batt_finishDVVoltageComboBox, ItemSource);

            ShowEdit &= accessControlUtility.GetSavedCount() != 0;

            return accessControlUtility;
        }

        #endregion

        #region Add MCB 

        internal UIAccessControlUtility GetSharedMcbAccessApply(bool isLithiumAnd2_5OrAbove)
        {
            accessControlUtility = new UIAccessControlUtility();

            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_TR_CurrentRate, Batt_trCurrentRateComboBox, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_CC_CurrentRate, Batt_ccCurrentRateComboBox, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_FI_CurrentRate, Batt_fiCurrentRateComboBox, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_TrickleVoltage, Batt_trickleVoltageTextBox, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_CVVoltage, Batt_cvVoltageTextBox, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_finishVoltage, Batt_finishVoltageTextBox, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_cvCurrentStep, Batt_cvCurrentStepComboBox, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_cvFinishCurrent, Batt_cvFinishCurrentRateComboBox, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_CVMaxTimer, Batt_cvTimerComboBox, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_finishTimer, Batt_finishTimerComboBox, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_finishTimer, MCB_ForceFinishDurationDisableRadio, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_finishdVdT, Batt_finishDVVoltageComboBox, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_finishdVdT, Batt_finishDTVoltageComboBox, ItemSource);

            if (!isLithiumAnd2_5OrAbove)
            {
                accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_EQ_CurrentRate, Batt_eqCurrentRateComboBox, ItemSource);
                accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_EqualaizeVoltage, Batt_equalizeVoltageTextBox, ItemSource);
                accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_EqualizeTimer, Batt_equalizeTimerComboBox, ItemSource);
                accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_desulfationTimer, Batt_DesulfationTimerComboBox, ItemSource);
            }

            ShowEdit &= accessControlUtility.GetSavedCount() != 0;

            return accessControlUtility;
        }

        #endregion

        #region Verfiy Cross Shared Settings

        internal VerifyControl VerfiyCrossSharedSettings(bool isLithiumAnd2_5OrAbove)
        {
            VerifyControl verifyControl = new VerifyControl();

            verifyControl.VerifyComboBox(Batt_trCurrentRateComboBox, Batt_trCurrentRateComboBox);
            verifyControl.VerifyComboBox(Batt_ccCurrentRateComboBox, Batt_ccCurrentRateComboBox);
            verifyControl.VerifyComboBox(Batt_fiCurrentRateComboBox, Batt_fiCurrentRateComboBox);
            verifyControl.VerifyComboBox(Batt_eqCurrentRateComboBox, Batt_eqCurrentRateComboBox);

            if (isLithiumAnd2_5OrAbove)
            {
                verifyControl.VerifyFloatNumber(Batt_trickleVoltageTextBox, Batt_trickleVoltageTextBox, ControlObject.FormLimits.liIon_CellMin, ControlObject.FormLimits.liIon_CellMax);
                verifyControl.VerifyFloatNumber(Batt_cvVoltageTextBox, Batt_cvVoltageTextBox, ControlObject.FormLimits.liIon_CellMin, ControlObject.FormLimits.liIon_CellMax);
                verifyControl.VerifyFloatNumber(Batt_finishVoltageTextBox, Batt_finishVoltageTextBox, ControlObject.FormLimits.liIon_CellMin, ControlObject.FormLimits.liIon_CellMax);
            }
            else
            {
                verifyControl.VerifyFloatNumber(Batt_trickleVoltageTextBox, Batt_trickleVoltageTextBox, ControlObject.FormLimits.trVoltage_min, ControlObject.FormLimits.trVoltage_max);
                verifyControl.VerifyFloatNumber(Batt_cvVoltageTextBox, Batt_cvVoltageTextBox, ControlObject.FormLimits.cvVoltage_min, ControlObject.FormLimits.cvVoltage_max);
                verifyControl.VerifyFloatNumber(Batt_finishVoltageTextBox, Batt_finishVoltageTextBox, ControlObject.FormLimits.fiVoltage_min, ControlObject.FormLimits.fiVoltage_max);

            }

            verifyControl.VerifyFloatNumber(Batt_equalizeVoltageTextBox, Batt_equalizeVoltageTextBox, ControlObject.FormLimits.eqVoltage_min, ControlObject.FormLimits.eqVoltage_max);

            verifyControl.VerifyComboBox(Batt_cvFinishCurrentRateComboBox, Batt_cvFinishCurrentRateComboBox);
            verifyControl.VerifyComboBox(Batt_cvCurrentStepComboBox, Batt_cvCurrentStepComboBox);
            verifyControl.VerifyComboBox(Batt_cvTimerComboBox, Batt_cvTimerComboBox);
            verifyControl.VerifyComboBox(Batt_finishTimerComboBox, Batt_finishTimerComboBox);
            verifyControl.VerifyComboBox(Batt_equalizeTimerComboBox, Batt_equalizeTimerComboBox);
            verifyControl.VerifyComboBox(Batt_DesulfationTimerComboBox, Batt_DesulfationTimerComboBox);
            verifyControl.VerifyComboBox(Batt_finishDVVoltageComboBox, Batt_finishDVVoltageComboBox);
            verifyControl.VerifyComboBox(Batt_finishDTVoltageComboBox, Batt_finishDTVoltageComboBox);

            if (!verifyControl.HasErrors() && !isLithiumAnd2_5OrAbove)
            {
                if (GetValueFromRates(Batt_trCurrentRateComboBox.SelectedItem) > GetValueFromRates(Batt_ccCurrentRateComboBox.SelectedItem))
                    verifyControl.InsertRemoveFault(true, Batt_trCurrentRateComboBox);
                else
                    verifyControl.InsertRemoveFault(false, Batt_trCurrentRateComboBox);

                if (GetValueFromRates(Batt_fiCurrentRateComboBox.SelectedItem) > GetValueFromRates(Batt_ccCurrentRateComboBox.SelectedItem))
                    verifyControl.InsertRemoveFault(true, Batt_fiCurrentRateComboBox);
                else
                    verifyControl.InsertRemoveFault(false, Batt_fiCurrentRateComboBox);

                if (GetValueFromRates(Batt_eqCurrentRateComboBox.SelectedItem) > GetValueFromRates(Batt_fiCurrentRateComboBox.SelectedItem))
                    verifyControl.InsertRemoveFault(true, Batt_eqCurrentRateComboBox);
                else
                    verifyControl.InsertRemoveFault(false, Batt_eqCurrentRateComboBox);

                if (GetValueFromRates(Batt_cvFinishCurrentRateComboBox.SelectedItem) > GetValueFromRates(Batt_ccCurrentRateComboBox.SelectedItem))
                    verifyControl.InsertRemoveFault(true, Batt_cvFinishCurrentRateComboBox);
                else
                    verifyControl.InsertRemoveFault(false, Batt_cvFinishCurrentRateComboBox);

                if (float.Parse(Batt_finishVoltageTextBox.Text) < float.Parse(Batt_cvVoltageTextBox.Text))
                    verifyControl.InsertRemoveFault(true, Batt_finishVoltageTextBox);
                else
                    verifyControl.InsertRemoveFault(false, Batt_finishVoltageTextBox);

                if (float.Parse(Batt_equalizeVoltageTextBox.Text) < float.Parse(Batt_finishVoltageTextBox.Text))
                    verifyControl.InsertRemoveFault(true, Batt_equalizeVoltageTextBox);
                else
                    verifyControl.InsertRemoveFault(false, Batt_equalizeVoltageTextBox);

                if (float.Parse(Batt_cvVoltageTextBox.Text) < float.Parse(Batt_trickleVoltageTextBox.Text))
                    verifyControl.InsertRemoveFault(true, Batt_cvVoltageTextBox);
                else
                    verifyControl.InsertRemoveFault(false, Batt_cvVoltageTextBox);
            }

            return verifyControl;
        }

        #endregion

        #region Save Shared BattView

        internal void SaveSharedBattViewToConfigObject(BattViewObject device)
        {
            var config = device.Config;

            config.trickleCurrentRate = (UInt16)Math.Round(100 * GetValueFromRates(Batt_trCurrentRateComboBox.Text));
            config.CCrate = (UInt16)Math.Round(100 * GetValueFromRates(Batt_ccCurrentRateComboBox.Text));
            config.FIcurrentRate = (UInt16)Math.Round(100 * GetValueFromRates(Batt_fiCurrentRateComboBox.Text));
            config.EQcurrentRate = (UInt16)Math.Round(100 * GetValueFromRates(Batt_eqCurrentRateComboBox.Text));
            config.trickleVoltage = (UInt16)Math.Round(100.0f * float.Parse(Batt_trickleVoltageTextBox.Text));
            config.CVTargetVoltage = (UInt16)Math.Round(100.0f * float.Parse(Batt_cvVoltageTextBox.Text));
            config.FItargetVoltage = (UInt16)Math.Round(100.0f * float.Parse(Batt_finishVoltageTextBox.Text));
            config.EQvoltage = (UInt16)Math.Round(100.0f * float.Parse(Batt_equalizeVoltageTextBox.Text));
            config.CVendCurrentRate = (byte)Math.Round(2 * GetValueFromRates(Batt_cvFinishCurrentRateComboBox.Text));

            if (Batt_cvCurrentStepComboBox.Text == AppResources.default_title)
                config.CVcurrentStep = 0;
            else
                config.CVcurrentStep = (byte)Math.Round(2 * GetValueFromRates(Batt_cvCurrentStepComboBox.Text));

            Match match;
            match = Regex.Match(Batt_cvTimerComboBox.Text, @"^(\d{1,2}){1}:(\d{1,2}){1}$", RegexOptions.IgnoreCase);
            config.cvMaxDuration = UInt32.Parse(match.Groups[1].Value) * 3600 + UInt32.Parse(match.Groups[2].Value) * 60;

            match = Regex.Match(Batt_finishTimerComboBox.Text, @"^(\d{1,2}){1}:(\d{1,2}){1}$", RegexOptions.IgnoreCase);
            config.FIduration = UInt32.Parse(match.Groups[1].Value) * 3600 + UInt32.Parse(match.Groups[2].Value) * 60;

            match = Regex.Match(Batt_equalizeTimerComboBox.Text, @"^(\d{1,2}){1}:(\d{1,2}){1}$", RegexOptions.IgnoreCase);
            config.EQduration = UInt32.Parse(match.Groups[1].Value) * 3600 + UInt32.Parse(match.Groups[2].Value) * 60;

            match = Regex.Match(Batt_DesulfationTimerComboBox.Text, @"^(\d{1,2}){1}:(\d{1,2}){1}$", RegexOptions.IgnoreCase);
            config.desulfation = UInt32.Parse(match.Groups[1].Value) * 3600 + UInt32.Parse(match.Groups[2].Value) * 60;

            config.FIdv = byte.Parse(Batt_finishDVVoltageComboBox.Text);
            config.FIdt = byte.Parse(Batt_finishDTVoltageComboBox.Text);
        }

        #endregion

        #region Save Shared MCB

        internal void SaveSharedMcbToConfigObject(MCBobject device, bool isLithiumAnd2_5OrAbove)
        {
            var config = device.Config;

            config.TRrate = (UInt16)(100 * GetValueFromRates(Batt_trCurrentRateComboBox.SelectedItem));
            config.CCrate = (UInt16)(100 * GetValueFromRates(Batt_ccCurrentRateComboBox.SelectedItem));
            config.FIrate = (UInt16)(100 * GetValueFromRates(Batt_fiCurrentRateComboBox.SelectedItem));
            config.EQrate = (UInt16)(100 * GetValueFromRates(Batt_eqCurrentRateComboBox.SelectedItem));

            float factor = 1.0f;

            if (isLithiumAnd2_5OrAbove)
                factor = config.enableAutoDetectMultiVoltage ? 1.5f : config.LiIon_CellVoltage / 200.0f;

            config.trickleVoltage = Math.Round(float.Parse(Batt_trickleVoltageTextBox.Text) / factor, 2).ToString();
            config.CVvoltage = Math.Round(float.Parse(Batt_cvVoltageTextBox.Text) / factor, 2).ToString();
            config.FIvoltage = Math.Round(float.Parse(Batt_finishVoltageTextBox.Text) / factor, 2).ToString();

            config.EQvoltage = Batt_equalizeVoltageTextBox.Text;
            config.CVfinishCurrent = (byte)(2 * GetValueFromRates(Batt_cvFinishCurrentRateComboBox.SelectedItem));

            if (Batt_cvCurrentStepComboBox.Text == AppResources.default_title)
                config.CVcurrentStep = 0;
            else
                config.CVcurrentStep = (byte)(2 * GetValueFromRates(Batt_cvCurrentStepComboBox.SelectedItem));

            config.CVtimer = Batt_cvTimerComboBox.SelectedItem;
            config.finishTimer = Batt_finishTimerComboBox.SelectedItem;
            config.forceFinishTimeout = MCB_ForceFinishDurationDisableRadio.IsSwitchEnabled;
            config.EQtimer = Batt_equalizeTimerComboBox.SelectedItem;
            config.desulfationTimer = Batt_DesulfationTimerComboBox.SelectedItem;
            config.finishDV = (byte)(int.Parse(Batt_finishDVVoltageComboBox.SelectedItem));
            config.finishDT = Batt_finishDTVoltageComboBox.SelectedItem;
        }

        #endregion
    }
}
