namespace actchargers
{
    public enum FirmwareUpdateStage
    {
        connectting,
        UPDATE_IS_REQUIRED,
        doingUpdate,
        sendingRequest,
        sentRequestDelayed,
        sentRequestPassed,
        updateCompleted,
        updateIsNotNeeded,
        FAILED
    }
}
