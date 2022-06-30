using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using actchargers.Code.Utility;

namespace actchargers
{
    public class MCBobject : DeviceObjectParent
    {
        const float MIN_ACCEPTED_FIRMWARE_VERSION = 2.27f;

        public string GetConfigWithPMsJson()
        {
            var jObject = JsonParser.SerializeToJsonObject(Config);
            PMState[] pms = GetPMsList();

            if (pms == null)
                return jObject.ToString(Newtonsoft.Json.Formatting.None);

            List<PMState> pmsList = pms.Where((arg) => arg.valid).ToList();

            var pmsJObject = JsonParser.SerializeToJsonArray(pms);

            jObject.Add("PMsList", pmsJObject);

            return jObject.ToString(Newtonsoft.Json.Formatting.None);
        }

        public static string AddressToSerial(uint address)
        {
            string chargerSerialNumber = MCBQuantum.Instance.GetMCB().Config.serialNumber;

            return AddressToSerial(address, chargerSerialNumber);
        }

        public static string AddressToSerial(uint address, string chargerSerialNumber)
        {
            //208/480 (1 bit) , (date 10 bits), SN (15 bits)

            UInt32 model = address >> 28;
            int modelMap = 0;
            switch (model)
            {
                case 0: modelMap = 1; break;
                case 1:
                    {
                        if ((String.Compare(((address & 0x00FFFFFF)).ToString("00000000").Substring(2, 2), "20") > 0) || (String.Compare(((address & 0x00FFFFFF)).ToString("00000000").Substring(0, 4), "1220") == 0))
                            modelMap = 71;
                        else
                            modelMap = 10;
                    }
                    break;
                case 3: modelMap = 73; break;
                case 4: modelMap = 2; break;
                case 5:
                    {
                        if ((String.Compare(((address & 0x00FFFFFF)).ToString("00000000").Substring(2, 2), "20") > 0) || (String.Compare(((address & 0x00FFFFFF)).ToString("00000000").Substring(0, 4), "1220") == 0))
                            modelMap = 72;
                        else
                            modelMap = 20;
                    }
                    break;
                case 6: modelMap = 3; break;
                case 7: modelMap = 41; break;
                case 8: modelMap = 42; break;
                case 9: modelMap = 43; break;
                case 10: modelMap = 51; break;
                case 11: modelMap = 52; break;
                case 12: modelMap = 53; break;
                case 13: modelMap = 61; break;
                case 14: modelMap = 62; break;
                case 15: modelMap = 63; break;
            }

            string fullSerialNumber = "";

            string startChargerSerialNumber = chargerSerialNumber[0].ToString();

            fullSerialNumber += startChargerSerialNumber == "7" ? "5" : "1";

            fullSerialNumber += modelMap.ToString("00");

            fullSerialNumber += ((address & 0x00FFFFFF)).ToString("00000000");
            return fullSerialNumber.Substring(0, 7) + ((byte)((address & 0x03000000) >> 24)).ToString() + fullSerialNumber.Substring(7, 4);
        }

        #region Commands

        public const byte testCommand = 0x1A;

        const byte readADCCommand = 0x26;
        const byte readTempADCCommand = 0x29;
        const byte setACTViewID = 0x2A;
        const byte AccessCommand = 0x2A;
        const byte readRTCCommand = 0x2C;
        const byte readFirmwareRevisionCommand = 0x25;
        const byte RecyclePowerCommand = 0x1B;
        const byte ForceRecyclePowerCommand = 0x1E;
        const byte setRTCCommand = 0x1C;

        const byte SetConfigCommandV1 = 0x11;
        const byte SetConfigCommandV2 = 0x12;

        const byte ReadConfigCommand = 0x21;

        const byte ReadGlobalRecordCommand = 0x2D;
        const byte ResetGlobalCommand = 0x1D;
        const byte ReadRecordCommand = 0x2E;
        const byte USB_CHARGE_READ_PMs_STATE_COMMAND = 0x55;
        const byte USB_CHARGE_READ_CHARGE_STATE_COMMAND = 0x56;
        const byte SetSinglePMsMode = 0x57;
        const byte USB_remoteControl = 0x58;
        const byte USB_getPMErrorsCounter = 0x59;
        const byte USB_GET_GET_MCB_ANALOG_COMMAND = 0x5A;
        const byte USB_RESET_LCD_CAL = 0x5B;
        const byte USB_READ_RECORDS_LIMIT = 0x5C;
        const byte USB_WRITE_TO_EX_FLASH = 0x70;
        const byte USB_Request_Code_update = 0x71;
        const byte USB_WRITE_PLC_FLASH = 0x72;
        const byte USB_WRITE_DC_FLASH = 0x7D;
        const byte USB_Request_PLC_Code_update = 0x73;
        const byte USB_check_mcb_health = 0x74;
        const byte USB_Get_LCD_SIM = 0x75;
        const byte USB_Get_LCD_Req = 0x76;
        const byte USB_PowerSnapshots = 0x79;
        const byte USB_Request_stuff = 0xAA;
        const byte USB_read_calibrator_state = 0xAB;
        const byte USB_read_issue_read_bv = 0xAC;
        const byte USB_read_do_read_bv = 0xAD;
        const byte USB_read_do_write_bv = 0xAE;
        const byte USB_read_do_RTCwrite_bv = 0xAF;
        const byte WIFI_DEBUG_COMMAND = 0x7B;

        #endregion

        public GlobalRecord globalRecord;
        public ChargeState chargeState;
        public MCBHealthCheck health_check;
        LcdSimulator lcd;
        CalibratorSimulator calibratorSIM;

        public BattViewConfig BV_PLC;

        public MCBConfig Config { get; set; }

        public MCBConfig SiteViewConfig { get; set; }

        public WiFiDebug WiFiInfo { get; set; }

        UInt32 _minPMFaultRecordID;
        public UInt32 minPMFaultRecordID
        {
            get
            {
                lock (myLock)
                {
                    return _minPMFaultRecordID;
                }
            }
            set
            {
                lock (myLock)
                {
                    _minPMFaultRecordID = value;
                }
            }
        }

        bool _configRead;

        public bool configRead
        {
            get
            {
                lock (myLock)
                {
                    return _configRead;
                }
            }
            set
            {
                lock (myLock)
                {
                    _configRead = value;
                }
            }
        }

        UInt32 _minChargeRecordID;
        public UInt32 minChargeRecordID
        {
            get
            {
                lock (myLock)
                {
                    return _minChargeRecordID;
                }
            }
            set
            {
                lock (myLock)
                {
                    _minChargeRecordID = value;
                }
            }
        }

        uint minChargePowerSnapShot;
        public uint MinChargePowerSnapShot
        {
            get
            {
                lock (myLock)
                {
                    return minChargePowerSnapShot;
                }
            }
            set
            {
                lock (myLock)
                {
                    minChargePowerSnapShot = value;
                }
            }
        }

        uint minChargeDebugRecord;
        public uint MinChargeDebugRecord
        {
            get
            {
                lock (myLock)
                {
                    return minChargeDebugRecord;
                }
            }
            set
            {
                lock (myLock)
                {
                    minChargeDebugRecord = value;
                }
            }
        }

        bool _timeLost;
        public bool timeLost
        {
            get
            {
                lock (myLock)
                {
                    return _timeLost;
                }
            }
            set
            {
                lock (myLock)
                {
                    _timeLost = value;
                }
            }
        }

        byte _myZone;
        public byte myZone
        {
            get
            {
                lock (myLock)
                {
                    return _myZone;
                }
            }
            set
            {
                lock (myLock)
                {
                    _myZone = value;
                }
            }
        }

        string _analogVoltage;
        public string analogVoltage
        {
            get
            {
                lock (myLock)
                {
                    return _analogVoltage;
                }
            }
            set
            {
                lock (myLock)
                {
                    _analogVoltage = value;
                }
            }
        }
        string _analogTemperature;
        public string analogTemperature
        {
            get
            {
                lock (myLock)
                {
                    return _analogTemperature;
                }
            }
            set
            {
                lock (myLock)
                {
                    _analogTemperature = value;
                }
            }
        }
        bool _analogOCD;
        public bool analogOCD
        {
            get
            {
                lock (myLock)
                {
                    return _analogOCD;
                }
            }
            set
            {
                lock (myLock)
                {
                    _analogOCD = value;
                }
            }
        }

        public string GetBmsText()
        {
            return SharedTexts.GetBmsText(DcId);
        }

        bool _deviceIsLoaded;
        public bool deviceIsLoaded
        {
            get
            {
                lock (myLock)
                {
                    return _deviceIsLoaded;
                }
            }
            set
            {
                lock (myLock)
                {
                    _deviceIsLoaded = value;
                }
            }
        }

        CommunicationResult _doLoadErrorCode;
        CommunicationResult doLoadErrorCode
        {
            get
            {
                lock (myLock)
                {
                    return _doLoadErrorCode;
                }
            }
            set
            {
                lock (myLock)
                {
                    _doLoadErrorCode = value;
                }
            }
        }

        /***********************/
        public MCBobject
        (string serialNumber, ConnectionManager connectionManager, DeviceBaseType deviceType)
            : base(serialNumber, connectionManager)
        {
            chargeState = new ChargeState();
            Config = new MCBConfig();
            WiFiInfo = new WiFiDebug(MIN_ACCEPTED_FIRMWARE_VERSION);
            chargeRcords = new List<ChargeRecord>();
            globalRecord = new GlobalRecord();
            PMs = new PMState[PM_MCB_SupportNumber];
            for (int i = 0; i < PM_MCB_SupportNumber; i++)
                PMs[i] = new PMState();
            health_check = new MCBHealthCheck();
            lcd = new LcdSimulator();
            calibratorSIM = new CalibratorSimulator();
            BV_PLC = new BattViewConfig();
            powerModeulesFaults = new List<PMfault>();
            LastSentCommandTime = DateTime.UtcNow;
            deviceIsLoaded = false;
            this.DeviceType = deviceType;
        }

        public CommunicationResult getDoLoadStatus()
        {
            return doLoadErrorCode;
        }

        #region Do Load

        async Task<bool> DoLoadInternal
        (bool routerLoad, float firmwareVersion, byte zoneID, bool isRTCLost)
        {
            if (!routerLoad)
            {
                doLoadErrorCode = Task.Run(async () => { return await this.TestConnection(); }).Result;
                //doLoadErrorCode = await this.TestConnection();

                if (doLoadErrorCode != CommunicationResult.OK)
                {
                    return false;
                }
            }

            //doLoadErrorCode = Task.Run(async () => { return await this.ReadConfig(); }).Result;

            doLoadErrorCode = await this.ReadConfig();
            if (doLoadErrorCode != CommunicationResult.OK)
            {
                return false;
            }

            doLoadErrorCode = await ReadPMs();

            //doLoadErrorCode = Task.Run(async () => { return await this.ReadlobalRecord(); }).Result;

            doLoadErrorCode = await this.ReadlGobalRecord();
            if (doLoadErrorCode != CommunicationResult.OK)
            {
                return false;
            }


            //doLoadErrorCode = Task.Run(async () => { return await this.ReadRecordslimits(); }).Result;

            // mcb

            doLoadErrorCode = await this.ReadRecordslimits();
            if (doLoadErrorCode != CommunicationResult.OK)
            {
                return false;
            }

            if (routerLoad)
            {
                this.myZone = zoneID;
            }
            else
            {
                //doLoadErrorCode = Task.Run(async () => { return await this.ReadRTC(); }).Result;

                // mcb

                doLoadErrorCode = await this.ReadRTC();
                if (doLoadErrorCode != CommunicationResult.OK)
                {
                    return false;
                }
            }

            if (routerLoad)
            {
                this.FirmwareRevision = firmwareVersion;

            }
            else
            {
                //doLoadErrorCode = Task.Run(async () => { return await this.ReadFirmwareRevision(); }).Result;

                doLoadErrorCode = await this.ReadFirmwareRevision();
                if (doLoadErrorCode != CommunicationResult.OK)
                {
                    return false;
                }
            }

            if (routerLoad)
            {
                timeLost = isRTCLost;
            }
            else
            {
                //doLoadErrorCode = Task.Run(async () => { return await this.sendDefineConnection(); }).Result;

                doLoadErrorCode = await this.SendDefineCommand();
                if (doLoadErrorCode != CommunicationResult.OK)
                {
                    return false;
                }
            }
            // last command

            this.Config.firmwareVersion = this.FirmwareRevision;
            this.Config.zoneID = this.myZone;

            deviceIsLoaded = true;
            if (!routerLoad)
            {
                bool saveConfig = false;
                if (Config.originalSerialNumber[0] == '\0')
                {
                    //for boards who missed the original SN
                    Config.originalSerialNumber = "C" + Config.serialNumber;
                    saveConfig = true;
                }
                if (Config.afterCommissionBoardID == 0)
                {
                    Config.afterCommissionBoardID = UInt32.Parse(Config.id);
                    saveConfig = true;
                }
                if (float.Parse(Config.foldTemperature) < 25)
                {
                    //MCBConfig.afterCommissionBoardID = MCBConfig.InternalChargerID;
                    Config.TRtemperature = "15.5";
                    Config.foldTemperature = "51.6";
                    Config.coolDownTemperature = "46.1";
                    //        MCBConfig.FoldtemperatureCompensation = 70;
                    saveConfig = true;
                }
                if (Config.actViewConnectFrequency == "300")
                    Config.actViewConnectFrequency = "60";
                if (saveConfig)
                {
                    await this.SaveConfigToDevice();
                }

                if (Firmware.GetLatestMCBFirmware() > 200)
                    await this.SaveTime();
            }
            return true;
        }

