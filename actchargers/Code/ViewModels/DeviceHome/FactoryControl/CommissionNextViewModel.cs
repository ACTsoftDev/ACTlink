using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;
using Newtonsoft.Json;
using static actchargers.ACUtility;

namespace actchargers
{
    public class CommissionNextViewModel : BaseViewModel
    {
        private ObservableCollection<ListViewItem> _commissionNextItemSource;
        public ObservableCollection<ListViewItem> CommissionNextItemSource
        {
            get { return _commissionNextItemSource; }
            set
            {
                _commissionNextItemSource = value;
                RaisePropertyChanged(() => CommissionNextItemSource);
            }
        }
        MvxSubscriptionToken _mListSelector;
        #region BattView list items
        ListViewItem Batt_Commission_DeviceOEM_ComboBox;//Batt_Commission_DeviceOEM_Label;
        ListViewItem Batt_Commission_DeviceDealer_ComboBox;//Batt_Commission_DeviceDealer_Label;
        ListViewItem Batt_Commission_DeviceServiceDealer_ComboBox;//Batt_Commission_DeviceServiceDealer_Label;
        ListViewItem Batt_Commission_DeviceCustomers_ComboBox;//Batt_Commission_DeviceCustomers_Label;
        ListViewItem Batt_Commission_DeviceSites_ComboBox;//Batt_Commission_DeviceSites_Label;

        ListViewItem Batt_Commission_DeviceSynchSitesButton;
        ListViewItem Batt_Commission_DeviceCommissionButton;
        //ListViewItem Batt_Commission_DeviceBackButton;
        ListViewItem Batt_Commission_DeviceSynchListsButton;

        #endregion

        #region Charger list items
        ListViewItem MCB_Commission_DeviceOEM_ComboBox;
        ListViewItem MCB_Commission_DeviceDealer_ComboBox;
        ListViewItem MCB_Commission_DeviceServiceDealer_ComboBox;
        ListViewItem MCB_Commission_DeviceCustomers_ComboBox;
        ListViewItem MCB_Commission_DeviceSites_ComboBox;

        ListViewItem MCB_Commission_DeviceSynchSitesButton;
        ListViewItem MCB_Commission_DeviceCommissionButton;
        ListViewItem MCB_Commission_DeviceSynchListsButton;
        #endregion

        public CommissionNextViewModel()
        {
            CommissionNextItemSource = new ObservableCollection<ListViewItem>();
            CreateList();
        }

        void CreateList()
        {
            if (IsBattView)
            {
                ViewTitle = AppResources.commission_a_battview;
                CreateListForBattView();
            }
            else
            {
                ViewTitle = AppResources.commission_a_charger;
                CreateListForChargers();
            }
        }


        #region BattView create list 
        void CreateListForBattView()
        {
            CommissionNextItemSource.Clear();
            Batt_Commission_DeviceOEM_ComboBox = new ListViewItem
            {
                Index = 0,
                Title = "Battery OEM (Optional)",
                DefaultCellType = CellTypes.ListSelector,
                EditableCellType = CellTypes.ListSelector,
                Items = new List<object>(),
                ListSelectionCommand = ListSelectorCommand,
                ListSelectorType = ACUtility.ListSelectorType.OEM
            };
            CommissionNextItemSource.Add(Batt_Commission_DeviceOEM_ComboBox);

            Batt_Commission_DeviceDealer_ComboBox = new ListViewItem
            {
                Index = 1,
                Title = "Dealer (Required)",
                DefaultCellType = CellTypes.ListSelector,
                EditableCellType = CellTypes.ListSelector,
                ListSelectionCommand = ListSelectorCommand,
                ListSelectorType = ACUtility.ListSelectorType.Dealer,
                Items = new List<object>()
            };
            CommissionNextItemSource.Add(Batt_Commission_DeviceDealer_ComboBox);

            Batt_Commission_DeviceServiceDealer_ComboBox = new ListViewItem
            {
                Index = 2,
                Title = "Service Dealer (Optional)",
                DefaultCellType = CellTypes.ListSelector,
                EditableCellType = CellTypes.ListSelector,
                ListSelectionCommand = ListSelectorCommand,
                ListSelectorType = ACUtility.ListSelectorType.ServiceDealer,
                Items = new List<object>()
            };
            CommissionNextItemSource.Add(Batt_Commission_DeviceServiceDealer_ComboBox);

            Batt_Commission_DeviceCustomers_ComboBox = new ListViewItem
            {
                Index = 3,
                Title = "Customer (Optional)",
                DefaultCellType = CellTypes.ListSelector,
                EditableCellType = CellTypes.ListSelector,
                ListSelectionCommand = ListSelectorCommand,
                ListSelectorType = ACUtility.ListSelectorType.Customers,
                Items = new List<object>()
            };
            CommissionNextItemSource.Add(Batt_Commission_DeviceCustomers_ComboBox);

            Batt_Commission_DeviceSites_ComboBox = new ListViewItem
            {
                Index = 4,
                Title = "Site (Optional)",
                DefaultCellType = CellTypes.ListSelector,
                EditableCellType = CellTypes.ListSelector,
                ListSelectionCommand = ListSelectorCommand,
                ListSelectorType = ACUtility.ListSelectorType.Sites,
                Items = new List<object>()
            };
            CommissionNextItemSource.Add(Batt_Commission_DeviceSites_ComboBox);

            Batt_Commission_DeviceSynchSitesButton = new ListViewItem
            {
                Index = 5,
                Title = "Synchronize Sites",
                DefaultCellType = CellTypes.Button,
                EditableCellType = CellTypes.Button,
                ListSelectionCommand = BattViewButtonSelectorCommand
            };
            CommissionNextItemSource.Add(Batt_Commission_DeviceSynchSitesButton);

            Batt_Commission_DeviceCommissionButton = new ListViewItem
            {
                Index = 6,
                Title = "Commission",
                DefaultCellType = CellTypes.Button,
                EditableCellType = CellTypes.Button,
                ListSelectionCommand = BattViewButtonSelectorCommand
            };
            CommissionNextItemSource.Add(Batt_Commission_DeviceCommissionButton);

            Batt_Commission_DeviceSynchListsButton = new ListViewItem
            {
                Index = 7,
                Title = "Synchronize Lists",
                DefaultCellType = CellTypes.Button,
                EditableCellType = CellTypes.Button,
                ListSelectionCommand = BattViewButtonSelectorCommand
            };
            CommissionNextItemSource.Add(Batt_Commission_DeviceSynchListsButton);
            Task.Run(async () =>
          {
           await Batt_CommissionActionInternal(null, null);
          });

        }
        #endregion

        #region BattView actions
        public IMvxCommand BattViewButtonSelectorCommand
        {
            get { return new MvxCommand<ListViewItem>(ExecuteBattViewButtonSelectorCommand); }
        }

        async void ExecuteBattViewButtonSelectorCommand(ListViewItem item)
        {
            await Batt_CommissionActionInternal(item, null);
        }

