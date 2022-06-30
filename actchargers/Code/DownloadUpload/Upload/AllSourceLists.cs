using System.Threading.Tasks;

namespace actchargers
{
    public class AllSourceLists
    {
        public AllUploadableLists AllLists { get; } = new AllUploadableLists();

        public async Task ReadLists()
        {
            await Task.Run(() => ReadListsTask());
        }

        void ReadListsTask()
        {
            AllLists.Replacements = DbSingleton
                .DBManagerServiceInstance
                .GetReplaceDevicesLoaders()
                .GetNotUploaded();

            AllLists.Devices = DbSingleton
                .DBManagerServiceInstance
                .GetDevicesObjectsLoader()
                .GetNotUploaded();

            AllLists.Cycles = DbSingleton
                .DBManagerServiceInstance
                .GetChargeCyclesLoader()
                .GetNotUploaded();

            AllLists.Pms = DbSingleton
                .DBManagerServiceInstance
                .GetPMFaultsLoader()
                .GetNotUploaded();

            AllLists.BattviewEvents = DbSingleton
                .DBManagerServiceInstance
                .GetBattviewEventsLoader()
                .GetNotUploaded();
        }
    }
}
