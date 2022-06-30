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
    [Register ("UsageAgreementView")]
    partial class UsageAgreementView
    {
        [Outlet]
        actchargers.iOS.RoundedRectButton acceptBtn { get; set; }


        [Outlet]
        UIKit.UIWebView agreementWebView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (acceptBtn != null) {
                acceptBtn.Dispose ();
                acceptBtn = null;
            }

            if (agreementWebView != null) {
                agreementWebView.Dispose ();
                agreementWebView = null;
            }
        }
    }
}