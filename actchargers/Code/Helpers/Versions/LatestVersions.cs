using System;
using System.Threading.Tasks;

namespace actchargers.Code.Helpers.Versions
{
    public class LatestVersions
    {
        const string SOFTWARE_KEY = "software";
        const int EXPECTED_VERSIONS_ARRAY_LENGTH = 4;

        public float LatestSoftwareVersion
        {
            get;
            private set;
        }

        public float LatestMcbVersion
        {
            get;
            private set;
        }

        public float LatestBattVersion
        {
            get;
            private set;
        }

        public async Task Read()
        {
            var status = await ACTViewConnect.GetQuantumVersions();
            ReadAllVersions(status.returnValue);
        }

        void ReadAllVersions(Newtonsoft.Json.Linq.JObject returnValue)
        {
            if (returnValue == null)
            {
                SetDefaultVersions();

                return;
            }

            string sourceSoftwareVersion = returnValue[SOFTWARE_KEY].ToString();
            string[] versionsArray = SplitVersionsToArray(sourceSoftwareVersion);

            if (IsValidVersionsArray(versionsArray))
            {
                SetVersions(versionsArray);
            }
            else
            {
                SetDefaultVersions();
            }
        }

        string[] SplitVersionsToArray(string sourceSoftwareVersion)
        {
            char[] stringSeparators = { '.' };

            return sourceSoftwareVersion.Split(stringSeparators);
        }

        bool IsValidVersionsArray(string[] versionsArray)
        {
            if (versionsArray == null)
                return false;

            return versionsArray.Length == EXPECTED_VERSIONS_ARRAY_LENGTH;
        }

        void SetVersions(string[] versionsArray)
        {
            try
            {
                LatestSoftwareVersion = GetSoftwareVersion(versionsArray);
            }
            catch (Exception)
            {
                LatestSoftwareVersion = 0.0f;
            }

            LatestMcbVersion = float.Parse(versionsArray[2]);
            LatestBattVersion = float.Parse(versionsArray[3]);
        }

        float GetSoftwareVersion(string[] versionsArray)
        {
            string fullVersionString = string.Format(
                "{0}.{1}", versionsArray[0], versionsArray[1]);

            return float.Parse(fullVersionString);
        }

        void SetDefaultVersions()
        {
            LatestSoftwareVersion = 0.0f;
            LatestMcbVersion = 0.0f;
            LatestBattVersion = 0.0f;
        }
    }
}
