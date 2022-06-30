namespace actchargers
{
    public class AboutUsViewModel : BaseViewModel
    {
        AboutUsController aboutUsController;

        public AboutUsViewModel()
        {
            ViewTitle = AppResources.about_us;

            aboutUsController = new AboutUsController();
        }

        public string AppVersionWithTitle
        {
            get
            {
                return aboutUsController.AppVersionWithTitle;
            }
        }

        public string McbVersionWithTitle
        {
            get
            {
                return aboutUsController.McbVersionWithTitle;
            }
        }

        public string BattViewVersionWithTitle
        {
            get
            {
                return aboutUsController.BattViewVersionWithTitle;
            }
        }

        public string CalibratorVersionWithTitle
        {
            get
            {
                return aboutUsController.CalibratorVersionWithTitle;
            }
        }

        public string CopyrightVersionWithTitle
        {
            get
            {
                return aboutUsController.CopyrightVersionWithTitle;
            }
        }

        public string AboutAct
        {
            get
            {
                return aboutUsController.AboutAct;
            }
        }

        public string PoweredTitle
        {
            get
            {
                return aboutUsController.PoweredTitle;
            }
        }

        public override void OnBackButtonClick()
        {
            base.OnBackButtonClick();

            ShowViewModel<AboutUsViewModel>(new { pop = "pop" });
        }
    }
}
