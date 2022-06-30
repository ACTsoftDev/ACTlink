using System;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;

namespace actchargers.iOS
{
	//[Register("PickerInputView")]
	public partial class PickerInputView : MvxView
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="T:actchargers.iOS.PickerInputView"/> class.
		/// </summary>
		public PickerInputView()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:actchargers.iOS.PickerInputView"/> class.
		/// </summary>
		/// <param name="h">The height.</param>
		public PickerInputView(IntPtr h) : base (h)
		{
		}

		/// <summary>
		/// Awakes from nib.
		/// </summary>
		public override void AwakeFromNib()
		{
			base.AwakeFromNib();

			var pickerViewModel = new MvxPickerViewModel(pickerView);
			pickerView.Model = pickerViewModel;
			pickerView.ShowSelectionIndicator = true;			
		}
	}
}
