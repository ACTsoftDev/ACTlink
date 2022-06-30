using System;

using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using UIKit;

namespace actchargers.iOS
{
    public partial class BatteryDailyUsageTableCell : MvxTableViewCell
    {
        public static readonly NSString Key = new NSString("BatteryDailyUsageTableCell");
        public static readonly UINib Nib;

        static BatteryDailyUsageTableCell()
        {
            Nib = UINib.FromName("BatteryDailyUsageTableCell", NSBundle.MainBundle);
        }

        protected BatteryDailyUsageTableCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
                        SelectionStyle = UITableViewCellSelectionStyle.None;

            this.DelayBind(() =>
            {
                var set = this.CreateBindingSet<BatteryDailyUsageTableCell, BATTDailyUsageModel>();
                set.Bind(text1).To(item => item.date);
                set.Bind(text2).To(item => item.is_working_day);
                set.Bind(text3).To(item => item.inuse_as);
                set.Bind(text4).To(item => item.inuse_ws);
                set.Bind(text5).To(item => item.charge_duration);
                set.Apply();
            });
        }
    }
}
