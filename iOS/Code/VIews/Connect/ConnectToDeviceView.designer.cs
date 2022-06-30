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
    [Register ("ConnectToDeviceView")]
    partial class ConnectToDeviceView
    {
        [Outlet]
        UIKit.UIActivityIndicatorView activityIndicator { get; set; }


        [Outlet]
        UIKit.UITableView listTableView { get; set; }


        [Outlet]
        UIKit.UIView progressView { get; set; }


        [Outlet]
        UIKit.UILabel scanningMessageLbl { get; set; }


        [Outlet]
        UIKit.UISegmentedControl segmentController { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (activityIndicator != null) {
                activityIndicator.Dispose ();
                activityIndicator = null;
            }

            if (listTableView != null) {
                listTableView.Dispose ();
                listTableView = null;
            }

            if (progressView != null) {
                progressView.Dispose ();
                progressView = null;
            }

            if (scanningMessageLbl != null) {
                scanningMessageLbl.Dispose ();
                scanningMessageLbl = null;
            }

            if (segmentController != null) {
                segmentController.Dispose ();
                segmentController = null;
            }
        }
    }
}