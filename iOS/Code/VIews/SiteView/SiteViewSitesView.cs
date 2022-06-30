using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using UIKit;

namespace actchargers.iOS
{
    public partial class SiteViewSitesView : BackViewController
    {
        SiteViewSitesViewModel currentViewModel;

        public SiteViewSitesView() : base("SiteViewSitesView", null)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            currentViewModel = ViewModel as SiteViewSitesViewModel;

            listTableView.SeparatorColor = UIColor.Clear;

            listTableView.RegisterNibForCellReuse(SiteViewSitesTableViewCell.Nib, SiteViewSitesTableViewCell.Key);

            var source = new MvxSimpleTableViewSource(listTableView, "SiteViewSitesTableViewCell");
            this.CreateBinding(source).For(s => s.ItemsSource).To((SiteViewSitesViewModel vm) => vm.ListItemSource).Apply();
            this.CreateBinding(source).For(s => s.SelectionChangedCommand).To((SiteViewSitesViewModel vm) => vm.SelectItemCommand).Apply();

            listTableView.Source = source;
            ListTableManager.SetHeight(listTableView, 60, 60);
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        public override void BackButton_TouchUpInside(object sender, System.EventArgs e)
        {
            base.BackButton_TouchUpInside(sender, e);

            currentViewModel.OnBackButtonClick();
        }
    }
}