        async Task<bool> StackedDoLoadInternal
        (bool routerLoad, float firmwareVersion, byte zoneID, bool isRTCLost)
        {
            List<Tuple<CommunicationResult, byte[]>> results = null;

            byte[] resultArray = new byte[1];
            var commandsList = new List<CommandObject>();

            CommandObject command;

            if (!routerLoad)
            {
                command = new CommandObject()
                {
                    CommandBytes = testCommand,
                    ExpectedSize = 0,
                    VerifyExpectedSize = true,
                    ResultArray = resultArray
                };
                commandsList.Add(command);
            }

            command = new CommandObject()
            {
                CommandBytes = ReadConfigCommand,
                ExpectedSize = MCBConfig.dataSize,
                VerifyExpectedSize = true,
                ResultArray = resultArray
            };
            commandsList.Add(command);

            command = new CommandObject()
            {
                CommandBytes = ReadGlobalRecordCommand,
                ExpectedSize = 62,
                VerifyExpectedSize = true,
                ResultArray = resultArray
            };
            commandsList.Add(command);

            if (DeviceType == DeviceBaseType.MCB)
            {
                command = new CommandObject()
                {
                    CommandBytes = USB_READ_RECORDS_LIMIT,
                    ExpectedSize = 8,
                    VerifyExpectedSize = true,
                    ResultArray = resultArray
                };
                commandsList.Add(command);
            }

            if (routerLoad)
            {
                this.myZone = zoneID;
            }
            else
            {
                if (DeviceType == DeviceBaseType.MCB)
                {
                    command = new CommandObject()
                    {
                        CommandBytes = readRTCCommand,
                        ExpectedSize = 5,
                        VerifyExpectedSize = true,
                        ResultArray = resultArray
                    };
                    commandsList.Add(command);
                }
            }

            if (routerLoad)
            {
                this.FirmwareRevision = firmwareVersion;
            }
            else
            {
                command = new CommandObject()
                {
                    CommandBytes = readFirmwareRevisionCommand,
                    ExpectedSize = 2,
                    VerifyExpectedSize = true,
                    ResultArray = resultArray
                };
                commandsList.Add(command);
            }

            if (routerLoad)
            {
                timeLost = isRTCLost;
            }
            else
            {
                command = new CommandObject()
                {
                    CommandBytes = CommProtocol.defineCommand,
                    ExpectedSize = 80,
                    VerifyExpectedSize = true,
                    ResultArray = resultArray
                };
                commandsList.Add(command);
            }
            // last command

            try
            {
                int tryTime = 3;
                while (StillLooping(tryTime, results))
                {
                    results =
                        await myCommunicator
                            .MySendRecieveStacked(commandsList, SerialNumber, 0);

                    if (IsAllOk(results))
                    {
                        break;
                    }

                    if (IsAllNotExist(results))
                    {
                        return false;
                    }

                    tryTime--;

                    await Task.Delay(1250);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());

                return false;
            }

            if (!IsAllOk(results))
            {
                return false;
            }


            #region Results

            int count = commandsList.Count;
            for (int i = 0; i < count; i++)
            {
                CommandObject commandItem = commandsList[i];
                var resultItem = results[i];
                var resultArrayItem = resultItem.Item2;
                byte commandBytes = commandItem.CommandBytes;

                switch (commandBytes)
                {
                    case testCommand:
                        break;

                    case readFirmwareRevisionCommand:
                        FirmwareRevision = ((10 * (resultArrayItem[0] & 0xF0) >> 4) +
                                            (resultArrayItem[0] & 0x0F)) +
                            (((10 * (resultArrayItem[1] & 0xF0) >> 4) +
                              (resultArrayItem[1] & 0x0F)) / 100.0f);

                        break;

                    case ReadConfigCommand:
                        //configRead = true;
                        Config.LoadFromArray(resultArrayItem, DeviceType);

                        break;

                    case ReadGlobalRecordCommand:
                        globalRecord.DeSerilaizeMe(resultArrayItem);

                        break;

                    case USB_READ_RECORDS_LIMIT:
                        minPMFaultRecordID = BitConverter.ToUInt32(resultArrayItem, 0);
                        minChargeRecordID = BitConverter.ToUInt32(resultArrayItem, 4);

                        if (resultArrayItem.Length >= 16)
                        {
                            MinChargePowerSnapShot = BitConverter.ToUInt32(resultArrayItem, 8);
                            MinChargeDebugRecord = BitConverter.ToUInt32(resultArrayItem, 12);
                        }

                        break;

                    case readRTCCommand:
                        UInt32 unixTimeStampNow =
                            (UInt32)(DateTime.UtcNow
                                     .Subtract(new DateTime
                                               (1970, 1, 1, 0, 0, 0, 0,
                                                DateTimeKind.Utc)).TotalSeconds);

                        byte[] dateAndTime = new byte[5];

                        Array.Copy(resultArrayItem, dateAndTime, 5);

                        UInt32 unixTimeStamp = BitConverter.ToUInt32(dateAndTime, 0);
                        myZone = dateAndTime[4];

                        break;

                    case CommProtocol.defineCommand:
                        if (resultArrayItem.Length != 80 && resultArrayItem.Length != 160)
                        {
                            doLoadErrorCode = CommunicationResult.SIZEERROR;
                            return false;
                        }

                        if (
                            (resultArrayItem[0] != CommProtocol.chargerDefineKey
                             && DeviceType == DeviceBaseType.MCB)
                            || (resultArrayItem[0] != CommProtocol.CalibratorDefineKey
                                && DeviceType == DeviceBaseType.CALIBRATOR)
                    )
                        {
                            doLoadErrorCode = CommunicationResult.RECEIVING_ERROR;
                            return false;
                        }

                        timeLost = resultArrayItem[39] != 0;
                        myZone = resultArrayItem[44];

                        int loc = 87;
                        if (FirmwareRevision > 2.87f && resultArray.Length >= 160)
                        {
                            DcId = resultArray[loc++];
                            FirmwareDcVersion = BitConverter.ToSingle(resultArray, loc);
                            loc += 4;
                        }

                        break;
                }
            }

            #endregion


            this.Config.firmwareVersion = this.FirmwareRevision;
            this.Config.zoneID = this.myZone;

            deviceIsLoaded = true;
            if (!routerLoad)
            {
                bool saveConfig = false;
                if (Config.originalSerialNumber[0] == '\0')
                {
                    //for boards who missed the original SN
                    Config.originalSerialNumber = "C" + Config.serialNumber;
                    saveConfig = true;
                }
                if (Config.afterCommissionBoardID == 0)
                {
                    Config.afterCommissionBoardID = UInt32.Parse(Config.id);
                    saveConfig = true;
                }
                if (float.Parse(Config.foldTemperature) < 25)
                {
                    //MCBConfig.afterCommissionBoardID = MCBConfig.InternalChargerID;
                    Config.TRtemperature = "15.5";
                    Config.foldTemperature = "51.6";
                    Config.coolDownTemperature = "46.1";
                    //        MCBConfig.FoldtemperatureCompensation = 70;
                    saveConfig = true;
                }
                if (Config.actViewConnectFrequency == "300")
                    Config.actViewConnectFrequency = "60";
                if (saveConfig)
                {
                    await this.SaveConfigToDevice();
                }

                if (Firmware.GetLatestMCBFirmware() > 200)
                    await this.SaveTime();
            }

            return true;
        }

        bool StillLooping
        (int tryTime, List<Tuple<CommunicationResult, byte[]>> results)
        {
            if (results != null && IsAllOk(results))
                return false;

            return tryTime > 0;
        }

        bool IsAllOk(List<Tuple<CommunicationResult, byte[]>> results)
        {
            bool isAllOk =
                results.All(
                    (arg) =>
                    arg.Item1 == CommunicationResult.OK
                );

            return isAllOk;
        }

        bool IsAllNotExist(List<Tuple<CommunicationResult, byte[]>> results)
        {
            bool isAllNotExist =
                results.All(
                    (arg) =>
                    arg.Item1 == CommunicationResult.NOT_EXIST
                );

            return isAllNotExist;
        }

        #endregion

        #region Do Load Commands

        public async Task<CommunicationResult> TestConnection()
        {
            try
            {
                byte[] resultArray = new byte[1];
                await ForceDelay();
                var tuple =
                    await myCommunicator
                        .MySendRecieve(testCommand, null, SerialNumber, 0,
                                       true, resultArray);
                CommunicationResult result = tuple.Item1;
                resultArray = tuple.Item2;
                LastSentCommandTime = DateTime.UtcNow;
                return result;
            }
            catch
            {
                return CommunicationResult.internalFailure;
            }
        }

        public async Task ReadConfigIfNotLoaded()
        {
            if (Config.NotLoaded)
                await ReadConfig();
        }

        public async Task<CommunicationResult> ReadConfig()
        {
            try
            {
                byte[] resultArray = new byte[1];
                await ForceDelay();
                var tuple =
                    await myCommunicator
                        .MySendRecieve(ReadConfigCommand, null, SerialNumber,
                                       MCBConfig.dataSize, true, resultArray);

                CommunicationResult result = tuple.Item1;
                resultArray = tuple.Item2;
                LastSentCommandTime = DateTime.UtcNow;
                if (result != CommunicationResult.OK)
                    return result;

                return Config.LoadFromArray(resultArray, DeviceType);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return CommunicationResult.internalFailure;
            }
        }

        public async Task<CommunicationResult> ReadlGobalRecord()
        {
            byte[] resultArray = new byte[1];
            try
            {
                await ForceDelay();
                var tuple =
                    await myCommunicator
                        .MySendRecieve(ReadGlobalRecordCommand, null, SerialNumber,
                                       62, true, resultArray);
                CommunicationResult result = tuple.Item1;
                resultArray = tuple.Item2;
                LastSentCommandTime = DateTime.UtcNow;
                if (result != CommunicationResult.OK)
                {
                    return result;
                }
                return this.globalRecord.DeSerilaizeMe(resultArray);
            }
            catch
            {
                return CommunicationResult.internalFailure;
            }
        }

        public async Task<CommunicationResult> ReadRecordslimits()
        {
            if (DeviceType != DeviceBaseType.MCB)
                return CommunicationResult.OK;
            try
            {
                byte[] resultArray = new byte[1];
                await ForceDelay();
                var tuple =
                    await myCommunicator
                        .MySendRecieve(USB_READ_RECORDS_LIMIT, null,
                                       SerialNumber, 8, false, resultArray);
                CommunicationResult result = tuple.Item1;
                resultArray = tuple.Item2;
                LastSentCommandTime = DateTime.UtcNow;
                if (result != CommunicationResult.OK)
                    return result;
                minPMFaultRecordID = BitConverter.ToUInt32(resultArray, 0);
                minChargeRecordID = BitConverter.ToUInt32(resultArray, 4);

            }
            catch
            {
                return CommunicationResult.internalFailure;
            }

            return CommunicationResult.OK;
        }

        async Task<CommunicationResult> ReadRTC()
        {
            if (DeviceType != DeviceBaseType.MCB)
                return CommunicationResult.OK;
            byte[] dateAndTime = new byte[5];

            try
            {
                UInt32 unixTimeStampNow =
                    (UInt32)(DateTime.UtcNow.Subtract(new DateTime
                                                      (1970, 1, 1, 0, 0, 0, 0,
                                                       DateTimeKind.Utc)).TotalSeconds);

                byte[] resultArray = new byte[1];
                await ForceDelay();
                var tuple =
                    await myCommunicator
                        .MySendRecieve(readRTCCommand, null, SerialNumber, 5,
                                       true, resultArray);
                CommunicationResult result = tuple.Item1;
                resultArray = tuple.Item2;
                LastSentCommandTime = DateTime.UtcNow;
                if (result != CommunicationResult.OK)
                    return result;
                Array.Copy(resultArray, dateAndTime, 5);

                UInt32 unixTimeStamp = BitConverter.ToUInt32(dateAndTime, 0);
                myZone = dateAndTime[4];
            }
            catch
            {
                return CommunicationResult.internalFailure;
            }

            return CommunicationResult.OK;
        }

