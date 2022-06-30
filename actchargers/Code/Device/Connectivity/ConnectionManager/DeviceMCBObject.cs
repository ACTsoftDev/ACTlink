using System;
using System.Threading.Tasks;

namespace actchargers
{
    public class DeviceMCBObject : GenericDevice
    {
        DeviceBaseType _deviceType;
        public DeviceBaseType DeviceType
        {
            get
            {
                lock (myLock)
                {
                    return _deviceType;
                }
            }
            set
            {
                lock (myLock)
                {
                    _deviceType = value;
                }
            }
        }

        UInt32 _startPMSynchID;
        public UInt32 StartPMSynchID
        {
            get
            {
                lock (myLock)
                {
                    return _startPMSynchID;
                }
            }
            set
            {
                lock (myLock)
                {
                    _startPMSynchID = value;
                }
            }
        }

        public MCBobject mcb;

        public override DeviceObjectParent GetDeviceObjectParent()
        {
            return mcb;
        }

        public override CommunicationResult readGlobal()
        {
            return Task.Run(async () =>
            {
                return await mcb.ReadlGobalRecord();
            }).Result;
        }

        public DeviceMCBObject(MCBobject mcb, DeviceBaseType deviceT)
        {

            this.mcb = mcb;
            if (this.mcb != null)
                InitEvents();

            DeviceType = deviceT;
            StartPMSynchID = UInt32.MaxValue;

        }

        void InitEvents()
        {
            mcb.OnFirmwareUpdateStepChanged += Mcb_OnFirmwareUpdateStepChanged;
            mcb.OnProgressCompletedChanged += Mcb_OnProgressCompletedChanged;
        }

        void Mcb_OnFirmwareUpdateStepChanged(object sender, EventArgs e)
        {
            FirmwareUpdateStep = mcb.FirmwareUpdateStep;
        }

        void Mcb_OnProgressCompletedChanged(object sender, EventArgs e)
        {
            ProgressCompleted = mcb.ProgressCompleted;
        }

        public override async Task<Tuple<bool, string>> SiteViewUpdate()
        {
            if (mcb == null)
                return new Tuple<bool, string>(false, AppResources.opration_failed);

            return await mcb.SiteViewUpdate();
        }

        public override void AbortUpdate()
        {
            mcb.Cancel();
        }
    }
}
