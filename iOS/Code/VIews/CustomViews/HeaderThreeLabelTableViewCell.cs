using System;

using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using UIKit;

namespace actchargers.iOS
{
    public partial class HeaderThreeLabelTableViewCell : MvxTableViewCell
    {
        public static readonly NSString Key = new NSString("HeaderThreeLabelTableViewCell");
        public static readonly UINib Nib;

        /// <summary>
        /// Initializes the <see cref="T:actchargers.iOS.ThreeLabelTableViewCell"/> class.
        /// </summary>
        static HeaderThreeLabelTableViewCell()
        {
            Nib = UINib.FromName("HeaderThreeLabelTableViewCell", NSBundle.MainBundle);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:actchargers.iOS.ThreeLabelTableViewCell"/> class.
        /// </summary>
        /// <param name="handle">Handle.</param>
        protected HeaderThreeLabelTableViewCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.

            //Setting SelectionStyle to None for this TableViewCell
            SelectionStyle = UITableViewCellSelectionStyle.None;
            this.DelayBind(() =>
            {
                //Binding ListViewItem with ThreeLabelTableViewCell
                var set = this.CreateBindingSet<HeaderThreeLabelTableViewCell, ListViewItem>();
                set.Bind(headerTitle).To(item => item.ItemHeader);
                set.Bind(titleLabel).To(item => item.Title);
                set.Bind(valueLabel).To(item => item.SubTitle);
                set.Bind(title2Label).To(item => item.Title2);
                set.Bind(value2Label).To(item => item.SubTitle2);
                set.Bind(title3Label).To(item => item.Title3);
                set.Bind(value3Label).To(item => item.SubTitle3);
                set.Apply();
            });
        }

        /// <summary>
        /// Awakes from nib.
        /// </summary>
        public override void AwakeFromNib()
        {
            base.AwakeFromNib();

            /// <summary>
            /// Applying Colors to labels
            /// </summary>
            titleLabel.TextColor = UIColorUtility.FromHex(ACColors.BLACK_COLOR);
            valueLabel.TextColor = UIColorUtility.FromHex(ACColors.TEXT_GRAY_COLOR);
            title2Label.TextColor = UIColorUtility.FromHex(ACColors.BLACK_COLOR);
            value2Label.TextColor = UIColorUtility.FromHex(ACColors.TEXT_GRAY_COLOR);
            title3Label.TextColor = UIColorUtility.FromHex(ACColors.BLACK_COLOR);
            value3Label.TextColor = UIColorUtility.FromHex(ACColors.TEXT_GRAY_COLOR);
        }
    }
}
