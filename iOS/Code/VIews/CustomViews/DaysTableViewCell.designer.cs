// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace actchargers.iOS
{
    [Register ("DaysTableViewCell")]
    partial class DaysTableViewCell
    {
        [Outlet]
        actchargers.iOS.DaysRoundedRectButton fridayButton { get; set; }


        [Outlet]
        actchargers.iOS.DaysRoundedRectButton modayButton { get; set; }


        [Outlet]
        actchargers.iOS.DaysRoundedRectButton saturdayButton { get; set; }


        [Outlet]
        actchargers.iOS.DaysRoundedRectButton sundayButton { get; set; }


        [Outlet]
        actchargers.iOS.DaysRoundedRectButton thursdayButton { get; set; }


        [Outlet]
        UIKit.UILabel titleLabel { get; set; }


        [Outlet]
        actchargers.iOS.DaysRoundedRectButton tuesdayButton { get; set; }


        [Outlet]
        actchargers.iOS.DaysRoundedRectButton wednesdayButton { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (fridayButton != null) {
                fridayButton.Dispose ();
                fridayButton = null;
            }

            if (modayButton != null) {
                modayButton.Dispose ();
                modayButton = null;
            }

            if (saturdayButton != null) {
                saturdayButton.Dispose ();
                saturdayButton = null;
            }

            if (sundayButton != null) {
                sundayButton.Dispose ();
                sundayButton = null;
            }

            if (thursdayButton != null) {
                thursdayButton.Dispose ();
                thursdayButton = null;
            }

            if (titleLabel != null) {
                titleLabel.Dispose ();
                titleLabel = null;
            }

            if (tuesdayButton != null) {
                tuesdayButton.Dispose ();
                tuesdayButton = null;
            }

            if (wednesdayButton != null) {
                wednesdayButton.Dispose ();
                wednesdayButton = null;
            }
        }
    }
}