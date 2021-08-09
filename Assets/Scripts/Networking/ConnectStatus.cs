namespace BreakoutStars
{
    public enum ConnectStatus
    {
        Undefined,
        Success,
        ServerFull,
        GameInProgress,
        LoggedInAgain,
        UserRequestedDisconnect,
        GenericDisconnect,
        IncorrectPassword
    }
}
