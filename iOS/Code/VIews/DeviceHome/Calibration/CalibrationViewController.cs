using System;
using MvvmCross.Binding.BindingContext;
using UIKit;

namespace actchargers.iOS
{
    public partial class CalibrationViewController : BackViewController
    {
        CalibrationViewModel currentViewModel;

        public CalibrationViewController() : base("CalibrationViewController", null)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.

            currentViewModel = ViewModel as CalibrationViewModel;

            segmentcontroller.TintColor = UIColorUtility.FromHex(ACColors.DARK_BLUE_COLOR);
            segmentcontroller.RemoveAllSegments();
            for (int i = 0; i < currentViewModel.listOfSegments.Count; i++)
            {
                segmentcontroller.InsertSegment(currentViewModel.listOfSegments[i], i, true);
            }
            segmentcontroller.SelectedSegment = 0;

            if (!BaseViewModel.IsBattView)
            {
                currentTableView.Hidden = true;
                voltageTableView.Hidden = false;
                socTableView.Hidden = true;

                //Voltage Tab
                voltageTableView.RegisterNibForCellReuse(LabelTextFieldTableViewCell.Nib, LabelTextFieldTableViewCell.Key);
                voltageTableView.RegisterNibForCellReuse(EnableDisableButtonTableViewCell.Nib, EnableDisableButtonTableViewCell.Key);
                voltageTableView.RegisterNibForCellReuse(ListSelectorTabelViewCell.Nib, ListSelectorTabelViewCell.Key);

                var voltageSource = new GroupItemTableViewSource(voltageTableView, null, currentViewModel);
                this.CreateBinding(voltageSource).For(s => s.ListItemsSource).To((CalibrationViewModel vm) => vm.CalibrationVoltageItemSource).Apply();
                voltageTableView.SeparatorColor = UIColor.Clear;
                voltageTableView.Source = voltageSource;

                voltageTableView.EstimatedRowHeight = 60;
            }
            else
            {
                currentTableView.Hidden = false;
                voltageTableView.Hidden = true;
                socTableView.Hidden = true;

                //Current Tab
                currentTableView.RegisterNibForCellReuse(LabelTextFieldTableViewCell.Nib, LabelTextFieldTableViewCell.Key);
                currentTableView.RegisterNibForCellReuse(LabelSwitchTableViewCell.Nib, LabelSwitchTableViewCell.Key);
                currentTableView.RegisterNibForCellReuse(EnableDisableButtonTableViewCell.Nib, EnableDisableButtonTableViewCell.Key);
                currentTableView.RegisterNibForCellReuse(LabelLabelTableViewCell.Nib, LabelLabelTableViewCell.Key);
                currentTableView.RegisterNibForCellReuse(LabelTableViewCell.Nib, LabelTableViewCell.Key);

                var currentSource = new GroupItemTableViewSource(currentTableView, null, currentViewModel);
                this.CreateBinding(currentSource).For(s => s.ListItemsSource).To((CalibrationViewModel vm) => vm.CalibrationCurrentItemSource).Apply();
                currentTableView.SeparatorColor = UIColor.Clear;
                currentTableView.Source = currentSource;

                if ((ViewModel as CalibrationViewModel).Is_Charger_VoltageTab_Visible)
                {
                    //Voltage Tab
                    voltageTableView.RegisterNibForCellReuse(LabelTextFieldTableViewCell.Nib, LabelTextFieldTableViewCell.Key);
                    voltageTableView.RegisterNibForCellReuse(EnableDisableButtonTableViewCell.Nib, EnableDisableButtonTableViewCell.Key);
                    voltageTableView.RegisterNibForCellReuse(LabelTableViewCell.Nib, LabelTableViewCell.Key);

                    var voltageSource = new GroupItemTableViewSource(voltageTableView, null, currentViewModel);
                    this.CreateBinding(voltageSource).For(s => s.ListItemsSource).To((CalibrationViewModel vm) => vm.CalibrationVoltageItemSource).Apply();
                    voltageTableView.SeparatorColor = UIColor.Clear;
                    voltageTableView.Source = voltageSource;
                }

                //SOC Tab
                socTableView.RegisterNibForCellReuse(LabelTextFieldTableViewCell.Nib, LabelTextFieldTableViewCell.Key);
                socTableView.RegisterNibForCellReuse(EnableDisableButtonTableViewCell.Nib, EnableDisableButtonTableViewCell.Key);
                socTableView.RegisterNibForCellReuse(LabelLabelTableViewCell.Nib, LabelLabelTableViewCell.Key);
                socTableView.RegisterNibForCellReuse(LabelTableViewCell.Nib, LabelTableViewCell.Key);

                var socSource = new GroupItemTableViewSource(socTableView, null, currentViewModel);
                this.CreateBinding(socSource).For(s => s.ListItemsSource).To((CalibrationViewModel vm) => vm.CalibrationSOCItemSource).Apply();
                socTableView.SeparatorColor = UIColor.Clear;
                socTableView.Source = socSource;

                currentTableView.EstimatedRowHeight = voltageTableView.EstimatedRowHeight = socTableView.EstimatedRowHeight = 60;

                segmentcontroller.ValueChanged += delegate
                {
                    this.View.EndEditing(true);
                    if ((ViewModel as CalibrationViewModel).Is_Charger_VoltageTab_Visible)
                    {
                        switch (segmentcontroller.SelectedSegment)
                        {
                            case 0:
                                currentTableView.Hidden = false;
                                voltageTableView.Hidden = true;
                                socTableView.Hidden = true;
                                break;

                            case 1:
                                currentTableView.Hidden = true;
                                voltageTableView.Hidden = false;
                                socTableView.Hidden = true;
                                break;

                            case 2:
                                currentTableView.Hidden = true;
                                voltageTableView.Hidden = true;
                                socTableView.Hidden = false;
                                break;
                        }
                    }
                    else
                    {
                        switch (segmentcontroller.SelectedSegment)
                        {
                            case 0:
                                currentTableView.Hidden = false;
                                voltageTableView.Hidden = true;
                                socTableView.Hidden = true;
                                break;

                            case 1:
                                currentTableView.Hidden = true;
                                voltageTableView.Hidden = true;
                                socTableView.Hidden = false;
                                break;
                        }
                    }

                    currentViewModel.SelectedIndex = (int)segmentcontroller.SelectedSegment;
                };
            }
        }

        public override void BackButton_TouchUpInside(object sender, EventArgs e)
        {
            base.BackButton_TouchUpInside(sender, e);

            currentViewModel.OnBackButtonClick();
        }
    }
}