using System;

namespace actchargers
{
    public class McbDeviceWarningManager : DeviceWarningManager
    {
        readonly MCBobject mcb;

        public McbDeviceWarningManager()
        {
            mcb = MCBQuantum.Instance.GetMCB();
        }

        protected override void CheckAll()
        {
            CheckFirmwareUpdate();
            CheckType();
        }

        void CheckFirmwareUpdate()
        {
            if (CanCheckFirmwareUpdate())
                warnings.Add(AppResources.warning_update_firmware);
        }

        bool CanCheckFirmwareUpdate()
        {
            byte actViewEnable = Convert.ToByte(mcb.Config.actViewEnable);

            bool canCheckFirmwareUpdate =
                (Firmware.DoesMcbRequireUpdate(mcb))
                && (ControlObject.UserAccess.Batt_FirmwareUpdate != AccessLevelConsts.noAccess)
                && (actViewEnable == 0);

            return canCheckFirmwareUpdate;
        }

        void CheckType()
        {
            if (IsWrongChargerType())
            {
                warnings.Add(AppResources.warning_charger_type_not_supported);
            }
        }

        bool IsWrongChargerType()
        {
            bool isWrongChargerType =
                mcb.Config.chargerType < 0 || mcb.Config.chargerType > 2;

            return isWrongChargerType;
        }
    }
}
