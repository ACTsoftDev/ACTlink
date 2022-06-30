using System;
using Foundation;
using MvvmCross.Binding.BindingContext;
using UIKit;

namespace actchargers.iOS
{
    public partial class CommissionAReplacementPartView : BackViewController
    {
        private NSObject _keyboardUp;
        private NSObject _keyboardDown;

        public CommissionAReplacementPartView() : base("CommissionAReplacementPartView", null)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.

            listTableView.RegisterNibForCellReuse(ButtonTableViewCell.Nib, ButtonTableViewCell.Key);
            listTableView.RegisterNibForCellReuse(LabelTextFieldTableViewCell.Nib, LabelTextFieldTableViewCell.Key);

            var source = new PlainItemTableViewSource(listTableView, null);
            this.CreateBinding(source).For(s => s.ListItemsSource).To((CommissionAReplacementPartViewModel vm) => vm.ReplacementPartItemSource).Apply();
            listTableView.Source = source;
            listTableView.SeparatorColor = UIColor.Clear;
            ListTableManager.SetHeight(listTableView, 60);
            listTableView.KeyboardDismissMode = UIScrollViewKeyboardDismissMode.OnDrag;
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
    }
}

