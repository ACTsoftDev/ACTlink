using Android.OS;
using Android.Views;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Core.ViewModels;

namespace actchargers.Droid
{
    public class EditEventControlView : ListFragment
    {
        private View view;
        private EditEventControlViewModel _mEditEventControlViewModel;
        private BaseActivity activity;

        public override void OnCreate(Bundle savedInstanceState)
        {
           base.OnCreate(savedInstanceState);
            HasOptionsMenu = true;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            view = this.BindingInflate(Resource.Layout.BaseListLayout, null);
            activity = (BaseActivity)Activity;
            activity.SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            activity.SupportActionBar.SetDisplayShowHomeEnabled(true);
            _mEditEventControlViewModel = ViewModel as EditEventControlViewModel;
            //set actionbar title
            activity.UpdateTitle(_mEditEventControlViewModel.ViewTitle);
            InitExpandableListViewUI(view, _mEditEventControlViewModel.EventControlItemSource, _mEditEventControlViewModel);
            ((MvxNotifyPropertyChanged)this.ViewModel).PropertyChanged += OnPropertyChanged;
            return view;
        }
        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            //clear activity menu items to add  specific fragment menu items
            menu.Clear();
            inflater.Inflate(Resource.Menu.edit_menu, menu);
            IMenuItem editItem = menu.FindItem(Resource.Id.edit);
            IMenuItem saveItem = menu.FindItem(Resource.Id.save);
            //visible edit option to based on mode 
            editItem.SetVisible(!_mEditEventControlViewModel.EditingMode);
            saveItem.SetVisible(_mEditEventControlViewModel.EditingMode);
            base.OnCreateOptionsMenu(menu, inflater);

        }
        void OnPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("EventControlItemSource"))
            {
                InitExpandableListViewUI(view, _mEditEventControlViewModel.EventControlItemSource, _mEditEventControlViewModel);
            }
            else if (e.PropertyName.Equals("EditingMode"))
            {
                activity.SupportInvalidateOptionsMenu();
            }

        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                //handling backpress of actionbar back here
                case Android.Resource.Id.Home:
                    activity.HideKeybord();
                    // this takes the user 'back', as if they pressed the left-facing triangle icon on the main android toolbar.
                    if (_mEditEventControlViewModel.EditingMode)
                    {
                        _mEditEventControlViewModel.BackBtnClickCommand.Execute();
                        activity.SupportInvalidateOptionsMenu();
                    }
                    else
                    {
                        activity.OnBackPressed();
                    }
                    return true;
                case Resource.Id.edit:
                    //edit mode action here
                    _mEditEventControlViewModel.EditBtnClickCommand.Execute();
                    activity.SupportInvalidateOptionsMenu();
                    return true;
                case Resource.Id.save:
                    activity.HideKeybord();
                    //edit mode action here
                    _mEditEventControlViewModel.SaveBtnClickCommand.Execute();
                    activity.SupportInvalidateOptionsMenu();
                    return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }

        }
    }
}
