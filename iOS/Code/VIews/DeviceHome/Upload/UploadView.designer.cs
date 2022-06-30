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
    [Register ("UploadView")]
    partial class UploadView
    {
        [Outlet]
        UIKit.UITableView listTableView { get; set; }


        [Outlet]
        UIKit.UIProgressView progressIndicator { get; set; }


        [Outlet]
        UIKit.UISegmentedControl segmentController { get; set; }


        [Outlet]
        UIKit.UILabel statusText { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (listTableView != null) {
                listTableView.Dispose ();
                listTableView = null;
            }

            if (progressIndicator != null) {
                progressIndicator.Dispose ();
                progressIndicator = null;
            }

            if (segmentController != null) {
                segmentController.Dispose ();
                segmentController = null;
            }

            if (statusText != null) {
                statusText.Dispose ();
                statusText = null;
            }
        }
    }
}