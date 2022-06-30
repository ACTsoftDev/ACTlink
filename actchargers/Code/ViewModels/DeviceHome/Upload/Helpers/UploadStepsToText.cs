namespace actchargers
{
    public static class UploadStepsToText
    {
        public static string GetTextByUploadSteps(UploadSteps uploadSteps)
        {
            switch (uploadSteps)
            {
                case UploadSteps.INIT:
                    return AppResources.UploadSteps_init;

                case UploadSteps.DATABASE:
                    return AppResources.UploadSteps_database;

                case UploadSteps.BACKUP:
                    return AppResources.UploadSteps_backup;

                case UploadSteps.ITEMS_VIEW:
                    return AppResources.UploadSteps_items_view;

                case UploadSteps.UPLOADING:
                    return AppResources.UploadSteps_uploading;

                case UploadSteps.UPLOADING_BACKUP:
                    return AppResources.UploadSteps_uploading_backup;

                case UploadSteps.CLEANING:
                    return AppResources.UploadSteps_cleaning;

                case UploadSteps.FINISHED:
                    return AppResources.UploadSteps_finished;

                default:
                    return AppResources.UploadSteps_init;
            }
        }
    }
}