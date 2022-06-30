using System;
using MvvmCross.Binding.BindingContext;
using MvvmCross.iOS.Views;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;
using UIKit;

namespace actchargers.iOS
{
    public partial class ExceptionsView : BaseView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:actchargers.iOS.ExceptionsView"/> class.
        /// </summary>
        public ExceptionsView() : base("ExceptionsView", null)
        {
        }

        /// <summary>
        /// Views the did load.
        /// </summary>
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.

            listTableView.RegisterNibForCellReuse(TwoLabelTableViewCell.Nib, TwoLabelTableViewCell.Key);

            var source = new ListTableViewSource(listTableView, null);
            this.CreateBinding(source).For(s => s.ListItemsSource).To((ExceptionsViewModel vm) => vm.ExceptionsItemSource).Apply();
            listTableView.SeparatorColor = UIColor.Clear;
            listTableView.Source = source;
            ListTableManager.SetHeight(listTableView, 100);

            titleViewLabel.Text = (ViewModel as ExceptionsViewModel).PagedViewId;

            nextButton.TouchUpInside += delegate
            {
                Mvx.Resolve<IMvxMessenger>().Publish(new EventsHistoryMessage(this, true));
            };

            previousButton.TouchUpInside += delegate
            {
                Mvx.Resolve<IMvxMessenger>().Publish(new EventsHistoryMessage(this, false));
            };

        }
    }
}

