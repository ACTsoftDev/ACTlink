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
    [Register ("DatePickerInputView")]
    partial class DatePickerInputView
    {
        [Outlet]
        UIKit.UIBarButtonItem cancelBarButton { get; set; }


        [Outlet]
        UIKit.UIDatePicker datePicker { get; set; }


        [Outlet]
        UIKit.UIBarButtonItem doneBarButton { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (cancelBarButton != null) {
                cancelBarButton.Dispose ();
                cancelBarButton = null;
            }

            if (datePicker != null) {
                datePicker.Dispose ();
                datePicker = null;
            }

            if (doneBarButton != null) {
                doneBarButton.Dispose ();
                doneBarButton = null;
            }
        }
    }
}