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
    [Register ("SyncSitesHeaderView")]
    partial class SyncSitesHeaderView
    {
        [Outlet]
        UIKit.UIButton checkBtn { get; set; }


        [Outlet]
        UIKit.UIButton expandBtn { get; set; }


        [Outlet]
        UIKit.UILabel HeaderTitle { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (checkBtn != null) {
                checkBtn.Dispose ();
                checkBtn = null;
            }

            if (expandBtn != null) {
                expandBtn.Dispose ();
                expandBtn = null;
            }

            if (HeaderTitle != null) {
                HeaderTitle.Dispose ();
                HeaderTitle = null;
            }
        }
    }
}