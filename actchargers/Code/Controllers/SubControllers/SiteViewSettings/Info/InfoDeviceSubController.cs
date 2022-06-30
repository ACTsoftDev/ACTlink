using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using MvvmCross.Core.ViewModels;

namespace actchargers
{
    public class InfoDeviceSubController : InfoBaseSubController
    {
        ListViewItem Batt_battViewSNTextBoxTOSAVE;
        ListViewItem Batt_hardwareRevisionTextBoxTOSAVE;
        ListViewItem Batt_setupByteTextBoxNOSAVE;
        ListViewItem Batt_lastChangeUserTextBoxNOSAVE;
        ListViewItem Batt_FirmwareRevTextBox;
        ListViewItem Batt_memorySignature;
        ListViewItem Batt_actView_idValue;

        ListViewItem MCB_actView_idValue;
        ListViewItem MCB_OriginalSerialNumberTOSAVE;
        ListViewItem MCB_SerialNumberTOSAVE;
        ListViewItem MCB_hardwareRevisionTextBoxTOSAVE;
        ListViewItem MCB_userNamedIDTOSAVE;
        ListViewItem MCB_SETUP_BYTE_TOSAVE;
        ListViewItem MCB_FIRMWARE_REV;
        ListViewItem MCB_memorySigniture;
        ListViewItem MCB_lastChangeUserID;
        ListViewItem MCB_lcdVersion;
        ListViewItem MCB_BmsId;
        ListViewItem MCB_ChangeBmsId;
        ListViewItem MCB_BmsVersion;

        ListViewItem CrossFirmwareWiFiVersion;

        public InfoDeviceSubController(bool isBattView) : base(isBattView, false)
        {
        }

        #region BattView Items

        internal override void InitExclusiveBattViewItems()
        {
            Batt_actView_idValue = new ListViewItem()
            {
                Index = 0,
                Title = AppResources.act_view_id,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.LabelTextEdit
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
            CrossFirmwareWiFiVersion = new ListViewItem()
            {
                Index = 7,
                Title = AppResources.firmware_wifi_version,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.LabelLabel,
                IsEditEnabled = false
            };
        }

        #endregion

        #region Mcb Items

        internal override void InitExclusiveMcbItems()
        {
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
            CrossFirmwareWiFiVersion = new ListViewItem()
            {
                Index = 15,
                Title = AppResources.firmware_wifi_version,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.LabelLabel,
                IsEditEnabled = false
            };

            MCB_lcdVersion = new ListViewItem()
            {
                Index = 16,
                Title = AppResources.lcd_version,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.LabelSwitch
            };
            MCB_BmsId = new ListViewItem()
            {
                Index = 17,
                Title = AppResources.bms_id,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.ListSelector,
                Items = BuildBmsIdsItems(),
                ListSelectionCommand = ListSelectorCommand
            };
            MCB_ChangeBmsId = new ListViewItem()
            {
                Index = 18,
                Title = AppResources.change_bms_id,
                DefaultCellType = ACUtility.CellTypes.Button,
                EditableCellType = ACUtility.CellTypes.Button,
                Items = BuildBmsIdsItems(),
                ListSelectionCommand = ChangeBmsIdCommand
            };
            MCB_BmsVersion = new ListViewItem()
            {
                Index = 19,
                Title = AppResources.bms_version,
                DefaultCellType = ACUtility.CellTypes.LabelLabel,
                EditableCellType = ACUtility.CellTypes.LabelLabel
            };
        }

        List<object> BuildBmsIdsItems()
        {
            return new List<object>
            {
                AppResources.bms_unknown,
                AppResources.bms_gct,
                AppResources.bms_navitas,
                AppResources.bms_electrovaya
            };
        }

        public IMvxCommand ChangeBmsIdCommand
        {
            get
            {
                return new MvxAsyncCommand(ExecuteChangeBmsIdCommand);
            }
        }

        async Task ExecuteChangeBmsIdCommand()
        {
            byte selectedBmsId = (byte)MCB_BmsId.SelectedIndex;

            if (selectedBmsId <= 0)
            {
                ACUserDialogs.ShowAlert(AppResources.select_bms_id);

                return;
            }

            ACUserDialogs.ShowProgress();

            try
            {
                await TryExecuteChangeBmsIdCommand(selectedBmsId);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }

            ACUserDialogs.HideProgress();
        }

        async Task TryExecuteChangeBmsIdCommand(byte selectedBmsId)
        {
            MCBobject currentMcb = MCBQuantum.Instance.GetMCB();

            var result = await currentMcb.UpdateDcFirmware(selectedBmsId, true);

            ACUserDialogs.ShowAlert(result.Item2);
        }

        #endregion

        #region Load BattView

        internal override void LoadBattViewValues()
        {
            try
            {
                TryLoadBattViewValues();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);

                Logger.AddLog(true, "X25" + ex);
            }
        }

