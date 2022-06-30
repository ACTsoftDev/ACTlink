using System;
using MvvmCross.Binding.BindingContext;
using MvvmCross.iOS.Views;
using UIKit;

namespace actchargers.iOS
{
    public partial class BaseView : MvxViewController
    {
        public BaseView() : base("BaseView", null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:actchargers.iOS.BaseView"/> class.
        /// </summary>
        /// <param name="nibName">Nib name.</param>
        /// <param name="bundle">Bundle.</param>
        protected BaseView(string nibName, Foundation.NSBundle bundle)
            : base(nibName, bundle)
        {

        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.

            UILabel titleLabel = new UILabel(new CoreGraphics.CGRect(0, 0, 25, 44));
            titleLabel.BackgroundColor = UIColor.Clear;
            titleLabel.TextColor = UIColorUtility.FromHex(ACColors.BLACK_COLOR);
            titleLabel.Font = UIFont.BoldSystemFontOfSize(16);
            titleLabel.TextAlignment = UITextAlignment.Center;
            titleLabel.Lines = 0;

            var bindingSet = this.CreateBindingSet<BaseView, BaseViewModel>();
            bindingSet.Bind(titleLabel).To((BaseViewModel vm) => vm.ViewTitle);
            bindingSet.Apply();

            NavigationItem.TitleView = titleLabel;

        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }
    }
}