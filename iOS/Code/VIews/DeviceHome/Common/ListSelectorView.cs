using System;
using MvvmCross.Binding.BindingContext;
using UIKit;

namespace actchargers.iOS
{
    public partial class ListSelectorView : BackViewController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:actchargers.iOS.ListSelectorView"/> class.
        /// </summary>
        public ListSelectorView() : base("ListSelectorView", null)
        {
        }

        /// <summary>
        /// Views the did load.
        /// </summary>
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.

            var source = new StringListTableViewSource(listTableView, null);
            this.CreateBinding(source).For(s => s.ListItemsSource).To((ListSelectorViewModel vm) => vm.ListItemSource).Apply();
            this.CreateBinding(source).For(s => s.SelectionChangedCommand).To((ListSelectorViewModel vm) => vm.ListSelectorCommand).Apply();
            listTableView.Source = source;

        }

        /// <summary>
        /// Backs the button touch up inside.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        public override void BackButton_TouchUpInside(object sender, EventArgs e)
        {
            base.BackButton_TouchUpInside(sender, e);
            (ViewModel as ListSelectorViewModel).OnBackButtonClick();
        }
    }
}

