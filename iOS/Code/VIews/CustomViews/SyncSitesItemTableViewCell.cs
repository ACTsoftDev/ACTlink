using System;

using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using UIKit;

namespace actchargers.iOS
{
    public partial class SyncSitesItemTableViewCell : MvxTableViewCell
    {
        public static readonly NSString Key = new NSString("SyncSitesItemTableViewCell");
        public static readonly UINib Nib;
        private MvxImageViewLoader checkImageloader;

        static SyncSitesItemTableViewCell()
        {
            Nib = UINib.FromName("SyncSitesItemTableViewCell", NSBundle.MainBundle);
        }

        protected SyncSitesItemTableViewCell(IntPtr handle) : base(handle)
        {
            checkImageloader = new MvxImageViewLoader(() => checkImageview);
            SelectionStyle = UITableViewCellSelectionStyle.None;
            this.DelayBind(() =>
            {
                var set = this.CreateBindingSet<SyncSitesItemTableViewCell,ACTViewSite>();
                set.Bind(itemLabel).To(item => item.name);
                set.Bind(itemCheckBtn).To(item => item.ItemCheckBtnCommand);
                set.Bind(checkImageloader).To(item => item.CheckedImageString).WithConversion("ImageName", 1);
                set.Apply();
            });
        }
    }
}
