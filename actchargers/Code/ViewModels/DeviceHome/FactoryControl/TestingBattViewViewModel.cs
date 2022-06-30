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
    public class TestingBattViewViewModel : BaseViewModel
    {
        ListViewItem batt_test_basicHWPanel;
        ListViewItem batt_test_SerialNumberPanel;//batt_test_voltageSerialNumberPic
        ListViewItem batt_test_HWVersionPanel;
        ListViewItem batt_test_loadPLCPanel;
        ListViewItem batt_test_voltagePanel;
        ListViewItem batt_test_currentShuntPanel;
        ListViewItem batt_test_clampPanel;
        ListViewItem batt_test_temperaturePanel;
        ListViewItem batt_test_externalLEDPanel;
        ListViewItem batt_test_voltageCalibrationPanel;
        ListViewItem batt_test_wifiPanel;
        ListViewItem batt_test_PLCPanel;

        ListViewItem Batt_testStartoverButton;

        ICustomAlert customAlert;
        string batt_test_regularStatusLabel, batt_testGenericButton, batt_testGenericTextBox;

        ACTimer batt_testingTimer;

        private ObservableCollection<ListViewItem> _listItemSource;
        public ObservableCollection<ListViewItem> ListItemSource
        {
            get { return _listItemSource; }
            set
            {
                _listItemSource = value;
                RaisePropertyChanged(() => ListItemSource);
            }
        }

        private bool _batt_simpleCommunicationLock;
        private bool batt_simpleCommunicationLock
        {
            get
            {
                return _batt_simpleCommunicationLock;
            }
            set
            {
                _batt_simpleCommunicationLock = value;
            }
        }

        public void Init(bool batt_testMobile)
        {
            batt_testBattViewMobile = batt_testMobile;
            ListItemSource = new ObservableCollection<ListViewItem>();

            batt_test_basicHWPanel = new ListViewItem
            {
                Index = 0,
                DefaultCellType = ACUtility.CellTypes.ImageLabel,
                Title = AppResources.quick_hw_check,
                ImageName = string.Empty
            };
            batt_test_SerialNumberPanel = new ListViewItem
            {
                Index = 1,
                DefaultCellType = ACUtility.CellTypes.ImageLabel,
                Title = AppResources.serial_number,
                ImageName = string.Empty
            };
            batt_test_HWVersionPanel = new ListViewItem
            {
                Index = 2,
                DefaultCellType = ACUtility.CellTypes.ImageLabel,
                Title = AppResources.hw_version,
                ImageName = string.Empty
            };
            batt_test_loadPLCPanel = new ListViewItem
            {
                Index = 3,
                DefaultCellType = ACUtility.CellTypes.ImageLabel,
                Title = AppResources.load_plc_firmware,
                ImageName = string.Empty
            };
            batt_test_voltagePanel = new ListViewItem
            {
                Index = 4,
                DefaultCellType = ACUtility.CellTypes.ImageLabel,
                Title = AppResources.applying_24_volts,
                ImageName = string.Empty
            };
            batt_test_currentShuntPanel = new ListViewItem
            {
                Index = 5,
                DefaultCellType = ACUtility.CellTypes.ImageLabel,
                Title = AppResources.current_shunt,
                ImageName = string.Empty
            };
            batt_test_clampPanel = new ListViewItem
            {
                Index = 6,
                DefaultCellType = ACUtility.CellTypes.ImageLabel,
                Title = AppResources.clamp,
                ImageName = string.Empty
            };
            batt_test_temperaturePanel = new ListViewItem
            {
                Index = 7,
                DefaultCellType = ACUtility.CellTypes.ImageLabel,
                Title = AppResources.temperature,
                ImageName = string.Empty
            };
            batt_test_externalLEDPanel = new ListViewItem
            {
                Index = 8,
                DefaultCellType = ACUtility.CellTypes.ImageLabel,
                Title = AppResources.electrolyte_and_external_led,
                ImageName = string.Empty
            };
            batt_test_voltageCalibrationPanel = new ListViewItem
            {
                Index = 9,
                DefaultCellType = ACUtility.CellTypes.ImageLabel,
                Title = AppResources.voltage_calibration,
                ImageName = string.Empty
            };
            batt_test_wifiPanel = new ListViewItem
            {
                Index = 10,
                DefaultCellType = ACUtility.CellTypes.ImageLabel,
                Title = AppResources.wifi,
                ImageName = string.Empty
            };
            batt_test_PLCPanel = new ListViewItem
            {
                Index = 11,
                DefaultCellType = ACUtility.CellTypes.ImageLabel,
                Title = AppResources.plc,
                ImageName = string.Empty
            };
            if (batt_testBattViewMobile)
            {
                ViewTitle = AppResources.start_testing_battview_mobile;
                ListItemSource.Add(batt_test_basicHWPanel);
                ListItemSource.Add(batt_test_SerialNumberPanel);
                ListItemSource.Add(batt_test_HWVersionPanel);
                ListItemSource.Add(batt_test_voltagePanel);
                ListItemSource.Add(batt_test_clampPanel);
                ListItemSource.Add(batt_test_voltageCalibrationPanel);
                ListItemSource.Add(batt_test_wifiPanel);
            }
            else
            {
                ViewTitle = AppResources.start_testing_battview_standard_pro;
                ListItemSource.Add(batt_test_basicHWPanel);
                ListItemSource.Add(batt_test_SerialNumberPanel);
                ListItemSource.Add(batt_test_HWVersionPanel);
                ListItemSource.Add(batt_test_loadPLCPanel);
                ListItemSource.Add(batt_test_voltagePanel);
                ListItemSource.Add(batt_test_currentShuntPanel);
                ListItemSource.Add(batt_test_temperaturePanel);
                ListItemSource.Add(batt_test_externalLEDPanel);
                ListItemSource.Add(batt_test_voltageCalibrationPanel);
                ListItemSource.Add(batt_test_wifiPanel);
                ListItemSource.Add(batt_test_PLCPanel);
            }


            Batt_testStartoverButton = new ListViewItem
            {
                Title = "Start Over Button",
            };
            customAlert = Mvx.Resolve<ICustomAlert>();
            Task.Run(async () =>
           {
               await Batt_teststartOverButton(null, null);
           });
        }

        public TestingBattViewViewModel()
        {

        }

        BattTestStages batt_testStage;
        bool batt_sendtestCommand;
        bool batt_applytest = false;
        DateTime batt_testTime;
        DateTime batt_operationTime;
        TimeSpan batt_waitTime;
        public async Task Batt_teststartOverButton(object sender, EventArgs e)
        {
            batt_testingTimer = new ACTimer(BattView_test_Tick, null, 1000, 1000);
            //wait 10 seconds, then read from analog
            batt_testStage = BattTestStages.justStarted;
            batt_applytest = true;
            ACUserDialogs.ShowProgress("Checking Basic HW");
            //batt_test_regularStatusLabel.ForeColor = Color.Black;

            try
            {
                await BATT_simpleCommunicationButtonActionInternal(Batt_testStartoverButton, null);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }


            batt_sendtestCommand = true;
            batt_testTime = DateTime.UtcNow;
            batt_operationTime = DateTime.UtcNow;
            batt_waitTime = TimeSpan.FromSeconds(1000);
            //batt_testGenericButton.Hide();
            //batt_testGenericTextBox.Hide();
            batt_testloadImageStatus(batt_test_basicHWPanel, BATT_test_ImageStatus.active);
            batt_testloadImageStatus(batt_test_loadPLCPanel, BATT_test_ImageStatus.wait);
            batt_testloadImageStatus(batt_test_voltagePanel, BATT_test_ImageStatus.wait);
            batt_testloadImageStatus(batt_test_currentShuntPanel, BATT_test_ImageStatus.wait);
            batt_testloadImageStatus(batt_test_clampPanel, BATT_test_ImageStatus.wait);
            batt_testloadImageStatus(batt_test_temperaturePanel, BATT_test_ImageStatus.wait);
            batt_testloadImageStatus(batt_test_externalLEDPanel, BATT_test_ImageStatus.wait);
            batt_testloadImageStatus(batt_test_voltageCalibrationPanel, BATT_test_ImageStatus.wait);
            batt_testloadImageStatus(batt_test_SerialNumberPanel, BATT_test_ImageStatus.wait);
            batt_testloadImageStatus(batt_test_HWVersionPanel, BATT_test_ImageStatus.wait);
            batt_testloadImageStatus(batt_test_wifiPanel, BATT_test_ImageStatus.wait);
            batt_testloadImageStatus(batt_test_PLCPanel, BATT_test_ImageStatus.wait);
        }

        private enum BATT_test_ImageStatus : int
        {
            bad,
            good,
            wait,
            active
        }

        void batt_testloadImageStatus(ListViewItem item, BATT_test_ImageStatus status)
        {
            string imagename = string.Empty;
            switch (status)
            {
                case BATT_test_ImageStatus.active:
                    imagename = "active.png";
                    break;
                case BATT_test_ImageStatus.good:
                    imagename = "good.png";
                    break;
                case BATT_test_ImageStatus.wait:
                    imagename = "wait.png";
                    break;
                case BATT_test_ImageStatus.bad:
                    imagename = "bad.png";
                    break;

            }
            item.ImageName = imagename;
        }

        private async Task BATT_simpleCommunicationButtonActionInternal(ListViewItem sender, EventArgs e)
        {
            BattViewCommunicationTypes caller = BattViewCommunicationTypes.NOCall;
            object arg1 = null;
            string msg = "Invalid Input";
            if (!BattViewQuantum.Instance.BattView_quickSerialNumberCheckStrict())
                return;

            if (sender.Title == Batt_testStartoverButton.Title)
            {
                caller = BattViewCommunicationTypes.readAll2;
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
                    bool internalFailure = (bool)results[0];
                    string internalFailureString = (string)results[1];
                    var status = (CommunicationResult)results[2];
                    batt_lastCommStatus = status;
                    caller = (BattViewCommunicationTypes)results[3];

                    if (results.Count > 0)
                    {
                        if (internalFailure)
                        {
                            if (!(caller == BattViewCommunicationTypes.loadDebugAnalog || caller == BattViewCommunicationTypes.loadDebugAnalog2))
                                ACUserDialogs.HideProgress();
                            batt_simpleCommunicationLock = false;
                            Logger.AddLog(true, "X6" + internalFailureString);
                        }
                        //var status = (commProtocol.Communication_Result)results[2];
                        if (status != CommunicationResult.OK)
                        {
                            ACUserDialogs.HideProgress();
                            Mvx.Resolve<IMvxMessenger>().Publish(new DeviceResetMessage(this, BattViewQuantum.Instance.GetBATTView().IPAddress));
                            ShowViewModel<ConnectToDeviceViewModel>(new { popto = "popto" });
                            ACUserDialogs.ShowAlert("Couldn't communicate with the battview, make sure it's powered and the usb cable is connected");
                            //MessageBoxForm mb = new MessageBoxForm();
                            //mb.render("Couldn't communicate with the battview, make sure it's powered and the usb cable is connected", MessageBoxFormTypes.error, MessageBoxFormButtons.OK);
                            //battView_MenusShowHideInternal(Batt_test_BackButton, null);
                        }
                        else
                        {
                            //showBusy = false;
                            //scanRelated_prepare(scanRelatedTypes.doScan);
                            //ACUserDialogs.HideDialog();
                            batt_testStage = BattTestStages.quickHWcheck;
                        }
                    }
                    //else
                    //{
                    //    ACUserDialogs.HideDialog();
                    //}

                    if (!(caller == BattViewCommunicationTypes.loadDebugAnalog || caller == BattViewCommunicationTypes.loadDebugAnalog2))
                        ACUserDialogs.HideProgress();
                    batt_simpleCommunicationLock = false;
                }
                catch (Exception ex)
                {
                    ACUserDialogs.HideProgress();
                    batt_simpleCommunicationLock = false;
                    Logger.AddLog(true, "X8" + ex);
                    return;
                }
            }
            else
            {
                ACUserDialogs.HideProgress();
                ACUserDialogs.ShowAlert(msg);
            }
        }

        private CommunicationResult _batt_lastCommStatus;
        private CommunicationResult batt_lastCommStatus
        {
            get
            {
                return _batt_lastCommStatus;
            }
            set
            {
                _batt_lastCommStatus = value;
            }
        }

        bool bat_autotestingPLC = false;
        bool bat_autotestingClamp = false;
        bool bat_autotestingCurrent = false;
        bool bat_autotestingTemperature = false;
        bool bat_autotestingskipCalibration = false;

        bool bat_autotestingskipeLED = false;

        bool batt_testBattViewMobile;

        private const float batt24V = 5805;
        private const float batt36V = 8685;
        private const float batt48V = 11560;

        //private const float batt24V = 2838;//12V
        // private const float batt36V = 4267;//18V
        //private const float batt48V = 5670;//24V

        private const float clampCh1_1V = 740;
        private const float clampCh2_1V = 740;
        private const float clampCh1_2_5V = 1850;
        private const float clampCh2_2_5V = 1850;
        private const int Batt_VoltageCalibrationPoints = 5;
        private float[] Batt_VoltageADCResults = new float[Batt_VoltageCalibrationPoints];
        private float[] Batt_VoltageADC_userValues = new float[Batt_VoltageCalibrationPoints];

        private async void BattView_test_Tick(object state)
        {
            if (!BattViewQuantum.Instance.BattView_quickSerialNumberCheckStrict() || batt_testStage == BattTestStages.justStarted || !batt_applytest || batt_simpleCommunicationLock)
            {
                return;
            }

            //customAlert.RemoveCustomAlert();
            //ACUserDialogs.HideDialog();

            List<object> arguments = new List<object>();
            bool sendCommandNow = false;
            bool resetSendTimer = true;
            string errorMSG = "";
            if (batt_lastCommStatus != CommunicationResult.OK)
            {
                batt_sendtestCommand = true;
                resetSendTimer = false;
            }
            if (batt_sendtestCommand)
            {
                sendCommandNow = true;
                if (resetSendTimer)
                    batt_testTime = DateTime.UtcNow;
                if (batt_testStage == BattTestStages.forceWait0)
                {
                    if (DateTime.UtcNow - batt_testTime > TimeSpan.FromSeconds(120))
                        errorMSG = "Communication Timeout For restart, please restart maually";
                    arguments.Add(BattViewCommunicationTypes.restartDeviceNoDisconnect);
                    arguments.Add(null);
                }
                else if (batt_testStage == BattTestStages.forceWait1)
                {
                    sendCommandNow = false;
                    batt_sendtestCommand = false;
                }
                else if (batt_testStage == BattTestStages.loadPLC)
                {
                    byte[] serials = null;
                    Firmware firmwareManager = new Firmware();
                    if (firmwareManager.GetPLCBinaries(ref serials) != FirmwareResult.fileOK)
                    {
                        errorMSG = AppResources.bad_firmware_encoding;
                    }
                    else
                    {
                        if (DateTime.UtcNow - batt_testTime > TimeSpan.FromSeconds(120))
                            errorMSG = "Communication Timeout";
                        arguments.Add(BattViewCommunicationTypes.loadPLCFirmware);
                        arguments.Add(serials);
                    }
                }
                else if (batt_testStage == BattTestStages.calibrationAllSave ||
                  batt_testStage == BattTestStages.serialnumberSave ||
                  batt_testStage == BattTestStages.HWVersionSave)
                {
                    arguments.Add(BattViewCommunicationTypes.saveConfigTest);
                    arguments.Add(batt_testStage == BattTestStages.serialnumberSave);
                }
                else
                {
                    if (DateTime.UtcNow - batt_testTime > TimeSpan.FromSeconds(60))
                        errorMSG = "Communication Timeout";
                    arguments.Add(BattViewCommunicationTypes.loadDebugAnalog2);
                    arguments.Add(null);
                }

                if (sendCommandNow && batt_applytest)
                {
                    await Batt_simpleCommunicationAction_Prepare(arguments);
                    batt_sendtestCommand = false;
                }
            }
            else
            {
                batt_sendtestCommand = true;
                batt_testTime = DateTime.UtcNow;
                BattViewObject activeBattView = BattViewQuantum.Instance.GetBATTView();
                if (activeBattView.battViewAdcRawObject.rtc_state == 2)
                {
                    errorMSG += "Real Time Clock Crystal (X1) is not working" + Environment.NewLine;
                }
                if (activeBattView.battViewAdcRawObject.plc_state == 1)
                {
                    errorMSG += "CAN NOT Do a successfull Basic communication with PLC Chip (U9)" + Environment.NewLine;
                }
                if (activeBattView.battViewAdcRawObject.plc_state == 2)
                {
                    errorMSG += "CAN NOT Load and communicate with the PLC Chip (U9)" + Environment.NewLine;
                }
                if (activeBattView.battViewAdcRawObject.wifi_state != 0)
                {
                    errorMSG += "WiFi Module Failed" + Environment.NewLine;
                }
                if (errorMSG == "")
                {
                    batt_testloadImageStatus(batt_test_basicHWPanel, BATT_test_ImageStatus.good);
                }
                else
                {
                    batt_testloadImageStatus(batt_test_basicHWPanel, BATT_test_ImageStatus.bad);
                }
                if (errorMSG == "")
                {
                    switch (batt_testStage)
                    {
                        case BattTestStages.quickHWcheck:

                            //check the Quick HW Check
                            //Control UI
                            //if (batt_testBattViewMobile)
                            //{
                            //    batt_test_loadPLCPanel.Hide();
                            //    batt_test_currentShuntPanel.Hide();
                            //    batt_test_clampPanel.Show();
                            //    batt_test_PLCPanel.Hide();
                            //    batt_test_temperaturePanel.Hide();
                            //    batt_test_externalLEDPanel.Hide();
                            //}
                            //else
                            //{
                            //    batt_test_clampPanel.Hide();
                            //    batt_test_currentShuntPanel.Show();
                            //    batt_test_loadPLCPanel.Show();
                            //    batt_test_PLCPanel.Show();
                            //    batt_test_temperaturePanel.Show();
                            //    batt_test_externalLEDPanel.Show();
                            //}

                            //NEXT STAGE
                            batt_testStage = BattTestStages.serialnumber0;
                            batt_testloadImageStatus(batt_test_SerialNumberPanel, BATT_test_ImageStatus.active);

                            break;
                        case BattTestStages.serialnumber0:
                            //batt_testloadImageStatus(batt_test_voltageCalibrationPanel, BATT_test_ImageStatus.good);
                            ACUserDialogs.HideProgress();
                            batt_test_regularStatusLabel = "Enter BattView Serial Number, and hit Save";
                            batt_operationTime = DateTime.UtcNow;
                            batt_waitTime = TimeSpan.FromSeconds(300);
                            //batt_testGenericButton.Show();
                            batt_testGenericButton = "Save";
                            //batt_testGenericTextBox.Show();
                            string model = "";
                            if (BattViewQuantum.Instance.batt_verifyBAttViewSerialNumber(activeBattView.Config.battViewSN, ref model))
                            {
                                batt_testGenericTextBox = activeBattView.Config.battViewSN;
                            }
                            else
                            {
                                batt_testGenericTextBox = "";
                            }
                            InvokeOnMainThread(() =>
                                {
                                    customAlert.ShowCustomAlert(batt_test_regularStatusLabel, batt_testGenericTextBox, batt_testGenericButton, MCBButtonSelectorCommand);
                                });
                            batt_sendtestCommand = false;
                            batt_testStage = BattTestStages.serialnumber1;

                            break;
                        case BattTestStages.serialnumber1:

                            if (DateTime.UtcNow - batt_operationTime > batt_waitTime)
                            {
                                batt_testloadImageStatus(batt_test_SerialNumberPanel, BATT_test_ImageStatus.bad);

                                errorMSG = "Operation TimedOut";
                            }

                            break;
                        case BattTestStages.serialnumberSave:

                            if (DateTime.UtcNow - batt_operationTime > batt_waitTime)
                            {
                                batt_testloadImageStatus(batt_test_SerialNumberPanel, BATT_test_ImageStatus.bad);
                                errorMSG = "Operation TimedOut";
                            }

                            break;
                        case BattTestStages.afterserial:
                            //Batt_battViewSNTextBoxTOSAVE.Text = activeBattView.config.battViewSN;
                            batt_testloadImageStatus(batt_test_SerialNumberPanel, BATT_test_ImageStatus.good);
                            batt_testloadImageStatus(batt_test_HWVersionPanel, BATT_test_ImageStatus.active);
                            batt_testStage = BattTestStages.HWVersion0;
                            break;
                        case BattTestStages.HWVersion0:
                            batt_test_regularStatusLabel = "Enter Latest HW Version";
                            batt_operationTime = DateTime.UtcNow;
                            batt_waitTime = TimeSpan.FromSeconds(300);
                            //batt_testGenericButton.Show();
                            batt_testGenericButton = "Save";
                            //batt_testGenericTextBox.Show();
                            batt_testGenericTextBox = string.Empty;
                            InvokeOnMainThread(() =>
                               {
                                   customAlert.ShowCustomAlert(batt_test_regularStatusLabel, batt_testGenericTextBox, batt_testGenericButton, MCBButtonSelectorCommand);
                               });
                            batt_testStage = BattTestStages.HWVersion1;
                            batt_sendtestCommand = false;
                            break;
                        case BattTestStages.HWVersion1:

                            if (DateTime.UtcNow - batt_operationTime > batt_waitTime)
                            {
                                batt_testloadImageStatus(batt_test_HWVersionPanel, BATT_test_ImageStatus.bad);

                                errorMSG = "Operation TimedOut";
                            }

                            break;
                        case BattTestStages.HWVersionSave:

                            if (DateTime.UtcNow - batt_operationTime > batt_waitTime)
                            {
                                batt_testloadImageStatus(batt_test_HWVersionPanel, BATT_test_ImageStatus.bad);
                                errorMSG = "Operation TimedOut";
                            }

                            break;
                        case BattTestStages.afterHWVersionSave:

                            //Batt_hardwareRevisionTextBoxTOSAVE.Text = activeBattView.config.HWversion;
                            batt_testloadImageStatus(batt_test_HWVersionPanel, BATT_test_ImageStatus.good);
                            //check the PLC
                            if (!batt_testBattViewMobile && (activeBattView.battViewAdcRawObject.plc_state == 3 || bat_autotestingPLC))
                            {
                                batt_testloadImageStatus(batt_test_loadPLCPanel, BATT_test_ImageStatus.active);
                                batt_test_regularStatusLabel = "Loading PLC Firmware....";
                                ACUserDialogs.ShowProgress(batt_test_regularStatusLabel);
                                batt_operationTime = DateTime.UtcNow;
                                batt_waitTime = TimeSpan.FromSeconds(180);
                                batt_testStage = BattTestStages.loadPLC;
                            }
                            else
                            {
                                batt_testloadImageStatus(batt_test_loadPLCPanel, BATT_test_ImageStatus.good);

                                batt_test_regularStatusLabel = "Please Apply 24 Volts";
                                ACUserDialogs.ShowProgress(batt_test_regularStatusLabel);
                                batt_operationTime = DateTime.UtcNow;
                                batt_waitTime = TimeSpan.FromSeconds(90);
                                batt_testloadImageStatus(batt_test_voltagePanel, BATT_test_ImageStatus.active);
                                batt_testStage = BattTestStages.checkVoltage;
                            }
                            break;
                        case BattTestStages.loadPLC:

                            if (DateTime.UtcNow - batt_operationTime > batt_waitTime)
                            {
                                errorMSG = "Unable to load PLC Firmware automatically, please try manual mode";
                                batt_testloadImageStatus(batt_test_loadPLCPanel, BATT_test_ImageStatus.bad);
                            }
                            else if (activeBattView.battViewAdcRawObject.plc_state == 3 || bat_autotestingPLC)
                            {
                                batt_testloadImageStatus(batt_test_loadPLCPanel, BATT_test_ImageStatus.active);
                                batt_test_regularStatusLabel = "Waiting restart";
                                ACUserDialogs.ShowProgress(batt_test_regularStatusLabel);
                                batt_operationTime = DateTime.UtcNow;
                                batt_waitTime = TimeSpan.FromSeconds(90);
                                batt_testStage = BattTestStages.forceWait0;
                            }
                            else
                            {
                                batt_testloadImageStatus(batt_test_loadPLCPanel, BATT_test_ImageStatus.good);
                                batt_test_regularStatusLabel = "Please Apply 24 Volts across battery terminals";
                                ACUserDialogs.ShowProgress(batt_test_regularStatusLabel);
                                batt_operationTime = DateTime.UtcNow;
                                batt_waitTime = TimeSpan.FromSeconds(90);
                                batt_testloadImageStatus(batt_test_voltagePanel, BATT_test_ImageStatus.active);
                                batt_testStage = BattTestStages.checkVoltage;
                            }

                            break;
                        case BattTestStages.forceWait0:

                            batt_test_regularStatusLabel = "Waiting restart";
                            ACUserDialogs.ShowProgress(batt_test_regularStatusLabel);
                            batt_operationTime = DateTime.UtcNow;
                            batt_waitTime = TimeSpan.FromSeconds(15);
                            batt_testStage = BattTestStages.forceWait1;

                            break;
                        case BattTestStages.forceWait1:
                            if (DateTime.UtcNow - batt_operationTime > batt_waitTime)
                            {
                                batt_test_regularStatusLabel = "Waiting restart";
                                ACUserDialogs.ShowProgress(batt_test_regularStatusLabel);
                                batt_operationTime = DateTime.UtcNow;
                                batt_waitTime = TimeSpan.FromSeconds(90);
                                batt_testStage = BattTestStages.forceWait;
                            }

                            break;
                        case BattTestStages.forceWait:
                            if (activeBattView.battViewAdcRawObject.plc_state == 0)
                            {
                                batt_testloadImageStatus(batt_test_loadPLCPanel, BATT_test_ImageStatus.good);
                                batt_test_regularStatusLabel = "Please Apply 24 Volts across battery terminals";
                                ACUserDialogs.ShowProgress(batt_test_regularStatusLabel);
                                batt_operationTime = DateTime.UtcNow;
                                batt_waitTime = TimeSpan.FromSeconds(90);
                                batt_testloadImageStatus(batt_test_voltagePanel, BATT_test_ImageStatus.active);
                                batt_testStage = BattTestStages.checkVoltage;

                            }
                            else if (DateTime.UtcNow - batt_operationTime > batt_waitTime)
                            {
                                errorMSG = "Unable to restart device or write the PLC firmware";
                                batt_testloadImageStatus(batt_test_loadPLCPanel, BATT_test_ImageStatus.bad);

                            }
                            break;
                        case BattTestStages.checkVoltage:

                            if (Math.Abs(batt24V - activeBattView.battViewAdcRawObject.VoltageFiltered) < 220)
                            {
                                batt_testloadImageStatus(batt_test_voltagePanel, BATT_test_ImageStatus.good);
                                if (!batt_testBattViewMobile)
                                {
                                    batt_testloadImageStatus(batt_test_currentShuntPanel, BATT_test_ImageStatus.active);
                                    batt_test_regularStatusLabel = "Please Apply negative 10mV @J16-1";
                                    batt_testStage = BattTestStages.negativeCurrentSense;
                                    ACUserDialogs.ShowProgress(batt_test_regularStatusLabel);

                                }
                                else
                                {
                                    batt_testloadImageStatus(batt_test_clampPanel, BATT_test_ImageStatus.active);
                                    batt_test_regularStatusLabel = "Please Apply 1 Volt @J17-3 & J17-4";
                                    batt_testStage = BattTestStages.clamp0Read;
                                    ACUserDialogs.ShowProgress(batt_test_regularStatusLabel);
                                }
                                batt_operationTime = DateTime.UtcNow;
                                batt_waitTime = TimeSpan.FromSeconds(90);

                            }
                            else if (DateTime.UtcNow - batt_operationTime > batt_waitTime)
                            {
                                batt_testloadImageStatus(batt_test_voltagePanel, BATT_test_ImageStatus.bad);
                                errorMSG = "Can't read voltage value of 24 Volts, reference:" + activeBattView.battViewAdcRawObject.VoltageFiltered.ToString();
                            }
                            break;
                        case BattTestStages.negativeCurrentSense:

                            float CurrentFiltered = activeBattView.battViewAdcRawObject.CurrentFiltered;
                            if (bat_autotestingCurrent)
                            {
                                CurrentFiltered = -1370;
                            }
                            if (Math.Abs(-1370 - CurrentFiltered) < 140)
                            {
                                batt_test_regularStatusLabel = "Please Apply Positive 10mV @J16-1";
                                ACUserDialogs.ShowProgress(batt_test_regularStatusLabel);
                                batt_operationTime = DateTime.UtcNow;
                                batt_waitTime = TimeSpan.FromSeconds(90);
                                batt_testStage = BattTestStages.positiveCurrentSense;
                            }
                            else if (DateTime.UtcNow - batt_operationTime > batt_waitTime)
                            {
                                errorMSG = "Can't read current value @ 10mV, reference:" + activeBattView.battViewAdcRawObject.CurrentFiltered.ToString();
                                batt_testloadImageStatus(batt_test_currentShuntPanel, BATT_test_ImageStatus.bad);
                            }

                            break;
                        case BattTestStages.positiveCurrentSense:

                            CurrentFiltered = activeBattView.battViewAdcRawObject.CurrentFiltered;
                            if (bat_autotestingCurrent)
                            {
                                CurrentFiltered = 1395;
                            }

                            if (Math.Abs(1395 - CurrentFiltered) < 140)
                            {
                                batt_test_regularStatusLabel = "Please Apply 0 mV @J16-1";
                                ACUserDialogs.ShowProgress(batt_test_regularStatusLabel);
                                batt_operationTime = DateTime.UtcNow;
                                batt_waitTime = TimeSpan.FromSeconds(90);
                                batt_testStage = BattTestStages.zeroCurrentSense;
                            }
                            else if (DateTime.UtcNow - batt_operationTime > batt_waitTime)
                            {
                                errorMSG = "Can't read current value @ -10mV, reference:" + activeBattView.battViewAdcRawObject.CurrentFiltered.ToString();
                                batt_testloadImageStatus(batt_test_currentShuntPanel, BATT_test_ImageStatus.bad);
                            }
                            break;
                        case BattTestStages.zeroCurrentSense:
                            if (Math.Abs(activeBattView.battViewAdcRawObject.CurrentFiltered) < 25)
                            {
                                batt_testloadImageStatus(batt_test_currentShuntPanel, BATT_test_ImageStatus.good);

                                batt_test_regularStatusLabel = "Checking Temperature";
                                ACUserDialogs.ShowProgress(batt_test_regularStatusLabel);
                                batt_operationTime = DateTime.UtcNow;
                                batt_waitTime = TimeSpan.FromSeconds(5);
                                batt_testloadImageStatus(batt_test_temperaturePanel, BATT_test_ImageStatus.active);
                                batt_testStage = BattTestStages.temperature;
                            }
                            else if (DateTime.UtcNow - batt_operationTime > batt_waitTime)
                            {
                                batt_testloadImageStatus(batt_test_currentShuntPanel, BATT_test_ImageStatus.bad);
                                errorMSG = "Can't read current value @ 0mV, reference:" + activeBattView.battViewAdcRawObject.CurrentFiltered.ToString();
                            }
                            break;
                        case BattTestStages.clamp0Read:

                            bool clamp1 = Math.Abs(clampCh1_1V - activeBattView.battViewAdcRawObject.ClampValueFiltered) < clampCh1_1V / 8;
                            bool clamp2 = Math.Abs(clampCh2_1V - activeBattView.battViewAdcRawObject.ClampValueChannel2tFiltered) < clampCh2_1V / 8;
                            if (bat_autotestingClamp)
                            {
                                clamp1 = true;
                                clamp2 = true;
                            }

                            if (clamp1 &&
                                clamp2)
                            {
                                //batt_testloadImageStatus(batt_test_clampPanel, BATT_test_ImageStatus.good);
                                //batt_test_regularStatusLabel.Text = "Set power supply voltage  to ~24V";
                                batt_test_regularStatusLabel = "Please Apply 2.5 Volts @J17-3 & J17-4";
                                ACUserDialogs.ShowProgress(batt_test_regularStatusLabel);
                                batt_operationTime = DateTime.UtcNow;
                                batt_waitTime = TimeSpan.FromSeconds(120);
                                batt_testStage = BattTestStages.clamp1Read;
                                //batt_testloadImageStatus(batt_test_voltageCalibrationPanel, BATT_test_ImageStatus.active);
                            }
                            else if (DateTime.UtcNow - batt_operationTime > batt_waitTime)
                            {
                                batt_testloadImageStatus(batt_test_clampPanel, BATT_test_ImageStatus.bad);
                                if (!clamp1)
                                {
                                    errorMSG = "Can't read Clamp value Channel 1, reference:" + activeBattView.battViewAdcRawObject.ClampValueFiltered.ToString();

                                }
                                if (!clamp2 && !clamp1)
                                {
                                    errorMSG += Environment.NewLine;
                                }
                                if (!clamp2)
                                {
                                    errorMSG += "Can't read Clamp value Channel 2, reference:" + activeBattView.battViewAdcRawObject.ClampValueFiltered.ToString();
                                }
                            }
                            break;
                        case BattTestStages.clamp1Read:

                            bool clamp1x = Math.Abs(clampCh1_2_5V - activeBattView.battViewAdcRawObject.ClampValueFiltered) < clampCh1_2_5V / 8;
                            bool clamp2x = Math.Abs(clampCh2_2_5V - activeBattView.battViewAdcRawObject.ClampValueChannel2tFiltered) < clampCh2_2_5V / 8;
                            if (bat_autotestingClamp)
                            {
                                clamp1x = true;
                                clamp2x = true;
                            }

                            if (clamp1x &&
                                clamp2x)
                            {
                                batt_testloadImageStatus(batt_test_clampPanel, BATT_test_ImageStatus.good);

                                batt_test_regularStatusLabel = "Set power supply voltage  to ~24V";
                                ACUserDialogs.ShowProgress(batt_test_regularStatusLabel);
                                batt_operationTime = DateTime.UtcNow;
                                batt_waitTime = TimeSpan.FromSeconds(10);
                                if (bat_autotestingskipCalibration)
                                {
                                    batt_testStage = BattTestStages.wifiandPLC0;

                                }
                                else
                                    batt_testStage = BattTestStages.calibration0set;
                                batt_testloadImageStatus(batt_test_voltageCalibrationPanel, BATT_test_ImageStatus.active);

                            }
                            else if (DateTime.UtcNow - batt_operationTime > batt_waitTime)
                            {
                                batt_testloadImageStatus(batt_test_clampPanel, BATT_test_ImageStatus.bad);
                                if (!clamp1x)
                                {
                                    errorMSG = "Can't read Clamp value Channel 1, reference:" + activeBattView.battViewAdcRawObject.ClampValueFiltered.ToString();
                                }
                                if (!clamp2x && !clamp1x)
                                {
                                    errorMSG += Environment.NewLine;
                                }
                                if (!clamp2x)
                                {
                                    errorMSG += "Can't read Clamp value Channel 2, reference:" + activeBattView.battViewAdcRawObject.ClampValueFiltered.ToString();
                                }
                            }
                            break;
                        case BattTestStages.temperature:
                            bool temp1 = false;
                            bool temp2 = false;

                            if (activeBattView.battViewAdcRawObject.IntercellNTC != float.MaxValue && activeBattView.battViewAdcRawObject.IntercellNTCFiltered != float.MaxValue && activeBattView.battViewAdcRawObject.NtcRefrence_the10K != 0 && activeBattView.battViewAdcRawObject.NtcBatteryRefFiltered != 0)
                            {
                                double res1 = (activeBattView.battViewAdcRawObject.IntercellNTCFiltered / activeBattView.battViewAdcRawObject.NtcBatteryRefFiltered) * activeBattView.Config.intercellTemperatureCALa + activeBattView.Config.intercellTemperatureCALb;
                                double res2 = (activeBattView.battViewAdcRawObject.NtcBatteryFiltered / activeBattView.battViewAdcRawObject.NtcBatteryRefFiltered) * activeBattView.Config.NTCcalA + activeBattView.Config.NTCcalB;
                                temp1 = Math.Abs(10000 - res1) < 1500;
                                temp2 = Math.Abs(5000 - res2) < 750;
                            }
                            if (bat_autotestingTemperature)
                            {
                                temp1 = true;
                                temp2 = true;
                            }

                            if (temp1 && temp2)
                            {
                                batt_testloadImageStatus(batt_test_temperaturePanel, BATT_test_ImageStatus.good);
                                batt_test_regularStatusLabel = "Please Push Electrolyte switch , keep your eyes on the external LED";
                                ACUserDialogs.ShowProgress(batt_test_regularStatusLabel);
                                batt_operationTime = DateTime.UtcNow;
                                batt_waitTime = TimeSpan.FromSeconds(60);
                                batt_testStage = BattTestStages.electrolyteHigh0;
                                batt_testloadImageStatus(batt_test_externalLEDPanel, BATT_test_ImageStatus.active);

                            }
                            else if (DateTime.UtcNow - batt_operationTime > batt_waitTime)
                            {
                                batt_testloadImageStatus(batt_test_temperaturePanel, BATT_test_ImageStatus.bad);
                                if (!temp1)
                                {
                                    errorMSG = "Can't read Local Temperature Sense, reference:" + activeBattView.battViewAdcRawObject.IntercellNTCFiltered.ToString() + "," + activeBattView.battViewAdcRawObject.NtcBatteryRefFiltered.ToString();
                                }
                                if (!temp2 && !temp1)
                                {
                                    errorMSG += Environment.NewLine;
                                }
                                if (!temp2)
                                {
                                    errorMSG += "Can't read external Temperature Sense, reference:" + activeBattView.battViewAdcRawObject.NtcBatteryFiltered.ToString() + "," + activeBattView.battViewAdcRawObject.NtcBatteryRefFiltered.ToString();
                                }
                            }
                            break;
                        case BattTestStages.electrolyteHigh0:



                            if (activeBattView.battViewAdcRawObject.ElectrolyeFiltered > 60 || bat_autotestingskipeLED)
                            {
                                batt_test_regularStatusLabel = "Click the button below if the LED turned ON";
                                batt_testGenericButton = "LED is ON";
                                //batt_testGenericButton.Show();
                                ACUserDialogs.ShowAlert(batt_test_regularStatusLabel, null, batt_testGenericButton);
                                batt_operationTime = DateTime.UtcNow;
                                batt_waitTime = TimeSpan.FromSeconds(60);
                                batt_testStage = BattTestStages.electrolyteHigh1;
                            }
                            else if (DateTime.UtcNow - batt_operationTime > batt_waitTime)
                            {
                                errorMSG = "Can't read High Electrolyte";
                                batt_testloadImageStatus(batt_test_externalLEDPanel, BATT_test_ImageStatus.bad);
                            }
                            break;
                        case BattTestStages.electrolyteHigh1:

                            if (DateTime.UtcNow - batt_operationTime > batt_waitTime)
                            {
                                batt_testloadImageStatus(batt_test_externalLEDPanel, BATT_test_ImageStatus.bad);
                                errorMSG = "Operation TimeOut";
                            }

                            break;
                        case BattTestStages.electrolyteLow0:

                            if (activeBattView.battViewAdcRawObject.ElectrolyeFiltered < 60 || bat_autotestingskipeLED)
                            {
                                batt_test_regularStatusLabel = "Click the button below if the LED turned OFF";
                                batt_testGenericButton = "LED is OFF";
                                //batt_testGenericButton.Show();
                                ACUserDialogs.ShowAlert(batt_test_regularStatusLabel, null, batt_testGenericButton);
                                batt_operationTime = DateTime.UtcNow;
                                batt_waitTime = TimeSpan.FromSeconds(300);
                                batt_testStage = BattTestStages.electrolyteLow1;
                            }
                            else if (DateTime.UtcNow - batt_operationTime > batt_waitTime)
                            {
                                batt_testloadImageStatus(batt_test_externalLEDPanel, BATT_test_ImageStatus.bad);
                                errorMSG = "Can't read LOW Electrolyte";
                            }
                            break;
                        case BattTestStages.electrolyteLow1:

                            if (DateTime.UtcNow - batt_operationTime > batt_waitTime)
                            {
                                batt_testloadImageStatus(batt_test_externalLEDPanel, BATT_test_ImageStatus.bad);
                                errorMSG = "Operation TimeOut";
                            }

                            break;
                        case BattTestStages.calibration0set:

                            if (Math.Abs(batt24V - activeBattView.battViewAdcRawObject.VoltageFiltered) < 220)
                            {
                                batt_test_regularStatusLabel = "Enter the exact voltage in the text box below, and click read";
                                batt_testGenericButton = "Read";
                                batt_operationTime = DateTime.UtcNow;
                                batt_waitTime = TimeSpan.FromSeconds(300);
                                //batt_testGenericButton.Show();
                                //batt_testGenericTextBox.Show();
                                batt_testGenericTextBox = string.Empty;
                                batt_testStage = BattTestStages.calibration0enter;
                                InvokeOnMainThread(() =>
                               {
                                   customAlert.ShowCustomAlert(batt_test_regularStatusLabel, batt_testGenericTextBox, batt_testGenericButton, MCBButtonSelectorCommand);
                               });
                            }
                            else if (DateTime.UtcNow - batt_operationTime > batt_waitTime)
                            {
                                errorMSG = "Value is not around 24V, reference:" + activeBattView.battViewAdcRawObject.VoltageFiltered.ToString();
                                batt_testloadImageStatus(batt_test_voltageCalibrationPanel, BATT_test_ImageStatus.bad);
                            }
                            break;
                        case BattTestStages.calibration0enter:

                            if (DateTime.UtcNow - batt_operationTime > batt_waitTime)
                            {
                                batt_testloadImageStatus(batt_test_voltageCalibrationPanel, BATT_test_ImageStatus.bad);
                                errorMSG = "Operation TimeOut";
                            }

                            break;
                        case BattTestStages.calibration0Save:
                            Batt_VoltageADCResults[0] = activeBattView.battViewAdcRawObject.VoltageFiltered;
                            Batt_VoltageADC_userValues[0] = 100 * float.Parse(batt_testGenericTextBox);

                            batt_test_regularStatusLabel = "Set power supply voltage  to ~36V";
                            ACUserDialogs.ShowProgress(batt_test_regularStatusLabel);
                            batt_operationTime = DateTime.UtcNow;
                            batt_waitTime = TimeSpan.FromSeconds(300);
                            batt_testStage = BattTestStages.calibration1set;
                            //batt_testGenericButton.Hide();
                            //batt_testGenericTextBox.Hide();
                            break;
                        case BattTestStages.calibration1set:


                            if (Math.Abs(batt36V - activeBattView.battViewAdcRawObject.VoltageFiltered) < 330)
                            {
                                batt_test_regularStatusLabel = "Enter the exact voltage in the text box below, and click read";
                                batt_testGenericButton = "Read";
                                batt_operationTime = DateTime.UtcNow;
                                batt_waitTime = TimeSpan.FromSeconds(300);
                                //batt_testGenericButton.Show();
                                //batt_testGenericTextBox.Show();
                                batt_testGenericTextBox = string.Empty;
                                batt_testStage = BattTestStages.calibration1enter;
                                InvokeOnMainThread(() =>
                               {
                                   customAlert.ShowCustomAlert(batt_test_regularStatusLabel, batt_testGenericTextBox, batt_testGenericButton, MCBButtonSelectorCommand);
                               });
                            }
                            else if (DateTime.UtcNow - batt_operationTime > batt_waitTime)
                            {
                                errorMSG = "Value is not around 36V, reference:" + activeBattView.battViewAdcRawObject.VoltageFiltered.ToString();
                                batt_testloadImageStatus(batt_test_voltageCalibrationPanel, BATT_test_ImageStatus.bad);
                            }
                            break;
                        case BattTestStages.calibration1enter:

                            if (DateTime.UtcNow - batt_operationTime > batt_waitTime)
                            {
                                errorMSG = "Operation TimeOut";
                                batt_testloadImageStatus(batt_test_voltageCalibrationPanel, BATT_test_ImageStatus.bad);
                            }

                            break;
                        case BattTestStages.calibration1Save:
                            Batt_VoltageADCResults[1] = activeBattView.battViewAdcRawObject.VoltageFiltered;
                            Batt_VoltageADC_userValues[1] = 100 * float.Parse(batt_testGenericTextBox);

                            batt_test_regularStatusLabel = "Set power supply voltage  to ~48V";
                            batt_operationTime = DateTime.UtcNow;
                            batt_waitTime = TimeSpan.FromSeconds(300);
                            //batt_testGenericButton.Hide();
                            //batt_testGenericTextBox.Hide();
                            batt_testStage = BattTestStages.calibration2set;
                            ACUserDialogs.ShowProgress(batt_test_regularStatusLabel);
                            break;
                        case BattTestStages.calibration2set:

                            if (Math.Abs(batt48V - activeBattView.battViewAdcRawObject.VoltageFiltered) < 440)
                            {
                                batt_test_regularStatusLabel = "Enter the exact voltage in the text box below, and click read";
                                batt_testGenericButton = "Read";
                                batt_operationTime = DateTime.UtcNow;
                                batt_waitTime = TimeSpan.FromSeconds(300);
                                //batt_testGenericButton.Show();
                                //batt_testGenericTextBox.Show();
                                batt_testGenericTextBox = string.Empty;
                                batt_testStage = BattTestStages.calibration2enter;
                                InvokeOnMainThread(() =>
                               {
                                   customAlert.ShowCustomAlert(batt_test_regularStatusLabel, batt_testGenericTextBox, batt_testGenericButton, MCBButtonSelectorCommand);
                               });

                            }
                            else if (DateTime.UtcNow - batt_operationTime > batt_waitTime)
                            {
                                batt_testloadImageStatus(batt_test_voltageCalibrationPanel, BATT_test_ImageStatus.bad);
                                errorMSG = "Value is not around 48V, reference:" + activeBattView.battViewAdcRawObject.VoltageFiltered.ToString();
                            }
                            break;
                        case BattTestStages.calibration2Save:
                            Batt_VoltageADCResults[2] = activeBattView.battViewAdcRawObject.VoltageFiltered;
                            Batt_VoltageADC_userValues[2] = 100 * float.Parse(batt_testGenericTextBox);

                            batt_test_regularStatusLabel = "Writing calibration data...";
                            ACUserDialogs.ShowProgress(batt_test_regularStatusLabel);
                            batt_operationTime = DateTime.UtcNow;
                            batt_waitTime = TimeSpan.FromSeconds(300);
                            //batt_testGenericButton.Hide();
                            //batt_testGenericTextBox.Hide();
                            batt_testStage = BattTestStages.calibrationAllSave;

                            try
                            {
                                float YSum = 0;
                                float XSum = 0;
                                float XXSum = 0;
                                float XYSum = 0;
                                int n = 3;

                                for (int i = 0; i < 3; i++)
                                {
                                    YSum += Batt_VoltageADC_userValues[i];
                                    XSum += Batt_VoltageADCResults[i];
                                    XXSum += Batt_VoltageADCResults[i] * Batt_VoltageADCResults[i];
                                    XYSum += Batt_VoltageADC_userValues[i] * Batt_VoltageADCResults[i];
                                }
                                float calB = (float)((YSum * XXSum) - (XSum * XYSum)) / ((n * XXSum) - (XSum * XSum));
                                float calA = (float)((n * XYSum) - (XSum * YSum)) / ((n * XXSum) - (XSum * XSum));
                                //Batt_Voltage_CAL_B_TextBox.Text = calA.ToString();
                                //Batt_Voltage_CAL_A_TextBox.Text = calB.ToString();
                                activeBattView.Config.voltageCalA = calA;
                                activeBattView.Config.voltageCalB = calB;
                            }
                            catch (Exception ex)
                            {
                                Logger.AddLog(true, "X20" + ex.ToString());
                                return;
                            }

                            break;
                        case BattTestStages.calibrationAllSave:

                            if (DateTime.UtcNow - batt_operationTime > batt_waitTime)
                            {
                                batt_testloadImageStatus(batt_test_voltageCalibrationPanel, BATT_test_ImageStatus.bad);
                                errorMSG = "Failed to Save Calibration";
                            }

                            break;

                        case BattTestStages.wifiandPLC0:
                            batt_testloadImageStatus(batt_test_voltageCalibrationPanel, BATT_test_ImageStatus.good);

                            if (!batt_testBattViewMobile)
                            {
                                batt_testloadImageStatus(batt_test_PLCPanel, BATT_test_ImageStatus.active);
                                batt_test_regularStatusLabel = "Checking Wifi And PLC";

                            }
                            else
                            {
                                batt_test_regularStatusLabel = "Checking Wifi";

                            }
                            ACUserDialogs.ShowProgress(batt_test_regularStatusLabel);
                            batt_testloadImageStatus(batt_test_wifiPanel, BATT_test_ImageStatus.active);


                            batt_operationTime = DateTime.UtcNow;
                            batt_waitTime = TimeSpan.FromSeconds(30);
                            batt_testStage = BattTestStages.wifiandPLC1;
                            //batt_testGenericButton.Hide();
                            //batt_testGenericTextBox.Hide();
                            break;
                        case BattTestStages.wifiandPLC1:
                            bool wifi = activeBattView.battViewAdcRawObject.wifi_status == 0xFF;
                            bool plc = activeBattView.battViewAdcRawObject.plc_status == 0xFF;

                            if (wifi && (plc || batt_testBattViewMobile))
                            {
                                batt_testloadImageStatus(batt_test_wifiPanel, BATT_test_ImageStatus.good);
                                if (!batt_testBattViewMobile)
                                    batt_testloadImageStatus(batt_test_PLCPanel, BATT_test_ImageStatus.good);

                                batt_testStage = BattTestStages.allDone;

                                //batt_test_regularStatusLabel.Text = "Testing Done Successfully ";
                                //batt_applytest = false;
                            }
                            else if (DateTime.UtcNow - batt_operationTime > batt_waitTime)
                            {

                                if (!wifi)
                                {
                                    batt_testloadImageStatus(batt_test_wifiPanel, BATT_test_ImageStatus.bad);

                                    errorMSG = "WiFi Failed, reference:" + activeBattView.battViewAdcRawObject.wifi_status.ToString();
                                }
                                else
                                {
                                    batt_testloadImageStatus(batt_test_wifiPanel, BATT_test_ImageStatus.good);

                                }
                                if (!batt_testBattViewMobile)
                                {
                                    if (!wifi && !plc)
                                    {
                                        errorMSG += Environment.NewLine;
                                    }
                                    if (!plc)
                                    {
                                        batt_testloadImageStatus(batt_test_PLCPanel, BATT_test_ImageStatus.bad);
                                        errorMSG += "PLC Failed, reference:" + activeBattView.battViewAdcRawObject.plc_status.ToString();
                                    }
                                    else
                                    {
                                        batt_testloadImageStatus(batt_test_PLCPanel, BATT_test_ImageStatus.good);

                                    }
                                }
                            }
                            break;

                        case BattTestStages.allDone:
                            batt_test_regularStatusLabel = "Testing Done Successfully";
                            ACUserDialogs.ShowAlertWithTitleAndOkButton(batt_test_regularStatusLabel);
                            //batt_test_regularStatusLabel.ForeColor = Color.Green;
                            batt_applytest = false;
                            break;
                    }
                }
            }

            if (errorMSG != "")
            {
                batt_test_regularStatusLabel = errorMSG;
                //batt_test_regularStatusLabel.ForeColor = Color.Red;
                batt_applytest = false;
                //batt_testGenericButton.Hide();
                //batt_testGenericTextBox.Hide();
                ClearTimer();
                InvokeOnMainThread(() =>
                                {
                                    customAlert.RemoveCustomAlert();
                                });
                ACUserDialogs.HideProgress();
                ACUserDialogs.ShowAlert(batt_test_regularStatusLabel);
            }

        }

        public void ClearTimer()
        {
            if (batt_testingTimer != null)
            {
                batt_testingTimer.Cancel();
                batt_testingTimer.Dispose();
                batt_testingTimer = null;
            }
        }

        async Task Batt_simpleCommunicationAction_Prepare(List<object> arg)
        {
            if (batt_simpleCommunicationLock)
                return;

            batt_simpleCommunicationLock = true;
            //ACUserDialogs.ShowDialog();
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
                            ACUserDialogs.HideProgress();
                            var status = (CommunicationResult)results[2];
                            if (caller == BattViewCommunicationTypes.restartDeviceNoDisconnect)
                            {
                                if (status == CommunicationResult.COMMAND_DELAYED)
                                {
                                    ACUserDialogs.ShowAlert("BATTView is busy.");
                                    //battView_LoadAll(false);//load again , to apply changes for capacity and others
                                }
                                else if (status == CommunicationResult.OK)
                                {
                                    //showBusy = false;
                                    //scanRelated_prepare(scanRelatedTypes.doScan);
                                    //msg = "BATTView Restarting...";
                                    Mvx.Resolve<IMvxMessenger>().Publish(new DeviceResetMessage(this, BattViewQuantum.Instance.GetBATTView().IPAddress));
                                    ShowViewModel<ConnectToDeviceViewModel>(new { popto = "popto" });
                                    ACUserDialogs.ShowAlert("BATTView Restarting...");
                                }
                                //ClearTimer();
                            }
                            else if (caller == BattViewCommunicationTypes.loadPLCFirmware)
                            {
                                if (status == CommunicationResult.OK)
                                {
                                    ACUserDialogs.ShowProgress("Battview is reflushing PLC Firmware");
                                }
                                else
                                {
                                    ACUserDialogs.ShowAlert(AppResources.cant_update_firmware);
                                    ClearTimer();
                                }
                            }
                            else if (caller == BattViewCommunicationTypes.saveConfigTest)
                            {
                                if (batt_testStage == BattTestStages.calibrationAllSave)
                                {
                                    if (status == CommunicationResult.OK)
                                        batt_testStage = BattTestStages.wifiandPLC0;
                                }
                                else if (batt_testStage == BattTestStages.serialnumberSave)
                                {
                                    if (status == CommunicationResult.OK)
                                        batt_testStage = BattTestStages.afterserial;
                                }
                                else if (batt_testStage == BattTestStages.HWVersionSave)
                                {
                                    if (status == CommunicationResult.OK)
                                        batt_testStage = BattTestStages.afterHWVersionSave;
                                }
                            }
                            else if (caller == BattViewCommunicationTypes.loadDebugAnalog2)
                            {

                                Logger.AddLog(true, "X6" + (string)results[1]);

                                //setSepcialStatus(StatusSpecialMessage.ERROR);
                            }

                            batt_simpleCommunicationLock = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        batt_simpleCommunicationLock = false;
                        Logger.AddLog(true, "X8" + ex);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                batt_simpleCommunicationLock = false;
                Logger.AddLog(true, "X8" + ex);
                return;
            }
            //ACUserDialogs.HideDialog();
        }

        public IMvxCommand MCBButtonSelectorCommand
        {
            get { return new MvxCommand<string>(batt_testGenericButton_Click); }
        }

        private void batt_testGenericButton_Click(string textValue)
        {
            if (!BattViewQuantum.Instance.BattView_quickSerialNumberCheckStrict() || !batt_applytest)
            {
                return;
            }
            batt_testGenericTextBox = textValue;
            float outVal = 0;
            if (batt_testStage == BattTestStages.electrolyteHigh1)
            {
                batt_testStage = BattTestStages.electrolyteLow0;
                batt_test_regularStatusLabel = "DO NOT Push Electrolyte button, keep your eyes on the external LED";
                ACUserDialogs.ShowProgress(batt_test_regularStatusLabel);
                batt_operationTime = DateTime.UtcNow;
                batt_waitTime = TimeSpan.FromSeconds(60);
                //batt_testGenericButton.Hide();
                InvokeOnMainThread(() =>
                               {
                                   customAlert.RemoveCustomAlert();
                               });

                return;
            }
            if (batt_testStage == BattTestStages.electrolyteLow1)
            {
                batt_testloadImageStatus(batt_test_externalLEDPanel, BATT_test_ImageStatus.good);
                if (bat_autotestingskipCalibration)
                    batt_testStage = BattTestStages.wifiandPLC0;
                else
                    batt_testStage = BattTestStages.calibration0set;
                batt_test_regularStatusLabel = "Set power supply voltage  to ~24V";
                ACUserDialogs.ShowProgress(batt_test_regularStatusLabel);
                batt_operationTime = DateTime.UtcNow;
                batt_waitTime = TimeSpan.FromSeconds(300);
                //batt_testGenericButton.Hide();
                //batt_testGenericTextBox.Hide();
                batt_testloadImageStatus(batt_test_voltageCalibrationPanel, BATT_test_ImageStatus.active);
                InvokeOnMainThread(() =>
                               {
                                   customAlert.RemoveCustomAlert();
                               });

                return;
            }

            if (batt_testStage == BattTestStages.calibration0enter ||
                batt_testStage == BattTestStages.calibration1enter ||
                batt_testStage == BattTestStages.calibration2enter)
            {
                float refrenceV = 24;
                BattTestStages next = BattTestStages.calibration0Save;
                if (batt_testStage == BattTestStages.calibration1enter)
                {
                    refrenceV = 36;
                    next = BattTestStages.calibration1Save;
                }
                if (batt_testStage == BattTestStages.calibration2enter)
                {
                    refrenceV = 48;
                    next = BattTestStages.calibration2Save;
                }
                if (float.TryParse(batt_testGenericTextBox, out outVal) &&
                    Math.Abs(refrenceV - outVal) < 1)
                {
                    batt_testStage = next;
                    batt_test_regularStatusLabel = "Wait...";
                    ACUserDialogs.ShowProgress(batt_test_regularStatusLabel);
                    batt_operationTime = DateTime.UtcNow;
                    batt_waitTime = TimeSpan.FromSeconds(10);
                    //batt_testGenericButton.Hide();
                    //batt_testGenericTextBox.Hide();
                    InvokeOnMainThread(() =>
                               {
                                   customAlert.RemoveCustomAlert();
                               });
                }
                else
                {
                    //MessageBoxForm mb = new MessageBoxForm();
                    //mb.render("Value entered is not right", MessageBoxFormTypes.error, MessageBoxFormButtons.OK);
                    ACUserDialogs.ShowAlert("Value entered is not right");
                }

                return;
            }
            if (batt_testStage == BattTestStages.serialnumber1)
            {

                string model = "";
                bool invaid = true;
                if (BattViewQuantum.Instance.batt_verifyBAttViewSerialNumber(batt_testGenericTextBox, ref model))
                {
                    if ((model == "30" && batt_testBattViewMobile) || (model != "30" && !batt_testBattViewMobile))
                    {
                        invaid = false;
                        BattViewQuantum.Instance.GetBATTView().Config.battViewSN = batt_testGenericTextBox;
                    }

                }
                if (invaid)
                {
                    //MessageBoxForm mb = new MessageBoxForm();
                    //mb.render("Invalid serial", MessageBoxFormTypes.error, MessageBoxFormButtons.OK);
                    ACUserDialogs.ShowAlert("Invalid Serial");
                    return;
                }


                batt_testStage = BattTestStages.serialnumberSave;
                batt_test_regularStatusLabel = "wait....";
                ACUserDialogs.ShowProgress(batt_test_regularStatusLabel);
                batt_operationTime = DateTime.UtcNow;
                batt_waitTime = TimeSpan.FromSeconds(300);
                //batt_testGenericButton.Hide();
                //batt_testGenericTextBox.Hide();
                InvokeOnMainThread(() =>
                               {
                                   customAlert.RemoveCustomAlert();
                               });
                return;
            }
            if (batt_testStage == BattTestStages.HWVersion1)
            {

                bool invaid = true;
                batt_testGenericTextBox = batt_testGenericTextBox.Replace(" ", "").ToUpper();

                if (batt_testGenericTextBox.Length > 0 && batt_testGenericTextBox.Length < 3)// && batt_testGenericTextBox.Text.All(char.IsLetter))
                {
                    BattViewQuantum.Instance.GetBATTView().Config.HWversion = batt_testGenericTextBox;

                    invaid = false;
                }

                if (invaid)
                {
                    //MessageBoxForm mb = new MessageBoxForm();
                    //mb.render("Invalid HW Version", MessageBoxFormTypes.error, MessageBoxFormButtons.OK);
                    ACUserDialogs.ShowAlert("Invalid HW Version");
                    return;
                }


                batt_testStage = BattTestStages.HWVersionSave;
                batt_test_regularStatusLabel = "wait....";
                ACUserDialogs.ShowProgress(batt_test_regularStatusLabel);
                batt_operationTime = DateTime.UtcNow;
                batt_waitTime = TimeSpan.FromSeconds(300);
                //batt_testGenericButton.Hide();
                //batt_testGenericTextBox.Hide();
                InvokeOnMainThread(() =>
                               {
                                   customAlert.RemoveCustomAlert();
                               });
                return;
            }
        }
    }
}