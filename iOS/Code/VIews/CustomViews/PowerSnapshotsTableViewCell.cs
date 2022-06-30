using System;

using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using UIKit;

namespace actchargers.iOS
{
    public partial class PowerSnapshotsTableViewCell : MvxTableViewCell
    {
        public static readonly NSString Key = new NSString("PowerSnapshotsTableViewCell");
        public static readonly UINib Nib;

        static PowerSnapshotsTableViewCell()
        {
            Nib = UINib.FromName("PowerSnapshotsTableViewCell", NSBundle.MainBundle);
        }

        protected PowerSnapshotsTableViewCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.

            SelectionStyle = UITableViewCellSelectionStyle.None;

            this.DelayBind(() =>
            {
                var set = this.CreateBindingSet<PowerSnapshotsTableViewCell, PowerSnapshotsModel>();
                set.Bind(Id).To(item => item.Id);
                set.Bind(Time).To(item => item.RecordTime);
                set.Bind(Voltage).To(item => item.Voltage);
                set.Bind(Current).To(item => item.Current);
                set.Bind(Power).To(item => item.Power);
                set.Apply();
            });
        }
    }
}
