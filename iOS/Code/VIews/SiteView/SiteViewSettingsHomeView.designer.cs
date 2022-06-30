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
    [Register ("SiteViewSettingsHomeView")]
    partial class SiteViewSettingsHomeView
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton applySettingsButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton cancelButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UICollectionView deviceCollectionView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (applySettingsButton != null) {
                applySettingsButton.Dispose ();
                applySettingsButton = null;
            }

            if (cancelButton != null) {
                cancelButton.Dispose ();
                cancelButton = null;
            }

            if (deviceCollectionView != null) {
                deviceCollectionView.Dispose ();
                deviceCollectionView = null;
            }
        }
    }
}