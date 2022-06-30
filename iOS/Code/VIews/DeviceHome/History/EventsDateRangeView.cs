using System;
using Foundation;
using MvvmCross.Binding.BindingContext;
using UIKit;

namespace actchargers.iOS
{
    public partial class EventsDateRangeView : BackViewController
    {

        private NSObject _keyboardUp;
        private NSObject _keyboardDown;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:actchargers.iOS.EventsDateRangeView"/> class.
        /// </summary>
        public EventsDateRangeView() : base("EventsDateRangeView", null)
        {
        }

        /// <summary>
        /// Views the did load.
        /// </summary>
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.
			groupTableView.RegisterNibForCellReuse(ThreeLabelTableViewCell.Nib, ThreeLabelTableViewCell.Key);
            groupTableView.RegisterNibForCellReuse(DatePickerTableViewCell.Nib, DatePickerTableViewCell.Key);
			groupTableView.RegisterNibForCellReuse(ButtonTableViewCell.Nib, ButtonTableViewCell.Key);
            groupTableView.RegisterNibForCellReuse(LabelLabelTableViewCell.Nib, LabelLabelTableViewCell.Key);

            var source = new GroupItemTableViewSource(groupTableView, null);
			this.CreateBinding(source).For(s => s.ListItemsSource).To((EventsDateRangeViewModel vm) => vm.EventsDateRangeViewItemSource).Apply();
			this.CreateBinding(source).For(s => s.SelectionChangedCommand).To((EventsDateRangeViewModel vm) => vm.ItemClickCommand).Apply();
			groupTableView.SeparatorColor = UIColor.Clear;
			groupTableView.Source = source;
			groupTableView.EstimatedRowHeight = 60;
			groupTableView.ContentInset = new UIEdgeInsets(-18, 0, 0, 0);
            groupTableView.KeyboardDismissMode = UIScrollViewKeyboardDismissMode.OnDrag;
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
            tableBottomConstraint.Constant = KEYBOARD_HEIGHT;
        }

        /// <summary>
        /// Ons the keyboard hide.
        /// </summary>
        /// <param name="notification">Notification.</param>
        public void OnKeyboardHide(NSNotification notification)
        {
            tableBottomConstraint.Constant = 0;
        }
    }
}

