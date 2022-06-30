using System;

using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using UIKit;

namespace actchargers.iOS
{
    public partial class LabelSwitchButtonTableViewCell : MvxTableViewCell
    {
        public static readonly NSString Key = new NSString("LabelSwitchButtonTableViewCell");
        public static readonly UINib Nib;

        static LabelSwitchButtonTableViewCell()
        {
            Nib = UINib.FromName("LabelSwitchButtonTableViewCell", NSBundle.MainBundle);
        }

        protected LabelSwitchButtonTableViewCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.

            SelectionStyle = UITableViewCellSelectionStyle.None;
            this.DelayBind(() =>
            {
                //Binding ListViewItem with properties of this cell 
                var set = this.CreateBindingSet<LabelSwitchButtonTableViewCell, ListViewItem>();
                set.Bind(titleLabel).To(item => item.Title);
                set.Bind(valueSwitch).For(valueLabel => valueLabel.On).To(item => item.IsSwitchEnabled);
                set.Bind(actionButton).For("Title").To(item => item.Title2);
                set.Bind(actionButton).To(item => item.ButtonSelectorCommand);
                set.Apply();
            });
        }
    }
}