        async Task<CommunicationResult> ReadFirmwareRevision()
        {
            try
            {
                byte[] resultArray = new byte[1];
                await ForceDelay();
                var tuple =
                    await myCommunicator
                        .MySendRecieve(readFirmwareRevisionCommand, null,
                                       SerialNumber, 2, true, resultArray);
                CommunicationResult result = tuple.Item1;
                resultArray = tuple.Item2;
                LastSentCommandTime = DateTime.UtcNow;
                if (result != CommunicationResult.OK)
                {
                    return result;
                }

                FirmwareRevision = ((10 * (resultArray[0] & 0xF0) >> 4)
                                    + (resultArray[0] & 0x0F))
                    + (((10 * (resultArray[1] & 0xF0) >> 4)
                        + (resultArray[1] & 0x0F)) / 100.0f);

            }
            catch
            {
                return CommunicationResult.internalFailure;
            }

            return CommunicationResult.OK;
        }

        public async Task<CommunicationResult> SendDefineCommand()
        {
            const int dataSize = 80; // or 80 * 2

            try
            {
                byte[] resultArray = new byte[1];
                await ForceDelay();
                var tuple =
                    await myCommunicator
                        .MySendRecieve(CommProtocol.defineCommand, null,
                                       SerialNumber, dataSize, true, resultArray);
                CommunicationResult result = tuple.Item1;
                resultArray = tuple.Item2;
                LastSentCommandTime = DateTime.UtcNow;
                if (result != CommunicationResult.OK)
                {
                    return result;
                }
                if (
                    (resultArray[0] != CommProtocol.chargerDefineKey
                     && DeviceType == DeviceBaseType.MCB)
                    || (resultArray[0] != CommProtocol.CalibratorDefineKey
                        && DeviceType == DeviceBaseType.CALIBRATOR)
                    )
                {

                    return CommunicationResult.RECEIVING_ERROR;
                }

                timeLost = resultArray[39] != 0;
                myZone = resultArray[44];
                FirmwareWiFiVersion = FirmwareWiFiVersionUtility.GetFirmwareWiFiVersion(FirmwareRevision, resultArray);

                int loc = 87;
                if (FirmwareRevision > 2.87f && resultArray.Length >= 160)
                {
                    DcId = resultArray[loc++];
                    FirmwareDcVersion = BitConverter.ToSingle(resultArray, loc);
                    loc += 4;
                }

                LastSentCommandTime = DateTime.UtcNow;

                return CommunicationResult.OK;
            }
            catch
            {
                return CommunicationResult.internalFailure;
            }
        }

        #endregion

        public void doLazyLoad
        (string sn, UInt32 myId, float firmwareVersion, float firmwareWiFiVersion,
         byte zoneID, bool isRTCLost, bool replacementPart, string name, string ipAddress, byte dcId, float firmwareDcVersion)
        {
            this.Config.serialNumber = sn;
            this.Config.id = myId.ToString();
            this.FirmwareRevision = firmwareVersion;
            this.FirmwareWiFiVersion = firmwareWiFiVersion;
            this.myZone = zoneID;
            this.timeLost = isRTCLost;
            this.Config.replacmentPart = replacementPart;
            this.Config.chargerUserName = name;
            deviceIsLoaded = false;
            this.IPAddress = ipAddress;
            this.DcId = dcId;
            this.FirmwareDcVersion = firmwareDcVersion;
        }

        public async Task<bool> RouterDoLoad(float firmwareVersion, byte zoneID, bool isRTCLost)
        {
            return await DoLoadInternal(true, firmwareVersion, zoneID, isRTCLost);
        }

        public MCBConfig getCopyOfArrayForReplacment(byte[] arr)
        {
            MCBConfig temp = new MCBConfig();
            temp.LoadFromArray(arr, DeviceType);
            //we have some fields that shoulf be loaded from the original config;
            temp.afterCommissionBoardID = Config.afterCommissionBoardID;
            temp.id = Config.id;
            temp.lcdMemoryVersion = Config.lcdMemoryVersion;
            temp.wifiFirmwareVersion = Config.wifiFirmwareVersion;
            temp.voltageCalBLow = Config.voltageCalBLow;
            temp.voltageCalALow = Config.voltageCalALow;
            temp.voltageCalB = Config.voltageCalB;
            temp.voltageCalA = Config.voltageCalA;
            temp.HWRevision = Config.HWRevision;
            temp.temperatureCalALow = Config.temperatureCalALow;
            temp.temperatureCalBLow = Config.temperatureCalBLow;
            temp.temperatureR = Config.temperatureR;
            temp.temperatureVI = Config.temperatureVI;
            temp.version = Config.version;

            return temp;

        }

        public async Task<bool> DoLoad()
        {
            return await Task.Run(async () =>
            {
                return await DoLoadInternal(false, 0, 0, false);
            });

        }
        public async Task<Tuple<CommunicationResult, UInt16>> ReadADC(UInt16 ADCResult, bool lowRange)
        {
            byte[] passData = new byte[1];
            if (lowRange)
                passData[0] = 0;
            else
                passData[0] = 1;
            //if (!mutex.WaitOne(15000))
            //    return new Tuple<commProtocol.Communication_Result, ushort>(commProtocol.Communication_Result.mutexKilled, ADCResult);

            try
            {
                byte[] resultArr = new byte[1];
                await ForceDelay();
                var tuple = await myCommunicator.MySendRecieve(readADCCommand, passData, SerialNumber, 2, true, resultArr);
                CommunicationResult result = tuple.Item1;
                resultArr = tuple.Item2;
                LastSentCommandTime = DateTime.UtcNow;

                if (result != CommunicationResult.OK)
                    return new Tuple<CommunicationResult, ushort>(result, ADCResult);
                ADCResult = BitConverter.ToUInt16(resultArr, 0);
            }
            catch
            {
                return new Tuple<CommunicationResult, ushort>(CommunicationResult.internalFailure, ADCResult);
            }
            finally
            {
                //mutex.ReleaseMutex();
            }
            return new Tuple<CommunicationResult, ushort>(CommunicationResult.OK, ADCResult);
        }
        public async Task<CommunicationResult> ReadMCB_Health()
        {
            //
            if (DeviceType != DeviceBaseType.MCB)
                return CommunicationResult.OK;
            //if (!mutex.WaitOne(15000)) return commProtocol.Communication_Result.mutexKilled;
            try
            {
                byte[] resultArr = new byte[1];
                await ForceDelay();
                var tuple = await myCommunicator.MySendRecieve(USB_check_mcb_health, null, SerialNumber, 20, true, resultArr);
                CommunicationResult result = tuple.Item1;
                resultArr = tuple.Item2;
                LastSentCommandTime = DateTime.UtcNow;

                if (result != CommunicationResult.OK)
                    return result;
                health_check.loadFromArray(resultArr);

            }
            catch
            {
                return CommunicationResult.internalFailure;
            }
            finally
            {
                //mutex.ReleaseMutex();
            }

            return CommunicationResult.OK;
        }
        public LcdSimulator getLCDInfo()
        {
            lock (myLock)
            {
                return lcd.GetAcopy();
            }
        }
        public CalibratorSimulator getCalibratorState()
        {
            lock (myLock)
            {
                return calibratorSIM.getACopy();
            }
        }

        public async Task<CommunicationResult> ReadCalibratorState()
        {
            if (DeviceType != DeviceBaseType.CALIBRATOR)
                return CommunicationResult.OK;
            //if (!mutex.WaitOne(15000)) return commProtocol.Communication_Result.mutexKilled;
            try
            {
                byte[] resultArr = new byte[1];
                await ForceDelay();
                var tuple = await myCommunicator.MySendRecieve(USB_read_calibrator_state, null, SerialNumber, 128, true, resultArr);
                CommunicationResult result = tuple.Item1;
                resultArr = tuple.Item2;
                LastSentCommandTime = DateTime.UtcNow;

                if (result != CommunicationResult.OK)
                    return result;
                lock (myLock)
                {
                    calibratorSIM.loadFromArray(resultArr);
                }

            }
            catch
            {
                return CommunicationResult.internalFailure;
            }
            finally
            {
                //mutex.ReleaseMutex();
            }

            return CommunicationResult.OK;
        }

        public async Task<Tuple<CommunicationResult, byte>> IssueReadCalibratorBV(byte res)
        {
            //BV_PLC
            if (DeviceType != DeviceBaseType.CALIBRATOR)
                return new Tuple<CommunicationResult, byte>(CommunicationResult.OK, res);
            //if (!mutex.WaitOne(15000)) return new Tuple<commProtocol.Communication_Result, byte>(commProtocol.Communication_Result.mutexKilled, res);
            try
            {
                byte[] resultArr = new byte[1];
                await ForceDelay();
                var tuple = await myCommunicator.MySendRecieve(USB_read_issue_read_bv, null, SerialNumber, 1, true, resultArr);
                CommunicationResult result = tuple.Item1;
                resultArr = tuple.Item2;
                LastSentCommandTime = DateTime.UtcNow;

                if (result != CommunicationResult.OK)
                    return new Tuple<CommunicationResult, byte>(result, res);
                res = resultArr[0];

            }
            catch
            {
                return new Tuple<CommunicationResult, byte>(CommunicationResult.internalFailure, res);
            }
            finally
            {
                //mutex.ReleaseMutex();
            }

            return new Tuple<CommunicationResult, byte>(CommunicationResult.OK, res);
        }


        public async Task<Tuple<CommunicationResult, byte>> doReadCalibratorBV(byte res, float firmwareVersion, byte zoneId)
        {
            //BV_PLC
            if (DeviceType != DeviceBaseType.CALIBRATOR)
                return new Tuple<CommunicationResult, byte>(CommunicationResult.OK, res);
            //if (!mutex.WaitOne(15000)) return new Tuple<commProtocol.Communication_Result, byte>(commProtocol.Communication_Result.mutexKilled, res);
            try
            {
                byte[] resultArr = new byte[1];
                await ForceDelay();
                var tuple = await myCommunicator.MySendRecieve(USB_read_do_read_bv, null, SerialNumber, 513, true, resultArr);
                CommunicationResult result = tuple.Item1;
                resultArr = tuple.Item2;
                LastSentCommandTime = DateTime.UtcNow;

                if (result != CommunicationResult.OK)
                    return new Tuple<CommunicationResult, byte>(result, res);
                res = resultArr[0];
                if (res == 1)
                {
                    byte[] tempArr = new byte[512];
                    Array.Copy(resultArr, 1, tempArr, 0, 512);
                    BV_PLC.LoadFromArray(tempArr, FirmwareRevision);
                    BV_PLC.zoneid = zoneId;
                    BV_PLC.firmwareversion = firmwareVersion;
                }

            }
            catch
            {
                return new Tuple<CommunicationResult, byte>(CommunicationResult.internalFailure, res);
            }
            finally
            {
                //mutex.ReleaseMutex();
            }

            return new Tuple<CommunicationResult, byte>(CommunicationResult.OK, res);
        }

