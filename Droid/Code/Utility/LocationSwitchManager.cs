using Android.Content;
using Android.Locations;

namespace actchargers.Droid
{
    public static class LocationSwitchManager
    {
        public static bool PrepareLocation(Context context)
        {
            bool isLocationOff = IsLocationOff(context);

            if (isLocationOff)
                EnableLocation(context);

            return isLocationOff;
        }

        static bool IsLocationOff(Context context)
        {
            var locationManager = (LocationManager)context.GetSystemService(Context.LocationService);

            return !locationManager.IsProviderEnabled(LocationManager.GpsProvider);
        }

        static void EnableLocation(Context context)
        {
            ACUserDialogs.ShowAlertWithTwoButtons(AppResources.enable_location_message, "", AppResources.ok, AppResources.cancel, () => GoToLocationIntent(context), null);
        }

        static void GoToLocationIntent(Context context)
        {
            var intent = new Intent(Android.Provider.Settings.ActionLocationSourceSettings);

            context.StartActivity(intent);
        }
    }
}
