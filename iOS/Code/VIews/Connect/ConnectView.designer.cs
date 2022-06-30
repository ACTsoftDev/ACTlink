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
    [Register ("ConnectView")]
    partial class ConnectView
    {
        [Outlet]
        actchargers.iOS.RoundedRectButton connectBtn { get; set; }


        [Outlet]
        UIKit.NSLayoutConstraint connectButtonTopConstraint { get; set; }


        [Outlet]
        UIKit.UIView connectView { get; set; }


        [Outlet]
        UIKit.NSLayoutConstraint connectViewHeight { get; set; }


        [Outlet]
        UIKit.UILabel messageLbl { get; set; }


        [Outlet]
        UIKit.UILabel passwordLbl { get; set; }


        [Outlet]
        UIKit.UITextField passwordTF { get; set; }


        [Outlet]
        UIKit.UISegmentedControl segmentController { get; set; }


        [Outlet]
        UIKit.UILabel ssidLbl { get; set; }


        [Outlet]
        UIKit.UITextField ssidTF { get; set; }


        [Outlet]
        UIKit.UIButton syncSitesBtn { get; set; }


        [Outlet]
        UIKit.UILabel synSiteSubTitleLbl { get; set; }


        [Outlet]
        UIKit.UILabel synSiteTitleLbl { get; set; }


        [Outlet]
        UIKit.UIButton uploadBtn { get; set; }


        [Outlet]
        UIKit.UILabel uploadSubTitleLbl { get; set; }


        [Outlet]
        UIKit.UILabel uploadTitleLbl { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        actchargers.iOS.RoundedRectButton pushBackupBtn { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (connectBtn != null) {
                connectBtn.Dispose ();
                connectBtn = null;
            }

            if (connectButtonTopConstraint != null) {
                connectButtonTopConstraint.Dispose ();
                connectButtonTopConstraint = null;
            }

            if (connectView != null) {
                connectView.Dispose ();
                connectView = null;
            }

            if (connectViewHeight != null) {
                connectViewHeight.Dispose ();
                connectViewHeight = null;
            }

            if (messageLbl != null) {
                messageLbl.Dispose ();
                messageLbl = null;
            }

            if (passwordLbl != null) {
                passwordLbl.Dispose ();
                passwordLbl = null;
            }

            if (passwordTF != null) {
                passwordTF.Dispose ();
                passwordTF = null;
            }

            if (pushBackupBtn != null) {
                pushBackupBtn.Dispose ();
                pushBackupBtn = null;
            }

            if (segmentController != null) {
                segmentController.Dispose ();
                segmentController = null;
            }

            if (ssidLbl != null) {
                ssidLbl.Dispose ();
                ssidLbl = null;
            }

            if (ssidTF != null) {
                ssidTF.Dispose ();
                ssidTF = null;
            }

            if (syncSitesBtn != null) {
                syncSitesBtn.Dispose ();
                syncSitesBtn = null;
            }

            if (synSiteSubTitleLbl != null) {
                synSiteSubTitleLbl.Dispose ();
                synSiteSubTitleLbl = null;
            }

            if (synSiteTitleLbl != null) {
                synSiteTitleLbl.Dispose ();
                synSiteTitleLbl = null;
            }

            if (uploadBtn != null) {
                uploadBtn.Dispose ();
                uploadBtn = null;
            }

            if (uploadSubTitleLbl != null) {
                uploadSubTitleLbl.Dispose ();
                uploadSubTitleLbl = null;
            }

            if (uploadTitleLbl != null) {
                uploadTitleLbl.Dispose ();
                uploadTitleLbl = null;
            }
        }
    }
}