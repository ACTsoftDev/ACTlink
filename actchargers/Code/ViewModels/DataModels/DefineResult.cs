using System;
namespace actchargers
{

    public class DefineObjectInfo
    {
        UInt32 _id;
        public UInt32 id { get { return _id; } }


        string _deviceSerialNumber;
        public string deviceSerialNumber { get { return _deviceSerialNumber; } }

        bool _lostRTC;
        public bool lostRTC { get { return _lostRTC; } }

        byte _zoneID;
        public byte zoneID { get { return _zoneID; } }

        string _ipAddress;
        public string IpAddress { get; set; }

        float _firmwareVersion;
        public float firmwareVersion { get { return _firmwareVersion; } }

        float _firmwareWiFiVersion;
        public float FirmwareWiFiVersion { get { return _firmwareWiFiVersion; } }

        bool _isCharger;
        public bool isCharger
        {
            get { return _isCharger; }
        }
        UInt32 _studyID;
        public UInt32 studyID
        {
            get { return _studyID; }
        }

        string _name;
        public string name
        {
            get { return _name; }
        }
        bool _replacementPart;
        public bool replacementPart
        {
            get { return _replacementPart; }
        }

        DeviceBaseType _deviceType;
        public DeviceBaseType deviceType
        {
            get { return _deviceType; }
        }

        byte _dcId;
        public byte DcId
        {
            get { return _dcId; }
        }

        float _firmwareDcVersion;
        public float FirmwareDcVersion
        {
            get { return _firmwareDcVersion; }
        }

        public DefineObjectInfo
        (UInt32 id, string sn, bool lostRTC, byte zoneID, float firmwareVersion,
         float firmwareWiFiVersion, bool isCharger, UInt32 studyID, string name,
         bool replacementPart, DeviceBaseType deviceType, string ipAddress, byte dcId, float firmwareDcVersion)
        {
            this._id = id;
            this._deviceSerialNumber = sn;
            this._lostRTC = lostRTC;
            this._zoneID = zoneID;
            this._firmwareVersion = firmwareVersion;
            this._firmwareWiFiVersion = firmwareWiFiVersion;
            this._isCharger = isCharger;
            this._studyID = studyID;
            this._name = name;
            this._replacementPart = replacementPart;
            this._deviceType = deviceType;
            this.IpAddress = ipAddress;
            this._dcId = dcId;
            this._firmwareDcVersion = firmwareDcVersion;
        }


    }
    public class DefineResult
    {

        public string ip;
        public bool validRes;
        public CommunicationResult res;
        public bool isCharger;
        public string deviceName;
        public byte validResNote;
        public string deviceSerialNumber;
        public UInt32 id;
        public bool lostRTC;
        public byte zoneID;
        public float firmwareVersion;
        public float firmwareWiFiVersion;
        public UInt32 studyID;
        public string name;
        public byte DcId;
        public float FirmwareDcVersion;
        public bool replacementPart;
        public DeviceBaseType deviceType;

        public DefineResult(string ip)
        {
            this.ip = ip;
            this.validRes = false;
            deviceName = "";
            res = 0;
            validResNote = 0;
            deviceSerialNumber = "";
            studyID = 0;
            name = "";
            DcId = 0;
            FirmwareDcVersion = 0.0f;
            replacementPart = false;
        }
    }
}
