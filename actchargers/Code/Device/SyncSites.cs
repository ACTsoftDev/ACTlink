using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using MvvmCross.Core.ViewModels;

namespace actchargers
{
    public class ACTViewSite : MvxViewModel
    {
        public ACTViewSite()
        {
            CheckedImageString = "circle";
        }


        /// <summary>
        /// Gets or sets the selection command.
        /// </summary>
        /// <value>The selection command.</value>
        public ICommand ItemCheckCommand { get; set; }

        public IMvxCommand ItemCheckBtnCommand
        {
            get
            {
                return new MvxCommand(ItemCheckClick);
            }
        }

        void ItemCheckClick()
        {
            if (ItemCheckCommand != null)
            {
                ItemCheckCommand.Execute(this);
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

        string _name;
        public string name
        {
            get { return _name; }
        }
        UInt32 _customerID;
        public UInt32 customerID
        {
            get { return _customerID; }
        }
        string _customerName;
        public string customerName
        {
            get { return _customerName; }
        }
        UInt32 _id;
        public UInt32 id
        {
            get { return _id; }
        }

        public ACTViewSite(string name, UInt32 id, UInt32 customerID, string customerName)
        {
            this._name = name;
            this._id = id;
            this._customerID = customerID;
            this._customerName = customerName;
        }
    }

    class ACTViewCustomer
    {
        string _name;
        public string name
        {
            get { return _name; }
        }
        UInt32 _id;
        public UInt32 id
        {
            get { return _id; }
        }

        Dictionary<UInt32, ACTViewSite> sites;
        public ACTViewCustomer(string name, UInt32 id)
        {
            this._name = name;
            this._id = id;
            sites = new Dictionary<uint, ACTViewSite>();
        }
        public void addSite(ACTViewSite site)
        {
            if (!sites.ContainsKey(site.id))
            {
                sites.Add(site.id, site);
            }
        }
        public List<ACTViewSite> getSites()
        {
            Dictionary<UInt32, ACTViewSite> temp = new Dictionary<uint, ACTViewSite>();
            foreach (ACTViewSite site in sites.Values)
            {
                temp.Add(site.id, site);

            }

            return temp.Values.ToList();
        }
    }
}
