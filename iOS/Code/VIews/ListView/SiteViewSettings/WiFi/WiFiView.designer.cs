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
    [Register ("WiFiView")]
    partial class WiFiView
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView btnView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView listTableView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        actchargers.iOS.RoundedRectButton restoreDefaultsBtn { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint tableViewBottomConstraint { get; set; }

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

            if (restoreDefaultsBtn != null) {
                restoreDefaultsBtn.Dispose ();
                restoreDefaultsBtn = null;
            }

            if (tableViewBottomConstraint != null) {
                tableViewBottomConstraint.Dispose ();
                tableViewBottomConstraint = null;
            }
        }
    }
}