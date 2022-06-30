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
    [Register ("ReplacementDeviceItemTableViewCell")]
    partial class ReplacementDeviceItemTableViewCell
    {
        [Outlet]
        UIKit.UILabel customerName { get; set; }


        [Outlet]
        UIKit.UILabel siteName { get; set; }


        [Outlet]
        UIKit.UILabel titleLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (customerName != null) {
                customerName.Dispose ();
                customerName = null;
            }

            if (siteName != null) {
                siteName.Dispose ();
                siteName = null;
            }

            if (titleLabel != null) {
                titleLabel.Dispose ();
                titleLabel = null;
            }
        }
    }
}