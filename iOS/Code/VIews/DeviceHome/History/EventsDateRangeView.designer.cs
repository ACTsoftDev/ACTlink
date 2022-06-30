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
    [Register ("EventsDateRangeView")]
    partial class EventsDateRangeView
    {
        [Outlet]
        UIKit.UITableView groupTableView { get; set; }


        [Outlet]
        UIKit.NSLayoutConstraint tableBottomConstraint { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (groupTableView != null) {
                groupTableView.Dispose ();
                groupTableView = null;
            }

            if (tableBottomConstraint != null) {
                tableBottomConstraint.Dispose ();
                tableBottomConstraint = null;
            }
        }
    }
}