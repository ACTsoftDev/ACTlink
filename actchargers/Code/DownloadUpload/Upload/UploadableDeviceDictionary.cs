using System.Collections.Generic;
using System.Linq;

namespace actchargers
{
    public class UploadableDeviceDictionary
    {
        List<UploadableDeviceViewModel> mainList =
            new List<UploadableDeviceViewModel>();
        public List<UploadableDeviceViewModel> MainList
        {
            get
            {
                return mainList;
            }
        }

        public void AddOrAppendDevice
        (DevicesObjects device)
        {
            FindOrAddDevice(device);
        }

        public void AddOrAppendReplacement
        (ReplaceDevices uploadableItem, DevicesObjects device)
        {
            UploadableDeviceViewModel uploadableDevice =
                 FindOrAddDevice(device);

            uploadableDevice.AllLists.Replacements.Add(uploadableItem);
        }

        public void AddOrAppendCycle
        (ChargeCycles uploadableItem, DevicesObjects device)
        {
            UploadableDeviceViewModel uploadableDevice =
                 FindOrAddDevice(device);

            uploadableDevice.AllLists.Cycles.Add(uploadableItem);
        }

        public void AddOrAppendPm
        (PmFaults uploadableItem, DevicesObjects device)
        {
            UploadableDeviceViewModel uploadableDevice =
                 FindOrAddDevice(device);

            uploadableDevice.AllLists.Pms.Add(uploadableItem);
        }

        public void AddOrAppendBattviewEvent
        (BattviewEvents uploadableItem, DevicesObjects device)
        {
            UploadableDeviceViewModel uploadableDevice =
                 FindOrAddDevice(device);

            uploadableDevice.AllLists.BattviewEvents.Add(uploadableItem);
        }

        UploadableDeviceViewModel FindOrAddDevice(DevicesObjects device)
        {
            var itemFound = FindUploadableDevice(device);

            if (itemFound == null)
                return AddUpdatableDevice(device);
            else
                return itemFound;
        }

        UploadableDeviceViewModel FindUploadableDevice(DevicesObjects device)
        {
            return mainList
                .FirstOrDefault(item =>
                       item.Device != null
                       && item.Device.Id == device.Id
                       && item.Device.Name == device.Name
                      );
        }

        UploadableDeviceViewModel AddUpdatableDevice(DevicesObjects device)
        {
            var item = new UploadableDeviceViewModel()
            {
                Device = device
            };

            mainList.Add(item);

            return item;
        }
    }
}
