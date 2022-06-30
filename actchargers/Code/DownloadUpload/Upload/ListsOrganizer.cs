using System;

namespace actchargers
{
    public class ListsOrganizer
    {
        AllSourceLists allSourceLists;
        UploadableDeviceDictionary mainDictionary;

        UploadableDevicesLists devicesLists =
           new UploadableDevicesLists();
        public UploadableDevicesLists DevicesLists
        {
            get { return devicesLists; }
        }

        public ListsOrganizer(AllSourceLists allSourceLists)
        {
            this.allSourceLists = allSourceLists;

            Init();
        }

        void Init()
        {
            mainDictionary = new UploadableDeviceDictionary();
        }

        public void Organize()
        {
            OrganizeToOneDictionary();
            OrganizeToSections();
        }

        void OrganizeToOneDictionary()
        {
            OrganizeReplacements();
            OrganizeDevices();
            OrganizeCycles();
            OrganizePms();
            OrganizeEvents();
        }

        void OrganizeReplacements()
        {
            foreach (var item in allSourceLists.AllLists.Replacements)
                mainDictionary.AddOrAppendReplacement
                              (item, item.GetOriginalDevice());
        }

        void OrganizeDevices()
        {
            foreach (var item in allSourceLists.AllLists.Devices)
                mainDictionary.AddOrAppendDevice(item);
        }

        void OrganizeCycles()
        {
            foreach (var item in allSourceLists.AllLists.Cycles)
                mainDictionary.AddOrAppendCycle(item, item.GetDevice());
        }

        void OrganizePms()
        {
            foreach (var item in allSourceLists.AllLists.Pms)
                mainDictionary.AddOrAppendPm(item, item.GetDevice());
        }

        void OrganizeEvents()
        {
            foreach (var item in allSourceLists.AllLists.BattviewEvents)
                mainDictionary.AddOrAppendBattviewEvent(item, item.GetDevice());
        }

        void OrganizeToSections()
        {
            var all = mainDictionary.MainList;
            foreach (var item in all)
            {
                PutItemInCorrectList(item);
            }
        }

        void PutItemInCorrectList(UploadableDeviceViewModel item)
        {
            var type = item.Device.GetDeviceType();
            switch (type)
            {
                case DeviceType.MCB:
                    DevicesLists.Chargers.Add(item);
                    break;

                case DeviceType.BATTVIEW:
                    DevicesLists.Battviews.Add(item);
                    break;

                case DeviceType.BATTVIEW_MOBILE:
                    DevicesLists.BattviewMobiles.Add(item);
                    break;
            }
        }
    }
}
