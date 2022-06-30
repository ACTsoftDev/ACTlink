using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using actchargers.Code.Utility;

namespace actchargers
{
    public sealed class BattViewQuantum
    {
        ConnectionManager connManager;
        static readonly BattViewQuantum _instance = new BattViewQuantum();
        private BattViewObject oldBATTViewData;
        BattTestStages batt_testStage;
        public static BattViewQuantum Instance
        {
            get
            {
                return _instance;
            }
        }
        internal void SetConnectionManager(ConnectionManager ConnManager)
        {
            this.connManager = ConnManager;
        }

        internal ConnectionManager GetConnectionManager()
        {
            return connManager;
        }

        internal BattViewObject GetBATTView()
        {
            if (connManager == null)
            {
                return null;
            }
            else
            {
                return connManager.activeBattView;
            }

        }

        internal void Clear()
        {
            connManager = null;
        }
        //Batt_CalibrationCommunication_Click_doWork functionality
        public async Task<List<object>> Batt_CalibrationCommunication(List<object> args)
        {
            CommunicationResult status = CommunicationResult.NOT_EXIST;
            List<object> genericlist = args;
            List<object> arguments = new List<object>();
            bool internalFailure = false;
            string internalFailureString = "";
            bool isvoltage = (bool)genericlist[0];
            bool isTemp = (bool)genericlist[1];
            bool isPA = (bool)genericlist[2];
            bool isPALowRange = (bool)genericlist[3];
            try
            {
                var tuple = await Instance.GetConnectionManager().activeBattView.ReadADCValues();
                status = tuple.Item1;

            }
            catch (Exception ex)
            {
                internalFailure = true;
                internalFailureString = ex.ToString();
            }
            arguments.Add(internalFailure);
            arguments.Add(internalFailureString);
            arguments.Add(status);
            arguments.Add(isvoltage);
            arguments.Add(isTemp);
            arguments.Add(isPA);
            arguments.Add(isPALowRange);
            return arguments;
        }




