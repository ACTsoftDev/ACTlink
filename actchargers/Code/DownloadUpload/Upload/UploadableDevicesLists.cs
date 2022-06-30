using System.Collections.Generic;

namespace actchargers
{
    public class UploadableDevicesLists
    {
        List<UploadableDeviceViewModel> chargers =
            new List<UploadableDeviceViewModel>();
        public List<UploadableDeviceViewModel> Chargers
        {
            get { return chargers; }
        }

        List<UploadableDeviceViewModel> battviews =
            new List<UploadableDeviceViewModel>();
        public List<UploadableDeviceViewModel> Battviews
        {
            get { return battviews; }
        }

        List<UploadableDeviceViewModel> battviewMobiles =
            new List<UploadableDeviceViewModel>();
        public List<UploadableDeviceViewModel> BattviewMobiles
        {
            get { return battviewMobiles; }
        }

        public bool HasUploads()
        {
            int chargersCount = Chargers.Count;
            int battviewsCount = Battviews.Count;
            int battviewMobilesCount = BattviewMobiles.Count;

            int allCount = chargersCount + battviewsCount + battviewMobilesCount;

            return (allCount > 0);
        }
    }
}
