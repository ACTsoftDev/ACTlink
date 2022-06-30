using System;

using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using UIKit;

namespace actchargers.iOS
{
    public partial class SiteViewInProgressDevicesTableViewCell : MvxTableViewCell
    {
        public static readonly NSString Key = new NSString("SiteViewInProgressDevicesTableViewCell");
        public static readonly UINib Nib;

        MvxImageViewLoader chargerImageloader;
        MvxImageViewLoader battviewImageloader;

        static SiteViewInProgressDevicesTableViewCell()
        {
            Nib = UINib.FromName("SiteViewDevicesTableViewCell", NSBundle.MainBundle);
        }

        protected SiteViewInProgressDevicesTableViewCell(IntPtr handle) : base(handle)
        {
            chargerImageloader = new MvxImageViewLoader(() => chargerImage);
            battviewImageloader = new MvxImageViewLoader(() => battImage);

            SelectionStyle = UITableViewCellSelectionStyle.None;

            this.DelayBind(() =>
            {
                var set = this.CreateBindingSet<SiteViewInProgressDevicesTableViewCell, SiteViewDeviceObject>();

                set.Bind(notSiteView).For(o => o.Hidden).To(item => item.IsSite);
                set.Bind(chargerImageloader).To(item => item.ImageString).WithConversion("ImageName", 1);
                set.Bind(battviewImageloader).To(item => item.ImageString).WithConversion("ImageName", 1);
                set.Bind(chargerImage).For(o => o.Hidden).To(item => item.BattviewImageVisibility);
                set.Bind(battImage).For(o => o.Hidden).To(item => item.ChargerImageVisibility);

                set.Bind(deviceTitle).To(item => item.deviceName);
                set.Bind(deviceSerialNumber).To(item => item.serialNumber);
                set.Bind(deviceStatus).To(item => item.DeviceStatus);
                set.Bind(progressIndicator).For(o => o.Progress).To(item => item.ProgressCompletedIOS);
                set.Bind(progressIndicator).For(o => o.Hidden).To(item => item.ProgressBarVisibilityIOS);

                set.Apply();
            });
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();

            notSiteView.BackgroundColor = UIColorUtility.FromHex(ACColors.TEXT_GRAY_COLOR);
        }
    }
}
