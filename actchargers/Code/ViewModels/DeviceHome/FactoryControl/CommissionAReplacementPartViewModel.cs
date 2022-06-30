using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;
using static actchargers.ACUtility;

namespace actchargers
{
    public class CommissionAReplacementPartViewModel : BaseViewModel
    {
        #region BattView listviewitems
        ListViewItem Batt_Commission_ReplacementDevice_comission_SN_TexBox;
        ListViewItem Batt_Commission_ReplacementDevice_comissionButton;
        #endregion

        #region Charger listviewitems
        ListViewItem MCB_Commission_ReplacementDevice_SN_TextBox;
        ListViewItem MCB_Commission_ReplacementDevice_comissionButton;
        #endregion

        private ObservableCollection<ListViewItem> _replacementPartItemSource;
        public ObservableCollection<ListViewItem> ReplacementPartItemSource
        {
            get { return _replacementPartItemSource; }
            set
            {
                _replacementPartItemSource = value;
                RaisePropertyChanged(() => ReplacementPartItemSource);
            }
        }

        public CommissionAReplacementPartViewModel()
        {
            ViewTitle = AppResources.commission_a_replacement_part;
            //device_LoadWarnings = new List<string>();
            ReplacementPartItemSource = new ObservableCollection<ListViewItem>();
            CreateList();
        }

        void CreateList()
        {
            if (IsBattView)
            {
                CreateListForBattView();
            }
            else
            {
                CreateListForChargers();
            }
        }

        #region BattView
        void CreateListForBattView()
        {
            Batt_Commission_ReplacementDevice_comission_SN_TexBox = new ListViewItem
            {
                Index = 0,
                Title = AppResources.board_serial_number,
                TextMaxLength = 12,
                IsEditable = true,
                DefaultCellType = CellTypes.LabelTextEdit,
                EditableCellType = CellTypes.LabelTextEdit
            };
            ReplacementPartItemSource.Add(Batt_Commission_ReplacementDevice_comission_SN_TexBox);

            Batt_Commission_ReplacementDevice_comissionButton = new ListViewItem
            {
                Index = 1,
                Title = AppResources.commission,
                DefaultCellType = CellTypes.Button,
                EditableCellType = CellTypes.Button,
                ListSelectionCommand = BattViewButtonSelectorCommand
            };
            ReplacementPartItemSource.Add(Batt_Commission_ReplacementDevice_comissionButton);

            if (BattViewQuantum.Instance.BattView_quickSerialNumberCheckStrict())
            {
                string model = "";
                //Batt_Commission_ReplacementDevice_comission_SN_Label.ForeColor = System.Drawing.Color.Black;

                if (BattViewQuantum.Instance.batt_verifyBAttViewSerialNumber(BattViewQuantum.Instance.GetBATTView().Config.battViewSN, ref model))
                {
                    if (model == "30")
                    {
                        //MessageBoxForm mb = new MessageBoxForm();
                        //mb.render("BATTView Mobile Cann't be comissioned as replacment Part", MessageBoxFormTypes.Warning, MessageBoxFormButtons.OK);
                        ShowViewModel<CommissionAReplacementPartViewModel>(new { pop = "pop" });
                        ACUserDialogs.ShowAlert(AppResources.battview_mobile_cannt_be_comissioned_as_replacment_part);
                        return;
                    }
                    Batt_Commission_ReplacementDevice_comission_SN_TexBox.Text = BattViewQuantum.Instance.GetBATTView().Config.battViewSN;

                }
                else
                {
                    Batt_Commission_ReplacementDevice_comission_SN_TexBox.Text = "";

                }
            }
        }

