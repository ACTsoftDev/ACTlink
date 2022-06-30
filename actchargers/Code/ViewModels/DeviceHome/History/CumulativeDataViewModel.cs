using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;

namespace actchargers
{
    public class CumulativeDataViewModel : BaseViewModel
    {
        public string ResetButtonTitle { get; set; }

        public bool IsResetButtonVisible { get; set; }

        ObservableCollection<ListViewItem> _cumulativeDataViewItemSource;
        public ObservableCollection<ListViewItem> CumulativeDataViewItemSource
        {
            get { return _cumulativeDataViewItemSource; }
            set
            {
                _cumulativeDataViewItemSource = value;
                RaisePropertyChanged(() => CumulativeDataViewItemSource);
            }
        }

        ListViewItem Batt_GR_TotalEventsCount;
        ListViewItem Batt_GR_TotalInUseAHRs;
        ListViewItem Batt_GR_TotalInUseKWHRs;
        ListViewItem Batt_GR_TotalInUseTime;
        ListViewItem Batt_GR_TotalChargeAHRs;
        ListViewItem Batt_GR_TotalChargeKWHRs;
        ListViewItem Batt_GR_TotalChargeTime;
        ListViewItem Batt_GR_TotalIdleTime;
        ListViewItem Batt_GR_rt_sequence;
        ListViewItem Batt_GR_Signature;
        ListViewItem Batt_GR_SEQ;
        ListViewItem Batt_GR_leftOverCharge_AS;
        ListViewItem Batt_GR_leftOverInUSE_AS;
        ListViewItem Batt_GR_leftOverInUSE_WS;
        ListViewItem Batt_GR_leftOverCharge_WS;
        ListViewItem Batt_GR_endSignature;
        ListViewItem Batt_GR_debug_sequence;

        public CumulativeDataViewModel()
        {
            ViewTitle = AppResources.cummulative_data;
            ResetButtonTitle = AppResources.reset;

            IsResetButtonVisible = false;

            CumulativeDataViewItemSource = new ObservableCollection<ListViewItem>();

            CreateListForBattView();
        }

        void CreateListForBattView()
        {
            Batt_GR_TotalEventsCount = new ListViewItem()
            {
                Title = AppResources.total_events,
                DefaultCellType = ACUtility.CellTypes.LabelLabel
            };
            Batt_GR_TotalInUseAHRs = new ListViewItem()
            {
                Title = AppResources.in_use,
                Title2 = AppResources.charge,
                Title3 = AppResources.in_use,
                DefaultCellType = ACUtility.CellTypes.ThreeLabel
            };
            Batt_GR_TotalChargeAHRs = new ListViewItem();
            Batt_GR_TotalInUseKWHRs = new ListViewItem();

            Batt_GR_TotalInUseTime = new ListViewItem()
            {
                Title = AppResources.in_use_time,
                Title2 = AppResources.charge_time,
                Title3 = AppResources.idle_time,
                DefaultCellType = ACUtility.CellTypes.ThreeLabel
            };
            Batt_GR_TotalChargeTime = new ListViewItem();
            Batt_GR_TotalIdleTime = new ListViewItem();

            Batt_GR_TotalChargeKWHRs = new ListViewItem()
            {
                Title = AppResources.charge,
                DefaultCellType = ACUtility.CellTypes.LabelLabel
            };

            Batt_GR_rt_sequence = new ListViewItem()
            {
                Title = AppResources.total_rt_records,
                DefaultCellType = ACUtility.CellTypes.LabelLabel
            };

            Batt_GR_Signature = new ListViewItem()
            {
                Title = AppResources.signature,
                Title2 = AppResources.seq,
                Title3 = AppResources.end_signature,
                DefaultCellType = ACUtility.CellTypes.ThreeLabel
            };
            Batt_GR_SEQ = new ListViewItem();
            Batt_GR_endSignature = new ListViewItem();

            Batt_GR_leftOverCharge_AS = new ListViewItem()
            {
                Title = AppResources.leftover_in_use_as,
                Title2 = AppResources.leftover_in_use_Ws,
                DefaultCellType = ACUtility.CellTypes.TwoLabel
            };
            Batt_GR_leftOverInUSE_WS = new ListViewItem();

            Batt_GR_leftOverInUSE_AS = new ListViewItem()
            {

                Title = AppResources.leftover_in_charge_as,
                Title2 = AppResources.leftover_in_charge_Ws,
                DefaultCellType = ACUtility.CellTypes.TwoLabel
            };
            Batt_GR_leftOverCharge_WS = new ListViewItem();

            Batt_GR_debug_sequence = new ListViewItem()
            {
                Title = AppResources.total_debug_records,
                DefaultCellType = ACUtility.CellTypes.LabelLabel
            };

            IsResetButtonVisible =
                ControlObject.UserAccess.Batt_canResetGlobalRecords != AccessLevelConsts.noAccess;

            if (BattViewGlobalRecordAccessApply() == 0)
            {
                ACUserDialogs.ShowAlertWithOkButtonsAction(AppResources.no_data_found, () => { ShowViewModel<CumulativeDataViewModel>(new { pop = "pop" }); });
                return;
            }

            try
            {
                Batt_LoadGlobalRecords();
            }
            catch (Exception ex)
            {
                Logger.AddLog(true, "X29" + ex.ToString());
            }


            CumulativeDataViewItemSource.Add(Batt_GR_TotalEventsCount);
            CumulativeDataViewItemSource.Add(Batt_GR_TotalInUseAHRs);
            CumulativeDataViewItemSource.Add(Batt_GR_TotalInUseTime);
            CumulativeDataViewItemSource.Add(Batt_GR_TotalChargeKWHRs);
            CumulativeDataViewItemSource.Add(Batt_GR_rt_sequence);
            CumulativeDataViewItemSource.Add(Batt_GR_Signature);
            CumulativeDataViewItemSource.Add(Batt_GR_leftOverCharge_AS);
            CumulativeDataViewItemSource.Add(Batt_GR_leftOverInUSE_AS);
            CumulativeDataViewItemSource.Add(Batt_GR_debug_sequence);
            RaisePropertyChanged(() => CumulativeDataViewItemSource);
        }

