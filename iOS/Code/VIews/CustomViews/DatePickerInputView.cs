using System;
using Foundation;
using MvvmCross.Binding.iOS.Views;
using UIKit;

namespace actchargers.iOS
{
	
	public partial class DatePickerInputView: MvxView
	{
        /// <summary>
        /// Gets or sets the date picker view.
        /// </summary>
        /// <value>The date picker view.</value>
		public UIDatePicker DatePickerView { get; set; }
        /// <summary>
        /// The cancel button.
        /// </summary>
        public UIBarButtonItem CancelButton, DoneButton;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:actchargers.iOS.PickerInputView"/> class.
		/// </summary>
		/// <param name="h">The height.</param>
		public DatePickerInputView(IntPtr h) : base (h)
		{
		}

		public override void AwakeFromNib()
		{
			base.AwakeFromNib();

			DatePickerView = datePicker;

            cancelBarButton.Title = AppResources.cancel;
            CancelButton = cancelBarButton;

            doneBarButton.Title = AppResources.done;
            DoneButton = doneBarButton;

            //datePicker.MinimumDate = new NSDate();
		}
	}
}
