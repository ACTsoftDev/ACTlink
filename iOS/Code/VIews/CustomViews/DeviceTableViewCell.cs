using System;

using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using UIKit;

namespace actchargers.iOS
{
    public partial class DeviceTableViewCell : MvxTableViewCell
    {
        public static readonly NSString Key = new NSString("DeviceTableViewCell");
        public static readonly UINib Nib;

        /// <summary>
        /// Initializes the <see cref="T:actchargers.iOS.DeviceTableViewCell"/> class.
        /// </summary>
        static DeviceTableViewCell()
        {
            Nib = UINib.FromName("DeviceTableViewCell", NSBundle.MainBundle);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:actchargers.iOS.DeviceTableViewCell"/> class.
        /// </summary>
        /// <param name="handle">Handle.</param>
        protected DeviceTableViewCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.

            BackgroundColor = UIColor.Clear;
            Initialize();
        }

        /// <summary>
        /// Initialize this instance.
        /// </summary>
        void Initialize()
        {
            this.DelayBind(() =>
            {
                var set = this.CreateBindingSet<DeviceTableViewCell, DeviceInfoItem>();
                set.Bind(titleLbl).To(item => item.Title);
                set.Bind(subTitleLbl).To(item => item.SubTitle);
                set.Bind(SelectedImageView).For(img => img.Hidden).To(item => item.IsConnected).WithConversion("Inverse");
                set.Apply();
            });
        }

        /// <summary>
        /// Awakes from nib.
        /// </summary>
        public override void AwakeFromNib()
        {
            base.AwakeFromNib();

            titleLbl.TextColor = UIColorUtility.FromHex(ACColors.DARK_BLUE_COLOR);
            subTitleLbl.TextColor = UIColorUtility.FromHex(ACColors.TEXT_GRAY_COLOR);
        }
    }
}
