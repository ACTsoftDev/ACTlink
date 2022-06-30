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
    [Register ("SettingsView")]
    partial class SettingsView
    {
        [Outlet]
        UIKit.UITableView listTableView { get; set; }


        [Outlet]
        actchargers.iOS.RoundedRectButton resetLCDCalibrationButton { get; set; }


        [Outlet]
        UIKit.UIView resetLCDView { get; set; }


        [Outlet]
        UIKit.NSLayoutConstraint resetViewHeightConstraint { get; set; }


        [Outlet]
        UIKit.NSLayoutConstraint tableViewBottomConstraint { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (listTableView != null) {
                listTableView.Dispose ();
                listTableView = null;
            }

            if (resetLCDCalibrationButton != null) {
                resetLCDCalibrationButton.Dispose ();
                resetLCDCalibrationButton = null;
            }

            if (resetLCDView != null) {
                resetLCDView.Dispose ();
                resetLCDView = null;
            }

            if (resetViewHeightConstraint != null) {
                resetViewHeightConstraint.Dispose ();
                resetViewHeightConstraint = null;
            }

            if (tableViewBottomConstraint != null) {
                tableViewBottomConstraint.Dispose ();
                tableViewBottomConstraint = null;
            }
        }
    }
}