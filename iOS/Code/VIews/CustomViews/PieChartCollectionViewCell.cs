using System;

using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using OxyPlot.Xamarin.iOS;
using UIKit;

namespace actchargers.iOS
{
    public partial class PieChartCollectionViewCell : MvxCollectionViewCell
    {
        public static readonly NSString Key = new NSString("PieChartCollectionViewCell");
        public static readonly UINib Nib;

        /// <summary>
        /// MvxImageViewLoader for UIImageView
        /// </summary>
        private MvxImageViewLoader loader;

        /// <summary>
        /// Gets the pie chart view.
        /// </summary>
        /// <value>The pie chart view.</value>
        public PlotView PieChartView { private set; get; }

        /// <summary>
        /// Initializes the <see cref="T:actchargers.iOS.PieChartCollectionViewCell"/> class.
        /// </summary>
        static PieChartCollectionViewCell()
        {
            Nib = UINib.FromName("PieChartCollectionViewCell", NSBundle.MainBundle);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:actchargers.iOS.PieChartCollectionViewCell"/> class.
        /// </summary>
        /// <param name="handle">Handle.</param>
        protected PieChartCollectionViewCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
            loader = new MvxImageViewLoader(() => imageView);
            this.DelayBind(() =>
            {
                var set = this.CreateBindingSet<PieChartCollectionViewCell, ChartViewItem>();
                set.Bind(loader).To(item => item.ChartImageName).WithConversion("ImageName", 1);
                set.Bind(titleLabel).To(item => item.ChartType);
                set.Bind(pieChartView).For(o => o.Model).To(item => item.PlotObject);
                set.Apply();
            });
        }

        /// <summary>
        /// Awakes from nib.
        /// </summary>
        public override void AwakeFromNib()
        {
            base.AwakeFromNib();

            //PieChartView = pieChartView;
            //PieChartView.UserInteractionEnabled = false;
        }
    }
}
