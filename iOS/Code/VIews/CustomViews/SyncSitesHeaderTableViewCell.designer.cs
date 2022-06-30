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
    [Register ("SyncSitesHeaderTableViewCell")]
    partial class SyncSitesHeaderTableViewCell
    {
        [Outlet]
        UIKit.UIButton checkBtn { get; set; }


        [Outlet]
        UIKit.UIImageView checkImageView { get; set; }


        [Outlet]
        UIKit.UIButton expandBtn { get; set; }


        [Outlet]
        UIKit.UIImageView expandImageView { get; set; }


        [Outlet]
        UIKit.UILabel HeaderTitle { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (checkBtn != null) {
                checkBtn.Dispose ();
                checkBtn = null;
            }

            if (checkImageView != null) {
                checkImageView.Dispose ();
                checkImageView = null;
            }

            if (expandBtn != null) {
                expandBtn.Dispose ();
                expandBtn = null;
            }

            if (expandImageView != null) {
                expandImageView.Dispose ();
                expandImageView = null;
            }

            if (HeaderTitle != null) {
                HeaderTitle.Dispose ();
                HeaderTitle = null;
            }
        }
    }
}