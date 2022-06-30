using System;
using System.Threading.Tasks;

namespace actchargers
{
    public class Uploader
    {
        public delegate void StatusChangedEventHandler
        (object sender, UploadStepsEventArgs e);
        public event StatusChangedEventHandler StatusChanged;
        public event EventHandler NoDataReceived;
        public event EventHandler<string> FailuresReceived;

        BackupAndCleaner backupAndCleaner;
        AllSourceLists allSourceLists;
        ListsOrganizer listsOrganizer;
        UploadableRequestManager uploadableRequestManager;

#pragma warning disable RECS0122 // Initializing field with default value is redundant
        readonly bool dontMarkAsUploaded = false;
#pragma warning restore RECS0122 // Initializing field with default value is redundant

        public UploadableDevicesLists DevicesLists
        {
            get { return listsOrganizer.DevicesLists; }
        }

        public Uploader()
        {
            ChangeUploadStep(UploadSteps.INIT);

            Init();
        }

        void Init()
        {
            backupAndCleaner = new BackupAndCleaner();
            allSourceLists = new AllSourceLists();
            listsOrganizer = new ListsOrganizer(allSourceLists);
            uploadableRequestManager =
                new UploadableRequestManager(dontMarkAsUploaded);
        }

        public async Task ReadAll()
        {
            ChangeUploadStep(UploadSteps.DATABASE);

            await allSourceLists.ReadLists();
            listsOrganizer.Organize();

            ChangeUploadStep(UploadSteps.ITEMS_VIEW);
        }

        void AddFakedUploads()
        {
#if DEBUG
            FakedUploadsGenerator.AddFakedUploads();
#endif
        }

        public async Task Start()
        {
            await BackupDatabase();
            await CheckToUpload();
            if (CanMarkAsUploaded())
                await CleanAfterUpload();
        }

        bool CanMarkAsUploaded()
        {
            return !dontMarkAsUploaded;
        }

        async Task BackupDatabase()
        {
            ChangeUploadStep(UploadSteps.BACKUP);

            await backupAndCleaner.BackupDatabase();
        }

        async Task CheckToUpload()
        {
            if (HasUploads())
            {
                await Upload();
            }
            else
            {
                OnNoData();
            }
        }

        bool HasUploads()
        {
            return DevicesLists != null && DevicesLists.HasUploads();
        }

        async Task Upload()
        {
            ChangeUploadStep(UploadSteps.UPLOADING);

            await uploadableRequestManager
                .UploadAll(listsOrganizer.DevicesLists);

            await OnUploadFinished();
        }

        async Task OnUploadFinished()
        {
            if (HasFailures())
            {
                await OnHasFailures();
            }

            SetLastUploadDate();
        }

        bool HasFailures()
        {
            return uploadableRequestManager.HasFailures;
        }

        bool ShouldUpdate()
        {
            return uploadableRequestManager.ShouldUpdate;
        }

        async Task OnHasFailures()
        {
            string message = ShouldUpdate() ?
                SoftwareUpdateHelper.GetUpdateWarningMessage() :
                AppResources.some_records_failed_to_upload;

            FailuresReceived?.Invoke(this, message);

            await UploadBackupDatabase();
        }

        void SetLastUploadDate()
        {
            DbSingleton
                .DBManagerServiceInstance
                .GetGenericObjectsLoader()
                .SetLastUploadDate();
        }

        async Task UploadBackupDatabase()
        {
            ChangeUploadStep(UploadSteps.UPLOADING_BACKUP);

            await backupAndCleaner.UploadBackupDatabase();
        }

        void OnNoData()
        {
            NoDataReceived?.Invoke(this, EventArgs.Empty);
        }

        async Task CleanAfterUpload()
        {
            ChangeUploadStep(UploadSteps.CLEANING);

            await backupAndCleaner.CleanUploadedData();

            ChangeUploadStep(UploadSteps.FINISHED);
        }

        void ChangeUploadStep(UploadSteps uploadSteps)
        {
            float progress =
                UploadStepsToProgress.GetPrgoressByUploadSteps(uploadSteps);

            var eventArgs = new UploadStepsEventArgs()
            {
                CurrentStep = uploadSteps,
                Progress = progress
            };

            OnUploadStepsChanged(eventArgs);
        }

        void OnUploadStepsChanged(UploadStepsEventArgs eventArgs)
        {
            StatusChanged?.Invoke(this, eventArgs);
        }
    }
}
