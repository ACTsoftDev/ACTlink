using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace actchargers
{
    public class PmLiveController : BaseController, IDisposable
    {
        const int PMS_LIVE_VIEW_TIMER_INTERVAL = 10000;

        public event EventHandler OnPMsRead;

        ACTimer pMsTimer;

        public ObservableCollection<PmLiveModel> PMsList
        {
            get;
            set;
        }

        public PmLiveController()
        {
            Init();
        }

        void Init()
        {
            PMsList = new ObservableCollection<PmLiveModel>();

            InitPMsLiveViewTimer();
        }

        void InitPMsLiveViewTimer()
        {
            pMsTimer =
                new ACTimer
                (PmsTimer_Elapsed, null,
                 0, PMS_LIVE_VIEW_TIMER_INTERVAL);
        }

        async void PmsTimer_Elapsed(object state)
        {
            await RefreshPMsReads();
        }

        async Task RefreshPMsReads()
        {
            var status = await MCBQuantum.Instance.GetMCB().ReadPMs();

            if (status == CommunicationResult.OK)
                ReadData();
        }

        void ReadData()
        {
            var pMsArray = MCBQuantum.Instance.GetMCB().GetPMsList();

            FillList(pMsArray);
        }

        void FillList(PMState[] pMsArray)
        {
            PrepareList();

            foreach (var item in pMsArray)
                AddItemToListIfOk(item);

            NotifySubscribers();
        }

        void PrepareList()
        {
            if (PMsList == null)
                PMsList = new ObservableCollection<PmLiveModel>();

            PMsList.Clear();
        }

        void AddItemToListIfOk(PMState item)
        {
            if (item.HasPm())
                PMsList.Add(new PmLiveModel(item));
        }

        void NotifySubscribers()
        {
            OnPMsRead?.Invoke(this, EventArgs.Empty);
        }

        public void Dispose()
        {
            ClearTimer();
        }

        void ClearTimer()
        {
            if (pMsTimer != null)
                ClearTimerIfNotNull();
        }

        void ClearTimerIfNotNull()
        {
            pMsTimer.Cancel();
            pMsTimer.Dispose();

            pMsTimer = null;
        }
    }
}
