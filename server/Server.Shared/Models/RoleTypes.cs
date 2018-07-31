namespace Server.Shared.Models
{
    public class RoleTypes
    {
        public static readonly string Admin = nameof(Admin).ToLower();
        public static readonly string Master = nameof(Master).ToLower();
        public static readonly string Vistor = nameof(Vistor).ToLower();
    }
}