        int BattViewGlobalRecordAccessApply()
        {
            UIAccessControlUtility accessControlUtility = new UIAccessControlUtility();

            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_ViewGlobalRecords, Batt_GR_TotalEventsCount, null);

            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_ViewGlobalRecords, Batt_GR_TotalInUseAHRs, null);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_ViewGlobalRecords, Batt_GR_TotalInUseKWHRs, null);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_ViewGlobalRecords, Batt_GR_TotalInUseTime, null);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_ViewGlobalRecords, Batt_GR_TotalChargeAHRs, null);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_ViewGlobalRecords, Batt_GR_TotalChargeKWHRs, null);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_ViewGlobalRecords, Batt_GR_TotalChargeTime, null);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_ViewGlobalRecords, Batt_GR_TotalIdleTime, null);

            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_ViewDebugGlobalRecords, Batt_GR_rt_sequence, null);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_ViewDebugGlobalRecords, Batt_GR_Signature, null);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_ViewDebugGlobalRecords, Batt_GR_SEQ, null);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_ViewDebugGlobalRecords, Batt_GR_leftOverCharge_AS, null);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_ViewDebugGlobalRecords, Batt_GR_leftOverInUSE_AS, null);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_ViewDebugGlobalRecords, Batt_GR_leftOverInUSE_WS, null);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_ViewDebugGlobalRecords, Batt_GR_leftOverCharge_WS, null);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_ViewDebugGlobalRecords, Batt_GR_endSignature, null);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_ViewDebugGlobalRecords, Batt_GR_debug_sequence, null);

            return accessControlUtility.GetVisibleCount();
        }

        void Batt_LoadGlobalRecords()
        {
            BattViewObject currentBattView = BattViewQuantum.Instance.GetBATTView();

            Batt_GR_TotalEventsCount.SubTitle = Batt_GR_TotalEventsCount.Text = currentBattView.globalRecord.eventsCount.ToString("N0");
            Batt_GR_TotalInUseAHRs.SubTitle = Batt_GR_TotalInUseAHRs.Text = currentBattView.globalRecord.inUseAHR.ToString("N0") + " AHRS";
            Batt_GR_TotalInUseAHRs.SubTitle3 = Batt_GR_TotalInUseKWHRs.Text = currentBattView.globalRecord.inUseKWHR.ToString("N1").TrimEnd('0').TrimEnd(System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator[0]) + " KWHrs";
            Batt_GR_TotalInUseTime.SubTitle = Batt_GR_TotalInUseTime.Text = ((double)currentBattView.globalRecord.inUseSeconds / 3600).ToString("N0");

            Batt_GR_TotalInUseAHRs.SubTitle2 = Batt_GR_TotalChargeAHRs.Text = currentBattView.globalRecord.chargeAHR.ToString("N0") + " AHRS";
            Batt_GR_TotalChargeKWHRs.SubTitle = Batt_GR_TotalChargeKWHRs.Text = currentBattView.globalRecord.chargeKWHR.ToString("N1").TrimEnd('0').TrimEnd(System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator[0]) + " KWHrs";
            Batt_GR_TotalInUseTime.SubTitle2 = Batt_GR_TotalChargeTime.Text = ((double)currentBattView.globalRecord.chargeSeconds / 3600).ToString("N0");
            Batt_GR_TotalInUseTime.SubTitle3 = Batt_GR_TotalIdleTime.Text = ((double)currentBattView.globalRecord.idleSeconds / 3600).ToString("N0");

            Batt_GR_rt_sequence.SubTitle = Batt_GR_rt_sequence.Text = currentBattView.globalRecord.RTrecordsCount.ToString("N0");
            Batt_GR_Signature.SubTitle = Batt_GR_Signature.Text = currentBattView.globalRecord.signature.ToString("x4");
            Batt_GR_Signature.SubTitle2 = Batt_GR_SEQ.Text = currentBattView.globalRecord.seq.ToString();
            Batt_GR_leftOverCharge_AS.SubTitle = Batt_GR_leftOverCharge_AS.Text = currentBattView.globalRecord.leftoverchargeas.ToString("N0");
            Batt_GR_leftOverInUSE_AS.SubTitle = Batt_GR_leftOverInUSE_AS.Text = currentBattView.globalRecord.leftoverinuseas.ToString("N0");
            Batt_GR_leftOverCharge_AS.SubTitle2 = Batt_GR_leftOverInUSE_WS.Text = currentBattView.globalRecord.leftoverinusews.ToString("N0");
            Batt_GR_leftOverInUSE_AS.SubTitle2 = Batt_GR_leftOverCharge_WS.Text = currentBattView.globalRecord.leftoverchargews.ToString("N0");
            Batt_GR_Signature.SubTitle3 = Batt_GR_endSignature.Text = currentBattView.globalRecord.endSignature.ToString();
            Batt_GR_debug_sequence.SubTitle = Batt_GR_debug_sequence.Text = currentBattView.globalRecord.debugCount.ToString("N0");
        }

        public IMvxCommand ResetBtnClickCommand
        {
            get { return new MvxCommand(OnResetClick); }
        }

        void OnResetClick()
        {
            ACUserDialogs
                .ShowAlertWithTwoButtons
                (AppResources.alert_reset_global_records_events, "",
                 AppResources.yes, AppResources.no,
                 OnYe, null);
        }

        void OnYe()
        {
            Task.Run(OnYesTask);
        }

        async Task OnYesTask()
        {
            ACUserDialogs.ShowProgress();
            BattViewCommunicationTypes caller = BattViewCommunicationTypes.NOCall;
            bool arg1 = false;

            try
            {
                caller = BattViewCommunicationTypes.resetGlobalRecords;
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
                results = await BattViewQuantum.Instance.CommunicateBATTView(arguments);
                if (results.Count > 0)
                {
                    var status = (CommunicationResult)results[2];
                    if (status == CommunicationResult.OK)
                    {
                        try
                        {
                            Mvx.Resolve<IMvxMessenger>().Publish(new DeviceResetMessage(this, BattViewQuantum.Instance.GetBATTView().IPAddress));
                            ShowViewModel<ConnectToDeviceViewModel>(new { popto = "popto" });
                            ACUserDialogs.ShowAlert("BATTView Restarting...");
                        }

                        catch (Exception ex)
                        {
                            Debug.WriteLine(ex.Message);
                            Logger.AddLog(true, "X24" + ex.ToString());
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
    }
}
