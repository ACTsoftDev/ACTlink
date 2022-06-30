namespace actchargers.Code.Helpers.Versions
{
    public static class VersionsConverter
    {
        const float FACTOR = 100.0f;

        public static int FromLocalToLatest(float localVersion)
        {
            return (int)(localVersion * FACTOR);
        }

        public static float FromLatestToLocal(int latestVersion)
        {
            return latestVersion / FACTOR;
        }
    }
}
