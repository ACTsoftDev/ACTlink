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
    [Register ("PmLiveTableViewCell")]
    partial class PmLiveTableViewCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel Current { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel Id { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel PmState { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel PmVoltage { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel Rating { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (Current != null) {
                Current.Dispose ();
                Current = null;
            }

            if (Id != null) {
                Id.Dispose ();
                Id = null;
            }

            if (PmState != null) {
                PmState.Dispose ();
                PmState = null;
            }

            if (PmVoltage != null) {
                PmVoltage.Dispose ();
                PmVoltage = null;
            }

            if (Rating != null) {
                Rating.Dispose ();
                Rating = null;
            }
        }
    }
}