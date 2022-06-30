using System;
using MvvmCross.Core.ViewModels;

namespace actchargers
{
    public class ViewCycle : MvxViewModel
    {
        string _ViewCycleID;
        public string ViewCycleID
        {
            get { return _ViewCycleID; }
            set
            {
                _ViewCycleID = value;
                RaisePropertyChanged(() => ViewCycleID);
            }
        }

        DateTime _date;
        public DateTime Date
        {
            get
            {
                return _date;
            }
            set
            {
                _date = value;
                RaisePropertyChanged(() => Date);

            }
        }

        string _Duration;
        public string Duration
        {
            get { return _Duration; }
            set
            {
                _Duration = value;
                RaisePropertyChanged(() => Duration);
            }
        }


        string _AHRS;
        public string AHRS
        {
            get { return _AHRS; }
            set
            {
                _AHRS = value;
                RaisePropertyChanged(() => AHRS);
            }
        }


        string _MaxWHR;
        public string MaxWHR
        {
            get { return _MaxWHR; }
            set
            {
                _MaxWHR = value;
                RaisePropertyChanged(() => MaxWHR);
            }
        }

        string _KWHRS;
        public string KWHRS
        {
            get { return _KWHRS; }
            set
            {
                _KWHRS = value;
                RaisePropertyChanged(() => KWHRS);
            }
        }

        string _MAxTemperature;
        public string MAxTemperature
        {
            get { return _MAxTemperature; }
            set
            {
                _MAxTemperature = value;
                RaisePropertyChanged(() => MAxTemperature);
            }
        }

        string _StartVoltage;
        public string StartVoltage
        {
            get { return _StartVoltage; }
            set
            {
                _StartVoltage = value;
                RaisePropertyChanged(() => StartVoltage);
            }
        }

        string _Endvoltage;
        public string Endvoltage
        {
            get { return _Endvoltage; }
            set
            {
                _Endvoltage = value;
                RaisePropertyChanged(() => Endvoltage);
            }
        }

        string _EXitStatus;
        public string EXitStatus
        {
            get { return _EXitStatus; }
            set
            {
                _EXitStatus = value;
                RaisePropertyChanged(() => EXitStatus);
            }
        }

        string _Profiles;
        public string Profiles
        {
            get { return _Profiles; }
            set
            {
                _Profiles = value;
                RaisePropertyChanged(() => Profiles);
            }
        }

        string _PMFaulted;
        public string PMFaulted
        {
            get { return _PMFaulted; }
            set
            {
                _PMFaulted = value;
                RaisePropertyChanged(() => PMFaulted);
            }
        }

        byte _BatteryType;
        public byte BatteryType
        {
            get { return _BatteryType; }
            set
            {
                SetProperty(ref _BatteryType, value);

                RaisePropertyChanged(() => BatteryTypeText);
            }
        }

        public string BatteryTypeText
        {
            get
            {
                return SharedTexts.GetBatteryTypeText(BatteryType);
            }
        }
    }
}
