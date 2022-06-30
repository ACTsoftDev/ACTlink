using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using MvvmCross.Core.ViewModels;

namespace actchargers
{
    public abstract class ListViewBaseViewModel : BaseViewModel
    {
        internal ListViewBaseController controller;

        public ObservableCollection<ListViewItem> ItemSource
        {
            get
            {
                return controller.ItemSource;
            }
        }

        bool editingMode;
        public bool EditingMode
        {
            get
            {
                return editingMode;
            }
            set
            {
                SetProperty(ref editingMode, value);
            }
        }

        bool isRestoreEnable;
        public bool IsRestoreEnable
        {
            get
            {
                return isRestoreEnable;
            }
            set
            {
                SetProperty(ref isRestoreEnable, value);

                RaisePropertyChanged(() => IsRestoreDisable);
            }
        }

        public bool IsRestoreDisable
        {
            get
            {
                return !IsRestoreEnable;
            }
        }

        bool showEdit;
        public bool ShowEdit
        {
            get
            {
                return showEdit;
            }
            set
            {
                SetProperty(ref showEdit, value);
            }
        }

        public string RestoreToDefaultTitle
        {
            get
            {
                return AppResources.restore_to_defaults;
            }
        }

        public void Init()
        {
            AfterInit();
        }

        internal void AfterInit()
        {
            InitController();

            InitEvents();

            if (controller != null)
                StartController();
        }

        void StartController()
        {
            Task.Run(controller.Start);
        }

        internal abstract void InitController();

        internal virtual void InitEvents()
        {
            controller.OnListChanged += Controller_OnListChanged;
            controller.OnValuesChanged += Controller_OnValuesChanged;
            controller.OnClosed += Controller_OnClosed;
            controller.OnNavigatingToListSelector += Controller_OnNavigatingToListSelector;
            controller.OnDisconnectingDevice += Controller_OnDisconnectingDevice;
        }

        void Controller_OnListChanged(object sender, EventArgs e)
        {
            RaisePropertyChanged(() => ItemSource);
        }

        void Controller_OnValuesChanged(object sender, EventArgs e)
        {
            EditingMode = controller.EditingMode;
            IsRestoreEnable = controller.IsRestoreEnable;
            ShowEdit = controller.ShowEdit;
            IsBusy = controller.IsBusy;
        }

        void Controller_OnClosed(object sender, EventArgs e)
        {
            OnBackButtonClick();
        }

        void Controller_OnNavigatingToListSelector(object sender, ListSelectorParameter e)
        {
            ShowViewModel<ListSelectorViewModel>
            (
                new
                {
                    type = e.SelectorType,
                    parentItemIndex = e.ParentItemIndex,
                    selectedItemIndex = e.SelectedItemIndex,
                    ItemSourceStr = e.ItemSourceStr,
                    title = e.Title
                }
            );
        }

        void Controller_OnDisconnectingDevice(object sender, EventArgs e)
        {
            DisconnectDevice();
        }

        MvxCommand<ListViewItem> itemClickCommand;
        public IMvxCommand ItemClickCommand
        {
            get
            {
                return itemClickCommand ?? (itemClickCommand = new MvxCommand<ListViewItem>(ExecuteItemClickCommnad));
            }
        }

        internal virtual void ExecuteItemClickCommnad(ListViewItem item)
        {
        }

        public IMvxCommand EditCommand
        {
            get { return new MvxAsyncCommand(OnEditCommandClickTask); }
        }

        async Task OnEditCommandClickTask()
        {
            IsLoading = true;

            await DelayToLoad();

            await controller.GoToEditMode();

            IsLoading = false;
        }

        public IMvxCommand BackCommand
        {
            get { return new MvxAsyncCommand(OnBackCommandClick); }
        }

        async Task OnBackCommandClick()
        {
            await controller.OnBack();
        }

        public IMvxCommand SaveCommand
        {
            get { return new MvxAsyncCommand(OnSaveCommandClick); }
        }

        async Task OnSaveCommandClick()
        {
            IsLoading = true;

            await DelayToLoad();

            await controller.SaveValues();

            IsLoading = false;
        }

        public IMvxCommand RestoreCommand
        {
            get { return new MvxCommand(OnRestoreCommandClick); }
        }

        void OnRestoreCommandClick()
        {
            controller.Restore();
        }
    }
}
