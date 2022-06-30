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
    [Register ("SiteViewInProgressDevicesTableViewCell")]
    partial class SiteViewInProgressDevicesTableViewCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView battImage { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView chargerImage { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView deviceImageContainer { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel deviceSerialNumber { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel deviceStatus { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel deviceTitle { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView notSiteView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIProgressView progressIndicator { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (battImage != null) {
                battImage.Dispose ();
                battImage = null;
            }

            if (chargerImage != null) {
                chargerImage.Dispose ();
                chargerImage = null;
            }

            if (deviceImageContainer != null) {
                deviceImageContainer.Dispose ();
                deviceImageContainer = null;
            }

            if (deviceSerialNumber != null) {
                deviceSerialNumber.Dispose ();
                deviceSerialNumber = null;
            }

            if (deviceStatus != null) {
                deviceStatus.Dispose ();
                deviceStatus = null;
            }

            if (deviceTitle != null) {
                deviceTitle.Dispose ();
                deviceTitle = null;
            }

            if (notSiteView != null) {
                notSiteView.Dispose ();
                notSiteView = null;
            }

            if (progressIndicator != null) {
                progressIndicator.Dispose ();
                progressIndicator = null;
            }
        }
    }
}