using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;
using static actchargers.ACUtility;

namespace actchargers
{
    public class NewStudySubController : WithBattViewMobileBaseSubController
    {
        ListViewItem TruckId;
        ListViewItem StudyName;
        ListViewItem StartNewStudy;

        public NewStudySubController() : base(true, true, false)
        {
        }

        public async override Task Start()
        {
            await base.Start();

            ShowEdit = false;
        }

        #region Init Items

        internal override void InitSharedBattViewMobileItems()
        {
            StudyName = new ListViewItem
            {
                Index = 0,
                Title = AppResources.study_name,
                Text = string.Empty,
                DefaultCellType = CellTypes.LabelTextEdit,
                EditableCellType = CellTypes.LabelTextEdit,
                IsEditable = true,
                TextMaxLength = 17
            };

            TruckId = new ListViewItem
            {
                Index = 1,
                Title = AppResources.truck_id,
                Text = string.Empty,
                DefaultCellType = CellTypes.LabelTextEdit,
                EditableCellType = CellTypes.LabelTextEdit,
                IsEditable = true,
                TextMaxLength = 17
            };

            StartNewStudy = new ListViewItem
            {
                Index = 2,
                Title = AppResources.start_new_study,
                DefaultCellType = CellTypes.Button,
                EditableCellType = CellTypes.Button,
                ListSelectionCommand = StartNewStudyButtonClicked
            };
        }

        #endregion

        #region Start New Study

        IMvxCommand StartNewStudyButtonClicked
        {
            get { return new MvxCommand(OnStartNewStudy); }
        }

        void OnStartNewStudy()
        {
            ACUserDialogs
                .ShowAlertWithTwoButtons
                (AppResources.start_new_study_alert_message,
                 AppResources.start_new_study_alert_title,
                 AppResources.ok,
                 AppResources.cancel,
                 OnYes,
                 null);
        }

        void OnYes()
        {
            Task.Run(OnYesTask);
        }

        async Task OnYesTask()
        {
            var verifyControl = VerifyInputs();
            if (verifyControl.HasErrors())
            {
                ShowVerificationError(verifyControl);

                return;
            }

            ACUserDialogs.ShowProgress();
            BattViewCommunicationTypes caller = BattViewCommunicationTypes.NOCall;
            bool arg1 = false;

            var config = BattViewQuantum.Instance.GetBATTView().Config;

            config.studyName = StudyName.Text;
            config.TruckId = TruckId.Text;
            config.installationDate = DateTime.UtcNow;

            caller = BattViewCommunicationTypes.startNewStudy;

            List<object> arguments = new List<object> { caller, arg1 };

            List<object> results = new List<object>();
            try
            {
                results = await BattViewQuantum.Instance.CommunicateBATTView(arguments);

                if (results.Count > 0)
                {
                    var status = (CommunicationResult)results[2];
                    if (status == CommunicationResult.OK)
                    {
                        try
                        {
                            ACUserDialogs.HideProgress();
                            Mvx.Resolve<IMvxMessenger>().Publish(new DeviceResetMessage(this, BattViewQuantum.Instance.GetBATTView().IPAddress));

                            FireOnClosed();

                            ACUserDialogs.ShowAlert(AppResources.batt_view_restarting);

                            return;
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

        VerifyControl VerifyInputs()
        {
            VerifyControl verifyControl = new VerifyControl();

            verifyControl.VerifyTextBox(StudyName, StudyName, 1, 17);
            verifyControl.VerifyTextBox(TruckId, TruckId, 1, 17);

            return verifyControl;
        }

        #endregion

        internal override void InitSharedMcbItems()
        {
        }

        internal override void InitSharedRegularBattViewItems()
        {
        }

        internal override void InitExclusiveBattViewMobileItems()
        {
        }

        internal override void InitExclusiveRegularBattViewItems()
        {
        }

        internal override void InitExclusiveMcbItems()
        {
        }

        internal override void LoadBattViewMobileValues()
        {
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

        public override void LoadDefaults()
        {
        }

        internal override int BattViewMobileAccessApply()
        {
            accessControlUtility = new UIAccessControlUtility();

            accessControlUtility.DoApplyAccessControl(AccessLevelConsts.write, StudyName, ItemSource);

            accessControlUtility.DoApplyAccessControl(AccessLevelConsts.write, TruckId, ItemSource);

            accessControlUtility.DoApplyAccessControl(AccessLevelConsts.write, StartNewStudy, ItemSource);

            return accessControlUtility.GetVisibleCount();
        }

        internal override int RegularBattViewAccessApply()
        {
            return 0;
        }

        internal override int McbAccessApply()
        {
            return 0;
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
