namespace Server.Shared.Results
{
    public enum InsertUserResult
    {
        Ok,
        ParamsIsEmpty,
        UidTooShort,
        UidIsNotNumbers,
        UidHasExist,
        PasswordTooShort,
        PasswordNoNumbers,
        PasswordNoLetters
    }
}