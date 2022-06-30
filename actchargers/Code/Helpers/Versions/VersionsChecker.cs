using System;
using MvvmCross.Platform;

namespace actchargers.Code.Helpers.Versions
{
    public class VersionsChecker
    {
        const long UPDATE_ALERT_PERIOD_IN_MINUTES = 15;

        AllVersions allVersions = new AllVersions();
        LatestVersions latestVersions = new LatestVersions();

        readonly IUserPreferences userPreferences = Mvx.Resolve<IUserPreferences>();

        public async void Check()
        {
            await latestVersions.Read();

            if (NeedsUpdate())
            {
                AlertIfAccepted();
            }
        }

        bool NeedsUpdate()
        {
            bool softwareIsLarger =
                latestVersions.LatestSoftwareVersion > allVersions.SoftwareVersion;
            bool mcbIsLarger =
                latestVersions.LatestMcbVersion > allVersions.McbVersionAsLatestFormat;
            bool battIsLarger =
                latestVersions.LatestBattVersion > allVersions.BattviewVersionAsLatestFormat;

            return softwareIsLarger || mcbIsLarger || battIsLarger;
        }

        void AlertIfAccepted()
        {
            if (AlertPeriodIsOk())
            {
                ShowAlert();
                SaveAlertDate();
            }
        }

        bool AlertPeriodIsOk()
        {
            DateTime nowDate = DateTime.Now;
            DateTime latestAlertPlusPeriodDate = GetLatestAlertPlusPeriodDate();

            return nowDate > latestAlertPlusPeriodDate;
        }

        void ShowAlert()
        {
            ACUserDialogs.ShowAlert(AppResources.new_software_is_available);
        }

        DateTime GetLatestAlertPlusPeriodDate()
        {
            return GetLatestAlertDate()
                .AddMinutes(UPDATE_ALERT_PERIOD_IN_MINUTES);
        }

        DateTime GetLatestAlertDate()
        {
            try
            {
                return TryGetLatestAlertDate();
            }
            catch (Exception)
            {
                return DateTime.MinValue;
            }
        }

        DateTime TryGetLatestAlertDate()
        {
            // It is saved as string because iOS preferences
            // does not support (long) directly.

            string source = userPreferences
                .GetString(ACConstants.PREF_UPDATE_ALERT_DATE);
            long ticks = long.Parse(source);

            return new DateTime(ticks);
        }

        void SaveAlertDate()
        {
            // It is saved as string because iOS preferences
            // does not support (long) directly.

            long ticks = DateTime.Now.Ticks;
            string ticksString = ticks.ToString();

            userPreferences.SetString(ACConstants.PREF_UPDATE_ALERT_DATE,
                                      ticksString);
        }
    }
}
