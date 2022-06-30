using System.Collections.Generic;

namespace actchargers
{
    public class ACUtility
    {
        static ACUtility mInstance;
        public static ACUtility Instance
        {
            get
            {
                if (mInstance == null)
                {
                    mInstance = new ACUtility();
                }
                return mInstance;
            }
        }

        /// <summary>
        /// API Execution mode.
        /// </summary>
        public enum APIExecutionMode
        {
            Unique,
            Distinct,
            Multiple
        }

        /// <summary>
        /// APIR equest priority.
        /// </summary>
        public enum APIRequestPriority
        {
            Immediate,
            Urgent,
            Normal,
            Low,
            Trivial
        }

        /// <summary>
        /// Defines the Module flow.
        /// </summary>
        public enum ModuleFlow
        {
            Login
        }

        /// <summary>
        /// differenct celltypes in the appliation.
        /// </summary>
        public enum CellTypes
        {
            Default,
            Label,
            Image,
            LabelLabel,
            TwoLabel,
            ThreeLabel,
            LabelTextEdit,
            LabelSwitch,
            LabelSpinner,
            ListSelector,
            DatePicker,
            DatePickerWithSwitch,
            Button,
            QuickViewPlotCollection,
            QuickViewThreeLabel,
            Days,
            TimePicker,
            LabelText,
            Plot,
            RTrecordsPlot,
            ButtonTextEdit,
            LabelSwitchButton,
            ImageLabel,
            DownloadImageText,
            LabelCenter,
            SectionHeader
        }

        public enum InputType
        {
            Default,
            Number,
            Decimal
        }

        public enum ActionsMenuType
        {
            Default,
            Download,
            Upload,
            SyncSites,
            SiteView,
            Update,
            Restart,
            Refresh,
            SITE_VIEW_DOWNLOAD,
            SITE_VIEW_UPDATE,
            SITE_VIEW_SETTINGS
        }

        /// <summary>
        /// Used to differenctiate among different list selectors.
        /// </summary>
        public enum ListSelectorType
        {
            None = 0,
            TemperatureFormate = 1,
            Timezone = 2,
            BatteryVoltageUnits = 3,
            BatteryType = 4,
            ApplicationType = 5,
            HistoryTimeUnits = 6,
            DefaultCharge_Tr_Amps = 7,
            DefaultCharge_Cc_Amps = 8,
            DefaultCharge_Fi_Amps = 9,
            DefaultCharge_Eq_Amps = 10,
            FinishCycleSettingsType = 11,
            DurationIntervalTypes = 12,
            DefaultCharge_Cv_Current = 13,
            DefaultCharge_Cv_Timer = 14,
            DefaultCharge_Eq_Timer = 15,
            DefaultCharge_Desulfate_Timer = 16,
            DefaultCharge_Finish_Dv_Voltage = 17,
            DefaultCharge_Finish_Timer = 18,
            DefaultCharge_Dt_Time = 19,
            DefaultCharge_PartialChargeStop = 20,
            TemperatureFallbackControl = 21,
            RTRecordsList = 22,
            VoltageDetectThreshold = 23,
            CurrentDetectThreshold = 24,
            TimerDetectThreshold = 25,
            Dealer = 26,
            ServiceDealer = 27,
            Customers = 28,
            Sites = 29,
            OEM = 30
        }

        List<string> _batteryType;
        public List<string> BatteryType { get { return _batteryType; } }

        List<string> _applicationTypes;
        public List<string> ApplicationTypes { get { return _applicationTypes; } }

        List<string> _temperatureUnits;
        public List<string> TemperatureUnits { get { return _temperatureUnits; } }

        List<string> _temperatureFallbackControllValues;
        public List<string> TemperatureFallbackControllValues { get { return _temperatureFallbackControllValues; } }

        List<string> _historyTimeUnits;
        public List<string> HistoryTimeUnits { get { return _historyTimeUnits; } }

        List<string> _finishCycleSettingsTypes;
        public List<string> FinishCycleSettingsTypes { get { return _finishCycleSettingsTypes; } }

        List<string> _durationIntervalTypes;
        public List<string> DurationIntervalTypes { get { return _durationIntervalTypes; } }

        List<object> _durationIntervalObjectsTypes;
        public List<object> DurationIntervalObjectsTypes { get { return _durationIntervalObjectsTypes; } }

        List<string> _rtRecordsList;
        public List<string> RTRecordsList { get { return _rtRecordsList; } }

        List<string> _voltageDetectThresholdList;
        public List<string> VoltageDetectThresholdList { get { return _voltageDetectThresholdList; } }

        List<string> _currentDetectThresholdList;
        public List<string> CurrentDetectThresholdList { get { return _currentDetectThresholdList; } }

        List<string> _timerDetectThresholdList;
        public List<string> TimerDetectThresholdList { get { return _timerDetectThresholdList; } }


