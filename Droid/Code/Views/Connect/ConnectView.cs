using System.Collections.Generic;
using actchargers.ViewModels;
using Android.Content;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Support.V4.View;
using Android.Views;
using Android.Widget;
using MvvmCross.Binding.Droid.BindingContext;

namespace actchargers.Droid
{
    public class ConnectView : BaseFragment
    {
        readonly string[] _tabHeader =
        {
            AppResources.mobile_router_mode,
            AppResources.usb,
            AppResources.stationary_router
        };

        ConnectViewModel currentViewModel;

        public override View OnCreateView
        (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            View view = this.BindingInflate(Resource.Layout.ConnectLayout, null);

            currentViewModel = ViewModel as ConnectViewModel;

            Init(view);

            return view;
        }

        void Init(View view)
        {
            var fragments = new List<MvxViewPagerFragmentAdapter.FragmentInfo> {
                new MvxViewPagerFragmentAdapter.FragmentInfo {
                    FragmentType = typeof(FixedRouterView),
                    ViewModel = currentViewModel
                },
                new MvxViewPagerFragmentAdapter.FragmentInfo {
                    FragmentType = typeof(USBView),
                    ViewModel = currentViewModel
                },
                new MvxViewPagerFragmentAdapter.FragmentInfo {
                    FragmentType = typeof(StationaryRouterView),
                    ViewModel = currentViewModel
                }
            };

            var _viewPager = view.FindViewById<ViewPager>(Resource.Id.viewPager);

            TabAdapter _adapter = new TabAdapter(view.Context, ChildFragmentManager, fragments);
            TabLayout _tabLayout = view.FindViewById<TabLayout>(Resource.Id.tabs);

            _viewPager.Adapter = _adapter;

            _tabLayout.SetupWithViewPager(_viewPager);

            for (int i = 0; i < _tabLayout.TabCount; i++)
            {
                TabLayout.Tab tab = _tabLayout.GetTabAt(i);

                tab.SetCustomView(_adapter.GetTabView(i, tab.IsSelected, _tabHeader[i]));
            }

            _tabLayout.TabSelected += (sender, e) =>
            {
                switch (e.Tab.Position)
                {
                    case 0:
                        ControlObject.connectMethods.Connectiontype =
                                         ConnectionTypesEnum.ROUTER;
                        currentViewModel.TabChanged(UiConnectionType.ROUTER);
                        break;

                    case 1:
                        ControlObject.connectMethods.Connectiontype =
                                         ConnectionTypesEnum.USB;
                        currentViewModel.TabChanged(UiConnectionType.USB);
                        break;

                    case 2:
                        ControlObject.connectMethods.Connectiontype =
                                         ConnectionTypesEnum.ROUTER;
                        currentViewModel.TabChanged(UiConnectionType.STATIONARY);
                        break;
                }
            };
        }

        class TabAdapter : MvxViewPagerFragmentAdapter
        {
            readonly Context _context;

            public TabAdapter
            (Context context, FragmentManager fragmentManager, IEnumerable<FragmentInfo> fragments)
                : base(context, fragmentManager, fragments)
            {
                _context = context;
            }

            public View GetTabView(int position, bool isSelected, string title)
            {
                LinearLayout layout = (LinearLayout)LayoutInflater.From(_context).Inflate(Resource.Layout.CustomTabLayout, null);
                TextView tv = (TextView)layout.FindViewById(Resource.Id.tabTitle);
                tv.Text = title;

                return tv;
            }
        }
    }
}
