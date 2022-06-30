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
    [Register ("PMFaultsTableViewCell")]
    partial class PMFaultsTableViewCell
    {
        [Outlet]
        UIKit.UILabel Date { get; set; }


        [Outlet]
        UIKit.UILabel ID { get; set; }


        [Outlet]
        UIKit.UILabel Power_Module_ID { get; set; }


        [Outlet]
        UIKit.UILabel Sequence { get; set; }


        [Outlet]
        UIKit.UILabel Valid { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (Date != null) {
                Date.Dispose ();
                Date = null;
            }

            if (ID != null) {
                ID.Dispose ();
                ID = null;
            }

            if (Power_Module_ID != null) {
                Power_Module_ID.Dispose ();
                Power_Module_ID = null;
            }

            if (Sequence != null) {
                Sequence.Dispose ();
                Sequence = null;
            }

            if (Valid != null) {
                Valid.Dispose ();
                Valid = null;
            }
        }
    }
}