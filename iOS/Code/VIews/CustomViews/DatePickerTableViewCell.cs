using System;
using System.Globalization;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using ObjCRuntime;
using UIKit;

namespace actchargers.iOS
{
    public partial class DatePickerTableViewCell : MvxTableViewCell
    {
        public static readonly NSString Key = new NSString("DatePickerTableViewCell");
        public static readonly UINib Nib;

        /// <summary>
        /// Initializes the <see cref="T:actchargers.iOS.DatePickerTableViewCell"/> class.
        /// </summary>
        static DatePickerTableViewCell()
        {
            Nib = UINib.FromName("DatePickerTableViewCell", NSBundle.MainBundle);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:actchargers.iOS.DatePickerTableViewCell"/> class.
        /// </summary>
        /// <param name="handle">Handle.</param>
        protected DatePickerTableViewCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.

            SelectionStyle = UITableViewCellSelectionStyle.None;
            this.DelayBind(() =>
            {
                var set = this.CreateBindingSet<DatePickerTableViewCell, ListViewItem>();
                set.Bind(titleLabel).To(item => item.Title);
                set.Bind(valueTextField).To(item => item.Text);
                set.Apply();
            });
        }

        /// <summary>
        /// Awakes from nib.
        /// </summary>
        public override void AwakeFromNib()
        {
            base.AwakeFromNib();

            titleLabel.TextColor = UIColorUtility.FromHex(ACColors.TEXT_GRAY_COLOR);


            valueTextField.ShouldBeginEditing += delegate
            {
                ListViewItem item = this.DataContext as ListViewItem;
                if (item.CellType == ACUtility.CellTypes.TimePicker)
                {
                    var views = NSBundle.MainBundle.LoadNib("DatePickerInputView", this, null);
                    DatePickerInputView inputView = Runtime.GetNSObject(views.ValueAt(0)) as DatePickerInputView;
                    inputView.Frame = new CoreGraphics.CGRect(0, 0, Frame.Width, Frame.Height);
                    valueTextField.InputView = inputView;

                    NSDateFormatter dateFormat = new NSDateFormatter();
                    dateFormat.DateFormat = "HH:mm";

                    inputView.DatePickerView.Mode = UIDatePickerMode.Time;
                    inputView.DatePickerView.Locale = new NSLocale("en_GB");

                    inputView.DatePickerView.ValueChanged += (object sender1, EventArgs e1) =>
                    {
                        item.Text = valueTextField.Text = dateFormat.ToString(inputView.DatePickerView.Date);
                    };

                    inputView.DoneButton.Clicked += (sender, e) =>
                    {
                        item.Text = valueTextField.Text = dateFormat.ToString(inputView.DatePickerView.Date);
                        valueTextField.ResignFirstResponder();
                    };

                    inputView.CancelButton.Clicked += (sender, e) =>
                    {
                        valueTextField.ResignFirstResponder();
                    };
                }
                else
                {
                    //DateTimeTFInputView implementation
                    var views = NSBundle.MainBundle.LoadNib("DatePickerInputView", this, null);
                    DatePickerInputView inputView = Runtime.GetNSObject(views.ValueAt(0)) as DatePickerInputView;
                    inputView.Frame = new CoreGraphics.CGRect(0, 0, Frame.Width, Frame.Height);
                    valueTextField.InputView = inputView;

                    NSDateFormatter dateFormat = new NSDateFormatter();
                    dateFormat.DateFormat = ACConstants.DATE_TIME_FORMAT_IOS_UI;

                    inputView.DatePickerView.MinimumDate = ConvertDateTimeToNSDate(item.MinDate.Date);
                    inputView.DatePickerView.MaximumDate = ConvertDateTimeToNSDate(item.MaxDate.Date);

                    inputView.DatePickerView.ValueChanged += (object sender1, EventArgs e1) =>
                    {
                        Console.WriteLine("dateFormat.ToString(datePicker.datePickerView.Date) = {0}", dateFormat.ToString(inputView.DatePickerView.Date));
                        item.Text = valueTextField.Text = dateFormat.ToString(inputView.DatePickerView.Date);
                        item.Date = DateTime.ParseExact(item.Text, ACConstants.DATE_TIME_FORMAT_IOS_UI, CultureInfo.InvariantCulture);
                    };

                    inputView.DoneButton.Clicked += (sender, e) =>
                    {
                        item.Text = valueTextField.Text = dateFormat.ToString(inputView.DatePickerView.Date);
                        item.Date = DateTime.ParseExact(item.Text, ACConstants.DATE_TIME_FORMAT_IOS_UI, CultureInfo.InvariantCulture);
                        valueTextField.ResignFirstResponder();
                    };

                    inputView.CancelButton.Clicked += (sender, e) =>
                    {
                        valueTextField.ResignFirstResponder();
                    };
                }

                tfHighlightViewHeightConstraint.Constant = 2;
                return true;
            };

            valueTextField.ShouldEndEditing += delegate
            {
                tfHighlightViewHeightConstraint.Constant = 1;
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

        public override bool CanPerform(Selector action, NSObject withSender)
        {
            NSOperationQueue.MainQueue.AddOperation(() =>
            {
                UIMenuController.SharedMenuController.SetMenuVisible(false, false);
            });

            return base.CanPerform(action, withSender);
        }
    }
}
