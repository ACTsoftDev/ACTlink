using System;
using MvvmCross.Core.ViewModels;

namespace actchargers
{
    public class ReplacementItem : MvxViewModel
    {
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title { get; set; }
        /// <summary>
        /// Gets or sets the customer.
        /// </summary>
        /// <value>The customer.</value>
        public string Customer { get; set; }
        /// <summary>
        /// Gets or sets the site.
        /// </summary>
        /// <value>The site.</value>
        public string Site { get; set; }
    }
}
