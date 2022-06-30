namespace actchargers
{
    public interface IDatabaseBackupManager
    {
        string GetDatabaseBackupPath();

        void BackupDatabase(string json);

        bool HasBackup();

        string GetSavedBackup();
    }
}
