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
    [Register ("ButtonTextFieldTableViewCell")]
    partial class ButtonTextFieldTableViewCell
    {
        [Outlet]
        actchargers.iOS.RoundedRectButton actionButton { get; set; }


        [Outlet]
        UIKit.NSLayoutConstraint tfHighlightViewHeightConstraint { get; set; }


        [Outlet]
        UIKit.UITextField valueTextField { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (actionButton != null) {
                actionButton.Dispose ();
                actionButton = null;
            }

            if (tfHighlightViewHeightConstraint != null) {
                tfHighlightViewHeightConstraint.Dispose ();
                tfHighlightViewHeightConstraint = null;
            }

            if (valueTextField != null) {
                valueTextField.Dispose ();
                valueTextField = null;
            }
        }
    }
}