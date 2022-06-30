using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;
using static actchargers.ACUtility;

namespace actchargers
{
    public class FactoryControlViewModel : BaseViewModel
    {
        #region MCB list items
        ListViewItem MCB_changeActViewID_ReComission;
        ListViewItem MCB_EnableDisableSimulationModeButton;
        ListViewItem MCB_PMSimulationModeButton;
        ListViewItem MCB_changeActViewIDGroupBox;//MCB_actViewIDtextBox//MCB_changeActViewID
        ListViewItem MCB_AdminLoadPLC;
        ListViewItem MCB_AdminCheckMCBHealth;
        ListViewItem MCB_AdminActionrunBurinInFroupBox;//MCB_AdminActionrunBurinInButtonTextBox//MCB_AdminActionrunBurinInButton
        ListViewItem MCB_changeBoardIDGroupBox;//MCB_BoardIDtextBox//MCB_changeBoardID
        ListViewItem MCB_AdminCommissionButton;
        ListViewItem MCB_AdminCommissionReplacementButton;
        ListViewItem MCB_mcbSN_groupBox;//MCB_mcbSN_saveButton//MCB_mcbSN_TextBox
        ListViewItem MCB_replacmentPart_panel;//MCB_replacmentPart_YesRadio//MCB_replacmentPart_NoRadio//MCB_replacementPart_SaveButton
        ListViewItem MCB_factoryReset;
        ListViewItem MCB_WitchToCalibrator_Button;
        ListViewItem MCB_AdminResetLCDcalibration;
        ListViewItem MCB_OEM_DirectUpload;
        //ListViewItem MCB_AdminActionsCancelButton;
        #endregion

        #region BattView list items
        ListViewItem Batt_ViewEventsControlButton;
        ListViewItem Batt_AdminLoadPlcFirmware;
        ListViewItem Batt_AdminActionsCommissionButton;
        ListViewItem Batt_AdminActionsReplacemntCommissionButton;
        //Testing not for mobile 
        //ListViewItem Batt_AdminActionsTestButton;
        //ListViewItem Batt_AdminActionsTestMobileButton;
        ListViewItem Batt_changeActviewPanel;//Batt_changeActviewID//Batt_actViewID
        ListViewItem Batt_replacementPart_groupBox;//Batt_replacementPart_YesRadio//Batt_replacementPart_NoRadio//Batt_replacementPart_SaveButton
        ListViewItem batt_factoryReset;
        ListViewItem Batt_changeActviewID_ReComission;
        ListViewItem BATT_OEM_DirectUpload;
        //ListViewItem Batt_AdminActionsCancelButton;
        #endregion

        List<string> device_LoadWarnings;

        /// <summary>
        /// The info item source.
        /// </summary>
        private ObservableCollection<ListViewItem> _factoryControlItemSource;
        public ObservableCollection<ListViewItem> FactoryControlItemSource
        {
            get { return _factoryControlItemSource; }
            set
            {
                _factoryControlItemSource = value;
                RaisePropertyChanged(() => FactoryControlItemSource);
            }
        }

        public FactoryControlViewModel()
        {
            ViewTitle = AppResources.factory_control;
            device_LoadWarnings = new List<string>();
            FactoryControlItemSource = new ObservableCollection<ListViewItem>();
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

        #region BattView list

        void CreateListForBattView()
        {
            FactoryControlItemSource.Clear();
            Batt_ViewEventsControlButton = new ListViewItem
            {
                Index = 0,
                Title = AppResources.edit_event_control,
                DefaultCellType = CellTypes.Button,
                EditableCellType = CellTypes.Button,
                ListSelectionCommand = BattViewButtonSelectorCommand
            };

            Batt_AdminLoadPlcFirmware = new ListViewItem
            {
                Index = 1,
                Title = AppResources.load_plc_firmware,
                DefaultCellType = CellTypes.Button,
                EditableCellType = CellTypes.Button,
                ListSelectionCommand = BattViewButtonSelectorCommand
            };

            Batt_AdminActionsCommissionButton = new ListViewItem
            {
                Index = 2,
                Title = AppResources.commission_a_battview,
                DefaultCellType = CellTypes.Button,
                EditableCellType = CellTypes.Button,
                ListSelectionCommand = BattViewButtonSelectorCommand
            };

            Batt_AdminActionsReplacemntCommissionButton = new ListViewItem
            {
                Index = 3,
                Title = AppResources.commission_a_replacement_part,
                DefaultCellType = CellTypes.Button,
                EditableCellType = CellTypes.Button,
                ListSelectionCommand = BattViewButtonSelectorCommand
            };

            /* Batt_AdminActionsTestButton = new ListViewItem
             {
                 Index = 4,
                 Title = AppResources.start_testing_battview_standard_pro,
                 DefaultCellType = CellTypes.Button,
                 EditableCellType = CellTypes.Button,
                 ListSelectionCommand = BattViewButtonSelectorCommand
             };

             Batt_AdminActionsTestMobileButton = new ListViewItem
             {
                 Index = 5,
                 Title = AppResources.start_testing_battview_mobile,
                 DefaultCellType = CellTypes.Button,
                 EditableCellType = CellTypes.Button,
                 ListSelectionCommand = BattViewButtonSelectorCommand
             };*/

            Batt_changeActviewPanel = new ListViewItem
            {
                Index = 6,
                Title = AppResources.change_actView_id_and_restart,
                DefaultCellType = CellTypes.ButtonTextEdit,
                EditableCellType = CellTypes.ButtonTextEdit,
                ListSelectionCommand = BattViewButtonSelectorCommand,
                TextInputType = ACUtility.InputType.Number
            };

            Batt_replacementPart_groupBox = new ListViewItem
            {
                Index = 7,
                Title = AppResources.replacement_part,
                Title2 = AppResources.Save_and_Restart,
                DefaultCellType = CellTypes.LabelSwitchButton,
                EditableCellType = CellTypes.LabelSwitchButton,
                ListSelectionCommand = BattViewButtonSelectorCommand
            };

            batt_factoryReset = new ListViewItem
            {
                Index = 8,
                Title = AppResources.factory_reset,
                DefaultCellType = CellTypes.Button,
                EditableCellType = CellTypes.Button,
                ListSelectionCommand = BattViewButtonSelectorCommand
            };

            Batt_changeActviewID_ReComission = new ListViewItem
            {
                Index = 9,
                Title = AppResources.allow_to_recomission_and_restart,
                DefaultCellType = CellTypes.Button,
                EditableCellType = CellTypes.Button,
                ListSelectionCommand = BattViewButtonSelectorCommand
            };

            BATT_OEM_DirectUpload = new ListViewItem
            {
                Index = 10,
                Title = AppResources.upload_after_commission,
                DefaultCellType = CellTypes.Button,
                EditableCellType = CellTypes.Button,
                ListSelectionCommand = BattViewButtonSelectorCommand
            };
            try
            {
                Batt_loadAdminTools();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Logger.AddLog(true, "X25" + ex.ToString());
            }

            if (battViewAdminToolsApplyAccess() == 0)
            {
                FactoryControlItemSource.Clear();
                ACUserDialogs.ShowAlertWithOkButtonsAction(AppResources.no_data_found, () => { ShowViewModel<FactoryControlViewModel>(new { pop = "pop" }); });
                return;
            }

            if (FactoryControlItemSource.Count > 0)
            {
                FactoryControlItemSource = new ObservableCollection<ListViewItem>(FactoryControlItemSource.OrderBy(o => o.Index));
            }
        }

        private int battViewAdminToolsApplyAccess()
        {
            UIAccessControlUtility accessControlUtility = new UIAccessControlUtility();

            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_AdminACTViewID, Batt_changeActviewPanel, FactoryControlItemSource);

            if (ControlObject.isDebugMaster)
                accessControlUtility.DoApplyAccessControl(AccessLevelConsts.write, Batt_replacementPart_groupBox, FactoryControlItemSource);

            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_eventsControl, Batt_ViewEventsControlButton, FactoryControlItemSource);

