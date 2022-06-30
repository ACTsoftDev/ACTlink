using System;

using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using UIKit;

namespace actchargers.iOS
{
    public partial class PMFaultsTableViewCell : MvxTableViewCell
    {
        public static readonly NSString Key = new NSString("PMFaultsTableViewCell");
        public static readonly UINib Nib;

        static PMFaultsTableViewCell()
        {
            Nib = UINib.FromName("PMFaultsTableViewCell", NSBundle.MainBundle);
        }

        protected PMFaultsTableViewCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
            SelectionStyle = UITableViewCellSelectionStyle.None;

            this.DelayBind(() =>
            {
                var set = this.CreateBindingSet<PMFaultsTableViewCell, PMFaultsModel>();
                set.Bind(Sequence).To(item => item.faultID);
                set.Bind(ID).To(item => item.debugHeader);
                set.Bind(Date).To(item => item.faultTime);
                set.Bind(Power_Module_ID).To(item => item.DebugString);
                set.Bind(Valid).To(item => item.isValidCRC7);
                set.Apply();
            });
        }
    }
}
