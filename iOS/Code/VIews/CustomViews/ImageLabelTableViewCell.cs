using System;

using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using UIKit;

namespace actchargers.iOS
{
    public partial class ImageLabelTableViewCell : MvxTableViewCell
    {
        public static readonly NSString Key = new NSString("ImageLabelTableViewCell");
        public static readonly UINib Nib;

        private MvxImageViewLoader loader;

        static ImageLabelTableViewCell()
        {
            Nib = UINib.FromName("ImageLabelTableViewCell", NSBundle.MainBundle);
        }

        protected ImageLabelTableViewCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
            loader = new MvxImageViewLoader(() => statusImageView);
            SelectionStyle = UITableViewCellSelectionStyle.None;
            this.DelayBind(() =>
            {
                //Binding tabelviewcell properties with ListViewItem
                var set = this.CreateBindingSet<ImageLabelTableViewCell, ListViewItem>();
                set.Bind(titleLabel).To(item => item.Title);
                set.Bind(loader).To(item => item.ImageName).WithConversion("ImageName", 1);
                set.Apply();
            });
        }
    }
}
