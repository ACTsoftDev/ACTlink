using System.Collections.Generic;

namespace actchargers
{
    public abstract class DeviceWarningManager
    {
        const string DIVIDE = "\r\n";

        protected List<string> warnings;

        protected DeviceWarningManager()
        {
            warnings = new List<string>();
        }

        protected abstract void CheckAll();

        public void ShowWarningsIfAny()
        {
            CheckAll();

            if (CanShowWarnings())
            {
                ForceShowWarnings();
            }
        }

        bool CanShowWarnings()
        {
            bool canShowWarnings =
                !ControlObject.isHWMnafacturer && warnings.Count > 0;

            return canShowWarnings;
        }

        void ForceShowWarnings()
        {
            string warningsText = string.Join(DIVIDE, warnings);

            ACUserDialogs.ShowAlert(warningsText);
        }
    }
}
