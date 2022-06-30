using System;
using UIKit;
namespace actchargers.iOS
{
    public class CustomNavigationController : UINavigationController
    {
        public CustomNavigationController(UIViewController root) : base(root)
        {
        }

        //public override bool ShouldAutorotate()
        //{
        //    return true;
        //}

        //public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations()
        //{
        //    return UIInterfaceOrientationMask.All;
        //}

        //public override UIInterfaceOrientation PreferredInterfaceOrientationForPresentation()
        //{
        //    return UIInterfaceOrientation.Portrait;
        //}
    }
}
