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
    [Register ("RTRecordsPlotsTableViewCell")]
    partial class RTRecordsPlotsTableViewCell
    {
        [Outlet]
        OxyPlot.Xamarin.iOS.PlotView currentPlot { get; set; }


        [Outlet]
        OxyPlot.Xamarin.iOS.PlotView socPlot { get; set; }


        [Outlet]
        OxyPlot.Xamarin.iOS.PlotView temparaturePlot { get; set; }


        [Outlet]
        OxyPlot.Xamarin.iOS.PlotView voltagePlot { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (currentPlot != null) {
                currentPlot.Dispose ();
                currentPlot = null;
            }

            if (socPlot != null) {
                socPlot.Dispose ();
                socPlot = null;
            }

            if (temparaturePlot != null) {
                temparaturePlot.Dispose ();
                temparaturePlot = null;
            }

            if (voltagePlot != null) {
                voltagePlot.Dispose ();
                voltagePlot = null;
            }
        }
    }
}