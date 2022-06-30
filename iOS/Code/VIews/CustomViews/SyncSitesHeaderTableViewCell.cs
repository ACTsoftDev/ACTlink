using System;

using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using UIKit;

namespace actchargers.iOS
{

    public partial class SyncSitesHeaderTableViewCell : MvxTableViewCell     {         public static readonly NSString Key = new NSString("SyncSitesHeaderView");         public static readonly UINib Nib; 
        /// <summary>
        /// MvxImageViewLoader for UIImageView
        /// </summary>
        private MvxImageViewLoader loader;
        private MvxImageViewLoader checkImageloader;
         static SyncSitesHeaderTableViewCell()         {             Nib = UINib.FromName("SyncSitesHeaderTableViewCell", NSBundle.MainBundle);         }          protected SyncSitesHeaderTableViewCell(IntPtr handle) : base(handle)         {
            // Note: this .ctor should not contain any initialization logic.
            loader = new MvxImageViewLoader(() => expandImageView);
            checkImageloader = new MvxImageViewLoader(() => checkImageView);

            SelectionStyle = UITableViewCellSelectionStyle.None;
            this.DelayBind(() =>
            {
                //Binding tabelviewcell properties with ListViewItem
                var set = this.CreateBindingSet<SyncSitesHeaderTableViewCell, SyncSitesHeaderItem>();
                set.Bind(HeaderTitle).To(item => item.Name);

                set.Bind(loader).To(item => item.ExpandImageString).WithConversion("ImageName", 1);
                set.Bind(expandBtn).To(item => item.ExpandBtnCommand);

                set.Bind(checkImageloader).To(item => item.CheckedImageString).WithConversion("ImageName", 1);
                set.Bind(checkBtn).To(item => item.CheckBtnCommand);


                set.Apply();
            });         }     } 
}
