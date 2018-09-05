using Server.Shared.Models.Auth;

namespace Server.Shared.Core.Services
{
    public interface IAccountManager<out TUser> where TUser : IAppUser
    {
        TUser AppUser { get; }

        (Status status, string jwt) Login(string uid, string pwd);

        (Status status, string jwt) Register(UserInfos m);

        Status DeleteUser();

        Status UpdateInfo(UserInfos m);

        Status UpdatePwd(string oldPwd, string newPwd);
    }
}