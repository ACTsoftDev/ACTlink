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
    [Register ("GlobalRecordsCollectionViewCell")]
    partial class GlobalRecordsCollectionViewCell
    {
        [Outlet]
        UIKit.UILabel subTitle { get; set; }


        [Outlet]
        UIKit.UILabel subTitle2 { get; set; }


        [Outlet]
        UIKit.UILabel titleLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (subTitle != null) {
                subTitle.Dispose ();
                subTitle = null;
            }

            if (subTitle2 != null) {
                subTitle2.Dispose ();
                subTitle2 = null;
            }

            if (titleLabel != null) {
                titleLabel.Dispose ();
                titleLabel = null;
            }
        }
    }
}