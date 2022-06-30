using System;

using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using UIKit;

namespace actchargers.iOS
{
    public partial class GlobalRecordsCollectionViewCell : MvxCollectionViewCell
    {
        public static readonly NSString Key = new NSString("GlobalRecordsCollectionViewCell");
        public static readonly UINib Nib;

        static GlobalRecordsCollectionViewCell()
        {
            Nib = UINib.FromName("GlobalRecordsCollectionViewCell", NSBundle.MainBundle);
        }

        protected GlobalRecordsCollectionViewCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.

            this.DelayBind(() =>
            {
                var set = this.CreateBindingSet<GlobalRecordsCollectionViewCell, ListViewItem>();
                set.Bind(titleLabel).To(item => item.Title);
                set.Bind(subTitle).To(item => item.SubTitle);
                set.Bind(subTitle2).To(item => item.SubTitle2);
                set.Apply();
            });
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();


        }
    }
}
