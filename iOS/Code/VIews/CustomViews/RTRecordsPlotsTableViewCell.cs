using System;

using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using UIKit;

namespace actchargers.iOS
{
    public partial class RTRecordsPlotsTableViewCell : MvxTableViewCell
    {
        public static readonly NSString Key = new NSString("RTRecordsPlotsTableViewCell");
        public static readonly UINib Nib;

        static RTRecordsPlotsTableViewCell()
        {
            Nib = UINib.FromName("RTRecordsPlotsTableViewCell", NSBundle.MainBundle);
        }

        protected RTRecordsPlotsTableViewCell(IntPtr handle) : base(handle)
        {
            SelectionStyle = UITableViewCellSelectionStyle.None;
            this.DelayBind(() =>
            {
                var set = this.CreateBindingSet<RTRecordsPlotsTableViewCell, ChartViewItem>();
                set.Bind(voltagePlot).For(plot => plot.Model).To(item => item.PlotObjList[0]);
                set.Bind(temparaturePlot).For(plot => plot.Model).To(item => item.PlotObjList[2]);
                set.Bind(currentPlot).For(plot => plot.Model).To(item => item.PlotObjList[1]);
                set.Bind(socPlot).For(plot => plot.Model).To(item => item.PlotObjList[3]);
                set.Apply();
            });
        }
    }
}
