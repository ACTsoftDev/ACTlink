using System;

using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using UIKit;

namespace actchargers.iOS
{
    public partial class ActionMenuTableViewCell : MvxTableViewCell
    {
        public static readonly NSString Key = new NSString("ActionMenuTableViewCell");
        public static readonly UINib Nib;

        /// <summary>
        /// MvxImageViewLoader for UIImageView
        /// </summary>
        private MvxImageViewLoader loader;

        /// <summary>
        /// Initializes the <see cref="T:actchargers.iOS.ActionMenuTableViewCell"/> class.
        /// </summary>
        static ActionMenuTableViewCell()
        {
            Nib = UINib.FromName("ActionMenuTableViewCell", NSBundle.MainBundle);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:actchargers.iOS.ActionMenuTableViewCell"/> class.
        /// </summary>
        /// <param name="handle">Handle.</param>
        protected ActionMenuTableViewCell(IntPtr handle) : base(handle)
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
            loader = new MvxImageViewLoader(() => iconImageView);
            this.SelectionStyle = UITableViewCellSelectionStyle.None;
            this.DelayBind(() =>
            {
                //Binding DeviceDeatilsItem with TabelViewCell
                var set = this.CreateBindingSet<ActionMenuTableViewCell, DeviceDeatilsItem>();
                set.Bind(loader).To(item => item.DeviceImage).WithConversion("ImageName", 1);
                set.Bind(titleLbl).To(item => item.DeviceTitle);
                //set.Bind(subTitleLbl).To(item => item.DeviceSubTitle);
                set.Apply();
            });
        }
    }
}
