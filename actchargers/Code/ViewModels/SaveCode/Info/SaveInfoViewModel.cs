using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;
using Newtonsoft.Json;
using Plugin.Connectivity;
using Plugin.Connectivity.Abstractions;

namespace actchargers
{
    public class SaveInfoViewModel : BaseViewModel
    {
        MvxSubscriptionToken _mListSelector;
        //BATTVIewInfo ListItems
        ListViewItem Batt_battViewSNTextBoxTOSAVE;
        ListViewItem Batt_hardwareRevisionTextBoxTOSAVE;
        ListViewItem Batt_setupByteTextBoxNOSAVE;
        ListViewItem Batt_lastChangeUserTextBoxNOSAVE;
        ListViewItem Batt_FirmwareRevTextBox;
        ListViewItem Batt_memorySignature;
        ListViewItem Batt_actView_idValue;
        //MCBViewInfo listitems
        ListViewItem MCB_actView_idValue;
        ListViewItem MCB_OriginalSerialNumberTOSAVE;
        ListViewItem MCB_SerialNumberTOSAVE;
        ListViewItem MCB_chargerModelTextBoxTOSAVE;
        ListViewItem MCB_hardwareRevisionTextBoxTOSAVE;
        ListViewItem MCB_userNamedIDTOSAVE;
        ListViewItem MCB_installationDatePickerTOSAVE;
        ListViewItem MCB_chargerTypeTOSAVE;
        ListViewItem MCB_timeZoneComboBoxTOSAVE;
        ListViewItem MCB_SETUP_BYTE_TOSAVE;
        ListViewItem MCB_FIRMWARE_REV;
        ListViewItem MCB_memorySigniture;
        ListViewItem MCB_lastChangeUserID;
        ListViewItem MCB_lcdVersion_textBox;
        ListViewItem MCB_WIFIVersion_textBox;



        VerifyControl verifyControl;

