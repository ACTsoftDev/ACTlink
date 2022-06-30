using System;
using MvvmCross.Core.ViewModels;
using OxyPlot;

namespace actchargers
{
    public interface ICustomAlert
    {
        void ShowCustomAlert
        (string message, string text, string actionText,
         IMvxCommand batt_testGenericButton_Click);

        void ShowProgressWithCancel(BaseViewModel viewModel);

        void HideProgressWithCancel();

        void ShowPlotView(PlotModel view);

        void RemoveCustomAlert();

        void AlertWithTwoButton
        (string message, string title = null, string okButtonText = "OK",
         string cancelButtonText = "Cancel", Action okClicked = null,
         Action cancelClikced = null);

        void ShowAlertThenOpenUrl
        (string message, string urlAddress, string title = "");
    }
}
