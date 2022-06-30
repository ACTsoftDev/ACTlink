using System;
using Android.Content;
using Android.Graphics;
using Android.Runtime;
using Android.Util;

namespace actchargers.Droid
{
	public class CiTextViewWhite : CiTextView
    {
        public CiTextViewWhite (Context context, IAttributeSet attr, int defStyle) : base (context, attr, defStyle)
        {
        }

        public CiTextViewWhite (Context context, IAttributeSet attr) : base (context, attr)
        {
        }

        public CiTextViewWhite (Context context) : base (context)
        {
        }

        protected CiTextViewWhite (IntPtr javaReference, JniHandleOwnership transfer) : base (javaReference, transfer)
        {
        }

        public override void Init()
        {
            base.Init ();

            SetTextColor(Color.White);
        }

    }
}