        private void Batt_Commission_ReplacementDevice_comissionButton_ClickInternal(object sender, EventArgs e)
        {
            //MessageBoxForm mb;
            if (!BattViewQuantum.Instance.BattView_quickSerialNumberCheckStrict())
            {
                //mb = new MessageBoxForm();
                //mb.render("Lost BATTView Connection", MessageBoxFormTypes.Warning, MessageBoxFormButtons.OK);
                ACUserDialogs.ShowAlert(AppResources.lost_battview_connection);
                return;
            }
            string model = "30";
            if (BattViewQuantum.Instance.batt_verifyBAttViewSerialNumber(Batt_Commission_ReplacementDevice_comission_SN_TexBox.Text, ref model) && model != "30")
            {
                //Batt_Commission_ReplacementDevice_comission_SN_Label.ForeColor = System.Drawing.Color.Black;

            }
            else
            {
                //Batt_Commission_ReplacementDevice_comission_SN_Label.ForeColor = System.Drawing.Color.Red;
                //mb = new MessageBoxForm();
                //mb.render("Invalid serial number", MessageBoxFormTypes.error, MessageBoxFormButtons.OK);
                ACUserDialogs.ShowAlert(AppResources.invalid_serial_number);
                return;
            }

            BattViewObject activeBattView = BattViewQuantum.Instance.GetBATTView();
            activeBattView.Config.battViewSN = Batt_Commission_ReplacementDevice_comission_SN_TexBox.Text;
            activeBattView.Config.ActViewEnabled = false;
            activeBattView.Config.replacmentPart = true;

            BattViewConfig Batt_Commission_config = new BattViewConfig();
            Batt_Commission_config = StaticDataAndHelperFunctions.DeepClone<BattViewConfig>(activeBattView.Config);
            Batt_Commission_config.zoneid = activeBattView.myZone;
            Batt_Commission_config.commissionRequest = true;
            Batt_Commission_config.firmwareversion = activeBattView.FirmwareRevision;

            List<object> arguments = new List<object>();
            arguments.Add(ActviewCommGeneric.commisionBattView);
            arguments.Add(Batt_Commission_config.ToJson());
            arguments.Add(false);

            ACTViewAction_Prepare(arguments);
        }

