using MvvmCross.Plugins.Messenger;

namespace actchargers
{
    public class DeviceResetMessage : MvxMessage
    {
        public string CurrentConnectedDeviceID { get; set; }

        public string InterfaceSN { get; set; }

        public DeviceResetMessage(object sender, string connecteddeviceID)
            : base(sender)
        {
            CurrentConnectedDeviceID = connecteddeviceID;
        }

        public DeviceResetMessage(object sender, string connecteddeviceID, string interfaceSN)
            : base(sender)
        {
            CurrentConnectedDeviceID = connecteddeviceID;
            InterfaceSN = interfaceSN;
        }
    }
}
