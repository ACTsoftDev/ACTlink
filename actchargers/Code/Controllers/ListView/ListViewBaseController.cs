using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace actchargers
{
    public abstract class ListViewBaseController : BaseController
    {
        public event EventHandler OnListChanged;
        public event EventHandler OnValuesChanged;
        public event EventHandler OnClosed;
        public event EventHandler<ListSelectorParameter> OnNavigatingToListSelector;
        public event EventHandler OnDisconnectingDevice;

        internal ListViewBaseSubController subController;

        internal readonly bool isBattView;

        public ObservableCollection<ListViewItem> ItemSource
        {
            get
            {
                return subController.ItemSource;
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
                editingMode = value;

                FireOnValuesChanged();
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
                isRestoreEnable = value;

                FireOnValuesChanged();
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
                showEdit = value;

                FireOnValuesChanged();
            }
        }

        bool isBusy;
        public bool IsBusy
        {
            get
            {
                return isBusy;
            }
            set
            {
                isBusy = value;

                FireOnValuesChanged();
            }
        }

        protected ListViewBaseController(bool isBattView)
        {
            this.isBattView = isBattView;
        }

        public async Task Start()
        {
            InitEvents();

            await subController.Start();
        }

        internal virtual void InitEvents()
        {
            subController.OnListChanged += SubController_OnListChanged;
            subController.OnValuesChanged += SubController_OnValuesChanged;
            subController.OnClosed += SubController_OnClosed;
            subController.OnNavigatingToListSelector += SubController_OnNavigatingToListSelector;
            subController.OnDisconnectingDevice += SubController_OnDisconnectingDevice;
        }

        void SubController_OnListChanged(object sender, EventArgs e)
        {
            FireOnListChanged();
        }

        void SubController_OnValuesChanged(object sender, EventArgs e)
        {
            EditingMode = subController.EditingMode;
            IsRestoreEnable = subController.IsRestoreEnable;
            ShowEdit = subController.ShowEdit;
            IsBusy = subController.IsBusy;

            FireOnValuesChanged();
        }

        void SubController_OnClosed(object sender, EventArgs e)
        {
            FireOnClosed();
        }

        void SubController_OnNavigatingToListSelector(object sender, ListSelectorParameter e)
        {
            FireOnNavigatingToListSelector(e);
        }

        void SubController_OnDisconnectingDevice(object sender, EventArgs e)
        {
            FireOnDisconnectingDevice();
        }

        public async Task GoToEditMode()
        {
            await subController.ChangeEditMode(true);
        }

        public async Task OnBack()
        {
            await subController.OnBack();
        }

        public async Task SaveValues()
        {
            await subController.SaveValues();
        }

        public void Restore()
        {
            subController.LoadDefaults();
        }

        void FireOnListChanged()
        {
            OnListChanged?.Invoke(this, EventArgs.Empty);
        }

        void FireOnValuesChanged()
        {
            OnValuesChanged?.Invoke(this, EventArgs.Empty);
        }

        internal void FireOnClosed()
        {
            OnClosed?.Invoke(this, EventArgs.Empty);
        }

        void FireOnNavigatingToListSelector(ListSelectorParameter e)
        {
            OnNavigatingToListSelector?.Invoke(this, e);
        }

        void FireOnDisconnectingDevice()
        {
            OnDisconnectingDevice?.Invoke(this, EventArgs.Empty);
        }
    }
}
