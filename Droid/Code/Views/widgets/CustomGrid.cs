using System;
using Android.Content;
using Android.Util;
using Android.Views;
using Android.Widget;
using Java.Lang;

namespace actchargers.Droid
{
    public class CustomGrid : MvvmCross.Binding.Droid.Views.MvxGridView
    {
        public CustomGrid(Context context, IAttributeSet attrs) : base(context, attrs)
        {
        }

        public override bool OnTouchEvent(MotionEvent e)
        {
            if (e.Action == MotionEventActions.Move)
            {
                return true;
            }
            return base.OnTouchEvent(e);
        }
        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            int expandSpec = MeasureSpec.MakeMeasureSpec(Integer.MaxValue >> 2,
                                                         MeasureSpecMode.AtMost);
            base.OnMeasure(widthMeasureSpec, expandSpec);
        }


    }
}