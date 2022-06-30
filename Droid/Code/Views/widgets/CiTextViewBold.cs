using System;
using Android.Content;
using Android.Graphics;
using Android.Runtime;
using Android.Util;

namespace actchargers.Droid
{
	public class  CiTextViewBold   : CiTextView
    {
        public  CiTextViewBold (Context context, IAttributeSet attr, int defStyle) : base (context, attr, defStyle)
        {
        }

        public  CiTextViewBold (Context context, IAttributeSet attr) : base (context, attr)
        {
        }

        public  CiTextViewBold (Context context) : base (context)
        {
        }

        protected  CiTextViewBold (IntPtr javaReference, JniHandleOwnership transfer) : base (javaReference, transfer)
        {
        }

        public override void Init ()
        {
            base.Init ();
			SetTypeface(null, TypefaceStyle.Bold);
        }

    }
}
