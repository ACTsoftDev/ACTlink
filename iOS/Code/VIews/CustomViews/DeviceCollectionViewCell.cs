using System;

using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using UIKit;

namespace actchargers.iOS
{
    public partial class DeviceCollectionViewCell : MvxCollectionViewCell
    {
        public static readonly NSString Key = new NSString("DeviceCollectionViewCell");
        public static readonly UINib Nib;

        /// <summary>
        /// MvxImageViewLoader for UIImageView
        /// </summary>
		private MvxImageViewLoader loader;

        /// <summary>
        /// Initializes the <see cref="T:actchargers.iOS.DeviceCollectionViewCell"/> class.
        /// </summary>
		static DeviceCollectionViewCell()
        {
            Nib = UINib.FromName("DeviceCollectionViewCell", NSBundle.MainBundle);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="T:actchargers.iOS.DeviceCollectionViewCell"/> class.
        /// </summary>
        /// <param name="handle">Handle.</param>
		protected DeviceCollectionViewCell(IntPtr handle) : base(handle)
        {
            Initialize();
        }
        /// <summary>
        /// Initialize this instance.
        /// </summary>
		void Initialize()
        {
            loader = new MvxImageViewLoader(() => iconImageView);
            this.DelayBind(() =>
            {
                var set = this.CreateBindingSet<DeviceCollectionViewCell, DeviceDeatilsItem>();
                set.Bind(loader).To(item => item.DeviceImage).WithConversion("ImageName", 1);
                set.Bind(titleLbl).To(item => item.DeviceTitle);
                set.Bind(disabledLayer).For(img => img.Hidden).To(item => item.IsDisabled).WithConversion("Inverse");
                set.Apply();
            });
        }
    }
}