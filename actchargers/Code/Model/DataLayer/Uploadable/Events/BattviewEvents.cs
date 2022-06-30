using System;

namespace actchargers
{
    public class BattviewEvents : EventBase
    {
        public UInt32 OriginalStartTime { get; set; }

        public string BattviewStudyID { get; set; }

        public override DevicesObjects GetDevice()
        {
            return GetBattviewDevice();
        }
    }
}
