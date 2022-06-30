using System;

namespace actchargers
{
    public class AdcRawClass
    {
        private byte _plc_state;
        public byte plc_state
        {
            get
            {
                lock (MyLock)
                {
                    return _plc_state;
                }
            }
        }


        private byte _plc_status;
        public byte plc_status
        {
            get
            {
                lock (MyLock)
                {
                    return _plc_status;
                }
            }
        }

        private byte _wifi_state;
        public byte wifi_state
        {
            get
            {
                lock (MyLock)
                {
                    return _wifi_state;
                }
            }

        }

        private byte _wifi_status;
        public byte wifi_status
        {
            get
            {
                lock (MyLock)
                {
                    return _wifi_status;
                }
            }

        }



        private byte _rtc_state;
        public byte rtc_state
        {
            get
            {
                lock (MyLock)
                {
                    return _rtc_state;
                }
            }
        }

        private byte _austria_state;
        public byte austria_state
        {
            get
            {
                lock (MyLock)
                {
                    return _austria_state;
                }
            }

        }
        private UInt16 _clampValue;
        public UInt16 ClampValue
        {
            get
            {
                lock (MyLock)
                {
                    return _clampValue;
                }
            }

        }
        private float _intercellNTC;
        public float IntercellNTC
        {
            get
            {
                lock (MyLock)
                {
                    return _intercellNTC;
                }
            }

        }
        private UInt16 _electrolye;
        public UInt16 Electrolye
        {
            get
            {
                lock (MyLock)
                {
                    return _electrolye;
                }
            }
        }
        private float _current;
        public float Current
        {
            get
            {
                lock (MyLock)
                {
                    return _current;
                }
            }
        }
        private UInt16 _clampValueRef;
        public UInt16 ClampValueRef
        {
            get
            {
                lock (MyLock)
                {
                    return _clampValueRef;
                }
            }
        }
        private UInt16 _clampValueChannel2;
        public UInt16 ClampValueChannel2
        {
            get
            {
                lock (MyLock)
                {
                    return _clampValueChannel2;
                }
            }
        }
        private float _voltage;
        public float Voltage
        {
            get
            {
                lock (MyLock)
                {
                    return _voltage;
                }
            }
        }
        private float _internalNTC;
        public float InternalNTC
        {
            get
            {
                lock (MyLock)
                {
                    return _internalNTC;
                }
            }
        }
        private float _ntcRefrence_the10K;
        public float NtcRefrence_the10K
        {
            get
            {
                lock (MyLock)
                {
                    return _ntcRefrence_the10K;
                }
            }
        }
        private float _ntcBattery;
        public float NtcBattery
        {
            get
            {
                lock (MyLock)
                {
                    return _ntcBattery;
                }
            }
        }
        private float _intercellNTCFiltered;
        public float IntercellNTCFiltered
        {
            get
            {
                lock (MyLock)
                {
                    return _intercellNTCFiltered;
                }
            }
        }

        private float _currentFiltered;
        public float CurrentFiltered
        {
            get
            {
                lock (MyLock)
                {
                    return _currentFiltered;
                }
            }
        }
        private float _voltageFiltered;
        public float VoltageFiltered
        {
            get
            {
                lock (MyLock)
                {
                    return _voltageFiltered;
                }
            }
        }
        private float _internalNTCFiltered;
        public float InternalNTCFiltered
        {
            get
            {
                lock (MyLock)
                {
                    return _internalNTCFiltered;
                }
            }
        }
        private float _ntcBatteryRefFiltered;
        public float NtcBatteryRefFiltered
        {
            get
            {
                lock (MyLock)
                {
                    return _ntcBatteryRefFiltered;
                }
            }
        }
        private float _ntcBatteryFiltered;
        public float NtcBatteryFiltered
        {
            get
            {
                lock (MyLock)
                {
                    return _ntcBatteryFiltered;
                }
            }
        }

        private UInt16 _clampValueFiltered;
        public UInt16 ClampValueFiltered
        {
            get
            {
                lock (MyLock)
                {
                    return _clampValueFiltered;
                }
            }
        }
        private UInt16 _electrolyeFiltered;
        public UInt16 ElectrolyeFiltered
        {
            get
            {
                lock (MyLock)
                {
                    return _electrolyeFiltered;
                }
            }
        }

