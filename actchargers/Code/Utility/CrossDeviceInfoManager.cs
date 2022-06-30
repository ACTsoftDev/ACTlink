using Plugin.DeviceInfo;
using Plugin.DeviceInfo.Abstractions;

namespace actchargers
{
    public static class CrossDeviceInfoManager
    {
        public static Platform GetDevicePlatform()
        {
            return CrossDeviceInfo.Current.Platform;
        }

        public static bool IsAndroid()
        {
            bool isAndroid = (GetDevicePlatform() == Platform.Android);

            return isAndroid;
        }
    }
}
