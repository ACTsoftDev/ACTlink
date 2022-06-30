using actchargers.Code.Helpers.Versions;

namespace actchargers
{
    public abstract class PlatformVersionBase : IPlatformVersion
    {
        const string TEMPLATE = "ACTLink {0}: {1}.{2}.{3}";

        public abstract string GetPlatform();

        public abstract string GetVersionName();

        public abstract int GetVersionCode();

        public string GetUserAgentValue()
        {
            AllVersions allVersions = new AllVersions();

            return string.Format(TEMPLATE,
                                 GetPlatform(),
                                 GetVersionName(),
                                 0, 0);
        }
    }
}
