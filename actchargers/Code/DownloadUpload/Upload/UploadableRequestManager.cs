using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace actchargers
{
    public class UploadableRequestManager
    {
        const int MAX_PATCH_SIZE = 50;
        readonly bool dontMarkAsUploaded;

        UploadableDevicesLists devicesLists;

        public int Completed { get; private set; }

        public int Failed { get; private set; }

        public bool HasFailures => (Failed > 0);

        public bool ShouldUpdate { get; private set; }

        public UploadableRequestManager(bool dontMarkAsUploaded)
        {
            this.dontMarkAsUploaded = dontMarkAsUploaded;
        }

        public async Task UploadAll(UploadableDevicesLists devicesLists)
        {
            this.devicesLists = devicesLists;

            Reset();

            await UploadMcbs();
            await UploadBattviews();
            await UploadBattviewMobiles();
        }

        void Reset()
        {
            Completed = 0;
            Failed = 0;
            ShouldUpdate = false;
        }

        async Task UploadMcbs()
        {
            if (ShouldUpdate)
                return;

            foreach (var item in devicesLists.Chargers)
                await FullyUploadMcb(item);
        }

        async Task UploadBattviews()
        {
            if (ShouldUpdate)
                return;

            foreach (var item in devicesLists.Battviews)
                await FullyUploaBattview(item);
        }

        async Task UploadBattviewMobiles()
        {
            if (ShouldUpdate)
                return;

            foreach (var item in devicesLists.BattviewMobiles)
                await FullyUploaBattview(item);
        }

        #region MCB

        async Task FullyUploadMcb(UploadableDeviceViewModel item)
        {
            var device = item.Device;
            bool isCompleted;

            isCompleted = await UploadMcbDevice(device);
            if (!isCompleted)
                return;

            await UploadAllMcbReplacements(item.AllLists.Replacements);
            await UploadAndUpdateMcbCycles(device.Id);
            await UploadAndUpdatePms(device.Id);
        }

        async Task<bool> UploadMcbDevice(DevicesObjects device)
        {
            var response =
                await ACTViewConnect
                    .UploadMCBDeviceObject(device.ToDictionary());

            bool isCompleted = IsCompleted(response);

            if (isCompleted)
            {
                MarkMcbDeviceAsUploaded(device.Id);
            }
            else
            {
                Failed++;
            }

            return isCompleted;
        }

        void MarkMcbDeviceAsUploaded(uint id)
        {
            if (dontMarkAsUploaded)
                return;

            DbSingleton
                .DBManagerServiceInstance
                .GetDevicesObjectsLoader()
                .MarkAsUploaded(true, id);

            Completed++;
        }

        async Task UploadAllMcbReplacements(List<ReplaceDevices> replacements)
        {
            foreach (var item in replacements)
                await UploadAndMcbReplacement(item);
        }

        async Task UploadAndMcbReplacement(ReplaceDevices item)
        {
            var response =
                await ACTViewConnect
                    .ReplaceMCBDevice(item.OriginalDeviceID, item.NewDeviceID);

            bool isCompleted = IsCompleted(response);

            if (isCompleted)
            {
                MarkMcbReplacementAsUploaded(item);
            }
            else
            {
                Failed++;
            }
        }

        void MarkMcbReplacementAsUploaded(ReplaceDevices item)
        {
            if (dontMarkAsUploaded)
                return;

            DbSingleton
                .DBManagerServiceInstance
                .GetReplaceDevicesLoaders()
                .MarkAsUploaded(item.OriginalDeviceID, item.NewDeviceID, true);

            Completed++;
        }

        async Task UploadAndUpdateMcbCycles(uint id)
        {
            var notUploaded =
                DbSingleton.DBManagerServiceInstance
                           .GetChargeCyclesLoader()
                           .GetSortedNotUploaded
                           (id, MAX_PATCH_SIZE);

            if (IsEmptyList(notUploaded))
                return;

            var ids = notUploaded.Keys.ToArray();
            var array = notUploaded.Values.ToArray();

            var response = await ACTViewConnect.UploadMCBCycles(id, array);

            bool isCompleted = IsCompleted(response);

            if (isCompleted)
            {
                MarkMcbCyclesAsUploaded(id, ids);
            }
            else
            {
                Failed++;
            }
        }

        void MarkMcbCyclesAsUploaded(uint id, uint[] ids)
        {
            if (dontMarkAsUploaded)
                return;

            DbSingleton.DBManagerServiceInstance
                       .GetChargeCyclesLoader()
                       .MarkAsUploaded(id, ids);

            Completed++;
        }

        async Task UploadAndUpdatePms(uint id)
        {
            var notUploaded =
                DbSingleton.DBManagerServiceInstance
                           .GetPMFaultsLoader()
                           .GetSortedNotUploaded
                           (id, MAX_PATCH_SIZE);

            if (IsEmptyList(notUploaded))
                return;

            var ids = notUploaded.Keys.ToArray();
            var array = notUploaded.Values.ToArray();

            var response = await ACTViewConnect.UploadMCBFaults(id, array);

            bool isCompleted = IsCompleted(response);

            if (isCompleted)
            {
                MarkPmsAsUploaded(id, ids);
            }
            else
            {
                Failed++;
            }
        }

        void MarkPmsAsUploaded(uint id, uint[] ids)
        {
            if (dontMarkAsUploaded)
                return;

            DbSingleton.DBManagerServiceInstance
                       .GetPMFaultsLoader()
                       .MarkAsUploaded(id, ids);

            Completed++;
        }

        #endregion

        #region Battview

        async Task FullyUploaBattview(UploadableDeviceViewModel item)
        {
            var device = item.Device;
            bool isCompleted;

            isCompleted = await UploadBattviewDevice(device);
            if (!isCompleted)
                return;

            await UploadAllBattviewReplacements(item.AllLists.Replacements);
            await UploadAndUpdateBattviewEvents(device.Id, device.BattviewStudyID);
        }

        async Task<bool> UploadBattviewDevice(DevicesObjects device)
        {
            var response =
                await ACTViewConnect
                    .UploadBattViewDeviceObject(device.ToDictionary());

            bool isCompleted = IsCompleted(response);

            if (isCompleted)
            {
                MarkBattviewDeviceAsUploaded(device.Id, device.BattviewStudyID);
            }
            else
            {
                Failed++;
            }

            return isCompleted;
        }

        void MarkBattviewDeviceAsUploaded(uint id, uint studyId)
        {
            if (dontMarkAsUploaded)
                return;

            DbSingleton
                .DBManagerServiceInstance
                .GetDevicesObjectsLoader()
                .MarkAsUploaded(false, id, studyId);

            Completed++;
        }

        async Task UploadAllBattviewReplacements(List<ReplaceDevices> replacements)
        {
            foreach (var item in replacements)
                await UploadBattviewReplacement(item);
        }

        async Task UploadBattviewReplacement(ReplaceDevices item)
        {
            var response =
                await ACTViewConnect.ReplaceBattViewDevice
                                (item.OriginalDeviceID, item.NewDeviceID);

            bool isCompleted = IsCompleted(response);

            if (isCompleted)
            {
                MarkBattviewReplacementAsUploaded(item);
            }
            else
            {
                Failed++;
            }
        }

        void MarkBattviewReplacementAsUploaded(ReplaceDevices item)
        {
            if (dontMarkAsUploaded)
                return;

            DbSingleton
                .DBManagerServiceInstance
                .GetReplaceDevicesLoaders()
                .MarkAsUploaded(item.OriginalDeviceID, item.NewDeviceID, false);

            Completed++;
        }

        async Task UploadAndUpdateBattviewEvents(uint id, uint studyId)
        {
            var notUploaded =
                DbSingleton.DBManagerServiceInstance
                           .GetBattviewEventsLoader()
                           .GetSortedNotUploaded
                           (id, MAX_PATCH_SIZE, studyId);

            if (IsEmptyList(notUploaded))
                return;

            var ids = notUploaded.Keys.ToArray();
            var array = notUploaded.Values.ToArray();

            var response =
                await ACTViewConnect.UploadBattViewEvents(id, studyId, array);

            bool isCompleted = IsCompleted(response);

            if (isCompleted)
            {
                MarkBattviewCyclesAsUploaded(id, ids, studyId);
            }
            else
            {
                Failed++;
            }
        }

        void MarkBattviewCyclesAsUploaded(uint id, uint[] ids, uint studyId)
        {
            if (dontMarkAsUploaded)
                return;

            DbSingleton.DBManagerServiceInstance
                       .GetBattviewEventsLoader()
                       .MarkAsUploaded(id, ids, studyId);

            Completed++;
        }

        #endregion

        bool IsEmptyList(Dictionary<uint, string> notUploaded)
        {
            return notUploaded == null
                || notUploaded.Count == 0;
        }

        bool IsCompleted(ACTViewResponse response)
        {
            if (response == null)
                return false;

            var responseType = response.responseType;

            ShouldUpdate = (responseType == ActviewResponseType.expiredAPI);

            return (
                responseType == ActviewResponseType.validResponse
                || responseType == ActviewResponseType.notInternet
            );
        }
    }
}
