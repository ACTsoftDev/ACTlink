namespace actchargers
{
    public class PmLiveModel : BaseModel
    {
        public PmLiveModel(PMState pmState)
        {
            InitFromPmState(pmState);
        }

        void InitFromPmState(PMState pmState)
        {
            MacAddreess = pmState.macAddress;
            State = pmState.state;
            VersionAndRevision = pmState.VersionAndRevision;
            Model = pmState.model;
            Current = pmState.Current;
            Voltage = pmState.lastReadingVoltage;
            InstantError = pmState.instantError;
            Rating = pmState.Rating;
        }

        string macAddreess;
        public string MacAddreess
        {
            get
            {
                return macAddreess;
            }
            set
            {
                SetProperty(ref macAddreess, value);
            }
        }

        string state;
        public string State
        {
            get
            {
                return state;
            }
            set
            {
                SetProperty(ref state, value);
            }
        }

        string versionAndRevision;
        public string VersionAndRevision
        {
            get
            {
                return versionAndRevision;
            }
            private set
            {
                SetProperty(ref versionAndRevision, value);
            }
        }

        string model;
        public string Model
        {
            get
            {
                return model;
            }
            set
            {
                SetProperty(ref model, value);
            }
        }

        string current;
        public string Current
        {
            get
            {
                return current;
            }
            set
            {
                SetProperty(ref current, value);
            }
        }

        string voltage;
        public string Voltage
        {
            get
            {
                return voltage;
            }
            set
            {
                SetProperty(ref voltage, value);
            }
        }

        string instantError;
        public string InstantError
        {
            get
            {
                return instantError;
            }
            set
            {
                SetProperty(ref instantError, value);
            }
        }

        string rating;
        public string Rating
        {
            get
            {
                return rating;
            }
            set
            {
                SetProperty(ref rating, value);
            }
        }
    }
}
