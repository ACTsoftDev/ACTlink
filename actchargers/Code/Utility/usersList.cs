using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;

namespace actchargers
{
    class OEM
    {
        public string name { get; set; }
        public UInt32 id { get; set; }
        public override string ToString()
        {
            return name;
        }
        public OEM(UInt32 id, string name)
        {
            this.id = id;
            this.name = name;
        }
    }
    class Dealer
    {
        public string name { get; set; }
        public UInt32 id { get; set; }
        public override string ToString()
        {
            return name;
        }
        public Dealer(UInt32 id, string name)
        {
            this.id = id;
            this.name = name;
        }

    }
    class Customer
    {
        public string name { get; set; }
        public UInt32 id { get; set; }
        public List<Site> mySites;
        public bool sitesLoaded;
        public override string ToString()
        {
            return name;
        }
        public Customer(UInt32 id, string name)
        {
            this.id = id;
            this.name = name;
            mySites = new List<Site>();
            sitesLoaded = false;
        }

    }
    class Site
    {
        public string name { get; set; }
        public UInt32 id { get; set; }
        public UInt32 countryId;
        public UInt32 stateId;
        double latitude;
        double longitude;
        string address;
        public override string ToString()
        {
            return name;
        }
        public Site(UInt32 id, string name)
        {
            this.id = id;
            this.name = name;
        }
    }
    class usersList 
    {
        private static List<OEM> oems = new List<OEM>();
        private static List<Dealer> dealers = new List<Dealer>();
        private static List<Customer> customers = new List<Customer>();
        private static bool oemsAndDealersLoaded = false;
        private static bool customersLoaded = false;
        public static bool AreOemsAndDealersLoaded() {
            return oemsAndDealersLoaded;
        }
        public static bool AreCustomersLoaded()
        {
            return customersLoaded;
        }
        private static async Task loadOEMSAndDealers()
        {
            try
            {
                oemsAndDealersLoaded = false;
                ACTViewResponse res = await ACTViewConnect.GetUserList(0, 0);
                //ACTViewResponse res = (ACTViewConnect.getUserList(0, 0)).Result;
                if (res.responseType == ActviewResponseType.validResponse &&
                    res.returnValue["oems"].Type == Newtonsoft.Json.Linq.JTokenType.Array &&
                     res.returnValue["delaers"].Type == Newtonsoft.Json.Linq.JTokenType.Array
                    )
                {
                    oems = new List<OEM>();
                    Newtonsoft.Json.Linq.JArray oemsList = (Newtonsoft.Json.Linq.JArray)res.returnValue["oems"];
                    foreach (Newtonsoft.Json.Linq.JObject item in oemsList)
                    {
                        OEM o = new OEM((UInt32)item["id"], (string)item["name"]);
                        oems.Add(o);
                    }
                    dealers = new List<Dealer>();
                    Newtonsoft.Json.Linq.JArray dealersList = (Newtonsoft.Json.Linq.JArray)res.returnValue["delaers"];
                    foreach (Newtonsoft.Json.Linq.JObject item in dealersList)
                    {
                        Dealer o = new Dealer((UInt32)item["id"], (string)item["name"]);
                        dealers.Add(o);
                    }
                    if (dealers.Count > 0 && oems.Count > 0)
                    {
                        oemsAndDealersLoaded = true;
                    }

                }
            }
            catch (Exception ex)
            {
                Logger.AddLog(true, "X121" + ex.ToString());
            }
        }
        public async static  Task<List<OEM>> getAllOEMs(bool forceRefresh) {
            if (!oemsAndDealersLoaded || forceRefresh)
            {
                await loadOEMSAndDealers();
            }

            List<OEM> temp = new List<OEM>();
            foreach (OEM d in oems)
            {
                OEM Dcopy = new OEM(d.id, d.name);
                temp.Add(Dcopy);
            }
            return temp;
        }
        public static async Task<List<Dealer>> getAllDealers(bool forceRefresh)
        {
            if (!oemsAndDealersLoaded || forceRefresh)
            {
                await loadOEMSAndDealers();
            }
            List<Dealer> temp = new List<Dealer>();
            foreach (Dealer d in dealers)
            {
                Dealer Dcopy = new Dealer(d.id, d.name);
                temp.Add(Dcopy);
            }
            return temp;
        }
        public static async Task<List<Customer>> getAllCustomers(bool forceRefresh)
        {
            if (!customersLoaded || forceRefresh)
            {
                customersLoaded = false;
                try
                {
                    ACTViewResponse res = await ACTViewConnect.GetUserList(1, 0);
                    if (res.responseType == ActviewResponseType.validResponse &&
                        res.returnValue["customers"].Type == Newtonsoft.Json.Linq.JTokenType.Array
                        )
                    {
                        Newtonsoft.Json.Linq.JArray customersList = (Newtonsoft.Json.Linq.JArray)res.returnValue["customers"];
                        customers = new List<Customer>();
                        foreach (Newtonsoft.Json.Linq.JObject item in customersList)
                        {
                            Customer o = new Customer((UInt32)item["id"], (string)item["name"]);
                            customers.Add(o);
                        }
                        if (customers.Count > 0)
                            customersLoaded = true;
                    }
                }
                catch (Exception ex)
                {
                    Logger.AddLog(true, "X122" + ex.ToString());
                }
            }
            List<Customer> temp = new List<Customer>();
            foreach (Customer d in customers)
            {
                Customer Dcopy = new Customer(d.id, d.name);
                temp.Add(Dcopy);
            }
            return temp;
        }

        public static bool isThisCustomerSitesLoaded(UInt32 customerID)
        {
            if (customerID == 0 || !customers.Exists(x => x.id == customerID))
            {
                return false;
            }
            Customer myCustomer = customers.Find(x => x.id == customerID);
            if (!myCustomer.sitesLoaded)
                return false;
            return true;
        }
        public static async Task<List<Site>> getCustomerSites(UInt32 customerID, bool forceRefresh)
        {

            if (customerID == 0 || !customers.Exists(x => x.id == customerID))
            {
                return new List<Site>();
            }
            Customer myCustomer = customers.Find(x => x.id == customerID);

            if (!myCustomer.sitesLoaded || forceRefresh)
            {
                myCustomer.sitesLoaded = false;
                try
                {
                    ACTViewResponse res = await ACTViewConnect.GetUserList(2, myCustomer.id);
                    //ACTViewResponse res = (ACTViewConnect.getUserList(2, myCustomer.id)).Result;
                    if (res.responseType == ActviewResponseType.validResponse &&
                        res.returnValue["sites"].Type == Newtonsoft.Json.Linq.JTokenType.Array)
                    {
                        Newtonsoft.Json.Linq.JArray sitesList = (Newtonsoft.Json.Linq.JArray)res.returnValue["sites"];
                        myCustomer.mySites = new List<Site>();
                        foreach (Newtonsoft.Json.Linq.JObject item in sitesList)
                        {
                            Site o = new Site((UInt32)item["id"], (string)item["name"]);
                            myCustomer.mySites.Add(o);
                        }
                        myCustomer.sitesLoaded = true;
                    }
                }
                catch (Exception ex)
                {
                    Logger.AddLog(true, "X123" + ex);
                }
            }
            List<Site> temp = new List<Site>();
            foreach (Site d in myCustomer.mySites)
            {
                Site Dcopy = new Site(d.id, d.name);
                temp.Add(Dcopy);
            }
            return temp;
        }

        //public static  Dictionary
    }
}