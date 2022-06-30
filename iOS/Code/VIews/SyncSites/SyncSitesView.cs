using System;
using MvvmCross.Binding.BindingContext;
using UIKit;

namespace actchargers.iOS
{
    public partial class SyncSitesView : EditViewController
    {
        SyncSitesViewModel currentViewModel;
        public SyncSitesView() : base("SyncSitesView", null)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            UIBarButtonItem saveButton = new UIBarButtonItem(UIBarButtonSystemItem.Save, SaveEventHandler);
            NavigationItem.RightBarButtonItem = saveButton;

            // Perform any additional setup after loading the view, typically from a nib.
            currentViewModel = ViewModel as SyncSitesViewModel;

            listTableView.SeparatorColor = UIColor.Clear;

            listTableView.RegisterNibForCellReuse(SyncSitesItemTableViewCell.Nib, SyncSitesItemTableViewCell.Key);
            listTableView.RegisterNibForCellReuse(SyncSitesHeaderTableViewCell.Nib, SyncSitesHeaderTableViewCell.Key);


            var source = new SyncSitesTableViewSource(listTableView, null);
            this.CreateBinding(source).For(s => s.ListItemsSource).To((SyncSitesViewModel vm) => vm.ListItemSource).Apply();
            listTableView.Source = source;
            ListTableManager.SetHeight(listTableView, 60);


        }

        public override void SaveEventHandler(object sender, EventArgs e)
        {
            base.SaveEventHandler(sender, e);
            currentViewModel.SaveBtnClickCommand.Execute();
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        public override void BackButton_TouchUpInside(object sender, EventArgs e)
        {
            currentViewModel.OnBackButtonClick();

        }


    }
}

