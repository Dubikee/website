 namespace Server.Shared.Core
{
    public interface IAppUser
    {
        string Uid { get; set; }
        string Name { get; set; }
        string Phone { get; set; }
        string Email { get; set; }
        string Role { get; set; }
        string WhutId { get; set; }
        string WhutPwd { get; set; }
        byte[] PwHash { get; set; }
    }
}