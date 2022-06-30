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
    [Register ("SyncSitesItemTableViewCell")]
    partial class SyncSitesItemTableViewCell
    {
        [Outlet]
        UIKit.UIImageView checkImageview { get; set; }


        [Outlet]
        UIKit.UIButton itemCheckBtn { get; set; }


        [Outlet]
        UIKit.UILabel itemLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (checkImageview != null) {
                checkImageview.Dispose ();
                checkImageview = null;
            }

            if (itemCheckBtn != null) {
                itemCheckBtn.Dispose ();
                itemCheckBtn = null;
            }

            if (itemLabel != null) {
                itemLabel.Dispose ();
                itemLabel = null;
            }
        }
    }
}