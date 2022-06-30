using System;
using MvvmCross.Core.ViewModels;

namespace actchargers
{
    public class DeviceInfoItem : MvxViewModel
    {
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the sub title.
        /// </summary>
        /// <value>The sub title.</value>
        public string SubTitle { get; set; }

        /// <summary>
        /// Gets or sets the is BATT Replacement.
        /// </summary>
        /// <value>The is BATTReplacement.</value>
        public bool IsBATTReplacement { get; set; }

        /// <summary>
        /// Gets or sets the name of the pretty.
        /// </summary>
        /// <value>The name of the pretty.</value>
        public string PrettyName { get; set; }

        /// <summary>
        /// Gets or sets the device identifier.
        /// </summary>
        /// <value>The device identifier.</value>
        public string DeviceID { get; set; }

        /// <summary>
        /// Gets or sets the IP address.
        /// </summary>
        /// <value>The IPA ddress.</value>
        public string IPAddress { get; set; }

        /// <summary>
        /// Is connected.
        /// </summary>
        private bool _isConnected = false;
        public bool IsConnected
        {
            get { return _isConnected; }
            set
            {
                _isConnected = value;
                RaisePropertyChanged(() => IsConnected);
            }
        }
    }
}
