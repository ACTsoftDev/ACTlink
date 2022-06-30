using Android.Content;
using Android.OS;
using Android.Views;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Binding.Droid.Views;

namespace actchargers.Droid
{
    public class ViewCyclesHistoryView : BaseFragmentWithOutMenu
    {
        public override View OnCreateView
        (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = this.BindingInflate(Resource.Layout.ViewCycleLayout, null);

            var _viewCyclesHistoryViewModel = ViewModel as ViewCyclesHistoryViewModel;

            var mListView = view.FindViewById<MvxListView>(Resource.Id.viewCycleListView);

            var adapter = new ViewCycleAdapter(view.Context, (IMvxAndroidBindingContext)BindingContext)
            {
                ItemsSource = _viewCyclesHistoryViewModel.MCB_cyclesHistoryGrid
            };

            mListView.Adapter = adapter;

            return view;
        }

        public override void OnResume()
        {
            base.OnResume();

            Activity.RequestedOrientation = OrientationManager.WIDE_ORIENTATION;
        }

        public override void OnPause()
        {
            base.OnPause();

            Activity.RequestedOrientation = OrientationManager.DEFAULT_ORIENTATION;
        }

        class ViewCycleAdapter : MvxAdapter
        {
            public ViewCycleAdapter
            (Context context, IMvxAndroidBindingContext bindingContext)
                : base(context, bindingContext)
            {
            }

            protected override View GetBindableView(View convertView, object dataContext, int templateId)
            {
                int itemTemplateId = templateId;
                itemTemplateId = Resource.Layout.item_view_cycle;
                View view = base.GetBindableView(convertView, dataContext, itemTemplateId);

                return view;
            }

            public override int GetItemViewType(int position)
            {
                return 0;
            }

            public override int ViewTypeCount
            {
                get
                {
                    return 1;
                }
            }
        }
    }
}
