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
    [Register ("PowerSnapshotsTableViewCell")]
    partial class PowerSnapshotsTableViewCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel Current { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel Id { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel Power { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel Time { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel Voltage { get; set; }

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

            if (Power != null) {
                Power.Dispose ();
                Power = null;
            }

            if (Time != null) {
                Time.Dispose ();
                Time = null;
            }

            if (Voltage != null) {
                Voltage.Dispose ();
                Voltage = null;
            }
        }
    }
}