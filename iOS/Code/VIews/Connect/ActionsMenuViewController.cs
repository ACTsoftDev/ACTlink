using System;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using MvvmCross.iOS.Views;
using UIKit;

namespace actchargers.iOS
{
    public partial class ActionsMenuViewController : MvxViewController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:actchargers.iOS.ActionsMenuViewController"/> class.
        /// </summary>
		public ActionsMenuViewController() : base("ActionsMenuViewController", null)
        {
        }

        /// <summary>
        /// Views the did load.
        /// </summary>
		public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.
            View.BackgroundColor = UIColorUtility.FromHex(ACColors.DARK_BLUE_COLOR);

            //Based on the TableView height rows height are assigned 
            listTableView.RowHeight = listTableView.Frame.Size.Height / 4;
            listTableView.SeparatorColor = UIColor.Clear;

            var source = new MvxSimpleTableViewSource(listTableView, "ActionMenuTableViewCell");
            this.CreateBinding(source).For(s => s.ItemsSource).To((ActionsMenuViewModel vm) => vm.ActionsMenuItems).Apply();
            this.CreateBinding(source).For(s => s.SelectionChangedCommand).To((ActionsMenuViewModel vm) => vm.SelectItemCommand).Apply();
            listTableView.Source = source;
            listTableView.BackgroundColor = UIColor.Clear;
        }
    }
}

