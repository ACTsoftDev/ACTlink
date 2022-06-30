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
    [Register ("LabelSwitchTableViewCell")]
    partial class LabelSwitchTableViewCell
    {
        [Outlet]
        UIKit.UILabel titleLabel { get; set; }


        [Outlet]
        UIKit.UISwitch valueSwitch { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (titleLabel != null) {
                titleLabel.Dispose ();
                titleLabel = null;
            }

            if (valueSwitch != null) {
                valueSwitch.Dispose ();
                valueSwitch = null;
            }
        }
    }
}