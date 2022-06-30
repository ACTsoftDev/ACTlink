using System;

using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using UIKit;

namespace actchargers.iOS
{
    public partial class PmLiveTableViewCell : MvxTableViewCell
    {
        public static readonly NSString Key = new NSString("PmLiveTableViewCell");
        public static readonly UINib Nib;

        static PmLiveTableViewCell()
        {
            Nib = UINib.FromName("PmLiveTableViewCell", NSBundle.MainBundle);
        }

        protected PmLiveTableViewCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.

            SelectionStyle = UITableViewCellSelectionStyle.None;

            this.DelayBind(() =>
            {
                var set = this.CreateBindingSet<PmLiveTableViewCell, PmLiveModel>();
                set.Bind(Id).To(item => item.MacAddreess);
                set.Bind(PmState).To(item => item.State);
                set.Bind(Current).To(item => item.Current);
                set.Bind(PmVoltage).To(item => item.Voltage);
                set.Bind(Rating).To(item => item.Rating);
                set.Apply();
            });
        }
    }
}
