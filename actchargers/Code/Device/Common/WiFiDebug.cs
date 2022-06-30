using System;
using System.Text;

namespace actchargers
{
    public class WiFiDebug
    {
        readonly float minAcceptedFirmwareVersion;

        public Int16 Rssi { get; private set; }

        public string AtVersion { get; private set; }

        public string Ip { get; private set; }

        public string ConnectionInfo { get; private set; }

        public bool IsCommunicating { get; private set; }

        public string RestartStatusString { get; private set; }

        public string Gateway { get; private set; }

        public string MacAddress { get; private set; }

        public WiFiDebug(float minAcceptedFirmwareVersion)
        {
            this.minAcceptedFirmwareVersion = minAcceptedFirmwareVersion;

            Init();
        }

        void Init()
        {
            Rssi = 0;
            AtVersion = "";
            Ip = "";
            ConnectionInfo = "";
            IsCommunicating = false;
            RestartStatusString = "";
            Gateway = "";
        }

        public void LoadFromArray(byte[] resultArray, float firmwareRev)
        {
            int loc = 0;

            Rssi = BitConverter.ToInt16(resultArray, loc); loc += 2;

            AtVersion = Encoding.UTF8.GetString(resultArray, loc, 32).Trim('\0').Trim(' '); loc += 32;
            Ip = Encoding.UTF8.GetString(resultArray, loc, 32).Trim('\0').Trim(' '); loc += 32;
            byte connectionType = resultArray[loc++];
            ConnectionInfo = "";

            if ((connectionType & 0x80) != 0)
            {
                if ((connectionType & 0x40) != 0)
                    ConnectionInfo += "Mobile";
                else if ((connectionType & 0x08) != 0)
                    ConnectionInfo += "Mobile Router";
                else
                    ConnectionInfo += "ACT-ACCESS";
                ConnectionInfo += " ";
                if ((connectionType & 0x20) != 0)
                    ConnectionInfo += "Active";
                else
                    ConnectionInfo += "Idle";
            }
            else if ((connectionType & 0x10) != 0)
            {
                ConnectionInfo += "Direct Connection";
            }
            else
            {
                ConnectionInfo += "Not connected";
            }

            IsCommunicating = resultArray[loc++] != 0;

            byte restartStatus = resultArray[loc++];
            bool passedRestartOnce = (restartStatus & 0x80) != 0;
            restartStatus &= 0x7F;

            switch (restartStatus)
            {
                case 0: RestartStatusString = "Wifi Restart Completed"; break;
                case 1: RestartStatusString = "_WIFI_SOFT_RESTART_STEP0"; break;
                case 2: RestartStatusString = "_WIFI_SOFT_RESTART_STEP1"; break;
                case 3: RestartStatusString = "_WIFI_SOFT_RESTART_STEP2"; break;
                case 4: RestartStatusString = "_WIFI_SOFT_RESTART_STEP3"; break;
                case 30: RestartStatusString = "_WIFI_HW_RESTART_STEP0"; break;
                case 31: RestartStatusString = "_WIFI_HW_RESTART_STEP1"; break;
                case 32: RestartStatusString = "_WIFI_HW_RESTART_STEP2"; break;
                case 33: RestartStatusString = "_WIFI_HW_RESTART_STEP3"; break;
                case 5: RestartStatusString = "_WIFI_RESTART_CWMODE_SEND"; break;
                case 6: RestartStatusString = "_WIFI_RESTART_CWMODE_CONFIRM"; break;
                case 7: RestartStatusString = "_WIFI_RESTART_CIPMUX_SEND"; break;
                case 8: RestartStatusString = "_WIFI_RESTART_CIPMUX_CONFIRM"; break;
                case 9: RestartStatusString = "_WIFI_RESTART_CIPSERVER_SEND"; break;
                case 10: RestartStatusString = "_WIFI_RESTART_CIPSERVER_CONFIRM"; break;
                case 11: RestartStatusString = "_WIFI_RESTART_CIPSTO_SEND"; break;
                case 12: RestartStatusString = "_WIFI_RESTART_CIPSTO_CONFIRM"; break;
                case 13: RestartStatusString = "_WIFI_RESTART_CWAUTOCONN_SEND"; break;
                case 14: RestartStatusString = "_WIFI_RESTART_CWAUTOCONN_CONFIRM"; break;
                case 15: RestartStatusString = "_WIFI_RESTART_FINALIZE"; break;
                case 16: RestartStatusString = "_WIFI_RESTART_CWDHCP_SEND"; break;
                case 17: RestartStatusString = "_WIFI_RESTART_CWDHCP_CONFIRM"; break;
                case 18: RestartStatusString = "_WIFI_RESTART_CIPDINFO_SEND"; break;
                case 19: RestartStatusString = "_WIFI_RESTART_CIPDINFO_CONFIRM"; break;
                case 20: RestartStatusString = "_WIFI_RESTART_CIPMODE_SEND"; break;
                case 21: RestartStatusString = "_WIFI_RESTART_CIPMODE_CONFIRM"; break;
                case 22: RestartStatusString = "_WIFI_RESTART_SOFT_RESET_SEND"; break;
                case 23: RestartStatusString = "_WIFI_RESTART_SOFT_RESET_CONFIRM"; break;
                default: RestartStatusString = "N/A (" + restartStatus.ToString() + ")"; break;
            }

            if (restartStatus != 0 && !passedRestartOnce)
            {
                RestartStatusString += ", Never Passed before!";
            }

            if (firmwareRev >= minAcceptedFirmwareVersion)
            {
                Gateway = Encoding.UTF8.GetString(resultArray, loc, 32).Trim('\0').Trim(' '); loc += 32;
                MacAddress = Encoding.UTF8.GetString(resultArray, loc, 32).Trim('\0').Trim(' '); loc += 32;
            }
        }
    }
}
