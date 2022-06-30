using System;
using Android.Content;
using Android.Runtime;
using Android.Util;
using MvvmCross.Binding.Droid.Views;

namespace actchargers.Droid
{
    public class CiExpandableListView : MvxExpandableListView, Android.Widget.ExpandableListView.IOnGroupCollapseListener
    {
        public CiExpandableListView (Context context, IAttributeSet attrs)
            : this (context, attrs, new MvxExpandableListAdapter (context))
        { 
            Init ();
        }

        public CiExpandableListView (Context context, IAttributeSet attrs, MvxExpandableListAdapter adapter)
            : base (context, attrs)
        {
            Init ();
        }

        protected CiExpandableListView (IntPtr javaReference, JniHandleOwnership transfer)
            : base (javaReference, transfer)
        {
            Init ();
        }

        protected override void OnAttachedToWindow ()
        {
            ExpandAllGroups ();
            base.OnAttachedToWindow ();
        }

        public void ExpandAllGroups ()
        {
            for (int i = 0; i < ExpandableListAdapter.GroupCount; i++) {
                ExpandGroup (i);
            }
        }

        public void Init ()
        {
            SetOnGroupCollapseListener (this);
            SetBackgroundColor (Context.Resources.GetColor (Resource.Color.app_background));
            SetGroupIndicator (null);
            SetFooterDividersEnabled(false);
        }

        public void OnGroupCollapse (int groupPosition)
        {
            ExpandGroup (groupPosition);
        }
       
        //protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        //{
        //    int expandSpec = MeasureSpec.MakeMeasureSpec(Integer.MaxValue >> 2,
        //                                               MeasureSpecMode.AtMost);
        //    base.OnMeasure(widthMeasureSpec, expandSpec);

        //    //base.OnMeasure(widthMeasureSpec, heightMeasureSpec);
        //}
    }
}
