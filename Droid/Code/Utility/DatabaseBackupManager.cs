﻿using System;
using System.Diagnostics;
using System.IO;

namespace actchargers.Droid
{
    public class DatabaseBackupManager : IDatabaseBackupManager
    {
        public string GetDatabaseBackupPath()
        {
            string documentsPath =
                Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;


            return Path
                .Combine
                (documentsPath, ACConstants.DATABASE_BACKUP_FILE_NAME);
        }

        public void BackupDatabase(string json)
        {
            try
            {
                TryBackupDatabase(json);
            }
            catch (Exception ex)
            {
                Debug.Write(ex.ToString());
            }
        }

        void TryBackupDatabase(string json)
        {
            var backupPath = GetDatabaseBackupPath();

            DeleteFileIfExists(backupPath);

            File.WriteAllText(backupPath, json);
        }

        void DeleteFileIfExists(string path)
        {
            try
            {
                TryDeleteFileIfExists(path);
            }
            catch (Exception ex)
            {
                Debug.Write(ex.ToString());
            }
        }

        void TryDeleteFileIfExists(string path)
        {
            if (File.Exists(path))
                File.Delete(path);
        }

        public bool HasBackup()
        {
            try
            {
                return TryHasBackup();
            }
            catch (Exception ex)
            {
                Debug.Write(ex.ToString());

                return false;
            }
        }

        bool TryHasBackup()
        {
            var backupPath = GetDatabaseBackupPath();

            return File.Exists(backupPath);
        }

        public string GetSavedBackup()
        {
            try
            {
                return TryGetSavedBackup();
            }
            catch (Exception ex)
            {
                Debug.Write(ex.ToString());

                return null;
            }
        }

        string TryGetSavedBackup()
        {
            var backupPath = GetDatabaseBackupPath();

            using (var reader = new StreamReader(backupPath))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
