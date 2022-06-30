using System.Collections.ObjectModel;
using System.Threading.Tasks;
using MvvmCross.Core.ViewModels;

namespace actchargers
{
    public class PowerSnapshotsViewModel : BaseViewModel
    {
        PowerSnapshotsCntroller powerSnapshotsCntroller;

        public string IdTitle
        {
            get
            {
                return AppResources.power_snapshots_id;
            }
        }

        public string TimeTitle
        {
            get
            {
                return AppResources.power_snapshots_time;
            }
        }

        public string VoltageTitle
        {
            get
            {
                return AppResources.power_snapshots_voltage;
            }
        }

        public string CurrentTitle
        {
            get
            {
                return AppResources.power_snapshots_current;
            }
        }

        public string PowerTitle
        {
            get
            {
                return AppResources.power_snapshots_power;
            }
        }

        public string StartFromTitle
        {
            get
            {
                return AppResources.start_from;
            }
        }

        public string ReadRecordsTitle
        {
            get
            {
                return AppResources.read_records;
            }
        }

        ObservableCollection<PowerSnapshotsModel> listItemSource;
        public ObservableCollection<PowerSnapshotsModel> ListItemSource
        {
            get { return listItemSource; }
            set
            {
                SetProperty(ref listItemSource, value);
            }
        }

        uint startId;
        public uint StartId
        {
            get
            {
                return startId;
            }
            set
            {
                SetProperty(ref startId, value);
            }
        }

        public IMvxCommand ReadRecordsCommand
        {
            get
            {
                return new MvxCommand<PowerSnapshotsModel>(ExecuteReadRecordsCommand);
            }
        }

        async void ExecuteReadRecordsCommand(PowerSnapshotsModel obj)
        {
            await ReadRecords();
        }

        public PowerSnapshotsViewModel()
        {
            Init();
        }

        void Init()
        {
            ViewTitle = AppResources.power_snapshots;

            ListItemSource = new ObservableCollection<PowerSnapshotsModel>();

            powerSnapshotsCntroller = new PowerSnapshotsCntroller();

            StartId = 1;
        }

        async Task ReadRecords()
        {
            IsBusy = true;

            await ReadRecordsFromDevice();

            IsBusy = false;
        }

        async Task ReadRecordsFromDevice()
        {
            await powerSnapshotsCntroller.ReadRecords(StartId);

            ListItemSource = powerSnapshotsCntroller.ListItemSource;
        }

        public override void OnBackButtonClick()
        {
            base.OnBackButtonClick();

            ShowViewModel<PowerSnapshotsViewModel>(new { pop = "pop" });
        }
    }
}
