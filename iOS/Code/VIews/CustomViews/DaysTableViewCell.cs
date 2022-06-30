using System;

using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using UIKit;

namespace actchargers.iOS
{
    public partial class DaysTableViewCell : MvxTableViewCell
    {
        public static readonly NSString Key = new NSString("DaysTableViewCell");
        public static readonly UINib Nib;

        static DaysTableViewCell()
        {
            Nib = UINib.FromName("DaysTableViewCell", NSBundle.MainBundle);
        }

        protected DaysTableViewCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.

            SelectionStyle = UITableViewCellSelectionStyle.None;
            this.DelayBind(() =>
            {
                var set = this.CreateBindingSet<DaysTableViewCell, ListViewItem>();

                set.Bind(titleLabel).To(item => item.Title);

                //set.Bind(modayButton).For("Title").To(item => item.MONDAY);
                //set.Bind(modayButton).For(btn => btn.IsSelected).To(item => item.IsMondaySelected);

                //set.Bind(tuesdayButton).For("Title").To(item => item.TUESDAY);
                //set.Bind(tuesdayButton).For(btn => btn.IsSelected).To(item => item.IsTuesdaySelected);

                //set.Bind(wednesdayButton).For("Title").To(item => item.WEDNESDAY);
                //set.Bind(wednesdayButton).For(btn => btn.IsSelected).To(item => item.IsWednusdaySelected);

                //set.Bind(thursdayButton).For("Title").To(item => item.THURSDAY);
                //set.Bind(thursdayButton).For(btn => btn.IsSelected).To(item => item.IsThursdaySelected);

                //set.Bind(fridayButton).For("Title").To(item => item.FRIDAY);
                //set.Bind(fridayButton).For(btn => btn.IsSelected).To(item => item.IsFridaySelected);

                //set.Bind(saturdayButton).For("Title").To(item => item.SATURDAY);
                //set.Bind(saturdayButton).For(btn => btn.IsSelected).To(item => item.IsSaturdaySelected);

                //set.Bind(sundayButton).For("Title").To(item => item.SUNDAY);
                //set.Bind(sundayButton).For(btn => btn.IsSelected).To(item => item.IsSundaySelected);

                set.Apply();
            });
        }
    }
}
