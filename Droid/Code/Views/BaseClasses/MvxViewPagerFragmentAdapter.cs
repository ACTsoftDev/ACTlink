using System;
using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Android.Support.V4.App;
using MvvmCross.Core.ViewModels;
using MvvmCross.Droid.Support.V4;

namespace actchargers.Droid
{
	public class MvxViewPagerFragmentAdapter
	: FragmentPagerAdapter
	{
		public class FragmentInfo
		{
			public string Title { get; set; }
			public Type FragmentType { get; set; }
			public IMvxViewModel ViewModel { get; set; }
		}

		private readonly Context _context;
		/// <summary>
		/// Initializes a new instance of the <see cref="T:actchargers.Droid.MvxViewPagerFragmentAdapter"/> class.
		/// </summary>
		/// <param name="context">Context.</param>
		/// <param name="fragmentManager">Fragment manager.</param>
		/// <param name="fragments">Fragments.</param>
		public MvxViewPagerFragmentAdapter(
		  Context context, FragmentManager fragmentManager, IEnumerable<FragmentInfo> fragments)
			: base(fragmentManager)
		{
			_context = context;
			Fragments = fragments;
		}

		public IEnumerable<FragmentInfo> Fragments { get; private set; }
		/// <summary>
		/// Gets the fragment count of view pager.
		/// </summary>
		/// <value>The count.</value>
		public override int Count
		{
			get { return Fragments.Count(); }
		}
		/// <summary>
		/// Gets the item.
		/// </summary>
		/// <returns>return fragment based on postion from viewpager</returns>
		/// <param name="position">Position.</param>
		public override Fragment GetItem(int position)
		{
			var frag = Fragments.ElementAt(position);
			var fragment = Fragment.Instantiate(_context,
				Java.Lang.Class.FromType(frag.FragmentType).Name);
			((MvxFragment)fragment).DataContext = frag.ViewModel;
			return fragment;
		}
		/// <summary>
		/// Fragments the name of the java.
		/// </summary>
		/// <returns>The java name.</returns>
		/// <param name="fragmentType">Fragment type.</param>
		protected virtual string FragmentJavaName(Type fragmentType)
		{
			var namespaceText = fragmentType.Namespace ?? "";
			if (namespaceText.Length > 0)
			{
				namespaceText = namespaceText.ToLowerInvariant() + ".";
			}
			return namespaceText + fragmentType.Name;
		}

		public override Java.Lang.ICharSequence GetPageTitleFormatted(int position) { return new Java.Lang.String(Fragments.ElementAt(position).Title); }
	}
}
