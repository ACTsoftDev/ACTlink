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
    [Register ("PmLiveView")]
    partial class PmLiveView
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
        UIKit.UILabel PmStateTitle { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel PmVoltageTitle { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel RatingTitle { get; set; }

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

            if (PmStateTitle != null) {
                PmStateTitle.Dispose ();
                PmStateTitle = null;
            }

            if (PmVoltageTitle != null) {
                PmVoltageTitle.Dispose ();
                PmVoltageTitle = null;
            }

            if (RatingTitle != null) {
                RatingTitle.Dispose ();
                RatingTitle = null;
            }
        }
    }
}