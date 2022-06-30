using System;
using Foundation;
using MvvmCross.Binding.iOS.Views;
using UIKit;

namespace actchargers.iOS
{
    public partial class CustomAlertView : MvxView
    {
        public static int KEYBOARD_HEIGHT = 216;

        public string MessageString, TextString, ButtonText;
        public UIButton ButtonDone;
        public UITextField valueTextField;
        public CustomAlertView(IntPtr h) : base (h)
        {
        }
       
        public override void AwakeFromNib()
        {
            base.AwakeFromNib();

            ButtonDone = doneButton;
            valueTextField = textField;

            BackgroundColor = UIColor.Clear;
            backgroundView.Alpha = (float)0.5;

            NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillShowNotification, OnKeyBoardShown);
            NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillHideNotification, OnKeyBoardHide);
        }

        public void applyText(string message, string text, string buttonText)
        {
            messageLabel.Text = message;
            textField.Text = text;
            doneButton.SetTitle(buttonText, UIControlState.Normal);
        }

        public void OnKeyBoardShown(NSNotification notification)
        {
            var keyboardBounds = (NSValue)notification.UserInfo.ObjectForKey(UIKeyboard.BoundsUserInfoKey);
            var keyboardSize = keyboardBounds.RectangleFValue;
            KEYBOARD_HEIGHT = (int)keyboardSize.Height;

            bottomConstraint.Constant = KEYBOARD_HEIGHT + (((Frame.Size.Height - KEYBOARD_HEIGHT) / 2) - (alertView.Frame.Size.Height/2));
        }

        public void OnKeyBoardHide(NSNotification notification)
        {
            bottomConstraint.Constant = ((this.Frame.Size.Height - KEYBOARD_HEIGHT) / 2) - (alertView.Frame.Size.Height / 2);
        }
    }
}
