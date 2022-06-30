using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;

namespace actchargers
{
    public class SiteViewDevicesController
    {
        readonly uint siteId;
        readonly bool isWithSynchSites;

        SiteViewListBuilder siteViewListBuilder;
        ConnectedDevicesPinger connectedDevicesPinger;

        public event EventHandler OnListUpdated;

        public ObservableCollection<SiteViewDeviceObject> SiteViewChargersList
        {
            get;
            private set;
        }

        public ObservableCollection<SiteViewDeviceObject> SiteViewBattviewsList
        {
            get;
            private set;
        }

        public ObservableCollection<SiteViewDeviceObject> SiteViewBattviewMobilesList
        {
            get;
            private set;
        }

        public SiteViewDevicesController(uint siteId, bool isWithSynchSites)
        {
            this.siteId = siteId;
            this.isWithSynchSites = isWithSynchSites;

            Init();
        }

        void Init()
        {
            SiteViewChargersList = new ObservableCollection<SiteViewDeviceObject>();
            SiteViewBattviewsList = new ObservableCollection<SiteViewDeviceObject>();
            SiteViewBattviewMobilesList = new ObservableCollection<SiteViewDeviceObject>();

            siteViewListBuilder = new SiteViewListBuilder(siteId, isWithSynchSites);

            InitForScan();
        }

        #region Scan

        void InitForScan()
        {
            connectedDevicesPinger = new ConnectedDevicesPinger();
            connectedDevicesPinger.OnDeviceDisconnected += ConnectedDevicesPinger_OnDeviceDisconnected;

            var addDeviceSubscriptionToken =
                Mvx.Resolve<IMvxMessenger>().Subscribe
                   <AddDeviceMessage>(OnAddDeviceMessageReceive);

            Task.Run(() => connectedDevicesPinger.StartTimerForConnectedDevices());
        }

        void ConnectedDevicesPinger_OnDeviceDisconnected(object sender, EventArgs e)
        {
            FireOnListUpdated();
        }

        void OnAddDeviceMessageReceive(AddDeviceMessage obj)
        {
            FireOnListUpdated();
        }

        #endregion

        void FireOnListUpdated()
        {
            OnListUpdated?.Invoke(this, EventArgs.Empty);
        }

        public ObservableCollection<SiteViewDeviceObject>
        GetListByType(DevicesTabs type)
        {
            FillLists();

            ObservableCollection<SiteViewDeviceObject> list;

            switch (type)
            {
                case DevicesTabs.CHARGER:
                    list = SiteViewChargersList;

                    break;

                case DevicesTabs.BATTVIEW:
                    list = SiteViewBattviewsList;

                    break;

                case DevicesTabs.BATTVIEW_MOBILE:
                    list = SiteViewBattviewMobilesList;

                    break;

                default:
                    list = new ObservableCollection<SiteViewDeviceObject>();

                    break;
            }

            GlobalLists.List = list.ToList();

            return list;
        }

        void FillLists()
        {
            ClearLists();

            var fullList = siteViewListBuilder.GetFullList();

            foreach (var item in fullList)
                PutItemInCorrectList(item);
        }

        void ClearLists()
        {
            SiteViewChargersList.Clear();
            SiteViewBattviewsList.Clear();
            SiteViewBattviewMobilesList.Clear();
        }

        void PutItemInCorrectList(SiteViewDeviceObject item)
        {
            switch (item.DeviceType)
            {
                case DeviceTypes.mcb:
                    SiteViewChargersList.Add(item);

                    break;

                case DeviceTypes.battview:
                    SiteViewBattviewsList.Add(item);

                    break;

                case DeviceTypes.battviewMobile:
                    SiteViewBattviewMobilesList.Add(item);

                    break;
            }
        }

        public void Close()
        {
            connectedDevicesPinger.StopTimer();
        }
    }
}
