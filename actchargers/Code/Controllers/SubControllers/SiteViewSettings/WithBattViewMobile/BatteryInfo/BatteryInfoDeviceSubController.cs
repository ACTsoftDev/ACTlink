using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using MvvmCross.Core.ViewModels;

namespace actchargers
{
    public class BatteryInfoDeviceSubController : BatteryInfoBaseSubController
    {
        ListViewItem StartNewStudy;
        ListViewItem StudyName;
        ListViewItem RunBattViewCalibration;

        public BatteryInfoDeviceSubController(bool isBattView, bool isBattViewMobile)
            : base(isBattView, isBattViewMobile, false)
        {
        }

        internal override void InitExclusiveBattViewMobileItems()
        {
            StartNewStudy = new ListViewItem
            {
                Title = AppResources.start_new_study,
                ViewModelType = typeof(NewStudyViewModel),
                DefaultCellType = ACUtility.CellTypes.Label
            };

            StudyName = new ListViewItem
            {
                Title = AppResources.study_name,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.LabelLabel,
                IsEditEnabled = false
            };
        }

        internal override void InitExclusiveRegularBattViewItems()
        {
        }

        internal override void InitExclusiveMcbItems()
        {
            if (CanAddRunBattViewCalibration())
            {
                RunBattViewCalibration = new ListViewItem
                {
                    DefaultCellType = ACUtility.CellTypes.Button,
                    Title = AppResources.run_battview_calibration,
                    ListSelectionCommand = CalibrationSelectionCommand
                };
            }
        }

        #region Calibration

        IMvxCommand CalibrationSelectionCommand
        {
            get
            {
                return new MvxCommand(ExecuteCalibrationSelectionCommand);
            }
        }

        void ExecuteCalibrationSelectionCommand()
        {
            Task.Run(ExecuteCalibrationSelectionTask);
        }

        async Task ExecuteCalibrationSelectionTask()
        {
            ACUserDialogs.ShowProgress();
            object[] arg1 = { (byte)0x03, (uint)0x00 };

            McbCommunicationTypes caller = McbCommunicationTypes.NOCall;
            try
            {
                caller = McbCommunicationTypes.switchMode;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            List<object> arguments = new List<object>
            {
                caller,
                arg1
            };

            List<object> results = new List<object>();
            try
            {
                results = await MCBQuantum.Instance.CommunicateMCB(arguments);
                if (results.Count > 0)
                {
                    var status = (CommunicationResult)results[2];
                    if (status == CommunicationResult.OK)
                    {
                        try
                        {
                            ACUserDialogs.ShowAlertWithTitleAndOkButton(AppResources.battView_calibration_success);
                        }

                        catch (Exception ex)
                        {
                            Debug.WriteLine(ex.Message);

                            Logger.AddLog(true, "X24" + ex);
                        }
                    }
                    else
                    {
                        ACUserDialogs.ShowAlertWithTitleAndOkButton(AppResources.save_failed);
                    }
                }
                else
                {
                    ACUserDialogs.ShowAlertWithTitleAndOkButton(AppResources.save_failed);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            ACUserDialogs.HideProgress();
        }

        #endregion

        internal override void LoadBattViewMobileValues()
        {
            var currentBattView = BattViewQuantum.Instance.GetBATTView();
            var config = currentBattView.Config;

            if (config.studyId == 0)
                StudyName.SubTitle = StudyName.Text = "";
            else
                StudyName.SubTitle = StudyName.Text = config.studyName;
        }

        internal override int BattViewMobileAccessApply()
        {
            AddBatterySettings();

            ItemSource.Add(StartNewStudy);
            ItemSource.Add(StudyName);

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

            var currentMcb = MCBQuantum.Instance.GetMCB();

            bool isLithiumAnd2_5OrAbove = McbHelper.IsLithiumAnd2_5OrAbove(currentMcb);

            if (!isLithiumAnd2_5OrAbove)
                ItemSource.Add(RunBattViewCalibration);

            return ItemSource.Count;
        }

        bool CanAddRunBattViewCalibration()
        {
            try
            {
                return TryCanAddRunBattViewCalibration();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());

                return false;
            }
        }

        bool TryCanAddRunBattViewCalibration()
        {
            return (ControlObject.UserAccess.MCB_RunBattViewCal == AccessLevelConsts.write)
                && (MCBQuantum.Instance.GetMCB().Config.enablePLC);
        }
    }
}
