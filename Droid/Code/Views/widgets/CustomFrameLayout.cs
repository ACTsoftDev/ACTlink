using Android.Content;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace actchargers.Droid
{
    public class CustomFrameLayout : FrameLayout
    {
        View sticky;

        public CustomFrameLayout(Context context, IAttributeSet attrs):base(context,attrs)
        {
            ViewTreeObserver.GlobalFocusChange += (sender, e) => {
                if (e.NewFocus == null) return;
                View baby = GetChildAt(0);

                if (e.NewFocus != baby)
                {
                    IViewParent parent = e.NewFocus.Parent;
                    while (parent != null && parent != parent.Parent)
                    {
                        if (parent == baby)
                        {
                            sticky = e.NewFocus;
                            break;
                        }
                        parent = parent.Parent;
                    }
                }

            };

            ViewTreeObserver.GlobalLayout += (sender, e) => {
               if (sticky != null)
                {
                    sticky.RequestFocus();
                    sticky.RequestFocusFromTouch();
                    sticky = null;
                } 
            };

        }

    }
}
