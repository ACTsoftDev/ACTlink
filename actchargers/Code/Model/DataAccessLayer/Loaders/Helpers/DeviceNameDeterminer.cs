namespace actchargers
{
    public static class DeviceNameDeterminer
    {
        public static string GetDeviceName(bool isMcb)
        {
            return isMcb ? ACConstants.MCB : ACConstants.BATTVIEW;
        }
    }
}
