using System;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using UIKit;

namespace actchargers.iOS
{
    public partial class ViewCyclesHistoryView : BackViewController
    {
        ViewCyclesHistoryViewModel currentViewModel;

        public ViewCyclesHistoryView() : base("ViewCyclesHistoryView", null)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.

            currentViewModel = ViewModel as ViewCyclesHistoryViewModel;

            listTableView.SeparatorColor = UIColor.Clear;

            listTableView.RegisterNibForCellReuse(ViewCyclesHistoryTableViewCell.Nib, ViewCyclesHistoryTableViewCell.Key);
            var source = new MvxSimpleTableViewSource(listTableView, "ViewCyclesHistoryTableViewCell");

            this.CreateBinding(heading1).For(o => o.Text).To(((ViewCyclesHistoryViewModel vm) => vm.MCB_cycleHistoryGridColumn_CycleID)).Apply();
            this.CreateBinding(heading2).For(o => o.Text).To(((ViewCyclesHistoryViewModel vm) => vm.MCB_cycleHistoryGridColumn_Date)).Apply();
            this.CreateBinding(heading3).For(o => o.Text).To(((ViewCyclesHistoryViewModel vm) => vm.MCB_cycleHistoryGridColumn_AHRs)).Apply();
            this.CreateBinding(heading4).For(o => o.Text).To(((ViewCyclesHistoryViewModel vm) => vm.MCB_cycleHistoryGridColumn_Duration)).Apply();
            this.CreateBinding(heading5).For(o => o.Text).To(((ViewCyclesHistoryViewModel vm) => vm.MCB_cycleHistoryGridColumn_exitStatus)).Apply();
            this.CreateBinding(heading6).For(o => o.Text).To(((ViewCyclesHistoryViewModel vm) => vm.MCB_cycleHistoryGridColumn_BatteryType)).Apply();

            this.CreateBinding(source).For(s => s.ItemsSource).To((ViewCyclesHistoryViewModel vm) => vm.MCB_cyclesHistoryGrid).Apply();

            listTableView.Source = source;
            ListTableManager.SetHeight(listTableView, 60);

        }

        public override void BackButton_TouchUpInside(object sender, EventArgs e)
        {
            currentViewModel.OnBackButtonClick();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            UIDevice.CurrentDevice.SetValueForKey(new NSNumber((int)UIDeviceOrientation.LandscapeRight), new NSString("orientation"));
        }
    }
}