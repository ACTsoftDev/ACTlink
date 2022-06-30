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
    [Register ("ViewGlobalRecordsView")]
    partial class ViewGlobalRecordsView
    {
        [Outlet]
        UIKit.UICollectionView deviceCollectionView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (deviceCollectionView != null) {
                deviceCollectionView.Dispose ();
                deviceCollectionView = null;
            }
        }
    }
}