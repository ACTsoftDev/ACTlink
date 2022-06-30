using System;

using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using OxyPlot;
using OxyPlot.Xamarin.iOS;
using UIKit;

namespace actchargers.iOS
{
    public partial class PlotTableViewCell : MvxTableViewCell
    {
        public UIButton PlotViewButton;

        public static readonly NSString Key = new NSString("PlotTableViewCell");
        public static readonly UINib Nib;

        /// <summary>
        /// Initializes the <see cref="T:actchargers.iOS.PlotTableViewCell"/> class.
        /// </summary>
		static PlotTableViewCell()
        {
            Nib = UINib.FromName("PlotTableViewCell", NSBundle.MainBundle);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:actchargers.iOS.PlotTableViewCell"/> class.
        /// </summary>
        /// <param name="handle">Handle.</param>
		protected PlotTableViewCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.

            SelectionStyle = UITableViewCellSelectionStyle.None;
            this.DelayBind(() =>
            {
                var set = this.CreateBindingSet<PlotTableViewCell, ChartViewItem>();
                set.Bind(graphPlotView).For(plot => plot.Model).To(item => item.PlotObject);
                set.Bind(plotButton).To(item => item.ButtonSelectorCommand);
                set.Apply();
            });


        }

        /// <summary>
        /// Awakes from nib.
        /// </summary>
		public override void AwakeFromNib()
        {
            base.AwakeFromNib();

            this.SelectionStyle = UITableViewCellSelectionStyle.None;
            graphPlotView.UserInteractionEnabled = true;

            PlotViewButton = plotButton;
        }
    }
}