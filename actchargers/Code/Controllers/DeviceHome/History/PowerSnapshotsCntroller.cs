using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace actchargers
{
    public class PowerSnapshotsCntroller : BaseController
    {
        MCBobject mcb;

        public ObservableCollection<PowerSnapshotsModel> ListItemSource
        {
            get;
            set;
        }

        public PowerSnapshotsCntroller()
        {
            Init();
        }

        void Init()
        {
            mcb = MCBQuantum.Instance.GetMCB();

            ListItemSource = new ObservableCollection<PowerSnapshotsModel>();
        }

        public async Task ReadRecords(uint startId)
        {
            var status = await mcb.ReadPowerSnapShotsLog(startId);

            if (status == CommunicationResult.OK)
                LoadPowerSnapshots();
        }

        void LoadPowerSnapshots()
        {
            if (!MCBQuantum.Instance.MCB_quickSerialNumberCheckStrict())
                return;

            var sourceList = mcb.GetPowerSnapShots();

            FillList(sourceList);
        }

        void FillList(List<PowerSnapshot> sourceList)
        {
            PrepareList();

            foreach (var item in sourceList)
                ListItemSource.Add(new PowerSnapshotsModel(item));
        }

        void PrepareList()
        {
            if (ListItemSource == null)
                ListItemSource = new ObservableCollection<PowerSnapshotsModel>();
        }
    }
}