        async void Batt_simpleCommunicationAction_Prepare(List<object> arg)
        {
            BattViewCommunicationTypes caller = (BattViewCommunicationTypes)arg[0];
            try
            {
                if (caller != BattViewCommunicationTypes.NOCall)
                {
                    List<object> results = new List<object>();
                    try
                    {
                        results = await BattViewQuantum.Instance.CommunicateBATTView(arg);
                        if (results.Count > 0)
                        {
                            var status = (CommunicationResult)results[2];
                            if (caller == BattViewCommunicationTypes.doFinalComission)
                            {
                                if (status == CommunicationResult.OK)
                                {
                                    //navigate to connect to device screen
                                    //battView_MenusShowHide(null, null);
                                    //SerialNumbersList.SelectedIndex = 0;
                                    //showBusy = false;
                                    //scanRelated_prepare(scanRelatedTypes.doScan);
                                    Mvx.Resolve<IMvxMessenger>().Publish(new DeviceResetMessage(this, BattViewQuantum.Instance.GetBATTView().IPAddress));
                                    ShowViewModel<ConnectToDeviceViewModel>(new { popto = "popto" });
                                    ACUserDialogs.ShowAlert("Commission Done");
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.AddLog(true, "X8" + ex);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.AddLog(true, "X8" + ex);
                return;
            }
        }
        #endregion

        #region Charger
        void CreateListForChargers()
        {
            MCB_Commission_ReplacementDevice_SN_TextBox = new ListViewItem
            {
                Index = 0,
                Title = AppResources.board_serial_number,
                TextMaxLength = 11,
                IsEditable = true,
                DefaultCellType = CellTypes.LabelTextEdit,
                EditableCellType = CellTypes.LabelTextEdit
            };
            ReplacementPartItemSource.Add(MCB_Commission_ReplacementDevice_SN_TextBox);

            MCB_Commission_ReplacementDevice_comissionButton = new ListViewItem
            {
                Index = 1,
                Title = AppResources.commission,
                DefaultCellType = CellTypes.Button,
                EditableCellType = CellTypes.Button,
                ListSelectionCommand = BattViewButtonSelectorCommand
            };
            ReplacementPartItemSource.Add(MCB_Commission_ReplacementDevice_comissionButton);
          
            MCB_loadCommissionAReplacementPartData();

        }

        void MCB_loadCommissionAReplacementPartData()
        {
            //Loading data from charger to views
            MCBobject activeMCB = MCBQuantum.Instance.GetMCB();

            if (MCBQuantum.Instance.MCB_quickSerialNumberCheckStrict())
            {
                if (MCB_ReplcementPartSNCheck(activeMCB.Config.serialNumber))
                {
                    MCB_Commission_ReplacementDevice_SN_TextBox.Text = activeMCB.Config.serialNumber;
                }
                else
                {
                    MCB_Commission_ReplacementDevice_SN_TextBox.Text = "";
                }
            }
        }

        private void MCB_Commission_ReplacementDevice_comissionButton_ClickInternal(object sender, EventArgs e)
        {
            //MessageBoxForm mb;
            if (!MCBQuantum.Instance.MCB_quickSerialNumberCheckStrict())
            {
                //mb = new MessageBoxForm();
                //mb.render("Lost MSB Connection", MessageBoxFormTypes.Warning, MessageBoxFormButtons.OK);
                ACUserDialogs.ShowProgress(AppResources.lost_mcb_connection);
                return;
            }
            if (MCB_ReplcementPartSNCheck(MCB_Commission_ReplacementDevice_SN_TextBox.Text))
            {
                //MCB_Commission_ReplacementDevice_SN_Label.ForeColor = System.Drawing.Color.Black;

            }
            else
            {
                //MCB_Commission_ReplacementDevice_SN_Label.ForeColor = System.Drawing.Color.Red;
                //mb = new MessageBoxForm();
                //mb.render("Invalid serial number", MessageBoxFormTypes.error, MessageBoxFormButtons.OK);
                ACUserDialogs.ShowAlert(AppResources.invalid_serial_number);
                return;
            }

            MCBobject activeMCB = MCBQuantum.Instance.GetMCB();
            activeMCB.Config.serialNumber = MCB_Commission_ReplacementDevice_SN_TextBox.Text;
            activeMCB.Config.originalSerialNumber = "C" + MCB_Commission_ReplacementDevice_SN_TextBox.Text;
            activeMCB.Config.actViewEnable = false;
            activeMCB.Config.replacmentPart = true;
            MCBQuantum.Instance.MCB_loadDefaultWIFI();
            //MCB_SaveIntoWiFiSettings();

            MCBConfig MCB_Commission_config = new MCBConfig();
            MCB_Commission_config = StaticDataAndHelperFunctions.DeepClone<MCBConfig>(activeMCB.Config);
            MCB_Commission_config.firmwareVersion = activeMCB.FirmwareRevision;
            MCB_Commission_config.zoneID = activeMCB.myZone;

            List<object> arguments = new List<object>();
            arguments.Add(ActviewCommGeneric.commisionMCB);
            arguments.Add(MCB_Commission_config.ToJson());
            arguments.Add(false);
            ACTViewAction_Prepare(arguments);
        }

        private bool MCB_ReplcementPartSNCheck(string sn)
        {
            if (sn.Length > 15 || sn.Length < 8)
                return false;
            else
                return true;

            //if (sn.Length == 11)
            //{
            //    UInt32 temp = 0;
            //    if (UInt32.TryParse(sn.Substring(0, 6), out temp))
            //    {
            //        if (UInt32.TryParse(sn.Substring(7, 4), out temp))
            //        {
            //            if (sn[6] == '-')
            //                return true;
            //        }
            //    }
            //}
            //return false;
        }

        async void MCB_simpleCommunicationAction_Prepare(List<object> arg)
        {
            McbCommunicationTypes caller = (McbCommunicationTypes)arg[0];
            try
            {
                if (caller != McbCommunicationTypes.NOCall)
                {
                    List<object> results = new List<object>();
                    try
                    {
                        results = await MCBQuantum.Instance.CommunicateMCB(arg);
                        if (results.Count > 0)
                        {
                            var status = (CommunicationResult)results[2];
                            if (caller != McbCommunicationTypes.doFinalComission)
                            {
                                if (status == CommunicationResult.OK)
                                {
                                    //battView_MenusShowHide(null, null);
                                    //SerialNumbersList.SelectedIndex = 0;
                                    //showBusy = false;
                                    //scanRelated_prepare(scanRelatedTypes.doScan);
                                    Mvx.Resolve<IMvxMessenger>().Publish(new DeviceResetMessage(this, MCBQuantum.Instance.GetMCB().IPAddress));
                                    ShowViewModel<ConnectToDeviceViewModel>(new { popto = "popto" });
                                    ACUserDialogs.ShowAlert("Commission Done");
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.AddLog(true, "X8" + ex);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.AddLog(true, "X8" + ex);
                return;
            }
        }
        #endregion

        #region Common 

        public IMvxCommand BattViewButtonSelectorCommand
        {
            get { return new MvxCommand<ListViewItem>(ExecuteBattViewButtonSelectorCommand); }
        }


        void ExecuteBattViewButtonSelectorCommand(ListViewItem item)
        {
            if (IsBattView)
                Batt_Commission_ReplacementDevice_comissionButton_ClickInternal(null, null);
            else
                MCB_Commission_ReplacementDevice_comissionButton_ClickInternal(null, null);
        }

        async void ACTViewAction_Prepare(List<object> arguments)
        {
            ACUserDialogs.ShowProgress();
            List<object> results = new List<object>();
            try
            {
                arguments.Insert(0, true);
                results = await ACTVIEWQuantum.Instance.CommunicateACTView(arguments);
                if (results.Count > 0)
                {
                    bool internalFailure = (bool)results[0];
                    string internalFailureString = (string)results[1];
                    ACTViewResponse status = (ACTViewResponse)results[2];
                    ActviewCommGeneric caller = (ActviewCommGeneric)results[3];
                    bool removeSetBusy = (bool)results[4];
                    List<object> ProcArgumentList = (List<object>)results[5];

                    if (caller == ActviewCommGeneric.commisionBattView)
                    {
                        if (status.returnValue["duplicate"] != null && status.returnValue["duplicate"].Type == Newtonsoft.Json.Linq.JTokenType.Boolean && (bool)status.returnValue["duplicate"])
                        {
                            ACUserDialogs.ShowAlertWithTwoButtons("Serial Number Exists, Do you wish to override the existing one? (If you are not sure,it is possible Monkeys Will jump in ACTView) ", "", AppResources.yes, AppResources.no, () => OnYesClick(ProcArgumentList), null);
                        }
                        else
                        {
                            if (status.returnValue["newID"] != null && status.returnValue["newID"].Type == Newtonsoft.Json.Linq.JTokenType.Integer && (UInt32)status.returnValue["newID"] != 0)
                            {
                                List<object> arg = new List<object>();
                                List<object> vars = new List<object>();
                                arg.Add(BattViewCommunicationTypes.doFinalComission);
                                vars.Add((UInt32)status.returnValue["newID"]);
                                arg.Add(vars);
                                removeSetBusy = false;
                                Batt_simpleCommunicationAction_Prepare(arg);
                            }
                            else
                            {
                                ACUserDialogs.ShowAlert("Commission Failed, Please try again");
                            }
                        }
                    }
                    else if (caller == ActviewCommGeneric.commisionMCB)
                    {
                        if (status.returnValue["duplicate"] != null && status.returnValue["duplicate"].Type == Newtonsoft.Json.Linq.JTokenType.Boolean && (bool)status.returnValue["duplicate"])
                        {
                            ACUserDialogs.ShowAlertWithTwoButtons("Serial Number Exists, Do you wish to override the existing one? (If you are not sure,it is possible Monkeys Will jump in ACTView) ", "", AppResources.yes, AppResources.no, () => OnYesClick(ProcArgumentList), null);
                        }
                        else
                        {
                            if (status.returnValue["newID"] != null && status.returnValue["newID"].Type == Newtonsoft.Json.Linq.JTokenType.Integer && (UInt32)status.returnValue["newID"] != 0
                                && status.returnValue["boardID"] != null && status.returnValue["boardID"].Type == Newtonsoft.Json.Linq.JTokenType.Integer && (UInt32)status.returnValue["boardID"] != 0)
                            {
                                List<object> arg = new List<object>();
                                List<object> vars = new List<object>();
                                arg.Add(McbCommunicationTypes.doFinalComission);
                                vars.Add((UInt32)status.returnValue["newID"]);
                                vars.Add((UInt32)status.returnValue["boardID"]);
                                arg.Add(vars);
                                removeSetBusy = false;
                                MCB_simpleCommunicationAction_Prepare(arg);
                            }
                            else
                            {
                                //MessageBoxForm mb3 = new MessageBoxForm();
                                //mb3.render("Commission Failed,Please try again", MessageBoxFormTypes.error, MessageBoxFormButtons.OK);
                                ACUserDialogs.ShowAlert("Commission Failed,Please try again");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.AddLog(true, "X86" + ex);
                //return;
            }
            ACUserDialogs.HideProgress();
        }

        void OnYesClick(List<object> ProcArgumentList)
        {
            List<object> newArguments = new List<object>();
            if (IsBattView)
                newArguments.Add(ActviewCommGeneric.commisionBattView);
            else
                newArguments.Add(ActviewCommGeneric.commisionMCB);

            newArguments.Add((string)ProcArgumentList[0]);
            newArguments.Add(true);
            //removeSetBusy = false;
            ACTViewAction_Prepare(newArguments);
        }
        #endregion
    }
}
