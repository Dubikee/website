namespace Server.Shared.Results
{
    public enum UpdateUserResult
    {
        Ok,
        TokenExpired,
        ParamsIsEmpty,
        UserNotFind,
        PasswordWrong,
        NewPasswordTooShort,
        NewPasswordNoNumbers,
        NewPasswordNoLetters,
        UnknownError,
    }
}