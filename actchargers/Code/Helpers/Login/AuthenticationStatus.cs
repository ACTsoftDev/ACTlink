namespace actchargers.Code.Helpers.Login
{
    public enum AuthenticationStatus
    {
        REJECTED,
        REJECTED_AND_MUST_REMOVED,
        MUST_UPDATE,
        OK_WITH_AGREEMENT,
        OK,
        ERROR
    }
}