        public ACUtility()
        {
            _temperatureUnits = new List<string>() {
                AppResources.fahrenheit,
                AppResources.celsius
            };

            _batteryType = new List<string> {
                "Lead Acid",
                "AGM",
                "GEL"
            };

            _applicationTypes = new List<string>() {
                "Fast",
                "Conventional",
                "Opportunity"
            };

            _historyTimeUnits = new List<string>() {
                "Last hour",
                "Last 2 hours",
                "Last 4 hours",
                "Last 8 hours",
                "Last 16 hours",
                "Last 24 hours",
                "Last 2 days",
                "Last week",
                "Last 2 weeks",
                "Last month",
                "Last 2 months"

            };
            _finishCycleSettingsTypes = new List<string>() {
                "Always",
                "Custom"
            };


            _temperatureFallbackControllValues = new List<string>() {
                "Post damaged --> inter-cell will support, inter-cell damaged -->Post will support",
                "Post damaged --> inter-cell will support",
                "inter-cell value will be always overridden by Post , if Post is damaged inter-cell value will be used for both",
                "Post value will be always overridden by inter-cell, if inter-cell is damaged Post value will be used for both",
                "Post value will be always overridden by inter-cell, if inter-cell is damaged , post will run independently and doesn't support inter-cell",
                "Post value will be always overridden by inter-cell, if inter-cell is damaged , post value will not be available",
                "inter-cell damaged --> Post will support",
                "inter-cell value will be always overridden by Post , if Post is damaged inter-cell will run independently and doesn't support Post",
                "inter-cell value will be always overridden by Post , if Post is damaged inter-cell value will not be available",
                "they both has nothing to do with each other."
            };

            _durationIntervalTypes = new List<string>();
            _durationIntervalObjectsTypes = new List<object>();

            for (int i = 1; i <= 96; i++)
            {
                int minute = i * 15;
                string minuteText = string.Format("{0:00}:{1:00}", (minute / 60), (minute % 60));

                _durationIntervalTypes.Add(minuteText);
                _durationIntervalObjectsTypes.Add(minuteText);
            }

            _rtRecordsList = new List<string>() {
                "Last hour",
                "Last 2 hours",
                "Last 4 hours",
                "Last 8 hours",
                "Last 16 hours",
                "Last 24 hours",
                "Last 2 days",
                "Last week",
                "Last 2 weeks",
                "Last month",
                "Last 2 months"
            };

            _voltageDetectThresholdList = new List<string> {
                "0.5%",
                "1.0%",
                "1.5%",
                "2.0%"
            };

            _timerDetectThresholdList = new List<string> {
                "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20",
            "21",
            "22",
            "23",
            "24",
            "25",
            "26",
            "27",
            "28",
            "29",
            "30",
            "31",
            "32",
            "33",
            "34",
            "35",
            "36",
            "37",
            "38",
            "39",
            "40",
            "41",
            "42",
            "43",
            "44",
            "45",
            "46",
            "47",
            "48",
            "49",
            "50",
            "51",
            "52",
            "53",
            "54",
            "55",
            "56",
            "57",
            "58",
            "59",
            "60",
            "61",
            "62",
            "63",
            "64",
            "65",
            "66",
            "67",
            "68",
            "69",
            "70",
            "71",
            "72",
            "73",
            "74",
            "75",
            "76",
            "77",
            "78",
            "79",
            "80"
            };

            _currentDetectThresholdList = new List<string> {
                 "0.5%",
                "1.0%",
            "1.5%",
            "2.0%",
            "2.5%",
            "3.5%",
            "4.0%",
            "4.5%",
            "5.0%",
            "5.5%",
            "6.0%",
            "6.5%",
            "7.0%",
            "7.5%",
            "8.0%",
            "8.5%",
            "9.0%",
            "9.5%",
            "10.0%",
            "10.5%",
            "11.0%",
            "11.5%",
            "12.0%",
            "12.5%",
            "13.0%",
            "13.5%",
            "14.0%",
            "14.5%",
            "15.0%",
            "15.5%",
            "16.5%",
            "17.0%",
            "17.5%",
            "18.0%",
            "18.5%",
            "19.0%",
            "19.5%",
            "20.0%",
            "20.5%",
            "21.0%",
            "21.5%",
            "22.0%",
            "22.5%",
            "23.0%",
            "23.5%",
            "24.0%",
            "24.5%",
            "25.0%",
            "25.5%",
            "26.0%",
            "26.5%",
            "27.0%",
            "27.5%",
            "28.0%",
            "28.5%",
            "29.0%",
            "29.5%",
            "30.0%",
            "30.5%",
            "31.0%",
            "31.5%",
            "32.0%",
            "32.5%",
            "33.0%",
            "33.5%",
            "34.0%",
            "34.5%",
            "35.0%",
            "35.5%",
            "36.0%",
            "36.5%",
            "37.0%",
            "37.5%",
            "38.0%",
            "38.5%",
            "39.0%",
            "39.5%",
            "40.0%",
            "40.5%",
            "41.0%",
            "41.5%",
            "42.0%",
            "42.5%",
            "43.0%",
            "43.5%",
            "44.0%",
            "44.5%",
            "45.0%",
            "45.5%",
            "46.0%",
            "46.5%",
            "47.0%",
            "47.5%",
            "48.0%",
            "48.5%",
            "49.0%",
            "49.5%",
            "50.0%",
            "50.5%",
            "51.0%",
            "51.5%",
            "52.0%",
            "52.5%",
            "53.0%",
            "53.5%",
            "54.0%",
            "54.5%",
            "55.0%",
            "55.5%",
            "56.0%",
            "56.5%",
            "57.0%",
            "57.5%",
            "58.0%",
            "58.5%",
            "59.0%",
            "59.5%",
            "60.0%",
            "60.5%",
            "61.0%",
            "61.5%",
            "62.0%",
            "62.5%",
            "63.0%",
            "63.5%",
            "64.0%",
            "64.5%",
            "65.0%",
            "65.5%",
            "66.0%",
            "66.5%",
            "67.0%",
            "67.5%",
            "68.0%",
            "68.5%",
            "69.0%",
            "69.5%",
            "70.0%",
            "70.5%",
            "71.0%",
            "71.5%",
            "72.0%",
            "72.5%",
            "73.0%",
            "73.5%",
            "74.0%",
            "74.5%",
            "75.0%",
            "75.5%",
            "76.0%",
            "76.5%",
            "77.0%",
            "77.5%",
            "78.0%",
            "78.5%",
            "79.0%",
            "79.5%",
            "80.0%"};
        }