            //accessControlUtility.doApplyAccessControl(access_level.write, Batt_AdminLoadPlcFirmware, FactoryControlItemSource);
            if (!FactoryControlItemSource.Contains(Batt_AdminLoadPlcFirmware))
            {
                FactoryControlItemSource.Add(Batt_AdminLoadPlcFirmware);
            }

            /* if (ControlObject.isHWMnafacturer || ControlObject.isACTOem || ControlObject.isDebugMaster || ControlObject.user_access.Batt_onlyForEnginneringTeam != access_level.noAccess)
             {
                 accessControlUtility.doApplyAccessControl(access_level.write, Batt_AdminActionsTestButton, FactoryControlItemSource);
                 accessControlUtility.doApplyAccessControl(access_level.write, Batt_AdminActionsTestMobileButton, FactoryControlItemSource);
             }
             else
             {
                 accessControlUtility.doApplyAccessControl(access_level.noAccess, Batt_AdminActionsTestButton, FactoryControlItemSource);
                 accessControlUtility.doApplyAccessControl(access_level.noAccess, Batt_AdminActionsTestMobileButton, FactoryControlItemSource);
             }*/
            if (ControlObject.isACTOem || ControlObject.isDebugMaster)
            {
                accessControlUtility.DoApplyAccessControl(AccessLevelConsts.write, batt_factoryReset, FactoryControlItemSource);
            }
            else
            {
                accessControlUtility.DoApplyAccessControl(AccessLevelConsts.noAccess, batt_factoryReset, FactoryControlItemSource);
            }

            /* if (accessControlUtility.getVisibleCount() == 0)
             {
                 Batt_AdminActionsButton.Hide();
             }
             if (ControlObject.user_access.Batt_onlyForEnginneringTeam == access_level.noAccess)
             {
                 Batt_DebugToolButton.Hide();
             }*/

            if (ControlObject.UserAccess.Batt_COMMISSION != AccessLevelConsts.noAccess)
            {
                if (BattViewQuantum.Instance.GetBATTView().Config.id > 10000)
                {
                    //Batt_AdminActionsCommissionButton.Hide();
                    //Batt_AdminActionsReplacemntCommissionButton.Hide();
                    //Batt_changeActviewID_ReComission.Show();
                    accessControlUtility.DoApplyAccessControl(AccessLevelConsts.write, Batt_changeActviewID_ReComission, FactoryControlItemSource);
                }
                else
                {
                    //Batt_AdminActionsCommissionButton.Show();
                    //Batt_AdminActionsReplacemntCommissionButton.Show();
                    //Batt_changeActviewID_ReComission.Hide();
                    //accessControlUtility.doApplyAccessControl(access_level.write, Batt_AdminActionsCommissionButton, FactoryControlItemSource);
                    accessControlUtility.DoApplyAccessControl(AccessLevelConsts.write, Batt_AdminActionsReplacemntCommissionButton, FactoryControlItemSource);
                }
            }

            if (ControlObject.isACTOem || ControlObject.isDebugMaster)
            {
                //BATT_OEM_DirectUpload.Show();
                accessControlUtility.DoApplyAccessControl(AccessLevelConsts.write, BATT_OEM_DirectUpload, FactoryControlItemSource);
            }

            return accessControlUtility.GetVisibleCount();
        }

        void Batt_loadAdminTools()
        {
            if (!BattViewQuantum.Instance.BattView_quickSerialNumberCheckStrict())
                return;

            BattViewObject activeBattView = BattViewQuantum.Instance.GetBATTView();
            Batt_changeActviewPanel.Text = activeBattView.Config.id.ToString();
            if (!Batt_verifyActViewID())
            {
                device_LoadWarnings.Add("BattView Not Commissioned");
            }

            Batt_replacementPart_groupBox.IsSwitchEnabled = activeBattView.Config.replacmentPart;
            if (activeBattView.Config.id >= 10000 && !activeBattView.Config.replacmentPart)
            {
                BATT_OEM_DirectUpload.IsEditEnabled = true;
            }
            else
            {
                BATT_OEM_DirectUpload.IsEditEnabled = false;
            }
        }
        #endregion

        #region BattView Button actions
        public IMvxCommand BattViewButtonSelectorCommand
        {
            get { return new MvxCommand<ListViewItem>(ExecuteBattViewButtonSelectorCommand); }
        }


        private void ExecuteBattViewButtonSelectorCommand(ListViewItem item)
        {
            if (item.Title == Batt_AdminLoadPlcFirmware.Title || item.Title == Batt_changeActviewPanel.Title || item.Title == Batt_replacementPart_groupBox.Title || item.Title == Batt_changeActviewID_ReComission.Title)
            {
                BATT_simpleCommunicationButtonAction(item, null);
            }
            else if (item.Title == batt_factoryReset.Title)
            {
                ACUserDialogs.ShowAlertWithTwoButtons(AppResources.factory_reset_alert_message_for_battview, "", AppResources.ok, AppResources.cancel, () => BATT_simpleCommunicationButtonAction(item, null), null);
            }
            else if (item.Title == Batt_ViewEventsControlButton.Title)
            {
                ShowViewModel<EditEventControlViewModel>();
            }
            else if (item.Title == Batt_AdminActionsReplacemntCommissionButton.Title)
            {
                ShowViewModel<CommissionAReplacementPartViewModel>();
            }
            else if (item.Title == BATT_OEM_DirectUpload.Title)
            {
                OEM_Direct_Upload(null, null);
            }
            else if (item.Title == Batt_AdminActionsCommissionButton.Title)
            {
                ShowViewModel<CommissionViewModel>();
            }
            /*else if (item.Title == Batt_AdminActionsTestButton.Title)
            {
                ShowViewModel<TestingBattViewViewModel>(new { batt_testMobile = false});
            }
            else if (item.Title == Batt_AdminActionsTestMobileButton.Title) 
            {
                ShowViewModel<TestingBattViewViewModel>(new {batt_testMobile = true});
            }*/
            else
            {
                ACUserDialogs.ShowAlert(AppResources.dev_in_progress);
            }
        }

        private void BATT_simpleCommunicationButtonAction(ListViewItem sender, EventArgs e)
        {
            BATT_simpleCommunicationButtonActionInternal(sender, e);
        }

