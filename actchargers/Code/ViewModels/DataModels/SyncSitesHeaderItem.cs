using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using MvvmCross.Core.ViewModels;

namespace actchargers
{

    public class SyncSitesHeaderItem : MvxViewModel
    {
        private string _ExpandImageString;
        public string ExpandImageString
        {
            get
            {
                return _ExpandImageString;
            }
            set
            {
                _ExpandImageString = value;
                RaisePropertyChanged(() => ExpandImageString);
            }
        }


            private string _CheckedImageString;
            public string CheckedImageString
            {
                get
                {
                    return _CheckedImageString;
                }
                set
                {
                    _CheckedImageString = value;
                    RaisePropertyChanged(() => CheckedImageString);
                }
            }

        public string Name { get; set; }

        public UInt32 Id { get; set; }


        private bool _IsExpanded;
        public bool IsExpanded
        {
            get
            {
                return _IsExpanded;
            }
            set
            {
                _IsExpanded = value;
                if (value)
                {
                    ExpandImageString = "downarrow";
                }
                else
                {
                    ExpandImageString = "rightarrow";
                }
                RaisePropertyChanged(() => IsExpanded);
            }
        }


        private bool _IsChecked;
        public bool IsChecked
        {
            get
            {
                return _IsChecked;
            }
            set
            {
                _IsChecked = value;
                if (value)
                {
                    CheckedImageString = "activeselect";
                }
                else
                {
                    CheckedImageString = "circle";
                }
                RaisePropertyChanged(() => IsChecked);
            }
        }

        private bool _IsPartiallyChecked;
        public bool IsPartiallyChecked
        {
            get
            {
                return _IsPartiallyChecked;
            }
            set
            {
                _IsPartiallyChecked = value;
                if (value)
                {
                    CheckedImageString = "inactiveselect";
                }
                else
                {
                    CheckedImageString = "circle";
                }
                RaisePropertyChanged(() => IsPartiallyChecked);
            }
        }


        private List<ACTViewSite> _ChildItems;
        public List<ACTViewSite> ChildItems
        {
            get
            {
                return _ChildItems;
            }
            set
            {
                _ChildItems = value;
                RaisePropertyChanged(() => ChildItems);
            }
        }


        public List<ACTViewSite> Sites { get; set; }

        public SyncSitesHeaderItem()
        {
            ExpandImageString = "rightarrow";
            CheckedImageString = "circle";
            Sites = new List<ACTViewSite>();
            ChildItems = new List<ACTViewSite>();
        }

        /// <summary>
        /// Gets or sets the selection command.
        /// </summary>
        /// <value>The selection command.</value>
        public ICommand ExpandCommand { get; set; }

        public IMvxCommand ExpandBtnCommand
        {
            get
            {
                return new MvxCommand(ExpandClick);
            }
        }

        void ExpandClick()
        {
            if (ExpandCommand != null)
            {
                ExpandCommand.Execute(this);
            }
        }


        /// <summary>
        /// Gets or sets the selection command.
        /// </summary>
        /// <value>The selection command.</value>
        public ICommand CheckCommand { get; set; }

        public IMvxCommand CheckBtnCommand
        {
            get
            {
                return new MvxCommand(CheckClick);
            }
        }

        void CheckClick()
        {
            if (CheckCommand != null)
            {
                CheckCommand.Execute(this);
            }
        }
    }
}
