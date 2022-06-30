using System;

using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using UIKit;

namespace actchargers.iOS
{
    public partial class LabelSwitchTableViewCell : MvxTableViewCell
    {
        public static readonly NSString Key = new NSString("LabelSwitchTableViewCell");
        public static readonly UINib Nib;

        /// <summary>
        /// Initializes the <see cref="T:actchargers.iOS.LabelSwitchTableViewCell"/> class.
        /// </summary>
        static LabelSwitchTableViewCell()
        {
            Nib = UINib.FromName("LabelSwitchTableViewCell", NSBundle.MainBundle);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:actchargers.iOS.LabelSwitchTableViewCell"/> class.
        /// </summary>
        /// <param name="handle">Handle.</param>
        protected LabelSwitchTableViewCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.

            //SelectionStyle None
            SelectionStyle = UITableViewCellSelectionStyle.None;
            this.DelayBind(() =>
            {
                //Binding ListViewItem with properties of this cell 
                var set = this.CreateBindingSet<LabelSwitchTableViewCell, ListViewItem>();
                set.Bind(titleLabel).To(item => item.Title);
                set.Bind(valueSwitch).For(valueLabel => valueLabel.On).To(item => item.IsSwitchEnabled);
                set.Apply();
            });
        }

        /// <summary>
        /// Awakes from nib.
        /// </summary>
        public override void AwakeFromNib()
        {
            base.AwakeFromNib();
            titleLabel.LayoutIfNeeded();
        }
    }
}
