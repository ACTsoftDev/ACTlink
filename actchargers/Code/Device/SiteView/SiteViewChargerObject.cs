using System;

namespace actchargers
{
    public class SiteViewChargerObject : SiteViewDeviceObject
    {
        ChargerImages _myImage;
        public ChargerImages MyImage
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

        bool _running;
        public bool Running
        {
            get
            {
                lock (myLock)
                {
                    return _running;
                }
            }
            set
            {
                lock (myLock)
                {
                    _requireRedraw |= value != _running;

                    _running = value;

                }

            }
        }

        bool _hasWarning;
        public bool HasWarning
        {
            get
            {
                lock (myLock)
                {
                    return _hasWarning;
                }
            }
            set
            {
                lock (myLock)
                {
                    _requireRedraw |= value != _hasWarning;

                    _hasWarning = value;

                }

            }
        }

        bool _hasFault;
        public bool HasFault
        {
            get
            {
                lock (myLock)
                {
                    return _hasFault;
                }
            }
            set
            {
                lock (myLock)
                {
                    _requireRedraw |= value != _hasFault;

                    _hasFault = value;
                }
            }
        }

        public SiteViewChargerObject
        (string serialNumber, string interfaceSn, UInt32 id,
         bool setFromSiteOverlay, string deviceName)
            : base(serialNumber, interfaceSn, id, setFromSiteOverlay, deviceName,
                   new SiteviewChargerDownloader(), DeviceTypes.mcb)
        {
            ChargerImageVisibility = true;
            BattviewImageVisibility = false;
            _myImage = ChargerImages.none;
            _running = false;
            _hasWarning = false;
            _hasFault = false;
            string imagePrefix = "";
            if (serialNumber[1] == '1')
                imagePrefix = "4";
            else if (serialNumber[1] == '2')
                imagePrefix = "6";
            else
                imagePrefix = "12";

            ImageString = "lostConnection_" + imagePrefix + "";
        }

        public override GenericDevice BuildGenericDeviceSource()
        {
            var managedMCBs =
                SiteViewQuantum.Instance.GetConnectionManager().managedMCBs;

            return managedMCBs.getDeviceByKey(InterfaceSn);
        }

        public override void OnConnectTriger()
        {
            _running = false;
            _hasWarning = false;
            _hasFault = false;
        }

        internal override string GetInterfaceSnPrefix()
        {
            return ACConstants.INTERFACE_MCB_PREFIX;
        }


        public override void LoadMyImage()
        {
            ChargerImages localImage = ChargerImages.none;
            if (IsConnected)
            {
                if (!Running)
                {
                    if (HasFault)
                        localImage = ChargerImages.idleWithFault;
                    else if (HasWarning)
                        localImage = ChargerImages.idleWithWarning;
                    else
                        localImage = ChargerImages.idle;

                }
                else
                {
                    if (HasFault)
                        localImage = ChargerImages.runningWithFault;
                    else if (HasWarning)
                        localImage = ChargerImages.runningWithWarning;
                    else
                        localImage = ChargerImages.running;
                }

            }
            else
            {
                if (fromSiteOnly)
                {
                    localImage = ChargerImages.notConnected;
                }
                else
                {
                    localImage = ChargerImages.lostConnection;
                }
            }

            if (MyImage != localImage)
            {
                MyImage = localImage;
                string imagename = "";
                string imagePrefix = "";
                if (serialNumber[1] == '1')
                    imagePrefix = "4";
                else if (serialNumber[1] == '2')
                    imagePrefix = "6";
                else
                    imagePrefix = "12";
                switch (MyImage)
                {
                    case ChargerImages.none:
                    case ChargerImages.lostConnection:
                        imagename = "lostConnection_" + imagePrefix + "";
                        break;

                    case ChargerImages.idle:
                        imagename = "idle_" + imagePrefix + "";

                        break;
                    case ChargerImages.idleWithWarning:
                        imagename = "idleWithWarning_" + imagePrefix + "";

                        break;
                    case ChargerImages.idleWithFault:
                        imagename = "idleWithFault_" + imagePrefix + "";

                        break;
                    case ChargerImages.running:
                        imagename = "running_" + imagePrefix + ".gif";

                        break;
                    case ChargerImages.runningWithWarning:
                        imagename = "runningWithWarning_" + imagePrefix + ".gif";

                        break;
                    case ChargerImages.runningWithFault:
                        imagename = "runningWithFault_" + imagePrefix + ".gif";

                        break;
                    default:
                        imagename = "notConnected_" + imagePrefix + "";
                        break;

                }

                ImageString = imagename;
            }
        }

        internal override bool RequireUpdate(DeviceObjectParent device)
        {
            return Firmware.DoesMcbRequireUpdate(device);
        }
    }
}