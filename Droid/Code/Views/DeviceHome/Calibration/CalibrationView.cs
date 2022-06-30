using System;
using System.Collections.Generic;
using Android.Content;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Views;
using Android.Widget;
using MvvmCross.Binding.Droid.BindingContext;

namespace actchargers.Droid
{
    public class CalibrationView : BaseFragmentWithOutMenu
    {
        private BaseActivity activity;
        private CalibrationViewModel _mCalibrationViewModel;
        private ViewPager _viewPager;
        private CalibrationFragmentAdapter _adapter;
        private TabLayout _tabLayout;
        /// <summary>
        ///  tab titles.
        /// </summary>
        string[] _tabHeader = { AppResources.current, AppResources.voltage, AppResources.soc };
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            HasOptionsMenu = true;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            View view = this.BindingInflate(Resource.Layout.CalibrationLayout, null);
            activity = (BaseActivity)Activity;
            activity.SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            activity.SupportActionBar.SetDisplayShowHomeEnabled(true);
            _mCalibrationViewModel = ViewModel as CalibrationViewModel;
            if (BaseViewModel.IsBattView)
            {
                _tabHeader = new string[_mCalibrationViewModel.listOfSegments.Count];
                for (int i = 0; i < _mCalibrationViewModel.listOfSegments.Count; i++)
                {
                    _tabHeader[i] = _mCalibrationViewModel.listOfSegments[i];
                }
            }

            if (!BaseViewModel.IsBattView)
            {
                _tabHeader[0] = AppResources.voltage;
                _tabHeader[1] = null;
                _tabHeader[2] = null;

            }
            //set actionbar title
            activity.UpdateTitle(_mCalibrationViewModel.ViewTitle);
            InitTabs(view);

            return view;
        }

        /// <summary>
        /// Inits the tabs.
        /// </summary>
        /// <param name="view">View.</param>
        private void InitTabs(View view)
        {
            var fragments = new List<MvxViewPagerFragmentAdapter.FragmentInfo>();

            if (BaseViewModel.IsBattView)
            {
                for (int i = 0; i < _mCalibrationViewModel.listOfSegments.Count; i++)
                {
                    if (_mCalibrationViewModel.listOfSegments[i] == AppResources.current)
                    {
                        fragments.Add(new MvxViewPagerFragmentAdapter.FragmentInfo
                        {
                            FragmentType = typeof(CalibrationCurrentFragment),
                            ViewModel = _mCalibrationViewModel
                        });
                    }
                    if (_mCalibrationViewModel.listOfSegments[i] == AppResources.voltage)
                    {
                        fragments.Add(new MvxViewPagerFragmentAdapter.FragmentInfo
                        {
                            FragmentType = typeof(VoltageFragment),
                            ViewModel = _mCalibrationViewModel
                        });

                    }

                    if (_mCalibrationViewModel.listOfSegments[i] == AppResources.soc)
                    {
                        fragments.Add(new MvxViewPagerFragmentAdapter.FragmentInfo
                        {
                            FragmentType = typeof(SOCFragment),
                            ViewModel = _mCalibrationViewModel
                        });
                    }
                }
            }
            else
            {
                fragments.Add(new MvxViewPagerFragmentAdapter.FragmentInfo
                {
                    FragmentType = typeof(VoltageFragment),
                    ViewModel = _mCalibrationViewModel
                });
            }
            _viewPager = view.FindViewById<ViewPager>(Resource.Id.calibrationViewPager);
            _adapter = new CalibrationFragmentAdapter(view.Context, ChildFragmentManager, fragments);
            _tabLayout = view.FindViewById<TabLayout>(Resource.Id.calibrationTabsView);
            // Set adapter to view pager
            _viewPager.Adapter = _adapter;
            // Setup tablayout with view pager
            _tabLayout.SetupWithViewPager(_viewPager);

            // Iterate over all tabs and set the custom view
            for (int i = 0; i < _tabLayout.TabCount; i++)
            {

                TabLayout.Tab tab = _tabLayout.GetTabAt(i);
                tab.SetCustomView(_adapter.GetTabView(i, tab.IsSelected, _tabHeader[i]));
            }

            //Tab selected
            _tabLayout.TabSelected += (sender, e) =>
         {
             switch (e.Tab.Position)
             {
                 case 0:
                     _mCalibrationViewModel.SelectedIndex = 0;
                     activity.HideKeybord();
                     break;
                 case 1:
                     _mCalibrationViewModel.SelectedIndex = 1;
                     activity.HideKeybord();
                     break;
                 case 2:
                     _mCalibrationViewModel.SelectedIndex = 2;
                     activity.HideKeybord();
                     break;
                 default:
                     break;
             }
         };


        }


        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                //handling backpress of actionbar back here
                case Android.Resource.Id.Home:
                    // this takes the user 'back', as if they pressed the left-facing triangle icon on the main android toolbar.
                    activity.HideKeybord();
                    activity.OnBackPressed();
                    return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }

        }

        class CalibrationFragmentAdapter : MvxViewPagerFragmentAdapter
        {
            private Context context;

            public CalibrationFragmentAdapter(Context context, Android.Support.V4.App.FragmentManager childFragmentManager, List<MvxViewPagerFragmentAdapter.FragmentInfo> fragments) : base(context, childFragmentManager, fragments)
            {
                this.context = context;
            }
            /// <summary>
            /// Gets the tab view.
            /// </summary>
            /// <returns>The tab view.</returns>
            /// <param name="position">Tab Position.</param>
            /// <param name="isSelected">If set to <c>true</c> is selected.</param>
            /// <param name="title">Tab Title.</param>
            public View GetTabView(int position, bool isSelected, string title)
            {
                LinearLayout layout = (LinearLayout)LayoutInflater.From(context).Inflate(Resource.Layout.CustomTabLayout, null);
                TextView tv = (TextView)layout.FindViewById(Resource.Id.tabTitle);
                tv.Text = title;
                return tv;
            }
        }
    }
}
