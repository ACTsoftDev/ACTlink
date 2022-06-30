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
    [Register ("ImageTableViewCell")]
    partial class ImageTableViewCell
    {
        [Outlet]
        UIKit.UIImageView imageview { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (imageview != null) {
                imageview.Dispose ();
                imageview = null;
            }
        }
    }
}