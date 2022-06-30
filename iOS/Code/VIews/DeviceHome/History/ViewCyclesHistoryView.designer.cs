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
    [Register ("ViewCyclesHistoryView")]
    partial class ViewCyclesHistoryView
    {
        [Outlet]
        UIKit.UILabel heading1 { get; set; }


        [Outlet]
        UIKit.UILabel heading2 { get; set; }


        [Outlet]
        UIKit.UILabel heading3 { get; set; }


        [Outlet]
        UIKit.UILabel heading4 { get; set; }


        [Outlet]
        UIKit.UILabel heading5 { get; set; }


        [Outlet]
        UIKit.UITableView listTableView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel heading6 { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (heading1 != null) {
                heading1.Dispose ();
                heading1 = null;
            }

            if (heading2 != null) {
                heading2.Dispose ();
                heading2 = null;
            }

            if (heading3 != null) {
                heading3.Dispose ();
                heading3 = null;
            }

            if (heading4 != null) {
                heading4.Dispose ();
                heading4 = null;
            }

            if (heading5 != null) {
                heading5.Dispose ();
                heading5 = null;
            }

            if (heading6 != null) {
                heading6.Dispose ();
                heading6 = null;
            }

            if (listTableView != null) {
                listTableView.Dispose ();
                listTableView = null;
            }
        }
    }
}