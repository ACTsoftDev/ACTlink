using Android.OS;
using Android.Views;
using MvvmCross.Binding.Droid.BindingContext;
namespace actchargers.Droid
{
    public class HistoryChartsView : BaseFragment
	{
		public override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Create your fragment here
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			// Use this to return your custom view for this Fragment
			// return inflater.Inflate(Resource.Layout.YourFragment, container, false);
			base.OnCreateView(inflater, container, savedInstanceState);
			View view = this.BindingInflate(Resource.Layout.HistoryChartsLayout, null);
			return view;
		}
	}
}
