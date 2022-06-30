using System;
using System.Diagnostics;
using CoreGraphics;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using MvvmCross.Core.ViewModels;
using MvvmCross.iOS.Views;
using UIKit;

namespace actchargers.iOS
{
    public partial class ConnectToDeviceView : BackViewController
    {
        ConnectToDeviceViewModel currentViewModel;
        private NSObject _foreground;
        public ConnectToDeviceView() : base("ConnectToDeviceView", null)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            currentViewModel = ViewModel as ConnectToDeviceViewModel;

            //COLORS
            segmentController.TintColor = UIColorUtility.FromHex(ACColors.DARK_BLUE_COLOR);
            progressView.BackgroundColor = UIColorUtility.FromHex(ACColors.DARK_BLUE_COLOR);
            //progressIndicator.ProgressTintColor = UIColorUtility.FromHex(ACColors.WHITE_COLOR);
            //progressIndicator.TrackTintColor = UIColorUtility.FromHex(ACColors.PROGRESS_TRACK_COLOR);

            UIButton menuButton = new UIButton(new CoreGraphics.CGRect(0, 0, 19, 17));
            menuButton.SetImage(UIImage.FromBundle("dots.png"), UIControlState.Normal);
            menuButton.ShowsTouchWhenHighlighted = true;
            menuButton.TouchUpInside += delegate
            {
                (UIApplication.SharedApplication.Delegate as AppDelegate).SideBar.ToggleMenu();
            };
            var menuItem = new UIBarButtonItem(menuButton);
            NavigationItem.RightBarButtonItem = menuItem;

            // Perform any additional setup after loading the view, typically from a nib.
            //this.CreateBinding(progressIndicator).For(ob => ob.Progress).To((ConnectToDeviceViewModel vm) => vm.ProgressCompletedIOS);
            //this.CreateBinding(progressIndicator).To((ConnectToDeviceViewModel vm) => vm.ProgressCompletedIOS).Apply();
            //this.CreateBinding(scanBtn).For(o => o.Hidden).To((ConnectToDeviceViewModel vm) => vm.ScanBtnVisibilityIOS).Apply();
            //this.CreateBinding(scanBtn).To((ConnectToDeviceViewModel vm) => vm.ScantBtnClicked).Apply();
            //this.CreateBinding(scanBtn).For("Title").To((ConnectToDeviceViewModel vm) => vm.ScanBtnText).Apply();
            this.CreateBinding(activityIndicator).For(ai => ai.Hidden).To((ConnectToDeviceViewModel vm) => vm.ScanBtnVisibility).Apply();
             this.CreateBinding(scanningMessageLbl).For(Lbl => Lbl.Hidden).To((ConnectToDeviceViewModel vm) => vm.ScanBtnVisibility).Apply();
            this.CreateBinding(scanningMessageLbl).To((ConnectToDeviceViewModel vm) => vm.ScanStatusMessage).Apply();

            var source = new MvxSimpleTableViewSource(listTableView, "DeviceTableViewCell");
            this.CreateBinding(source).For(s => s.ItemsSource).To((ConnectToDeviceViewModel vm) => vm.ListItemSource).Apply();
            this.CreateBinding(source).For(s => s.SelectionChangedCommand).To((ConnectToDeviceViewModel vm) => vm.SelectItemCommand).Apply();
            listTableView.Source = source;
            this.CreateBinding(segmentController).For(o => o.SelectedSegment).To((ConnectToDeviceViewModel vm) => vm.SelectedTabIndex).Apply();
            //progressIndicator.Progress = 0;
            ListTableManager.SetHeight(listTableView, 60);
            UIView footerView = new UIView(new CGRect(0, 0, listTableView.Frame.Size.Width, 1));
            listTableView.TableFooterView = footerView;
            listTableView.SeparatorColor = UIColor.Clear;

            ((MvxNotifyPropertyChanged)this.ViewModel).PropertyChanged += OnPropertyChanged;

            segmentController.ApportionsSegmentWidthsByContent = true;
        }

        void OnPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("BattViewCount"))
            {
                segmentController.SetTitle("BATTView (" + currentViewModel.BattViewCount.ToString() + ")", 0);
            }
            else if (e.PropertyName.Equals("Chargerscount"))
            {
                segmentController.SetTitle("Chargers (" + currentViewModel.Chargerscount.ToString() + ")", 1);
            }
            else if (e.PropertyName.Equals("ReplacementCount"))
            {
                segmentController.SetTitle("Replacements (" + currentViewModel.ReplacementCount.ToString() + ")", 2);
            }
            else if (e.PropertyName.Equals("ScanBtnVisibility"))
            {
                if(currentViewModel.ScanBtnVisibility)
                {
                    activityIndicator.StopAnimating();
                }
                else
                {
                    activityIndicator.StartAnimating();
                }
            }
            segmentController.ApportionsSegmentWidthsByContent = true;
        }

        public override void BackButton_TouchUpInside(object sender, EventArgs e)
        {
            base.BackButton_TouchUpInside(sender, e);
            currentViewModel.OnBackButtonClick();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            _foreground = NSNotificationCenter.DefaultCenter.AddObserver(UIApplication.WillEnterForegroundNotification, HandleAction);
        }

        void HandleAction(NSNotification obj)
        {
            try
            {
                currentViewModel.OnForeground();
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            if (_foreground != null)
            {
                NSNotificationCenter.DefaultCenter.RemoveObserver(_foreground);
            }
        }
    }
}