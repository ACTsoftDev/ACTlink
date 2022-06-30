using System;
using System.Collections.ObjectModel;

namespace actchargers
{
    public class PmLiveViewModel : BaseViewModel
    {
        PmLiveController pmLiveController;

        public string IdTitle
        {
            get
            {
                return AppResources.pm_id_live_title;
            }
        }

        public string PmStateTitle
        {
            get
            {
                return AppResources.pm_state_live_title;
            }
        }

        public string CurrentTitle
        {
            get
            {
                return AppResources.pm_current_live_title;
            }
        }

        public string PmVoltageTitle
        {
            get
            {
                return AppResources.pm_voltage_live_title;
            }
        }

        public string RatingTitle
        {
            get
            {
                return AppResources.pm_rating_live_title;
            }
        }

        ObservableCollection<PmLiveModel> listItemSource;
        public ObservableCollection<PmLiveModel> ListItemSource
        {
            get { return listItemSource; }
            set
            {
                SetProperty(ref listItemSource, value);
            }
        }

        public PmLiveViewModel()
        {
            Init();
        }

        void Init()
        {
            InitTitles();

            ListItemSource = new ObservableCollection<PmLiveModel>();

            pmLiveController = new PmLiveController();
            pmLiveController.OnPMsRead += PmLiveController_OnPMsRead;
        }

        void InitTitles()
        {
            ViewTitle = AppResources.pm_live_title;
        }

        void PmLiveController_OnPMsRead(object sender, EventArgs e)
        {
            SetDataOnUi();
        }

        void SetDataOnUi()
        {
            Action action = new Action(SetData);
            InvokeOnMainThread(action);
        }

        void SetData()
        {
            ListItemSource = pmLiveController.PMsList;
        }

        public void OnBackButtonClickDroid()
        {
            DisposePmLiveController();
        }

        public override void OnBackButtonClick()
        {
            base.OnBackButtonClick();

            DisposePmLiveController();

            ShowViewModel<PmLiveViewModel>(new { pop = "pop" });
        }

        void DisposePmLiveController()
        {
            if (pmLiveController != null)
                pmLiveController.Dispose();
        }
    }
}
