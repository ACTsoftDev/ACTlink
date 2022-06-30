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
    [Register ("SiteViewSitesTableViewCell")]
    partial class SiteViewSitesTableViewCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel customerNameLbl { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel siteNameLbl { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (customerNameLbl != null) {
                customerNameLbl.Dispose ();
                customerNameLbl = null;
            }

            if (siteNameLbl != null) {
                siteNameLbl.Dispose ();
                siteNameLbl = null;
            }
        }
    }
}