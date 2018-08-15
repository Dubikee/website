namespace Server.Shared.Results
{
    public enum AuthStatus
    {
        UnknownError,
        Ok,
        InputIllegal,
        NotAllowed,
        TokenExpired,
        UidIllegal,
        UIdNotFind,
        UidHasExist,
        PasswordWrong,
        PasswordIllegal,
    }
}
