using System;
using Android.App;
using Android.Content;
using Android.Views;
using Android.Widget;
using MvvmCross.Core.ViewModels;
using OxyPlot;
using OxyPlot.Xamarin.Android;

namespace actchargers.Droid
{
    public class CustomAlert : ICustomAlert
    {
        AlertDialog dialog;
        AlertDialog PlotViewDialog;
        PlotView plotViewType;

#pragma warning disable CS0618 // Type or member is obsolete
        ProgressDialog progressAlert;
#pragma warning restore CS0618 // Type or member is obsolete

        public void AlertWithTwoButton(string message, string title = null, string okButtonText = "OK", string cancelButtonText = "Cancel", Action okClicked = null, Action cancelClikced = null)
        {
            AlertDialog.Builder alertDialogBuilder =
                           new AlertDialog.Builder(BaseActivity.GetTopActivity())
                       .SetMessage(message)
                       .SetTitle(title)
                       .SetCancelable(false)
                       .SetPositiveButton(okButtonText, delegate
                       {
                           okClicked?.Invoke();
                       })
                       .SetNegativeButton(cancelButtonText, delegate
                       {
                           cancelClikced?.Invoke();
                       });
            try
            {
                Application.SynchronizationContext.Post(ignored =>
               {
                   if (BaseActivity.GetTopActivity() == null ||
                       BaseActivity.GetTopActivity().IsFinishing)
                   {
                       return;
                   }
                   BaseActivity.GetTopActivity().RunOnUiThread(() =>
               {
                   alertDialogBuilder.Show();
               });

               }, null);
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

        }

        public void RemoveCustomAlert()
        {
            if (dialog != null && dialog.IsShowing)
            {
                dialog.Dismiss();
            }
        }

        public void ShowCustomAlert
        (string message, string text, string actionText,
         IMvxCommand batt_testGenericButton_Click)
        {
            try
            {
                Application.SynchronizationContext.Post(ignored =>
                {
                    if (BaseActivity.GetTopActivity() == null ||
                        BaseActivity.GetTopActivity().IsFinishing)
                    {
                        return;
                    }

                    BaseActivity.GetTopActivity().RunOnUiThread(() =>
                    {
                        var input = new EditText(BaseActivity.GetTopActivity())
                        {
                            Hint = "",
                            Text = text
                        };

                        dialog = new AlertDialog.Builder(BaseActivity.GetTopActivity())
                        .SetMessage(message)
                            .SetTitle("")
                            .SetView(input)
                            .SetPositiveButton(actionText, delegate
                            {
                                if (batt_testGenericButton_Click != null)
                                    batt_testGenericButton_Click.Execute(input.Text);
                            }).Show();
                    });

                }, null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public void ShowProgressWithCancel(BaseViewModel viewModel)
        {
#pragma warning disable CS0618 // Type or member is obsolete
            progressAlert = new ProgressDialog(BaseActivity.GetTopActivity());
#pragma warning restore CS0618 // Type or member is obsolete

            progressAlert.SetMessage(AppResources.updating);
            progressAlert.SetCancelable(true);
            progressAlert
                .SetButton
                (-2, AppResources.cancel, (sender, e) => HandleCancelAction(viewModel));
            progressAlert.CancelEvent += (sender, e) => HandleCancelAction(viewModel);

            progressAlert.Show();
        }

        void HandleCancelAction(BaseViewModel viewModel)
        {
            if (viewModel != null)
                viewModel.Cancel();
        }

        public void HideProgressWithCancel()
        {
            if (progressAlert != null)
                progressAlert.Cancel();
        }

        public void ShowPlotView(PlotModel plotView)
        {

            LayoutInflater inflater = (LayoutInflater)BaseActivity.GetTopActivity().GetSystemService(Context.LayoutInflaterService);
            View view = inflater.Inflate(Resource.Layout.DialogChartLayout, null);

            Button backButton = (Button)view.FindViewById(Resource.Id.back);
            plotViewType = (PlotView)view.FindViewById(Resource.Id.plot);
            plotViewType.Model = plotView;

            try
            {
                Application.SynchronizationContext.Post(ignored =>
                {
                    if (BaseActivity.GetTopActivity() == null ||
                        BaseActivity.GetTopActivity().IsFinishing)
                    {
                        return;
                    }

                    BaseActivity.GetTopActivity().RunOnUiThread(() =>
                    {
                        PlotViewDialog = new AlertDialog.Builder(BaseActivity.GetTopActivity())
                                                .SetView(view)
                                                .Show();
                        //Check
                        WindowManagerLayoutParams lp = new WindowManagerLayoutParams();
                        Window window = PlotViewDialog.Window;
                        lp.CopyFrom(window.Attributes);
                        //This makes the dialog take up the full width
                        lp.Width = ViewGroup.LayoutParams.MatchParent;
                        lp.Height = ViewGroup.LayoutParams.WrapContent;
                        window.Attributes = lp;
                        //stop checking
                    });

                }, null);

                backButton.Click += OnBackPressed;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        void OnBackPressed(object sender, EventArgs e)
        {
            try
            {
                if (PlotViewDialog != null && PlotViewDialog.IsShowing)
                {
                    PlotViewDialog.Dismiss();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public void ShowAlertThenOpenUrl
        (string message, string urlAddress, string title = "")
        {
            Action okClicked = new Action(() => OpenUrl(urlAddress));

            AlertWithTwoButton
            (message, title, AppResources.download, AppResources.cancel,
             okClicked, null);
        }

        void OpenUrl(string urlAddress)
        {
            if (urlAddress != null)
            {
                var uri = Android.Net.Uri.Parse(urlAddress);
                var intent = new Intent(Intent.ActionView, uri);

                BaseActivity.GetTopActivity().StartActivity(intent);
            }
        }
    }
}