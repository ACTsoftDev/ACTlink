using System;

using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using UIKit;

namespace actchargers.iOS
{
    public partial class QuickViewThreeLabelTableViewCell : MvxTableViewCell
    {
        public static readonly NSString Key = new NSString("QuickViewThreeLabelTableViewCell");
        public static readonly UINib Nib;

        static QuickViewThreeLabelTableViewCell()
        {
            Nib = UINib.FromName("QuickViewThreeLabelTableViewCell", NSBundle.MainBundle);
        }

        protected QuickViewThreeLabelTableViewCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
            SelectionStyle = UITableViewCellSelectionStyle.None;
            this.DelayBind(() =>
            {
                //Binding ListViewItem with ThreeLabelTableViewCell
                var set = this.CreateBindingSet<QuickViewThreeLabelTableViewCell, ListViewItem>();
                set.Bind(ahrlabel).To(item => item.Title);
                set.Bind(ahrvalue).To(item => item.SubTitle);
                set.Bind(kwhrlabel).To(item => item.Title2);
                set.Bind(kwhrvalue).To(item => item.SubTitle2);
                set.Bind(durationlabel).To(item => item.Title3);
                set.Bind(durationhour).To(item => item.SubTitle3);
                set.Bind(durationmin).To(item => item.Text);
                set.Bind(durationsec).To(item => item.Seconds);
                set.Apply();
            });
        }
    }
}
