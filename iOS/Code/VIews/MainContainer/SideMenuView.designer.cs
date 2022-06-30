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
    [Register ("SideMenuView")]
    partial class SideMenuView
    {
        [Outlet]
        UIKit.UITableView listTableView { get; set; }


        [Outlet]
        UIKit.UIButton logoutBtn { get; set; }


        [Outlet]
        UIKit.UIImageView logoutImageView { get; set; }


        [Outlet]
        UIKit.UILabel logoutLbl { get; set; }


        [Outlet]
        UIKit.UILabel userEmailLbl { get; set; }


        [Outlet]
        UIKit.UIImageView userImageView { get; set; }


        [Outlet]
        UIKit.UILabel userNameLbl { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView logoutView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (listTableView != null) {
                listTableView.Dispose ();
                listTableView = null;
            }

            if (logoutBtn != null) {
                logoutBtn.Dispose ();
                logoutBtn = null;
            }

            if (logoutImageView != null) {
                logoutImageView.Dispose ();
                logoutImageView = null;
            }

            if (logoutLbl != null) {
                logoutLbl.Dispose ();
                logoutLbl = null;
            }

            if (logoutView != null) {
                logoutView.Dispose ();
                logoutView = null;
            }

            if (userEmailLbl != null) {
                userEmailLbl.Dispose ();
                userEmailLbl = null;
            }

            if (userNameLbl != null) {
                userNameLbl.Dispose ();
                userNameLbl = null;
            }
        }
    }
}