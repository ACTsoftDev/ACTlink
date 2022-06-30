using MvvmCross.Plugins.Messenger;

namespace actchargers
{
    public class CableSettingsMessage : MvxMessage
    {
        public float Ir { get; private set; }

        public CableSettingsMessage(object sender, float ir) : base(sender)
        {
            Ir = ir;
        }
    }
}
