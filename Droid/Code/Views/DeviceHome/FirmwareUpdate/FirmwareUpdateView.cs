using Android.OS;
using Android.Views;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Binding.Droid.Views;

namespace actchargers.Droid
{
    public class FirmwareUpdateView : BasicListFragment
	{
		public override View OnCreateView
        (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			base.OnCreateView(inflater, container, savedInstanceState);

            var view = this.BindingInflate(Resource.Layout.BasicListLayout, null);

            var listView = view.FindViewById<MvxListView>(Resource.Id.listView);

            InitList(listView);

            return view;
		}
	}
}
