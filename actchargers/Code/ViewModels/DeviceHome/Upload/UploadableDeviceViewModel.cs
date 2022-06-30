namespace actchargers
{
    public class UploadableDeviceViewModel : BaseViewModel
    {
        DevicesObjects device;
        public DevicesObjects Device
        {
            get { return device; }
            set { SetProperty(ref device, value); }
        }

        AllUploadableLists allLists = new AllUploadableLists();
        public AllUploadableLists AllLists
        {
            get
            {
                return allLists;
            }
        }
    }
}