        void TryLoadBattViewValues()
        {
            if (!BattViewQuantum.Instance.BattView_quickSerialNumberCheckStrict())
                return;

            var currentBattView = BattViewQuantum.Instance.GetBATTView();
            var config = currentBattView.Config;

            Batt_actView_idValue.SubTitle = Batt_actView_idValue.Text = config.id.ToString();
            Batt_battViewSNTextBoxTOSAVE.SubTitle = Batt_battViewSNTextBoxTOSAVE.Text = config.battViewSN;
            Batt_hardwareRevisionTextBoxTOSAVE.SubTitle = Batt_hardwareRevisionTextBoxTOSAVE.Text = config.HWversion.Trim();
            Batt_setupByteTextBoxNOSAVE.SubTitle = Batt_setupByteTextBoxNOSAVE.Text = config.battviewVersion.ToString();
            Batt_lastChangeUserTextBoxNOSAVE.SubTitle = Batt_lastChangeUserTextBoxNOSAVE.Text = config.lastChangeUserID.ToString();
            Batt_FirmwareRevTextBox.SubTitle = Batt_FirmwareRevTextBox.Text = currentBattView.FirmwareRevision.ToString();
            Batt_memorySignature.SubTitle = Batt_memorySignature.Text = config.memorySignature.ToString();
            CrossFirmwareWiFiVersion.SubTitle = CrossFirmwareWiFiVersion.Text = currentBattView.FirmwareWiFiVersion.ToString();
        }

        #endregion

        #region Load MCB

        internal override void LoadMcbValues()
        {
            try
            {
                TryLoadMcbValues();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Logger.AddLog(true, "X25" + ex);
            }
        }

        void TryLoadMcbValues()
        {
            Task.Run((Action)TryLoadMcbValuesTask);
        }

