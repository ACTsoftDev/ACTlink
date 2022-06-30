namespace actchargers
{
    public class ChargeCycles : EventBase
    {
        public override DevicesObjects GetDevice()
        {
            return GetMcbDevice();
        }
    }
}
