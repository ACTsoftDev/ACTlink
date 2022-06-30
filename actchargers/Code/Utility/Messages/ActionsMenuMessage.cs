using System;
using MvvmCross.Plugins.Messenger;

namespace actchargers
{
    public class ActionsMenuMessage : MvxMessage
    {
        public ACUtility.ActionsMenuType ActionsMenuType;
        public ActionsMenuMessage(object sender, ACUtility.ActionsMenuType actionsMenuType)
            : base(sender)
        {
            ActionsMenuType = actionsMenuType;
        }
    }
}
