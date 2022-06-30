namespace actchargers
{
    public class BattviewDeviceWarningManager : DeviceWarningManager
    {
        readonly BattViewObject battview;

        public BattviewDeviceWarningManager()
        {
            battview = BattViewQuantum.Instance.GetBATTView();
        }

        protected override void CheckAll()
        {
            CheckFirmwareUpdate();
            CheckStudyId();
            CheckTimezone();
            CheckMainSenseErrorCode8();
            CheckMainSenseErrorCode10();
            CheckMainSenseErrorCode20();
            CheckForEngOrNot();
        }

        void CheckFirmwareUpdate()
        {
            if (CanCheckFirmwareUpdate())
                warnings.Add(AppResources.warning_update_firmware);
        }

        bool CanCheckFirmwareUpdate()
        {
            bool canCheckFirmwareUpdate =
                (Firmware.DoesBattViewRequireUpdate(battview))
                && (ControlObject.UserAccess.Batt_FirmwareUpdate != AccessLevelConsts.noAccess)
                && (!battview.Config.ActViewEnabled);

            return canCheckFirmwareUpdate;
        }

        void CheckStudyId()
        {
            if (HasStudyIdError())
                warnings.Add(AppResources.study_name_error);
        }

        bool HasStudyIdError()
        {
            return (battview.Config.isPA != 0x00) && (battview.Config.studyId == 0);
        }

        void CheckTimezone()
        {
            if (battview.myZone == 0)
            {
                warnings.Add(AppResources.warning_timezone);
            }
        }

        void CheckMainSenseErrorCode8()
        {
            if ((battview.quickView.mainSenseErrorCode & 0x08) != 0
                && !ControlObject.isHWMnafacturer)
            {
                if ((battview.Config.enableHallEffectSensing != 0)
                    && (battview.FirmwareRevision > 2.19f))
                    warnings.Add(AppResources.warning_hall_effect);

                else if (!ControlObject.isACTOem)
                    warnings.Add(AppResources.warning_battview_not_calibrated);
            }
        }

        void CheckMainSenseErrorCode10()
        {
            if (((battview.quickView.mainSenseErrorCode & 0x10) != 0)
                && (ControlObject.isACTOem
                    || ControlObject.UserAccess.Batt_onlyForEnginneringTeam != AccessLevelConsts.noAccess))
            {
                warnings.Add(AppResources.warning_voltage_not_calibrated);
            }
        }

        void CheckMainSenseErrorCode20()
        {
            if ((battview.quickView.mainSenseErrorCode & 0x20) != 0
                && (ControlObject.isACTOem
                    || ControlObject.UserAccess.Batt_onlyForEnginneringTeam != AccessLevelConsts.noAccess))
            {
                warnings.Add(AppResources.warning_voltage_reading_error);
            }
        }

        void CheckForEngOrNot()
        {
            if ((ControlObject.isACTOem ||
                 ControlObject.UserAccess.Batt_onlyForEnginneringTeam != AccessLevelConsts.noAccess))
            {
                if ((battview.quickView.mainSenseErrorCode & 0x40) != 0
                    && battview.Config.enableExtTempSensing != 0)
                {
                    warnings.Add(AppResources.warning_temperature_sensor_not_working);
                }

                if ((battview.quickView.mainSenseErrorCode & 0x80) != 0
                    && battview.Config.enableHallEffectSensing == 0)
                {
                    warnings.Add(AppResources.warning_intercell_temperature_not_working);
                }
            }
            else if (battview.Config.enableHallEffectSensing == 0)
            {
                if ((battview.quickView.mainSenseErrorCode & 0x80) != 0
                    && (battview.quickView.mainSenseErrorCode & 0x40) != 0)
                {
                    warnings.Add(AppResources.warning_temperature_reading_error);
                }
            }
        }
    }
}
