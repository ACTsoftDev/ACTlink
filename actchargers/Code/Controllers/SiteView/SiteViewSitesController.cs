using System.Collections.ObjectModel;

namespace actchargers
{
    public class SiteViewSitesController : BaseController
    {
        public SiteViewSitesController()
        {
            Init();
        }

        void Init()
        {
            var list =
                DbSingleton
                    .DBManagerServiceInstance
                    .GetSynchSiteObjectsLoader()
                    .GetAll();

            ListItemSource = new ObservableCollection<SynchSiteObjects>(list);
        }

        public ObservableCollection<SynchSiteObjects> ListItemSource
        {
            get;
            set;
        }
    }
}
