using System.Collections.Generic;
using System.Linq;

namespace actchargers
{
    public static class GlobalLists
    {
        public static List<SiteViewDeviceObject> List;

        public static List<BattViewObject> GetActiveBattViewList()
        {
            var globalList = GetGlobalConnectedList();
            var managedDevices = SiteViewQuantum.Instance.GetConnectionManager().managedBATTViews;

            var list = new List<BattViewObject>();

            if (globalList == null || managedDevices == null)
                return list;

            foreach (var item in globalList)
            {
                string serialNumber = item.InterfaceSn;

                if (managedDevices.keyExists(serialNumber))
                {
                    var oneManagedDevice = managedDevices.getDeviceByKey(serialNumber);

                    list.Add(oneManagedDevice.battview);
                }
            }

            return list;
        }

        public static List<MCBobject> GetActiveMcbList()
        {
            var globalList = GetGlobalConnectedList();
            var managedDevices = SiteViewQuantum.Instance.GetConnectionManager().managedMCBs;

            var list = new List<MCBobject>();

            if (globalList == null || managedDevices == null)
                return list;

            foreach (var item in globalList)
            {
                string serialNumber = item.InterfaceSn;

                if (managedDevices.keyExists(serialNumber))
                {
                    var oneManagedDevice = managedDevices.getDeviceByKey(serialNumber);

                    list.Add(oneManagedDevice.mcb);
                }
            }

            return list;
        }

        public static List<SiteViewDeviceObject> GetGlobalConnectedList()
        {
            if (List == null)
                return null;

            return List.Where((arg) => arg.IsCheckedAndConnected()).ToList();
        }

        public static bool HasGlobalConnected()
        {
            return List.Any((arg) => arg.IsCheckedAndConnected());
        }

        public static bool IsEmptyGlobalConnected()
        {
            return !HasGlobalConnected();
        }
    }
}
