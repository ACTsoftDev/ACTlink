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
    [Register ("DefaultChargeProfileView")]
    partial class DefaultChargeProfileView
    {
        [Outlet]
        UIKit.UIView btnView { get; set; }


        [Outlet]
        UIKit.UITableView listTableView { get; set; }


        [Outlet]
        UIKit.NSLayoutConstraint listTableViewBottomConstraint { get; set; }


        [Outlet]
        actchargers.iOS.RoundedRectButton loadDefaultsButton { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (btnView != null) {
                btnView.Dispose ();
                btnView = null;
            }

            if (listTableView != null) {
                listTableView.Dispose ();
                listTableView = null;
            }

            if (listTableViewBottomConstraint != null) {
                listTableViewBottomConstraint.Dispose ();
                listTableViewBottomConstraint = null;
            }

            if (loadDefaultsButton != null) {
                loadDefaultsButton.Dispose ();
                loadDefaultsButton = null;
            }
        }
    }
}