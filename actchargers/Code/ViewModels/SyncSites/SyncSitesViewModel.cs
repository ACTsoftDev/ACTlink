using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;

namespace actchargers
{
    public class SyncSitesViewModel : BaseViewModel
    {
        readonly bool requestCancelSynch;

        ObservableCollection<SyncSitesHeaderItem> _ListItemSource;
        public ObservableCollection<SyncSitesHeaderItem> ListItemSource
        {
            get { return _ListItemSource; }
            set
            {
                _ListItemSource = value;
                RaisePropertyChanged(() => ListItemSource);
            }
        }

        public SyncSitesViewModel()
        {
            ViewTitle = AppResources.sync_sites;
            ListItemSource = new ObservableCollection<SyncSitesHeaderItem>();

            Task.Run(async () =>
            {
                ACUserDialogs.ShowProgress();

                List<object> arguments = new List<object>
                {
                    false,
                    ActviewCommGeneric.loadUserCustomersAndSites
                };

                try
                {
                    var results = await ACTVIEWQuantum.Instance.CommunicateACTView(arguments);

                    if (results != null && results.Count > 0)
                    {
                        bool internalFailure = (bool)results[0];
                        string internalFailureString = (string)results[1];
                        ACTViewResponse status = (ACTViewResponse)results[2];
                        ActviewCommGeneric caller = (ActviewCommGeneric)results[3];
                        bool removeSetBusy = (bool)results[4];
                        List<object> ProcArgumentList = (List<object>)results[5];

                        switch (status.responseType)
                        {
                            case ActviewResponseType.validResponse:
                                var SyncSites = ((Dictionary<UInt32, ACTViewCustomer>)ProcArgumentList[0]).Values.ToList();

                                foreach (var item in SyncSites)
                                {
                                    SyncSitesHeaderItem syncSitesHeaderItem = new SyncSitesHeaderItem
                                    {
                                        Name = item.name,
                                        Id = item.id,
                                        Sites = item.getSites(),
                                        ExpandCommand = ExpandCommand,
                                        CheckCommand = CheckCommand
                                    };

                                    ListItemSource.Add(syncSitesHeaderItem);
                                }

                                RaisePropertyChanged(() => ListItemSource);

                                break;

                            case ActviewResponseType.notInternet:
                                InvokeOnMainThread(() =>
                                {
                                    ACUserDialogs.ShowAlert(AppResources.no_internet_connection);
                                });

                                break;

                            case ActviewResponseType.expiredAPI:
                                InvokeOnMainThread(SoftwareUpdateHelper.ShowUpdateWarning);

                                break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
                ACUserDialogs.HideProgress();
            });
        }

        public IMvxCommand CheckCommand
        {
            get { return new MvxCommand<SyncSitesHeaderItem>(CheckClick); }
        }

        void CheckClick(SyncSitesHeaderItem item)
        {
            item.IsChecked = !item.IsChecked;

            if (item.Sites != null && item.Sites.Count > 0)
            {
                if (item.IsChecked)
                {
                    foreach (var childitem in item.Sites)
                    {
                        childitem.IsChecked = true;
                    }
                }
                else
                {
                    foreach (var childitem in item.Sites)
                    {
                        childitem.IsChecked = false;
                    }
                }
            }
        }

        public IMvxCommand ItemCheckCommand
        {
            get { return new MvxCommand<ACTViewSite>(ItemCheckClick); }
        }

        void ItemCheckClick(ACTViewSite item)
        {
            item.IsChecked = !item.IsChecked;

            var parentItem = ListItemSource.FirstOrDefault(o => o.Id == item.customerID);

            if (parentItem != null)
            {
                //If all the child items are clicked, then make the parent fully checked
                if (parentItem.ChildItems.All(o => o.IsChecked))
                {
                    parentItem.IsChecked = true;
                }

                //If any the child item is clicked, then make the parent partially checked
                else if (parentItem.ChildItems.Any(o => o.IsChecked))
                {
                    parentItem.IsPartiallyChecked = true;
                }

                //If no child item is clicked, then make the parent unchecked
                else if (parentItem.ChildItems.All(o => !o.IsChecked))
                {
                    parentItem.IsChecked = false;
                }
            }

        }

        public IMvxCommand ExpandCommand
        {
            get { return new MvxCommand<SyncSitesHeaderItem>(ExpandClick); }
        }

        void ExpandClick(SyncSitesHeaderItem item)
        {
            if (item.Sites != null && item.Sites.Count > 0)
            {
                if (item.IsExpanded)
                {
                    item.ChildItems.Clear();
                    item.IsExpanded = false;
                }
                else
                {
                    item.ChildItems.AddRange(item.Sites);
                    foreach (var childitem in item.ChildItems)
                    {
                        if (string.IsNullOrEmpty(childitem.CheckedImageString))
                        {
                            childitem.CheckedImageString = "circle";
                        }
                        if (childitem.ItemCheckCommand == null)
                        {
                            childitem.ItemCheckCommand = ItemCheckCommand;
                        }

                        childitem.IsChecked = item.IsChecked;

                    }
                    item.IsExpanded = true;
                }
                RaisePropertyChanged(() => ListItemSource);
            }
        }

        public override void OnBackButtonClick()
        {
            base.OnBackButtonClick();

            Mvx.Resolve<IMvxMessenger>().Publish(new BackScanMessage(this));
            ShowViewModel<SyncSitesViewModel>(new { pop = "pop" });
        }

        public IMvxCommand SaveBtnClickCommand
        {
            get
            {
                return new MvxCommand(async () =>
                {
                    await OnSaveClick();
                });
            }
        }

        async Task OnSaveClick()
        {
            List<ACTViewSite> selctedSites = new List<ACTViewSite>();
            int customersCount = 0;

            var selectedCustomers = ListItemSource.Where(o => o.IsChecked || o.IsPartiallyChecked).ToList();

            foreach (var item in selectedCustomers)
            {
                selctedSites.AddRange(item.Sites.Where(o => o.IsChecked));
            }

            customersCount = selectedCustomers.Count();

            string errorMsg = "";

            if (selctedSites.Count == 0)
            {
                errorMsg = "Choose Sites to sync";
            }

            if (selctedSites.Count > 15 && !ControlObject.isDebugMaster)
            {
                errorMsg = "Maximum of 15 sites can be Synchronized";
            }

            else if (customersCount > 4 && !ControlObject.isDebugMaster)
            {
                errorMsg = "Maximum of 5 customers can be Synchronized";
            }

            if (errorMsg != "")
            {
                ACUserDialogs.ShowAlert(errorMsg);

                return;
            }

            List<object> arguments = new List<object>
            {
                false,
                ActviewCommGeneric.downloadDevicesFromSite,
                selctedSites
            };

            ACUserDialogs.ShowProgress();

            try
            {
                var results = await ACTVIEWQuantum.Instance.CommunicateACTView(arguments);
                if (results != null && results.Count > 0)
                {
                    bool internalFailure = (bool)results[0];
                    string internalFailureString = (string)results[1];
                    ACTViewResponse status = (ACTViewResponse)results[2];
                    ActviewCommGeneric caller = (ActviewCommGeneric)results[3];
                    bool removeSetBusy = (bool)results[4];
                    List<object> ProcArgumentList = (List<object>)results[5];

                    if (status.responseType == ActviewResponseType.validResponse)
                    {
                        ACUserDialogs.HideProgress();

                        SaveSynchedSitesDoneWork
                        (status.responseType == ActviewResponseType.validResponse,
                         (int)ProcArgumentList[2], (int)ProcArgumentList[3]);
                    }
                }
            }
            catch (Exception ex)
            {
                ACUserDialogs.HideProgress();

                ACUserDialogs.ShowAlert("Error Downloading the SncSites");

                Debug.WriteLine("Error Downloading the SncSites");
                Debug.WriteLine(ex.ToString());
            }
        }

        void SaveSynchedSitesDoneWork(bool OK, int battC, int mcbC)
        {
            try
            {
                string Message = string.Empty;
                if (!OK)
                {
                    if (!requestCancelSynch)
                    {
                        Message = "Failed to download and save devices localy";
                    }
                    return;
                }
                Message = battC.ToString() + " BATTViews & " + mcbC.ToString() + " Chargers downloaded";

                SetLastSynchSiteDate();

                ACUserDialogs.HideProgress();
                Mvx.Resolve<IMvxMessenger>().Publish(new BackScanMessage(this));
                ShowViewModel<SyncSitesViewModel>(new { pop = "pop" });
                ACUserDialogs.ShowAlert(Message);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        void SetLastSynchSiteDate()
        {
            DbSingleton
                .DBManagerServiceInstance
                .GetGenericObjectsLoader()
                .SetLastSynchSiteDate();
        }
    }
}
