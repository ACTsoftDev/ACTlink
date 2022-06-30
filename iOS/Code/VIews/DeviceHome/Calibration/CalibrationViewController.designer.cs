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
    [Register ("CalibrationViewController")]
    partial class CalibrationViewController
    {
        [Outlet]
        UIKit.UITableView currentTableView { get; set; }


        [Outlet]
        actchargers.iOS.RoundedRectButton resetButton { get; set; }


        [Outlet]
        UIKit.UIView resetView { get; set; }


        [Outlet]
        UIKit.NSLayoutConstraint resetViewHeight { get; set; }


        [Outlet]
        UIKit.UISegmentedControl segmentcontroller { get; set; }


        [Outlet]
        UIKit.UITableView socTableView { get; set; }


        [Outlet]
        UIKit.NSLayoutConstraint tableViewBottomConstraint { get; set; }


        [Outlet]
        UIKit.UITableView voltageTableView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (currentTableView != null) {
                currentTableView.Dispose ();
                currentTableView = null;
            }

            if (segmentcontroller != null) {
                segmentcontroller.Dispose ();
                segmentcontroller = null;
            }

            if (socTableView != null) {
                socTableView.Dispose ();
                socTableView = null;
            }

            if (voltageTableView != null) {
                voltageTableView.Dispose ();
                voltageTableView = null;
            }
        }
    }
}