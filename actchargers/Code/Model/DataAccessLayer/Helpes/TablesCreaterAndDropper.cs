using SQLite;

namespace actchargers
{
    public static class TablesCreaterAndDropper
    {
        public static void CreateAll(SQLiteConnection database)
        {
            database.CreateTable<BattviewEvents>();
            database.CreateTable<ChargeCycles>();
            database.CreateTable<PmFaults>();
            database.CreateTable<SynchObjectsBufferedData>();
            database.CreateTable<SynchSiteObjects>();
            database.CreateTable<DBConnectedDevices>();
            database.CreateTable<DBUser>();
            database.CreateTable<AccessPermission>();
            database.CreateTable<DevicesObjects>();
            database.CreateTable<FirmwareDownloaded>();
            database.CreateTable<GenericObjects>();
            database.CreateTable<ReplaceDevices>();
            database.CreateTable<IncompleteDownload>();
        }

        public static void DropAll(SQLiteConnection database)
        {
            database.DropTable<BattviewEvents>();
            database.DropTable<ChargeCycles>();
            database.DropTable<PmFaults>();
            database.DropTable<SynchObjectsBufferedData>();
            database.DropTable<SynchSiteObjects>();
            database.DropTable<DBConnectedDevices>();
            database.DropTable<DBUser>();
            database.DropTable<AccessPermission>();
            database.DropTable<DevicesObjects>();
            database.DropTable<FirmwareDownloaded>();
            database.DropTable<GenericObjects>();
            database.DropTable<ReplaceDevices>();
            database.DropTable<IncompleteDownload>();
        }

        public static void RecreateResettableTables(SQLiteConnection database)
        {
            database.CreateTable<BattviewEvents>();
            database.CreateTable<ChargeCycles>();
            database.CreateTable<PmFaults>();
            database.CreateTable<SynchObjectsBufferedData>();
            database.CreateTable<SynchSiteObjects>();
            database.CreateTable<FirmwareDownloaded>();
            database.CreateTable<GenericObjects>();
            database.CreateTable<ReplaceDevices>();
        }

        public static void DropResettableTables(SQLiteConnection database)
        {
            database.DropTable<BattviewEvents>();
            database.DropTable<ChargeCycles>();
            database.DropTable<PmFaults>();
            database.DropTable<SynchObjectsBufferedData>();
            database.DropTable<SynchSiteObjects>();
            database.DropTable<FirmwareDownloaded>();
            database.DropTable<GenericObjects>();
            database.DropTable<ReplaceDevices>();
        }
    }
}
