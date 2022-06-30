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
    [Register ("LabelLabelTableViewCell")]
    partial class LabelLabelTableViewCell
    {
        [Outlet]
        UIKit.UILabel titleLabel { get; set; }


        [Outlet]
        UIKit.UILabel valueLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (titleLabel != null) {
                titleLabel.Dispose ();
                titleLabel = null;
            }

            if (valueLabel != null) {
                valueLabel.Dispose ();
                valueLabel = null;
            }
        }
    }
}