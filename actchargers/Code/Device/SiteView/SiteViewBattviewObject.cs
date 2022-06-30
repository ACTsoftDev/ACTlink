using System;
namespace actchargers
{
    public class SiteViewBattviewObject : SiteViewDeviceObject
    {
        UInt32 _studyID;
        public UInt32 StudyID
        {
            get
            {
                lock (myLock)
                    return _studyID;
            }
            set
            {
                lock (myLock)
                    _studyID = value;
            }
        }

        BattviewImages _myImage;
        public BattviewImages MyImage
        {
            get
            {
                lock (myLock)
                    return _myImage;
            }
            set
            {
                lock (myLock)
                    _myImage = value;
            }
        }

        public SiteViewBattviewObject
        (string serialNumber, string interfaceSn, UInt32 id, bool setFromSiteOverlay,
         string deviceName, DeviceTypes type, UInt32 studyID)
            : base(serialNumber, interfaceSn, id, setFromSiteOverlay, deviceName,
                   new SiteviewBattViewDownloader(), type)
        {
            ChargerImageVisibility = false;
            BattviewImageVisibility = true;
            StudyID = studyID;
            _myImage = BattviewImages.none;
            ImageString = "battview";
        }

        public override GenericDevice BuildGenericDeviceSource()
        {
            var managedBATTViews =
                SiteViewQuantum.Instance.GetConnectionManager().managedBATTViews;

            return managedBATTViews.getDeviceByKey(InterfaceSn);
        }

        public override void OnConnectTriger()
        {

        }

        internal override string GetInterfaceSnPrefix()
        {
            return ACConstants.INTERFACE_BATTVIEW_PREFIX;
        }


        public override void LoadMyImage()
        {
            BattviewImages localImage = BattviewImages.none;
            if (IsConnected)
            {
                localImage = BattviewImages.idle;
            }
            else
            {
                if (fromSiteOnly)
                {
                    localImage = BattviewImages.notConnected;
                }
                else
                {
                    localImage = BattviewImages.lostConnection;
                }
            }

            if (MyImage != localImage)
            {
                MyImage = localImage;
                string imagename = "";
                if (MyImage == BattviewImages.idle)
                    imagename = "battview";
                else if (MyImage == BattviewImages.lostConnection)
                    imagename = "battview_lostConnection";
                else
                    imagename = "battview_notConnected";

                ImageString = imagename;
            }
        }

        internal override bool RequireUpdate(DeviceObjectParent device)
        {
            return Firmware.DoesBattViewRequireUpdate(device);
        }
    }
}
