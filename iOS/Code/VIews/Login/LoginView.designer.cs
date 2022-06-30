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
    [Register ("LoginView")]
    partial class LoginView
    {
        [Outlet]
        UIKit.UIButton contactUsBtn { get; set; }


        [Outlet]
        UIKit.UITextField emailIdTF { get; set; }


        [Outlet]
        UIKit.UILabel havingTroubleLbl { get; set; }


        [Outlet]
        UIKit.UILabel notRegisteredLbl { get; set; }


        [Outlet]
        UIKit.UITextField passwordTF { get; set; }


        [Outlet]
        UIKit.UIButton RegisterAtBtn { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (contactUsBtn != null) {
                contactUsBtn.Dispose ();
                contactUsBtn = null;
            }

            if (emailIdTF != null) {
                emailIdTF.Dispose ();
                emailIdTF = null;
            }

            if (havingTroubleLbl != null) {
                havingTroubleLbl.Dispose ();
                havingTroubleLbl = null;
            }

            if (notRegisteredLbl != null) {
                notRegisteredLbl.Dispose ();
                notRegisteredLbl = null;
            }

            if (passwordTF != null) {
                passwordTF.Dispose ();
                passwordTF = null;
            }

            if (RegisterAtBtn != null) {
                RegisterAtBtn.Dispose ();
                RegisterAtBtn = null;
            }
        }
    }
}