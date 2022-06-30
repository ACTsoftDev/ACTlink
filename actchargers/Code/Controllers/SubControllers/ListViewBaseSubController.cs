using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using actchargers.Code.Utility;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;

namespace actchargers
{
    public abstract class ListViewBaseSubController
    {
        public event EventHandler OnListChanged;
        public event EventHandler OnValuesChanged;
        public event EventHandler OnClosed;
        public event EventHandler<ListSelectorParameter> OnNavigatingToListSelector;
        public event EventHandler OnDisconnectingDevice;

        MvxSubscriptionToken listSelector;

        internal readonly bool isBattView;

        internal UIAccessControlUtility accessControlUtility;

        public ObservableCollection<ListViewItem> ItemSource { get; set; }

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

        protected ListViewBaseSubController(bool isBattView)
        {
            this.isBattView = isBattView;

            ItemSource = new ObservableCollection<ListViewItem>();
        }

        public virtual Task Start()
        {
            try
            {
                TryStart();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }

            return Task.FromResult(default(object));
        }

        void TryStart()
        {
            SetProperties();

            InitItems();

            LoadAndAdd();
        }

        void SetProperties()
        {
            EditingMode = false;
            IsRestoreEnable = false;
            ShowEdit = true;
        }

        internal virtual void InitItems()
        {
            InitSharedItems();
        }

        void InitSharedItems()
        {
            if (isBattView)
                InitSharedBattViewItems();
            else
                InitSharedMcbItems();
        }

        internal abstract void InitSharedBattViewItems();

        internal abstract void InitSharedMcbItems();

        internal ListSelectorParameter CreateListSelectorParameter
        (ListViewItem item, int selectedItemIndex)
        {
            string itemSourceStr = JsonParser.SerializeObject(item.Items);

            ListSelectorParameter listSelectorParameter = new ListSelectorParameter()
            {
                Title = item.Title,
                ParentItemIndex = item.Index,
                SelectorType = item.ListSelectorType,
                SelectedItemIndex = selectedItemIndex,
                ItemSourceStr = itemSourceStr
            };

            return listSelectorParameter;
        }

        internal void LoadAndAdd()
        {
            LoadValues();
            AddItems();
        }

        internal void LoadValues()
        {
            if (isBattView)
                LoadBattViewValues();
            else
                LoadMcbValues();

            LoadExclusiveValues();
        }

        internal abstract void LoadExclusiveValues();

        internal abstract void LoadMcbValues();

        internal abstract void LoadBattViewValues();

        internal void AddItems()
        {
            if (isBattView)
                AddBattViewItems();
            else
                AddMcbItems();

            AddExclusiveItems();

            FireOnListChanged();
        }

        internal void AddBattViewItems()
        {
            if (BattViewAccessApply() == 0)
            {
                OnNoData();

                return;
            }

            if (ItemSource.Count > 0)
                InitAndOrderItemSource();
        }

        internal abstract int BattViewAccessApply();

        internal void AddMcbItems()
        {
            if (McbAccessApply() == 0)
            {
                OnNoData();

                return;
            }

            if (ItemSource.Count > 0)
                InitAndOrderItemSource();
        }

        internal abstract int McbAccessApply();

        void OnNoData()
        {
            ItemSource.Clear();
            ACUserDialogs.ShowAlert(AppResources.no_data_found);
        }

        void InitAndOrderItemSource()
        {
            ItemSource = new ObservableCollection<ListViewItem>(ItemSource.OrderBy(o => o.Index));
        }

        internal abstract void AddExclusiveItems();

        internal async Task SaveValues()
        {
            if (isBattView)
                await SaveBattViewValues();
            else
                await SaveMcbValues();
        }

        #region Save BattView

        async Task SaveBattViewValues()
        {
            var verifyControl = VerfiyBattViewSettings();

            if (verifyControl.HasErrors())
                ShowVerificationError(verifyControl);
            else
                await SaveBattViewSettings();
        }

        internal abstract VerifyControl VerfiyBattViewSettings();

