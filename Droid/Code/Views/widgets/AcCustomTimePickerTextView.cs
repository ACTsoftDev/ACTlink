using System;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Runtime;
using Android.Util;
using Android.Widget;

namespace actchargers.Droid
{
    public class AcCustomTimePickerTextView : TextView
	{
        public ListViewItem DataContext { get; set; }
		public AcCustomTimePickerTextView(Context context, IAttributeSet attr, int defStyle) : base(context, attr, defStyle)
		{
			Init();
		}

		public AcCustomTimePickerTextView(Context context, IAttributeSet attr) : base(context, attr)
		{
			Init();
		}

		public AcCustomTimePickerTextView(Context context) : base(context)
		{
			Init();
		}

		protected AcCustomTimePickerTextView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
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
			ShowTimeDialog();
		}

		void ShowTimeDialog()
		{
            TimePickerDialog dlg = new TimePickerDialog(Context, OnTimeSet, DateTime.Now.Hour, DateTime.Now.Minute, true);
			dlg.Show();
		}

		void OnTimeSet(object sender, TimePickerDialog.TimeSetEventArgs e)
		{
            TimeSpan Time = new TimeSpan(e.HourOfDay, e.Minute, 0);
            Text = Time.ToString(@"hh\:mm");
            DataContext.Text = Text;
		}
	}
}


