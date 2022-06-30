namespace actchargers
{
    class DoLoadResult
    {
        public bool res;
        public string InterfaceSN;
        public bool isMCB;
        public object device;
        public DeviceBaseType deviceType;
        public DoLoadResult(bool isMCBx, string sn, object d)
        {
            InterfaceSN = sn;
            device = d;
            isMCB = isMCBx;
            res = false;
        }
    }
}
