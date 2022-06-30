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
    [Register ("CommissionNextView")]
    partial class CommissionNextView
    {
        [Outlet]
        UIKit.UITableView listTableView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (listTableView != null) {
                listTableView.Dispose ();
                listTableView = null;
            }
        }
    }
}