// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace actchargers.iOS
{
    [Register ("CustomAlertView")]
    partial class CustomAlertView
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView alertView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView backgroundView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint bottomConstraint { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        actchargers.iOS.RoundedRectButton doneButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel messageLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField textField { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (alertView != null) {
                alertView.Dispose ();
                alertView = null;
            }

            if (backgroundView != null) {
                backgroundView.Dispose ();
                backgroundView = null;
            }

            if (bottomConstraint != null) {
                bottomConstraint.Dispose ();
                bottomConstraint = null;
            }

            if (doneButton != null) {
                doneButton.Dispose ();
                doneButton = null;
            }

            if (messageLabel != null) {
                messageLabel.Dispose ();
                messageLabel = null;
            }

            if (textField != null) {
                textField.Dispose ();
                textField = null;
            }
        }
    }
}