using System;
using Android.Content;
using Android.Graphics;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace actchargers.Droid
{
    public class AcEditText : EditText//, Android.Text.ITextWatcher
    {
        public bool HighlightOnFocus { get; set; }

        public AcEditText(Context context, IAttributeSet attr, int defStyle) : base(context, attr, defStyle)
        {
            Init();
            SetTypeface(context, attr);
        }

        public AcEditText(Context context, IAttributeSet attr) : base(context, attr)
        {
            Init();
            SetTypeface(context, attr);
        }

        public AcEditText(Context context) : base(context)
        {
            Init();
            SetTypeface(context, null);
        }

        protected AcEditText(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
            Init();
        }

        public void Init()
        {
            
            //Set HighlightOnFocus true by default
            HighlightOnFocus = true;
        }


        private void SetTypeface(Context context, IAttributeSet attr)
        {
            base.SetTypeface(null, TypefaceStyle.Normal);
        }

        private bool isVisible = true;

        public bool IsVisible
        {
            get
            {
                return isVisible;
            }
            set
            {
                isVisible = value;
                Visibility = IsVisible ? ViewStates.Visible : ViewStates.Gone;
            }
        }

        private bool _isEnabled;

        public bool IsEnabled
        {
            get
            {
                return _isEnabled;
            }
            set
            {
                _isEnabled = value;
                if (_isEnabled)
                {
                    Alpha = 1.0f;
                }
                else {
                    Alpha = 0.5f;
                }
            }
        }

        //protected override void OnFocusChanged(bool gainFocus, FocusSearchDirection direction, Rect previouslyFocusedRect)
        //{
        //    if (HighlightOnFocus)
        //    {
        //        this.SetCursorVisible(true);
        //        if (Text != null && Text.Length > 0)
        //        {
        //            SetSelection(Text.Length);
        //        }
        //        else {
        //            SetSelection(0);
        //        }

        //    }
        //    base.OnFocusChanged(gainFocus, direction, previouslyFocusedRect);
        //}

        //protected override void OnTextChanged(ICharSequence text, int start, int lengthBefore, int lengthAfter)
        //{
        //    base.OnTextChanged(text, start, lengthBefore, lengthAfter);
        //    if (lengthAfter == text.Length())
        //    {
        //        SetSelection(text.Length());
        //    }
        //}
        //void ITextWatcher.AfterTextChanged(IEditable s)
        //{
        //    //throw new NotImplementedException () 
        //}

        //void ITextWatcher.BeforeTextChanged(ICharSequence s, int start, int count, int after)
        //{
        //    //throw new NotImplementedException ();
        //}

     
    }
}
