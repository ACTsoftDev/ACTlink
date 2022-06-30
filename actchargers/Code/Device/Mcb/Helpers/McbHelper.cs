using System;

namespace actchargers
{
    public static class McbHelper
    {
        public static bool IsLithiumAnd2_5OrAbove(MCBobject device)
        {
            var config = device.Config;

            bool is2_5OrAbove = IsFirmwarEequalOrAbove(device, 2.5f);
            bool isLithium = IsLithium(config);

            return is2_5OrAbove && isLithium;
        }

        public static bool IsFirmwarEequalOrAbove(MCBobject device, float version)
        {
            return device.FirmwareRevision >= version;
        }

        public static bool IsLithium(MCBConfig config)
        {
            return config.batteryType.Equals("Lithium-ion", StringComparison.OrdinalIgnoreCase);
        }
    }
}