        private async void BATT_simpleCommunicationButtonActionInternal(ListViewItem sender, EventArgs e)
        {
            BattViewCommunicationTypes caller = BattViewCommunicationTypes.NOCall;
            object arg1 = null;
            string msg = string.Empty;
            if (!BattViewQuantum.Instance.BattView_quickSerialNumberCheckStrict())
                return;

            IsBusy = true;
            try
            {
                if (sender.Title == Batt_AdminLoadPlcFirmware.Title)
                {
                    byte[] serials = null;
                    Firmware firmwareManager = new Firmware();
                    if (firmwareManager.GetPLCBinaries(ref serials) != FirmwareResult.fileOK)
                    {
                        msg = AppResources.bad_firmware_encoding;
                    }
                    else
                    {
                        caller = BattViewCommunicationTypes.loadPLCFirmware;
                        arg1 = serials;
                    }
                }
                else if (sender.Title == Batt_changeActviewPanel.Title)
                {
                    if (Batt_verifyActViewID())
                    {
                        Batt_saveIntoActViewID();
                        caller = BattViewCommunicationTypes.saveActViewIDandRestart;
                    }
                }
                else if (sender.Title == Batt_replacementPart_groupBox.Title)
                {
                    BattViewQuantum.Instance.GetBATTView().Config.replacmentPart = Batt_replacementPart_groupBox.IsSwitchEnabled;
                    caller = BattViewCommunicationTypes.saveConfigAndRestart;
                }
                else if (sender.Title == batt_factoryReset.Title)
                {
                    BATT_ResetFactoryData();
                    caller = BattViewCommunicationTypes.resetFactorySettings;
                }
                else if (sender.Title == Batt_changeActviewID_ReComission.Title)// || ReplacementDevice_decomissionButton.Name == b.Name)
                {
                    Batt_saveIntoActViewID(false);
                    caller = BattViewCommunicationTypes.saveActViewIDandRestart;
                }

                if (caller != BattViewCommunicationTypes.NOCall)
                {
                    List<object> arguments = new List<object>();
                    arguments.Add(caller);
                    arguments.Add(arg1);
                    List<object> results = new List<object>();
                    try
                    {
                        results = await BattViewQuantum.Instance.CommunicateBATTView(arguments);
                        if (results.Count > 0)
                        {
                            var callerStatus = results[3];
                            var status = (CommunicationResult)results[2];
                            if (callerStatus.Equals(BattViewCommunicationTypes.restartDevice))
                            {
                                if (status == CommunicationResult.COMMAND_DELAYED)
                                {
                                    //TODO: refresh Battview
                                    msg = "BATTView is busy.";
                                    //battView_LoadAll(false);//load again , to apply changes for capacity and others
                                }
                                else if (status == CommunicationResult.OK)
                                {
                                    if (caller != BattViewCommunicationTypes.restartDeviceNoDisconnect)
                                    {
                                        //showBusy = false;
                                        //scanRelated_prepare(scanRelatedTypes.doScan);
                                        Mvx.Resolve<IMvxMessenger>().Publish(new DeviceResetMessage(this, BattViewQuantum.Instance.GetBATTView().IPAddress));
                                        ShowViewModel<ConnectToDeviceViewModel>(new { popto = "popto" });
                                        msg = "BATTView Restarting...";
                                    }
                                }
                            }
                            else if (callerStatus.Equals(BattViewCommunicationTypes.loadPLCFirmware))
                            {
                                if (status == CommunicationResult.OK)
                                {
                                    msg = "Battview is reflushing PLC Firmware";
                                }
                                else
                                {
                                    //isWarning = true;
                                    msg = AppResources.cant_update_firmware;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.AddLog(true, "X86" + ex);
                        return;
                    }
                }
                else
                {
                    ACUserDialogs.ShowAlertWithTitleAndOkButton(AppResources.invalid_input);
                }
            }
            catch (Exception ex)
            {
                Logger.AddLog(true, "X8" + ex);
                return;
            }

            IsBusy = false;
            if (msg != string.Empty)
            {
                ACUserDialogs.ShowAlertWithTitleAndOkButton(msg);
            }
        }

        bool Batt_verifyActViewID()
        {
            if (!BattViewQuantum.Instance.BattView_quickSerialNumberCheckStrict())
                return false;
            VerifyControl v = new VerifyControl();

            v.VerifyUInteger(Batt_changeActviewPanel, Batt_changeActviewPanel, 10000, UInt32.MaxValue);
            return !v.HasErrors();
        }
        void Batt_saveIntoActViewID(bool fromText = true)
        {
            if (!BattViewQuantum.Instance.BattView_quickSerialNumberCheckStrict())
                return;
            if (fromText)
                BattViewQuantum.Instance.GetBATTView().Config.id = UInt32.Parse(Batt_changeActviewPanel.Text);
            else
            {
                Random r = new Random(DateTime.UtcNow.Second);
                BattViewQuantum.Instance.GetBATTView().Config.id = (UInt32)r.Next(1, 1022);
            }
        }

        void BATT_ResetFactoryData()
        {
            if (!BattViewQuantum.Instance.BattView_quickSerialNumberCheckStrict())
                return;

            Random r = new Random();
            BattViewObject activeBattView = BattViewQuantum.Instance.GetBATTView();
            activeBattView.Config.batteryID = "My Battery";
            activeBattView.Config.batterymodel = "";
            activeBattView.Config.batterysn = "";
            activeBattView.Config.TruckId = "";
            activeBattView.Config.installationDate = DateTime.Now;
            activeBattView.Config.ahrcapacity = 850;
            activeBattView.Config.warrantedAHR = (UInt32)1250 * 850;
            activeBattView.myZone = (byte)15;
            activeBattView.Config.studyId = 0;
            activeBattView.Config.chargerType = (byte)0;

            BattViewQuantum.Instance.Batt_saveDefaultChargeProfile();
            BattViewQuantum.Instance.Batt_loadDefaultWifiSettings();
            //Batt_SaveIntoWiFiSettings();

            activeBattView.Config.ActViewEnabled = false;
            activeBattView.Config.nominalvoltage = 24;
            activeBattView.Config.id = (UInt32)r.Next(1, 9999);
            activeBattView.Config.replacmentPart = false;
            activeBattView.Config.TRTemperature = 100;
        }

        #endregion

        #region MCB Factory list
        void CreateListForChargers()
        {
            FactoryControlItemSource.Clear();
            MCB_changeActViewID_ReComission = new ListViewItem
            {
                Index = 0,
                Title = AppResources.allow_to_recomission_and_restart,
                DefaultCellType = CellTypes.Button,
                EditableCellType = CellTypes.Button,
                ListSelectionCommand = MCBButtonSelectorCommand
            };

            MCB_EnableDisableSimulationModeButton = new ListViewItem
            {
                Index = 1,
                Title = AppResources.enable_simulation_mode_and_restart,
                DefaultCellType = CellTypes.Button,
                EditableCellType = CellTypes.Button,
                ListSelectionCommand = MCBButtonSelectorCommand
            };

            MCB_PMSimulationModeButton = new ListViewItem
            {
                Index = 2,
                Title = AppResources.enable_pm_simulation_mode_and_restart,
                DefaultCellType = CellTypes.Button,
                EditableCellType = CellTypes.Button,
                ListSelectionCommand = MCBButtonSelectorCommand
            };

            //MCB_actViewIDtextBox//MCB_changeActViewID
            MCB_changeActViewIDGroupBox = new ListViewItem
            {
                Index = 3,
                Title = AppResources.change_actView_id_and_restart,
                DefaultCellType = CellTypes.ButtonTextEdit,
                EditableCellType = CellTypes.ButtonTextEdit,
                ListSelectionCommand = MCBButtonSelectorCommand,
                TextInputType = ACUtility.InputType.Number
            };

            MCB_AdminLoadPLC = new ListViewItem
            {
                Index = 4,
                Title = AppResources.load_plc_firmware_and_restart,
                DefaultCellType = CellTypes.Button,
                EditableCellType = CellTypes.Button,
                ListSelectionCommand = MCBButtonSelectorCommand
            };

            MCB_AdminCheckMCBHealth = new ListViewItem
            {
                Index = 5,
                Title = AppResources.check_mcb_health,
                DefaultCellType = CellTypes.Button,
                EditableCellType = CellTypes.Button,
                ListSelectionCommand = MCBButtonSelectorCommand
            };

            //MCB_AdminActionrunBurinInButtonTextBox//MCB_AdminActionrunBurinInButton
            MCB_AdminActionrunBurinInFroupBox = new ListViewItem
            {
                Index = 6,
                Title = AppResources.run_stop_burn_in_in_minutes,
                DefaultCellType = CellTypes.ButtonTextEdit,
                EditableCellType = CellTypes.ButtonTextEdit,
                ListSelectionCommand = MCBButtonSelectorCommand,
                TextInputType = ACUtility.InputType.Decimal
            };

            //MCB_BoardIDtextBox//MCB_changeBoardID
            MCB_changeBoardIDGroupBox = new ListViewItem
            {
                Index = 7,
                Title = AppResources.change_board_id,
                DefaultCellType = CellTypes.ButtonTextEdit,
                EditableCellType = CellTypes.ButtonTextEdit,
                ListSelectionCommand = MCBButtonSelectorCommand,
                TextInputType = ACUtility.InputType.Decimal
            };

            MCB_AdminCommissionButton = new ListViewItem
            {
                Index = 8,
                Title = AppResources.commission_a_charger,
                DefaultCellType = CellTypes.Button,
                EditableCellType = CellTypes.Button,
                ListSelectionCommand = MCBButtonSelectorCommand
            };

            MCB_AdminCommissionReplacementButton = new ListViewItem
            {
                Index = 9,
                Title = AppResources.commission_a_replacement_part,
                DefaultCellType = CellTypes.Button,
                EditableCellType = CellTypes.Button,
                ListSelectionCommand = MCBButtonSelectorCommand
            };

            MCB_mcbSN_groupBox = new ListViewItem
            {
                Index = 10,
                Title = AppResources.Save_mcb_serial_number,
                DefaultCellType = CellTypes.Button,
                EditableCellType = CellTypes.Button,
                ListSelectionCommand = MCBButtonSelectorCommand
            };

            //MCB_replacmentPart_YesRadio//MCB_replacmentPart_NoRadio//MCB_replacementPart_SaveButton
            MCB_replacmentPart_panel = new ListViewItem
            {
                Index = 11,
                Title = AppResources.replacement_part,
                Title2 = AppResources.Save_and_Restart,
                DefaultCellType = CellTypes.LabelSwitchButton,
                EditableCellType = CellTypes.LabelSwitchButton,
                ListSelectionCommand = MCBButtonSelectorCommand
            };

            MCB_factoryReset = new ListViewItem
            {
                Index = 12,
                Title = AppResources.factory_reset,
                DefaultCellType = CellTypes.Button,
                EditableCellType = CellTypes.Button,
                ListSelectionCommand = MCBButtonSelectorCommand
            };

            MCB_WitchToCalibrator_Button = new ListViewItem()
            {
                Index = 13,
                Title = AppResources.switch_to_calibrator_mcb,
                DefaultCellType = CellTypes.Button,
                EditableCellType = CellTypes.Button,
                ListSelectionCommand = MCBButtonSelectorCommand
            };

            MCB_AdminResetLCDcalibration = new ListViewItem()
            {
                Index = 14,
                Title = AppResources.reset_lcd_calibration,
                DefaultCellType = CellTypes.Button,
                EditableCellType = CellTypes.Button,
                ListSelectionCommand = MCBButtonSelectorCommand
            };

            //TODO: related to Upload and DB
            MCB_OEM_DirectUpload = new ListViewItem()
            {
                Index = 15,
                Title = AppResources.upload_after_commission_upload,
                DefaultCellType = CellTypes.Button,
                EditableCellType = CellTypes.Button,
                ListSelectionCommand = MCBButtonSelectorCommand
            };

            try
            {
                MCB_LoadAdminTools();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Logger.AddLog(true, "X25" + ex.ToString());
            }

            if (chargerAdminToolsApplyAccess() == 0)
            {
                FactoryControlItemSource.Clear();
                ACUserDialogs.ShowAlertWithOkButtonsAction(AppResources.no_data_found, () => { ShowViewModel<FactoryControlViewModel>(new { pop = "pop" }); });
                return;
            }

            if (FactoryControlItemSource.Count > 0)
            {
                FactoryControlItemSource = new ObservableCollection<ListViewItem>(FactoryControlItemSource.OrderBy(o => o.Index));
            }
        }

        void MCB_LoadAdminTools()
        {
            if (!MCBQuantum.Instance.MCB_quickSerialNumberCheckStrict())
                return;

            MCBobject activeMCB = MCBQuantum.Instance.GetMCB();
            if (activeMCB.Config.enableChargerSimulationMode)
            {
                MCB_EnableDisableSimulationModeButton.Title = AppResources.disable_simulation_mode_and_restart;
            }
            else
            {
                MCB_EnableDisableSimulationModeButton.Title = AppResources.enable_simulation_mode_and_restart;
            }
            if (activeMCB.Config.enablePMsimulation)
            {
                MCB_PMSimulationModeButton.Title = AppResources.disable_pm_simulation_mode_dev_and_restart;
            }
            else
            {
                MCB_PMSimulationModeButton.Title = AppResources.enable_pm_simulation_mode_dev_and_restart;
            }

            MCB_changeActViewIDGroupBox.Text = activeMCB.Config.id;
            if (!MCB_verifyActViewID())
            {
                device_LoadWarnings.Add("Charger Not Commissioned, charging is disabled");
            }

            MCB_changeBoardIDGroupBox.Text = activeMCB.Config.afterCommissionBoardID.ToString();
            MCB_mcbSN_groupBox.Text = activeMCB.Config.originalSerialNumber;

            //MCB_replacmentPart_NoRadio.Checked = !activeMCB.MCBConfig.replacmentPart;
            //MCB_replacmentPart_YesRadio.Checked = activeMCB.MCBConfig.replacmentPart;

            if (UInt32.Parse(activeMCB.Config.id) >= 10000 && !activeMCB.Config.replacmentPart)
            {
                MCB_OEM_DirectUpload.IsEditEnabled = true;
            }
            else
            {
                MCB_OEM_DirectUpload.IsEditEnabled = false;
            }

            MCB_AdminActionrunBurinInFroupBox.Text = "60";
        }

        bool MCB_verifyActViewID()
        {
            if (!MCBQuantum.Instance.MCB_quickSerialNumberCheckStrict())
                return false;
            VerifyControl v = new VerifyControl();
            v.VerifyUInteger(MCB_changeActViewIDGroupBox, MCB_changeActViewIDGroupBox, (uint)(ControlObject.isDebugMaster ? 1 : 1024), 4294967294);
            return !v.HasErrors();
        }

        private int chargerAdminToolsApplyAccess()
        {
            //MCB_AdminActionrunBurinInFroupBox.Hide();
            UIAccessControlUtility accessControlUtility = new UIAccessControlUtility();
            if (ControlObject.isHWMnafacturer || ControlObject.isACTOem || ControlObject.isDebugMaster || ControlObject.UserAccess.MCB_onlyForEnginneringTeam != AccessLevelConsts.noAccess)
            {
                //MCB_AdminActionrunBurinInFroupBox.Show();
                accessControlUtility.DoApplyAccessControl(AccessLevelConsts.write, MCB_AdminActionrunBurinInFroupBox, FactoryControlItemSource);
            }

            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_AdminSimulation, MCB_EnableDisableSimulationModeButton, FactoryControlItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_AdminACTViewID, MCB_changeActViewIDGroupBox, FactoryControlItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_AdminACTViewID, MCB_changeBoardIDGroupBox, FactoryControlItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_AdminPMSimulation, MCB_PMSimulationModeButton, FactoryControlItemSource);

            //accessControlUtility.doApplyAccessControl(access_level.write, MCB_AdminCheckMCBHealth, FactoryControlItemSource);
            //accessControlUtility.doApplyAccessControl(access_level.write, MCB_AdminLoadPLC, FactoryControlItemSource);

            if (!FactoryControlItemSource.Contains(MCB_AdminLoadPLC))
            {
                FactoryControlItemSource.Add(MCB_AdminLoadPLC);
            }
            if (!FactoryControlItemSource.Contains(MCB_AdminCheckMCBHealth))
            {
                FactoryControlItemSource.Add(MCB_AdminCheckMCBHealth);
            }


            if (ControlObject.UserAccess.MCB_COMMISSION != AccessLevelConsts.noAccess)
            {
                if (UInt32.Parse(MCBQuantum.Instance.GetMCB().Config.id) > 10000)
                {
                    //MCB_AdminCommissionButton.Hide();
                    //MCB_AdminCommissionReplacementButton.Hide();
                    //MCB_changeActViewID_ReComission.Show();
                    accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_COMMISSION, MCB_changeActViewID_ReComission, FactoryControlItemSource);
                }
                else
                {

                    //MCB_AdminCommissionButton.Show();
                    accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_COMMISSION, MCB_AdminCommissionButton, FactoryControlItemSource);
                    accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_COMMISSION, MCB_AdminCommissionReplacementButton, FactoryControlItemSource);
                    //MCB_AdminCommissionReplacementButton.Show();
                    //MCB_changeActViewID_ReComission.Hide();
                }
            }