        public async Task<CommunicationResult> ReadMCB_LCD()
        {
            if (DeviceType != DeviceBaseType.MCB)
                return CommunicationResult.OK;
            //if (!mutex.WaitOne(15000)) return commProtocol.Communication_Result.mutexKilled;
            try
            {
                byte[] resultArr = new byte[1];
                await ForceDelay();
                var tuple = await myCommunicator.MySendRecieve(USB_Get_LCD_SIM, null, SerialNumber, 128, true, resultArr);
                CommunicationResult result = tuple.Item1;
                resultArr = tuple.Item2;
                LastSentCommandTime = DateTime.UtcNow;

                if (result != CommunicationResult.OK)
                    return result;
                lock (myLock)
                {
                    lcd.LoadFromArray(resultArr);
                }

            }
            catch
            {
                return CommunicationResult.internalFailure;
            }
            finally
            {
                //mutex.ReleaseMutex();
            }

            return CommunicationResult.OK;


        }
        public async Task<CommunicationResult> setMCB_LCD(LcdRequest req)
        {
            if (DeviceType != DeviceBaseType.MCB)
                return CommunicationResult.OK;
            //if (!mutex.WaitOne(15000)) return commProtocol.Communication_Result.mutexKilled;
            try
            {
                byte[] resultArr = new byte[1];
                await ForceDelay();
                var tuple = await myCommunicator.MySendRecieve(USB_Get_LCD_Req, req.getArray(), SerialNumber, 0, true, resultArr);
                CommunicationResult result = tuple.Item1;
                resultArr = tuple.Item2;
                LastSentCommandTime = DateTime.UtcNow;

                if (result != CommunicationResult.OK)
                    return result;
                lock (myLock)
                {
                    lcd.LoadFromArray(resultArr);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return CommunicationResult.internalFailure;
            }
            finally
            {
                //mutex.ReleaseMutex();
            }

            return CommunicationResult.OK;

        }

        public async Task<CommunicationResult> RessetLCDCal()
        {
            //if (!mutex.WaitOne(15000))
            //    return commProtocol.Communication_Result.mutexKilled;
            try
            {
                byte[] resultArr = new byte[1];
                await ForceDelay();
                var tuple = await myCommunicator.MySendRecieve(USB_RESET_LCD_CAL, null, SerialNumber, 0, true, resultArr);
                CommunicationResult result = tuple.Item1;
                resultArr = tuple.Item2;
                LastSentCommandTime = DateTime.UtcNow;
                if (result != CommunicationResult.OK)
                {

                    return result;
                }
            }
            catch
            {
                return CommunicationResult.internalFailure;
            }
            finally
            {
                //mutex.ReleaseMutex();
            }
            return CommunicationResult.OK;

        }

        internal async override Task<CommunicationResult> RecyclePower()
        {
            try
            {
                byte[] resArr = new byte[1];

                var tuple = await myCommunicator.MySendRecieve(RecyclePowerCommand, null, SerialNumber, 0, true, resArr, TimeoutLevel.shortTimeout);
                CommunicationResult result = tuple.Item1;
                resArr = tuple.Item2;
                LastSentCommandTime = DateTime.UtcNow;

                return result;
            }
            catch
            {
                return CommunicationResult.internalFailure;
            }
        }

        public async Task<CommunicationResult> ForceRecyclePower()
        {
            try
            {
                byte[] data = new byte[1];
                byte[] resArr = new byte[1];

                var tuple = await myCommunicator.MySendRecieve(ForceRecyclePowerCommand, data, SerialNumber, 0, true, resArr, TimeoutLevel.shortTimeout);
                CommunicationResult result = tuple.Item1;
                resArr = tuple.Item2;
                LastSentCommandTime = DateTime.UtcNow;

                return result;
            }
            catch
            {
                return CommunicationResult.internalFailure;
            }
        }

        public async Task<CommunicationResult> SetSinglePMModeMac(UInt32 macAddresses)
        {
            if (DeviceType != DeviceBaseType.MCB)
                return CommunicationResult.OK;
            byte[] address = new byte[4];
            Array.Copy(BitConverter.GetBytes(macAddresses), 0, address, 0, 4);
            //if (!mutex.WaitOne(15000))
            //    return commProtocol.Communication_Result.mutexKilled;
            try
            {
                byte[] resArr = new byte[1];
                await ForceDelay();
                var tuple = await myCommunicator.MySendRecieve(SetSinglePMsMode, address, SerialNumber, 0, true, resArr);
                CommunicationResult result = tuple.Item1;
                resArr = tuple.Item2;
                LastSentCommandTime = DateTime.UtcNow;
                return result;
            }
            catch
            {
                return CommunicationResult.internalFailure;
            }
            finally
            {
                //mutex.ReleaseMutex();
            }

        }
        public async Task<Tuple<CommunicationResult, UInt16>> ReadTempADC(UInt16 ADCResult)
        {
            if (DeviceType != DeviceBaseType.MCB)
            {
                ADCResult = 0;
                return new Tuple<CommunicationResult, ushort>(CommunicationResult.OK, ADCResult);
            }
            //if (!mutex.WaitOne(15000))
            //    return new Tuple<commProtocol.Communication_Result, ushort>(commProtocol.Communication_Result.mutexKilled, ADCResult);
            try
            {
                byte[] resArr = new byte[1];
                await ForceDelay();
                var tuple = await myCommunicator.MySendRecieve(readTempADCCommand, null, SerialNumber, 2, true, resArr);
                CommunicationResult result = tuple.Item1;
                resArr = tuple.Item2;
                LastSentCommandTime = DateTime.UtcNow;
                if (result != CommunicationResult.OK)
                {
                    return new Tuple<CommunicationResult, ushort>(result, ADCResult);
                }
                ADCResult = BitConverter.ToUInt16(resArr, 0);
                return new Tuple<CommunicationResult, ushort>(CommunicationResult.OK, ADCResult);
            }
            catch
            {
                return new Tuple<CommunicationResult, ushort>(CommunicationResult.internalFailure, ADCResult);
            }
        }

        public async Task<CommunicationResult> SaveTime()
        {
            if (DeviceType != DeviceBaseType.MCB)
                return CommunicationResult.OK;

            byte[] passData = new byte[213];
            Array.Copy(BitConverter.GetBytes((UInt32)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc)).TotalSeconds), passData, 4);
            JsonZone j = StaticDataAndHelperFunctions.getZoneByID(myZone);
            for (int i = 0; i < 50; i++)
                Array.Copy(BitConverter.GetBytes(j.changes_time[i]), 0, passData, 4 + i * 4, 4);
            Array.Copy(BitConverter.GetBytes(j.changes_value), 0, passData, 204, 4);
            Array.Copy(BitConverter.GetBytes(j.base_utc), 0, passData, 208, 4);
            passData[212] = j.id;

            try
            {
                byte[] resArr = new byte[1];
                await ForceDelay();
                var tuple = await myCommunicator.MySendRecieve(setRTCCommand, passData, SerialNumber, 0, true, resArr);
                CommunicationResult result = tuple.Item1;
                resArr = tuple.Item2;
                LastSentCommandTime = DateTime.UtcNow;

                if (result != CommunicationResult.OK)
                {

                    return result;
                }
                timeLost = false;
                return CommunicationResult.OK;
            }
            catch
            {
                return CommunicationResult.internalFailure;
            }
        }

        public async Task<CommunicationResult> setActviewID()
        {
            byte[] passData = new byte[4];
            Array.Copy(BitConverter.GetBytes(uint.Parse(Config.id)), passData, 4);
            //if (!mutex.WaitOne(15000))
            //    return commProtocol.Communication_Result.mutexKilled;
            try
            {
                byte[] resArr = new byte[1];
                await ForceDelay();
                var tuple = await myCommunicator.MySendRecieve(setACTViewID, passData, SerialNumber, 2, true, resArr);
                CommunicationResult result = tuple.Item1;
                resArr = tuple.Item2;
                LastSentCommandTime = DateTime.UtcNow;
                return result;
            }
            catch
            {
                return CommunicationResult.internalFailure;
            }
            finally
            {
                //mutex.ReleaseMutex();
            }

        }
        public async Task<CommunicationResult> writetoPLCFlash(byte[] data, int length)
        {
            if (DeviceType != DeviceBaseType.MCB)
                return CommunicationResult.OK;
            CommunicationResult result;
            byte[] temp = new byte[1028];
            int tryTime = 0;
            byte[] resArr = new byte[1];
            for (int i = 0; i < (int)(length / 1024); i++)
            {
                Array.Copy(BitConverter.GetBytes((UInt32)i * 1024), 0, temp, 0, 4);
                Array.Copy(data, i * 1024, temp, 4, 1024);
                //if (!mutex.WaitOne(15000))
                //    return commProtocol.Communication_Result.mutexKilled;
                await ForceDelay();
                try
                {
                    var tuple = await myCommunicator.MySendRecieve(USB_WRITE_PLC_FLASH, temp, SerialNumber, 0, false, resArr);
                    result = tuple.Item1;
                    resArr = tuple.Item2;
                }
                catch
                {
                    return CommunicationResult.internalFailure;
                }
                finally
                {
                    LastSentCommandTime = DateTime.UtcNow;
                    //mutex.ReleaseMutex();
                }
                if (result != CommunicationResult.OK)
                {
                    if (tryTime >= 3)
                        return result;
                    else
                    {

                        tryTime++;
                        Task.Delay(500);
                        //Thread.Sleep(500);
                        i--;
                        continue;
                    }
                }
                tryTime = 0;
            }
            tryTime = 0;
            result = CommunicationResult.OK;
            if ((length % 1024) != 0)
            {
                while (tryTime < 3)
                {
                    byte[] temp2 = new byte[length % 1024 + 4];
                    Array.Copy(BitConverter.GetBytes((((UInt32)(length / 1024))) * 1024), 0, temp2, 0, 4);
                    Array.Copy(data, (((int)(length / 1024))) * 1024, temp2, 4, (length % 1024));
                    //if (!mutex.WaitOne(15000))
                    //    return commProtocol.Communication_Result.mutexKilled;
                    await ForceDelay();
                    try
                    {
                        var tuple = await myCommunicator.MySendRecieve(USB_WRITE_PLC_FLASH, temp2, SerialNumber, 0, false, resArr);
                        result = tuple.Item1;
                        resArr = tuple.Item2;
                    }
                    catch
                    {
                        return CommunicationResult.internalFailure;
                    }
                    finally
                    {
                        LastSentCommandTime = DateTime.UtcNow;
                        //mutex.ReleaseMutex();
                    }
                    if (result == CommunicationResult.OK)
                        break;
                    await Task.Delay(500);
                    //Thread.Sleep(500);
                    tryTime++;

                }
            }
            return result;
        }
        public async Task<CommunicationResult> requestPLCUpdate()
        {
            if (DeviceType != DeviceBaseType.MCB)
                return CommunicationResult.OK;
            //if (!mutex.WaitOne(15000))
            //    return commProtocol.Communication_Result.mutexKilled;
            try
            {
                byte[] resArr = new byte[1];
                await ForceDelay();
                var tuple = await myCommunicator.MySendRecieve(USB_Request_PLC_Code_update, null, SerialNumber, 0, false, resArr, TimeoutLevel.extended);
                CommunicationResult status = tuple.Item1;
                resArr = tuple.Item2;
                LastSentCommandTime = DateTime.UtcNow;
                if (status != CommunicationResult.OK)
                {
                    await ForceDelay();
                    await Task.Delay(500);
                    //Thread.Sleep(500);
                    tuple = await myCommunicator.MySendRecieve(USB_Request_PLC_Code_update, null, SerialNumber, 0, false, resArr, TimeoutLevel.extended);
                    status = tuple.Item1;
                    resArr = tuple.Item2;
                    LastSentCommandTime = DateTime.UtcNow;
                }

                return status;
            }
            catch
            {
                return CommunicationResult.internalFailure;
            }
        }

        public async Task<CommunicationResult> setCalibratorCurrent(float current, byte setCalibratorManualMode, byte driveBitsControl, bool disablePLC)
        {
            if (DeviceType != DeviceBaseType.CALIBRATOR)
                throw new Exception("Not Supported");
            try
            {
                byte[] passData = new byte[32];
                int loc = 0;

                Array.Copy(BitConverter.GetBytes(current), 0, passData, loc, 4);
                loc += 4;
                passData[loc++] = setCalibratorManualMode;
                passData[loc++] = driveBitsControl;
                passData[loc++] = (byte)(disablePLC ? 0x01 : 0x00);

                byte[] resArr = new byte[1];
                await ForceDelay();
                var tuple = await myCommunicator.MySendRecieve(USB_Request_stuff, passData, SerialNumber, 0, true, resArr);
                CommunicationResult result = tuple.Item1;
                resArr = tuple.Item2;
                LastSentCommandTime = DateTime.UtcNow;
                return result;
            }
            catch
            {
                return CommunicationResult.internalFailure;
            }
        }

        public async Task<Tuple<CommunicationResult, byte>> doCalibratorWriteBV(byte res)
        {
            if (DeviceType != DeviceBaseType.CALIBRATOR)
                throw new Exception("Not Supported");
            byte[] passData = BV_PLC.formatAll();
            try
            {
                byte[] resArr = new byte[1];
                await ForceDelay();
                var tuple = await myCommunicator.MySendRecieve(USB_read_do_write_bv, passData, SerialNumber, 1, true, resArr);
                CommunicationResult result = tuple.Item1;
                resArr = tuple.Item2;
                LastSentCommandTime = DateTime.UtcNow;
                if (result == CommunicationResult.OK)
                    res = resArr[0];
                return new Tuple<CommunicationResult, byte>(result, res);
            }
            catch
            {
                return new Tuple<CommunicationResult, byte>(CommunicationResult.internalFailure, res);
            }
        }

