using System;
using MvvmCross.Plugins.Messenger;

namespace actchargers
{
    public class EventsHistoryMessage : MvxMessage
    {
        public bool isNextButton;
        public EventsHistoryMessage(object sender, bool isNextBtn) : base(sender)
        {
            isNextButton = isNextBtn;
        }
    }
}
