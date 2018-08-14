namespace Server.Shared.Results
{
    public enum AuthStatus
    {
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
