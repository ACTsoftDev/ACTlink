using System;

namespace actchargers
{
    public static class SoftwareUpdateHelper
    {
        public static void ShowUpdateWarning()
        {
            string updateWarningMessage = GetUpdateWarningMessage();
            ACUserDialogs.ShowAlert(updateWarningMessage);
        }

        public static void ShowUpdateWarningWithAction(Action handleAction)
        {
            string updateWarningMessage = GetUpdateWarningMessage();
            ACUserDialogs.ShowAlertWithOkButtonsAction
                         (updateWarningMessage, handleAction);
        }

        public static string GetUpdateWarningMessage()
        {
            string message =
                ControlObject.isHWMnafacturer ?
                             AppResources.contact_act_for_sw
                             : AppResources.download_it_again;

            return string.Format("{0} {1}", AppResources.sw_expired, message);
        }
    }
}
