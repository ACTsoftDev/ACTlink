using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using MvvmCross.Platform;

namespace actchargers
{
    public class Communicator
    {
        public event EventHandler OnProgressCompletedChanged;
        public event EventHandler<Tuple<byte, int>> OnLastSucceededIndexChanged;

        bool hasToCancel;

        public string CurrentSerialNumber { get; private set; }

        ConnectionTypesEnum connectionType;
        bool allowMCB;
        bool allowBattView;

        object myLock;

        int progressCompleted;
        public int ProgressCompleted
        {
            get
            {
                return progressCompleted;
            }
            private set
            {
                progressCompleted = value;

                FireOnProgressCompletedChanged();
            }
        }

        void FireOnProgressCompletedChanged()
        {
            OnProgressCompletedChanged?.Invoke(this, EventArgs.Empty);
        }

        int GetProgressCompleted(int progress, int max)
        {
            if (max == 0)
                return 0;
            if (progress == 0)
                return 0;

            return (int)(progress / (double)max * 100.0);
        }

        void FireOnLastSucceededIndexChanged(Tuple<byte, int> item)
        {
            OnLastSucceededIndexChanged?.Invoke(this, item);
        }

        public RouterInterface _router;
        public RouterInterface Router
        {
            get
            {
                lock (myLock)
                {
                    return _router;
                }
            }
            set
            {
                lock (myLock)
                {
                    _router = value;
                }
            }
        }

        public Communicator
        (ConnectionTypesEnum connectionTypeX, bool allowMCB,
         bool allowBattView)
        {
            myLock = new object();
            connectionType = connectionTypeX;
            this.allowMCB = allowMCB;
            this.allowBattView = allowBattView;

            if (connectionTypeX == ConnectionTypesEnum.ROUTER)
            {
                Router = new RouterInterface(allowBattView, allowMCB);

                Router.OnProgressCompletedChanged += Router_OnProgressCompletedChanged;

                Router.OnLastSucceededIndexChanged += Router_OnLastSucceededIndexChanged;
            }
        }

        public async Task<ValidateDeviceResponse> IsThisAValidDevice(string serialNumber)
        {
            ValidateDeviceResponse response = new ValidateDeviceResponse();
            byte[] resultArr = new byte[1];

            var tuple =
                await MySendRecieve
                (CommProtocol.defineCommand, null, serialNumber,
                 0, false, resultArr);

            CommunicationResult result = tuple.Item1;
            resultArr = tuple.Item2;
            response.communicateResult = result;
            if (result != CommunicationResult.OK)
                return response;
            if (resultArr.Length == 0)
            {
                response.communicateResult = CommunicationResult.SIZEERROR;
                return response;
            }
            if (allowMCB && resultArr[0] == CommProtocol.chargerDefineKey)
            {
                response.isCharger = true;
                response.deviceType = DeviceBaseType.MCB;
            }
            else if (allowBattView && resultArr[0] == CommProtocol.battViewDefineKey)
            {
                response.isCharger = false;
                response.deviceType = DeviceBaseType.BATTVIEW;
            }
            else if (allowBattView && resultArr[0] == CommProtocol.CalibratorDefineKey)
            {
                response.isCharger = true;
                response.deviceType = DeviceBaseType.CALIBRATOR;
            }
            else
            {
                response.communicateResult = CommunicationResult.NOT_EXIST;
                if (ControlObject.isDebugMaster)
                    Logger.AddLog(false, "3)");
            }
            return response;
        }

        internal void RemoveIP(string ip)
        {
            if (connectionType == ConnectionTypesEnum.ROUTER)
                Router.RemoveIP(ip);
        }

        public Dictionary<string, DefineObjectInfo> GetSerialNumbersWithDefineInfo()
        {
            if (connectionType == ConnectionTypesEnum.ROUTER)
            {
                return Router.GetSerialNumbersWithDefineInfo();
            }
            else
                return new Dictionary<string, DefineObjectInfo>();
        }

