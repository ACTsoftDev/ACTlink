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
    [Register ("CumulativeDataView")]
    partial class CumulativeDataView
    {
        [Outlet]
        UIKit.UITableView listTableView { get; set; }


        [Outlet]
        UIKit.UIView resetBtnView { get; set; }


        [Outlet]
        actchargers.iOS.RoundedRectButton resetButton { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (listTableView != null) {
                listTableView.Dispose ();
                listTableView = null;
            }
        }
    }
}