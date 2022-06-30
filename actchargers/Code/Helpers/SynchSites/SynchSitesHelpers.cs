using System;

namespace actchargers.Code.Helpers.SynchSites
{
    public static class SynchSitesHelpers
    {
        public static bool CanDoSynchSites()
        {
            int uploadsStatus = GetUploadsStatus();

            return (uploadsStatus != 1);
        }

        static int GetUploadsStatus()
        {
            bool replaces =
                DbSingleton.DBManagerServiceInstance
                           .GetReplaceDevicesLoaders()
                           .DoWeHaveUploads();
            if (replaces)
                return 1;

            bool devices =
                DbSingleton.DBManagerServiceInstance
                           .GetDevicesObjectsLoader()
                           .DoWeHaveUploads();
            if (devices)
                return 2;

            bool pMFaults =
                DbSingleton.DBManagerServiceInstance
                           .GetPMFaultsLoader()
                           .DoWeHaveUploads();
            if (pMFaults)
                return 3;

            bool battview =
                DbSingleton.DBManagerServiceInstance
                           .GetBattviewEventsLoader()
                           .DoWeHaveUploads();
            if (battview)
                return 4;

            bool chargeCycles =
                DbSingleton.DBManagerServiceInstance
                           .GetChargeCyclesLoader()
                           .DoWeHaveUploads();
            if (chargeCycles)
                return 5;

            return 0;
        }

        public static void ShowUploadsError()
        {
            ACUserDialogs.ShowAlert(AppResources.upload_before_synch_error);
        }
    }
}
