using System;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Core.ViewModels;
using UIKit;

namespace actchargers.iOS
{
    public partial class EditEventControlView : EditViewController
    {
        EditEventControlViewModel currentViewModel;

        private NSObject _keyboardUp;
        private NSObject _keyboardDown;

        public EditEventControlView() : base("EditEventControlView", null)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.
            currentViewModel = ViewModel as EditEventControlViewModel;

            listTableView.RegisterNibForCellReuse(LabelLabelTableViewCell.Nib, LabelLabelTableViewCell.Key);
            listTableView.RegisterNibForCellReuse(LabelTextFieldTableViewCell.Nib, LabelTextFieldTableViewCell.Key);
            listTableView.RegisterNibForCellReuse(ListSelectorTabelViewCell.Nib, ListSelectorTabelViewCell.Key);

            var source = new GroupItemTableViewSource(listTableView, null);
            this.CreateBinding(source).For(s => s.ListItemsSource).To((EditEventControlViewModel vm) => vm.EventControlItemSource).Apply();
            listTableView.Source = source;
            listTableView.SeparatorColor = UIColor.Clear;
            ListTableManager.SetHeight(listTableView, 60);
            listTableView.KeyboardDismissMode = UIScrollViewKeyboardDismissMode.OnDrag;

            ((MvxNotifyPropertyChanged)this.ViewModel).PropertyChanged += OnPropertyChanged;
        }

        /// <summary>
        /// Backs the button touch up inside.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        public override void BackButton_TouchUpInside(object sender, EventArgs e)
        {
            base.BackButton_TouchUpInside(sender, e);
            this.NavigationController.PopViewController(true);
        }

        /// <summary>
        /// Edits the event handler.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        public override void EditEventHandler(object sender, EventArgs e)
        {
            base.EditEventHandler(sender, e);
            currentViewModel.EditBtnClickCommand.Execute();
        }

        /// <summary>
        /// Saves the event handler.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        public override void SaveEventHandler(object sender, EventArgs e)
        {
            base.SaveEventHandler(sender, e);
            currentViewModel.SaveBtnClickCommand.Execute();
        }

        /// <summary>
        /// Cancels the event handler.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        public override void CancelEventHandler(object sender, EventArgs e)
        {
            base.CancelEventHandler(sender, e);
            currentViewModel.BackBtnClickCommand.Execute();
        }

        /// <summary>
        /// Views the will appear.
        /// </summary>
        /// <param name="animated">If set to <c>true</c> animated.</param>
        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            _keyboardUp = NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillShowNotification, OnKeyBoardShow);
            _keyboardDown = NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillHideNotification, OnKeyboardHide);
        }

        /// <summary>
        /// Views the will disappear.
        /// </summary>
        /// <param name="animated">If set to <c>true</c> animated.</param>
        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);

            if (_keyboardUp != null && _keyboardDown != null)
            {
                NSNotificationCenter.DefaultCenter.RemoveObserver(_keyboardUp);
                NSNotificationCenter.DefaultCenter.RemoveObserver(_keyboardDown);
            }
        }

        /// <summary>
        /// Ons the key board show.
        /// </summary>
        /// <param name="notification">Notification.</param>
        public void OnKeyBoardShow(NSNotification notification)
        {
            tableViewBottomConstraint.Constant = KEYBOARD_HEIGHT;
        }

        /// <summary>
        /// Ons the keyboard hide.
        /// </summary>
        /// <param name="notification">Notification.</param>
        public void OnKeyboardHide(NSNotification notification)
        {
            tableViewBottomConstraint.Constant = 0;
        }

        /// <summary>
        /// Ons the property changed.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        void OnPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("EditingMode"))
            {
                if (!(ViewModel as EditEventControlViewModel).EditingMode)
                {
                    CreateEditAndBackButton();
                }
            }
        }
    }
}

