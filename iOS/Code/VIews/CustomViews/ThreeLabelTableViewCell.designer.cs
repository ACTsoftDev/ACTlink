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
    [Register ("ThreeLabelTableViewCell")]
    partial class ThreeLabelTableViewCell
    {
        [Outlet]
        UIKit.UILabel title2Label { get; set; }


        [Outlet]
        UIKit.UILabel title3Label { get; set; }


        [Outlet]
        UIKit.UILabel titleLabel { get; set; }


        [Outlet]
        UIKit.UILabel value2Label { get; set; }


        [Outlet]
        UIKit.UILabel value3Label { get; set; }


        [Outlet]
        UIKit.UILabel valueLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (title2Label != null) {
                title2Label.Dispose ();
                title2Label = null;
            }

            if (title3Label != null) {
                title3Label.Dispose ();
                title3Label = null;
            }

            if (titleLabel != null) {
                titleLabel.Dispose ();
                titleLabel = null;
            }

            if (value2Label != null) {
                value2Label.Dispose ();
                value2Label = null;
            }

            if (value3Label != null) {
                value3Label.Dispose ();
                value3Label = null;
            }

            if (valueLabel != null) {
                valueLabel.Dispose ();
                valueLabel = null;
            }
        }
    }
}