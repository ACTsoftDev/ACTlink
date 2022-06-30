using System;
using MvvmCross.Plugins.Messenger;

namespace actchargers
{
    public class ProgressChangeMessage : MvxMessage
    {
        public int ProgressCompleted;
        public int ProgressMax;

        public ProgressChangeMessage(object sender, int progressCompleted, int progressMax) : base(sender)
        {
            ProgressCompleted = progressCompleted;
            ProgressMax = progressMax;
        }
    }
}
