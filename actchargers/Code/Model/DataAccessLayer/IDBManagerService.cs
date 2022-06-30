namespace actchargers
{
    public interface IDBManagerService
    {
        void ResetAfterUpload();

        BattviewEventsLoader GetBattviewEventsLoader();

        ChargeCyclesLoader GetChargeCyclesLoader();

        PMFaultsLoader GetPMFaultsLoader();

        SynchObjectsBufferedDataLoader GetSynchObjectsBufferedDataLoader();

        SynchSiteObjectsLoader GetSynchSiteObjectsLoader();

        DBConnectedDevicesLoader GetDBConnectedDevicesLoader();

        DBUserLoader GetDBUserLoader();

        DevicesObjectsLoader GetDevicesObjectsLoader();

        FirmwareDownloadedLoader GetFirmwareDownloadedLoader();

        GenericObjectsLoader GetGenericObjectsLoader();

        ReplaceDevicesLoaders GetReplaceDevicesLoaders();

        IncompleteDownloadLoader GetIncompleteDownloadLoader();

        string GetAllAsJson();
    }
}
