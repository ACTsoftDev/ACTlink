namespace actchargers
{
    public enum BattTestStages
    {
        justStarted,
        quickHWcheck,
        loadPLC,
        forceWait0,
        forceWait1,
        forceWait,
        checkVoltage,
        positiveCurrentSense,
        negativeCurrentSense,
        zeroCurrentSense,
        clamp0Read,
        clamp1Read,

        temperature,
        electrolyteLow0,
        electrolyteLow1,
        electrolyteHigh0,
        electrolyteHigh1,

        calibration0set,
        calibration0enter,
        calibration0Save,
        calibration1set,
        calibration1enter,
        calibration1Save,
        calibration2set,
        calibration2enter,
        calibration2Save,
        calibrationAllSave,


        wifiandPLC0,
        wifiandPLC1,
        serialnumber0,
        serialnumber1,
        serialnumberSave,
        afterserial,
        HWVersion0,
        HWVersion1,
        HWVersionSave,
        afterHWVersionSave,
        allDone,

    };
}
