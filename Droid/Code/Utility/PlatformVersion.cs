using System;
using Android.App;
using Android.Content.PM;
using MvvmCross.Platform;
using MvvmCross.Platform.Droid.Platform;

namespace actchargers.Droid
{
    public class PlatformVersion : PlatformVersionBase
    {
        const string PLATFORM_CODE = "ANDROID";

        PackageInfo packageInfo;

        public PlatformVersion()
        {
            Activity context = Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity;

            if (context != null)
            {
                packageInfo = context
                    .PackageManager
                    .GetPackageInfo(context.PackageName, 0);
            }
        }

        public override string GetPlatform()
        {
            return PLATFORM_CODE;
        }

        public override string GetVersionName()
        {
            try
            {
                return packageInfo.VersionName;
            }
            catch (Exception)
            {
                return "";
            }
        }

        public override int GetVersionCode()
        {
            try
            {
                return packageInfo.VersionCode;
            }
            catch (Exception)
            {
                return 0;
            }
        }
    }
}