        private UInt16 _clampValueReftFiltered;
        public UInt16 ClampValueReftFiltered
        {
            get
            {
                lock (MyLock)
                {
                    return _clampValueReftFiltered;
                }
            }
        }
        private UInt16 _clampValueChannel2tFiltered;
        public UInt16 ClampValueChannel2tFiltered
        {
            get
            {
                lock (MyLock)
                {
                    return _clampValueChannel2tFiltered;
                }
            }
        }
        private bool _hallEffectEnabled;
        public bool HallEffectEnabled
        {
            get
            {
                lock (MyLock)
                {
                    return _hallEffectEnabled;
                }
            }
        }
        public void loadArray(byte[] resultArray, float firmwareVersion)
        {
            lock (MyLock)
            {
                if (firmwareVersion > 1.94)
                {
                    _clampValue = BitConverter.ToUInt16(resultArray, 0);
                    _clampValueRef = BitConverter.ToUInt16(resultArray, 2);
                    _clampValueChannel2 = BitConverter.ToUInt16(resultArray, 4);
                    _clampValueFiltered = BitConverter.ToUInt16(resultArray, 6);
                    _clampValueReftFiltered = BitConverter.ToUInt16(resultArray, 8);
                    _clampValueChannel2tFiltered = BitConverter.ToUInt16(resultArray, 10);
                    //intercell
                    _intercellNTC = BitConverter.ToSingle(resultArray, 12);
                    _intercellNTCFiltered = BitConverter.ToSingle(resultArray, 16);

                    _electrolye = BitConverter.ToUInt16(resultArray, 20);
                    _electrolyeFiltered = BitConverter.ToUInt16(resultArray, 22);

                    _current = BitConverter.ToSingle(resultArray, 24);
                    _currentFiltered = BitConverter.ToSingle(resultArray, 28);

                    _voltage = BitConverter.ToSingle(resultArray, 32);
                    _voltageFiltered = BitConverter.ToSingle(resultArray, 36);


                    _internalNTC = BitConverter.ToSingle(resultArray, 40);
                    _internalNTCFiltered = BitConverter.ToSingle(resultArray, 44);


                    _ntcRefrence_the10K = BitConverter.ToSingle(resultArray, 48);
                    _ntcBatteryRefFiltered = BitConverter.ToSingle(resultArray, 52);

                    _ntcBattery = BitConverter.ToSingle(resultArray, 56);
                    _ntcBatteryFiltered = BitConverter.ToSingle(resultArray, 60);

                    _hallEffectEnabled = (resultArray[64] != 0);
                    //jump to index 128
                    _plc_state = resultArray[128];
                    _plc_status = resultArray[129];
                    _wifi_state = resultArray[130];
                    _wifi_status = resultArray[131];
                    _rtc_state = resultArray[132];
                    _austria_state = resultArray[133];
                    return;
                }

                _hallEffectEnabled = (resultArray[56] != 0);


                _clampValue = BitConverter.ToUInt16(resultArray, 0);
                _intercellNTC = BitConverter.ToSingle(resultArray, 2);
                _electrolye = BitConverter.ToUInt16(resultArray, 6);
                if (_hallEffectEnabled)
                {
                    _clampValueRef = BitConverter.ToUInt16(resultArray, 8);
                    _clampValueChannel2 = BitConverter.ToUInt16(resultArray, 10);
                    _current = 0;

                }
                else
                {
                    _clampValueRef = 0;
                    _clampValueChannel2 = 0;
                    _current = BitConverter.ToSingle(resultArray, 8);

                }



                _voltage = BitConverter.ToSingle(resultArray, 12);
                _internalNTC = BitConverter.ToSingle(resultArray, 16);
                _ntcRefrence_the10K = BitConverter.ToSingle(resultArray, 20);
                _ntcBattery = BitConverter.ToSingle(resultArray, 24);

                _clampValueFiltered = BitConverter.ToUInt16(resultArray, 28);
                _intercellNTCFiltered = BitConverter.ToSingle(resultArray, 30);
                //SGFiltered = BitConverter.ToUInt16(resultArray, 34);
                //clampRefFiltered = BitConverter.ToUInt16(resultArray, 32);
                _electrolyeFiltered = BitConverter.ToUInt16(resultArray, 34);


                if (_hallEffectEnabled)
                {
                    _clampValueReftFiltered = BitConverter.ToUInt16(resultArray, 36);
                    _clampValueChannel2tFiltered = BitConverter.ToUInt16(resultArray, 38);
                    _currentFiltered = 0;

                }
                else
                {
                    _clampValueReftFiltered = 0;
                    _clampValueChannel2tFiltered = 0;
                    _currentFiltered = BitConverter.ToSingle(resultArray, 36);
                }




                _voltageFiltered = BitConverter.ToSingle(resultArray, 40);
                _internalNTCFiltered = BitConverter.ToSingle(resultArray, 44);
                _ntcBatteryRefFiltered = BitConverter.ToSingle(resultArray, 48);
                _ntcBatteryFiltered = BitConverter.ToSingle(resultArray, 52);

            }

        }

        private readonly object MyLock;
        public AdcRawClass()
        {
            MyLock = new object();
        }
    }
}
