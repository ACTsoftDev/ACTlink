using Android.Content;
using Android.Graphics;
using Android.Views;

namespace actchargers.Droid
{
    public class CustomView : View
    {

        Bitmap myBitmap;
        public CustomView(Context context, Bitmap myBitmap) : base(context)
        {
            this.myBitmap = myBitmap;

        }
        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);
            canvas.DrawBitmap(myBitmap, 0, 0, null);
        }
    }
}

//private class myView : View
//{
//    Bitmap myBitmap;
//    public myView(Context context, Bitmap myBitmap) : base(context)
//    {
//        this.myBitmap = myBitmap;

//    }
//    protected override void OnDraw(Canvas canvas)
//    {
//        base.OnDraw(canvas);
//        canvas.DrawBitmap(myBitmap, 0, 0, null);
//    }

//}