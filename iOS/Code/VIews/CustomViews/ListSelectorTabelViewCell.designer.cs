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
    [Register ("ListSelectorTabelViewCell")]
    partial class ListSelectorTabelViewCell
    {
        [Outlet]
        UIKit.UILabel titleLbl { get; set; }


        [Outlet]
        UIKit.UILabel valueLbl { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (titleLbl != null) {
                titleLbl.Dispose ();
                titleLbl = null;
            }

            if (valueLbl != null) {
                valueLbl.Dispose ();
                valueLbl = null;
            }
        }
    }
}