        private BattViewConfig Batt_Commission_config;
        private async Task  Batt_CommissionActionInternal(ListViewItem sender, EventArgs e)
        {

            if (!BattViewQuantum.Instance.BattView_quickSerialNumberCheckStrict())
            {
                return;
            }
            try
            {
                ACConstants.DEVICE_COMMISSION_ACTIONS_LIST caller = ACConstants.DEVICE_COMMISSION_ACTIONS_LIST.nothing;
                object arg1 = null;
                if (sender == null)
                {
                    if (!BattViewQuantum.Instance.BattView_quickSerialNumberCheckStrict())
                    {
                        ACUserDialogs.ShowAlert("Lost BATTView Connection");
                        return;
                    }

                    Batt_Commission_config = new BattViewConfig();
                    BattViewObject activeBattView = BattViewQuantum.Instance.GetBATTView();
                    Batt_Commission_config = StaticDataAndHelperFunctions.DeepClone<BattViewConfig>(activeBattView.Config);
                    Batt_Commission_config.zoneid = activeBattView.myZone;
                    Batt_Commission_config.commissionRequest = true;
                    Batt_Commission_config.firmwareversion = activeBattView.FirmwareRevision;
                    //battView_MenusShowHideInternal(Batt_AdminActionsCommissionStep1Button, null);
                    if (!usersList.AreOemsAndDealersLoaded() && !usersList.AreCustomersLoaded())
                    {
                        caller = ACConstants.DEVICE_COMMISSION_ACTIONS_LIST.all;
                    }
                    else if (!usersList.AreOemsAndDealersLoaded())
                    {
                        caller = ACConstants.DEVICE_COMMISSION_ACTIONS_LIST.onlyOEMsAndDealers;
                    }
                    else if (!usersList.AreCustomersLoaded())
                    {
                        caller = ACConstants.DEVICE_COMMISSION_ACTIONS_LIST.onlyCustomers;
                    }
                    else
                    {
                        IsBusy = true;
                        if (Batt_Commission_DeviceOEM_ComboBox.Items.Count == 0)
                        {
                            //Batt_Commission_DeviceOEM_ComboBox.DataSource = null;
                            Batt_Commission_DeviceOEM_ComboBox.Items.Clear();
                            List<OEM> oems = await usersList.getAllOEMs(false);
                            if (oems.Find(x => x.id == 0) == null)
                            {
                                OEM selectOEM = new OEM(0, "Select OEM");
                                oems.Insert(0, selectOEM);
                            }
                            Batt_Commission_DeviceOEM_ComboBox.Items = new List<object>(oems);
                        }
                        Batt_Commission_DeviceOEM_ComboBox.SelectedIndex = 0;
                        Batt_Commission_DeviceOEM_ComboBox.SelectedItem = Batt_Commission_DeviceOEM_ComboBox.Items[Batt_Commission_DeviceOEM_ComboBox.SelectedIndex].ToString();

                        if (Batt_Commission_DeviceDealer_ComboBox.Items.Count == 0)
                        {
                            List<Dealer> dealers = await usersList.getAllDealers(false);
                            //Batt_Commission_DeviceDealer_ComboBox.DataSource = null;
                            Batt_Commission_DeviceDealer_ComboBox.Items.Clear();
                            if (dealers.Find(x => x.id == 0) == null)
                            {
                                Dealer selecyDealer = new Dealer(0, "Select Dealer");
                                dealers.Insert(0, selecyDealer);
                            }
                            Batt_Commission_DeviceDealer_ComboBox.Items = new List<object>(dealers);
                        }
                        Batt_Commission_DeviceDealer_ComboBox.SelectedIndex = 0;
                        Batt_Commission_DeviceDealer_ComboBox.SelectedItem = Batt_Commission_DeviceDealer_ComboBox.Items[Batt_Commission_DeviceDealer_ComboBox.SelectedIndex].ToString();


                        if (Batt_Commission_DeviceServiceDealer_ComboBox.Items.Count == 0)
                        {
                            List<Dealer> servicedealers = await usersList.getAllDealers(false);
                            //Batt_Commission_DeviceServiceDealer_ComboBox.DataSource = null;
                            Batt_Commission_DeviceServiceDealer_ComboBox.Items.Clear();
                            if (servicedealers.Find(x => x.id == 0) == null)
                            {
                                Dealer selecyDealer = new Dealer(0, "Select Service Dealer");
                                servicedealers.Insert(0, selecyDealer);
                            }
                            Batt_Commission_DeviceServiceDealer_ComboBox.Items = new List<object>(servicedealers);
                        }
                        Batt_Commission_DeviceServiceDealer_ComboBox.SelectedIndex = 0;
                        Batt_Commission_DeviceServiceDealer_ComboBox.SelectedItem = Batt_Commission_DeviceServiceDealer_ComboBox.Items[Batt_Commission_DeviceServiceDealer_ComboBox.SelectedIndex].ToString();


                        if (Batt_Commission_DeviceCustomers_ComboBox.Items.Count == 0)
                        {
                            List<Customer> customers = await usersList.getAllCustomers(false);
                            //Batt_Commission_DeviceCustomers_ComboBox.DataSource = null;
                            Batt_Commission_DeviceCustomers_ComboBox.Items.Clear();
                            if (customers.Find(x => x.id == 0) == null)
                            {
                                Customer selectCustomer = new Customer(0, "Select Customer");
                                customers.Insert(0, selectCustomer);
                            }
                            Batt_Commission_DeviceCustomers_ComboBox.Items = new List<object>(customers);
                        }
                        Batt_Commission_DeviceCustomers_ComboBox.SelectedIndex = 0;
                        Batt_Commission_DeviceCustomers_ComboBox.SelectedItem = Batt_Commission_DeviceCustomers_ComboBox.Items[Batt_Commission_DeviceCustomers_ComboBox.SelectedIndex].ToString();


                        //Batt_Commission_DeviceSites_ComboBox.DataSource = null;
                        Batt_Commission_DeviceSites_ComboBox.Items.Clear();
                        Batt_Commission_DeviceSites_ComboBox.SelectedIndex = -1;
                        IsBusy = false;
                    }
                }
                else if (sender != null)
                {
                    if (sender.Title == Batt_Commission_DeviceSynchListsButton.Title)
                    {
                        caller = ACConstants.DEVICE_COMMISSION_ACTIONS_LIST.all;
                    }
                    else if (sender.Title == Batt_Commission_DeviceSynchSitesButton.Title)
                    {
                        if (Batt_Commission_DeviceCustomers_ComboBox.SelectedIndex != 0)
                        {
                            List<object> arg1List = new List<object>();
                            Customer customer = (Customer)Batt_Commission_DeviceCustomers_ComboBox.Items[Batt_Commission_DeviceCustomers_ComboBox.SelectedIndex];
                            arg1List.Add(customer.id);
                            if (e == null)
                            {
                                //force refresh
                                arg1List.Add(false);
                            }
                            else
                            {
                                arg1List.Add(true);
                                //normal
                            }

                            caller = ACConstants.DEVICE_COMMISSION_ACTIONS_LIST.getSites;
                            arg1 = arg1List;
                        }
                    }
                    else if (sender.Title == Batt_Commission_DeviceCommissionButton.Title)
                    {
                        if (Batt_Commission_config.id == 0)
                        {
                            //Navigate to Connect to device 
                            //MessageBoxForm mb = new MessageBoxForm();
                            //battView_MenusShowHideInternal(null, null);
                            //mb.render("BATTView is Disconnected", MessageBoxFormTypes.error, MessageBoxFormButtons.OK);
                            Mvx.Resolve<IMvxMessenger>().Publish(new DeviceResetMessage(this, BattViewQuantum.Instance.GetBATTView().IPAddress));
                            ShowViewModel<ConnectToDeviceViewModel>(new { popto = "popto" });
                            ACUserDialogs.ShowAlert("BATTView is Disconnected");
                            return;
                        }
                        else if (Batt_Commission_DeviceDealer_ComboBox.SelectedIndex < 1)
                        {
                            ACUserDialogs.ShowAlert("Must SELECT a Dealer");
                            //MessageBoxForm mb = new MessageBoxForm();
                            //mb.render("Must SELECT a Dealer", MessageBoxFormTypes.Warning, MessageBoxFormButtons.OK);
                        }
                        else
                        {
                            IsBusy = true;
                            Dealer dealer = (Dealer)Batt_Commission_DeviceDealer_ComboBox.Items[Batt_Commission_DeviceDealer_ComboBox.SelectedIndex];
                            Batt_Commission_config.dealerid = dealer.id;

                            if (Batt_Commission_DeviceServiceDealer_ComboBox.Items.Count > 0)
                            {
                                Batt_Commission_config.servicedealerid = dealer.id;
                            }
                            else
                            {
                                Batt_Commission_config.servicedealerid = 0;
                            }

                            if (Batt_Commission_DeviceSites_ComboBox.Items.Count > 0)
                            {
                                Site site = (Site)Batt_Commission_DeviceSites_ComboBox.Items[Batt_Commission_DeviceSites_ComboBox.SelectedIndex];
                                Batt_Commission_config.siteid = site.id;
                            }
                            else
                            {
                                Batt_Commission_config.siteid = 0;
                            }

                            if (Batt_Commission_DeviceOEM_ComboBox.Items.Count > 0)
                            {
                                OEM oem = (OEM)Batt_Commission_DeviceOEM_ComboBox.Items[Batt_Commission_DeviceOEM_ComboBox.SelectedIndex];
                                Batt_Commission_config.oemid = oem.id;
                            }
                            else
                            {
                                Batt_Commission_config.oemid = 0;
                            }

                            if (Batt_Commission_DeviceCustomers_ComboBox.Items.Count > 0)
                            {
                                Customer customer = (Customer)Batt_Commission_DeviceCustomers_ComboBox.Items[Batt_Commission_DeviceCustomers_ComboBox.SelectedIndex];
                                Batt_Commission_config.customerid = customer.id;
                            }
                            else
                            {
                                Batt_Commission_config.customerid = 0;
                            }
                            arg1 = Batt_Commission_config.ToJson();

                            List<object> arguments = new List<object>();
                            arguments.Add(ActviewCommGeneric.commisionBattView);
                            arguments.Add(Batt_Commission_config.ToJson());
                            arguments.Add(false);
                            IsBusy = false;
                            ACTViewAction_Prepare(true, arguments);
                            return;
                        }
                    }
                }

                if (caller != ACConstants.DEVICE_COMMISSION_ACTIONS_LIST.nothing)
                {
                    List<object> arguments = new List<object>();
                    arguments.Add(caller);
                    arguments.Add(arg1);
                    Device_CommissionAction_Prepare(arguments, false);
                }
            }
            catch (Exception ex)
            {
                Logger.AddLog(true, "X18" + ex);

                //if (!setSepcialStatus(StatusSpecialMessage.ERROR))
                //setFormBusy(false);
                ACUserDialogs.ShowAlert("Status: Error");
            }
            IsBusy = false;
        }

