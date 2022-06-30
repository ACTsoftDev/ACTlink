namespace actchargers
{
    public static class FirmwareWiFiVersionUtility
    {
        public static float GetFirmwareWiFiVersion(float firmwareVersion, byte[] resultArray)
        {
            if (firmwareVersion < 2.40f)
                return 0.0f;

            int count = resultArray.Length;

            if (count < 2)
                return 0.0f;

            int lastIndex = count - 1;
            int preLastIndex = lastIndex - 1;

            float firmwareWiFiVersion =
                ((10 * (resultArray[preLastIndex] & 0xF0) >> 4) +
                 (resultArray[preLastIndex] & 0x0F)) +
                (((10 * (resultArray[lastIndex] & 0xF0) >> 4) +
                  (resultArray[lastIndex] & 0x0F)) / 100.0f);

            return firmwareWiFiVersion;
        }
    }
}
