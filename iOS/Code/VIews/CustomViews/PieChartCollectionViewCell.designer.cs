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
    [Register ("PieChartCollectionViewCell")]
    partial class PieChartCollectionViewCell
    {
        [Outlet]
        UIKit.UIImageView imageView { get; set; }


        [Outlet]
        OxyPlot.Xamarin.iOS.PlotView pieChartView { get; set; }


        [Outlet]
        UIKit.UILabel titleLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (imageView != null) {
                imageView.Dispose ();
                imageView = null;
            }

            if (pieChartView != null) {
                pieChartView.Dispose ();
                pieChartView = null;
            }

            if (titleLabel != null) {
                titleLabel.Dispose ();
                titleLabel = null;
            }
        }
    }
}