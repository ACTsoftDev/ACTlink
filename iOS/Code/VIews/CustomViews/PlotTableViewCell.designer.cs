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
    [Register ("PlotTableViewCell")]
    partial class PlotTableViewCell
    {
        [Outlet]
        OxyPlot.Xamarin.iOS.PlotView graphPlotView { get; set; }


        [Outlet]
        UIKit.UIButton plotButton { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (graphPlotView != null) {
                graphPlotView.Dispose ();
                graphPlotView = null;
            }

            if (plotButton != null) {
                plotButton.Dispose ();
                plotButton = null;
            }
        }
    }
}