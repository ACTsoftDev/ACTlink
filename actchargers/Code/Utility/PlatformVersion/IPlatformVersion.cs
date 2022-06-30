namespace actchargers
{
    public interface IPlatformVersion
    {
        string GetPlatform();

        string GetVersionName();

        int GetVersionCode();

        string GetUserAgentValue();
    }
}
