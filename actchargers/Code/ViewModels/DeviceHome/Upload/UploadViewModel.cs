using System;
using System.Collections.Generic; using System.Threading.Tasks; 
namespace actchargers {     public class UploadViewModel : BaseViewModel     {         Uploader uploader;          List<UploadableDeviceViewModel> listItemSource;         public List<UploadableDeviceViewModel> ListItemSource         {             get { return listItemSource; }             set             {                 SetProperty(ref listItemSource, value);             }         }          DevicesTabs selectedTabIndex;         public DevicesTabs SelectedTabIndex         {             get { return selectedTabIndex; }             set             {                 SetProperty(ref selectedTabIndex, value);                  SetListItemSource();             }         }

        void SetListItemSource()
        {
            UploadableDevicesLists devicesLists = uploader.DevicesLists;
            switch (SelectedTabIndex)
            {
                case DevicesTabs.CHARGER:
                    listItemSource = devicesLists.Chargers; 
                    break;

                case DevicesTabs.BATTVIEW:
                    listItemSource = devicesLists.Battviews; 
                    break;

                case DevicesTabs.BATTVIEW_MOBILE:
                    listItemSource = devicesLists.BattviewMobiles; 
                    break;
            }              RaisePropertyChanged(() => ListItemSource);
        }          string statusLabel;         public string StatusLabel          {             get { return statusLabel; }             set             {                 SetProperty(ref statusLabel, value);             }         }          float progressCompleted;         public float ProgressCompleted         {             get { return progressCompleted; }             set             {                 SetProperty(ref progressCompleted, value);                  RaisePropertyChanged(() => ProgressCompletedIOS);             }         }          public float ProgressCompletedIOS         {             get { return GetProgressCompletedIOS(); }         }          float GetProgressCompletedIOS()         {             float progress = ProgressCompleted / ProgressMax;              if (progress < 0.0f)                 return 0.0f;             else if (progress > 1.0f)                 return 1.0f;             else                 return progress;         }

        float progressMax;         public float ProgressMax          {             get { return progressMax; }             set             {                 SetProperty(ref progressMax, value);             }         }          public UploadViewModel()         {             Task.Run(Init);         }

        async Task Init()
        {
            InitProgress();             InitTitles();
            InitData();

            await uploader.ReadAll();

            SelectedTabIndex = 0;

            await uploader.Start();
        }

        void InitProgress()
        {
            ProgressMax = 100;
            ProgressCompleted = 0;
        }

        void InitTitles()
        {
            ViewTitle = AppResources.upload;
            StatusLabel = string.Empty;
        }          void InitData()         {             uploader = new Uploader();             uploader.StatusChanged += OnStatusChanged;
            uploader.NoDataReceived += (sender, e) => OnNoData();
            uploader.FailuresReceived += (sender, e) => OnHasFailures(e);              ListItemSource = new List<UploadableDeviceViewModel>();
        }

        void OnNoData()
        {
            ACUserDialogs.ShowAlert(AppResources.no_data_to_upload);

            statusLabel = AppResources.no_data_to_upload;
            ProgressCompleted = 0;
        }

        void OnHasFailures(string messagee)
        {
            ACUserDialogs.ShowAlert(messagee);
        }          public override void OnBackButtonClick()
        {
            base.OnBackButtonClick();              ShowViewModel<UploadViewModel>(new { pop = "pop" });
        }

        void OnStatusChanged(object sender, UploadStepsEventArgs e)
        {
            UpdateStatus(e.CurrentStep, e.Progress);
        }

        void UpdateStatus(UploadSteps currentStep, float progress)
        {
            StatusLabel = UploadStepsToText.GetTextByUploadSteps(currentStep);
            ProgressCompleted = progress;
        }
    } } 