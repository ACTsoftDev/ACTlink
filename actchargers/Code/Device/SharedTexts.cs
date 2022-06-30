namespace actchargers
{
    public static class SharedTexts
    {
        public static readonly string[] ALL_BATTERIES =
        {
            AppResources.battrey_type_lead_acid,
            AppResources.battrey_type_lithium_ion,
            AppResources.battrey_type_lead_gel,
            AppResources.battrey_type_lithium_ion_bms
        };

        public static string GetBatteryTypeText(byte batteryType)
        {
            if (batteryType <= 0 || batteryType > ALL_BATTERIES.Length)
                return ALL_BATTERIES[0];

            return ALL_BATTERIES[batteryType];
        }

        public static string GetBmsText(byte bmsId)
        {
            switch (bmsId)
            {
                case 1:
                    return AppResources.bms_gct;

                case 2:
                    return AppResources.bms_navitas;

                case 3:
                    return AppResources.bms_electrovaya;

                default:
                    return AppResources.bms_unknown;

            }
        }
    }
}
