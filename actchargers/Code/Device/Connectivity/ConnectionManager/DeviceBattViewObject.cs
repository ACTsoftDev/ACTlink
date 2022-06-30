using System;
using System.Threading.Tasks;

namespace actchargers
{
    public class DeviceBattViewObject : GenericDevice
    {
        public BattViewObject battview;

        public override DeviceObjectParent GetDeviceObjectParent()
        {
            return battview;
        }

        public override CommunicationResult readGlobal()
        {
            return Task.Run(async () =>
            {
                var tuple = await battview.readGlobalRecord();
                return tuple.Item1;
            }).Result;
        }

        public DeviceBattViewObject(BattViewObject battview)
        {
            this.battview = battview;
            if (this.battview != null)
                InitEvents();
        }

        void InitEvents()
        {
            battview.OnFirmwareUpdateStepChanged += Battview_OnFirmwareUpdateStepChanged;
            battview.OnProgressCompletedChanged += Battview_OnProgressCompletedChanged;
        }

        void Battview_OnFirmwareUpdateStepChanged(object sender, EventArgs e)
        {
            FirmwareUpdateStep = battview.FirmwareUpdateStep;
        }

        void Battview_OnProgressCompletedChanged(object sender, EventArgs e)
        {
            ProgressCompleted = battview.ProgressCompleted;
        }

        public override async Task<Tuple<bool, string>> SiteViewUpdate()
        {
            if (battview == null)
                return new Tuple<bool, string>(false, AppResources.opration_failed);

            return await battview.SiteViewUpdate();
        }

        public override void AbortUpdate()
        {
            battview.Cancel();
        }
    }
}
