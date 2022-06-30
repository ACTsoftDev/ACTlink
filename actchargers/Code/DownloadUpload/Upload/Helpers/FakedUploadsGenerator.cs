namespace actchargers
{
    public class FakedUploadsGenerator
    {
        const int FAKED_UPLOADS_COUNT = 5;

        public static void AddFakedUploads()
        {
            var all =
                DbSingleton
                    .DBManagerServiceInstance
                    .GetDevicesObjectsLoader()
                    .GetAll();

            foreach (var item in all)
            {
                AddUploadsToDevice(item);
            }
        }

        static void AddUploadsToDevice(DevicesObjects device)
        {
            var deviceType = device.GetDeviceType();
            var deviceId = device.Id;

            switch (deviceType)
            {
                case DeviceType.MCB:
                    AddChargeCyclesToDevice(deviceId);
                    AddPmFaultsToDevice(device.Id);
                    break;

                case DeviceType.BATTVIEW:
                    AddBattviewEvents(deviceId);
                    break;

                case DeviceType.BATTVIEW_MOBILE:
                    AddBattviewEvents(deviceId);
                    break;
            }

            MarkDiviceAsNotUploaded(device);
        }

        static void AddChargeCyclesToDevice(uint deviceId)
        {
            for (uint i = 0; i < FAKED_UPLOADS_COUNT; i++)
            {
                AddOneChargeCyclesToDevice(deviceId, i);
            }
        }

        static void AddOneChargeCyclesToDevice(uint deviceId, uint fakedId)
        {
            var fakedItem = new ChargeCycles()
            {
                EventText = "",
                Id = deviceId,
                EventId = fakedId,
                IsUploaded = false
            };

            DbSingleton
                .DBManagerServiceInstance
                .GetChargeCyclesLoader()
                .InsertOrUpdate(fakedItem);
        }

        static void AddPmFaultsToDevice(uint deviceId)
        {
            for (uint i = 0; i < FAKED_UPLOADS_COUNT; i++)
            {
                AddOnePmToDevice(deviceId, i);
            }
        }

        static void AddOnePmToDevice(uint deviceId, uint fakedId)
        {
            var fakedItem = new PmFaults()
            {
                EventText = "",
                Id = deviceId,
                EventId = fakedId,
                IsUploaded = false
            };

            DbSingleton
                .DBManagerServiceInstance
                .GetPMFaultsLoader()
                .InsertOrUpdate(fakedItem);
        }

        static void AddBattviewEvents(uint deviceId)
        {
            for (uint i = 0; i < FAKED_UPLOADS_COUNT; i++)
            {
                AddOneBattviewEventsToDevice(deviceId, i);
            }
        }

        static void AddOneBattviewEventsToDevice(uint deviceId, uint fakedId)
        {
            var fakedItem = new BattviewEvents()
            {
                EventText = "",
                Id = deviceId,
                EventId = fakedId,
                IsUploaded = false
            };

            DbSingleton
                .DBManagerServiceInstance
                .GetBattviewEventsLoader()
                .InsertOrUpdate(fakedItem);
        }

        static void MarkDiviceAsNotUploaded(DevicesObjects device)
        {
            device.IsUploaded = false;

            DbSingleton
                .DBManagerServiceInstance
                .GetDevicesObjectsLoader()
                .InsertOrUpdate(device);
        }
    }
}