            //if (ControlObject.isACTOem || ControlObject.isHWMnafacturer)
            //{
            //    Device_SynchButton.Hide();
            //    Devices_uploadStatusButton.Hide();
            //    synchSitesButton.Hide();

            //}
            if (ControlObject.isACTOem || ControlObject.isDebugMaster)
            {
                accessControlUtility.DoApplyAccessControl(AccessLevelConsts.write, MCB_OEM_DirectUpload, FactoryControlItemSource);
            }

            if (ControlObject.isHWMnafacturer || ControlObject.isDebugMaster || ControlObject.UserAccess.MCB_onlyForEnginneringTeam != AccessLevelConsts.noAccess)
            {
                accessControlUtility.DoApplyAccessControl(AccessLevelConsts.write, MCB_AdminResetLCDcalibration, FactoryControlItemSource);
            }



            if (!ControlObject.isHWMnafacturer && !ControlObject.isDebugMaster)
            {
                //MCB_mcbSN_groupBox.Hide();
            }
            else
            {
                accessControlUtility.DoApplyAccessControl(AccessLevelConsts.write, MCB_mcbSN_groupBox, FactoryControlItemSource);
            }

            if (ControlObject.isACTOem || ControlObject.isDebugMaster || ControlObject.UserAccess.MCB_onlyForEnginneringTeam != AccessLevelConsts.noAccess)
            {
                accessControlUtility.DoApplyAccessControl(AccessLevelConsts.write, MCB_factoryReset, FactoryControlItemSource);
            }
            else
            {
                accessControlUtility.DoApplyAccessControl(AccessLevelConsts.noAccess, MCB_factoryReset, FactoryControlItemSource);

            }
            if (!ControlObject.isDebugMaster)
            {
                //MCB_replacmentPart_panel.Hide();
            }
            else
            {
                accessControlUtility.DoApplyAccessControl(AccessLevelConsts.write, MCB_replacmentPart_panel, FactoryControlItemSource);
            }

