namespace actchargers
{
    public class NewStudyViewModel : SiteViewSettingsBaseViewModel
    {
        public NewStudyViewModel()
        {
            ViewTitle = AppResources.start_new_study;
        }

        internal override void InitController()
        {
            controller = new NewStudyController();
        }

        public override void OnBackButtonClick()
        {
            base.OnBackButtonClick();

            ShowViewModel<NewStudyViewModel>(new { pop = "pop" });
        }
    }
}
