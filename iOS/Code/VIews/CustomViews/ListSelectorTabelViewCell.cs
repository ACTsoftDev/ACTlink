using MvvmCross.Binding.iOS.Views;

using System;

using Foundation;
using UIKit;
using MvvmCross.Binding.BindingContext;

namespace actchargers.iOS
{
    public partial class ListSelectorTabelViewCell : MvxTableViewCell
    {
        public static readonly NSString Key = new NSString("ListSelectorTabelViewCell");
        public static readonly UINib Nib;

        /// <summary>
        /// Initializes the <see cref="T:actchargers.iOS.ListSelectorTabelViewCell"/> class.
        /// </summary>
        static ListSelectorTabelViewCell()
        {
            Nib = UINib.FromName("ListSelectorTabelViewCell", NSBundle.MainBundle);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:actchargers.iOS.ListSelectorTabelViewCell"/> class.
        /// </summary>
        /// <param name="handle">Handle.</param>
        protected ListSelectorTabelViewCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.

            SelectionStyle = UITableViewCellSelectionStyle.None;
            this.DelayBind(() =>
            {
                //Binding ListViewItem with TabelViewCell
                var set = this.CreateBindingSet<ListSelectorTabelViewCell, ListViewItem>();
                set.Bind(titleLbl).To(item => item.Title);
                set.Bind(valueLbl).To(item => item.Text);
                set.Apply();
            });
        }

        /// <summary>
        /// Awakes from nib.
        /// </summary>
        public override void AwakeFromNib()
        {
            base.AwakeFromNib();
            valueLbl.TextColor = UIColor.LightGray;
        }
    }
}