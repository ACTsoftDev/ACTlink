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
    [Register ("AboutUsView")]
    partial class AboutUsView
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextView aboutAct { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel appName { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel appVersion { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel battViewVersion { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel calibratorVersion { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel copyright { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView logo { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel mcbVersion { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel poweredBy { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView poweredLogo { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView poweredView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (aboutAct != null) {
                aboutAct.Dispose ();
                aboutAct = null;
            }

            if (appName != null) {
                appName.Dispose ();
                appName = null;
            }

            if (appVersion != null) {
                appVersion.Dispose ();
                appVersion = null;
            }

            if (battViewVersion != null) {
                battViewVersion.Dispose ();
                battViewVersion = null;
            }

            if (calibratorVersion != null) {
                calibratorVersion.Dispose ();
                calibratorVersion = null;
            }

            if (copyright != null) {
                copyright.Dispose ();
                copyright = null;
            }

            if (logo != null) {
                logo.Dispose ();
                logo = null;
            }

            if (mcbVersion != null) {
                mcbVersion.Dispose ();
                mcbVersion = null;
            }

            if (poweredBy != null) {
                poweredBy.Dispose ();
                poweredBy = null;
            }

            if (poweredLogo != null) {
                poweredLogo.Dispose ();
                poweredLogo = null;
            }

            if (poweredView != null) {
                poweredView.Dispose ();
                poweredView = null;
            }
        }
    }
}