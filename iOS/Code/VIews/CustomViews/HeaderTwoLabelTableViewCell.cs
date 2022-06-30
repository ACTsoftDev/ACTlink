using System;

using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using UIKit;

namespace actchargers.iOS
{
    public partial class HeaderTwoLabelTableViewCell : MvxTableViewCell
    {
        public static readonly NSString Key = new NSString("HeaderTwoLabelTableViewCell");
        public static readonly UINib Nib;

        /// <summary>
        /// Initializes the <see cref="T:actchargers.iOS.TwoLabelTableViewCell"/> class.
        /// </summary>
        static HeaderTwoLabelTableViewCell()
        {
            Nib = UINib.FromName("HeaderTwoLabelTableViewCell", NSBundle.MainBundle);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:actchargers.iOS.TwoLabelTableViewCell"/> class.
        /// </summary>
        /// <param name="handle">Handle.</param>
        protected HeaderTwoLabelTableViewCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.

            /// <summary>
            /// SelectionStyle None.
            /// </summary>
            SelectionStyle = UITableViewCellSelectionStyle.None;
            this.DelayBind(() =>
            {
                /// <summary>
                /// Binding ListViewItem properties to TwoLabelTableViewCell properties
                /// </summary>
                var set = this.CreateBindingSet<HeaderTwoLabelTableViewCell, ListViewItem>();
                set.Bind(headerTitle).To(item => item.ItemHeader);
                set.Bind(titleLabel).To(item => item.Title);
                set.Bind(valueLabel).To(item => item.SubTitle);
                set.Bind(title2Label).To(item => item.Title2);
                set.Bind(value2Label).To(item => item.SubTitle2);
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
            title2Label.TextColor = UIColorUtility.FromHex(ACColors.BLACK_COLOR);
            value2Label.TextColor = UIColorUtility.FromHex(ACColors.TEXT_GRAY_COLOR);
        }
    }
}