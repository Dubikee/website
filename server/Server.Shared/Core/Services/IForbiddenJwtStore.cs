namespace Server.Shared.Core.Services
{
    public interface IForbiddenJwtStore
    {
        bool IsForbidden(string jwt);
        bool Push(string jwt);
    }
}
