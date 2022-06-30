using System;

using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform.Core;
using UIKit;

namespace actchargers.iOS
{
    public partial class LabelTextFieldTableViewCell : MvxTableViewCell
    {
        public static readonly NSString Key = new NSString("LabelTextFieldTableViewCell");
        public static readonly UINib Nib;

        /// <summary>
        /// Initializes the <see cref="T:actchargers.iOS.LabelTextFieldTableViewCell"/> class.
        /// </summary>
        static LabelTextFieldTableViewCell()
        {
            Nib = UINib.FromName("LabelTextFieldTableViewCell", NSBundle.MainBundle);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:actchargers.iOS.LabelTextFieldTableViewCell"/> class.
        /// </summary>
        /// <param name="handle">Handle.</param>
        protected LabelTextFieldTableViewCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.

            SelectionStyle = UITableViewCellSelectionStyle.None;
            this.DelayBind(() =>
            {
                var set = this.CreateBindingSet<LabelTextFieldTableViewCell, ListViewItem>();
                set.Bind(titleLabel).To(item => item.Title);
                set.Bind(valueTextField).To(item => item.Text);
                set.Bind(valueTextField).For(tf => tf.Enabled).To(item => item.IsEditable);
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
            /// ShouldBeginEditing
            /// </summary>
            valueTextField.ShouldBeginEditing += delegate
            {
                var bindable1 = this as IMvxDataConsumer;
                ListViewItem item2 = bindable1.DataContext as ListViewItem;
                if (item2 != null)
                {
                    switch (item2.TextInputType)
                    {
                        case ACUtility.InputType.Number:
                            valueTextField.KeyboardType = UIKeyboardType.NumberPad;
                            break;
                        case ACUtility.InputType.Decimal:
                            valueTextField.KeyboardType = UIKeyboardType.DecimalPad;
                            break;
                        default:
                            break;
                    }
                }

                tfHighlightViewHeightConstraint.Constant = 2;
                return true;
            };

            /// <summary>
            /// ShouldEndEditing
            /// </summary>
            valueTextField.ShouldEndEditing += delegate
            {
                tfHighlightViewHeightConstraint.Constant = 1;
                return true;
            };

            /// <summary>
            /// ShouldReturn
            /// </summary>
            valueTextField.ShouldReturn += delegate
            {
                valueTextField.EndEditing(true);
                return true;
            };
          
            titleLabel.TextColor = UIColorUtility.FromHex(ACColors.TEXT_GRAY_COLOR);
            valueTextField.TextColor = UIColorUtility.FromHex(ACColors.BLACK_COLOR);
        }

        public override void TouchesEnded(NSSet touches, UIEvent evt)
        {
            base.TouchesEnded(touches, evt);
            valueTextField.ResignFirstResponder();
        }
    }
}