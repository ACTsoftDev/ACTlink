using System;

using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using UIKit;

namespace actchargers.iOS
{
    public partial class ImageTableViewCell : MvxTableViewCell
    {
        public static readonly NSString Key = new NSString("ImageTableViewCell");
        public static readonly UINib Nib;

        /// <summary>
        /// MvxImageViewLoader for UIImageView
        /// </summary>
        private MvxImageViewLoader loader;

        static ImageTableViewCell()
        {
            Nib = UINib.FromName("ImageTableViewCell", NSBundle.MainBundle);
        }

        protected ImageTableViewCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
            SelectionStyle = UITableViewCellSelectionStyle.None;

            loader = new MvxImageViewLoader(() => imageview);
            this.DelayBind(() =>
            {
                var set = this.CreateBindingSet<ImageTableViewCell, ListViewItem>();
                set.Bind(loader).To(item => item.Text).WithConversion("ImageName", 1);
                set.Apply();
            });
        }
    }
}
