namespace Server.Shared.Results
{
    public enum DeleteUserResult
    {
        Ok,
        ParamsIsEmpty,
        TokenExpired,
        UserNotFind,
        NotAllowed,
        UnknownError
    }
}