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
    [Register ("ChargeSummaryView")]
    partial class ChargeSummaryView
    {
        [Outlet]
        UIKit.UITableView listTableView { get; set; }


        [Outlet]
        UIKit.UIButton nextButton { get; set; }


        [Outlet]
        UIKit.UIButton previousButton { get; set; }


        [Outlet]
        UIKit.UILabel titleViewLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (listTableView != null) {
                listTableView.Dispose ();
                listTableView = null;
            }

            if (nextButton != null) {
                nextButton.Dispose ();
                nextButton = null;
            }

            if (previousButton != null) {
                previousButton.Dispose ();
                previousButton = null;
            }

            if (titleViewLabel != null) {
                titleViewLabel.Dispose ();
                titleViewLabel = null;
            }
        }
    }
}