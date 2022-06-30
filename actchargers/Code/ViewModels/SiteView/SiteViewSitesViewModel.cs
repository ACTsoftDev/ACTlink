using System.Collections.ObjectModel;
using System.Windows.Input;
using MvvmCross.Core.ViewModels;

namespace actchargers
{
    public class SiteViewSitesViewModel : BaseViewModel
    {
        SiteViewSitesController siteViewSitesController;

        public SiteViewSitesViewModel()
        {
            ViewTitle = AppResources.sites;

            InitItems();
        }

        void InitItems()
        {
            siteViewSitesController = new SiteViewSitesController();

            ListItemSource = siteViewSitesController.ListItemSource;
        }

        ObservableCollection<SynchSiteObjects> listItemSource;
        public ObservableCollection<SynchSiteObjects> ListItemSource
        {
            get { return listItemSource; }
            set
            {
                SetProperty(ref listItemSource, value);
            }
        }

        public ICommand SelectItemCommand
        {
            get
            {
                return new MvxCommand<SynchSiteObjects>(ExecuteSelectItemCommand);
            }
        }

        void ExecuteSelectItemCommand(SynchSiteObjects item)
        {
            ShowViewModel<SiteViewDevicesViewModel>(
                new
                {
                    siteId = item.SiteId,
                    isWithSynchSites = true
                });
        }

        public override void OnBackButtonClick()
        {
            base.OnBackButtonClick();

            ShowViewModel<SiteViewSitesViewModel>(new { pop = "pop" });
        }
    }
}