        public string[] GetDevicesSerialNumbers()
        {
            try
            {
                if (connectionType == ConnectionTypesEnum.USB)
                {
                    string[] devices = Mvx.Resolve<IUSBInterface>().GetDevicesSerialNumbers();
                    return devices;
                }

                if (connectionType == ConnectionTypesEnum.ROUTER)
                {
                    return Router.GetSerialNumbers();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
            return new string[0];
        }

        void Router_OnProgressCompletedChanged(object sender, EventArgs e)
        {
            ProgressCompleted = Router.ProgressCompleted;
        }

        void Router_OnLastSucceededIndexChanged(object sender, Tuple<byte, int> e)
        {
            FireOnLastSucceededIndexChanged(e);
        }

        #region My Send Recieve

        internal async Task<Tuple<CommunicationResult, byte[]>> MySendRecieve
        (byte cmd, byte[] data, string serialNumber, int expectedSize,
         bool verifyExpectedSize, byte[] resultArray,
         TimeoutLevel timeout = TimeoutLevel.normal, bool allowToRepeat = true)
        {
            if (connectionType == ConnectionTypesEnum.UPLOAD_NOT_CONNECT
                || ControlObject.isSynchSitesForm)
            {
                return new Tuple<CommunicationResult, byte[]>
                    (CommunicationResult.NOT_EXIST, resultArray);
            }

            CommunicationResult communicationResult =
                CommunicationResult.ACCESS_ERROR;
            try
            {
                CurrentSerialNumber = serialNumber;

                byte[] readData = new byte[0];
                if (connectionType == ConnectionTypesEnum.USB)
                {
                    communicationResult =
                        Mvx.Resolve<IUSBInterface>()
                           .SendReceive(cmd, data, serialNumber, expectedSize,
                                        verifyExpectedSize, ref readData, timeout);
                }

                if (connectionType == ConnectionTypesEnum.ROUTER)
                {
                    var tuple =
                        await Router
                            .MySendRecieve
                            (serialNumber, cmd, data, expectedSize,
                             verifyExpectedSize, readData, timeout);

                    communicationResult = tuple.Item1;
                    readData = tuple.Item2;
                }

                UInt16 length;

                if ((communicationResult == CommunicationResult.OK
                     || communicationResult == CommunicationResult.CHARGER_BUSY
                     || communicationResult == CommunicationResult.COMMAND_DELAYED))
                {
                    if (connectionType == ConnectionTypesEnum.USB)
                    {
                        length = BitConverter.ToUInt16(readData, 0);
                        resultArray =
                            new byte
                            [length + 2 - CommProtocol.MIN_EXPECTED_PACKET_SIZE];

                        Array.Copy(readData, 4, resultArray, 0, resultArray.Length);
                    }
                    else
                    {
                        resultArray = new byte[readData.Length];

                        Array.Copy(readData, 0, resultArray, 0, resultArray.Length);
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.AddLog(true, "X40" + ex);
            }

            if (communicationResult == CommunicationResult.FTDI_OPENING_ERROR
                && allowToRepeat)
            {
                await Task.Delay(ACConstants.SENDING_DELAY);

                var tuple =
                    await MySendRecieve
                    (cmd, data, serialNumber, expectedSize, verifyExpectedSize,
                     resultArray, timeout, false);

                return new Tuple<CommunicationResult, byte[]>
                    (tuple.Item1, tuple.Item2);
            }

            return new Tuple<CommunicationResult, byte[]>
                (communicationResult, resultArray);
        }

        public async Task<List<Tuple<CommunicationResult, byte[]>>>
        MySendRecieveStacked
        (List<CommandObject> commands, string serialNumber,
         int startFrom, TimeoutLevel timeoutLevel = TimeoutLevel.normal)
        {
            hasToCancel = false;

            CurrentSerialNumber = serialNumber;

            List<Tuple<CommunicationResult, byte[]>> finalResult =
                new List<Tuple<CommunicationResult, byte[]>>();

            if (connectionType == ConnectionTypesEnum.UPLOAD_NOT_CONNECT
                || ControlObject.isSynchSitesForm)
                return finalResult;

            if (ConsecutiveCommands())
            {
                finalResult = await UsbMySendRecieveStacked
                    (commands, serialNumber, startFrom, timeoutLevel);
            }
            else
            {
                finalResult = await WifiMySendRecieveStacked
                    (commands, serialNumber, startFrom, timeoutLevel);
            }

            if (hasToCancel)
            {
                finalResult = new List<Tuple<CommunicationResult, byte[]>>();

                hasToCancel = false;
            }

            return finalResult;
        }

        bool ConsecutiveCommands()
        {
            return !IsWifi() || CrossDeviceInfoManager.IsAndroid();
        }

        bool IsWifi()
        {
            bool isWifi = (connectionType == ConnectionTypesEnum.ROUTER);

            return isWifi;
        }

        async Task<List<Tuple<CommunicationResult, byte[]>>> WifiMySendRecieveStacked
        (List<CommandObject> commands, string serialNumber, int startFrom, TimeoutLevel timeoutLevel)
        {
            List<Tuple<CommunicationResult, byte[]>> finalResult =
                await Router.MySendRecieveStacked
                            (serialNumber, commands, startFrom, timeoutLevel);

            return finalResult;
        }

        async Task<List<Tuple<CommunicationResult, byte[]>>> UsbMySendRecieveStacked
        (List<CommandObject> commands, string serialNumber, int startFrom, TimeoutLevel timeoutLevel)
        {
            List<Tuple<CommunicationResult, byte[]>> finalResult =
                new List<Tuple<CommunicationResult, byte[]>>();

            if (commands == null)
                return finalResult;

            int count = commands.Count;

            int firstIndex = GetFirstIndex(startFrom, count);

            for (int i = firstIndex; i < count; i++)
            {
                if (hasToCancel)
                {
                    break;
                }

                var commandItem = commands[i];
                Tuple<CommunicationResult, byte[]> result =
                    await SendRecieveOneCommand
                    (commandItem, serialNumber, timeoutLevel);

                finalResult.Add(result);

                if (result.Item1 == CommunicationResult.OK)
                    OnCommandSucceeded(commandItem, i, count);
                else
                    Debug.WriteLine("Not OK the status is: " + result.Item1);
            }

            return finalResult;
        }

        int GetFirstIndex(int startFrom, int count)
        {
            int firstIndex;

            if (startFrom <= 0 || startFrom >= count - 1)
                firstIndex = 0;
            else
                firstIndex = startFrom + 1;

            return firstIndex;
        }

        async Task<Tuple<CommunicationResult, byte[]>> SendRecieveOneCommand
        (CommandObject command, string serialNumber, TimeoutLevel timeoutLevel)
        {
            var commandByte = command.CommandBytes;
            var data = command.Data;
            int expectedSize = command.ExpectedSize;
            bool verifyExpectedSize = command.VerifyExpectedSize;
            byte[] resultArray = new byte[0];

            var result = await MySendRecieve
                (commandByte, data, serialNumber, expectedSize, verifyExpectedSize,
                 resultArray, timeoutLevel);

            return result;
        }

        void OnCommandSucceeded(CommandObject command, int index, int count)
        {
            ProgressCompleted = GetProgressCompleted(index + 1, count);

            if (command.SaveLastSucceededIndex)
                FireOnLastSucceededIndexChanged(new Tuple<byte, int>(command.CommandBytes, index));
        }

        #endregion

        internal void RequestSoftDisconnect(string sn)
        {
            if (connectionType == ConnectionTypesEnum.UPLOAD_NOT_CONNECT
                || ControlObject.isSynchSitesForm)
                return;
            if (connectionType == ConnectionTypesEnum.USB)
            {
                Mvx.Resolve<IUSBInterface>().RequestSoftDisconnect(sn);
            }

            if (connectionType == ConnectionTypesEnum.ROUTER)
            {
                Router.RequestSoftKill(sn);
            }
        }

        public void StopScan()
        {
            if (connectionType == ConnectionTypesEnum.ROUTER)
                RouterStopScan();
        }

        void RouterStopScan()
        {
            Router.InitialiseTCPToken();
            Close();
            Router.ResetProgress();
        }

        public void Close()
        {
            if (connectionType == ConnectionTypesEnum.ROUTER)
                Router.Close();
        }

        public void Cancel()
        {
            hasToCancel = true;

            if (Router != null)
            {
                Router.Cancel();
            }
        }
    }
}
