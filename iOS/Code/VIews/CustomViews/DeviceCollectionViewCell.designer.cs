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
    [Register ("DeviceCollectionViewCell")]
    partial class DeviceCollectionViewCell
    {
        [Outlet]
        UIKit.UIImageView iconImageView { get; set; }


        [Outlet]
        UIKit.UILabel titleLbl { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView disabledLayer { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (disabledLayer != null) {
                disabledLayer.Dispose ();
                disabledLayer = null;
            }

            if (iconImageView != null) {
                iconImageView.Dispose ();
                iconImageView = null;
            }

            if (titleLbl != null) {
                titleLbl.Dispose ();
                titleLbl = null;
            }
        }
    }
}