        void TryLoadMcbValuesTask()
        {
            if (!MCBQuantum.Instance.MCB_quickSerialNumberCheckStrict())
                return;

            MCBobject currentMcb = MCBQuantum.Instance.GetMCB();
            var config = currentMcb.Config;

            MCB_actView_idValue.SubTitle = MCB_actView_idValue.Text = config.id;
            MCB_OriginalSerialNumberTOSAVE.SubTitle = MCB_OriginalSerialNumberTOSAVE.Text = config.originalSerialNumber;

            MCB_SerialNumberTOSAVE.SubTitle = MCB_SerialNumberTOSAVE.Text = config.serialNumber;
            MCB_chargerModelTextBoxTOSAVE.SubTitle = MCB_chargerModelTextBoxTOSAVE.Text = config.model;
            MCB_hardwareRevisionTextBoxTOSAVE.SubTitle = MCB_hardwareRevisionTextBoxTOSAVE.Text = config.HWRevision;
            MCB_userNamedIDTOSAVE.SubTitle = MCB_userNamedIDTOSAVE.Text = config.chargerUserName;

            MCB_installationDatePickerTOSAVE.MinDate = minInstallationDate;
            MCB_installationDatePickerTOSAVE.MaxDate = maxInstallationDate;

            if (config.InstallationDate < MCB_installationDatePickerTOSAVE.MinDate ||
                config.InstallationDate > MCB_installationDatePickerTOSAVE.MaxDate)
            {
                MCB_installationDatePickerTOSAVE.Date = DateTime.Now;
                MCB_installationDatePickerTOSAVE.SubTitle = DateTime.Now.ToString(ACConstants.DATE_TIME_FORMAT_IOS_UI);
                MCB_installationDatePickerTOSAVE.Text = config.InstallationDate.ToString(ACConstants.DATE_TIME_FORMAT_IOS_UI);
            }
            else
            {
                MCB_installationDatePickerTOSAVE.Date = config.InstallationDate;
                MCB_installationDatePickerTOSAVE.SubTitle = config.InstallationDate.ToString(ACConstants.DATE_TIME_FORMAT_IOS_UI);
                MCB_installationDatePickerTOSAVE.Text = config.InstallationDate.ToString(ACConstants.DATE_TIME_FORMAT_IOS_UI);
            }

            if (config.chargerType < 0 || config.chargerType > 2)
            {
                MCB_chargerTypeTOSAVE.SelectedIndex = -1;
            }
            else
            {
                MCB_chargerTypeTOSAVE.Items = new List<object>(chargerTypes);
                if (MCB_chargerTypeTOSAVE.Items.Count > config.chargerType)
                {
                    MCB_chargerTypeTOSAVE.SelectedIndex = config.chargerType;
                    MCB_chargerTypeTOSAVE.SelectedItem = MCB_chargerTypeTOSAVE.Items[MCB_chargerTypeTOSAVE.SelectedIndex].ToString();
                }

                config.FIschedulingMode |= MCB_chargerTypeTOSAVE.SelectedItem != AppResources.charger_type_conventional;
            }

            MCB_timeZoneComboBoxTOSAVE.Items = StaticDataAndHelperFunctions.GetZonesListAsObjects();
            if (currentMcb.myZone == 0)
            {
                MCB_timeZoneComboBoxTOSAVE.SelectedIndex = -1;
            }
            else
            {
                MCB_timeZoneComboBoxTOSAVE.SelectedItem = StaticDataAndHelperFunctions.getZoneByID(currentMcb.myZone).display_name;
                MCB_timeZoneComboBoxTOSAVE.SelectedIndex = StaticDataAndHelperFunctions.GetZonesList().FindIndex(o => o.display_name == MCB_timeZoneComboBoxTOSAVE.SelectedItem);
            }

            MCB_SETUP_BYTE_TOSAVE.SubTitle = MCB_SETUP_BYTE_TOSAVE.Text = config.version.ToString();
            MCB_FIRMWARE_REV.SubTitle = MCB_FIRMWARE_REV.Text = currentMcb.FirmwareRevision.ToString();
            MCB_memorySigniture.SubTitle = MCB_memorySigniture.Text = config.memorySignature;

            MCB_lastChangeUserID.SubTitle = MCB_lastChangeUserID.Text = config.lastChangeUserId;
            MCB_lcdVersion_textBox.SubTitle = MCB_lcdVersion_textBox.Text = config.lcdMemoryVersion;
            MCB_WIFIVersion_textBox.SubTitle = MCB_WIFIVersion_textBox.Text = config.wifiFirmwareVersion;
            CrossFirmwareWiFiVersion.SubTitle = CrossFirmwareWiFiVersion.Text = currentMcb.FirmwareWiFiVersion.ToString();

            MCB_lcdVersion.IsSwitchEnabled = config.LCD_FQ;

            if (config.DaughterCardEnabled != 0 && currentMcb.FirmwareRevision > 2.87f)
            {
                MCB_BmsId.IsVisible = true;
                MCB_ChangeBmsId.IsVisible = true;
                MCB_BmsVersion.IsVisible = true;

                int bmsSelectedIndex = 0;
                if (currentMcb.DcId > 0 && currentMcb.DcId < MCB_BmsId.Items.Count)
                    bmsSelectedIndex = currentMcb.DcId;
                else
                    bmsSelectedIndex = 0;

                MCB_BmsId.SelectedIndex = bmsSelectedIndex;
                MCB_BmsId.SubTitle = MCB_BmsId.SelectedItem = MCB_BmsId.Items[bmsSelectedIndex].ToString();

                MCB_BmsVersion.SubTitle = MCB_BmsVersion.Text = currentMcb.FirmwareDcVersion.ToString("f");
            }
            else
            {
                MCB_BmsId.IsVisible = false;
                MCB_ChangeBmsId.IsVisible = false;
                MCB_BmsVersion.IsVisible = false;
            }
        }

        #endregion

        #region Add BattView

        internal override int BattViewAccessApply()
        {
            accessControlUtility = new UIAccessControlUtility();

            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_SN, Batt_battViewSNTextBoxTOSAVE, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_HWRevision, Batt_hardwareRevisionTextBoxTOSAVE, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_setup_version, Batt_setupByteTextBoxNOSAVE, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_setup_version, Batt_lastChangeUserTextBoxNOSAVE, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_readFrimWareVersion, Batt_FirmwareRevTextBox, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_memorySignature, Batt_memorySignature, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_onlyForEnginneringTeam, CrossFirmwareWiFiVersion, ItemSource);