        /// <summary>
        /// The edit mode.
        /// </summary>
        private bool _showEdit;
        public bool ShowEdit
        {
            get
            {
                return _showEdit;
            }
            set
            {
                _showEdit = value;
                RaisePropertyChanged(() => ShowEdit);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:actchargers.InfoViewModel"/> class.
        /// </summary>
        public SaveInfoViewModel()
        {
            if (IsBattView)
                createListForBATTView();
            else
                createListForCharger();
        }

        void createListForBATTView()
        {
            ViewTitle = AppResources.batt_view_info;
            EditingMode = false;
            ShowEdit = true;
            InfoItemSource = new ObservableCollection<ListViewItem>();
            //CreateData();

            #region BATTView Info Data
            Batt_actView_idValue = new ListViewItem()
            {
                Index = 0,
                Title = AppResources.act_view_id,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.LabelTextEdit
                //TextMaxLength = 12
            };

            Batt_battViewSNTextBoxTOSAVE = new ListViewItem()
            {
                Index = 1,
                Title = AppResources.serial_number,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.LabelTextEdit,
                TextMaxLength = 12
            };
            Batt_hardwareRevisionTextBoxTOSAVE = new ListViewItem()
            {
                Index = 2,
                Title = AppResources.hardware_version,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.LabelTextEdit,
                TextMaxLength = 2
            };
            Batt_setupByteTextBoxNOSAVE = new ListViewItem()
            {
                Index = 3,
                Title = AppResources.setup_byte,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.LabelLabel
            };
            Batt_lastChangeUserTextBoxNOSAVE = new ListViewItem()
            {
                Index = 4,
                Title = AppResources.last_change_userid,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.LabelLabel
            };
            Batt_FirmwareRevTextBox = new ListViewItem()
            {
                Index = 5,
                Title = AppResources.firmware_revision,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.LabelLabel
            };
            Batt_memorySignature = new ListViewItem()
            {
                Index = 6,
                Title = AppResources.memory_signature,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.LabelLabel
            };

            #endregion

            try
            {
                Batt_loadBattViewInfo();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Logger.AddLog(true, "X25" + ex.ToString());
            }

            if (BattViewInfoAccessApply() == 0)
            {
                InfoItemSource.Clear();
                ACUserDialogs.ShowAlertWithOkButtonsAction(AppResources.no_data_found, () => { ShowViewModel<InfoViewModel>(new { pop = "pop" }); });
                return;
            }

            if (InfoItemSource.Count > 0)
            {
                InfoItemSource = new ObservableCollection<ListViewItem>(InfoItemSource.OrderBy(o => o.Index));
            }
            RaisePropertyChanged(() => InfoItemSource);
        }

        void createListForCharger()
        {
            ViewTitle = AppResources.charger_info;
            EditingMode = false;
            ShowEdit = true;
            InfoItemSource = new ObservableCollection<ListViewItem>();
            MCB_actView_idValue = new ListViewItem()
            {
                Index = 0,
                Title = AppResources.act_view_id,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.LabelLabel
            };
            MCB_OriginalSerialNumberTOSAVE = new ListViewItem()
            {
                Index = 1,
                Title = AppResources.mcb_serial_number,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.LabelTextEdit,
                TextInputType = ACUtility.InputType.Number,
                TextMaxLength = 16,
            };
            MCB_SerialNumberTOSAVE = new ListViewItem()
            {
                Index = 2,
                Title = AppResources.charger_serial_number,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.LabelTextEdit,
                TextInputType = ACUtility.InputType.Number,
                TextMaxLength = 12

            };
            MCB_chargerModelTextBoxTOSAVE = new ListViewItem()
            {
                Index = 3,
                Title = AppResources.charger_model,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.LabelTextEdit,
                TextMaxLength = 15
            };
            MCB_hardwareRevisionTextBoxTOSAVE = new ListViewItem()
            {
                Index = 4,
                Title = AppResources.hardware_revision,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.LabelTextEdit,
                TextMaxLength = 2
            };
            MCB_userNamedIDTOSAVE = new ListViewItem()
            {
                Index = 5,
                Title = AppResources.client_charger_id,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.LabelTextEdit,
                TextMaxLength = 23
            };
            MCB_installationDatePickerTOSAVE = new ListViewItem()
            {
                Index = 6,
                Title = AppResources.installation_date,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.DatePicker
            };
            MCB_chargerTypeTOSAVE = new ListViewItem()
            {
                Index = 7,
                Title = AppResources.charger_type,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.ListSelector,
                ListSelectionCommand = ListSelectorCommand
            };
            MCB_timeZoneComboBoxTOSAVE = new ListViewItem()
            {
                Index = 8,
                Title = AppResources.time_zone,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.ListSelector,
                ListSelectorType = ACUtility.ListSelectorType.Timezone,
                ListSelectionCommand = ListSelectorCommand
            };
            MCB_lcdVersion_textBox = new ListViewItem()
            {
                Index = 9,
                Title = AppResources.lcd_images_version,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.LabelTextEdit,
                TextMaxLength = 3
            };
            MCB_WIFIVersion_textBox = new ListViewItem()
            {
                Index = 10,
                Title = AppResources.wifi_firmware_version,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.LabelTextEdit,
                TextMaxLength = 3
            };
            MCB_FIRMWARE_REV = new ListViewItem()
            {
                Index = 11,
                Title = AppResources.firmware_revision,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.LabelLabel
            };
            MCB_memorySigniture = new ListViewItem()
            {
                Index = 12,
                Title = AppResources.memory_signature,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.LabelLabel
            };
            MCB_SETUP_BYTE_TOSAVE = new ListViewItem()
            {
                Index = 13,
                Title = AppResources.setup_byte,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.LabelLabel
            };

            MCB_lastChangeUserID = new ListViewItem()
            {
                Index = 14,
                Title = AppResources.last_change_userid,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.LabelLabel
            };

            try
            {
                MCB_loadChargerInfo();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Logger.AddLog(true, "X25" + ex.ToString());
            }
            if (chargerInfoAccessApply() == 0)
            {
                InfoItemSource.Clear();
                ACUserDialogs.ShowAlertWithOkButtonsAction(AppResources.no_data_found, () => { ShowViewModel<InfoViewModel>(new { pop = "pop" }); });
                return;
            }

            if (InfoItemSource.Count > 0)
            {
                InfoItemSource = new ObservableCollection<ListViewItem>(InfoItemSource.OrderBy(o => o.Index));
            }
            RaisePropertyChanged(() => InfoItemSource);
        }

        private int BattViewInfoAccessApply()
        {
            UIAccessControlUtility accessControlUtility = new UIAccessControlUtility();
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_SN, Batt_battViewSNTextBoxTOSAVE, InfoItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_HWRevision, Batt_hardwareRevisionTextBoxTOSAVE, InfoItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_setup_version, Batt_setupByteTextBoxNOSAVE, InfoItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_setup_version, Batt_lastChangeUserTextBoxNOSAVE, InfoItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_readFrimWareVersion, Batt_FirmwareRevTextBox, InfoItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_memorySignature, Batt_memorySignature, InfoItemSource);


            if (accessControlUtility.GetSavedCount() == 0)
            {
                //Batt_SaveBattViewInfoButton.Hide();
                ShowEdit = false;
            }

            int count = accessControlUtility.GetVisibleCount();
            accessControlUtility.DoApplyAccessControl(AccessLevelConsts.write, Batt_actView_idValue, InfoItemSource);
            return count;
        }

        private int chargerInfoAccessApply()
        {

            UIAccessControlUtility accessControlUtility = new UIAccessControlUtility();
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_SN, MCB_SerialNumberTOSAVE, InfoItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_Model, MCB_chargerModelTextBoxTOSAVE, InfoItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_HWRevision, MCB_hardwareRevisionTextBoxTOSAVE, InfoItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_UserNamedID, MCB_userNamedIDTOSAVE, InfoItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_InstallationDate, MCB_installationDatePickerTOSAVE, InfoItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_chargerType, MCB_chargerTypeTOSAVE, InfoItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_TimeZone, MCB_timeZoneComboBoxTOSAVE, InfoItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_setup_version, MCB_SETUP_BYTE_TOSAVE, InfoItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_setup_version, MCB_lastChangeUserID, InfoItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_readFrimWareVersion, MCB_FIRMWARE_REV, InfoItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_memorySignature, MCB_memorySigniture, InfoItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_EditOriginalSerialNumber, MCB_OriginalSerialNumberTOSAVE, InfoItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_HWversionControl, MCB_lcdVersion_textBox, InfoItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_HWversionControl, MCB_WIFIVersion_textBox, InfoItemSource);

            //MCB_EditOriginalSerialNumber
            if (accessControlUtility.GetSavedCount() == 0)
            {
                //Batt_SaveBattViewInfoButton.Hide();
                ShowEdit = false;
            }
            //if (accessControlUtility.getVisibleCount() == 0)
            //{
            //    MCB_ChargerInfoButton.Hide();
            //}
            //if (accessControlUtility.getSavedCount() == 0)
            //{
            //    MCB_chargerInfoSaveButton.Hide();
            //}
            int visibleCount = accessControlUtility.GetVisibleCount();//ignoure MCB_actView_idValue 

            accessControlUtility.DoApplyAccessControl(AccessLevelConsts.write, MCB_actView_idValue, InfoItemSource);

            return visibleCount;
        }


