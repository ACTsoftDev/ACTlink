using System;

using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using UIKit;

namespace actchargers.iOS
{
    public partial class LabelLabelTableViewCell : MvxTableViewCell
    {
        public static readonly NSString Key = new NSString("LabelLabelTableViewCell");
        public static readonly UINib Nib;

        /// <summary>
        /// Initializes the <see cref="T:actchargers.iOS.LabelLabelTableViewCell"/> class.
        /// </summary>
        static LabelLabelTableViewCell()
        {
            Nib = UINib.FromName("LabelLabelTableViewCell", NSBundle.MainBundle);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:actchargers.iOS.LabelLabelTableViewCell"/> class.
        /// </summary>
        /// <param name="handle">Handle.</param>
        protected LabelLabelTableViewCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.

            //SelectionStyle None
            SelectionStyle = UITableViewCellSelectionStyle.None;
            this.DelayBind(() =>
            {
                //Binding tabelviewcell properties with ListViewItem
                var set = this.CreateBindingSet<LabelLabelTableViewCell, ListViewItem>();
                set.Bind(titleLabel).To(item => item.Title);
                set.Bind(valueLabel).To(item => item.SubTitle);
                set.Apply();
            });
        }

        /// <summary>
        /// Awakes from nib.
        /// </summary>
        public override void AwakeFromNib()
        {
            base.AwakeFromNib();

            titleLabel.TextColor = UIColorUtility.FromHex(ACColors.BLACK_COLOR);
            valueLabel.TextColor = UIColorUtility.FromHex(ACColors.TEXT_GRAY_COLOR);
        }
    }
}
