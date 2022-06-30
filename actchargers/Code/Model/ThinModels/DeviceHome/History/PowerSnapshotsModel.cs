using System;

namespace actchargers
{
    public class PowerSnapshotsModel : BaseModel
    {
        public PowerSnapshotsModel(PowerSnapshot powerSnapshot)
        {
            LoadFromPowerSnapshot(powerSnapshot);
        }

        void LoadFromPowerSnapshot(PowerSnapshot powerSnapshot)
        {
            if (powerSnapshot == null)
                return;

            SetId(powerSnapshot);

            RecordTime = powerSnapshot.Time;

            SetVoltage(powerSnapshot);

            SetCurrent(powerSnapshot);

            SetPower(powerSnapshot);
        }

        void SetId(PowerSnapshot powerSnapshot)
        {
            string idPrefix = "";

            if (powerSnapshot.IsValid)
                idPrefix = AppResources.corrupted;

            Id = idPrefix + powerSnapshot.Id.ToString();
        }

        void SetVoltage(PowerSnapshot powerSnapshot)
        {
            double smallVoltage = powerSnapshot.Voltage / 100.0;

            Voltage = smallVoltage.ToString("N2");
        }

        void SetCurrent(PowerSnapshot powerSnapshot)
        {
            double smallCurrent = powerSnapshot.Current / 10.0;

            Current = smallCurrent.ToString("N1");
        }

        void SetPower(PowerSnapshot powerSnapshot)
        {
            double smallPower = powerSnapshot.Current * powerSnapshot.Voltage / 1000.0;

            Power = smallPower.ToString("N1");
        }

        string id;
        public string Id
        {
            get
            {
                return id;
            }
            private set
            {
                SetProperty(ref id, value);
            }
        }

        DateTime recordTime;
        public DateTime RecordTime
        {
            get
            {
                return recordTime;
            }
            private set
            {
                SetProperty(ref recordTime, value);
            }
        }

        string voltage;
        public string Voltage
        {
            get
            {
                return voltage;
            }
            private set
            {
                SetProperty(ref voltage, value);
            }
        }

        string current;
        public string Current
        {
            get
            {
                return current;
            }
            private set
            {
                SetProperty(ref current, value);
            }
        }

        string power;
        public string Power
        {
            get
            {
                return power;
            }
            private set
            {
                SetProperty(ref power, value);
            }
        }
    }
}
