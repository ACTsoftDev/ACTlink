using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using MvvmCross.Core.ViewModels;

namespace actchargers
{
    public class SaveBatteryInfoViewModel : BaseViewModel
    {
        public SaveBatteryInfoViewModel()
        {
            BatteryInfoItemSource = new ObservableCollection<ListViewItem>();
            ViewTitle = AppResources.battery_info;
            createList();
        }


        private ObservableCollection<ListViewItem> _batteryInfoItemSource;
        public ObservableCollection<ListViewItem> BatteryInfoItemSource
        {
            get { return _batteryInfoItemSource; }
            set
            {
                _batteryInfoItemSource = value;
                RaisePropertyChanged(() => BatteryInfoItemSource);
            }
        }
        public void createList()
        {
            if (IsBattView)
                CreateDataForBattView();
            else
                createDataForCharger();
        }

        public void CreateDataForBattView()
        {
            this.BatteryInfoItemSource.Clear();

            this._batteryInfoItemSource.Add(new ListViewItem
            {
                Title = AppResources.battery_settings,
                ViewModelType = typeof(BatterySettingsViewModel),
                DefaultCellType = ACUtility.CellTypes.Label
            });

            if (ControlObject.UserAccess.Batt_CreateStudy == AccessLevelConsts.write && BattViewQuantum.Instance.GetBATTView().Config.isPA != 0)
            {
                // Add the Start New Study Button
                this._batteryInfoItemSource.Add(new ListViewItem
                {
                    Title = AppResources.start_new_study,
                    ViewModelType = typeof(NewStudyViewModel),
                    DefaultCellType = ACUtility.CellTypes.Label
                });

            }
            else
            {
                // Add the Default Charge Profile & Finish EQ Scheduling Buttons
                this._batteryInfoItemSource.Add(new ListViewItem
                {
                    Title = AppResources.default_charge_profile,
                    ViewModelType = typeof(DefaultChargeProfileViewModel),
                    DefaultCellType = ACUtility.CellTypes.Label
                });
                this._batteryInfoItemSource.Add(new ListViewItem
                {
                    Title = AppResources.finish_eq_scheduling,
                    ViewModelType = typeof(FinishAndEQSettingsViewModel),
                    DefaultCellType = ACUtility.CellTypes.Label
                });
            }
        }
        void createDataForCharger()
        {
            this.BatteryInfoItemSource.Clear();

            this._batteryInfoItemSource.Add(new ListViewItem
            {
                Title = AppResources.battery_settings,
                ViewModelType = typeof(BatterySettingsViewModel),
                DefaultCellType = ACUtility.CellTypes.Label
            });

            // Add the Default Charge Profile & Finish EQ Scheduling Buttons
            this._batteryInfoItemSource.Add(new ListViewItem
            {
                Title = AppResources.default_charge_profile,
                ViewModelType = typeof(DefaultChargeProfileViewModel),
                DefaultCellType = ACUtility.CellTypes.Label
            });
            this._batteryInfoItemSource.Add(new ListViewItem
            {
                Title = AppResources.finish_eq_scheduling,
                ViewModelType = typeof(FinishAndEQSettingsViewModel),
                DefaultCellType = ACUtility.CellTypes.Label
            });

            if (ControlObject.UserAccess.MCB_RunBattViewCal == AccessLevelConsts.write && MCBQuantum.Instance.GetMCB().Config.enablePLC == true)
            {
                //Run/Stop BattView Calibration Button
                this._batteryInfoItemSource.Add(new ListViewItem
                {
                    DefaultCellType = ACUtility.CellTypes.Button,
                    Title = AppResources.run_battview_calibration,
                    ListSelectionCommand = CalibrationSelectionCommand
                });
            }
        }

        /// <summary>
        /// Gets the calibration selection command.
        /// </summary>
        /// <value>The calibration selection command.</value>
        public IMvxCommand CalibrationSelectionCommand
        {
            get
            {
                return new MvxCommand(async () =>
                {
                    await ExecuteCalibrationSelectionCommand();
                });
            }
        }
        /// <summary>
        /// Executes the calibration selection command.
        /// </summary>
        async Task ExecuteCalibrationSelectionCommand()
        {
            ACUserDialogs.ShowProgress();
            object[] arg1 = new object[] { (byte)0x03, (UInt32)0x00 };

            McbCommunicationTypes caller = McbCommunicationTypes.NOCall;
            try
            {
                caller = McbCommunicationTypes.switchMode;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            List<object> arguments = new List<object>();
            arguments.Add(caller);
            arguments.Add(arg1);

            List<object> results = new List<object>();
            try
            {
                results = await MCBQuantum.Instance.CommunicateMCB(arguments);
                if (results.Count > 0)
                {
                    var status = (CommunicationResult)results[2];
                    if (status == CommunicationResult.OK)
                    {
                        try
                        {
                            ACUserDialogs.ShowAlertWithTitleAndOkButton("BattView Calibration success");
                        }

                        catch (Exception ex)
                        {
                            Debug.WriteLine(ex.Message);
                            Logger.AddLog(true, "X24" + ex.ToString());
                        }
                    }
                    else
                    {
                        ACUserDialogs.ShowAlertWithTitleAndOkButton(AppResources.save_failed);
                    }
                }
                else
                {
                    ACUserDialogs.ShowAlertWithTitleAndOkButton(AppResources.save_failed);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            ACUserDialogs.HideProgress();
        }

        private MvxCommand<ListViewItem> _mItemClickCommand;

        public ICommand ItemClickCommand
        {
            get
            {
                return this._mItemClickCommand ?? (this._mItemClickCommand = new MvxCommand<ListViewItem>(this.ExecuteItemClickCommnad));
            }
        }

        /// <summary>
        /// Executes the item click commnad.
        /// </summary>
        /// <param name="item">DeviceDeatilsItem.</param>
        private void ExecuteItemClickCommnad(ListViewItem item)
        {
            if (item.ViewModelType == null)
            {
                return;
            }
            if (item.ViewModelType == typeof(BatterySettingsViewModel))
            {

                ShowViewModel<BatterySettingsViewModel>();
            }
            else if (item.ViewModelType == typeof(NewStudyViewModel))
            {
                //ACUserDialogs.ShowAlert(AppResources.dev_in_progress);
                ShowViewModel<NewStudyViewModel>();
            }
            else if (item.ViewModelType == typeof(FinishAndEQSettingsViewModel))
            {
                //ACUserDialogs.ShowAlert(AppResources.dev_in_progress);
                ShowViewModel<FinishAndEQSettingsViewModel>();
            }
            else if (item.ViewModelType == typeof(DefaultChargeProfileViewModel))
            {
                ShowViewModel<DefaultChargeProfileViewModel>();
            }
        }

        /// <summary>
        /// Ons the back button click.
        /// </summary>
        public void OnBackButtonClick()
        {
            ShowViewModel<BatteryInfoViewModel>(new { pop = "pop" });
        }

    }
}
