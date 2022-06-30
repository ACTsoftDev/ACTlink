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
    [Register ("DeviceTableViewCell")]
    partial class DeviceTableViewCell
    {
        [Outlet]
        UIKit.UIImageView SelectedImageView { get; set; }


        [Outlet]
        UIKit.UILabel subTitleLbl { get; set; }


        [Outlet]
        UIKit.UILabel titleLbl { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (SelectedImageView != null) {
                SelectedImageView.Dispose ();
                SelectedImageView = null;
            }

            if (subTitleLbl != null) {
                subTitleLbl.Dispose ();
                subTitleLbl = null;
            }

            if (titleLbl != null) {
                titleLbl.Dispose ();
                titleLbl = null;
            }
        }
    }
}