using System;

using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using UIKit;

namespace actchargers.iOS
{
    public partial class ViewCyclesHistoryTableViewCell : MvxTableViewCell
    {
        public static readonly NSString Key = new NSString("ViewCyclesHistoryTableViewCell");
        public static readonly UINib Nib;

        static ViewCyclesHistoryTableViewCell()
        {
            Nib = UINib.FromName("ViewCyclesHistoryTableViewCell", NSBundle.MainBundle);
        }

        protected ViewCyclesHistoryTableViewCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
            SelectionStyle = UITableViewCellSelectionStyle.None;

            this.DelayBind(() =>
            {
                var set = this.CreateBindingSet<ViewCyclesHistoryTableViewCell, ViewCycle>();
                set.Bind(text1).To(item => item.ViewCycleID);
                set.Bind(text2).To(item => item.Date);
                set.Bind(text3).To(item => item.AHRS);
                set.Bind(text4).To(item => item.Duration);
                set.Bind(text5).To(item => item.EXitStatus);
                set.Bind(text6).To(item => item.BatteryTypeText);
                set.Apply();
            });
        }
    }
}
