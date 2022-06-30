using System;

using Foundation;
using MvvmCross.Binding.iOS.Views;
using UIKit;

namespace actchargers.iOS
{
    public partial class SyncSitesHeaderView : MvxView
    {
        public UILabel HeaderLabel { get; set; }
        public UIButton CheckBtn { get; set; }
        public UIButton ExpandBtn { get; set; }

        protected SyncSitesHeaderView(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();

            HeaderLabel = HeaderTitle;
            CheckBtn = checkBtn;
            ExpandBtn = expandBtn;


        }
    }
}