            ShowEdit &= accessControlUtility.GetSavedCount() != 0;

            accessControlUtility.DoApplyAccessControl(AccessLevelConsts.write, Batt_actView_idValue, ItemSource);

            return accessControlUtility.GetVisibleCount();
        }

        #endregion

        #region Add MCB

        internal override int McbAccessApply()
        {
            accessControlUtility = new UIAccessControlUtility();

            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_SN, MCB_SerialNumberTOSAVE, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_Model, MCB_chargerModelTextBoxTOSAVE, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_HWRevision, MCB_hardwareRevisionTextBoxTOSAVE, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_UserNamedID, MCB_userNamedIDTOSAVE, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_InstallationDate, MCB_installationDatePickerTOSAVE, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_chargerType, MCB_chargerTypeTOSAVE, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_TimeZone, MCB_timeZoneComboBoxTOSAVE, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_setup_version, MCB_SETUP_BYTE_TOSAVE, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_setup_version, MCB_lastChangeUserID, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_readFrimWareVersion, MCB_FIRMWARE_REV, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_memorySignature, MCB_memorySigniture, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_EditOriginalSerialNumber, MCB_OriginalSerialNumberTOSAVE, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_HWversionControl, MCB_lcdVersion_textBox, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_HWversionControl, MCB_WIFIVersion_textBox, ItemSource);
            accessControlUtility.DoApplyAccessControl(AccessLevelConsts.write, MCB_actView_idValue, ItemSource);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_onlyForEnginneringTeam, CrossFirmwareWiFiVersion, ItemSource);
            accessControlUtility.DoApplyAccessControl(AccessLevelConsts.write, MCB_lcdVersion, ItemSource);

            if (MCB_BmsId.IsVisible)
                accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_BMS_FW_CHANGE, MCB_BmsId, ItemSource);
            if (MCB_ChangeBmsId.IsVisible)
                accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.MCB_BMS_FW_CHANGE, MCB_ChangeBmsId, ItemSource);
            if (MCB_BmsVersion.IsVisible)
                accessControlUtility.DoApplyAccessControl(AccessLevelConsts.write, MCB_BmsVersion, ItemSource);

            ShowEdit &= accessControlUtility.GetSavedCount() != 0;

            return accessControlUtility.GetVisibleCount();
        }

        #endregion

        internal override void AddExclusiveItems()
        {
        }

        #region Save BattView

        internal override VerifyControl VerfiyBattViewSettings()
        {
            var verifyControl = new VerifyControl();

            string model = "";
            bool snError = false;

            if (ControlObject.UserAccess.Batt_SN == AccessLevelConsts.write)
                snError = !BattViewQuantum.Instance.batt_verifyBAttViewSerialNumber(Batt_battViewSNTextBoxTOSAVE.Text, ref model);

            verifyControl.InsertRemoveFault(snError, Batt_battViewSNTextBoxTOSAVE);
            Batt_hardwareRevisionTextBoxTOSAVE.Text = Batt_hardwareRevisionTextBoxTOSAVE.Text.Replace(" ", "").ToUpper();

            if (Batt_hardwareRevisionTextBoxTOSAVE.Text.Length > 0 && ValidationUtility.IsValidCharacters(Batt_hardwareRevisionTextBoxTOSAVE.Text))
                verifyControl.InsertRemoveFault(false, Batt_hardwareRevisionTextBoxTOSAVE);
            else
                verifyControl.InsertRemoveFault(true, Batt_hardwareRevisionTextBoxTOSAVE);

            return verifyControl;
        }

        internal override void SaveBattViewToConfigObject(BattViewObject device)
        {
            var config = device.Config;

            config.battViewSN = Batt_battViewSNTextBoxTOSAVE.Text;
            config.HWversion = Batt_hardwareRevisionTextBoxTOSAVE.Text;
        }

        #endregion

        #region Save MCB

        internal override VerifyControl VerfiyMcbSettings()
        {
            var verifyControl = new VerifyControl();

            bool snError = false;

            if (ControlObject.UserAccess.MCB_SN == AccessLevelConsts.write)
            {
                if (MCB_SerialNumberTOSAVE.Text.Length != 12)
                {
                    snError = true;
                }
                else if (!ControlObject.isDebugMaster)
                {
                    List<string> validModels = new List<string> { "10", "20", "30" };
                    string productFamily = MCB_SerialNumberTOSAVE.Text.Substring(0, 1);
                    string model = MCB_SerialNumberTOSAVE.Text.Substring(1, 2);
                    string month = MCB_SerialNumberTOSAVE.Text.Substring(3, 2);
                    string year = MCB_SerialNumberTOSAVE.Text.Substring(5, 2);
                    string subid = MCB_SerialNumberTOSAVE.Text.Substring(7, 5);
                    if (productFamily != "2" || !validModels.Contains(model)
                        || !int.TryParse(month, out int tempInt) || tempInt < 1 || tempInt > 12
                        || !int.TryParse(year, out tempInt) || tempInt < 16 || tempInt > 99
                        || !int.TryParse(subid, out tempInt) || tempInt < 0 || tempInt > 99999)
                    {
                        snError = true;
                    }
                }
            }

            if (snError)
                verifyControl.InsertRemoveFault(true, MCB_SerialNumberTOSAVE);
            else
                verifyControl.InsertRemoveFault(false, MCB_SerialNumberTOSAVE);

            if (ControlObject.isHWMnafacturer)
                return verifyControl;

            verifyControl.VerifyTextBox(MCB_chargerModelTextBoxTOSAVE, MCB_chargerModelTextBoxTOSAVE, 1, 15);
            MCB_hardwareRevisionTextBoxTOSAVE.Text = MCB_hardwareRevisionTextBoxTOSAVE.Text.Replace(" ", "").ToUpper();

            if (MCB_hardwareRevisionTextBoxTOSAVE.Text.Length > 0 && ValidationUtility.IsValidCharacters(MCB_hardwareRevisionTextBoxTOSAVE.Text))
                verifyControl.InsertRemoveFault(false, MCB_hardwareRevisionTextBoxTOSAVE);
            else
                verifyControl.InsertRemoveFault(true, MCB_hardwareRevisionTextBoxTOSAVE);

            verifyControl.VerifyTextBox(MCB_userNamedIDTOSAVE, MCB_userNamedIDTOSAVE, 1, 23);
            verifyControl.VerifyComboBox(MCB_chargerTypeTOSAVE, MCB_chargerTypeTOSAVE);
            verifyControl.VerifyComboBox(MCB_timeZoneComboBoxTOSAVE, MCB_timeZoneComboBoxTOSAVE);
            verifyControl.VerifyInteger(MCB_lcdVersion_textBox, MCB_lcdVersion_textBox, 1, 255);
            verifyControl.VerifyInteger(MCB_WIFIVersion_textBox, MCB_WIFIVersion_textBox, 1, 255);

            return verifyControl;
        }

        internal override void SaveMcbToConfigObject(MCBobject device)
        {
            var config = device.Config;

            config.serialNumber = MCB_SerialNumberTOSAVE.Text;
            config.HWRevision = MCB_hardwareRevisionTextBoxTOSAVE.Text;

            if (ControlObject.isHWMnafacturer)
                return;

            config.model = MCB_chargerModelTextBoxTOSAVE.Text;
            config.chargerUserName = MCB_userNamedIDTOSAVE.Text;
            config.InstallationDate = MCB_installationDatePickerTOSAVE.Date;

            if (config.chargerType != (byte)MCB_chargerTypeTOSAVE.SelectedIndex)
            {
                config.chargerType = (byte)MCB_chargerTypeTOSAVE.SelectedIndex;

                if (!CompareMcbProfileWithDefault(config))
                    MCBQuantum.Instance.MCB_saveDefaultChargeProfile();
            }

            config.FIschedulingMode |= MCB_chargerTypeTOSAVE.Text != "Conventional";

            var zone = StaticDataAndHelperFunctions.getZoneByText(MCB_timeZoneComboBoxTOSAVE.Text);
            device.myZone = zone.id;

            config.lcdMemoryVersion = MCB_lcdVersion_textBox.Text;
            config.wifiFirmwareVersion = MCB_WIFIVersion_textBox.Text;
            config.LCD_FQ = MCB_lcdVersion.IsSwitchEnabled;
        }

        internal override async Task<bool> SaveMcbAndReturnResult(MCBobject device)
        {
            return await device.SaveConfigAndTime();
        }

        #endregion
    }
}
