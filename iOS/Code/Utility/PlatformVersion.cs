using Foundation;

namespace actchargers.iOS
{
    public class PlatformVersion : PlatformVersionBase
    {
        const string PLATFORM_CODE = "IOS";

        public override string GetPlatform()
        {
            return PLATFORM_CODE;
        }

        public override string GetVersionName()
        {
            return NSBundle.MainBundle.InfoDictionary
                           [new NSString("CFBundleShortVersionString")].ToString();
        }

        public override int GetVersionCode()
        {
            string bundleVersionString = NSBundle.MainBundle.InfoDictionary
                            [new NSString("CFBundleVersion")].ToString();

            return int.Parse(bundleVersionString);
        }
    }
}
