using System;

using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using UIKit;

namespace actchargers.iOS
{
    public partial class ListCollectionViewCell : MvxCollectionViewCell
    {
        public static readonly NSString Key = new NSString("ListCollectionViewCell");
        public static readonly UINib Nib;

        static ListCollectionViewCell()
        {
            Nib = UINib.FromName("ListCollectionViewCell", NSBundle.MainBundle);
        }

        protected ListCollectionViewCell(IntPtr handle) : base(handle)
        {
            Initialize();

            // Note: this .ctor should not contain any initialization logic.
        }

        /// <summary>
        /// Initialize this instance.
        /// </summary>
        void Initialize()
        {
            this.DelayBind(() =>
            {
                var set = this.CreateBindingSet<ListCollectionViewCell, DayViewItem>();
                set.Bind(gridButton).For("Title").To(o => o.Title);
                set.Bind(gridButton).For(o => o.IsSelected).To(o => o.IsSelected);
                set.Bind(gridButton).For(o => o.Tag).To(o => o.id);
                set.Bind(gridButton).To(o => o.ButtonCommand);
                set.Apply();
            });
        }
    }
}
