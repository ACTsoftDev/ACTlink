using System;
using System.Diagnostics;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Plugins.Email;

namespace actchargers
{
    public class ContactUsViewModel : BaseViewModel
    {
        public string HTMLFileName { get; set; }

        /// <summary>
        /// Init the specified isForContactUs.
        /// </summary>
        /// <param name="isForContactUs">Is for contact us and about us.</param>
        public void Init(string isForContactUs)
        {
            if (isForContactUs == "true")
            {
                ViewTitle = AppResources.contact_us;
                HTMLFileName = "contact_us";
            }
            else
            {
                ViewTitle = AppResources.about_us;
                HTMLFileName = "about_us";
            }
        }
        /// <summary>
        /// Gets the email link click command.
        /// </summary>
        /// <value>The email link click command.</value>
        public IMvxCommand EmailLinkClickCommand
        {
            get { return new MvxCommand(OnEmailLinkClick); }
        }
        /// <summary>
        /// Ons the email link click.
        /// </summary>
        void OnEmailLinkClick()
        {
            try
            {
                Mvx.Resolve<IMvxComposeEmailTask>().ComposeEmail("support@act-view.com");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Logger.AddLog(true, "X26" + ex.ToString());
            }

        }

        /// <summary>
        /// Ons the back button click.
        /// </summary>
        public void OnBackButtonClick()
        {
            ShowViewModel<ContactUsViewModel>(new { pop = "pop" });
        }
    }
}
