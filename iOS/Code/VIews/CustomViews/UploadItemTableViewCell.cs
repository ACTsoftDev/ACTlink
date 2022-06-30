using System;

using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using UIKit;

namespace actchargers.iOS
{
    public partial class UploadItemTableViewCell : MvxTableViewCell
    {
        public static readonly NSString Key = new NSString("UploadItemTableViewCell");
        public static readonly UINib Nib;

        static UploadItemTableViewCell()
        {
            Nib = UINib.FromName("UploadItemTableViewCell", NSBundle.MainBundle);
        }

        protected UploadItemTableViewCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
            SelectionStyle = UITableViewCellSelectionStyle.None;
            this.DelayBind(() =>

            {
                var set = this.CreateBindingSet<UploadItemTableViewCell, UploadableDeviceViewModel>();
                set.Bind(deviceName).To(item => item.Device.UploadTitle);
                set.Bind(statusLabel).To(item => item.Device.Id);
                //set.Bind(progressIndicator).For(o => o.Progress).To(item => item.ProgressCompletedIOS);

                set.Apply();
            });

        }
    }
}
