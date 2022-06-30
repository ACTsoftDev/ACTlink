using System.Collections.Generic;

namespace actchargers
{
    public class AllUploadableLists
    {
        public List<ReplaceDevices> Replacements
        {
            get;
            set;
        }

        public List<DevicesObjects> Devices
        {
            get;
            set;
        }

        public List<ChargeCycles> Cycles
        {
            get;
            set;
        }

        public List<PmFaults> Pms
        {
            get;
            set;
        }

        public List<BattviewEvents> BattviewEvents
        {
            get;
            set;
        }

        public AllUploadableLists()
        {
            Replacements = new List<ReplaceDevices>();
            Devices = new List<DevicesObjects>();
            Cycles = new List<ChargeCycles>();
            Pms = new List<PmFaults>();
            BattviewEvents = new List<BattviewEvents>();
        }
    }
}
