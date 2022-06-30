using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;
using static actchargers.ACUtility;
namespace actchargers
{
    public class SaveNewStudyViewModel : BaseViewModel
    {

        ListViewItem Batt_BattViewSettingsStartNewStudy_truckID_textbox;
        ListViewItem Batt_BattViewSettingsStartNewStudy_studyname_textbox;

        /// <summary>
        /// The new study item source.
        /// </summary>
        private ObservableCollection<ListViewItem> _newStudyItemSource;
        public ObservableCollection<ListViewItem> NewStudyItemSource
        {
            get { return _newStudyItemSource; }
            set
            {
                _newStudyItemSource = value;
                RaisePropertyChanged(() => NewStudyItemSource);
            }
        }

        private string _startNewStudy;
        public string StartNewStudy
        {
            get { return _startNewStudy; }
            set
            {
                _startNewStudy = value;
                RaisePropertyChanged(() => StartNewStudy);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:actchargers.NewStudyViewModel"/> class.
        /// </summary>
        public SaveNewStudyViewModel()
        {
            NewStudyItemSource = new ObservableCollection<ListViewItem>();
            ViewTitle = AppResources.battery_info;
            StartNewStudy = AppResources.start_new_study;
            CreateList();
        }


        public IMvxCommand StartNewStudyButtonClicked
        {
            get { return new MvxCommand(OnStartNewStudy); }
        }

        void OnStartNewStudy()
        {
            if (!string.IsNullOrEmpty(Batt_BattViewSettingsStartNewStudy_truckID_textbox.Text) || !string.IsNullOrEmpty(Batt_BattViewSettingsStartNewStudy_studyname_textbox.Text))
            {
                VerifyControl v = new VerifyControl();
                v.VerifyTextBox(Batt_BattViewSettingsStartNewStudy_studyname_textbox, Batt_BattViewSettingsStartNewStudy_studyname_textbox, 1, 17);
                v.VerifyTextBox(Batt_BattViewSettingsStartNewStudy_truckID_textbox, Batt_BattViewSettingsStartNewStudy_studyname_textbox, 0, 17);
                if (!v.HasErrors())
                {
                    ACUserDialogs.ShowAlertWithTwoButtons("Make sure the previous study is fully synched. Don't forget to set the battery settings", "This Operation will reset BATTView Mobile", AppResources.ok, AppResources.cancel, async () => await onSave(), null);

                }
            }
            else
            {
                ACUserDialogs.ShowAlert("Please enter all fields", "", AppResources.ok);
            }
        }


        async Task onSave()
        {
            ACUserDialogs.ShowProgress();
            BattViewCommunicationTypes caller = BattViewCommunicationTypes.NOCall;
            bool arg1 = false;
            try
            {
                BattViewQuantum.Instance.GetBATTView().Config.studyName = Batt_BattViewSettingsStartNewStudy_studyname_textbox.Text;
                BattViewQuantum.Instance.GetBATTView().Config.TruckId = Batt_BattViewSettingsStartNewStudy_truckID_textbox.Text;
                BattViewQuantum.Instance.GetBATTView().Config.installationDate = DateTime.UtcNow;

                caller = BattViewCommunicationTypes.startNewStudy;
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
                results = await BattViewQuantum.Instance.CommunicateBATTView(arguments);
                if (results.Count > 0)
                {
                    var status = (CommunicationResult)results[2];
                    if (status == CommunicationResult.OK)
                    {
                        try
                        {
                            //TODO: Navigate to connect to device screena and start Scanning for devices 
                            //msg = "BATTView Restarting...";
                            //battView_MenusShowHide(Batt_BattViewSettingsStartNewStudyCancelButton, null);
                            ACUserDialogs.HideProgress();
                            Mvx.Resolve<IMvxMessenger>().Publish(new DeviceResetMessage(this, BattViewQuantum.Instance.GetBATTView().IPAddress));
                            ShowViewModel<ConnectToDeviceViewModel>(new { popto = "popto" });
                            ACUserDialogs.ShowAlert("BATTView Restarting...");
                            return;
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

        /// <summary>
        /// Creates the list.
        /// </summary>
        public void CreateList()
        {
            this.NewStudyItemSource.Clear();

            Batt_BattViewSettingsStartNewStudy_truckID_textbox = new ListViewItem
            {
                Title = AppResources.study_name,
                Text = string.Empty,
                DefaultCellType = CellTypes.LabelTextEdit,
                EditableCellType = CellTypes.LabelTextEdit,
                IsEditable = true,
                TextMaxLength = 17
            };
            this._newStudyItemSource.Add(Batt_BattViewSettingsStartNewStudy_truckID_textbox);

            Batt_BattViewSettingsStartNewStudy_studyname_textbox = new ListViewItem
            {
                Title = AppResources.truck_id,
                Text = string.Empty,
                DefaultCellType = CellTypes.LabelTextEdit,
                EditableCellType = CellTypes.LabelTextEdit,
                IsEditable = true,
                TextMaxLength = 17
            };
            this._newStudyItemSource.Add(Batt_BattViewSettingsStartNewStudy_studyname_textbox);
        }
    }
}