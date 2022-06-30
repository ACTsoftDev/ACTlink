using System;
using SQLite;

namespace actchargers
{
    public abstract class EventBase : UploadableBase
    {
        [MaxLength(2048)]
        public string EventText { get; set; }

        public UInt32 Id { get; set; }

        public UInt32 EventId { get; set; }

        public Boolean IsUploaded { get; set; }

        public abstract DevicesObjects GetDevice();

        protected DevicesObjects GetMcbDevice()
        {
            return DbSingleton
                .DBManagerServiceInstance
                .GetDevicesObjectsLoader()
                .GetDevice(Id, ACConstants.MCB);
        }

        protected DevicesObjects GetBattviewDevice()
        {
            return DbSingleton
                .DBManagerServiceInstance
                .GetDevicesObjectsLoader()
                .GetDevice(Id, ACConstants.BATTVIEW);
        }

        public override bool KeysEqual(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var i = (EventBase)obj;
            return
                Id == i.Id
                       && EventId == i.EventId;
        }
    }
}
