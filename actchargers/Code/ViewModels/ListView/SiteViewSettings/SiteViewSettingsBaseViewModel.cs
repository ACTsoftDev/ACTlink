namespace actchargers
{
    public abstract class SiteViewSettingsBaseViewModel : ListViewBaseViewModel
    {
        bool isSiteView;
        public bool IsSiteView
        {
            get
            {
                return isSiteView;
            }
            set
            {
                SetProperty(ref isSiteView, value);
            }
        }

        public void Init(bool isSiteView)
        {
            IsSiteView = isSiteView;
        }
    }
}
