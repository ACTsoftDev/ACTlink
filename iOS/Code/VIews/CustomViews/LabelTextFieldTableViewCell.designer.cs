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
    [Register ("LabelTextFieldTableViewCell")]
    partial class LabelTextFieldTableViewCell
    {
        [Outlet]
        UIKit.UIView textFieldHighlightView { get; set; }


        [Outlet]
        UIKit.NSLayoutConstraint tfHighlightViewHeightConstraint { get; set; }


        [Outlet]
        UIKit.UILabel titleLabel { get; set; }


        [Outlet]
        UIKit.UITextField valueTextField { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (textFieldHighlightView != null) {
                textFieldHighlightView.Dispose ();
                textFieldHighlightView = null;
            }

            if (tfHighlightViewHeightConstraint != null) {
                tfHighlightViewHeightConstraint.Dispose ();
                tfHighlightViewHeightConstraint = null;
            }

            if (titleLabel != null) {
                titleLabel.Dispose ();
                titleLabel = null;
            }

            if (valueTextField != null) {
                valueTextField.Dispose ();
                valueTextField = null;
            }
        }
    }
}