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
    [Register ("ListCollectionViewCell")]
    partial class ListCollectionViewCell
    {
        [Outlet]
        actchargers.iOS.DaysRoundedRectButton gridButton { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (gridButton != null) {
                gridButton.Dispose ();
                gridButton = null;
            }
        }
    }
}