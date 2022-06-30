using System;

using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using UIKit;

namespace actchargers.iOS
{
    public partial class LabelTableViewCell : MvxTableViewCell
    {
        public static readonly NSString Key = new NSString("LabelTableViewCell");
        public static readonly UINib Nib;

        static LabelTableViewCell()
        {
            Nib = UINib.FromName("LabelTableViewCell", NSBundle.MainBundle);
        }

        protected LabelTableViewCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.

            SelectionStyle = UITableViewCellSelectionStyle.None;
            this.DelayBind(() =>
            {
                var set = this.CreateBindingSet<LabelTableViewCell, ListViewItem>();
                set.Bind(titleLabel).To(item => item.Text);
                set.Apply();
            });
        }
    }
}
