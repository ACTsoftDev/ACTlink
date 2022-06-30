using System;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Runtime;
using Android.Util;
using Android.Widget;

namespace actchargers.Droid
{
	public class AcCustomDateTextView : TextView
	{
        public ListViewItem DataContext { get; set; }
		public AcCustomDateTextView(Context context, IAttributeSet attr, int defStyle) : base(context, attr, defStyle)
		{
			Init();
		}

		public AcCustomDateTextView(Context context, IAttributeSet attr) : base(context, attr)
		{
			Init();
		}

		public AcCustomDateTextView(Context context) : base(context)
		{
			Init();
		}

		protected AcCustomDateTextView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
		{
			Init();
		}

		public void Init()
		{
			Click += ACTextViewClick;
			SetTextColor(Color.ParseColor(ACColors.TEXT_GRAY_COLOR));

		}

		void ACTextViewClick(object sender, EventArgs e)
		{
			ShowDateDialog();
		}

		void ShowDateDialog()
		{
            DateTime date = DataContext.Date; //  DateTime.Now;
			DatePickerDialog dlg = new DatePickerDialog(Context, OnDateSet, date.Year, date.Month - 1, date.Day);
            dlg.DatePicker.MinDate = new Java.Util.Date(DataContext.MinDate.Year - 1900, DataContext.MinDate.Month - 1, DataContext.MinDate.Day).Time;
            dlg.DatePicker.MaxDate = new Java.Util.Date(DataContext.MaxDate.Year - 1900, DataContext.MaxDate.Month - 1, DataContext.MaxDate.Day).Time;
            dlg.Show();
		}

		void OnDateSet(object sender, DatePickerDialog.DateSetEventArgs e)
		{
			DateTime date = e.Date;
            Text=""+date.ToString(ACConstants.DATE_TIME_FORMAT_IOS_UI);
            DataContext.Text = Text;
            DataContext.Date = date.Date;
		}
	}
}

