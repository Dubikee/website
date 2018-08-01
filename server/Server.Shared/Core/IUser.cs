namespace Server.Shared.Core
{
    public interface IUser
    {
        int Id { get; set; }
        string Uid { get; set; }
        string Name { get; set; }
        string Phone { get; set; }
        string Email { get; set; }
        string Role { get; set; }
        byte[] PwHash { get; set; }   
    }
}