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
    [Register ("QuickViewTableViewCell")]
    partial class QuickViewTableViewCell
    {
        [Outlet]
        UIKit.UICollectionView listCollectionView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (listCollectionView != null) {
                listCollectionView.Dispose ();
                listCollectionView = null;
            }
        }
    }
}