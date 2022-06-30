using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace actchargers
{
    public class ACTVIEWQuantum
    {
        static readonly ACTVIEWQuantum _instance = new ACTVIEWQuantum();
        public static ACTVIEWQuantum Instance
        {
            get
            {
                return _instance;
            }
        }

        public ACTVIEWQuantum()
        {
            requestCancelSynch = false;
            synchSitesIsRunning = false;
        }


        bool _requestCancelSynch;
        bool requestCancelSynch
        {
            set { _requestCancelSynch = value; }
            get { return _requestCancelSynch; }
        }

        bool _SynchSitesISRunning;
        bool synchSitesIsRunning
        {
            set { _SynchSitesISRunning = value; }
            get { return _SynchSitesISRunning; }
        }

        public async Task<List<object>> CommunicateACTView(List<object> arguments)
        {
            ACTViewResponse status = null;
            List<object> genericlist = arguments;
            List<object> result = new List<object>();
            bool internalFailure = false;
            string internalFailureString = "";
            bool userAction = (bool)genericlist[0];

            List<object> ProcArgumentList = new List<object>();
            ActviewCommGeneric caller = (ActviewCommGeneric)genericlist[1];
            try
            {
                switch (caller)
                {

                    case ActviewCommGeneric.OEM_UploadBattView:
                        status = await ACTViewConnect.UploadBattViewDeviceObjectDirect((UInt32)genericlist[2], 0, (string)genericlist[3], (string)genericlist[4], (float)genericlist[5], (byte)genericlist[6]);

                        break;
                    case ActviewCommGeneric.OEM_UploadMCB:
                        status = await ACTViewConnect.UploadMCBDeviceObjectDirect((UInt32)genericlist[2], (string)genericlist[3], (string)genericlist[4], (float)genericlist[5], (byte)genericlist[6]);

                        break;
                    case ActviewCommGeneric.downloadDevicesFromSite:
                        List<ACTViewSite> sites = (List<ACTViewSite>)genericlist[2];

                        var devices = new List<SynchObjectsBufferedData>();

                        int reached = 0;
                        int total = 2 * sites.Count;
                        int battC = 0;
                        int mcbC = 0;
                        foreach (ACTViewSite site in sites)
                        {
                            if (requestCancelSynch)
                            {
                                status.responseType = ActviewResponseType.softwareError;
                                break;
                            }


                            status = await ACTViewConnect.DownloadBattViewConfig(site.customerID, site.id);
                            if (status.responseType != ActviewResponseType.validResponse)
                            {
                                break;

                            }

                            if (status.returnValue["battviews"] == null || status.returnValue["battviews"].Type != Newtonsoft.Json.Linq.JTokenType.Array)
                            {
                                status.responseType = ActviewResponseType.invalidResponse;
                                break;

                            }

                            Newtonsoft.Json.Linq.JArray battviews = (Newtonsoft.Json.Linq.JArray)status.returnValue["battviews"];
                            foreach (Newtonsoft.Json.Linq.JObject item in battviews)
                            {
                                string config = item.ToString();
                                string serialNumber;
                                string deviceName;
                                int zone;
                                bool isMCB = false;
                                UInt32 id;
                                UInt32 siteId = site.id;
                                UInt32 customerId = site.customerID;
                                if (item["serialnumber"] == null || item["serialnumber"].Type != Newtonsoft.Json.Linq.JTokenType.String ||
                                    item["zone"] == null || item["zone"].Type != Newtonsoft.Json.Linq.JTokenType.Integer ||
                                    item["batteryid"] == null || item["batteryid"].Type != Newtonsoft.Json.Linq.JTokenType.String ||
                                    item["id"] == null || item["id"].Type != Newtonsoft.Json.Linq.JTokenType.Integer)
                                {
                                    status.responseType = ActviewResponseType.invalidResponse;
                                    break;
                                }
                                Newtonsoft.Json.Linq.JObject data = (Newtonsoft.Json.Linq.JObject)item["data"];

                                serialNumber = (string)item["serialnumber"];
                                deviceName = (string)item["batteryid"];
                                zone = (int)item["zone"];
                                string configbuffer = data["data"].ToString();
                                configbuffer = configbuffer.Replace("\r\n", "").Replace(" ", "");
                                id = (UInt32)item["id"];

                                string deviceTypeName =
                                    DeviceNameDeterminer.GetDeviceName(isMCB);
                                var deviceObj = new SynchObjectsBufferedData()
                                {
                                    Id = id,
                                    DeviceTypeName = deviceTypeName,
                                    Config = configbuffer,
                                    SerialNumber = serialNumber,
                                    DeviceName = deviceName,
                                    SiteId = siteId,
                                    CustomerId = customerId,
                                    Zone = zone
                                };

                                DbSingleton
                                    .DBManagerServiceInstance
                                    .GetSynchObjectsBufferedDataLoader()
                                    .InsertOrUpdate(deviceObj);

                                devices.Add(deviceObj);
                                battC++;
                            }
                            if (requestCancelSynch)
                            {
                                status.responseType = ActviewResponseType.softwareError;
                                break;
                            }
                            reached++;

                            //if (this.synchSites_progressBar.InvokeRequired)
                            //{
                            //    this.synchSites_progressBar.BeginInvoke((MethodInvoker)delegate () { this.synchSites_progressBar.Value = 100 * reached / total; });
                            //}
                            //else
                            //{
                            //    this.synchSites_progressBar.Value = 100 * reached / total;
                            //}

                            status = await ACTViewConnect.DownloadMCBConfig(site.customerID, site.id);
                            if (status.responseType != ActviewResponseType.validResponse)
                            {
                                break;

                            }
                            Newtonsoft.Json.Linq.JArray mcbs = (Newtonsoft.Json.Linq.JArray)status.returnValue["chargers"];
                            foreach (Newtonsoft.Json.Linq.JObject item in mcbs)
                            {
                                string config = item.ToString();
                                string serialNumber;
                                string deviceName;
                                bool isMCB = true;
                                UInt32 id;
                                int zone;
                                UInt32 siteId = site.id;
                                UInt32 customerId = site.customerID;

                                if (item["serialnumber"] == null || item["serialnumber"].Type != Newtonsoft.Json.Linq.JTokenType.String ||
                                    item["zone"] == null || item["zone"].Type != Newtonsoft.Json.Linq.JTokenType.Integer ||
                                    item["chargerusername"] == null || item["chargerusername"].Type != Newtonsoft.Json.Linq.JTokenType.String ||
                                    item["id"] == null || item["id"].Type != Newtonsoft.Json.Linq.JTokenType.Integer ||
                                    item["data"] == null || item["data"].Type != Newtonsoft.Json.Linq.JTokenType.Object)
                                {
                                    status.responseType = ActviewResponseType.invalidResponse;
                                    break;
                                }
                                Newtonsoft.Json.Linq.JObject data = (Newtonsoft.Json.Linq.JObject)item["data"];


                                serialNumber = (string)item["serialnumber"];
                                deviceName = (string)item["chargerusername"];
                                string configbuffer = data["data"].ToString();
                                configbuffer = configbuffer.Replace("\r\n", "").Replace(" ", "");
                                id = (UInt32)item["id"];
                                zone = (int)item["zone"];

                                string deviceTypeName =
                                    DeviceNameDeterminer.GetDeviceName(isMCB);
                                var deviceObj = new SynchObjectsBufferedData()
                                {
                                    Id = id,
                                    DeviceTypeName = deviceTypeName,
                                    Config = configbuffer,
                                    SerialNumber = serialNumber,
                                    DeviceName = deviceName,
                                    SiteId = siteId,
                                    CustomerId = customerId,
                                    Zone = zone
                                };

                                DbSingleton
                                    .DBManagerServiceInstance
                                    .GetSynchObjectsBufferedDataLoader()
                                    .InsertOrUpdate(deviceObj);

                                devices.Add(deviceObj);
                                mcbC++;
                            }

                            reached++;

                            //if (this.synchSites_progressBar.InvokeRequired)
                            //{
                            //    this.synchSites_progressBar.BeginInvoke((MethodInvoker)delegate () { this.synchSites_progressBar.Value = 100 * reached / total; });
                            //}
                            //else
                            //{
                            //    this.synchSites_progressBar.Value = 100 * reached / total;
                            //}


                        }
                        ProcArgumentList.Add(devices);
                        ProcArgumentList.Add(sites);
                        ProcArgumentList.Add(battC);
                        ProcArgumentList.Add(mcbC);
                        if (status.responseType == ActviewResponseType.validResponse)
                        {
                            DeleteAllSynchedSites();

                            foreach (SynchObjectsBufferedData item in devices)
                            {
                                DbSingleton
                                    .DBManagerServiceInstance
                                    .GetSynchObjectsBufferedDataLoader()
                                    .InsertOrUpdate(item);
                            }

                            foreach (ACTViewSite s in sites)
                            {
                                DbSingleton.DBManagerServiceInstance
                                           .GetSynchSiteObjectsLoader()
                                           .AddUsingFields
                                           (s.name, s.customerName, s.id, s.customerID);
                            }
                        }


                        break;


                    case ActviewCommGeneric.loadUserCustomersAndSites:

                        UInt32 startFrom = 0;//for pagination

                        Dictionary<UInt32, ACTViewCustomer> newSites = new Dictionary<uint, ACTViewCustomer>();

                        int resetCount = 0;
                        int loopedCount = 0;
                        while (true)
                        {
                            if (requestCancelSynch)
                            {
                                status.responseType = ActviewResponseType.softwareError;
                                break;
                            }
                            if (startFrom == 0)
                            {
                                resetCount++;
                                newSites = new Dictionary<uint, ACTViewCustomer>();
                            }
                            loopedCount++;

                            if (resetCount > 3)
                            {
                                status.responseType = ActviewResponseType.softwareError;
                                break;
                            }
                            if (loopedCount > 250)
                            {
                                status.responseType = ActviewResponseType.softwareError;
                                break;
                            }
                            status = await ACTViewConnect.GetUserSites(startFrom);
                            startFrom++;

                            if (status.responseType != ActviewResponseType.validResponse)
                            {
                                break;
                            }
                            //check if we have customers value & nextValue
                            if (status.returnValue["next"] == null || status.returnValue["next"].Type != Newtonsoft.Json.Linq.JTokenType.Boolean ||
                                status.returnValue["customers"] == null || status.returnValue["customers"].Type != Newtonsoft.Json.Linq.JTokenType.Array)
                            {
                                status.responseType = ActviewResponseType.invalidResponse;
                                break;

                            }

                            Newtonsoft.Json.Linq.JArray customersList = (Newtonsoft.Json.Linq.JArray)status.returnValue["customers"];
                            foreach (Newtonsoft.Json.Linq.JObject item in customersList)
                            {
                                ACTViewCustomer cusObj = new ACTViewCustomer((string)item["name"], (UInt32)item["id"]);
                                Newtonsoft.Json.Linq.JArray sitesList = (Newtonsoft.Json.Linq.JArray)item["sites"];
                                foreach (Newtonsoft.Json.Linq.JObject siteItem in sitesList)
                                {
                                    ACTViewSite o = new ACTViewSite((string)siteItem["name"], (UInt32)siteItem["id"], cusObj.id, cusObj.name);
                                    cusObj.addSite(o);
                                }

                                if (newSites.ContainsKey(cusObj.id))
                                {
                                    startFrom = 0;
                                    break;
                                }
                                newSites.Add(cusObj.id, cusObj);
                            }

                            bool next = (bool)status.returnValue["next"];
                            if (!next)
                                break;
                        }

                        ProcArgumentList.Add(newSites);



                        break;
                    case ActviewCommGeneric.commisionMCB:
                        {
                            ProcArgumentList.Add(genericlist[2]);
                            ProcArgumentList.Add(genericlist[3]);
                            status = await ACTViewConnect.CommissionMCB((string)genericlist[2], (bool)genericlist[3]);
                        }
                        break;
                    case ActviewCommGeneric.commisionBattView:
                        ProcArgumentList.Add(genericlist[2]);
                        ProcArgumentList.Add(genericlist[3]);
                        status = await ACTViewConnect.CommissionBattView((string)genericlist[2], (bool)genericlist[3]);
                        break;
                    case ActviewCommGeneric.readVersions:
                        {
                            status = await ACTViewConnect.GetQuantumVersions();
                        }
                        break;
                    case ActviewCommGeneric.checkLogin:
                        {
                            status = await ACTViewConnect.UserExist(ControlObject.userID);
                        }
                        break;
                    case ActviewCommGeneric.downladbattHex:
                        {
                            //status = await ACTViewConnect.downloadQuantumResource("battview");
                            if (status.responseType == ActviewResponseType.validResponse)
                            {
                                //if (System.IO.File.Exists(status.responseString))
                                //{
                                //    string res = System.IO.File.ReadAllText(status.responseString);
                                //    DB_ACCESS.DB_HelperFunctions.FIRMWARE_insertUpdateFirmwareVersion(false, actView_batt_version, res);
                                //    System.IO.File.Delete(status.responseString);
                                //}
                            }
                        }
                        break;
                    case ActviewCommGeneric.downloadMCBHexFiles:
                        {
                            //status = await ACTViewConnect.downloadQuantumResource("MCB");
                            if (status.responseType == ActviewResponseType.validResponse)
                            {
                                //if (System.IO.File.Exists(status.responseString))
                                //{
                                //    string res = System.IO.File.ReadAllText(status.responseString);
                                //    DB_ACCESS.DB_HelperFunctions.FIRMWARE_insertUpdateFirmwareVersion(true, actView_mcb_version, res);
                                //    System.IO.File.Delete(status.responseString);
                                //}
                            }
                        }
                        break;
                    case ActviewCommGeneric.downloadQuantum:
                        {
                            //status = await ACTViewConnect.downloadQuantumResource("software");
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
            result.Add(userAction);
            result.Add(ProcArgumentList);
            return result;

        }

        void DeleteAllSynchedSites()
        {
            DbSingleton.DBManagerServiceInstance
                       .GetSynchObjectsBufferedDataLoader()
                       .DeleteAll();
            DbSingleton.DBManagerServiceInstance
                       .GetSynchSiteObjectsLoader()
                       .DeleteAll();
        }
    }
}
