using System.Collections.Generic;
using Server.Shared.Models.Auth;

namespace Server.Shared.Core.Services
{
    public interface IAdminManager<TUser> where TUser : IAppUser
    {
        IEnumerable<TUser> Users { get; }

        (Status status, TUser user) FindUser(string uid);

        Status DeleteUser(string uid);

        Status AddUser(UserInfos m);

        Status EditUser(UserInfos m);
    }
}
