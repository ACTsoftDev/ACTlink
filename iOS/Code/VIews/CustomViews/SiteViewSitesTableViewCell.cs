using System;

using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using UIKit;

namespace actchargers.iOS
{
    public partial class SiteViewSitesTableViewCell : MvxTableViewCell
    {
        public static readonly NSString Key = new NSString("SiteViewSitesTableViewCell");
        public static readonly UINib Nib;

        static SiteViewSitesTableViewCell()
        {
            Nib = UINib.FromName("SiteViewSitesTableViewCell", NSBundle.MainBundle);
        }

        protected SiteViewSitesTableViewCell(IntPtr handle) : base(handle)
        {
            BackgroundColor = UIColor.Clear;

            Initialize();
        }

        void Initialize()
        {
            this.DelayBind(() =>
            {
                var set = this.CreateBindingSet<SiteViewSitesTableViewCell, SynchSiteObjects>();
                set.Bind(siteNameLbl).To(item => item.SiteName);
                set.Bind(customerNameLbl).To(item => item.CustomerName);
                set.Apply();
            });
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();

            siteNameLbl.TextColor = UIColorUtility.FromHex(ACColors.DARK_BLUE_COLOR);
            customerNameLbl.TextColor = UIColorUtility.FromHex(ACColors.TEXT_GRAY_COLOR);
        }
    }
}
