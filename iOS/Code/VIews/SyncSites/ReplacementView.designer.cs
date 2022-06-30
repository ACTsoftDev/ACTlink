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
    [Register ("ReplacementView")]
    partial class ReplacementView
    {
        [Outlet]
        UIKit.UITableView listTableView { get; set; }


        [Outlet]
        UIKit.UISearchBar searchBar { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        actchargers.iOS.RoundedRectButton updateFirmwareButton { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (listTableView != null) {
                listTableView.Dispose ();
                listTableView = null;
            }

            if (searchBar != null) {
                searchBar.Dispose ();
                searchBar = null;
            }

            if (updateFirmwareButton != null) {
                updateFirmwareButton.Dispose ();
                updateFirmwareButton = null;
            }
        }
    }
}