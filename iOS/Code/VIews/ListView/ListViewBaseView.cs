using System;
using Foundation;
using UIKit;

namespace actchargers.iOS
{
    public abstract class ListViewBaseView : EditViewController
    {
        internal ListViewBaseViewModel currentViewModel;

        NSObject _keyboardUp;
        NSObject _keyboardDown;

        protected ListViewBaseView(string nibName, NSBundle bundle)
            : base(nibName, bundle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.

            currentViewModel = ViewModel as ListViewBaseViewModel;

            CheckToChangeEditMode();

            InitData();

            currentViewModel.PropertyChanged += OnPropertyChanged;
        }

        void CheckToChangeEditMode()
        {
            if (currentViewModel is SiteViewSettingsBaseViewModel)
                ChangeEditModeIfSiteView((SiteViewSettingsBaseViewModel)currentViewModel);
        }

        void ChangeEditModeIfSiteView(SiteViewSettingsBaseViewModel siteViewSettingsBaseViewModel)
        {
            if (siteViewSettingsBaseViewModel.IsSiteView)
                ChangeEditMode();
        }

        void ChangeEditMode()
        {
            EditEventHandler(this, EventArgs.Empty);
        }

        internal abstract void InitData();

        public override void BackButton_TouchUpInside(object sender, EventArgs e)
        {
            base.BackButton_TouchUpInside(sender, e);

            currentViewModel.OnBackButtonClick();
        }

        public override void EditEventHandler(object sender, EventArgs e)
        {
            base.EditEventHandler(sender, e);

            currentViewModel.EditCommand.Execute();
        }

        public override void SaveEventHandler(object sender, EventArgs e)
        {
            base.SaveEventHandler(sender, e);

            currentViewModel.SaveCommand.Execute();
        }

        public override void CancelEventHandler(object sender, EventArgs e)
        {
            base.CancelEventHandler(sender, e);

            currentViewModel.BackCommand.Execute();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            _keyboardUp = NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillShowNotification, OnKeyBoardShow);
            _keyboardDown = NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillHideNotification, OnKeyboardHide);
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);

            if (_keyboardUp != null && _keyboardDown != null)
            {
                NSNotificationCenter.DefaultCenter.RemoveObserver(_keyboardUp);
                NSNotificationCenter.DefaultCenter.RemoveObserver(_keyboardDown);
            }
        }

        internal abstract void OnKeyBoardShow(NSNotification obj);

        internal abstract void OnKeyboardHide(NSNotification obj);

        void OnPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("EditingMode"))
                OnEditingModeChanged(currentViewModel.EditingMode);
            else if (e.PropertyName.Equals("ShowEdit"))
                OnShowEditChanged(currentViewModel.ShowEdit);
        }

        void OnEditingModeChanged(bool editingMode)
        {
            if (!editingMode)
                ShowEditIfPossible();
        }

        void ShowEditIfPossible()
        {
            if (currentViewModel.ShowEdit)
                ShowEdit();
        }

        void ShowEdit()
        {
            CreateEditAndBackButton();
        }

        void OnShowEditChanged(bool showEdit)
        {
            if (!showEdit)
                NavigationItem.RightBarButtonItem = null;
        }
    }
}
