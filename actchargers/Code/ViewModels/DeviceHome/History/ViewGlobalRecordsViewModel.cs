using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;

namespace actchargers
{
    public class ViewGlobalRecordsViewModel : BaseViewModel
    {
        public string ResetButtonTitle { get; set; }
        public bool IsResetButtonVisible { get; set; }

        ObservableCollection<ListViewItem> _viewGlobalRecordsItemSource;
        public ObservableCollection<ListViewItem> ViewGlobalRecordsItemSource
        {
            get { return _viewGlobalRecordsItemSource; }
            set
            {
                _viewGlobalRecordsItemSource = value;
                RaisePropertyChanged(() => ViewGlobalRecordsItemSource);
            }
        }

        ListViewItem MCB_TotalChargeAHRs;
        ListViewItem MCB_TotalChargeKWHrs;
        ListViewItem MCB_TotalChargeInKWHRs;
        ListViewItem MCB_TotalChargeDuration;
        ListViewItem MCB_TotalChargeCycles;
        ListViewItem MCB_TotalChargePMFaults;

        public ViewGlobalRecordsViewModel()
        {
            ViewTitle = AppResources.view_global_records;

            IsResetButtonVisible = false;

            ResetButtonTitle = AppResources.reset;

            ViewGlobalRecordsItemSource = new ObservableCollection<ListViewItem>();

            CreateList();
        }

        void CreateList()
        {
            ViewTitle = AppResources.view_global_records;

            MCB_TotalChargeAHRs = new ListViewItem()
            {
                Index = 0,
                Title = AppResources.total_charge,
                SubTitle2 = AppResources.ahrs,
                DefaultCellType = ACUtility.CellTypes.LabelLabel
            };
            MCB_TotalChargeKWHrs = new ListViewItem()
            {
                Index = 1,
                Title = AppResources.total_charge,
                SubTitle2 = AppResources.kwhrs,
                DefaultCellType = ACUtility.CellTypes.LabelLabel
            };
            MCB_TotalChargeInKWHRs = new ListViewItem()
            {
                Index = 2,
                Title = AppResources.total_charge_consumption,
                SubTitle2 = AppResources.kwhrs,
                DefaultCellType = ACUtility.CellTypes.LabelLabel
            };
            MCB_TotalChargeDuration = new ListViewItem()
            {
                Index = 3,
                Title = AppResources.total_charge_duration,
                SubTitle2 = AppResources.days,
                DefaultCellType = ACUtility.CellTypes.LabelLabel
            };
            MCB_TotalChargeCycles = new ListViewItem()
            {
                Index = 4,
                Title = AppResources.total_charge_cycles,
                SubTitle2 = AppResources.k,
                DefaultCellType = ACUtility.CellTypes.LabelLabel
            };
            MCB_TotalChargePMFaults = new ListViewItem()
            {
                Index = 5,
                Title = AppResources.total_pm_faults,
                DefaultCellType = ACUtility.CellTypes.LabelLabel
            };

            try
            {
                MCB_loadViewGlobalRecords();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);

            }

            if (ChargerGlobalRecordAccessApply() == 0)
            {
                ACUserDialogs.ShowAlertWithOkButtonsAction(AppResources.no_data_found, () => { ShowViewModel<ViewGlobalRecordsViewModel>(new { pop = "pop" }); });
                return;
            }

            if (ViewGlobalRecordsItemSource.Count > 0)
            {
                ViewGlobalRecordsItemSource = new ObservableCollection<ListViewItem>(ViewGlobalRecordsItemSource.OrderBy(o => o.Index));
            }
        }

        void MCB_loadViewGlobalRecords()
        {
            MCBobject currentMcb = MCBQuantum.Instance.GetMCB();
            MCB_TotalChargeAHRs.SubTitle = MCB_TotalChargeAHRs.Text = currentMcb.globalRecord.AHR.ToString("N0");
            MCB_TotalChargeKWHrs.SubTitle = MCB_TotalChargeAHRs.Text = currentMcb.globalRecord.KWHR.ToString("N0");

            float eff = 0;

            if (!float.TryParse(currentMcb.Config.PMefficiency, out eff))
                eff = 1;

            MCB_TotalChargeInKWHRs.SubTitle = MCB_TotalChargeInKWHRs.Text = (100.0f * currentMcb.globalRecord.KWHR / eff).ToString("N0");
            MCB_TotalChargeDuration.SubTitle = MCB_TotalChargeDuration.Text = ((double)currentMcb.globalRecord.totalChargeSeconds / 3600).ToString("N0");
            MCB_TotalChargeCycles.SubTitle = MCB_TotalChargeCycles.Text = currentMcb.globalRecord.chargeCycles.ToString("N0");
            MCB_TotalChargePMFaults.SubTitle = MCB_TotalChargePMFaults.Text = currentMcb.globalRecord.PMfaults.ToString("N0");
        }

        int ChargerGlobalRecordAccessApply()
        {
            UIAccessControlUtility accessControlUtility = new UIAccessControlUtility();

            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_ViewGlobalRecords, MCB_TotalChargeAHRs, ViewGlobalRecordsItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_ViewGlobalRecords, MCB_TotalChargeKWHrs, ViewGlobalRecordsItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_ViewGlobalRecords, MCB_TotalChargeInKWHRs, ViewGlobalRecordsItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_ViewGlobalRecords, MCB_TotalChargeCycles, ViewGlobalRecordsItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_ViewGlobalRecords, MCB_TotalChargeDuration, ViewGlobalRecordsItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_ViewTotalPMFaults, MCB_TotalChargePMFaults, ViewGlobalRecordsItemSource);

            IsResetButtonVisible =
                ControlObject.UserAccess.MCB_canResetGlobalRecords != AccessLevelConsts.noAccess;

            return accessControlUtility.GetVisibleCount();
        }

        public IMvxCommand ResetBtnClickCommand
        {
            get { return new MvxCommand(OnResetClick); }
        }

        void OnResetClick()
        {
            ACUserDialogs
                .ShowAlertWithTwoButtons
                (AppResources.alert_reset_global_records_cycles_pm, "",
                 AppResources.yes, AppResources.no,
                 OnYes, null);
        }

        void OnYes()
        {
            Task.Run(OnYesClickTask);
        }

        async Task OnYesClickTask()
        {
            ACUserDialogs.ShowProgress();
            McbCommunicationTypes caller = McbCommunicationTypes.NOCall;
            bool arg1 = false;

            try
            {
                caller = McbCommunicationTypes.resetGlobalRecords;
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
                            Mvx.Resolve<IMvxMessenger>().Publish(new DeviceResetMessage(this, MCBQuantum.Instance.GetMCB().IPAddress));
                            ShowViewModel<ConnectToDeviceViewModel>(new { popto = "popto" });
                            ACUserDialogs.ShowAlert(AppResources.charger_restarting);
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine(ex.Message);
                            Logger.AddLog(true, "X24" + ex);
                        }
                    }
                    else
                    {
                        ACUtility.SetStatus(status);
                    }
                }
                else
                {
                    ACUserDialogs.ShowAlertWithTitleAndOkButton(AppResources.failed);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            ACUserDialogs.HideProgress();
        }
    }
}
