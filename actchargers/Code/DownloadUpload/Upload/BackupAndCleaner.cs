using System;
using System.Diagnostics;
using System.Threading.Tasks;
using MvvmCross.Platform;

namespace actchargers
{
    public class BackupAndCleaner
    {
        string allAsJson;

        public async Task BackupDatabase()
        {
            allAsJson = GetAllTablesAsJson();

            await SaveJsonBackupDatabase(allAsJson);
        }

        public async Task UploadBackupDatabase()
        {
            if (string.IsNullOrEmpty(allAsJson))
                allAsJson = GetAllTablesAsJson();

            await UploadJsonBackupDatabase(allAsJson);
        }

        public async Task CleanUploadedData()
        {
            await Task.Run(() => CleanUploadedDataTask());
        }

        void CleanUploadedDataTask()
        {
            DbSingleton
                .DBManagerServiceInstance
                .ResetAfterUpload();
        }

        public async Task PushBackup()
        {
            bool hasBackup = Mvx.Resolve<IDatabaseBackupManager>().HasBackup();

            if (hasBackup)
            {
                await ReadBackupAndUpload();
            }
            else
            {
                await BackupAndUpload();
            }
        }

        async Task ReadBackupAndUpload()
        {
            string backupJson = GetSavedBackup();

            await UploadJsonBackupDatabase(backupJson);
        }

        string GetSavedBackup()
        {
            return Mvx.Resolve<IDatabaseBackupManager>().GetSavedBackup();
        }

        async Task BackupAndUpload()
        {
            string backupJson = GetAllTablesAsJson();

            await UploadJsonBackupDatabase(backupJson);

            await SaveJsonBackupDatabase(backupJson);
        }

        async Task UploadJsonBackupDatabase(string jsonDatabase)
        {
            try
            {
                await UploadJsonBackupDatabaseTry(jsonDatabase);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        async Task UploadJsonBackupDatabaseTry(string jsonDatabase)
        {
            await ACTViewConnect.UploadFailedDB(jsonDatabase);
        }

        string GetAllTablesAsJson()
        {
            return DbSingleton
                .DBManagerServiceInstance
                .GetAllAsJson();
        }

        async Task SaveJsonBackupDatabase(string jsonDatabase)
        {
            await Task.Run(() => SaveJsonBackupDatabaseTask(jsonDatabase));
        }

        void SaveJsonBackupDatabaseTask(string jsonDatabase)
        {
            Mvx.Resolve<IDatabaseBackupManager>().BackupDatabase(jsonDatabase);
        }
    }
}