        internal virtual async Task SaveBattViewSettings()
        {
            await SaveBattViewDeviceSettings();
        }

        internal async Task SaveBattViewDeviceSettings()
        {
            if (!BattViewQuantum.Instance.BattView_quickSerialNumberCheckStrict())
                return;

            try
            {
                await TrySaveBattViewDeviceSettings();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        async Task TrySaveBattViewDeviceSettings()
        {
            SaveBattViewToConfigObject(BattViewQuantum.Instance.GetBATTView());

            bool result = await SaveBattViewAndReturnResult(BattViewQuantum.Instance.GetBATTView());

            if (result)
                await OnAfterDeviceSaved();
            else
                ACUserDialogs.ShowAlertWithTitleAndOkButton(AppResources.save_failed);
        }

        internal virtual async Task<bool> SaveBattViewAndReturnResult(BattViewObject device)
        {
            return await device.SaveConfig();
        }

        internal abstract void SaveBattViewToConfigObject(BattViewObject device);

        #endregion

        #region Save MCB

        async Task SaveMcbValues()
        {
            var verifyControl = VerfiyMcbSettings();

            if (verifyControl.HasErrors())
                ShowVerificationError(verifyControl);
            else
                await SaveMcbSettings();
        }

        internal abstract VerifyControl VerfiyMcbSettings();

        internal virtual async Task SaveMcbSettings()
        {
            await SaveMcbDeviceSettings();
        }

        internal async Task SaveMcbDeviceSettings()
        {
            if (!MCBQuantum.Instance.MCB_quickSerialNumberCheckStrict())
                return;

            try
            {
                await TrySaveMcbDeviceSettings();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        async Task TrySaveMcbDeviceSettings()
        {
            SaveMcbToConfigObject(MCBQuantum.Instance.GetMCB());

            bool result = await SaveMcbAndReturnResult(MCBQuantum.Instance.GetMCB());

            if (result)
                await OnAfterDeviceSaved();
            else
                ACUserDialogs.ShowAlertWithTitleAndOkButton(AppResources.save_failed);
        }

        internal virtual async Task<bool> SaveMcbAndReturnResult(MCBobject device)
        {
            return await device.SaveConfig();
        }

        internal abstract void SaveMcbToConfigObject(MCBobject device);

        #endregion

        async Task OnAfterDeviceSaved()
        {
            await OnSaved(MCBQuantum.Instance.GetMCB());

            await ChangeEditMode(false);

            ResetData();

            LoadValues();

            FireOnListChanged();
        }

        internal virtual async Task OnSaved(MCBobject device)
        {
            if (device == null)
                await Task.Delay(10);
        }

        internal IMvxCommand SwitchValueChanged
        {
            get
            {
                return new MvxCommand<ListViewItem>(ExecuteSwitchValueChanged);
            }
        }

        internal virtual void ExecuteSwitchValueChanged(ListViewItem item)
        {
            ChangeAbilityToChildren(item);
        }

        void ChangeAbilityToChildren(ListViewItem item)
        {
            if (!ItemSource.Contains(item))
                return;

            List<int> childrenItems = item.EnableItemsList;

            if (childrenItems == null)
                return;

            foreach (int child in childrenItems)
                ChangeAbilityToChild(child, item.IsSwitchEnabled);

            FireOnListChanged();
        }

        void ChangeAbilityToChild(int enableItemIndex, bool isChecked)
        {
            var childItem = ItemSource.FirstOrDefault((arg) => arg.Index == enableItemIndex);

            if (childItem == null)
                return;

            ChangeChildEditMode(childItem, isChecked);
        }

        void ChangeChildEditMode(ListViewItem childItem, bool isChecked)
        {
            childItem.Enable = isChecked;

            bool childEditMode = isChecked && EditingMode;
            childItem.IsEditEnabled = childEditMode;
            childItem.ChangeEditMode(childEditMode);

            FireOnListChanged();
        }

        internal IMvxCommand ListSelectorCommand
        {
            get { return new MvxCommand<ListViewItem>(ExecuteListSelectorCommand); }
        }

        void ExecuteListSelectorCommand(ListViewItem item)
        {
            if (item.CellType == ACUtility.CellTypes.ListSelector)
                ExecuteListSelector(item);
        }

        void ExecuteListSelector(ListViewItem item)
        {
            listSelector = Mvx.Resolve<IMvxMessenger>().Subscribe<ListSelectorMessage>(OnListSelectorMessage);

            int selectedItemIndex = ItemSource.IndexOf(item);

            ListSelectorParameter listSelectorParameter = CreateListSelectorParameter(item, selectedItemIndex);

            FireOnNavigatingToListSelector(listSelectorParameter);
        }

        void OnListSelectorMessage(ListSelectorMessage obj)
        {
            if (listSelector == null)
                return;

            Mvx.Resolve<IMvxMessenger>().Unsubscribe<ListSelectorMessage>(listSelector);

            listSelector = null;

            ItemSource[obj.SelectedItemindex].Text = obj.SelectedItem;
            ItemSource[obj.SelectedItemindex].SelectedItem = obj.SelectedItem;
            ItemSource[obj.SelectedItemindex].SelectedIndex = obj.SelectedIndex;

            OnListItemSelected(obj);

            FireOnListChanged();
        }

        internal virtual void OnListItemSelected(ListSelectorMessage selectedListItem)
        {
        }

        internal void ShowVerificationError(VerifyControl verifyControl)
        {
            ACUserDialogs
                    .ShowAlert
                    (AppResources.alert_enter_valid + " " + verifyControl.GetErrorString());
        }

        internal void ResetData()
        {
            foreach (var item in ItemSource)
                item.Reset();
        }

        async Task OnAfterSiteViewSaved()
        {
            await ChangeEditMode(false);
        }

        public async Task OnBack()
        {
            if (CheckForEditedChanges())
            {
                ACUserDialogs
                .ShowAlertWithTwoButtons
                    (AppResources.cancel_confirmation, "", AppResources.yes,
                     AppResources.no, OnYesAction, null);
            }
            else
            {
                await ChangeEditMode(false);
            }
        }

        bool CheckForEditedChanges()
        {
            bool textChanged = false;
            foreach (var item in ItemSource)
            {
                textChanged |= item.Text != item.SubTitle;
            }

            return textChanged;
        }

        void OnYesAction()
        {
            Task.Run(() => ChangeEditMode(false));
        }

        public abstract void LoadDefaults();

        public async Task ChangeEditMode(bool editingMode)
        {
            EditingMode = editingMode;

            await RefreshList();

            FireOnListChanged();
        }

        async Task RefreshList()
        {
            await Task.Run((Action)RefreshListTask);
        }

        void RefreshListTask()
        {
            foreach (var item in ItemSource)
                RefreshOneItem(item);
        }

        void RefreshOneItem(ListViewItem item)
        {
            RefreshOneItemBasedOnType(item);

            if (!EditingMode)
                item.Apply();

            item.ChangeEditMode(EditingMode);
        }

        void RefreshOneItemBasedOnType(ListViewItem item)
        {
            switch (item.EditableCellType)
            {
                case ACUtility.CellTypes.LabelSwitch:
                    ExecuteSwitchValueChanged(item);

                    break;
            }
        }

        internal void FireOnListChanged()
        {
            OnListChanged?.Invoke(this, EventArgs.Empty);
        }

        internal void FireOnValuesChanged()
        {
            OnValuesChanged?.Invoke(this, EventArgs.Empty);
        }

        internal void FireOnClosed()
        {
            OnClosed?.Invoke(this, EventArgs.Empty);
        }

        internal void FireOnNavigatingToListSelector(ListSelectorParameter e)
        {
            OnNavigatingToListSelector?.Invoke(this, e);
        }

        internal void FireOnDisconnectingDevice()
        {
            OnDisconnectingDevice?.Invoke(this, EventArgs.Empty);
        }
    }
}
