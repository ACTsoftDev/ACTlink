using System;
using Android.Content;
using Android.Graphics;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace actchargers.Droid
{
	public class CiTextView : TextView
	{
		public CiTextView(Context context, IAttributeSet attr, int defStyle) : base(context, attr, defStyle)
		{
			SetTypeface(context, attr);
			Init();
		}

		public CiTextView(Context context, IAttributeSet attr) : base(context, attr)
		{
			SetTypeface(context, attr);
			Init();
		}

		public CiTextView(Context context) : base(context)
		{
			SetTypeface(context, null);
			Init();
		}

		protected CiTextView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
		{
			Init();
		}

		protected virtual void SetTypeface(Context context, IAttributeSet attr)
		{
			//Set default font
			SetTypeface(null, TypefaceStyle.Normal);

		}
		/// <summary>
		/// Init this instance.
		/// </summary>
		public virtual void Init()
		{
			//Set default font
			SetTypeface(null, TypefaceStyle.Normal);


		}


		private bool IsHidedefaultLabel ;
		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="T:actchargers.Droid.CiTextView"/> custom visibility.
		/// </summary>
		/// <value><c>true</c> if custom visibility; otherwise, <c>false</c>.</value>
		public bool CustomVisibility
		{
			get
			{
				return IsHidedefaultLabel;
			}
			set
			{
				IsHidedefaultLabel = value;
				Visibility = IsHidedefaultLabel ? Android.Views.ViewStates.Gone : Android.Views.ViewStates.Visible;
			}
		}

		private bool _isVisible = true;
		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="T:actchargers.Droid.CiTextView"/> is visible.
		/// </summary>
		/// <value><c>true</c> if is visible; otherwise, <c>false</c>.</value>
		public bool IsVisible
		{
			get
			{
				return _isVisible;
			}
			set
			{
				_isVisible = value;
				Visibility = _isVisible ? ViewStates.Visible : ViewStates.Gone;

			}
		}



		private bool _isEnabled;
		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="T:actchargers.Droid.CiTextView"/> is enabled.
		/// </summary>
		/// <value><c>true</c> if is enabled; otherwise, <c>false</c>.</value>
		public bool IsEnabled
		{
			get
			{
				return _isEnabled;
			}
			set
			{
				_isEnabled = value;
				Alpha = _isEnabled ? 1.0f : 0.5f;
			}
		}
	}
}
