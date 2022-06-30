// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace actchargers.iOS
{
    [Register ("SiteViewDevicesView")]
    partial class SiteViewDevicesView
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView checkBoxContainer { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton checkBtn { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView checkImage { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView listTableView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UISegmentedControl segmentController { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView selectAllContainer { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel selectAllTitle { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (checkBoxContainer != null) {
                checkBoxContainer.Dispose ();
                checkBoxContainer = null;
            }

            if (checkBtn != null) {
                checkBtn.Dispose ();
                checkBtn = null;
            }

            if (checkImage != null) {
                checkImage.Dispose ();
                checkImage = null;
            }

            if (listTableView != null) {
                listTableView.Dispose ();
                listTableView = null;
            }

            if (segmentController != null) {
                segmentController.Dispose ();
                segmentController = null;
            }

            if (selectAllContainer != null) {
                selectAllContainer.Dispose ();
                selectAllContainer = null;
            }

            if (selectAllTitle != null) {
                selectAllTitle.Dispose ();
                selectAllTitle = null;
            }
        }
    }
}