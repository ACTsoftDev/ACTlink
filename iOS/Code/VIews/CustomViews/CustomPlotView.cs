using System;
using MvvmCross.Binding.iOS.Views;

namespace actchargers.iOS
{
    public partial class CustomPlotView : MvxView
    {
        public CustomPlotView(IntPtr h) : base (h)
        {
        }

        public void ApplyPlotView(OxyPlot.PlotModel view)
        {
            plotView.Model = view;
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();
            cancelButton.TouchUpInside += delegate {
                plotView.Dispose();
                RemoveFromSuperview();
            };
        }
    }
}