        /// <summary>
        /// Gets the list selector command.
        /// </summary>
        /// <value>The list selector command.</value>
        public IMvxCommand ListSelectorCommand
        {
            get { return new MvxCommand<ListViewItem>(ExecuteListSelectorCommand); }
        }


        private void ExecuteListSelectorCommand(ListViewItem item)
        {
            if (item.CellType == ACUtility.CellTypes.ListSelector)
            {
                _mListSelector = Mvx.Resolve<IMvxMessenger>().Subscribe<ListSelectorMessage>(OnListSelectorMessage);
                ShowViewModel<ListSelectorViewModel>(new { title = item.Title, type = item.ListSelectorType, selectedItemIndex = InfoItemSource.IndexOf(item), ItemSourceStr = JsonConvert.SerializeObject(item.Items) });
            }
        }

        /// <summary>
        /// Ons the list selector message.
        /// </summary>
        /// <param name="obj">Object.</param>
        private void OnListSelectorMessage(ListSelectorMessage obj)
        {
            if (_mListSelector != null)
            {
                Mvx.Resolve<IMvxMessenger>().Unsubscribe<ListSelectorMessage>(_mListSelector);
                _mListSelector = null;
                InfoItemSource[obj.SelectedItemindex].Text = obj.SelectedItem;
                InfoItemSource[obj.SelectedItemindex].SelectedIndex = obj.SelectedIndex;
                RaisePropertyChanged(() => InfoItemSource);

            }
        }

        /// <summary>
        /// The info item source.
        /// </summary>
        private ObservableCollection<ListViewItem> _infoItemSource;
        public ObservableCollection<ListViewItem> InfoItemSource
        {
            get { return _infoItemSource; }
            set
            {
                _infoItemSource = value;
                RaisePropertyChanged(() => InfoItemSource);
            }
        }

        /// <summary>
        /// The edit mode.
        /// </summary>
        private bool _editMode;
        public bool EditingMode
        {
            get
            {
                return _editMode = _editMode ? _editMode : false;
            }
            set
            {
                _editMode = value;
                RaisePropertyChanged(() => EditingMode);
            }
        }

        /// <summary>
        /// Gets the edit button click command.
        /// </summary>
        /// <value>The edit button click command.</value>
        public IMvxCommand EditBtnClickCommand
        {
            get { return new MvxCommand(OnEditClick); }
        }


        /// <summary>
        /// Ons the edit click.
        /// </summary>
        void OnEditClick()
        {
            EditingMode = true;
            CreateData();
            RaisePropertyChanged(() => InfoItemSource);
        }


        /// <summary>
        /// Gets the save button click command.
        /// </summary>
        /// <value>The save button click command.</value>
        public IMvxCommand SaveBtnClickCommand
        {
            get
            {
                return new MvxCommand(async () =>
                {
                    await OnSaveClick();
                });
            }
        }

