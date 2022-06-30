using MvvmCross.Platform;
using Newtonsoft.Json.Linq;
using SQLite;

namespace actchargers
{
    public class DBManagerService : IDBManagerService
    {
        readonly SQLiteConnection database;

        public DBManagerService()
        {
            string databasePath = Mvx.Resolve<ISQLite>().GetDatabasePath();
            database = new SQLiteConnection(databasePath);

            Init();
        }

        void Init()
        {
            TablesCreaterAndDropper.CreateAll(database);
        }

        public void ResetAfterUpload()
        {
            BackupTables();

            DropTables();
            RecreateTables();

            RestoreTables();
        }

        void BackupTables()
        {
            GetGenericObjectsLoader().Backup();
            GetChargeCyclesLoader().Backup();
            GetPMFaultsLoader().Backup();
            GetBattviewEventsLoader().Backup();
            GetFirmwareDownloadedLoader().Backup();
            GetSynchSiteObjectsLoader().Backup();
            GetSynchObjectsBufferedDataLoader().Backup();
        }

        void DropTables()
        {
            TablesCreaterAndDropper.DropResettableTables(database);
        }

        void RecreateTables()
        {
            TablesCreaterAndDropper.RecreateResettableTables(database);
        }

        void RestoreTables()
        {
            GetGenericObjectsLoader().Restore();
            GetChargeCyclesLoader().Restore();
            GetPMFaultsLoader().Restore();
            GetBattviewEventsLoader().Restore();
            GetFirmwareDownloadedLoader().Restore();
            GetSynchSiteObjectsLoader().Restore();
            GetSynchObjectsBufferedDataLoader().Restore();
        }

        BattviewEventsLoader _BattviewEventsLoader;
        public BattviewEventsLoader GetBattviewEventsLoader()
        {
            if (_BattviewEventsLoader == null)
                _BattviewEventsLoader = new BattviewEventsLoader(database);

            return _BattviewEventsLoader;
        }

        ChargeCyclesLoader _ChargeCyclesLoader;
        public ChargeCyclesLoader GetChargeCyclesLoader()
        {
            if (_ChargeCyclesLoader == null)
                _ChargeCyclesLoader = new ChargeCyclesLoader(database);

            return _ChargeCyclesLoader;
        }

        PMFaultsLoader _PMFaultsLoader;
        public PMFaultsLoader GetPMFaultsLoader()
        {
            if (_PMFaultsLoader == null)
                _PMFaultsLoader = new PMFaultsLoader(database);

            return _PMFaultsLoader;
        }

        SynchObjectsBufferedDataLoader _SynchObjectsBufferedDataLoader;
        public SynchObjectsBufferedDataLoader GetSynchObjectsBufferedDataLoader()
        {
            if (_SynchObjectsBufferedDataLoader == null)
                _SynchObjectsBufferedDataLoader = new SynchObjectsBufferedDataLoader(database);

            return _SynchObjectsBufferedDataLoader;
        }

        DBConnectedDevicesLoader _DBConnectedDevicesLoader;
        public DBConnectedDevicesLoader GetDBConnectedDevicesLoader()
        {
            if (_DBConnectedDevicesLoader == null)
                _DBConnectedDevicesLoader = new DBConnectedDevicesLoader(database);

            return _DBConnectedDevicesLoader;
        }

        SynchSiteObjectsLoader _SynchSiteObjectsLoader;
        public SynchSiteObjectsLoader GetSynchSiteObjectsLoader()
        {
            if (_SynchSiteObjectsLoader == null)
                _SynchSiteObjectsLoader = new SynchSiteObjectsLoader(database);

            return _SynchSiteObjectsLoader;
        }

        DBUserLoader _DBUserLoader;
        public DBUserLoader GetDBUserLoader()
        {
            if (_DBUserLoader == null)
                _DBUserLoader = new DBUserLoader(database);

            return _DBUserLoader;
        }

        DevicesObjectsLoader _DevicesObjectsLoader;
        public DevicesObjectsLoader GetDevicesObjectsLoader()
        {
            if (_DevicesObjectsLoader == null)
                _DevicesObjectsLoader = new DevicesObjectsLoader(database);

            return _DevicesObjectsLoader;
        }

        FirmwareDownloadedLoader _FirmwareDownloadedLoader;
        public FirmwareDownloadedLoader GetFirmwareDownloadedLoader()
        {
            if (_FirmwareDownloadedLoader == null)
                _FirmwareDownloadedLoader = new FirmwareDownloadedLoader(database);

            return _FirmwareDownloadedLoader;
        }

        GenericObjectsLoader _GenericObjectsLoader;
        public GenericObjectsLoader GetGenericObjectsLoader()
        {
            if (_GenericObjectsLoader == null)
                _GenericObjectsLoader = new GenericObjectsLoader(database);

            return _GenericObjectsLoader;
        }

        ReplaceDevicesLoaders _ReplaceDevicesLoaders;
        public ReplaceDevicesLoaders GetReplaceDevicesLoaders()
        {
            if (_ReplaceDevicesLoaders == null)
                _ReplaceDevicesLoaders = new ReplaceDevicesLoaders(database);

            return _ReplaceDevicesLoaders;
        }

        IncompleteDownloadLoader _IncompleteDownloadLoader;
        public IncompleteDownloadLoader GetIncompleteDownloadLoader()
        {
            if (_IncompleteDownloadLoader == null)
                _IncompleteDownloadLoader = new IncompleteDownloadLoader(database);

            return _IncompleteDownloadLoader;
        }

        public string GetAllAsJson()
        {
            return GetAllAsJsonObject().ToString();
        }

        JObject GetAllAsJsonObject()
        {
            JObject jObject = new JObject();
            var allJsonProperties = GetAllJsonProperties();

            foreach (var item in allJsonProperties)
            {
                jObject.Add(item);
            }

            return jObject;
        }

        JProperty[] GetAllJsonProperties()
        {
            var allJProperties = new JProperty[]
            {
                GetBattviewEventsLoader().SerializeToJsonProperty(),
                GetChargeCyclesLoader().SerializeToJsonProperty(),
                GetPMFaultsLoader().SerializeToJsonProperty(),
                GetSynchObjectsBufferedDataLoader().SerializeToJsonProperty(),
                GetSynchSiteObjectsLoader().SerializeToJsonProperty(),
                GetDBConnectedDevicesLoader().SerializeToJsonProperty(),
                GetDBUserLoader().SerializeToJsonProperty(),
                GetDevicesObjectsLoader().SerializeToJsonProperty(),
                GetFirmwareDownloadedLoader().SerializeToJsonProperty(),
                GetGenericObjectsLoader().SerializeToJsonProperty(),
                GetReplaceDevicesLoaders().SerializeToJsonProperty(),
                GetIncompleteDownloadLoader().SerializeToJsonProperty()
            };

            return allJProperties;
        }
    }
}