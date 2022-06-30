using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using actchargers.Code.Utility;

namespace actchargers
{
    public class MCBQuantum
    {
        ConnectionManager connManager;
        static readonly MCBQuantum _instance = new MCBQuantum();

        public static MCBQuantum Instance
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

        internal bool MCB_quickSerialNumberCheckStrict()
        {
            return (connManager.getWorkingSerialNumber().StartsWith("CHRG_", StringComparison.Ordinal) && connManager.activeMCB.deviceIsLoaded);
        }

        internal ConnectionManager GetConnectionManager()
        {
            return connManager;
        }

        internal MCBobject GetMCB()
        {
            if (connManager == null)
            {
                return null;
            }
            else
            {
                return connManager.activeMCB;
            }

        }

        internal void Clear()
        {
            connManager = null;
        }



        public async Task<List<object>> CommunicateMCB(List<object> arguments)         {             CommunicationResult status = CommunicationResult.NOT_EXIST;             List<object> genericlist = arguments;             List<object> result = new List<object>();             bool internalFailure = false;             string internalFailureString = "";             McbCommunicationTypes caller = (McbCommunicationTypes)genericlist[0];             object arg1 = (object)genericlist[1];             //bool showBusy = (bool)genericlist[2];
            bool showBusy = false;             List<object> extraObjectList = new List<object>();              try             {                 switch (caller)                 {                     case McbCommunicationTypes.replaceDevice:                         {                             //get JSON object                             var d =
                                DbSingleton.DBManagerServiceInstance
										   .GetSynchObjectsBufferedDataLoader()
										   .GetSynchedDevices
                                           (true, (UInt32)arg1);
                                                         //format                              string configbuff = d[0].Config.Remove(0, 1);                             configbuff = configbuff.Remove(configbuff.Length - 1, 1);                             string[] bytes = configbuff.Split(new char[] { ',' });                             byte[] toSend = new byte[512];                             int x = 0;                             foreach (string sub in bytes)                             {                                 toSend[x++] = byte.Parse(sub);                             }                              connManager.activeMCB.myZone = (byte)d[0].Zone;
                           
                            status = await Task.Run(async () =>
                            {
                                return await connManager.activeMCB.SaveTime();
                            });                              if (status == CommunicationResult.OK)                             {
                                MCBConfig conf =  connManager.activeMCB.getCopyOfArrayForReplacment(toSend);

                                status = await Task.Run(async () =>
                                {                                     return await connManager.activeMCB.setConfigFromConfig(conf);
                                });                                  if (status == CommunicationResult.OK)                                 {                                     ReplaceAndUpdate
									(conf, connManager.activeMCB.globalRecord,
										 (UInt32)arg1,
										 connManager.activeMCB.Config.serialNumber);                                    
                                     status = await Task.Run(async () =>
                                    {
                                        return await connManager.activeMCB.RecyclePower();
                                    });                                      caller = McbCommunicationTypes.restartDevice;                                     connManager.ForceSoftDisconnectDevice(connManager.getWorkingSerialNumber(), true);                                 }                              }                          }                         break;                     case McbCommunicationTypes.factoryReset:
                        status = await Task.Run(async () =>
                        {
                            return await connManager.activeMCB.SaveConfigToDevice();
                        }); 
                        if (status == CommunicationResult.OK)
                        {
                            status = await Task.Run(async () =>
                            {
                                return await connManager.activeMCB.ResetGlobalRecord();
                            }); 
                            if (status == CommunicationResult.OK)
                            {
                                status = await Task.Run(async () =>
                                {                                     return await connManager.activeMCB.SaveTime();
                                }); 
                                if (status == CommunicationResult.OK)
                                {
                                    status = await Task.Run(async () =>
                                    {
                                        return await connManager.activeMCB.setActviewID();
                                    }); 
                                    if (status == CommunicationResult.OK)
                                    {
                                        status = await Task.Run(async () =>
                                        {
                                            return await connManager.activeMCB.RecyclePower();
                                        });                                         caller = McbCommunicationTypes.restartDevice;
                                        connManager.ForceSoftDisconnectDevice(connManager.getWorkingSerialNumber(), true);

                                    }
                                }
                            }
                        }                         break;                     case McbCommunicationTypes.lcdReq:
                        status = await Task.Run(async () =>
                        {
                            return await connManager.activeMCB.setMCB_LCD(((LcdRequest)arg1));
                        });                         break;                     case McbCommunicationTypes.lcdView:
                        status = await Task.Run(async () =>
                        {
                            return await connManager.activeMCB.ReadMCB_LCD();
                        });                          break;                     case McbCommunicationTypes.calibrator_StateRead:                     case McbCommunicationTypes.calibrator_voltageCalGet:                         if (caller == McbCommunicationTypes.calibrator_voltageCalGet)                             extraObjectList.Add((int)arg1);
                        status = await Task.Run(async () =>
                        {
                            return await connManager.activeMCB.ReadCalibratorState();
                        });                          break;                      case McbCommunicationTypes.calibrator_do_BVreadCommand:                         {                             List<object> vars = arg1 as List<object>;                             byte resState = 0;
                            var tuple = await Task.Run(async () =>
                            {
                                return await connManager.activeMCB.doReadCalibratorBV(resState, (float)vars[0], (byte)vars[1]);
                            });                             status = tuple.Item1;                             resState = tuple.Item2;                             extraObjectList.Add((int)resState);                         }                          break;                     case McbCommunicationTypes.calibratorDebug_Set:                     case McbCommunicationTypes.calibratorDebug_Set2:                         {                             List<object> varsxx = arg1 as List<object>;
                            status = await Task.Run(async () =>
                            {
                                return await connManager.activeMCB.setCalibratorCurrent((float)varsxx[0], (byte)varsxx[1], (byte)varsxx[2], (bool)varsxx[3]);
                            });                         }                         break;                     case McbCommunicationTypes.calibrator_SAVEBV_Quick:                         {                             List<object> vars3x = arg1 as List<object>;                             byte resState3x = 0;
                            var tuple = await Task.Run(async () =>
                            {
                                return await connManager.activeMCB.doCalibratorWriteBV(resState3x);
                            });                             status = tuple.Item1;                             resState3x = tuple.Item2;                             extraObjectList.Add((int)resState3x);                         }                         break;                     case McbCommunicationTypes.calibrator_do_RTC_WRITE:                         {                              List<object> vars3x = arg1 as List<object>;                             byte resState3x = 0;
                            var tuple = await Task.Run(async () =>                             {                                 return await connManager.activeMCB.doCalibratorRTCWrite(resState3x);                             });
                             status = tuple.Item1;                             resState3x = tuple.Item2;                             extraObjectList.Add((int)resState3x);                          }                         break;                     case McbCommunicationTypes.calibratorCalibration_Set:                         {                             List<object> vars2x = arg1 as List<object>;
                            status = await Task.Run(async () =>                             {                                 return await connManager.activeMCB.setCalibratorCurrent((float)vars2x[0], (byte)vars2x[1], (byte)vars2x[2], (bool)vars2x[3]);                             });
                             if (status == CommunicationResult.OK && ((byte)vars2x[1] == 1 || (byte)vars2x[1] == 2))                             {                                 await Task.Delay(5000);                                 extraObjectList.Add((int)vars2x[4]);
                             status = await Task.Run(async () =>
                             {
                                 return await connManager.activeMCB.ReadCalibratorState();
                             });
                                 if (status != CommunicationResult.OK)                                 {                                     await Task.Delay(1250);
                                    status = await Task.Run(async () =>
                                {
                                return await connManager.activeMCB.ReadCalibratorState();
                                });
                                 }
                                var isSetCalibrator = await Task.Run(async () =>
                                {                                     return await connManager.activeMCB.setCalibratorCurrent(0, 3, 0, true);
                                });
                                 if (isSetCalibrator != CommunicationResult.OK)                                 {                                     await Task.Delay(500);
                                    await Task.Run(async () =>
                                    {
                                        return await connManager.activeMCB.setCalibratorCurrent(0, 3, 0, true);
                                    });
                                 }                              }                          }                         break;                      case McbCommunicationTypes.healthCheck:                         {
                            status = await Task.Run(async () =>
                            {
                                return await connManager.activeMCB.ReadMCB_Health();
                            });
                         }                         break;                     case McbCommunicationTypes.saveConfigAndTime:
                        status = await Task.Run(async () =>
                        {
                            return await connManager.activeMCB.SaveConfigToDevice();
                        });
                         if (status == CommunicationResult.OK)                         {
                            status = await Task.Run(async () =>
                            {
                                return await connManager.activeMCB.SaveTime();
                            });
                             if ((int)arg1 == 2)                             {                                 connManager.ForceSoftDisconnectDevice(connManager.getWorkingSerialNumber());                              }                             extraObjectList.Add((int)arg1);                         }                         else                             extraObjectList.Add(0);                         if (status == CommunicationResult.OK)                         {                             connManager.siteView.setDeviceConfigurationSaved(connManager.getWorkingSerialNumber(), false);                         }                         break;                       case McbCommunicationTypes.saveConfig:
                        status = await Task.Run(async () =>
                        {
                            return await connManager.activeMCB.SaveConfigToDevice();
                        });
                        if (status == CommunicationResult.OK)                         {                             connManager.siteView.setDeviceConfigurationSaved(connManager.getWorkingSerialNumber(), false);                         }                         break;                     case McbCommunicationTypes.calibrator_calibrationSaveConfig:                         extraObjectList.Add((bool)arg1);
                        status = await Task.Run(async () =>
                        {
                            return await connManager.activeMCB.SaveConfigToDevice();
                        });                          break;                     case McbCommunicationTypes.calibrator_issue_BVreadCommand:                         {
                            byte resStatex = 0;
                            var tuple = await Task.Run(async () =>
                            {
                                return await connManager.activeMCB.IssueReadCalibratorBV(resStatex);
                            });
                            status = tuple.Item1;
                            resStatex = tuple.Item2;                         extraObjectList.Add((int)resStatex);
                        }                         break;                      case McbCommunicationTypes.calibrator_InformationSaveConfig:
                        status = await Task.Run(async () =>
                        {
                            return await connManager.activeMCB.SaveConfigToDevice();
                        });
                         if (status == CommunicationResult.OK)                         {                             if ((int)arg1 == 2)                             {                                 connManager.ForceSoftDisconnectDevice(connManager.getWorkingSerialNumber(), true);                              }                             extraObjectList.Add((int)arg1);                         }                         else                             extraObjectList.Add(0);                         break;                     case McbCommunicationTypes.saveConfigAndRestart:                     case McbCommunicationTypes.saveConfigCommission:
                        status = await Task.Run(async () =>
                        {
                            return await connManager.activeMCB.SaveConfigToDevice();
                        });
                         if (caller == McbCommunicationTypes.saveConfigAndRestart &&                             status == CommunicationResult.OK)                         {
                            status = await Task.Run(async () =>
                            {
                                return await connManager.activeMCB.RecyclePower();
                            });
                             if (status == CommunicationResult.OK)                             {                                 caller = McbCommunicationTypes.restartDevice;//                                 connManager.ForceSoftDisconnectDevice(connManager.getWorkingSerialNumber(), arg1 != null && (bool)arg1);                             }                             if (status == CommunicationResult.OK)
                            {
                                connManager.siteView.setDeviceConfigurationSaved(connManager.getWorkingSerialNumber(), false);
                            }                         }                         if (caller == McbCommunicationTypes.saveConfigCommission &&                             status == CommunicationResult.OK)                         {
                            status = await Task.Run(async () =>
                            {
                                return await connManager.activeMCB.SaveTime();
                            });

                           bool isdoLoad = await Task.Run(async () =>
                            {
                                return await connManager.activeMCB.DoLoad();
                            });
                             if (isdoLoad)
                            {
                                connManager.siteView.setDeviceConfigurationRead(connManager.getWorkingSerialNumber(), true, connManager.activeMCB.DcId, connManager.activeMCB.FirmwareRevision, connManager.activeMCB.FirmwareWiFiVersion, connManager.activeMCB.FirmwareDcVersion);
                            }                          }                         break;                     case McbCommunicationTypes.MCB_TemperaturecalibrationSaveConfig:                     case McbCommunicationTypes.MCB_voltagecalibrationSaveConfig:                         status = await Task.Run(async () =>
                        {
                            return await connManager.activeMCB.SaveConfigToDevice();
                        });
                          if (status == CommunicationResult.OK)                         {                             connManager.siteView.setDeviceConfigurationSaved(connManager.getWorkingSerialNumber(), false);                         }                         break;                     case McbCommunicationTypes.doFinalComission:                         List<object> varsx = arg1 as List<object>;                         connManager.activeMCB.Config.id = ((UInt32)varsx[0]).ToString();                         connManager.activeMCB.Config.afterCommissionBoardID = (UInt32)varsx[1];
                        status = await Task.Run(async () =>
                        {
                            return await connManager.activeMCB.SaveConfigToDevice();
                        });
                         if (status == CommunicationResult.OK)                         {
                             await Task.Run(async () =>
                            {
                                return await connManager.activeMCB.ResetGlobalRecord();
                            });
                            status = await Task.Run(async () =>
                            {
                                return await connManager.activeMCB.setActviewID();
                            });                             if (status == CommunicationResult.OK)                             {
                                status = await Task.Run(async () =>
                                {                                     return await connManager.activeMCB.RecyclePower();
                                });
                                 if (status == CommunicationResult.OK)                                 {                                     connManager.ForceSoftDisconnectDevice(connManager.getWorkingSerialNumber(), true);                                 }                             }                         }                         break;                     case McbCommunicationTypes.resetGlobalRecords:                         status =await connManager.activeMCB.ResetGlobalRecord();                         if (status == CommunicationResult.OK)                         {                             status =await connManager.activeMCB.RecyclePower();                             if (status == CommunicationResult.OK)                             {                                 connManager.ForceSoftDisconnectDevice(connManager.getWorkingSerialNumber(), false);                             }                         }                          break;                      case McbCommunicationTypes.connectcomamnd:                     case McbCommunicationTypes.readAll:
                       bool isLoadSuccess = await Task.Run(async () =>
                        {
                            return await connManager.activeMCB.DoLoad();
                        });
                        if (isLoadSuccess)
                        {
                            connManager.siteView.setDeviceConfigurationRead(connManager.getWorkingSerialNumber(), true, connManager.activeMCB.DcId, connManager.activeMCB.FirmwareRevision, connManager.activeMCB.FirmwareWiFiVersion, connManager.activeMCB.FirmwareDcVersion);
                        }
                                                status = connManager.activeMCB.getDoLoadStatus();                         break;                     case McbCommunicationTypes.firmwareUpdateRequest:
                        status = await Task.Run(async () =>
                       {
                            return await connManager.activeMCB.RequestBootLoaderUpdate(false);
                       });
                         if (status == CommunicationResult.OK)                         {                             connManager.ForceSoftDisconnectDevice(connManager.getWorkingSerialNumber(), false);                         }                         break;                     case McbCommunicationTypes.firmwareWrite:                         byte[] serials = (byte[])arg1;                         DateTime start = DateTime.UtcNow;
                        status = await Task.Run(async () =>
                        {
                            return await connManager.activeMCB.WriteToBootLoaderFlash(serials, serials.Length);//no config updates
                        });
                         if (status == CommunicationResult.OK)                         {                             await Task.Delay(500);
                            status = await Task.Run(async () =>
                            {
                                return await connManager.activeMCB.RequestBootLoaderUpdate(false);
                            });
                             if (status == CommunicationResult.OK || connManager.activeMCB.FirmwareRevision < 2.05f)                             {                                 connManager.ForceSoftDisconnectDevice(connManager.getWorkingSerialNumber());                             }                              caller = McbCommunicationTypes.firmwareUpdateRequest;//                         }                         Logger.AddLog(false, "MCB update took:" + (DateTime.UtcNow - start).TotalSeconds.ToString());                          break;                     case McbCommunicationTypes.loadPLC:                         byte[] plc_serials = (byte[])arg1;
                        status = await Task.Run(async () =>
                        {
                            return await connManager.activeMCB.writetoPLCFlash(plc_serials, plc_serials.Length);
                        });
                         if (status == CommunicationResult.OK)                         {
                            status = await Task.Run(async () =>
                            {
                                return await connManager.activeMCB.requestPLCUpdate();
                            });
                         }                         if (status == CommunicationResult.OK)                         {
                            status = await Task.Run(async () =>
                            {
                                return await connManager.activeMCB.RecyclePower();
                            });
                             if (status == CommunicationResult.OK)                             {                                 caller = McbCommunicationTypes.restartDevice;                                 connManager.ForceSoftDisconnectDevice(connManager.getWorkingSerialNumber(), false);                              }                         }                         break;                     case McbCommunicationTypes.restartDevice:
                        status = await Task.Run(async () =>
                        {
                            return await connManager.activeMCB.RecyclePower();
                        });
                         if (status == CommunicationResult.OK)                         {                             connManager.ForceSoftDisconnectDevice(connManager.getWorkingSerialNumber(), false);                         }                         break;                     case McbCommunicationTypes.ResetLCDCalibration:
                        status = await Task.Run(async () =>
                        {
                            return await connManager.activeMCB.RessetLCDCal();
                        });
                         if (status == CommunicationResult.OK)                         {
                            status = await Task.Run(async () =>
                            {
                                return await connManager.activeMCB.RecyclePower();
                            });                             if (status == CommunicationResult.OK)                             {                                 caller = McbCommunicationTypes.restartDevice;//                                 connManager.ForceSoftDisconnectDevice(connManager.getWorkingSerialNumber(), false);                             }                          }                         break;                     case McbCommunicationTypes.saveActViewIDandRestart:                          connManager.activeMCB.Config.replacmentPart = false;
                        status = await Task.Run(async () =>
                        {
                            return await connManager.activeMCB.SaveConfigToDevice();
                        });
                         if (status == CommunicationResult.OK)                         {
                            status = await Task.Run(async () =>
                            {
                                return await connManager.activeMCB.setActviewID();
                            });
                             if (status == CommunicationResult.OK)                             {
                                status = await Task.Run(async () =>
                                {                                     return await connManager.activeMCB.RecyclePower();
                                });
                                 caller = McbCommunicationTypes.restartDevice;//                                 connManager.ForceSoftDisconnectDevice(connManager.getWorkingSerialNumber(), true);                              }                          }                         break;                     case McbCommunicationTypes.switchMode:                         object[] modeSwitchargs = (object[])arg1;
                        status = await Task.Run(async () =>
                        {
                            return await connManager.activeMCB.debugSwitchMode((byte)modeSwitchargs[0], (UInt32)modeSwitchargs[1]);
                        });                         //if (status == commProtocol.Communication_Result.OK)                         //{                         //    Task.Delay(1000);                         //    status = connManager.activeMCB.readChargeState();                          //}                         break;                  }             }             catch (Exception ex)             {                 internalFailure = true;                 internalFailureString = ex.ToString();             }             result.Add(internalFailure);             result.Add(internalFailureString);             result.Add(status);             result.Add(caller);             result.Add(showBusy);             result.Add(extraObjectList); 
            return result;          }

        void ReplaceAndUpdate
        (MCBConfig conf, GlobalRecord globalRecord, UInt32 newDeviceID, 
         string originalDeviceSN)
        {
            UInt32 originalDeviceID = UInt32.Parse(conf.id);
            string newDeviceSN = conf.serialNumber;

            bool isMcb = true;
            UInt32 id = UInt32.Parse(conf.id);
            string configJson = JsonParser.SerializeObject(conf);
            string globalRecordJson = globalRecord.TOJSON();
            int memorySignature =
                UInt16.Parse(conf.memorySignature);
            UInt32 eventsCount = globalRecord.chargeCycles;
            float firmwareVersion = conf.firmwareVersion;
            byte zone = conf.zoneID;
            UInt32 battviewStudyID = 0;

            DbSingleton.DBManagerServiceInstance.GetReplaceDevicesLoaders()
                       .InsertOrUpdateMcbUsingFielfds
                       (originalDeviceID, originalDeviceSN, newDeviceID, newDeviceSN);

            DbSingleton.DBManagerServiceInstance
                       .GetDevicesObjectsLoader()
                       .InsertOrUpdateDevice
                       (isMcb, id, configJson, globalRecordJson,
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

        public async Task<List<object>> MCB_CalibrationCommunication_Click_doWork(List<object> arguments)
		{
			CommunicationResult status = CommunicationResult.NOT_EXIST;
			ushort temp = 0;
			UInt16 ADCResult=0;
			List<object> genericlist = arguments as List<object>;
			List<object> result = new List<object>();
			bool internalFailure = false;
			string internalFailureString = "";
			bool isTemperatureCalibration = (bool)genericlist[0];
			object arg1 = genericlist[1];
			try
			{
				
				if (isTemperatureCalibration)
				{
					var tuple=await connManager.activeMCB.ReadTempADC(ADCResult);
					status = tuple.Item1;
					temp = tuple.Item2;
				}
				else
				{
					var tuple= await connManager.activeMCB.ReadADC(ADCResult, (bool)arg1);
					status = tuple.Item1;
					temp = tuple.Item2;

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
			result.Add(isTemperatureCalibration);
			result.Add(temp);
			result.Add(arg1);
			return result;
		}

        public void MCB_loadDefaultWIFI()
        {
            if (!Instance.MCB_quickSerialNumberCheckStrict())
                return;

            MCBobject activeMCB = Instance.GetMCB();
            activeMCB.Config.mobileAccessPassword = "shlonak5al";
            activeMCB.Config.mobileAccessSSID = "act24mobile";
            activeMCB.Config.mobilePort = "50000";
            activeMCB.Config.softAPenabled = false;
            activeMCB.Config.softAPpassword = "actDirmank";
            activeMCB.Config.actViewEnable = false;
            activeMCB.Config.actViewIP = "act-view.com";
            activeMCB.Config.actViewPort = "9309";
            activeMCB.Config.actViewConnectFrequency = "60";
            activeMCB.Config.actAccessPassword = "hala3ami102";
            activeMCB.Config.actAccessSSID = "actAccess24";
        }

        public void MCB_saveDefaultChargeProfile()
        {
            if (!MCB_quickSerialNumberCheckStrict())
                return;

            GetMCB().SaveDefaultChargeProfile();
        }

        public void Cancel()
        {
            if(connManager == null)
            {
                return;
            }

            if (connManager.activeMCB == null)
            {
                return;
            }

            connManager.activeMCB.Cancel();
        }
    }
}