        public async Task<List<object>> CommunicateBATTView(List<object> arguments)
        {
            CommunicationResult status = CommunicationResult.NOT_EXIST;
            List<object> genericlist = arguments;
            List<object> result = new List<object>();
            bool internalFailure = false;
            string internalFailureString = "";
            BattViewCommunicationTypes caller = (BattViewCommunicationTypes)genericlist[0];
            object arg1 = genericlist[1];
            List<object> extraObjectList = new List<object>();
            try
            {

                switch (caller)
                {
                    case BattViewCommunicationTypes.plc_Cal:
                        {
                            var tuple = await Task.Run(async () =>
                            {
                                return await connManager.activeBattView.requectPLCCalibration();
                            });                             status = tuple.Item1;
                        }
                        break;
                    case BattViewCommunicationTypes.replaceDevice:
                        {
                            //get JSON object
                            var d =
                                DbSingleton.DBManagerServiceInstance
                                           .GetSynchObjectsBufferedDataLoader()
                                           .GetSynchedDevices
                                           (false, (UInt32)arg1);
                            //format

                            string configbuff = d[0].Config.Remove(0, 1);
                            configbuff = configbuff.Remove(configbuff.Length - 1, 1);
                            string[] bytes = configbuff.Split(new char[] { ',' });
                            byte[] toSend = new byte[512];
                            int x = 0;
                            foreach (string sub in bytes)
                            {
                                toSend[x++] = byte.Parse(sub);
                            }
                            connManager.activeBattView.myZone = (byte)d[0].Zone;

                            var tuple = await Task.Run(async () =>
                           {
                               return await connManager.activeBattView.SaveTime();
                           });

                            status = tuple.Item1;

                            if (status == CommunicationResult.OK)
                            {
                                BattViewConfig conf = connManager.activeBattView.getCopyOfArrayForReplacment(toSend);
                                tuple = await Task.Run(async () =>
                            {
                                return await connManager.activeBattView.setConfigFromConfig(conf);
                            });
                                status = tuple.Item1;

                                if (status == CommunicationResult.OK)
                                {
                                    ReplaceAndUpdate
                                    (conf, connManager.activeBattView.globalRecord,
                                         (UInt32)arg1, d[0].SerialNumber);

                                    tuple = await Task.Run(async () =>
                                    {
                                        return await connManager.activeBattView.restart();
                                    }); 
                                    status = tuple.Item1;

                                    caller = BattViewCommunicationTypes.restartDevice;
                                    connManager.ForceSoftDisconnectDevice(connManager.getWorkingSerialNumber(), true);
                                }

                            }

                        }
                        break;
                    case BattViewCommunicationTypes.resetFactorySettings:
                        var resultTuple = await connManager.activeBattView.SaveConfigToDevice();
                        status = resultTuple.Item1;
                        if (status == CommunicationResult.OK)
                        {
                            resultTuple = await Task.Run(async () =>
                              {
                                  return await connManager.activeBattView.SaveTime();
                              });

                            status = resultTuple.Item1;
                            if (status == CommunicationResult.OK)
                            {
                                status = await Task.Run(async () =>
                              {
                                  return await connManager.activeBattView.setInternalBattViewID();
                              });

                                if (status == CommunicationResult.OK)
                                {
                                    resultTuple = await Task.Run(async () =>
                                  {
                                      return await connManager.activeBattView.resetGlobalRecord();
                                  });

                                    status = resultTuple.Item1;
                                    connManager.ForceSoftDisconnectDevice(connManager.getWorkingSerialNumber());

                                    caller = BattViewCommunicationTypes.restartDevice;
                                }
                            }
                        }
                        break;
                    case BattViewCommunicationTypes.saveConfigAndRestart:
                        {
                            var tuple = await Task.Run(async () =>
                             {
                                 return await Instance.GetConnectionManager().activeBattView.SaveConfigToDevice();
                             });

                            status = tuple.Item1;
                            if (status == CommunicationResult.OK)
                            {
                                connManager.siteView.setDeviceConfigurationSaved(connManager.getWorkingSerialNumber(), false);

                                tuple = await Task.Run(async () =>
                            {
                                return await connManager.activeBattView.restart();
                            });
                                status = tuple.Item1;
                                connManager.ForceSoftDisconnectDevice(connManager.getWorkingSerialNumber(), true);

                                caller = BattViewCommunicationTypes.restartDevice;
                            }
                        }
                        break;
                    case BattViewCommunicationTypes.startNewStudy:
                        {
                            UInt32 tempSyudyID = Instance.GetConnectionManager().activeBattView.Config.studyId;

                            var tuple = await Task.Run(async () =>
                            {
                                return await Instance.GetConnectionManager().activeBattView.SaveConfigToDevice();
                            });

                            status = tuple.Item1;
                            if (status == CommunicationResult.OK)
                            {
                                tuple = await Task.Run(async () =>
                             {
                                 return await connManager.activeBattView.startNewStudy();
                             });

                                status = tuple.Item1;
                                string[] extra = arg1 as string[];

                                connManager.siteView.setDeviceConfigurationSaved(connManager.getWorkingSerialNumber(), false);
                                connManager.ForceSoftDisconnectDevice(connManager.getWorkingSerialNumber(), false);
                            }
                        }
                        break;

                    case BattViewCommunicationTypes.saveConfigTest:
                        {
                            var tuple = await Task.Run(async () =>
                           {
                               return await Instance.GetConnectionManager().activeBattView.SaveConfigToDevice();
                           });

                            status = tuple.Item1;
                            if (batt_testStage == BattTestStages.serialnumberSave && status == CommunicationResult.OK)
                            {
                                UInt32 ti = 0;

                                if (UInt32.TryParse(connManager.activeBattView.Config.battViewSN.Substring(Math.Max(0, connManager.activeBattView.Config.battViewSN.Length - 5)), out ti))
                                {
                                    connManager.activeBattView.Config.id = (ti % 9999) + 1;
                                }
                                else
                                {
                                    Random r = new Random();
                                    connManager.activeBattView.Config.id = (uint)r.Next(1, 9999);

                                }
                                status = await Task.Run(async () =>
                          {
                              return await connManager.activeBattView.setInternalBattViewID();
                          });

                            }
                        }
                        break;
                    case BattViewCommunicationTypes.saveConfigAndTime:
                    case BattViewCommunicationTypes.saveConfigCommission:
                        {
                            var tuple = await Task.Run(async () =>
                         {
                             return await Instance.GetConnectionManager().activeBattView.SaveConfigToDevice();
                         });

                            status = tuple.Item1;

                            if (status == CommunicationResult.OK)
                            {
                                tuple = await Task.Run(async () =>
                            {
                                return await connManager.activeBattView.SaveTime();
                            });

                                status = tuple.Item1;
                                if (status == CommunicationResult.OK)
                                {

                                    bool ISLoad = await Task.Run(async () =>
                             {
                                 return await connManager.activeBattView.DoLoad();
                             });

                                    if (ISLoad)
                                    {
                                        connManager.siteView.setDeviceConfigurationRead(connManager.getWorkingSerialNumber(), true, connManager.activeBattView.DcId, connManager.activeBattView.FirmwareRevision, connManager.activeBattView.FirmwareWiFiVersion, connManager.activeBattView.FirmwareDcVersion);
                                    }

                                    extraObjectList.Add(caller == BattViewCommunicationTypes.saveConfigCommission || (bool)arg1);

                                    status = connManager.activeBattView.getDoLoadStatus();
                                }
                            }
                            if (status == CommunicationResult.OK)
                            {
                                connManager.siteView.setDeviceConfigurationSaved(connManager.getWorkingSerialNumber(), false);
                            }

                        }
                        break;
                    case BattViewCommunicationTypes.Batt_voltagecalibrationSaveConfig:
                    case BattViewCommunicationTypes.Batt_currentcalibrationSaveConfig:
                        {
                            var tuple = await Task.Run(async () =>
                            {
                                return await Instance.GetConnectionManager().activeBattView.SaveConfigToDevice();
                            });

                            status = tuple.Item1;

                            if (status == CommunicationResult.OK)
                            {
                                connManager.siteView.setDeviceConfigurationSaved(connManager.getWorkingSerialNumber(), false);
                            }
                        }
                        break;
                    case BattViewCommunicationTypes.saveConfig:
                    case BattViewCommunicationTypes.saveConfigDisconnect:
                        {
                            var tuple = await Task.Run(async () =>
                           {
                               return await Instance.GetConnectionManager().activeBattView.SaveConfigToDevice();
                           });

                            status = tuple.Item1;
                            if (arg1 != null && (bool)arg1)
                            {
                                connManager.ForceSoftDisconnectDevice(connManager.getWorkingSerialNumber(), caller == BattViewCommunicationTypes.saveConfigDisconnect);
                                extraObjectList.Add(true);
                            }
                            else
                            {
                                extraObjectList.Add(false);
                            }
                            caller = BattViewCommunicationTypes.saveConfig;
                            if (status == CommunicationResult.OK)
                            {

                                connManager.siteView.setDeviceConfigurationSaved(connManager.getWorkingSerialNumber(), false);
                            }

                        }
                        break;
                    case BattViewCommunicationTypes.doFinalComission:
                        {
                            List<object> vars = arg1 as List<object>;
                            Instance.GetConnectionManager().activeBattView.Config.id = (UInt32)vars[0];
                            var tuple = await Task.Run(async () =>
                          {
                              return await Instance.GetConnectionManager().activeBattView.SaveConfigToDevice();
                          });

                            status = tuple.Item1;
                            if (status == CommunicationResult.OK)
                            {
                                status = await Task.Run(async () =>
                             {
                                 return await connManager.activeBattView.setInternalBattViewID();
                             });

                                if (status == CommunicationResult.OK)
                                {
                                    tuple = await Task.Run(async () =>
                        {
                            return await connManager.activeBattView.resetGlobalRecord();
                        });

                                    status = tuple.Item1;
                                    //ACConstants.all_downloadStat.removeKey(false,connManager.activeBattView.config.id, connManager.activeBattView.config.isPA!=0? connManager.activeBattView.config.studyId:0);
                                    //DB_HelperFunctions.DEVICEOBJECT_updateDevice(false, connManager.activeBattView.config.id, connManager.activeBattView.config.TOJSON(), connManager.activeBattView.globalRecord.TOJSON(), connManager.activeBattView.config.memorySignature, connManager.activeBattView.globalRecord.eventsCount, connManager.activeBattView.firmwareRevision, connManager.activeBattView.myZone, connManager.activeBattView.config.studyId);
                                    connManager.ForceSoftDisconnectDevice(connManager.getWorkingSerialNumber());
                                }
                            }
                        }
                        break;
                    case BattViewCommunicationTypes.resetGlobalRecords:
                        {
                            var tuple = await Task.Run(async () =>
                        {
                            return await Instance.GetConnectionManager().activeBattView.resetGlobalRecord();
                        });

                            status = tuple.Item1;
                            //await connManager.activeBattView.doLoad();
                            if (status == CommunicationResult.OK)
                            {
                                connManager.ForceSoftDisconnectDevice(connManager.getWorkingSerialNumber(), false);
                            }
                        }
                        break;
                    case BattViewCommunicationTypes.readAll:
                    case BattViewCommunicationTypes.connectCommand:
                        bool IsDoLoadSuccess = await Task.Run(async () =>
                        {                             return await Instance.GetConnectionManager().activeBattView.DoLoad();                         });

                        if (IsDoLoadSuccess)
                        {
                            connManager.siteView.setDeviceConfigurationRead(connManager.getWorkingSerialNumber(), true, connManager.activeBattView.DcId, connManager.activeBattView.FirmwareRevision, connManager.activeBattView.FirmwareWiFiVersion, connManager.activeBattView.FirmwareDcVersion);
                        }

                        status = Instance.GetConnectionManager().activeBattView.getDoLoadStatus();
                        if (status == CommunicationResult.OK)
                        {
                            connManager.siteView.setDeviceConfigurationSaved(connManager.getWorkingSerialNumber(), false);
                        }
                        break;
                    case BattViewCommunicationTypes.readAll2:
                        await Task.Delay(10000);
                        bool IsSuccess = await Task.Run(async () =>
                       {
                           return await Instance.GetConnectionManager().activeBattView.DoLoad();
                       });

                        if (IsSuccess)
                        {
                            connManager.siteView.setDeviceConfigurationRead(connManager.getWorkingSerialNumber(), true, connManager.activeBattView.DcId, connManager.activeBattView.FirmwareRevision, connManager.activeBattView.FirmwareWiFiVersion, connManager.activeBattView.FirmwareDcVersion);
                        }
                        status = Instance.GetConnectionManager().activeBattView.getDoLoadStatus();
                        break;
                    case BattViewCommunicationTypes.firmwareUpdateRequest:
                        status = await Task.Run(async () =>
                        {
                            return await Instance.GetConnectionManager().activeBattView.RequestBootLoaderUpdate(false);
                        });
                        if (status == CommunicationResult.OK)
                        {
                            connManager.ForceSoftDisconnectDevice(connManager.getWorkingSerialNumber(), false);
                        }
                        break;
                    case BattViewCommunicationTypes.firmwareWrite:
                        byte[] serials = (byte[])arg1;
                        status = await Task.Run(async () =>
                      {
                          return await connManager.activeBattView.WriteToBootLoaderFlash(serials, serials.Length);
                      });

                        if (status == CommunicationResult.OK)
                        {
                            await Task.Delay(500);
                            status = await Task.Run(async () =>
                         {
                             return await connManager.activeBattView.RequestBootLoaderUpdate(false);
                         });

                            if (status == CommunicationResult.OK)
                            {
                                connManager.ForceSoftDisconnectDevice(connManager.getWorkingSerialNumber(), false);
                            }
                            caller = BattViewCommunicationTypes.firmwareUpdateRequest;//
                        }

                        break;
                    case BattViewCommunicationTypes.restartDevice:
                    case BattViewCommunicationTypes.restartDeviceNoDisconnect:
                        {
                            var tuple = await Task.Run(async () =>
                        {
                            return await Instance.GetConnectionManager().activeBattView.restart();
                        });

                            status = tuple.Item1;
                            if (caller != BattViewCommunicationTypes.restartDeviceNoDisconnect && status == CommunicationResult.OK)
                            {
                                connManager.ForceSoftDisconnectDevice(connManager.getWorkingSerialNumber(), false);

                            }
                        }
                        break;

                    case BattViewCommunicationTypes.saveActViewIDandRestart:

                        status = await Task.Run(async () =>
                       {
                           return await Instance.GetConnectionManager().activeBattView.setInternalBattViewID();
                       }); 
                        if (status == CommunicationResult.OK)
                        {
                            var tuple = await Task.Run(async () =>
                        {
                            return await connManager.activeBattView.restart();
                        });

                            status = tuple.Item1;

                            caller = BattViewCommunicationTypes.restartDevice;//
                        }
                        if (status == CommunicationResult.OK)
                        {
                            connManager.siteView.setDeviceConfigurationSaved(connManager.getWorkingSerialNumber(), false);
                            connManager.ForceSoftDisconnectDevice(connManager.getWorkingSerialNumber(), true);
                        }
                        break;
                    case BattViewCommunicationTypes.quickView:
                    case BattViewCommunicationTypes.quickViewDirect:
                        {
                            var tuple = await Task.Run(async () =>
                            {
                                return await Instance.GetConnectionManager().activeBattView.getQuickView();
                            }); 
                            status = tuple.Item1;
                        }
                        break;
                    case BattViewCommunicationTypes.loadDebugAnalog:
                    case BattViewCommunicationTypes.loadDebugAnalog2:
                        {
                            var tuple = await Task.Run(async () =>
                           {
                               return await Instance.GetConnectionManager().activeBattView.ReadADCValues();
                           });

                            status = tuple.Item1;
                        }
                        break;
                    //
                    case BattViewCommunicationTypes.setSOC:
                        {
                            var tuple = await Task.Run(async () =>
                         {
                             return await Instance.GetConnectionManager().activeBattView.SETSOC((byte)arg1);
                         });

                            status = tuple.Item1;
                        }
                        break;
                    case BattViewCommunicationTypes.readDebugRecords:
                        {
                            var tuple = await Task.Run(async () =>
                          {
                              return await Instance.GetConnectionManager().activeBattView.readRunningRT();
                          });

                            status = tuple.Item1;
                            if (status == CommunicationResult.OK)
                            {
                                tuple = await Task.Run(async () =>
                                {                                     return await connManager.activeBattView.readRunningEvent();
                                }); 
                                status = tuple.Item1;
                            }
                        }
                        break;
                    case BattViewCommunicationTypes.setDebugRecord:
                        {
                            List<object> list = (List<object>)arg1;
                            var tuple = await Task.Run(async () =>
                                {
                                    return await Instance.GetConnectionManager().activeBattView.controlAnalog((bool)list[0],
                                 (float)list[1], (float)list[2], (float)list[3], (float)list[4], (float)list[5],
                                  (UInt16)list[6], (float)list[7], (UInt16)list[8], (UInt16)list[9], (UInt16)list[10]);
                                });

                            status = tuple.Item1;
                            if (status == CommunicationResult.OK)
                            {
                                tuple = await Task.Run(async () =>
                                {
                                    return await connManager.activeBattView.readRunningRT();
                                });

                                status = tuple.Item1;
                                if (status == CommunicationResult.OK)
                                {
                                    tuple = await Task.Run(async () =>
                                    {
                                        return await connManager.activeBattView.readRunningEvent();
                                    });

                                    status = tuple.Item1;
                                }
                            }
                        }
                        break;
                    case BattViewCommunicationTypes.loadPLCFirmware:
                        {
                            byte[] plc_serials = (byte[])arg1;
                            var tuple = await Task.Run(async () =>
                                {
                                    return await Instance.GetConnectionManager().activeBattView.writetoPLCFlash(plc_serials, plc_serials.Length);
                                });

                            status = tuple.Item1;
                            if (status == CommunicationResult.OK)
                            {
                                tuple = await Task.Run(async () =>
                                {
                                    return await connManager.activeBattView.requestPLCUpdate();
                                });

                                status = tuple.Item1;
                            }
                        }
                        break;
                    default:
                        {
                            status = CommunicationResult.NOT_EXIST;
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                internalFailure = true;
                internalFailureString = ex.ToString();
            }
            result.Add(internalFailure);
            result.Add(internalFailureString);
            result.Add(status);
            result.Add(caller);
            result.Add(extraObjectList);

            return result;

        }

        void ReplaceAndUpdate
        (BattViewConfig conf, BattViewObjectGlobalRecord globalRecord,
         uint newDeviceID, string originalSN)
        {
            UInt32 originalDeviceID = conf.id;
            string newDeviceSN = conf.battViewSN;

            bool isMcb = false;
            string configJson = JsonParser.SerializeObject(conf);
            string globalRecordJson = globalRecord.ToJson();
            int memorySignature = conf.memorySignature;
            UInt32 eventsCount = globalRecord.eventsCount;
            float firmwareVersion = conf.firmwareversion;
            byte zone = conf.zoneid;
            UInt32 battviewStudyID = 0;

            DbSingleton.DBManagerServiceInstance.GetReplaceDevicesLoaders()
                       .InsertOrUpdateBattviewUsingFielfds
                       (originalDeviceID, originalSN, newDeviceID, newDeviceSN);

            DbSingleton.DBManagerServiceInstance
                       .GetDevicesObjectsLoader()
                       .InsertOrUpdateDevice
                       (isMcb, originalDeviceID, configJson, globalRecordJson,
                        memorySignature, eventsCount,
                        firmwareVersion, zone, battviewStudyID);

            var toDelete = new SynchObjectsBufferedData()
            {
                Id = newDeviceID
            };
            DbSingleton.DBManagerServiceInstance
                       .GetSynchObjectsBufferedDataLoader()
                       .Delete(toDelete);
        }

        internal void ResetBATTViewData()
        {
            if (oldBATTViewData != null)
            {
                Instance.GetConnectionManager().activeBattView = oldBATTViewData;
            }
        }



        internal bool BattView_quickSerialNumberCheckStrict()
        {
            return (Instance.GetConnectionManager().getWorkingSerialNumber().StartsWith("BATT_", StringComparison.CurrentCulture) && Instance.GetConnectionManager().activeBattView.deviceIsLoaded);
        }

        internal void SaveBATTViewData()
        {
            oldBATTViewData = Instance.GetConnectionManager().activeBattView;

        }


        public bool batt_verifyBAttViewSerialNumber(string sn, ref string model)
        {
            if (sn.Length != 12)
                return false;
            //if (ControlObject.isDebugMaster)
            //    return true;
            List<string> validModels =
                new List<string>
            { "10", "11", "20", "21", "30" };
            string productFamily = sn.Substring(0, 1);
            model = sn.Substring(1, 2);
            string month = sn.Substring(3, 2);
            string year = sn.Substring(5, 2);
            string subid = sn.Substring(7, 5);
            int tempInt = 0;
            if (productFamily != "3" || !validModels.Contains(model)
                || !int.TryParse(month, out tempInt) || tempInt < 1 || tempInt > 12
                || !int.TryParse(year, out tempInt) || tempInt < 16 || tempInt > 99
                || !int.TryParse(subid, out tempInt) || tempInt < 0 || tempInt > 99999)

            {
                return false;
            }
            return true;
        }

        public void Batt_saveDefaultChargeProfile()
        {
            if (!BattView_quickSerialNumberCheckStrict())
                return;

            GetBATTView().SaveDefaultChargeProfile();
        }

        public void Batt_loadDefaultWifiSettings()
        {
            if (!BattView_quickSerialNumberCheckStrict())
                return;

            BattViewObject activeBattView = GetBATTView();
            activeBattView.Config.mobileAccessSSIDpassword = "shlonak5al";
            activeBattView.Config.mobileAccessSSID = "act24mobile";
            activeBattView.Config.mobilePort = 50000;
            activeBattView.Config.IsSoftApEnable = false;
            activeBattView.Config.softAPpassword = "actDirmank";
            activeBattView.Config.ActViewEnabled = false;
            activeBattView.Config.actViewIP = "act-view.com";
            activeBattView.Config.actViewPort = 9309;
            activeBattView.Config.actViewConnectFrequency = 900;
            activeBattView.Config.actAccessSSIDpassword = "hala3ami102";
            activeBattView.Config.actAccessSSID = "actAccess24";
        }

        public async Task<List<object>> ReadHistoryRecords(List<object> arguments)
        {
            if (!BattView_quickSerialNumberCheckStrict())
                return new List<object>();

            CommunicationResult status = CommunicationResult.NOT_EXIST;
            bool internalFailure = false;
            string internalFailureString = "";

            List<object> genericlist = arguments as List<object>;
            UInt32 startRecord = (UInt32)genericlist[0];
            bool byID = (bool)genericlist[1];
            DateTime startTime = (DateTime)genericlist[2];
            DateTime endTime = (DateTime)genericlist[3];
            int type = (int)genericlist[4];
            try
            {
                if (type == 1)
                {
                    if (byID)
                    {
                        status = await connManager.activeBattView.readEvents(startRecord, DateTime.MaxValue, false);
                    }
                    else
                    {
                        var tuple = await connManager.activeBattView.searchEventRecordByDate(startTime.Date);
                        status = tuple.Item1;
                        status = await connManager.activeBattView.readEvents(startRecord, endTime.Date.AddDays(1), true);
                    }
                }
                else if (type == 2)
                {
                    if (byID)
                    {
                        status = await connManager.activeBattView.readRealTimeRecords(startRecord, DateTime.UtcNow.Add(new TimeSpan(365, 0, 0, 0, 0)), false);
                    }
                    else
                    {
                        var tuple = await connManager.activeBattView.searchRTRecordByDate(startTime.Date);
                        status = tuple.Item1;
                        status = await connManager.activeBattView.readRealTimeRecords(startRecord, endTime.Date.AddDays(1).AddSeconds(-1), true);
                    }
                }
                else if (type == 3)
                {
                    if (byID)
                    {
                        status = await connManager.activeBattView.readDebugRecords(startRecord, DateTime.UtcNow.Add(new TimeSpan(365, 0, 0, 0, 0)), false);
                    }
                    else
                    {
                        var tuple = await connManager.activeBattView.searchDebugRecordByDate(startTime.Date);
                        status = tuple.Item1;
                        status = await connManager.activeBattView.readDebugRecords(startRecord, endTime.Date.AddDays(1).AddSeconds(-1), true);
                    }
                }
            }
            catch (Exception ex)
            {
                internalFailure = true;
                internalFailureString = ex.ToString();
            }
            List<object> results = new List<object>();
            results.Add(internalFailure);
            results.Add(internalFailureString);
            results.Add(status);
            results.Add(byID);

            return results;
        }

        public void Cancel()
        {
            if (connManager == null)
            {
                return;
            }

            if (connManager.activeBattView == null)
            {
                return;
            }

            connManager.activeBattView.Cancel();
        }
    }
}
