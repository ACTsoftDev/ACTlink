using System;
using System.Globalization;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using MvvmCross.Platform.Core;
using ObjCRuntime;
using UIKit;

namespace actchargers.iOS
{
	public partial class DatePickerSwitchTableViewCell : MvxTableViewCell
	{
        UIDatePicker datePicker;

		public static readonly NSString Key = new NSString("DatePickerSwitchTableViewCell");
		public static readonly UINib Nib;

		static DatePickerSwitchTableViewCell()
		{
			Nib = UINib.FromName("DatePickerSwitchTableViewCell", NSBundle.MainBundle);
		}

		protected DatePickerSwitchTableViewCell(IntPtr handle) : base(handle)
		{
			// Note: this .ctor should not contain any initialization logic.

			SelectionStyle = UITableViewCellSelectionStyle.None;
			this.DelayBind(() =>
			{
				var set = this.CreateBindingSet<DatePickerSwitchTableViewCell, ListViewItem>();
				set.Bind(titleLabel).To(item => item.Title);
				set.Bind(valueTextField).To(item => item.Text);
                //set.Bind(valueTextField).For(TF => TF.Enabled).To(item => item.IsSwitchEnabled);
				set.Bind(valueSwitch).For(valueLabel => valueLabel.On).To(item => item.IsSwitchEnabled);
				set.Apply();
			});
		}

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();
            titleLabel.TextColor = UIColorUtility.FromHex(ACColors.TEXT_GRAY_COLOR);

            //DateTimeTFInputView implementation
            var views = NSBundle.MainBundle.LoadNib("DatePickerInputView", this, null);
            DatePickerInputView inputView = Runtime.GetNSObject(views.ValueAt(0)) as DatePickerInputView;
            inputView.Frame = new CoreGraphics.CGRect(0, 0, Frame.Width, Frame.Height);
            valueTextField.InputView = inputView;

            NSDateFormatter dateFormat = new NSDateFormatter();
            dateFormat.DateFormat = ACConstants.DATE_TIME_FORMAT_IOS_UI;

            inputView.DatePickerView.ValueChanged += (object sender1, EventArgs e1) =>
            {
                ListViewItem item = this.DataContext as ListViewItem;
                Console.WriteLine("dateFormat.ToString(datePicker.datePickerView.Date) = {0}", dateFormat.ToString(inputView.DatePickerView.Date));
                item.Text = valueTextField.Text = dateFormat.ToString(inputView.DatePickerView.Date);
                item.Date = DateTime.ParseExact(item.Text, ACConstants.DATE_TIME_FORMAT_IOS_UI, CultureInfo.InvariantCulture);
            };

            inputView.DoneButton.Clicked += (sender, e) =>
            {
                ListViewItem item = this.DataContext as ListViewItem;
                item.Text = valueTextField.Text = dateFormat.ToString(inputView.DatePickerView.Date);
                item.Date = DateTime.ParseExact(item.Text, ACConstants.DATE_TIME_FORMAT_IOS_UI, CultureInfo.InvariantCulture);
                valueTextField.ResignFirstResponder();
            };

            inputView.CancelButton.Clicked += (sender, e) =>
            {
                valueTextField.ResignFirstResponder();
            };

            /// <summary>
            /// ShouldBeginEditing
            /// </summary>
            valueTextField.ShouldBeginEditing += delegate
            {
                var bindable1 = this as IMvxDataConsumer;
                ListViewItem item = bindable1.DataContext as ListViewItem;
                inputView.DatePickerView.MinimumDate = ConvertDateTimeToNSDate(item.MinDate.Date);
                inputView.DatePickerView.MaximumDate = ConvertDateTimeToNSDate(item.MaxDate.Date);
                if (item.IsSwitchEnabled)
                {
                    return false;
                }
                return true;
            };
        }

        public NSDate ConvertDateTimeToNSDate(DateTime date)
        {
            DateTime newDate = TimeZone.CurrentTimeZone.ToLocalTime(
                new DateTime(2001, 1, 1, 0, 0, 0));
            return NSDate.FromTimeIntervalSinceReferenceDate(
                (date - newDate).TotalSeconds);
        }
	}
}