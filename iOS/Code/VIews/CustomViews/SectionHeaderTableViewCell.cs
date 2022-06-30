using System;

using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using UIKit;

namespace actchargers.iOS
{
    public partial class SectionHeaderTableViewCell : MvxTableViewCell
    {
        public static readonly NSString Key = new NSString("SectionHeaderTableViewCell");
        public static readonly UINib Nib;

        static SectionHeaderTableViewCell()
        {
            Nib = UINib.FromName("SectionHeaderTableViewCell", NSBundle.MainBundle);
        }

        protected SectionHeaderTableViewCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.

            SelectionStyle = UITableViewCellSelectionStyle.None;
            this.DelayBind(() =>
            {
                var set = this.CreateBindingSet<SectionHeaderTableViewCell, ListViewItem>();
                set.Bind(titleLabel).To(item => item.Title);
                set.Apply();
            });
        }
    }
}
