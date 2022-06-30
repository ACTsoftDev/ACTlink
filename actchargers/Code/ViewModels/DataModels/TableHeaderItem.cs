using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MvvmCross.Core.ViewModels;

namespace actchargers
{
    public class TableHeaderItem : ObservableCollection<ListViewItem>
    {
        /// <summary>
        /// Gets or sets the section header.
        /// </summary>
        /// <value>The section header.</value>
		public string SectionHeader { get; set; }

        public string ID { get; set; }

        public static implicit operator ObservableCollection<object>(TableHeaderItem v)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The is visible.
        /// </summary>
        private bool _isVisible;
        public bool IsVisible
        {
            get { return _isVisible; }
            set
            {
                _isVisible = value;
                //RaisePropertyChanged(() => IsVisible);
            }
        }
    }
}
