using System;
using System.Collections.Generic;

namespace actchargers
{
    public sealed class SiteViewQuantum
    {
        ConnectionManager connManager;

        static readonly SiteViewQuantum _instance = new SiteViewQuantum();

        public static SiteViewQuantum Instance
        {
            get
            {
                return _instance;
            }
        }

        internal void SetConnectionManager(ConnectionManager ConnManager)
        {
            connManager = ConnManager;
        }

        internal ConnectionManager GetConnectionManager()
        {
            return connManager;
        }

        public List<ACTViewSite> LoadUpdate()
        {

            List<ACTViewSite> sites = new List<ACTViewSite>();
            //synch SiteView variables with managed variables
            try
            {
                connManager.SynchSiteViewEveryThing();

                sites = DbSingleton
                    .DBManagerServiceInstance
                    .GetSynchSiteObjectsLoader()
                    .GetAllAsACTViewSiteList();
            }
            catch (Exception ex)
            {
                Logger.AddLog(true, ex.ToString());
            }

            return sites;
        }
    }
}
