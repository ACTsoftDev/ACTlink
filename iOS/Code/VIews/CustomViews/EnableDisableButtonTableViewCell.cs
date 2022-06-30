using System;

using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using UIKit;

namespace actchargers.iOS
{
    public partial class EnableDisableButtonTableViewCell : MvxTableViewCell
    {
        public static readonly NSString Key = new NSString("EnableDisableButtonTableViewCell");
        public static readonly UINib Nib;

        static EnableDisableButtonTableViewCell()
        {
            Nib = UINib.FromName("EnableDisableButtonTableViewCell", NSBundle.MainBundle);
        }

        protected EnableDisableButtonTableViewCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.

            SelectionStyle = UITableViewCellSelectionStyle.None;
            this.DelayBind(() =>
            {
                var set = this.CreateBindingSet<EnableDisableButtonTableViewCell, ListViewItem>();
                set.Bind(actionButton).For("Title").To(item => item.Title);
                //set.Bind(actionButton).To(item => item.ListSelectionCommand);
                set.Bind(actionButton).For(button => button.IsEditable).To(item => item.IsEditable);
                set.Apply();
            });
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();

            actionButton.TouchUpInside += delegate {
                ListViewItem item = this.DataContext as ListViewItem;
                if(item != null)
                {
                    item.ListSelectionCommand.Execute(item);
                }
            };
        }
    }
}
