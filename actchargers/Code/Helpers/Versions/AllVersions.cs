using System;
using MvvmCross.Platform;

namespace actchargers.Code.Helpers.Versions
{
    public class AllVersions
    {
        const int MAX_VERSION_PARTS = 2;
        const char SEPARATOR = '.';

        public string SoftwareVersionText
        {
            get
            {
                return GetSoftwareVersionText();
            }
        }

        public float SoftwareVersion
        {
            get
            {
                return GetSoftwareVersion();
            }

        }

        float GetSoftwareVersion()
        {
            string twoPartsSoftwareVersionText = GetTwoPartsSoftwareVersionText();
            float softwareVersion = float.Parse(twoPartsSoftwareVersionText);

            return softwareVersion;
        }

        string GetTwoPartsSoftwareVersionText()
        {
            string softwareVersionText = GetSoftwareVersionText();
            string[] parts = softwareVersionText.Split(SEPARATOR);
            int count = parts.Length;
            int min = Math.Min(count, MAX_VERSION_PARTS);

            string twoParts = string.Join(SEPARATOR.ToString(), parts, 0, min);

            return twoParts;
        }

        string GetSoftwareVersionText()
        {
            return Mvx.Resolve<IPlatformVersion>().GetVersionName();
        }

        public float McbVersion
        {
            get
            {
                return Firmware.software_MCBFirmwareFileVersion;
            }
        }

        public float McbVersionAsLatestFormat
        {
            get
            {
                return VersionsConverter.FromLocalToLatest(McbVersion);
            }
        }

        public float BattviewVersion
        {
            get
            {
                return Firmware.software_BattFirmwareFileVersion;
            }
        }

        public float BattviewVersionAsLatestFormat
        {
            get
            {
                return VersionsConverter.FromLocalToLatest(BattviewVersion);
            }
        }

        public float CalibratorVersion
        {
            get
            {
                return Firmware.software_calibratorFirmwareVersion;
            }
        }

        public float CalibratorVersionAsLatestFormat
        {
            get
            {
                return VersionsConverter.FromLocalToLatest(CalibratorVersion);
            }
        }
    }
}
