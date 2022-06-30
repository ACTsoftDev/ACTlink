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
    [Register ("QuickView")]
    partial class QuickView
    {
        [Outlet]
        UIKit.UILabel AHrTitle { get; set; }


        [Outlet]
        UIKit.UILabel AHrValue { get; set; }


        [Outlet]
        UIKit.UILabel HrTitle { get; set; }


        [Outlet]
        UIKit.UILabel IDLEMins { get; set; }


        [Outlet]
        UIKit.UILabel IDLETitle { get; set; }


        [Outlet]
        UIKit.UILabel IDLEValue { get; set; }


        [Outlet]
        UIKit.UILabel KHWrValue { get; set; }


        [Outlet]
        UIKit.UILabel KWHrTitle { get; set; }


        [Outlet]
        UIKit.UICollectionView pieChartCollectionView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (AHrTitle != null) {
                AHrTitle.Dispose ();
                AHrTitle = null;
            }

            if (AHrValue != null) {
                AHrValue.Dispose ();
                AHrValue = null;
            }

            if (HrTitle != null) {
                HrTitle.Dispose ();
                HrTitle = null;
            }

            if (IDLEMins != null) {
                IDLEMins.Dispose ();
                IDLEMins = null;
            }

            if (IDLETitle != null) {
                IDLETitle.Dispose ();
                IDLETitle = null;
            }

            if (IDLEValue != null) {
                IDLEValue.Dispose ();
                IDLEValue = null;
            }

            if (KHWrValue != null) {
                KHWrValue.Dispose ();
                KHWrValue = null;
            }

            if (KWHrTitle != null) {
                KWHrTitle.Dispose ();
                KWHrTitle = null;
            }

            if (pieChartCollectionView != null) {
                pieChartCollectionView.Dispose ();
                pieChartCollectionView = null;
            }
        }
    }
}