            if (ControlObject.UserAccess.MCB_onlyForEnginneringTeam == AccessLevelConsts.noAccess && !ControlObject.isACTOem && !ControlObject.isDebugMaster)
            {
                //MCB_WitchToCalibrator_Button.Hide();
            }
            else
            {
                accessControlUtility.DoApplyAccessControl(AccessLevelConsts.write, MCB_WitchToCalibrator_Button, FactoryControlItemSource);
            }

            int extra = 0;
            if (ControlObject.isHWMnafacturer || ControlObject.isACTOem || ControlObject.isDebugMaster || ControlObject.UserAccess.MCB_onlyForEnginneringTeam != AccessLevelConsts.noAccess)
            {
                extra = 1;
            }

            /* if (accessControlUtility.getVisibleCount() == 0 && extra == 0)
             {
                 MCB_AdminActionsButton.Hide();
             }

             if (ControlObject.user_access.MCB_onlyForEnginneringTeam == access_level.noAccess)
             {
                 MCB_DebugToolButton.Hide();
                 Calibrator_TabControl.TabPages.Remove(Calibrator_DebugTabPage);

             }*/

            return accessControlUtility.GetVisibleCount() + extra;
        }

        #endregion

        #region MCB Button actions

        public IMvxCommand MCBButtonSelectorCommand
        {
            get { return new MvxCommand<ListViewItem>(ExecuteMCBButtonSelectorCommand); }
        }


        private void ExecuteMCBButtonSelectorCommand(ListViewItem item)
        {
            if (item.Title == MCB_AdminCommissionReplacementButton.Title)
            {
                ShowViewModel<CommissionAReplacementPartViewModel>();
            }
            else if (item.Title == MCB_OEM_DirectUpload.Title)
            {
                OEM_Direct_Upload(null, null);
            }
            else if (item.Title == MCB_factoryReset.Title)
            {
                ACUserDialogs.ShowAlertWithTwoButtons(AppResources.factory_reset_alert_message_for_mcb, "", AppResources.ok, AppResources.cancel, () => MCB_simpleCommunicationButtonAction(item, null), null);
            }
            else if (item.Title == MCB_AdminCommissionButton.Title)
            {
                ShowViewModel<CommissionViewModel>();
            }
            else
            {
                MCB_simpleCommunicationButtonAction(item, null);
            }
        }

        private void MCB_simpleCommunicationButtonAction(ListViewItem sender, EventArgs e)
        {
            MCB_simpleCommunicationButtonActionInternal(sender, e);
        }

        private async void MCB_simpleCommunicationButtonActionInternal(ListViewItem sender, EventArgs e)
        {
            IsBusy = true;
            McbCommunicationTypes caller = McbCommunicationTypes.NOCall;
            object arg1 = null;
            //TODO: Create Alert
            string msg = string.Empty;
            if (!MCBQuantum.Instance.MCB_quickSerialNumberCheckStrict())
            {
                IsBusy = false;
                return;
            }
            try
            {
                //Enable Simulation mode and restart
                if (sender.Title == MCB_changeActViewID_ReComission.Title)
                {
                    MCB_SaveIntoAdminTools(4);
                    caller = McbCommunicationTypes.saveActViewIDandRestart;
                }
                else if (sender.Title == MCB_EnableDisableSimulationModeButton.Title)
                {
                    MCB_SaveIntoAdminTools(0);
                    caller = McbCommunicationTypes.saveConfigAndRestart;
                }
                else if (sender.Title == MCB_PMSimulationModeButton.Title)
                {
                    MCB_SaveIntoAdminTools(1);
                    caller = McbCommunicationTypes.saveConfigAndRestart;
                }
                else if (sender.Title == MCB_changeActViewIDGroupBox.Title)
                {
                    if (MCB_verifyActViewID())
                    {
                        MCB_SaveIntoAdminTools(2);
                        caller = McbCommunicationTypes.saveActViewIDandRestart;
                    }
                }
                else if (sender.Title == MCB_AdminLoadPLC.Title)
                {
                    byte[] serials = null;
                    Firmware firmwareManager = new Firmware();
                    if (firmwareManager.GetPLCBinaries(ref serials) != FirmwareResult.fileOK)
                    {
                        msg = AppResources.bad_firmware_encoding;
                    }
                    else
                    {
                        caller = McbCommunicationTypes.loadPLC;
                        arg1 = serials;
                    }
                }
                else if (sender.Title == MCB_AdminCheckMCBHealth.Title)                 {
                    caller = McbCommunicationTypes.healthCheck;                 }
                else if (sender.Title == MCB_AdminActionrunBurinInFroupBox.Title)
                {
                    UInt32 length = 0;
                    if (UInt32.TryParse(MCB_AdminActionrunBurinInFroupBox.Text, out length) || length > 60 * 24)
                    {
                        arg1 = new object[] { (byte)0x02, length * 60 };
                        caller = McbCommunicationTypes.switchMode;
                    }
                }
                else if (sender.Title == MCB_changeBoardIDGroupBox.Title)
                {
                    if (MCB_verifyBoardID())
                    {
                        MCB_SaveIntoAdminTools(3);
                        caller = McbCommunicationTypes.saveConfig;
                    }
                }
                else if (sender.Title == MCB_mcbSN_groupBox.Title)
                {
                    if (MCBQuantum.Instance.MCB_quickSerialNumberCheckStrict())
                    {
                        MCBQuantum.Instance.GetMCB().Config.originalSerialNumber = MCB_mcbSN_groupBox.Text;
                        caller = McbCommunicationTypes.saveConfig;
                    }
                }
                else if (sender.Title == MCB_replacmentPart_panel.Title)
                {
                    MCBQuantum.Instance.GetMCB().Config.replacmentPart = MCB_replacmentPart_panel.IsSwitchEnabled;
                    caller = McbCommunicationTypes.saveConfigAndRestart;
                }
                else if (sender.Title == MCB_factoryReset.Title)
                {
                    MCB_ResetFactoryData();
                    caller = McbCommunicationTypes.factoryReset;
                }
                else if (sender.Title == MCB_WitchToCalibrator_Button.Title)
                {
                    float currentFirmwareVersion = MCBQuantum.Instance.GetMCB().FirmwareRevision;

                    byte[] serials = null;
                    Firmware firmwareManager = new Firmware();
                    FirmwareResult res = firmwareManager.UpdateFileBinary(DeviceBaseType.CALIBRATOR, ref serials);
                    if (res != FirmwareResult.fileOK)
                    {
                        switch (res)
                        {
                            case FirmwareResult.badFileEncode:
                                msg = "Bad file encoding"; break;
                            case FirmwareResult.badFileFormat:
                                msg = "Bad file format"; break;
                            case FirmwareResult.fileNotFound:
                                msg = "File not found"; break;
                            case FirmwareResult.noAcessFile:
                                msg = "Can't read file"; break;
                        }
                    }
                    else
                    {
                        caller = McbCommunicationTypes.firmwareWrite;
                        arg1 = serials;
                    }
                }
                else if (sender.Title == MCB_AdminResetLCDcalibration.Title)
                {
                    caller = McbCommunicationTypes.ResetLCDCalibration;
                }

                if (caller != McbCommunicationTypes.NOCall)
                {
                    List<object> arguments = new List<object>();
                    arguments.Add(caller);
                    arguments.Add(arg1);
                    List<object> results = new List<object>();
                    try
                    {
                        results = await MCBQuantum.Instance.CommunicateMCB(arguments);
                        IsBusy = false;
                        if (results.Count > 0)
                        {
                            var callerStatus = results[3];
                            var status = (CommunicationResult)results[2];

                            if (McbCommunicationTypes.restartDevice.Equals(callerStatus)
                                || McbCommunicationTypes.ResetLCDCalibration.Equals(callerStatus))
                            {
                                if (status == CommunicationResult.CHARGER_BUSY)
                                {
                                    msg = AppResources.charger_is_busy;
                                    //MCB_LoadAll(false);//load again , to apply changes for capacity and others
                                }
                                else if (status == CommunicationResult.OK || MCBQuantum.Instance.GetMCB().FirmwareRevision < 2.05f)
                                {
                                    //TODO: Move to Connect to device screen
                                    //MCB_LoadAll(false);//load again , to apply changes for capacity and others
                                    Mvx.Resolve<IMvxMessenger>().Publish(new DeviceResetMessage(this, MCBQuantum.Instance.GetMCB().IPAddress));
                                    ShowViewModel<ConnectToDeviceViewModel>(new { popto = "popto" });
                                    msg = AppResources.charger_restarting;
                                    //showBusy = false;
                                    //scanRelated_prepare(scanRelatedTypes.doScan);
                                }
                            }
                            else if (McbCommunicationTypes.healthCheck.Equals(callerStatus))
                            {
                                if (status == CommunicationResult.OK)
                                    MCB_LoadHealthCheck();
                            }
                            else if (McbCommunicationTypes.saveConfig.Equals(callerStatus))
                            {
                                //if (status == commProtocol.Communication_Result.OK)
                                //{
                                //MCB_LoadAdminTools();
                                //MCB_LoadHealthCheck();
                                //}
                                SetStatus(status);
                            }
                            else if (McbCommunicationTypes.firmwareUpdateRequest.Equals(callerStatus))
                            {
                                //TODO: Firmware Update for Switch to Calibrator MCB
                                //firmwareUpdateDict firmwareUpdateCtrl = new firmwareUpdateDict();
                                if (status == CommunicationResult.COMMAND_DELAYED)
                                {
                                    msg = "Charger is busy, the firmware update will take place automatically once  the charge cycle is done.";
                                    //firmwareUpdateCtrl.updateStage(uint.Parse(connManager.activeMCB.MCBConfig.id), true, firmwareUpdateStage.sentRequestDelayed);
                                }
                                else if (status == CommunicationResult.OK || MCBQuantum.Instance.GetMCB().FirmwareRevision < 2.05f)
                                {
                                    msg = "charger is reflushing Firmware..it may take few minutes to update the firmware";
                                    //firmwareUpdateCtrl.updateFirmware(uint.Parse(connManager.activeMCB.MCBConfig.id), true, Quantum_Firmware.Firmware.getLatestMCBFirmware());
                                    //firmwareUpdateCtrl.updateStage(uint.Parse(connManager.activeMCB.MCBConfig.id), true, firmwareUpdateStage.updateIsNotNeeded);
                                    //showBusy = false;
                                    //scanRelated_prepare(scanRelatedTypes.doScan);
                                }
                                else
                                {
                                    //firmwareUpdateCtrl.updateStage(uint.Parse(connManager.activeMCB.MCBConfig.id), true, firmwareUpdateStage.sendingRequest);
                                }
                            }
                            else if (McbCommunicationTypes.loadPLC.Equals(callerStatus))
                            {
                                if (status == CommunicationResult.OK)
                                {
                                    msg = AppResources.charger_is_reflushing_plc_firmware;
                                }
                                else
                                {
                                    msg = AppResources.cant_update_firmware;
                                }
                            }
                            else if (McbCommunicationTypes.firmwareWrite.Equals(callerStatus))
                            {
                                msg = AppResources.cant_update_firmware;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.AddLog(true, "X86" + ex);
                    }
                }
                else
                {
                    ACUserDialogs.ShowAlertWithTitleAndOkButton(AppResources.invalid_input);
                }
            }
            catch (Exception ex)
            {
                Logger.AddLog(true, "X86" + ex.ToString());
            }


            if (msg != string.Empty)
            {
                ACUserDialogs.ShowAlertWithTitleAndOkButton(msg);
            }
        }

        bool MCB_verifyBoardID()
        {
            if (!MCBQuantum.Instance.MCB_quickSerialNumberCheckStrict())
                return false;
            VerifyControl v = new VerifyControl();
            v.VerifyUInteger(MCB_changeBoardIDGroupBox, MCB_changeBoardIDGroupBox, (uint)(ControlObject.isDebugMaster ? 1 : 1024), 4294967294);
            return !v.HasErrors();
        }

        void MCB_SaveIntoAdminTools(int type)
        {
            MCBobject activeMCB = MCBQuantum.Instance.GetMCB();
            if (type == 0)
            {
                activeMCB.Config.enableChargerSimulationMode = !activeMCB.Config.enableChargerSimulationMode;
            }
            else if (type == 1)
            {
                activeMCB.Config.enablePMsimulation = !activeMCB.Config.enablePMsimulation;
            }
            else if (type == 2)
            {
                activeMCB.Config.id = MCB_changeActViewIDGroupBox.Text;
            }
            else if (type == 3)
            {
                activeMCB.Config.afterCommissionBoardID = UInt32.Parse(MCB_changeBoardIDGroupBox.Text);
            }
            else if (type == 4)
            {
                Random r = new Random(DateTime.UtcNow.Second);
                activeMCB.Config.id = r.Next(1, 1022).ToString();
            }
        }

        bool MCB_LoadHealthCheck()
        {
            if (!MCBQuantum.Instance.MCB_quickSerialNumberCheckStrict())
                return false;
            try
            {
                bool allIsOK = true;
                byte internalFailure = MCBQuantum.Instance.GetMCB().health_check.internalFailureCode;
                string print = "";
                if (internalFailure != 0)
                {
                    allIsOK = false;
                    if (internalFailure == 20)
                    {
                        print = "EEPROM Chip Failed (U1)..cant do testing";
                    }
                    else if (internalFailure / 10 == 1)
                    {
                        if (internalFailure == 12)
                            print = "Real time Clock Chip Failed (U3) PIN7 seems not connected";
                        else
                            print = "Real Time Clock Chip Failed (U3)..cant do testing";
                    }
                    else if (internalFailure / 10 == 8)
                    {
                        print = "FLASH Memory Failed (U9)..cant do testing";
                    }
                    else
                    {
                        print = "Internal Failure code" + internalFailure.ToString();

                    }
                    //MCB_HealthCheck_richTextBox.ForeColor = Color.Red;
                    //MCB_HealthCheck_richTextBox.Text = print;
                    ACUserDialogs.ShowAlert(print);
                    return false;
                }
                if (MCBQuantum.Instance.GetMCB().health_check.PM_failure != 0)
                {
                    print = "";
                    switch (MCBQuantum.Instance.GetMCB().health_check.PM_failure)
                    {
                        case 1: print = "CAN BUS FAILED,NO HW detected"; break;
                        case 2: print = "CAN BUS FAILED,NO Power modules found."; break;
                        case 3: print = "Power modules error, messed up voltage"; break;
                    }
                    //MCB_HealthCheck_richTextBox.ForeColor = Color.Red;
                    //MCB_HealthCheck_richTextBox.Text += print + System.Environment.NewLine;
                    print += System.Environment.NewLine;
                    allIsOK = false;
                    //PM_failure
                }
                if (MCBQuantum.Instance.GetMCB().health_check.WIFI_Failure != 0)
                {
                    print += "WIFI MODULE FAILED" + Environment.NewLine;

                    //MCB_HealthCheck_richTextBox.ForeColor = Color.Red;
                    //MCB_HealthCheck_richTextBox.Text += print + System.Environment.NewLine;
                    allIsOK = false;
                }

                if (MCBQuantum.Instance.GetMCB().health_check.PLC_Failure != 0)
                {
                    print = "";
                    switch (MCBQuantum.Instance.GetMCB().health_check.PLC_Failure)
                    {
                        case 1: print = "CANNOT Do a successfull Basic communication with PLC Chip (U18)"; break;
                        case 2: print = "CANNOT Load and communicate with the PLC Chip (U18)"; break;
                        case 3: print = "PLC Firmware Never downloaded (U18)"; break;
                    }
                    //MCB_HealthCheck_richTextBox.ForeColor = Color.Red;
                    //MCB_HealthCheck_richTextBox.Text += print + System.Environment.NewLine;
                    print += System.Environment.NewLine;
                    allIsOK = false;
                }
                bool msgWait = false;
                bool shouldWait = (DateTime.UtcNow - MCBQuantum.Instance.GetMCB().health_check.connectTime).TotalSeconds < 60;
                if (MCBQuantum.Instance.GetMCB().health_check.WIFI_communicated != 0xFF)
                {
                    if (!shouldWait)
                    {
                        print += "Idle WIFI!!" + Environment.NewLine;
                        //MCB_HealthCheck_richTextBox.ForeColor = Color.Red;
                        //MCB_HealthCheck_richTextBox.Text += print + System.Environment.NewLine;
                    }
                    else
                    {
                        msgWait = true;
                    }
                    allIsOK = false;
                }
                if (MCBQuantum.Instance.GetMCB().health_check.PLC_communicated == 0)
                {
                    if (!shouldWait)
                    {
                        print += "Idle PLC!!" + Environment.NewLine;
                        //MCB_HealthCheck_richTextBox.ForeColor = Color.Red;
                        //MCB_HealthCheck_richTextBox.Text += print + System.Environment.NewLine;
                    }
                    else
                    {
                        msgWait = true;
                    }

                    allIsOK = false;
                }
                if (MCBQuantum.Instance.GetMCB().health_check.V_OK == 0)
                {

                    print += "No voltage Detected!!" + Environment.NewLine;
                    //MCB_HealthCheck_richTextBox.Text += print + System.Environment.NewLine;
                    //MCB_HealthCheck_richTextBox.ForeColor = Color.Red;
                    allIsOK = false;
                }
                //if(connManager.activeMCB.PMs[0].)

                if (allIsOK)
                {
                    print = "All is Good";
                    //MCB_HealthCheck_richTextBox.ForeColor = Color.Green;
                    //MCB_HealthCheck_richTextBox.Text = print;
                }
                else if (msgWait)
                {
                    //MCB_HealthCheck_richTextBox.ForeColor = Color.Black;
                    print = "Please Wait, still checking....";
                    //MCB_HealthCheck_richTextBox.Text = print;
                }
                ACUserDialogs.ShowAlert(print);
            }
            catch (Exception ex)
            {
                Logger.AddLog(true, "X94" + ex);
                return false;
            }
            return true;
        }

        void MCB_ResetFactoryData()
        {
            if (!MCBQuantum.Instance.MCB_quickSerialNumberCheckStrict())
                return;

            Random r = new Random();

            MCBobject activeMCB = MCBQuantum.Instance.GetMCB();
            activeMCB.myZone = 15;
            //Load all config....
            activeMCB.Config.id = r.Next(1, 1024).ToString();

            activeMCB.Config.serialNumber = "RANDOM" + r.Next(1, 99999).ToString();

            activeMCB.Config.replacmentPart = false;
            activeMCB.Config.afterCommissionBoardID = 0;

            activeMCB.Config.numberOfInstalledPMs = "1";
            activeMCB.Config.chargerUserName = "My ACT Charger";
            activeMCB.Config.TRtemperature = "10.0";

            MCBQuantum.Instance.MCB_loadDefaultWIFI();//MCB_SaveIntoWiFiSettings();
            activeMCB.Config.InstallationDate = DateTime.Now;

            activeMCB.Config.chargerType = 0;
            activeMCB.Config.batteryType = MCBConfig.batteryTypes[0];

            MCBQuantum.Instance.MCB_saveDefaultChargeProfile();

            activeMCB.Config.IR = 3;

            activeMCB.Config.enableChargerSimulationMode = false; // 0x0002
            activeMCB.Config.temperatureSensorInstalled = false;//0x0004
            activeMCB.Config.enableRefreshCycleAfterFI = false;//0x0008
            activeMCB.Config.enableRefreshCycleAfterEQ = false;//0x0010
            activeMCB.Config.enablePMsimulation = false;//0x0020
            activeMCB.Config.chargerType = 0;
            activeMCB.Config.batteryCapacity = "850";

            activeMCB.Config.autoStartEnable = true;
            activeMCB.Config.autoStartMask.Saturday = true;//byte
            activeMCB.Config.autoStartMask.Sunday = true;//byte
            activeMCB.Config.autoStartMask.Monday = true;//byte
            activeMCB.Config.autoStartMask.Tuesday = true;//byte
            activeMCB.Config.autoStartMask.Thursday = true;//byte
            activeMCB.Config.autoStartMask.Friday = true;//byte
            activeMCB.Config.autoStartMask.Wednesday = true;//byte
            activeMCB.Config.temperatureFormat = false;

            activeMCB.Config.PMvoltage = "48";
            //connManager.activeMCB.MCBConfig.CVTimer;
            //connManager.activeMCB.MCBConfig.finishTimer;
            //connManager.activeMCB.MCBConfig.finishdTTimer;
            //connManager.activeMCB.MCBConfig.EqualizeTimer;
            activeMCB.Config.refreshTimer = "08:00";
            activeMCB.Config.desulfationTimer = "12:00";

            activeMCB.Config.enablePLC = false;
            activeMCB.Config.ledcontrol = 0;

            activeMCB.Config.enableManualEQ = false;
            activeMCB.Config.enableManualDesulfate = false;
            activeMCB.Config.energyDaysMask.Saturday = false;
            activeMCB.Config.energyDaysMask.Sunday = false;
            activeMCB.Config.energyDaysMask.Monday = false;
            activeMCB.Config.energyDaysMask.Tuesday = false;
            activeMCB.Config.energyDaysMask.Wednesday = false;
            activeMCB.Config.energyDaysMask.Thursday = false;
            activeMCB.Config.energyDaysMask.Friday = false;
            activeMCB.Config.lockoutStartTime = 0;
            activeMCB.Config.lockoutCloseTime = 6 * 3600;
            activeMCB.Config.energyStartTime = 0;
            activeMCB.Config.energyCloseTime = 6 * 3600;
            activeMCB.Config.enableManualDesulfate = false;
            activeMCB.Config.lockoutDaysMask.Saturday = false;
            activeMCB.Config.lockoutDaysMask.Sunday = false;
            activeMCB.Config.lockoutDaysMask.Monday = false;
            activeMCB.Config.lockoutDaysMask.Tuesday = false;
            activeMCB.Config.lockoutDaysMask.Wednesday = false;
            activeMCB.Config.lockoutDaysMask.Thursday = false;
            activeMCB.Config.lockoutDaysMask.Friday = false;
            activeMCB.Config.energyDecreaseValue = 90;
            activeMCB.Config.PMvoltageInputValue = 0;


            activeMCB.Config.disablePushButton = false;

            activeMCB.Config.model = "Q4" + "-";
            activeMCB.Config.model += activeMCB.Config.batteryVoltage + "-";
            int currentRating = int.Parse(activeMCB.Config.numberOfInstalledPMs) * (activeMCB.Config.PMvoltage == "36" ? 50 : 40);
            activeMCB.Config.model += currentRating.ToString() + "-";
            activeMCB.Config.model += (activeMCB.Config.PMvoltageInputValue != 0 ? "208" : "480");
            activeMCB.Config.model += "-B";

            activeMCB.Config.batteryCapacity24 = activeMCB.Config.batteryCapacity36 = activeMCB.Config.batteryCapacity48 = UInt16.Parse(activeMCB.Config.batteryCapacity);

            activeMCB.Config.chargerOverrideBattviewFIEQsched = false;
            activeMCB.Config.ignoreBATTViewSOC = false;
            activeMCB.Config.battviewAutoCalibrationEnable = false;
            activeMCB.Config.forceFinishTimeout = false;
        }
        #endregion

        #region Upload after commision 
        //OEM_Direct_Upload

        private void OEM_Direct_Upload(object sender, EventArgs e)
        {
            if (!IsBattView)
            {
                MCBobject activeMCB = MCBQuantum.Instance.GetMCB();
                if (MCBQuantum.Instance.MCB_quickSerialNumberCheckStrict() && UInt32.Parse(activeMCB.Config.id) >= 10000 && !activeMCB.Config.replacmentPart)
                {
                    List<object> arguments = new List<object>();
                    arguments.Add(ActviewCommGeneric.OEM_UploadMCB);
                    arguments.Add(UInt32.Parse(activeMCB.Config.id));
                    arguments.Add(activeMCB.Config.ToJson());
                    arguments.Add(activeMCB.globalRecord.TOJSON());
                    arguments.Add(activeMCB.FirmwareRevision);
                    arguments.Add(activeMCB.myZone);
                    ACTViewAction_Prepare(arguments);
                }
            }
            else
            {
                BattViewObject activeBattView = BattViewQuantum.Instance.GetBATTView();
                if (BattViewQuantum.Instance.BattView_quickSerialNumberCheckStrict() && activeBattView.Config.id >= 10000 && !activeBattView.Config.replacmentPart)
                {
                    //
                    //UInt32 id, UInt32 studyId, string configJson, string globalRecordObject, float firmwareVersion, byte zone
                    List<object> arguments = new List<object>();
                    arguments.Add(ActviewCommGeneric.OEM_UploadBattView);
                    arguments.Add(activeBattView.Config.id);
                    arguments.Add(activeBattView.Config.ToJson());
                    arguments.Add(activeBattView.globalRecord.ToJson());
                    arguments.Add(activeBattView.FirmwareRevision);
                    arguments.Add(activeBattView.myZone);
                    ACTViewAction_Prepare(arguments);
                }
            }
        }

        async void ACTViewAction_Prepare(List<object> arguments)
        {
            IsBusy = true;
            List<object> results = new List<object>();
            try
            {
                arguments.Insert(0, true);//userAction
                results = await ACTVIEWQuantum.Instance.CommunicateACTView(arguments);
                if (results.Count > 0)
                {
                    bool internalFailure = (bool)results[0];
                    string internalFailureString = (string)results[1];
                    ACTViewResponse status = (ACTViewResponse)results[2];
                    ActviewCommGeneric caller = (ActviewCommGeneric)results[3];
                    bool removeSetBusy = (bool)results[4];
                    List<object> ProcArgumentList = (List<object>)results[5];

                    IsBusy = false;
                    ACUserDialogs.ShowAlert(AppResources.uploaded);
                }
            }
            catch (Exception ex)
            {
                Logger.AddLog(true, "X86" + ex);
            }
            IsBusy = false;
        }
        #endregion

        public override void OnBackButtonClick()
        {
            base.OnBackButtonClick();

            ShowViewModel<FactoryControlViewModel>(new { pop = "pop" });
        }
    }
}