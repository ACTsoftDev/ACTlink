using System;
using MvvmCross.Core.ViewModels;
using ObjCRuntime;
using OxyPlot;
using UIKit;

namespace actchargers.iOS
{
    public class CustomAlert : ICustomAlert
    {
        CustomAlertView alertView;
        CustomPlotView plotView;
        UIAlertView progressAlert;

        public void AlertWithTwoButton
        (string message, string title = null, string okButtonText = "OK",
         string cancelButtonText = "Cancel",
         Action okClicked = null, Action cancelClikced = null)
        {
            UIApplication.SharedApplication.InvokeOnMainThread(new Action(() =>
            {
                AlertWithTwoButtonAction
                (message, title, okButtonText, cancelButtonText, okClicked,
                 cancelClikced);
            }));
        }

        void AlertWithTwoButtonAction
       (string message, string title = null, string okButtonText = "OK",
        string cancelButtonText = "Cancel",
        Action okClicked = null, Action cancelClikced = null)
        {
            UIAlertView alert = new UIAlertView
                (title, message, null, cancelButtonText, null);
            alert.AddButton(okButtonText);
            alert.Clicked += (object sender, UIButtonEventArgs e) =>
            {
                switch (e.ButtonIndex)
                {
                    case 0:
                        cancelClikced?.Invoke();
                        break;
                    case 1:
                        okClicked?.Invoke();
                        break;
                }
            };
            alert.Show();
        }

        public void ShowProgressWithCancel(BaseViewModel viewModel)
        {
            progressAlert = BuildProgressAlert(viewModel);

            progressAlert.Show();
        }

        UIAlertView BuildProgressAlert(BaseViewModel viewModel)
        {
            UIAlertView alert = new UIAlertView(null, null, null, "Cancel", null);
            alert.Clicked += (object sender, UIButtonEventArgs e) =>
            {
                if (e.ButtonIndex == 0)
                {
                    HandleCancelAction(viewModel);
                }
            };

            UIActivityIndicatorView indicator =
                new UIActivityIndicatorView(UIActivityIndicatorViewStyle.Gray);
            indicator.Frame = new CoreGraphics.CGRect(0, 0, 50, 50);

            indicator.StartAnimating();

            alert.SetValueForKey(indicator, (Foundation.NSString)"accessoryView");

            return alert;
        }

        void HandleCancelAction(BaseViewModel viewModel)
        {
            if (viewModel != null)
                viewModel.Cancel();
        }

        public void HideProgressWithCancel()
        {
            if (progressAlert != null)
                progressAlert.DismissWithClickedButtonIndex(-1, true);
        }

        public void ShowPlotView(PlotModel model)
        {
            var views = Foundation.NSBundle.MainBundle.LoadNib("CustomPlotView", null, null);
            plotView = Runtime.GetNSObject(views.ValueAt(0)) as CustomPlotView;
            plotView.Frame = new CoreGraphics.CGRect(0, 0, UIScreen.MainScreen.Bounds.Width, UIScreen.MainScreen.Bounds.Height);
            plotView.ApplyPlotView(model);
            UIApplication.SharedApplication.Windows[0].AddSubview(plotView);
        }

        public void RemoveCustomAlert()         {             if (alertView != null)             {                 alertView.RemoveFromSuperview();             }         }          public void ShowCustomAlert(string message, string text, string actionText, IMvxCommand batt_testGenericButton_Click)         {             var views = Foundation.NSBundle.MainBundle.LoadNib("CustomAlertView", null, null);             alertView = Runtime.GetNSObject(views.ValueAt(0)) as CustomAlertView;             alertView.Frame = new CoreGraphics.CGRect(0, 0, UIScreen.MainScreen.Bounds.Width, UIScreen.MainScreen.Bounds.Height);             alertView.applyText(message, text, actionText);             UIApplication.SharedApplication.Windows[0].AddSubview(alertView);              alertView.ButtonDone.TouchUpInside += (sender, e) =>
            {
                batt_testGenericButton_Click.Execute(alertView.valueTextField.Text);             };         }

        public void ShowAlertThenOpenUrl
        (string message, string urlAddress, string title = "")
        {
            UIApplication.SharedApplication.InvokeOnMainThread(new Action(() =>
            {
                ShowAlertThenOpenUrlAction
                (message, urlAddress, title);
            }));
        }

        void ShowAlertThenOpenUrlAction
       (string message, string urlAddress, string title = "")
        {
            // create text view with variable size message
            UITextView alertTextView = new UITextView();
            alertTextView.Text = urlAddress;

            // enable links data inside textview and customize textview
            alertTextView.DataDetectorTypes = UIDataDetectorType.All;
            alertTextView.ScrollEnabled = false; // is necessary 
            alertTextView.BackgroundColor = UIColor.FromRGB(243, 243, 243); // close to alertview default color
            alertTextView.Editable = false;
            alertTextView.TextAlignment = UITextAlignment.Center;

            // create UIAlertView 
            UIAlertView alert = new UIAlertView(title, message, null, null, null);
            alert.AddButton("OK");

            alert.SetValueForKey(alertTextView, (Foundation.NSString)"accessoryView");

            // IMPORTANT/OPTIONAL need to set frame of textview after adding to subview
            // this will size the text view appropriately so that all data is shown (also resizes alertview
            alertTextView.Frame = new CoreGraphics.CGRect
                (0, 0, alertTextView.ContentSize.Width,
                alertTextView.ContentSize.Height);

            alert.Show();
        }
    }
}
