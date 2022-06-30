using System;
using System.Diagnostics;
using Acr.UserDialogs;
using MvvmCross.Platform;

namespace actchargers
{
    public static class ACUserDialogs
    {
        public static void ShowProgress(string loadingText = null)
        {
            try
            {
                UserDialogs.Instance.ShowLoading(loadingText);
            }
            catch (Exception ex)
            {
                Logger.AddLog(true, "X83" + ex);
                Debug.WriteLine(ex.Message);
            }
        }

        public static void HideProgress()
        {
            try
            {
                UserDialogs.Instance.HideLoading();
            }
            catch (Exception ex)
            {
                Logger.AddLog(true, "X83" + ex);
                Debug.WriteLine(ex.Message);
            }
        }

        public static void ShowProgressWithCancel(BaseViewModel viewModel)
        {
            try
            {
                Mvx.Resolve<ICustomAlert>().ShowProgressWithCancel(viewModel);
            }
            catch (Exception ex)
            {
                Logger.AddLog(true, "X83" + ex);
                Debug.WriteLine(ex.Message);
            }
        }

        public static void HideProgressWithCancel()
        {
            try
            {
                Mvx.Resolve<ICustomAlert>().HideProgressWithCancel();
            }
            catch (Exception ex)
            {
                Logger.AddLog(true, "X83" + ex);
                Debug.WriteLine(ex.Message);
            }
        }

        public static void ShowAlert(string alertMessage = null)
        {
            try
            {
                UserDialogs.Instance.Alert(alertMessage);
            }
            catch (Exception ex)
            {
                Logger.AddLog(true, "X83" + ex);
                Debug.WriteLine(ex.Message);
            }

        }

        public static void ShowAlert
        (string alertMessage, string title, string okText)
        {
            try
            {
                UserDialogs.Instance.Alert(alertMessage, title, okText);
            }
            catch (Exception ex)
            {
                Logger.AddLog(true, "X83" + ex);
                Debug.WriteLine(ex.Message);
            }
        }

        public static void ShowAlertWithTitleAndOkButton(string alertMessage)
        {
            try
            {
                ShowAlert(alertMessage, "", AppResources.ok);
            }
            catch (Exception ex)
            {
                Logger.AddLog(true, "X83" + ex);
                Debug.WriteLine(ex.Message);
            }

        }

        public static void ShowAlertWithTwoButtons
        (string alertMessage, string title, string okText = "Ok",
         string cancelText = "Cancel",
         Action okClicked = null, Action cancelClicked = null)
        {
            try
            {
                Mvx.Resolve<ICustomAlert>()
                   .AlertWithTwoButton
                   (alertMessage, title, okText, cancelText, okClicked,
                    cancelClicked);
            }
            catch (Exception ex)
            {
                Logger.AddLog(true, "X83" + ex);
                Debug.WriteLine(ex.Message);
            }

        }

        public static void ShowAlertWithOkButtonsAction
        (string alertMessage, Action okClicked = null)
        {
            try
            {
                AlertConfig alertConfig = new AlertConfig
                {
                    Message = alertMessage,
                    OkText = AppResources.ok,
                    OnAction = okClicked
                };

                UserDialogs.Instance.Alert(alertConfig);
            }
            catch (Exception ex)
            {
                Logger.AddLog(true, "X83" + ex);
                Debug.WriteLine(ex.Message);
            }
        }

        public static void ShowAlertThenOpenUrl
        (string alertMessage = null, string urlAddress = null)
        {
            try
            {
                Mvx.Resolve<ICustomAlert>().ShowAlertThenOpenUrl
                   (alertMessage, urlAddress);
            }
            catch (Exception ex)
            {
                Logger.AddLog(true, "X83" + ex);
                Debug.WriteLine(ex.Message);
            }
        }
    }
}