        private void Batt_CommissionAction_doneWork(object e)
        {
            IsBusy = true;

            List<object> genericlist = e as List<object>;
            ACConstants.DEVICE_COMMISSION_ACTIONS_LIST caller = (ACConstants.DEVICE_COMMISSION_ACTIONS_LIST)genericlist[0];
            object arg1 = (object)genericlist[1];
            bool valid = (bool)genericlist[2];
            if (!valid)
            {
                /* if (!setSepcialStatus(StatusSpecialMessage.ERROR))
                     setFormBusy(false);*/
                IsBusy = false;
                ACUserDialogs.ShowAlert("Status: Error");
                return;
            }
            List<OEM> oems = new List<OEM>();
            List<Dealer> dealers = new List<Dealer>();
            List<Dealer> serviceDealers = new List<Dealer>();
            List<Customer> customers = new List<Customer>();
            List<Site> Sites = new List<Site>();
            bool allIsGood = true;
            try
            {
                switch (caller)
                {
                    case ACConstants.DEVICE_COMMISSION_ACTIONS_LIST.all:
                        {
                            oems = (List<OEM>)genericlist[3];
                            dealers = (List<Dealer>)genericlist[4];

                            customers = (List<Customer>)genericlist[5];
                            serviceDealers = (List<Dealer>)genericlist[6];
                        }
                        break;
                    case ACConstants.DEVICE_COMMISSION_ACTIONS_LIST.onlyCustomers:
                        {
                            customers = (List<Customer>)genericlist[3];
                        }
                        break;
                    case ACConstants.DEVICE_COMMISSION_ACTIONS_LIST.onlyOEMsAndDealers:
                        {
                            oems = (List<OEM>)genericlist[3];
                            dealers = (List<Dealer>)genericlist[4];
                            serviceDealers = (List<Dealer>)genericlist[5];
                        }
                        break;
                    case ACConstants.DEVICE_COMMISSION_ACTIONS_LIST.getSites:
                        Sites = (List<Site>)genericlist[3];//Batt_Commission_DeviceSites_ComboBox
                        break;
                }

                if (caller == ACConstants.DEVICE_COMMISSION_ACTIONS_LIST.all || caller == ACConstants.DEVICE_COMMISSION_ACTIONS_LIST.onlyOEMsAndDealers)
                {
                    //Batt_Commission_DeviceDealer_ComboBox.DataSource = null;
                    Batt_Commission_DeviceDealer_ComboBox.Items.Clear();
                    if (dealers.Find(x => x.id == 0) == null)
                    {
                        Dealer selecyDealer = new Dealer(0, "Select Dealer");
                        dealers.Insert(0, selecyDealer);
                    }
                    Batt_Commission_DeviceDealer_ComboBox.Items = new List<object>(dealers);
                    Batt_Commission_DeviceDealer_ComboBox.SelectedIndex = 0;
                    Batt_Commission_DeviceDealer_ComboBox.SelectedItem = Batt_Commission_DeviceDealer_ComboBox.Items[Batt_Commission_DeviceDealer_ComboBox.SelectedIndex].ToString();

                    Batt_Commission_DeviceServiceDealer_ComboBox.Items.Clear();
                    if (serviceDealers.Find(x => x.id == 0) == null)
                    {
                        Dealer selecyDealer = new Dealer(0, "Select Service Dealer");
                        serviceDealers.Insert(0, selecyDealer);
                    }
                    Batt_Commission_DeviceServiceDealer_ComboBox.Items = new List<object>(serviceDealers);
                    Batt_Commission_DeviceServiceDealer_ComboBox.SelectedIndex = 0;
                    Batt_Commission_DeviceServiceDealer_ComboBox.SelectedItem = Batt_Commission_DeviceServiceDealer_ComboBox.Items[Batt_Commission_DeviceServiceDealer_ComboBox.SelectedIndex].ToString();


                    Batt_Commission_DeviceOEM_ComboBox.Items.Clear();
                    if (oems.Find(x => x.id == 0) == null)
                    {
                        OEM selectOEM = new OEM(0, "Select OEM");
                        oems.Insert(0, selectOEM);
                    }
                    Batt_Commission_DeviceOEM_ComboBox.Items = new List<object>(oems);
                    Batt_Commission_DeviceOEM_ComboBox.SelectedIndex = 0;
                    Batt_Commission_DeviceOEM_ComboBox.SelectedItem = Batt_Commission_DeviceOEM_ComboBox.Items[Batt_Commission_DeviceOEM_ComboBox.SelectedIndex].ToString();


                    if (!usersList.AreOemsAndDealersLoaded())
                    {
                        allIsGood = false;
                    }
                }
                if (caller == ACConstants.DEVICE_COMMISSION_ACTIONS_LIST.all || caller == ACConstants.DEVICE_COMMISSION_ACTIONS_LIST.onlyCustomers)
                {
                    Batt_Commission_DeviceCustomers_ComboBox.Items.Clear();
                    if (customers.Find(x => x.id == 0) == null)
                    {
                        Customer selectCustomer = new Customer(0, "Select Customer");
                        customers.Insert(0, selectCustomer);

                    }

                    Batt_Commission_DeviceCustomers_ComboBox.Items = new List<object>(customers);
                    Batt_Commission_DeviceCustomers_ComboBox.SelectedIndex = 0;
                    Batt_Commission_DeviceCustomers_ComboBox.SelectedItem = Batt_Commission_DeviceCustomers_ComboBox.Items[Batt_Commission_DeviceCustomers_ComboBox.SelectedIndex].ToString();

                    Batt_Commission_DeviceSites_ComboBox.Items.Clear();
                    if (!usersList.AreCustomersLoaded())
                    {
                        allIsGood = false;
                    }

                }
                if (caller == ACConstants.DEVICE_COMMISSION_ACTIONS_LIST.getSites)
                {
                    Batt_Commission_DeviceSites_ComboBox.Items.Clear();
                    if (Sites.Find(x => x.id == 0) == null)
                    {
                        Site selectSite = new Site(0, "Select Site");
                        Sites.Insert(0, selectSite);
                    }
                    Batt_Commission_DeviceSites_ComboBox.Items = new List<object>(Sites);
                    Batt_Commission_DeviceSites_ComboBox.SelectedIndex = 0;
                    Batt_Commission_DeviceSites_ComboBox.SelectedItem = Batt_Commission_DeviceSites_ComboBox.Items[Batt_Commission_DeviceSites_ComboBox.SelectedIndex].ToString();

                    
                    List<object> arg1List = arg1 as List<object>;
                    if (!usersList.isThisCustomerSitesLoaded((UInt32)arg1List[0]))
                    {
                        allIsGood = false;
                    }

                }
            }
            catch (Exception ex)
            {
                allIsGood = false;
                Logger.AddLog(true, "X19" + ex.ToString());
            }
            IsBusy = false;
            if (allIsGood)
            {
                ACUserDialogs.ShowAlert("Status: Connected");
                //hideBusy = !setSepcialStatus(StatusSpecialMessage.connected);
            }
            else
            {
                ACUserDialogs.ShowAlert("Status: Error");
                //hideBusy = !setSepcialStatus(StatusSpecialMessage.ERROR);
            }
        }

