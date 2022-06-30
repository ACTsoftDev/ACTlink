using System;
namespace actchargers
{
    public class PMFaultsModel
    {
        private UInt32 _faultID;
        public UInt32 faultID
        {
            get
            {
                return _faultID;
            }
            set
            {
                _faultID = value;
            }
        }


        private bool _isValidCRC7;
        public bool isValidCRC7
        {
            get
            {
                return _isValidCRC7;
            }

            set
            {
                _isValidCRC7 = value;
            }
        }



        private string _debugHeader = string.Empty;
        public string debugHeader
        {
            get
            {
                return _debugHeader;

            }

            set
            {
                _debugHeader = value;

            }
        }



        private DateTime _faultTime;
        public DateTime faultTime
        {
            get
            {
                return _faultTime;

            }

            set
            {
                _faultTime = value;

            }
        }



        private string _DebugString = string.Empty;
        public string DebugString
        {
            get
            {
                return _DebugString;

            }

            set
            {
                _DebugString = value;
            }
        }
    }
}
