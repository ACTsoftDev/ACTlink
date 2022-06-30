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
    [Register ("PowerSnapshotsViewController")]
    partial class PowerSnapshotsView
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel CurrentTitle { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel IdTitle { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView listTableView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel PowerTitle { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton ReadRecordsButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView ReadRecordsView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField StartFrom { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel StartFromTitle { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel TimeTitle { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel VoltageTitle { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (CurrentTitle != null) {
                CurrentTitle.Dispose ();
                CurrentTitle = null;
            }

            if (IdTitle != null) {
                IdTitle.Dispose ();
                IdTitle = null;
            }

            if (listTableView != null) {
                listTableView.Dispose ();
                listTableView = null;
            }

            if (PowerTitle != null) {
                PowerTitle.Dispose ();
                PowerTitle = null;
            }

            if (ReadRecordsButton != null) {
                ReadRecordsButton.Dispose ();
                ReadRecordsButton = null;
            }

            if (ReadRecordsView != null) {
                ReadRecordsView.Dispose ();
                ReadRecordsView = null;
            }

            if (StartFrom != null) {
                StartFrom.Dispose ();
                StartFrom = null;
            }

            if (StartFromTitle != null) {
                StartFromTitle.Dispose ();
                StartFromTitle = null;
            }

            if (TimeTitle != null) {
                TimeTitle.Dispose ();
                TimeTitle = null;
            }

            if (VoltageTitle != null) {
                VoltageTitle.Dispose ();
                VoltageTitle = null;
            }
        }
    }
}