using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;
using Newtonsoft.Json;
using Plugin.DeviceInfo;

namespace actchargers
{
    public class CalibrationViewModel : BaseViewModel
    {
        MvxSubscriptionToken _mListSelector;
        Plugin.DeviceInfo.Abstractions.Platform DevicePlatform;

        #region Chargers Voltage item
        ListViewItem MCB_CALA_TextBox;
        ListViewItem MCB_CALB_TextBox;
        ListViewItem MCB_CALA_LOW_TextBox;
        ListViewItem MCB_CALB_LOW_TextBox;
        ListViewItem MCB_CAL_ADC_Raw;
        ListViewItem MCB_VoltageCalibrationRange;

        ListViewItem MCB_voltageCalibrationDirectSave;
        private int MCB_voltageCalStep;
        bool MCB_voltageCalibrationisLowRange;
        private int MCB_voltageADCResultsPtr;
        private const int MCB_voltageCalibrationPoints = 5;
        private int MCB_voltageCalibrationPoints_MIN = 2;
        private UInt16[] MCB_voltageADCResults = new UInt16[MCB_voltageCalibrationPoints];
        float[] MCB_voltageADC_userValues = new float[MCB_voltageCalibrationPoints];

        ListViewItem MCB_voltage_ADC_TextBox;
        ListViewItem MCB_voltage_CAL_ReadADCButton;
        ListViewItem MCB_voltage_CAL_ActionButton;//set another reading
        ListViewItem MCB_voltage_CAL_SaveActionButton;
        ListViewItem MCB_VoltageCalStartOverButton;

        TableHeaderItem MCB_Voltage_StartOverSection;
        TableHeaderItem MCB_Voltage_DirectSaveSection;
        private VerifyControl verifyControl;
        #endregion

        #region battview list items
        private ListViewItem Batt_Current_CAL_ReadADCButton;//Send Current button         private ListViewItem Batt_Current_ADC_TextBox;//edit         private ListViewItem Batt_Current_CAL_ActionButton;//Set another reading         private ListViewItem Batt_Current_CAL_SaveActionButton;//Save new calibration         private ListViewItem Batt_CurrentCalStartOverButton;//Start Over         private ListViewItem Batt_CurrentCalibrationDirectSave;         private ListViewItem Batt_Current_CAL_B_TextBox;         private ListViewItem Batt_Current_CAL_A_TextBox;         private ListViewItem Batt_CAL_Current_ADCraw;

        private ListViewItem Batt_Current_calibration_LowRange;         private ListViewItem Batt_Current_CAL_Clamp2B_TextBox;         private ListViewItem Batt_Current_CAL_Clamp2A_TextBox;         private ListViewItem Batt_Current_CAL_ClampB_TextBox;         private ListViewItem Batt_Current_CAL_ClampA_TextBox;

        private TableHeaderItem Batt_Current_StartOverSection;
        private TableHeaderItem Batt_Current_CalSection;
        private TableHeaderItem Batt_Current_DirectSaveSection;
        #endregion

        #region BattView SOC tab.
        private ListViewItem Batt_overrideSOC_TextBox;//edit
        private ListViewItem Batt_setSOCButton;//Set SOC button
        #endregion

        #region BattView Voltage tab items list.
        private ListViewItem Batt_Voltage_ADC_TextBox;//Batt_VoltageCalTipLabel
        private ListViewItem Batt_Voltage_CAL_ReadADCButton;//read the value of
        private ListViewItem Batt_Voltage_CAL_ActionButton;//set another reading
        private ListViewItem Batt_Voltage_CAL_SaveActionButton;//calculate and save
        private ListViewItem Batt_VoltageCalStartOverButton;//Start over
        private ListViewItem Batt_CAL_Voltage_ADCraw;//Raw
        private ListViewItem Batt_Voltage_CAL_B_TextBox;//A
        private ListViewItem Batt_Voltage_CAL_A_TextBox;//B
        private ListViewItem Batt_VoltageCalibrationDirectSave;// Save

        private TableHeaderItem Batt_Voltage_StartOverSection;
        //private TableHeaderItem Batt_Voltage_CalSection;
        private TableHeaderItem Batt_Voltage_DirectSaveSection;
        #endregion

        #region BattView Voltage

        private ListViewItem Batt_ResetVoltageCalibration;

        public bool Is_Charger_VoltageTab_Visible { get; set; }

        #endregion

        /// <summary>
        /// The list of segments.
        /// </summary>
        public List<string> listOfSegments;

        private int _selectedIndex;
        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set
            {
                _selectedIndex = value;
                //InitialiseProgressBar(ProgressCompleted + 0, ProgressMax + newIps.Count);
                CreateData();
            }
        }

