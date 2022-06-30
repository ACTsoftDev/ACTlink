using System;
using MvvmCross.Plugins.Messenger;

namespace actchargers
{
    public class StartScanMessage : MvxMessage
    {
        public StartScanMessage(object sender) : base(sender)
        {
        }
    }
}