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
    [Register ("UploadItemTableViewCell")]
    partial class UploadItemTableViewCell
    {
        [Outlet]
        UIKit.UILabel deviceName { get; set; }


        [Outlet]
        UIKit.UILabel statusLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (deviceName != null) {
                deviceName.Dispose ();
                deviceName = null;
            }

            if (statusLabel != null) {
                statusLabel.Dispose ();
                statusLabel = null;
            }
        }
    }
}