        public static void SetStatus(CommunicationResult id)
        {
            string StatusLabel = string.Empty;
            switch (id)
            {
                case CommunicationResult.FTDI_OPENING_ERROR: StatusLabel = "Status: Failed to initilaize Communication"; break;
                case CommunicationResult.SENDING_ERROR: StatusLabel = "Status: Failed to send to the Device"; break;
                case CommunicationResult.EXPECTED_DATA_COUNT_ERROR: StatusLabel = "Status: Failed to Read from the Device"; break;
                case CommunicationResult.RECEIVING_ERROR: StatusLabel = "Status: Can't Read from the Device -- Timeout"; break;
                case CommunicationResult.PACKET_SIZE_ERROR: StatusLabel = "Status: Communication interruption"; break;
                case CommunicationResult.CRC_ERROR: StatusLabel = "Status: Corrupted communication"; break;
                case CommunicationResult.RECEIVER_KEY_ERROR: StatusLabel = "Status: Can't detect ACT device"; break;
                case CommunicationResult.ACK_NACK_ERROR: StatusLabel = "Status: Busy"; break;//colorType = 1; doScan = false; break;
                case CommunicationResult.OK: StatusLabel = "Status: Operation Done"; break; //colorType = 2; doScan = false; break;
                case CommunicationResult.ERROR_IN_EEPROM_READING: StatusLabel = "Status: USB chip Failure R"; break;
                case CommunicationResult.ERROR_IN_EEPROM_WRITING: StatusLabel = "Status: USB chip Failure W"; break;
                case CommunicationResult.ACCESS_ERROR: StatusLabel = "Status: Access Error"; break;
                case CommunicationResult.SIZEERROR: StatusLabel = "Status: Size Error"; break;
                case CommunicationResult.ReadSomethingElse: StatusLabel = "Status: Communication invalid"; break;
                case CommunicationResult.CHARGER_BUSY: StatusLabel = "Status: Charger Busy"; break;//colorType = 1; doScan = false; break;
                case CommunicationResult.COMMAND_DELAYED: StatusLabel = "Status: Running Charger"; break;//doScan = false; break;
                case CommunicationResult.NOT_EXIST: StatusLabel = "Status: Lost Communication"; break;
                case CommunicationResult.invalidArg: StatusLabel = "Invalid argument"; break;//doScan = false; break;
            }
            //switch (colorType)
            //{
            //	case -1: StatusLabel.ForeColor = Color.Red; break;
            //	case 1: StatusLabel.ForeColor = Color.Orange; break;
            //	case 2: StatusLabel.ForeColor = Color.Green; break;
            //	case 3: StatusLabel.ForeColor = Color.Blue; break;

            //}
            //if (doScan)
            //	scanRelated_prepare(scanRelatedTypes.doScan);
            //return doScan;
            ACUserDialogs.ShowAlertWithTitleAndOkButton(StatusLabel);
        }
    }
}