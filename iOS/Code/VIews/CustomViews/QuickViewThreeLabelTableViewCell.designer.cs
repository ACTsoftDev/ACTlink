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
    [Register ("QuickViewThreeLabelTableViewCell")]
    partial class QuickViewThreeLabelTableViewCell
    {
        [Outlet]
        UIKit.UILabel ahrlabel { get; set; }


        [Outlet]
        UIKit.UILabel ahrvalue { get; set; }


        [Outlet]
        UIKit.UILabel durationhour { get; set; }


        [Outlet]
        UIKit.UILabel durationlabel { get; set; }


        [Outlet]
        UIKit.UILabel durationmin { get; set; }


        [Outlet]
        UIKit.UILabel durationsec { get; set; }


        [Outlet]
        UIKit.UILabel kwhrlabel { get; set; }


        [Outlet]
        UIKit.UILabel kwhrvalue { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (ahrlabel != null) {
                ahrlabel.Dispose ();
                ahrlabel = null;
            }

            if (ahrvalue != null) {
                ahrvalue.Dispose ();
                ahrvalue = null;
            }

            if (durationhour != null) {
                durationhour.Dispose ();
                durationhour = null;
            }

            if (durationlabel != null) {
                durationlabel.Dispose ();
                durationlabel = null;
            }

            if (durationmin != null) {
                durationmin.Dispose ();
                durationmin = null;
            }

            if (durationsec != null) {
                durationsec.Dispose ();
                durationsec = null;
            }

            if (kwhrlabel != null) {
                kwhrlabel.Dispose ();
                kwhrlabel = null;
            }

            if (kwhrvalue != null) {
                kwhrvalue.Dispose ();
                kwhrvalue = null;
            }
        }
    }
}