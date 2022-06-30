using System;
using MvvmCross.Core.ViewModels;

namespace actchargers
{
    public class DeviceDeatilsItem : MvxViewModel
    {
        public string DeviceTitle { get; set; }

        public string DeviceSubTitle { get; set; }

        public string DeviceImage { get; set; }

        public bool IsDisabled { get; set; }

        public Type ViewModelType { get; set; }

        public ACUtility.ActionsMenuType ActionType { get; set; }
    }
}
