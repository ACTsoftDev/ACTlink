using System;

using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using UIKit;

namespace actchargers.iOS
{
    public partial class ReplacementDeviceItemTableViewCell : MvxTableViewCell
    {
        public static readonly NSString Key = new NSString("ReplacementDeviceItemTableViewCell");
        public static readonly UINib Nib;

        static ReplacementDeviceItemTableViewCell()
        {
            Nib = UINib.FromName("ReplacementDeviceItemTableViewCell", NSBundle.MainBundle);
        }

        protected ReplacementDeviceItemTableViewCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
            SelectionStyle = UITableViewCellSelectionStyle.None;
            this.DelayBind(() =>
            {
                var set = this.CreateBindingSet<ReplacementDeviceItemTableViewCell, SynchObjectsBufferedData>();
                set.Bind(titleLabel).To(item => item.DeviceFullName);
                set.Bind(customerName).To(item => item.SynchSite.CustomerName);
                set.Bind(siteName).To(item => item.SynchSite.SiteName);
                set.Apply();
            });
        }
    }
}
