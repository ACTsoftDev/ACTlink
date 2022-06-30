using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace actchargers
{
    public class BattViewObject : DeviceObjectParent, IDisposable
    {
        const float MIN_ACCEPTED_FIRMWARE_VERSION = 2.21f;

        #region Commands

        const byte testCommand = 0x01;
        const byte readADCCommand = 0x02;
        const byte readBattViewStatus = 0x02;
        const byte ReadConfigCommand = 0x04;
        const byte setConfigAllV1 = 0x05;
        const byte setConfigAllV2 = 0x06;

        const byte getChargeProfile = 0x07;
        const byte getRTC = 0x08;
        const byte setRTC = 0x09;
        const byte getFirmwareRev = 0x0A;
        const byte setAnalog = 0x0B;
        const byte getRunningEvent = 0x0C;
        const byte getGlobalRecord = 0x0D;
        const byte ResetGlobalRecord = 0x0E;
        const byte readEventsCMD = 0x0F;
        const byte getRunningRT = 0x10;
        const byte readRTsCMD = 0x11;
        const byte setSOC = 0x12;
        const byte readDebugRecordsCMD = 0x13;
        const byte requestUpdateFirmware = 0x15;
        const byte saveToNewfirmware = 0x16;
        const byte getRecordsLimits = 0x17;
        const byte setInternalBattViewIDCMD = 0x18;
        const byte restartBATTView = 0x19;
        const byte searchRecordByDate = 0x1A;
        const byte getQuyickViewCMD = 0x1B;
        const byte startNewStudyCMD = 0x1C;
        const byte getQuyickViewCMD2 = 0x2B;
        const byte send_PLC_CAL = 0x1D;
        const byte USB_WRITE_PLC_FLASH = 0x72;
        const byte USB_Request_PLC_Code_update = 0x73;
        const byte WIFI_DEBUG_COMMAND = 0x7B;

        #endregion

        #region dataObjects

        public AdcRawClass battViewAdcRawObject;
        public QuickView quickView;

        //formatAll

        List<DebugRecord> savedDebugRecords;
        object savedDebugRecordsLock;
        public List<DebugRecord> getDebugRecords()
        {
            List<DebugRecord> cpy = new List<DebugRecord>();
            lock (savedDebugRecordsLock)
            {

                foreach (DebugRecord r in savedDebugRecords)
                {
                    DebugRecord n = new DebugRecord();
                    n.loadFromDebugRecord(r);
                    cpy.Add(n);
                }

            }
            return cpy;
        }

        List<RealTimeRecord> RealTimeRecords;
        object savedRTRecordsLock;
        public List<RealTimeRecord> getRTRecords()
        {
            List<RealTimeRecord> cpy = new List<RealTimeRecord>();
            lock (savedRTRecordsLock)
            {

                foreach (RealTimeRecord r in RealTimeRecords)
                {
                    RealTimeRecord n = new RealTimeRecord();
                    n.loadFromRecord(r);
                    cpy.Add(n);
                }

            }
            return cpy;
        }
        RealTimeRecord savedRuuningRT;
        object savedRuuningRTLock;

        public RealTimeRecord getRunningRealTimeRecord()
        {
            RealTimeRecord cpy = new RealTimeRecord();
            lock (savedRuuningRTLock)
            {
                cpy.loadFromRecord(savedRuuningRT);

            }
            return cpy;
        }

        BattViewObjectEvent runningEvent;
        object runningEventLock;
        public BattViewObjectEvent getRunningEventObject()
        {
            lock (runningEventLock)
            {
                BattViewObjectEvent r = new BattViewObjectEvent();
                r.loadFromEvent(runningEvent);
                return r;
            }
        }


        List<BattViewObjectEvent> history_events;
        object history_eventsLock;


        public BattViewObjectGlobalRecord globalRecord;

        #endregion

        #region data object vrs

        public BattViewConfig Config { get; set; }

        public BattViewConfig SiteViewConfig { get; set; }

        public WiFiDebug WiFiInfo { get; set; }

        UInt32 _debugRecordsStartID;
        public UInt32 debugRecordsStartID
        {
            get
            {
                lock (recordsLock)
                {
                    return _debugRecordsStartID;
                }
            }
            set
            {
                lock (recordsLock)
                {
                    _debugRecordsStartID = value;
                }
            }
        }

        UInt32 _debugRecordsLastID;
        public UInt32 debugRecordsLastID
        {
            get
            {
                lock (recordsLock)
                {
                    return _debugRecordsLastID;
                }
            }
            set
            {
                lock (recordsLock)
                {
                    _debugRecordsLastID = value;
                }
            }
        }

        DateTime _debugRecordsStartIDTime;
        public DateTime debugRecordsStartIDTime
        {
            get
            {
                lock (recordsLock)
                {
                    return new DateTime(_debugRecordsStartIDTime.Ticks);
                }
            }
            set
            {
                lock (recordsLock)
                {
                    _debugRecordsStartIDTime = value;
                }
            }
        }
        DateTime _debugRecordsLastIDTime;
        public DateTime debugRecordsLastIDTime
        {
            get
            {
                lock (recordsLock)
                {
                    return new DateTime(_debugRecordsLastIDTime.Ticks);
                }
            }
            set
            {
                lock (recordsLock)
                {
                    _debugRecordsLastIDTime = value;
                }
            }
        }
        UInt32 _eventsRecordsStartID;
        public UInt32 eventsRecordsStartID
        {
            get
            {
                lock (recordsLock)
                {
                    return _eventsRecordsStartID;
                }
            }
            set
            {
                lock (recordsLock)
                {
                    _eventsRecordsStartID = value;
                }
            }
        }
        UInt32 _eventsRecordsLastID;
        public UInt32 eventsRecordsLastID
        {
            get
            {
                lock (recordsLock)
                {
                    return _eventsRecordsLastID;
                }
            }
            set
            {
                lock (recordsLock)
                {
                    _eventsRecordsLastID = value;
                }
            }
        }
        DateTime _eventsRecordsStartIDTime;
        public DateTime eventsRecordsStartIDTime
        {
            get
            {
                lock (recordsLock)
                {
                    return new DateTime(_eventsRecordsStartIDTime.Ticks);
                }
            }
            set
            {
                lock (recordsLock)
                {
                    _eventsRecordsStartIDTime = value;
                }
            }
        }
        DateTime _eventsRecordsLastIDTime;
        public DateTime eventsRecordsLastIDTime
        {
            get
            {
                lock (recordsLock)
                {
                    return new DateTime(_eventsRecordsLastIDTime.Ticks);
                }
            }
            set
            {
                lock (recordsLock)
                {
                    _eventsRecordsLastIDTime = value;
                }
            }
        }
        UInt32 _realtimeRecordsStartID;
        public UInt32 realtimeRecordsStartID
        {
            get
            {
                lock (recordsLock)
                {
                    return _realtimeRecordsStartID;
                }
            }
            set
            {
                lock (recordsLock)
                {
                    _realtimeRecordsStartID = value;
                }
            }
        }
        UInt32 _realtimeRecordsLastID;
        public UInt32 realtimeRecordsLastID
        {
            get
            {
                lock (recordsLock)
                {
                    return _realtimeRecordsLastID;
                }
            }
            set
            {
                lock (recordsLock)
                {
                    _realtimeRecordsLastID = value;
                }
            }
        }
        DateTime _realtimeRecordsStartIDTime;
        public DateTime realtimeRecordsStartIDTime
        {
            get
            {
                lock (recordsLock)
                {
                    return new DateTime(_realtimeRecordsStartIDTime.Ticks);
                }
            }
            set
            {
                lock (recordsLock)
                {
                    _realtimeRecordsStartIDTime = value;
                }
            }
        }
        DateTime _realtimeRecordsLastIDTime;
        public DateTime realtimeRecordsLastIDTime
        {
            get
            {
                lock (recordsLock)
                {
                    return new DateTime(_realtimeRecordsLastIDTime.Ticks);
                }
            }
            set
            {
                lock (recordsLock)
                {
                    _realtimeRecordsLastIDTime = value;
                }
            }
        }



        UInt32 _searchEventTimeID = 0;
        UInt32 searchEventTimeID
        {
            get
            {
                lock (recordsLock)
                {
                    return _searchEventTimeID;
                }
            }
            set
            {
                lock (recordsLock)
                {
                    _searchEventTimeID = value;
                }
            }
        }
        UInt32 _searchRTTimeID = 0;
        UInt32 searchRTTimeID
        {
            get
            {
                lock (recordsLock)
                {
                    return _searchRTTimeID;
                }
            }
            set
            {
                lock (recordsLock)
                {
                    _searchRTTimeID = value;
                }
            }
        }
        UInt32 _searchDebugTimeID = 0;
        UInt32 searchDebugTimeID
        {
            get
            {
                lock (recordsLock)
                {
                    return _searchDebugTimeID;
                }
            }
            set
            {
                lock (recordsLock)
                {
                    _searchDebugTimeID = value;
                }
            }
        }


        bool _timeLost;
        public bool timeLost
        {
            get
            {
                lock (recordsLock)
                {
                    return _timeLost;
                }
            }
            set
            {
                lock (recordsLock)
                {
                    _timeLost = value;
                }
            }
        }
        CommunicationResult _doLoadErrorCode;
        public CommunicationResult doLoadErrorCode
        {
            get
            {
                lock (recordsLock)
                {
                    return _doLoadErrorCode;
                }
            }
            set
            {
                lock (recordsLock)
                {
                    _doLoadErrorCode = value;
                }
            }
        }
        byte _myZone;
        public byte myZone
        {
            get
            {
                lock (recordsLock)
                {
                    return _myZone;
                }
            }
            set
            {
                lock (recordsLock)
                {
                    _myZone = value;
                }
            }
        }

        bool _deviceIsLoaded;

        public bool deviceIsLoaded
        {
            get
            {
                lock (recordsLock)
                {
                    return _deviceIsLoaded;
                }
            }
            set
            {
                lock (recordsLock)
                {
                    _deviceIsLoaded = value;
                }
            }
        }

        bool _configLoaded;

        public bool configLoaded
        {
            get
            {
                lock (recordsLock)
                {
                    return _configLoaded;
                }
            }
            set
            {
                lock (recordsLock)
                {
                    _configLoaded = value;
                }
            }
        }

        #endregion

        public BattViewObject(string serialNumber, ConnectionManager connectionManager)
            : base(serialNumber, connectionManager)
        {
            battViewAdcRawObject = new AdcRawClass();
            Config = new BattViewConfig();
            WiFiInfo = new WiFiDebug(MIN_ACCEPTED_FIRMWARE_VERSION);
            history_events = new List<BattViewObjectEvent>();
            runningEventLock = new object();
            history_eventsLock = new object();
            RealTimeRecords = new List<RealTimeRecord>();
            savedRTRecordsLock = new object();
            savedDebugRecords = new List<DebugRecord>();
            savedDebugRecordsLock = new object();
            quickView = new QuickView();
            savedRuuningRTLock = new object();
            deviceIsLoaded = false;
        }

        public CommunicationResult getDoLoadStatus()
        {
            return doLoadErrorCode;
        }


        public void doLazyLoad
        (string sn, UInt32 myId, float firmwareVersion, float firmwareWiFiVersion,
         byte zoneID, bool isRTCLost, UInt32 studyID, bool replacmentPart,
         string name, string ipAddress)
        {
            this.Config.battViewSN = sn;
            this.Config.id = myId;
            this.FirmwareRevision = firmwareVersion;
            this.FirmwareWiFiVersion = firmwareWiFiVersion;
            this.myZone = zoneID;
            this.timeLost = isRTCLost;
            this.Config.studyId = studyID;
            this.Config.batteryID = name;
            this.Config.replacmentPart = replacmentPart;
            deviceIsLoaded = false;
            this.IPAddress = ipAddress;
        }

        public BattViewConfig getCopyOfArrayForReplacment(byte[] arr)
        {
            BattViewConfig temp = new BattViewConfig();
            temp.LoadFromArray(arr, this.FirmwareRevision);
            //we have some fields that shoulf be loaded from the original config;
            temp.id = Config.id;
            temp.battViewSN = Config.battViewSN;
            temp.tempFa = Config.tempFa;
            temp.tempFb = Config.tempFb;
            temp.tempFc = Config.tempFc;
            temp.intercellCoefficient = Config.intercellCoefficient;
            temp.voltageCalA = Config.voltageCalA;
            temp.voltageCalB = Config.voltageCalB;
            temp.HWversion = Config.HWversion;

            temp.NTCcalA = Config.NTCcalA;
            temp.NTCcalB = Config.NTCcalB;
            temp.currentCalA = Config.currentCalA;
            temp.currentCalB = Config.currentCalB;
            temp.intercellTemperatureCALa = Config.intercellTemperatureCALa;
            temp.intercellTemperatureCALb = Config.intercellTemperatureCALb;
            temp.currentClamp2CalA = Config.currentClamp2CalA;
            temp.currentClamp2CalB = Config.currentClamp2CalB;
            temp.currentClampCalA = Config.currentClampCalA;
            temp.currentClampCalB = Config.currentClampCalB;
            temp.enableElectrolyeSensing = Config.enableElectrolyeSensing;
            temp.enableHallEffectSensing = Config.enableHallEffectSensing;
            temp.enableExtTempSensing = Config.enableExtTempSensing;
            temp.enablePLC = Config.enablePLC;
            temp.temperatureControl = Config.temperatureControl;
            temp.disableIntercell = Config.disableIntercell;
            temp.battviewVersion = Config.battviewVersion;
            return temp;
        }

        async Task<bool> doLoadInternal(bool lazyLoading, float firmwareVersion, byte zoneID, bool isRTCLost)
        {
            Tuple<CommunicationResult, byte[]> tuple;
            if (!lazyLoading)
            {
                tuple = await this.TestConnection();
                doLoadErrorCode = tuple.Item1;
                if (doLoadErrorCode != CommunicationResult.OK)
                {
                    return false;
                }
            }
            if (lazyLoading)
            {
                this.FirmwareRevision = firmwareVersion;
            }
            else
            {
                tuple = await this.readFirmwareRev();
                doLoadErrorCode = tuple.Item1;
                if (doLoadErrorCode != CommunicationResult.OK)
                {
                    return false;
                }
            }

            if (lazyLoading)
            {
                this.myZone = zoneID;
            }
            else
            {
                tuple = await this.readTime();
                doLoadErrorCode = tuple.Item1;
                if (doLoadErrorCode != CommunicationResult.OK)
                {
                    return false;
                }
            }
            tuple = await this.ReadConfig();
            doLoadErrorCode = tuple.Item1;

            if (doLoadErrorCode != CommunicationResult.OK)
            {
                return false;
            }
            tuple = await this.readGlobalRecord();
            doLoadErrorCode = tuple.Item1;
            if (doLoadErrorCode != CommunicationResult.OK)
            {
                return false;
            }



            if (lazyLoading)
            {
                this.timeLost = isRTCLost;
            }
            else
            {
                tuple = await this.SendDefineCommand();
                doLoadErrorCode = tuple.Item1;
                if (doLoadErrorCode != CommunicationResult.OK)
                {
                    return false;
                }
            }



            tuple = await this.ReadRecordsLimits();
            doLoadErrorCode = tuple.Item1;
            if (doLoadErrorCode != CommunicationResult.OK)
            {
                return false;
            }


            tuple = await getQuickView();
            doLoadErrorCode = tuple.Item1;
            if (doLoadErrorCode != CommunicationResult.OK)
            {
                return false;
            }


            //if (timeLost)
            //{
            //    doLoadErrorCode = saveTime();
            //    if (doLoadErrorCode != commProtocol.Communication_Result.OK)
            //    {
            //        return false;
            //    }

            //}
            if (!lazyLoading)
            {
                bool doAutoSave = false;
                if (this.Config.FIcloseWindow == this.Config.FIstartWindow)
                {
                    if (this.Config.FIcloseWindow == 0)
                        this.Config.FIcloseWindow = 86400;
                    else
                        this.Config.FIcloseWindow--;
                    doAutoSave = true;
                }
                if (Config.foldTemperature < 250)
                {
                    Config.TRTemperature = 100;
                    Config.foldTemperature = 516;
                    Config.coolDownTemperature = 461;
                    doAutoSave = true;
                }
                if (Config.intercellCoefficient > 0.0046f && Config.intercellCoefficient < 0.0048f)
                {
                    Config.intercellCoefficient = 0.004f;
                    doAutoSave = true;
                }
                if (Config.eventDetectTimeRangePercentage == 33 || Config.eventDetectTimeRangePercentage == 50)
                {
                    Config.eventDetectTimeRangePercentage = 67;
                    doAutoSave = true;
                }

                if (this.Config.EQcloseWindow == this.Config.EQstartWindow)
                {
                    if (this.Config.EQcloseWindow == 0)
                        this.Config.EQcloseWindow = 86400;
                    else
                        this.Config.EQcloseWindow--;
                    doAutoSave = true;
                }
                if (this.Config.currentIdleToCharge != 110 ||
                    this.Config.currentIdleToInUse != -110 ||
                    this.Config.currentChargeToIdle != 90 ||
                    this.Config.currentChargeToInUse != -110 ||
                    this.Config.currentInUseToCharge != 110 ||
                    this.Config.currentInUseToIdle != -90)
                {
                    this.Config.currentIdleToCharge = 110;
                    this.Config.currentIdleToInUse = -110;
                    this.Config.currentChargeToIdle = 90;
                    this.Config.currentChargeToInUse = -110;
                    this.Config.currentInUseToCharge = 110;
                    this.Config.currentInUseToIdle = -90;

                    doAutoSave = true;
                }

                if (doAutoSave)
                {
                    await this.SaveConfigToDevice();
                }

                //if (Quantum_Firmware.Firmware.getLatestBattViewFirmware() > 200)
                //	this.saveTime();
            }
            deviceIsLoaded = true;
            return true;
        }

        public async Task<bool> RouterDoLoad(float firmwareVersion, byte zoneID, bool isRTCLost)
        {

            return await doLoadInternal(true, firmwareVersion, zoneID, isRTCLost);
        }

        public async Task<bool> DoLoad()
        {

            return await Task.Run(async () =>
            {
                return await doLoadInternal(false, 0, 0, false);
            });

        }

        public async Task<Tuple<CommunicationResult, byte[]>> writetoPLCFlash(byte[] data, int length)
        {

            CommunicationResult result = CommunicationResult.NOT_EXIST;
            byte[] temp = new byte[1028];
            int tryTime = 0;
            //if (!batt_mutex.WaitOne(15000))
            //return new Tuple<commProtocol.Communication_Result, byte[]>(commProtocol.Communication_Result.mutexKilled, new byte[1]);
            byte[] tempArr = new byte[1];
            try
            {
                for (int i = 0; i < (int)(length / 1024); i++)
                {
                    Array.Copy(BitConverter.GetBytes((UInt32)i * 1024), 0, temp, 0, 4);

                    Array.Copy(data, i * 1024, temp, 4, 1024);

                    await ForceDelay();
                    var tuple = await myCommunicator.MySendRecieve(USB_WRITE_PLC_FLASH, temp, SerialNumber, 0, true, tempArr);
                    result = tuple.Item1;
                    tempArr = tuple.Item2;

                    LastSentCommandTime = DateTime.UtcNow;
                    if (result != CommunicationResult.OK)
                    {
                        if (tryTime >= 3)
                            return new Tuple<CommunicationResult, byte[]>(result, tempArr);
                        else
                        {
                            Task.Delay(1250);
                            tryTime++;
                            i--;
                            continue;
                        }
                    }
                    tryTime = 0;
                }
                tryTime = 0;
                if ((length % 1024) != 0)
                {
                    byte[] temp2 = new byte[length % 1024 + 4];
                    Array.Copy(BitConverter.GetBytes((((UInt32)(length / 1024))) * 1024), 0, temp2, 0, 4);
                    Array.Copy(data, ((length / 1024) * 1024), temp2, 4, (length % 1024));
                    while (tryTime < 3)
                    {
                        await ForceDelay();
                        var tuple = await myCommunicator.MySendRecieve(USB_WRITE_PLC_FLASH, temp2, SerialNumber, 0, true, tempArr);
                        result = tuple.Item1;
                        tempArr = tuple.Item2;
                        LastSentCommandTime = DateTime.UtcNow;
                        if (result != CommunicationResult.OK)
                        {
                            if (tryTime >= 3)
                                return new Tuple<CommunicationResult, byte[]>(result, tempArr);
                            else
                            {
                                Task.Delay(1250);
                                tryTime++;
                                continue;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                    if (result != CommunicationResult.OK)
                        return new Tuple<CommunicationResult, byte[]>(result, tempArr);


                }
            }
            catch
            {
                return new Tuple<CommunicationResult, byte[]>(CommunicationResult.internalFailure, tempArr);
            }
            finally
            {
                //batt_mutex.ReleaseMutex();
            }
            return new Tuple<CommunicationResult, byte[]>(CommunicationResult.OK, tempArr);
        }

        public async Task<Tuple<CommunicationResult, byte[]>> requectPLCCalibration()
        {
            byte[] tempArr = new byte[1];
            //if (!batt_mutex.WaitOne(15000)) return new Tuple<commProtocol.Communication_Result, byte[]>(commProtocol.Communication_Result.mutexKilled, tempArr);
            CommunicationResult status;
            try
            {
                await ForceDelay();
                var tuple = await myCommunicator.MySendRecieve(send_PLC_CAL, null, SerialNumber, 0, true, tempArr, TimeoutLevel.normal);
                status = tuple.Item1;
                LastSentCommandTime = DateTime.UtcNow;
            }
            catch
            {
                return new Tuple<CommunicationResult, byte[]>(CommunicationResult.internalFailure, tempArr);
            }
            finally
            {
                //batt_mutex.ReleaseMutex();
            }
            return new Tuple<CommunicationResult, byte[]>(status, tempArr);

        }

        public async Task<Tuple<CommunicationResult, byte[]>> requestPLCUpdate()
        {
            byte[] tempArr = new byte[1];
            //if (!batt_mutex.WaitOne(15000)) return new Tuple<commProtocol.Communication_Result, byte[]>(commProtocol.Communication_Result.mutexKilled, tempArr);
            CommunicationResult status;
            try
            {
                await ForceDelay();
                var tuple = await myCommunicator.MySendRecieve(USB_Request_PLC_Code_update, null, SerialNumber, 0, true, tempArr, TimeoutLevel.extended);
                status = tuple.Item1;
                tempArr = tuple.Item2;
                LastSentCommandTime = DateTime.UtcNow;
                if (status != CommunicationResult.OK)
                {
                    await ForceDelay();
                    tuple = await myCommunicator.MySendRecieve(USB_Request_PLC_Code_update, null, SerialNumber, 0, true, tempArr, TimeoutLevel.extended);
                    status = tuple.Item1;
                    tempArr = tuple.Item2;
                    LastSentCommandTime = DateTime.UtcNow;
                }
            }
            catch
            {
                return new Tuple<CommunicationResult, byte[]>(CommunicationResult.internalFailure, tempArr);
            }
            finally
            {
                //batt_mutex.ReleaseMutex();
            }
            return new Tuple<CommunicationResult, byte[]>(status, tempArr);

        }
        public async Task<Tuple<CommunicationResult, byte[]>> TestConnection()
        {
            CommunicationResult result;
            byte[] tempArr = new byte[1];
            //if (!batt_mutex.WaitOne(15000)) return new Tuple<commProtocol.Communication_Result, byte[]>(commProtocol.Communication_Result.mutexKilled, tempArr);
            try
            {
                //await forceDelay();
                var tuple = await myCommunicator.MySendRecieve(testCommand, null, SerialNumber, 0, true, tempArr);
                result = tuple.Item1;
                tempArr = tuple.Item2;
                LastSentCommandTime = DateTime.UtcNow;
            }
            catch (Exception ex)
            {
                return new Tuple<CommunicationResult, byte[]>(CommunicationResult.internalFailure, tempArr);
            }
            finally
            {
                //batt_mutex.ReleaseMutex();
            }
            return new Tuple<CommunicationResult, byte[]>(result, tempArr);
        }
        public async Task<Tuple<CommunicationResult, byte[]>> ReadADCValues()
        {
            byte[] resultArray = new byte[1];
            CommunicationResult result;
            //if (!batt_mutex.WaitOne(15000))
            //	return new Tuple<commProtocol.Communication_Result, byte[]>(commProtocol.Communication_Result.mutexKilled, resultArray);
            try
            {
                await ForceDelay();
                var tuple = await myCommunicator.MySendRecieve(readBattViewStatus, null, SerialNumber, (this.FirmwareRevision > 1.94 ? 256 : 57), true, resultArray);
                result = tuple.Item1;
                resultArray = tuple.Item2;
                LastSentCommandTime = DateTime.UtcNow;
                //Changing result to OK if its USB, Because USB returns receiving error everytime.
                if (ACConstants.ConnectionType == ConnectionTypesEnum.USB && result == CommunicationResult.RECEIVING_ERROR)
                {
                    result = CommunicationResult.OK;
                }
                if (result != CommunicationResult.OK)
                {
                    return new Tuple<CommunicationResult, byte[]>(result, resultArray);
                }
                battViewAdcRawObject.loadArray(resultArray, this.FirmwareRevision);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return new Tuple<CommunicationResult, byte[]>(CommunicationResult.internalFailure, resultArray);
            }

            return new Tuple<CommunicationResult, byte[]>(CommunicationResult.OK, resultArray);

        }
        public async Task<Tuple<CommunicationResult, byte[]>> SendDefineCommand()
        {
            const int dataSize = 80; // or 80 * 2

            byte[] resultArray = new byte[1];
            try
            {
                await ForceDelay();
                var tuple = await myCommunicator.MySendRecieve(CommProtocol.defineCommand, null, SerialNumber, dataSize, true, resultArray);
                CommunicationResult result = tuple.Item1;
                resultArray = tuple.Item2;
                LastSentCommandTime = DateTime.UtcNow;

                if (result != CommunicationResult.OK)
                {
                    return new Tuple<CommunicationResult, byte[]>(result, resultArray);
                }
                if (resultArray[0] != CommProtocol.battViewDefineKey)
                {
                    return new Tuple<CommunicationResult, byte[]>(CommunicationResult.RECEIVING_ERROR, resultArray);
                }
                timeLost = resultArray[39] != 0;
                myZone = resultArray[40];
                FirmwareWiFiVersion = FirmwareWiFiVersionUtility.GetFirmwareWiFiVersion(FirmwareRevision, resultArray);
            }
            catch
            {
                return new Tuple<CommunicationResult, byte[]>(CommunicationResult.internalFailure, resultArray);
            }

            return new Tuple<CommunicationResult, byte[]>(CommunicationResult.OK, resultArray);
        }

        public async Task ReadConfigIfNotLoaded()
        {
            if (Config.NotLoaded)
                await ReadConfig();
        }

        public async Task<Tuple<CommunicationResult, byte[]>> ReadConfig()
        {
            byte[] resultArray = new byte[1];

            try
            {
                await ForceDelay();
                var tuple =
                    await
                    myCommunicator.
                                  MySendRecieve
                                  (ReadConfigCommand, null, SerialNumber, 512, true, resultArray);

                CommunicationResult result = tuple.Item1;
                resultArray = tuple.Item2;
                LastSentCommandTime = DateTime.UtcNow;

                if (result != CommunicationResult.OK)
                {
                    return new Tuple<CommunicationResult, byte[]>(result, resultArray);
                }

                Config.LoadFromArray(resultArray, FirmwareRevision);
            }
            catch
            {
                return new Tuple<CommunicationResult, byte[]>(CommunicationResult.internalFailure, resultArray);
            }

            return new Tuple<CommunicationResult, byte[]>(CommunicationResult.OK, resultArray);

        }
        public async Task<Tuple<CommunicationResult, byte[]>> startNewStudy()
        {
            //startNewStudy
            byte[] passData = new byte[4];
            Array.Copy(BitConverter.GetBytes(ControlObject.userID), passData, 4);
            byte[] resultArray = new byte[1];
            //if (!batt_mutex.WaitOne(15000))
            //	return new Tuple<commProtocol.Communication_Result, byte[]>(commProtocol.Communication_Result.mutexKilled, resultArray);
            try
            {
                await ForceDelay();
                var tuple = await myCommunicator.MySendRecieve(startNewStudyCMD, passData, SerialNumber, 6, true, resultArray);
                CommunicationResult result = tuple.Item1;
                resultArray = tuple.Item2;
                LastSentCommandTime = DateTime.UtcNow;

                if (result != CommunicationResult.OK)
                {
                    return new Tuple<CommunicationResult, byte[]>(result, resultArray);
                }
                Config.memorySignature = BitConverter.ToUInt16(resultArray, 0);
                Config.studyId = BitConverter.ToUInt32(resultArray, 2);

                await Task.Delay(8);
            }
            catch
            {
                return new Tuple<CommunicationResult, byte[]>(CommunicationResult.internalFailure, resultArray);
            }

            return new Tuple<CommunicationResult, byte[]>(CommunicationResult.OK, resultArray);
        }

        public async Task<bool> SaveSiteViewConfigAndTime()
        {
            if (HasConfigError())
                return false;

            var saveConfigResult = await SaveSiteViewConfig();

            if (saveConfigResult)
            {
                var saveTimeResult = await SaveTime();

                return (saveTimeResult.Item1 == CommunicationResult.OK);
            }

            return false;
        }

        bool HasConfigError()
        {
            if (SiteViewConfig.battviewVersion == 1)
            {
                if (SiteViewConfig.actAccessSSIDpassword.Length > 13)
                    return true;
            }
            else
            {
                if (SiteViewConfig.actAccessSSIDpassword.Length > 31)
                    return true;
            }

            return false;
        }

        public async Task<bool> SaveSiteViewConfig()
        {
            if (SiteViewConfig == null)
                return true;

            var result = await SaveSiteViewToDevice();
            bool ok = result.Item1 == CommunicationResult.OK;

            if (ok)
                Config = SiteViewConfig;

            return ok;
        }

        async Task<Tuple<CommunicationResult, byte[]>> SaveSiteViewToDevice()
        {
            try
            {
                return await SaveConfigToDeviceFromObject(SiteViewConfig);
            }
            catch
            {
                return new Tuple<CommunicationResult, byte[]>(CommunicationResult.internalFailure, new byte[1]);
            }
        }

        public async Task<bool> SaveConfigAndTime()
        {
            var saveConfigResult = await SaveConfig();

            if (saveConfigResult)
            {
                var saveTimeResult = await SaveTime();

                return (saveTimeResult.Item1 == CommunicationResult.OK);
            }

            return false;
        }

        public async Task<Tuple<bool, bool>> SaveConfigForPlc()
        {
            bool saveConfigResult = await SaveConfig();

            bool rebootResult = false;

            if (saveConfigResult)
            {
                if (Config.RequireRebootForPlc())
                {
                    Config.ResetSaveEnablePlc();

                    rebootResult = await Reboot();
                }
            }

            return new Tuple<bool, bool>(saveConfigResult, rebootResult);
        }

        async Task<bool> Reboot()
        {
            var restartResult = await restart();

            return restartResult.Item1 == CommunicationResult.OK;
        }

        public async Task<bool> SaveConfig()
        {
            var result = await SaveConfigToDevice();

            if (result.Item1 == CommunicationResult.OK)
            {
                BattViewQuantum.Instance.GetConnectionManager().siteView.setDeviceConfigurationSaved(SerialNumber, false);

                return true;
            }

            return false;
        }

        public async Task<Tuple<CommunicationResult, byte[]>> SaveConfigToDevice()
        {
            try
            {
                return await SaveConfigToDeviceFromObject(Config);
            }
            catch
            {
                return new Tuple<CommunicationResult, byte[]>(CommunicationResult.internalFailure, new byte[1]);
            }
        }

        async Task<Tuple<CommunicationResult, byte[]>> SaveConfigToDeviceFromObject(BattViewConfig config)
        {
            byte[] passData = config.formatAll();
            byte[] resultArray = new byte[1];

            try
            {
                await ForceDelay();

                byte saveCommand = config.battviewVersion == 1 ? setConfigAllV1 : setConfigAllV2;

                var tuple = await myCommunicator.MySendRecieve(saveCommand, passData, SerialNumber, 2, true, resultArray);

                CommunicationResult result = tuple.Item1;
                resultArray = tuple.Item2;
                LastSentCommandTime = DateTime.UtcNow;

                if (result == CommunicationResult.RECEIVING_ERROR && ACConstants.ConnectionType == ConnectionTypesEnum.USB)
                {
                    return new Tuple<CommunicationResult, byte[]>(CommunicationResult.OK, resultArray);
                }
                if (result != CommunicationResult.OK)
                {
                    return new Tuple<CommunicationResult, byte[]>(result, resultArray);
                }

                config.memorySignature = BitConverter.ToUInt16(resultArray, 0);
            }
            catch
            {
                return new Tuple<CommunicationResult, byte[]>(CommunicationResult.internalFailure, resultArray);
            }

            return new Tuple<CommunicationResult, byte[]>(CommunicationResult.OK, resultArray);
        }

        public async Task<Tuple<CommunicationResult, byte[]>> setConfigFromConfig(BattViewConfig temp)
        {
            byte[] arr1 = temp.formatAll();
            byte[] resArr = new byte[1];

            //if (!batt_mutex.WaitOne(15000))
            //	return new Tuple<commProtocol.Communication_Result, byte[]>(commProtocol.Communication_Result.mutexKilled, resArr);
            try
            {
                await ForceDelay();

                byte saveCommand = Config.battviewVersion == 1 ? setConfigAllV1 : setConfigAllV2;

                var tuple = await myCommunicator.MySendRecieve(saveCommand, arr1, SerialNumber, 2, true, resArr);
                CommunicationResult result = tuple.Item1;
                resArr = tuple.Item2;
                LastSentCommandTime = DateTime.UtcNow;
                if (result != CommunicationResult.OK && result != CommunicationResult.COMMAND_DELAYED)
                    return new Tuple<CommunicationResult, byte[]>(result, resArr);
                Config.memorySignature = BitConverter.ToUInt16(resArr, 0);

                return new Tuple<CommunicationResult, byte[]>(result, resArr);
            }
            catch
            {
                return new Tuple<CommunicationResult, byte[]>(CommunicationResult.internalFailure, resArr);
            }
            finally
            {
                //batt_mutex.ReleaseMutex();
            }

        }
        public async Task<Tuple<CommunicationResult, byte[]>> readTime()
        {
            byte[] resultArray = new byte[1];
            UInt32 unixTimeStampNow = (UInt32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc)).TotalSeconds);
            //                        (UInt32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc)).TotalSeconds
            UInt32 unixTimeStamp;
            //if (!batt_mutex.WaitOne(15000)) return new Tuple<commProtocol.Communication_Result, byte[]>(commProtocol.Communication_Result.mutexKilled, resultArray);
            try
            {
                await ForceDelay();
                var tuple = await myCommunicator.MySendRecieve(getRTC, null, SerialNumber, 5, true, resultArray);
                CommunicationResult result = tuple.Item1;
                resultArray = tuple.Item2;
                LastSentCommandTime = DateTime.UtcNow;
                if (result != CommunicationResult.OK)
                {
                    return new Tuple<CommunicationResult, byte[]>(result, resultArray);
                }
                unixTimeStamp = BitConverter.ToUInt32(resultArray, 0);
                myZone = resultArray[4];
            }
            catch
            {
                return new Tuple<CommunicationResult, byte[]>(CommunicationResult.internalFailure, resultArray);
            }
            finally
            {
                //batt_mutex.ReleaseMutex();
            }


            //if (Math.Abs((double)unixTimeStamp - (double)unixTimeStampNow) > 1800 || timeLost)
            //    return saveTime();
            return new Tuple<CommunicationResult, byte[]>(CommunicationResult.OK, resultArray);

        }
        public async Task<Tuple<CommunicationResult, byte[]>> SaveTime()
        {
            byte[] passData = new byte[217];
            UInt32 userID = ControlObject.userID;
            Array.Copy(BitConverter.GetBytes((UInt32)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc)).TotalSeconds), passData, 4);
            Array.Copy(BitConverter.GetBytes(userID), 0, passData, 4, 4);
            JsonZone j = StaticDataAndHelperFunctions.getZoneByID(myZone);
            for (int i = 0; i < 50; i++)
                Array.Copy(BitConverter.GetBytes(j.changes_time[i]), 0, passData, 8 + i * 4, 4);
            Array.Copy(BitConverter.GetBytes(j.changes_value), 0, passData, 208, 4);
            Array.Copy(BitConverter.GetBytes(j.base_utc), 0, passData, 212, 4);
            passData[216] = j.id;

            //if (!batt_mutex.WaitOne(15000)) return new Tuple<commProtocol.Communication_Result, byte[]>(commProtocol.Communication_Result.mutexKilled, new byte[1]);
            try
            {
                byte[] tempArr = new byte[1];
                await ForceDelay();
                var tuple = await myCommunicator.MySendRecieve(setRTC, passData, SerialNumber, 0, true, tempArr);
                CommunicationResult result = tuple.Item1;
                tempArr = tuple.Item2;
                LastSentCommandTime = DateTime.UtcNow;

                if (result == CommunicationResult.OK)
                    timeLost = false;
                return new Tuple<CommunicationResult, byte[]>(result, tempArr);
            }
            catch
            {
                return new Tuple<CommunicationResult, byte[]>(CommunicationResult.internalFailure, new byte[1]);
            }
            finally
            {
                //batt_mutex.ReleaseMutex();
            }



        }




        public async Task<Tuple<CommunicationResult, byte[]>> searchEventRecordByDate(DateTime time)
        {
            byte[] passData = new byte[5];
            passData[4] = 1;
            DateTime tx = new DateTime(time.Year, time.Month, time.Day, 0, 0, 0, DateTimeKind.Utc);
            tx = StaticDataAndHelperFunctions.getZoneTimeFromUTC(myZone, tx, Config.enableDayLightSaving != 0);
            Array.Copy(BitConverter.GetBytes((UInt32)tx.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds), passData, 4);
            byte[] resultArray = new byte[1];
            //if (!batt_mutex.WaitOne(15000)) return new Tuple<commProtocol.Communication_Result, byte[]>(commProtocol.Communication_Result.mutexKilled, resultArray);
            try
            {
                await ForceDelay();
                var tuple = await myCommunicator.MySendRecieve(searchRecordByDate, passData, SerialNumber, 4, true, resultArray);
                CommunicationResult result = tuple.Item1;
                resultArray = tuple.Item2;
                LastSentCommandTime = DateTime.UtcNow;

                if (result != CommunicationResult.OK)
                {
                    return new Tuple<CommunicationResult, byte[]>(result, resultArray);
                }


                searchEventTimeID = BitConverter.ToUInt32(resultArray, 0);
            }
            catch
            {
                return new Tuple<CommunicationResult, byte[]>(CommunicationResult.internalFailure, resultArray);
            }
            finally
            {
                //batt_mutex.ReleaseMutex();
            }
            return new Tuple<CommunicationResult, byte[]>(CommunicationResult.OK, resultArray);

        }
        public async Task<Tuple<CommunicationResult, byte[]>> searchRTRecordByDate(DateTime time)
        {
            byte[] passData = new byte[5];
            passData[4] = 2;
            Array.Copy(BitConverter.GetBytes((UInt32)time.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds), passData, 4);
            byte[] resultArray = new byte[1];
            //if (!batt_mutex.WaitOne(15000)) return new Tuple<commProtocol.Communication_Result, byte[]>(commProtocol.Communication_Result.mutexKilled, resultArray);
            try
            {
                await ForceDelay();
                var tuple = await myCommunicator.MySendRecieve(searchRecordByDate, passData, SerialNumber, 4, true, resultArray);
                CommunicationResult result = tuple.Item1;
                resultArray = tuple.Item2;
                LastSentCommandTime = DateTime.UtcNow;

                if (result != CommunicationResult.OK)
                {
                    return new Tuple<CommunicationResult, byte[]>(result, resultArray);
                }
                searchRTTimeID = BitConverter.ToUInt32(resultArray, 0);

            }
            catch
            {
                return new Tuple<CommunicationResult, byte[]>(CommunicationResult.internalFailure, resultArray);
            }
            finally
            {
                //batt_mutex.ReleaseMutex();
            }
            return new Tuple<CommunicationResult, byte[]>(CommunicationResult.OK, resultArray);

        }
        public async Task<Tuple<CommunicationResult, byte[]>> searchDebugRecordByDate(DateTime time)
        {
            byte[] passData = new byte[5];
            passData[4] = 3;
            Array.Copy(BitConverter.GetBytes((UInt32)time.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds), passData, 4);
            byte[] resultArray = new byte[1];
            //if (!batt_mutex.WaitOne(15000)) return new Tuple<commProtocol.Communication_Result, byte[]>(commProtocol.Communication_Result.mutexKilled, resultArray);
            try
            {
                await ForceDelay();
                var tuple = await myCommunicator.MySendRecieve(searchRecordByDate, passData, SerialNumber, 4, true, resultArray);
                CommunicationResult result = tuple.Item1;
                resultArray = tuple.Item2;
                LastSentCommandTime = DateTime.UtcNow;

                if (result != CommunicationResult.OK)
                {
                    return new Tuple<CommunicationResult, byte[]>(result, resultArray);
                }


                searchDebugTimeID = BitConverter.ToUInt32(resultArray, 0);
                LastSentCommandTime = DateTime.UtcNow;
            }
            catch
            {
                return new Tuple<CommunicationResult, byte[]>(CommunicationResult.internalFailure, resultArray);
            }
            finally
            {
                //batt_mutex.ReleaseMutex();
            }
            return new Tuple<CommunicationResult, byte[]>(CommunicationResult.OK, resultArray);

        }


        public async Task<Tuple<CommunicationResult, byte[]>> readFirmwareRev()
        {
            byte[] resultArray = new byte[1];
            //if (!batt_mutex.WaitOne(15000))
            //	return new Tuple<commProtocol.Communication_Result, byte[]>(commProtocol.Communication_Result.mutexKilled, resultArray);
            try
            {
                await ForceDelay();
                var tuple = await myCommunicator.MySendRecieve(getFirmwareRev, null, SerialNumber, 2, true, resultArray);
                CommunicationResult result = tuple.Item1;
                resultArray = tuple.Item2;
                LastSentCommandTime = DateTime.UtcNow;
                if (result != CommunicationResult.OK)
                {

                    return new Tuple<CommunicationResult, byte[]>(result, resultArray);
                }

                FirmwareRevision = ((10 * (resultArray[0] & 0xF0) >> 4) + (resultArray[0] & 0x0F)) + (((10 * (resultArray[1] & 0xF0) >> 4) + (resultArray[1] & 0x0F)) / 100.0f);
            }
            catch
            {
                return new Tuple<CommunicationResult, byte[]>(CommunicationResult.internalFailure, resultArray);
            }
            finally
            {
                //batt_mutex.ReleaseMutex();
            }
            return new Tuple<CommunicationResult, byte[]>(CommunicationResult.OK, resultArray);

        }
        public async Task<Tuple<CommunicationResult, byte[]>> controlAnalog(bool enable, float ADC_Current, float ADC_Voltage, float ADC_InternalNTC, float ADC_NTCBattery, float ADC_NTCBatteryRef, UInt16 ADC_Clamp, float ADC_IntercelltNTC, UInt16 ADC_ClampRef, UInt16 ADC_Electrolyte, UInt16 clampChannel2)
        {
            byte[] passData = new byte[33];

            int loc = 0;

            Array.Copy(BitConverter.GetBytes(ADC_Current), 0, passData, loc, 4);
            loc += 4;
            Array.Copy(BitConverter.GetBytes(ADC_Voltage), 0, passData, loc, 4);
            loc += 4;
            Array.Copy(BitConverter.GetBytes(ADC_InternalNTC), 0, passData, loc, 4);
            loc += 4;
            Array.Copy(BitConverter.GetBytes(ADC_NTCBatteryRef), 0, passData, loc, 4);
            loc += 4;
            Array.Copy(BitConverter.GetBytes(ADC_NTCBattery), 0, passData, loc, 4);
            loc += 4;
            Array.Copy(BitConverter.GetBytes(ADC_IntercelltNTC), 0, passData, loc, 4);
            loc += 4;

            Array.Copy(BitConverter.GetBytes(ADC_Clamp), 0, passData, loc, 2);
            loc += 2;

            Array.Copy(BitConverter.GetBytes(ADC_ClampRef), 0, passData, loc, 2);
            loc += 2;
            Array.Copy(BitConverter.GetBytes(clampChannel2), 0, passData, loc, 2);
            loc += 2;
            Array.Copy(BitConverter.GetBytes(ADC_Electrolyte), 0, passData, loc, 2);
            loc += 2;
            passData[loc] = (byte)(enable ? 0x01 : 0x00);


            //if (!batt_mutex.WaitOne(15000)) return new Tuple<commProtocol.Communication_Result, byte[]>(commProtocol.Communication_Result.mutexKilled, new byte[1]);
            try
            {
                byte[] tempArr = new byte[1];
                await ForceDelay();
                var tuple = await myCommunicator.MySendRecieve(setAnalog, passData, SerialNumber, 0, true, tempArr);
                CommunicationResult result = tuple.Item1;
                tempArr = tuple.Item2;

                LastSentCommandTime = DateTime.UtcNow;
                return new Tuple<CommunicationResult, byte[]>(result, tempArr);
            }
            catch
            {
                return new Tuple<CommunicationResult, byte[]>(CommunicationResult.internalFailure, new byte[1]);
            }
            finally
            {
                //batt_mutex.ReleaseMutex();
            }
        }
        public async Task<Tuple<CommunicationResult, byte[]>> readRunningEvent()
        {

            byte[] resultArray = new byte[1];
            runningEvent = new BattViewObjectEvent();
            //if (!batt_mutex.WaitOne(15000)) return new Tuple<commProtocol.Communication_Result, byte[]>(commProtocol.Communication_Result.mutexKilled, resultArray);
            try
            {
                await ForceDelay();
                var tuple = await myCommunicator.MySendRecieve(getRunningEvent, null, SerialNumber, 64, true, resultArray);
                CommunicationResult result = tuple.Item1;
                resultArray = tuple.Item2;
                LastSentCommandTime = DateTime.UtcNow;

                if (result != CommunicationResult.OK)
                {
                    return new Tuple<CommunicationResult, byte[]>(result, resultArray);
                }
                lock (runningEventLock)
                {
                    runningEvent.loadFromBuffer(resultArray, 0);
                }
            }
            catch
            {
                return new Tuple<CommunicationResult, byte[]>(CommunicationResult.internalFailure, resultArray);
            }
            finally
            {
                //batt_mutex.ReleaseMutex();
            }
            return new Tuple<CommunicationResult, byte[]>(CommunicationResult.OK, resultArray);

        }


        public async Task<Tuple<CommunicationResult, byte[]>> readGlobalRecord()
        {
            byte[] resultArray = new byte[1];
            globalRecord = new BattViewObjectGlobalRecord();
            //if (!batt_mutex.WaitOne(15000)) return new Tuple<commProtocol.Communication_Result, byte[]>(commProtocol.Communication_Result.mutexKilled, resultArray);
            try
            {
                await ForceDelay();
                var tuple = await myCommunicator.MySendRecieve(getGlobalRecord, null, SerialNumber, this.FirmwareRevision > 1.05 ? 128 : 64, true, resultArray);
                CommunicationResult result = tuple.Item1;
                resultArray = tuple.Item2;
                LastSentCommandTime = DateTime.UtcNow;

                if (result != CommunicationResult.OK)
                {
                    return new Tuple<CommunicationResult, byte[]>(result, resultArray);
                }

                globalRecord.loadFromBuffer(resultArray, this.FirmwareRevision);


            }
            catch
            {
                return new Tuple<CommunicationResult, byte[]>(CommunicationResult.internalFailure, resultArray);
            }
            finally
            {
                //batt_mutex.ReleaseMutex();
            }
            return new Tuple<CommunicationResult, byte[]>(CommunicationResult.OK, resultArray);

        }
        public async Task<Tuple<CommunicationResult, byte[]>> getQuickView()
        {
            byte[] resultArray = new byte[1];


            //if (!batt_mutex.WaitOne(15000))
            //	return new Tuple<commProtocol.Communication_Result, byte[]>(commProtocol.Communication_Result.mutexKilled, resultArray);
            try
            {
                await ForceDelay();
                var tuple = await myCommunicator.MySendRecieve(this.FirmwareRevision >= 2.09f ? getQuyickViewCMD2 : getQuyickViewCMD, null, SerialNumber, this.FirmwareRevision >= 2.09f ? 64 : 32, true, resultArray);
                CommunicationResult result = tuple.Item1;
                resultArray = tuple.Item2;
                LastSentCommandTime = DateTime.UtcNow;

                if (result != CommunicationResult.OK)
                {
                    //quickView = new QUICK_VIEW();
                    return new Tuple<CommunicationResult, byte[]>(result, resultArray);
                }

                quickView.loadFromArray(resultArray, this.FirmwareRevision);



            }
            catch
            {
                return new Tuple<CommunicationResult, byte[]>(CommunicationResult.internalFailure, resultArray);
            }
            finally
            {
                //batt_mutex.ReleaseMutex();
            }

            return new Tuple<CommunicationResult, byte[]>(CommunicationResult.OK, resultArray);

        }
        public async Task<Tuple<CommunicationResult, byte[]>> resetGlobalRecord()
        {
            //if (!batt_mutex.WaitOne(15000)) return new Tuple<commProtocol.Communication_Result, byte[]>(commProtocol.Communication_Result.mutexKilled, new byte[1]);
            try
            {
                byte[] tempArr = new byte[1];
                await ForceDelay();
                var tuple = await myCommunicator.MySendRecieve(ResetGlobalRecord, null, SerialNumber, 0, true, tempArr);
                CommunicationResult result = tuple.Item1;
                tempArr = tuple.Item2;

                LastSentCommandTime = DateTime.UtcNow;
                return new Tuple<CommunicationResult, byte[]>(result, tempArr);
            }
            catch
            {
                return new Tuple<CommunicationResult, byte[]>(CommunicationResult.internalFailure, new byte[1]);
            }
            finally
            {
                //batt_mutex.ReleaseMutex();
            }


        }

        internal async override Task<CommunicationResult> RecyclePower()
        {
            var result = await restart();

            return result.Item1;
        }

        public async Task<Tuple<CommunicationResult, byte[]>> restart()
        {
            try
            {
                byte[] tempArr = new byte[1];
                await ForceDelay();
                var tuple = await myCommunicator.MySendRecieve(restartBATTView, null, SerialNumber, 0, true, tempArr);
                CommunicationResult result = tuple.Item1;
                tempArr = tuple.Item2;
                LastSentCommandTime = DateTime.UtcNow;
                return new Tuple<CommunicationResult, byte[]>(result, tempArr);
            }
            catch
            {
                return new Tuple<CommunicationResult, byte[]>(CommunicationResult.internalFailure, new byte[1]);
            }
        }

        public async Task<Tuple<CommunicationResult, byte[]>> ReadRecordsLimits()
        {
            byte[] resultArray = new byte[1];
            //if (!batt_mutex.WaitOne(15000)) return new Tuple<commProtocol.Communication_Result, byte[]>(commProtocol.Communication_Result.mutexKilled, resultArray);
            try
            {
                await ForceDelay();
                var tuple = await myCommunicator.MySendRecieve(getRecordsLimits, null, SerialNumber, 48, true, resultArray);
                CommunicationResult result = tuple.Item1;
                resultArray = tuple.Item2;
                LastSentCommandTime = DateTime.UtcNow;

                if (result != CommunicationResult.OK)
                {
                    return new Tuple<CommunicationResult, byte[]>(result, resultArray);
                }


                debugRecordsStartID = BitConverter.ToUInt32(resultArray, 0);
                debugRecordsLastID = BitConverter.ToUInt32(resultArray, 4);
                debugRecordsStartIDTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(BitConverter.ToUInt32(resultArray, 8));
                debugRecordsLastIDTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(BitConverter.ToUInt32(resultArray, 12));

                eventsRecordsStartID = BitConverter.ToUInt32(resultArray, 16);
                eventsRecordsLastID = BitConverter.ToUInt32(resultArray, 20);
                eventsRecordsStartIDTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(BitConverter.ToUInt32(resultArray, 24));
                eventsRecordsStartIDTime = StaticDataAndHelperFunctions.getZoneTimeFromUTC(myZone, eventsRecordsStartIDTime, Config.enableDayLightSaving != 0);
                eventsRecordsLastIDTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(BitConverter.ToUInt32(resultArray, 28));
                eventsRecordsLastIDTime = StaticDataAndHelperFunctions.getZoneTimeFromUTC(myZone, eventsRecordsLastIDTime, Config.enableDayLightSaving != 0);

                realtimeRecordsStartID = BitConverter.ToUInt32(resultArray, 32);
                realtimeRecordsLastID = BitConverter.ToUInt32(resultArray, 36);
                realtimeRecordsStartIDTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(BitConverter.ToUInt32(resultArray, 40));
                realtimeRecordsLastIDTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(BitConverter.ToUInt32(resultArray, 44));

            }
            catch
            {
                return new Tuple<CommunicationResult, byte[]>(CommunicationResult.internalFailure, resultArray);
            }
            finally
            {
                //batt_mutex.ReleaseMutex();
            }

            return new Tuple<CommunicationResult, byte[]>(CommunicationResult.OK, resultArray);
        }

        const int events_read_patch = 14;
        bool weHaveOneGoodEventRecord = false;


        public List<BattViewObjectEvent> getBattViewEvent()
        {
            lock (history_eventsLock)
            {
                List<BattViewObjectEvent> temp = new List<BattViewObjectEvent>();
                foreach (BattViewObjectEvent h in history_events)
                {
                    BattViewObjectEvent r = new BattViewObjectEvent();
                    r.loadFromEvent(h);
                    temp.Add(h);
                }

                return temp;
            }

        }
        async Task<Tuple<CommunicationResult, byte[], bool, bool>> doReadEvents(UInt32 startRecord, DateTime maxDate, bool done, bool readLimits)
        {
            done = true;
            readLimits = false;
            byte[] resultArray = new byte[1];
            byte[] passData = new byte[5];
            UInt32 maxDateUnixTimeStamp = (UInt32)maxDate.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
            Array.Copy(BitConverter.GetBytes(startRecord), passData, 4);
            passData[4] = events_read_patch;
            //if (!batt_mutex.WaitOne(15000)) return new Tuple<commProtocol.Communication_Result, byte[], bool, bool>(commProtocol.Communication_Result.mutexKilled, resultArray, done, readLimits);
            try
            {
                await ForceDelay();
                var tuple = await myCommunicator.MySendRecieve(readEventsCMD, passData, SerialNumber, 0, false, resultArray);
                CommunicationResult result = tuple.Item1;
                resultArray = tuple.Item2;
                LastSentCommandTime = DateTime.UtcNow;
                if (result != CommunicationResult.OK)
                {

                    return new Tuple<CommunicationResult, byte[], bool, bool>(result, resultArray, done, readLimits);
                }


                if (resultArray.Length == 0)
                {
                    return new Tuple<CommunicationResult, byte[], bool, bool>(CommunicationResult.SIZEERROR, resultArray, done, readLimits);
                }
                if (resultArray[0] == 0x7F)
                {
                    readLimits = true;
                    return new Tuple<CommunicationResult, byte[], bool, bool>(CommunicationResult.OK, resultArray, done, readLimits);
                }
                //verify length and size
                int recoevedRecordsCount = resultArray[0];
                if (recoevedRecordsCount == 0)
                {
                    return new Tuple<CommunicationResult, byte[], bool, bool>(CommunicationResult.OK, resultArray, done, readLimits);
                }
                if (recoevedRecordsCount != events_read_patch)
                    done = true;
                else
                    done = false;
                if (recoevedRecordsCount * 64 + 1 != resultArray.Length)
                {
                    return new Tuple<CommunicationResult, byte[], bool, bool>(CommunicationResult.SIZEERROR, resultArray, done, readLimits);
                }

                for (int i = 0; i < recoevedRecordsCount; i++)
                {
                    BattViewObjectEvent eventRecord = new BattViewObjectEvent();
                    eventRecord.loadFromBuffer(resultArray, 1 + i * 64);
                    if (!eventRecord.valid)
                        eventRecord.eventID = startRecord + (uint)i;
                    if (eventRecord.valid && eventRecord.startTime > maxDateUnixTimeStamp)
                        break;
                    if (history_events.Find(x => x.eventID == eventRecord.eventID) == null)
                    {
                        history_events.Add(eventRecord);
                    }
                    else
                    {
                        continue;
                    }
                    if (eventRecord.valid)
                        weHaveOneGoodEventRecord = true;

                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return new Tuple<CommunicationResult, byte[], bool, bool>(CommunicationResult.internalFailure, resultArray, done, readLimits);
            }
            finally
            {
                //batt_mutex.ReleaseMutex();
            }

            return new Tuple<CommunicationResult, byte[], bool, bool>(CommunicationResult.OK, resultArray, done, readLimits);
        }
        async Task<CommunicationResult> readsubEvents(UInt32 startRecord, DateTime maxDate)
        {
            UInt32 startPointer = startRecord;
            if (eventsRecordsStartID == 0xFFFFFFFF)
                return CommunicationResult.OK;

            bool isDone = true;
            do
            {
                bool readLimits = false;
                var tupleExtra = await doReadEvents(startPointer, maxDate, isDone, readLimits);
                CommunicationResult status = tupleExtra.Item1;
                isDone = tupleExtra.Item3;
                readLimits = tupleExtra.Item4;
                if (readLimits)
                {
                    isDone = false;
                    var tuple = await ReadRecordsLimits();
                    status = tuple.Item1;
                    if (status != CommunicationResult.OK)
                    {
                        return status;
                    }
                    startRecord = startPointer = eventsRecordsStartID;
                    continue;
                }
                if (status != CommunicationResult.OK)
                {
                    return status;
                }
                startPointer += events_read_patch;

            } while (!isDone);

            return CommunicationResult.OK;
        }
        public async Task<CommunicationResult> readEvents(UInt32 startRecord, DateTime t, bool buSearchTime)
        {
            CommunicationResult status = 0;
            //lock (history_events)
            //{
            try
            {
                if (buSearchTime)
                    startRecord = searchEventTimeID;
                DateTime maxDate = new DateTime(t.Year, t.Month, t.Day, 0, 0, 0, DateTimeKind.Utc);
                //  maxDate = Quantum_Software.staticDataAndHelperFunctions.getZoneTimeFromUTC(myZone, maxDate, config.enableDayLightSaving != 0);
                //search if we have any event with id > startRecordEvent
                history_events.Sort();
                status = CommunicationResult.OK;
                int GoodIndexStart;
                int GoodIndexEnd;

                if (history_events.Count != 0 && weHaveOneGoodEventRecord)
                {

                    for (GoodIndexStart = 0; GoodIndexStart < history_events.Count; GoodIndexStart++)
                    {
                        if (history_events[GoodIndexStart].valid)
                            break;
                    }
                    for (GoodIndexEnd = history_events.Count - 1; GoodIndexEnd >= 0; GoodIndexEnd--)
                    {
                        if (history_events[GoodIndexEnd].valid)
                            break;
                    }

                    bool skipRightEnd = false;
                    bool resort = false;
                    if (history_events[GoodIndexStart].eventID > startRecord)
                    {
                        status = await readsubEvents(startRecord, new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(history_events[GoodIndexStart].startTime));
                        if (status != CommunicationResult.OK)
                        {
                            return status;
                        }
                        resort = true;
                    }
                    else if (history_events[GoodIndexEnd].eventID < startRecord)
                    {
                        status = await readsubEvents(history_events[GoodIndexEnd].eventID, maxDate);
                        if (status != CommunicationResult.OK)
                        {
                            return status;
                        }
                        skipRightEnd = true;
                        resort = true;
                    }
                    if (!skipRightEnd)
                    {
                        if (resort)
                        {
                            history_events.Sort();
                            for (GoodIndexEnd = history_events.Count - 1; GoodIndexEnd >= 0; GoodIndexEnd--)
                            {
                                if (history_events[GoodIndexEnd].valid)
                                    break;
                            }

                        }
                        if (maxDate > new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(history_events[GoodIndexEnd].startTime + history_events[GoodIndexStart].duration))
                        {
                            status = await readsubEvents(history_events[GoodIndexEnd].eventID, maxDate);
                        }
                    }

                }
                else
                {
                    status = await readsubEvents(startRecord, maxDate);
                }



            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return CommunicationResult.internalFailure;

            }
            finally
            {
                history_events.Sort();

            }
            return status;
            //}
        }


        const int raltime_read_patch = 56;
        bool weHaveOneGoodRTRecord = false;

        const int debug_read_patch = 56;
        bool weHaveOneGoodDebugRecord = false;
        async Task<Tuple<CommunicationResult, byte[], bool, bool>> doReadRTs(UInt32 startRecord, DateTime maxDate, bool done, bool readLimits)
        {
            done = true;
            byte[] resultArray = new byte[1];
            readLimits = false;
            byte[] passData = new byte[5];
            UInt32 maxDateUnixTimeStamp = (UInt32)maxDate.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
            Array.Copy(BitConverter.GetBytes(startRecord), passData, 4);
            passData[4] = raltime_read_patch;
            //if (!batt_mutex.WaitOne(15000)) return new Tuple<commProtocol.Communication_Result, byte[], bool, bool>(commProtocol.Communication_Result.mutexKilled, resultArray, done, readLimits);
            try
            {
                await ForceDelay();
                var tuple = await myCommunicator.MySendRecieve(readRTsCMD, passData, SerialNumber, 0, false, resultArray);
                CommunicationResult result = tuple.Item1;
                resultArray = tuple.Item2;
                LastSentCommandTime = DateTime.UtcNow;
                if (result != CommunicationResult.OK)
                {

                    return new Tuple<CommunicationResult, byte[], bool, bool>(result, resultArray, done, readLimits);
                }



                if (resultArray.Length == 0)
                {
                    return new Tuple<CommunicationResult, byte[], bool, bool>(CommunicationResult.SIZEERROR, resultArray, done, readLimits);
                }
                if (resultArray[0] == 0x7F)
                {
                    readLimits = true;
                    return new Tuple<CommunicationResult, byte[], bool, bool>(CommunicationResult.OK, resultArray, done, readLimits);
                }
                //verify length and size
                int recoevedRecordsCount = resultArray[0];
                if (recoevedRecordsCount == 0)
                {
                    return new Tuple<CommunicationResult, byte[], bool, bool>(CommunicationResult.OK, resultArray, done, readLimits);
                }
                if (recoevedRecordsCount != raltime_read_patch)
                    done = true;
                else
                    done = false;
                if (recoevedRecordsCount * 16 + 1 != resultArray.Length)
                {
                    return new Tuple<CommunicationResult, byte[], bool, bool>(CommunicationResult.SIZEERROR, resultArray, done, readLimits);
                }

                for (UInt32 i = 0; i < recoevedRecordsCount; i++)
                {
                    RealTimeRecord rtRecord = new RealTimeRecord();
                    rtRecord.loadFromBuffer(resultArray, 1 + i * 16);
                    rtRecord.recordID = startRecord + i;
                    if (!rtRecord.valid)
                        continue;
                    if (rtRecord.timestamp > maxDateUnixTimeStamp)
                        break;
                    if (RealTimeRecords.Find(x => x.timestamp == rtRecord.timestamp) == null)
                    {
                        RealTimeRecords.Add(rtRecord);
                    }
                    else
                    {
                        continue;
                    }
                    weHaveOneGoodRTRecord = true;

                }
            }
            catch
            {
                return new Tuple<CommunicationResult, byte[], bool, bool>(CommunicationResult.internalFailure, resultArray, done, readLimits);
            }
            finally
            {
                //batt_mutex.ReleaseMutex();
            }

            return new Tuple<CommunicationResult, byte[], bool, bool>(CommunicationResult.OK, resultArray, done, readLimits);
        }
        async Task<CommunicationResult> readsubrtRecords(UInt32 startRecord, DateTime maxDate)
        {
            UInt32 startPointer = startRecord;
            if (realtimeRecordsStartID == 0xFFFFFFFF)
                return CommunicationResult.OK;

            bool isDone = true;
            do
            {
                bool readLimits = false;
                var tupleExtra = await doReadRTs(startPointer, maxDate, isDone, readLimits);
                CommunicationResult status = tupleExtra.Item1;
                isDone = tupleExtra.Item3;
                readLimits = tupleExtra.Item4;

                //TODO Currently retrcited to 100 records , remove this restriction after completing the correct implementaion
                if (startPointer > 500)
                {
                    isDone = true;
                }

                if (readLimits)
                {
                    isDone = false;
                    var tuple = await ReadRecordsLimits();
                    status = tuple.Item1;
                    if (status != CommunicationResult.OK)
                    {
                        return status;
                    }
                    startRecord = startPointer = realtimeRecordsStartID;
                    continue;
                }
                if (status != CommunicationResult.OK)
                {
                    return status;
                }
                startPointer += raltime_read_patch;
                Debug.WriteLine("startPointer - " + startPointer.ToString());
            } while (!isDone);

            return CommunicationResult.OK;
        }
        public async Task<CommunicationResult> readRealTimeRecords(UInt32 startRecord, DateTime maxDate, bool buSearchTime)
        {
            //lock (savedRTRecordsLock)
            //{
            if (buSearchTime)
                startRecord = searchRTTimeID;

            //search if we have any event with id > searchRTTimeID
            RealTimeRecords.Sort();
            CommunicationResult status = CommunicationResult.OK;
            int GoodIndexStart;
            int GoodIndexEnd;

            if (RealTimeRecords.Count != 0 && weHaveOneGoodRTRecord)
            {

                for (GoodIndexStart = 0; GoodIndexStart < RealTimeRecords.Count; GoodIndexStart++)
                {
                    if (RealTimeRecords[GoodIndexStart].valid)
                        break;
                }
                for (GoodIndexEnd = RealTimeRecords.Count - 1; GoodIndexEnd >= 0; GoodIndexEnd--)
                {
                    if (RealTimeRecords[GoodIndexEnd].valid)
                        break;
                }

                bool skipRightEnd = false;
                bool resort = false;
                if (RealTimeRecords[GoodIndexStart].recordID > startRecord)
                {
                    status = await readsubrtRecords(startRecord, new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(RealTimeRecords[GoodIndexStart].timestamp));
                    if (status != CommunicationResult.OK)
                    {
                        return status;
                    }
                    resort = true;
                }
                else if (RealTimeRecords[GoodIndexEnd].recordID < startRecord)
                {
                    status = await readsubrtRecords(RealTimeRecords[GoodIndexEnd].recordID, maxDate);
                    if (status != CommunicationResult.OK)
                    {
                        return status;
                    }
                    skipRightEnd = true;
                    resort = true;
                }
                if (!skipRightEnd)
                {
                    if (resort)
                    {
                        RealTimeRecords.Sort();
                        for (GoodIndexEnd = RealTimeRecords.Count - 1; GoodIndexEnd >= 0; GoodIndexEnd--)
                        {
                            if (RealTimeRecords[GoodIndexEnd].valid)
                                break;
                        }

                    }
                    if (maxDate > new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(RealTimeRecords[GoodIndexEnd].timestamp + 60))
                    {
                        status = await readsubrtRecords(RealTimeRecords[GoodIndexEnd].recordID, maxDate);
                    }
                }

            }
            else
            {
                status = await readsubrtRecords(startRecord, maxDate);
            }

            RealTimeRecords.Sort();
            return status;
            //}

        }


        public async Task<Tuple<CommunicationResult, byte[]>> readRunningRT()
        {
            //lock (savedRuuningRTLock)
            //{
            byte[] resultArray = new byte[1];
            savedRuuningRT = new RealTimeRecord();
            //if (!batt_mutex.WaitOne(15000)) return new Tuple<commProtocol.Communication_Result, byte[]>(commProtocol.Communication_Result.mutexKilled, resultArray);
            try
            {
                await ForceDelay();
                var tuple = await myCommunicator.MySendRecieve(getRunningRT, null, SerialNumber, 16, true, resultArray);
                CommunicationResult result = tuple.Item1;
                LastSentCommandTime = DateTime.UtcNow;
                if (result != CommunicationResult.OK)
                {

                    return new Tuple<CommunicationResult, byte[]>(result, resultArray);
                }

                savedRuuningRT.loadFromBuffer(resultArray, 0);

            }
            catch
            {
                return new Tuple<CommunicationResult, byte[]>(CommunicationResult.internalFailure, resultArray);
            }
            finally
            {
                //batt_mutex.ReleaseMutex();
            }
            return new Tuple<CommunicationResult, byte[]>(CommunicationResult.OK, resultArray);
            //}

        }
        public async Task<Tuple<CommunicationResult, byte[]>> SETSOC(byte soc)
        {
            byte[] passData = new byte[5];
            UInt32 userID = ControlObject.userID;
            Array.Copy(BitConverter.GetBytes(userID), passData, 4);
            passData[4] = soc;

            //if (!batt_mutex.WaitOne(15000)) return new Tuple<commProtocol.Communication_Result, byte[]>(commProtocol.Communication_Result.mutexKilled, new byte[1]);
            try
            {
                byte[] tempArr = new byte[1];
                await ForceDelay();
                var tuple = await myCommunicator.MySendRecieve(setSOC, passData, SerialNumber, 0, true, tempArr);
                CommunicationResult result = tuple.Item1;
                tempArr = tuple.Item2;
                LastSentCommandTime = DateTime.UtcNow;
                return new Tuple<CommunicationResult, byte[]>(result, tempArr);
            }
            catch
            {
                return new Tuple<CommunicationResult, byte[]>(CommunicationResult.internalFailure, new byte[1]);
            }
            finally
            {
                //batt_mutex.ReleaseMutex();
            }


        }


        async Task<Tuple<CommunicationResult, byte[], bool, bool>> doReadDebugs(UInt32 startDebugRecord, DateTime maxDate, bool done, bool reReadLimits)
        {
            reReadLimits = false;
            done = true;
            byte[] resultArray = new byte[1];
            byte[] passData = new byte[5];
            UInt32 maxDateUnixTimeStamp = (UInt32)maxDate.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
            Array.Copy(BitConverter.GetBytes(startDebugRecord), passData, 4);
            passData[4] = debug_read_patch;
            //if (!batt_mutex.WaitOne(15000))
            //	return new Tuple<commProtocol.Communication_Result, byte[], bool, bool>(commProtocol.Communication_Result.mutexKilled, resultArray, done, reReadLimits);
            try
            {
                await ForceDelay();
                var tuple = await myCommunicator.MySendRecieve(readDebugRecordsCMD, passData, SerialNumber, 0, false, resultArray);
                CommunicationResult result = tuple.Item1;
                resultArray = tuple.Item2;
                LastSentCommandTime = DateTime.UtcNow;

                if (result != CommunicationResult.OK)
                {
                    return new Tuple<CommunicationResult, byte[], bool, bool>(result, resultArray, done, reReadLimits);
                }



                if (resultArray.Length == 0)
                {
                    return new Tuple<CommunicationResult, byte[], bool, bool>(CommunicationResult.SIZEERROR, resultArray, done, reReadLimits);
                }
                if (resultArray[0] == 0x7F)
                {
                    reReadLimits = true;
                    return new Tuple<CommunicationResult, byte[], bool, bool>(CommunicationResult.OK, resultArray, done, reReadLimits);
                }
                //verify length and size
                int recoevedRecordsCount = resultArray[0];
                if (recoevedRecordsCount == 0)
                {
                    return new Tuple<CommunicationResult, byte[], bool, bool>(CommunicationResult.OK, resultArray, done, reReadLimits);
                }
                if (recoevedRecordsCount != debug_read_patch)
                    done = true;
                else
                    done = false;
                if (recoevedRecordsCount * 16 + 1 != resultArray.Length)
                {
                    return new Tuple<CommunicationResult, byte[], bool, bool>(CommunicationResult.SIZEERROR, resultArray, done, reReadLimits);
                }

                for (Int32 i = 0; i < recoevedRecordsCount; i++)
                {
                    DebugRecord debugRecord = new DebugRecord();
                    debugRecord.loadFromBuffer(resultArray, 1 + i * 16);
                    //if (!debugRecord.valid)
                    //    continue;
                    if (debugRecord.timestamp > maxDateUnixTimeStamp)
                        break;
                    if (savedDebugRecords.Find(x => x.ID == debugRecord.ID) == null)
                    {
                        savedDebugRecords.Add(debugRecord);
                    }
                    else
                    {
                        continue;
                    }
                    weHaveOneGoodDebugRecord = true;

                }

            }
            catch
            {
                return new Tuple<CommunicationResult, byte[], bool, bool>(CommunicationResult.internalFailure, resultArray, done, reReadLimits);
            }
            finally
            {
                //batt_mutex.ReleaseMutex();
            }
            return new Tuple<CommunicationResult, byte[], bool, bool>(CommunicationResult.OK, resultArray, done, reReadLimits);
        }
        async Task<CommunicationResult> readsubdebugRecords(UInt32 startDebugRecord, DateTime maxDate)
        {
            UInt32 startPointer = startDebugRecord;
            if (debugRecordsStartID == 0xFFFFFFFF)
                return CommunicationResult.OK;

            bool isDone = true;
            do
            {
                bool reReadLimits = false;
                var tupleExtra = await doReadDebugs(startPointer, maxDate, isDone, reReadLimits);
                CommunicationResult status = tupleExtra.Item1;
                isDone = tupleExtra.Item3;
                reReadLimits = tupleExtra.Item4;
                if (reReadLimits)
                {
                    isDone = false;
                    var tuple = await ReadRecordsLimits();
                    status = tuple.Item1;
                    if (status != CommunicationResult.OK)
                    {
                        return status;
                    }
                    startDebugRecord = startPointer = debugRecordsStartID;
                    continue;
                }
                if (status != CommunicationResult.OK)
                {
                    return status;
                }
                startPointer += debug_read_patch;
                await Task.Delay(1250);

            } while (!isDone);

            return CommunicationResult.OK;
        }
        public async Task<CommunicationResult> readDebugRecords(UInt32 startDebugRecord, DateTime maxDate, bool buSearchTime)
        {

            //lock (savedDebugRecordsLock)
            //{
            if (buSearchTime)
                startDebugRecord = searchDebugTimeID;

            //search if we have any event with id > searchDebugTimeIDb
            savedDebugRecords.Sort();
            CommunicationResult status = CommunicationResult.OK;
            int GoodIndexStart;
            int GoodIndexEnd;

            if (savedDebugRecords.Count != 0 && weHaveOneGoodDebugRecord)
            {

                for (GoodIndexStart = 0; GoodIndexStart < savedDebugRecords.Count; GoodIndexStart++)
                {
                    if (savedDebugRecords[GoodIndexStart].valid)
                        break;
                }
                for (GoodIndexEnd = savedDebugRecords.Count - 1; GoodIndexEnd >= 0; GoodIndexEnd--)
                {
                    if (savedDebugRecords[GoodIndexEnd].valid)
                        break;
                }

                bool skipRightEnd = false;
                bool resort = false;
                if (savedDebugRecords[GoodIndexStart].ID > startDebugRecord)
                {
                    status = await readsubdebugRecords(startDebugRecord, new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(savedDebugRecords[GoodIndexStart].timestamp));
                    if (status != CommunicationResult.OK)
                    {
                        return status;
                    }
                    resort = true;
                }
                else if (savedDebugRecords[GoodIndexEnd].ID < startDebugRecord)
                {
                    status = await readsubdebugRecords(savedDebugRecords[GoodIndexEnd].ID, maxDate);
                    if (status != CommunicationResult.OK)
                    {
                        return status;
                    }
                    skipRightEnd = true;
                    resort = true;
                }
                if (!skipRightEnd)
                {
                    if (resort)
                    {
                        savedDebugRecords.Sort();
                        for (GoodIndexEnd = savedDebugRecords.Count - 1; GoodIndexEnd >= 0; GoodIndexEnd--)
                        {
                            if (savedDebugRecords[GoodIndexEnd].valid)
                                break;
                        }

                    }
                    if (maxDate > new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(savedDebugRecords[GoodIndexEnd].timestamp + 60))
                    {
                        status = await readsubdebugRecords(savedDebugRecords[GoodIndexEnd].ID, maxDate);
                    }
                }

            }
            else
            {
                status = await readsubdebugRecords(startDebugRecord, maxDate);
            }

            savedDebugRecords.Sort();

            return status;
        }

        internal override byte GetNoneEsp32RequestBootLoaderCommand()
        {
            return requestUpdateFirmware;
        }

        internal override byte[] GetRequestBootLoaderUpdatePassedData()
        {
            byte[] passedData = new byte[4];

            if (!IsEsp32WiFi)
                Array.Copy(BitConverter.GetBytes(ControlObject.userID), passedData, 4);

            return passedData;
        }

        public async override Task<Tuple<bool, string>> UpdateDcFirmware(byte id, bool restart)
        {
            if (id != 0)
                await Task.Delay(10);

            return new Tuple<bool, string>(true, "");
        }

        public async Task<CommunicationResult> sendOneFirmwarePacket(byte[] data, int step)
        {
            CommunicationResult result = CommunicationResult.internalFailure;
            //if (!batt_mutex.WaitOne(15000)) return commProtocol.Communication_Result.mutexKilled;
            if (data.Length > 1024)
                return CommunicationResult.internalFailure;

            try
            {
                byte[] temp = new byte[data.Length + 4];
                byte[] tempArr = new byte[1];

                Array.Copy(BitConverter.GetBytes((UInt32)step * 1024), 0, temp, 0, 4);
                Array.Copy(data, 0, temp, 4, data.Length);

                await ForceDelay();
                var tuple = await myCommunicator.MySendRecieve(saveToNewfirmware, temp, SerialNumber, 0, true, tempArr, TimeoutLevel.shortTimeout, false);
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
            BattViewCommunicationTypes caller = BattViewCommunicationTypes.NOCall;
            object arg1 = null;
            string msg = "";

            float currentFirmwareVersion = FirmwareRevision;

            if (!Firmware.DoesBattViewRequireFirmwareUpdate(currentFirmwareVersion))
            {
                msg = "BATTView has the latest Firmware";

                FirmwareUpdateStep = FirmwareUpdateStage.updateIsNotNeeded;

                return new Tuple<bool, string>(false, msg);
            }

            byte[] serials = null;
            Firmware firmwareManager = new Firmware();
            FirmwareResult res = firmwareManager.UpdateFileBinary(DeviceBaseType.BATTVIEW, ref serials);

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
                caller = BattViewCommunicationTypes.firmwareWrite;
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
                var callerStatus = (BattViewCommunicationTypes)results[1];
                var status = (CommunicationResult)results[0];
                if (callerStatus == BattViewCommunicationTypes.firmwareUpdateRequest)
                {
                    if (status == CommunicationResult.COMMAND_DELAYED)
                    {
                        msg = "BATTView is busy, the firmware update will take place automatically.";

                        FirmwareUpdateStep = FirmwareUpdateStage.sentRequestDelayed;
                    }
                    else if (status == CommunicationResult.OK)
                    {
                        msg = "BATTView is reflushing Firmware..it may take few minutes to update the firmware";

                        FirmwareUpdateStep = FirmwareUpdateStage.sentRequestPassed;
                    }
                    else
                    {
                        msg = "Something went wrong, please retry";

                        FirmwareUpdateStep = FirmwareUpdateStage.FAILED;
                    }

                }
                else if (callerStatus == BattViewCommunicationTypes.firmwareWrite)
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
            BattViewCommunicationTypes caller = (BattViewCommunicationTypes)genericlist[0];
            object arg1 = genericlist[1];

            byte[] serials = (byte[])arg1;
            DateTime start = DateTime.UtcNow;
            status = await WriteToBootLoaderFlash(serials, serials.Length);

            if (excecDspic)
            {
                caller = BattViewCommunicationTypes.firmwareUpdateRequest;

                result.Add(status);
                result.Add(caller);

                return result;
            }

            if (status == CommunicationResult.OK)
            {
                status = await RequestBootLoaderUpdate(excecDspic);

                if (status == CommunicationResult.OK)
                    connectionManager.ForceSoftDisconnectDevice(SerialNumber, false, true);

                caller = BattViewCommunicationTypes.firmwareUpdateRequest;
            }

            Logger.AddLog(false, "Battview update took:" + (DateTime.UtcNow - start).TotalSeconds.ToString());

            result.Add(status);
            result.Add(caller);

            return result;
        }

        internal override byte GetNoneEsp32UpdateFirmwareCommand()
        {
            return saveToNewfirmware;
        }

        #endregion

        public override void Cancel()
        {
            if (myCommunicator != null)
            {
                myCommunicator.Cancel();
            }
        }

        public async Task<CommunicationResult> setInternalBattViewID()
        {
            byte[] passData = new byte[4];
            Array.Copy(BitConverter.GetBytes(Config.id), 0, passData, 0, 4);

            CommunicationResult result;
            try
            {
                byte[] resultArray = new byte[1];
                await ForceDelay();
                var tuple = await myCommunicator.MySendRecieve(setInternalBattViewIDCMD, passData, SerialNumber, 2, true, resultArray);
                result = tuple.Item1;
                resultArray = tuple.Item2;
                LastSentCommandTime = DateTime.UtcNow;
                if (result == CommunicationResult.OK)
                    Config.memorySignature = BitConverter.ToUInt16(resultArray, 0);
            }
            catch
            {
                return CommunicationResult.internalFailure;
            }

            return result;
        }


        #region New Download

        public async Task<List<BattViewObjectEvent>> readEvents_synchRightx(UInt32 startRecordId, UInt32 maxRecordtoRead)
        {
            if (IsEsp32WiFi)
                return await readEvents_synchRightx_esp32(startRecordId, maxRecordtoRead);

            return await readEvents_synchRightx_old(startRecordId, maxRecordtoRead);
        }

        async Task<List<BattViewObjectEvent>> readEvents_synchRightx_esp32(UInt32 startRecordId, UInt32 maxRecordtoRead)
        {
            List<BattViewObjectEvent> RightSideEvents = new List<BattViewObjectEvent>();
            List<Tuple<CommunicationResult, byte[]>> result = new List<Tuple<CommunicationResult, byte[]>>();

            if (startRecordId > maxRecordtoRead)
                return RightSideEvents;

            UInt32 patchesCount = (maxRecordtoRead - startRecordId) / esp32_patchsize;
            if ((maxRecordtoRead - startRecordId) % esp32_patchsize != 0)
                patchesCount++;

            if (patchesCount > 10)
                patchesCount = 10;

            List<CommandObject> commands = new List<CommandObject>();

            for (int i = 0; i < patchesCount; i++)
            {
                byte[] passData = new byte[6];

                Array.Copy(BitConverter.GetBytes(startRecordId + i * esp32_patchsize), passData, 4);
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

                Int16 recoevedRecordsCount = BitConverter.ToInt16(resultArray, 0);
                if (recoevedRecordsCount == -1)
                    return RightSideEvents;

                if (recoevedRecordsCount == 0)
                    return RightSideEvents;

                if (recoevedRecordsCount * 64 + 2 != resultArray.Length)
                    return RightSideEvents;

                for (int i = 0; i < recoevedRecordsCount; i++)
                {
                    BattViewObjectEvent eventRecord = new BattViewObjectEvent();

                    eventRecord.loadFromBuffer(resultArray, 2 + i * 64);

                    if (!eventRecord.valid)
                        eventRecord.eventID = startRecordId + (uint)i;

                    if (RightSideEvents.Find(x => x.eventID == eventRecord.eventID) == null)
                        RightSideEvents.Add(eventRecord);

                }

                startRecordId += (uint)recoevedRecordsCount;
            }

            return RightSideEvents;
        }

        async Task<List<BattViewObjectEvent>> readEvents_synchRightx_old(UInt32 startRecordId, UInt32 maxRecordtoRead)
        {
            List<BattViewObjectEvent> RightSideEvents = new List<BattViewObjectEvent>();
            List<Tuple<CommunicationResult, byte[]>> result = new List<Tuple<CommunicationResult, byte[]>>();

            if (startRecordId > maxRecordtoRead)
                return RightSideEvents;

            UInt32 patchesCount = (maxRecordtoRead - startRecordId) / events_read_patch;

            if ((maxRecordtoRead - startRecordId) % events_read_patch != 0)
                patchesCount++;

            if (patchesCount > 10)
                patchesCount = 10;

            var commands = new List<CommandObject>();

            for (int i = 0; i < patchesCount; i++)
            {
                byte[] passData = new byte[5];

                Array.Copy(BitConverter.GetBytes(startRecordId + i * events_read_patch), passData, 4);
                passData[4] = events_read_patch;

                CommandObject commandObject = new CommandObject()
                {
                    CommandBytes = readEventsCMD,
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

                if (recoevedRecordsCount * 64 + 1 != resultArray.Length)
                    return RightSideEvents;

                for (int i = 0; i < recoevedRecordsCount; i++)
                {
                    BattViewObjectEvent eventRecord = new BattViewObjectEvent();

                    eventRecord.loadFromBuffer(resultArray, 1 + i * 64);

                    if (!eventRecord.valid)
                        eventRecord.eventID = startRecordId + (uint)i;

                    if (RightSideEvents.Find(x => x.eventID == eventRecord.eventID) == null)
                        RightSideEvents.Add(eventRecord);
                }

                startRecordId += (uint)recoevedRecordsCount;
            }

            return RightSideEvents;
        }

        #endregion

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

        public void SaveDefaultChargeProfile()
        {
            byte batteryType = Config.batteryType;

            Config.trickleCurrentRate = 500;

            Config.FIcurrentRate = 500;
            Config.EQcurrentRate = 400;
            Config.trickleVoltage = 200;

            Config.FItargetVoltage = 260;
            Config.EQvoltage = 265;
            Config.CVendCurrentRate = 24;
            Config.CVcurrentStep = 0;
            Config.cvMaxDuration = 14400;
            Config.FIduration = 10800;
            Config.EQduration = 14400;
            Config.desulfation = 43200;
            Config.FIdv = 5;
            Config.FIdt = 59;
            Config.FIcloseWindow = 86400;
            Config.FIdaysMask = 0x7f;
            switch (Config.chargerType)
            {
                case 0: //FAST
                    Config.CCrate = 4000;
                    Config.CVTargetVoltage = 242;

                    Config.EQdaysMask = (0x01);

                    break;

                case 1: //Conventional
                    Config.CCrate = 1700;
                    Config.CVTargetVoltage = 237;

                    Config.EQdaysMask = (0x01);

                    break;

                case 2: //Opp
                    Config.CCrate = 2500;
                    Config.CVTargetVoltage = 240;

                    Config.EQdaysMask = (0x01);

                    break;
            }

            Config.EQcloseWindow = 86400;
            Config.EQstartWindow = 0;
            Config.FIstartWindow = 0;
            Config.blockedEQDays = 0;
            Config.blockedFIDays = 0;

            if (batteryType == 2)
            {
                Config.FItargetVoltage = (ushort)Math.Round(100.0f * 2.55f);
                Config.EQvoltage = (ushort)Math.Round(100.0f * 2.55f);
                Config.CVendCurrentRate = 8 * 2;
                Config.cvMaxDuration = 3 * 3600;
                Config.FIduration = 4 * 3600;

                Config.CVTargetVoltage = (ushort)Math.Round(100.0f * 2.33f);
                Config.CCrate = 1700;

                Config.FIstartWindow = 0;
                Config.FIcloseWindow = 86400;
                Config.FIdaysMask = 0x7F;

                Config.EQstartWindow = 0;
                Config.EQcloseWindow = 0;
                Config.EQdaysMask = 0;

                Config.trickleCurrentRate = 500;
                Config.FIcurrentRate = 150;
                Config.EQcurrentRate = 100;
                Config.enableExtTempSensing = 0;
                Config.enableElectrolyeSensing = 0;
            }
            else if(batteryType == 0 && Config.isPA == 0)
            {
                Config.enableExtTempSensing = 1;
                Config.enableElectrolyeSensing = 1;
            }
        }

        internal override bool IsCommissioned()
        {
            if (Config == null)
                return false;

            return Config.id > MIN_COMMISSIONED_ID;
        }

        public void Dispose()
        {
            mutex.Dispose();
        }
    }
}