        async void Batt_simpleCommunicationAction_Prepare(List<object> arg)
        {
            BattViewCommunicationTypes caller = (BattViewCommunicationTypes)arg[0];
            try
            {
                if (caller != BattViewCommunicationTypes.NOCall)
                {
                    List<object> results = new List<object>();
                    try
                    {
                        IsBusy = true;
                        results = await BattViewQuantum.Instance.CommunicateBATTView(arg);
                        IsBusy = false;
                        if (results.Count > 0)
                        {
                            var status = (CommunicationResult)results[2];
                            //if (caller != BATT_COMMUNICATION_TYPES.doFinalComission)
                            //{
                                if (status == CommunicationResult.OK)
                                {
                                    //navigate to connect to device screen
                                    //battView_MenusShowHide(null, null);
                                    //SerialNumbersList.SelectedIndex = 0;
                                    //showBusy = false;
                                    //scanRelated_prepare(scanRelatedTypes.doScan);
                                    Mvx.Resolve<IMvxMessenger>().Publish(new DeviceResetMessage(this, BattViewQuantum.Instance.GetBATTView().IPAddress));
                                    ShowViewModel<ConnectToDeviceViewModel>(new { popto = "popto" });
                                    ACUserDialogs.ShowAlert("Commission Done");
                                }
                            //}
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.AddLog(true, "X8" + ex);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.AddLog(true, "X8" + ex);
                return;
            }
        }

        //MCB_Commission_DeviceCustomers_ComboBox list selector functionality
        private void Batt_Commission_DeviceSites_ComboBox_SelectedIndexChanged(ListViewItem item)
        {
            Task.Run(async () =>
      {
          await Batt_Commission_DeviceSites_ComboBox_SelectedIndexChangedInternal(item);
      });

        }

        private async Task Batt_Commission_DeviceSites_ComboBox_SelectedIndexChangedInternal(ListViewItem item)
        {
            if (Batt_Commission_DeviceCustomers_ComboBox.SelectedIndex > 0)
            {
                Customer customer = (Customer)Batt_Commission_DeviceCustomers_ComboBox.Items[Batt_Commission_DeviceCustomers_ComboBox.SelectedIndex];
                if (customer.id != 0)
                {
                    await Batt_CommissionActionInternal(Batt_Commission_DeviceSynchSitesButton, null);
                }
            }

        }
        #endregion

        #region Charger create list 
        void CreateListForChargers()
        {
            CommissionNextItemSource.Clear();
            MCB_Commission_DeviceOEM_ComboBox = new ListViewItem
            {
                Index = 0,
                Title = "Battery OEM (Optional)",
                DefaultCellType = CellTypes.ListSelector,
                EditableCellType = CellTypes.ListSelector,
                ListSelectionCommand = ListSelectorCommand,
                ListSelectorType = ACUtility.ListSelectorType.OEM,
                Items = new List<object>()

            };
            CommissionNextItemSource.Add(MCB_Commission_DeviceOEM_ComboBox);

            MCB_Commission_DeviceDealer_ComboBox = new ListViewItem
            {
                Index = 1,
                Title = "Dealer (Required)",
                DefaultCellType = CellTypes.ListSelector,
                EditableCellType = CellTypes.ListSelector,
                ListSelectionCommand = ListSelectorCommand,
                ListSelectorType = ACUtility.ListSelectorType.Dealer,
                Items = new List<object>()
            };
            CommissionNextItemSource.Add(MCB_Commission_DeviceDealer_ComboBox);

            MCB_Commission_DeviceServiceDealer_ComboBox = new ListViewItem
            {
                Index = 2,
                Title = "Service Dealer (Optional)",
                DefaultCellType = CellTypes.ListSelector,
                EditableCellType = CellTypes.ListSelector,
                ListSelectionCommand = ListSelectorCommand,
                ListSelectorType = ACUtility.ListSelectorType.ServiceDealer,
                Items = new List<object>()
            };
            CommissionNextItemSource.Add(MCB_Commission_DeviceServiceDealer_ComboBox);

            MCB_Commission_DeviceCustomers_ComboBox = new ListViewItem
            {
                Index = 3,
                Title = "Customer (Optional)",
                DefaultCellType = CellTypes.ListSelector,
                EditableCellType = CellTypes.ListSelector,
                ListSelectionCommand = ListSelectorCommand,
                ListSelectorType = ACUtility.ListSelectorType.Customers,
                Items = new List<object>()
            };
            CommissionNextItemSource.Add(MCB_Commission_DeviceCustomers_ComboBox);

            MCB_Commission_DeviceSites_ComboBox = new ListViewItem
            {
                Index = 4,
                Title = "Site (Optional)",
                DefaultCellType = CellTypes.ListSelector,
                EditableCellType = CellTypes.ListSelector,
                ListSelectionCommand = ListSelectorCommand,
                ListSelectorType = ACUtility.ListSelectorType.Sites,
                Items = new List<object>()
            };
            CommissionNextItemSource.Add(MCB_Commission_DeviceSites_ComboBox);

            MCB_Commission_DeviceSynchSitesButton = new ListViewItem
            {
                Index = 5,
                Title = "Synchronize Sites",
                DefaultCellType = CellTypes.Button,
                EditableCellType = CellTypes.Button,
                ListSelectionCommand = MCBButtonSelectorCommand
            };
            CommissionNextItemSource.Add(MCB_Commission_DeviceSynchSitesButton);

            MCB_Commission_DeviceCommissionButton = new ListViewItem
            {
                Index = 6,
                Title = "Commission",
                DefaultCellType = CellTypes.Button,
                EditableCellType = CellTypes.Button,
                ListSelectionCommand = MCBButtonSelectorCommand
            };
            CommissionNextItemSource.Add(MCB_Commission_DeviceCommissionButton);

            MCB_Commission_DeviceSynchListsButton = new ListViewItem
            {
                Index = 7,
                Title = "Synchronize Lists",
                DefaultCellType = CellTypes.Button,
                EditableCellType = CellTypes.Button,
                ListSelectionCommand = MCBButtonSelectorCommand
            };
            CommissionNextItemSource.Add(MCB_Commission_DeviceSynchListsButton);

            Task.Run(async () =>
         {
             await MCB_CommissionActionInternal(null, null);
         });

        }

        public IMvxCommand ListSelectorCommand
        {
            get { return new MvxCommand<ListViewItem>(ExecuteListSelectorCommand); }
        }

        private void ExecuteListSelectorCommand(ListViewItem item)
        {
            if (item.Items != null)
            {
                _mListSelector = Mvx.Resolve<IMvxMessenger>().Subscribe<ListSelectorMessage>(OnListSelectorMessage);
                ShowViewModel<ListSelectorViewModel>(new { title = item.Title, type = item.ListSelectorType, selectedItemIndex = CommissionNextItemSource.IndexOf(item), ItemSourceStr = JsonConvert.SerializeObject(item.Items) });

            }
        }
        /// <summary>
        /// Ons the list selector message.
        /// </summary>
        /// <param name="obj">Object.</param>
        private void OnListSelectorMessage(ListSelectorMessage obj)
        {
            if (_mListSelector != null)
            {
                Mvx.Resolve<IMvxMessenger>().Unsubscribe<ListSelectorMessage>(_mListSelector);
                _mListSelector = null;
               
                    ListViewItem item = CommissionNextItemSource[obj.SelectedItemindex];
                    item.Text = obj.SelectedItem;
                    item.SelectedIndex = obj.SelectedIndex;

                    if (item.CellType == ACUtility.CellTypes.ListSelector && item.ListSelectorType == ACUtility.ListSelectorType.Customers)
                    {
                        if (IsBattView)
                    {
                        Batt_Commission_DeviceSites_ComboBox_SelectedIndexChanged(Batt_Commission_DeviceCustomers_ComboBox);
                    }
                    else
                    {
                        MCB_Commission_DeviceSites_ComboBox_SelectedIndexChanged(MCB_Commission_DeviceCustomers_ComboBox);
                    }

                }
               
            }
        }
        public IMvxCommand MCBButtonSelectorCommand
        {
            get { return new MvxCommand<ListViewItem>(ExecuteMCBButtonSelectorCommand); }
        }

        async void ExecuteMCBButtonSelectorCommand(ListViewItem item)
        {
            await MCB_CommissionActionInternal(item, null);
        }
        #endregion

        #region Commision MCB

        private async Task MCB_CommissionActionInternal(ListViewItem sender, EventArgs e)
        {
            try
            {
                ACConstants.DEVICE_COMMISSION_ACTIONS_LIST caller = ACConstants.DEVICE_COMMISSION_ACTIONS_LIST.nothing;
                object arg1 = null;
                //List<object> arguments = new List<object>();
                //arguments.Add(caller);
                //arguments.Add(arg1);
                if (sender == null)
                {
                    IsBusy = true;
                    if (!MCBQuantum.Instance.MCB_quickSerialNumberCheckStrict())
                    {
                        //MessageBoxForm mb = new MessageBoxForm();
                        //mb.render("Lost MSB Connection", MessageBoxFormTypes.Warning, MessageBoxFormButtons.OK);
                        ACUserDialogs.ShowAlert("Lost MSB Connection");
                        IsBusy = false;
                        return;
                    }
                    MCB_Commission_config = new MCBConfig();
                    MCB_Commission_config = StaticDataAndHelperFunctions.DeepClone<MCBConfig>(MCBQuantum.Instance.GetMCB().Config);
                    MCB_Commission_config.firmwareVersion = MCBQuantum.Instance.GetMCB().FirmwareRevision;
                    MCB_Commission_config.zoneID = MCBQuantum.Instance.GetMCB().myZone;

                    //MCB_MenusShowHideInternal(MCB_AdminActionsCommissionStep1Button, null);
                    if (!usersList.AreOemsAndDealersLoaded() && !usersList.AreCustomersLoaded())
                    {
                        caller = ACConstants.DEVICE_COMMISSION_ACTIONS_LIST.all;

                    }
                    else if (!usersList.AreOemsAndDealersLoaded())
                    {
                        caller = ACConstants.DEVICE_COMMISSION_ACTIONS_LIST.onlyOEMsAndDealers;
                    }
                    else if (!usersList.AreCustomersLoaded())
                    {
                        caller = ACConstants.DEVICE_COMMISSION_ACTIONS_LIST.onlyCustomers;

                    }
                    else
                    {
                        if (MCB_Commission_DeviceOEM_ComboBox.Items.Count == 0)
                        {
                            //MCB_Commission_DeviceOEM_ComboBox.DataSource = null;
                            if (MCB_Commission_DeviceOEM_ComboBox.Items != null)
                            {
                                MCB_Commission_DeviceOEM_ComboBox.Items.Clear();
                            }
                            List<OEM> oems = await usersList.getAllOEMs(false);
                            if (oems.Find(x => x.id == 0) == null)
                            {
                                OEM selectOEM = new OEM(0, "Select OEM");
                                oems.Insert(0, selectOEM);
                            }
                            MCB_Commission_DeviceOEM_ComboBox.Items = new List<object>(oems);
                        }
                        MCB_Commission_DeviceOEM_ComboBox.SelectedIndex = 0;
                        MCB_Commission_DeviceOEM_ComboBox.SelectedItem = MCB_Commission_DeviceOEM_ComboBox.Items[MCB_Commission_DeviceOEM_ComboBox.SelectedIndex].ToString();


                        if (MCB_Commission_DeviceDealer_ComboBox.Items.Count == 0)
                        {
                            List<Dealer> dealers = await usersList.getAllDealers(false);
                            //MCB_Commission_DeviceDealer_ComboBox.DataSource = null;
                            if (MCB_Commission_DeviceDealer_ComboBox.Items != null)
                            {
                                MCB_Commission_DeviceDealer_ComboBox.Items.Clear();
                            }
                            if (dealers.Find(x => x.id == 0) == null)
                            {
                                Dealer selecyDealer = new Dealer(0, "Select Dealer");
                                dealers.Insert(0, selecyDealer);
                            }
                            MCB_Commission_DeviceDealer_ComboBox.Items = new List<object>(dealers);
                        }
                        MCB_Commission_DeviceDealer_ComboBox.SelectedIndex = 0;
                        MCB_Commission_DeviceDealer_ComboBox.SelectedItem = MCB_Commission_DeviceDealer_ComboBox.Items[MCB_Commission_DeviceDealer_ComboBox.SelectedIndex].ToString();

                        if (MCB_Commission_DeviceServiceDealer_ComboBox.Items.Count == 0)
                        {
                            List<Dealer> servicedealers = await usersList.getAllDealers(false);
                            //MCB_Commission_DeviceServiceDealer_ComboBox.DataSource = null;
                            if (MCB_Commission_DeviceServiceDealer_ComboBox.Items != null)
                            {
                                MCB_Commission_DeviceServiceDealer_ComboBox.Items.Clear();
                            }
                            if (servicedealers.Find(x => x.id == 0) == null)
                            {
                                Dealer selecyDealer = new Dealer(0, "Select Service Dealer");
                                servicedealers.Insert(0, selecyDealer);
                            }
                            MCB_Commission_DeviceServiceDealer_ComboBox.Items = new List<object>(servicedealers);
                        }
                        MCB_Commission_DeviceServiceDealer_ComboBox.SelectedIndex = 0;
                        MCB_Commission_DeviceServiceDealer_ComboBox.SelectedItem = MCB_Commission_DeviceServiceDealer_ComboBox.Items[MCB_Commission_DeviceServiceDealer_ComboBox.SelectedIndex].ToString();


                        if (MCB_Commission_DeviceCustomers_ComboBox.Items.Count == 0)
                        {
                            List<Customer> customers = await usersList.getAllCustomers(false);
                            //MCB_Commission_DeviceCustomers_ComboBox.DataSource = null;

                            if (MCB_Commission_DeviceCustomers_ComboBox.Items != null)
                            {
                                MCB_Commission_DeviceCustomers_ComboBox.Items.Clear();
                            }
                            if (customers.Find(x => x.id == 0) == null)
                            {
                                Customer selectCustomer = new Customer(0, "Select Customer");
                                customers.Insert(0, selectCustomer);
                            }
                            MCB_Commission_DeviceCustomers_ComboBox.Items = new List<object>(customers);
                        }
                        MCB_Commission_DeviceCustomers_ComboBox.SelectedIndex = 0;
                        MCB_Commission_DeviceCustomers_ComboBox.SelectedItem = MCB_Commission_DeviceCustomers_ComboBox.Items[MCB_Commission_DeviceCustomers_ComboBox.SelectedIndex].ToString();


                        //MCB_Commission_DeviceSites_ComboBox.DataSource = null;
                        if (MCB_Commission_DeviceSites_ComboBox.Items != null)
                        {
                            MCB_Commission_DeviceSites_ComboBox.Items.Clear();
                        }
                        MCB_Commission_DeviceSites_ComboBox.SelectedIndex = -1;
                        //scanRelated_prepare(scanRelatedTypes.doScan);

                    }
                    IsBusy = false;
                }
                else if (sender.Title == MCB_Commission_DeviceSynchListsButton.Title)
                {
                    caller = ACConstants.DEVICE_COMMISSION_ACTIONS_LIST.all;
                }
                else if (sender.Title == MCB_Commission_DeviceSynchSitesButton.Title)
                {
                    if (MCB_Commission_DeviceCustomers_ComboBox.SelectedIndex != 0)
                    {
                        List<object> arg1List = new List<object>();

                        Customer customer = (Customer)MCB_Commission_DeviceCustomers_ComboBox.Items[MCB_Commission_DeviceCustomers_ComboBox.SelectedIndex];
                        arg1List.Add(customer.id);
                        if (e == null)
                        {
                            //force refresh
                            arg1List.Add(false);
                        }
                        else
                        {
                            arg1List.Add(true);
                            //normal
                        }

                        caller = ACConstants.DEVICE_COMMISSION_ACTIONS_LIST.getSites;
                        arg1 = arg1List;
                    }
                }
                else if (sender.Title == MCB_Commission_DeviceCommissionButton.Title)
                {
                    if (MCB_Commission_config.id == "0")
                    {
                        //MessageBoxForm mb = new MessageBoxForm();
                        //MCB_MenusShowHideInternal(null, null);
                        //mb.render("MCB is Disconnected", MessageBoxFormTypes.error, MessageBoxFormButtons.OK);
                        //Navigate to Connect to device screen
                        Mvx.Resolve<IMvxMessenger>().Publish(new DeviceResetMessage(this, MCBQuantum.Instance.GetMCB().IPAddress));
                        ShowViewModel<ConnectToDeviceViewModel>(new { popto = "popto" });
                        //ACUserDialogs.HideDialog();
                        ACUserDialogs.ShowAlert("MCB is Disconnected");
                        return;
                    }
                    else
                        if (MCB_Commission_DeviceDealer_ComboBox.SelectedIndex < 1)
                    {
                        //MessageBoxForm mb = new MessageBoxForm();
                        //mb.render("Must SELECT a Dealer", MessageBoxFormTypes.Warning, MessageBoxFormButtons.OK);
                        //ACUserDialogs.HideDialog();
                        ACUserDialogs.ShowAlert("Must SELECT a Dealer");
                    }
                    else
                    {
                        Dealer dealer = (Dealer)MCB_Commission_DeviceDealer_ComboBox.Items[MCB_Commission_DeviceDealer_ComboBox.SelectedIndex];

                        MCB_Commission_config.dealerid = dealer.id;

                        if (MCB_Commission_DeviceServiceDealer_ComboBox.Items.Count > 0)
                        {
                            MCB_Commission_config.servicedealerid = dealer.id;
                        }
                        else
                        {
                            MCB_Commission_config.servicedealerid = 0;
                        }

                        if (MCB_Commission_DeviceSites_ComboBox.Items.Count > 0)
                        {
                            Site site = (Site)MCB_Commission_DeviceSites_ComboBox.Items[MCB_Commission_DeviceSites_ComboBox.SelectedIndex];
                            MCB_Commission_config.siteid = site.id;
                        }
                        else
                        {
                            MCB_Commission_config.siteid = 0;
                        }

                        if (MCB_Commission_DeviceOEM_ComboBox.Items.Count > 0)
                        {
                            OEM oem = (OEM)MCB_Commission_DeviceOEM_ComboBox.Items[MCB_Commission_DeviceOEM_ComboBox.SelectedIndex];
                            MCB_Commission_config.oemid = oem.id;
                        }
                        else
                        {
                            MCB_Commission_config.oemid = 0;
                        }

                        if (MCB_Commission_DeviceCustomers_ComboBox.Items.Count > 0)
                        {
                            Customer customer = (Customer)MCB_Commission_DeviceCustomers_ComboBox.Items[MCB_Commission_DeviceCustomers_ComboBox.SelectedIndex];
                            MCB_Commission_config.customerid = customer.id;
                        }
                        else
                        {
                            MCB_Commission_config.customerid = 0;
                        }
                        arg1 = MCB_Commission_config.ToJson();
                        List<object> arguments = new List<object>();
                        arguments.Add(ActviewCommGeneric.commisionMCB);
                        arguments.Add(MCB_Commission_config.ToJson());
                        arguments.Add(false);
                        ACTViewAction_Prepare(true, arguments);
                        return;
                        //ok get the MCB object JSON
                    }
                }

                if (caller != ACConstants.DEVICE_COMMISSION_ACTIONS_LIST.nothing)
                {
                    List<object> arguments = new List<object>();
                    arguments.Add(caller);
                    arguments.Add(arg1);
                    Device_CommissionAction_Prepare(arguments, true);
                }
            }
            catch (Exception ex)
            {
                Logger.AddLog(true, "X84" + ex.ToString());

                //if (!setSepcialStatus(StatusSpecialMessage.ERROR))
                //    setFormBusy(false);
                ACUserDialogs.ShowAlert("Status: Error");
            }
        }


        private MCBConfig MCB_Commission_config;//Not accessed except by one thread

        private void MCB_CommissionAction_doneWork(object e)
        {
            IsBusy = true;
            List<object> genericlist = e as List<object>;
            ACConstants.DEVICE_COMMISSION_ACTIONS_LIST caller = (ACConstants.DEVICE_COMMISSION_ACTIONS_LIST)genericlist[0];
            object arg1 = (object)genericlist[1];
            bool valid = (bool)genericlist[2];
            if (!valid)
            {
                //if (!setSepcialStatus(StatusSpecialMessage.ERROR))
                //    setFormBusy(false);
                ACUserDialogs.ShowAlert("Status: Error");
                IsBusy = false;
                return;
            }
            List<OEM> oems = new List<OEM>();
            List<Dealer> dealers = new List<Dealer>();
            List<Dealer> serviceDealers = new List<Dealer>();
            List<Customer> customers = new List<Customer>();
            List<Site> Sites = new List<Site>();
            bool allIsGood = true;

            try
            {
                switch (caller)
                {
                    case ACConstants.DEVICE_COMMISSION_ACTIONS_LIST.all:
                        {
                            oems = (List<OEM>)genericlist[3];
                            dealers = (List<Dealer>)genericlist[4];

                            customers = (List<Customer>)genericlist[5];
                            serviceDealers = (List<Dealer>)genericlist[6];
                        }
                        break;
                    case ACConstants.DEVICE_COMMISSION_ACTIONS_LIST.onlyCustomers:
                        {
                            customers = (List<Customer>)genericlist[3];
                        }
                        break;
                    case ACConstants.DEVICE_COMMISSION_ACTIONS_LIST.onlyOEMsAndDealers:
                        {
                            oems = (List<OEM>)genericlist[3];
                            dealers = (List<Dealer>)genericlist[4];
                            serviceDealers = (List<Dealer>)genericlist[5];
                        }
                        break;
                    case ACConstants.DEVICE_COMMISSION_ACTIONS_LIST.getSites:
                        Sites = (List<Site>)genericlist[3];//MCB_Commission_DeviceSites_ComboBox
                        break;
                }

                if (caller == ACConstants.DEVICE_COMMISSION_ACTIONS_LIST.all || caller == ACConstants.DEVICE_COMMISSION_ACTIONS_LIST.onlyOEMsAndDealers)
                {
                    if (MCB_Commission_DeviceDealer_ComboBox.Items != null)
                    {
                        MCB_Commission_DeviceDealer_ComboBox.Items.Clear();
                    }
                    if (dealers.Find(x => x.id == 0) == null)
                    {
                        Dealer selecyDealer = new Dealer(0, "Select Dealer");
                        dealers.Insert(0, selecyDealer);
                    }
                    MCB_Commission_DeviceDealer_ComboBox.Items = new List<object>(dealers);
                    MCB_Commission_DeviceDealer_ComboBox.SelectedIndex = 0;
                    MCB_Commission_DeviceDealer_ComboBox.SelectedItem = MCB_Commission_DeviceDealer_ComboBox.Items[MCB_Commission_DeviceDealer_ComboBox.SelectedIndex].ToString();

                    if (MCB_Commission_DeviceServiceDealer_ComboBox.Items != null)
                    {
                        MCB_Commission_DeviceServiceDealer_ComboBox.Items.Clear();
                    }
                    if (serviceDealers.Find(x => x.id == 0) == null)
                    {
                        Dealer selecyDealer = new Dealer(0, "Select Service Dealer");
                        serviceDealers.Insert(0, selecyDealer);
                    }
                    MCB_Commission_DeviceServiceDealer_ComboBox.Items = new List<object>(serviceDealers);
                    MCB_Commission_DeviceServiceDealer_ComboBox.SelectedIndex = 0;
                    MCB_Commission_DeviceServiceDealer_ComboBox.SelectedItem = MCB_Commission_DeviceServiceDealer_ComboBox.Items[MCB_Commission_DeviceServiceDealer_ComboBox.SelectedIndex].ToString();


                    if (MCB_Commission_DeviceOEM_ComboBox.Items != null)
                    {
                        MCB_Commission_DeviceOEM_ComboBox.Items.Clear();
                    }
                    if (oems.Find(x => x.id == 0) == null)
                    {
                        OEM selectOEM = new OEM(0, "Select OEM");
                        oems.Insert(0, selectOEM);
                    }
                    MCB_Commission_DeviceOEM_ComboBox.Items = new List<object>(oems);
                    MCB_Commission_DeviceOEM_ComboBox.SelectedIndex = 0;
                    MCB_Commission_DeviceOEM_ComboBox.SelectedItem = MCB_Commission_DeviceOEM_ComboBox.Items[MCB_Commission_DeviceOEM_ComboBox.SelectedIndex].ToString();

                    if (!usersList.AreOemsAndDealersLoaded())
                    {
                        allIsGood = false;
                    }
                }
                if (caller == ACConstants.DEVICE_COMMISSION_ACTIONS_LIST.all || caller == ACConstants.DEVICE_COMMISSION_ACTIONS_LIST.onlyCustomers)
                {
                    if (MCB_Commission_DeviceCustomers_ComboBox.Items != null)
                    {
                        MCB_Commission_DeviceCustomers_ComboBox.Items.Clear();
                    }
                    if (customers.Find(x => x.id == 0) == null)
                    {
                        Customer selectCustomer = new Customer(0, "Select Customer");
                        customers.Insert(0, selectCustomer);
                    }

                    MCB_Commission_DeviceCustomers_ComboBox.Items = new List<object>(customers);
                    MCB_Commission_DeviceCustomers_ComboBox.SelectedIndex = 0;
                    MCB_Commission_DeviceCustomers_ComboBox.SelectedItem = MCB_Commission_DeviceCustomers_ComboBox.Items[MCB_Commission_DeviceCustomers_ComboBox.SelectedIndex].ToString();

                    if (MCB_Commission_DeviceSites_ComboBox.Items != null)
                    {
                        MCB_Commission_DeviceSites_ComboBox.Items.Clear();
                    }
                    if (!usersList.AreCustomersLoaded())
                    {
                        allIsGood = false;
                    }

                }
                if (caller == ACConstants.DEVICE_COMMISSION_ACTIONS_LIST.getSites)
                {
                    if (MCB_Commission_DeviceSites_ComboBox.Items != null)
                    {
                        MCB_Commission_DeviceSites_ComboBox.Items.Clear();
                    }
                    if (Sites.Find(x => x.id == 0) == null)
                    {
                        Site selectSite = new Site(0, "Select Site");
                        Sites.Insert(0, selectSite);
                    }
                    MCB_Commission_DeviceSites_ComboBox.Items = new List<object>(Sites);
                    MCB_Commission_DeviceSites_ComboBox.SelectedIndex = 0;
                    MCB_Commission_DeviceSites_ComboBox.SelectedItem = MCB_Commission_DeviceSites_ComboBox.Items[MCB_Commission_DeviceSites_ComboBox.SelectedIndex].ToString();

                    List<object> arg1List = arg1 as List<object>;
                    if (!usersList.isThisCustomerSitesLoaded((UInt32)arg1List[0]))
                    {
                        allIsGood = false;
                    }
                }
                if (allIsGood)
                {
                    ACUserDialogs.ShowAlert("Status: Connected");

                }
                else
                {
                    ACUserDialogs.ShowAlert("Status: Error");
                }
            }

            catch (Exception ex)
            {
                allIsGood = false;
                Logger.AddLog(true, "X83" + ex);
            }
            IsBusy = false;

        }

        async void MCB_simpleCommunicationAction_Prepare(List<object> arg)
        {
            McbCommunicationTypes caller = (McbCommunicationTypes)arg[0];
            try
            {

                if (caller != McbCommunicationTypes.NOCall)
                {
                    List<object> results = new List<object>();
                    try
                    {
                        IsBusy = true;
                        results = await MCBQuantum.Instance.CommunicateMCB(arg);
                        IsBusy = false;
                        if (results.Count > 0)
                        {
                            var status = (CommunicationResult)results[2];
                            if (status == CommunicationResult.OK)
                            {
                                //battView_MenusShowHide(null, null);
                                //SerialNumbersList.SelectedIndex = 0;
                                //showBusy = false;
                                //scanRelated_prepare(scanRelatedTypes.doScan);
                                Mvx.Resolve<IMvxMessenger>().Publish(new DeviceResetMessage(this, MCBQuantum.Instance.GetMCB().IPAddress));
                                ShowViewModel<ConnectToDeviceViewModel>(new { popto = "popto" });
                                ACUserDialogs.ShowAlert("Commission Done");
                            }

                        }

                    }
                    catch (Exception ex)
                    {
                        Logger.AddLog(true, "X8" + ex);
                    }

                }
            }
            catch (Exception ex)
            {
                Logger.AddLog(true, "X8" + ex);
            }

        }
        #endregion

        #region Common functions 
        private async void ACTViewAction_Prepare(bool userAction, List<object> arguments)
        {

            try
            {
                arguments.Insert(0, userAction);
                List<object> results = new List<object>();
                IsBusy = true;
                results = await ACTVIEWQuantum.Instance.CommunicateACTView(arguments);
                IsBusy = false;
                if (results != null && results.Count > 0)
                {
                    bool internalFailure = (bool)results[0];
                    string internalFailureString = (string)results[1];
                    ACTViewResponse status = (ACTViewResponse)results[2];
                    ActviewCommGeneric caller = (ActviewCommGeneric)results[3];
                    bool removeSetBusy = (bool)results[4];
                    List<object> ProcArgumentList = (List<object>)results[5];

                    if (caller == ActviewCommGeneric.commisionBattView)
                    {
                        if (status.returnValue["duplicate"] != null && status.returnValue["duplicate"].Type == Newtonsoft.Json.Linq.JTokenType.Boolean && (bool)status.returnValue["duplicate"])
                        {
                            ACUserDialogs.ShowAlertWithTwoButtons("Serial Number Exists, Do you wish to override the existing one? (If you are not sure,it is possible Monkeys Will jump in ACTView) ", "", AppResources.yes, AppResources.no, () => OnYesClick(ProcArgumentList), null);
                           
                        }
                        else
                        {
                            if (status.returnValue["newID"] != null && status.returnValue["newID"].Type == Newtonsoft.Json.Linq.JTokenType.Integer && (UInt32)status.returnValue["newID"] != 0)
                            {
                                List<object> arg = new List<object>();
                                List<object> vars = new List<object>();
                                arg.Add(BattViewCommunicationTypes.doFinalComission);
                                vars.Add((UInt32)status.returnValue["newID"]);
                                arg.Add(vars);
                                removeSetBusy = false;
                                Batt_simpleCommunicationAction_Prepare(arg);
                            }
                            else
                            {
                                ACUserDialogs.ShowAlert("Commission Failed,Please try again");
                            }
                        }
                    }
                    else if (caller == ActviewCommGeneric.commisionMCB)
                    {
                        if (status.returnValue["duplicate"] != null && status.returnValue["duplicate"].Type == Newtonsoft.Json.Linq.JTokenType.Boolean && (bool)status.returnValue["duplicate"])
                        {
                            ACUserDialogs.ShowAlertWithTwoButtons("Serial Number Exists, Do you wish to override the existing one? (If you are not sure,it is possible Monkeys Will jump in ACTView) ", "", AppResources.yes, AppResources.no, () => OnYesClick(ProcArgumentList), null);
                        }
                        else
                        {
                            if (status.returnValue["newID"] != null && status.returnValue["newID"].Type == Newtonsoft.Json.Linq.JTokenType.Integer && (UInt32)status.returnValue["newID"] != 0
                                && status.returnValue["boardID"] != null && status.returnValue["boardID"].Type == Newtonsoft.Json.Linq.JTokenType.Integer && (UInt32)status.returnValue["boardID"] != 0)
                            {
                                List<object> arg = new List<object>();
                                List<object> vars = new List<object>();
                                arg.Add(McbCommunicationTypes.doFinalComission);
                                vars.Add((UInt32)status.returnValue["newID"]);
                                vars.Add((UInt32)status.returnValue["boardID"]);
                                arg.Add(vars);
                                removeSetBusy = false;
                                MCB_simpleCommunicationAction_Prepare(arg);
                            }
                            else
                            {
                                //MessageBoxForm mb3 = new MessageBoxForm();
                                //mb3.render("Commission Failed,Please try again", MessageBoxFormTypes.error, MessageBoxFormButtons.OK);
                                ACUserDialogs.ShowAlert("Commission Failed,Please try again");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.AddLog(true, "X86" + ex);
                //return;
            }
            //ACUserDialogs.HideDialog();
        }

        void OnYesClick(List<object> ProcArgumentList)
        {
            List<object> newArguments = new List<object>();
            if (IsBattView)
                newArguments.Add(ActviewCommGeneric.commisionBattView);
            else
                newArguments.Add(ActviewCommGeneric.commisionMCB);

            newArguments.Add((string)ProcArgumentList[0]);
            newArguments.Add(true);
            //removeSetBusy = false;
            ACTViewAction_Prepare(true, newArguments);
        }


        private async void Device_CommissionAction_Prepare(object arguments, bool isMCB)
        {


            List<object> genericlist = arguments as List<object>;
            List<object> result = new List<object>();

            ACConstants.DEVICE_COMMISSION_ACTIONS_LIST caller = (ACConstants.DEVICE_COMMISSION_ACTIONS_LIST)genericlist[0];
            object arg1 = (object)genericlist[1];
            result.Add(caller);
            result.Add(arg1);
            result.Add(true);
            try
            {
                IsBusy = true;
                switch (caller)
                {
                    case ACConstants.DEVICE_COMMISSION_ACTIONS_LIST.all:
                        List<OEM> allOems = await usersList.getAllOEMs(true);
                        result.Add(allOems);

                        List<Dealer> allDealers = await usersList.getAllDealers(false);
                        result.Add(allDealers);

                        List<Customer> allCustomers = await usersList.getAllCustomers(true);
                        result.Add(allCustomers);

                        List<Dealer> dealers = await usersList.getAllDealers(false);
                        result.Add(dealers);

                        break;
                    case ACConstants.DEVICE_COMMISSION_ACTIONS_LIST.onlyCustomers:
                        result.Add(usersList.getAllCustomers(true));
                        break;
                    case ACConstants.DEVICE_COMMISSION_ACTIONS_LIST.onlyOEMsAndDealers:
                        result.Add(usersList.getAllOEMs(true));
                        result.Add(usersList.getAllDealers(false));
                        result.Add(usersList.getAllDealers(false));
                        break;
                    case ACConstants.DEVICE_COMMISSION_ACTIONS_LIST.getSites:
                        List<object> arg1List = arg1 as List<object>;

                        List<Site> sites = await usersList.getCustomerSites((UInt32)arg1List[0], (bool)arg1List[1]);
                        result.Add(sites);

                        break;
                }
                IsBusy = false;
                try
                {

                    if (IsBattView)
                    {
                            Batt_CommissionAction_doneWork(result);
                    }
                    else
                    {
                        MCB_CommissionAction_doneWork(result);
                    }
                }
                catch (Exception ex)
                {
                    Logger.AddLog(true, "X86" + ex);
                    return;
                }
            }
            catch (Exception ex)
            {
                Logger.AddLog(true, "X86" + ex);
                result.RemoveAt(2);
                result.Add(false);
            }

        }
        //MCB_Commission_DeviceCustomers_ComboBox list selector functionality
        private void MCB_Commission_DeviceSites_ComboBox_SelectedIndexChanged(ListViewItem item)
        {
            Task.Run(async () =>
      {
          await MCB_Commission_DeviceSites_ComboBox_SelectedIndexChangedInternal(item);
      });

        }

        private async Task MCB_Commission_DeviceSites_ComboBox_SelectedIndexChangedInternal(ListViewItem item)
        {
            if (MCB_Commission_DeviceCustomers_ComboBox.SelectedIndex > 0)
            {
                Customer customer = (Customer)MCB_Commission_DeviceCustomers_ComboBox.Items[MCB_Commission_DeviceCustomers_ComboBox.SelectedIndex];
                if (customer.id != 0)
                {
                    await MCB_CommissionActionInternal(MCB_Commission_DeviceSynchSitesButton, null);
                }
            }
        }
        #endregion
    }
}