        /// <summary>  
        /// The calibration current item source.
        /// </summary>
        private ObservableCollection<TableHeaderItem> calibrationCurrentItemSource = new ObservableCollection<TableHeaderItem>();
        public ObservableCollection<TableHeaderItem> CalibrationCurrentItemSource
        {
            get { return calibrationCurrentItemSource; }

            set
            {
                calibrationCurrentItemSource = value;
                RaisePropertyChanged(() => CalibrationCurrentItemSource);
            }
        }
        /// <summary>
        /// The calibration voltage item source.
        /// </summary>
        private ObservableCollection<TableHeaderItem> calibrationVoltageItemSource = new ObservableCollection<TableHeaderItem>();
        public ObservableCollection<TableHeaderItem> CalibrationVoltageItemSource
        {
            get { return calibrationVoltageItemSource; }

            set
            {
                calibrationVoltageItemSource = value;
                RaisePropertyChanged(() => CalibrationVoltageItemSource);
            }
        }
        /// <summary>
        /// The calibration SOCI tem source.
        /// </summary>
        private ObservableCollection<TableHeaderItem> calibrationSOCItemSource = new ObservableCollection<TableHeaderItem>();
        public ObservableCollection<TableHeaderItem> CalibrationSOCItemSource
        {
            get { return calibrationSOCItemSource; }

            set
            {
                calibrationSOCItemSource = value;
                RaisePropertyChanged(() => CalibrationSOCItemSource);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:actchargers.CalibrationViewModel"/> class.
        /// </summary>
        public CalibrationViewModel()
        {
            DevicePlatform = CrossDeviceInfo.Current.Platform;
            ViewTitle = AppResources.calibration;

            SelectedIndex = 0;

            CreateData();

        }
        /// <summary>
        /// Gets the set reading button click command.
        /// </summary>
        /// <value>The set reading button click command.</value>
        public IMvxCommand SetReadingBtnClickCommand
        {
            get { return new MvxCommand(OnSetReadingButtonClick); }
        }

        void OnSetReadingButtonClick()
        {
            //set reading command action here
        }

        /// <summary>
        /// Gets the list selector command.
        /// </summary>
        /// <value>The list selector command.</value>
        public IMvxCommand ListSelectorCommand
        {
            get { return new MvxCommand<ListViewItem>(ExecuteListSelectorCommand); }
        }

        /// <summary>
        /// Executes the list selector command.
        /// </summary>
        /// <param name="item">Item.</param>
        /// <param name="groupPosition">Group position.</param>
        /// <param name="childPosition">Child position.</param>
        public void ExecuteListSelectorCommand(ListViewItem item)
        {
            if (item.CellType == ACUtility.CellTypes.ListSelector)
            {
                _mListSelector = Mvx.Resolve<IMvxMessenger>().Subscribe<ListSelectorMessage>(OnListSelectorMessage);
                ShowViewModel<ListSelectorViewModel>(new { selectedItemIndex = item.ParentIndex, selectedChildPosition = CalibrationVoltageItemSource[item.ParentIndex].IndexOf(item), ItemSourceStr = JsonConvert.SerializeObject(item.Items), title = item.Title });
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
                calibrationVoltageItemSource[obj.SelectedItemindex][obj.SelectedChildItemindex].Text = obj.SelectedItem;
                calibrationVoltageItemSource[obj.SelectedItemindex][obj.SelectedChildItemindex].SelectedIndex = obj.SelectedIndex;
                RaisePropertyChanged(() => CalibrationVoltageItemSource);

            }
        }

        /// <summary>
        /// Gets the start over button click command.
        /// </summary>
        /// <value>The start over button click command.</value>
        public IMvxCommand StartOverBtnClickCommand
        {
            get { return new MvxCommand(OnStartOverButtonClick); }
        }

        void OnStartOverButtonClick()
        {
            //start over command action here
        }

        public IMvxCommand SetAnotherReadingBtnClickCommand
        {
            get { return new MvxCommand(OnSetAnotherReadingButtonClick); }
        }

        void OnSetAnotherReadingButtonClick()
        {
            //set another command action here
        }

        /// <summary>
        /// Gets the SOCB tn click command.
        /// </summary>
        /// <value>The SOCB tn click command.</value>
        public IMvxCommand SOCBtnClickCommand
        {
            get { return new MvxCommand(OnSOCButtonClick); }
        }

        void OnSOCButtonClick()
        {
            //BATT_simpleCommunicationButtonAction();
            //soc button command action here
        }

        /// <summary>
        /// Creates the data.
        /// </summary>
        void CreateData()
        {
            listOfSegments = new List<string>();

            if (IsBattView)
            {
                if (!ControlObject.isHWMnafacturer)
                {
                    Batt_ResetVoltageCalibration = new ListViewItem() { Title = AppResources.reset_voltage_cal };
                }

                listOfSegments.Add(AppResources.current);
                //checking permissions to add tabs here
                if (ControlObject.UserAccess.Batt_setSOC != AccessLevelConsts.write)
                {
                    //no  voltage tab  
                    Is_Charger_VoltageTab_Visible = false;
                }
                else
                {
                    listOfSegments.Add(AppResources.voltage);
                    Is_Charger_VoltageTab_Visible = true;
                }

                if (ControlObject.isHWMnafacturer)
                {
                    //no soc tab
                }
                else
                {
                    listOfSegments.Add(AppResources.soc);
                }
                CreateDataForBattView();
            }
            else
            {
                listOfSegments.Add(AppResources.voltage);
                createDataforCharger();
            }
        }

        void CreateDataForBattView()
        {
            CreateDataForBattViewCurrentTab();
            CreateDataForBattViewVoltageTab();
            CreateDataForBattViewSOCTab();

            InvokeOnMainThread(() =>
            {
                RaisePropertyChanged(() => CalibrationSOCItemSource);
            });

        }

        #region BattView Voltage implementation.
        void CreateDataForBattViewVoltageTab()
        {
            //Voltage tab data source

            calibrationVoltageItemSource.Clear();

            Batt_Voltage_StartOverSection = new TableHeaderItem();
            Batt_Voltage_DirectSaveSection = new TableHeaderItem();

            CalibrationVoltageItemSource.Add(Batt_Voltage_StartOverSection);
            CalibrationVoltageItemSource.Add(Batt_Voltage_DirectSaveSection);

            //   private ListViewItem Batt_Voltage_ADC_TextBox;//Batt_VoltageCalTipLabel
            Batt_Voltage_ADC_TextBox = new ListViewItem
            {
                Title = " Choose Calibration Input ",
                Text = string.Empty,
                DefaultCellType = ACUtility.CellTypes.LabelTextEdit,
                EditableCellType = ACUtility.CellTypes.LabelTextEdit,
                Index = 0,
                IsEditable = true
            };
            //private ListViewItem Batt_Voltage_CAL_ReadADCButton;//read the value of
            Batt_Voltage_CAL_ReadADCButton = new ListViewItem
            {
                Title = AppResources.read_the_value_of,
                DefaultCellType = ACUtility.CellTypes.Button,
                EditableCellType = ACUtility.CellTypes.Button,
                ListSelectionCommand = ButtonSelectorCommand,
                Index = 1,
                IsEditable = true
            };
            //private ListViewItem Batt_Voltage_CAL_ActionButton;//set another reading
            Batt_Voltage_CAL_ActionButton = new ListViewItem
            {
                Title = AppResources.set_another_reading,
                DefaultCellType = ACUtility.CellTypes.Button,
                EditableCellType = ACUtility.CellTypes.Button,
                ListSelectionCommand = ButtonSelectorCommand,
                Index = 2,
                IsEditable = true
            };
            //private ListViewItem Batt_Voltage_CAL_SaveActionButton;//calculate and save
            Batt_Voltage_CAL_SaveActionButton = new ListViewItem
            {
                Title = AppResources.calculate_and_save,
                DefaultCellType = ACUtility.CellTypes.Button,
                EditableCellType = ACUtility.CellTypes.Button,
                ListSelectionCommand = ButtonSelectorCommand,
                Index = 3,
                IsEditable = true
            };
            //private ListViewItem Batt_VoltageCalStartOverButton;//Start over
            Batt_VoltageCalStartOverButton = new ListViewItem
            {
                Title = AppResources.start_over,
                DefaultCellType = ACUtility.CellTypes.Button,
                EditableCellType = ACUtility.CellTypes.Button,
                ListSelectionCommand = ButtonSelectorCommand,
                Index = 4,
                IsEditable = true
            };
            //private ListViewItem Batt_CAL_Voltage_ADCraw;//Raw
            Batt_CAL_Voltage_ADCraw = new ListViewItem
            {
                Title = AppResources.raw,
                DefaultCellType = ACUtility.CellTypes.Default,
                EditableCellType = ACUtility.CellTypes.Default,
                Index = 5,
                IsEditable = true
            };
            //private ListViewItem Batt_Voltage_CAL_A_TextBox;//A
            Batt_Voltage_CAL_A_TextBox = new ListViewItem
            {
                Title = "A",
                Text = "1500",
                DefaultCellType = ACUtility.CellTypes.LabelTextEdit,
                EditableCellType = ACUtility.CellTypes.LabelTextEdit,
                Index = 6,
                IsEditable = true
            };
            //private ListViewItem Batt_Voltage_CAL_A_TextBox;//B
            Batt_Voltage_CAL_B_TextBox = new ListViewItem
            {
                Title = "B",
                Text = "58.71014",
                DefaultCellType = ACUtility.CellTypes.LabelTextEdit,
                EditableCellType = ACUtility.CellTypes.LabelTextEdit,
                Index = 7,
                IsEditable = true
            };
            //private ListViewItem Batt_VoltageCalibrationDirectSave;// Save
            Batt_VoltageCalibrationDirectSave = new ListViewItem
            {
                Title = AppResources.save,
                DefaultCellType = ACUtility.CellTypes.Button,
                EditableCellType = ACUtility.CellTypes.Button,
                ListSelectionCommand = ButtonSelectorCommand,
                Index = 8,
                IsEditable = true
            };

            //End voltage tab data
            //CalibrationItemSource = new ObservableCollection<TableHeaderItem>(calibrationVoltageItemSource);
            //CalibrationItemSource = calibrationVoltageItemSource;

            try
            {
                Batt_loadVoltageTab();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Logger.AddLog(true, "X25" + ex.ToString());
            }

            if (battViewCalibrationAccessApply_voltage() == 0)
            {
                ACUserDialogs.ShowAlert(AppResources.no_data_found);
                if (DevicePlatform == Plugin.DeviceInfo.Abstractions.Platform.iOS)
                {
                    ShowViewModel<CalibrationViewModel>(new { pop = "pop" });
                }
                return;
            }
            else
            {
                //Batt_CurrentCalibrationActionButton(null);
                //batt_voltagecalibrat
                Batt_Voltage_CAL_ActionButton_Click(null);
            }

        }

        void Batt_loadVoltageTab()
        {
            if (!BattViewQuantum.Instance.BattView_quickSerialNumberCheckStrict())
                return;

            BattViewObject activeBattView = BattViewQuantum.Instance.GetBATTView();
            Batt_Voltage_CAL_A_TextBox.Text = activeBattView.Config.voltageCalA.ToString();
            Batt_Voltage_CAL_B_TextBox.Text = activeBattView.Config.voltageCalB.ToString();
            Batt_validateVoltageTab();
        }

        bool Batt_validateVoltageTab()
        {
            if (!BattViewQuantum.Instance.BattView_quickSerialNumberCheckStrict())
                return false;
            verifyControl = new VerifyControl();
            verifyControl.VerifyFloatNumber(Batt_Voltage_CAL_B_TextBox, Batt_Voltage_CAL_B_TextBox, -500, 500);
            verifyControl.VerifyFloatNumber(Batt_Voltage_CAL_A_TextBox, Batt_Voltage_CAL_A_TextBox, 0.00001f, 2000.0f);

            return !verifyControl.HasErrors();
        }

        void Batt_saveIntoVoltageTab()
        {
            if (!BattViewQuantum.Instance.BattView_quickSerialNumberCheckStrict())
                return;
            BattViewObject activeBattView = BattViewQuantum.Instance.GetBATTView();
            activeBattView.Config.voltageCalA = float.Parse(Batt_Voltage_CAL_A_TextBox.Text);
            activeBattView.Config.voltageCalB = float.Parse(Batt_Voltage_CAL_B_TextBox.Text);
        }

        private const int Batt_VoltageCalibrationPoints = 5;
        private const int Batt_VoltageCalibrationPoints_MIN = 3;
        private float[] Batt_VoltageADCResults = new float[Batt_VoltageCalibrationPoints];
        float[] Batt_VoltageADC_userValues = new float[Batt_VoltageCalibrationPoints];
        private int Batt_VoltageCalStep;
        private int Batt_VoltageADCResultsPtr;
        private void Batt_Voltage_CAL_ActionButton_Click(ListViewItem item)
        {
            if (item == null || item.Title == Batt_VoltageCalStartOverButton.Title)
            {
                Batt_VoltageADCResultsPtr = 0;
                Batt_VoltageCalStep = 1;
                Batt_Voltage_ADC_TextBox.Title = "Enter Voltage Value";
                Batt_Voltage_ADC_TextBox.Text = "";
                AddOrRemoveSectionItem(Batt_Voltage_StartOverSection, Batt_Voltage_ADC_TextBox, true);//Batt_Voltage_ADC_TextBox.Show();
                AddOrRemoveSectionItem(Batt_Voltage_StartOverSection, Batt_VoltageCalStartOverButton, true);//Batt_VoltageCalStartOverButton.Show();
                AddOrRemoveSectionItem(Batt_Voltage_StartOverSection, Batt_Voltage_CAL_ReadADCButton, true); //Batt_Voltage_CAL_ReadADCButton.Show();
                AddOrRemoveSectionItem(Batt_Voltage_StartOverSection, Batt_Voltage_CAL_SaveActionButton, false);//Batt_Voltage_CAL_SaveActionButton.Hide();
                Batt_Voltage_CAL_ReadADCButton.IsEditable = true;
                //Batt_Voltage_CAL_ReadADCButton.BackColor = System.Drawing.Color.DarkOrange;
                Batt_Voltage_ADC_TextBox.IsEditable = true;
                AddOrRemoveSectionItem(Batt_Voltage_StartOverSection, Batt_Voltage_CAL_ActionButton, true);//Batt_Voltage_CAL_ActionButton.Show();
                Batt_Voltage_CAL_ActionButton.IsEditable = false;
                //Batt_Voltage_CAL_ActionButton.BackColor = System.Drawing.Color.LightGray;
                //Batt_Voltage_CAL_ActionButton.Text = "Set another reading";
                sortCalibrationVoltageSectionChildItems();
                return;
            }
            if (!BattViewQuantum.Instance.BattView_quickSerialNumberCheckStrict())
                return;
            if (item.Title == Batt_Voltage_CAL_ReadADCButton.Title)
            {
                bool allowCalculate = false;
                bool canReadMore = true;
                if (Batt_VoltageADCResultsPtr >= Batt_VoltageCalibrationPoints_MIN - 1)
                    allowCalculate = true;
                Batt_VoltageADCResultsPtr++;
                if (Batt_VoltageADCResultsPtr >= Batt_VoltageCalibrationPoints)
                    canReadMore = false;
                Batt_VoltageCalStep++;

                if (allowCalculate)
                {
                    Batt_Voltage_ADC_TextBox.Title = "Click \"Save\" ";
                    if (canReadMore)

                        Batt_Voltage_ADC_TextBox.Title += ", you can read more for more accurate calibration";
                }
                else
                {
                    Batt_Voltage_ADC_TextBox.Title = "Change Voltage input, and then click \"Set another reading\" ";
                }

                Batt_Voltage_ADC_TextBox.IsEditable = false;
                Batt_Voltage_CAL_ReadADCButton.IsEditable = false;
                //Batt_Voltage_CAL_ReadADCButton.BackColor = System.Drawing.Color.LightGray;

                if (!canReadMore)
                {
                    AddOrRemoveSectionItem(Batt_Voltage_StartOverSection, Batt_Voltage_ADC_TextBox, false);//Batt_Voltage_ADC_TextBox.Hide();
                    AddOrRemoveSectionItem(Batt_Voltage_StartOverSection, Batt_Voltage_CAL_ReadADCButton, false);//Batt_Voltage_CAL_ReadADCButton.Hide();
                    AddOrRemoveSectionItem(Batt_Voltage_StartOverSection, Batt_Voltage_CAL_ActionButton, false);//Batt_Voltage_CAL_ActionButton.Hide();
                }
                else
                {
                    Batt_Voltage_CAL_ActionButton.IsEditable = true;
                    //Batt_Voltage_CAL_ActionButton.BackColor = System.Drawing.Color.DarkOrange;
                }
                if (allowCalculate)
                {
                    AddOrRemoveSectionItem(Batt_Voltage_StartOverSection, Batt_Voltage_CAL_SaveActionButton, true);//Batt_Voltage_CAL_SaveActionButton.Show();
                }

            }
            else
            {
                AddOrRemoveSectionItem(Batt_Voltage_StartOverSection, Batt_Voltage_ADC_TextBox, true);//Batt_Voltage_ADC_TextBox.Show();
                AddOrRemoveSectionItem(Batt_Voltage_StartOverSection, Batt_Voltage_CAL_ReadADCButton, true);//Batt_Voltage_CAL_ReadADCButton.Show();
                Batt_Voltage_ADC_TextBox.IsEditable = true;
                Batt_Voltage_CAL_ReadADCButton.IsEditable = true;
                //Batt_Voltage_CAL_ReadADCButton.BackColor = System.Drawing.Color.DarkOrange;
                Batt_Voltage_CAL_ActionButton.IsEditable = false;
                //Batt_Voltage_CAL_ActionButton.BackColor = System.Drawing.Color.LightGray;
                Batt_Voltage_ADC_TextBox.Text = "";
                Batt_Voltage_ADC_TextBox.Title = "Enter Voltage Value";
            }

            sortCalibrationVoltageSectionChildItems();
        }

        private void Batt_Voltage_CAL_SaveActionButton_Click(ListViewItem item)
        {
            if (!BattViewQuantum.Instance.BattView_quickSerialNumberCheckStrict())
                return;
            try
            {
                float YSum = 0;
                float XSum = 0;
                float XXSum = 0;
                float XYSum = 0;
                int n = Batt_VoltageADCResultsPtr;
                for (int i = 0; i < Batt_VoltageADCResultsPtr; i++)
                {
                    YSum += Batt_VoltageADC_userValues[i];
                    XSum += Batt_VoltageADCResults[i];
                    XXSum += Batt_VoltageADCResults[i] * Batt_VoltageADCResults[i];
                    XYSum += Batt_VoltageADC_userValues[i] * Batt_VoltageADCResults[i];

                }
                float calA = (float)((YSum * XXSum) - (XSum * XYSum)) / ((n * XXSum) - (XSum * XSum));
                float calB = (float)((n * XYSum) - (XSum * YSum)) / ((n * XXSum) - (XSum * XSum));
                double YYDiffSum = 0;
                for (int i = 0; i < Batt_VoltageADCResultsPtr; i++)
                {

                    YYDiffSum += Math.Pow((double)Batt_VoltageADC_userValues[i] - (double)(calB * Batt_VoltageADCResults[i] + calA), 2);
                }
                YYDiffSum = Math.Pow(YYDiffSum, 0.5) / Batt_VoltageADCResultsPtr;
                if (YYDiffSum / YSum > 0.0015)
                {
                    //MessageBoxForm mb = new MessageBoxForm();
                    //mb.render("Calibration values caused calibration to fail", MessageBoxFormTypes.error, MessageBoxFormButtons.OK);
                    ACUserDialogs.ShowAlert("Calibration values caused calibration to fail");
                    Batt_Voltage_CAL_ActionButton_Click(item);
                    return;
                }
                Batt_Voltage_CAL_B_TextBox.Text = calA.ToString();
                Batt_Voltage_CAL_A_TextBox.Text = calB.ToString();

            }
            catch (Exception ex)
            {
                Debug.WriteLine("X16" + ex.Message);
                return;
            }

            BATT_simpleCommunicationButtonAction(item);
        }

        async void Batt_CalibrationCommunication_Click(ListViewItem item)
        {
            bool isVoltageOrNtc = false;
            bool isTemp = false;
            bool isPA = false;
            bool isPALowRange = false;
            float userVal = 0;
            if (!BattViewQuantum.Instance.BattView_quickSerialNumberCheckStrict())
                return;
            if (item.Title == Batt_Voltage_CAL_ReadADCButton.Title)
            {
                isTemp = false;
                isVoltageOrNtc = true;
                if (!float.TryParse(Batt_Voltage_ADC_TextBox.Text, out userVal) ||
                    userVal < 12)
                {
                    //MessageBoxForm mb = new MessageBoxForm();
                    //mb.render("Invalid Input Value", MessageBoxFormTypes.Warning, MessageBoxFormButtons.OK);
                    ACUserDialogs.ShowAlert("Invalid Input Value");
                    return;
                }
                for (int i = 0; i < Batt_VoltageADCResultsPtr; i++)
                {
                    if (Math.Abs((double)userVal - (double)Batt_VoltageADC_userValues[i]) < 1)
                    {
                        //MessageBoxForm mb = new MessageBoxForm();
                        //mb.render("Value is very close to previous value, please change", MessageBoxFormTypes.info, MessageBoxFormButtons.OK);
                        ACUserDialogs.ShowAlert("Value is very close to previous value, please change");
                        return;
                    }
                }
                Batt_VoltageADC_userValues[Batt_VoltageADCResultsPtr] = 100 * userVal;
            }

            List<object> arguments = new List<object>();
            arguments.Add(isVoltageOrNtc);
            arguments.Add(isTemp);
            arguments.Add(isPA);
            arguments.Add(isPALowRange);

            ACUserDialogs.ShowProgress();
            List<object> results = new List<object>();
            try
            {
                results = await BattViewQuantum.Instance.Batt_CalibrationCommunication(arguments);
                Batt_CalibrationCommunication_Click_doneWork(results);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Logger.AddLog(true, "X24" + ex.ToString());
            }
            ACUserDialogs.HideProgress();

        }

        void sortCalibrationVoltageSectionChildItems()
        {
            List<ListViewItem> sublist = (CalibrationVoltageItemSource[0] as IEnumerable).Cast<ListViewItem>().ToList();
            sublist = new List<ListViewItem>(sublist.OrderBy(o => o.Index));
            Batt_Voltage_StartOverSection.Clear();

            foreach (ListViewItem item in sublist)
            {
                Batt_Voltage_StartOverSection.Add(item);
            }

            RaisePropertyChanged(() => CalibrationVoltageItemSource);
        }
        #endregion


        void CreateDataForBattViewSOCTab()
        {
            //start soc tab data source 
            calibrationSOCItemSource.Clear();

            calibrationSOCItemSource.Add(new TableHeaderItem
            {
            });

            Batt_overrideSOC_TextBox = new ListViewItem
            {
                Title = AppResources.soc,
                DefaultCellType = ACUtility.CellTypes.LabelTextEdit,
                EditableCellType = ACUtility.CellTypes.LabelTextEdit,
                IsEditable = true
            };
            calibrationSOCItemSource[0].Add(Batt_overrideSOC_TextBox);

            Batt_setSOCButton = new ListViewItem
            {
                Title = AppResources.soc_set,
                DefaultCellType = ACUtility.CellTypes.Button,
                EditableCellType = ACUtility.CellTypes.Button,
                ListSelectionCommand = ButtonSelectorCommand,
                IsEditable = true
            };
            calibrationSOCItemSource[0].Add(Batt_setSOCButton);
        }

        void CreateDataForBattViewCurrentTab()
        {
            CalibrationCurrentItemSource.Clear();

            Batt_Current_StartOverSection = new TableHeaderItem();
            Batt_Current_CalSection = new TableHeaderItem();
            Batt_Current_DirectSaveSection = new TableHeaderItem();

            CalibrationCurrentItemSource.Add(Batt_Current_StartOverSection);
            CalibrationCurrentItemSource.Add(Batt_Current_CalSection);
            CalibrationCurrentItemSource.Add(Batt_Current_DirectSaveSection);

            Batt_Current_calibration_LowRange = new ListViewItem
            {
                Title = AppResources.low_range_calibration,
                DefaultCellType = ACUtility.CellTypes.LabelSwitch,
                EditableCellType = ACUtility.CellTypes.LabelSwitch,
                SwitchValueChanged = SwitchValueChanged,
                IsEditable = true
            };

            Batt_Current_ADC_TextBox = new ListViewItem
            {
                Title = "Current value",
                DefaultCellType = ACUtility.CellTypes.LabelTextEdit,
                EditableCellType = ACUtility.CellTypes.LabelTextEdit,
                Index = 1,
                IsEditable = true

            };

            Batt_Current_CAL_ReadADCButton = new ListViewItem

            {
                Title = AppResources.send_current,
                DefaultCellType = ACUtility.CellTypes.Button,
                EditableCellType = ACUtility.CellTypes.Button,
                ListSelectionCommand = ButtonSelectorCommand,
                Index = 2,
                IsEditable = true
            };

            Batt_Current_CAL_ActionButton = new ListViewItem

            {
                Title = AppResources.set_another_reading,
                DefaultCellType = ACUtility.CellTypes.Button,
                EditableCellType = ACUtility.CellTypes.Button,
                ListSelectionCommand = ButtonSelectorCommand,
                Index = 3,
                IsEditable = true
            };



            Batt_Current_CAL_SaveActionButton = new ListViewItem
            {
                Title = AppResources.save_new_calibration,
                DefaultCellType = ACUtility.CellTypes.Button,
                EditableCellType = ACUtility.CellTypes.Button,
                ListSelectionCommand = ButtonSelectorCommand,
                Index = 4,
                IsEditable = true
            };
            Batt_CurrentCalStartOverButton = new ListViewItem

            {
                Title = AppResources.start_over,
                DefaultCellType = ACUtility.CellTypes.Button,
                EditableCellType = ACUtility.CellTypes.Button,
                ListSelectionCommand = ButtonSelectorCommand,
                Index = 5,
                IsEditable = true
            };

            Batt_CAL_Current_ADCraw = new ListViewItem
            {
                Title = AppResources.raw,
                DefaultCellType = ACUtility.CellTypes.Default,
                EditableCellType = ACUtility.CellTypes.Default,
                Index = 6,
                IsEditable = true
            };

            Batt_Current_CAL_B_TextBox = new ListViewItem
            {
                Title = "B",
                DefaultCellType = ACUtility.CellTypes.LabelTextEdit,
                EditableCellType = ACUtility.CellTypes.LabelTextEdit,
                Index = 1,
                Text = "1500",
                IsEditable = true
            };

            Batt_Current_CAL_A_TextBox = new ListViewItem

            {
                Title = "A",
                DefaultCellType = ACUtility.CellTypes.LabelTextEdit,
                EditableCellType = ACUtility.CellTypes.LabelTextEdit,
                Index = 2,
                Text = "58.71014",
                IsEditable = true
            };

            Batt_Current_CAL_ClampA_TextBox = new ListViewItem

            {
                Title = AppResources.low,
                DefaultCellType = ACUtility.CellTypes.LabelTextEdit,
                EditableCellType = ACUtility.CellTypes.LabelTextEdit,
                Index = 1,
                IsEditable = true
            };
            Batt_Current_CAL_ClampB_TextBox = new ListViewItem

            {
                Title = "B",
                DefaultCellType = ACUtility.CellTypes.LabelTextEdit,
                EditableCellType = ACUtility.CellTypes.LabelTextEdit,
                Index = 2,
                IsEditable = true
            };
            Batt_Current_CAL_Clamp2A_TextBox = new ListViewItem

            {
                Title = AppResources.all,
                DefaultCellType = ACUtility.CellTypes.LabelTextEdit,
                EditableCellType = ACUtility.CellTypes.LabelTextEdit,
                Index = 3,
                IsEditable = true

            };
            Batt_Current_CAL_Clamp2B_TextBox = new ListViewItem

            {
                Title = "B",
                DefaultCellType = ACUtility.CellTypes.LabelTextEdit,
                EditableCellType = ACUtility.CellTypes.LabelTextEdit,
                Index = 4,
                IsEditable = true
            };

            Batt_CurrentCalibrationDirectSave = new ListViewItem

            {
                Title = AppResources.direct_save,
                DefaultCellType = ACUtility.CellTypes.Button,
                EditableCellType = ACUtility.CellTypes.Button,
                ListSelectionCommand = ButtonSelectorCommand,
                Index = 5,
                IsEditable = true
            };
            try
            {
                Batt_loadCurrentTab();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Logger.AddLog(true, "X25" + ex.ToString());
            }
            if (battViewCalibrationAccessApply() == 0)
            {
                ACUserDialogs.ShowAlert(AppResources.no_data_found);
                if (DevicePlatform == Plugin.DeviceInfo.Abstractions.Platform.iOS)
                {
                    ShowViewModel<CalibrationViewModel>(new { pop = "pop" });
                }
                return;
            }
            else
            {
                Batt_CurrentCalibrationActionButton(null);
            }

        }

        public IMvxCommand SwitchValueChanged
        {
            get
            {
                return new MvxCommand<ListViewItem>(ExecuteSwitchValueChanged);
            }
        }

        /// <param name="item">Item.</param>
        void ExecuteSwitchValueChanged(ListViewItem item)
        {
        }

        async Task BattViewCurrentDirectSave()
        {
            if (NetworkCheck())
            {

                ACUserDialogs.ShowProgress();
                BattViewCommunicationTypes caller = BattViewCommunicationTypes.NOCall;
                object arg1 = null;

                if (Batt_validateCurrentTab())
                {
                    Batt_saveIntoCurrentTab();
                    arg1 = false;
                    caller = BattViewCommunicationTypes.saveConfig;
                }
                if (caller != BattViewCommunicationTypes.NOCall)
                {
                    List<object> arguments = new List<object>();
                    arguments.Add(caller);
                    arguments.Add(arg1);
                    arguments.Add(false);
                    List<object> results = new List<object>();
                    try
                    {
                        results = await BattViewQuantum.Instance.CommunicateBATTView(arguments);
                        if (results.Count > 0)
                        {
                            var status = (CommunicationResult)results[2];
                            if (status == CommunicationResult.OK)
                            {
                                ResetOldData();
                                Batt_loadCurrentTab();
                            }
                            else
                            {
                                ACUserDialogs.ShowAlertWithTitleAndOkButton(AppResources.opration_failed);
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }
                }
                else
                {
                    ACUserDialogs.ShowAlertWithTitleAndOkButton(AppResources.alert_enter_valid + "\n" + verifyControl.GetErrorString());
                }
                ACUserDialogs.HideProgress();
            }
        }

        #region battview current cal save button functionality
        private int Batt_CurrentADCResultsPtr;
        private const int Batt_CurrentCalibrationPoints = 3;
        private const int Batt_CurrentCalibrationPoints_MIN = 3;
        float[] Batt_CurrentADC_userValues = new float[Batt_CurrentCalibrationPoints];
        private float[] Batt_CurrentADCResults = new float[Batt_CurrentCalibrationPoints];
        private float[] Batt_intercellTemp = new float[Batt_CurrentCalibrationPoints];
        private int Batt_CurrentCalStep;
        private void Batt_Current_CAL_SaveActionButton_Click()
        {
            if (!BattViewQuantum.Instance.BattView_quickSerialNumberCheckStrict())
            {
                return;
            }
            BattViewObject activeBattView = BattViewQuantum.Instance.GetBATTView();
            float currentDrift = 0;
            for (int i = 0; i < Batt_CurrentADCResultsPtr; i++)
            {
                if (Math.Abs(Batt_CurrentADC_userValues[i]) < 0.1)
                    currentDrift = Batt_CurrentADCResults[i];
            }
            currentDrift = 0;
            try
            {
                float YSum = 0;
                float XSum = 0;
                float XXSum = 0;
                float XYSum = 0;
                int n = Batt_CurrentADCResultsPtr;
                for (int i = 0; i < Batt_CurrentADCResultsPtr; i++)
                {
                    if (activeBattView.Config.enableHallEffectSensing == 0)
                    {
                        // Batt_CurrentADCResults[i] -= currentDrift;
                        Batt_CurrentADCResults[i] /= (1.0f + ((Batt_intercellTemp[i] - 25) * activeBattView.Config.intercellCoefficient));
                    }

                    YSum += Batt_CurrentADC_userValues[i];
                    XSum += Batt_CurrentADCResults[i];
                    XXSum += Batt_CurrentADCResults[i] * Batt_CurrentADCResults[i];
                    XYSum += Batt_CurrentADC_userValues[i] * Batt_CurrentADCResults[i];

                }
                float calB = (float)((YSum * XXSum) - (XSum * XYSum)) / ((n * XXSum) - (XSum * XSum));
                float calA = (float)((n * XYSum) - (XSum * YSum)) / ((n * XXSum) - (XSum * XSum));
                double YYDiffSum = 0;
                double absYSum = 0;
                for (int i = 0; i < Batt_CurrentADCResultsPtr; i++)
                {

                    YYDiffSum += Math.Pow((double)Batt_CurrentADC_userValues[i] - ((double)(calA * Batt_CurrentADCResults[i] + calB)), 2);
                    absYSum += Math.Abs(Batt_CurrentADC_userValues[i]);
                }
                YYDiffSum = Math.Pow(YYDiffSum, 0.5) / Batt_CurrentADCResultsPtr;
                if (Math.Abs(YYDiffSum / absYSum) > 0.01)
                {
                    ACUserDialogs.ShowAlertWithTitleAndOkButton("Calibration values caused calibration to fail");
                    Batt_CurrentCalibrationActionButton(Batt_Current_CAL_SaveActionButton);
                    return;
                }
                if (activeBattView.Config.enableHallEffectSensing == 0)
                {
                    Batt_Current_CAL_A_TextBox.Text = calA.ToString();
                    Batt_Current_CAL_B_TextBox.Text = calB.ToString();
                }
                else if (Batt_Current_calibration_LowRange.IsSwitchEnabled)
                {
                    Batt_Current_CAL_ClampA_TextBox.Text = calA.ToString();
                    Batt_Current_CAL_ClampB_TextBox.Text = calB.ToString();
                }
                else
                {
                    Batt_Current_CAL_Clamp2A_TextBox.Text = calA.ToString();
                    Batt_Current_CAL_Clamp2B_TextBox.Text = calB.ToString();
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine("X15" + ex.ToString());
                return;
            }
            BATT_CALSaveButtonAction();

        }

        async void BATT_CALSaveButtonAction()
        {
            if (NetworkCheck())
            {
                BattViewCommunicationTypes caller = BattViewCommunicationTypes.NOCall;
                object arg1 = null;
                if (Batt_validateCurrentTab())
                {
                    Batt_saveIntoCurrentTab();
                    caller = BattViewCommunicationTypes.Batt_currentcalibrationSaveConfig;
                    arg1 = false;
                }

                if (caller != BattViewCommunicationTypes.NOCall)
                {
                    ACUserDialogs.ShowProgress();
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
                                try
                                {
                                    Batt_CurrentCalibrationActionButton(null);
                                }

                                catch (Exception ex)
                                {
                                    Debug.WriteLine(ex.Message);
                                    Logger.AddLog(true, "X24" + ex.ToString());
                                }
                            }
                            else
                            {
                                ACUserDialogs.ShowAlertWithTitleAndOkButton(AppResources.opration_failed);
                            }

                        }
                        else
                        {
                            ACUserDialogs.ShowAlertWithTitleAndOkButton(AppResources.opration_failed);
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
                    ACUserDialogs.ShowAlertWithTitleAndOkButton(AppResources.alert_enter_valid + "\n" + verifyControl.GetErrorString());
                }
            }
        }
        void Batt_CurrentCalibrationActionButton(ListViewItem item)
        {
            if (item == null || item.Title == AppResources.start_over)
            {
                BattViewObject activeBattView = BattViewQuantum.Instance.GetBATTView();

                if (BattViewQuantum.Instance.BattView_quickSerialNumberCheckStrict())
                {
                    if (activeBattView.Config.enableHallEffectSensing != 0)
                    {
                        AddOrRemoveSectionItem(Batt_Current_StartOverSection, Batt_Current_calibration_LowRange, true);
                    }
                    else
                    {
                        AddOrRemoveSectionItem(Batt_Current_StartOverSection, Batt_Current_calibration_LowRange, false);
                    }
                }
                Batt_CurrentADCResultsPtr = 0;
                Batt_CurrentCalStep = 1;
                Batt_Current_ADC_TextBox.Title = "Enter Current Value (Negative for In Use)";

                Batt_Current_ADC_TextBox.Text = "0";
                AddOrRemoveSectionItem(Batt_Current_StartOverSection, Batt_Current_ADC_TextBox, true);
                AddOrRemoveSectionItem(Batt_Current_StartOverSection, Batt_CurrentCalStartOverButton, true);
                AddOrRemoveSectionItem(Batt_Current_StartOverSection, Batt_Current_CAL_ReadADCButton, true);
                AddOrRemoveSectionItem(Batt_Current_StartOverSection, Batt_Current_CAL_SaveActionButton, false);
                AddOrRemoveSectionItem(Batt_Current_StartOverSection, Batt_Current_CAL_ActionButton, true);

                Batt_Current_CAL_ReadADCButton.IsEditable = true;
                Batt_Current_ADC_TextBox.IsEditable = true;//false
                Batt_Current_CAL_ActionButton.IsEditable = false;

                sortCalibrationSectionChildItems();
                return;
            }

            if (!BattViewQuantum.Instance.BattView_quickSerialNumberCheckStrict())
                return;
            AddOrRemoveSectionItem(Batt_Current_StartOverSection, Batt_Current_calibration_LowRange, false);

            if (item.Title == Batt_Current_CAL_ReadADCButton.Title)
            {
                bool allowCalculate = false;
                bool canReadMore = true;
                if (Batt_CurrentADCResultsPtr >= Batt_CurrentCalibrationPoints_MIN - 1)
                    allowCalculate = true;
                Batt_CurrentADCResultsPtr++;
                if (Batt_CurrentADCResultsPtr >= Batt_CurrentCalibrationPoints)
                    canReadMore = false;
                Batt_CurrentCalStep++;

                if (allowCalculate)
                {
                    Batt_Current_ADC_TextBox.Title = "Click \"Save\"";
                    if (canReadMore)

                        Batt_Current_ADC_TextBox.Title += ", you can send more for more accurate calibration";
                }
                else
                {
                    Batt_Current_ADC_TextBox.Title = "Change Current, then click \"Set another reading\"";
                }

                Batt_Current_ADC_TextBox.IsEditable = false;
                Batt_Current_CAL_ReadADCButton.IsEditable = false;

                if (!canReadMore)
                {
                    AddOrRemoveSectionItem(Batt_Current_StartOverSection, Batt_Current_ADC_TextBox, false);
                    AddOrRemoveSectionItem(Batt_Current_StartOverSection, Batt_Current_CAL_ReadADCButton, false);
                    AddOrRemoveSectionItem(Batt_Current_StartOverSection, Batt_Current_CAL_ActionButton, false);
                }
                else
                {
                    Batt_Current_CAL_ActionButton.IsEditable = true;
                }
                if (allowCalculate)
                {
                    AddOrRemoveSectionItem(Batt_Current_StartOverSection, Batt_Current_CAL_SaveActionButton, true);
                }

            }
            else
            {
                AddOrRemoveSectionItem(Batt_Current_StartOverSection, Batt_Current_ADC_TextBox, true);
                if (Batt_CurrentADCResultsPtr == 0)
                {
                    Batt_Current_ADC_TextBox.Text = "0";
                    Batt_Current_ADC_TextBox.IsEditable = false;
                }
                else
                {
                    Batt_Current_ADC_TextBox.Text = "";
                    Batt_Current_ADC_TextBox.IsEditable = true;
                }
                AddOrRemoveSectionItem(Batt_Current_StartOverSection, Batt_Current_CAL_ReadADCButton, true);
                Batt_Current_CAL_ReadADCButton.IsEditable = true;
                Batt_Current_CAL_ActionButton.IsEditable = false;

                Batt_Current_ADC_TextBox.Title = "Enter Current Value (Negative for In Use)";
            }
            sortCalibrationSectionChildItems();

        }

        void AddOrRemoveSectionItem(TableHeaderItem section, ListViewItem item, bool isAdd)
        {
            if (isAdd)
            {
                if (!section.Contains(item))
                {
                    section.Add(item);
                }
            }
            else
            {

                if (section.Contains(item))
                {
                    section.Remove(item);
                }
            }
        }
        void sortCalibrationSectionChildItems()
        {
            List<ListViewItem> sublist = (CalibrationCurrentItemSource[0] as IEnumerable).Cast<ListViewItem>().ToList();
            sublist = new List<ListViewItem>(sublist.OrderBy(o => o.Index));
            Batt_Current_StartOverSection.Clear();

            foreach (ListViewItem item in sublist)
            {
                Batt_Current_StartOverSection.Add(item);
            }

            RaisePropertyChanged(() => CalibrationCurrentItemSource);
        }

        async void Batt_Current_CAL_ReadADCButtonClick()
        {
            bool isVoltageOrNtc;
            bool isTemp = false;
            bool isPA = false;
            bool isPALowRange = false;
            float userVal = 0;
            if (!BattViewQuantum.Instance.BattView_quickSerialNumberCheckStrict())
            {
                return;
            }
            BattViewObject activeBattView = BattViewQuantum.Instance.GetBATTView();
            isTemp = false;
            isVoltageOrNtc = false;
            if (activeBattView.Config.enableHallEffectSensing != 0)
                isPA = true;
            if (isPA && Batt_Current_calibration_LowRange.IsSwitchEnabled)
                isPALowRange = true;
            if (!float.TryParse(Batt_Current_ADC_TextBox.Text, out userVal) ||
                Math.Abs(userVal) > 1000 ||
                isPALowRange && Math.Abs(float.Parse(Batt_Current_ADC_TextBox.Text)) > 75)
            {
                ACUserDialogs.ShowAlertWithTitleAndOkButton("Invalid Input Value");
                return;
            }
            for (int i = 0; i < Batt_CurrentADCResultsPtr; i++)
            {
                if (Math.Abs((double)userVal - (double)Batt_CurrentADC_userValues[i]) < 1)
                {
                    ACUserDialogs.ShowAlertWithTitleAndOkButton("Value is very close to previous value, please change");
                    return;
                }
            }
            Batt_CurrentADC_userValues[Batt_CurrentADCResultsPtr] = 10 * userVal;
            //do work arguments framing doing here
            List<object> arguments = new List<object>();
            arguments.Add(isVoltageOrNtc);
            arguments.Add(isTemp);
            arguments.Add(isPA);
            arguments.Add(isPALowRange);

            ACUserDialogs.ShowProgress();
            List<object> results = new List<object>();
            try
            {
                results = await BattViewQuantum.Instance.Batt_CalibrationCommunication(arguments);
                Batt_CalibrationCommunication_Click_doneWork(results);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Logger.AddLog(true, "X24" + ex.ToString());
            }
            ACUserDialogs.HideProgress();

        }
        //Batt_CalibrationCommunication_Click_doneWork functionality
        private void Batt_CalibrationCommunication_Click_doneWork(List<object> args)
        {
            List<object> genericlist = args;
            bool internalFailure = (bool)genericlist[0];
            string internalFailureString = (string)genericlist[1];
            CommunicationResult status = (CommunicationResult)genericlist[2];
            bool isvoltageOrNtc = (bool)genericlist[3];
            bool isTemp = (bool)genericlist[4];
            bool isPA = (bool)genericlist[5];
            bool isPALowRange = (bool)genericlist[6];
            float adcVal;

            if (internalFailure)
            {
                //if (!setSepcialStatus(StatusSpecialMessage.ERROR))
                //    setFormBusy(false);
                //Logger.addLog(true, "X17" + "Batt_CalibrationCommunication" + internalFailureString);
                return;
            }

            if (status != CommunicationResult.OK)
            {
                //if (!setStatus(status))
                //    setFormBusy(false);
                return;
            }
            if (!BattViewQuantum.Instance.BattView_quickSerialNumberCheckStrict())
            {
                return;
            }
            BattViewObject activeBattView = BattViewQuantum.Instance.GetBATTView();
            if (!isTemp)
            {
                if (isvoltageOrNtc)
                {
                    adcVal = activeBattView.battViewAdcRawObject.VoltageFiltered;
                    for (int i = 0; i < Batt_VoltageADCResultsPtr; i++)
                    {
                        if (Math.Abs((double)adcVal - (double)Batt_VoltageADCResults[i]) < 2)
                        {
                            ACUserDialogs.ShowAlertWithTitleAndOkButton("Value has been read is very close to previous one");
                            return;
                        }
                    }
                    Batt_VoltageADCResults[Batt_VoltageADCResultsPtr] = adcVal;
                    Batt_CAL_Voltage_ADCraw.Title = "RAW: " + adcVal.ToString();
                    Batt_Voltage_CAL_ActionButton_Click(Batt_Voltage_CAL_ReadADCButton);
                }
                else
                {
                    if (!isPA)
                        adcVal = activeBattView.battViewAdcRawObject.Current;
                    else if (isPALowRange)
                        adcVal = activeBattView.battViewAdcRawObject.ClampValueFiltered;
                    else
                        adcVal = activeBattView.battViewAdcRawObject.ClampValueChannel2tFiltered;


                    for (int i = 0; i < Batt_CurrentADCResultsPtr; i++)
                    {
                        if (Math.Abs((double)adcVal - (double)Batt_CurrentADCResults[i]) < 2)
                        {
                            ACUserDialogs.ShowAlertWithTitleAndOkButton("Current is very close to a previous value");
                            return;
                        }
                    }
                    if ((activeBattView.battViewAdcRawObject.IntercellNTCFiltered == 0 || activeBattView.battViewAdcRawObject.IntercellNTCFiltered == float.MaxValue) && activeBattView.Config.enableHallEffectSensing == 0)
                    {
                        ACUserDialogs.ShowAlertWithTitleAndOkButton("Intercell Temperature Sensor is damaged, Can't calibrate");
                        return;
                    }
                    Batt_CurrentADCResults[Batt_CurrentADCResultsPtr] = adcVal;
                    Batt_intercellTemp[Batt_CurrentADCResultsPtr] = (activeBattView.Config.intercellTemperatureCALa * (activeBattView.battViewAdcRawObject.IntercellNTCFiltered / activeBattView.battViewAdcRawObject.NtcRefrence_the10K) + activeBattView.Config.intercellTemperatureCALb);
                    Batt_CAL_Current_ADCraw.Title = "RAW: " + adcVal.ToString();
                    if (!isPA)
                    {
                        double resistor = Math.Log(Batt_intercellTemp[Batt_CurrentADCResultsPtr]);
                        resistor = ((1 / (activeBattView.Config.tempFa + activeBattView.Config.tempFb * resistor + activeBattView.Config.tempFc * Math.Pow(resistor, 3))) - 273.15f);
                        Batt_intercellTemp[Batt_CurrentADCResultsPtr] = (float)resistor;
                        Batt_CAL_Current_ADCraw.Title += "," + Batt_intercellTemp[Batt_CurrentADCResultsPtr].ToString();

                    }
                    Batt_CurrentCalibrationActionButton(Batt_Current_CAL_ReadADCButton);
                }
            }
            else
            {
                if (isvoltageOrNtc)
                {
                    if (activeBattView.battViewAdcRawObject.NtcBattery == float.MaxValue)
                    {
                        ACUserDialogs.ShowAlertWithTitleAndOkButton("NTC is not connected");
                        return;
                    }
                    double realR = activeBattView.battViewAdcRawObject.NtcBattery / activeBattView.battViewAdcRawObject.NtcBatteryRefFiltered * 10000;
                    //Batt_ntcCalATextBoxTOSAVE.Text = (realR / Batt_ntc_R).ToString();
                    //Batt_ntcCalBTextBoxTOSAVE.Text = "0";
                }
            }


        }
        #endregion

        #region Battview load , validation and saving
        void Batt_loadCurrentTab()
        {
            if (!BattViewQuantum.Instance.BattView_quickSerialNumberCheckStrict())
                return;

            BattViewObject activeBattView = BattViewQuantum.Instance.GetBATTView();

            Batt_Current_CAL_A_TextBox.Text = activeBattView.Config.currentCalA.ToString();
            Batt_Current_CAL_B_TextBox.Text = activeBattView.Config.currentCalB.ToString();

            Batt_Current_CAL_ClampA_TextBox.Text = activeBattView.Config.currentClampCalA.ToString();
            Batt_Current_CAL_ClampB_TextBox.Text = activeBattView.Config.currentClampCalB.ToString();
            Batt_Current_CAL_Clamp2A_TextBox.Text = activeBattView.Config.currentClamp2CalA.ToString();
            Batt_Current_CAL_Clamp2B_TextBox.Text = activeBattView.Config.currentClamp2CalB.ToString();

            Batt_validateCurrentTab();
        }

        bool Batt_validateCurrentTab()
        {
            if (!BattViewQuantum.Instance.BattView_quickSerialNumberCheckStrict())
            {
                return false;
            }
            verifyControl = new VerifyControl();
            verifyControl.VerifyFloatNumber(Batt_Current_CAL_B_TextBox, Batt_Current_CAL_B_TextBox, -2500, 2500);
            verifyControl.VerifyFloatNumber(Batt_Current_CAL_A_TextBox, Batt_Current_CAL_A_TextBox, -500, 500);
            verifyControl.VerifyFloatNumber(Batt_Current_CAL_ClampB_TextBox, Batt_Current_CAL_ClampB_TextBox, -50000, 50000);
            verifyControl.VerifyFloatNumber(Batt_Current_CAL_ClampA_TextBox, Batt_Current_CAL_ClampA_TextBox, -500, 500);
            verifyControl.VerifyFloatNumber(Batt_Current_CAL_Clamp2B_TextBox, Batt_Current_CAL_Clamp2B_TextBox, -50000, 50000);
            verifyControl.VerifyFloatNumber(Batt_Current_CAL_Clamp2A_TextBox, Batt_Current_CAL_Clamp2A_TextBox, -500, 500);
            return !verifyControl.HasErrors();
        }
        void Batt_saveIntoCurrentTab()
        {
            if (!BattViewQuantum.Instance.BattView_quickSerialNumberCheckStrict())
            {
                return;
            }
            BattViewQuantum.Instance.GetBATTView().Config.currentCalA = float.Parse(Batt_Current_CAL_A_TextBox.Text);
            BattViewQuantum.Instance.GetBATTView().Config.currentCalB = float.Parse(Batt_Current_CAL_B_TextBox.Text);

            BattViewQuantum.Instance.GetBATTView().Config.currentClampCalA = float.Parse(Batt_Current_CAL_ClampA_TextBox.Text);
            BattViewQuantum.Instance.GetBATTView().Config.currentClampCalB = float.Parse(Batt_Current_CAL_ClampB_TextBox.Text);

            BattViewQuantum.Instance.GetBATTView().Config.currentClamp2CalA = float.Parse(Batt_Current_CAL_Clamp2A_TextBox.Text);
            BattViewQuantum.Instance.GetBATTView().Config.currentClamp2CalB = float.Parse(Batt_Current_CAL_Clamp2B_TextBox.Text);
        }

        private int battViewCalibrationAccessApply()
        {
            if (ControlObject.UserAccess.Batt_Calibration == AccessLevelConsts.noAccess)
            {
                return 0;
            }
            UIAccessControlUtility accessControlUtility = new UIAccessControlUtility();
            BattViewObject activeBattView = BattViewQuantum.Instance.GetBATTView();
            //Batt_Calibration_manual (current)
            accessControlUtility.DoApplyAccessControlDayList(AccessLevelConsts.write, Batt_Current_calibration_LowRange, Batt_Current_StartOverSection);
            accessControlUtility.DoApplyAccessControlDayList(AccessLevelConsts.write, Batt_Current_ADC_TextBox, Batt_Current_StartOverSection);
            accessControlUtility.DoApplyAccessControlDayList(AccessLevelConsts.write, Batt_Current_CAL_ReadADCButton, Batt_Current_StartOverSection);
            accessControlUtility.DoApplyAccessControlDayList(AccessLevelConsts.write, Batt_Current_CAL_ActionButton, Batt_Current_StartOverSection);
            accessControlUtility.DoApplyAccessControlDayList(AccessLevelConsts.write, Batt_Current_CAL_SaveActionButton, Batt_Current_StartOverSection);
            accessControlUtility.DoApplyAccessControlDayList(AccessLevelConsts.write, Batt_CurrentCalStartOverButton, Batt_Current_StartOverSection);
            accessControlUtility.DoApplyAccessControlDayList(ControlObject.UserAccess.Batt_view_Raw_values, Batt_CAL_Current_ADCraw, Batt_Current_StartOverSection);

            accessControlUtility.DoApplyAccessControlDayList(ControlObject.UserAccess.Batt_Calibration_manual, Batt_Current_CAL_A_TextBox, Batt_Current_CalSection);
            accessControlUtility.DoApplyAccessControlDayList(ControlObject.UserAccess.Batt_Calibration_manual, Batt_Current_CAL_B_TextBox, Batt_Current_CalSection);


            accessControlUtility.DoApplyAccessControlDayList(ControlObject.UserAccess.Batt_Calibration_manual, Batt_Current_CAL_ClampA_TextBox, Batt_Current_DirectSaveSection);
            accessControlUtility.DoApplyAccessControlDayList(ControlObject.UserAccess.Batt_Calibration_manual, Batt_Current_CAL_ClampB_TextBox, Batt_Current_DirectSaveSection);
            accessControlUtility.DoApplyAccessControlDayList(ControlObject.UserAccess.Batt_Calibration_manual, Batt_Current_CAL_Clamp2A_TextBox, Batt_Current_DirectSaveSection);
            accessControlUtility.DoApplyAccessControlDayList(ControlObject.UserAccess.Batt_Calibration_manual, Batt_Current_CAL_Clamp2B_TextBox, Batt_Current_DirectSaveSection);
            accessControlUtility.DoApplyAccessControlDayList(ControlObject.UserAccess.Batt_Calibration_manual, Batt_CurrentCalibrationDirectSave, Batt_Current_DirectSaveSection);

            if (activeBattView.Config.enableHallEffectSensing != 0)
            {
                AddOrRemoveSectionItem(Batt_Current_CalSection, Batt_Current_CAL_A_TextBox, false);
                AddOrRemoveSectionItem(Batt_Current_CalSection, Batt_Current_CAL_B_TextBox, false);

                AddOrRemoveSectionItem(Batt_Current_DirectSaveSection, Batt_Current_CAL_ClampA_TextBox, true);
                AddOrRemoveSectionItem(Batt_Current_DirectSaveSection, Batt_Current_CAL_ClampB_TextBox, true);

                AddOrRemoveSectionItem(Batt_Current_DirectSaveSection, Batt_Current_CAL_Clamp2A_TextBox, true);
                AddOrRemoveSectionItem(Batt_Current_DirectSaveSection, Batt_Current_CAL_Clamp2B_TextBox, true);

                AddOrRemoveSectionItem(Batt_Current_StartOverSection, Batt_Current_calibration_LowRange, true);

            }
            else
            {
                AddOrRemoveSectionItem(Batt_Current_CalSection, Batt_Current_CAL_A_TextBox, true);
                AddOrRemoveSectionItem(Batt_Current_CalSection, Batt_Current_CAL_B_TextBox, true);

                AddOrRemoveSectionItem(Batt_Current_DirectSaveSection, Batt_Current_CAL_ClampA_TextBox, false);
                AddOrRemoveSectionItem(Batt_Current_DirectSaveSection, Batt_Current_CAL_ClampB_TextBox, false);

                AddOrRemoveSectionItem(Batt_Current_DirectSaveSection, Batt_Current_CAL_Clamp2A_TextBox, false);
                AddOrRemoveSectionItem(Batt_Current_DirectSaveSection, Batt_Current_CAL_Clamp2B_TextBox, false);

                AddOrRemoveSectionItem(Batt_Current_StartOverSection, Batt_Current_calibration_LowRange, false);
            }

            return accessControlUtility.GetVisibleCount();
        }

        private int battViewCalibrationAccessApply_voltage()
        {
            if (ControlObject.UserAccess.Batt_Calibration == AccessLevelConsts.noAccess)
            {
                return 0;
            }

            UIAccessControlUtility accessControlUtility = new UIAccessControlUtility();
            //BattView_Object activeBattView = BATTQuantum.Instance.GetBATTView();
            //Batt_Calibration_manual (Voltage)
            //accessControlUtility.doApplyAccessControl(ControlObject.user_access.Batt_Calibration_manual, Batt_Voltage_CAL_ALabel, null);
            //accessControlUtility.doApplyAccessControl(ControlObjec/'t.user_access.Batt_Calibration_manual, Batt_Voltage_CAL_BLabel, null);


            accessControlUtility.DoApplyAccessControlDayList(AccessLevelConsts.write, Batt_Voltage_ADC_TextBox, Batt_Voltage_StartOverSection);
            accessControlUtility.DoApplyAccessControlDayList(AccessLevelConsts.write, Batt_Voltage_CAL_ReadADCButton, Batt_Voltage_StartOverSection);
            accessControlUtility.DoApplyAccessControlDayList(AccessLevelConsts.write, Batt_Voltage_CAL_ActionButton, Batt_Voltage_StartOverSection);
            accessControlUtility.DoApplyAccessControlDayList(AccessLevelConsts.write, Batt_Voltage_CAL_SaveActionButton, Batt_Voltage_StartOverSection);
            accessControlUtility.DoApplyAccessControlDayList(AccessLevelConsts.write, Batt_VoltageCalStartOverButton, Batt_Voltage_StartOverSection);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_view_Raw_values, Batt_CAL_Voltage_ADCraw, Batt_Voltage_StartOverSection);

            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_Calibration_manual, Batt_Voltage_CAL_A_TextBox, Batt_Voltage_DirectSaveSection);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_Calibration_manual, Batt_Voltage_CAL_B_TextBox, Batt_Voltage_DirectSaveSection);
            accessControlUtility.DoApplyAccessControl(ControlObject.UserAccess.Batt_Calibration_manual, Batt_VoltageCalibrationDirectSave, Batt_Voltage_DirectSaveSection);

            return accessControlUtility.GetVisibleCount();
        }

        #endregion

        void createDataforCharger()
        {
            //Voltage tab data source for charger connection.
            calibrationVoltageItemSource.Clear();
            MCB_Voltage_StartOverSection = new TableHeaderItem();
            MCB_Voltage_DirectSaveSection = new TableHeaderItem();

            MCB_VoltageCalibrationRange = new ListViewItem
            {
                Index = 0,
                Title = AppResources.voltage_calibration_range,
                DefaultCellType = ACUtility.CellTypes.ListSelector,
                EditableCellType = ACUtility.CellTypes.ListSelector,
                ListSelectionCommand = ListSelectorCommand,
                SelectedIndex = -1,
                IsEditable = true
            };

            MCB_VoltageCalibrationRange.Items = new List<object>();
            MCB_VoltageCalibrationRange.Items.Add(AppResources.low_range);
            MCB_VoltageCalibrationRange.Items.Add(AppResources.high_range);

            MCB_voltage_ADC_TextBox = new ListViewItem
            {
                Index = 1,
                Title = AppResources.voltage,
                DefaultCellType = ACUtility.CellTypes.LabelTextEdit,
                EditableCellType = ACUtility.CellTypes.LabelTextEdit,
                IsEditable = true
            };

            MCB_voltage_CAL_ReadADCButton = new ListViewItem
            {
                Index = 2,
                Title = AppResources.read_the_value_of,
                DefaultCellType = ACUtility.CellTypes.Button,
                EditableCellType = ACUtility.CellTypes.Button,
                ListSelectionCommand = ButtonSelectorCommand,
                IsEditable = true
            };

            MCB_voltage_CAL_ActionButton = new ListViewItem
            {
                Index = 3,
                Title = AppResources.set_another_reading,
                DefaultCellType = ACUtility.CellTypes.Button,
                EditableCellType = ACUtility.CellTypes.Button,
                ListSelectionCommand = ButtonSelectorCommand,
                IsEditable = true,
                KeepIndex = 0
            };


            MCB_voltage_CAL_SaveActionButton = new ListViewItem
            {
                Index = 4,
                Title = AppResources.calculate_and_save,
                DefaultCellType = ACUtility.CellTypes.Button,
                EditableCellType = ACUtility.CellTypes.Button,
                ListSelectionCommand = ButtonSelectorCommand,
                IsEditable = false,
                IsVisible = false

            };
            MCB_VoltageCalStartOverButton = new ListViewItem
            {
                Index = 5,
                Title = AppResources.start_over,
                DefaultCellType = ACUtility.CellTypes.Button,
                EditableCellType = ACUtility.CellTypes.Button,
                ListSelectionCommand = ButtonSelectorCommand,
                IsEditable = true
            };

            MCB_CAL_ADC_Raw = new ListViewItem
            {
                Index = 6,
                Title = AppResources.raw,
                DefaultCellType = ACUtility.CellTypes.Default,
                EditableCellType = ACUtility.CellTypes.Default,
                IsEditable = true
            };

            MCB_CALA_TextBox = new ListViewItem
            {
                Title = AppResources.a_up_range,
                DefaultCellType = ACUtility.CellTypes.LabelTextEdit,
                EditableCellType = ACUtility.CellTypes.LabelTextEdit,
                //IsEditable = true,
                Index = 0

            };

            MCB_CALA_LOW_TextBox = new ListViewItem
            {
                Index = 1,
                Title = AppResources.a_low_range,
                DefaultCellType = ACUtility.CellTypes.LabelTextEdit,
                EditableCellType = ACUtility.CellTypes.LabelTextEdit,
                //IsEditable = true

            };

            MCB_CALB_TextBox = new ListViewItem
            {
                Index = 2,
                Title = AppResources.b_up_range,
                DefaultCellType = ACUtility.CellTypes.LabelTextEdit,
                EditableCellType = ACUtility.CellTypes.LabelTextEdit,
                //IsEditable = true
            };

            MCB_CALB_LOW_TextBox = new ListViewItem
            {
                Index = 3,
                Title = AppResources.b_low_range,
                DefaultCellType = ACUtility.CellTypes.LabelTextEdit,
                EditableCellType = ACUtility.CellTypes.LabelTextEdit,
                //IsEditable = true

            };

            MCB_voltageCalibrationDirectSave = new ListViewItem
            {
                Index = 4,
                Title = AppResources.save,
                DefaultCellType = ACUtility.CellTypes.Button,
                EditableCellType = ACUtility.CellTypes.Button,
                ListSelectionCommand = ButtonSelectorCommand,
                IsEditable = true
            };

            CalibrationVoltageItemSource.Add(MCB_Voltage_StartOverSection);
            CalibrationVoltageItemSource.Add(MCB_Voltage_DirectSaveSection);

            //End voltage tab data

            //Start loading data from charger through a call.
            try
            {
                MCB_loadCalibrationValues();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Logger.AddLog(true, "X25" + ex.ToString());
            }
            if (chargerCalibrationAccessApply() == 0)
            {
                ACUserDialogs.ShowAlert(AppResources.no_data_found);
                if (DevicePlatform == Plugin.DeviceInfo.Abstractions.Platform.iOS)
                {
                    ShowViewModel<CalibrationViewModel>(new { pop = "pop" });
                }
                return;
            }
            else
            {
                MCB_voltageCalibrationActionButton(null);
            }
        }

        void SortChargerVoltageSectionChildItems()
        {
            List<ListViewItem> sublist = (MCB_Voltage_StartOverSection as IEnumerable).Cast<ListViewItem>().ToList();
            sublist = new List<ListViewItem>(sublist.OrderBy(o => o.Index));
            MCB_Voltage_StartOverSection.Clear();

            foreach (ListViewItem item in sublist)
            {
                MCB_Voltage_StartOverSection.Add(item);
            }
            RaisePropertyChanged(() => CalibrationVoltageItemSource);
        }

        void MCB_loadCalibrationValues()
        {
            MCBobject currentMcb = MCBQuantum.Instance.GetMCB();
            if (!MCBQuantum.Instance.MCB_quickSerialNumberCheckStrict())
                return;
            MCB_CALA_TextBox.Text = currentMcb.Config.voltageCalA.ToString();
            MCB_CALB_TextBox.Text = currentMcb.Config.voltageCalB.ToString();
            MCB_CALA_LOW_TextBox.Text = currentMcb.Config.voltageCalALow.ToString();
            MCB_CALB_LOW_TextBox.Text = currentMcb.Config.voltageCalBLow.ToString();

            if (MCB_VoltageCalibrationRange.SelectedIndex == -1)
            {
                MCB_VoltageCalibrationRange.Text = MCB_VoltageCalibrationRange.SelectedItem = MCB_VoltageCalibrationRange.Items[1].ToString();
            }

            MCB_VerifyVoltageCalibration();
        }

        async Task MCB_SaveIntoVoltageCalibration()
        {
            if (NetworkCheck())
            {
                if (MCB_VerifyVoltageCalibration())
                {
                    ACUserDialogs.ShowProgress();
                    McbCommunicationTypes caller = McbCommunicationTypes.NOCall;
                    bool arg1 = false;
                    try
                    {
                        MCB_SaveIntoVoltage();
                        caller = McbCommunicationTypes.saveConfig;
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
                        results = await MCBQuantum.Instance.CommunicateMCB(arguments);
                        if (results.Count > 0)
                        {
                            var status = (CommunicationResult)results[2];
                            if (status == CommunicationResult.OK)
                            {

                                try
                                {
                                    ResetOldData();
                                    MCB_loadCalibrationValues();
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
                    ACUserDialogs.ShowAlert(AppResources.alert_enter_valid + "\n" + verifyControl.GetErrorString());
                }
            }
        }

        void ResetOldData()
        {
            foreach (var item in calibrationVoltageItemSource)
            {
                List<ListViewItem> sublist = (item as IEnumerable).Cast<ListViewItem>().ToList();
                foreach (var childItem in sublist)
                {
                    childItem.SubTitle = string.Empty;
                }
            }
        }

        bool MCB_VerifyVoltageCalibration()
        {
            if (!MCBQuantum.Instance.MCB_quickSerialNumberCheckStrict())
                return false;
            verifyControl = new VerifyControl();//MCB_V_CAL_ALabel
            verifyControl.VerifyFloatNumber(MCB_CALA_TextBox, MCB_CALA_TextBox, -500, 500);
            verifyControl.VerifyFloatNumber(MCB_CALB_TextBox, MCB_CALB_TextBox, 0.00001f, 2000.0f);
            verifyControl.VerifyFloatNumber(MCB_CALA_LOW_TextBox, MCB_CALA_LOW_TextBox, -500, 500);
            verifyControl.VerifyFloatNumber(MCB_CALB_LOW_TextBox, MCB_CALB_LOW_TextBox, 0.00001f, 2000.0f);
            return !verifyControl.HasErrors();
        }

        void MCB_SaveIntoVoltage()
        {
            MCBobject currentMcb = MCBQuantum.Instance.GetMCB();
            if (!MCBQuantum.Instance.MCB_quickSerialNumberCheckStrict())
                return;
            currentMcb.Config.voltageCalA = float.Parse(MCB_CALA_TextBox.Text);
            currentMcb.Config.voltageCalB = float.Parse(MCB_CALB_TextBox.Text);
            currentMcb.Config.voltageCalALow = float.Parse(MCB_CALA_LOW_TextBox.Text);
            currentMcb.Config.voltageCalBLow = float.Parse(MCB_CALB_LOW_TextBox.Text);
        }

        private int chargerCalibrationAccessApply()
        {
            UIAccessControlUtility accessControlUtility = new UIAccessControlUtility();
            if (ControlObject.UserAccess.MCB_Calibration == AccessLevelConsts.noAccess)
            {
                //MCB_CalibrationButton.Hide();
                return 0;
            }
            accessControlUtility.DoApplyAccessControlDayList(AccessLevelConsts.write, MCB_VoltageCalibrationRange, MCB_Voltage_StartOverSection);
            accessControlUtility.DoApplyAccessControlDayList(ControlObject.UserAccess.MCB_view_Raw_values, MCB_CAL_ADC_Raw, MCB_Voltage_StartOverSection);
            accessControlUtility.DoApplyAccessControlDayList(AccessLevelConsts.write, MCB_voltage_CAL_ActionButton, MCB_Voltage_StartOverSection);

            accessControlUtility.DoApplyAccessControlDayList(ControlObject.UserAccess.MCB_Calibration_manual, MCB_CALA_TextBox, MCB_Voltage_DirectSaveSection);
            MCB_CALA_TextBox.IsEditable = MCB_CALA_TextBox.IsEditEnabled;
            accessControlUtility.DoApplyAccessControlDayList(ControlObject.UserAccess.MCB_Calibration_manual, MCB_CALB_TextBox, MCB_Voltage_DirectSaveSection);
            MCB_CALB_TextBox.IsEditable = MCB_CALB_TextBox.IsEditEnabled;
            accessControlUtility.DoApplyAccessControlDayList(ControlObject.UserAccess.MCB_Calibration_manual, MCB_CALA_LOW_TextBox, MCB_Voltage_DirectSaveSection);
            MCB_CALA_LOW_TextBox.IsEditable = MCB_CALA_LOW_TextBox.IsEditEnabled;
            accessControlUtility.DoApplyAccessControlDayList(ControlObject.UserAccess.MCB_Calibration_manual, MCB_CALB_LOW_TextBox, MCB_Voltage_DirectSaveSection);
            MCB_CALB_LOW_TextBox.IsEditable = MCB_CALB_LOW_TextBox.IsEditEnabled;
            accessControlUtility.DoApplyAccessControlDayList(AccessLevelConsts.write, MCB_voltageCalibrationDirectSave, MCB_Voltage_DirectSaveSection);

            accessControlUtility.DoApplyAccessControlDayList(AccessLevelConsts.write, MCB_voltage_ADC_TextBox, MCB_Voltage_StartOverSection);
            accessControlUtility.DoApplyAccessControlDayList(AccessLevelConsts.write, MCB_voltage_CAL_ReadADCButton, MCB_Voltage_StartOverSection);
            accessControlUtility.DoApplyAccessControlDayList(AccessLevelConsts.write, MCB_voltage_CAL_SaveActionButton, MCB_Voltage_StartOverSection);
            accessControlUtility.DoApplyAccessControlDayList(AccessLevelConsts.write, MCB_VoltageCalStartOverButton, MCB_Voltage_StartOverSection);

            if (ControlObject.UserAccess.MCB_Calibration_manual != AccessLevelConsts.write)
            {
                AddOrRemoveSectionItem(MCB_Voltage_DirectSaveSection, MCB_voltageCalibrationDirectSave, false);

            }
            return 1;
        }

        /// <summary>
        /// Gets the list selector command.
        /// </summary>
        /// <value>The list selector command.</value>
        public IMvxCommand ButtonSelectorCommand
        {
            get { return new MvxCommand<ListViewItem>(ExecuteButtonSelectorCommand); }
        }


        private async void ExecuteButtonSelectorCommand(ListViewItem item)
        {
            if (IsBattView)
            {
                if (SelectedIndex == 0)//Current
                {
                    if (item.Title == AppResources.direct_save)
                    {
                        await BattViewCurrentDirectSave();//direct save for battview current
                    }
                    else if (item.Title == AppResources.save_new_calibration)
                    {
                        Batt_Current_CAL_SaveActionButton_Click();
                    }
                    else if (item.Title == AppResources.send_current)//send current
                    {
                        Batt_Current_CAL_ReadADCButtonClick();//
                    }
                    else if (item.Title == AppResources.set_another_reading)//current cal action button
                    {
                        Batt_CurrentCalibrationActionButton(Batt_Current_CAL_ActionButton);//
                    }
                    else if (item.Title == AppResources.start_over)//start over
                    {
                        Batt_CurrentCalibrationActionButton(Batt_CurrentCalStartOverButton);
                    }
                }
                else if (SelectedIndex == 1)//Voltage
                {
                    if (item.Title == Batt_Voltage_CAL_ActionButton.Title)//set another reading
                    {
                        Batt_Voltage_CAL_ActionButton_Click(item);
                    }
                    if (item.Title == Batt_Voltage_CAL_ReadADCButton.Title)//read the value of
                    {
                        Batt_CalibrationCommunication_Click(item);
                    }
                    else if (item.Title == Batt_VoltageCalStartOverButton.Title)//start over
                    {
                        Batt_Voltage_CAL_ActionButton_Click(item);
                    }
                    else if (item.Title == Batt_Voltage_CAL_SaveActionButton.Title)//calculate and save
                    {
                        Batt_Voltage_CAL_SaveActionButton_Click(item);
                    }
                    else if (item.Title == Batt_VoltageCalibrationDirectSave.Title)//save
                    {
                        BATT_simpleCommunicationButtonAction(item);
                    }
                }
                else //set SOC button.
                {
                    BATT_simpleCommunicationButtonAction(Batt_setSOCButton);
                    //BATT_simpleCommunicationButtonAction(item);
                }
            }
            else
            {
                if (item.Title == AppResources.save)
                {
                    await MCB_SaveIntoVoltageCalibration();
                }
                else if (item.Title == AppResources.set_another_reading)
                {
                    MCB_voltageCalibrationActionButton(item);
                }
                else if (item.Title == AppResources.calculate_and_save)
                {
                    MCB_voltage_CAL_SaveActionButton_Click(item);
                }
                else if (item.Title == AppResources.read_the_value_of)
                {
                    MCB_CalibrationCommunication_Click(item);
                }
                else if (item.Title == AppResources.start_over)
                {
                    MCB_voltageCalibrationActionButton(item);
                }
            }

        }

        async private void BATT_simpleCommunicationButtonAction(ListViewItem item)
        {
            await BATT_simpleCommunicationButtonActionInternal(item);
        }

        async Task BATT_simpleCommunicationButtonActionInternal(ListViewItem item)
        {
            ACUserDialogs.ShowProgress();
            BattViewCommunicationTypes caller = BattViewCommunicationTypes.NOCall;
            object arg1 = null;
            string msg = "Invalid Input";
            if (!BattViewQuantum.Instance.BattView_quickSerialNumberCheckStrict())
                return;
            byte tempByte = 0;

            if (item.Title == Batt_setSOCButton.Title)
            {
                if (byte.TryParse(Batt_overrideSOC_TextBox.Text, out tempByte) && tempByte <= 100 && tempByte >= 0)
                {
                    arg1 = tempByte;
                    caller = BattViewCommunicationTypes.setSOC;
                }
            }
            else
                if (item.Title == AppResources.reset_voltage_cal)
            {
                //MessageBoxForm mb = new MessageBoxForm();
                //mb.render("By clicking OK, the software will reset voltage calibration to the default values.", MessageBoxFormTypes.info, MessageBoxFormButtons.OkCancel);
                //if (MessageBoxForm.result != DialogResult.OK)
                //    return;
                Batt_Voltage_CAL_A_TextBox.Text = "0.418002";
                Batt_Voltage_CAL_B_TextBox.Text = "-18.65123";
                if (Batt_validateVoltageTab())
                {
                    Batt_saveIntoVoltageTab();
                    arg1 = false;
                    caller = BattViewCommunicationTypes.saveConfig;
                }
            }

            else if (item.Title == Batt_VoltageCalibrationDirectSave.Title)
            {
                if (Batt_validateVoltageTab())
                {
                    Batt_saveIntoVoltageTab();
                    arg1 = false;
                    caller = BattViewCommunicationTypes.saveConfig;
                }
            }
            else if (item.Title == Batt_Voltage_CAL_SaveActionButton.Title)
            {
                if (Batt_validateVoltageTab())
                {
                    Batt_saveIntoVoltageTab();
                    caller = BattViewCommunicationTypes.Batt_voltagecalibrationSaveConfig;
                }
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
                        var status = (CommunicationResult)results[2];
                        if (status == CommunicationResult.OK)
                        {
                            if (item.Title == Batt_VoltageCalibrationDirectSave.Title)
                            {
                                Batt_loadVoltageTab();
                                Batt_Voltage_CAL_ActionButton_Click(item);
                            }
                            else if (item.Title == Batt_Voltage_CAL_SaveActionButton.Title)
                            {
                                Batt_Voltage_CAL_ActionButton_Click(item);
                            }
                            else if (item.Title == AppResources.reset_voltage_cal)
                            {
                                //TODO: start Scanning for devices 
                                //scanRelated_prepare(scanRelatedTypes.doScan);
                            }
                            else if (item.Title == Batt_setSOCButton.Title)
                            {
                                ACUtility.SetStatus(status);
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

            }
            else
            {
                if (!string.IsNullOrEmpty(verifyControl.GetErrorString()))
                {
                    ACUserDialogs.ShowAlertWithTitleAndOkButton(AppResources.alert_enter_valid + "\n" + verifyControl.GetErrorString());
                }
                else
                {
                    ACUserDialogs.ShowAlert(msg);
                }
                //MessageBoxForm mb = new MessageBoxForm();
                //mb.render(msg, MessageBoxFormTypes.Warning, MessageBoxFormButtons.OK);
            }

            ACUserDialogs.HideProgress();
        }



        private void MCB_voltageCalibrationActionButton(ListViewItem item)
        {
            if (item == null || item.Title == AppResources.start_over)
            {
                MCB_voltageCalStep = 0;
                AddOrRemoveSectionItem(MCB_Voltage_StartOverSection, MCB_VoltageCalibrationRange, true);
                MCB_voltage_ADC_TextBox.Title = "Choose Input type";
                AddOrRemoveSectionItem(MCB_Voltage_StartOverSection, MCB_voltage_ADC_TextBox, false);

                AddOrRemoveSectionItem(MCB_Voltage_StartOverSection, MCB_VoltageCalStartOverButton, false);
                AddOrRemoveSectionItem(MCB_Voltage_StartOverSection, MCB_voltage_CAL_ReadADCButton, false);
                AddOrRemoveSectionItem(MCB_Voltage_StartOverSection, MCB_voltage_CAL_SaveActionButton, false);
                AddOrRemoveSectionItem(MCB_Voltage_StartOverSection, MCB_voltage_CAL_ActionButton, true);

                MCB_voltage_ADC_TextBox.Text = "";
                MCB_voltage_CAL_ReadADCButton.IsEditable = false;
                MCB_voltage_ADC_TextBox.IsEditable = false;
                MCB_voltage_CAL_ActionButton.IsEditable = true;
                //MCB_voltage_CAL_ActionButton.Text = "Set another reading";
                SortChargerVoltageSectionChildItems();

                return;
            }

            if (!MCBQuantum.Instance.MCB_quickSerialNumberCheckStrict())
                return;
            if (MCB_voltageCalStep == 0)
            {
                if (CalibrationVoltageItemSource[0][item.KeepIndex].Text == AppResources.low_range)
                {
                    MCB_voltageCalibrationisLowRange = true;
                    //Set resistor value
                }
                else if (CalibrationVoltageItemSource[0][item.KeepIndex].Text == AppResources.high_range)
                {
                    MCB_voltageCalibrationisLowRange = false;
                }
                else
                {
                    ACUserDialogs.ShowAlert("Please choose a range");
                    return;
                }
                MCB_voltageADCResultsPtr = 0;
                MCB_voltageCalStep++;
                AddOrRemoveSectionItem(MCB_Voltage_StartOverSection, MCB_VoltageCalibrationRange, false);
                MCB_voltage_ADC_TextBox.Text = "Enter Voltage Value";
                AddOrRemoveSectionItem(MCB_Voltage_StartOverSection, MCB_voltage_CAL_ReadADCButton, true);
                MCB_voltage_ADC_TextBox.Text = "";
                AddOrRemoveSectionItem(MCB_Voltage_StartOverSection, MCB_voltage_ADC_TextBox, true);
                AddOrRemoveSectionItem(MCB_Voltage_StartOverSection, MCB_VoltageCalStartOverButton, true);
                AddOrRemoveSectionItem(MCB_Voltage_StartOverSection, MCB_voltage_CAL_SaveActionButton, false);
                MCB_voltage_CAL_ReadADCButton.IsEditable = true;
                MCB_voltage_ADC_TextBox.IsEditable = true;
                MCB_voltage_CAL_ActionButton.IsEditable = false;
                //MCB_voltage_CAL_ActionButton.Text = "Set another reading";

            }
            else if (item.Title == AppResources.read_the_value_of)
            {

                bool allowCalculate = false;
                bool canReadMore = true;
                if (MCB_voltageADCResultsPtr >= MCB_voltageCalibrationPoints_MIN - 1)
                    allowCalculate = true;
                MCB_voltageADCResultsPtr++;
                if (MCB_voltageADCResultsPtr >= MCB_voltageCalibrationPoints)
                    canReadMore = false;
                MCB_voltageCalStep++;

                //Call method to calculate RAW value.

                //MCB_CalibrationCommunication_Click(item);
                //End of method calling to calculate RAW value.
                //This section of code is not useful at this point of time as it does not have any tip label. 
                if (allowCalculate)
                {

                    MCB_voltage_ADC_TextBox.Title = "Click Calculate to calibrate";
                    if (canReadMore)
                    {
                        MCB_voltage_ADC_TextBox.Title += ", you can read more for more accurate calibration";
                    }
                }
                else
                {
                    MCB_voltage_ADC_TextBox.Title = "Click Next to continue";
                }
                MCB_voltage_ADC_TextBox.IsEditable = false;
                MCB_voltage_CAL_ReadADCButton.IsEditable = false;

                if (!canReadMore)
                {
                    AddOrRemoveSectionItem(MCB_Voltage_StartOverSection, MCB_voltage_ADC_TextBox, false);
                    AddOrRemoveSectionItem(MCB_Voltage_StartOverSection, MCB_voltage_CAL_ReadADCButton, false);
                    AddOrRemoveSectionItem(MCB_Voltage_StartOverSection, MCB_voltage_CAL_ActionButton, false);
                }
                else
                {
                    MCB_voltage_CAL_ActionButton.IsEditable = true;
                }
                if (allowCalculate)
                {
                    AddOrRemoveSectionItem(MCB_Voltage_StartOverSection, MCB_voltage_CAL_SaveActionButton, true);
                }

            }
            else
            {
                AddOrRemoveSectionItem(MCB_Voltage_StartOverSection, MCB_voltage_ADC_TextBox, true);
                AddOrRemoveSectionItem(MCB_Voltage_StartOverSection, MCB_voltage_CAL_ReadADCButton, true);
                MCB_voltage_ADC_TextBox.IsEditable = true;
                MCB_voltage_CAL_ReadADCButton.IsEditable = true;
                MCB_voltage_CAL_ActionButton.IsEditable = false;
                MCB_voltage_ADC_TextBox.Text = "";
                MCB_voltage_ADC_TextBox.Title = "Enter Voltage Value";
            }
            SortChargerVoltageSectionChildItems();
        }

        private void MCB_CalibrationCommunication_Click(ListViewItem item)
        {
            bool isTemperatureCalibration;
            object arg1 = null;
            float userVal = 0;
            if (!MCBQuantum.Instance.MCB_quickSerialNumberCheckStrict())
                return;

            isTemperatureCalibration = false;
            if (!float.TryParse(MCB_voltage_ADC_TextBox.Text, out userVal) || userVal < 0 || userVal > 110)
            {
                ACUserDialogs.ShowAlert("Invalid Input value");
                return;
            }
            for (int i = 0; i < MCB_voltageADCResultsPtr; i++)
            {
                if (Math.Abs((double)userVal - (double)MCB_voltageADC_userValues[i]) < 1)
                {
                    ACUserDialogs.ShowAlert("Value is very close to previous value, please change");
                    return;
                }
            }
            MCB_voltageADC_userValues[MCB_voltageADCResultsPtr] = 100 * userVal;
            arg1 = MCB_voltageCalibrationisLowRange;

            List<object> arguments = new List<object>();
            arguments.Add(isTemperatureCalibration);
            arguments.Add(arg1);
            MCB_CalibrationCommunication_Click_prepare(arguments);

        }
        private async void MCB_CalibrationCommunication_Click_prepare(List<object> arguments)
        {
            ACUserDialogs.ShowProgress();
            List<object> results = new List<object>();
            try
            {
                results = await MCBQuantum.Instance.MCB_CalibrationCommunication_Click_doWork(arguments);

                List<object> genericlist = results as List<object>;
                bool internalFailure = (bool)genericlist[0];
                string internalFailureString = (string)genericlist[1];
                CommunicationResult status = (CommunicationResult)genericlist[2];
                bool isTemperatureCalibration = (bool)genericlist[3];
                ushort adcVal = (ushort)genericlist[4];

                if (internalFailure)
                {
                    //if (!setSepcialStatus(StatusSpecialMessage.ERROR))
                    //	setFormBusy(false);
                    //Logger.addLog(true, "X91" + internalFailureString);
                    ACUserDialogs.HideProgress();
                    return;
                }



                if (status != CommunicationResult.OK)
                {
                    //if (!setStatus((status)))
                    //	setFormBusy(false);
                    ACUserDialogs.HideProgress();
                    return;
                }
                //setFormBusy(false);

                for (int i = 0; i < MCB_voltageADCResultsPtr; i++)
                {
                    if (Math.Abs((double)adcVal - (double)MCB_voltageADCResults[i]) < 2)
                    {
                        ACUserDialogs.ShowAlert("Value has been read is very close to previous one");
                        //MessageBoxForm mb = new MessageBoxForm();
                        //mb.render("Value has been read is very close to previous one", MessageBoxFormTypes.info, MessageBoxFormButtons.OK);
                        ACUserDialogs.HideProgress();
                        return;
                    }
                }
                MCB_voltageADCResults[MCB_voltageADCResultsPtr] = adcVal;
                MCB_CAL_ADC_Raw.Title = MCB_CAL_ADC_Raw.Title = "RAW: " + adcVal.ToString();
                MCB_voltageCalibrationActionButton(MCB_voltage_CAL_ReadADCButton);

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            ACUserDialogs.HideProgress();
        }



        private void MCB_voltage_CAL_SaveActionButton_Click(ListViewItem item)
        {
            if (!MCBQuantum.Instance.MCB_quickSerialNumberCheckStrict())
                return;
            try
            {
                float YSum = 0;
                float XSum = 0;
                float XXSum = 0;
                float XYSum = 0;
                int n = MCB_voltageADCResultsPtr;
                for (int i = 0; i < MCB_voltageADCResultsPtr; i++)
                {
                    YSum += MCB_voltageADC_userValues[i];
                    XSum += MCB_voltageADCResults[i];
                    XXSum += MCB_voltageADCResults[i] * MCB_voltageADCResults[i];
                    XYSum += MCB_voltageADC_userValues[i] * MCB_voltageADCResults[i];

                }
                float calA = (float)((YSum * XXSum) - (XSum * XYSum)) / ((n * XXSum) - (XSum * XSum));
                float calB = (float)((n * XYSum) - (XSum * YSum)) / ((n * XXSum) - (XSum * XSum));
                //check statndard error estimation
                double YYDiffSum = 0;
                for (int i = 0; i < MCB_voltageADCResultsPtr; i++)
                {

                    YYDiffSum += Math.Pow((double)MCB_voltageADC_userValues[i] - (double)(calB * MCB_voltageADCResults[i] + calA), 2);
                }
                YYDiffSum = Math.Pow(YYDiffSum, 0.5) / MCB_voltageADCResultsPtr;
                if (YYDiffSum / YSum > 0.0005)
                {
                    ACUserDialogs.ShowAlert("Calibration values caused calibration to fail");
                    //MessageBoxForm mb = new MessageBoxForm();
                    //mb.render("Calibration values caused calibration to fail", MessageBoxFormTypes.error, MessageBoxFormButtons.OK);
                    MCB_voltageCalibrationActionButton(null);
                    return;
                }
                if (MCB_voltageCalibrationisLowRange)
                {
                    MCB_CALA_LOW_TextBox.Text = calA.ToString();
                    MCB_CALB_LOW_TextBox.Text = calB.ToString();
                }
                else
                {
                    MCB_CALA_TextBox.Text = calA.ToString();
                    MCB_CALB_TextBox.Text = calB.ToString();
                }
            }
            catch (Exception ex)
            {
                //Logger.addLog(true, "X90" + ex.ToString());
                return;
            }
            MCB_simpleCommunicationButtonAction(MCB_voltage_CAL_SaveActionButton, null);

        }

        void MCB_simpleCommunicationButtonAction(ListViewItem item, object p)
        {
            MCB_simpleCommunicationButtonActionInternal(item, p);

        }

        async void MCB_simpleCommunicationButtonActionInternal(ListViewItem item, object p)
        {

            if (!MCBQuantum.Instance.MCB_quickSerialNumberCheckStrict())
                return;
            if (item.Title == AppResources.calculate_and_save)
            {
                //Start editing
                ACUserDialogs.ShowProgress();
                McbCommunicationTypes caller = McbCommunicationTypes.NOCall;
                object arg1 = null;

                if (MCB_VerifyVoltageCalibration())
                {
                    await MCB_SaveIntoVoltageCalibration();
                    caller = McbCommunicationTypes.MCB_voltagecalibrationSaveConfig;
                }

                if (caller != McbCommunicationTypes.NOCall)
                {
                    List<object> arguments = new List<object>();
                    arguments.Add(caller);
                    arguments.Add(arg1);
                    //arguments.Add(false);
                    List<object> results = new List<object>();
                    try
                    {
                        results = await MCBQuantum.Instance.CommunicateMCB(arguments);
                        if (results.Count > 0)
                        {
                            var status = (CommunicationResult)results[2];
                            if (status == CommunicationResult.OK)
                            {
                                ResetOldData();
                                MCB_voltageCalibrationActionButton(null);
                            }
                            else
                            {
                                ACUserDialogs.ShowAlertWithTitleAndOkButton("Operation Failed");
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }
                }
                else
                {
                    ACUserDialogs.ShowAlertWithTitleAndOkButton(AppResources.alert_enter_valid + "\n" + verifyControl.GetErrorString());
                }
                ACUserDialogs.HideProgress();
                //End 
            }
        }

        public void OnBackButtonClick()
        {
            ShowViewModel<CalibrationViewModel>(new { pop = "pop" });
        }
    }
}
