namespace actchargers
{
    public class PmFaults : EventBase
    {
        public override DevicesObjects GetDevice()
        {
            return GetMcbDevice();
        }
    }
}
