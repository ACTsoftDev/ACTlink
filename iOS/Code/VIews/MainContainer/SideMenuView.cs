using System;
using actchargers.ViewModels;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using MvvmCross.Core.ViewModels;
using MvvmCross.iOS.Views;
using MvvmCross.Platform;
using UIKit;

namespace actchargers.iOS
{
    public partial class SideMenuView : MvxViewController
    {
        private MvxImageViewLoader userImageLoader;
        private MvxImageViewLoader logoutImageLoader;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:actchargers.iOS.SideMenuView"/> class.
        /// </summary>
        public SideMenuView() : base("SideMenuView", null)
        {
        }

        /// <summary>
        /// Views the did load.
        /// </summary>
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.

            View.BackgroundColor = UIColorUtility.FromHex(ACColors.BLACK_COLOR);

            NSString CellIdentifier = new NSString("UITableViewCell");
            /// MvxImageViewLoader for userImageView
            userImageLoader = new MvxImageViewLoader(() => userImageView);
            this.CreateBinding(userImageLoader).For(s => s.ImageUrl).To((SideMenuViewModel vm) => vm.UserNameImageName).WithConversion("ImageName", 1).Apply();
            /// MvxImageViewLoader for logoutImageView
            logoutImageLoader = new MvxImageViewLoader(() => logoutImageView);
            this.CreateBinding(logoutImageLoader).For(s => s.ImageUrl).To((SideMenuViewModel vm) => vm.LogoutImageName).WithConversion("ImageName", 1).Apply();
           
            this.CreateBinding(userNameLbl).To((SideMenuViewModel vm) => vm.UserName).Apply();
            this.CreateBinding(userEmailLbl).To((SideMenuViewModel vm) => vm.EmailId).Apply();
            this.CreateBinding(logoutLbl).To((SideMenuViewModel vm) => vm.LogoutTitle).Apply();

            ///Binding SideMenuTableViewSource
            var source = new SideMenuTableViewSource(listTableView, CellIdentifier);
            this.CreateBinding(source).For(s => s.TableItemsSource).To((SideMenuViewModel vm) => vm.HomeScreenMenuItems).Apply();
            this.CreateBinding(source).For(s => s.SelectionChangedCommand).To((SideMenuViewModel vm) => vm.SelectMenuItemCommand).Apply();
            listTableView.Source = source;

            /// <summary>
            /// TouchUpInside for logoutBtn
            /// </summary>
            logoutBtn.TouchUpInside += delegate
            {
                SideMenuViewModel currentViewModel = ViewModel as SideMenuViewModel;
                currentViewModel.LogoutBtnClicked.Execute();
            };
            listTableView.SeparatorColor = UIColorUtility.FromHex(ACColors.SIDE_MENU_SEPERATOR_COLOR);
            listTableView.BackgroundColor = UIColor.Clear;

            ((MvxNotifyPropertyChanged)this.ViewModel).PropertyChanged += OnPropertyChanged;
        }

        /// <summary>
        /// Ons the property changed.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        void OnPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("LogoutClicked"))
            {
                if ((ViewModel as SideMenuViewModel).LogoutClicked)
                {
                    ///Implementing logout
                    ///Using ICustomPresenter making Host as Login
                    var customPresenter = Mvx.Resolve<ICustomPresenter>();
                    customPresenter.Register(null);
                }
            }
        }
    }
}