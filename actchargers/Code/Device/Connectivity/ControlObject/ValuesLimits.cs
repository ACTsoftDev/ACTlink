namespace actchargers
{
    public class ValuesLimits
    {
        private int _trCurrentRate_min;
        public int trCurrentRate_min
        {
            get
            {
                lock (myLock)
                {
                    return _trCurrentRate_min;
                }
            }
            set
            {
                lock (myLock)
                {
                    _trCurrentRate_min = value;
                }
            }
        }
        private int _trCurrentRate_max;
        public int trCurrentRate_max
        {
            get
            {
                lock (myLock)
                {
                    return _trCurrentRate_max;
                }
            }
            set
            {
                lock (myLock)
                {
                    _trCurrentRate_max = value;
                }
            }
        }
        private int _ccCurrentRate_min;
        public int ccCurrentRate_min
        {
            get
            {
                lock (myLock)
                {
                    return _ccCurrentRate_min;
                }
            }
            set
            {
                lock (myLock)
                {
                    _ccCurrentRate_min = value;
                }
            }
        }
        private int _ccCurrentRate_max;
        public int ccCurrentRate_max
        {
            get
            {
                lock (myLock)
                {
                    return _ccCurrentRate_max;
                }
            }
            set
            {
                lock (myLock)
                {
                    _ccCurrentRate_max = value;
                }
            }
        }
        private int _fiCurrentRate_min;
        public int fiCurrentRate_min
        {
            get
            {
                lock (myLock)
                {
                    return _fiCurrentRate_min;
                }
            }
            set
            {
                lock (myLock)
                {
                    _fiCurrentRate_min = value;
                }
            }
        }
        private int _fiCurrentRate_max;
        public int fiCurrentRate_max
        {
            get
            {
                lock (myLock)
                {
                    return _fiCurrentRate_max;
                }
            }
            set
            {
                lock (myLock)
                {
                    _fiCurrentRate_max = value;
                }
            }
        }
        private int _eqCurrentRate_min;
        public int eqCurrentRate_min
        {
            get
            {
                lock (myLock)
                {
                    return _eqCurrentRate_min;
                }
            }
            set
            {
                lock (myLock)
                {
                    _eqCurrentRate_min = value;
                }
            }
        }
        private int _eqCurrentRate_max;
        public int eqCurrentRate_max
        {
            get
            {
                lock (myLock)
                {
                    return _eqCurrentRate_max;
                }
            }
            set
            {
                lock (myLock)
                {
                    _eqCurrentRate_max = value;
                }
            }
        }
        private float _trVoltage_min;
        public float trVoltage_min
        {
            get
            {
                lock (myLock)
                {
                    return _trVoltage_min;
                }
            }
            set
            {
                lock (myLock)
                {
                    _trVoltage_min = value;
                }
            }
        }
        private float _trVoltage_max;
        public float trVoltage_max
        {
            get
            {
                lock (myLock)
                {
                    return _trVoltage_max;
                }
            }
            set
            {
                lock (myLock)
                {
                    _trVoltage_max = value;
                }
            }
        }
        private float _cvVoltage_min;
        public float cvVoltage_min
        {
            get
            {
                lock (myLock)
                {
                    return _cvVoltage_min;
                }
            }
            set
            {
                lock (myLock)
                {
                    _cvVoltage_min = value;
                }
            }
        }
        private float _cvVoltage_max;
        public float cvVoltage_max
        {
            get
            {
                lock (myLock)
                {
                    return _cvVoltage_max;
                }
            }
            set
            {
                lock (myLock)
                {
                    _cvVoltage_max = value;
                }
            }
        }
        private float _fiVoltage_min;
        public float fiVoltage_min
        {
            get
            {
                lock (myLock)
                {
                    return _fiVoltage_min;
                }
            }
            set
            {
                lock (myLock)
                {
                    _fiVoltage_min = value;
                }
            }
        }
        private float _fiVoltage_max;
        public float fiVoltage_max
        {
            get
            {
                lock (myLock)
                {
                    return _fiVoltage_max;
                }
            }
            set
            {
                lock (myLock)
                {
                    _fiVoltage_max = value;
                }
            }
        }
        private float _eqVoltage_min;
        public float eqVoltage_min
        {
            get
            {
                lock (myLock)
                {
                    return _eqVoltage_min;
                }
            }
            set
            {
                lock (myLock)
                {
                    _eqVoltage_min = value;
                }
            }
        }
        private float _eqVoltage_max;
        public float eqVoltage_max
        {
            get
            {
                lock (myLock)
                {
                    return _eqVoltage_max;
                }
            }
            set
            {
                lock (myLock)
                {
                    _eqVoltage_max = value;
                }
            }
        }

        private float _liIon_CellMin;
        public float liIon_CellMin
        {
            get
            {
                lock (myLock)
                {
                    return _liIon_CellMin;
                }
            }
            set
            {
                lock (myLock)
                {
                    _liIon_CellMin = value;
                }
            }
        }
        private float _liIon_CellMax;
        public float liIon_CellMax
        {
            get
            {
                lock (myLock)
                {
                    return _liIon_CellMax;
                }
            }
            set
            {
                lock (myLock)
                {
                    _liIon_CellMax = value;
                }
            }
        }
        private float _cvFinishCurrentRate_min;
        public float cvFinishCurrentRate_min
        {
            get
            {
                lock (myLock)
                {
                    return _cvFinishCurrentRate_min;
                }
            }
            set
            {
                lock (myLock)
                {
                    _cvFinishCurrentRate_min = value;
                }
            }
        }
        private float _cvFinishCurrentRate_max;
        public float cvFinishCurrentRate_max
        {
            get
            {
                lock (myLock)
                {
                    return _cvFinishCurrentRate_max;
                }
            }
            set
            {
                lock (myLock)
                {
                    _cvFinishCurrentRate_max = value;
                }
            }
        }
        private float _cvCurrentStep_min;
        public float cvCurrentStep_min
        {
            get
            {
                lock (myLock)
                {
                    return _cvCurrentStep_min;
                }
            }
            set
            {
                lock (myLock)
                {
                    _cvCurrentStep_min = value;
                }
            }
        }
        private float _cvCurrentStep_max;
        public float cvCurrentStep_max
        {
            get
            {
                lock (myLock)
                {
                    return _cvCurrentStep_max;
                }
            }
            set
            {
                lock (myLock)
                {
                    _cvCurrentStep_max = value;
                }
            }
        }
        private int _cvTimerStart;
        public int cvTimerStart
        {
            get
            {
                lock (myLock)
                {
                    return _cvTimerStart;
                }
            }
            set
            {
                lock (myLock)
                {
                    _cvTimerStart = value;
                }
            }
        }
        private int _cvTimerStep;
        public int cvTimerStep
        {
            get
            {
                lock (myLock)
                {
                    return _cvTimerStep;
                }
            }
            set
            {
                lock (myLock)
                {
                    _cvTimerStep = value;
                }
            }
        }
        private int _cvTimerEnd;
        public int cvTimerEnd
        {
            get
            {
                lock (myLock)
                {
                    return _cvTimerEnd;
                }
            }
            set
            {
                lock (myLock)
                {
                    _cvTimerEnd = value;
                }
            }
        }
        private int _fiTimerStart;
        public int fiTimerStart
        {
            get
            {
                lock (myLock)
                {
                    return _fiTimerStart;
                }
            }
            set
            {
                lock (myLock)
                {
                    _fiTimerStart = value;
                }
            }
        }
        private int _fiTimerStep;
        public int fiTimerStep
        {
            get
            {
                lock (myLock)
                {
                    return _fiTimerStep;
                }
            }
            set
            {
                lock (myLock)
                {
                    _fiTimerStep = value;
                }
            }
        }
        private int _fiTimerEnd;
        public int fiTimerEnd
        {
            get
            {
                lock (myLock)
                {
                    return _fiTimerEnd;
                }
            }
            set
            {
                lock (myLock)
                {
                    _fiTimerEnd = value;
                }
            }
        }
        private int _eqTimerStart;
        public int eqTimerStart
        {
            get
            {
                lock (myLock)
                {
                    return _eqTimerStart;
                }
            }
            set
            {
                lock (myLock)
                {
                    _eqTimerStart = value;
                }
            }
        }
        private int _eqTimerStep;
        public int eqTimerStep
        {
            get
            {
                lock (myLock)
                {
                    return _eqTimerStep;
                }
            }
            set
            {
                lock (myLock)
                {
                    _eqTimerStep = value;
                }
            }
        }
        private int _eqTimerEnd;
        public int eqTimerEnd
        {
            get
            {
                lock (myLock)
                {
                    return _eqTimerEnd;
                }
            }
            set
            {
                lock (myLock)
                {
                    _eqTimerEnd = value;
                }
            }
        }
        private int _desTimerStart;
        public int desTimerStart
        {
            get
            {
                lock (myLock)
                {
                    return _desTimerStart;
                }
            }
            set
            {
                lock (myLock)
                {
                    _desTimerStart = value;
                }
            }
        }
        private int _desTimerStep;
        public int desTimerStep
        {
            get
            {
                lock (myLock)
                {
                    return _desTimerStep;
                }
            }
            set
            {
                lock (myLock)
                {
                    _desTimerStep = value;
                }
            }
        }
        private int _desTimerEnd;
        public int desTimerEnd
        {
            get
            {
                lock (myLock)
                {
                    return _desTimerEnd;
                }
            }
            set
            {
                lock (myLock)
                {
                    _desTimerEnd = value;
                }
            }
        }
        private int _fidVStart;
        public int fidVStart
        {
            get
            {
                lock (myLock)
                {
                    return _fidVStart;
                }
            }
            set
            {
                lock (myLock)
                {
                    _fidVStart = value;
                }
            }
        }
        private int _fidVEnd;
        public int fidVEnd
        {
            get
            {
                lock (myLock)
                {
                    return _fidVEnd;
                }
            }
            set
            {
                lock (myLock)
                {
                    _fidVEnd = value;
                }
            }
        }
        private int _fidtStart;
        public int fidtStart
        {
            get
            {
                lock (myLock)
                {
                    return _fidtStart;
                }
            }
            set
            {
                lock (myLock)
                {
                    _fidtStart = value;
                }
            }
        }
        private int _fidtEnd;
        public int fidtEnd
        {
            get
            {
                lock (myLock)
                {
                    return _fidtEnd;
                }
            }
            set
            {
                lock (myLock)
                {
                    _fidtEnd = value;
                }
            }
        }
        private int _rfTimerStart;
        public int rfTimerStart
        {
            get
            {
                lock (myLock)
                {
                    return _rfTimerStart;
                }
            }
            set
            {
                lock (myLock)
                {
                    _rfTimerStart = value;
                }
            }
        }
        private int _rfTimerStep;
        public int rfTimerStep
        {
            get
            {
                lock (myLock)
                {
                    return _rfTimerStep;
                }
            }
            set
            {
                lock (myLock)
                {
                    _rfTimerStep = value;
                }
            }
        }
        private int _rfTimerEnd;
        public int rfTimerEnd
        {
            get
            {
                lock (myLock)
                {
                    return _rfTimerEnd;
                }
            }
            set
            {
                lock (myLock)
                {
                    _rfTimerEnd = value;
                }
            }
        }
        private int _energyFactorMin;
        public int energyFactorMin
        {
            get
            {
                lock (myLock)
                {
                    return _energyFactorMin;
                }
            }
            set
            {
                lock (myLock)
                {
                    _energyFactorMin = value;
                }
            }
        }
        private int _energyFactorMax;
        public int energyFactorMax
        {
            get
            {
                lock (myLock)
                {
                    return _energyFactorMax;
                }
            }
            set
            {
                lock (myLock)
                {
                    _energyFactorMax = value;
                }
            }
        }
        private int _energyFactorStep;
        public int energyFactorStep
        {
            get
            {
                lock (myLock)
                {
                    return _energyFactorStep;
                }
            }
            set
            {
                lock (myLock)
                {
                    _energyFactorStep = value;
                }
            }
        }
        private int _energyTimerStart;
        public int energyTimerStart
        {
            get
            {
                lock (myLock)
                {
                    return _energyTimerStart;
                }
            }
            set
            {
                lock (myLock)
                {
                    _energyTimerStart = value;
                }
            }
        }
        private int _energyTimerStep;
        public int energyTimerStep
        {
            get
            {
                lock (myLock)
                {
                    return _energyTimerStep;
                }
            }
            set
            {
                lock (myLock)
                {
                    _energyTimerStep = value;
                }
            }
        }
        private int _energyTimerEnd;
        public int energyTimerEnd
        {
            get
            {
                lock (myLock)
                {
                    return _energyTimerEnd;
                }
            }
            set
            {
                lock (myLock)
                {
                    _energyTimerEnd = value;
                }
            }
        }
        private int _numberOfInstalledPMs_Min;
        public int numberOfInstalledPMs_Min
        {
            get
            {
                lock (myLock)
                {
                    return _numberOfInstalledPMs_Min;
                }
            }
            set
            {
                lock (myLock)
                {
                    _numberOfInstalledPMs_Min = value;
                }
            }
        }
        private int _numberOfInstalledPMs_Max;
        public int numberOfInstalledPMs_Max
        {
            get
            {
                lock (myLock)
                {
                    return _numberOfInstalledPMs_Max;
                }
            }
            set
            {
                lock (myLock)
                {
                    _numberOfInstalledPMs_Max = value;
                }
            }
        }
        object myLock;
        public ValuesLimits(bool isEngineeringTeam)
        {
            myLock = new object();
            if (isEngineeringTeam)
            {
                trCurrentRate_max = 99;
                trCurrentRate_min = 1;
                ccCurrentRate_min = 1;
                ccCurrentRate_max = 100;
                fiCurrentRate_min = 1;
                fiCurrentRate_max = 99;
                eqCurrentRate_min = 1;
                eqCurrentRate_max = 99;
                trVoltage_min = 1.5f;
                trVoltage_max = 2.99f;
                cvVoltage_min = 1.5f;
                cvVoltage_max = 2.99f;
                fiVoltage_min = 1.5f;
                fiVoltage_max = 2.99f;
                eqVoltage_min = 1.5f;
                eqVoltage_max = 2.99f;
                liIon_CellMin = 2.5f;
                liIon_CellMax = 4.0f;
                cvFinishCurrentRate_min = 1;
                cvFinishCurrentRate_max = 50;
                cvCurrentStep_min = 1;
                cvCurrentStep_max = 50;
                cvTimerStart = 0;
                cvTimerStep = 1;
                cvTimerEnd = 72;
                fiTimerStart = 0;
                fiTimerStep = 1;
                fiTimerEnd = 72;
                eqTimerStart = 0;
                eqTimerStep = 1;
                eqTimerEnd = 72;
                desTimerStart = 0;
                desTimerStep = 1;
                desTimerEnd = 72;
                fidVStart = 1;
                fidVEnd = 20;
                fidtStart = 1;
                fidtEnd = 59;
                rfTimerStart = 0;
                rfTimerStep = 1;
                rfTimerEnd = 96;
                energyFactorMin = 1;
                energyFactorMax = 99;
                energyFactorStep = 1;
                energyTimerStart = 1;
                energyTimerStep = 1;
                energyTimerEnd = 72;
                numberOfInstalledPMs_Min = 1;
                numberOfInstalledPMs_Max = 12;

            }
            else
            {
                trCurrentRate_max = 15;
                trCurrentRate_min = 3;
                ccCurrentRate_min = 4;
                ccCurrentRate_max = 100;
                fiCurrentRate_min = 1;
                fiCurrentRate_max = 25;
                eqCurrentRate_min = 1;
                eqCurrentRate_max = 11;
                trVoltage_min = 1.95f;
                trVoltage_max = 2.1f;
                cvVoltage_min = 2.1f;
                cvVoltage_max = 2.5f;
                fiVoltage_min = 2.25f;
                fiVoltage_max = 2.65f;
                eqVoltage_min = 2.3f;
                eqVoltage_max = 2.7f;
                liIon_CellMin = 2.5f;
                liIon_CellMax = 4.0f;
                cvFinishCurrentRate_min = 5;
                cvFinishCurrentRate_max = 15;
                cvCurrentStep_min = 1;
                cvCurrentStep_max = 10;
                cvTimerStart = 4;
                cvTimerStep = 2;
                cvTimerEnd = 20;
                fiTimerStart = 4;
                fiTimerStep = 2;
                fiTimerEnd = 20;
                eqTimerStart = 8;
                eqTimerStep = 2;
                eqTimerEnd = 32;
                desTimerStart = 24;
                desTimerStep = 2;
                desTimerEnd = 72;
                fidVStart = 1;
                fidVEnd = 20;
                fidtStart = 10;
                fidtEnd = 59;
                rfTimerStart = 20;
                rfTimerStep = 2;
                rfTimerEnd = 96;
                energyFactorMin = 50;
                energyFactorMax = 95;
                energyFactorStep = 5;
                energyTimerStart = 12;
                energyTimerStep = 2;
                energyTimerEnd = 72;
                numberOfInstalledPMs_Min = 1;
                numberOfInstalledPMs_Max = 12;
            }

        }
    }
}
