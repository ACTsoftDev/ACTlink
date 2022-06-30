namespace actchargers
{
    public class NewStudyController : WithBattViewMobileBaseController
    {
        public NewStudyController() : base(false, true, true)
        {
            subController = new NewStudySubController();
        }
    }
}