        public async Task<Tuple<CommunicationResult, byte>> doCalibratorRTCWrite(byte res)
        {
            if (DeviceType != DeviceBaseType.CALIBRATOR)
                throw new Exception("Not Supported");
            byte[] passData = new byte[217];
            UInt32 userID = ControlObject.userID;
            Array.Copy(BitConverter.GetBytes((UInt32)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc)).TotalSeconds), passData, 4);
            Array.Copy(BitConverter.GetBytes(userID), 0, passData, 4, 4);
            JsonZone j = StaticDataAndHelperFunctions.getZoneByID(this.BV_PLC.zoneid);
            for (int i = 0; i < 50; i++)
                Array.Copy(BitConverter.GetBytes(j.changes_time[i]), 0, passData, 8 + i * 4, 4);
            Array.Copy(BitConverter.GetBytes(j.changes_value), 0, passData, 208, 4);
            Array.Copy(BitConverter.GetBytes(j.base_utc), 0, passData, 212, 4);
            passData[216] = j.id;
            try
            {
                //if (!mutex.WaitOne(15000))
                //    return new Tuple<commProtocol.Communication_Result, byte>(commProtocol.Communication_Result.mutexKilled, res);
                byte[] resArr = new byte[1];
                await ForceDelay();
                var tuple = await myCommunicator.MySendRecieve(USB_read_do_RTCwrite_bv, passData, SerialNumber, 1, true, resArr);
                CommunicationResult result = tuple.Item1;
                resArr = tuple.Item2;
                LastSentCommandTime = DateTime.UtcNow;
                if (result == CommunicationResult.OK)
                    res = resArr[0];
                return new Tuple<CommunicationResult, byte>(result, res);
            }
            catch
            {
                return new Tuple<CommunicationResult, byte>(CommunicationResult.internalFailure, res);
            }
        }

        #region Config

        public async Task<bool> SaveSiteViewConfigAndTime()
        {
            if (HasConfigError())
                return false;

            var saveConfigResult = await SaveSiteViewConfig();

            if (saveConfigResult)
            {
                var saveTimeResult = await SaveTime();

                return (saveTimeResult == CommunicationResult.OK);
            }

            return false;
        }

        bool HasConfigError()
        {
            if (SiteViewConfig.version == 1)
            {
                if (SiteViewConfig.actAccessPassword.Length > 13)
                    return true;
            }
            else
            {
                if (SiteViewConfig.actAccessPassword.Length > 31)
                    return true;
            }

            return false;
        }

        public async Task<bool> SaveSiteViewConfig()
        {
            if (SiteViewConfig == null)
                return true;

            var result = await SaveSiteViewToDevice();
            bool ok = result == CommunicationResult.OK;

            if (ok)
                Config = SiteViewConfig;

            return ok;
        }

        async Task<CommunicationResult> SaveSiteViewToDevice()
        {
            try
            {
                return await SaveConfigToDeviceFromObject(SiteViewConfig);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);

                return CommunicationResult.internalFailure;
            }
        }

        public async Task<bool> SaveConfigAndTime()
        {
            var saveConfigResult = await SaveConfig();

            if (saveConfigResult)
            {
                var saveTimeResult = await SaveTime();

                return (saveTimeResult == CommunicationResult.OK);
            }

            return false;
        }

        public async Task<bool> SaveConfig()
        {
            var result = await SaveConfigToDevice();

            if (result == CommunicationResult.OK)
            {
                MCBQuantum.Instance.GetConnectionManager().siteView.setDeviceConfigurationSaved(SerialNumber, false);

                return true;
            }

            return false;
        }

        public async Task<CommunicationResult> SaveConfigToDevice()
        {
            try
            {
                return await SaveConfigToDeviceFromObject(Config);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);

                return CommunicationResult.internalFailure;
            }
        }

        async Task<CommunicationResult> SaveConfigToDeviceFromObject(MCBConfig config)
        {
            byte[] resArr = new byte[1];
            await ForceDelay();

            byte saveCommand = config.version == 1 ? SetConfigCommandV1 : SetConfigCommandV2;

            var tuple =
                await myCommunicator
                    .MySendRecieve
                    (saveCommand, config.SerializeMe(), SerialNumber, 2, true, resArr);

            CommunicationResult result = tuple.Item1;
            resArr = tuple.Item2;
            LastSentCommandTime = DateTime.UtcNow;

            if (result == CommunicationResult.RECEIVING_ERROR && ACConstants.ConnectionType == ConnectionTypesEnum.USB)
            {
                return CommunicationResult.OK;
            }

            if (result != CommunicationResult.OK && result != CommunicationResult.COMMAND_DELAYED)
                return result;

            config.memorySignature = BitConverter.ToUInt16(resArr, 0).ToString();

            return result;
        }

        public async Task<CommunicationResult> setConfigFromConfig(MCBConfig temp)
        {
            byte[] arr1 = temp.SerializeMe();

            //if (!mutex.WaitOne(15000))
            //    return commProtocol.Communication_Result.mutexKilled;
            try
            {
                byte[] resArr = new byte[1];
                await ForceDelay();

                byte saveCommand = Config.version == 1 ? SetConfigCommandV1 : SetConfigCommandV2;

                var tuple = await myCommunicator.MySendRecieve(saveCommand, arr1, SerialNumber, 2, true, resArr);

                CommunicationResult result = tuple.Item1;
                resArr = tuple.Item2;
                LastSentCommandTime = DateTime.UtcNow;
                if (result != CommunicationResult.OK && result != CommunicationResult.COMMAND_DELAYED)
                    return result;
                Config.memorySignature = BitConverter.ToUInt16(resArr, 0).ToString();
                return result;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return CommunicationResult.internalFailure;
            }
            finally
            {
                //mutex.ReleaseMutex();
            }

        }

        #endregion

        #region Records

        List<ChargeRecord> chargeRcords;

        public List<ChargeRecord> getRecordsList()
        {
            List<ChargeRecord> temp = new List<ChargeRecord>();
            lock (recordsLock)
            {
                foreach (ChargeRecord x in chargeRcords)
                {
                    ChargeRecord c = new ChargeRecord();
                    c.copyFromRecord(x);
                    temp.Add(c);
                }
            }
            return temp;
        }


        async Task<Tuple<CommunicationResult, UInt32, bool>> getNextRecordsBulk(UInt32 ResultRecordStartID, bool emptyRecords)
        {
            if (DeviceType != DeviceBaseType.MCB)
            {
                ResultRecordStartID = 0;
                emptyRecords = true;

                return new Tuple<CommunicationResult, uint, bool>(CommunicationResult.OK, ResultRecordStartID, emptyRecords);
            }
            byte[] data = new byte[5];
            byte[] resultArray = new byte[1];
            Array.Copy(BitConverter.GetBytes(ResultRecordStartID), 0, data, 0, 4);
            data[4] = 14;//max size of read
            //if (!mutex.WaitOne(15000)) return new Tuple<commProtocol.Communication_Result, uint, bool>(commProtocol.Communication_Result.mutexKilled, ResultRecordStartID, emptyRecords);
            CommunicationResult result = CommunicationResult.NOT_EXIST;
            try
            {
                await ForceDelay();
                var tuple = await myCommunicator.MySendRecieve(ReadRecordCommand, data, SerialNumber, 0, false, resultArray);
                result = tuple.Item1;
                resultArray = tuple.Item2;
                LastSentCommandTime = DateTime.UtcNow;

                if (result != CommunicationResult.OK)
                {

                    return new Tuple<CommunicationResult, uint, bool>(result, ResultRecordStartID, emptyRecords);
                }
            }
            catch
            {
                return new Tuple<CommunicationResult, uint, bool>(CommunicationResult.internalFailure, ResultRecordStartID, emptyRecords);
            }
            finally
            {
                //mutex.ReleaseMutex();
            }
            if (resultArray.Length == 0)
            {

                return new Tuple<CommunicationResult, uint, bool>(CommunicationResult.SIZEERROR, ResultRecordStartID, emptyRecords);
            }
            if (resultArray[0] == 0x7F)
            {
                result = await this.ReadRecordslimits();
                if (result != CommunicationResult.OK)
                    return new Tuple<CommunicationResult, uint, bool>(result, ResultRecordStartID, emptyRecords);
                ResultRecordStartID = this.minChargeRecordID;
                emptyRecords = false;
                return new Tuple<CommunicationResult, uint, bool>(CommunicationResult.OK, ResultRecordStartID, emptyRecords);
            }


            emptyRecords = true;
            int count = resultArray[0];
            if (count * ChargeRecord.dataSize + 1 != resultArray.Length)
                return new Tuple<CommunicationResult, uint, bool>(CommunicationResult.SIZEERROR, ResultRecordStartID, emptyRecords);
            ResultRecordStartID += (uint)count;
            for (int i = 0; i < count; i++)
            {
                emptyRecords = false;
                ChargeRecord record = new ChargeRecord();
                byte[] arr = new byte[ChargeRecord.dataSize];
                Array.Copy(resultArray, 1 + i * ChargeRecord.dataSize, arr, 0, ChargeRecord.dataSize);
                record.loadFromArray(arr);
                chargeRcords.Add(record);
            }
            return new Tuple<CommunicationResult, uint, bool>(CommunicationResult.OK, ResultRecordStartID, emptyRecords);
        }
        public async Task<CommunicationResult> getAllRecords(UInt32 CycleHistoryCounter, UInt32 startRecord)
        {
            //lock (recordsLock)
            //{
            chargeRcords = new List<ChargeRecord>();
            bool emptyRecords = true;
            if (CycleHistoryCounter == 0)
            {
                return CommunicationResult.OK;
            }
            UInt32 ResultRecordStartID = startRecord;
            CommunicationResult status;
            do
            {
                var tuple = await getNextRecordsBulk(ResultRecordStartID, emptyRecords);
                status = tuple.Item1;
                ResultRecordStartID = tuple.Item2;
                emptyRecords = tuple.Item3;
                if (status != CommunicationResult.OK)
                    return status;
            } while (!emptyRecords);

            chargeRcords.Sort((a1, a2) => a2.cycleID.CompareTo(a1.cycleID));
            return CommunicationResult.OK;
            //}

        }
        #endregion
        #region global records
        public async Task<CommunicationResult> ResetGlobalRecord()
        {
            if (DeviceType != DeviceBaseType.MCB)
                return CommunicationResult.OK;
            //ResetGlobalCommand,
            //if (!mutex.WaitOne(15000)) return commProtocol.Communication_Result.mutexKilled;
            try
            {
                await ForceDelay();
                byte[] resArr = new byte[1];
                var tuple = await myCommunicator.MySendRecieve(ResetGlobalCommand, null, SerialNumber, 0, true, resArr);
                CommunicationResult result = tuple.Item1;
                resArr = tuple.Item2;
                LastSentCommandTime = DateTime.UtcNow;
                if (result != CommunicationResult.OK)
                {
                    return result;
                }
                this.globalRecord.ResetMe();
                return CommunicationResult.OK;
            }
            catch
            {
                return CommunicationResult.internalFailure;
            }
            finally
            {
                //mutex.ReleaseMutex();
            }

        }

        #endregion
        public async Task<CommunicationResult> readChargeState()
        {
            if (DeviceType != DeviceBaseType.MCB)
                return CommunicationResult.OK;
            byte[] resultArray = new byte[1];
            //if (!mutex.WaitOne(15000)) return commProtocol.Communication_Result.mutexKilled;
            try
            {
                await ForceDelay();
                var tuple = await myCommunicator.MySendRecieve(USB_CHARGE_READ_CHARGE_STATE_COMMAND, null, SerialNumber, 14, true, resultArray);
                CommunicationResult result = tuple.Item1;
                resultArray = tuple.Item2;
                LastSentCommandTime = DateTime.UtcNow;
                if (result != CommunicationResult.OK)
                {
                    return result;
                }
                chargeState.DeSerialize(resultArray);
                return CommunicationResult.OK;
            }
            catch
            {
                return CommunicationResult.internalFailure;
            }
            finally
            {
                //mutex.ReleaseMutex();
            }
        }

        #region PMS

        PMState[] PMs;

        List<PMfault> powerModeulesFaults;
        public List<PMfault> getPowerModeulesFaultsArrayList()
        {
            lock (recordsLock)
            {
                List<PMfault> temp = new List<PMfault>();
                foreach (PMfault x in powerModeulesFaults)
                {
                    PMfault c = new PMfault();
                    c.loadFromRecord(x);
                    temp.Add(c);
                }
                return temp;
            }
        }

        public async Task<CommunicationResult> getPMErrorLog(UInt32 startRecord)
        {
            if (DeviceType != DeviceBaseType.MCB)
                return CommunicationResult.OK;
            //lock (recordsLock)
            //{
            powerModeulesFaults.Clear();

            byte[] vars = new byte[5];
            if (startRecord < minPMFaultRecordID)
                startRecord = minPMFaultRecordID;
            vars[4] = 56;

            CommunicationResult result = 0;

            while (true)
            {
                Array.Copy(BitConverter.GetBytes(startRecord), vars, 4);
                //if (!mutex.WaitOne(15000)) return commProtocol.Communication_Result.mutexKilled;
                byte[] data = new byte[1];
                try
                {
                    await ForceDelay();
                    var tuple = await myCommunicator.MySendRecieve(USB_getPMErrorsCounter, vars, SerialNumber, 0, false, data);
                    result = tuple.Item1;
                    data = tuple.Item2;
                    LastSentCommandTime = DateTime.UtcNow;
                    if (result != CommunicationResult.OK)
                        return result;
                }
                catch
                {
                    return CommunicationResult.internalFailure;
                }
                finally
                {
                    //mutex.ReleaseMutex();
                }
                if (data.Length == 0)
                    return CommunicationResult.SIZEERROR;

                if (data[0] == 0x7F)
                {
                    //starting point is Wrong
                    result = await ReadRecordslimits();
                    if (result != CommunicationResult.OK)
                        return result;
                    startRecord = minPMFaultRecordID;
                    Array.Copy(BitConverter.GetBytes(startRecord), vars, 4);
                    continue;

                }
                if (data[0] == 0)
                    break;
                //check size
                if (data[0] * PMfault.recordSize + 1 != data.Length)
                    return CommunicationResult.SIZEERROR;

                for (int i = 1; i < data[0] * PMfault.recordSize + 1; i += PMfault.recordSize)
                {
                    PMfault f = new PMfault();
                    byte[] aRecord = new byte[PMfault.recordSize];
                    Array.Copy(data, i, aRecord, 0, PMfault.recordSize);
                    f.loadFromArray(aRecord, this.myZone, Config.daylightSaving);
                    uint id = (uint)(startRecord + (i - 1) / PMfault.recordSize);
                    f.verifyID(id);
                    powerModeulesFaults.Add(f);

                }
                startRecord += data[0];


            }
            return CommunicationResult.OK;
            //}
        }

        public PMState[] GetPMsList()
        {
            lock (recordsLock)
            {
                PMState[] pms = new PMState[PMs.Length];
                for (int i = 0; i < PMs.Length; i++)
                {
                    pms[i] = PMs[i];
                }

                return pms;
            }
        }

        const byte PM_MCB_SupportNumber = 32;
        const int PM_Record_Size = 30;

        internal int getPMCommandPageSize()
        {
            int pageSize = 8;

            if (FirmwareRevision > 2.17)
                pageSize = 16;

            return pageSize;
        }

        public async Task<CommunicationResult> ReadPMs()
        {
            if (DeviceType != DeviceBaseType.MCB)
                return CommunicationResult.OK;

            int pageSize = getPMCommandPageSize();

            byte[] vars = new byte[1];
            byte[] singlePM = new byte[PM_Record_Size];
            byte[] totalResult = new byte[PM_Record_Size * PM_MCB_SupportNumber];

            byte[] resArr = new byte[1];

            for (int page = 0; page < (PM_MCB_SupportNumber / pageSize); page++)
            {
                vars[0] = (byte)(page * pageSize);

                try
                {
                    await ForceDelay();

                    var tuple = await myCommunicator.MySendRecieve(USB_CHARGE_READ_PMs_STATE_COMMAND, vars, SerialNumber, PM_Record_Size * pageSize, true, resArr);

                    CommunicationResult Cresult = tuple.Item1;
                    resArr = tuple.Item2;
                    LastSentCommandTime = DateTime.UtcNow;

                    if (Cresult != CommunicationResult.OK)
                        return Cresult;

                    Array.Copy(resArr, 0, totalResult, pageSize * PM_Record_Size * page, pageSize * PM_Record_Size);
                }
                catch
                {
                    return CommunicationResult.internalFailure;
                }
            }

            for (int i = 0; i < PM_MCB_SupportNumber; i++)
            {
                Array.Copy(totalResult, i * PM_Record_Size, singlePM, 0, PM_Record_Size);

                PMs[i].DeSerialize(singlePM, Config.serialNumber, FirmwareRevision);
            }

            return CommunicationResult.OK;
        }

        #endregion

        #region analog  read and flash write

        public async Task<CommunicationResult> getAnalogValues()
        {
            byte[] data = new byte[1];
            //if (!mutex.WaitOne(15000)) return commProtocol.Communication_Result.mutexKilled;
            try
            {
                await ForceDelay();
                var tuple = await myCommunicator.MySendRecieve(USB_GET_GET_MCB_ANALOG_COMMAND, null, SerialNumber, 6, true, data);
                CommunicationResult result = tuple.Item1;
                data = tuple.Item2;
                LastSentCommandTime = DateTime.UtcNow;
                if (result != CommunicationResult.OK)
                    return result;
                analogVoltage = (BitConverter.ToInt16(data, 0) / 100.0).ToString();
                if (data[4] == 0)
                    analogTemperature = "N/A";
                else
                    analogTemperature = (BitConverter.ToInt16(data, 2) / 10.0).ToString();

                analogOCD = data[5] == 1;
                return CommunicationResult.OK;
            }
            catch
            {
                return CommunicationResult.internalFailure;
            }
            finally
            {
                //mutex.ReleaseMutex();
            }

        }
        #endregion
        public async Task<CommunicationResult> sendOneFirmwarePacket(byte[] data, int step)
        {
            CommunicationResult result = CommunicationResult.internalFailure;
            //if (!mutex.WaitOne(15000)) return commProtocol.Communication_Result.mutexKilled;
            if (data.Length > 1024)
                return CommunicationResult.internalFailure;

            try
            {
                byte[] temp = new byte[data.Length + 4];
                byte[] tempArr = new byte[1];

                Array.Copy(BitConverter.GetBytes((UInt32)step * 1024), 0, temp, 0, 4);
                Array.Copy(data, 0, temp, 4, data.Length);

                await ForceDelay();
                var tuple = await myCommunicator.MySendRecieve(USB_WRITE_TO_EX_FLASH, temp, SerialNumber, 0, true, tempArr, TimeoutLevel.shortTimeout, false);
                result = tuple.Item1;
                tempArr = tuple.Item2;
                LastSentCommandTime = DateTime.UtcNow;

            }
            catch
            {
                return CommunicationResult.internalFailure;
            }

            return result;
        }

        #region Update Firmware

        public override async Task<Tuple<bool, string>> UpdateFirmware(bool excecDspic)
        {
            try
            {
                return await TryUpdateFirmware(excecDspic);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());

                FirmwareUpdateStep = FirmwareUpdateStage.FAILED;

                return new Tuple<bool, string>(false, AppResources.opration_failed);
            }
        }

        async Task<Tuple<bool, string>> TryUpdateFirmware(bool excecDspic)
        {
            McbCommunicationTypes caller = McbCommunicationTypes.NOCall;
            object arg1 = null;
            string msg = "";

            float currentFirmwareVersion = FirmwareRevision;
            if (!Firmware.DoesMcbRequireFirmwareUpdate(currentFirmwareVersion))
            {
                msg = "Charger has the latest Firmware";

                FirmwareUpdateStep = FirmwareUpdateStage.updateIsNotNeeded;

                return new Tuple<bool, string>(false, msg);
            }

            byte[] serials = null;
            //IsBusy = true;
            Firmware firmwareManager = new Firmware();
            FirmwareResult res = firmwareManager.UpdateFileBinary(DeviceBaseType.MCB, ref serials);
            //IsBusy = false;
            if (res != FirmwareResult.fileOK)
            {
                switch (res)
                {
                    case FirmwareResult.badFileEncode:
                        msg = ("Bad file encoding");
                        break;
                    case FirmwareResult.badFileFormat:
                        msg = ("Bad file encoding");
                        break;
                    case FirmwareResult.fileNotFound:
                        msg = ("File not found");
                        break;
                    case FirmwareResult.noAcessFile:
                        msg = ("Can't read file");
                        break;
                }
            }
            else
            {
                caller = McbCommunicationTypes.firmwareWrite;
                arg1 = serials;
            }

            List<object> arguments = new List<object>
            {
                caller,
                arg1
            };
            List<object> results = new List<object>();

            FirmwareUpdateStep = FirmwareUpdateStage.sendingRequest;

            results = await WriteFirmware(arguments, excecDspic);

            if (results.Count > 0)
            {
                var callerStatus = (McbCommunicationTypes)results[1];
                var status = (CommunicationResult)results[0];
                if (callerStatus == McbCommunicationTypes.firmwareUpdateRequest)
                {
                    if (status == CommunicationResult.COMMAND_DELAYED)
                    {
                        msg = "Charger is busy, the firmware update will take place automatically once  the charge cycle is done.";

                        FirmwareUpdateStep = FirmwareUpdateStage.sentRequestDelayed;
                    }
                    else if (status == CommunicationResult.OK || FirmwareRevision < 2.05f)
                    {
                        msg = "charger is reflushing Firmware..it may take few minutes to update the firmware";

                        FirmwareUpdateStep = FirmwareUpdateStage.sentRequestPassed;
                    }
                    else
                    {
                        msg = "Something went wrong, please retry";

                        FirmwareUpdateStep = FirmwareUpdateStage.FAILED;
                    }

                }
                else if (callerStatus == McbCommunicationTypes.firmwareWrite)
                {
                    msg = AppResources.cant_update_firmware;

                    FirmwareUpdateStep = FirmwareUpdateStage.FAILED;
                }
            }
            else
            {
                msg = AppResources.opration_failed;

                FirmwareUpdateStep = FirmwareUpdateStage.FAILED;
            }

            bool ok = (FirmwareUpdateStep == FirmwareUpdateStage.sentRequestPassed) ||
                (FirmwareUpdateStep == FirmwareUpdateStage.sentRequestDelayed);

            return new Tuple<bool, string>(ok, msg);
        }

        async Task<List<object>> WriteFirmware(List<object> arguments, bool excecDspic)
        {
            await Task.Delay(500);

            CommunicationResult status = CommunicationResult.NOT_EXIST;
            List<object> genericlist = arguments;
            List<object> result = new List<object>();
            McbCommunicationTypes caller = (McbCommunicationTypes)genericlist[0];
            object arg1 = genericlist[1];

            byte[] serials = (byte[])arg1;
            DateTime start = DateTime.UtcNow;
            status = await WriteToBootLoaderFlash(serials, serials.Length);

            if (excecDspic)
            {
                caller = McbCommunicationTypes.firmwareUpdateRequest;

                result.Add(status);
                result.Add(caller);

                return result;
            }

            if (status == CommunicationResult.OK)
            {
                status = await RequestBootLoaderUpdate(excecDspic);

                if (status == CommunicationResult.OK || FirmwareRevision < 2.05f)
                    connectionManager.ForceSoftDisconnectDevice(SerialNumber, false, true);

                caller = McbCommunicationTypes.firmwareUpdateRequest;
            }
            Logger.AddLog(false, "MCB update took:" + (DateTime.UtcNow - start).TotalSeconds.ToString());

            result.Add(status);
            result.Add(caller);

            return result;
        }

        internal override byte GetNoneEsp32UpdateFirmwareCommand()
        {
            return USB_WRITE_TO_EX_FLASH;
        }

        internal override byte GetNoneEsp32RequestBootLoaderCommand()
        {
            return USB_Request_Code_update;
        }

        internal override byte[] GetRequestBootLoaderUpdatePassedData()
        {
            return IsEsp32WiFi ? new byte[4] : null;
        }

        public override async Task<Tuple<bool, string>> UpdateDcFirmware(byte id, bool restart)
        {
            CommunicationResult status = CommunicationResult.NOT_EXIST;

            string msg = "";
            byte[] serials = null;

            if (Firmware.GetDCBinaries(ref serials, id) != FirmwareResult.fileOK)
            {
                msg = "Bad Firmware encoding";
            }
            else
            {
                try
                {
                    status = await WriteToDcFlash(serials);
                }
                catch (Exception ex)
                {
                    Logger.AddLog(true, "XX343:" + ex);

                    status = CommunicationResult.internalFailure;
                }

                if (status == CommunicationResult.OK)
                {
                    if (restart)
                    {
                        try
                        {
                            status = await ForceRecyclePower();
                        }
                        catch (Exception ex)
                        {
                            Logger.AddLog(true, "XX344:" + ex);

                            status = CommunicationResult.internalFailure;
                        }
                        if (status == CommunicationResult.OK)
                        {
                            msg = "charger is updating Daughter Card Firmware";

                            connectionManager.ForceSoftDisconnectDevice(SerialNumber, false);
                        }
                        else if (status == CommunicationResult.CHARGER_BUSY)
                        {
                            msg = "Charger is busy, the firmware update will take place automatically.";
                        }
                    }
                }
                else
                {
                    msg = "Can't Update Firmware";
                }
            }

            return new Tuple<bool, string>(status == CommunicationResult.OK, msg);
        }

        async Task<CommunicationResult> WriteToDcFlash(byte[] data)
        {
            int length = data.Length;

            List<CommandObject> cmdObjs = new List<CommandObject>();

            for (int i = 0; i < length / 1024; i++)
            {
                byte[] temp = new byte[1028];

                Array.Copy(BitConverter.GetBytes((UInt32)i * 1024), 0, temp, 0, 4);
                Array.Copy(data, i * 1024, temp, 4, 1024);

                CommandObject commandObject = new CommandObject()
                {
                    CommandBytes = USB_WRITE_DC_FLASH,
                    Data = temp,
                    ExpectedSize = 0,
                    VerifyExpectedSize = true,
                    SaveLastSucceededIndex = true
                };

                cmdObjs.Add(commandObject);
            }

            if ((length % 1024) != 0)
            {
                byte[] temp2 = new byte[length % 1024 + 4];
                Array.Copy(BitConverter.GetBytes((((UInt32)(length / 1024))) * 1024), 0, temp2, 0, 4);
                Array.Copy(data, length / 1024 * 1024, temp2, 4, (length % 1024));

                CommandObject commandObject = new CommandObject()
                {
                    CommandBytes = USB_WRITE_DC_FLASH,
                    Data = temp2,
                    ExpectedSize = 0,
                    VerifyExpectedSize = true
                };

                cmdObjs.Add(commandObject);
            }

            Tuple<CommunicationResult, byte[]> lastResult = new Tuple<CommunicationResult, byte[]>(CommunicationResult.internalFailure, new byte[1]);

            try
            {
                await Task.Delay(500);

                int tryTime = 3;
                do
                {
                    tryTime--;

                    var result =
                        await
                        myCommunicator
                            .MySendRecieveStacked(cmdObjs, SerialNumber, 0, TimeoutLevel.extended);

                    if (result == null || result.Count == 0)
                        continue;

                    lastResult = result[result.Count - 1];

                    if (lastResult == null)
                        continue;

                    if (lastResult.Item1 != CommunicationResult.OK)
                        await Task.Delay(1250);

                } while (tryTime-- > 0 && lastResult.Item1 != CommunicationResult.OK);

                return lastResult.Item1;
            }
            catch
            {
                return CommunicationResult.internalFailure;
            }
        }

        #endregion

        public override void Cancel()
        {
            if (myCommunicator != null)
            {
                myCommunicator.Cancel();
            }
        }

        #region debug

        const int debugCommandSize = 128;
        public async Task<CommunicationResult> debugStopStartCharger(bool start)
        {
            byte[] command = new byte[debugCommandSize];
            for (int i = 0; i < debugCommandSize; i++)
                command[i] = 0;
            command[0] = 1;
            if (start)
            {
                command[37] = 2;
            }
            else
            {
                command[37] = 1;
            }
            //if (!mutex.WaitOne(15000)) return commProtocol.Communication_Result.mutexKilled;
            byte[] resArr = new byte[1];
            await ForceDelay();

            CommunicationResult result;
            try
            {
                var tuple = await myCommunicator.MySendRecieve(USB_remoteControl, command, SerialNumber, 0, true, resArr);
                result = tuple.Item1;
                resArr = tuple.Item2;
            }
            catch
            {
                return CommunicationResult.internalFailure;
            }
            finally
            {
                LastSentCommandTime = DateTime.UtcNow;
                //mutex.ReleaseMutex();
            }
            return result;

        }

        public async Task<CommunicationResult> debugSetBatteryVoltage(bool enable, UInt16 volts)
        {
            byte[] command = new byte[debugCommandSize];
            for (int i = 0; i < debugCommandSize; i++)
                command[i] = 0;
            command[0] = 2;
            if (enable)
            {
                command[38] = 1;
            }
            else
            {
                command[38] = 0;
            }
            Array.Copy(BitConverter.GetBytes(volts), 0, command, 1, 2);
            //if (!mutex.WaitOne(15000)) return commProtocol.Communication_Result.mutexKilled;
            byte[] resArr = new byte[1];
            await ForceDelay();

            CommunicationResult result;
            try
            {
                var tuple = await myCommunicator.MySendRecieve(USB_remoteControl, command, SerialNumber, 0, true, resArr);
                result = tuple.Item1;
                resArr = tuple.Item2;
            }
            catch
            {
                return CommunicationResult.internalFailure;
            }
            finally
            {
                LastSentCommandTime = DateTime.UtcNow;
                //mutex.ReleaseMutex();
            }
            return result;

        }
        public async Task<CommunicationResult> debugSetBatteryTemperature(bool enable, Int16 temperature)
        {
            byte[] command = new byte[debugCommandSize];
            for (int i = 0; i < debugCommandSize; i++)
                command[i] = 0;
            command[0] = 3;
            if (enable)
            {
                command[39] = 1;
            }
            else
            {
                command[39] = 0;
            }

            Array.Copy(BitConverter.GetBytes(temperature), 0, command, 3, 2);
            //if (!mutex.WaitOne(15000)) return commProtocol.Communication_Result.mutexKilled;
            byte[] resArr = new byte[1];
            await ForceDelay();

            CommunicationResult result;
            try
            {
                var tuple = await myCommunicator.MySendRecieve(USB_remoteControl, command, SerialNumber, 0, true, resArr);
                result = tuple.Item1;
                resArr = tuple.Item2;

            }
            catch
            {
                return CommunicationResult.internalFailure;
            }
            finally
            {
                LastSentCommandTime = DateTime.UtcNow;
                //mutex.ReleaseMutex();
            }
            return result;

        }
        public async Task<CommunicationResult> debugScheduleDesulfate(bool enable)
        {
            byte[] command = new byte[debugCommandSize];
            for (int i = 0; i < debugCommandSize; i++)
                command[i] = 0;
            command[0] = 4;
            if (enable)
            {
                command[40] = 1;
            }
            else
            {
                command[40] = 0;
            }
            //if (!mutex.WaitOne(15000)) return commProtocol.Communication_Result.mutexKilled;
            byte[] resArr = new byte[1];
            await ForceDelay();

            CommunicationResult result;
            try
            {
                var tuple = await myCommunicator.MySendRecieve(USB_remoteControl, command, SerialNumber, 0, true, resArr);
                result = tuple.Item1;
                resArr = tuple.Item2;
            }
            catch
            {
                return CommunicationResult.internalFailure;
            }
            finally
            {
                LastSentCommandTime = DateTime.UtcNow;
                //mutex.ReleaseMutex();
            }
            return result;

        }
        public async Task<CommunicationResult> debugScheduleEQ(bool enable)
        {
            byte[] command = new byte[debugCommandSize];
            for (int i = 0; i < debugCommandSize; i++)
                command[i] = 0;
            command[0] = 5;
            if (enable)
            {
                command[41] = 1;
            }
            else
            {
                command[41] = 0;
            }
            //if (!mutex.WaitOne(15000)) return commProtocol.Communication_Result.mutexKilled;
            byte[] resArr = new byte[1];
            await ForceDelay();

            CommunicationResult result;
            try
            {
                var tuple = await myCommunicator.MySendRecieve(USB_remoteControl, command, SerialNumber, 0, true, resArr);
                result = tuple.Item1;
                resArr = tuple.Item2;
            }
            catch
            {
                return CommunicationResult.internalFailure;
            }
            finally
            {
                LastSentCommandTime = DateTime.UtcNow;
                //mutex.ReleaseMutex();
            }
            return result;
        }
        public async Task<CommunicationResult> debugAddError(int id, byte error_id, bool persistent)
        {
            byte[] command = new byte[debugCommandSize];
            for (int i = 0; i < debugCommandSize; i++)
                command[i] = 0;
            command[0] = 6;
            command[id + 5] = error_id;
            if (persistent)
                command[id + 5] |= 0x80;

            command[42] = (byte)id;
            //if (!mutex.WaitOne(15000)) return commProtocol.Communication_Result.mutexKilled;
            byte[] resArr = new byte[1];
            await ForceDelay();

            CommunicationResult result;
            try
            {
                var tuple = await myCommunicator.MySendRecieve(USB_remoteControl, command, SerialNumber, 0, true, resArr);
                result = tuple.Item1;
                resArr = tuple.Item2;
            }
            catch
            {
                return CommunicationResult.internalFailure;
            }
            finally
            {
                LastSentCommandTime = DateTime.UtcNow;
                //mutex.ReleaseMutex();
            }
            return result;


        }
        public async Task<CommunicationResult> debugSwitchMode(byte simModeType, UInt32 l)
        {
            byte[] command = new byte[debugCommandSize];
            for (int i = 0; i < debugCommandSize; i++)
                command[i] = 0;
            command[0] = 8;
            command[45] = simModeType;
            Array.Copy(BitConverter.GetBytes(l), 0, command, 46, 4);
            //if (!mutex.WaitOne(15000)) return commProtocol.Communication_Result.mutexKilled;
            byte[] resArr = new byte[1];
            await ForceDelay();

            CommunicationResult result;
            try
            {
                var tuple = await myCommunicator.MySendRecieve(USB_remoteControl, command, SerialNumber, 0, true, resArr);
                result = tuple.Item1;
                resArr = tuple.Item2;
            }
            catch
            {
                return CommunicationResult.internalFailure;
            }
            finally
            {
                LastSentCommandTime = DateTime.UtcNow;
                //mutex.ReleaseMutex();
            }
            return result;
        }
        #endregion

        #region New Download

        public async Task<List<ChargeRecord>> readchargeCycles_synchRightx(UInt32 startRecordID, UInt32 endRecordID)
        {
            if (IsEsp32WiFi)
                return await readchargeCycles_synchRightx_esp32(startRecordID, endRecordID);

            return await readchargeCycles_synchRightx_old(startRecordID, endRecordID);
        }

        public async Task<List<ChargeRecord>> readchargeCycles_synchRightx_esp32(UInt32 startRecordID, UInt32 endRecordID)
        {
            List<ChargeRecord> RightSideEvents = new List<ChargeRecord>();
            List<Tuple<CommunicationResult, byte[]>> result = new List<Tuple<CommunicationResult, byte[]>>();

            if (DeviceType != DeviceBaseType.MCB)
                return RightSideEvents;

            if (startRecordID > endRecordID)
                return RightSideEvents;

            UInt32 patchesCount = (endRecordID - startRecordID) / esp32_patchsize;

            if ((endRecordID - startRecordID) % esp32_patchsize != 0)
                patchesCount++;

            if (patchesCount > 10)
                patchesCount = 10;

            List<CommandObject> commands = new List<CommandObject>();

            for (int i = 0; i < patchesCount; i++)
            {
                byte[] passData = new byte[6];

                Array.Copy(BitConverter.GetBytes(startRecordID + i * esp32_patchsize), passData, 4);
                Array.Copy(BitConverter.GetBytes(esp32_patchsize), 0, passData, 4, 2);

                CommandObject commandObject = new CommandObject()
                {
                    CommandBytes = ESP32_readRecords,
                    Data = passData,
                    ExpectedSize = 0,
                    VerifyExpectedSize = false
                };

                commands.Add(commandObject);
            }

            try
            {
                await ForceDelay();

                result = await myCommunicator.MySendRecieveStacked(commands, SerialNumber, 0);

                LastSentCommandTime = DateTime.UtcNow;
            }
            catch
            {
                return RightSideEvents;
            }

            foreach (var item in result)
            {
                var resultArray = item.Item2;

                if (resultArray.Length < 2)
                    return RightSideEvents;

                Int16 recievedRecordsCount = BitConverter.ToInt16(resultArray, 0);
                if (recievedRecordsCount == -1)
                    return RightSideEvents;

                recievedRecordsCount = resultArray[0];

                if (recievedRecordsCount == 0)
                    return RightSideEvents;

                if (recievedRecordsCount * ChargeRecord.dataSize + 2 != resultArray.Length)
                    return RightSideEvents;

                for (int i = 0; i < recievedRecordsCount; i++)
                {
                    ChargeRecord record = new ChargeRecord();

                    byte[] arr = new byte[ChargeRecord.dataSize];
                    Array.Copy(resultArray, 2 + i * ChargeRecord.dataSize, arr, 0, ChargeRecord.dataSize);

                    record.loadFromArray(arr);

                    RightSideEvents.Add(record);
                }

                startRecordID += (uint)recievedRecordsCount;
            }

            return RightSideEvents;
        }

        async Task<List<ChargeRecord>> readchargeCycles_synchRightx_old(UInt32 startRecordID, UInt32 endRecordID)
        {
            List<ChargeRecord> RightSideEvents = new List<ChargeRecord>();
            List<Tuple<CommunicationResult, byte[]>> result = new List<Tuple<CommunicationResult, byte[]>>();

            const int patchsize = 14;

            if (DeviceType != DeviceBaseType.MCB)
                return RightSideEvents;

            if (startRecordID > endRecordID)
                return RightSideEvents;

            UInt32 patchesCount = (endRecordID - startRecordID) / patchsize;

            if ((endRecordID - startRecordID) % patchsize != 0)
                patchesCount++;

            if (patchesCount > 10)
                patchesCount = 10;

            List<CommandObject> commands = new List<CommandObject>();

            for (int i = 0; i < patchesCount; i++)
            {
                byte[] passData = new byte[5];

                Array.Copy(BitConverter.GetBytes(startRecordID + i * patchsize), passData, 4);
                passData[4] = patchsize;

                CommandObject commandObject = new CommandObject()
                {
                    CommandBytes = ReadRecordCommand,
                    Data = passData,
                    ExpectedSize = 0,
                    VerifyExpectedSize = false
                };

                commands.Add(commandObject);
            }

            try
            {
                await ForceDelay();

                result = await myCommunicator.MySendRecieveStacked(commands, SerialNumber, 0);
                LastSentCommandTime = DateTime.UtcNow;
            }
            catch
            {
                return RightSideEvents;
            }

            foreach (var item in result)
            {
                var resultArray = item.Item2;

                if (resultArray.Length < 2)
                    return RightSideEvents;

                if (resultArray[0] == 0x7F)
                    return RightSideEvents;

                int recoevedRecordsCount = resultArray[0];

                if (recoevedRecordsCount == 0)
                    return RightSideEvents;

                if (recoevedRecordsCount * ChargeRecord.dataSize + 1 != resultArray.Length)
                    return RightSideEvents;

                for (int i = 0; i < recoevedRecordsCount; i++)
                {
                    ChargeRecord record = new ChargeRecord();

                    byte[] arr = new byte[ChargeRecord.dataSize];
                    Array.Copy(resultArray, 1 + i * ChargeRecord.dataSize, arr, 0, ChargeRecord.dataSize);

                    record.loadFromArray(arr);

                    RightSideEvents.Add(record);

                }

                startRecordID += (uint)recoevedRecordsCount;
            }

            return RightSideEvents;
        }

        #endregion

        async Task<List<PMfault>> readPMFaults_synch(UInt32 startRecord_faultID, List<PMfault> SideEvents)
        {
            if (DeviceType != DeviceBaseType.MCB)
                return SideEvents;
            byte[] vars = new byte[5];
            Array.Copy(BitConverter.GetBytes(startRecord_faultID), vars, 4);
            vars[4] = 56;

            byte[] data = new byte[1];
            try
            {
                await ForceDelay();
                var tuple = await myCommunicator.MySendRecieve(USB_getPMErrorsCounter, vars, SerialNumber, 0, false, data);
                CommunicationResult result = tuple.Item1;
                data = tuple.Item2;
                LastSentCommandTime = DateTime.UtcNow;
                if (result != CommunicationResult.OK)
                    return SideEvents;
            }
            catch
            {
                return SideEvents;
            }
            finally
            {
                //mutex.ReleaseMutex();
            }
            if (data.Length == 0)
                return SideEvents;

            if (data[0] == 0x7F)
            {
                return SideEvents;
            }
            if (data[0] == 0)
                return SideEvents;
            //check size
            if (data[0] * PMfault.recordSize + 1 != data.Length)
                return SideEvents;

            for (int i = 1; i < data[0] * PMfault.recordSize + 1; i += PMfault.recordSize)
            {
                PMfault f = new PMfault();
                byte[] aRecord = new byte[PMfault.recordSize];
                Array.Copy(data, i, aRecord, 0, PMfault.recordSize);
                f.loadFromArray(aRecord, this.myZone, Config.daylightSaving);
                uint id = (uint)(startRecord_faultID + (i - 1) / PMfault.recordSize);
                f.verifyID(id);
                SideEvents.Add(f);

            }
            return SideEvents;
        }


        public async Task<List<PMfault>> readPMs_synchRight(UInt32 ResultRecordStartID)
        {

            List<PMfault> RightSideEvents = new List<PMfault>();
            UInt32 totalCount = 0;
            int fetchCl = 0;
            int maxF = 2;

            bool cont;
            do
            {
                totalCount = (UInt32)RightSideEvents.Count;
                RightSideEvents = await readPMFaults_synch(ResultRecordStartID + totalCount, RightSideEvents);
                cont = (RightSideEvents.Count != 0 && RightSideEvents.Count != totalCount &&
                    (RightSideEvents.Count % 14) == 0 && fetchCl < maxF);
                fetchCl++;
            } while (cont);

            return RightSideEvents;

        }

        public async Task<CommunicationResult> ReadWiFiInfo()
        {
            try
            {
                return await TryReadWiFiInfo();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());

                return CommunicationResult.internalFailure;
            }
        }

        async Task<CommunicationResult> TryReadWiFiInfo()
        {
            byte[] resultArray = new byte[1];
            CommunicationResult result;

            var tuple =
                await
                myCommunicator
                    .MySendRecieve
                    (WIFI_DEBUG_COMMAND, null, SerialNumber, 256, true, resultArray);

            result = tuple.Item1;
            resultArray = tuple.Item2;

            LastSentCommandTime = DateTime.UtcNow;

            if (result != CommunicationResult.OK)
                return result;

            WiFiInfo.LoadFromArray(resultArray, FirmwareRevision);

            return CommunicationResult.OK;
        }

        public void PrepareSiteViewConfig()
        {
            if (SiteViewConfig == null)
                SiteViewConfig = Config;
        }

        public void ResetSiteViewConfig()
        {
            SiteViewConfig = null;
        }

        #region PowerSnapshot

        List<PowerSnapshot> powerRecords = new List<PowerSnapshot>();

        public List<PowerSnapshot> GetPowerSnapShots()
        {
            lock (recordsLock)
            {
                List<PowerSnapshot> temp = new List<PowerSnapshot>();
                foreach (PowerSnapshot x in powerRecords)
                {
                    PowerSnapshot c = new PowerSnapshot(x);
                    temp.Add(c);
                }
                return temp;
            }
        }

        public async Task<CommunicationResult> ReadPowerSnapShotsLog(uint startRecord)
        {
            if (DeviceType != DeviceBaseType.MCB)
                return CommunicationResult.OK;

            powerRecords.Clear();

            byte[] vars = new byte[5];
            if (startRecord < MinChargePowerSnapShot)
                startRecord = MinChargePowerSnapShot;
            vars[4] = 56;

            int maxRead = 100;
            while (maxRead > 0)
            {

                if (startRecord >= globalRecord.totalPowerSnapShots)
                    break;
                if (startRecord + 56 > globalRecord.totalPowerSnapShots)
                {
                    vars[4] = (byte)(globalRecord.totalPowerSnapShots - startRecord);

                }
                Array.Copy(BitConverter.GetBytes(startRecord), vars, 4);
                if (!mutex.WaitOne(15000)) return CommunicationResult.mutexKilled;
                byte[] data = new byte[1];
                try
                {
                    await ForceDelay();

                    var result = await
                        myCommunicator
                            .MySendRecieve
                            (USB_PowerSnapshots, vars, SerialNumber, 0,
                             false, data);
                    data = result.Item2;

                    LastSentCommandTime = DateTime.UtcNow;
                    if (result.Item1 != CommunicationResult.OK)
                        return result.Item1;
                }
                catch
                {
                    return CommunicationResult.internalFailure;
                }
                finally
                {
                    mutex.ReleaseMutex();
                }
                if (data.Length == 0)
                    return CommunicationResult.SIZEERROR;

                if (data[0] == 0x7F)
                {
                    //starting point is Wrong
                    var status = await ReadRecordslimits();
                    if (status != CommunicationResult.OK)
                        return status;

                    startRecord = minChargePowerSnapShot;
                    Array.Copy(BitConverter.GetBytes(startRecord), vars, 4);

                    continue;
                }

                if (data[0] == 0)
                    break;
                //check size
                if (data[0] * PowerSnapshot.READ_SIZE + 1 != data.Length)
                    return CommunicationResult.SIZEERROR;

                for (int i = 1; i < data[0] * PowerSnapshot.READ_SIZE + 1; i += PowerSnapshot.READ_SIZE)
                {
                    maxRead--;
                    PowerSnapshot f = new PowerSnapshot();
                    byte[] aRecord = new byte[PowerSnapshot.READ_SIZE];
                    Array.Copy(data, i, aRecord, 0, PowerSnapshot.READ_SIZE);
                    f.LoadFromArray(aRecord);
                    uint id = (uint)(startRecord + (i - 1) / PowerSnapshot.READ_SIZE);
                    f.VerifyID(id - 1);
                    powerRecords.Add(f);

                }
                startRecord += data[0];


            }
            return CommunicationResult.OK;
        }

        #endregion

        public void SaveDefaultChargeProfile()
        {
            int type = Config.chargerType;
            string batteryTypeS = Config.batteryType;

            int batteryType = Array.IndexOf(MCBConfig.batteryTypes, batteryTypeS);

            Config.TRrate = 500;//FOR ALL it's 5%

            if (type != 1)
                Config.EQrate = 400;
            else
                Config.EQrate = 500;
            if (batteryType != 2)
                Config.FIrate = 500;
            else
            {
                Config.FIrate = 150;
                Config.EQrate = 100;
            }

            Config.useNewEastPennProfile = false;
            Config.FIStopCurrent = 0;

            Config.trickleVoltage = "2.0";
            Config.EQdaysMask = new DaysMask();
            Config.finishDaysMask = new DaysMask();
            Config.forceFinishTimeout = false;

            Config.EQtimer = "04:00";
            Config.desulfationTimer = "12:00";
            Config.finishDV = 5;
            Config.finishDT = "59";

            Config.FIstartWindow = "00:00";
            Config.EQstartWindow = "00:00";

            Config.EQwindow = "24:00";

            if (batteryType != 2)
            {
                Config.maxTemperatureFault = "54.4";
                Config.foldTemperature = "51.6";
                Config.coolDownTemperature = "46.1";
            }
            else
            {
                Config.maxTemperatureFault = "48.9";
                Config.foldTemperature = "46.1";
                Config.coolDownTemperature = "40.6";
            }

            switch (batteryType)
            {
                case 0:
                    Config.cc_ramping_min_steps = 0;
                    Config.FIvoltage = "2.6";
                    Config.EQvoltage = "2.65";
                    Config.CVfinishCurrent = 24;
                    Config.CVtimer = "04:00";
                    Config.finishTimer = "03:00";
                    Config.CVcurrentStep = 0;
                    Config.finishWindow = "24:00";
                    Config.FIschedulingMode = false;
                    Config.finishDaysMask.Saturday = true;
                    Config.finishDaysMask.Sunday = true;
                    Config.finishDaysMask.Monday = true;
                    Config.finishDaysMask.Tuesday = true;
                    Config.finishDaysMask.Wednesday = true;
                    Config.finishDaysMask.Thursday = true;
                    Config.finishDaysMask.Friday = true;
                    Config.TRrate = 500;

                    if (type == 0)
                    {
                        //FAST
                        Config.CVvoltage = "2.42";
                        Config.CCrate = 4000;

                        Config.EQdaysMask.Saturday = false;
                        Config.EQdaysMask.Sunday = true;
                        Config.EQdaysMask.Monday = false;
                        Config.EQdaysMask.Tuesday = false;
                        Config.EQdaysMask.Wednesday = false;
                        Config.EQdaysMask.Thursday = false;
                        Config.EQdaysMask.Friday = false;
                        Config.FIschedulingMode = true;

                    }
                    else if (type == 1)
                    {
                        //Conventional
                        Config.CVvoltage = "2.37";
                        Config.CCrate = 1700;


                        Config.EQdaysMask.Saturday = false;
                        Config.EQdaysMask.Sunday = true;
                        Config.EQdaysMask.Monday = false;
                        Config.EQdaysMask.Tuesday = false;
                        Config.EQdaysMask.Wednesday = false;
                        Config.EQdaysMask.Thursday = false;
                        Config.EQdaysMask.Friday = false;

                    }
                    else if (type == 2)
                    {
                        //Opp
                        Config.CVvoltage = "2.4";
                        Config.CCrate = 2500;


                        Config.EQdaysMask.Saturday = false;
                        Config.EQdaysMask.Sunday = true;
                        Config.EQdaysMask.Monday = false;
                        Config.EQdaysMask.Tuesday = false;
                        Config.EQdaysMask.Wednesday = false;
                        Config.EQdaysMask.Thursday = false;
                        Config.EQdaysMask.Friday = false;
                    }

                    break;
                case 1:
                    Config.cc_ramping_min_steps = 60;

                    Config.FIvoltage = "2.34";
                    Config.EQvoltage = "2.34";

                    Config.CVfinishCurrent = 20;
                    Config.CVtimer = "04:00";
                    Config.finishTimer = "03:00";
                    Config.CVcurrentStep = 2;
                    Config.CVvoltage = "2.3";
                    Config.trickleVoltage = "2.2";
                    Config.TRrate = 4000;


                    Config.CCrate = 5000;
                    Config.FIschedulingMode = false;
                    Config.finishDaysMask.Saturday = true;
                    Config.finishDaysMask.Sunday = true;
                    Config.finishDaysMask.Monday = true;
                    Config.finishDaysMask.Tuesday = true;
                    Config.finishDaysMask.Wednesday = true;
                    Config.finishDaysMask.Thursday = true;
                    Config.finishDaysMask.Friday = true;

                    Config.EQdaysMask.Saturday = false;
                    Config.EQdaysMask.Sunday = false;
                    Config.EQdaysMask.Monday = false;
                    Config.EQdaysMask.Tuesday = false;
                    Config.EQdaysMask.Wednesday = false;
                    Config.EQdaysMask.Thursday = false;
                    Config.EQdaysMask.Friday = false;

                    Config.finishWindow = "24:00";

                    break;

                case 2:
                    Config.cc_ramping_min_steps = 0;
                    Config.FIvoltage = "2.55";
                    Config.EQvoltage = "2.55";
                    Config.CVfinishCurrent = 16;
                    Config.CVtimer = "03:00";
                    Config.finishTimer = "04:00";
                    Config.CVcurrentStep = 0;

                    Config.CVvoltage = "2.33";
                    Config.CCrate = 1700;
                    Config.FIschedulingMode = false;
                    Config.finishDaysMask.Saturday = true;
                    Config.finishDaysMask.Sunday = true;
                    Config.finishDaysMask.Monday = true;
                    Config.finishDaysMask.Tuesday = true;
                    Config.finishDaysMask.Wednesday = true;
                    Config.finishDaysMask.Thursday = true;
                    Config.finishDaysMask.Friday = true;

                    Config.EQdaysMask.Saturday = false;
                    Config.EQdaysMask.Sunday = false;
                    Config.EQdaysMask.Monday = false;
                    Config.EQdaysMask.Tuesday = false;
                    Config.EQdaysMask.Wednesday = false;
                    Config.EQdaysMask.Thursday = false;
                    Config.EQdaysMask.Friday = false;

                    Config.finishWindow = "24:00";
                    Config.TRrate = 500;

                    break;
            }
        }

        internal override bool IsCommissioned()
        {
            if (Config == null)
                return false;

            if (string.IsNullOrEmpty(Config.id))
                return false;

            int idNumber = 0;

            if (!int.TryParse(Config.id, out idNumber))
                return false;

            return idNumber > MIN_COMMISSIONED_ID;
        }
    }
}