        /// <summary>
        /// Ons the save click.
        /// </summary>
        /// <returns>The save click.</returns>
        async Task OnSaveClick()
        {
            if (NetworkCheck())
            {
                if (IsBattView)
                {
                    //Save logic
                    if (Batt_VerfiyBattViewInfo())
                    {
                        ACUserDialogs.ShowProgress();
                        BattViewCommunicationTypes caller = BattViewCommunicationTypes.NOCall;
                        bool arg1 = false;
                        try
                        {
                            arg1 = Batt_SaveIntoBattViewInfo();
                            caller = BattViewCommunicationTypes.saveConfigDisconnect;
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine(ex.Message);
                        }

                        List<object> arguments = new List<object>();
                        arguments.Add(caller);
                        arguments.Add(arg1);
                        List<object> results = new List<object>();
                        try
                        {
                            results = await BattViewQuantum.Instance.CommunicateBATTView(arguments);
                            if (results.Count > 0)
                            {
                                var status = (CommunicationResult)results[2];
                                if (status == CommunicationResult.OK)
                                {
                                    EditingMode = false;
                                    CreateData();
                                    try
                                    {
                                        ResetOldData();
                                        Batt_loadBattViewInfo();
                                        RaisePropertyChanged(() => InfoItemSource);
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
                    else
                    {
                        //TODO Show Correct Errors Alert
                        ACUserDialogs.ShowAlert(AppResources.alert_enter_valid + " " + verifyControl.GetErrorString());
                    }
                    //End of save logic
                }
                else
                {
                    if (MCB_VerfiyChargerInfo())
                    {
                        ACUserDialogs.ShowProgress();
                        McbCommunicationTypes caller = McbCommunicationTypes.NOCall;
                        bool arg1 = false;
                        try
                        {
                            MCB_SaveIntoChargerInfo();
                            caller = McbCommunicationTypes.saveConfigAndTime;
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine(ex.Message);

                        }

                        List<object> arguments = new List<object>();
                        arguments.Add(caller);
                        arguments.Add(arg1);
                        arguments.Add(false);
                        List<object> results = new List<object>();
                        try
                        {
                            results = await MCBQuantum.Instance.CommunicateMCB(arguments);
                            if (results.Count > 0)
                            {
                                var status = (CommunicationResult)results[2];
                                if (status == CommunicationResult.OK)
                                {
                                    EditingMode = false;
                                    CreateData();
                                    try
                                    {
                                        ResetOldData();
                                        MCB_loadChargerInfo();
                                        RaisePropertyChanged(() => InfoItemSource);
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
                    else
                    {
                        ACUserDialogs.ShowAlert(AppResources.alert_enter_valid + " " + verifyControl.GetErrorString());
                    }
                }
            }
        }

        void ResetOldData()
        {
            foreach (var item in InfoItemSource)
            {
                item.SubTitle = string.Empty;
            }
        }

        /// <summary>
        /// Gets the back button click command.
        /// </summary>
        /// <value>The back button click command.</value>
        public IMvxCommand BackBtnClickCommand
        {
            get { return new MvxCommand(OnBackClick); }
        }

        /// <summary>
        /// Gets the cancel button click command.
        /// </summary>
        /// <value>The cancel button click command.</value>
        public IMvxCommand CancelBtnClickCommand
        {
            get { return new MvxCommand(OnBackClick); }
        }


        int MCB_SaveIntoChargerInfo()
        {
            MCBobject currentMcb = MCBQuantum.Instance.GetMCB();
            if (!MCBQuantum.Instance.MCB_quickSerialNumberCheckStrict())
                return 0;
            int reqRef = (currentMcb.Config.chargerUserName != MCB_userNamedIDTOSAVE.Text) ? 1 : 0;
            reqRef = (currentMcb.Config.serialNumber != MCB_SerialNumberTOSAVE.Text) ? 2 : reqRef;
            currentMcb.Config.serialNumber = MCB_SerialNumberTOSAVE.Text;
            currentMcb.Config.HWRevision = MCB_hardwareRevisionTextBoxTOSAVE.Text;


            if (ControlObject.isHWMnafacturer)
                return 0;
            //currentMcb.MCBConfig.originalSerialNumber = MCB_OriginalSerialNumberTOSAVE.Text;
            currentMcb.Config.model = MCB_chargerModelTextBoxTOSAVE.Text;
            currentMcb.Config.chargerUserName = MCB_userNamedIDTOSAVE.Text;
            currentMcb.Config.InstallationDate = MCB_installationDatePickerTOSAVE.Date;

            if (currentMcb.Config.chargerType != (byte)MCB_chargerTypeTOSAVE.SelectedIndex)
            {
                currentMcb.Config.chargerType = (byte)MCB_chargerTypeTOSAVE.SelectedIndex;
                if (!compareChargeProfileWithDefault())
                    MCBQuantum.Instance.MCB_saveDefaultChargeProfile();
            }
            if ((string)MCB_chargerTypeTOSAVE.Text != "Conventional")
            {
                currentMcb.Config.FIschedulingMode = true;
            }

            JsonZone info = StaticDataAndHelperFunctions.getZoneByText(MCB_timeZoneComboBoxTOSAVE.Text);
            currentMcb.myZone = info.id;
            currentMcb.Config.lcdMemoryVersion = MCB_lcdVersion_textBox.Text;
            currentMcb.Config.wifiFirmwareVersion = MCB_WIFIVersion_textBox.Text;
            return reqRef;
        }

        bool compareChargeProfileWithDefault()
        {
            MCBobject currentMcb = MCBQuantum.Instance.GetMCB();
            if (!MCBQuantum.Instance.MCB_quickSerialNumberCheckStrict())
                return true;

            if (currentMcb.Config.TRrate != 500 ||
            currentMcb.Config.FIrate != 500 ||
            currentMcb.Config.EQrate != 400 ||
            currentMcb.Config.trickleVoltage != "2.0" ||
            currentMcb.Config.FIvoltage != "2.6" ||
            currentMcb.Config.EQvoltage != "2.65" ||
            currentMcb.Config.CVfinishCurrent != 24 ||
            currentMcb.Config.CVtimer != "04:00" ||
            currentMcb.Config.finishTimer != "03:00" ||
            currentMcb.Config.EQtimer != "04:00" ||
            currentMcb.Config.desulfationTimer != "12:00" ||
            currentMcb.Config.finishDV != 5 ||
            currentMcb.Config.finishDT != "59" ||
            currentMcb.Config.CVcurrentStep != 0 ||
                currentMcb.Config.FIstartWindow != "00:00" ||
                currentMcb.Config.EQstartWindow != "00:00" ||
                currentMcb.Config.EQwindow != "24:00"
                )
                return false;
            if (currentMcb.Config.chargerType == 0)
            {
                //FAST
                if (currentMcb.Config.CVvoltage != "2.42" ||
                currentMcb.Config.CCrate != 4000 ||
                    currentMcb.Config.finishWindow != "24:00")
                    return false;

            }
            else if (currentMcb.Config.chargerType == 1)
            {
                //Conventional
                if (currentMcb.Config.CVvoltage != "2.37" ||
                currentMcb.Config.CCrate != 1700 ||
                    currentMcb.Config.finishWindow != "24:00")
                    return false;
            }
            else if (currentMcb.Config.chargerType == 2)
            {
                //Opp
                if (currentMcb.Config.CVvoltage != "2.4" ||
                currentMcb.Config.CCrate != 2500 ||
                    currentMcb.Config.finishWindow != "08:00")
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Ons the cancel click.
        /// </summary>
        void OnBackClick()
        {
            if (CheckForEditedChanges())
            {
                ACUserDialogs.ShowAlertWithTwoButtons(AppResources.cancel_confirmation, "", AppResources.yes, AppResources.no, () => OnYesClick(), null);

            }
            else
            {
                OnYesClick();
            }
        }

        private void OnYesClick()
        {
            EditingMode = false;
            CreateData();
            RaisePropertyChanged(() => InfoItemSource);
        }

        private bool CheckForEditedChanges()
        {
            bool textChanged = false;
            foreach (var item in InfoItemSource)
            {
                if (item.Text != item.SubTitle)
                {
                    textChanged = true;
                }
            }

            return textChanged;
        }

        /// <summary>
        /// Ons the back button click.
        /// </summary>
        public void OnBackButtonClick()
        {
            ShowViewModel<InfoViewModel>(new { pop = "pop" });
        }

        /// <summary>
        /// Creates the data.
        /// </summary>
        //View mode for admin and user
        public void CreateData()
        {
            if (IsBattView)
            {
                foreach (var item in InfoItemSource)
                {
                    if (item.Title.Equals(AppResources.serial_number) || item.Title.Equals(AppResources.hardware_version))
                    {
                        item.IsEditable = EditingMode && item.IsEditEnabled;
                        item.Text = item.SubTitle;
                    }
                }
            }
            else
            {
                foreach (var item in InfoItemSource)
                {
                    if (item.Title.Equals(AppResources.mcb_serial_number) || item.Title.Equals(AppResources.charger_serial_number) || item.Title.Equals(AppResources.charger_model) || item.Title.Equals(AppResources.hardware_revision) || item.Title.Equals(AppResources.client_charger_id) || item.Title.Equals(AppResources.installation_date) || item.Title.Equals(AppResources.charger_type) || item.Title.Equals(AppResources.time_zone) || item.Title.Equals(AppResources.lcd_images_version) || item.Title.Equals(AppResources.wifi_firmware_version))
                    {
                        item.IsEditable = EditingMode && item.IsEditEnabled;
                        item.Text = item.SubTitle;
                    }
                }
            }
        }

        /// <summary>
        /// Loads the BATTView Info Data from BattView_Object
        /// </summary>
        void Batt_loadBattViewInfo()
        {
            BattViewObject currentBattView = BattViewQuantum.Instance.GetBATTView();
            Batt_actView_idValue.SubTitle = Batt_actView_idValue.Text = currentBattView.Config.id.ToString();
            Batt_battViewSNTextBoxTOSAVE.SubTitle = Batt_battViewSNTextBoxTOSAVE.Text = currentBattView.Config.battViewSN;
            Batt_hardwareRevisionTextBoxTOSAVE.SubTitle = Batt_hardwareRevisionTextBoxTOSAVE.Text = currentBattView.Config.HWversion.Trim();
            Batt_setupByteTextBoxNOSAVE.SubTitle = Batt_setupByteTextBoxNOSAVE.Text = currentBattView.Config.battviewVersion.ToString();
            Batt_lastChangeUserTextBoxNOSAVE.SubTitle = Batt_lastChangeUserTextBoxNOSAVE.Text = currentBattView.Config.lastChangeUserID.ToString();
            Batt_FirmwareRevTextBox.SubTitle = Batt_FirmwareRevTextBox.Text = currentBattView.FirmwareRevision.ToString();
            Batt_memorySignature.SubTitle = Batt_memorySignature.Text = currentBattView.Config.memorySignature.ToString();
            Batt_VerfiyBattViewInfo();
        }

        /// <summary>
        /// Mcbs the load mcb info.
        /// </summary>
        void MCB_loadChargerInfo()
        {
            List<string> device_LoadWarnings = new List<string>();
            MCBobject currentMcb = MCBQuantum.Instance.GetMCB();
            //enable this later after merging the code.
            if (!MCBQuantum.Instance.MCB_quickSerialNumberCheckStrict())
                return;
            MCB_actView_idValue.SubTitle = MCB_actView_idValue.Text = currentMcb.Config.id;
            MCB_OriginalSerialNumberTOSAVE.SubTitle = MCB_OriginalSerialNumberTOSAVE.Text = currentMcb.Config.originalSerialNumber;

            MCB_SerialNumberTOSAVE.SubTitle = MCB_SerialNumberTOSAVE.Text = currentMcb.Config.serialNumber;
            MCB_chargerModelTextBoxTOSAVE.SubTitle = MCB_chargerModelTextBoxTOSAVE.Text = currentMcb.Config.model;
            MCB_hardwareRevisionTextBoxTOSAVE.SubTitle = MCB_hardwareRevisionTextBoxTOSAVE.Text = currentMcb.Config.HWRevision;
            MCB_userNamedIDTOSAVE.SubTitle = MCB_userNamedIDTOSAVE.Text = currentMcb.Config.chargerUserName;
            MCB_installationDatePickerTOSAVE.MaxDate = DateTime.MaxValue.AddYears(-2);
            MCB_installationDatePickerTOSAVE.MinDate = new DateTime(2015, 9, 1);
            MCB_installationDatePickerTOSAVE.MaxDate = DateTime.Now.Add(new TimeSpan(180, 0, 0, 0, 0));
            if (currentMcb.Config.InstallationDate < MCB_installationDatePickerTOSAVE.MinDate ||
                currentMcb.Config.InstallationDate > MCB_installationDatePickerTOSAVE.MaxDate)
            {
                MCB_installationDatePickerTOSAVE.Date = DateTime.Now;
                device_LoadWarnings.Add("Installation Date Value is out of Range");
            }
            else
            {
                MCB_installationDatePickerTOSAVE.Date = currentMcb.Config.InstallationDate;
                MCB_installationDatePickerTOSAVE.SubTitle = currentMcb.Config.InstallationDate.ToString(ACConstants.DATE_TIME_FORMAT_IOS_UI);
            }
            if (currentMcb.Config.chargerType < 0 || currentMcb.Config.chargerType > 2)
            {
                MCB_chargerTypeTOSAVE.SelectedIndex = -1;
                device_LoadWarnings.Add("Charger Type Value is not supported");
            }
            else
            {
                MCB_chargerTypeTOSAVE.Items = new List<object>();
                this.MCB_chargerTypeTOSAVE.Items.AddRange(new object[] {
                     "Fast",
                     "Conventional",
                     "Opportunity"});
                if (MCB_chargerTypeTOSAVE.Items.Count > currentMcb.Config.chargerType)
                {
                    MCB_chargerTypeTOSAVE.SelectedIndex = currentMcb.Config.chargerType;
                    MCB_chargerTypeTOSAVE.SelectedItem = MCB_chargerTypeTOSAVE.Items[MCB_chargerTypeTOSAVE.SelectedIndex].ToString();
                }
                if ((string)MCB_chargerTypeTOSAVE.SelectedItem != "Conventional")
                {
                    currentMcb.Config.FIschedulingMode = true;
                }
            }
            MCB_timeZoneComboBoxTOSAVE.Items = JsonConvert.DeserializeObject<List<object>>(JsonConvert.SerializeObject(StaticDataAndHelperFunctions.GetZonesList()));
            if (currentMcb.myZone == 0)
            {
                MCB_timeZoneComboBoxTOSAVE.SelectedIndex = -1;
                device_LoadWarnings.Add("Charger Time is not correct");
            }
            else
            {
                MCB_timeZoneComboBoxTOSAVE.SelectedItem = StaticDataAndHelperFunctions.getZoneByID(currentMcb.myZone).display_name;
                MCB_timeZoneComboBoxTOSAVE.SelectedIndex = StaticDataAndHelperFunctions.GetZonesList().FindIndex(o => o.display_name == MCB_timeZoneComboBoxTOSAVE.SelectedItem);
            }
            MCB_SETUP_BYTE_TOSAVE.SubTitle = MCB_SETUP_BYTE_TOSAVE.Text = currentMcb.Config.version.ToString();
            MCB_FIRMWARE_REV.SubTitle = MCB_FIRMWARE_REV.Text = currentMcb.FirmwareRevision.ToString();
            if (Firmware.DoesMcbRequireUpdate(currentMcb) && ControlObject.UserAccess.MCB_FirmwareUpdate != AccessLevelConsts.noAccess && !currentMcb.Config.actViewEnable)
                device_LoadWarnings.Add("New Firmware is available,update it!");
            MCB_memorySigniture.SubTitle = MCB_memorySigniture.Text = currentMcb.Config.memorySignature;

            MCB_lastChangeUserID.SubTitle = MCB_lastChangeUserID.Text = currentMcb.Config.lastChangeUserId;
            MCB_lcdVersion_textBox.SubTitle = MCB_lcdVersion_textBox.Text = currentMcb.Config.lcdMemoryVersion;
            MCB_WIFIVersion_textBox.SubTitle = MCB_WIFIVersion_textBox.Text = currentMcb.Config.wifiFirmwareVersion;
            MCB_VerfiyChargerInfo();
        }



        /// <summary>
        /// Batts the verfiy batt view info.
        /// </summary>
        /// <returns><c>true</c>, if verfiy batt view info was batted, <c>false</c> otherwise.</returns>
        bool Batt_VerfiyBattViewInfo()
        {
            verifyControl = new VerifyControl();
            string model = "";
            bool snERROR = false;

            //TODO Valication - Check Whether Serial Number is in Correct Format else throw the user an Alert
            if (ControlObject.UserAccess.Batt_SN == AccessLevelConsts.write)
                snERROR = !BattViewQuantum.Instance.batt_verifyBAttViewSerialNumber(Batt_battViewSNTextBoxTOSAVE.Text, ref model);

            verifyControl.InsertRemoveFault(snERROR ? true : false, Batt_battViewSNTextBoxTOSAVE);
            Batt_hardwareRevisionTextBoxTOSAVE.Text = Batt_hardwareRevisionTextBoxTOSAVE.Text.Replace(" ", "").ToUpper();

            //TODO Valication - Check Whether Hardware Revision is in Correct Format else throw the user an Alert
            if (Batt_hardwareRevisionTextBoxTOSAVE.Text.Length > 0 && ValidationUtility.IsValidCharacters(Batt_hardwareRevisionTextBoxTOSAVE.Text))
            {
                verifyControl.InsertRemoveFault(false, Batt_hardwareRevisionTextBoxTOSAVE);
            }
            else
            {
                verifyControl.InsertRemoveFault(true, Batt_hardwareRevisionTextBoxTOSAVE);
            }

            return !verifyControl.HasErrors();
        }

        bool MCB_VerfiyChargerInfo()
        {
            if (!MCBQuantum.Instance.MCB_quickSerialNumberCheckStrict())
                return false;
            verifyControl = new VerifyControl();
            bool snERROR = false;

            //if (ControlObject.user_access.MCB_EditOriginalSerialNumber == access_level.write)
            //{

            //if (MCB_OriginalSerialNumberTOSAVE.Text.Length != 12)
            //{
            //    snERROR = true;
            //}
            //else if (!ControlObject.isDebugMaster)
            //{
            //    List<string> validModels = new List<string> { "01", "02", "10", "20" };
            //    string productFamily = MCB_OriginalSerialNumberTOSAVE.Text.Substring(0, 1);
            //    string model = MCB_OriginalSerialNumberTOSAVE.Text.Substring(1, 2);
            //    string month = MCB_OriginalSerialNumberTOSAVE.Text.Substring(3, 2);
            //    string year = MCB_OriginalSerialNumberTOSAVE.Text.Substring(5, 2);
            //    string subid = MCB_OriginalSerialNumberTOSAVE.Text.Substring(7, 5);
            //    int tempInt = 0;
            //    if (productFamily != "2" || !validModels.Contains(model)
            //        || !int.TryParse(month, out tempInt) || tempInt < 1 || tempInt > 12
            //        || !int.TryParse(year, out tempInt) || tempInt < 16 || tempInt > 99
            //        || !int.TryParse(subid, out tempInt) || tempInt < 0 || tempInt > 99999)
            //    {
            //        snERROR = true;
            //    }

            //}

            //}
            //if (snERROR)
            //    v.insertRemoveFault(true, MCB_OriginalSerialNumberLabel);
            //else
            //    v.insertRemoveFault(false, MCB_OriginalSerialNumberLabel);

            snERROR = false;
            if (ControlObject.UserAccess.MCB_SN == AccessLevelConsts.write)
            {

                if (MCB_SerialNumberTOSAVE.Text.Length != 12)
                {
                    snERROR = true;
                }
                else if (!ControlObject.isDebugMaster)
                {
                    List<string> validModels = new List<string> { "10", "20", "30" };
                    string productFamily = MCB_SerialNumberTOSAVE.Text.Substring(0, 1);
                    string model = MCB_SerialNumberTOSAVE.Text.Substring(1, 2);
                    string month = MCB_SerialNumberTOSAVE.Text.Substring(3, 2);
                    string year = MCB_SerialNumberTOSAVE.Text.Substring(5, 2);
                    string subid = MCB_SerialNumberTOSAVE.Text.Substring(7, 5);
                    int tempInt = 0;
                    if (productFamily != "2" || !validModels.Contains(model)
                        || !int.TryParse(month, out tempInt) || tempInt < 1 || tempInt > 12
                        || !int.TryParse(year, out tempInt) || tempInt < 16 || tempInt > 99
                        || !int.TryParse(subid, out tempInt) || tempInt < 0 || tempInt > 99999)
                    {
                        snERROR = true;
                    }

                }

            }
            if (snERROR)
                verifyControl.InsertRemoveFault(true, MCB_SerialNumberTOSAVE);
            else
                verifyControl.InsertRemoveFault(false, MCB_SerialNumberTOSAVE);

            if (ControlObject.isHWMnafacturer)
                return !verifyControl.HasErrors();

            verifyControl.VerifyTextBox(MCB_chargerModelTextBoxTOSAVE, MCB_chargerModelTextBoxTOSAVE, 1, 15);
            MCB_hardwareRevisionTextBoxTOSAVE.Text = MCB_hardwareRevisionTextBoxTOSAVE.Text.Replace(" ", "").ToUpper();

            if (MCB_hardwareRevisionTextBoxTOSAVE.Text.Length > 0 && ValidationUtility.IsValidCharacters(MCB_hardwareRevisionTextBoxTOSAVE.Text))

            {
                verifyControl.InsertRemoveFault(false, MCB_hardwareRevisionTextBoxTOSAVE);
            }
            else
            {
                verifyControl.InsertRemoveFault(true, MCB_hardwareRevisionTextBoxTOSAVE);
            }

            verifyControl.VerifyTextBox(MCB_userNamedIDTOSAVE, MCB_userNamedIDTOSAVE, 1, 23);
            //MCB_installationDatePickerTOSAVE is defaulted and limited
            verifyControl.VerifyComboBox(MCB_chargerTypeTOSAVE, MCB_chargerTypeTOSAVE);
            verifyControl.VerifyComboBox(MCB_timeZoneComboBoxTOSAVE, MCB_timeZoneComboBoxTOSAVE);
            verifyControl.VerifyInteger(MCB_lcdVersion_textBox, MCB_lcdVersion_textBox, 1, 255);
            verifyControl.VerifyInteger(MCB_WIFIVersion_textBox, MCB_WIFIVersion_textBox, 1, 255);
            return !verifyControl.HasErrors();
        }

        /// <summary>
        /// Batts the save into batt view info.
        /// </summary>
        /// <returns><c>true</c>, if save into batt view info was batted, <c>false</c> otherwise.</returns>
        bool Batt_SaveIntoBattViewInfo()
        {
            if (!BattViewQuantum.Instance.BattView_quickSerialNumberCheckStrict())
                return false;
            bool reqRef = BattViewQuantum.Instance.GetBATTView().Config.battViewSN != Batt_battViewSNTextBoxTOSAVE.Text;
            BattViewQuantum.Instance.GetBATTView().Config.battViewSN = Batt_battViewSNTextBoxTOSAVE.Text;
            BattViewQuantum.Instance.GetBATTView().Config.HWversion = Batt_hardwareRevisionTextBoxTOSAVE.Text;
            return reqRef;
        }
    }
}