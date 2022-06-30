namespace actchargers
{
    public static class UploadStepsToProgress
    {
        public static float GetPrgoressByUploadSteps(UploadSteps uploadSteps)
        {
            switch (uploadSteps)
            {
                case UploadSteps.INIT:
                    return 0.0f;

                case UploadSteps.DATABASE:
                    return 4.0f;

                case UploadSteps.BACKUP:
                    return 7.0f;

                case UploadSteps.ITEMS_VIEW:
                    return 40.0f;

                case UploadSteps.UPLOADING:
                    return 45.0f;

                case UploadSteps.UPLOADING_BACKUP:
                    return 80.0f;

                case UploadSteps.CLEANING:
                    return 90.0f;

                case UploadSteps.FINISHED:
                    return 100.0f;

                default:
                    return 0.0f;
            }
        }
    }
}
