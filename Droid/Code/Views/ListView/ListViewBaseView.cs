using Android.OS;
using Android.Views;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Binding.Droid.Views;
using MvvmCross.Core.ViewModels;

namespace actchargers.Droid
{
    public abstract class ListViewBaseView : BasicListFragment
    {
        View view;
        MvxListView listView;

        ListViewBaseViewModel currentViewModel;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            HasOptionsMenu = true;

            ((MvxNotifyPropertyChanged)ViewModel).PropertyChanged += OnPropertyChanged;
        }

        public override View OnCreateView
        (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            view = this.BindingInflate(GetLayoutResource(), null);

            currentViewModel = ViewModel as ListViewBaseViewModel;

            listView = view.FindViewById<MvxListView>(Resource.Id.listView);

            InitList(listView);

            return view;
        }

        internal abstract int GetLayoutResource();

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            menu.Clear();
            inflater.Inflate(Resource.Menu.edit_menu, menu);
            IMenuItem editItem = menu.FindItem(Resource.Id.edit);
            IMenuItem saveItem = menu.FindItem(Resource.Id.save);

            editItem.SetVisible(currentViewModel.ShowEdit && !currentViewModel.EditingMode);
            saveItem.SetVisible(currentViewModel.ShowEdit && currentViewModel.EditingMode);

            base.OnCreateOptionsMenu(menu, inflater);
        }

        void OnPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (Context != null)
                Activity.RunOnUiThread(() => OnPropertyChangedAction(e.PropertyName));
        }

        void OnPropertyChangedAction(string propertyName)
        {
            if (propertyName.Equals("ItemSource"))
                RefreshList();

            else if (propertyName.Equals("EditingMode"))
                activity.SupportInvalidateOptionsMenu();
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    activity.HideKeybord();

                    if (currentViewModel.EditingMode)
                    {
                        currentViewModel.BackCommand.Execute();
                        activity.SupportInvalidateOptionsMenu();
                    }
                    else
                    {
                        activity.OnBackPressed();
                    }

                    return true;

                case Resource.Id.edit:
                    currentViewModel.EditCommand.Execute();
                    activity.SupportInvalidateOptionsMenu();

                    return true;

                case Resource.Id.save:
                    activity.HideKeybord();

                    currentViewModel.SaveCommand.Execute();
                    activity.SupportInvalidateOptionsMenu();

                    return true;

                default:
                    return base.OnOptionsItemSelected(item);
            }
        }
    }
}
