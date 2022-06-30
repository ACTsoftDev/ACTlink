using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace actchargers
{
    public abstract class DeviceObjectParent
    {
        internal const int MIN_COMMISSIONED_ID = 10000;

        public const int esp32_patchsize = 128;

        const byte ESP32_FWupdate = 0xE0;
        const byte ESP32_FWexec = 0xE1;
        const byte ESP32_Read_Debug_COMMAND = 0xE3;
        const byte ESP32_DspicFWupdate = 0xF0;
        const byte ESP32_DspicFWexec = 0xF1;

        internal const byte ESP32_readRecords = 0xF2;

        public event EventHandler OnFirmwareUpdateStepChanged;
        public event EventHandler OnProgressCompletedChanged;

        protected object myLock;
        protected object recordsLock;

        internal readonly Mutex mutex = new Mutex(false);

        protected ConnectionManager connectionManager;
        protected Communicator myCommunicator;

        FirmwareUpdateStage firmwareUpdateStep;
        public FirmwareUpdateStage FirmwareUpdateStep
        {
            get
            {
                return firmwareUpdateStep;
            }
            set
            {
                firmwareUpdateStep = value;

                FireOnFirmwareUpdateStepChanged();
            }
        }

        DeviceBaseType _deviceType;
        public DeviceBaseType DeviceType
        {
            get
            {
                lock (myLock)
                {
                    return _deviceType;
                }
            }
            set
            {
                lock (myLock)
                {
                    _deviceType = value;
                }
            }
        }

        bool _requireFirmwareUpdate;
        public bool RequireFirmwareUpdate
        {
            get
            {
                lock (myLock)
                {
                    return _requireFirmwareUpdate;
                }
            }
            set
            {
                lock (myLock)
                {
                    _requireFirmwareUpdate = value;
                }
            }
        }

        bool _requireFirmwareWiFiUpdate;
        public bool RequireFirmwareWiFiUpdate
        {
            get
            {
                lock (myLock)
                {
                    return _requireFirmwareWiFiUpdate;
                }
            }
            set
            {
                lock (myLock)
                {
                    _requireFirmwareWiFiUpdate = value;
                }
            }
        }

        bool _requireFirmwareDcUpdate;
        public bool RequireFirmwareDcUpdate
        {
            get
            {
                lock (myLock)
                {
                    return _requireFirmwareDcUpdate;
                }
            }
            set
            {
                lock (myLock)
                {
                    _requireFirmwareDcUpdate = value;
                }
            }
        }

        public bool RequireAllFirmwareUpdates
        {
            get
            {
                lock (myLock)
                {
                    return RequireFirmwareUpdate && RequireFirmwareWiFiUpdate && RequireFirmwareDcUpdate;
                }
            }
        }

        public bool NotRequireAnyFirmwareUpdates
        {
            get
            {
                lock (myLock)
                {
                    return !RequireFirmwareUpdate && !RequireFirmwareWiFiUpdate && !RequireFirmwareDcUpdate;
                }
            }
        }

        string _serialNumber;
        public string SerialNumber
        {
            get
            {
                lock (recordsLock)
                {
                    return _serialNumber;
                }
            }
            set
            {
                lock (recordsLock)
                {
                    _serialNumber = value;
                }
            }
        }

        public string IPAddress
        {
            get;
            set;
        }

        DateTime _lastSentCommandTime;
        internal DateTime LastSentCommandTime
        {
            get
            {
                lock (myLock)
                {
                    return new DateTime(_lastSentCommandTime.Ticks);
                }
            }
            set
            {
                lock (myLock)
                {
                    _lastSentCommandTime = value;
                }
            }
        }

        public bool RequireRefresh
        {
            get;
            set;
        }

        int progressCompleted;
        public int ProgressCompleted
        {
            get
            {
                return progressCompleted;
            }
            set
            {
                progressCompleted = value;

                FireOnProgressCompletedChanged();
            }
        }

        float _firmwareRevision;
        public float FirmwareRevision
        {
            get
            {
                lock (myLock)
                {
                    return _firmwareRevision;
                }
            }
            set
            {
                lock (myLock)
                {
                    _firmwareRevision = value;
                }
            }
        }

        float _firmwareWiFiVersion;
        public float FirmwareWiFiVersion
        {
            get
            {
                lock (myLock)
                {
                    return _firmwareWiFiVersion;
                }
            }
            set
            {
                lock (myLock)
                {
                    _firmwareWiFiVersion = value;
                }
            }
        }

        byte _dcId;
        public byte DcId
        {
            get
            {
                lock (myLock)
                {
                    return _dcId;
                }
            }
            set
            {
                lock (myLock)
                {
                    _dcId = value;
                }
            }
        }

        float _firmwareDcVersion;
        public float FirmwareDcVersion
        {
            get
            {
                lock (myLock)
                {
                    return _firmwareDcVersion;
                }
            }
            set
            {
                lock (myLock)
                {
                    _firmwareDcVersion = value;
                }
            }
        }

        public bool IsEsp32WiFi
        {
            get
            {
                lock (myLock)
                {
                    return FirmwareWiFiVersion > 0;
                }
            }
        }

        protected DeviceObjectParent(string serialNumber, ConnectionManager connectionManager)
        {
            myLock = new object();
            recordsLock = new object();

            SerialNumber = serialNumber;
            this.connectionManager = connectionManager;
            myCommunicator = connectionManager.MyCommunicator;

            if (this.myCommunicator != null)
                InitEvents();
        }

        void InitEvents()
        {
            myCommunicator.OnProgressCompletedChanged += MyCommunicator_OnProgressCompletedChanged;
            myCommunicator.OnLastSucceededIndexChanged += MyCommunicator_OnLastSucceededIndexChanged;
        }

        void MyCommunicator_OnProgressCompletedChanged(object sender, EventArgs e)
        {
            if (AmITheSender(sender))
                ProgressCompleted = myCommunicator.ProgressCompleted;
        }

        void MyCommunicator_OnLastSucceededIndexChanged(object sender, Tuple<byte, int> e)
        {
            if (AmITheSender(sender))
                SetLastSucceededIndex(e);
        }

        bool AmITheSender(object sender)
        {
            try
            {
                return TryAmITheSender(sender);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());

                return false;
            }
        }

        bool TryAmITheSender(object sender)
        {
            Communicator communicator = sender as Communicator;

            if (communicator == null)
                return false;

            return SerialNumber == communicator.CurrentSerialNumber;
        }

        internal abstract bool IsCommissioned();

        void FireOnProgressCompletedChanged()
        {
            OnProgressCompletedChanged?.Invoke(this, EventArgs.Empty);
        }

        void FireOnFirmwareUpdateStepChanged()
        {
            OnFirmwareUpdateStepChanged?.Invoke(this, EventArgs.Empty);
        }

        public abstract void Cancel();

        internal int GetLastSucceededIndex(byte command)
        {
            return DbSingleton
                .DBManagerServiceInstance
                .GetIncompleteDownloadLoader()
                .GetLastSucceededIndexsingSerialNumber(command, SerialNumber);
        }

        internal void SetLastSucceededIndex(Tuple<byte, int> item)
        {
            DbSingleton
                .DBManagerServiceInstance
                .GetIncompleteDownloadLoader()
                .InsertOrUpdateUsingSerialNumber(SerialNumber, item.Item1, item.Item2);
        }

        internal void RemoveLastSucceededIndex(byte command)
        {
            DbSingleton
                .DBManagerServiceInstance
                .GetIncompleteDownloadLoader()
                .DeleteUsingSerialNumber(command, SerialNumber);
        }

        #region Update

        public async Task<Tuple<bool, string>> SiteViewUpdate()
        {
            Tuple<bool, string> result = new Tuple<bool, string>(false, "");

            if (RequireAllFirmwareUpdates)
            {
                result = await UpdateAll();
            }
            else if (NotRequireAnyFirmwareUpdates)
            {
                FirmwareUpdateStep = FirmwareUpdateStage.updateIsNotNeeded;

                result = new Tuple<bool, string>(false, AppResources.device_has_latest_firmware);

                return result;
            }
            else
            {
                bool dcRestart = !RequireFirmwareUpdate && !RequireFirmwareWiFiUpdate;
                bool wifiRestart = RequireFirmwareDcUpdate && !RequireFirmwareUpdate;

                if (RequireFirmwareDcUpdate)
                    result = await UpdateDcFirmware(DcId, dcRestart);

                if (RequireFirmwareUpdate)
                    result = await UpdateFirmware(RequireFirmwareWiFiUpdate);

                if (RequireFirmwareWiFiUpdate)
                    result = await UpdateWiFiFirmware(RequireFirmwareUpdate, wifiRestart);
            }

            if (result.Item1)
            {
                await Task.Delay(1000);

                FirmwareUpdateStep = FirmwareUpdateStage.updateCompleted;
            }
            else
            {
                FirmwareUpdateStep = FirmwareUpdateStage.FAILED;
            }

            return result;
        }

        async Task<Tuple<bool, string>> UpdateAll()
        {
            var result = await UpdateDcFirmware(DcId, false);

            if (result.Item1)
            {
                await Task.Delay(1000);

                result = await UpdateFirmware(true);
            }

            if (result.Item1)
            {
                await Task.Delay(1000);

                result = await UpdateWiFiFirmware(true, false);
            }

            return result;
        }

        public abstract Task<Tuple<bool, string>> UpdateFirmware(bool excecDspic);

        public async Task<CommunicationResult> WriteToBootLoaderFlash
        (byte[] data, int length)
        {
            List<Tuple<CommunicationResult, byte[]>> results;
            byte[] resArr = new byte[1];

            //foreach write block calculate CRC

            ushort crcblock;
            for (int block = 0; block < data.Length; block += 1536)
            {
                byte[] tempBlock = new byte[1536];
                Array.Copy(data, block, tempBlock, 0, 1536);
                crcblock = CommProtocol.CRCCalculation(tempBlock, tempBlock.Length);
                Array.Copy
                     (BitConverter.GetBytes(crcblock), 0, data,
                      data.Length - 768 + (2 * block / 1536), 2);
            }

            ushort CRC = CommProtocol.CRCCalculation(data, data.Length - 2);
            data[data.Length - 1] = (byte)CRC;
            data[data.Length - 2] = (byte)(CRC >> 8);

            int flashSize = IsEsp32WiFi ? 1024 * 8 : 1024;

            byte[] temp = new byte[flashSize + 4];

            var commands = new List<CommandObject>();
            for (int i = 0; i < data.Length / flashSize; i++)
            {
                try
                {
                    AppendUpdateFirmwareTempArray(i, flashSize, data, ref temp);

                    CommandObject command = CreateUpdateFirmwareCommand(temp);

                    commands.Add(command);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error in " + i + " is " + ex);
                }
            }

            if ((length % flashSize) != 0)
            {
                byte[] temp2 = new byte[length % flashSize + 4];
                Array.Copy(BitConverter.GetBytes((((UInt32)(length / flashSize))) * flashSize), 0, temp2, 0, 4);
                Array.Copy(data, ((length / flashSize)) * flashSize, temp2, 4, (length % flashSize));

                CommandObject command = CreateUpdateFirmwareCommand(temp2);

                commands.Add(command);
            }

            try
            {
                int lastSucceededIndex = GetLastSucceededIndex(commands[0].CommandBytes);
                results =
                    await myCommunicator
                        .MySendRecieveStacked(commands, SerialNumber, lastSucceededIndex, TimeoutLevel.extended);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());

                RemoveLastSucceededIndex(commands[0].CommandBytes);

                return CommunicationResult.internalFailure;
            }
            finally
            {
                LastSentCommandTime = DateTime.UtcNow;
            }

            CommunicationResult result = GetLastResult(results);

            if (result == CommunicationResult.OK)
                RemoveLastSucceededIndex(commands[0].CommandBytes);

            return result;
        }

        CommandObject CreateUpdateFirmwareCommand(byte[] temp)
        {
            byte noneEsp32UpdateFirmwareCommand = GetNoneEsp32UpdateFirmwareCommand();
            byte updateCommand = IsEsp32WiFi ? ESP32_DspicFWupdate : noneEsp32UpdateFirmwareCommand;

            CommandObject command = new CommandObject()
            {
                CommandBytes = updateCommand,
                Data = (byte[])temp.Clone(),
                ExpectedSize = 0,
                VerifyExpectedSize = true,
                ResultArray = new byte[1],
                SaveLastSucceededIndex = true
            };

            return command;
        }

        internal abstract byte GetNoneEsp32UpdateFirmwareCommand();

        public async Task<CommunicationResult> RequestBootLoaderUpdate(bool excecDspic)
        {
            if (excecDspic)
                return CommunicationResult.OK;

            await Task.Delay(500);

            byte noneEsp32RequestBootLoaderCommand = GetNoneEsp32RequestBootLoaderCommand();
            byte requestBootLoaderCommand = IsEsp32WiFi ? ESP32_DspicFWexec : noneEsp32RequestBootLoaderCommand;

            byte[] resultArray = new byte[1];

            byte[] passedData = GetRequestBootLoaderUpdatePassedData();

            try
            {
                await ForceDelay();

                var tuple = await myCommunicator
                    .MySendRecieve
                    (requestBootLoaderCommand, passedData, SerialNumber, 0, true, resultArray);
                CommunicationResult status = tuple.Item1;

                resultArray = tuple.Item2;
                LastSentCommandTime = DateTime.UtcNow;

                if (status != CommunicationResult.OK && status != CommunicationResult.COMMAND_DELAYED)
                {
                    await ForceDelay();
                    await Task.Delay(500);

                    tuple = await myCommunicator
                        .MySendRecieve
                        (requestBootLoaderCommand, passedData, SerialNumber, 0, true, resultArray);

                    status = tuple.Item1;
                    resultArray = tuple.Item2;

                    LastSentCommandTime = DateTime.UtcNow;
                }

                return status;
            }
            catch
            {
                return CommunicationResult.internalFailure;
            }
        }

        internal abstract byte GetNoneEsp32RequestBootLoaderCommand();

        internal abstract byte[] GetRequestBootLoaderUpdatePassedData();

        void AppendUpdateFirmwareTempArray(int i, int flashSize, byte[] data, ref byte[] temp)
        {
            temp = new byte[flashSize + 4];

            Array.Copy(BitConverter.GetBytes((UInt32)i * flashSize), 0, temp, 0, 4);
            Array.Copy(data, i * flashSize, temp, 4, flashSize);
        }

        public async Task<Tuple<bool, string>> UpdateWiFiFirmware(bool excecDspic, bool restart)
        {
            ProgressCompleted = 0;
            FirmwareUpdateStep = FirmwareUpdateStage.sendingRequest;
            string msg = "";

            bool result;

            Firmware firmwareManager = new Firmware();

            if (!Firmware.DeviceWiFiRequireUpdate(FirmwareWiFiVersion))
                return new Tuple<bool, string>(false, AppResources.device_has_latest_firmware);

            byte[] data = firmwareManager.UpdateWiFiFileBinary();

            var updateResult = await WriteWiFiFirmware(data, excecDspic);
            result = updateResult == CommunicationResult.OK;

            if (result)
            {
                msg = AppResources.device_is_reflushing_firmware;

                FirmwareUpdateStep = FirmwareUpdateStage.sentRequestPassed;

                if (restart)
                {
                    CommunicationResult status;

                    try
                    {
                        await Task.Delay(5000);

                        status = await RecyclePower();
                    }
                    catch (Exception ex)
                    {
                        Logger.AddLog(true, "XX344:" + ex);

                        status = CommunicationResult.internalFailure;
                    }
                    if (status == CommunicationResult.OK)
                    {
                        msg = "Restarting!";

                        connectionManager.ForceSoftDisconnectDevice(SerialNumber, false);
                    }
                    else if (status == CommunicationResult.CHARGER_BUSY)
                    {
                        msg = "Device is busy, the firmware update will take place automatically.";
                    }
                }

                connectionManager.ForceSoftDisconnectDevice(SerialNumber, false, true);
            }
            else
            {
                FirmwareUpdateStep = FirmwareUpdateStage.FAILED;
            }

            return new Tuple<bool, string>(result, msg);
        }

        internal abstract Task<CommunicationResult> RecyclePower();

        async Task<CommunicationResult> WriteWiFiFirmware(byte[] data, bool excecDspic)
        {
            byte FW_Update_CMD;
            byte FW_Excec_CMD;
            if (DeviceType == DeviceBaseType.blackboxInterface)
            {
                FW_Update_CMD = 0x04;
                FW_Excec_CMD = 0x05;
            }
            else
            {
                FW_Update_CMD = ESP32_FWupdate;
                FW_Excec_CMD = ESP32_FWexec;
            }

            const int FW_PACKET_SIZE = 8 * 1024;

            UInt32 crc32 = 0;

            List<CommandObject> cmdObjs = new List<CommandObject>();

            for (int i = 0; i < data.Length / FW_PACKET_SIZE; i++)
            {
                byte[] temp = new byte[FW_PACKET_SIZE + 8];

                Array.Copy(BitConverter.GetBytes(FW_PACKET_SIZE), 0, temp, 0, 4);
                Array.Copy(BitConverter.GetBytes((UInt32)i), 0, temp, 4, 4);
                Array.Copy(data, i * FW_PACKET_SIZE, temp, 8, FW_PACKET_SIZE);

                crc32 = Crc32_calc(crc32, data, (UInt32)(i * FW_PACKET_SIZE), FW_PACKET_SIZE);

                CommandObject commandObject = new CommandObject()
                {
                    CommandBytes = FW_Update_CMD,
                    Data = temp,
                    ExpectedSize = 0,
                    VerifyExpectedSize = true,
                    SaveLastSucceededIndex = true
                };

                cmdObjs.Add(commandObject);
            }
            if ((data.Length % FW_PACKET_SIZE) != 0)
            {
                byte[] temp2 = new byte[data.Length % FW_PACKET_SIZE + 8];

                Array.Copy(BitConverter.GetBytes(data.Length % FW_PACKET_SIZE), 0, temp2, 0, 4);
                Array.Copy(BitConverter.GetBytes((UInt32)cmdObjs.Count), 0, temp2, 4, 4);
                Array.Copy(data, cmdObjs.Count * FW_PACKET_SIZE, temp2, 8, data.Length % FW_PACKET_SIZE);
                crc32 = Crc32_calc(crc32, data, (UInt32)(cmdObjs.Count * FW_PACKET_SIZE), (UInt32)(data.Length % FW_PACKET_SIZE));

                CommandObject commandObject = new CommandObject()
                {
                    CommandBytes = FW_Update_CMD,
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

                await Task.Delay(500);

                if (lastResult.Item1 != CommunicationResult.OK)
                    return lastResult.Item1;

                byte[] resultArray = new byte[1];
                byte[] passedData = new byte[5];
                Array.Copy(BitConverter.GetBytes(crc32), passedData, 4);
                passedData[4] = (byte)(excecDspic ? 0xAA : 0x00);

                var oneResult = await myCommunicator.MySendRecieve(FW_Excec_CMD, passedData, SerialNumber, 0, false, resultArray, TimeoutLevel.extended);

                return oneResult.Item1;
            }
            catch
            {
                return CommunicationResult.internalFailure;
            }
        }

        public abstract Task<Tuple<bool, string>> UpdateDcFirmware(byte id, bool restart);

        static UInt32 Crc32_calc(UInt32 crc, byte[] data, UInt32 startIndex, UInt32 len)
        {
            UInt32 k, m, d;
            UInt32[] crc32_table = {
                0x00000000, 0x1db71064, 0x3b6e20c8, 0x26d930ac,
                0x76dc4190, 0x6b6b51f4, 0x4db26158, 0x5005713c,
                0xedb88320, 0xf00f9344, 0xd6d6a3e8, 0xcb61b38c,
                0x9b64c2b0, 0x86d3d2d4, 0xa00ae278, 0xbdbdf21c
              };
            for (k = 0; k < len; k++)
            {
                d = data[k + startIndex] ^ crc;
                m = crc32_table[d & 0x0f];

                d >>= 4;
                d ^= m;

                crc >>= 8;
                crc ^= m >> 4;
                crc ^= crc32_table[d & 0x0f];
            }
            return crc;
        }

        #endregion

        CommunicationResult GetLastResult
        (List<Tuple<CommunicationResult, byte[]>> results)
        {
            if (results == null)
                return CommunicationResult.internalFailure;

            if (results.Count == 0)
                return CommunicationResult.internalFailure;

            var lastItem = results.Last();

            return lastItem.Item1;
        }

        internal async Task ForceDelay()
        {
            double waitTime = 500;

            if (ControlObject.connectMethods.Connectiontype == ConnectionTypesEnum.USB)
                waitTime = 1;

            TimeSpan delay = new TimeSpan();
            delay = DateTime.UtcNow - LastSentCommandTime;

            if (delay.TotalMilliseconds >= waitTime)
                return;

            await Task.Delay((int)waitTime - (int)delay.TotalMilliseconds);
        }
    }
}
